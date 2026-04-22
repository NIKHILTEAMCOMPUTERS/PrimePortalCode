using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Xml.Linq;
using System.Numerics;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections;
using RMS.Data.DTOs;

namespace RMS.Service.Repositories.Master
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly List<string> TimesheeetProjectList = new List<string>() { "POC", "Pre-Sales", "OnBench" };
        public ProjectRepository(RmsDevContext context, IHostingEnvironment env) : base(context)
        {
            _context = context;
            _env = env;

        }

        public async Task<Project> SaveProjectsAsDraftAsync(ProjectSaveAsDraftRequestDto DtoObject, JwtLoginDetailDto LoginDetails)
        {


            try
            {
                var project = new Project
                {
                    Projectno = string.IsNullOrEmpty(DtoObject.Projectno) ? await GenerateNewProjectNumber() : DtoObject.Projectno,
                    Projectname = DtoObject.Projectname,
                    Projectdescription = DtoObject.Projectdescription,
                    Customerid = DtoObject.Customerid,
                    //Projectmodelid=DtoObject.Projectmodelid,
                    //Projectheadid =DtoObject.Projectheadid,
                    Projecttypeid = DtoObject.Projecttypeid,
                    Subpracticeid = DtoObject.Subpractiseid,
                    Createdby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync(),
                    Createddate = DateTime.Now,
                    Accountmanagerid = DtoObject.Accountmanagerid,
                    Billingcycledate = DtoObject.Billingcycledate ?? null,
                    CommittedClientBillingDate = DtoObject.CommittedClientBillingDate ?? null

                };
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                return await _context.Projects.AsNoTracking().Include(c => c.Customer)
                                              .FirstOrDefaultAsync(x => x.Projectid == project.Projectid);
            }
            catch (Exception e)
            {

                throw;
            }


        }
        public async Task<Project> UpdateProjectAsDeaft(int projectId, ProjectSaveAsDraftRequestDto DtoObject, JwtLoginDetailDto LoginDetails)
        {


            try
            {
                #region[Valiidatios]
                if (DtoObject.ProjectId == 0 && await _context.Projects.AnyAsync(x => x.Projectno == DtoObject.Projectno))
                {
                    throw new Exception($"Given Project Number already exists -- {DtoObject.Projectno}");
                }
                #endregion



                if (projectId != DtoObject.ProjectId)
                {
                    throw new Exception("both projectid are not same");
                }
                var existingProject = await _context.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Projectid == projectId);
                if (existingProject == null)
                {
                    throw new Exception($"Project does not exists at the given ProjectId :: {projectId}");

                }
                existingProject.Projecttypeid = DtoObject.Projecttypeid ?? existingProject.Projecttypeid;
                existingProject.Projectname = DtoObject.Projectname ?? existingProject.Projectname;
                existingProject.Projectno = DtoObject.Projectno ?? existingProject.Projectno;
                existingProject.Projectdescription = DtoObject.Projectdescription ?? existingProject.Projectdescription;
                existingProject.Customerid = DtoObject.Customerid ?? existingProject.Customerid;
                //existingProject.Projectmodelid = DtoObject.Projectmodelid;
                //existingProject.Projectheadid =DtoObject.Projectheadid;
                existingProject.Subpracticeid = DtoObject.Subpractiseid ?? existingProject.Subpracticeid;
                existingProject.Lastupdatedby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync();
                existingProject.Lastupdatedate = DateTime.Now;
                existingProject.Accountmanagerid = DtoObject.Accountmanagerid ?? existingProject.Accountmanagerid;
                existingProject.Billingcycledate = DtoObject.Billingcycledate ?? existingProject.Billingcycledate;
                existingProject.CommittedClientBillingDate = DtoObject.CommittedClientBillingDate ?? existingProject.CommittedClientBillingDate;


                _context.Projects.Update(existingProject);
                await _context.SaveChangesAsync();

                return await _context.Projects.AsNoTracking().Include(c => c.Customer)
                                              .FirstOrDefaultAsync(x => x.Projectid == projectId);
            }
            catch (Exception e)
            {

                throw;
            }


        }
        public async Task<Project> GetByName(string name)
        {
            return await _context.Projects.AsNoTracking().Where(c => c.Projectname == name).FirstOrDefaultAsync();
        }
        public async Task<List<ResponseForProjectsWithDetailsDto>> GetProjectsWithDetails()
        {
            var currentDate = DateTime.Now.Date;
            var result = await _context.Projects.AsNoTracking().Where(x => !TimesheeetProjectList.Contains(x.Projectname))
                .Include(status => status.Status)
                .Include(x => x.Projectcontracts).ThenInclude(x => x.Deliveryanchor)
                .Include(p => p.Customer)
                .Include(p => p.Projecttype)
                .Include(p => p.Subpractice)
                .ThenInclude(sp => sp.Practice)
                .Include(assignment => assignment.Projectemployeeassignments)
                .ThenInclude(employee => employee.Employee)
                .Include(accManager => accManager.Accountmanager)
                .ToListAsync();

            List<ResponseForProjectsWithDetailsDto> list = new List<ResponseForProjectsWithDetailsDto>();
            foreach (var r in result)
            {
                //var activeContractInfo = r.Projectcontracts == null ? null
                //                       : r.Projectcontracts.Any(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                //                       ? r.Projectcontracts.FirstOrDefault(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate.Value.Date >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                //                       : null;

                var activeContractInfo = r.Projectcontracts == null ? null
                                      : r.Projectcontracts.Any(c => c.Contractenddate >= DateTime.Now)
                                      ? r.Projectcontracts.FirstOrDefault(c => c.Contractenddate >= DateTime.Now)
                                      : null;

                //var lContractExpired = r.Projectcontracts == null ? null
                //                        : r.Projectcontracts.Any(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                //                        ? "NO"
                //                        : "YES";

                var lContractExpired = r.Projectcontracts == null ? null
                                       : r.Projectcontracts.Any(c => c.Contractenddate >= DateTime.Now)
                                       ? "NO"
                                       : "YES";


                list.Add(new ResponseForProjectsWithDetailsDto
                {
                    Projectid = r.Projectid,
                    Projectname = r.Projectname ?? "",
                    Projectdescription = r.Projectdescription ?? "",
                    ProjectCreatedOn = r.Createddate,
                    CommittedClientBillingDate = r.CommittedClientBillingDate,
                    AccounmanagerName = r.Accountmanager == null ? "" : r.Accountmanager.Employeename,
                    Customerid = r.Customerid,
                    CustomerName = string.IsNullOrEmpty(r.Customer?.Firstname) ? r.Customer?.Companyname : r.Customer?.Firstname,
                    //CustomerCompanyname = string.IsNullOrEmpty(r.Customer?.Firstname) ? r.Customer?.Companyname : r.Customer?.Firstname,
                    CustomerCompanyname = r.Customer.Companyname ?? "NA",
                    CustomerEmailid = r.Customer?.Companyemail ?? "",
                    CustomerPhoneno = string.IsNullOrEmpty(r.Customer?.Phone1) ? r.Customer?.Phone1 : r.Customer?.Phone1,
                    CustomerLocationAddress = string.IsNullOrEmpty(r.Customer?.Address1) ? r.Customer?.Address2 : r.Customer?.Address1,
                    CustomerProfilePhoto = r.Customer.Companylogourl,

                    Projecttypeid = r.Projecttypeid,
                    ProjecttypeName = r.Projecttype?.Projecttypename,
                    ProjectNo = r.Projectno ?? "",
                    PracticeId = r.Subpractice?.Practice?.Practiceid ?? 0,
                    PracticeName = r.Subpractice?.Practice?.Practicename ?? "",
                    Subpracticeid = r.Subpracticeid ?? 0,
                    SubpracticeName = r.Subpractice?.Subpracticename ?? "",
                    Ponumber = (string)(r.Projectcontracts
                    ?.Where(contract => contract.Contractenddate > DateTime.Now || contract.Contractenddate < DateTime.Now)
                    .OrderByDescending(contract => contract.Contractenddate)
                    .Select(contract => contract.Ponumber)
                    .FirstOrDefault()) ?? "",
                    Contractid = r.Projectcontracts == null || !r.Projectcontracts.Any(c => c.Projectid == r.Projectid)
                    ? 0
                    : r.Projectcontracts.Where(c => c.Projectid == r.Projectid && c.Statusid != 6 && c.Contractenddate < DateTime.Now)
                        .Select(c => (int?)c.Contractid).FirstOrDefault()
                    ?? r.Projectcontracts.Where(c => c.Projectid == r.Projectid)
                        .OrderByDescending(c => c.Contractenddate).Select(c => (int?)c.Contractid).FirstOrDefault() ?? 0,
                    Contractno = activeContractInfo == null ? null : activeContractInfo.Contractno,
                    DeliveryAnchorName = activeContractInfo == null ? null : activeContractInfo.Deliveryanchor != null ? activeContractInfo.Deliveryanchor.Employeename : null,
                    Contractstartdate = activeContractInfo == null ? null : activeContractInfo.Contractstartdate,
                    Contractenddate = activeContractInfo == null ? null : activeContractInfo.Contractenddate,
                    Amount = activeContractInfo == null ? null : activeContractInfo.Amount,
                    //Status = lContractExpired == null ? "Draft" : lContractExpired == "NO" ? "Progress" : "Completed"
                    //Status = lContractExpired == null ? "Draft" :
                    //         lContractExpired == "NO" ? "Progress" :
                    //         (r.Projectcontracts.Any(a => a.Statusid == 6) ? "Completed" : "Expired"),
                    //Status = r.Projectcontracts == null || r.Projectcontracts.Count == 0 ? "Draft"
                    //        : r.Projectcontracts.Any(c => c.Contractenddate >= DateTime.Now) ? "Progress"
                    //        : r.Projectcontracts.Any(a => a.Statusid == 6) ? "Completed"
                    //        : "Expired",
                    Status = r.Projectcontracts == null || r.Projectcontracts.Count == 0 ? "Draft"
                             : r.Projectcontracts.Any(c => c.Statusid != 6 && c.Contractenddate < DateTime.Now) ? "Expired"
                             : r.Projectcontracts.Any(c => c.Contractenddate >= DateTime.Now) ? "Progress"
                             : r.Projectcontracts.Any(a => a.Statusid == 6) ? "Completed"
                             : "Expired"


                });
            }

            return list;
        }
        public async Task<ResponseForProjectsWithDetailsDto> GetProjectsWithDetailsById(int ProjectId)
        {
            var currentDate = DateTime.Now.Date;
            var result = await _context.Projects.AsNoTracking()
                .Include(status => status.Status)
                .Include(x => x.Projectcontracts).ThenInclude(x => x.Deliveryanchor)
                 .Include(x => x.Projectcontracts).ThenInclude(x => x.Status)
                .Include(p => p.Customer)
                //.Include(p => p.Projectmodel)
                .Include(p => p.Projecttype)
                .Include(p => p.Subpractice)
                .ThenInclude(sp => sp.Practice)
                .Include(accManager => accManager.Accountmanager)
                .Where(x => x.Projectid == ProjectId)
                .FirstOrDefaultAsync();



            if (result == null)
            {
                return new ResponseForProjectsWithDetailsDto();
            }


            var activeContractInfo = result.Projectcontracts == null ? null : result.Projectcontracts.Any(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                                                                ? result.Projectcontracts.FirstOrDefault(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate.Value.Date >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                                                                : null;

            var lContractExpired = result.Projectcontracts == null ? null
                                        : result.Projectcontracts.Any(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                                        ? "NO"
                                        : "YES";


            ///  var status = result.Statusid.HasValue ? _context.Statuses.FirstOrDefault(x => x.Statusid == result.Statusid) : null;
            ///  nothing to change 
            return new ResponseForProjectsWithDetailsDto
            {
                Projectid = result.Projectid,
                Billingcycledate = result.Billingcycledate,
                //Projectname = result.Projectname ?? ((string.IsNullOrEmpty(result.Customer?.Firstname) ? result.Customer?.Companyname : result.Customer?.Firstname) + "-" + result.Subpractice?.Practice?.Practicename ?? ""),
                Projectname = result.Projectname ?? "",
                Projectdescription = result.Projectdescription ?? "",
                ///////////// ProjectCreatedBy = _context.Rmsemployees.FirstOrDefault(x=>x.Employeeid== result.Createdby).Employeename??"",
                ProjectCreatedOn = result.Createddate,
                CommittedClientBillingDate = result.CommittedClientBillingDate,
                AccounmanagerName = result.Accountmanager == null ? "" : result.Accountmanager.Employeename,
                AccounmanagerId = result.Accountmanager == null ? null : result.Accountmanagerid.Value,
                Customerid = result.Customerid,
                CustomerName = string.IsNullOrEmpty(result.Customer?.Firstname) ? result.Customer?.Companyname : result.Customer?.Firstname,
                CustomerCompanyname = string.IsNullOrEmpty(result.Customer?.Firstname) ? result.Customer?.Companyname : result.Customer?.Firstname,
                CustomerEmailid = result.Customer?.Companyemail ?? "",
                CustomerPhoneno = string.IsNullOrEmpty(result.Customer?.Phone1) ? result.Customer?.Phone1 : result.Customer?.Phone1,
                CustomerLocationAddress = string.IsNullOrEmpty(result.Customer?.Address1) ? result.Customer?.Address2 : result.Customer?.Address1,
                CustomerProfilePhoto = result.Customer.Companylogourl,


                //&& c.Contractstartdate <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
                //                   && c.Contractenddate >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))

                //Projectmodelid = result.Projectmodelid,
                //ProjectmodelName = result.Projectmodel?.Projectmodelname,
                //Projectheadid = result.Projectheadid ?? "",
                Projecttypeid = result.Projecttypeid,
                ProjecttypeName = result.Projecttype?.Projecttypename,
                PracticeId = result.Subpractice?.Practice?.Practiceid ?? 0,
                PracticeName = result.Subpractice?.Practice?.Practicename ?? "",
                Subpracticeid = result.Subpracticeid ?? 0,
                SubpracticeName = result.Subpractice?.Subpracticename ?? "",

                Contractid = activeContractInfo == null ? null : activeContractInfo.Contractid,
                Contractno = activeContractInfo == null ? null : activeContractInfo.Contractno,
                DeliveryAnchorName = activeContractInfo == null ? null : activeContractInfo.Deliveryanchor != null ? activeContractInfo.Deliveryanchor.Employeename : null,
                Contractstartdate = activeContractInfo == null ? null : activeContractInfo.Contractstartdate,
                Contractenddate = activeContractInfo == null ? null : activeContractInfo.Contractenddate,
                Amount = activeContractInfo == null ? null : activeContractInfo.Amount,
                ProjectNo = result.Projectno ?? "",
                // Status = lContractExpired == null ? "Draft" : lContractExpired == "NO" ? "Progress" : "Completed",
                Status = lContractExpired == null ? "Draft" :
                             lContractExpired == "NO" ? "Progress" :
                             (result.Projectcontracts.Any(a => a.Statusid == 6) ? "Completed" : "Expired"),
                TeamMembers = result.Projectemployeeassignments.Select(x => x.Employee).ToList(),
                contracts = result.Projectcontracts == null ? null : result.Projectcontracts.Select(x => new ContrtInfoDetails
                {
                    Contractid = x.Contractid,
                    Contractno = x.Contractno,
                    Ponumber = x.Ponumber,
                    Contractstartdate = x.Contractstartdate,
                    Contractenddate = x.Contractenddate,
                    Amount = x.Amount,
                    Statusid = x.Statusid,
                    Status = x.Status.Statusname
                }).ToList(),
            };
        }
        public async Task<List<ProjectListResponseDto>> GetListProjectsByCustomerId(int CustomerId)
        {
            try
            {
                var result = await _context.Projects.AsNoTracking()
                                                     .Include(x => x.Subpractice).ThenInclude(x => x.Practice)
                                                     .Include(x => x.Projectcontracts).Where(x => x.Customerid == CustomerId).ToListAsync();


                var newresult = result.Select(p => new ProjectListResponseDto
                {

                    ProjectId = p.Projectid,
                    Projectname = p.Projectname,
                    PracticeId = p.Subpractice?.Practiceid,
                    PracticeName = p.Subpractice?.Practice.Practicename,
                    SubpracticeId = p.Subpractice?.Subpracticeid,
                    SubPracticeName = p.Subpractice?.Subpracticename,
                    ContractEndDate = p.Projectcontracts?.Where(x => x.Contractenddate >= DateTime.Now)
                                                     .Select(x => x.Contractenddate).FirstOrDefault()




                }).ToList();
                return newresult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        // old code till 18-01-2024
        //public async Task<List<ResponseForProjectsWithDetailsDto>> GetProjectsWithDetailsByDA(int EmployeeId)
        //{
        //    var currentDate = DateTime.Now.Date;
        //    var result = await _context.Projects
        //        .Include(status => status.Status)
        //        .Include(x => x.Projectcontracts).ThenInclude(x => x.Deliveryanchor)
        //        .Include(p => p.Customer)
        //        .Include(p => p.Projecttype)
        //        .Include(p => p.Subpractice)
        //        .ThenInclude(sp => sp.Practice)
        //        .Where(c => c.Projectcontracts.Select(x => x.Deliveryanchorid).Contains(EmployeeId)).ToListAsync();


        //    List<ResponseForProjectsWithDetailsDto> list = new List<ResponseForProjectsWithDetailsDto>();
        //    foreach(var r in result)
        //    {
        //        var activeContractInfo = r.Projectcontracts == null ? null
        //                               : r.Projectcontracts.Any(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate >= DateTime.Now.Date || c.Contractstartdate > currentDate)
        //                               ? r.Projectcontracts.FirstOrDefault(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate.Value.Date >= DateTime.Now.Date || c.Contractstartdate > currentDate)
        //                               : null;

        //        var lContractExpired = r.Projectcontracts == null ? null
        //                                : r.Projectcontracts.Any(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate >= DateTime.Now.Date || c.Contractstartdate > currentDate)
        //                                ? "NO"
        //                                : "YES";

        //        list.Add(new ResponseForProjectsWithDetailsDto
        //        {
        //            Projectid = r.Projectid,
        //            //Projectname = r.Projectname ?? ((string.IsNullOrEmpty(r.Customer?.Firstname) ? r.Customer?.Companyname : r.Customer?.Firstname) + "-" + r.Subpractice?.Practice?.Practicename ?? ""),
        //            Projectname = r.Projectname ?? "",
        //            Projectdescription = r.Projectdescription ?? "",
        //            ///////////// ProjectCreatedBy= _context.Rmsemployees.FirstOrDefault(x => x.Employeeid == r.Createdby).Employeename ?? "",
        //            ProjectCreatedOn = r.Createddate,

        //            Customerid = r.Customerid,
        //            CustomerName = string.IsNullOrEmpty(r.Customer?.Firstname) ? r.Customer?.Companyname : r.Customer?.Firstname,
        //            //CustomerCompanyname = string.IsNullOrEmpty(r.Customer?.Firstname) ? r.Customer?.Companyname : r.Customer?.Firstname,
        //            CustomerCompanyname=r.Customer?.Companyname ?? "NA",
        //            CustomerEmailid = r.Customer?.Companyemail ?? "",
        //            CustomerPhoneno = string.IsNullOrEmpty(r.Customer?.Phone1) ? r.Customer?.Phone1 : r.Customer?.Phone1,
        //            CustomerLocationAddress = string.IsNullOrEmpty(r.Customer?.Address1) ? r.Customer?.Address2 : r.Customer?.Address1,
        //            CustomerProfilePhoto = r.Customer.Companylogourl,

        //            Projecttypeid = r.Projecttypeid,
        //            ProjecttypeName = r.Projecttype?.Projecttypename,
        //            PracticeId = r.Subpractice?.Practice?.Practiceid ?? 0,
        //            PracticeName = r.Subpractice?.Practice?.Practicename ?? "",
        //            Subpracticeid = r.Subpracticeid ?? 0,
        //            SubpracticeName = r.Subpractice?.Subpracticename ?? "",
        //            Ponumber = (string)(r.Projectcontracts
        //            ?.Where(contract => contract.Contractenddate > DateTime.Now || contract.Contractenddate < DateTime.Now)
        //            .OrderByDescending(contract => contract.Contractenddate)
        //            .Select(contract => contract.Ponumber)
        //            .FirstOrDefault()) ?? "",
        //            Contractid = activeContractInfo == null ? null : activeContractInfo.Contractid,
        //            Contractno = activeContractInfo == null ? null : activeContractInfo.Contractno,
        //            DeliveryAnchorName = activeContractInfo == null ? null : activeContractInfo.Deliveryanchor != null ? activeContractInfo.Deliveryanchor.Employeename : null,
        //            Contractstartdate = activeContractInfo == null ? null : activeContractInfo.Contractstartdate,
        //            Contractenddate = activeContractInfo == null ? null : activeContractInfo.Contractenddate,
        //            Amount = activeContractInfo == null ? null : activeContractInfo.Amount,
        //            ProjectNo = r.Projectno ?? "",
        //            Status = lContractExpired == null ? "Draft" : lContractExpired == "NO" ? "Progress" : "Completed"
        //        });
        //    }

        //    return list;
        //}
        public async Task<List<ResponseForProjectsWithDetailsDto>> GetProjectsWithDetailsByDA(int EmployeeId)
        {
            var currentDate = DateTime.Now.Date;
            var result = await _context.Projects.AsNoTracking()
                .Include(status => status.Status)
                .Include(x => x.Projectcontracts).ThenInclude(x => x.Deliveryanchor)
                .Include(p => p.Customer)
                .Include(p => p.Projecttype)
                .Include(p => p.Subpractice)
                .ThenInclude(sp => sp.Practice)
                .Where(c => c.Projectcontracts.Select(x => x.Deliveryanchorid).Contains(EmployeeId) && !TimesheeetProjectList.Contains(c.Projectname)).ToListAsync();


            List<ResponseForProjectsWithDetailsDto> list = new List<ResponseForProjectsWithDetailsDto>();
            foreach (var r in result)
            {
                var activeContractInfo = r.Projectcontracts == null ? null
                                       : r.Projectcontracts.Any(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                                       ? r.Projectcontracts.FirstOrDefault(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate.Value.Date >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                                       : null;

                var lContractExpired = r.Projectcontracts == null ? null
                                        : r.Projectcontracts.Any(c => c.Contractstartdate.Value.Date <= DateTime.Now.Date && c.Contractenddate >= DateTime.Now.Date || c.Contractstartdate > currentDate)
                                        ? "NO"
                                        : "YES";

                list.Add(new ResponseForProjectsWithDetailsDto
                {
                    Projectid = r.Projectid,
                    Billingcycledate = r.Billingcycledate,
                    //Projectname = r.Projectname ?? ((string.IsNullOrEmpty(r.Customer?.Firstname) ? r.Customer?.Companyname : r.Customer?.Firstname) + "-" + r.Subpractice?.Practice?.Practicename ?? ""),
                    Projectname = r.Projectname ?? "",
                    Projectdescription = r.Projectdescription ?? "",
                    ///////////// ProjectCreatedBy= _context.Rmsemployees.FirstOrDefault(x => x.Employeeid == r.Createdby).Employeename ?? "",
                    ProjectCreatedOn = r.Createddate,
                    CommittedClientBillingDate = r.CommittedClientBillingDate,

                    Customerid = r.Customerid,
                    CustomerName = string.IsNullOrEmpty(r.Customer?.Firstname) ? r.Customer?.Companyname : r.Customer?.Firstname,
                    //CustomerCompanyname = string.IsNullOrEmpty(r.Customer?.Firstname) ? r.Customer?.Companyname : r.Customer?.Firstname,
                    CustomerCompanyname = r.Customer?.Companyname ?? "NA",
                    CustomerEmailid = r.Customer?.Companyemail ?? "",
                    CustomerPhoneno = string.IsNullOrEmpty(r.Customer?.Phone1) ? r.Customer?.Phone1 : r.Customer?.Phone1,
                    CustomerLocationAddress = string.IsNullOrEmpty(r.Customer?.Address1) ? r.Customer?.Address2 : r.Customer?.Address1,
                    CustomerProfilePhoto = r.Customer.Companylogourl,

                    Projecttypeid = r.Projecttypeid,
                    ProjecttypeName = r.Projecttype?.Projecttypename,
                    PracticeId = r.Subpractice?.Practice?.Practiceid ?? 0,
                    PracticeName = r.Subpractice?.Practice?.Practicename ?? "",
                    Subpracticeid = r.Subpracticeid ?? 0,
                    SubpracticeName = r.Subpractice?.Subpracticename ?? "",
                    Ponumber = (string)(r.Projectcontracts
                    ?.Where(contract => contract.Contractenddate > DateTime.Now || contract.Contractenddate < DateTime.Now)
                    .OrderByDescending(contract => contract.Contractenddate)
                    .Select(contract => contract.Ponumber)
                    .FirstOrDefault()) ?? "",

                    Contractid = activeContractInfo == null ? null : activeContractInfo.Contractid,
                    Contractno = activeContractInfo == null ? null : activeContractInfo.Contractno,
                    DeliveryAnchorName = activeContractInfo == null ? null : activeContractInfo.Deliveryanchor != null ? activeContractInfo.Deliveryanchor.Employeename : null,
                    Contractstartdate = activeContractInfo == null ? null : activeContractInfo.Contractstartdate,
                    Contractenddate = activeContractInfo == null ? null : activeContractInfo.Contractenddate,
                    Amount = activeContractInfo == null ? null : activeContractInfo.Amount,
                    ProjectNo = r.Projectno ?? "",
                    //Status = lContractExpired == null ? "Draft" : lContractExpired == "NO" ? "Progress" : "Completed"
                    Status = lContractExpired == null ? "Draft" :
                             lContractExpired == "NO" ? "Progress" :
                             (r.Projectcontracts.Any(a => a.Statusid == 6) ? "Completed" : "Expired"),
                });
            }

            return list;
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

        public async Task<bool> CheckProjectNo(int? Projectid, string Projectno)
        {
            try
            {

                if (Projectid > 0)
                {
                    var result = await _context.Projects.AsNoTracking().AnyAsync(x => x.Projectno == Projectno
                                          && x.Projectid == Projectid);
                    if (result == true)
                    {
                        return false;
                    }
                    else
                    {
                        return await _context.Projects.AsNoTracking().AnyAsync(x => x.Projectno == Projectno);
                    }

                }
                else
                {
                    return await _context.Projects.AsNoTracking().AnyAsync(x => x.Projectno == Projectno);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable> GetProjetsByEmployeeId(int id)
        {
            var finalResult = new List<TimeSheetEmployeeDetailDto>();

            try
            {
                var result = await _context.TimeSheetEmployeeDetailDtos
                                           .FromSqlRaw("select * from get_employee_details({0})", id)
                                           .ToListAsync();

                if (result == null || !result.Any())
                {
                    return new List<TimeSheetEmployeeDetailDto>();
                }

                var groupedResults = result.GroupBy(x => x.ProjectId);

                foreach (var group in groupedResults)
                {
                    var elements = group.ToList();

                    if (elements.Count == 1)
                    {
                        finalResult.Add(elements.First());
                        continue;
                    }

                    var deployedElement = elements.FirstOrDefault(e => e.Flag.Equals("deployed", StringComparison.OrdinalIgnoreCase));
                    var overRunElement = elements.FirstOrDefault(e => e.Flag.Equals("over run", StringComparison.OrdinalIgnoreCase));

                    if (deployedElement != null)
                    {
                        finalResult.Add(deployedElement);
                    }
                    else if (overRunElement != null)
                    {
                        finalResult.Add(overRunElement);
                    }
                    else
                    {
                        finalResult.Add(elements.First());
                    }
                }

                return finalResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IEnumerable> GetProjetsEmployeesByHeadId(string id)
        {
            var finalResult = new List<TimeSheetEmployeeDetailDto>();

            try
            {
                var result = await _context.TimeSheetEmployeeDetailDtos
                                           .FromSqlRaw("select * from get_employee_details_By_ManagerId({0})", id)
                                           .ToListAsync();

                if (result == null || !result.Any())
                {
                    return new List<TimeSheetEmployeeDetailDto>();
                }

                var groupedResults = result.GroupBy(x => new { x.ProjectId, x.EmployeeName });

                foreach (var group in groupedResults)
                {
                    var elements = group.ToList();

                    if (elements.Count == 1)
                    {
                        finalResult.Add(elements.First());
                        continue;
                    }

                    var deployedElement = elements.FirstOrDefault(e => e.Flag.Equals("deployed", StringComparison.OrdinalIgnoreCase));
                    var overRunElement = elements.FirstOrDefault(e => e.Flag.Equals("over run", StringComparison.OrdinalIgnoreCase));

                    if (deployedElement != null)
                    {
                        finalResult.Add(deployedElement);
                    }
                    else if (overRunElement != null)
                    {
                        finalResult.Add(overRunElement);
                    }
                    else
                    {
                        finalResult.Add(elements.First());
                    }
                }

                return finalResult;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IEnumerable<object>> CheckEmployeeActiveProjects(int employeeid, DateTime startdate, DateTime enddate)
        {
            try
            {
                if (employeeid <= 0)
                {
                    throw new ArgumentException("Invalid employee ID.");
                }

                var activeProjects = await _context.Projects
               .Include(p => p.Projectcontracts)
                   .ThenInclude(pc => pc.Contractemployees)
               .Where(p => p.Projectcontracts.Any(c =>
                   (c.Contractstartdate >= startdate && c.Contractstartdate <= enddate) ||
                   (c.Contractenddate >= startdate && c.Contractenddate <= enddate) ||
                   (c.Contractstartdate <= startdate && c.Contractenddate >= enddate)
               ) && p.Projectcontracts.Any(c => c.Contractemployees.Any(e => e.Employeeid == employeeid && e.Categorysubstatusid != 11)) && (p.Statusid != 6))


               .Select(p => new
               {
                   ProjectName = p.Projectname,
                   ContractStartDate = p.Projectcontracts.FirstOrDefault().Contractstartdate,
                   ContractEndDate = p.Projectcontracts.FirstOrDefault().Contractenddate,
                   Employeeid = employeeid,
                   DeliveryAnchorId = p.Projectcontracts.FirstOrDefault().Deliveryanchorid,
                   DeliveryAnchorName = _context.Rmsemployees
                                         .Where(a => a.Employeeid == p.Projectcontracts.FirstOrDefault().Deliveryanchorid)
                                         .Select(a => a.Employeename)
                                         .FirstOrDefault(),
                   EmployeeStatus = p.Projectcontracts.FirstOrDefault().Contractemployees
                                   .Where(e => e.Employeeid == employeeid)
                                   .Select(e => e.Categorysubstatus.Categorysubstatusname)
                                   .FirstOrDefault()

               })
               .ToListAsync();

                var filteredProjects = activeProjects.Where(_ => _.ContractEndDate > DateTime.Now).ToList();
                return filteredProjects;
            }
            catch (ArgumentException argEx)
            {
                throw new ArgumentException(argEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching active projects.", ex);
            }
        }

        public async Task<GenericResponse<string>> UpsertEmployeeRequest(EmployeeReleaseRequest value, JwtLoginDetailDto logindetails)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Request is empty");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var lastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
                                       .Where(x => x.Userid == logindetails.TmcId)
                                       .Select(x => x.Employeeid)
                                       .FirstOrDefaultAsync();

                //var projections = await _context.Projections.Where(_ => _.CrmDealsid == value.crmId).FirstOrDefaultAsync();

                //int projectionId = projections.Projectionid;
                if (value.ProjectionRequestId == 0)
                {
                    // Create new ProjectionRequest
                    var newProjectRequest = new Projectionrequest
                    {
                        Employeeid = value.EmployeeId,
                        Requestsentby = value.EmployeeId,
                        Requestsentto = value.SentTo,
                        Createddate = DateTime.Now,
                        Lastupdatedate = DateTime.Now,
                        Lastupdatedby = lastUpdatedBy,
                        Status = value.Status,
                        Projectionid = value.ProjectionId
                    };

                    await _context.Projectionrequests.AddAsync(newProjectRequest);
                    await _context.SaveChangesAsync();
                    value.ProjectionRequestId = newProjectRequest.Projectionrequestid;
                }
                else
                {
                    // Update existing ProjectionRequest
                    var existingProjectRequest = await _context.Projectionrequests
                                                                .FirstOrDefaultAsync(_ => _.Projectionrequestid == value.ProjectionRequestId);

                    if (existingProjectRequest == null)
                    {
                        throw new KeyNotFoundException($"Projection Request not found with ID: {value.ProjectionRequestId}");
                    }

                    existingProjectRequest.Status = value.Status;
                    existingProjectRequest.Lastupdatedate = DateTime.Now;
                    existingProjectRequest.Remarks = value.Remarks == null ? null : value.Remarks;
                    existingProjectRequest.Lastupdatedby = lastUpdatedBy;

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return new GenericResponse<string>
                {
                    responseCode = 200,
                    responseMessage = "Request has been sent successfully!",
                    DataObject = null,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error in upserting Projection Request", ex);
            }
        }

    }
}
