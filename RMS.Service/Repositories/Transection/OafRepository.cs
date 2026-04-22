using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.Collections;
using System.Data;
using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.Json;
using System.Xml.Linq;

namespace RMS.Service.Repositories.Master
{
    public class OafRepository: GenericRepository<Oaf>, IOafRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OafRepository(RmsDevContext context,IHostingEnvironment env, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }       

        Task<Oaf> IOafRepository.GetByName(string name)
        {
            throw new NotImplementedException();
        }
        //public async Task<IEnumerable> GetList()
        //{
        //    try
        //    {
        //        var query = _context.Oafs.AsNoTracking()
        //                            .Include(x => x.Status)
        //                            .Join(_context.Rmsemployees, oaf => oaf.Createdby, employee => employee.Employeeid, (oaf, employee) => new
        //                            {
        //                                Oafid = oaf.Oafid,
        //                                OafNo=oaf.Oafno??"",
        //                                PoNumber = oaf.Ponumber,
        //                                CreatedOn = oaf.Createddate,
        //                                CreatedBy = employee.Employeename,
        //                                StatusId= oaf.Statusid, 
        //                                Status = oaf.Status.Statusname
        //                            });

        //        return await query.ToListAsync(); 
        //    }
        //    catch (Exception ex)
        //    {

        //        throw; 
        //    }
        //}
        public async Task<IEnumerable>  GetList(JwtLoginDetailDto LoginDetails)
        {

            var query = new List<OafListDto>();
            try
            {
                var employee = await _context.Rmsemployees
                         .Include(e => e.Employeeroles)
                         .ThenInclude(er => er.Role)
                         .FirstOrDefaultAsync(e => e.Userid == LoginDetails.TmcId);

                if (employee == null)
                {
                    throw new Exception("Employee not found.");
                }
                var employeeRole = employee.Employeeroles.FirstOrDefault();
                if (employeeRole == null)
                {
                    throw new Exception("You don't have any authorized role to access this dataset.");
                }
                // Fetch practice head data
                var practiceHead = await _context.Practiceheads
                                                 .Include(ph => ph.Practice)
                                                 .Where(ph => ph.Employeeid == employee.Employeeid)
                                                 .ToListAsync();
                if (practiceHead == null && employeeRole.Roleid==9)
                {
                    return new List<object>();
                }
                var practiceNames = practiceHead.Select(x => x.Practice.Practicename.ToLower().Trim()).ToList();


                //var query = _context.Oafs.AsNoTracking()
                //                         .Include(x=>x.OafExtendedHistories)
                //                         .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
                //                         .Include(x => x.Status)                                    
                //                    .Join(_context.Rmsemployees, oaf => oaf.Createdby, employee => employee.Employeeid, (oaf, employee) => new
                //                    {
                //                        Oafid = oaf.Oafid,
                //                        OafNo = oaf.Oafno ?? "",
                //                        PoNumber = oaf.Ponumber,
                //                        CreatedOn = oaf.Createddate,
                //                        CreatedBy = employee.Employeename,
                //                        CreatedById=oaf.Createdby,
                //                        StatusId = oaf.Statusid,
                //                        Status = oaf.Status.Statusname,
                //                        Practice=oaf.Subpractice.Practice.Practicename,
                //                        isExteded=oaf.Isextended==null?false:oaf.Isextended,

                //                    });


                 query = await _context.OafListDtos
                                                  .FromSqlRaw("SELECT * FROM get_oaf_and_history_List()")
                                                  .ToListAsync();


                if (employeeRole.Roleid == 6)
                {

                    query = query.Where(x=>x.StatusId==2).ToList();
                }
                else if(employeeRole.Roleid == 9)
                {
                    query = query.Where(x => practiceNames.Contains(x.PracticeName.ToLower().Trim())).ToList();
                   
                }
                else
                {
                    //query = query.Where(x => x.CreatedById == employee.Employeeid);
                    query = query.ToList(); 
                }


                return query.ToList();
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while fetching the list.", ex);
            }
        }
        public async Task<OafDto> GetOafById(int id)
        {
            try
            {
                var result=new OafDto();
                var maxRevisionNumber = await _context.OafExtendedHistories.AsNoTracking().Where(x => x.Oafid == id).MaxAsync(x => x.Revisionnumber);
                var extendedData = await _context.OafExtendedHistories.AsNoTracking()
                                                      .Include(x => x.Costsheet)
                                                      .Include(x => x.Status)
                                                      .FirstOrDefaultAsync(x => x.Oafid == id && x.Revisionnumber==maxRevisionNumber);

                var firstResult = await _context.Oafs.AsNoTracking()
                                        .Where(x => x.Oafid == id)
                                        .Include(x => x.Projectcontracts).ThenInclude(x => x.Status)
                                        .Include(x => x.Projectcontracts).ThenInclude(x => x.Contractemployees).ThenInclude(x => x.Employee)
                                        .Include(x => x.Accountmanager)
                                        .Include(x => x.Projecttype)
                                        .Include(x => x.Deliveryanchor)
                                        .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
                                        .Include(x => x.Customer).ThenInclude(x => x.Projects)
                                        .Include(x => x.Oaflines)
                                        .Include(x => x.Oafchecklists).ThenInclude(x => x.Status)
                                        .Include(x => x.Costsheet).ThenInclude(x => x.Costsheetdetails).ThenInclude(x => x.Skill).ThenInclude(x => x.Skillcostings)
                                        .Include(x => x.Milestonedetails)
                                        .Include(x => x.Status)
                                        .Join(_context.Rmsemployees, oaf => oaf.Createdby, employee => employee.Employeeid, (oaf, employee) => new { oaf, employee }).FirstOrDefaultAsync();               

                if (firstResult != null)
                {

                    result = new OafDto
                    {
                        Oafid = firstResult.oaf.Oafid,
                        HistoryId = firstResult.oaf.Isextended == null ?
                                                                               null : (extendedData == null ? null : extendedData.Historyid),
                        //Ponumber = oaf.Ponumber,
                        Ponumber = firstResult.oaf.Isextended == null ?
                                                           firstResult.oaf.Ponumber :
                                                           (firstResult.oaf.Isextended == true ? extendedData.Ponumber ?? firstResult.oaf.Ponumber : firstResult.oaf.Ponumber),
                        CommittedClientBillingDate = firstResult.oaf.CommittedClientBillingDate,
                        //Povalue = oaf.Povalue,
                        Povalue = firstResult.oaf.Isextended == null ?
                                                           firstResult.oaf.Povalue : (extendedData != null ? extendedData.Povalue ?? firstResult.oaf.Povalue : firstResult.oaf.Povalue),
                        Orderdescription = firstResult.oaf.Orderdescription,
                        Potermscondition = firstResult.oaf.Potermscondition,
                        Customerid = firstResult.oaf.Customerid,
                        Isextended = firstResult.oaf.Isextended == null ? false : firstResult.oaf.Isextended,
                        Customer = new CustomerInfo
                        {
                            Companyname = firstResult.oaf.Customer.Companyname,
                            Companyemail = firstResult.oaf.Customer.Companyemail,
                            Companylogourl = string.IsNullOrEmpty(firstResult.oaf.Customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}" +
                                                                                                                                                  $"://{_httpContextAccessor.HttpContext.Request.Host}{firstResult.oaf.Customer.Companylogourl}",
                            Address = string.IsNullOrEmpty(firstResult.oaf.Customer.Address1) ? firstResult.oaf.Customer.Address2 : firstResult.oaf.Customer.Address1,
                            PhoneNumber = string.IsNullOrEmpty(firstResult.oaf.Customer.Phone1) ? firstResult.oaf.Customer.Phone2 : firstResult.oaf.Customer.Phone1
                        },
                        Projectname = firstResult.oaf.Projectname,
                        Projectmodel = firstResult.oaf.Projectmodel,
                        Projecttypeid = firstResult.oaf.Projecttypeid,
                        Projecttype = firstResult.oaf.Projecttype.Projecttypename,
                        Subpracticeid = firstResult.oaf.Subpracticeid,
                        Subpractice = firstResult.oaf.Subpractice.Subpracticename,
                        Practiceid = firstResult.oaf.Subpractice.Practice.Practiceid,
                        Practice = firstResult.oaf.Subpractice.Practice.Practicename,
                        Projectdescription = firstResult.oaf.Projectdescription,
                        //Contractno = oaf.Contractno,
                        Contractno = firstResult.oaf.Isextended == null ?
                                                            firstResult.oaf.Contractno : (extendedData != null ? extendedData.Contractno ?? firstResult.oaf.Contractno : firstResult.oaf.Contractno),
                        //Contractstartdate = oaf.Contractstartdate,
                        Contractstartdate = firstResult.oaf.Isextended == null ?
                                                                    firstResult.oaf.Contractstartdate : (extendedData != null ? extendedData.Contractstartdate ?? firstResult.oaf.Contractstartdate : firstResult.oaf.Contractstartdate),
                        //Contractenddate = oaf.Contractenddate,
                        Contractenddate = firstResult.oaf.Isextended == null ?
                                                                  firstResult.oaf.Contractenddate : (extendedData != null ? extendedData.Contractenddate ?? firstResult.oaf.Contractenddate : firstResult.oaf.Contractenddate),
                        //Clientcoordinator = oaf.Clientcoordinator,
                        Clientcoordinator = firstResult.oaf.Isextended == null ?
                                                                     firstResult.oaf.Clientcoordinator : (extendedData != null ? extendedData.Clientcoordinator ?? firstResult.oaf.Clientcoordinator : firstResult.oaf.Clientcoordinator),
                        Milestones = firstResult.oaf.Milestones,
                        //Costsheetid = oaf.Costsheetid,
                        //CostsheetName=oaf.Costsheet.Costsheetname,
                        Costsheetid = firstResult.oaf.Isextended == null ? firstResult.oaf.Costsheetid : (extendedData != null ? extendedData.Costsheetid ?? firstResult.oaf.Costsheetid : firstResult.oaf.Costsheetid),
                        CostsheetName = firstResult.oaf.Isextended == null ? firstResult.oaf.Costsheet.Costsheetname : (extendedData != null ? extendedData.Costsheet.Costsheetname ?? firstResult.oaf.Costsheet.Costsheetname : firstResult.oaf.Costsheet.Costsheetname),
                        Emailattachment = string.IsNullOrEmpty(firstResult.oaf.Emailattachment) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{firstResult.oaf.Emailattachment}",
                        Proposalattachment = string.IsNullOrEmpty(firstResult.oaf.Proposalattachment) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{firstResult.oaf.Proposalattachment}",
                        Poattachment = string.IsNullOrEmpty(firstResult.oaf.Poattachment) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{firstResult.oaf.Poattachment}",
                        Costattachment = string.IsNullOrEmpty(firstResult.oaf.Costattachment) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{firstResult.oaf.Costattachment}",
                        //Statusid = oaf.Statusid,
                        Statusid = firstResult.oaf.Isextended == null ? firstResult.oaf.Statusid : (extendedData != null ? extendedData.Statusid ?? firstResult.oaf.Statusid : firstResult.oaf.Statusid),
                        //Status =oaf.Status.Statusname,
                        Status = firstResult.oaf.Isextended == null ? firstResult.oaf.Status.Statusname : (extendedData != null ? extendedData.Status.Statusname ?? firstResult.oaf.Status.Statusname : firstResult.oaf.Status.Statusname),
                        Remarks = firstResult.oaf.Remarks,
                        Deliveryanchorid = firstResult.oaf.Deliveryanchorid,
                        CreatedBy = firstResult.employee.Employeename,
                        // Xvalue = oaf.Xvalue  ,
                        Xvalue = firstResult.oaf.Isextended == null ? firstResult.oaf.Xvalue : (extendedData != null ? extendedData.Xvalue ?? firstResult.oaf.Xvalue : firstResult.oaf.Xvalue),
                        CreatedDate = firstResult.oaf.Createddate,
                        Advanceamount = firstResult.oaf.Advanceamount,
                        Advancepercent = firstResult.oaf.Advancepercent,
                        ContractId = firstResult.oaf.Projectcontracts == null ? null : firstResult.oaf.Projectcontracts.Where(c => c.Contractenddate >= DateTime.Now)
                                                                                                                    .Select(c => c.Contractid).FirstOrDefault(),
                        IsResourceDeployed = firstResult.oaf.Projectcontracts.SelectMany(x => x.Contractemployees).Any(),
                        Accountmanagerid = firstResult.oaf.Accountmanagerid,
                        Accountmanager = firstResult.oaf.Accountmanager.Employeename,

                        Oaflines = firstResult.oaf.Oaflines.Select(line => new OaflineInfo
                        {

                            Oaflineid = line.Oaflineid,
                            Lineno = line.Lineno,
                            Linedescription1 = line.Linedescription1,
                            Linedescription2 = line.Linedescription2,
                            Lineamount = line.Lineamount,
                        }).ToList(),

                        Oafchecklists = firstResult.oaf.Oafchecklists.Select(check => new OafchecklistInfo
                        {
                            Oafchecklistid = check.Oafchecklistid,
                            Question = check.Question,
                            Clientresponse = check.Clientresponse,
                            Isextra = check.Isextra,
                            Statusid = check.Statusid,
                            Status = check.Status.Statusname,
                            Remarks = check.Remarks,
                        }).ToList(),
                        Milestonlist = firstResult.oaf.Milestonedetails == null ? null : firstResult.oaf.Milestonedetails.Select(mile => new MilestonInfoDto
                        {
                            Id = mile.Id,
                            Oafid = mile.Oafid,
                            Milestonedesc = mile.Milestonedesc,
                            Percentage = mile.Percentage,
                            Amount = mile.Amount,
                            Days = mile.Days,
                        }).ToList(),
                        ActiveContract = firstResult.oaf.Projectcontracts == null ? null : firstResult.oaf.Projectcontracts
                                                                                                   .Where(pc => pc.Contractenddate >= DateTime.Now && pc.Statusid != 6)
                                                                                                   .Select(pc => new ActiveContractInfo
                                                                                                   {
                                                                                                       Contractid = pc.Contractid,
                                                                                                       Contrantno = pc.Contractno,
                                                                                                       Contractstartdate = pc.Contractstartdate,
                                                                                                       Contractenddate = pc.Contractenddate,
                                                                                                       StatusId = pc.Statusid,
                                                                                                       Status = pc.Status.Statusname,
                                                                                                       Resources = pc.Contractemployees == null ? null : pc.Contractemployees
                                                                                                                                                   .Select(emp => new ResourceInfo
                                                                                                                                                   {
                                                                                                                                                       Employeeid = emp.Employeeid,
                                                                                                                                                       Tmc = emp.Employee.Userid,
                                                                                                                                                       Employeename = emp.Employee.Employeename
                                                                                                                                                   }).ToList(),

                                                                                                   }).FirstOrDefault(),

                    };
                }
                else 
                {
                    result = null;
                }
               

                  
                return result;    

            }
            catch (Exception ex) 
            {
                throw;
            }
        }       
        public async Task<Response> UpsertOaf(int? OafId, OafDto Value, IFormFile Emailattachment, IFormFile Proposalattachment, IFormFile Poattachment, IFormFile Costattachment, JwtLoginDetailDto LoginDetails)
        {
            #region File Processing
            string basePath = Path.Combine(this._env.WebRootPath, @"OafAttachments\", LoginDetails.TmcId.ToString());
            if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
            string databasefilePath_Emailattachment = (Emailattachment == null) ? await _context.Oafs.AsNoTracking().Where(x => x.Oafid == OafId).Select(x => x.Emailattachment).FirstOrDefaultAsync() ?? ""
                                                          : ProcessAttachment(Emailattachment, basePath);

       
     
            string databasefilePath_Proposalattachment = (Proposalattachment == null) ? await _context.Oafs.AsNoTracking().Where(x => x.Oafid == OafId).Select(x => x.Proposalattachment).FirstOrDefaultAsync() ?? ""
                                                          : ProcessAttachment(Proposalattachment, basePath);


         
        
            string databasefilePath_Poattachment = (Poattachment == null) ? await _context.Oafs.AsNoTracking().Where(x => x.Oafid == OafId).Select(x => x.Poattachment).FirstOrDefaultAsync() ?? ""
                                                          : ProcessAttachment(Poattachment, basePath);


          
         
            string databasefilePath_Costattachment = (Costattachment == null) ? await _context.Oafs.AsNoTracking().Where(x => x.Oafid == OafId).Select(x => x.Costattachment).FirstOrDefaultAsync() ?? ""
                                                          : ProcessAttachment(Costattachment, basePath);



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
                return Path.Combine(@"\OafAttachments\", uniqueFileName);
            }
            #endregion

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                string salepersonname = "";
                string salespersonemail = "";
                string practiceheadname = "";
                string practiceheademail = "";
                string companyname= "";

                var practiceHead = await _context.Practiceheads
                                    .Include(x => x.Practice)
                                    .ThenInclude(p => p.Subpractices)
                                    .Where(x => x.Practice.Subpractices.Any(y => y.Subpracticeid == Value.Subpracticeid))
                                    .FirstOrDefaultAsync();
                salepersonname = _context.Rmsemployees.Where(a => a.Userid == LoginDetails.TmcId)
                       .Select(a => a.Employeename)
                       .FirstOrDefault();

                salespersonemail = _context.Rmsemployees.Where(a => a.Userid == LoginDetails.TmcId)
                    .Select(a => a.Companyemail)
                    .FirstOrDefault();
                practiceheadname = _context.Rmsemployees.Where(a => a.Employeeid == practiceHead.Employeeid).Select(x => x.Employeename).FirstOrDefault();
                practiceheademail = _context.Rmsemployees.Where(a => a.Employeeid == practiceHead.Employeeid).Select(x => x.Companyemail).FirstOrDefault();
                companyname = _context.Customers.Where(a => a.Customerid == Value.Customerid).Select(a => a.Companyname).FirstOrDefault();

                #region Create OafForm
                if (!OafId.HasValue || OafId.Value == 0)
                {
                    var createdBy = await _context.Rmsemployees.AsNoTracking()
                                               .Where(x => x.Userid == LoginDetails.TmcId)
                                               .Select(x => x.Employeeid)
                                               .FirstOrDefaultAsync();
                    var CreatedOaf = new Oaf
                    {
                        Ponumber=Value.Ponumber,
                        Povalue=Value.Povalue,
                        Orderdescription=Value.Orderdescription,
                        Potermscondition=Value.Potermscondition,
                        Customerid=Value.Customerid,
                        Projectname=Value.Projectname,
                        Projectmodel=Value.Projectmodel,
                        Projecttypeid=Value.Projecttypeid,
                        Subpracticeid=Value.Subpracticeid,
                        Projectdescription=Value.Projectdescription,
                        Contractno=Value.Contractno,
                        Contractstartdate=Value.Contractstartdate,  
                        Contractenddate=Value.Contractenddate,
                        Clientcoordinator=Value.Clientcoordinator,
                        Milestones=Value.Milestones,
                        Costsheetid=Value.Costsheetid,
                        Emailattachment= databasefilePath_Emailattachment,
                        Proposalattachment=databasefilePath_Proposalattachment,
                        Poattachment=databasefilePath_Poattachment,
                        Statusid=2,
                        //Remarks=Value.Remarks,
                        Xvalue=Value.Xvalue,
                        Costattachment=databasefilePath_Costattachment,
                        Createdby = createdBy,
                        Createddate = DateTime.Now,
                        Accountmanagerid = (Value.Accountmanagerid == null || Value.Accountmanagerid <= 0)
                                               ? throw new ArgumentNullException($"the value of accountmanager can not be null or zero {Value.Accountmanagerid.GetType()}")
                                               : Value.Accountmanagerid,
                        Advanceamount=Value.Advanceamount,
                        Advancepercent=Value.Advancepercent,
                        CommittedClientBillingDate=Value.CommittedClientBillingDate?? null,

                    };
                    _context.Oafs.Add(CreatedOaf);
                    await _context.SaveChangesAsync();
                    if (Value.Oaflines != null)
                    {
                        foreach (var lineitem in Value.Oaflines)
                        {
                            var CreatedOafLines = new Oafline
                            {
                                Oafid=CreatedOaf.Oafid,
                                Lineno=lineitem.Lineno,
                                Linedescription1=lineitem.Linedescription1,
                                Linedescription2=lineitem.Linedescription2,
                                Lineamount= lineitem.Lineamount 
                            };
                            _context.Oaflines.AddRange(CreatedOafLines);
                        }
                    }
                    if (Value.Oafchecklists != null)
                    {
                        foreach (var Queitem in Value.Oafchecklists)
                        {
                            var Question = new Oafchecklist
                            {
                                Oafid=CreatedOaf.Oafid,
                                Question=Queitem.Question,
                                Clientresponse=Queitem.Clientresponse,
                                Isextra = Queitem.Isextra,
                                Statusid=2,
                            };
                            _context.Oafchecklists.AddRange(Question);
                        }
                    }
                    if (Value.Milestonlist != null)
                    {
                        foreach (var MilestoneItem in Value.Milestonlist)
                        {
                            var CreatedMilestone = new Milestonedetail
                            {
                                Oafid = CreatedOaf.Oafid,
                                Milestonedesc=MilestoneItem.Milestonedesc,
                                Percentage= MilestoneItem.Percentage,
                                Amount= MilestoneItem.Amount,
                                Days= MilestoneItem.Days,   
                                Createddate=DateTime.Now,
                                Createdby=createdBy,

                            };
                            _context.Milestonedetails.AddRange(CreatedMilestone);
                        }
                    }
                   
                    #endregion
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new Response { responseCode = StatusCodes.Status200OK, responseMessage = "OAF created successfully.",
                        data = new
                        {
                            practiceheademail = practiceheademail,
                            salespersonemail = salespersonemail,
                            salespersonname = salepersonname,
                            practiceheadname = practiceheadname,
                            projectname = Value.Projectname,
                            customername = companyname,


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

                    var existingOaf = await _context.Oafs.AsNoTracking()
                                                       .Include(x => x.Oaflines)
                                                       .Include(x => x.Oafchecklists)
                                                       .FirstOrDefaultAsync(x => x.Oafid == OafId);
                    if (existingOaf == null)
                    {
                        return new Response
                        {
                            responseCode = StatusCodes.Status400BadRequest,
                            responseMessage = $"OAF does not exists at given ID  : {OafId}"
                        };

                    } 
                    if (OafId != Value.Oafid)
                    {
                        return new Response
                        {
                            responseCode = StatusCodes.Status400BadRequest,
                            responseMessage = $" Both the IDs inside inside json data and querystring are not same "
                        };
                    }
                   

                    // update OAF
                    existingOaf.Ponumber = Value.Ponumber;
                    existingOaf.Povalue = Value.Povalue;
                    existingOaf.Orderdescription = Value.Orderdescription;
                    existingOaf.Potermscondition = Value.Potermscondition;
                    existingOaf.Customerid = Value.Customerid;
                    existingOaf.Projectname = Value.Projectname;
                    existingOaf.Projectmodel = Value.Projectmodel;
                    existingOaf.Projecttypeid = Value.Projecttypeid;
                    existingOaf.Subpracticeid = Value.Subpracticeid;
                    existingOaf.Projectdescription = Value.Projectdescription;
                    existingOaf.Contractno = Value.Contractno;
                    existingOaf.Contractstartdate = Value.Contractstartdate;
                    existingOaf.Contractenddate = Value.Contractenddate;
                    existingOaf.Clientcoordinator = Value.Clientcoordinator;
                    existingOaf.Milestones = Value.Milestones;
                    existingOaf.Costsheetid = Value.Costsheetid;
                    existingOaf.Emailattachment = databasefilePath_Emailattachment;
                    existingOaf.Proposalattachment = databasefilePath_Proposalattachment;
                    existingOaf.Poattachment = databasefilePath_Poattachment;
                   // existingOaf.Statusid = Value.Statusid;
                    existingOaf.Remarks = Value.Remarks;
                    existingOaf.Deliveryanchorid = Value.Deliveryanchorid;
                    existingOaf.Xvalue = Value.Xvalue;
                    existingOaf.Costattachment = databasefilePath_Costattachment;
                    existingOaf.Lastupdateby = LastUpdatedBy;
                    existingOaf.Lastupdatedate = DateTime.Now;
                    existingOaf.Accountmanagerid = (Value.Accountmanagerid == null || Value.Accountmanagerid <= 0)
                                               ? throw new ArgumentNullException($"the value of accountmanager can not be null or zero {Value.Accountmanagerid.GetType()}")
                                               : Value.Accountmanagerid;
                    existingOaf.Advanceamount = Value.Advanceamount ?? existingOaf.Advanceamount;
                    existingOaf.Advancepercent = Value.Advancepercent ?? existingOaf.Advancepercent;
                    existingOaf.CommittedClientBillingDate=Value.CommittedClientBillingDate??existingOaf.CommittedClientBillingDate;    
                    _context.Oafs.Update( existingOaf ); 
                    //deletting records from line 
                    _context.Oaflines.RemoveRange(existingOaf.Oaflines);
                    // deleting question responses 
                    _context.Oafchecklists.RemoveRange(existingOaf.Oafchecklists);
                    _context.Milestonedetails.RemoveRange(existingOaf.Milestonedetails);    
                    await _context.SaveChangesAsync();

                    if (Value.Oaflines != null)
                    {
                        foreach (var lineitem in Value.Oaflines)
                        {
                            var CreatedOafLines = new Oafline
                            {
                                Oafid = existingOaf.Oafid,
                                Lineno = lineitem.Lineno,
                                Linedescription1 = lineitem.Linedescription1,
                                Linedescription2 = lineitem.Linedescription2,
                                Lineamount = lineitem.Lineamount
                            };
                            _context.Oaflines.AddRange(CreatedOafLines);
                        }
                    }
                    if (Value.Oafchecklists != null)
                    {
                        foreach (var Queitem in Value.Oafchecklists)
                        {
                            var Question = new Oafchecklist
                            {
                                Oafid = existingOaf.Oafid,
                                Question = Queitem.Question,
                                Clientresponse = Queitem.Clientresponse,
                                Isextra = Queitem.Isextra,
                                Statusid = 4,
                            };
                            _context.Oafchecklists.AddRange(Question);
                        }
                    }
                    if (Value.Milestonlist != null)
                    {
                        foreach (var MilestoneItem in Value.Milestonlist)
                        {
                            var CreatedMilestone = new Milestonedetail
                            {
                                Oafid = existingOaf.Oafid,
                                Milestonedesc = MilestoneItem.Milestonedesc,
                                Percentage = MilestoneItem.Percentage,
                                Amount = MilestoneItem.Amount,
                                Days = MilestoneItem.Days,  
                                Createddate = DateTime.Now,
                                Createdby = LastUpdatedBy,

                            };
                            _context.Milestonedetails.AddRange(CreatedMilestone);
                        }
                    }
                    #endregion
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new Response { responseCode = StatusCodes.Status200OK, responseMessage = "OAF Updated successfully.",
                        data = new
                        {
                            practiceheademail = practiceheademail,
                            salespersonemail = salespersonemail,
                            salespersonname = salepersonname,
                            practiceheadname = practiceheadname,
                            projectname = Value.Projectname,
                            customername = companyname,


                        }
                    };

                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new Response
                {
                    responseCode = StatusCodes.Status500InternalServerError,
                    responseMessage = $"An error occurred while upserting the OAF: {ex.Message}"
                };
            }
        }
        public async Task<Response> ApproverAction(OafDto Value, JwtLoginDetailDto LoginDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var createdby = await _context.Rmsemployees.AsNoTracking()
                                                .Where(x => x.Userid == LoginDetails.TmcId)
                                                .Select(x => x.Employeeid)
                                                .FirstOrDefaultAsync();

                //var oafQuery = _context.Oafs.AsNoTracking()
                //                        .Where(x => x.Oafid == Value.Oafid)
                //                        .Include(x => x.Projecttype)
                //                        .Include(x => x.Deliveryanchor)
                //                        .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
                //                        .Include(x => x.Customer).ThenInclude(x => x.Projects)
                //                        .Include(x => x.Oaflines)
                //                        .Include(x => x.Oafchecklists).ThenInclude(x => x.Status)
                //                        .Include(x => x.Costsheet).ThenInclude(x => x.Costsheetdetails).ThenInclude(x => x.Skill).ThenInclude(x => x.Skillcostings)
                //                        .Include(x => x.Status)
                //                        .Join(_context.Rmsemployees, oaf => oaf.Createdby, employee => employee.Employeeid, (oaf, employee) => new { oaf, employee });
                                        
                //var existingOaf = await oafQuery.FirstOrDefaultAsync();

               var existingOaf = await _context.Oafs.AsNoTracking()                                                
                                                .Where(x => x.Oafid == Value.Oafid)
                                                .Include(x => x.Projecttype)
                                                .Include(x => x.Deliveryanchor)
                                                .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
                                                .Include(x => x.Customer).ThenInclude(x => x.Projects)
                                                .Include(x => x.Oaflines)
                                                .Include(x => x.Oafchecklists)
                                                .Include(x => x.Costsheet)
                                              //  .Include(x => x.Status)
                                                .Join(_context.Rmsemployees.AsNoTracking(), oaf => oaf.Createdby, employee => employee.Employeeid, (oaf, employee) => new { oaf, employee })
                                                .FirstOrDefaultAsync();



                if (Value.Statusid == 5 || Value.Statusid == 4)
                {
                    if (existingOaf != null)
                    {
                        //here status is not updating need to check in future 
                        existingOaf.oaf.Statusid = Value.Statusid;
                        existingOaf.oaf.Deliveryanchorid = Value.Deliveryanchorid;
                        existingOaf.oaf.Remarks = Value.Remarks;
                        existingOaf.oaf.Lastupdateby = createdby;
                        existingOaf.oaf.Lastupdatedate = DateTime.Now;
                        _context.Oafs.Update(existingOaf.oaf);                     

                        if (Value.Oafchecklists != null)
                        {
                            foreach (var item in Value.Oafchecklists)
                            {
                                var existingOafChecklist = await _context.Oafchecklists.FirstOrDefaultAsync(x => x.Oafchecklistid == item.Oafchecklistid);
                                if (existingOafChecklist != null)
                                {
                                    existingOafChecklist.Statusid = item.Statusid;
                                    existingOafChecklist.Remarks = item.Remarks;
                                    _context.Oafchecklists.Update(existingOafChecklist);
                                }
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                    if (Value.Statusid == 5)
                    {
                        var ExistingDetails =await _context.Projects
                                                     .Include(x=>x.Projectcontracts)
                                                     .ThenInclude(x=>x.Oaf).Where(x=>x.Projectcontracts.Any(x=>x.Oafid== existingOaf.oaf.Oafid)).FirstOrDefaultAsync();
                        if (ExistingDetails != null)
                        {
                            throw new Exception("A project and contract is alredy created for this oaf");
                        }


                        var newProject = new Project
                        {
                            Projectname = existingOaf.oaf.Projectname,
                            Projectdescription = existingOaf.oaf.Projectdescription,
                            Customerid = existingOaf.oaf.Customerid,
                            Projecttypeid = existingOaf.oaf.Projecttypeid,
                            Subpracticeid = existingOaf.oaf.Subpracticeid,
                            Statusid = existingOaf.oaf.Statusid,
                            Accountmanagerid=existingOaf.oaf.Accountmanagerid,
                            CommittedClientBillingDate=existingOaf.oaf.CommittedClientBillingDate,
                            Projectno=await GenerateNewProjectNumber()
                            
                            
                        };

                        _context.Projects.Add(newProject);
                        await _context.SaveChangesAsync();

                        string contractNo = !string.IsNullOrEmpty(existingOaf.oaf.Contractno)
                                              ? existingOaf.oaf.Contractno
                                              : await GenerateNewContractNumber();

                        var newContract = new Projectcontract
                        {
                            Projectid = newProject.Projectid,
                            Contractstartdate = existingOaf.oaf.Contractstartdate,
                            Contractenddate = existingOaf.oaf.Contractenddate,
                            Povalue = decimal.Parse(existingOaf.oaf.Povalue),
                            Amount = decimal.Parse(existingOaf.oaf.Povalue),
                            Ponumber = existingOaf.oaf.Ponumber,
                            Contactpersonname = existingOaf.oaf.Clientcoordinator,
                            Statusid = existingOaf.oaf.Statusid,
                            Costsheetid = existingOaf.oaf.Costsheetid,
                            Remarks = existingOaf.oaf.Remarks,
                            Deliveryanchorid = existingOaf.oaf.Deliveryanchorid,
                            Oafid = existingOaf.oaf.Oafid,
                            Createdby = createdby,
                            Createddate = DateTime.Now,
                            Contractno = contractNo
                        };

                        _context.Projectcontracts.Add(newContract);
                        await _context.SaveChangesAsync();

                        var newContractLines = existingOaf.oaf.Oaflines.Select(l => new Contractline
                        {
                            Contractid = newContract.Contractid,
                            Lineno = l.Lineno,
                            Linedescription1 = l.Linedescription1,
                            Linedescription2 = l.Linedescription2,
                            Lineamount = l.Lineamount,
                        }).ToList();

                        await _context.Contractlines.AddRangeAsync(newContractLines);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return new Response { responseCode = 200, responseMessage = "Action Taken Successfully!" };
                }
                else
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"StatusId is wrong: {Value.Statusid}");
                }
            }
            catch (Exception ex)
            {
                
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async  Task<Response> OAFwiseResourceAllotment(OAFwiseResourceAllotmentDto Value, JwtLoginDetailDto Userdetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var createdby = await _context.Rmsemployees.AsNoTracking()
                                                .Where(x => x.Userid == Userdetails.TmcId)
                                                .Select(x => x.Employeeid)
                                                .FirstOrDefaultAsync();
                if (Value.resource_details == null)
                {
                    throw new Exception("Resource details can not be null");
                }
                

                foreach (var item in Value.resource_details)
                {
                    var newContractEmployee = new Contractemployee
                    {
                        Contractid = Value.contractid,
                        Employeeid = item.employeeid,
                        Createdby=createdby,
                        Createddate=DateTime.Now,
                        Costsheetdetailid=item.costsheetdetailid,
                        Empxvalue=item.xvalue,
                    };
                    await _context.Contractemployees.AddAsync(newContractEmployee);
                   await  _context.SaveChangesAsync();
                    if(item.deployement_details != null && item.deployement_details.Count>0)
                    {
                        foreach (var itemDeployment in item.deployement_details)
                        {
                            var newDeployment = new Contractemployeedeploymentdate
                            {
                                Contractemployeeid = newContractEmployee.Contractemployeeid,
                                Startdate = itemDeployment.startdate,
                                Enddate = itemDeployment.enddate,
                                Categorysubstatusid=itemDeployment.categorysubstatusid, 
                                Createdby = createdby,
                                Createddate = DateTime.Now,
                            };
                            await _context.Contractemployeedeploymentdates.AddRangeAsync(newDeployment); 
                        }                        
                    }
                    if (item.billing_Details != null && item.billing_Details.Count>0)
                    {
                        foreach (var itemBillingDetails in item.billing_Details)
                        {
                            var newIntialBilling = new Initialsplitcontractbilling
                            {
                                Contractemployeeid = newContractEmployee.Contractemployeeid,
                                Billingmonthyear = itemBillingDetails.Monthyear,
                                Costing = itemBillingDetails.costing,
                                Createdby = createdby,
                                Createddate = DateTime.Now,
                            };
                            await _context.Initialsplitcontractbillings.AddRangeAsync(newIntialBilling);
                        }
                        foreach (var itemBillingDetails in item.billing_Details)
                        {
                            var newContractBilling = new Contractbilling
                            {
                                Contractemployeeid = newContractEmployee.Contractemployeeid,
                                Billingmonthyear = itemBillingDetails.Monthyear,
                                Costing = itemBillingDetails.costing,
                                Createdby = createdby,
                                Createddate = DateTime.Now,
                            };
                            await _context.Contractbillings.AddRangeAsync(newContractBilling);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                return new Response { responseCode = 200, responseMessage = "Save Data Successfully!" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<IEnumerable> GetOafListByDAId(int DeliveryAnchorId)
        {
            try
            {
                var query = _context.Oafs.AsNoTracking()
                                    .Include(x => x.Status)
                                    .Join(_context.Rmsemployees, oaf => oaf.Createdby, employee => employee.Employeeid, (oaf, employee) => new
                                    {
                                        Oafid = oaf.Oafid,
                                        OafNo=oaf.Oafno??"",
                                        PoNumber = oaf.Ponumber,
                                        createdDate = oaf.Createddate,
                                        CreatedByName = employee.Employeename,
                                        StatusId= oaf.Statusid, 
                                        Status = oaf.Status.Statusname,
                                        Deliveryanchorid = oaf.Deliveryanchorid
                                    });
              
                return await query.Where(x => x.Deliveryanchorid == DeliveryAnchorId).ToListAsync();
            }
            catch (Exception ex)
            {
                
                throw; 
            }
        }
        private async Task<string> GenerateNewProjectNumber()
        {
            var currentYear = DateTime.Now.Year;
            var prefix = $"PRP-{currentYear}-";
            var lastProjectNumber = await _context.Projects
                                    .Where(p => p.Projectno.StartsWith(prefix))
                                    .OrderByDescending(x => x.Projectno)
                                    .Select(x => x.Projectno)
                                    .FirstOrDefaultAsync();

            if (lastProjectNumber == null)
            {
                return $"{prefix}0001";
            }
            else
            {
                var lastNumber = int.Parse(lastProjectNumber.Split('-').Last());
                var newNumber = (lastNumber + 1).ToString().PadLeft(4, '0');
                return $"{prefix}{newNumber}";
            }
        }
        //private async Task<string> GenrateNewContractNumber()
        //{
        //    var currentYear = DateTime.Now.Year;
        //    var prefix = $"PR-{currentYear}-";

        //    var lastContract = await _context.Projectcontracts.AsNoTracking()
        //        .Where(c => c.Contractno.StartsWith(prefix))
        //        .OrderByDescending(c => c.Contractno)
        //        .Select(c => c.Contractno)  
        //        .FirstOrDefaultAsync();

        //    int nextNumber = 1;

        //    if (lastContract == null )
        //    {
        //        return $"{prefix}0001";
        //    }
        //    else
        //    {
        //        var lastNumber = int.Parse(lastContract.Split('-').Last());
        //        var newNumber = (lastNumber + 1).ToString().PadLeft(4, '0');
        //        return $"{prefix}{newNumber}";
        //    }

        //}
        private async Task<string> GenerateNewContractNumber()
        {
            var currentYear = DateTime.Now.Year;
            var prefix = $"PR-{currentYear}-";

            var lastContract = await _context.Projectcontracts.AsNoTracking()
                .Where(c => c.Contractno != null && c.Contractno.StartsWith(prefix))
                .OrderByDescending(c => c.Contractno)
                .Select(c => c.Contractno)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastContract != null)
            {
                if (int.TryParse(lastContract.Split('-').Last(), out var lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{prefix}{nextNumber.ToString().PadLeft(4, '0')}";
        }

        public async  Task<Response> ExtendOaf(ExtendOafDto Value ,JwtLoginDetailDto LoginDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                #region Create OafForm
                if (Value.HistoryId==null || Value.HistoryId == 0)
                {
                    var createdBy = await _context.Rmsemployees.AsNoTracking()
                                               .Where(x => x.Userid == LoginDetails.TmcId)
                                               .Select(x => x.Employeeid)
                                               .FirstOrDefaultAsync();
                    var existingOaf=await _context.Oafs.Where(x=>x.Oafid==Value.Oafid).FirstOrDefaultAsync();
                    if (existingOaf == null)
                    {
                        throw new Exception($"There is no OAF with id {Value.Oafid}");
                    }
                    var revision_number = (await _context.OafExtendedHistories
                                                         .Where(x => x.Oafid == Value.Oafid)
                                                         .MaxAsync(x => x.Revisionnumber) ?? 0) + 1;
                    var CreatedOaf_History = new OafExtendedHistory
                    {
                        Oafid=  Value.Oafid.Value,
                        Ponumber = Value.Ponumber,
                        Povalue = Value.Povalue,
                        Orderdescription = existingOaf.Orderdescription,
                        Potermscondition = existingOaf.Potermscondition,
                        Customerid = existingOaf.Customerid,
                        Projectname = existingOaf.Projectname,
                        Projectmodel = existingOaf.Projectmodel,
                        Projecttypeid = existingOaf.Projecttypeid,
                        Subpracticeid = existingOaf.Subpracticeid,
                        Projectdescription = existingOaf.Projectdescription,
                        Contractno = Value.Contractno,
                        Contractstartdate = Value.Contractstartdate,
                        Contractenddate = Value.Contractenddate,
                        Clientcoordinator = Value.Clientcoordinator,
                        Milestones = existingOaf.Milestones,
                        Costsheetid = Value.Costsheetid,
                        Emailattachment = existingOaf.Emailattachment,
                        Proposalattachment = existingOaf.Proposalattachment,
                        Poattachment = existingOaf.Poattachment,
                        Statusid = 2,
                        //Remarks=Value.Remarks,
                        Xvalue = Value.Xvalue,
                        Costattachment = existingOaf.Costattachment,
                        Createdby = createdBy,
                        Createddate = DateTime.Now,
                        Accountmanagerid = existingOaf.Accountmanagerid,
                        //Advanceamount = Value.Advanceamount,
                        //Advancepercent = Value.Advancepercent,
                        CommittedClientBillingDate = existingOaf.CommittedClientBillingDate ?? null,
                        Revisionnumber= revision_number
                    };
                    await _context.OafExtendedHistories.AddAsync(CreatedOaf_History);                  
                    existingOaf.Isextended = true;
                    _context.Oafs.Update(existingOaf); 
                    await _context.SaveChangesAsync();
                    #endregion
                    await transaction.CommitAsync();
                    return new Response { responseCode = StatusCodes.Status200OK, responseMessage = "OAF extended successfully." };
                }
                else
                {
                    #region [Update Oaf History]
                    var LastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
                                                .Where(x => x.Userid == LoginDetails.TmcId)
                                                .Select(x => x.Employeeid)
                                                .FirstOrDefaultAsync();

                    var existingOaf_History = await _context.OafExtendedHistories.FirstOrDefaultAsync(x => x.Oafid == Value.HistoryId);
                    if (existingOaf_History == null)
                    {
                        return new Response
                        {
                            responseCode = StatusCodes.Status400BadRequest,
                            responseMessage = $"OAF does not exists at given ID  : {Value.HistoryId}"
                        };

                    }

                    // update OAF
                    existingOaf_History.Ponumber = Value.Ponumber;
                    existingOaf_History.Povalue = Value.Povalue;                   
                    existingOaf_History.Contractno = Value.Contractno;
                    existingOaf_History.Contractstartdate = Value.Contractstartdate;
                    existingOaf_History.Contractenddate = Value.Contractenddate;
                    existingOaf_History.Clientcoordinator = Value.Clientcoordinator;
                    existingOaf_History.Costsheetid = Value.Costsheetid;                 
                    existingOaf_History.Xvalue = Value.Xvalue;                   
                    existingOaf_History.Lastupdateby = LastUpdatedBy;
                    existingOaf_History.Lastupdatedate = DateTime.Now;                   
                    _context.OafExtendedHistories.Update(existingOaf_History);                   ;
                    await _context.SaveChangesAsync();                   
                    #endregion                   
                    await transaction.CommitAsync();
                    return new Response { responseCode = StatusCodes.Status200OK, responseMessage = "OAF Updated successfully." };
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new Response
                {
                    responseCode = StatusCodes.Status500InternalServerError,
                    responseMessage = $"An error occurred while upserting the OAF: {ex.Message}"
                };
            }
        }
    }
}
