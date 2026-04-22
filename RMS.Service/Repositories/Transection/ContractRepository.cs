using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using RMS.Data;
using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Extentions;
using RMS.Service.Interfaces.Master;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace RMS.Service.Repositories.Master
{
    public class ContractRepository: GenericRepository<Projectcontract>, IContractRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeStatusService _employeeStatusService;

        public ContractRepository(RmsDevContext context,IHostingEnvironment env, IHttpContextAccessor httpContextAccessor,IEmployeeStatusService employeeStatusService) : base(context)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _employeeStatusService = employeeStatusService;
        }
        public async Task<Response> UpsertContracts(int ?ContractId, ContractRequestDto Value, IFormFile Attachment,JwtLoginDetailDto LoginDetails)
        {

            #region[Valiidatios]
            if (Value.Contractid == 0 && await _context.Projectcontracts.AnyAsync(x => x.Contractno == Value.Contractno && Value.isExtended !=true))
            {
                throw new Exception($"Given Contract Number already exists -- {Value.Contractno}");
            }
            if (Value.Contractid == 0 && await _context.Projectcontracts.AnyAsync(x => x.Ponumber == Value.Ponumber && Value.isExtended !=true))
            {
                throw new Exception($"Given PO Number already exists -- {Value.Ponumber}");
            }
            #endregion
            #region File Processing
            string basePath = Path.Combine(this._env.WebRootPath, @"Contracts\", LoginDetails.TmcId.ToString());
            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
            string databsefilePath = (Attachment == null) ? await _context.Projectcontracts.AsNoTracking().Where(x=>x.Contractid==ContractId).Select(x=>x.Attachment).FirstOrDefaultAsync()??""
                                                          : ProcessAttachment(Attachment, basePath);
            string ProcessAttachment(IFormFile Attachment, string basePath)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Attachment.FileName);
                string filePath = Path.Combine(basePath, uniqueFileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        Attachment.CopyTo(stream);
                    }
                }
                return Path.Combine(@"\Contracts\", uniqueFileName);
            }
            #endregion

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                string newContractno = null;

                

                if ((Value.isExtended == true || Value.isProjection == true) && string.IsNullOrEmpty(Value.Contractno))
                {
                    var currentYear = DateTime.Now.Year;
                    var prefix = $"PR-{currentYear}-";

                    var lastContract = await _context.Projectcontracts.AsNoTracking()
                        .Where(c => c.Contractno.StartsWith(prefix))
                        .OrderByDescending(c => c.Contractno)
                        .FirstOrDefaultAsync();

                    int nextNumber = 1;

                    if (lastContract != null && int.TryParse(lastContract.Contractno.Split('-').Last(), out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }

                    newContractno = $"{prefix}{nextNumber:D4}";
                }
                else if (!string.IsNullOrEmpty(Value.Contractno))
                {
                    newContractno = Value.Contractno;
                }

                // Now, newContractno will have the format "PR-YYYY-0001".

                // Now, you can use the newContractno for creating the new contract.


                #region Create Contract
                if (!ContractId.HasValue || ContractId.Value <= 0)
                {
                    var createdBy = await _context.Rmsemployees.AsNoTracking()
                                               .Where(x => x.Userid == LoginDetails.TmcId)
                                               .Select(x => x.Employeeid)
                                               .FirstOrDefaultAsync();
                    var ContractDetails = new Projectcontract
                    {
                        Contractno = Value.isExtended == true || Value.isProjection == true
                       ? !string.IsNullOrEmpty(Value.Contractno) ? Value.Contractno : newContractno
                       : !string.IsNullOrEmpty(Value.Contractno) ? Value.Contractno : null,


                    Ponumber = Value.Ponumber,
                        Projectid = Value.Projectid,
                        Contractstartdate = Value.Contractstartdate,
                        Contractenddate = Value.Contractenddate,
                        Amount = Value.Amount,
                        Contactpersonname = Value.Contactpersonname,
                        Costsheetid = Value.Costsheetid == 0 ? null : Value.Costsheetid,
                        Contactnumber = Value.Contractno,
                        Statusid = 12,
                        Remarks = Value.Remarks,
                        Deliveryanchorid = Value.Deliveryanchorid,
                        Attachment = string.IsNullOrEmpty(databsefilePath) ? null : databsefilePath,
                        Invoiceperiod = Value.Invoiceperiod,
                        Createdby = createdBy,
                        Createddate = DateTime.Now,
                    };

                    // Function to generate a random number
                   
                    _context.Projectcontracts.Add(ContractDetails);
                    await _context.SaveChangesAsync();
                    int newContractId = ContractDetails.Contractid;
                    if (Value.Contractlines != null)
                    {
                        foreach (var lineitem in Value.Contractlines)
                        {
                            var Contractlineitem = new Contractline
                            {
                                Contractid = ContractDetails.Contractid,
                                Lineno = lineitem.Lineno,
                                Linedescription1 = lineitem.Linedescription1,
                                Linedescription2 = lineitem.Linedescription2,
                                Lineamount = lineitem.Lineamount
                            };
                            _context.Contractlines.AddRange(Contractlineitem);
                        }
                    }
                    if (Value.Questions != null)
                    {
                        foreach (var Queitem in Value.Questions)
                        {
                            var Question = new Contractpresalesresponse
                            {
                                Contractid = ContractDetails.Contractid,
                                Clientresponse = Queitem.response,
                                Statusid = 12,
                                Question = Queitem.Question,
                                Refresponse = Queitem.Refresponse,
                                Createdby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync(),
                                Createddate = DateTime.Now,
                                Isextra=Queitem.IsExtra

                            };
                            _context.Contractpresalesresponses.AddRange(Question);
                        }
                    }
                   
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    if (Value.isExtended == true)
                    {
                        var existingContract = _context.Projectcontracts.Where(a => a.Contractid == Value.OldContractId).FirstOrDefault();
                        var cid = Value.OldContractId;
                        ContractRequestDto obj = new ContractRequestDto();
                        obj.Contractid = (int)cid;
                        if(existingContract.Contractenddate.HasValue && DateTime.Now >= existingContract.Contractenddate.Value.AddMonths(-3))
                        obj.Statusid = 12;
                        else
                        obj.Statusid = 6;
                        await UpdateContractStatus(cid, obj, LoginDetails,newContractId);

                    }

                    #endregion

                    return new Response { responseCode = StatusCodes.Status200OK, responseMessage = "Contract created successfully.",
                        data = new
                        {
                            contractid = newContractId,
                           
                        }
                    };
                }
                else
                {
                    #region Update Contract
                    var LastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
                                                .Where(x => x.Userid == LoginDetails.TmcId)
                                                .Select(x => x.Employeeid)
                                                .FirstOrDefaultAsync();

                    var existingContract=await _context.Projectcontracts.AsNoTracking()
                                                       .Include(x=>x.Contractlines)
                                                       .Include(x=>x.Contractpresalesresponses)
                                                       .FirstOrDefaultAsync(x=>x.Contractid== ContractId);  
                    if (existingContract==null) 
                    {
                        return new Response
                        {
                            responseCode = StatusCodes.Status400BadRequest,
                            responseMessage = $"Contract does not exists at given ID  : {ContractId}"
                        };
                       
                    }
                    if (ContractId !=  Value.Contractid)
                    {
                        return new Response
                        {
                            responseCode = StatusCodes.Status400BadRequest,
                            responseMessage = $" both the IDs inside inside jsaon data and querystring are not same "
                        };
                    }
                    // update contract
                    existingContract.Contractno = Value.Contractno??existingContract.Contractno;   
                    existingContract.Ponumber = Value.Ponumber??existingContract.Ponumber;
                    existingContract.Projectid=Value.Projectid??existingContract.Projectid;
                    existingContract.Contractstartdate = Value.Contractstartdate ?? existingContract.Contractstartdate;
                    existingContract.Contractenddate = Value.Contractenddate??existingContract.Contractstartdate;
                    existingContract.Amount=Value.Amount ?? existingContract.Amount;
                    existingContract.Contactpersonname=Value.Contactpersonname ?? existingContract.Contactpersonname; 
                    existingContract.Costsheetid=Value.Costsheetid ?? existingContract.Costsheetid; 
                    existingContract.Contactnumber= Value.Contactnumber ?? existingContract.Contactnumber;
                    existingContract.Statusid=Value.Statusid ?? existingContract.Statusid;   
                    existingContract.Remarks=Value.Remarks ?? existingContract.Remarks;
                    existingContract.Deliveryanchorid=Value.Deliveryanchorid ?? existingContract.Deliveryanchorid;
                    existingContract.Attachment = databsefilePath;
                    existingContract.Invoiceperiod=Value.Invoiceperiod ?? existingContract.Invoiceperiod ; 
                   // existingContract.Ctype=Value.Ctype; 
                   // existingContract.Contracttypeid=Value.Contracttypeid;
                    existingContract.Lastupdateby = LastUpdatedBy;
                    existingContract.Lastupdatedate=DateTime.Now;
                   
                    _context.Projectcontracts.Update(existingContract);
                    
                    
                   await  _context.SaveChangesAsync();

                    if (Value.Contractlines != null)
                    {
                        _context.Contractlines.RemoveRange(existingContract.Contractlines);
                        foreach (var lineitem in Value.Contractlines)
                        {
                            var Contractlineitem = new Contractline
                            {
                                Contractid = existingContract.Contractid,
                                Lineno = lineitem.Lineno,
                                Linedescription1 = lineitem.Linedescription1,
                                Linedescription2 = lineitem.Linedescription2,
                                Lineamount = lineitem.Lineamount
                            };
                            _context.Contractlines.AddRange(Contractlineitem);
                        }
                    }
                    if (Value.Questions != null)
                    {
                        _context.Contractpresalesresponses.RemoveRange(existingContract.Contractpresalesresponses);
                        foreach (var Queitem in Value.Questions)
                        {
                            var Question = new Contractpresalesresponse
                            {
                                Contractid = existingContract.Contractid,
                                Clientresponse = Queitem.response,
                                Statusid = 2,
                                Question = Queitem.Question,
                                Refresponse = Queitem.Refresponse,
                                Createdby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync(),
                                Createddate = DateTime.Now,
                                Isextra = Queitem.IsExtra
                            };
                            _context.Contractpresalesresponses.AddRange(Question);
                        }
                    }
                    #endregion
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new Response { responseCode = StatusCodes.Status200OK, responseMessage = "Contract Updated successfully." ,
                        data = new
                        {
                            contractid = existingContract.Contractid,

                        }
                    };

                }
                  
            }
            catch(Exception ex) 
            {
                await transaction.RollbackAsync();

                return new Response
                {
                    responseCode = StatusCodes.Status500InternalServerError,
                    responseMessage = $"An error occurred while upserting the contract: {ex.Message}"
                };
            }
           
        }

        //public async  Task<List<ContractListResponseDto>> GetList()
        //public async Task<List<ContractRequestDto>> GetList()
        //{   
        //    try
        //    {
        //        var response= await _context.Projectcontracts
        //                                                   .Include(x=>x.Deliveryanchor)
        //                                                   .Include(x => x.Contractlines)
        //                                                   .Include(x => x.Contractpresalesresponses)
        //                                                        .ThenInclude(x=>x.Status)
        //                                                   .Include(x => x.Status)
        //                                                   .Include(proj => proj.Project)
        //                                                       .ThenInclude(cust => cust.Customer)
        //                                                   .Include(proj => proj.Project)
        //                                                        .ThenInclude(subprac => subprac.Subpractice)
        //                                                        .ThenInclude(pract => pract.Practice)
        //                                                   .Include(proj => proj.Project)
        //                                                       .ThenInclude(ptype => ptype.Projecttype)

        //                                                   .Include(assign => assign.Contractemployees)
        //                                                        .ThenInclude(emp => emp.Employee)
        //                                                          .ThenInclude(x=>x.Subpractice)
        //                                                          .ThenInclude(x=>x.Practice)

        //                                                   .Include(assign => assign.Contractemployees)
        //                                                        .ThenInclude(bill => bill.Contractbillings)
        //                                                   .Include(assign => assign.Contractemployees)
        //                                                   .ToListAsync();

        //        var objContractRequestDto = response.Select(c => new ContractRequestDto
        //        {
        //            Contractid = c.Contractid,
        //            Contractno = c.Contractno,
        //            Ponumber = c.Ponumber,
        //            Projectid = c.Projectid,
        //            Contractstartdate = c.Contractstartdate == null ? null : c.Contractstartdate.Value.Date,
        //            Contractenddate = c.Contractenddate == null ? null : c.Contractenddate.Value.Date,
        //            Amount = c.Amount,
        //            Contactpersonname = c.Contactpersonname,
        //            Statusid = c.Statusid,
        //            Costsheetid =c.Costsheetid,
        //            Contactnumber =c.Contactnumber,
        //            Remarks =c.Remarks,
        //            Deliveryanchorid =c.Deliveryanchorid,
        //            Attachment =c.Attachment,
        //            Invoiceperiod =c.Invoiceperiod,
        //            CustomerCompanyName=c.Project.Customer.Companyname, 
        //           // Ctype =c.Ctype,
        //           // Contracttypeid =c.Contracttypeid,
        //            Contractlines = c.Contractlines.Select(cLine => new ContractlineInfo
        //            {
        //                Contractid = cLine.Contractid,
        //                Contractlineid = cLine.Contractlineid,
        //                Lineno = cLine.Lineno,
        //                Linedescription1 = cLine.Linedescription1,
        //                Linedescription2 = cLine.Linedescription2,
        //                Lineamount = cLine.Lineamount
        //            }).ToList(),
        //            Questions =c.Contractpresalesresponses.Select(x=> new ContractQuestionsInfo
        //            {
        //                Responseid = x.Responseid,
        //                Contractid=x.Contractid,
        //                response=x.Clientresponse,
        //                Statusid=x.Statusid,
        //                StatusName=x.Status == null ? "" : x.Status.Statusname,
        //                Question=x.Question,
        //                Refresponse=x.Refresponse,
        //                Remarks=x.Remarks,
        //                IsExtra=x.Isextra
        //            }).ToList(),
        //            //Extra
        //            AttachmentURL = string.IsNullOrEmpty(c.Attachment) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{c.Attachment}",
        //            //StatusName = c.Status == null ? "" : c.Status.Statusname,
        //            StatusName = c.Contractenddate >= DateTime.Now ? (c.Status == null ? "Active" : c.Status.Statusname) : "Expired",
        //            DeliveryanchorInfo = c.Deliveryanchor==null?null: new DeliveryanchorInfo { DeliveryAnchorName=c.Deliveryanchor.Employeename},
        //            ProjectInfo =c.Project==null?null:new ProjectInfo { 
        //                                          ProjectId=c.Project.Projectid,
        //                ProjectName=c.Project.Projectname,
        //                SubPracticeName=c.Project.Subpractice.Subpracticename,
        //                PracticeName = c.Project.Subpractice.Practice.Practicename,
        //                ProjectType=c.Project.Projecttype.Projecttypename,
        //                CustomerName=c.Project.Customer.Companyname,
        //                Email=c.Project.Customer.Companyemail,
        //                ContactNo=string.IsNullOrEmpty(c.Project.Customer.Phone1)? c.Project.Customer.Phone2: c.Project.Customer.Phone1,
        //                Address1=c.Project.Customer.Address1,
        //                Address2 = c.Project.Customer.Address2,
        //                CustomerLogo= string.IsNullOrEmpty(c.Project.Customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{c.Project.Customer.Companylogourl}",

        //            },
        //            ContractEmployees = c.Contractemployees==null?null:c.Contractemployees.Select(x=> new ContractEmployee
        //            {
        //                ContractEmployeeId=x.Contractemployeeid,
        //                EmployeeName=x.Employee.Employeename,
        //                TmcCode=x.Employee.Userid,
        //                PracticeName=x.Employee.Subpractice.Practice.Practicename,
        //                ContractBillings= x.Contractbillings==null?null:x.Contractbillings.Select(b=>new ContractBillinginfo
        //                {
        //                                                    ContractBillingId=b.Contractbillingid,
        //                    ContractEmployeeId=b.Contractemployeeid,
        //                    BillingMonthYear=b.Billingmonthyear,
        //                    Costing=b.Costing
        //                }).ToList(),

        //            }).ToList(),
        //        }).ToList();


        //        return objContractRequestDto;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}
        public async Task<List<ContractRequestDto>> GetList()
        {
            try
            {
                var response = await _context.Projectcontracts.AsNoTracking()
                    .Include(x => x.Deliveryanchor)
                    .Include(x => x.Contractlines)
                    .Include(x => x.Contractpresalesresponses)
                        .ThenInclude(x => x.Status)
                    .Include(x => x.Status)
                    .Include(proj => proj.Project)
                        .ThenInclude(cust => cust.Customer)
                    .Include(proj => proj.Project)
                        .ThenInclude(subprac => subprac.Subpractice)
                            .ThenInclude(pract => pract.Practice)
                    .Include(proj => proj.Project)
                        .ThenInclude(ptype => ptype.Projecttype)
                    .Include(assign => assign.Contractemployees)
                        .ThenInclude(emp => emp.Employee)
                            .ThenInclude(x => x.Subpractice)
                                .ThenInclude(x => x.Practice)
                    .Include(assign => assign.Contractemployees)
                        .ThenInclude(bill => bill.Contractbillings)
                    .Include(assign => assign.Contractemployees)
                    .ToListAsync();

                var objContractRequestDto = response.Select(c => new ContractRequestDto
                {
                    Contractid = c.Contractid,
                    Contractno = c.Contractno,
                    Ponumber = c.Ponumber,
                    Projectid = c.Projectid,
                    Contractstartdate = c.Contractstartdate == null ? null : c.Contractstartdate.Value.Date,
                    Contractenddate = c.Contractenddate == null ? null : c.Contractenddate.Value.Date,
                    Amount = c.Amount,
                    Contactpersonname = c.Contactpersonname,
                    Statusid = c.Statusid,
                    Costsheetid = c.Costsheetid,
                    Contactnumber = c.Contactnumber,
                    Remarks = c.Remarks,
                    Deliveryanchorid = c.Deliveryanchorid,
                    Attachment = c.Attachment,
                    Invoiceperiod = c.Invoiceperiod,
                    CustomerCompanyName = c.Project?.Customer?.Companyname ?? "",
                    //StatusName = c.Contractenddate >= DateTime.Now ? (c.Statusid == null ? "Active" : c.Status.Statusname) : c.Status.Statusname,
                    //StatusName = c.Contractenddate >= DateTime.Now && c.Status != null ? c.Status.Statusname : "Active",
                    StatusName = c.Contractenddate >= DateTime.Now ? (c.Statusid == null ? "Active" : c.Status?.Statusname) : c.Status?.Statusname ?? "Active",

                    //DeliveryanchorInfo = c.Deliveryanchor == null ? null : new DeliveryanchorInfo { DeliveryAnchorName = c.Deliveryanchor.Employeename },
                    DeliveryAnchorName = c.Deliveryanchor == null ? null : c.Deliveryanchor.Employeename,
                    ProjectInfo = c.Project == null ? null : new ProjectInfo
                    {
                        ProjectId = c.Project.Projectid,
                        ProjectName = c.Project.Projectname,
                        SubPracticeName = c.Project.Subpractice?.Subpracticename ?? "",
                        PracticeName = c.Project.Subpractice?.Practice?.Practicename ?? "",
                        ProjectType = c.Project.Projecttype?.Projecttypename ?? "",
                        CustomerName = c.Project.Customer?.Companyname ?? "",
                        Email = c.Project.Customer?.Companyemail ?? "",
                        ContactNo = string.IsNullOrEmpty(c.Project.Customer?.Phone1) ? c.Project.Customer?.Phone2 : c.Project.Customer?.Phone1 ?? "",
                        Address1 = c.Project.Customer?.Address1 ?? "",
                        Address2 = c.Project.Customer?.Address2 ?? "",
                        CustomerLogo = string.IsNullOrEmpty(c.Project.Customer?.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{c.Project.Customer.Companylogourl}",
                    },
                    ContractEmployees = c.Contractemployees == null ? null : c.Contractemployees.Select(x => new ContractEmployee
                    {
                        Contractemployeeid = x.Contractemployeeid,
                        EmployeeName = x.Employee?.Employeename ?? "",
                        TmcCode = x.Employee?.Userid ?? "",
                        PracticeName = x.Employee?.Subpractice?.Practice?.Practicename ?? "",
                        ContractBillings = x.Contractbillings == null ? null : x.Contractbillings.Select(b => new ContractBillinginfo
                        {
                            ContractBillingId = b.Contractbillingid,
                            Contractemployeeid = b.Contractemployeeid,
                            BillingMonthYear = b.Billingmonthyear,
                            Costing = b.Costing
                        }).ToList() ?? new List<ContractBillinginfo>(),
                    }).ToList() ?? new List<ContractEmployee>(),
                }).ToList();

                return objContractRequestDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<ContractRequestDto>> GetContractlistByProcedure(int? deliveryAnchorId)
        {
            var filteredResult =new List<ContractRequestDto>();    
            try
            {
                var response = await _context.ContractlistDtos
                                                    .FromSqlRaw("SELECT * FROM GetListofContracts({0})", deliveryAnchorId)
                                                    .ToListAsync();
                if (response != null)
                {
                    filteredResult = response.Select(r=>new ContractRequestDto
                    {
                        Contractid=r.Contractid,
                        Contractno=r.Contractno,
                        Ponumber=r.Ponumber,
                        Contractstartdate=r.Contractstartdate==null?null: r.Contractstartdate,  
                        Contractenddate=r.Contractenddate==null?null: r.Contractenddate,
                        Amount=r.Amount,
                        Statusid=r.Statusid,
                        StatusName = r.Contractenddate >= DateTime.Now ? (r.Statusid == null ? "Active" : r.StatusName) : r.StatusName ?? "Active",
                        DeliveryAnchorName= r.DeliveryAnchorName ,
                        CustomerCompanyName=r.customerCompanyName
                        
                    }).ToList();
                }
              




                return filteredResult;

            }
            catch
            {
                throw;
            }
        }

        public async Task<int> ContractCount()
        {
            var count = await _context.Projectcontracts
                                        .Include(x => x.Deliveryanchor)
                                        .Include(x => x.Contractlines)
                                        .Include(x => x.Contractpresalesresponses)
                                            .ThenInclude(x => x.Status)
                                        .Include(x => x.Status)
                                        .Include(proj => proj.Project)
                                            .ThenInclude(cust => cust.Customer)
                                        .Include(proj => proj.Project)
                                            .ThenInclude(subprac => subprac.Subpractice)
                                                .ThenInclude(pract => pract.Practice)
                                        .Include(proj => proj.Project)
                                            .ThenInclude(ptype => ptype.Projecttype)
                                        .Include(assign => assign.Contractemployees)
                                            .ThenInclude(emp => emp.Employee)
                                                .ThenInclude(x => x.Subpractice)
                                                    .ThenInclude(x => x.Practice)
                                        .Include(assign => assign.Contractemployees)
                                            .ThenInclude(bill => bill.Contractbillings)
                                        .Include(assign => assign.Contractemployees)
                                        .OrderBy(c => c.Contractid) 
                                        .CountAsync();

            return count;
        }


        public async Task<List<ContractRequestDto>> GetListPagination(int pageNumber, int pageSize)
        {
            try
            {
                // Skip the records from the previous pages and take the number of records defined by pageSize
                var response = await _context.Projectcontracts.AsNoTracking()
                   
                                            .Include(x => x.Deliveryanchor)
                                            .Include(x => x.Contractlines)
                                            .Include(x => x.Contractpresalesresponses)
                                                .ThenInclude(x => x.Status)
                                            .Include(x => x.Status)
                                            .Include(proj => proj.Project)
                                                .ThenInclude(cust => cust.Customer)
                                            .Include(proj => proj.Project)
                                                .ThenInclude(subprac => subprac.Subpractice)
                                                    .ThenInclude(pract => pract.Practice)
                                            .Include(proj => proj.Project)
                                                .ThenInclude(ptype => ptype.Projecttype)
                                            .Include(assign => assign.Contractemployees)
                                                .ThenInclude(emp => emp.Employee)
                                                    .ThenInclude(x => x.Subpractice)
                                                        .ThenInclude(x => x.Practice)
                                            .Include(assign => assign.Contractemployees)
                                                .ThenInclude(bill => bill.Contractbillings)
                                            .Include(assign => assign.Contractemployees)
            
                                        .OrderBy(c => c.Contractid) // Make sure to order by a unique key
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

                var objContractRequestDto = response.Select(c => new ContractRequestDto
                {
                    Contractid = c.Contractid,
                    Contractno = c.Contractno,
                    Ponumber = c.Ponumber,
                    Projectid = c.Projectid,
                    Contractstartdate = c.Contractstartdate == null ? null : c.Contractstartdate.Value.Date,
                    Contractenddate = c.Contractenddate == null ? null : c.Contractenddate.Value.Date,
                    Amount = c.Amount,
                    Contactpersonname = c.Contactpersonname,
                    Statusid = c.Statusid,
                    Costsheetid = c.Costsheetid,
                    Contactnumber = c.Contactnumber,
                    Remarks = c.Remarks,
                    Deliveryanchorid = c.Deliveryanchorid,
                    Attachment = c.Attachment,
                    Invoiceperiod = c.Invoiceperiod,
                    CustomerCompanyName = c.Project?.Customer?.Companyname ?? "",
                    //StatusName = c.Contractenddate >= DateTime.Now ? (c.Statusid == null ? "Active" : c.Status.Statusname) : c.Status.Statusname,
                    //StatusName = c.Contractenddate >= DateTime.Now && c.Status != null ? c.Status.Statusname : "Active",
                    StatusName = c.Contractenddate >= DateTime.Now ? (c.Statusid == null ? "Active" : c.Status?.Statusname) : c.Status?.Statusname ?? "Active",

                    //DeliveryanchorInfo = c.Deliveryanchor == null ? null : new DeliveryanchorInfo { DeliveryAnchorName = c.Deliveryanchor.Employeename },
                    DeliveryAnchorName = c.Deliveryanchor == null ? null : c.Deliveryanchor.Employeename,
                    ProjectInfo = c.Project == null ? null : new ProjectInfo
                    {
                        ProjectId = c.Project.Projectid,
                        ProjectName = c.Project.Projectname,
                        SubPracticeName = c.Project.Subpractice?.Subpracticename ?? "",
                        PracticeName = c.Project.Subpractice?.Practice?.Practicename ?? "",
                        ProjectType = c.Project.Projecttype?.Projecttypename ?? "",
                        CustomerName = c.Project.Customer?.Companyname ?? "",
                        Email = c.Project.Customer?.Companyemail ?? "",
                        ContactNo = string.IsNullOrEmpty(c.Project.Customer?.Phone1) ? c.Project.Customer?.Phone2 : c.Project.Customer?.Phone1 ?? "",
                        Address1 = c.Project.Customer?.Address1 ?? "",
                        Address2 = c.Project.Customer?.Address2 ?? "",
                        CustomerLogo = string.IsNullOrEmpty(c.Project.Customer?.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{c.Project.Customer.Companylogourl}",
                    },
                    ContractEmployees = c.Contractemployees == null ? null : c.Contractemployees.Select(x => new ContractEmployee
                    {
                        Contractemployeeid = x.Contractemployeeid,
                        EmployeeName = x.Employee?.Employeename ?? "",
                        TmcCode = x.Employee?.Userid ?? "",
                        PracticeName = x.Employee?.Subpractice?.Practice?.Practicename ?? "",
                        ContractBillings = x.Contractbillings == null ? null : x.Contractbillings.Select(b => new ContractBillinginfo
                        {
                            ContractBillingId = b.Contractbillingid,
                            Contractemployeeid = b.Contractemployeeid,
                            BillingMonthYear = b.Billingmonthyear,
                            Costing = b.Costing
                        }).ToList() ?? new List<ContractBillinginfo>(),
                    }).ToList() ?? new List<ContractEmployee>(),
                }).ToList();
                return objContractRequestDto;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }



        public Task<Projectcontract> GetByName(string name)
        {
            throw new NotImplementedException();
        }
        //public async Task<ContractRequestDto> GetContractById(int id)
        //{
        //    try
        //    {
        //        var c = await _context.Projectcontracts.AsNoTracking().Where(x => x.Contractid == id)
        //                                                   .Include(x => x.Deliveryanchor)
        //                                                   .Include(x => x.Contractlines)
        //                                                   .Include(x => x.Contractpresalesresponses)
        //                                                        .ThenInclude(x => x.Status)
        //                                                   .Include(x => x.Status)
        //                                                   .Include(proj => proj.Project)
        //                                                       .ThenInclude(cust => cust.Customer)
        //                                                   .Include(proj => proj.Project)
        //                                                        .ThenInclude(subprac => subprac.Subpractice)
        //                                                        .ThenInclude(pract => pract.Practice)
        //                                                   .Include(proj => proj.Project)
        //                                                       .ThenInclude(ptype => ptype.Projecttype)

        //                                                   .Include(assign => assign.Contractemployees)
        //                                                        .ThenInclude(emp => emp.Employee)
        //                                                          .ThenInclude(x => x.Subpractice)
        //                                                          .ThenInclude(x => x.Practice)

        //                                                        .Include(assign => assign.Contractemployees)
        //                                                        .ThenInclude(bill => bill.Contractbillings)

        //                                                        .Include(assign => assign.Contractemployees)
        //                                                        .ThenInclude(pro => pro.Contractbillingprovesions)
        //                                                             .ThenInclude(x=>x.Status)

        //                                                        .Include(assign => assign.Contractemployees)
        //                                                        .ThenInclude(pro => pro.Contractbillingprovesions)
        //                                                            .ThenInclude(history=>history.Contractbillingprovisionhistories)
        //                                                             .ThenInclude(x => x.Status)
        //                                                    .Include(assign => assign.Contractemployees)
        //                                                        .ThenInclude(initial => initial.Initialsplitcontractbillings)



        //                                                                                 .FirstOrDefaultAsync();




        //        var objContractRequestDto = c==null?null: new ContractRequestDto
        //        {
        //            Contractid = c.Contractid,
        //            Contractno = c.Contractno,
        //            Ponumber = c.Ponumber,
        //            Projectid = c.Projectid,
        //            Contractstartdate = c.Contractstartdate == null ? null : c.Contractstartdate.Value.Date,
        //            Contractenddate = c.Contractenddate == null ? null : c.Contractenddate.Value.Date,
        //            Amount = c.Amount,
        //            Contactpersonname = c.Contactpersonname,
        //            Statusid = c.Statusid,
        //            Costsheetid = c.Costsheetid,
        //            Contactnumber = c.Contactnumber,
        //            Remarks = c.Remarks,
        //            Deliveryanchorid = c.Deliveryanchorid,
        //            Attachment = c.Attachment,
        //            Invoiceperiod = c.Invoiceperiod,
        //           // Ctype = c.Ctype,
        //            //Contracttypeid = c.Contracttypeid,
        //            Contractlines = c.Contractlines==null?null: c.Contractlines.Select(cLine=>new ContractlineInfo
        //                                                                    {
        //                                                                        Contractid=cLine.Contractid,
        //                                                                        Contractlineid=cLine.Contractlineid,
        //                                                                        Lineno=cLine.Lineno,
        //                                                                        Linedescription1= cLine.Linedescription1,
        //                                                                        Linedescription2= cLine.Linedescription2,
        //                                                                        Lineamount = cLine.Lineamount   
        //                                                                    }).ToList(),
        //            Questions = c.Contractpresalesresponses.Select(x => new ContractQuestionsInfo
        //            {
        //                Responseid = x.Responseid,
        //                Contractid = x.Contractid,
        //                response = x.Clientresponse,
        //                Statusid = x.Statusid,
        //                StatusName = x.Status == null ? "" : x.Status.Statusname,
        //                Question = x.Question,
        //                Refresponse = x.Refresponse,
        //                Remarks = x.Remarks,
        //                IsExtra=x.Isextra
        //            }).ToList(),
        //            //Extra
        //            AttachmentURL = string.IsNullOrEmpty(c.Attachment) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{c.Attachment}",
        //            StatusName = c.Status == null ? "" : c.Status.Statusname,
        //            //DeliveryanchorInfo = c.Deliveryanchor == null ? null : new DeliveryanchorInfo { DeliveryAnchorName = c.Deliveryanchor.Employeename },
        //            DeliveryAnchorName =c.Deliveryanchor == null ? null : c.Deliveryanchor.Employeename,
        //            ProjectInfo = c.Project == null ? null : new ProjectInfo
        //            {
        //                ProjectId = c.Project.Projectid,
        //                ProjectName = c.Project.Projectname,
        //                SubPracticeName = c.Project.Subpractice.Subpracticename,
        //                PracticeName = c.Project.Subpractice.Practice.Practicename,
        //                ProjectType = c.Project.Projecttype.Projecttypename,
        //                CustomerName = c.Project.Customer.Companyname,
        //                CustomerId = c.Project.Customer.Customerid,
        //                Email = c.Project.Customer.Companyemail,
        //                ContactNo = string.IsNullOrEmpty(c.Project.Customer.Phone1) ? c.Project.Customer.Phone2 : c.Project.Customer.Phone1,
        //                Address1 = c.Project.Customer.Address1,
        //                Address2 = c.Project.Customer.Address2,
        //                CustomerLogo = string.IsNullOrEmpty(c.Project.Customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{c.Project.Customer.Companylogourl}",

        //            },
        //            ContractEmployees = c.Contractemployees?.Select(x => new ContractEmployee
        //            {
        //                Contractemployeeid = x.Contractemployeeid,
        //                EmployeeName = x.Employee?.Employeename, 
        //                EmployeeId = x.Employeeid,
        //                TmcCode = x.Employee?.Userid, 
        //                PracticeName = x.Employee?.Subpractice?.Practice?.Practicename, 
        //                InitialBillings=x.Initialsplitcontractbillings?.Select(i=> new InitialBillinginfo
        //                {
        //                    Contractbillingid = i.Contractbillingid,
        //                    Contractemployeeid = i.Contractemployeeid,
        //                    BillingMonthYear = ConvertToValidDate(i.Billingmonthyear), 
        //                    Costing = i.Costing,                         
        //                }).OrderBy(i=> DateTime.ParseExact(i.BillingMonthYear, "MMM-yy", CultureInfo.InvariantCulture)) 
        //                  .ToList(),

        //                ContractBillings = x.Contractbillings?.Select(b => new ContractBillinginfo
        //                {
        //                    ContractBillingId = b.Contractbillingid,
        //                    Contractemployeeid = b.Contractemployeeid,
        //                    BillingMonthYear = ConvertToValidDate(b.Billingmonthyear), 
        //                    Costing = b.Costing,
        //                    Expecteddate = b.Estimatedbillingdate,
        //                    BillingStatus=b.Isbilled==true? "Isbilled":(b.Istobebilled==true? "Istobebilled":null)
        //                })
        //                 .OrderBy(b => DateTime.ParseExact(b.BillingMonthYear, "MMM-yy", CultureInfo.InvariantCulture)) 
        //                 .ToList(),
        //                ContractBillingsProvisional = x.Contractbillingprovesions?.Select(bp => new ContractBillingProvisionalInfo
        //                {
        //                    Contractbillingprovesionid = bp.Contractbillingprovesionid,
        //                    Contractemployeeid = bp.Contractemployeeid,
        //                    Billingmonthyear = ConvertToValidDate(bp.Billingmonthyear),
        //                    //Costing =bp.Statusid==4?0:
        //                    //                          (bp.Isrevised==false? (bp.Costing):
        //                    //                          (bp.Contractbillingprovisionhistories.OrderByDescending(x=>x.Revisionnumber).Select(x=>x.Statusid).FirstOrDefault()==2? (bp.Costing) :
        //                    //                           bp.Contractbillingprovisionhistories.OrderByDescending(x => x.Revisionnumber).Select(x => x.Oldcosting).FirstOrDefault())),
        //                    Costing = bp.Statusid == 4 ? 0 :
        //                                   bp.Isrevised == false ? bp.Costing :
        //                                                           bp.Contractbillingprovisionhistories.OrderByDescending(x => x.Revisionnumber).FirstOrDefault(x => x.Statusid == 2)?.Oldcosting ?? bp.Costing,




        //                    StatusId = bp.Statusid,
        //                    Status=bp.Status==null? "Draft" : bp.Status.Statusname,
        //                    Expecteddate = bp.EstimatedBillingDate,
        //                    BillingStatus = bp.Isbilled == true ? "Isbilled" : (bp.Istobebilled == true ? "Istobebilled" : null)
        //                }).OrderBy(bp => DateTime.ParseExact(bp.Billingmonthyear, "MMM-yy", CultureInfo.InvariantCulture)).ToList(),

        //            }).ToList(),

        //    };

        //        if (objContractRequestDto != null)
        //        {
        //            foreach (var item in objContractRequestDto.ContractEmployees)
        //            {
        //                // Asynchronously fetch and set the employee status
        //                item.EmployeeSataus = await _employeeStatusService.GetEmployeeStatusAsync(item.Contractemployeeid);
        //            }
        //        }




        //        return objContractRequestDto;

        //    }

        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public async Task<ContractRequestDto> GetContractById(int id)
        {
            try
            {
                var contractQuery = _context.Projectcontracts.AsNoTracking().Where(x => x.Contractid == id).Include(x => x.Extendedcontracts)
                                                              .Include(x => x.Deliveryanchor)
                                                              .Include(x => x.Status)
                                                              .Include(proj => proj.Project)
                                                                  .ThenInclude(cust => cust.Customer)
                                                              .Include(proj => proj.Project)
                                                                  .ThenInclude(subprac => subprac.Subpractice)
                                                                  .ThenInclude(pract => pract.Practice)
                                                              .Include(proj => proj.Project)
                                                                  .ThenInclude(ptype => ptype.Projecttype);

                var contract = await contractQuery.FirstOrDefaultAsync();

                if (contract == null)
                    return null;

                var contractLines = await _context.Contractlines.AsNoTracking().Where(cl => cl.Contractid == id).ToListAsync();
                var contractPresalesResponses = await _context.Contractpresalesresponses.AsNoTracking().Where(cpr => cpr.Contractid == id)
                                                                .Include(cpr => cpr.Status).ToListAsync();
                var contractEmployees = await _context.Contractemployees.AsNoTracking().Where(ce => ce.Contractid == id)
                                                                .Include(emp => emp.Employee)
                                                                .ThenInclude(x => x.Subpractice)
                                                                .ThenInclude(x => x.Practice)
                                                                .Include(emp => emp.Contractbillings)
                                                                .Include(emp => emp.Contractbillingprovesions)
                                                                .ThenInclude(bp => bp.Status)
                                                                .Include(emp => emp.Contractbillingprovesions)
                                                                .ThenInclude(bp => bp.Contractbillingprovisionhistories)
                                                                .ThenInclude(h => h.Status)
                                                                .Include(emp => emp.Initialsplitcontractbillings)
                                                                .ToListAsync();

                var oaf = await _context.Oafs.AsNoTracking().Where(oaf => oaf.Oafid == contract.Oafid).ToListAsync();


                var contractRequestDto = new ContractRequestDto
                {
                    Contractid = contract.Contractid,
                    Contractno = contract.Contractno,
                    Ponumber = contract.Ponumber,
                    Projectid = contract.Projectid,
                    Contractstartdate = contract.Contractstartdate?.Date,
                    Contractenddate = contract.Contractenddate?.Date,
                    Amount = contract.Amount,
                    Contactpersonname = contract.Contactpersonname,
                    Statusid = contract.Statusid,
                    Costsheetid = contract.Costsheetid,
                    Contactnumber = contract.Contactnumber,
                    Remarks = contract.Remarks,
                    Deliveryanchorid = contract.Deliveryanchorid,
                    Attachment = contract.Attachment,
                    Invoiceperiod = contract.Invoiceperiod,
                    Isprojectestimationdone = contract.Isprojectestimationdone,
                    extendedcontracts = contract?.Extendedcontracts.Where(exc => exc.Oldcontractid == id).ToList(),
                    Oaf = oaf.Select(o => new Oaf{
                        Oafid = o.Oafid,
                        Costsheetid=o.Costsheetid,
                    }).ToList(),
                    Contractlines = contractLines.Select(cLine => new ContractlineInfo
                    {
                        Contractid = cLine.Contractid,
                        Contractlineid = cLine.Contractlineid,
                        Lineno = cLine.Lineno,
                        Linedescription1 = cLine.Linedescription1,
                        Linedescription2 = cLine.Linedescription2,
                        Lineamount = cLine.Lineamount
                    }).ToList(),
                    Questions = contractPresalesResponses.Select(x => new ContractQuestionsInfo
                    {
                        Responseid = x.Responseid,
                        Contractid = x.Contractid,
                        response = x.Clientresponse,
                        Statusid = x.Statusid,
                        StatusName = x.Status?.Statusname ?? "",
                        Question = x.Question,
                        Refresponse = x.Refresponse,
                        Remarks = x.Remarks,
                        IsExtra = x.Isextra
                    }).ToList(),
                    AttachmentURL = string.IsNullOrEmpty(contract.Attachment) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{contract.Attachment}",
                    StatusName = contract.Status?.Statusname ?? "",
                    DeliveryAnchorName = contract.Deliveryanchor?.Employeename,
                    ProjectInfo = contract.Project == null ? null : new ProjectInfo
                    {
                        ProjectId = contract.Project.Projectid,
                        ProjectName = contract.Project.Projectname,
                        CommittedClientBillingDate=contract.Project?.CommittedClientBillingDate,
                        SubPracticeName = contract.Project.Subpractice.Subpracticename,
                        PracticeName = contract.Project.Subpractice.Practice.Practicename,
                        ProjectType = contract.Project.Projecttype.Projecttypename,
                        CustomerName = contract.Project.Customer.Companyname,
                        CustomerId = contract.Project.Customer.Customerid,
                        Email = contract.Project.Customer.Companyemail,
                        ContactNo = string.IsNullOrEmpty(contract.Project.Customer.Phone1) ? contract.Project.Customer.Phone2 : contract.Project.Customer.Phone1,
                        Address1 = contract.Project.Customer.Address1,
                        Address2 = contract.Project.Customer.Address2,
                        CustomerLogo = string.IsNullOrEmpty(contract.Project.Customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{contract.Project.Customer.Companylogourl}",
                    },
                    ContractEmployees = contractEmployees.Select(x => new ContractEmployee
                    {
                        Contractemployeeid = x.Contractemployeeid,
                        EmployeeName = x.Employee?.Employeename,
                        EmployeeId = x.Employeeid,
                        TmcCode = x.Employee?.Userid,
                        PracticeName = x.Employee?.Subpractice?.Practice?.Practicename,
                        InitialBillings = x.Initialsplitcontractbillings?.Select(i => new InitialBillinginfo
                        {
                            Contractbillingid = i.Contractbillingid,
                            Contractemployeeid = i.Contractemployeeid,
                            BillingMonthYear = ConvertToValidDate(i.Billingmonthyear),
                            Costing = i.Costing,
                        }).OrderBy(i => DateTime.ParseExact(i.BillingMonthYear, "MMM-yy", CultureInfo.InvariantCulture)).ToList(),
                        ContractBillings = x.Contractbillings?.Select(b => new ContractBillinginfo
                        {
                            ContractBillingId = b.Contractbillingid,
                            Contractemployeeid = b.Contractemployeeid,
                            BillingMonthYear = ConvertToValidDate(b.Billingmonthyear),
                            Costing = b.Costing,
                            Expecteddate = b.Estimatedbillingdate,
                            BillingStatus = b.Isbilled == true ? "Isbilled" : (b.Istobebilled == true ? "Istobebilled" : null)
                        }).OrderBy(i=> DateTime.ParseExact(i.BillingMonthYear, "MMM-yy", CultureInfo.InvariantCulture)).ToList(),
                        ContractBillingsProvisional = x.Contractbillingprovesions?.Select(bp => new ContractBillingProvisionalInfo
                        {
                            Contractbillingprovesionid = bp.Contractbillingprovesionid,
                            Contractemployeeid = bp.Contractemployeeid,
                            Billingmonthyear = ConvertToValidDate(bp.Billingmonthyear),
                            Costing = bp.Statusid == 4 ? 0 :
                                      bp.Isrevised == false ? bp.Costing :
                                      bp.Contractbillingprovisionhistories.OrderByDescending(x => x.Revisionnumber).FirstOrDefault(x => x.Statusid == 2)?.Oldcosting ?? bp.Costing,
                            StatusId = bp.Statusid,
                            Status = bp.Status?.Statusname ?? "Draft",
                            Expecteddate = bp.EstimatedBillingDate,
                            BillingStatus = bp.Isbilled == true ? "Isbilled" : (bp.Istobebilled == true ? "Istobebilled" : null)
                        }).OrderBy(i => DateTime.ParseExact(i.Billingmonthyear, "MMM-yy", CultureInfo.InvariantCulture)).ToList(),
                        
                    }).ToList(),
                };

                if (contractRequestDto != null)
                {
                    foreach (var item in contractRequestDto.ContractEmployees)
                    {
                        item.EmployeeSataus = await _employeeStatusService.GetEmployeeStatusAsync(item.Contractemployeeid);
                    }
                }

                return contractRequestDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<List<ContractRequestDto>> GetContractByAnchorId(int id)
        {
            try
            {
                var response = await _context.Projectcontracts.AsNoTracking()
                                                           .Include(x => x.Deliveryanchor)
                                                           .Include(x => x.Contractlines)
                                                           .Include(x => x.Contractpresalesresponses)
                                                                .ThenInclude(x => x.Status)
                                                           .Include(x => x.Status)
                                                           .Include(proj => proj.Project)
                                                               .ThenInclude(cust => cust.Customer)
                                                           .Include(proj => proj.Project)
                                                                .ThenInclude(subprac => subprac.Subpractice)
                                                                .ThenInclude(pract => pract.Practice)
                                                           .Include(proj => proj.Project)
                                                               .ThenInclude(ptype => ptype.Projecttype)

                                                           .Include(assign => assign.Contractemployees)
                                                                .ThenInclude(emp => emp.Employee)
                                                                  .ThenInclude(x => x.Subpractice)
                                                                  .ThenInclude(x => x.Practice)

                                                           .Include(assign => assign.Contractemployees)
                                                                .ThenInclude(bill => bill.Contractbillings)
                                            .ToListAsync();

                var objContractRequestDto = response.Select(c => new ContractRequestDto
                {
                    Contractid = c.Contractid,
                    Contractno = c.Contractno,
                    Ponumber = c.Ponumber,
                    Projectid = c.Projectid,
                    Contractstartdate = c.Contractstartdate == null ? null : c.Contractstartdate.Value.Date,
                    Contractenddate = c.Contractenddate == null ? null : c.Contractenddate.Value.Date,
                    Amount = c.Amount,
                    Contactpersonname = c.Contactpersonname,
                    Statusid = c.Statusid,
                    Costsheetid = c.Costsheetid,
                    Contactnumber = c.Contactnumber,
                    Remarks = c.Remarks,
                    Deliveryanchorid = c.Deliveryanchorid,
                    Attachment = c.Attachment,
                    Invoiceperiod = c.Invoiceperiod,
                    // Ctype = c.Ctype,
                    // Contracttypeid = c.Contracttypeid,
                    CustomerCompanyName = c.Project?.Customer?.Companyname ?? "",
                    Contractlines = c.Contractlines.Select(cLine => new ContractlineInfo
                    {
                        Contractid = cLine.Contractid,
                        Contractlineid = cLine.Contractlineid,
                        Lineno = cLine.Lineno,
                        Linedescription1 = cLine.Linedescription1,
                        Linedescription2 = cLine.Linedescription2,
                        Lineamount = cLine.Lineamount
                    }).ToList(),
                    Questions = c.Contractpresalesresponses.Select(x => new ContractQuestionsInfo
                    {
                        Responseid = x.Responseid,
                        Contractid = x.Contractid,
                        response = x.Clientresponse,
                        Statusid = x.Statusid,
                        StatusName = x.Status == null ? "" : x.Status.Statusname,
                        Question = x.Question,
                        Refresponse = x.Refresponse,
                        Remarks = x.Remarks,
                        IsExtra = x.Isextra
                    }).ToList(),
                    //Extra
                    AttachmentURL = string.IsNullOrEmpty(c.Attachment) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{c.Attachment}",
                    //StatusName = c.Status == null ? "" : c.Status.Statusname,
                    StatusName = c.Contractenddate >= DateTime.Now ? (c.Status == null ? "Active" : c.Status.Statusname) : "Expired",
                    DeliveryAnchorName = c.Deliveryanchor == null ? null :  c.Deliveryanchor.Employeename ,
                    ProjectInfo = c.Project == null ? null : new ProjectInfo
                    {
                        ProjectId = c.Project.Projectid,
                        ProjectName = c.Project.Projectname,
                        SubPracticeName = c.Project.Subpractice.Subpracticename,
                        PracticeName = c.Project.Subpractice.Practice.Practicename,
                        ProjectType = c.Project.Projecttype.Projecttypename,
                        CustomerName = c.Project.Customer.Companyname,
                        Email = c.Project.Customer.Companyemail,
                        ContactNo = string.IsNullOrEmpty(c.Project.Customer.Phone1) ? c.Project.Customer.Phone2 : c.Project.Customer.Phone1,
                        Address1 = c.Project.Customer.Address1,
                        Address2 = c.Project.Customer.Address2,
                        CustomerLogo = string.IsNullOrEmpty(c.Project.Customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{c.Project.Customer.Companylogourl}",

                    },
                    ContractEmployees = c.Contractemployees == null ? null : c.Contractemployees.Select(x => new ContractEmployee
                    {
                        Contractemployeeid = x.Contractemployeeid,
                        EmployeeName = x.Employee.Employeename,
                        TmcCode = x.Employee.Userid,
                        PracticeName = x.Employee.Subpractice?.Practice?.Practicename,
                        ContractBillings = x.Contractbillings == null ? null : x.Contractbillings.Select(b => new ContractBillinginfo
                        {
                            ContractBillingId = b.Contractbillingid,
                            Contractemployeeid = b.Contractemployeeid,
                            BillingMonthYear = b.Billingmonthyear,
                            Costing = b.Costing
                        }).ToList(),


                    }).ToList(),
                }).ToList();


                return objContractRequestDto.Where(x=>x.Deliveryanchorid==id).ToList();

            }

            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Response> UpdateContractStatus(int? ContractId, ContractRequestDto Value, JwtLoginDetailDto LoginDetails, int? newcontractid)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get the LastUpdatedBy ID
                var LastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
                    .Where(x => x.Userid == LoginDetails.TmcId)
                    .Select(x => x.Employeeid)
                    .FirstOrDefaultAsync();

                // Fetch the existing contract
                var existingContract = await _context.Projectcontracts
                    .FirstOrDefaultAsync(x => x.Contractid == ContractId);

                // Check if contract exists
                if (existingContract == null)
                {
                    return new Response
                    {
                        responseCode = StatusCodes.Status400BadRequest,
                        responseMessage = $"Contract does not exist at given ID: {ContractId}"
                    };
                }

                // Check if ContractId matches the one in Value
                if (ContractId != Value.Contractid)
                {
                    return new Response
                    {
                        responseCode = StatusCodes.Status400BadRequest,
                        responseMessage = "Both the IDs inside JSON data and query string are not the same."
                    };
                }

                // Update contract status based on the conditions
                if (existingContract.Contractenddate.HasValue && DateTime.Now >= existingContract.Contractenddate.Value.AddMonths(-3) && Value.Statusid !=6)
                {
                    existingContract.Statusid = existingContract.Statusid; // Retain existing status
                }
                else
                {
                    existingContract.Statusid = Value.Statusid ?? existingContract.Statusid;
                }

                // Update the contract's last updated info
                existingContract.Lastupdateby = LastUpdatedBy;
                existingContract.Lastupdatedate = DateTime.Now;
                _context.Projectcontracts.Update(existingContract);

                // Create and add the extension update if applicable
                if (newcontractid.HasValue)
                {
                    var extensionUpdate = new Extendedcontract
                    {
                        Extendedcontractid = newcontractid.Value,
                        Oldcontractid = existingContract.Contractid,
                        Lastupdatedate = DateTime.Now,
                        Lastupdatedby = LastUpdatedBy.ToString(),
                        Createdby = LastUpdatedBy.ToString(),
                        Createddate = DateTime.Now,
                    };
                    _context.Extendedcontracts.Add(extensionUpdate);
                }

                // Save the changes
                await _context.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                return new Response { responseCode = StatusCodes.Status200OK, responseMessage = "Contract Updated successfully." };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new Response
                {
                    responseCode = StatusCodes.Status500InternalServerError,
                    responseMessage = $"An error occurred while updating the contract: {ex.Message}"
                };
            }
        }

        private string ConvertToValidDate(string billingMonthYear)
        {
            // Replace "Sept" with "Sep" if present
            return billingMonthYear.Replace("Sept", "Sep");
        }

        public async Task<bool> CheckContractNo(int? ContractId, string Contractno)
        {
            try
            {

                if (ContractId > 0)
                {
                    var result = await _context.Projectcontracts.AsNoTracking().AnyAsync(x => x.Contractno == Contractno && x.Contractid == ContractId);
                    if (result == true)
                    {
                        return false;
                    }
                    else
                    {
                        return await _context.Projectcontracts.AsNoTracking().AnyAsync(x => x.Contractno == Contractno);
                    }

                }
                else
                {
                    return await _context.Projectcontracts.AsNoTracking().AnyAsync(x => x.Contractno == Contractno);
                }
            }
            catch(Exception ex) 
            {
                throw;
            }
        }

        public async Task<bool> CheckPoNo(int? ContractId, string PoNo)
        {
            //try
            //{
            //    IQueryable<Projectcontract> query = _context.Projectcontracts.AsNoTracking()
            //                                                .Where(x => x.Ponumber == PoNo);

            //    if (ContractId > 0)
            //    {
            //        query = query.Where(x => x.Contractid == ContractId);
            //    }

            //    return !(await query.AnyAsync());
            //}
            try
            {

                if (ContractId > 0)
                {
                    var result = await _context.Projectcontracts.AsNoTracking().AnyAsync(x => x.Ponumber == PoNo && x.Contractid == ContractId);
                    if (result == true)
                    {
                        return false;
                    }
                    else
                    {
                        return await _context.Projectcontracts.AsNoTracking().AnyAsync(x => x.Ponumber == PoNo);
                    }

                }
                else
                {
                    return await _context.Projectcontracts.AsNoTracking().AnyAsync(x => x.Ponumber == PoNo);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<object> ForeclosureBillingAction(ForeclosureInputDTO value, JwtLoginDetailDto logindetails)
        {
            try
            {
                if(value==null)
                {
                    throw new ArgumentNullException("Input value is null");
                }
                var existingContract = _context.Projectcontracts.FirstOrDefault(x => x.Contractid == value.ContractId);
                if(existingContract == null)
                {
                    throw new Exception("No contract found at the given ContractId");
                }
                existingContract.Contractenddate = value.ForeClosureDate;
                existingContract.Isforeclosure=true;
                existingContract.Statusid=await _context.Statuses.Where(x=>x.Statusname.ToLower().Trim()== "completed").Select(x=>x.Statusid).FirstOrDefaultAsync();
                existingContract.Lastupdateby = await _context.Rmsemployees.Where(x => x.Userid == logindetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync();
                existingContract.Lastupdatedate=DateTime.Now;
                _context.Projectcontracts.Update(existingContract); 
                await _context.SaveChangesAsync();
                return new Response { responseCode=200,responseMessage="Contract has been updated!"};
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<object> CheckContractForeClosure(ContractCheckDto value)
        {
            try
            {
               var flag = false;

                var existingContract = await _context.Projectcontracts.AsNoTracking()
                                                                      .FirstOrDefaultAsync(x => x.Contractid == value.Contractid);
                if (existingContract.Contractenddate < value.Inputdate)
                {
                    return new
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Contract end date can not be less than the input date",
                        isBiling = flag
                    };
                }
                var monthyearSeries = GetMonthYearRange(value.Inputdate.Value, existingContract.Contractenddate.Value);
                if (existingContract == null)
                {
                    return new 
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Contract not found",
                        isBiling = flag
                    };
                }

                var existingContractEmployeeIds =await _context.Contractemployees.AsNoTracking()
                                                                            .Where(x=>x.Contractid==existingContract.Contractid)
                                                                            .ToListAsync();
                if (existingContractEmployeeIds == null)
                {
                    return new 
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Billing not found",
                        isBiling = flag
                    };
                }

                var existingActualBilling = await _context.Contractbillings.AsNoTracking()
                                                        .Where(x => monthyearSeries.Contains(x.Billingmonthyear) &&
                                                               existingContractEmployeeIds.Contains(x.Contractemployee) && x.Costing > 0).AnyAsync();

                var existingProvisionBilling = await _context.Contractbillingprovesions.AsNoTracking()
                                                        .Where(x => monthyearSeries.Contains(x.Billingmonthyear) &&
                                                               existingContractEmployeeIds.Contains(x.Contractemployee) && x.Costing > 0).AnyAsync();

                if (existingActualBilling || existingProvisionBilling)
                {
                    flag = true;
                }

                return new
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Billing exists",
                    isBiling=flag
                };
            }
            catch (Exception ex)
            {
                // Handle the exception as per your requirement.
                throw;
            }
        }

        private List<string> GetMonthYearRange(DateTime startDate, DateTime endDate)
        {
            List<string> monthYearList = new List<string>();

            DateTime current = new DateTime(startDate.Year, startDate.Month+1, 1);

            while (current <= endDate)
            {
                monthYearList.Add(current.ToString("MMM-yy", CultureInfo.InvariantCulture));
                current = current.AddMonths(1);
            }

            return monthYearList;
        }

        //nick
        public async Task<List<ContractendingsoonDto>> getcontractsendingsoon()
        {
            var filteredResult = new List<ContractendingsoonDto>();
            try
            {
                var response = await _context.ContractendingsoonDto
                                                    .FromSqlRaw("SELECT * FROM get_contracts_ending_soon()")
                                                    .ToListAsync();
                if (response != null)
                {
                    filteredResult = response.Select(r => new ContractendingsoonDto
                    {
                        ContractId = r.ContractId,
                        ContractNo = r.ContractNo,
                        Companyname = r.Companyname,
                        PoNumber = r.PoNumber,
                        ContractEndDate = r.ContractEndDate == null ? null : r.ContractEndDate,
                        DeliveryAnchorEmail = r.DeliveryAnchorEmail,
                        AccountManagerEmail = r.AccountManagerEmail,
                        PracticeHeadEmail   = r.PracticeHeadEmail,
                        Duration = r.Duration,
                       
                    }).ToList();
                }


                return filteredResult;

            }
            catch(Exception ex) 
            {
                throw;
            }
        }
        public async Task<Response> projectestimationstatusupdate(ContractRequestDto Value, JwtLoginDetailDto LoginDetails)
        {
            try
            {
                var LastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
                                            .Where(x => x.Userid == LoginDetails.TmcId)
                                            .Select(x => x.Employeeid)
                                            .FirstOrDefaultAsync();

                var existingContract = await _context.Projectcontracts.AsNoTracking()
                                                   .FirstOrDefaultAsync(x => x.Contractid == Value.Contractid);
                if (existingContract == null)
                {
                    return new Response
                    {
                        responseCode = StatusCodes.Status400BadRequest,
                        responseMessage = $"Contract does not exists at given ID  : {Value.Contractid}"
                    };

                }
                
                existingContract.Isprojectestimationdone = Value.Isprojectestimationdone;
                existingContract.Lastupdateby = LastUpdatedBy;
                existingContract.Lastupdatedate = DateTime.Now;

                _context.Projectcontracts.Update(existingContract);
                await _context.SaveChangesAsync();
                return new Response() { responseCode = 200, responseMessage = "Updated Successfully" };

            }
            catch (Exception e)
            {
                throw;
            }
        }


    }
}
