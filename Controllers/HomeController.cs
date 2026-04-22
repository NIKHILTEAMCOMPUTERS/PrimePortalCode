using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.ObjectModelRemoting;
using Newtonsoft.Json;
using RMS.Client.Models;
using RMS.Client.Models.Dashboard;
using RMS.Client.Models.Employee;
using RMS.Client.Models.Master;
using RMS.Client.Utility;
using System.Diagnostics;

namespace RMS.Client.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(IConfiguration config, IWebHostEnvironment env)
              : base(config, env)
        {
        }

        [Route("/Authorize")]
        public async Task<IActionResult> Authorize()
        {
            var pages = await GetAuthorizationAsync(Session.EmpCode);
            if (pages != null && pages.Count > 0)
            {
                HttpContext.Session.SetString("RoleName", Session.RoleName);
               // var provisionrequestcount = RequestCount;
                var firstPage = pages.First();
                return RedirectToAction(firstPage.Action, firstPage.Controller);
            }

            return PartialView("~/Views/Shared/Common/_NotAuthorized.cshtml");
        }

        public async Task<IActionResult> Index()
        {

            if(RoleName == "Employee") {
                return RedirectToAction("EmployeeDashboard", "Employee");
            }

            var projects = RowLevelSecurity ? await GetProjectByDa(Session.EmployeeId) : await GetProjects();
            var employees = RowLevelSecurity ? await GetEmployeeByDaByProcedure(Session.EmployeeId) : await GetEmployeeByNewProcedure();
            // var employees = RowLevelSecurity ? await GetEmployeeByDa(Session.EmployeeId) : await GetEmployeesForCount();


            //var overrunCounts = employees.Where(e => e.CategoryStatusName.ToLower().Trim() == "over run").Count();
            //var OnbenchCounts = employees.Where(e => e.CategoryStatusName.ToLower().Trim() == "onbench").Count();
            //var DeployedCount = employees.Where(e => e.CategoryStatusName.ToLower().Trim() == "deployed").Count();
            //var Freshercount = employees.Where(e => e.CategoryStatusName.ToLower().Trim() == "onbench" && e.Experience == "Fresher").Count();
            //var Expcount = employees.Where(e => e.CategoryStatusName.ToLower().Trim() == "onbench" && e.Experience == "Experienced").Count();

            var overrunCounts = employees.Where(e => e.Flag.ToLower().Trim() == "over run").Count();
            var OnbenchCounts = employees.Where(e => e.Flag.ToLower().Trim() == "on bench").Count();
            var DeployedCount = employees.Where(e => e.Flag.ToLower().Trim() == "deployed").Count();
            var Freshercount = employees.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "fresher").Count();
            var Expcount = employees.Where(e => e.Flag.ToLower().Trim() == "on bench" && e.Exe?.ToLower().Trim() == "experienced").Count();

            var objResponse = new HRDashboard
            {
                Projects = projects.Where(a => Convert.ToDateTime(a.ContractEndDate) >= DateTime.Now).ToList(),
                EmployeeCount = employees.Count(),
                EmployeeOverrun = overrunCounts,
                EmployeeOnbench = OnbenchCounts,
                EmployeeDeployed = DeployedCount,
                BenchFreshersCount = Freshercount,
                BenchExpCount = Expcount,
            };
            ViewBag.Completedcount = projects.Where(a => a.Status == "Completed").Count();
            ViewBag.Overruncount = projects.Where(c => (c.ContractId > 0) && Convert.ToDateTime(c.ContractEndDate) < DateTime.Now && c.Status.ToLower().Trim() != "completed").Count();
            ViewBag.TotalProjects = projects.Count();
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View(objResponse);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NotAuthorized()
        {
            return PartialView("~/Views/Shared/Common/_NotAuthorized.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<JsonResult> GetProjectInfo(int mode = 1)
        {
            var projectList = RowLevelSecurity ? await GetProjectByDa(Session.EmployeeId) : await GetProjects();
            object objResponse = null;
            switch (mode)
            {
                case 1:
                    objResponse = new
                    {
                        projects = projectList.Where(c => Convert.ToDateTime(c.ContractEndDate) >= DateTime.Now),
                        completed = projectList.Where(c => c.Status != null && c.Status.ToLower().Trim() == "completed").Count(),
                        pending = projectList.Where(c => c.Status != null && c.Status.ToLower().Trim() == "pending").Count(),
                        progress = projectList.Where(c => c.Status != null && c.Status.ToLower().Trim() == "progress").Count(),
                        hold = projectList.Where(c => c.Status != null && c.Status.ToLower().Trim() == "hold").Count(),
                        draft = projectList.Where(c => c.Status != null && c.Status.ToLower().Trim() == "draft").Count(),
                        overrun = projectList.Where(c => (c.ContractId > 0) && Convert.ToDateTime(c.ContractEndDate) < DateTime.Now && c.Status.ToLower().Trim() != "completed").Count(),
                    };
                    break;

                case 2:

                    var now = DateTime.UtcNow;
                    objResponse = new
                    {
                        projects = projectList.Where(c => (c.ContractId > 0) && Convert.ToDateTime(c.ContractEndDate) < now && c.Status != null 
                                   && c.Status.ToLower().Trim() != "completed"),
                        completed = projectList.Count(c => c.Status?.ToLower().Trim() == "completed"),
                        pending = projectList.Count(c => c.Status?.ToLower().Trim() == "pending"),
                        progress = projectList.Count(c => c.Status?.ToLower().Trim() == "progress"),
                        hold = projectList.Count(c => c.Status?.ToLower().Trim() == "hold"),
                        draft = projectList.Count(c => c.Status?.ToLower().Trim() == "draft"),
                        overrun = projectList.Count(c => (c.ContractId > 0) && Convert.ToDateTime(c.ContractEndDate) < now && c.Status?.ToLower().Trim() != "completed")
                    };

                    break;

                default:
                    
                    break;
            }

            return Json(objResponse);
        }

           

        public async Task<JsonResult> GetEmployeeInfo(int mode, bool all = false)
        {
            var filteredEmployees = new List<Employees>();
            var employees = await (RowLevelSecurity && !all ? GetEmployeeByDa(Session.EmployeeId) : GetEmployee());
            var overrunCount = employees.Count(e => e.CategoryStatusName.ToLower().Trim() == "over run");
            var onbenchCount = employees.Count(e => e.CategoryStatusName.ToLower().Trim() == "onbench");
            var deployedCount = employees.Count(e => e.CategoryStatusName.ToLower().Trim() == "deployed"); 
            var Freshercount = employees.Where(e => e.CategoryStatusName.ToLower().Trim() == "onbench" && e.Experience == "Fresher").Count();
            var Expcount = employees.Where(e => e.CategoryStatusName.ToLower().Trim() == "onbench" && e.Experience == "Experienced").Count();
            var empcount = employees.Count();

            switch (mode)
            {
                case 1:
                    //return Json(new
                    //{
                    //    Employees = employees,
                    //    Overrun = overrunCount,
                    //    OnBench = onbenchCount,
                    //    Deployed = deployedCount,
                    //    Total = empcount,
                    //    benchFreshersCount = Freshercount,
                    //    benchExpCount = Expcount,

                    //});
                 
                    foreach (var employee in employees)
                    {
                        var projectGroups = new Dictionary<int, List<employeeProject>>();

                        // Group projects by Projectid
                        foreach (var project in employee.employeeProjects)
                        {
                            if (!projectGroups.ContainsKey(project.Projectid))
                            {
                                projectGroups.Add(project.Projectid, new List<employeeProject>());
                            }
                            projectGroups[project.Projectid].Add(project);
                        }

                        // Filter projects within each group
                        foreach (var group in projectGroups.Values)
                        {
                            employeeProject chosenProject = null;
                            foreach (var project in group)
                            {
                                if (project.Categorysubstatusname == "Deployed")
                                {
                                    chosenProject = project;
                                    break;
                                }
                                else if (project.Categorysubstatusname == "Overrun")
                                {
                                    chosenProject = project;
                                }
                                else if (project.Categorysubstatusname == "Allocation")
                                {
                                    chosenProject = project;
                                }
                                else if (project.Categorysubstatusname == "Shadow")
                                {
                                    chosenProject = project;
                                }
                                else if (project.Categorysubstatusname == "Projection")
                                {
                                    chosenProject = project;
                                }
                                else if (project.Categorysubstatusname == "Bench" && chosenProject == null)
                                {
                                    chosenProject = project;
                                }
                            }

                            // Keep only the chosen project
                            group.Clear();
                            if (chosenProject != null)
                            {
                                group.Add(chosenProject);
                            }
                        }

                        // Flatten the groups back into EmployeeProjects
                        var flattenedProjects = new List<employeeProject>();
                        foreach (var group in projectGroups.Values)
                        {
                            flattenedProjects.AddRange(group);
                        }

                        var filteredEmployee = new Employees
                        {
                            Employeeid = employee.Employeeid,
                            Experience = employee.Experience,
                            TmcId = employee.TmcId,
                            Employeename = employee.Employeename,
                            email = employee.email,
                            Contactno = employee.Contactno,
                            Reporthead = employee.Reporthead,
                            Department = employee.Department,
                            Branchid = employee.Branchid,
                            Branch = employee.Branch,
                            Dateofbirth = employee.Dateofbirth,
                            Dateofjoining = employee.Dateofjoining,
                            PracticeName = employee.PracticeName,
                            SubPracticeName = employee.SubPracticeName,
                            CategorySubStatusName = employee.CategorySubStatusName,
                            CategoryStatusName = employee.CategoryStatusName,
                            companyemail = employee.companyemail,
                            userid = employee.userid,
                            VendorName = employee.VendorName,
                            VendorId = employee.VendorId,
                            Udf3 = employee.Udf3,
                            isAde = employee.isAde,
                            Ade = employee.Ade,
                            costcenter= employee.costcenter,    
                            employeeProjects = flattenedProjects,
                        };
                        filteredEmployees.Add(filteredEmployee);

                    }

                    return Json(new
                    {
                        Employees = filteredEmployees,
                        Overrun = overrunCount,
                        OnBench = onbenchCount,
                        Deployed = deployedCount,
                        Total = empcount,
                        benchFreshersCount = Freshercount,
                        benchExpCount = Expcount,
                    });
                case 2:
                    var data = employees.Where(e => e.CategoryStatusName.ToLower() == "over run").ToList();

                    foreach (var item in data)
                    {
                        item.employeeProjects = item.employeeProjects
                            .Where(a => a.Categorysubstatusname != null && a.Categorysubstatusname.ToLower() == "over run")
                            .ToList();
                    }


                    return Json(new
                    {
                        Employees = data,
                        Overrun = overrunCount,
                        OnBench = onbenchCount,
                        Deployed = deployedCount,
                        Total = empcount,
                        benchFreshersCount = Freshercount,
                        benchExpCount = Expcount,
                    });

                case 3:
                    var datas = employees.Where(e => e.CategoryStatusName.ToLower() == "onbench").ToList();

                    foreach (var item in datas)
                    {
                        item.employeeProjects = item.employeeProjects.Where(a => a.Categorysubstatusname != null && a.Categorysubstatusname.ToLower() == "onbench").ToList();
                    }
                    return Json(new
                    {
                        Employees = datas,
                        Overrun = overrunCount,
                        OnBench = onbenchCount,
                        Deployed = deployedCount,
                        Total = empcount,
                        benchFreshersCount = Freshercount,
                        benchExpCount = Expcount,
                    });

                case 4:
                    var datass = employees.Where(e => e.CategoryStatusName != null && e.CategoryStatusName.ToLower().Trim() == "deployed").ToList();

                    foreach (var item in datass)
                    {
                        if (item.employeeProjects != null)
                        {
                            item.employeeProjects = item.employeeProjects
                                .Where(a => a.Categorysubstatusname != null && a.Categorysubstatusname.ToLower().Trim() == "deployed")
                                .ToList();
                        }
                    }
                    return Json(new
                    {
                        Employees = datass,
                        Overrun = overrunCount,
                        OnBench = onbenchCount,
                        Deployed = deployedCount,
                        Total = empcount,
                        benchFreshersCount = Freshercount,
                        benchExpCount = Expcount,
                    });

                default:
                    //    return Json(new
                    //    {
                    //        Employees = employees,
                    //        Overrun = overrunCount,
                    //        OnBench = onbenchCount,
                    //        Deployed = deployedCount,
                    //        Total = empcount,
                    //        benchFreshersCount = Freshercount,
                    //        benchExpCount = Expcount,
                    //    });
                   

                    foreach (var employee in employees)
                    {
                        var projectGroups = new Dictionary<int, List<employeeProject>>();

                        // Group projects by Projectid
                        foreach (var project in employee.employeeProjects)
                        {
                            if (!projectGroups.ContainsKey(project.Projectid))
                            {
                                projectGroups.Add(project.Projectid, new List<employeeProject>());
                            }
                            projectGroups[project.Projectid].Add(project);
                        }

                        // Filter projects within each group
                        foreach (var group in projectGroups.Values)
                        {
                            employeeProject chosenProject = null;
                            foreach (var project in group)
                            {
                                if (project.Categorysubstatusname == "Deployed")
                                {
                                    chosenProject = project;
                                    break;
                                }
                                else if (project.Categorysubstatusname == "Overrun")
                                {
                                    chosenProject = project;
                                }
                                else if (project.Categorysubstatusname == "Allocation")
                                {
                                    chosenProject = project;
                                }
                                else if (project.Categorysubstatusname == "Shadow")
                                {
                                    chosenProject = project;
                                }
                                else if (project.Categorysubstatusname == "Projection")
                                {
                                    chosenProject = project;
                                }
                                else if (project.Categorysubstatusname == "Bench" && chosenProject == null)
                                {
                                    chosenProject = project;
                                }
                            }

                            // Keep only the chosen project
                            group.Clear();
                            if (chosenProject != null)
                            {
                                group.Add(chosenProject);
                            }
                        }

                        // Flatten the groups back into EmployeeProjects
                        var flattenedProjects = new List<employeeProject>();
                        foreach (var group in projectGroups.Values)
                        {
                            flattenedProjects.AddRange(group);
                        }

                        var filteredEmployee = new Employees
                        {
                            Employeeid = employee.Employeeid,
                            Experience = employee.Experience,
                            TmcId = employee.TmcId,
                            Employeename = employee.Employeename,
                            email = employee.email,
                            Contactno = employee.Contactno,
                            Reporthead = employee.Reporthead,
                            Department = employee.Department,
                            Branchid = employee.Branchid,
                            Branch = employee.Branch,
                            Dateofbirth = employee.Dateofbirth,
                            Dateofjoining = employee.Dateofjoining,
                            PracticeName = employee.PracticeName,
                            SubPracticeName = employee.SubPracticeName,
                            CategorySubStatusName = employee.CategorySubStatusName,
                            CategoryStatusName = employee.CategoryStatusName,
                            companyemail = employee.companyemail,
                            userid = employee.userid,
                            VendorName = employee.VendorName,
                            VendorId = employee.VendorId,
                            Udf3 = employee.Udf3,
                            isAde = employee.isAde,
                            Ade = employee.Ade,
                            costcenter = employee.costcenter,
                            employeeProjects = flattenedProjects,
                        };
                        filteredEmployees.Add(filteredEmployee);

                    }

                    return Json(new
                    {
                        Employees = filteredEmployees,
                        Overrun = overrunCount,
                        OnBench = onbenchCount,
                        Deployed = deployedCount,
                        Total = empcount,
                        benchFreshersCount = Freshercount,
                        benchExpCount = Expcount,
                    });
            }
        }


        public IActionResult Sheet()
        {
            ViewData["controller"] = this.ControllerContext.RouteData.Values["controller"].ToString();
            return View();
        }
        

    }
}