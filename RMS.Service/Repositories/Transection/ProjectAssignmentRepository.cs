using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection;

namespace RMS.Service.Repositories.Master
{
    public class ProjectAssignmentRepository : GenericRepository<Projectemployeeassignment>, IProjectAssignmentRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        public ProjectAssignmentRepository(RmsDevContext context,IHostingEnvironment env) : base(context)
        {
            _context = context;
            _env = env;

        }






        public async Task<Response> AssignEmployeesToProjectAsync(ProjectemployeeassignmentRequestDto requestDto, JwtLoginDetailDto loginDetails)
        {
            if (requestDto == null)
            {
                throw new ArgumentNullException(nameof(requestDto), "The request object cannot be null.");
            }

            try
            {
                var CreatedBy = await _context.Rmsemployees.Where(x => x.Userid == loginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync();

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    if (requestDto.ProjectWithStatus != null && requestDto.ProjectWithStatus.Any())
                    {
                        var currentDate = DateTime.Now.Date;

                        foreach (var project in requestDto.ProjectWithStatus)
                        {
                            var projectContractDetail = await _context.Projectcontracts.AsNoTracking().Where(c => c.Projectid == project.Projectid
                                                                          && ((c.Contractstartdate <= currentDate && c.Contractenddate >= currentDate) || c.Contractstartdate > currentDate))
                                                                         .FirstOrDefaultAsync();

                            if (projectContractDetail == null)
                            {
                                return new Response { responseCode = 404, responseMessage = $"Please create a new contract for the projectid {project.Projectid} first." };
                            }

                            var contractId = projectContractDetail.Contractid;

                            var existingContractEmployee = await _context.Contractemployees.AsNoTracking()
                                                                            .FirstOrDefaultAsync(c => c.Employeeid == requestDto.Employeeid && c.Contractid == contractId);

                            if (existingContractEmployee == null)
                            {
                                await _context.Contractemployees.AddAsync(new Contractemployee
                                {
                                    Contractid = contractId,
                                    Employeeid = requestDto.Employeeid,
                                    Categorysubstatusid = project.Categorysubstatusid,
                                    Lastupdatedby = CreatedBy,
                                    Lastupdateon = DateTime.Now,
                                    Createdby= CreatedBy,
                                    Createddate = DateTime.Now,
                                });
                                //await _context.Contractemployees.AddRangeAsync(existingContractEmployee);   
                            }
                            else
                            {
                                existingContractEmployee.Categorysubstatusid = project.Categorysubstatusid;
                                existingContractEmployee.Lastupdatedby = CreatedBy;
                                existingContractEmployee.Lastupdateon = DateTime.Now;
                                _context.Contractemployees.UpdateRange(existingContractEmployee);
                            }
                        }

                        var allStatus = requestDto.ProjectWithStatus.Select(c => c.Categorysubstatusid).ToList();

                        var empCategorySubStatusId = GetEmployeeCategorySubStatusId(allStatus);

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        await RemoveContractEmployeesAsync(requestDto.Employeeid);
                    }

                    await UpdateEmployeeStatusAsync(requestDto.Employeeid, requestDto.CategorySubStatusId, CreatedBy);

                    await transaction.CommitAsync();

                    return new Response { responseCode = 200, responseMessage = "Data processed successfully." };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new Response { responseCode = 500, responseMessage = "An error occurred while processing the request." };
            }
        }

        private int GetEmployeeCategorySubStatusId(List<int> allStatus)
        {
            if (allStatus.Count == 1)
                return allStatus[0];

            if (allStatus.Contains(5) || allStatus.Contains(3) || allStatus.Contains(2) || allStatus.Contains(1))
                return 5;

            if (allStatus.Contains(4))
                return 4;

            return 0;
        }

        private async Task RemoveContractEmployeesAsync(int employeeId)
        {
            var contractEmployees = await _context.Contractemployees.AsNoTracking()
                .Where(c => c.Employeeid == employeeId)
                .Include(x => x.Contractbillings)
                .ToListAsync();

            foreach (var item in contractEmployees.ToList())
            {
                if (item.Contractbillings == null || item.Contractbillings.Count() == 0)
                {
                    _context.Contractemployees.Remove(item);
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateEmployeeStatusAsync(int employeeId, int categorySubStatusId, int lastUpdatedBy)
        {
            var empObj = await _context.Rmsemployees.Where(x => x.Employeeid == employeeId).FirstOrDefaultAsync();

            if (empObj != null)
            {
                var contractEmployees = await _context.Contractemployees.AsNoTracking()
                    .Where(c => c.Employeeid == employeeId)
                    .Include(x => x.Contractbillings)
                    .ToListAsync();

                empObj.Categorysubstatusid = categorySubStatusId > 0 ? categorySubStatusId : 5;
                if (empObj.Categorysubstatusid == 0 && contractEmployees.Where(x => x.Contractbillings.Count() > 0).Count() == 0)
                {
                    empObj.Categorysubstatusid = 10;
                }

                empObj.Lastupdatedby = lastUpdatedBy;
                empObj.Lastupdatedon = DateOnly.FromDateTime(DateTime.Now);

                _context.Rmsemployees.Update(empObj);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Employee not found.");
            }
        }



        public async Task<Response> AssignEmployee(EmployeeAssignDto requestDto, JwtLoginDetailDto loginDetails)
        {
            if (requestDto == null)
            {
                throw new ArgumentNullException(nameof(requestDto), "The request object cannot be null.");
            }
            var CreatedBy = await _context.Rmsemployees.AsNoTracking()
                    .Where(x => x.Userid == loginDetails.TmcId)
                    .Select(x => x.Employeeid)
                    .FirstOrDefaultAsync();
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (requestDto.ProjectWithStatus != null && requestDto.ProjectWithStatus.Count > 0)
                {
                    foreach (var project in requestDto.ProjectWithStatus)
                    {
                        var contractId = requestDto.contractId;
                        var currentDate = DateTime.Now.Date; // Get the current date without time
                         var projectContractDetail = await _context.Projectcontracts.AsNoTracking()
                          .Where(c => c.Projectid == project.Projectid
                                  && ((c.Contractstartdate <= currentDate && c.Contractenddate >= currentDate) || c.Contractstartdate > currentDate))
                          .FirstOrDefaultAsync();
                        if (contractId > 0)
                        {
                             projectContractDetail = await _context.Projectcontracts.AsNoTracking()
                                                      .Where(c => c.Contractid == contractId
                                                              && ((c.Contractstartdate <= currentDate && c.Contractenddate >= currentDate) || c.Contractstartdate > currentDate))
                                                      .FirstOrDefaultAsync();

                        }
                       
                        var contractemployeeid = await _context.Contractemployees.AsNoTracking().Where(x => x.Contractid == contractId).FirstOrDefaultAsync();
                        if (projectContractDetail == null)
                        {
                            return new Response { responseCode = 404, responseMessage = "Please create a contract to assign project resource." };
                           
                        }
                        else
                        {
                            contractId = projectContractDetail.Contractid;
                            contractemployeeid = await _context.Contractemployees.AsNoTracking().Where(x => x.Contractid == contractId).FirstOrDefaultAsync();
                        }

                        if (contractId != 0)
                        {
                            if (requestDto.Employees.Count > 0)
                            {
                                foreach (var item in requestDto.Employees)
                                {
                                    await _context.Contractemployees.AddAsync(new Contractemployee
                                    {
                                        Contractid = contractId,
                                        Employeeid = item.Employeeid,
                                        Categorysubstatusid = item.CategorySubStatusId,
                                        Lastupdatedby = CreatedBy,
                                        Lastupdateon = DateTime.Now,
                                        Createdby = CreatedBy,  
                                        Createddate = DateTime.Now, 
                                    });
                                    var empObj = await _context.Rmsemployees.AsNoTracking().Where(x => x.Employeeid == item.Employeeid).FirstOrDefaultAsync();
                                    if (empObj != null)
                                    {
                                        var contractEmployees = await _context.Contractemployees.AsNoTracking().Where(c => c.Employeeid == item.Employeeid)
                                                                                                .Include(x => x.Contractbillings).ToListAsync();
                                        empObj.Categorysubstatusid = item.CategorySubStatusId;
                                        empObj.Lastupdatedby = CreatedBy;
                                        empObj.Lastupdatedon = DateOnly.FromDateTime(DateTime.Now);

                                        _context.Rmsemployees.Update(empObj);
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        return new Response { responseCode = 404, responseMessage = "Employee not exists." };
                                    }

                                }
                            }
                            else
                            {
                                return new Response { responseCode = 404, responseMessage = "Please deploy at least one resource to the newly created contract, or you can choose to do it later." };
                            }
                        }
                        
                            await _context.SaveChangesAsync();
                        }

                    }

                    var allStatus = requestDto.ProjectWithStatus.Select(c => c.Categorysubstatusid).ToList();
      
                await transaction.CommitAsync();

                return new Response
                {
                    responseCode = 200,
                    responseMessage = "Data processed successfully."
                };
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Response> DeleteProjectEmployee(ProjectemployeeassignmentRequestDto requestDto, JwtLoginDetailDto loginDetails)
        {
            if (requestDto == null)
            {
                throw new ArgumentNullException(nameof(requestDto), "The request object cannot be null.");
            }

            var LastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
                .Where(x => x.Userid == loginDetails.TmcId)
                .Select(x => x.Employeeid)
                .FirstOrDefaultAsync();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                

                if (requestDto.ProjectWithStatus != null && requestDto.ProjectWithStatus.Any())
                {
                    var projectid = requestDto.ProjectWithStatus.FirstOrDefault()?.Projectid;

                    var contractEmployees = await _context.Contractemployees.AsNoTracking()
                     .Where(c => c.Employeeid == requestDto.Employeeid && c.Contract.Projectid == projectid)
                     .Include(Z => Z.Contract)
                     .Include(x => x.Contractbillings)
                     .ToListAsync();

                    // Now, filter the contractEmployees based on the contractno condition
                    contractEmployees = contractEmployees
                        .Where(ce => requestDto.ProjectWithStatus.Any(ps => ps.contractno == ce.Contract.Contractno))
                        .ToList();


                    foreach (var item in contractEmployees)
                    {
                        if (item.Contractbillings == null || !item.Contractbillings.Any())
                        {
                            item.Lastupdatedby = LastUpdatedBy;
                            item.Lastupdateon = DateTime.Now;
                            _context.Contractemployees.Remove(item); // Remove the individual entity
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            return new Response { responseCode = 404, responseMessage = "The presence of billing for the employee on this contract means that this project contract cannot be deleted." };
                        }
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return new Response
                {
                    responseCode = 200,
                    responseMessage = "Data processed successfully."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<Response> Changeemployeeprojectstatus(ProjectemployeeassignmentRequestDto requestDto, JwtLoginDetailDto loginDetails)
        {
            if (requestDto == null)
            {
                throw new ArgumentNullException(nameof(requestDto), "The request object cannot be null.");
            }

            var LastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
                .Where(x => x.Userid == loginDetails.TmcId)
                .Select(x => x.Employeeid)
                .FirstOrDefaultAsync();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var projectWithStatus in requestDto.ProjectWithStatus)
                {
                    var projectid = projectWithStatus.Projectid;
                    var contractno = projectWithStatus.contractno;
                    int empCategorySubStatusId = projectWithStatus.Categorysubstatusid;
                    var listofContrctemployeeIds=new List<int>();   

                    var contractEmployees = await _context.Contractemployees.AsNoTracking()
                        .Where(c => c.Employeeid == requestDto.Employeeid && c.Contract.Projectid == projectid && c.Contract.Contractno == contractno)
                        .Include(x => x.Contractbillings)
                        .Include (x => x.Contractbillingprovesions)
                        .Include (x => x.Contract)  
                        .ToListAsync();
                    listofContrctemployeeIds= contractEmployees.Select(x => x.Contractemployeeid).ToList(); 
                    foreach (var contractEmployee in contractEmployees)
                    {
                        // Update the existing Contractemployee
                        contractEmployee.Categorysubstatusid = empCategorySubStatusId;
                        contractEmployee.Lastupdatedby = LastUpdatedBy;
                        contractEmployee.Lastupdateon = DateTime.Now;
                        _context.Contractemployees.UpdateRange(contractEmployee);
                    }
                    var actualbillingItems= contractEmployees.Select(x=> x.Contractbillings).ToList();
                    var provisionbillingItems = contractEmployees.Select(x => x.Contractbillingprovesions).ToList();
                    if (empCategorySubStatusId==11 && (actualbillingItems != null && actualbillingItems[0].Count > 0) && listofContrctemployeeIds.Count>0)
                    {
                        var contractEndDate = contractEmployees.Select(x => x.Contract.Contractenddate.Value).FirstOrDefault();
                        var monthYearList = MonthYearSeries(DateTime.Now, contractEndDate);
                        var allcontractbillingforupdation=await _context.Contractbillings.Where(x=> monthYearList.Contains(x.Billingmonthyear) 
                                                                             && listofContrctemployeeIds.Contains(x.Contractemployeeid.Value)).ToListAsync();

                        if (allcontractbillingforupdation!=null || allcontractbillingforupdation.Count>0)
                        {
                            foreach (var billing in allcontractbillingforupdation)
                            {
                                billing.Costing = 0;
                            }
                        }
                        _context.Contractbillings.UpdateRange(allcontractbillingforupdation);

                    }
                    if (empCategorySubStatusId == 11 && (provisionbillingItems != null && provisionbillingItems[0].Count > 0) && listofContrctemployeeIds.Count > 0)
                    {
                        var contractEndDate = contractEmployees.Select(x => x.Contract.Contractenddate.Value).FirstOrDefault();
                        var monthYearList = MonthYearSeries(DateTime.Now, contractEndDate);
                        var allprovisionbillingforupdation = await _context.Contractbillingprovesions.Where(x => monthYearList.Contains(x.Billingmonthyear)
                                                                             && listofContrctemployeeIds.Contains(x.Contractemployeeid)).ToListAsync();

                        if (allprovisionbillingforupdation != null || allprovisionbillingforupdation.Count > 0)
                        {
                            foreach (var billing in allprovisionbillingforupdation)
                            {
                                billing.Costing = 0;
                            }
                        }
                        _context.Contractbillingprovesions.UpdateRange(allprovisionbillingforupdation);

                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new Response
                {
                    responseCode = 200,
                    responseMessage = "Data processed successfully."
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private List<string> MonthYearSeries(DateTime startDate,DateTime endDate)
        {
            List<string> monthYearList = new List<string>();
            DateTime nextMontStartDate= startDate.AddMonths(1);
            DateTime currentDate = new DateTime(nextMontStartDate.Year, nextMontStartDate.Month, 1);
            DateTime endDateOfMonth = new DateTime(endDate.Year, endDate.Month, 1);

            while (currentDate <= endDateOfMonth)
            {
                monthYearList.Add(currentDate.ToString("MMM-yy", CultureInfo.InvariantCulture));
                currentDate = currentDate.AddMonths(1);
            }

            return monthYearList;

        }


        //public async Task<Response> AssignEmployeesToProjectAsync(ProjectemployeeassignmentRequestDto requestDto, JwtLoginDetailDto loginDetails)
        //{
        //    if (requestDto == null)
        //    {
        //        throw new ArgumentNullException(nameof(requestDto), "The request object cannot be null.");
        //    }
        //    var LastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
        //            .Where(x => x.Userid == loginDetails.TmcId)
        //            .Select(x => x.Employeeid)
        //            .FirstOrDefaultAsync();

        //    await using var transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        ///code to check the contract exists against the project
        //        ///if not exists then add new contract and get contract id
        //        ///if exists then get contract id
        //        ///check if enteries exists in contractemployee table then delete it
        //        ///add new enteries in contract employees
        //        ///return
        //        ///

        //        int empCategorySubStatusId = requestDto.CategorySubStatusId;
        //        if (requestDto.ProjectWithStatus != null && requestDto.ProjectWithStatus.Count > 0)
        //        {
        //            var contractEmployees = await _context.Contractemployees.AsNoTracking().Where(c => c.Employeeid == requestDto.Employeeid)
        //                                                                     .Include(x => x.Contractbillings)
        //                                                                     .Include(c => c.Contract)
        //                                                                     .ThenInclude(a => a.Project)
        //                                                                     .ToListAsync();
        //            //var selectedData = await _context.Contractbillings
        //            //                                   .Include(x => x.Contractemployee).ThenInclude(x => x.Employee)
        //            //                                   .Include(x => x.Contractemployee).ThenInclude(x => x.Contract)
        //            //                                       .ThenInclude(x => x.Project).ThenInclude(x => x.Subpractice).ThenInclude(x => x.Practice)
        //            //                                   .Include(x => x.Contractemployee).ThenInclude(x => x.Contract)
        //            //                                       .ThenInclude(x => x.Project).ThenInclude(x => x.Customer)
        //            //                                   .Where(x => x.Contractemployee.Employeeid == requestDto.Employeeid)
        //            //                                   .ToListAsync();


        //            if (contractEmployees.Any())
        //            {
        //                var projectsToRemoveFromDto = new List<ProjectWithStatus>();

        //                foreach (var item in contractEmployees)
        //                {
        //                    foreach (var test in requestDto.ProjectWithStatus)
        //                    {
        //                        if (test.Projectid == item.Contract.Projectid && item.Contract.Contractno == test.contractno)
        //                        {
        //                            projectsToRemoveFromDto.Add(test);
        //                        }
        //                    }


        //                }

        //                // Remove the collected projects from requestDto.ProjectWithStatus
        //                foreach (var projectToRemove in projectsToRemoveFromDto)
        //                {
        //                    requestDto.ProjectWithStatus.Remove(projectToRemove);
        //                }


        //            }
        //            //if (contractEmployees.Any())
        //            //{

        //            //    foreach (var item in contractEmployees.ToList())
        //            //    {
        //            //        if (requestDto.ProjectWithStatus.All(a => a.Projectid != item.Contract.Projectid))
        //            //        {
        //            //            if (item.Contractbillings == null || item.Contractbillings.Count() == 0)
        //            //            {
        //            //                _context.Contractemployees.Remove(item); // Remove the individual entity
        //            //            }
        //            //        }
        //            //    }
        //            //    await _context.SaveChangesAsync();
        //            //}



        //            foreach (var project in requestDto.ProjectWithStatus)
        //            {
        //                var contractId = 0;
        //                var currentDate = DateTime.Now.Date; // Get the current date without time

        //                var projectContractDetail = await _context.Projectcontracts.AsNoTracking()
        //                    .Where(c => c.Projectid == project.Projectid
        //                            && ((c.Contractstartdate <= currentDate && c.Contractenddate >= currentDate) || c.Contractstartdate > currentDate))
        //                    .FirstOrDefaultAsync();

        //                var contractemployeeid = await _context.Contractemployees.Where(x => x.Contractid == contractId).FirstOrDefaultAsync();
        //                if (projectContractDetail == null)
        //                {
        //                    return new Response { responseCode = 404, responseMessage = "Please create a contract to assign project resource." };
        //                    //var pC = new Projectcontract();
        //                    //pC.Projectid = project.Projectid;
        //                    //pC.Ponumber = "[Default]";
        //                    //pC.Amount = 0;
        //                    //pC.Contactnumber = "[Default]";
        //                    //pC.Contactpersonname = "[Default]";
        //                    //pC.Contractstartdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //                    //pC.Contractenddate = new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddYears(1).Month, DateTime.Now.AddYears(1).Day);
        //                    //pC.Contractno = "[Default]";
        //                    //pC.Costsheetid = 0;
        //                    //pC.Deliveryanchorid = null;
        //                    //pC.Statusid = 12;
        //                    //await _context.Projectcontracts.AddAsync(pC);
        //                    //await _context.SaveChangesAsync();

        //                    //contractId = pC.Contractid;

        //                    //var pcL = new Contractline
        //                    //{
        //                    //    Contractid = contractId,
        //                    //    Linedescription1 = "[Default]",
        //                    //    Linedescription2 = "[Default]",
        //                    //    Lineamount = 0,
        //                    //    Lineno = "1"
        //                    //};

        //                    //await _context.Contractlines.AddAsync(pcL);
        //                    //await _context.SaveChangesAsync();
        //                }
        //                else
        //                {
        //                    contractId = projectContractDetail.Contractid;
        //                    contractemployeeid = await _context.Contractemployees.AsNoTracking().Where(x => x.Contractid == contractId).FirstOrDefaultAsync();
        //                }

        //                if (contractId != 0)
        //                {
        //                    var existingContractEmployee = await _context.Contractemployees.AsNoTracking()
        //                        .FirstOrDefaultAsync(c => c.Employeeid == requestDto.Employeeid && c.Contractid == contractId);

        //                    if (existingContractEmployee == null)
        //                    {
        //                        // Add a new Contractemployee
        //                        await _context.Contractemployees.AddAsync(new Contractemployee
        //                        {
        //                            Contractid = contractId,
        //                            Employeeid = requestDto.Employeeid,
        //                            Categorysubstatusid = project.Categorysubstatusid,
        //                            Lastupdatedby = LastUpdatedBy.ToString(),
        //                            Lastupdateon = DateTime.Now,
        //                        });
        //                    }
        //                    else
        //                    {
        //                        // Update the existing Contractemployee
        //                        existingContractEmployee.Categorysubstatusid = project.Categorysubstatusid;
        //                        existingContractEmployee.Lastupdatedby = LastUpdatedBy.ToString();
        //                        existingContractEmployee.Lastupdateon = DateTime.Now;
        //                    }

        //                    await _context.SaveChangesAsync();
        //                }

        //            }

        //            var allStatus = requestDto.ProjectWithStatus.Select(c => c.Categorysubstatusid).ToList();
        //            //1  Projection
        //            //2  Shadow
        //            //3  Allocation
        //            //4  Over Run
        //            //5  Deployed

        //            if (allStatus.Count == 1)
        //                empCategorySubStatusId = allStatus[0];
        //            else
        //            {
        //                if (allStatus.Contains(5) || allStatus.Contains(3) || allStatus.Contains(2) || allStatus.Contains(1))
        //                    empCategorySubStatusId = 5;
        //                else if (allStatus.Contains(4))
        //                    empCategorySubStatusId = 4;
        //            }
        //        }
        //        else
        //        {
        //            var contractEmployees = await _context.Contractemployees.AsNoTracking().Where(c => c.Employeeid == requestDto.Employeeid)
        //                                                                     .Include(x => x.Contractbillings).ToListAsync();
        //            if (contractEmployees.Any())
        //            {
        //                foreach (var item in contractEmployees.ToList())
        //                {
        //                    // Check the condition for removal
        //                    if (item.Contractbillings == null || item.Contractbillings.Count() == 0)
        //                    {
        //                        _context.Contractemployees.Remove(item); // Remove the individual entity
        //                    }
        //                }


        //                await _context.SaveChangesAsync();
        //            }
        //        }
        //        //change the employee status
        //        var empObj = await _context.Rmsemployees.Where(x => x.Employeeid == requestDto.Employeeid).FirstOrDefaultAsync();
        //        if (empObj != null)
        //        {
        //            var contractEmployees = await _context.Contractemployees.AsNoTracking().Where(c => c.Employeeid == requestDto.Employeeid)
        //                                                                    .Include(x => x.Contractbillings).ToListAsync();
        //            empObj.Categorysubstatusid = empCategorySubStatusId;
        //            if (empObj.Categorysubstatusid == 0 && contractEmployees.Where(x => x.Contractbillings.Count() > 0).Count() == 0)
        //            {
        //                empObj.Categorysubstatusid = 10;
        //            }
        //            else
        //            {
        //                if (requestDto.CategorySubStatusId > 0)
        //                {
        //                    empObj.Categorysubstatusid = requestDto.CategorySubStatusId;
        //                    empObj.Lastupdatedby = LastUpdatedBy;
        //                    empObj.Lastupdatedon = DateOnly.FromDateTime(DateTime.Now);

        //                }
        //                else
        //                {
        //                    empObj.Categorysubstatusid = 5;
        //                    empObj.Lastupdatedby = LastUpdatedBy;
        //                    empObj.Lastupdatedon = DateOnly.FromDateTime(DateTime.Now);
        //                }
        //            }
        //            _context.Rmsemployees.Update(empObj);
        //            await _context.SaveChangesAsync();
        //        }
        //        else
        //        {
        //            return new Response { responseCode = 404, responseMessage = "Employee not exists." };
        //        }

        //        //////////////var allExistingAssignments = _context.Projectemployeeassignments
        //        //////////////                                     .Where(assignment => assignment.Employeeid == requestDto.Employeeid);

        //        //////////////if (allExistingAssignments.Any())
        //        //////////////{
        //        //////////////    _context.Projectemployeeassignments.RemoveRange(allExistingAssignments);                 
        //        //////////////    await _context.SaveChangesAsync();
        //        //////////////}


        //        //////////////var creatorEmployeeId = await _context.Rmsemployees
        //        //////////////                                      .Where(x => x.Userid == loginDetails.TmcId)
        //        //////////////                                      .Select(x => x.Employeeid)
        //        //////////////                                      .FirstOrDefaultAsync();


        //        //////////////foreach (var item in requestDto.ProjectWithStatus)
        //        //////////////{
        //        //////////////    var newAssignment = new Projectemployeeassignment
        //        //////////////    {
        //        //////////////        Employeeid = requestDto.Employeeid,
        //        //////////////        Projectid = item.Projectid,
        //        //////////////        Categorysubstatusid = item.Categorysubstatusid,
        //        //////////////        Createdby = creatorEmployeeId,
        //        //////////////        Createddate = DateTime.Now
        //        //////////////    };

        //        //////////////    await _context.Projectemployeeassignments.AddAsync(newAssignment);
        //        //////////////}

        //        //////////////await _context.SaveChangesAsync();


        //        await transaction.CommitAsync();

        //        return new Response
        //        {
        //            responseCode = 200,
        //            responseMessage = "Data processed successfully."
        //        };
        //    }
        //    catch (Exception ex)
        //    {

        //        await transaction.RollbackAsync();
        //        throw;
        //    }
        //}




    }
}
