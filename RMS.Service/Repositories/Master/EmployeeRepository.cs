using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RMS.Service.Repositories.Master
{
    public partial class EmployeeRepository : GenericRepository<Rmsemployee>, IEmployeeRepository
    {
        private readonly RmsDevContext _context;
        private readonly ILogger<EmployeeRepository> _logger;
        //1426, 7123, 10376, 14449, 15166, 16349, 17423, 17473, 17478
        string[] emp = { "13150", "16918", "3844", "15323", "18286", "8571", "16196", "14014", "13369",
                "14010", "17160", "18085","18247","4850","7112","7373","8018","11823","11881","3543","TA0028",
                "12214","14986","16171","16208"   };
        public EmployeeRepository(RmsDevContext context, ILogger<EmployeeRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> UpsertEmployeesOnebyOne(List<EmployeAPIDarwinDto> employeeList)
        {
            bool flag = false;
            //using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            //{
            try
            {
                foreach (var employee in employeeList)
                {
                    var sbuid = await UpsertSbu(employee);
                    var branchid = await UpsertBranch(employee);
                    var deisgnationid = await UpsertDesignation(employee);
                    var departmentid = await UpsertDepartment(employee);
                    var existingEmployee = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(e => e.Userid == employee.EmployeeId);

                    if (existingEmployee == null)
                    {
                        // Employee doesn't exist, insert it
                        var emp = new Rmsemployee
                        {
                            Userid = employee.EmployeeId,
                            Employeename = employee.EmployeeName,
                            Companyemail = employee.CompanyEmailId,
                            Sbu = employee.Sbu,
                            Contactno = employee.OfficeMobileNo,
                            Dateofjoining = employee.JoiningDate,
                            Designationid = deisgnationid,
                            Reportheadid = employee.ReportingHeadId,
                            Departmentid = departmentid,
                            Sbuid = sbuid,
                            Branchid = branchid,
                        };
                        _context.Rmsemployees.Add(emp);
                    }
                    else
                    {
                        existingEmployee.Userid = employee.EmployeeId;
                        existingEmployee.Employeename = employee.EmployeeName;
                        existingEmployee.Companyemail = employee.CompanyEmailId;
                        existingEmployee.Sbu = employee.Sbu;
                        existingEmployee.Contactno = employee.OfficeMobileNo;
                        existingEmployee.Dateofjoining = employee.JoiningDate;
                        existingEmployee.Designationid = deisgnationid;
                        existingEmployee.Reportheadid = employee.ReportingHeadId;
                        existingEmployee.Departmentid = departmentid;
                        existingEmployee.Sbuid = sbuid;
                        existingEmployee.Branchid = branchid;
                        _context.Rmsemployees.Update(existingEmployee);
                    }
                    await _context.SaveChangesAsync();
                }
                // Commit changes to the database
                // await _context.SaveChangesAsync();
                //    transaction.Commit();
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                return false;
            }

            return flag;
        }
        private async Task<int> UpsertSbu(EmployeAPIDarwinDto employee)
        {
            var existingsbu = await _context.Sbus.AsNoTracking().FirstOrDefaultAsync(e => e.Sbudesc == employee.Sbu && e.Sbucode == employee.SbuCode);

            if (existingsbu == null)
            {
                Sbu newSbu = new Sbu
                {
                    Sbudesc = employee.Sbu,
                    Sbucode = employee.SbuCode
                    // set other properties if needed
                };

                _context.Sbus.Add(newSbu);
                await _context.SaveChangesAsync();
                return newSbu.Sbuid;  // This will return the ID of the newly inserted record.
            }
            else
            {

                return existingsbu.Sbuid;
            }
        }
        private async Task<int> UpsertBranch(EmployeAPIDarwinDto employee)
        {
            string base_office_location = employee.BaseOfficeLocation;
            string branch_code = employee.BranchCode;
            if (string.IsNullOrEmpty(branch_code))
            {
                Match match = Regex.Match(base_office_location, @"\((\d+)\)$");
                if (match.Success)
                {
                    branch_code = match.Groups[1].Value;
                }
            }
            var existingBranch = await _context.Branches.AsNoTracking().FirstOrDefaultAsync(e => e.Branchname == base_office_location && e.Branchcode == branch_code);

            if (existingBranch == null)
            {
                Branch newBranch = new Branch
                {
                    Branchname = base_office_location,
                    Branchcode = branch_code
                    // set other properties if needed
                };

                _context.Branches.Add(newBranch);
                await _context.SaveChangesAsync();
                return newBranch.Branchid;  // This will return the ID of the newly inserted record.
            }
            else
            {

                return existingBranch.Branchid;
            }
        }
        private async Task<int> UpsertDesignation(EmployeAPIDarwinDto employee)
        {
            var existingDesignation = await _context.Designations.AsNoTracking().FirstOrDefaultAsync(e => e.Designationname == employee.Designation);

            if (existingDesignation == null)
            {
                Designation newDesignation = new Designation
                {
                    Designationname = employee.Designation,

                };

                _context.Designations.Add(newDesignation);
                await _context.SaveChangesAsync();
                return newDesignation.Designationid;  // This will return the ID of the newly inserted record.
            }
            else
            {

                return existingDesignation.Designationid;
            }
        }
        private async Task<int> UpsertDepartment(EmployeAPIDarwinDto employee)
        {
            var existingDepartment = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(e => e.Departmentname == employee.Department);

            if (existingDepartment == null)
            {
                Department newDepartment = new Department
                {
                    Departmentname = employee.Department,

                };

                _context.Departments.Add(newDepartment);
                await _context.SaveChangesAsync();
                return newDepartment.Departmentid;  // This will return the ID of the newly inserted record.
            }
            else
            {

                return existingDepartment.Departmentid;
            }
        }
        public async Task<Rmsemployee> GetByName(string name)
        {
            return await _context.Rmsemployees.AsNoTracking().Where(c => c.Employeename == name).FirstOrDefaultAsync();
        }
        public async Task<Rmsemployee> GetByTmc(string tmc)
        {
            return await _context.Rmsemployees.AsNoTracking().Include(empRole => empRole.Employeeroles).ThenInclude(role => role.Role)
                                              .Where(c => c.Userid == tmc).FirstOrDefaultAsync();
        }
        //public async Task<bool> UpsertEmployeesBulkData(List<EmployeAPIDarwinDtoJson> employeeList)
        //{
        //    try
        //    {
        //       // employeeList= employeeList.Where(x=>x.employee_id!= "17489").ToList();   
        //        // Serialize each individual employee to a list of JSON strings
        //        List<string> individualJsons = employeeList.Select(emp => JsonSerializer.Serialize(emp)).ToList();

        //        // Call the stored procedure
        //        int result = await _context.Database.ExecuteSqlRawAsync("CALL upsertemployees(@data)",
        //             new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individualJsons });

        //        // calling for role assignmeent and the defalut role will be enmployee 
        //        int roleResult = await _context.Database.ExecuteSqlRawAsync("CALL updaterole()");

        //        //settig the employees billing zero if he resigns 
        //        int setBilligResult= await _context.Database.ExecuteSqlRawAsync("CALL deleting_resigned_employees_billing()");



        //        return result > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //}

        public async Task<bool> UpsertEmployeesBulkData(List<EmployeAPIDarwinDtoJson> employeeList)
        {
            //employeeList= employeeList.Where(x=>x.employee_id!= "15888").ToList();

            var startTime = DateTime.UtcNow;
            var stopwatch = Stopwatch.StartNew();
            _logger.LogInformation($"--------------------------------------{startTime}--------------------------------------------------");
            _logger.LogInformation($"--------------------------------------{startTime}--------------------------------------------------");

            _logger.LogInformation("UpsertEmployeesBulkData started at {StartTime}", startTime);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Serialize each individual employee to a list of JSON strings
                List<string> individualJsons = employeeList.Select(emp => JsonSerializer.Serialize(emp)).ToList();
                _logger.LogInformation("Serialized {EmployeeCount} employees to JSON.", employeeList.Count);

                // Log start of upsertemployees stored procedure call
                _logger.LogInformation("Calling stored procedure 'upsertemployees' with {EmployeeCount} employees.", employeeList.Count);

                // Call the stored procedure
                int upsertResult = await _context.Database.ExecuteSqlRawAsync("CALL upsertemployees(@data)",
                    new NpgsqlParameter("data", NpgsqlDbType.Array | NpgsqlDbType.Jsonb) { Value = individualJsons });

                // Log result of upsertemployees stored procedure call
                _logger.LogInformation("Stored procedure 'upsertemployees' completed with result {Result}", upsertResult);

                // Call the stored procedure for updating roles
                _logger.LogInformation("Calling stored procedure 'updaterole'...");
                int roleResult = await _context.Database.ExecuteSqlRawAsync("CALL updaterole()");
                _logger.LogInformation("Stored procedure 'updaterole' completed with result {RoleResult}", roleResult);

                // Call the stored procedure for setting billing to zero for resigned employees
                _logger.LogInformation("Calling stored procedure 'deleting_resigned_employees_billing'...");
                int billingResult = await _context.Database.ExecuteSqlRawAsync("CALL deleting_resigned_employees_billing()");
                _logger.LogInformation("Stored procedure 'deleting_resigned_employees_billing' completed with result {BillingResult}", billingResult);

                // Commit the transaction if all procedures succeed
                await transaction.CommitAsync();

                stopwatch.Stop();
                _logger.LogInformation("UpsertEmployeesBulkData executed successfully in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

                _logger.LogInformation($"--------------------------------------{startTime}--------------------------------------------------");
                _logger.LogInformation($"--------------------------------------{startTime}--------------------------------------------------");

                return upsertResult > 0;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                await transaction.RollbackAsync();

                _logger.LogError(ex, "An error occurred while executing UpsertEmployeesBulkData at {Time}. Duration: {ElapsedMilliseconds} ms", DateTime.UtcNow, stopwatch.ElapsedMilliseconds);

                return false;
            }
        }
        public async Task<List<employeeResponseDto>> GetEmployeesWithDetails()
        {
            string currentMonth = DateTime.Now.ToString("MMM-yy");
            try
            {
                var employees = await _context.Rmsemployees.AsNoTracking()
                    .Where(e =>
                        (
                            (e.Departmentid == 22 && e.Sbuid == 1)
                            || (e.Departmentid == 6 && e.Sbuid == 55)
                            ||(e.Departmentid == 22 && e.Sbuid == 55)
                        )
                        && !emp.Contains(e.Userid)
                        && e.Isactive == true
                        && e.Udf3 != "2"
                    )
                    .Include(x => x.Categorysubstatus).ThenInclude(x => x.Categorystatus)
                    .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
                    .Include(c => c.Department)
                    .Include(c => c.Branch)
                    .Include(e => e.Contractemployees)
                        .ThenInclude(pea => pea.Contract)
                            .ThenInclude(c => c.Project)
                                .ThenInclude(p => p.Customer)
                    .Include(e => e.Contractemployees)
                        .ThenInclude(pea => pea.Contract)
                            .ThenInclude(c => c.Project)
                                .ThenInclude(p => p.Subpractice)
                                    .ThenInclude(sp => sp.Practice)
                    .Include(e => e.Contractemployees)
                        .ThenInclude(pea => pea.Categorysubstatus)
                            .ThenInclude(s => s.Categorystatus)
                    .Include(e => e.Contractemployees)
                        .ThenInclude(pea => pea.Contract)
                            .ThenInclude(c => c.Project)
                                .ThenInclude(p => p.Projecttype)
                    .Include(e => e.Contractemployees).ThenInclude(pea => pea.Contract)
                        .ThenInclude(c => c.Deliveryanchor)
                    .Include(e => e.Contractemployees)
                        .ThenInclude(pea => pea.Contractbillings)
                    .ToListAsync();

                // Separate query to retrieve reporting heads
                var reportingHeads = await _context.Rmsemployees.AsNoTracking().ToListAsync();

                var response = employees.Select(e => new employeeResponseDto
                {
                    Employeeid = e.Employeeid,
                    Experience = e.Dateofjoining == null
                        ? "Experienced"
                        : ExperienceCalculation(DateTime.Parse(e.Dateofjoining), e.Workexedays, e.Ade),
                    TmcId = e.Userid,
                    Udf3 = e.Udf3 ?? "",
                    Employeename = e.Employeename ?? "",
                    email = e.Companyemail ?? "",
                    Contactno = e.Contactno ?? "",
                    Department = e.Department?.Departmentname ?? "",
                    Branch = e.Branch?.Branchname ?? "",
                    Dateofbirth = e.Dateofbirth ?? "",
                    Dateofjoining = e.Dateofjoining ?? "",
                    Reporthead = e.Reportheadid != null ? reportingHeads.FirstOrDefault(rh => rh.Userid == e.Reportheadid)?.Employeename : "",
                    PracticeName = e.Subpractice == null ? "" : e.Subpractice.Practice.Practicename,
                    SubPracticeName = e.Subpractice == null ? "" : e.Subpractice.Subpracticename,
                    costcenter = e.Costcenter ?? "",
                    CategorySubStatusName = GetStatusFromCollection(e.Contractemployees),
                    CategoryStatusName = GetStatusFromCollection(e.Contractemployees),
                    employeeProjects = e.Contractemployees?.Select(ce => new employeeProject
                    {
                        Projectid = ce.Contract?.Project.Projectid ?? 0,
                        Projectname = ce.Contract?.Project.Projectname ?? "",
                        Categorysubstatusid = ce.Categorysubstatus?.Categorysubstatusid ?? 0,
                        Categorysubstatusname = (ce.Contract.Contractenddate >= DateTime.Now && ce.Categorysubstatusid != 6)
                            ? ce.Categorysubstatus?.Categorysubstatusname
                            : (ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid != 6) ? "Over Run"
                            : (ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid == 6) ? "Bench"
                            : ce.Categorysubstatus?.Categorysubstatusname ?? "",
                        Categorystatusname = (ce.Contract.Contractenddate >= DateTime.Now && ce.Categorysubstatusid != 6)
                            ? ce.Categorysubstatus?.Categorysubstatusname
                            : (ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid != 6) ? "Over Run"
                            : (ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid == 6) ? "Bench"
                            : ce.Categorysubstatus?.Categorysubstatusname ?? "",
                        CustomerCompanyname = ce.Contract?.Project.Customer?.Companyname ?? "",
                        Projecttypename = ce.Contract?.Project.Projecttype?.Projecttypename ?? "",
                        Subpracticename = ce.Contract?.Project.Subpractice?.Subpracticename ?? "",
                        Practicename = ce.Contract?.Project.Subpractice?.Practice?.Practicename ?? "",
                        BillingCosting = e.Contractemployees?.SelectMany(ce => ce.Contractbillings
                                            .Where(cb => cb.Billingmonthyear == currentMonth)
                                            .Select(ings => ings.Costing))
                                            .FirstOrDefault() ?? 0,
                        Contractbillings = ce.Contractbillings.Where(x => x.Billingmonthyear == currentMonth)?.Select(cb => new ContractbillingInfo
                        {
                            Contractbillingid = cb.Contractbillingid,
                            Billingmonthyear = cb.Billingmonthyear,
                            Costing = cb.Costing
                        }).ToList() ?? new List<ContractbillingInfo>(),
                        Billingmonthyear = e.Contractemployees?.SelectMany(ce => ce.Contractbillings
                                            .Where(cb => cb.Billingmonthyear == currentMonth)
                                            .Select(ings => ings.Billingmonthyear))
                                            .FirstOrDefault() ?? currentMonth,
                        DeliveryAnchorName = e.Contractemployees?.Select(ce => ce.Contract?.Deliveryanchor?.Employeename)
                                            .FirstOrDefault() ?? "",
                        contracts = ce.Contract?.Contractemployees.Select(c => new Contractinfo
                        {
                            Contractid = c.Contract.Contractid,
                            ContractNo = c.Contract.Contractno,
                            DeliveryAnchorName = c.Contract.Deliveryanchor?.Employeename ?? "",
                            Ponumber = c.Contract.Ponumber,
                            Contractstartdate = c.Contract.Contractstartdate,
                            Contractenddate = c.Contract.Contractenddate,
                            Amount = c.Contract.Amount,
                            Contactpersonname = c.Contract.Contactpersonname,
                            Deliveryanchorid = c.Contract.Deliveryanchorid,
                            Povalue = c.Contract.Povalue ?? 0,
                            Contractemployees = c.Contract.Contractemployees?.Select(ce => new ContractemployeeInfo
                            {
                                Contractemployeeid = ce.Contractemployeeid,
                                Categorysubstatusid = ce.Categorysubstatusid,
                                Contractbillings = ce.Contractbillings?.Select(cb => new ContractbillingInfo
                                {
                                    Contractbillingid = cb.Contractbillingid,
                                    Billingmonthyear = cb.Billingmonthyear,
                                    Costing = cb.Costing
                                }).ToList() ?? new List<ContractbillingInfo>()
                            }).ToList() ?? new List<ContractemployeeInfo>()
                        }).ToList() ?? new List<Contractinfo>()
                    }).ToList() ?? new List<employeeProject>(),
                    Role = e.Employeeroles == null ? null : e.Employeeroles.Select(x => new employeeRole
                    {
                        Role = x.Role.Rolename,
                        RoleId = x.Role.Roleid,
                    }).ToList(),
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<employeeResponseDto> GetEmployeesByIdWithDetails(int EmployeeId)
        {
            string currentMonth = DateTime.Now.ToString("MMM-yy");
            try
            {
                var e = await _context.Rmsemployees.AsNoTracking()
                                    .Include(x => x.Categorysubstatus).ThenInclude(x => x.Categorystatus)
                                   .Include(r => r.Employeeroles).ThenInclude(r => r.Role)
                                   .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
                                   .Include(c => c.Department)
                                   .Include(c => c.Branch)
                                   .Include(e => e.Contractemployees)
                                       .ThenInclude(pea => pea.Contract)
                                           .ThenInclude(c => c.Project)
                                               .ThenInclude(p => p.Customer)
                                   .Include(e => e.Contractemployees)
                                       .ThenInclude(pea => pea.Contract)
                                           .ThenInclude(c => c.Project)
                                               .ThenInclude(p => p.Subpractice)
                                                   .ThenInclude(sp => sp.Practice)
                                   .Include(e => e.Contractemployees)
                                       .ThenInclude(pea => pea.Categorysubstatus)
                                           .ThenInclude(s => s.Categorystatus)
                                             .Include(e => e.Contractemployees)
                                     .ThenInclude(pea => pea.Contract)
                                           .ThenInclude(c => c.Project)
                                               .ThenInclude(p => p.Customer)
                                       //.Include(e => e.Projectemployeeassignments)
                                       //    .ThenInclude(pea => pea.Project)
                                       //        .ThenInclude(p => p.Subpractice)
                                       //            .ThenInclude(sp => sp.Practice)
                                       //.Include(e => e.Projectemployeeassignments)
                                       //    .ThenInclude(pea => pea.Project)
                                       //        .ThenInclude(p => p.Customer)
                                       .Include(e => e.Contractemployees)
                                          .ThenInclude(pea => pea.Employeeprojecthistories)
                                             .ThenInclude(x => x.Categorysubstatus)
                                       .Include(e => e.Contractemployees)
                                       .ThenInclude(pea => pea.Contract)
                                           .ThenInclude(c => c.Project)
                                               .ThenInclude(p => p.Projecttype)
                                        .Include(e => e.Contractemployees).ThenInclude(pea => pea.Contract)
                                           .ThenInclude(c => c.Deliveryanchor)
                                        .Include(e => e.Contractemployees).ThenInclude(pea => pea.Contractbillings)
                                        .Include(e => e.Employeeskills).ThenInclude(es => es.Skill)
                                   .Where(e => e.Employeeid == EmployeeId)
                                   .FirstOrDefaultAsync();


                var response = new employeeResponseDto
                {
                    Employeeid = e.Employeeid,
                    Experience = e.Dateofjoining == null ? "Experienced" : ExperienceCalculation(DateTime.Parse(e.Dateofjoining), e.Workexedays, e.Ade),
                    TmcId = e.Userid,
                    Udf3 = e.Udf3,
                    Employeename = e.Employeename ?? "",
                    email = e.Companyemail ?? "",
                    Contactno = e.Contactno ?? "",
                    Department = e.Department?.Departmentname ?? "",
                    Branch = e.Branch?.Branchname ?? "",
                    Dateofbirth = e.Dateofbirth ?? "",
                    Dateofjoining = e.Dateofjoining ?? "",
                    PracticeName = e.Subpractice == null ? "" : e.Subpractice.Practice.Practicename,
                    SubPracticeName = e.Subpractice == null ? "" : e.Subpractice.Subpracticename,
                    CategorySubStatusName = e.Categorysubstatus == null ? "" : e.Categorysubstatus.Categorysubstatusname ?? "",
                    CategoryStatusName = e.Categorysubstatus == null ? "" :
                                   e.Categorysubstatus.Categorystatus == null ? "" :
                                   e.Categorysubstatus.Categorystatus.Categorystatusname,
                    Ade = e.Ade,
                    PrimarySkill = e.Employeeskills?.Where(es => es.Isprimary == true).Select(es => new Skill_Employee_Info()
                    {
                        Employeeskillid = es.Employeeskillid,
                        Skillid = es.Skillid,
                        Skillname = es.Skill.Skillname,
                        Experinceinmonths = es.Experinceinmonths,
                        IsPrimary = es.Isprimary,
                        Managerrating = es.Managerrating,
                        Selfreting = es.Selfreting
                    }).FirstOrDefault(),
                    SecondarySkills = e.Employeeskills?.Where(es => es.Isprimary == false).Select(es => new Skill_Employee_Info()
                    {
                        Employeeskillid = es.Employeeskillid,
                        Skillid = es.Skillid,
                        Skillname = es.Skill.Skillname,
                        Experinceinmonths = es.Experinceinmonths,
                        IsPrimary = es.Isprimary,
                        Managerrating = es.Managerrating,
                        Selfreting = es.Selfreting
                    }).ToList(),
                    employeeProjects = e.Contractemployees?.Select(ce => new employeeProject
                    {
                        Projectid = ce.Contract?.Project.Projectid ?? 0,
                        Projectname = ce.Contract?.Project.Projectname ?? "",
                        Categorysubstatusid = ce.Categorysubstatus?.Categorysubstatusid ?? 0,
                        Categorysubstatusname = ce.Categorysubstatus?.Categorysubstatusname ?? "",
                        Categorystatusname = ce.Categorysubstatus?.Categorystatus.Categorystatusname ?? "",
                        CustomerCompanyname = ce.Contract?.Project.Customer?.Companyname ?? "",
                        Projecttypename = ce.Contract?.Project.Projecttype?.Projecttypename ?? "",
                        Subpracticename = ce.Contract?.Project.Subpractice?.Subpracticename ?? "",
                        Practicename = ce.Contract?.Project.Subpractice?.Practice?.Practicename ?? "",
                        BillingCosting = e.Contractemployees?.SelectMany(ce => ce.Contractbillings
                                                           .Where(cb => cb.Billingmonthyear == currentMonth)
                                                                   .Select(ings => ings.Costing))
                                                                    .FirstOrDefault() ?? 0,
                        Billingmonthyear = e.Contractemployees?.SelectMany(ce => ce.Contractbillings
                                                            .Where(cb => cb.Billingmonthyear == currentMonth)
                                                            .Select(ings => ings.Billingmonthyear))
                                                            .FirstOrDefault() ?? currentMonth,
                        DeliveryAnchorName = e.Contractemployees?.Select(ce => ce.Contract?.Deliveryanchor?.Employeename)
                                                            .FirstOrDefault() ?? "",

                        EffectiveStartDate = e.Contractemployees?
                                                                .SelectMany(ce => ce.Employeeprojecthistories)
                                                                .OrderByDescending(hist => hist.Historyid)
                                                                .Select(hist => hist.Effectivestartdate)
                                                                .FirstOrDefault(),

                        EffectiveEndDate = e.Contractemployees?
                                                                .SelectMany(ce => ce.Employeeprojecthistories)
                                                                .OrderByDescending(hist => hist.Historyid)
                                                                .Select(hist => hist.Effectiveenddate)
                                                                .FirstOrDefault(),

                        contracts = ce.Contract?.Contractemployees.Select(c => new Contractinfo
                        {
                            Contractid = c.Contract.Contractid,
                            ContractNo = c.Contract.Contractno,
                            DeliveryAnchorName = c.Contract.Deliveryanchor?.Employeename ?? "",
                            Deliveryanchorid = c.Contract.Deliveryanchorid,
                            Statusid = c.Contract.Statusid,
                            Contractstartdate = c.Contract.Contractstartdate,
                            Contractenddate = c.Contract.Contractenddate,
                            Lastupdatedate = c.Contract.Lastupdatedate,
                        }).ToList() ?? new List<Contractinfo>()
                        //contracts = ce.Contract?.Contractemployees
                        //                        .Where(cc=> cc.Contract.Contractstartdate.Value.Date <= DateTime.Now.Date && cc.Contract.Contractenddate.Value.Date <= DateTime.Now.Date)
                        //                        .Select(c => new Contractinfo
                        //                        {
                        //                            Contractid = c.Contract.Contractid,
                        //                            ContractNo = c.Contract.Contractno,
                        //                            DeliveryAnchorName = c.Contract.Deliveryanchor?.Employeename ?? "",
                        //                            Ponumber = c.Contract.Ponumber,
                        //                            Contractstartdate = c.Contract.Contractstartdate,
                        //                            Contractenddate = c.Contract.Contractenddate,
                        //                            Amount = c.Contract.Amount,
                        //                            Contactpersonname = c.Contract.Contactpersonname,
                        //                            Deliveryanchorid = c.Contract.Deliveryanchorid,
                        //                            Povalue = c.Contract.Povalue ?? 0,
                        //                            Contractemployees = c.Contract.Contractemployees?.Select(ce => new ContractemployeeInfo
                        //                            {
                        //                                Contractemployeeid = ce.Contractemployeeid,
                        //                                Categorysubstatusid = ce.Categorysubstatusid,
                        //                                Contractbillings = ce.Contractbillings?.Select(cb => new ContractbillingInfo
                        //                                {
                        //                                    Contractbillingid = cb.Contractbillingid,
                        //                                    Billingmonthyear = cb.Billingmonthyear,
                        //                                    Costing = cb.Costing
                        //                                }).ToList() ?? new List<ContractbillingInfo>()
                        //                            }).ToList() ?? new List<ContractemployeeInfo>()
                        //                        }).ToList() ?? new List<Contractinfo>()
                    }).ToList() ?? new List<employeeProject>(),
                    Role = e.Employeeroles == null ? null : e.Employeeroles.Select(x => new employeeRole
                    {
                        Role = x.Role.Rolename,
                        RoleId = x.Role.Roleid,

                    }).ToList(),
                };

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        //old code for employee list till 07-12-23
        //public async Task<List<employeeResponseDto>> GetEmployeesWithDetails()
        //{
        //    try
        //    {

        //        var employees = await _context.Rmsemployees
        //                            .Include(x => x.Categorysubstatus).ThenInclude(x => x.Categorystatus)
        //                            .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
        //                            .Include(c => c.Department)
        //                            .Include(c => c.Branch)
        //                            .Include(e => e.Contractemployees)
        //                                .ThenInclude(pea => pea.Contract)
        //                                    .ThenInclude(c => c.Project)
        //                                        .ThenInclude(p => p.Customer)
        //                            .Include(e => e.Contractemployees)
        //                                .ThenInclude(pea => pea.Contract)
        //                                    .ThenInclude(c => c.Project)
        //                                        .ThenInclude(p => p.Subpractice)
        //                                            .ThenInclude(sp => sp.Practice)
        //                            .Include(e => e.Contractemployees)
        //                                .ThenInclude(pea => pea.Categorysubstatus)
        //                                    .ThenInclude(s => s.Categorystatus)
        //                                //.Include(e => e.Projectemployeeassignments)
        //                                //    .ThenInclude(pea => pea.Project)
        //                                //        .ThenInclude(p => p.Subpractice)
        //                                //            .ThenInclude(sp => sp.Practice)
        //                                //.Include(e => e.Projectemployeeassignments)
        //                                //    .ThenInclude(pea => pea.Project)
        //                                //        .ThenInclude(p => p.Customer)
        //                                .Include(e => e.Contractemployees)
        //                                .ThenInclude(pea => pea.Contract)
        //                                    .ThenInclude(c => c.Project)
        //                                        .ThenInclude(p => p.Projecttype)
        //                                .Include(e => e.Contractemployees)
        //                                    .ThenInclude(pea => pea.Contractbillings)
        //                            .Where(e => e.Departmentid == 22 && !emp.Contains(e.Userid))
        //                            .ToListAsync();


        //        var response = employees.Select(r => new employeeResponseDto
        //        {
        //            Employeeid = r.Employeeid,
        //            TmcId = r.Userid,
        //            Employeename = r.Employeename,
        //            email = r.Companyemail,
        //            Contactno = r.Contactno,
        //            //Designationid=r.Departmentid,
        //            // Designation=_context.Designations.FirstOrDefault(x=>x.Designationid==r.Designationid).Designationname??"",
        //            // Reportheadid=r.Reportheadid,
        //            ////////////////////// Reporthead= _context.Rmsemployees.FirstOrDefault(x => x.Userid == r.Reportheadid).Employeename ?? "",
        //            // Departmentid =r.Departmentid,
        //            Department = r.Department.Departmentname,
        //            // Branchid =r.Branchid,
        //            Branch = r.Branch.Branchname,
        //            Dateofbirth = r.Dateofbirth = "",
        //            Dateofjoining = r.Dateofjoining ?? "",
        //            PracticeName = r.Subpractice == null ? "" : r.Subpractice.Practice.Practicename,
        //            SubPracticeName = r.Subpractice == null ? "" : r.Subpractice.Subpracticename,
        //            CategorySubStatusName = r.Categorysubstatus == null ? "" : r.Categorysubstatus.Categorysubstatusname,
        //            CategoryStatusName = r.Categorysubstatus == null ? "" :
        //                            r.Categorysubstatus.Categorystatus == null ? "" :
        //                            r.Categorysubstatus.Categorystatus.Categorystatusname,
        //            employeeProjects = r.Contractemployees.Select(x => new employeeProject
        //            {
        //                Projectid = x.Contract.Project.Projectid,
        //                Projectname = x.Contract.Project.Projectname,
        //                Categorysubstatusid = x.Categorysubstatus.Categorysubstatusid,
        //                Categorysubstatusname = x.Categorysubstatus.Categorysubstatusname,
        //                Categorystatusname = x.Categorysubstatus.Categorystatus.Categorystatusname,
        //                CustomerCompanyname = x.Contract.Project.Customer.Companyname,
        //                Projecttypename = x.Contract.Project.Projecttype.Projecttypename,
        //                Subpracticename = x.Contract.Project.Subpractice.Subpracticename,
        //                Practicename = x.Contract.Project.Subpractice.Practice.Practicename,
        //                contracts = r.Contractemployees.Select(c => new Contractinfo
        //                {
        //                    Contractid = c.Contractid,
        //                    ContractNo = c.Contract.Contractno,
        //                    DeliveryAnchorName = c.Contract.Deliveryanchor.Employeename,
        //                    Ponumber = c.Contract.Ponumber,
        //                    Contractstartdate = c.Contract.Contractstartdate,
        //                    Contractenddate = c.Contract.Contractenddate,
        //                    Amount = c.Contract.Amount,
        //                    Contactpersonname = c.Contract.Contactpersonname,
        //                    Deliveryanchorid = c.Contract.Deliveryanchorid,
        //                    Povalue = c.Contract.Povalue ?? 0,
        //                    Contractemployees = c.Contract.Contractemployees.Select(ce => new ContractemployeeInfo
        //                    {
        //                        Contractemployeeid = ce.Contractemployeeid,
        //                        Categorysubstatusid = ce.Categorysubstatusid,
        //                        Contractbillings = ce.Contractbillings.Select(cb => new ContractbillingInfo
        //                        {
        //                            Contractbillingid = cb.Contractbillingid,
        //                            Billingmonthyear = cb.Billingmonthyear,
        //                            Costing = cb.Costing,
        //                        }).ToList(),
        //                    }).ToList(),

        //                }).ToList(),
        //            }).ToList(),

        //            //// Projectemployeeassignmentid =r.Projectemployeeassignments.Select(x=>x.Projectemployeeassignmentid).FirstOrDefault(),
        //            // Projectid = r.Contractemployees.Select(x => x.Projectid).FirstOrDefault(),
        //            // Categorysubstatusid= r.Projectemployeeassignments.Select(x => x.Categorysubstatusid).FirstOrDefault(),
        //            //// Categorystatusid= r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorystatusid).FirstOrDefault(),
        //            // Categorysubstatusname= r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorysubstatusname).FirstOrDefault(),
        //            // Categorystatusname = r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorystatus.Categorystatusname).FirstOrDefault(),
        //            // Projectname=r.Projectemployeeassignments.Select(x=>x.Project.Projectname).FirstOrDefault(),
        //            //// Projectdescription= r.Projectemployeeassignments.Select(x => x.Project.Projectdescription).FirstOrDefault(),
        //            //// Customerid = r.Projectemployeeassignments.Select(x => x.Project.Customerid).FirstOrDefault()    ,
        //            // //Projecttypeid = r.Projectemployeeassignments.Select(x => x.Project.Projecttypeid).FirstOrDefault(),
        //            //// Subpracticeid= r.Projectemployeeassignments.Select(x => x.Project.Projecttypeid).FirstOrDefault(),
        //            // CustomerCompanyname= r.Projectemployeeassignments.Select(x => x.Project.Customer.Companyname).FirstOrDefault(),
        //            //// CustomerLastname = r.Projectemployeeassignments.Select(x => x.Project.Customer.Lastname).FirstOrDefault()  ,
        //            //// CustomerFirstname = r.Projectemployeeassignments.Select(x => x.Project.Customer.Firstname).FirstOrDefault() ,
        //            //// CustomerCompanylogourl = r.Projectemployeeassignments.Select(x => x.Project.Customer.Companylogourl).FirstOrDefault() ,
        //            // Projecttypename= r.Projectemployeeassignments.Select(x => x.Project.Projecttype.Projecttypename).FirstOrDefault(),
        //            // //Practiceid= r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Practiceid).FirstOrDefault(),
        //            // Subpracticename= r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Subpracticename).FirstOrDefault(),
        //            // Practicename = r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Practice.Practicename).FirstOrDefault()
        //        }).ToList();

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<employeeResponseDto> GetEmployeesByIdWithDetails(int EmployeeId)
        //{
        //    try
        //    {
        //        //////////var result = await _context.Rmsemployees
        //        //////////                                        .Include(c => c.Department)
        //        //////////                                        .Include(c => c.Branch)
        //        //////////                                       .Include(e => e.Projectemployeeassignments)
        //        //////////                                           .ThenInclude(pea => pea.Categorysubstatus)
        //        //////////                                               .ThenInclude(css => css.Categorystatus)
        //        //////////                                       .Include(e => e.Projectemployeeassignments)
        //        //////////                                           .ThenInclude(pea => pea.Project)
        //        //////////                                               .ThenInclude(p => p.Subpractice)
        //        //////////                                                   .ThenInclude(sp => sp.Practice)
        //        //////////                                       .Include(e => e.Projectemployeeassignments)
        //        //////////                                           .ThenInclude(pea => pea.Project)
        //        //////////                                               .ThenInclude(p => p.Customer)
        //        //////////                                       .Include(e => e.Projectemployeeassignments)
        //        //////////                                           .ThenInclude(pea => pea.Project)
        //        //////////                                               .ThenInclude(p => p.Projecttype)
        //        //////////                                       .Where(e => e.Sbuid == 1 && e.Employeeid== EmployeeId)
        //        //////////                                        .FirstOrDefaultAsync();
        //        //////////var response = new employeeResponseDto
        //        //////////{
        //        //////////    Employeeid = result.Employeeid,
        //        //////////    TmcId = result.Userid,
        //        //////////    Employeename = result.Employeename,
        //        //////////    email = result.Companyemail,
        //        //////////    Contactno = result.Contactno,
        //        //////////    // Designationid = result.Departmentid,
        //        //////////    // Designation = _context.Designations.FirstOrDefault(x => x.Designationid == result.Designationid).Designationname ?? "",
        //        //////////    // Reportheadid = result.Reportheadid,
        //        //////////    ////////// Reporthead = _context.Rmsemployees.FirstOrDefault(x => x.Userid == result.Reportheadid).Employeename ?? "",
        //        //////////    // Departmentid = result.Departmentid,
        //        //////////    Department = result.Department.Departmentname,
        //        //////////    // Branchid = result.Branchid,
        //        //////////    Branch = result.Branch.Branchname,
        //        //////////    Dateofbirth = result.Dateofbirth = "",
        //        //////////    Dateofjoining = result.Dateofjoining ?? "",
        //        //////////   // Projectemployeeassignmentid =result.Projectemployeeassignments.Select(x => x.Projectemployeeassignmentid).FirstOrDefault(),
        //        //////////    Projectid =result.Projectemployeeassignments.Select(x => x.Projectid).FirstOrDefault(),
        //        //////////    Categorysubstatusid =result.Projectemployeeassignments.Select(x => x.Categorysubstatusid).FirstOrDefault(),
        //        //////////  //  Categorystatusid =result.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorystatusid).FirstOrDefault(),
        //        //////////    Categorysubstatusname =result.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorysubstatusname).FirstOrDefault(),
        //        //////////    Categorystatusname =result.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorystatus.Categorystatusname).FirstOrDefault(),
        //        //////////    Projectname =result.Projectemployeeassignments.Select(x => x.Project.Projectname).FirstOrDefault(),
        //        //////////   // Projectdescription =result.Projectemployeeassignments.Select(x => x.Project.Projectdescription).FirstOrDefault(),
        //        //////////   // Customerid =result.Projectemployeeassignments.Select(x => x.Project.Customerid).FirstOrDefault(),
        //        //////////  //  Projecttypeid =result.Projectemployeeassignments.Select(x => x.Project.Projecttypeid).FirstOrDefault(),
        //        //////////  //  Subpracticeid =result.Projectemployeeassignments.Select(x => x.Project.Projecttypeid).FirstOrDefault(),
        //        //////////    CustomerCompanyname =result.Projectemployeeassignments.Select(x => x.Project.Customer.Companyname).FirstOrDefault(),
        //        //////////  //  CustomerLastname =result.Projectemployeeassignments.Select(x => x.Project.Customer.Lastname).FirstOrDefault(),
        //        //////////  //  CustomerFirstname =result.Projectemployeeassignments.Select(x => x.Project.Customer.Firstname).FirstOrDefault(),
        //        //////////  //  CustomerCompanylogourl =result.Projectemployeeassignments.Select(x => x.Project.Customer.Companylogourl).FirstOrDefault(),
        //        //////////    Projecttypename =result.Projectemployeeassignments.Select(x => x.Project.Projecttype.Projecttypename).FirstOrDefault(),
        //        //////////    //Practiceid =result.Projectemployeeassignments.Select(x => x.Project.Subpractice.Practiceid).FirstOrDefault(),
        //        //////////    Subpracticename =result.Projectemployeeassignments.Select(x => x.Project.Subpractice.Subpracticename).FirstOrDefault(),
        //        //////////    Practicename =result.Projectemployeeassignments.Select(x => x.Project.Subpractice.Practice.Practicename).FirstOrDefault()
        //        //////////};
        //        ///

        //        var r = await _context.Rmsemployees
        //                            .Include(x => x.Categorysubstatus).ThenInclude(x => x.Categorystatus)
        //                           .Include(r=>r.Employeeroles).ThenInclude(r=>r.Role)
        //                           .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
        //                           .Include(c => c.Department)
        //                           .Include(c => c.Branch)
        //                           .Include(e => e.Contractemployees)
        //                               .ThenInclude(pea => pea.Contract)
        //                                   .ThenInclude(c => c.Project)
        //                                       .ThenInclude(p => p.Customer)
        //                           .Include(e => e.Contractemployees)
        //                               .ThenInclude(pea => pea.Contract)
        //                                   .ThenInclude(c => c.Project)
        //                                       .ThenInclude(p => p.Subpractice)
        //                                           .ThenInclude(sp => sp.Practice)
        //                           .Include(e => e.Contractemployees)
        //                               .ThenInclude(pea => pea.Categorysubstatus)
        //                                   .ThenInclude(s => s.Categorystatus)
        //                                     .Include(e => e.Contractemployees)
        //                             .ThenInclude(pea => pea.Contract)
        //                                   .ThenInclude(c => c.Project)
        //                                       .ThenInclude(p => p.Customer)
        //                               //.Include(e => e.Projectemployeeassignments)
        //                               //    .ThenInclude(pea => pea.Project)
        //                               //        .ThenInclude(p => p.Subpractice)
        //                               //            .ThenInclude(sp => sp.Practice)
        //                               //.Include(e => e.Projectemployeeassignments)
        //                               //    .ThenInclude(pea => pea.Project)
        //                               //        .ThenInclude(p => p.Customer)
        //                               .Include(e => e.Contractemployees)
        //                               .ThenInclude(pea => pea.Contract)
        //                                   .ThenInclude(c => c.Project)
        //                                       .ThenInclude(p => p.Projecttype)
        //                                .Include(e => e.Contractemployees).ThenInclude(pea => pea.Contract)
        //                                   .ThenInclude(c => c.Deliveryanchor)
        //                           .Where(e => e.Sbuid == 1 && e.Employeeid == EmployeeId)
        //                           .FirstOrDefaultAsync();


        //        var response = new employeeResponseDto
        //        {
        //            Employeeid = r.Employeeid,
        //            TmcId = r.Userid,
        //            Employeename = r.Employeename,
        //            email = r.Companyemail,
        //            Contactno = r.Contactno,
        //            Department = r.Department.Departmentname,
        //            Branch = r.Branch.Branchname,
        //            Dateofbirth = r.Dateofbirth = "",
        //            Dateofjoining = r.Dateofjoining ?? "",
        //            PracticeName = r.Subpractice==null?"":r.Subpractice.Practice.Practicename,
        //            SubPracticeName = r.Subpractice==null?"": r.Subpractice.Subpracticename,
        //            CategorySubStatusName = r.Categorysubstatus == null ? "" : r.Categorysubstatus.Categorysubstatusname,
        //            CategoryStatusName = r.Categorysubstatus == null ? "" :
        //                            r.Categorysubstatus.Categorystatus == null ? "" :
        //                            r.Categorysubstatus.Categorystatus.Categorystatusname,

        //            employeeProjects = r.Contractemployees.Select(x => new employeeProject
        //            {
        //                //Contractid = x.Contractid,
        //                //ContractNo = x.Contract.Contractno,
        //               // DeliveryAnchorName = x.Contract.Deliveryanchor == null ? "" : x.Contract.Deliveryanchor.Employeename,
        //                Projectid = x.Contract.Project.Projectid,
        //                Projectname = x.Contract.Project.Projectname,
        //                Categorysubstatusid = x.Categorysubstatus.Categorysubstatusid,
        //                Categorysubstatusname = x.Categorysubstatus.Categorysubstatusname,
        //                Categorystatusname = x.Categorysubstatus.Categorystatus.Categorystatusname,
        //                CustomerCompanyname = x.Contract.Project.Customer.Companyname,
        //                Projecttypename = x.Contract.Project.Projecttype.Projecttypename,
        //                Subpracticename = x.Contract.Project.Subpractice.Subpracticename,
        //                Practicename = x.Contract.Project.Subpractice.Practice.Practicename,
        //               // Costing = x.Contract.Povalue == null ? x.Contract.Amount : x.Contract.Povalue,
        //                ProjectStartDate = x.Contract.Project.Createddate,
        //            }).ToList(),
        //            Role=r.Employeeroles==null?null: r.Employeeroles.Select(x=>new employeeRole
        //            {
        //                Role=x.Role.Rolename,
        //                RoleId=x.Role.Roleid,

        //            }).ToList(),                  


        //        };

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //old code for employeeofDA till 18-01-2024
        //public async Task<List<employeeResponseDto>> GetEmployeesOfDA(int DeliveryAnchorId)
        //{
        //    try
        //    {
        //        var ReportingHedTMC=await _context.Rmsemployees.Where(x=>x.Employeeid==DeliveryAnchorId).Select(x=>x.Userid).FirstOrDefaultAsync();
        //        var ReportingHedName = await _context.Rmsemployees.Where(x => x.Employeeid == DeliveryAnchorId).Select(x => x.Employeename).FirstOrDefaultAsync();

        //        var employees = await _context.Rmsemployees
        //                             .Include(x=>x.Categorysubstatus).ThenInclude(x=>x.Categorystatus)
        //                             .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
        //                            .Include(c => c.Department)
        //                            .Include(c => c.Branch)
        //                            .Include(e => e.Contractemployees)
        //                                .ThenInclude(pea => pea.Contract)
        //                                    .ThenInclude(c => c.Project)
        //                                        .ThenInclude(p => p.Customer)
        //                            .Include(e => e.Contractemployees)
        //                                .ThenInclude(pea => pea.Contract)
        //                                    .ThenInclude(c => c.Project)
        //                                        .ThenInclude(p => p.Subpractice)
        //                                            .ThenInclude(sp => sp.Practice)
        //                            .Include(e => e.Contractemployees)
        //                                .ThenInclude(pea => pea.Categorysubstatus)
        //                                    .ThenInclude(s => s.Categorystatus)
        //                                //.Include(e => e.Projectemployeeassignments)
        //                                //    .ThenInclude(pea => pea.Project)
        //                                //        .ThenInclude(p => p.Subpractice)
        //                                //            .ThenInclude(sp => sp.Practice)
        //                                //.Include(e => e.Projectemployeeassignments)
        //                                //    .ThenInclude(pea => pea.Project)
        //                                //        .ThenInclude(p => p.Customer)
        //                                .Include(e => e.Contractemployees)
        //                                .ThenInclude(pea => pea.Contract)
        //                                    .ThenInclude(c => c.Project)
        //                                        .ThenInclude(p => p.Projecttype)
        //                            .Where(e => e.Departmentid == 22 && !emp.Contains(e.Userid) && e.Isactive == true && e.Udf3 != "2" && e.Reportheadid==ReportingHedTMC
        //                            || e.Contractemployees.Select(c => c.Contract.Deliveryanchorid).Contains(DeliveryAnchorId))
        //                            .ToListAsync();



        //        var response = employees.Select(r => new employeeResponseDto
        //        {
        //            Employeeid = r.Employeeid,
        //            TmcId = r.Userid,
        //            Employeename = r.Employeename,
        //            email = r.Companyemail,
        //            Contactno = r.Contactno,
        //            //Designationid=r.Departmentid,
        //            // Designation=_context.Designations.FirstOrDefault(x=>x.Designationid==r.Designationid).Designationname??"",
        //            Reportheadid=r.Reportheadid,
        //            ////////////////////// Reporthead= _context.Rmsemployees.FirstOrDefault(x => x.Userid == r.Reportheadid).Employeename ?? "",
        //            Reporthead= ReportingHedName,
        //            // Departmentid =r.Departmentid,
        //            Department = r.Department.Departmentname,
        //            // Branchid =r.Branchid,
        //            Branch = r.Branch.Branchname,
        //            Dateofbirth = r.Dateofbirth = "",
        //            Dateofjoining = r.Dateofjoining ?? "",
        //            PracticeName = r.Subpractice == null ? "" : r.Subpractice.Practice.Practicename,
        //            SubPracticeName = r.Subpractice == null ? "" : r.Subpractice.Subpracticename,
        //            CategorySubStatusName = r.Categorysubstatus == null ? "" : r.Categorysubstatus.Categorysubstatusname,
        //            CategoryStatusName = r.Categorysubstatus == null ? "" :
        //                            r.Categorysubstatus.Categorystatus == null ? "" :
        //                            r.Categorysubstatus.Categorystatus.Categorystatusname,

        //            employeeProjects = r.Contractemployees.Select(x => new employeeProject
        //            {
        //                Projectid = x.Contract.Project.Projectid,
        //                Projectname = x.Contract.Project.Projectname,
        //                Categorysubstatusid = x.Categorysubstatus.Categorysubstatusid,
        //                Categorysubstatusname = x.Categorysubstatus.Categorysubstatusname,
        //                Categorystatusname = x.Categorysubstatus.Categorystatus.Categorystatusname,
        //                CustomerCompanyname = x.Contract.Project.Customer.Companyname,
        //                Projecttypename = x.Contract.Project.Projecttype.Projecttypename,
        //                Subpracticename = x.Contract.Project.Subpractice.Subpracticename,
        //                Practicename = x.Contract.Project.Subpractice.Practice.Practicename,
        //                DeliveryAnchorName= ReportingHedName
        //            }).ToList(),

        //            //// Projectemployeeassignmentid =r.Projectemployeeassignments.Select(x=>x.Projectemployeeassignmentid).FirstOrDefault(),
        //            // Projectid = r.Contractemployees.Select(x => x.Projectid).FirstOrDefault(),
        //            // Categorysubstatusid= r.Projectemployeeassignments.Select(x => x.Categorysubstatusid).FirstOrDefault(),
        //            //// Categorystatusid= r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorystatusid).FirstOrDefault(),
        //            // Categorysubstatusname= r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorysubstatusname).FirstOrDefault(),
        //            // Categorystatusname = r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorystatus.Categorystatusname).FirstOrDefault(),
        //            // Projectname=r.Projectemployeeassignments.Select(x=>x.Project.Projectname).FirstOrDefault(),
        //            //// Projectdescription= r.Projectemployeeassignments.Select(x => x.Project.Projectdescription).FirstOrDefault(),
        //            //// Customerid = r.Projectemployeeassignments.Select(x => x.Project.Customerid).FirstOrDefault()    ,
        //            // //Projecttypeid = r.Projectemployeeassignments.Select(x => x.Project.Projecttypeid).FirstOrDefault(),
        //            //// Subpracticeid= r.Projectemployeeassignments.Select(x => x.Project.Projecttypeid).FirstOrDefault(),
        //            // CustomerCompanyname= r.Projectemployeeassignments.Select(x => x.Project.Customer.Companyname).FirstOrDefault(),
        //            //// CustomerLastname = r.Projectemployeeassignments.Select(x => x.Project.Customer.Lastname).FirstOrDefault()  ,
        //            //// CustomerFirstname = r.Projectemployeeassignments.Select(x => x.Project.Customer.Firstname).FirstOrDefault() ,
        //            //// CustomerCompanylogourl = r.Projectemployeeassignments.Select(x => x.Project.Customer.Companylogourl).FirstOrDefault() ,
        //            // Projecttypename= r.Projectemployeeassignments.Select(x => x.Project.Projecttype.Projecttypename).FirstOrDefault(),
        //            // //Practiceid= r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Practiceid).FirstOrDefault(),
        //            // Subpracticename= r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Subpracticename).FirstOrDefault(),
        //            // Practicename = r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Practice.Practicename).FirstOrDefault()
        //        }).ToList();

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        public async Task<List<employeeResponseDto>> GetEmployeesOfDA(int DeliveryAnchorId)
        {
            try
            {

                var reportingHeads = await _context.Rmsemployees.AsNoTracking().ToListAsync();
                var DeliveryAnchorName = await _context.Rmsemployees.AsNoTracking().Where(x => x.Employeeid == DeliveryAnchorId).Select(x => x.Employeename).FirstOrDefaultAsync();
                var deliveryanchorid = await _context.Rmsemployees.AsNoTracking().Where(x => x.Employeeid == DeliveryAnchorId).Select(x => x.Userid).FirstOrDefaultAsync();
                var employees = await _context.Rmsemployees.AsNoTracking()
     .Include(x => x.Categorysubstatus).ThenInclude(x => x.Categorystatus)
     .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
     .Include(c => c.Department)
     .Include(c => c.Branch)
     .Include(e => e.Contractemployees)
         .ThenInclude(pea => pea.Contract)
             .ThenInclude(c => c.Project)
                 .ThenInclude(p => p.Customer)
     .Include(e => e.Contractemployees)
         .ThenInclude(pea => pea.Contract)
             .ThenInclude(c => c.Project)
                 .ThenInclude(p => p.Subpractice)
                     .ThenInclude(sp => sp.Practice)
     .Include(e => e.Contractemployees)
         .ThenInclude(pea => pea.Categorysubstatus)
             .ThenInclude(s => s.Categorystatus)
     //.Include(e => e.Projectemployeeassignments)
     //    .ThenInclude(pea => pea.Project)
     //        .ThenInclude(p => p.Subpractice)
     //            .ThenInclude(sp => sp.Practice)
     //.Include(e => e.Projectemployeeassignments)
     //    .ThenInclude(pea => pea.Project)
     //        .ThenInclude(p => p.Customer)
     .Include(e => e.Contractemployees)
         .ThenInclude(pea => pea.Contract)
             .ThenInclude(c => c.Project)
                 .ThenInclude(p => p.Projecttype)
     .Include(e => e.Contractemployees).ThenInclude(pea => pea.Contract)
         .ThenInclude(c => c.Deliveryanchor)
     .Include(e => e.Contractemployees)
         .ThenInclude(pea => pea.Contractbillings)
     .Where(e =>
         !emp.Contains(e.Userid)
         && e.Isactive == true
         && e.Udf3 != "2"
         && (
             e.Contractemployees.Select(c => c.Contract.Deliveryanchorid).Contains(DeliveryAnchorId)
             || e.Reportheadid == deliveryanchorid
         )
     )
     .ToListAsync();

                string currentMonth = DateTime.Now.ToString("MMM-yy");

                var response = employees.Select(r => new employeeResponseDto
                {
                    Employeeid = r.Employeeid,
                    Experience = r.Dateofjoining == null ? "Experienced" : ExperienceCalculation(DateTime.Parse(r.Dateofjoining), r.Workexedays, r.Ade),
                    TmcId = r.Userid,
                    Udf3 = r.Udf3 ?? "",
                    Employeename = r.Employeename ?? "",
                    email = r.Companyemail ?? "",
                    Contactno = r.Contactno ?? "",
                    //Designationid=r.Departmentid,
                    // Designation=_context.Designations.FirstOrDefault(x=>x.Designationid==r.Designationid).Designationname??"",
                    Reportheadid = r.Reportheadid,
                    ////////////////////// Reporthead= _context.Rmsemployees.FirstOrDefault(x => x.Userid == r.Reportheadid).Employeename ?? "",
                    Reporthead = r.Reportheadid != null ? reportingHeads.FirstOrDefault(rh => rh.Userid == r.Reportheadid)?.Employeename : "",
                    // Departmentid =r.Departmentid,
                    Department = r.Department.Departmentname ?? "",
                    // Branchid =r.Branchid,
                    Branch = r.Branch.Branchname ?? "",
                    Dateofbirth = r.Dateofbirth = "",
                    Dateofjoining = r.Dateofjoining ?? "",
                    PracticeName = r.Subpractice == null ? "" : r.Subpractice.Practice.Practicename,
                    SubPracticeName = r.Subpractice == null ? "" : r.Subpractice.Subpracticename,
                    //CategorySubStatusName = r.Categorysubstatus == null ? "" : r.Categorysubstatus.Categorysubstatusname,
                    //CategoryStatusName = r.Categorysubstatus == null ? "" :
                    //                r.Categorysubstatus.Categorystatus == null ? "" :
                    //                r.Categorysubstatus.Categorystatus.Categorystatusname,

                    CategorySubStatusName = GetStatusFromCollection(r.Contractemployees),


                    CategoryStatusName = GetStatusFromCollection(r.Contractemployees),


                    employeeProjects = r.Contractemployees?.Select(x => new employeeProject
                    {
                        Projectid = x.Contract?.Project.Projectid ?? 0,
                        Projectname = x.Contract?.Project.Projectname ?? "",
                        Categorysubstatusid = x.Categorysubstatus?.Categorysubstatusid ?? 0,
                        //Categorysubstatusname = x.Categorysubstatus?.Categorysubstatusname ?? "",
                        //Categorystatusname = x.Categorysubstatus?.Categorystatus.Categorystatusname ?? "",
                        //Categorysubstatusname = (x.Contract.Contractenddate>=DateTime.Now && x.Categorysubstatusid!=6)?"Deployed"
                        //                         : (x.Contract.Contractenddate < DateTime.Now && x.Contract.Statusid != 6)?"Over Run"
                        //                             : (x.Contract.Contractenddate < DateTime.Now && x.Contract.Statusid == 6) ? "Bench"
                        //                                 : x.Categorysubstatus?.Categorysubstatusname ?? "",
                        Categorysubstatusname = (x.Contract.Contractenddate >= DateTime.Now && x.Categorysubstatusid != 6) ? x.Categorysubstatus?.Categorysubstatusname
                                                 : (x.Contract.Contractenddate < DateTime.Now && x.Contract.Statusid != 6) ? "Over Run"
                                                     : (x.Contract.Contractenddate < DateTime.Now && x.Contract.Statusid == 6) ? "Bench"
                                                         : x.Categorysubstatus?.Categorysubstatusname ?? "",

                        //Categorystatusname = (x.Contract.Contractenddate >= DateTime.Now && x.Categorysubstatusid != 6) ? "Deployed"
                        //                         : (x.Contract.Contractenddate < DateTime.Now && x.Contract.Statusid != 6) ? "Over Run"
                        //                             : (x.Contract.Contractenddate < DateTime.Now && x.Contract.Statusid == 6) ? "Bench"
                        //                                 : x.Categorysubstatus?.Categorysubstatusname ?? "",
                        Categorystatusname = (x.Contract.Contractenddate >= DateTime.Now && x.Categorysubstatusid != 6) ? x.Categorysubstatus?.Categorysubstatusname
                                                 : (x.Contract.Contractenddate < DateTime.Now && x.Contract.Statusid != 6) ? "Over Run"
                                                     : (x.Contract.Contractenddate < DateTime.Now && x.Contract.Statusid == 6) ? "Bench"
                                                         : x.Categorysubstatus?.Categorysubstatusname ?? "",
                        CustomerCompanyname = x.Contract?.Project.Customer.Companyname ?? "",
                        Projecttypename = x.Contract?.Project.Projecttype.Projecttypename ?? "",
                        Subpracticename = x.Contract?.Project.Subpractice.Subpracticename ?? "",
                        Practicename = x.Contract?.Project.Subpractice.Practice.Practicename ?? "",
                        BillingCosting = r.Contractemployees?.SelectMany(ce => ce.Contractbillings
                                            .Where(cb => cb.Billingmonthyear == currentMonth)
                                            .Select(ings => ings.Costing))
                                            .FirstOrDefault() ?? 0,
                        Contractbillings = x.Contractbillings.Where(x => x.Billingmonthyear == currentMonth)?.Select(cb => new ContractbillingInfo
                        {
                            Contractbillingid = cb.Contractbillingid,
                            Billingmonthyear = cb.Billingmonthyear,
                            Costing = cb.Costing
                        }).ToList() ?? new List<ContractbillingInfo>(),

                        Billingmonthyear = r.Contractemployees?.SelectMany(ce => ce.Contractbillings
                                            .Where(cb => cb.Billingmonthyear == currentMonth)
                                            .Select(ings => ings.Billingmonthyear))
                                            .FirstOrDefault() ?? currentMonth,
                        DeliveryAnchorName = DeliveryAnchorName,
                        contracts = x.Contract?.Contractemployees.Select(c => new Contractinfo
                        {
                            Contractid = c.Contract.Contractid,
                            ContractNo = c.Contract.Contractno,
                            DeliveryAnchorName = c.Contract.Deliveryanchor?.Employeename ?? "",
                            Ponumber = c.Contract.Ponumber,
                            Contractstartdate = c.Contract.Contractstartdate,
                            Contractenddate = c.Contract.Contractenddate,
                            Amount = c.Contract.Amount,
                            Contactpersonname = c.Contract.Contactpersonname,
                            Deliveryanchorid = c.Contract.Deliveryanchorid,
                            Povalue = c.Contract.Povalue ?? 0,
                            Contractemployees = c.Contract.Contractemployees?.Select(ce => new ContractemployeeInfo
                            {
                                Contractemployeeid = ce.Contractemployeeid,
                                Categorysubstatusid = ce.Categorysubstatusid,
                                Contractbillings = ce.Contractbillings?.Select(cb => new ContractbillingInfo
                                {
                                    Contractbillingid = cb.Contractbillingid,
                                    Billingmonthyear = cb.Billingmonthyear,
                                    Costing = cb.Costing
                                }).ToList() ?? new List<ContractbillingInfo>()
                            }).ToList() ?? new List<ContractemployeeInfo>()
                        }).ToList() ?? new List<Contractinfo>()
                    }).ToList() ?? new List<employeeProject>(),
                    Role = r.Employeeroles == null ? null : r.Employeeroles.Select(x => new employeeRole
                    {
                        Role = x.Role.Rolename,
                        RoleId = x.Role.Roleid,


                    }).ToList(),

                    //// Projectemployeeassignmentid =r.Projectemployeeassignments.Select(x=>x.Projectemployeeassignmentid).FirstOrDefault(),
                    // Projectid = r.Contractemployees.Select(x => x.Projectid).FirstOrDefault(),
                    // Categorysubstatusid= r.Projectemployeeassignments.Select(x => x.Categorysubstatusid).FirstOrDefault(),
                    //// Categorystatusid= r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorystatusid).FirstOrDefault(),
                    // Categorysubstatusname= r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorysubstatusname).FirstOrDefault(),
                    // Categorystatusname = r.Projectemployeeassignments.Select(x => x.Categorysubstatus.Categorystatus.Categorystatusname).FirstOrDefault(),
                    // Projectname=r.Projectemployeeassignments.Select(x=>x.Project.Projectname).FirstOrDefault(),
                    //// Projectdescription= r.Projectemployeeassignments.Select(x => x.Project.Projectdescription).FirstOrDefault(),
                    //// Customerid = r.Projectemployeeassignments.Select(x => x.Project.Customerid).FirstOrDefault()    ,
                    // //Projecttypeid = r.Projectemployeeassignments.Select(x => x.Project.Projecttypeid).FirstOrDefault(),
                    //// Subpracticeid= r.Projectemployeeassignments.Select(x => x.Project.Projecttypeid).FirstOrDefault(),
                    // CustomerCompanyname= r.Projectemployeeassignments.Select(x => x.Project.Customer.Companyname).FirstOrDefault(),
                    //// CustomerLastname = r.Projectemployeeassignments.Select(x => x.Project.Customer.Lastname).FirstOrDefault()  ,
                    //// CustomerFirstname = r.Projectemployeeassignments.Select(x => x.Project.Customer.Firstname).FirstOrDefault() ,
                    //// CustomerCompanylogourl = r.Projectemployeeassignments.Select(x => x.Project.Customer.Companylogourl).FirstOrDefault() ,
                    // Projecttypename= r.Projectemployeeassignments.Select(x => x.Project.Projecttype.Projecttypename).FirstOrDefault(),
                    // //Practiceid= r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Practiceid).FirstOrDefault(),
                    // Subpracticename= r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Subpracticename).FirstOrDefault(),
                    // Practicename = r.Projectemployeeassignments.Select(x => x.Project.Subpractice.Practice.Practicename).FirstOrDefault()
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //get employees by the stored procedure 
        public async Task<List<EmployeeDetailDto>> GetEmployeelistStoredProcedure()
        {
            try
            {
                // Assuming EmployeeDetailDtos is the entity class mapped to the database table
                var employeeDetails = await _context.EmployeeDetailDtos
                                                    .FromSqlRaw("SELECT * FROM getemployeelist({0})", "Dec-23")
                                                    .ToListAsync();

                return employeeDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Response> EmployeeResignedStatus(int id, Rmsemployee employeeResponseDto, JwtLoginDetailDto LoginDetails)
        {
            var empUpdate = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(a => a.Employeeid == id);
            if (empUpdate == null)
            {
                return new Response { responseCode = 404, responseMessage = $"Vendor not found at given VendorId: {id} " };

            }
            empUpdate.Udf3 = employeeResponseDto.Udf3;    // udf 3 for employee resign status 1 for resigned 2 for employee exit o for active
            empUpdate.Lastupdatedon = DateOnly.FromDateTime(DateTime.Now);
            empUpdate.Lastupdatedby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync();

            try
            {
                _context.Rmsemployees.Update(empUpdate);
                await _context.SaveChangesAsync();
                return new Response { responseCode = 200, responseMessage = "Employee Resigned Status Updated Successfully" };
            }
            catch (Exception e)
            {
                return new Response { responseCode = 500, responseMessage = $"Something Went Wrong!! Error: {e.Message}" };
            }
        }

        private string ExperienceCalculation(DateTime? joiningDate, int? workExperienceDays, bool ade)
        {
            //int ?totalExperienceDays = (DateTime.Now - joiningDate.Value).Days + (ade ? 0 : workExperienceDays);

            //return totalExperienceDays >= 365 ? "Experienced" : "Fresher";

            // Check if joiningDate is null before accessing its value to avoid InvalidOperationException
            if (!joiningDate.HasValue)
            {
                throw new ArgumentException("Joining date must be provided.");
            }
            int daysSinceJoining = (DateTime.Now - joiningDate.Value).Days;

            int totalExperienceDays = daysSinceJoining + (ade ? 0 : workExperienceDays.GetValueOrDefault());

            return totalExperienceDays >= 365 ? "Experienced" : "Fresher";
        }

        public async Task<Response> AdeUpdation(adeUpdationDto Value, JwtLoginDetailDto LoginDetails)
        {
            try
            {
                var lastupdatedby = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(x => x.Userid == LoginDetails.TmcId);
                var existingEmployee = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(x => x.Employeeid == Value.employeeid);
                if (existingEmployee == null)
                {
                    throw new Exception($"Employee does not exists at give employee ID {Value.employeeid}");
                }
                existingEmployee.Ade = Value.IsAde.HasValue == true ? Value.IsAde.Value
                                                                : throw new Exception($"Please send a valid value in IsAde :{Value.IsAde} ");
                existingEmployee.Lastupdatedby = lastupdatedby.Employeeid;
                existingEmployee.Lastupdatedon = DateOnly.FromDateTime(DateTime.Now);
                _context.Rmsemployees.Update(existingEmployee);
                await _context.SaveChangesAsync();

                return new Response() { responseCode = 200, responseMessage = "Updated Successfully" };

            }
            catch (Exception e)
            {
                throw;
            }
        }



        //private string Experince_Calculation( DateTime joinigdate,int workexperiencedays,bool ade)
        //{
        //    if (ade )
        //    {
        //        var currentDate= DateTime.Now-joinigdate;   
        //        var days = currentDate.Days;    
        //        if (days >= 364) 
        //        {
        //            return "Experinced";
        //        }
        //        else
        //        {
        //            return "Fresher";

        //        }
        //    }
        //    else
        //    {
        //        if(workexperiencedays>= 365)
        //        {
        //            return "Experinced";

        //        }
        //        else
        //        {
        //            var currentDate = DateTime.Now - joinigdate;
        //            var days = currentDate.Days;
        //            var total = days + workexperiencedays;
        //            var status = "";

        //            status= total >= 365 ? "Experinced" : "Fresher";


        //                return status;

        //        }

        //    }

        //}



        //#region [Reporting section]
        //public async Task<List<rptContractBillingDto>> GetReportByMonthYearProcedure(string monthyear)
        //{
        //    try
        //    {
        //        var result = await _context.rptContractBillingDtos
        //                                            .FromSqlRaw("SELECT * FROM rptcontractbillingmonthyear({0})", monthyear)
        //                                            .ToListAsync();

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        //public async Task<IEnumerable> GetReportByMonthYearProcedure_DA_Wise(string monthyear)
        //{
        //    try
        //    {
        //        //var endresult = new List<rptContratBillingDA_WiseDto>();
        //        var result = await _context.rptContractBillingDtos
        //                                            .FromSqlRaw("SELECT * FROM rptcontractbillingmonthyear({0})", monthyear)
        //                                            .ToListAsync();

        //        var groupedResult = result
        //                                .GroupBy(x => x.DAname)
        //                                        .Select(g => new
        //                                        {
        //                                            DAname = g.Key,
        //                                            TAndMCount = g.Where(p => p.ProjectType == "T&M").Select(p => p.projectno).Distinct().Count(),
        //                                            FixedBidCount = g.Where(p => p.ProjectType == "Fixed Bid").Select(p => p.projectno).Distinct().Count(),
        //                                            PresalesCount = g.Where(p => p.ProjectType == "Presales").Select(p => p.projectno).Distinct().Count(),
        //                                            InternalCount = g.Where(p => p.ProjectType == "Internal").Select(p => p.projectno).Distinct().Count(),
        //                                            ProjectCount = g.Select(x => x.projectno).Distinct().Count(),
        //                                            TotalActualBilling = g.Sum(x => x.actualbilling),
        //                                            TotalProvisionBilling = g.Where(x => x.provesionbilling != null).Sum(x => x.provesionbilling) // Sum of provision billing, considering non-null values
        //                                        }).OrderBy(x => x.DAname);



        //        //if(groupedResult!=null || groupedResult.Count>0)
        //        //{
        //        //    foreach (var item in groupedResult)
        //        //    {
        //        //        endresult.Add( new rptContratBillingDA_WiseDto
        //        //        {
        //        //            CCname=item.Key.CCname==null ? null : item.Key.CCname,
        //        //            ProjectType=item.Key.ProjectType==null ? null : item.Key.ProjectType,
        //        //            CustomerName=item.Key.CustomerName==null ? null : item.Key.CustomerName,
        //        //            DAname=item.Key.DAname==null ? null : item.Key.DAname,
        //        //            Cno = item.Key.Cno == null ? null : item.Key.Cno,
        //        //            POno = item.Key.POno == null ? null : item.Key.POno,
        //        //            contractstartdate = item.Key.contractstartdate == null ? null : item.Key.contractstartdate ,
        //        //            contractenddate = item.Key.contractenddate == null ? null : item.Key.contractenddate,
        //        //            projectname = item.Key.projectname == null ? null : item.Key.projectname ,
        //        //            projectno = item.Key.projectno == null ? null : item.Key.projectno ,
        //        //            billingmonth = item.Key.billingmonth == null ? null : item.Key.billingmonth,
        //        //            totalActualBilling=item.TotalActualBilling,
        //        //            totalProvisionBilling=item.TotalProvisionBilling
        //        //        });

        //        //    }

        //        //}

        //        return groupedResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        //public async Task<IEnumerable> GetRptProvisionHistory(string monthyear, int contractid)
        //{
        //    try
        //    {
        //        var result = await _context.RevisionDetailsDtos
        //                                            .FromSqlRaw("SELECT * FROM getProvision_Revision({0},{1})", monthyear, contractid)
        //                                            .ToListAsync();

        //        var groupedResult = result.GroupBy(x => new
        //        {
        //            x.TMC,
        //            x.ResourceName,
        //            x.ProjectName,
        //            x.ProjectNo,
        //            x.ContractNo,
        //            x.ContractId,
        //            x.PONumber,
        //            x.DeliveryAnchor,
        //            x.EstimatedBillingDate
        //        }
        //                                           )
        //                                                    .Select(group => new
        //                                                    {
        //                                                        Item = group.Key,
        //                                                        ItemDetails = group.Select(item => new
        //                                                        {
        //                                                            item.RevisionNumber,
        //                                                            item.ProvisionAmount,
        //                                                            item.CreatedDate
        //                                                        }).ToList()
        //                                                    });


        //        return groupedResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public async Task<List<rptContractBillingProvisionDto>> GetReportByMonthYearProcedureProvision(string monthyear)
        //{
        //    try
        //    {
        //        var result = await _context.rptContractBillingProvisionDtos
        //                                            .FromSqlRaw("SELECT * FROM rptcontractbillingprovisionmonthyear({0})", monthyear)
        //                                            .ToListAsync();

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;



        //    }
        //}
        //#endregion[Reporting section]


        public async Task<bool> CheckMemberCanSeeResourceTimesheet(string tmc)
        {
            return await _context.Rmsemployees.AsNoTracking().AnyAsync(x => x.Reportheadid == tmc);
        }


        //private  string GetStatusFromCollection(ICollection<Contractemployee> Contractemployees)
        //{
        //    List<string> statuscollection = new List<string>();

        //    if (Contractemployees.Any(ce => ce.Categorysubstatus?.Categorysubstatusname == "Deployed"))
        //    {
        //        return "Deployed";
        //    }

        //    if (Contractemployees.Any(ce => ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid != 6))
        //    {
        //        return "Over Run";
        //    }

        //    if (Contractemployees.Any(ce => ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid == 6))
        //    {
        //        return "onbench";
        //    }

        //    if (Contractemployees.Any(ce => ce.Categorysubstatus?.Categorysubstatusname == null ||
        //                            new[] { "Projection", "Shadow", "Maternity Leave", "Leave Without Pay", "Bench", "Released" }.Contains(ce.Categorysubstatus?.Categorysubstatusname)))
        //    {
        //        return "onbench";
        //    }

        //    return "onbench"; // Default case if no conditions are met
        //}
        private string GetStatusFromCollection(ICollection<Contractemployee> Contractemployees)
        {
            bool isOverRun = Contractemployees.Any(ce => ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid != 6);
            bool isDeployed = Contractemployees.Any(ce => ce.Categorysubstatus?.Categorysubstatusname == "Deployed");
            bool isOnBench = Contractemployees.Any(ce => ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid == 6)
                             || Contractemployees.Any(ce => ce.Categorysubstatus?.Categorysubstatusname == null ||
                                                   new[] { "Projection", "Shadow", "Maternity Leave", "Leave Without Pay", "Bench", "Released" }.Contains(ce.Categorysubstatus?.Categorysubstatusname));

            if (isOverRun)
            {
                return "Over Run";
            }
            else if (isDeployed)
            {
                return "Deployed";
            }
            else if (isOnBench)
            {
                return "onbench";
            }
            else
            {
                return "onbench"; // Default case if no conditions are met
            }
        }

        public async Task<Response> UpdateEmployeeSubpractice(RequestSubpracticeUpdationDto Value, JwtLoginDetailDto LoginDetails)
        {
            try
            {
                if (Value.EmployeeId == 0)
                {
                    throw new InvalidOperationException("Employee id can not be equal to zero ");
                }
                if (Value.SubpracticeId == 0)
                {
                    throw new InvalidOperationException("Practice id can not be equal to zero");
                }

                var lastupdatedby = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(x => x.Userid == LoginDetails.TmcId);
                var existingEmployee = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(x => x.Employeeid == Value.EmployeeId);
                if (existingEmployee == null)
                {
                    throw new Exception($"Employee does not exists at give employee ID {Value.EmployeeId}");
                }
                existingEmployee.Subpracticeid = Value.SubpracticeId;
                existingEmployee.Lastupdatedby = lastupdatedby.Employeeid;
                existingEmployee.Lastupdatedon = DateOnly.FromDateTime(DateTime.Now);
                _context.Rmsemployees.Update(existingEmployee);
                await _context.SaveChangesAsync();
                return new Response() { responseCode = 200, responseMessage = "Updated Successfully" };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<Object>> GetAllBillingDataReourceWiseNew()
        {

            try
            {
                var response = await _context.BillingDataViews.AsNoTracking().ToListAsync<Object>();
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<employeeResponseDto>> GetEmployees()
        {
            string currentMonth = DateTime.Now.ToString("MMM-yy");
            try
            {
                var employeeQuery = _context.Rmsemployees.AsNoTracking()
                    .Where(e =>
                        (
                            (e.Departmentid == 22 && e.Sbuid == 1)
                            || (e.Departmentid == 6 && e.Sbuid == 55)
                            || (e.Departmentid == 22 && e.Sbuid == 55)
                        )
                        && !emp.Contains(e.Userid)
                        && e.Isactive == true
                        && e.Udf3 != "2"
                    )
                    .Select(e => new employeeResponseDto
                    {
                        Employeeid = e.Employeeid,
                        TmcId = e.Userid,
                        Udf3 = e.Udf3 ?? "",
                        Employeename = e.Employeename ?? "",
                        email = e.Companyemail ?? "",
                        Contactno = e.Contactno ?? ""
                    });
                var response = await employeeQuery.ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching employees", ex);
            }
        }


        public async Task<object> GetProfile(JwtLoginDetailDto logindetasils)
        {
            try
            {
                var response = await _context.Rmsemployees
                                             .Include(e => e.Department)
                                             .Include(e => e.Designation)
                                             .Include(e => e.Branch)
                                             .Include(e => e.Employeeroles).ThenInclude(erole => erole.Role)
                                             .Include(e => e.Employeeskills).ThenInclude(eskill => eskill.Skill)
                                             .Include(e => e.Subpractice).ThenInclude(practice => practice.Practice)
                                             .AsNoTracking().Where(e => e.Userid == logindetasils.TmcId).FirstOrDefaultAsync();
                var reportingHeads = await _context.Rmsemployees.AsNoTracking().ToListAsync();
                var restunningObject = new ProfileDto
                {
                    Employeeid = response?.Employeeid,
                    Userid = response?.Userid,
                    Employeename = response?.Employeename,
                    Companyemail = response?.Companyemail,
                    Sbu = response?.Sbu,
                    Contactno = response?.Contactno,
                    Designationid = response?.Designationid,
                    DesignationName = response?.Designation?.Designationname,
                    ReportheadName = response?.Reportheadid != null ? reportingHeads.FirstOrDefault(rh => rh.Userid == response.Reportheadid)?.Employeename : "",
                    Reportheadid = response?.Reportheadid,
                    Departmentid = response?.Departmentid,
                    DepartmentName = response?.Department?.Departmentname,
                    Branchid = response?.Branchid,
                    BranchName = response?.Branch?.Branchname,
                    Dateofbirth = response?.Dateofbirth,
                    Dateofjoining = response?.Dateofjoining,
                    EmployeeRoles = response?.Employeeroles.Select(roles => new EmployeeRoleDto { Roleid = roles?.Roleid, Rolename = roles?.Role?.Rolename }).ToList(),
                    PrimarySkill = response?.Employeeskills?.Where(primary => primary.Isprimary == true)
                                                          ?.Select(primary => new EmployeeSkillDto
                                                          {
                                                              EmployeeskillId = primary.Employeeskillid,
                                                              SkillId = primary.Skillid,
                                                              SkillName = primary.Skill.Skillname,
                                                              Isprimary = primary.Isprimary,
                                                              ManagerRating = primary.Managerrating,
                                                              SelfRating = primary.Selfreting,
                                                              Experinceinmonth = primary.Experinceinmonths
                                                          }
                                                          ).FirstOrDefault(),
                    SecondarySkill = response?.Employeeskills?.Where(primary => primary.Isprimary == false)
                                                          ?.Select(primary => new EmployeeSkillDto
                                                          {
                                                              EmployeeskillId = primary.Employeeskillid,
                                                              SkillId = primary.Skillid,
                                                              SkillName = primary.Skill.Skillname,
                                                              Isprimary = primary.Isprimary,
                                                              ManagerRating = primary.Managerrating,
                                                              SelfRating = primary.Selfreting,
                                                              Experinceinmonth = primary.Experinceinmonths
                                                          }
                                                          ).ToList(),
                    SubPracticeName = response?.Subpractice?.Subpracticename,
                    PracticeName = response?.Subpractice?.Practice?.Practicename,
                    Employeeregion = response?.Employeeregion,
                    Baseoffice = response?.Baseoffice,
                    Costcenter = response?.Costcenter,
                    Workexperience = response?.Workexperience,
                    Workexedays = response?.Workexedays,
                    Pastworkexperience = response?.Workexedays,
                    Teamworkexperience = response.Dateofjoining != null && DateTime.TryParse(response.Dateofjoining, out var joiningDate)
                        ? (DateTime.Now - joiningDate).Days
                        : null
                };
                return restunningObject;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<employeeResponseDto>> GetEmployeesForCount()
        {
            try
            {
                var employees = await _context.Rmsemployees.AsNoTracking()
                    .Where(e =>
                        (
                            (e.Departmentid == 22 && e.Sbuid == 1)
                            || (e.Departmentid == 6 && e.Sbuid == 55)
                            || (e.Departmentid == 22 && e.Sbuid == 55)
                        )
                        && !emp.Contains(e.Userid)
                        && e.Isactive == true
                        && e.Udf3 != "2"
                    )
                    .Include(x => x.Categorysubstatus).ThenInclude(x => x.Categorystatus)
                    .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
                    .Include(c => c.Department)
                    .Include(c => c.Branch)
                    .Include(e => e.Contractemployees)
                        .ThenInclude(pea => pea.Categorysubstatus)
                            .ThenInclude(s => s.Categorystatus)
                    .Include(e => e.Contractemployees)
                        .ThenInclude(pea => pea.Contract)
                            .ThenInclude(c => c.Project)
                    .ToListAsync();

                var response = employees.Select(e => new employeeResponseDto
                {
                    Employeeid = e.Employeeid,
                    Experience = e.Dateofjoining == null
                        ? "Experienced"
                        : ExperienceCalculation(DateTime.Parse(e.Dateofjoining), e.Workexedays, e.Ade),
                    TmcId = e.Userid,
                    Udf3 = e.Udf3 ?? "",
                    Employeename = e.Employeename ?? "",
                    email = e.Companyemail ?? "",
                    Contactno = e.Contactno ?? "",
                    Department = e.Department?.Departmentname ?? "",
                    Branch = e.Branch?.Branchname ?? "",
                    Dateofbirth = e.Dateofbirth ?? "",
                    Dateofjoining = e.Dateofjoining ?? "",
                    PracticeName = e.Subpractice == null ? "" : e.Subpractice.Practice.Practicename,
                    SubPracticeName = e.Subpractice == null ? "" : e.Subpractice.Subpracticename,
                    CategorySubStatusName = GetStatusFromCollection(e.Contractemployees),
                    CategoryStatusName = GetStatusFromCollection(e.Contractemployees),
                    employeeProjects = e.Contractemployees?.Select(ce => new employeeProject
                    {
                        Projectid = ce.Contract?.Project.Projectid ?? 0,
                        Projectname = ce.Contract?.Project.Projectname ?? "",
                        Categorysubstatusid = ce.Categorysubstatus?.Categorysubstatusid ?? 0,
                        Categorysubstatusname = (ce.Contract.Contractenddate >= DateTime.Now && ce.Categorysubstatusid != 6)
                            ? ce.Categorysubstatus?.Categorysubstatusname
                            : (ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid != 6) ? "Over Run"
                            : (ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid == 6) ? "Bench"
                            : ce.Categorysubstatus?.Categorysubstatusname ?? "",
                        Categorystatusname = (ce.Contract.Contractenddate >= DateTime.Now && ce.Categorysubstatusid != 6)
                            ? ce.Categorysubstatus?.Categorysubstatusname
                            : (ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid != 6) ? "Over Run"
                            : (ce.Contract.Contractenddate < DateTime.Now && ce.Contract.Statusid == 6) ? "Bench"
                            : ce.Categorysubstatus?.Categorysubstatusname ?? "",
                    }).ToList() ?? new List<employeeProject>(),
                }).ToList();

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //public async Task<List<DataForEmployeeReport>> GetEmployeeDetailsView(int? id)
        //{
        //    string daName = string.Empty;
        //    try
        //    {
        //        if (id != null && id > 0)
        //        {
        //            daName = await _context.Rmsemployees
        //                .Where(x => x.Employeeid == id)
        //                .Select(x => x.Employeename)
        //                .FirstOrDefaultAsync();
        //        }

        //        // Fetch all records and filter out based on conditions
        //        var result = await _context.DataForEmployeeReports
        //                    .Where(x =>
        //                        (x.Contractenddate > DateTime.Now && x.Categorysubstatusid != 11) || // Include if contract end date is in the future
        //                        (x.Contractenddate < DateTime.Now &&
        //                         (x.Contractstatusid != 6 && x.Categorysubstatusid != 11)))// Include if contract end date is in the past and either status conditions are met
        //                    .ToListAsync();




        //        if (!string.IsNullOrEmpty(daName))
        //        {
        //            // Filter results for DA
        //            var resultDA = result
        //                .Where(x => x.Da.ToLower()
        //                .Split(',')
        //                .Any(da => da.Trim() == daName.ToLower().Trim()))
        //                .ToList();

        //            // Filter results for Reporting Head
        //            var resultManager = result
        //                .Where(x => x.Reportinghead.ToLower().Trim() == daName.ToLower().Trim())
        //                .ToList();

        //            // Combine both filtered results
        //            result = resultDA.Union(resultManager).ToList();
        //        }

        //        return result;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //public async Task<List<DataForEmployeeReport>> GetEmployeeDetailsView(int? id)
        //{
        //    try
        //    {
        //        id = 5768;
        //        // Fetch DA Name based on Employee ID
        //        string daName = id.HasValue && id > 0
        //            ? await _context.Rmsemployees
        //                .Where(x => x.Employeeid == id)
        //                .Select(x => x.Employeename)
        //                .FirstOrDefaultAsync()
        //            : string.Empty;

        //        // Fetch filtered employee report data
        //        var query = _context.DataForEmployeeReports.AsQueryable();

        //        //if (id > 0)
        //        //{
        //        //    query = query.Where(x =>
        //        //        (x.Contractenddate > DateTime.Now && x.Categorysubstatusid != 11) ||
        //        //        (x.Contractenddate <= DateTime.Now &&
        //        //         (x.Contractstatusid != 6 && x.Categorysubstatusid != 11)));
        //        //}
        //        if (id > 0)
        //        {
        //            query = 
        //                query.Where(x =>
        //                (x.Contractenddate > DateTime.Now && x.Categorysubstatusid != 11) ||
        //                (x.Contractenddate <= DateTime.Now &&
        //                 x.Contractstatusid != 6 && x.Categorysubstatusid != 11));
        //        }


        //        var result = await query.ToListAsync();

        //        if (!string.IsNullOrEmpty(daName))
        //        {
        //            string daNameLower = daName.ToLower().Trim();
        //            var resultDA = result
        //                .Where(x => x.Da.ToLower()
        //                    .Split(',')
        //                    .Any(da => da.Trim() == daNameLower))
        //                .ToList();

        //            var resultManager = result
        //                .Where(x => x.Reportinghead.ToLower().Trim() == daNameLower)
        //                .ToList();

        //            result = resultDA.Union(resultManager).ToList();
        //        }

        //        // Grouping and transforming the data
        //        var groupedResult = result
        //            .GroupBy(x => new { x.Employeeid, x.Resourcename })
        //            .Select(g => new DataForEmployeeReport
        //            {
        //                Employeeid = g.Key.Employeeid,
        //                Resourcename = g.Key.Resourcename,
        //                Tmc = string.Join(", ", g.Select(x => x.Tmc).Distinct()),
        //                Practice = string.Join(", ", g.Select(x => x.Practice).Distinct()),
        //                Subpractice = string.Join(", ", g.Select(x => x.Subpractice).Distinct()),
        //                Region = string.Join(", ", g.Select(x => x.Region).Distinct()),
        //                Function = string.Join(", ", g.Select(x => x.Function).Distinct()),
        //                Flag = string.Join(", ", g.Select(x => x.Flag).Distinct()),
        //                Billablenonbillable = string.Join(", ", g.Select(x => x.Billablenonbillable).Distinct()),
        //                Contractstartdate = g.Min(x => x.Contractstartdate),
        //                Contractenddate = g.Max(x => x.Contractenddate),
        //                Projectnames = string.Join(", ", g.Select(x => x.Projectnames).Distinct()),
        //                Projecttypes = string.Join(", ", g.Select(x => x.Projecttypes).Distinct()),
        //                Customernames = string.Join(", ", g.Select(x => x.Customernames).Distinct()),
        //                Da = string.Join(", ", g.Select(x => x.Da).Distinct()),
        //                Userid = string.Join(", ", g.Select(x => x.Userid).Distinct()),
        //                Currentmonthbilling = g.Sum(x => x.Currentmonthbilling ?? 0),
        //                Ade = g.FirstOrDefault()?.Ade,
        //                Dateofjoining = string.Join(", ", g.Select(x => x.Dateofjoining).Distinct()),
        //                Workexedays = g.Sum(x => x.Workexedays ?? 0),
        //                Billingmonthyear = string.Join(", ", g.Select(x => x.Billingmonthyear).Distinct()),
        //                Exe = string.Join(", ", g.Select(x => x.Exe).Distinct()),
        //                Reportinghead = string.Join(", ", g.Select(x => x.Reportinghead).Distinct()),
        //                Categorysubstatusid = g.FirstOrDefault()?.Categorysubstatusid,
        //                Contractstatusid = g.FirstOrDefault()?.Contractstatusid
        //            })
        //            .ToList();

        //        return groupedResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (logging framework is recommended)
        //        throw new Exception("An error occurred while fetching employee details.", ex);
        //    }
        //}

        public async Task<List<DataForEmployeeReport>> GetEmployeeDetailsView(int? id)
        {
            try
            {
                //id = 8447;
                // Fetch DA Name based on Employee ID
                string daName = id.HasValue && id > 0
                    ? await _context.Rmsemployees
                        .Where(x => x.Employeeid == id)
                        .Select(x => x.Employeename)
                        .FirstOrDefaultAsync()
                    : string.Empty;

                // Fetch Report Head Name based on Employee ID
                string reportheadname = id.HasValue && id > 0
                    ? await _context.Rmsemployees
                        .Where(a => a.Employeeid == id)
                        .Select(a => a.Employeename)
                        .FirstOrDefaultAsync()
                    : string.Empty;

                var query = _context.DataForEmployeeReports.AsQueryable();

                // Apply filters only if id is provided and greater than 0
                if (id > 0)
                {
                    // Fetch users with valid contract conditions
                    var validContracts = query.Where(x =>
                        (x.Contractenddate > DateTime.Now && x.Categorysubstatusid != 11) ||
                        (x.Contractenddate <= DateTime.Now &&
                         x.Contractstatusid != 6 && x.Categorysubstatusid != 11));

                    // Fetch users reported to the DA, even if they have no contracts
                    var reportedUsers = query.Where(x =>
                        !string.IsNullOrEmpty(reportheadname) &&
                        x.Reportinghead.ToLower().Trim() == reportheadname.ToLower().Trim());

                    // Combine both conditions
                    query = validContracts.Union(reportedUsers);
                }

                var result = await query.ToListAsync();

                // Filter results based on DA Name
                if (!string.IsNullOrEmpty(daName))
                {
                    string daNameLower = daName.ToLower().Trim();
                    var resultDA = result
                        .Where(x => x.Da.ToLower()
                            .Split(',')
                            .Any(da => da.Trim() == daNameLower))
                        .ToList();

                    var resultManager = result
                        .Where(x => x.Reportinghead.ToLower().Trim() == daNameLower)
                        .ToList();

                    result = resultDA.Union(resultManager).ToList();
                }

                // Grouping and transforming the data
                var groupedResult = result
                    .GroupBy(x => new { x.Employeeid, x.Resourcename })
                    .Select(g => new DataForEmployeeReport
                    {
                        Employeeid = g.Key.Employeeid,
                        Resourcename = g.Key.Resourcename,
                        Tmc = string.Join(", ", g.Select(x => x.Tmc).Distinct()),
                        Practice = string.Join(", ", g.Select(x => x.Practice).Distinct()),
                        Subpractice = string.Join(", ", g.Select(x => x.Subpractice).Distinct()),
                        Region = string.Join(", ", g.Select(x => x.Region).Distinct()),
                        Function = string.Join(", ", g.Select(x => x.Function).Distinct()),
                        Flag = string.Join(", ", g.Select(x => x.Flag).Distinct()),
                        Billablenonbillable = string.Join(", ", g.Select(x => x.Billablenonbillable).Distinct()),
                        Contractstartdate = g.Min(x => x.Contractstartdate),
                        Contractenddate = g.Max(x => x.Contractenddate),
                        Projectnames = string.Join(", ", g.Select(x => x.Projectnames).Distinct()),
                        Projecttypes = string.Join(", ", g.Select(x => x.Projecttypes).Distinct()),
                        Customernames = string.Join(", ", g.Select(x => x.Customernames).Distinct()),
                        Da = string.Join(", ", g.Select(x => x.Da).Distinct()),
                        Userid = string.Join(", ", g.Select(x => x.Userid).Distinct()),
                        Currentmonthbilling = g.Sum(x => x.Currentmonthbilling ?? 0),
                        Ade = g.FirstOrDefault()?.Ade,
                        Dateofjoining = string.Join(", ", g.Select(x => x.Dateofjoining).Distinct()),
                        Workexedays = g.Sum(x => x.Workexedays ?? 0),
                        Billingmonthyear = string.Join(", ", g.Select(x => x.Billingmonthyear).Distinct()),
                        Exe = string.Join(", ", g.Select(x => x.Exe).Distinct()),
                        Reportinghead = string.Join(", ", g.Select(x => x.Reportinghead).Distinct()),
                        Categorysubstatusid = g.FirstOrDefault()?.Categorysubstatusid,
                        Contractstatusid = g.FirstOrDefault()?.Contractstatusid
                    })
                    .ToList();

                return groupedResult;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching employee details.", ex);
            }
        }



        public async Task<List<DaListDto>> DaList()
        {
            try
            {

                var delieveryanchorids = await _context.Projectcontracts
                                                       .Select(x => x.Deliveryanchorid)
                                                       .Distinct()
                                                       .ToListAsync();


                var result = await _context.Rmsemployees
                                           .Where(x => delieveryanchorids.Contains(x.Employeeid))
                                           .Select(x => new DaListDto
                                           {
                                               DaId = x.Employeeid,
                                               DaName = x.Employeename
                                           })
                                           .ToListAsync();

                return result;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<employeeResponseDto>> GetEmployeesBySkill(int skillId)
        {
            try
            {
                var employeeQuery = await _context.Rmsemployees
                    .AsNoTracking()
                    .Include(e => e.Employeeskills)
                    .Where(e => e.Employeeskills.Any(es => es.Skillid == skillId)) // Use Any() to check if the employee has the skill
                    .Select(e => new employeeResponseDto
                    {
                        Employeeid = e.Employeeid,
                        TmcId = e.Userid,
                        Employeename = e.Employeename
                    })
                    .ToListAsync();

                return employeeQuery;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching employees", ex);
            }
        }

    }
}