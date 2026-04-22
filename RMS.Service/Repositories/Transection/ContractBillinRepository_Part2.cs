using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RMS.Data;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RMS.Service.Repositories.Master
{
    public partial class ContractBillinRepository 
    {      
        public async Task<Contractbillingprovesion> Provision_GetContractBillingsById(int id)
        {
            try
            {
                var contractbilling = await _context.Contractbillingprovesions.AsNoTracking().FirstOrDefaultAsync(x => x.Contractbillingprovesionid == id);
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

        // private List<string> DateFormatter(DateTime fromDate, DateTime toDate)
        //{
        //    List<string> dateList = new List<string>();
        //    while (fromDate <= toDate)
        //    {
        //        string formattedDate = fromDate.ToString("MMM-yy");
        //        dateList.Add(formattedDate);
        //        fromDate = fromDate.AddMonths(1);
        //    }
        //    return dateList;
        //}
        public static List<string> DateFormatter(DateTime fromDate, DateTime toDate)
        {
            List<string> dateList = new List<string>();
            while (fromDate <= toDate)
            {
                // Debug: Print the current fromDate
              //  Console.WriteLine("Processing Date: " + fromDate.ToString("yyyy-MM-dd"));

                // Format the date and add to the list
                string formattedDate = fromDate.ToString("MMM-yy", CultureInfo.InvariantCulture);
                dateList.Add(formattedDate);

                // Add one month to fromDate
                fromDate = fromDate.AddMonths(1);

                // Debug: Print the next fromDate
               // Console.WriteLine("Next Date: " + fromDate.ToString("yyyy-MM-dd"));
            }
            return dateList;
        }
        // provision contractbilling

        public async Task<Response> Provision_UpsertContractBillings(List<ContractEmpBillingDto> contractBillingDtos, JwtLoginDetailDto loginDetails)
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
                    var existingContractBillings = await _context.Contractbillingprovesions.AsNoTracking()
                                                            .Where(x => x.Contractemployeeid == item.Contractemployeeid && x.Billingmonthyear == item.BillingMonthYear)
                                                            .FirstOrDefaultAsync();
                    if (existingContractBillings != null)
                    {
                        UpdateContractBillings_Provision(existingContractBillings, item, employeeId);
                    }
                    else
                    {
                        CreateContractBillings_Provision(item, employeeId);
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
        private void CreateContractBillings_Provision(ContractEmpBillingDto contractBillingDto, int employeeId)
        {
            var contractBillingsToAdd = new Contractbillingprovesion
            {
                Contractemployeeid = contractBillingDto.Contractemployeeid,
                Billingmonthyear = contractBillingDto.BillingMonthYear,
                Costing = contractBillingDto.Costing,
                Createdby = employeeId,
                Createddate = DateTime.Now,
                EstimatedBillingDate = contractBillingDto.Exptectedbillingdate.HasValue ? contractBillingDto.Exptectedbillingdate.Value : (DateTime?)null
            };

            _context.Contractbillingprovesions.AddRange(contractBillingsToAdd);
        }
        private void UpdateContractBillings_Provision(Contractbillingprovesion existingContractBilling, ContractEmpBillingDto contractBillingDto, int employeeId)
        {
            if (contractBillingDto.isRevised == true)
            {
                existingContractBilling.Costing = contractBillingDto.Costing;
                existingContractBilling.Lastupdatedate = DateTime.Now;
                existingContractBilling.Lastupdateby = employeeId;
                existingContractBilling.EstimatedBillingDate = contractBillingDto.Exptectedbillingdate.HasValue ? contractBillingDto.Exptectedbillingdate.Value : (DateTime?)null;
                existingContractBilling.Isrevised = contractBillingDto.isRevised.HasValue ? contractBillingDto.isRevised.Value : (bool?)null;
                existingContractBilling.Statusid = 2;
                _context.Contractbillingprovesions.UpdateRange(existingContractBilling);
                var existingApprovalData = _context.Probillapprls.AsNoTracking().FirstOrDefault(x => x.Contractbillingprovesionid == existingContractBilling.Contractbillingprovesionid);
                if (existingApprovalData != null)
                {
                    existingApprovalData.Statusid = 2; //pending
                    _context.Probillapprls.Update(existingApprovalData);
                    _context.SaveChanges();
                }


            }
            else
            {



                existingContractBilling.Costing = contractBillingDto.Costing;
                existingContractBilling.Lastupdatedate = DateTime.Now;
                existingContractBilling.Lastupdateby = employeeId;
                existingContractBilling.EstimatedBillingDate = contractBillingDto.Exptectedbillingdate.HasValue ? contractBillingDto.Exptectedbillingdate.Value : (DateTime?)null;

                existingContractBilling.Isrevised = existingContractBilling.Isdeleted == true ?
                                                                                               existingContractBilling.Isdeleted :
                                                                                               (contractBillingDto.isRevised.HasValue ? contractBillingDto.isRevised.Value : (bool?)null);
                _context.Contractbillingprovesions.UpdateRange(existingContractBilling);

                //approval data update for panding 
                var existingApprovalData = _context.Probillapprls.AsNoTracking().FirstOrDefault(x => x.Contractbillingprovesionid == existingContractBilling.Contractbillingprovesionid);
                if (existingApprovalData != null)
                {
                    existingApprovalData.Statusid = 2; //pending
                    _context.Probillapprls.Update(existingApprovalData);
                    _context.SaveChanges();
                }
                _context.Contractbillingprovesions.UpdateRange(existingContractBilling);
            }

        }
        public async Task<Response> UpsertSendForApproval_Provision(JwtLoginDetailDto logindetails, List<BillingApprvalDto> Value)
        {
            try
            {
                string daname = "";
                string daemail = "";
                string practiceheademail = "";
                string practiceheadname = "";
                string projectname = "";
                var EmployeeId = _context.Rmsemployees.AsNoTracking().FirstOrDefault(x => x.Userid == logindetails.TmcId).Employeeid;
                var UserRole = _context.Employeeroles.AsNoTracking().Where(x => x.Employeeid == EmployeeId).FirstOrDefault();
                if (UserRole != null)
                {
                    if (UserRole.Roleid == 3 || UserRole.Roleid == 8)
                    {
                        using var transaction = await _context.Database.BeginTransactionAsync();
                        foreach (var item in Value)
                        {
                            var existingProBillingData = await _context.Contractbillingprovesions.Include(ce=>ce.Contractemployee)
                                                                                                   .ThenInclude(c=>c.Contract)
                                                                                                     .ThenInclude(p=>p.Project)
                                                                                                       .ThenInclude(spc=>spc.Subpractice)
                                                                                                         .ThenInclude(pc=>pc.Practice)
                                                                  //.AsNoTracking()
                                                                  .FirstOrDefaultAsync(x => x.Contractbillingprovesionid == item.Contractbillingid);
                            

                            if (existingProBillingData == null)
                            {
                                return new Response
                                {
                                    responseCode = StatusCodes.Status401Unauthorized,
                                    responseMessage = $"No Billing data found at BillingId {existingProBillingData.Contractbillingprovesionid}"
                                };
                            }

                            var practiceHead = await _context.Practiceheads
                                  .Include(x => x.Practice)
                                  .FirstOrDefaultAsync(x => x.Practiceid== existingProBillingData.Contractemployee.Contract.Project.Subpractice.Practiceid);
                            
                            if (practiceHead == null)
                            {
                                return new Response
                                {
                                    responseCode = StatusCodes.Status401Unauthorized,
                                    responseMessage = $"No Practice head found against this project {existingProBillingData.Contractbillingprovesionid}"
                                };
                            }

                            var practiceName = practiceHead.Practice.Practicename.ToLower().Trim();
                            //by nick for mail
                            projectname = _context.Projects
                                .Where(a => a.Projectid == existingProBillingData.Contractemployee.Contract.Project.Projectid)
                                .Select(a => a.Projectname)
                                .FirstOrDefault();
                             practiceheademail = _context.Rmsemployees
                                                      .Where(a => a.Employeeid == practiceHead.Employeeid)
                                                      .Select(a => a.Companyemail)
                                                      .FirstOrDefault();
                            daemail = _context.Rmsemployees
                                                     .Where(a => a.Userid == logindetails.TmcId)
                                                     .Select(a => a.Companyemail)
                                                     .FirstOrDefault();
                            daname = _context.Rmsemployees
                                                     .Where(a => a.Userid == logindetails.TmcId)
                                                     .Select(a => a.Employeename)
                                                     .FirstOrDefault();
                            practiceheadname = _context.Rmsemployees
                                                .Where(a => a.Employeeid == practiceHead.Employeeid)
                                                .Select(a => a.Employeename)
                                                .FirstOrDefault();
                            var CurrentStageDate = await _context.Probillapprlstages.Include(ph=>ph.Practicehead).AsNoTracking()
                                                                 .FirstOrDefaultAsync(x => x.Stageorder == 1 && x.Practicehead.Practiceid==practiceHead.Practiceid);
                            if (CurrentStageDate == null)
                            {
                                return new Response
                                {
                                    responseCode = StatusCodes.Status401Unauthorized,
                                    responseMessage = "No satge found to send your approval request"
                                };
                            }
                            var existingApprovalRequest = await _context.Probillapprls.AsNoTracking()
                                                           .Where(x => x.Contractbillingprovesionid == item.Contractbillingid).FirstOrDefaultAsync();
                            if (existingApprovalRequest != null)
                            {
                                if (existingApprovalRequest.Currentstageid == 2)
                                {
                                    return new Response
                                    {
                                        responseCode = StatusCodes.Status401Unauthorized,
                                        responseMessage = "You are not authorized since the request is pending at level-2"
                                    };

                                }
                                existingProBillingData.Statusid = 2;
                                existingProBillingData.Costing = item.Costing;
                                existingProBillingData.EstimatedBillingDate = item.Exptectedbillingdate;
                                _context.Contractbillingprovesions.UpdateRange(existingProBillingData);
                                //var existingAppvalDertails = await _context.Probillapprldetails
                                //                                           .Where(x => x.Probillapprlid == existingApprovalRequest.Probillapprlid).ToListAsync();
                                // _context.Contractbillingprovesions.Add(existingProBillingData);                                    
                            }
                            else
                            {
                                //_context.Entry(existingProBillingData).State = EntityState.Detached;
                                existingProBillingData.Statusid = 2;
                                _context.Contractbillingprovesions.UpdateRange(existingProBillingData);
                                var NewBillingApprovalData = new Probillapprl
                                {
                                    Contractbillingprovesionid = item.Contractbillingid,
                                    Currentstageid = CurrentStageDate.Probillapprlstageid,
                                    Statusid = 2
                                };
                                _context.Probillapprls.AddRange(NewBillingApprovalData);
                                await _context.SaveChangesAsync();
                                var NewBillingApprovalDetailsData = new Probillapprldetail
                                {
                                    Probillapprlid = NewBillingApprovalData.Probillapprlid,
                                    Stageid = NewBillingApprovalData.Currentstageid,

                                };
                                _context.Probillapprldetails.AddRange(NewBillingApprovalDetailsData);

                            }
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new Response
                        {
                            responseCode = StatusCodes.Status200OK,
                            responseMessage = "Data sent Succesfully",
                            data = new
                            {
                                practiceheademail = practiceheademail,
                                daemail = daemail,
                                daname = daname,
                                practiceheadname = practiceheadname,
                                projectname = projectname

                            }
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

        //public async Task<List<ProvisionBillingApprovalDataDto>> GetListForApprovals( JwtLoginDetailDto logindetails)
        public async Task<IEnumerable> GetListForApprovals_Provision(JwtLoginDetailDto logindetails)
        {
            try
            {
                List<ProvisionBillingApprovalDataDto> filterReslt = new List<ProvisionBillingApprovalDataDto>();
                var EmployeeId = _context.Rmsemployees.AsNoTracking().FirstOrDefault(x => x.Userid == logindetails.TmcId)?.Employeeid;
                if (EmployeeId == null)
                {
                    throw new Exception("Employee not found.");
                }
                var UserRole = _context.Employeeroles.AsNoTracking().Where(x => x.Employeeid == EmployeeId).FirstOrDefault();
                if (UserRole == null)
                {
                    throw new Exception("You don't have any Authorized role to access this dataset");

                }
                var practiceHead = await _context.Practiceheads.AsNoTracking()
                                         .Include(x => x.Practice)
                                         .Include(x=>x.Probillapprlstages)
                                         .Where((x => x.Employeeid == EmployeeId))
                                         .ToListAsync();
              
                var practiceNames = practiceHead.Select(x => x.Practice?.Practicename.ToLower().Trim()).ToList();
                
               
                //var currentstageIds= practiceHead.Select(x=>x.Probillapprlstages.Select(x=>x.Stageapproverid)).ToList();    


                if (UserRole.Roleid == 3)
                {
                    var queryRersult = await _context.Contractbillingprovesions.AsNoTracking()
                                     .Include(contractEmp => contractEmp.Contractemployee)
                                       .ThenInclude(emp => emp.Employee)
                                     .Include(contractEmp => contractEmp.Contractemployee)
                                       .ThenInclude(contract => contract.Contract)
                                        .ThenInclude(project => project.Project)
                                     .Include(approval => approval.Probillapprls)
                                       .ThenInclude(approvalDetails => approvalDetails.Probillapprldetails)
                                     .Include(approval => approval.Probillapprls)
                                       .ThenInclude(approvalDetails => approvalDetails.Probillapprldetails)
                                     .Include(status => status.Status)
                                     .Where(provision => provision.Probillapprls.Any(apprl => !apprl.Isdeleted))
                                     .ToListAsync();


                    foreach (var item in queryRersult.Where(x => x.Contractemployee.Contract.Deliveryanchorid == EmployeeId))
                    {
                        filterReslt.Add(new ProvisionBillingApprovalDataDto
                        {
                            Contractbillingprovesionid = item.Contractbillingprovesionid,
                            Contractemployeeid = item.Contractemployeeid,
                            ContractId = item.Contractemployee.Contractid,
                            ResourceName = item.Contractemployee.Employee.Employeename,
                            ProjectName = item.Contractemployee.Contract.Project.Projectname,
                            MonthYear = item.Billingmonthyear,
                            ProvisionAmount = item.Costing,
                            EstimatedBilligDate = item.EstimatedBillingDate ?? null,
                            TmcId = item.Contractemployee.Employee.Userid.ToString(),
                            Status = item.Status == null ? "Draft" : item.Status.Statusname,
                            RemarkByDH = item.Probillapprls.Select(x => x.Probillapprldetails.Select(y => y.Remark).FirstOrDefault()).FirstOrDefault()

                        });

                    }
                    return filterReslt;

                }
                else if (UserRole.Roleid == 6)
                {
                    var queryRersult = await _context.Probillapprls.AsNoTracking()
                                                     .Include(billings => billings.Contractbillingprovesion)
                                                        .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                                           .ThenInclude(contract => contract.Contract)
                                                               .ThenInclude(project => project.Project)
                                                      .Include(billings => billings.Contractbillingprovesion)
                                                        .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                                           .ThenInclude(contract => contract.Contract)
                                                               .ThenInclude(da => da.Deliveryanchor)
                                                     .Include(billings => billings.Contractbillingprovesion)
                                                        .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                                           .ThenInclude(emp => emp.Employee)
                                                     .Include(billings => billings.Probillapprldetails)
                                                     .Include(status => status.Status).ToListAsync();

                    foreach (var item in queryRersult.Where(x => x.Statusid == 2 && x.Contractbillingprovesion.Statusid == 2))
                    {
                        filterReslt.Add(new ProvisionBillingApprovalDataDto
                        {
                            ProjectName = item.Contractbillingprovesion.Contractemployee.Contract.Project.Projectname,
                            PoNumber = item.Contractbillingprovesion.Contractemployee.Contract.Ponumber,
                            ContractNumber = item.Contractbillingprovesion.Contractemployee.Contract.Contractno,
                            DeliveryAnchor = item.Contractbillingprovesion.Contractemployee.Contract.Deliveryanchor.Employeename,

                            TmcId = item.Contractbillingprovesion.Contractemployee.Employee.Userid.ToString(),
                            Contractbillingprovesionid = item.Contractbillingprovesionid,
                            Contractemployeeid = item.Contractbillingprovesion.Contractemployeeid,
                            ContractId = item.Contractbillingprovesion.Contractemployee.Contractid,
                            ResourceName = item.Contractbillingprovesion.Contractemployee.Employee.Employeename,
                            //ProjectName = item.Contractbillingprovesion.Contractemployee.Contract.Project.Projectname,
                            MonthYear = item.Contractbillingprovesion.Billingmonthyear,
                            ProvisionAmount = item.Contractbillingprovesion.Costing,
                            EstimatedBilligDate = item.Contractbillingprovesion.EstimatedBillingDate,
                            Status = item.Status.Statusname,
                            RemarkByDH = item.Probillapprldetails.Select(x => x.Remark).FirstOrDefault()
                        });
                    }
                    //----------------------------------------------------------------------------                   
                    var groupedResult = filterReslt
                                                        .GroupBy(x => new
                                                        {
                                                            x.ProjectName,
                                                            x.PoNumber,
                                                            x.ContractNumber,
                                                            x.DeliveryAnchor,
                                                            x.ContractId
                                                        })
                                                        .Select(g => new { Key = g.Key, Value = g.ToList() })
                                                        .ToList();
                    return groupedResult;
                }
                else if (UserRole.Roleid == 9)
                {


                    var queryRersult = await _context.Probillapprls.AsNoTracking()
                                 .Include(billings => billings.Contractbillingprovesion)
                                    .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                       .ThenInclude(contract => contract.Contract)
                                           .ThenInclude(project => project.Project)
                                  .Include(billings => billings.Contractbillingprovesion)
                                    .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                       .ThenInclude(contract => contract.Contract)
                                           .ThenInclude(da => da.Deliveryanchor)
                                 .Include(billings => billings.Contractbillingprovesion)
                                    .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                       .ThenInclude(emp => emp.Employee)
                                 .Include(billings => billings.Probillapprldetails)
                                 .Include(status => status.Status)
                                 .Include(billings => billings.Contractbillingprovesion)
                                    .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                       .ThenInclude(contract => contract.Contract)
                                           .ThenInclude(project => project.Project)
                                             .ThenInclude(spc => spc.Subpractice)
                                               .ThenInclude(px => px.Practice)
                                 .ToListAsync();

                    queryRersult = queryRersult.Where(x => practiceNames
                                    .Contains(x.Contractbillingprovesion.Contractemployee.Contract.Project.Subpractice.Practice.Practicename.ToLower().Trim())).ToList();

                    


                    foreach (var item in queryRersult.Where(x => (x.Statusid == 2 || x.Statusid == 4) && x.Contractbillingprovesion.Statusid == 2))
                    {
                        filterReslt.Add(new ProvisionBillingApprovalDataDto
                        {
                            ProjectName = item.Contractbillingprovesion.Contractemployee.Contract.Project.Projectname,
                            PoNumber = item.Contractbillingprovesion.Contractemployee.Contract.Ponumber,
                            ContractNumber = item.Contractbillingprovesion.Contractemployee.Contract.Contractno,
                            DeliveryAnchor = item.Contractbillingprovesion.Contractemployee.Contract.Deliveryanchor.Employeename,

                            TmcId = item.Contractbillingprovesion.Contractemployee.Employee.Userid.ToString(),
                            Contractbillingprovesionid = item.Contractbillingprovesionid,
                            Contractemployeeid = item.Contractbillingprovesion.Contractemployeeid,
                            ContractId = item.Contractbillingprovesion.Contractemployee.Contractid,
                            ResourceName = item.Contractbillingprovesion.Contractemployee.Employee.Employeename,
                            //ProjectName = item.Contractbillingprovesion.Contractemployee.Contract.Project.Projectname,
                            MonthYear = item.Contractbillingprovesion.Billingmonthyear,
                            ProvisionAmount = item.Contractbillingprovesion.Costing,
                            EstimatedBilligDate = item.Contractbillingprovesion.EstimatedBillingDate,
                            Status = item.Status.Statusname,
                            RemarkByDH = item.Probillapprldetails.Select(x => x.Remark).FirstOrDefault()
                        });
                    }
                    var groupedResult = filterReslt
                                                       .GroupBy(x => new
                                                       {
                                                           x.ProjectName,
                                                           x.PoNumber,
                                                           x.ContractNumber,
                                                           x.DeliveryAnchor,
                                                           x.ContractId
                                                       })
                                                       .Select(g => new { Key = g.Key, Value = g.ToList() })
                                                       .ToList();
                    return groupedResult;

                  
                }
                else if (logindetails.TmcId == "3844")
                {
                    var queryRersult = await _context.Probillapprls.AsNoTracking()
                                                     .Include(billings => billings.Contractbillingprovesion)
                                                        .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                                           .ThenInclude(contract => contract.Contract)
                                                               .ThenInclude(project => project.Project)
                                                      .Include(billings => billings.Contractbillingprovesion)
                                                        .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                                           .ThenInclude(contract => contract.Contract)
                                                               .ThenInclude(da => da.Deliveryanchor)
                                                     .Include(billings => billings.Contractbillingprovesion)
                                                        .ThenInclude(contractEmp => contractEmp.Contractemployee)
                                                           .ThenInclude(emp => emp.Employee)
                                                     .Include(billings => billings.Probillapprldetails)
                                                     .Include(status => status.Status).ToListAsync();

                    foreach (var item in queryRersult.Where(x =>  x.Statusid == 2))
                    {
                        filterReslt.Add(new ProvisionBillingApprovalDataDto
                        {
                            ProjectName = item.Contractbillingprovesion.Contractemployee.Contract.Project.Projectname,
                            PoNumber = item.Contractbillingprovesion.Contractemployee.Contract.Ponumber,
                            ContractNumber = item.Contractbillingprovesion.Contractemployee.Contract.Contractno,
                            DeliveryAnchor = item.Contractbillingprovesion.Contractemployee.Contract.Deliveryanchor.Employeename,

                            TmcId = item.Contractbillingprovesion.Contractemployee.Employee.Userid.ToString(),
                            Contractbillingprovesionid = item.Contractbillingprovesionid,
                            Contractemployeeid = item.Contractbillingprovesion.Contractemployeeid,
                            ContractId = item.Contractbillingprovesion.Contractemployee.Contractid,
                            ResourceName = item.Contractbillingprovesion.Contractemployee.Employee.Employeename,
                            // TmcId = item.Contractbillingprovesion.Contractemployee.Employee.Userid.ToString(),
                            //ProjectName = item.Contractbillingprovesion.Contractemployee.Contract.Project.Projectname,
                            MonthYear = item.Contractbillingprovesion.Billingmonthyear,
                            ProvisionAmount = item.Contractbillingprovesion.Costing,
                            EstimatedBilligDate = item.Contractbillingprovesion.EstimatedBillingDate,
                            Status = item.Status.Statusname,
                            RemarkByDH = item.Probillapprldetails.Select(x => x.Remark).FirstOrDefault()
                        });
                    }
                    //----------------------------------------------------------------------------                   

                    var groupedResult = filterReslt
                                                    .GroupBy(x => new
                                                    {
                                                        x.ProjectName,
                                                        x.PoNumber,
                                                        x.ContractNumber,
                                                        x.DeliveryAnchor,
                                                        x.ContractId
                                                    })
                                                    .Select(g => new { Key = g.Key, Value = g.ToList() })
                                                    .ToList();
                    return groupedResult;
                }
                else if (UserRole.Roleid == 8)
                {
                    var queryRersult = await _context.Contractbillingprovesions.AsNoTracking()
                                     .Include(contractEmp => contractEmp.Contractemployee)
                                       .ThenInclude(emp => emp.Employee)
                                     .Include(contractEmp => contractEmp.Contractemployee)
                                       .ThenInclude(contract => contract.Contract)
                                        .ThenInclude(project => project.Project)
                                     .Include(approval => approval.Probillapprls)
                                       .ThenInclude(approvalDetails => approvalDetails.Probillapprldetails)
                                     .Include(approval => approval.Probillapprls)
                                       .ThenInclude(approvalDetails => approvalDetails.Probillapprldetails)
                                     .Include(status => status.Status).ToListAsync();

                    foreach (var item in queryRersult)
                    {
                        filterReslt.Add(new ProvisionBillingApprovalDataDto
                        {
                            Contractbillingprovesionid = item.Contractbillingprovesionid,
                            Contractemployeeid = item.Contractemployeeid,
                            ContractId = item.Contractemployee.Contractid,
                            ResourceName = item.Contractemployee.Employee.Employeename,
                            ProjectName = item.Contractemployee.Contract.Project.Projectname,
                            MonthYear = item.Billingmonthyear,
                            ProvisionAmount = item.Costing,
                            EstimatedBilligDate = item.EstimatedBillingDate ?? null,
                            Status = item.Status == null ? "Draft" : item.Status.Statusname,
                            TmcId = item.Contractemployee.Employee.Userid.ToString(),
                            RemarkByDH = item.Probillapprls.Select(x => x.Probillapprldetails.Select(y => y.Remark).FirstOrDefault()).FirstOrDefault()

                        });

                    }
                    return filterReslt;

                }
                else
                {
                    throw new Exception("You are not authorized to acees this dataset ");

                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable> GetProvisionRequestProjectWise(JwtLoginDetailDto logindetails)
        {
            var employeeId = _context.Rmsemployees.AsNoTracking().FirstOrDefault(x => x.Userid == logindetails.TmcId)?.Employeeid;
            if (employeeId == null)
            {
                throw new Exception("Employee not found.");
            }

            var userRole = _context.Employeeroles.AsNoTracking().FirstOrDefault(x => x.Employeeid == employeeId);
            if (userRole == null || (userRole.Roleid != 3 && userRole.Roleid != 8))
            {
                throw new Exception("You don't have any authorized role to access this dataset.");
            }

            var queryResult = await _context.Contractbillingprovesions.AsNoTracking()
                                    .Include(contractEmp => contractEmp.Contractemployee)
                                            .ThenInclude(emp => emp.Employee)
                                    .Include(contractEmp => contractEmp.Contractemployee)
                                      .ThenInclude(contract => contract.Contract)
                                         .ThenInclude(project => project.Project)
                                    .Include(contractEmp => contractEmp.Contractemployee)
                                      .ThenInclude(contract => contract.Contract)
                                          .ThenInclude(DA=>DA.Deliveryanchor)
                                    .Include(approval => approval.Probillapprls)
                                      .ThenInclude(approvalDetails => approvalDetails.Probillapprldetails)
                                      .Include(status => status.Status)
                                    .ToListAsync();

            if (userRole.Roleid == 3)
            {
                queryResult = queryResult.Where(x => x.Contractemployee.Contract.Deliveryanchorid == employeeId).ToList();
            }

            List<ProvisionBillingApprovalDataDto> filteredResult = queryResult.Select(item => new ProvisionBillingApprovalDataDto
            {
                Contractbillingprovesionid = item.Contractbillingprovesionid,
                Contractemployeeid = item.Contractemployeeid,
                ContractId = item.Contractemployee.Contractid,
                ResourceName = item.Contractemployee.Employee.Employeename,
                ProjectName = item.Contractemployee.Contract.Project.Projectname,
                PoNumber=item.Contractemployee.Contract.Ponumber,
                ContractNumber=item.Contractemployee.Contract.Contractno,
                DeliveryAnchor=item.Contractemployee.Contract.Deliveryanchor==null?"": item.Contractemployee.Contract.Deliveryanchor.Employeename,
                MonthYear = item.Billingmonthyear,
                ProvisionAmount = item.Costing,
                EstimatedBilligDate = item.EstimatedBillingDate,
                TmcId = item.Contractemployee.Employee.Userid.ToString(),
                Status = item.Status?.Statusname ?? "Draft",
                RemarkByDH = item.Probillapprls.SelectMany(x => x.Probillapprldetails).Select(y => y.Remark).FirstOrDefault()
            }).ToList();
            var groupedResult = filteredResult
                          .GroupBy(x => new
                          {
                              x.ProjectName,
                              x.PoNumber,
                              x.ContractNumber,
                              x.DeliveryAnchor,
                              x.ContractId
                          })
                          .Select(g => new
                          {
                              Key = g.Key,
                              Value = g.Select(item => new ProvisionBillingApprovalDataDto
                              {
                                  ProjectName=item.ProjectName,
                                  PoNumber=item.PoNumber,   
                                  ContractNumber=item.ContractNumber,   
                                  DeliveryAnchor=item.DeliveryAnchor,   
                                  Contractbillingprovesionid = item.Contractbillingprovesionid,
                                  Contractemployeeid = item.Contractemployeeid,
                                  ResourceName = item.ResourceName,
                                  TmcId = item.TmcId,
                                  MonthYear = item.MonthYear,
                                  ProvisionAmount = item.ProvisionAmount,
                                  EstimatedBilligDate = item.EstimatedBilligDate,
                                  Status = item.Status,
                                  RemarkByDH = item.RemarkByDH,
                                  RemarkByBH = item.RemarkByBH
                              }).Distinct().ToList()
                          })
                          .ToList();
            return groupedResult;
        }
        public async Task<Contractbillingprovesion> Provision_delete(int contractbillingid)
        {
            try
            {
                var exixtingContractbilling = await _context.Contractbillingprovesions.AsNoTracking().FirstOrDefaultAsync(x => x.Contractbillingprovesionid == contractbillingid);

                if (exixtingContractbilling == null)
                {
                    throw new Exception($"Provision biling record is not exists at this ID : {contractbillingid}");
                }
                bool isApprovalStatusExists = await _context.Probillapprls.AsNoTracking().AnyAsync(x => x.Contractbillingprovesionid == contractbillingid);
                if (isApprovalStatusExists)
                {
                    throw new Exception($"Provision biling record can not be deleted sice it is in approval stage : {contractbillingid}");

                }
                _context.Contractbillingprovesions.RemoveRange(exixtingContractbilling);
                await _context.SaveChangesAsync();
                return exixtingContractbilling;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<Response> UpsertApproverAction_Provision(List<ApproverActionDto> Value, JwtLoginDetailDto logindetails)
        {
            string daname = "";
            string daemail = "";
            string practiceheademail = "";
            string practiceheadname = "";
            string projectname = "";
            string flag = "";
            int[] validStatusIds = { 4, 5 };
            try
            {
                var EmployeeId = _context.Rmsemployees.AsNoTracking().FirstOrDefault(x => x.Userid == logindetails.TmcId).Employeeid;
                var UserRole = _context.Employeeroles.AsNoTracking().Where(x => x.Employeeid == EmployeeId).FirstOrDefault();
                if (UserRole != null)
                {
                    if (UserRole.Roleid == 6 || UserRole.Roleid == 9)
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
                            var existingContractBillingData = await _context.Contractbillingprovesions
                                                                  .Include(c => c.Contractemployee)
                                                                  .ThenInclude(e => e.Contract)
                                                                  .ThenInclude(a => a.Project)
                                                                  .ThenInclude(q => q.Subpractice)
                                                                  .ThenInclude(p => p.Practice)
                                                                  .FirstOrDefaultAsync(x => x.Contractbillingprovesionid == existingApprovalData.Contractbillingprovesionid);

                            if (existingContractBillingData == null)
                            {
                                return new Response
                                {
                                    responseCode = StatusCodes.Status401Unauthorized,
                                    responseMessage = $"No Billing data found at BillingId {existingContractBillingData.Contractbillingprovesionid}"
                                };
                            }
                            //by nick for mail
                            //var practiceHead = await _context.Practiceheads
                          //      .Include(x => x.Practice)
                           //     .FirstOrDefaultAsync(x => x.Practiceid == existingContractBillingData.Contractemployee.Contract.Project.Subpractice.Practiceid);

                            var deliveryanchor = await _context.Rmsemployees
                                                 .Where(x => x.Employeeid == existingContractBillingData.Contractemployee.Contract.Deliveryanchorid)
                                                 .FirstOrDefaultAsync();
                                            

                            projectname = _context.Projects
                                .Where(a => a.Projectid == existingContractBillingData.Contractemployee.Contract.Project.Projectid)
                                .Select(a => a.Projectname)
                                .FirstOrDefault();
                            practiceheademail =  _context.Rmsemployees
                                                     .Where(a => a.Userid == logindetails.TmcId)
                                                     .Select(a => a.Companyemail)
                                                     .FirstOrDefault();
                               
                            daemail = _context.Rmsemployees
                                                     .Where(a => a.Employeeid == deliveryanchor.Employeeid)
                                                     .Select(a => a.Companyemail)
                                                     .FirstOrDefault();
                            daname = _context.Rmsemployees
                                                     .Where(a => a.Employeeid == deliveryanchor.Employeeid)
                                                     .Select(a => a.Employeename)
                                                     .FirstOrDefault();
                            practiceheadname = _context.Rmsemployees
                                                     .Where(a => a.Userid == logindetails.TmcId)
                                                     .Select(a => a.Employeename)
                                                     .FirstOrDefault();
                            existingApprovalData.Statusid = item.StatusId;
                            _context.Probillapprls.UpdateRange(existingApprovalData);

                            existingApprovalDetailsData.Isactiontaken = true;
                            existingApprovalDetailsData.Actiontakenon = DateTime.Now;
                            existingApprovalDetailsData.Remark = item.Remark != null ? item.Remark : null;
                            _context.Probillapprldetails.UpdateRange(existingApprovalDetailsData);

                            existingContractBillingData.Statusid = item.StatusId;
                            existingContractBillingData.Lastupdatedate = DateTime.Now;
                            existingContractBillingData.Lastupdateby = EmployeeId;
                            _context.Contractbillingprovesions.UpdateRange(existingContractBillingData);
                            if (item.StatusId == 5)
                                flag = "Approved";
                            else
                                flag = "Rejected";
                        }
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return new Response
                        {
                            responseCode = StatusCodes.Status200OK,
                            responseMessage = "Data sent Succesfully",
                            data = new
                            {
                                practiceheademail = practiceheademail,
                                daemail = daemail,
                                daname = daname,
                                practiceheadname = practiceheadname,
                                projectname = projectname,
                                flag = flag

                            }
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

        public async Task<Response> SwapFromProvisionToActual(List<SwappingRequestDto> values, JwtLoginDetailDto loginDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            int actualBillinId = 0;
            decimal? actualCosting=null;
            DateTime? actualDate = null;
            try
            {
                if (values == null || values.Count == 0)
                {
                    throw new ArgumentException("The input data must not be null or empty.");
                }

                var employee = await _context.Rmsemployees.FirstOrDefaultAsync(x => x.Userid == loginDetails.TmcId);
                if (employee == null)
                {
                    throw new InvalidOperationException("Employee not found.");
                }

                foreach (var item in values)
                {
                    if (item.contrtactbillingprovesionid == null || item.contrtactbillingprovesionid <= 0)
                    {
                        throw new ArgumentException("Contractbillingprovesionid must not be null or less than or equal to zero.");
                    }

                    var existingProvisionBilling = await _context.Contractbillingprovesions
                        .Include(ce => ce.Contractemployee)
                        .Include(approval => approval.Probillapprls)
                            .ThenInclude(detail => detail.Probillapprldetails)
                        .FirstOrDefaultAsync(x => x.Contractbillingprovesionid == item.contrtactbillingprovesionid && x.Billingmonthyear == item.BillingMonthyear);

                    if (existingProvisionBilling != null)
                    {
                        existingProvisionBilling.Costing = (item.costing >0 && item.costing != null) ? 0:existingProvisionBilling.Costing;
                        existingProvisionBilling.Isswaped = true;
                        existingProvisionBilling.Swapingdate = item.billingdate ?? DateTime.Now;
                        _context.Contractbillingprovesions.UpdateRange(existingProvisionBilling);

                        if (existingProvisionBilling.Probillapprls != null)
                        {
                            foreach (var approval in existingProvisionBilling.Probillapprls)
                            {
                                approval.Isdeleted = true;
                                approval.Lastupdatedate = DateTime.Now;
                                approval.Lastupdateby = employee.Employeeid;
                                _context.Probillapprls.UpdateRange(approval);
                            }
                        }

                        var existingProvisionHistory = await _context.Contractbillingprovisionhistories
                            .Where(x => x.Contractbillingprovesionid == item.contrtactbillingprovesionid && x.Statusid == 2)
                            .ToListAsync();

                        foreach (var historyItem in existingProvisionHistory)
                        {
                            historyItem.Isdeleted = true;
                            historyItem.Lastupdateby = employee.Employeeid;
                            historyItem.Lastupdatedate = DateTime.Now;
                            _context.Contractbillingprovisionhistories.UpdateRange(historyItem);
                        }

                        if (existingProvisionBilling.Contractemployee != null)
                        {
                            var existingActualBilling = await _context.Contractbillings
                                                       .FirstOrDefaultAsync(x => x.Contractemployeeid == existingProvisionBilling.Contractemployee.Contractemployeeid && x.Billingmonthyear == item.BillingMonthyear);

                            if (existingActualBilling != null)
                            {
                                
                                

                                existingActualBilling.Costing = (item.costing > 0 && item.costing != null) ? item.costing : existingActualBilling.Costing;
                                existingActualBilling.Isfromprovision = true;
                                existingActualBilling.Swapingdate = DateTime.Now;
                                _context.Contractbillings.UpdateRange(existingActualBilling);

                                actualDate = existingActualBilling.Estimatedbillingdate;
                                actualBillinId =existingActualBilling.Contractbillingid;
                                actualCosting = (item.costing > 0 && item.costing != null) ? item.costing : existingActualBilling.Costing;
                            }
                            else
                            {
                                var newActualBilling = new Contractbilling
                                {
                                    Contractemployeeid = existingProvisionBilling.Contractemployee.Contractemployeeid,
                                    Billingmonthyear = item.BillingMonthyear,
                                    Costing = item.costing,
                                    Isfromprovision = true,
                                    Swapingdate = DateTime.Now,
                                    Createddate = DateTime.Now,
                                    Createdby = employee.Employeeid
                                };
                                _context.Contractbillings.AddRange(newActualBilling);
                                actualBillinId = newActualBilling.Contractbillingid;
                                
                            }
                        }
                        
                    }
                    var swaphistory = new ContractbillingprovesionToContractbillingHistory
                    {
                        Contractbillingprovesionid = existingProvisionBilling.Contractbillingprovesionid,
                        Contractbillingid = actualBillinId,
                        Contractemployeeid = existingProvisionBilling.Contractemployee.Contractemployeeid,
                        Billingmonthyear = item.BillingMonthyear,
                        ProvisionCosting = existingProvisionBilling.Costing,
                        ActualCosting = actualCosting,
                        ProvisionEstimatedbillingdate = existingProvisionBilling.EstimatedBillingDate,
                        ActualEstimatedbillingdate = actualDate
                    };
                    _context.ContractbillingprovesionToContractbillingHistories.AddRange(swaphistory);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response { responseCode = 200, responseMessage = "Completed Successfully!" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("An exception occurred while updating data.", ex);
            }
        }


    }


}
