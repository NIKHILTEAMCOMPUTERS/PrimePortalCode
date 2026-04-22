using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Timesheets;
using RMS.Service.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Service.Repositories.Timesheets
{
    public class TimesheetRepository : GenericRepository<Timesheet>, ITimesheetRepository
    {
        private readonly RmsDevContext _context;
        IConfiguration _config;
        Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        public TimesheetRepository(RmsDevContext context, IConfiguration configuration,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment environment) : base(context)
        {
            _env = environment;
            _config = configuration;
            _context = context;
        }

        //public async Task<GenericRepository<List<Timesheetheader>> GetHeaderListAsync( JwtLoginDetailDto logindetails)
        //{

        //    try
        //    {
        //        var employee = await _context.Rmsemployees.FirstOrDefaultAsync(x => x.Userid == logindetails.TmcId);
        //        if (employee == null) { throw new Exception($"No user exists at the given TMC {logindetails.TmcId}"); }
        //        var result =await _context.Timesheetheaders.Where(x=>x.Createdby== employee.Employeeid).ToListAsync();
        //        return new GenericRepository<List<Timesheetheader>>
        //        {
        //            res
        //        }

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return null;
        //}

        public async Task<GenericResponse<Timesheetheader>> AddHeaderAsync(string monthyear, JwtLoginDetailDto userdetails)
        {
            try
            {
                var employee = await _context.Rmsemployees.FirstOrDefaultAsync(x => x.Userid == userdetails.TmcId);
                if (employee == null)
                {
                    throw new Exception($"Employee does not exists at given TMC={userdetails.TmcId}  inside the resource managment system.");
                }
                if (await _context.Timesheetheaders.AnyAsync(x => x.Employeeid == employee.Employeeid && x.Monthyear == monthyear))
                {
                    throw new Exception($"Timesheet header details alredy exists for the given userid={employee.Employeeid} and the monthyear={employee.Employeeid}");
                }


                var addHeader = new Timesheetheader
                {
                    Monthyear = monthyear,
                    Employeeid = employee.Employeeid,
                    Createddate = DateTime.Now,
                    Lastupdatedate = DateTime.Now,
                    Createdby = employee.Employeeid,
                    Lastupdateby = employee.Employeeid
                };

                await _context.Timesheetheaders.AddAsync(addHeader);
                await _context.SaveChangesAsync();
                return new GenericResponse<Timesheetheader>
                {
                    responseCode = 200,
                    responseMessage = $"Timesheet for Month-Year {monthyear} has been created successfully!",
                    DataObject = addHeader // Include the created timesheet header in the response
                };

            }
            catch (Exception ex)
            {
                return new GenericResponse<Timesheetheader>
                {
                    responseCode = 500,
                    responseMessage = ex.Message
                };
            }
        }

        //   public async Task<GenericResponse<List<Timesheetheader>>> GetHeaderAsync(JwtLoginDetailDto userdetails)
        //   {
        //       var timesheader = new List<Timesheetheader>();
        //       int groupedCount = 0;
        //       try
        //       {
        //           var employee = await _context.Rmsemployees.FirstOrDefaultAsync(x => x.Userid == userdetails.TmcId);
        //           if (employee == null)
        //           {
        //               return new GenericResponse<List<Timesheetheader>>
        //               {
        //                   responseCode = 500,
        //                   responseMessage = ($"Employee does not exist at given TMC={userdetails.TmcId} inside the resource management system.")
        //               };
        //           }

        //           timesheader = await _context.Timesheetheaders.Include(x => x.Timesheetdetails)
        //               .Where(x => x.Employeeid == employee.Employeeid)
        //               .ToListAsync();

        //                                groupedCount = await _context.Timesheetdetails
        //                    .Where(x => timesheader.Select(t => t.Timesheetid).Contains(x.Timesheetid) && x.Isdeleted == false)
        //                    .GroupBy(x => x.Timesheetdate.Date) // Ensuring you're grouping by only the date part
        //                    .CountAsync();
        //           var monthlyEntries = await _context.Timesheetdetails
        //.Where(x => timesheader.Select(t => t.Timesheetid).Contains(x.Timesheetid) && x.Isdeleted == false)
        //.GroupBy(x => new { x.Timesheetdate.Year, x.Timesheetdate.Month, x.Timesheetdate.Date }) // Grouping by Year, Month, and Date
        //.Select(g => new { g.Key.Year, g.Key.Month }) // Only selecting Year and Month for the final grouping
        //.GroupBy(g => new { g.Year, g.Month })        // Group by Year and Month again
        //.Select(g => new
        //{
        //    Year = g.Key.Year,
        //    Month = g.Key.Month,
        //    EntryCount = g.Count() // Counting unique days within each month
        //})
        //.ToListAsync();
        //       }
        //       catch (Exception ex)
        //       {
        //           return new GenericResponse<List<Timesheetheader>>
        //           {
        //               responseCode = 500,
        //               responseMessage = ex.Message
        //           };
        //       }

        //       return new GenericResponse<List<Timesheetheader>>
        //       {
        //           responseCode = 200,
        //           responseMessage = $"Data retrieved successfully. Grouped count of unique dates: {groupedCount}",
        //           DataObject = timesheader
        //       };
        //   }
        public async Task<GenericResponse<List<Timesheetheader>>> GetHeaderAsync(JwtLoginDetailDto userdetails)
        {
            var timesheader = new List<Timesheetheader>();
            try
            {
                var employee = await _context.Rmsemployees.FirstOrDefaultAsync(x => x.Userid == userdetails.TmcId);
                if (employee == null)
                {
                    return new GenericResponse<List<Timesheetheader>>
                    {
                        responseCode = 500,
                        responseMessage = $"Employee does not exist at given TMC={userdetails.TmcId} inside the resource management system."
                    };
                }

                timesheader = await _context.Timesheetheaders.Include(x => x.Timesheetdetails)
                    .Where(x => x.Employeeid == employee.Employeeid)
                    .ToListAsync();

                // Get the grouped count of unique dates
                var currentYear = DateTime.Now.Year;

                var groupedCount = await _context.Timesheetdetails
                    .Where(x => timesheader.Select(t => t.Timesheetid).Contains(x.Timesheetid)
                                && x.Isdeleted != true
                                )  // Filter for the current year
                    .GroupBy(x => x.Timesheetdate.Date)
                    .CountAsync();

                // Get the entry count for each month, considering unique dates
                var monthlyEntryCounts = await _context.Timesheetdetails
                    .Where(x => timesheader.Select(t => t.Timesheetid).Contains(x.Timesheetid) && x.Isdeleted != true)
                    .GroupBy(x => new { x.Timesheetdate.Year, x.Timesheetdate.Month })
                    .Select(g => new MonthlyEntryCount
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        EntryCount = g.Select(x => x.Timesheetdate.Date).Distinct().Count() // Count unique dates
                    })
                    .ToListAsync();

                var leaveCount = _context.Timesheetdetails
                                 .Count(td => td.Categoryofactivityid == 19
                                 && td.Isdeleted != true
                                    && _context.Timesheetheaders.Any(th => th.Timesheetid == td.Timesheetid
                                                        && th.Employeeid == employee.Employeeid
                                                        && th.Timesheetdetails.Any(a => a.Timesheetdate.Year == currentYear))); // Filter by current year

                return new GenericResponse<List<Timesheetheader>>
                {
                    responseCode = 200,
                    responseMessage = $"Data retrieved successfully. Grouped count of unique dates: {groupedCount}",
                    DataObject = timesheader,
                    data = new
                    {
                        monthlyEntryCounts,
                        leaveCount
                    } // Returning monthly entry counts here
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<Timesheetheader>>
                {
                    responseCode = 500,
                    responseMessage = ex.Message
                };
            }
        }


        public class MonthlyEntryCount
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int EntryCount { get; set; }
            public string MonthYear => $"{GetMonthName(Month).ToLower()}-{Year.ToString().Substring(2)}"; // e.g., "jun-24"

            private string GetMonthName(int month)
            {
                return new DateTime(1, month, 1).ToString("MMM", CultureInfo.InvariantCulture);
            }
        }




        public async Task<GenericResponse<TimesheetDTO>> UpsertHeaderDetailItemsAsync(TimeSheetItemDto value, JwtLoginDetailDto userdetails)
        {
            //var onBenchProjectList = new List<string>() {"onbench","pre-sales","poc" };
            var onBenchProjectList = new List<string>() {"nodata" };
            var existingContract = new Projectcontract();
            string? OnBechStatus=null;
            try
            {


                if (value is null) throw new ArgumentNullException(nameof(value), "The input object cannot be null.");

                var employee = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(x => x.Userid == userdetails.TmcId)
                    ?? throw new KeyNotFoundException($"Employee does not exist with TMC ID = {userdetails.TmcId}.");

                if (value.Dayhours > 15) return new GenericResponse<TimesheetDTO> { responseCode = 400, responseMessage = "Working hours cannot be more than 14." };

                // Fetch the existing header
                var existingHeader = await _context.Timesheetheaders.AsNoTracking().FirstOrDefaultAsync(x => x.Timesheetid == value.Timesheetid)
                    ?? throw new KeyNotFoundException($"No Timesheet header exists with the ID {value.Timesheetid}.");

                // Format the date using invariant culture
                var monthYear = value.Timesheetdate?.ToString("MMM-yy", CultureInfo.InvariantCulture)
                    ?? throw new ArgumentException("Time sheet date cannot be null.");

                // Convert the month-year string if needed
                var validMonthYear = ConvertToValidDate(monthYear);
                var existingMonthYear = ConvertToValidDate(existingHeader.Monthyear);

                if (existingHeader.Monthyear != monthYear)
                {
                    return new GenericResponse<TimesheetDTO>
                    {
                        responseCode = 400,
                        responseMessage = $"Inconsistency in dates: header is for {existingHeader.Monthyear}, but detail item is for {monthYear}."
                    };
                }

                var project = await _context.Projects.AsNoTracking().Include(x=>x.Customer)
                                            .FirstOrDefaultAsync(x => x.Projectid == value.ProjectId)
                                            ?? throw new KeyNotFoundException($"No project exists with the ID {value.ProjectId}.");

                if (onBenchProjectList.Contains(project.Projectname, StringComparer.OrdinalIgnoreCase))
                {
                    OnBechStatus = project.Projectname;
                    existingContract=null;
                }
                else
                {
                    existingContract = await _context.Projectcontracts.AsNoTracking()
                        .Include(x => x.Project)
                        .FirstOrDefaultAsync(x => x.Projectid == value.ProjectId)
                        ?? throw new KeyNotFoundException("No contract found for the given projectid.");
                }

                Timesheetdetail addItem;
                if (value.Timesheetdetailid == 0 || value.Timesheetdetailid == null)
                {
                    addItem = new Timesheetdetail
                    {
                        Timesheetid = value.Timesheetid,
                        Departmentid = employee.Departmentid,
                        Contractid = existingContract == null ? null : existingContract.Contractid,
                        Activity = value.Activity,
                        Dayhours = value.Dayhours,
                        Remarks = value.Remarks,
                        Isdrafted = true,
                        Timesheetdate = value.Timesheetdate ?? throw new ArgumentException("Time sheet date cannot be null."),
                        Createdby = employee.Employeeid,
                        Createddate = DateTime.Now,
                        Benchstatus = existingContract == null ? OnBechStatus : null,
                        Categoryofactivityid = value.categoryofactivityid
                    };
                    await _context.Timesheetdetails.AddAsync(addItem);
                }
                else
                {
                    addItem = await _context.Timesheetdetails.FirstOrDefaultAsync(x => x.Timesheetdetailid == value.Timesheetdetailid)
                       ?? throw new KeyNotFoundException($"No Timesheet detail exists with the ID {value.Timesheetdetailid}.");

                    addItem.Departmentid = value.Departmentid.Value;
                    addItem.Contractid = value.Contractid == null ? (existingContract==null?null: existingContract.Contractid) : value.Contractid.Value;
                    addItem.Activity = string.IsNullOrEmpty(value.Activity) ? addItem.Activity : value.Activity;
                    addItem.Dayhours = value.Dayhours == null ? addItem.Dayhours : value.Dayhours;
                    addItem.Remarks = string.IsNullOrEmpty(value.Remarks) ? addItem.Remarks : value.Remarks;
                    addItem.Isdrafted = value.Isdrafted ?? true;
                    addItem.Timesheetdate = value.Timesheetdate ?? addItem.Timesheetdate;
                    addItem.Lastupdateby = employee.Employeeid;
                    addItem.Lastupdatedate = DateTime.Now;
                    addItem.Benchstatus= existingContract == null?OnBechStatus:null;
                    addItem.Categoryofactivityid = value.categoryofactivityid.Value;
                    _context.Timesheetdetails.Update(addItem);
                }

                await _context.SaveChangesAsync();

                return new GenericResponse<TimesheetDTO>
                {
                    responseCode = 200,
                    responseMessage = $"Timesheet item for date {value.Timesheetdate:d} has been processed successfully!",
                    //Data = addItem
                    DataObject = new TimesheetDTO()
                    {
                        Timesheetdetailid=addItem.Timesheetdetailid,
                        Timesheetid=addItem.Timesheetid,
                        Departmentid=addItem.Departmentid,
                        Contractid = addItem.Contractid,
                        Activity= addItem.Activity,
                        Dayhours = addItem.Dayhours ,
                        Remarks= addItem.Remarks ,
                        Isdrafted = addItem.Isdrafted ,
                        Isactive = addItem.Isactive ,
                        Isdeleted = addItem.Isdeleted  ,
                        Createddate = addItem.Createddate  ,
                        Lastupdatedate = addItem.Lastupdatedate ,
                        Createdby = addItem.Createdby   ,
                        Lastupdateby = addItem.Lastupdateby ,
                        Timesheetdate = addItem.Timesheetdate  ,
                        Benchstatus = addItem.Benchstatus  ,
                        Contract= addItem.Contract ,
                        Department=addItem.Department ,
                        Timesheet=addItem.Timesheet ,   
                        CustomerId= project.Customer.Customerid ,
                        Companyname=project.Customer.Companyname,
                        ProjectId = project.Projectid,
                        Projectname=project.Projectname, 
                        categoryofactivityid = addItem.Categoryofactivityid
                    }


                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //nick imp method
                private string ConvertToValidDate(string Monthyear)
                {
                    // Replace "Sept" with "Sep" if present
                    return Monthyear.Replace("Sept", "Sep");
                }


        public async Task<GenericResponse<string>> SubmitTimeSheetDetailItemsAsync(
         List<int> timesheetDetailIds,
         JwtLoginDetailDto userdetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                DateTime today = DateTime.Today;
                DateTime allowedFromDate = today.AddDays(-2); // today + last 2 days = max 3 days

                var employee = await _context.Rmsemployees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Userid == userdetails.TmcId);

                if (employee == null)
                {
                    return new GenericResponse<string>
                    {
                        responseCode = 404,
                        responseMessage = $"Employee does not exist with TMC ID = {userdetails.TmcId}.",
                        DataObject = null
                    };
                }

                // Fetch timesheet records once
                var timesheets = await _context.Timesheetdetails
                    .Where(td => timesheetDetailIds.Contains(td.Timesheetdetailid))
                    .ToListAsync();

                if (timesheets.Count != timesheetDetailIds.Count)
                {
                    return new GenericResponse<string>
                    {
                        responseCode = 400,
                        responseMessage = "One or more timesheet detail records were not found.",
                        DataObject = null
                    };
                }

                // 🚫 Enforce 3-day rule
                if (timesheets.Any(td => td.Timesheetdate < allowedFromDate))
                {
                    return new GenericResponse<string>
                    {
                        responseCode = 400,
                        responseMessage = "You can submit timesheets only for the last 3 days (including today).",
                        DataObject = null
                    };
                }

                // ✅ Submit timesheets
                foreach (var item in timesheets)
                {
                    item.Isdrafted = false;
                    item.Lastupdateby = employee.Employeeid;
                    item.Lastupdatedate = DateTime.Now;
                }

                _context.Timesheetdetails.UpdateRange(timesheets);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // 📧 Send mail (safe after commit)
                await Timesheetmailnotification(timesheetDetailIds, userdetails);

                return new GenericResponse<string>
                {
                    responseCode = 200,
                    responseMessage = "Timesheet has been submitted successfully!",
                    DataObject = null
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return new GenericResponse<string>
                {
                    responseCode = 500,
                    responseMessage = $"An error occurred while submitting timesheet: {ex.Message}",
                    DataObject = null
                };
            }
        }



        public async Task<GenericResponse<string>> DeleteTimeSheetDetailItemsAsync(int timesheetDetailId, JwtLoginDetailDto userdetails)
        {

            var employee = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(x => x.Userid == userdetails.TmcId)
                ?? throw new KeyNotFoundException($"Employee does not exist with TMC ID = {userdetails.TmcId}.");

            var detail = await _context.Timesheetdetails.FirstOrDefaultAsync(td => td.Timesheetdetailid == timesheetDetailId)
                 ?? throw new KeyNotFoundException($"No Timesheet detail exists with the ID {timesheetDetailId}.");



            var data = await _context.Timesheetdetails.FirstOrDefaultAsync(x => x.Timesheetdetailid == timesheetDetailId);

            data.Isdeleted = true;
            data.Lastupdateby = employee.Employeeid;
            data.Lastupdatedate = DateTime.Now;
            _context.Timesheetdetails.Update(data);

            await _context.SaveChangesAsync();

            return new GenericResponse<string>
            {
                responseCode = 200,
                responseMessage = "Timesheet has been deleted successfully!",
                DataObject = null,
            };
        }

        public async Task<GenericResponse<List<object>>> GetTimesheetByManagerId(string monthYear, int projectId, int employeeId, JwtLoginDetailDto userdetails)
        {
            var employee = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(x => x.Userid == userdetails.TmcId)
                ?? throw new KeyNotFoundException($"Employee does not exist with TMC ID = {userdetails.TmcId}.");

            //var response = await (from a in _context.Timesheetheaders
            //                      join c in _context.Timesheetdetails on a.Timesheetid equals c.Timesheetid
            //                      join b in _context.Rmsemployees on a.Employeeid equals b.Employeeid
            //                      join d in _context.Projectcontracts on c.Contractid equals d.Contractid
            //                      join e in _context.Projects on d.Projectid equals e.Projectid
            //                      join q in _context.Categoryofactivities on c.Categoryofactivityid equals q.Categoryofactivityid
            //                      where b.Reportheadid == userdetails.TmcId && a.Monthyear == monthYear && !c.Isdeleted.Value && !c.Isdrafted.Value
            //                      && (projectId == 0 || e.Projectid == projectId) &&
            //                         (employeeId == 0 || b.Employeeid == employeeId)
            //                      orderby a.Employeeid, c.Timesheetdate
            //                      select new
            //                      {
            //                          a.Employeeid,
            //                          b.Userid,
            //                          b.Employeename,
            //                          b.Companyemail,
            //                          c.Timesheetdetailid,
            //                          c.Timesheetid,
            //                          e.Projectname,
            //                          c.Timesheetdate,
            //                          c.Dayhours,
            //                          c.Activity,
            //                          q.Categoryofactivityname
            //                      }).ToListAsync();


            var subordinateIds = await GetTwoLevelSubordinateIds(userdetails.TmcId);


            var response = await (from a in _context.Timesheetheaders
                                  join c in _context.Timesheetdetails on a.Timesheetid equals c.Timesheetid
                                  join b in _context.Rmsemployees on a.Employeeid equals b.Employeeid
                                  join d in _context.Projectcontracts on c.Contractid equals d.Contractid
                                  join e in _context.Projects on d.Projectid equals e.Projectid
                                  join q in _context.Categoryofactivities on c.Categoryofactivityid equals q.Categoryofactivityid
                                  where subordinateIds.Contains(b.Userid) &&
                                        a.Monthyear == monthYear &&
                                        !c.Isdeleted.Value &&
                                        !c.Isdrafted.Value &&
                                        (projectId == 0 || e.Projectid == projectId) &&
                                        (employeeId == 0 || b.Employeeid == employeeId)
                                  orderby a.Employeeid, c.Timesheetdate
                                  select new
                                  {
                                      a.Employeeid,
                                      b.Userid,
                                      b.Employeename,
                                      b.Companyemail,
                                      c.Timesheetdetailid,
                                      c.Timesheetid,
                                      e.Projectname,
                                      c.Timesheetdate,
                                      c.Dayhours,
                                      c.Activity,
                                      q.Categoryofactivityname
                                  }).ToListAsync();

            return new GenericResponse<List<object>>
            {
                responseCode = 200,
                responseMessage = "",
                DataObject = response.Cast<dynamic>().ToList(),
            };
        }

        private async Task<List<string>> GetTwoLevelSubordinateIds(string managerId)
        {
            var levelOneIds = await _context.Rmsemployees
                .Where(x => x.Reportheadid == managerId)
                .Select(x => x.Userid)
                .ToListAsync();

            var levelTwoIds = await _context.Rmsemployees
                .Where(x => levelOneIds.Contains(x.Reportheadid))
                .Select(x => x.Userid)
                .ToListAsync();

            return levelOneIds.Concat(levelTwoIds).Distinct().ToList();

            //return levelOneIds;
        }
        public async Task<List<Project>> GetProjectsWithDetailsByTimesheet(int Employeeid)
        {
            try
            {
                var result = await _context.Projects.AsNoTracking()

             .Where(a => a.Projectcontracts
                 .Any(pc => pc.Contractemployees
                     .Any(ce => ce.Employee.Employeeid == Employeeid)))
             .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //BY ABHISHEK PROVIDED ON 08/APR
        public async Task<GenericResponse<List<TimesheetDTO>>> GetListofTimesheetDetailByHeaderid(int HeaderId)
        {
            var returnedResult=new List<TimesheetDTO>(); 

            if (HeaderId == 0)
            {
                return new GenericResponse<List<TimesheetDTO>>
                {
                    responseCode = 400,
                    responseMessage = "could not find records at HeaderId Zero"

                };
            }
            var result = await _context.Timesheetdetails.Include(x => x.Contract).ThenInclude(x => x.Project)
                .ThenInclude(x => x.Customer)
            .Where(x => x.Timesheetid == HeaderId && x.Isdeleted == false)
            .ToListAsync();
            var projects = await _context.Projects
                                            .AsNoTracking()
                                            .Include(x => x.Customer)
                                            .ToListAsync();


            foreach (var item in result)
            {

                returnedResult.Add( new TimesheetDTO
                {
                    Timesheetdetailid = item.Timesheetdetailid,
                    Timesheetid = item.Timesheetid,
                    Departmentid = item.Departmentid,
                    Contractid = item.Contractid,
                    Activity = item.Activity,
                    Dayhours = item.Dayhours,
                    Remarks = item.Remarks,
                    Isdrafted = item.Isdrafted,
                    Isactive = item.Isactive,
                    Isdeleted = item.Isdeleted,
                    Createddate = item.Createddate,
                    Lastupdatedate = item.Lastupdatedate,
                    Createdby = item.Createdby,
                    Lastupdateby = item.Lastupdateby,
                    Timesheetdate = item.Timesheetdate,
                    Benchstatus = item.Benchstatus,
                    Contract = item.Contract,
                    Department = item.Department,
                    Timesheet = item.Timesheet,
                    CustomerId = item.Contract == null ? projects.FirstOrDefault(x => x.Projectname.ToLower() == item.Benchstatus.ToLower())?.Customerid : item.Contract.Project.Customer.Customerid,
                    Companyname = item.Contract == null ? projects.FirstOrDefault(x => x.Projectname.ToLower() == item.Benchstatus.ToLower())?.Customer.Companyname : item.Contract.Project.Customer.Companyname,
                    ProjectId = item.Contract == null ? projects.FirstOrDefault(x => x.Projectname.ToLower() == item.Benchstatus.ToLower())?.Projectid : item.Contract.Project.Projectid,
                    Projectname= item.Contract == null ? projects.FirstOrDefault(x => x.Projectname.ToLower() == item.Benchstatus.ToLower())?.Projectname : item.Contract.Project.Projectname,
                    categoryofactivityid = item.Categoryofactivityid,
                    
                });
            }




            //returnedResult = result.Select(item => new TimesheetDTO
            //{
            //    Timesheetdetailid = item.Timesheetdetailid,
            //    Timesheetid = item.Timesheetid,
            //    Departmentid = item.Departmentid,
            //    Contractid = item.Contractid,
            //    Activity = item.Activity,
            //    Dayhours = item.Dayhours,
            //    Remarks = item.Remarks,
            //    Isdrafted = item.Isdrafted,
            //    Isactive = item.Isactive,
            //    Isdeleted = item.Isdeleted,
            //    Createddate = item.Createddate,
            //    Lastupdatedate = item.Lastupdatedate,
            //    Createdby = item.Createdby,
            //    Lastupdateby = item.Lastupdateby,
            //    Timesheetdate = item.Timesheetdate,
            //    Benchstatus = item.Benchstatus,
            //    Contract = item.Contract,
            //    Department = item.Department,
            //    Timesheet = item.Timesheet,
            //    CustomerId = item.Contract == null ? projects.FirstOrDefault(x => x.Projectname.ToLower() == item.Benchstatus.ToLower())?.Customerid : item.Contract.Project.Customer.Customerid,
            //    Companyname = item.Contract == null ? projects.FirstOrDefault(x => x.Projectname.ToLower() == item.Benchstatus.ToLower())?.Customer.Companyname : item.Contract.Project.Customer.Companyname,
            //    ProjectId = item.Contract == null ? projects.FirstOrDefault(x => x.Projectname.ToLower() == item.Benchstatus.ToLower())?.Projectid : item.Contract.Project.Projectid
            //}).ToList();

            return new GenericResponse<List<TimesheetDTO>>
            {
                responseCode = result.Any() ? 200 : 400, // Use .Any() for clarity
                responseMessage = result.Any() ? "Records found." : "No records found.",
                DataObject = returnedResult
            };
        }    
        public async Task<GenericResponse<Timesheetdetail>> Timesheetdecline(List<TimeSheetItemDto> items, JwtLoginDetailDto userdetails)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var response = new GenericResponse<Timesheetdetail>();

                try
                {
                    var employee = await _context.Rmsemployees.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Userid == userdetails.TmcId)
                        ?? throw new KeyNotFoundException($"No employee exists with the user ID {userdetails.TmcId}.");

                    foreach (var item in items)
                    {
                        var timesheetDetail = await _context.Timesheetdetails
                            .FirstOrDefaultAsync(a => a.Timesheetdetailid == item.Timesheetdetailid);

                        if (timesheetDetail == null)
                        {
                            throw new Exception($"No Timesheet detail exists with the ID {item.Timesheetdetailid}.");
                        }

                        timesheetDetail.Remarks = item.Remarks ?? "Decline By Manager";
                        timesheetDetail.Isdrafted = true;
                        timesheetDetail.Lastupdateby = employee.Employeeid;
                        timesheetDetail.Lastupdatedate = DateTime.Now;

                        _context.Timesheetdetails.Update(timesheetDetail);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    response.responseCode = 200;
                    response.responseMessage = "All timesheet items have been processed successfully!";
                    return response;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new GenericResponse<Timesheetdetail>
                    {
                        responseCode = 500,
                        responseMessage = $"An error occurred: {ex.Message}",
                        DataObject = null
                    };
                }
            }
        }


        public async Task<GenericResponse<List<Categoryofactivity>>> Getcategoryofactivity()
        {
            var categoryofactivities = new List<Categoryofactivity>();
            try
            {

                categoryofactivities = await _context.Categoryofactivities.ToListAsync();


            }
            catch (Exception ex)
            {
                return new GenericResponse<List<Categoryofactivity>>
                {
                    responseCode = 500,
                    responseMessage = ex.Message
                };
            }
            return new GenericResponse<List<Categoryofactivity>>
            {
                responseCode = 200,
                responseMessage = "",
                DataObject = categoryofactivities
            };
        }
        public async Task<IEnumerable<TimesheetHRExcelDto>> GetTdeTimesheetCount(string monthYear)
        {
            try
            {

                var filteredResult = new List<TimesheetHRExcelDto>();

                var query = await _context.TimesheetHRExcelDto
                               .FromSqlRaw("SELECT * FROM get_hr_timesheet_data() WHERE monthyear = {0}", monthYear)
                                       .ToListAsync();

                if (query == null || !query.Any()) return new List<TimesheetHRExcelDto>();


                return query.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #region timesheetmailnotify
        public class TimesheetDetailnotifyDto
        {
            public int Timesheetdetailid { get; set; }
            public DateTime Timesheetdate { get; set; }
            public double Dayhours { get; set; }
            public string Activity { get; set; }
            public string Categoryofactivityname { get; set; }
            public string ContractName { get; set; }
            public string ProjectName { get; set; }
            public string DeliveryAnchorEmail { get; set; }
        }

        public async Task<bool> Timesheetmailnotification(List<int> timesheetDetailIds, JwtLoginDetailDto userdetails)
        {
            bool mailSend = false;
            decimal totalHoursPerDate = 0m;

            DateTime today = DateTime.Today;
            DateTime fromDate = today.AddDays(-2);   // last 3 days → today, -1, -2

            var employeeId = await _context.Rmsemployees
                .Where(e => e.Userid == userdetails.TmcId)
                .Select(e => e.Employeeid)
                .FirstOrDefaultAsync();

            // 👉 Only fetch last 3 days data
            var groupedByDate = await _context.Timesheetdetails
                .Where(td => timesheetDetailIds.Contains(td.Timesheetdetailid)
                          && td.Timesheetdate >= fromDate
                          && td.Timesheetdate <= today)
                .GroupBy(td => td.Timesheetdate)
                .ToListAsync();

            var draftedDates = await _context.Timesheetdetails
                .Where(td => td.Isdrafted == false
                          && td.Isdeleted == false
                          && td.Timesheetdate >= fromDate
                          && td.Timesheetdate <= today
                          && _context.Timesheetheaders.Any(th =>
                                 th.Timesheetid == td.Timesheetid &&
                                 th.Employeeid == employeeId))
                .ToListAsync();

            foreach (var group in groupedByDate)
            {
                var groupDate = group.Key;

                var groupedHours = group.Sum(td => td.Dayhours.GetValueOrDefault());

                var draftedHours = draftedDates
                    .Where(td => td.Timesheetdate == groupDate &&
                                !group.Any(g => g.Timesheetdetailid == td.Timesheetdetailid))
                    .Sum(td => td.Dayhours.GetValueOrDefault());

                totalHoursPerDate = groupedHours + draftedHours;

                if (totalHoursPerDate > 8m)
                {
                    var filteredIds = group
                        .Select(td => td.Timesheetdetailid)
                        .Union(draftedDates
                            .Where(td => td.Timesheetdate == groupDate)
                            .Select(td => td.Timesheetdetailid))
                        .ToList();

                    try
                    {
                        await SenMailerTimesheet(true, filteredIds, userdetails);
                        mailSend = true;
                    }
                    catch (Exception ex)
                    {
                        // optional logging
                    }
                }
            }

            return mailSend;
        }

        public async Task<bool> SenMailerTimesheet(bool isSend, List<int> timesheetDetailIds, JwtLoginDetailDto userdetails)
        {
            if (!isSend)
            {
                return false;
            }

            var timesheetDetails = await GetTimesheetDetail(timesheetDetailIds);
            var uniqueDates = timesheetDetails
               .Select(td => td.Timesheetdate)
               .Distinct()
               .OrderBy(d => d)
               .ToList();
            if (timesheetDetails == null || !timesheetDetails.Any())
            {
                return false;
            }
            var employeeName = await _context.Rmsemployees.Where(e => e.Userid == userdetails.TmcId).Select(e => e.Employeename).FirstOrDefaultAsync();

            var employeeEmail = await _context.Rmsemployees.Where(e => e.Userid == userdetails.TmcId).Select(e => e.Companyemail).FirstOrDefaultAsync();
            var managerId = await _context.Rmsemployees.Where(e => e.Userid == userdetails.TmcId).Select(e => e.Reportheadid).FirstOrDefaultAsync();
            var managerEmail = await _context.Rmsemployees.Where(e => e.Userid == managerId)
                                     .Select(e => e.Companyemail).ToListAsync();

            var formattedDate = uniqueDates.FirstOrDefault().ToString("dd/MM/yyyy") ?? string.Empty;

            //var deliveryAnchorEmail = timesheetDetails
            //    .FirstOrDefault(td => !string.IsNullOrWhiteSpace(td.DeliveryAnchorEmail))?.DeliveryAnchorEmail;

            var ccEmails = managerEmail.ToList();

            //if (!string.IsNullOrWhiteSpace(deliveryAnchorEmail))
            //{
            //    ccEmails.Add(deliveryAnchorEmail);
            //}

            var mailData = $"<p style='padding: .75rem 1rem; font-weight: bold; font-size: 1.25rem;'>Timesheet - ({formattedDate})</p>" +
               "<table border='1' cellpadding='5' cellspacing='0' style='width: 100%; border-collapse: collapse; border: 1px solid #e2e8f0; margin-bottom: 1rem;'>" +
               "<thead style='background-color: #e2e8f0;'><tr>" +
               "<th style='text-align: left; padding: .75rem 1rem; font-weight: bold; font-size: 1rem;'>Working Hours</th>" +
               "<th style='text-align: left; padding: .75rem 1rem; font-weight: bold; font-size: 1rem;'>Activity</th>" +
               "<th style='text-align: left; padding: .75rem 1rem; font-weight: bold; font-size: 1rem;'>Category Of Activity</th>" +
               "<th style='text-align: left; padding: .75rem 1rem; font-weight: bold; font-size: 1rem;'>Project</th>" +
               "<th style='text-align: left; padding: .75rem 1rem; font-weight: bold; font-size: 1rem;'>Contract</th>" +
               "</tr></thead>" +
               "<tbody>";

            foreach (var item in timesheetDetails)
            {
                mailData += $"<tr><td style='padding: 1rem;'>{item.Dayhours}</td><td style='padding: 1rem;'>{item.Activity}</td><td style='padding: 1rem;'>{item.Categoryofactivityname}</td><td style='padding: 1rem;'>{item.ProjectName}</td><td style='padding: 1rem;'>{item.ContractName}</td></tr>";
            }

            mailData += "</tbody></table>";

            var mailTemplatePath = Path.Combine(_env.WebRootPath, "Timesheet", "Mailer", "genericmailer.html");
            string mailBody = System.IO.File.Exists(mailTemplatePath) ? System.IO.File.ReadAllText(mailTemplatePath) : string.Empty;

            string sMailBody = string.Format(mailBody, DateTime.Now.ToString("yyyy"), mailData, employeeName+"("+userdetails.TmcId+")");
            string sMailSubject = $"Timesheet - {formattedDate} - ({employeeName})({userdetails.TmcId})";

            string sMailOutput;
            bool mailSuccess = SendMail(
                employeeEmail, // Employee's email
                sMailSubject,
                sMailBody,
                ccEmails,
                out sMailOutput
            );

            return mailSuccess;
        }


        

        public async Task<List<TimesheetDetailnotifyDto>> GetTimesheetDetail(List<int> timesheetDetailIds)
        {
            var timesheetDetails = await _context.Timesheetdetails
                .Where(td => timesheetDetailIds.Contains(td.Timesheetdetailid))
                .ToListAsync();

            //var draftedDates = await _context.Timesheetdetails
            //    .Where(_ => _.Isdrafted == true && _.Isdeleted == false)
            //    .Select(_ => _.Timesheetdate)
            //    .ToListAsync();

            var result = new List<TimesheetDetailnotifyDto>();
            if (!timesheetDetails.Any())
            {
                return result;
            }

            foreach (var item in timesheetDetails)
            {
                //if (!draftedDates.Contains(item.Timesheetdate))
                //{
                    var categoryOfActivity = await _context.Categoryofactivities
                        .Where(_ => _.Categoryofactivityid == item.Categoryofactivityid)
                        .Select(_ => _.Categoryofactivityname)
                        .FirstOrDefaultAsync();

                    var contract = item.Contractid != null
                        ? await _context.Projectcontracts
                            .Include(pc => pc.Project)
                            .FirstOrDefaultAsync(pc => pc.Contractid == item.Contractid)
                        : null;

                    var contractName = contract?.Contractno;
                    var deliveryAnchorId = contract?.Deliveryanchorid;
                    var deliveryAnchorEmail = await _context.Rmsemployees
                        .Where(_ => _.Employeeid == deliveryAnchorId)
                        .Select(_ => _.Companyemail)
                        .FirstOrDefaultAsync();

                    var projectName = contract?.Project?.Projectname;

                    result.Add(new TimesheetDetailnotifyDto
                    {
                        Timesheetdetailid = item.Timesheetdetailid,
                        Timesheetdate = item.Timesheetdate,
                        Dayhours = (double)item.Dayhours,
                        Activity = item.Activity,
                        Categoryofactivityname = categoryOfActivity,
                        ContractName = contractName,
                        ProjectName = projectName,
                        DeliveryAnchorEmail = deliveryAnchorEmail
                    });
               // }
            }
            return result;
        }
        public bool SendMail(string username, string sMailSubject, string sMailBody, List<string> ccemails, out string sMailOutput, string attachement = null)
        {
            try
            {
                string strSenderEmail = _config["MailCredential:Email"] ?? "";
                string strSenderPassword = _config["MailCredential:Password"] ?? "";
                int strSenderPort = int.Parse(_config["MailCredential:Port"] ?? "587");
                string strSenderHost = _config["MailCredential:Host"] ?? "";
                bool lEnableSsl = bool.Parse(_config["MailCredential:EnableSsl"] ?? "true");

                MailingService mail = new MailingService(strSenderEmail, strSenderPassword, strSenderPort, strSenderHost, lEnableSsl);

                mail.SendMail(username.Split(",").ToList<string>(), sMailSubject, sMailBody, out sMailOutput, ccemails, attachement);
                return true;
            }
            catch (Exception ex)
            {
                sMailOutput = ex.Message;
                return false;
            }
        }
        #endregion
    }
}
