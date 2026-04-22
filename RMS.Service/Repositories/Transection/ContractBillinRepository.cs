using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RMS.Data;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;

namespace RMS.Service.Repositories.Master
{
    public partial class ContractBillinRepository : GenericRepository<Contractbilling>, IContractBillinRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContractBillinRepository(RmsDevContext context, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Response> UpsertContractBillings(List<ContractEmpBillingDto> contractBillingDtos, JwtLoginDetailDto loginDetails)
        {
            if (contractBillingDtos == null || !contractBillingDtos.Any())
            {
                return new Response
                {
                    responseCode = StatusCodes.Status400BadRequest,
                    responseMessage = "No contract billing data provided."
                };
            }
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                //var contractemployeeids = contractBillingDtos.Select(c => c.ConractEmployeeId).ToList();
                var employeeId = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == loginDetails.TmcId)
                                                          .Select(x => x.Employeeid).FirstOrDefaultAsync();
                if (employeeId == 0 || employeeId == null)
                {
                    return new Response
                    {
                        responseCode = StatusCodes.Status400BadRequest,
                        responseMessage = "You dont have permesion to take actions on Billing data"
                    };
                }

                foreach (var item in contractBillingDtos.Distinct())
                {
                    var existingContractBillings = await _context.Contractbillings.AsNoTracking()
                                                            .Where(x => x.Contractemployeeid == item.Contractemployeeid && x.Billingmonthyear == item.BillingMonthYear)
                                                            .FirstOrDefaultAsync();
                    if (existingContractBillings != null)
                    {
                        UpdateContractBillings(existingContractBillings, item, employeeId);
                    }
                    else
                    {
                        CreateContractBillings(item, employeeId);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response
                {
                    responseCode = StatusCodes.Status200OK,
                    responseMessage = "Contract Billings submitted successfully."
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void UpdateContractBillings(Contractbilling existingContractBilling, ContractEmpBillingDto contractBillingDto, int employeeId)
        {
            existingContractBilling.Costing = contractBillingDto.Costing;
            existingContractBilling.Lastupdateby = employeeId;
            existingContractBilling.Lastupdatedate = DateTime.Now;
            existingContractBilling.Estimatedbillingdate = contractBillingDto.Exptectedbillingdate.HasValue ? contractBillingDto.Exptectedbillingdate.Value : (DateTime?)null;
            _context.Contractbillings.UpdateRange(existingContractBilling);
        }
        private void CreateContractBillings(ContractEmpBillingDto contractBillingDto, int employeeId)
        {
            var contractBillingsToAdd = new Contractbilling
            {
                Contractemployeeid = contractBillingDto.Contractemployeeid,
                Billingmonthyear = contractBillingDto.BillingMonthYear,
                Costing = contractBillingDto.Costing,
                Createdby = employeeId,
                Createddate = DateTime.Now,
                Estimatedbillingdate = contractBillingDto.Exptectedbillingdate.HasValue ? contractBillingDto.Exptectedbillingdate.Value : (DateTime?)null
            };

            _context.Contractbillings.AddRange(contractBillingsToAdd);
        }
        //public async  Task<List<ContractListResponseDto>> GetList()
        public async Task<List<Contractbilling>> GetList()
        {
            try
            {
                return null;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<Contractbilling> GetContractBillingsById(int id)
        {
            try
            {
                var contractbilling = await _context.Contractbillings.AsNoTracking().FirstOrDefaultAsync(x => x.Contractemployeeid == id);
               return contractbilling;  
            }
            catch (Exception ex)
            {
                throw;
            }
        }        

        //   public async Task<List<ResponseForBillingDetailsDto>> BilligDetails(RequestforBillingDetailsDto Value)
        //   {
        //       try
        //       {
        //           var dateFilterList = DateFormatter(Value.FromDate, Value.ToDate);
        //           var selectedData = await _context.Contractbillings.AsNoTracking()
        //.Include(x => x.Contractemployee).ThenInclude(x => x.Employee)
        //.Include(x => x.Contractemployee).ThenInclude(x => x.Contract)
        //    .ThenInclude(x => x.Project).ThenInclude(x => x.Subpractice).ThenInclude(x => x.Practice)
        //.Include(x => x.Contractemployee).ThenInclude(x => x.Contract)
        //    .ThenInclude(x => x.Project).ThenInclude(x => x.Customer)
        //.Include(x => x.Contractemployee).ThenInclude(x => x.Contract)
        //    .ThenInclude(x => x.Deliveryanchor) // Include Deliveryanchor directly
        //.Where(x => dateFilterList.Contains(x.Billingmonthyear))
        //.ToListAsync();

        //           if (Value.Mode == "employeewise")
        //           {
        //               var groupedData = selectedData.Select(x => new ResponseForBillingDetailsDto
        //               {
        //                   Code = x.Contractemployee.Employee.Userid,
        //                   Name = x.Contractemployee.Employee.Employeename,
        //                   CustomerName = x.Contractemployee.Contract.Project.Customer.Companyname,
        //                   ProjectName = x.Contractemployee.Contract.Project.Projectname,
        //                   PracticeName = x.Contractemployee.Contract.Project.Subpractice.Practice.Practicename,
        //                   SubPracticeName = x.Contractemployee.Contract.Project.Subpractice.Subpracticename,
        //                   StartDate = x.Contractemployee.Contract.Contractstartdate,
        //                   EndDate = x.Contractemployee.Contract.Contractenddate,
        //                   MonthYear = x.Billingmonthyear,
        //                   Cost = x.Costing,
        //                   DeliveryAnchor = x.Contractemployee.Contract.Deliveryanchor?.Employeename // Access Employeename directly from Deliveryanchor
        //               }).ToList();

        //               return groupedData;
        //           }
        //           else if (Value.Mode == "customerwise")
        //           {
        //               var groupedData = selectedData.Select(x => new ResponseForBillingDetailsDto
        //               {
        //                   Code = x.Contractemployee.Contract.Project.Customer.Customercode,
        //                   Name = x.Contractemployee.Contract.Project.Customer.Companyname,
        //                   CustomerName = x.Contractemployee.Contract.Project.Customer.Companyname,
        //                   ProjectName = x.Contractemployee.Contract.Project.Projectname,
        //                   PracticeName = x.Contractemployee.Contract.Project.Subpractice.Practice.Practicename,
        //                   SubPracticeName = x.Contractemployee.Contract.Project.Subpractice.Subpracticename,
        //                   StartDate = x.Contractemployee.Contract.Contractstartdate,
        //                   EndDate = x.Contractemployee.Contract.Contractenddate,
        //                   MonthYear = x.Billingmonthyear,
        //                   Cost = x.Costing,
        //                   DeliveryAnchor = x.Contractemployee.Contract.Deliveryanchor?.Employeename // Access Employeename directly from Deliveryanchor
        //               }).ToList();

        //               return groupedData;

        //           }
        //           else { throw new Exception("The Mode parameter is not as expected"); }
        //       }
        //       catch (Exception ex)
        //       {
        //           throw;
        //       }
        //   }
        public async Task<List<ResponseForBillingDetailsDto>> BilligDetails(RequestforBillingDetailsDto Value)
        {
            try
            {
                var dateFilterList = DateFormatter(Value.FromDate, Value.ToDate);

                // Projected query for Contractbillings
                var billingQuery = _context.Contractbillings.AsNoTracking()
                    .Where(x => dateFilterList.Contains(x.Billingmonthyear))
                    .Select(x => new ResponseForBillingDetailsDto
                    {
                        Code = x.Contractemployee.Employee.Userid,
                        Name = x.Contractemployee.Employee.Employeename,
                        CustomerName = x.Contractemployee.Contract.Project.Customer.Companyname,
                        ProjectName = x.Contractemployee.Contract.Project.Projectname,
                        PracticeName = x.Contractemployee.Contract.Project.Subpractice.Practice.Practicename,
                        SubPracticeName = x.Contractemployee.Contract.Project.Subpractice.Subpracticename,
                        StartDate = x.Contractemployee.Contract.Contractstartdate,
                        EndDate = x.Contractemployee.Contract.Contractenddate,
                        MonthYear = x.Billingmonthyear,
                        Cost = x.Costing ?? 0,
                        ProvisionCost = 0,
                        DeliveryAnchor = x.Contractemployee.Contract.Deliveryanchor != null ? x.Contractemployee.Contract.Deliveryanchor.Employeename : null,
                        PoNumber = x.Contractemployee.Contract.Ponumber // Added PoNumber mapping
                    });

                // Projected query for Contractbillingprovesions
                var provisionQuery = _context.Contractbillingprovesions.AsNoTracking()
                    .Where(x => dateFilterList.Contains(x.Billingmonthyear) && x.Statusid == 5)
                    .Select(x => new ResponseForBillingDetailsDto
                    {
                        Code = x.Contractemployee.Employee.Userid,
                        Name = x.Contractemployee.Employee.Employeename,
                        CustomerName = x.Contractemployee.Contract.Project.Customer.Companyname,
                        ProjectName = x.Contractemployee.Contract.Project.Projectname,
                        PracticeName = x.Contractemployee.Contract.Project.Subpractice.Practice.Practicename,
                        SubPracticeName = x.Contractemployee.Contract.Project.Subpractice.Subpracticename,
                        StartDate = x.Contractemployee.Contract.Contractstartdate,
                        EndDate = x.Contractemployee.Contract.Contractenddate,
                        MonthYear = x.Billingmonthyear,
                        Cost = 0,
                        ProvisionCost = x.Costing ?? 0,
                        DeliveryAnchor = x.Contractemployee.Contract.Deliveryanchor != null ? x.Contractemployee.Contract.Deliveryanchor.Employeename : null,
                        PoNumber = x.Contractemployee.Contract.Ponumber // Added PoNumber mapping
                    });

                // Execute queries separately and concatenate in memory to avoid EF Core "different store types" translation issues
                var billings = await billingQuery.ToListAsync();
                var provisions = await provisionQuery.ToListAsync();

                var allSelectedData = billings.Concat(provisions).ToList();

                if (Value.Mode == "employeewise")
                {
                    return allSelectedData;
                }
                else if (Value.Mode == "customerwise")
                {
                    var groupedData = allSelectedData.GroupBy(x => x.CustomerName).Select(group => new ResponseForBillingDetailsDto
                    {
                        CustomerName = group.Key,
                        Code = group.First().Code,
                        Name = group.First().Name,
                        ProjectName = group.First().ProjectName,
                        PracticeName = group.First().PracticeName,
                        SubPracticeName = group.First().SubPracticeName,
                        StartDate = group.First().StartDate,
                        EndDate = group.First().EndDate,
                        MonthYear = group.First().MonthYear,
                        Cost = group.Sum(x => x.Cost),
                        ProvisionCost = group.Sum(x => x.ProvisionCost),
                        DeliveryAnchor = group.First().DeliveryAnchor,
                        PoNumber = group.First().PoNumber
                    }).ToList();

                    return groupedData;
                }
                else
                {
                    throw new Exception("The Mode parameter is not as expected");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }















        //nick imp method
        private string ConvertToValidDate(string billingMonthYear)
        {
            // Replace "Sept" with "Sep" if present
            return billingMonthYear.Replace("Sept", "Sep");
        }

        //public async Task<List<ProvisionBillingApprovalDataDto>> GetListForApprovals( JwtLoginDetailDto logindetails)        
        public async Task<Response> UpsertApproverAction(List<ApproverActionDto> Value, JwtLoginDetailDto logindetails)
        {
            int[] validStatusIds = { 4, 5 };
            try
            {   
                var EmployeeId = _context.Rmsemployees.AsNoTracking().FirstOrDefault(x => x.Userid == logindetails.TmcId).Employeeid;
                var UserRole = _context.Employeeroles.AsNoTracking().Where(x => x.Employeeid == EmployeeId).FirstOrDefault();
                if (UserRole != null)
                {
                    if (UserRole.Roleid == 6)
                    {
                        using var transaction = await _context.Database.BeginTransactionAsync();
                        foreach (var item in Value)
                        {

                            if (!validStatusIds.Contains(item.StatusId))
                            {
                                return new Response
                                {
                                    responseCode = StatusCodes.Status401Unauthorized,
                                    responseMessage = $"You are not sending an approperiate StatusId{item.StatusId}"
                                };
                            }                            

                            var existingApprovalData = await _context.Probillapprls.AsNoTracking()
                                                                  .FirstOrDefaultAsync(x => x.Contractbillingprovesionid == item.Contractbillingprovesionid);
                            if (existingApprovalData == null)
                            {
                                return new Response
                                {
                                    responseCode = StatusCodes.Status401Unauthorized,
                                    responseMessage = $"No Billing data found at BillingId {existingApprovalData.Contractbillingprovesionid}"
                                };
                            }

                            var existingApprovalDetailsData = await _context.Probillapprldetails.AsNoTracking()
                                                                   .FirstOrDefaultAsync(x => x.Probillapprlid == existingApprovalData.Probillapprlid);

                            if (existingApprovalDetailsData == null)
                            {
                                return new Response
                                {
                                    responseCode = StatusCodes.Status401Unauthorized,
                                    responseMessage = $"No Billing data found at BillingId {existingApprovalDetailsData.Probillapprlid}"
                                };
                            }
                            var existingContractBillingData = await _context.Contractbillingprovesions.AsNoTracking()
                                                                  .FirstOrDefaultAsync(x => x.Contractbillingprovesionid == existingApprovalData.Contractbillingprovesionid);

                            if (existingContractBillingData == null)
                            {
                                return new Response
                                {
                                    responseCode = StatusCodes.Status401Unauthorized,
                                    responseMessage = $"No Billing data found at BillingId {existingContractBillingData.Contractbillingprovesionid}"
                                };
                            }
                            existingApprovalData.Statusid = item.StatusId;
                            _context.Probillapprls.UpdateRange(existingApprovalData);

                            existingApprovalDetailsData.Isactiontaken = true;
                            existingApprovalDetailsData.Actiontakenon = DateTime.Now;
                            existingApprovalDetailsData.Remark = item.Remark!=null? item.Remark :null ; 
                            _context.Probillapprldetails.UpdateRange(existingApprovalDetailsData);

                            existingContractBillingData.Statusid = item.StatusId;
                            existingContractBillingData.Lastupdatedate = DateTime.Now;
                            existingContractBillingData.Lastupdateby = EmployeeId;
                            _context.Contractbillingprovesions.UpdateRange(existingContractBillingData);
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new Response
                        {
                            responseCode = StatusCodes.Status200OK,
                            responseMessage = "Data sent Succesfully"
                        };
                    }
                    else
                    {
                        return new Response
                        {
                            responseCode = StatusCodes.Status401Unauthorized,
                            responseMessage = "You are Unauthorized to send for approval"
                        };
                    }
                }
                else
                {
                    return new Response
                    {
                        responseCode = StatusCodes.Status401Unauthorized,
                        responseMessage = "You are Unauthorized to send for approval"
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Response> UpdateBillingStatus(List<BillingStatusUpdateRequestDto> value,IFormFile file,JwtLoginDetailDto logindetails)
        {
            try
            {
                #region[Image Upload Region]
                string wwwPath = _env.WebRootPath;


                // Handle logo upload (similar to AddCustomerAsync method)
                string basePath = Path.Combine(this._env.WebRootPath, @"BillingAttachments\" + logindetails.TmcId.ToString());
                string databasePath = @"\BillingAttachments\" + logindetails.TmcId.ToString();
                string databsefilePath = "";

                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                if (file != null)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(basePath, uniqueFileName);
                    databsefilePath = Path.Combine(databasePath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                #endregion
                foreach (var item in value)
                {
                    if (item.contractbillingid != 0)
                    {
                        var existingBilling = await _context.Contractbillings.FirstOrDefaultAsync(x => x.Contractbillingid == item.contractbillingid);

                        if (existingBilling == null)
                        {
                            throw new Exception($"No Billing exists for the given contractbilling Id--{item.contractbillingid}");
                        }

                        existingBilling.Isbilled = item.isBilled ?? existingBilling.Isbilled;
                        existingBilling.Istobebilled = item.isTobeBilled ?? existingBilling.Istobebilled;
                        existingBilling.Estimatedbillingdate = item.Exptectedbillingdate ?? existingBilling.Estimatedbillingdate;
                        
                        if (item.isTobeBilled == true && item.isBilled == false)
                        {
                            existingBilling.Documenturl = string.IsNullOrEmpty(databsefilePath) ? null : databsefilePath;
                        }

                        _context.Contractbillings.UpdateRange(existingBilling);
                    }
                }

                await _context.SaveChangesAsync();
                return new Response { responseCode = 200, responseMessage = "Updated Successfully!" };
            }            
            catch
            {
                throw;
            }
        }
        public async Task<Response> UpdateActualBilling(List<BillingApprvalDto> Value, JwtLoginDetailDto logindetails)
        {
            var transection = _context.Database.BeginTransaction();
            try
            {
                 
                if (Value == null)
                {
                    throw new ArgumentNullException(nameof(Value)); 
                }
                foreach (var item in Value)
                {
                    if (item.Contractbillingid == null || item.Contractbillingid == 0)
                    {
                        throw new Exception($"ContracbillingId can not be null at {item.Costing}");
                    }
                    var existingcontractbilling = await _context.Contractbillings.AsNoTracking().FirstOrDefaultAsync(x => x.Contractbillingid == item.Contractbillingid);
                    if (existingcontractbilling != null) 
                    { 
                        existingcontractbilling.Costing=(item.Costing==0 || item.Costing == null)?existingcontractbilling.Costing:item.Costing;
                        existingcontractbilling.Isrevised=item.IsRevised==null?false:item.IsRevised;    
                        existingcontractbilling.Statusid=item.IsRevised==true?2:(item.StatusId==null? existingcontractbilling.Statusid : item.StatusId);
                        existingcontractbilling.Estimatedbillingdate= item.Exptectedbillingdate ==null ? existingcontractbilling.Estimatedbillingdate : item.Exptectedbillingdate;
                        existingcontractbilling.Remark = item.Remarks ?? existingcontractbilling.Remark;
                        existingcontractbilling.Isbilled = (item.IsRevised == true && item.StatusId == 2) ? false : existingcontractbilling.Isbilled;
                        existingcontractbilling.Istobebilled = (item.IsRevised == true && item.StatusId == 2) ? false : existingcontractbilling.Istobebilled;
                    }
                    _context.Contractbillings.UpdateRange(existingcontractbilling);
                }
                await _context.SaveChangesAsync();  
                await transection.CommitAsync();

                return new Response { responseCode=200,responseMessage="Updated Sussecfullly"};
                

            }
            catch(Exception ex)
            {
               await transection.RollbackAsync();
                throw;
            }
        }
        public async Task<Response> SendApprovalActualBilling(List<BillingApprvalDto> Value, JwtLoginDetailDto logindetails)
        {
           
            var transection = _context.Database.BeginTransaction();
            try
            {

                if (Value == null)
                {
                    throw new ArgumentNullException(nameof(Value));
                }
                foreach (var item in Value)
                {
                    if (item.Contractbillingid == null || item.Contractbillingid == 0)
                    {
                        throw new Exception($"ContracbillingId can not be null at {item.Costing}");
                    }
                    var statusId = item.StatusId ?? throw new ArgumentException($"StatusId is null at contractbillingid {item.Contractbillingid}", nameof(item.StatusId));
                    var existingcontractbilling = await _context.Contractbillings.AsNoTracking().FirstOrDefaultAsync(x => x.Contractbillingid == item.Contractbillingid);
                    if (existingcontractbilling == null)
                    {
                        throw new Exception($"No biling data is found at contractbillingid {item.Contractbillingid}");
                    }
                    dynamic oldCosting = null;
                    dynamic oldDate=null;
                    if (statusId == 4)
                    {
                        var result = await _context.Contractbillingactualhistories
                            .Where(cbah => cbah.Contractbillingid == item.Contractbillingid)
                            .OrderByDescending(cbah => cbah.Revisionnumber)
                           // .Select(cbah => cbah.Oldcosting)
                            .FirstOrDefaultAsync();
                        if (result != null)
                        {
                            oldCosting=result.Oldcosting;
                            oldDate=result.Oldestimatedbillingdate;
                        }
                    }
                        existingcontractbilling.Costing = oldCosting ?? existingcontractbilling.Costing;
                        existingcontractbilling.Estimatedbillingdate = oldDate ?? existingcontractbilling.Estimatedbillingdate;
                        existingcontractbilling.Isrevised = item.IsRevised??true;
                        existingcontractbilling.Istobebilled = false;
                        existingcontractbilling.Isbilled = false;
                        existingcontractbilling.Statusid = statusId;
                        existingcontractbilling.Remark = item.Remarks ?? existingcontractbilling.Remark;
                        _context.Contractbillings.UpdateRange(existingcontractbilling);
                }
                await _context.SaveChangesAsync();
                await transection.CommitAsync();
                return new Response { responseCode = 200, responseMessage = "Updated Sussecfullly" };
            }        
            catch (Exception ex)
            {
                await transection.RollbackAsync();
                throw;
            }
        }
        // public async Task<List<Contractbilling>> ListofActualContractbillingForApproval(JwtLoginDetailDto logindetails)
        public async Task<IEnumerable> ListofActualContractbillingForApproval(JwtLoginDetailDto logindetails)
        {
            try
            {
                List<ActualBillingApprovalDataDto> filterReslt = new List<ActualBillingApprovalDataDto>();
                var employee = await _context.Rmsemployees.AsNoTracking()
                                 .Include(e => e.Employeeroles)
                                 .ThenInclude(er => er.Role)
                                 .FirstOrDefaultAsync(e => e.Userid == logindetails.TmcId);

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
                var practiceHead = await _context.Practiceheads.AsNoTracking()
                                                 .Include(ph => ph.Practice)
                                                 .Where(ph => ph.Employeeid == employee.Employeeid)
                                                 .ToListAsync();
                if (practiceHead == null && employeeRole.Roleid == 9)
                {
                    return new List<object>();
                }
                var practiceNames = practiceHead.Select(x => x.Practice.Practicename.ToLower().Trim()).ToList();


                var queryRersult = await _context.Contractbillingactualhistories.AsNoTracking()
                                       .Include(billings => billings.Contractbilling)
                                          .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                             .ThenInclude(contract => contract.Contract)
                                                 .ThenInclude(project => project.Project)
                                                     .ThenInclude(subprac => subprac.Subpractice)
                                                         .ThenInclude(prac => prac.Practice)
                                        .Include(billings => billings.Contractbilling)
                                          .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                             .ThenInclude(contract => contract.Contract)
                                                 .ThenInclude(da => da.Deliveryanchor)
                                       .Include(billings => billings.Contractbilling)
                                          .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                             .ThenInclude(emp => emp.Employee)
                                       .Include(status => status.Status)
                                       .Where(cbh => cbh.Statusid == 2 && cbh.Contractbilling.Statusid == 2 && cbh.Contractbilling.Isrevised == true).ToListAsync();


                var filterResult = queryRersult.Select(item => new ActualBillingApprovalDataDto
                {
                    ProjactName = item.Contractbilling.Contractemployee.Contract.Project.Projectname,
                    PoNumber = item.Contractbilling.Contractemployee.Contract.Ponumber,
                    ContractNumber = item.Contractbilling.Contractemployee.Contract.Contractno,
                    DeliveryAnchor = item.Contractbilling.Contractemployee.Contract.Deliveryanchor.Employeename,
                    PracticeName = item.Contractbilling.Contractemployee.Contract.Project.Subpractice.Practice.Practicename,
                    TmcId = item.Contractbilling.Contractemployee.Employee.Userid.ToString(),
                    Contractbillingid = item.Contractbillingid,
                    Contractemployeeid = item.Contractbilling.Contractemployeeid,
                    ContractId = item.Contractbilling.Contractemployee.Contractid,
                    ResourceName = item.Contractbilling.Contractemployee.Employee.Employeename,
                    ProjectName = item.Contractbilling.Contractemployee.Contract.Project.Projectname,
                    MonthYear = item.Contractbilling.Billingmonthyear,
                    Billing = item.Contractbilling.Costing,
                    EstimatedBilligDate = item.Contractbilling.Estimatedbillingdate,
                    Status = item.Status.Statusname,
                    RemarkByDH = item.Remark,
                    RevisionNo = item.Revisionnumber,
                    CreatedOn = item.Createddate,
                    OldBilling = item.Oldcosting,
                    OldEstimatedBilligDate = item.Oldestimatedbillingdate,
                    ApproverActionTakenon = item.Lastupdatedate,
                }).ToList();

                if (employeeRole.Roleid==6)
                {
                    var groupedResult = filterReslt
                                                .GroupBy(x => new
                                                {
                                                    x.ProjactName,
                                                    x.PoNumber,
                                                    x.ContractNumber,
                                                    x.DeliveryAnchor,
                                                    x.ContractId
                                                })
                                                .Select(g => new { Key = g.Key, Value = g.ToList() })
                                                .ToList();
                    return groupedResult;

                }
                else
                {
                    filterReslt = filterResult.Where(x => practiceNames.Contains(x.PracticeName.ToLower().Trim())).ToList();

                    var groupedResult = filterReslt
                                                    .GroupBy(x => new
                                                    {
                                                        x.ProjactName,
                                                        x.PoNumber,
                                                        x.ContractNumber,
                                                        x.DeliveryAnchor,
                                                        x.ContractId
                                                    })
                                                    .Select(g => new { Key = g.Key, Value = g.ToList() })
                                                    .ToList();
                    return groupedResult;

                }
                
            }
            catch (Exception ex)
            {
                throw;
            }
            

        }

        public async Task<dynamic> GetEmployeeActualBillins(GetEmployeeActualBillinDto Value)
        {
            try
            {
                if (Value==null)
                {
                    throw new ArgumentNullException(nameof(Value)); 

                }


                var queryResult = await _context.Contractbillings.AsNoTracking()
                                                  .Where(x=>x.Contractemployeeid==Value.contractEmployeeId && x.Billingmonthyear==Value.monthyear)                                             
                                                 .Include(contractEmp => contractEmp.Contractemployee)
                                                    .FirstOrDefaultAsync();
                //var groupedResult = queryRersult
                //                                .GroupBy(x => new
                //                                {
                //                                    x.Costing,                                                    
                //                                })
                //                                .Select(g=>new { Costing=g.Key.Costing,details=g.ToList()})
                //                                .ToList();               

                return new { costing = queryResult.Costing };
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }


}
