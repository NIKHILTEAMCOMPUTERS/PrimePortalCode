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
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace RMS.Service.Repositories.Master
{
    public class ProjectionRepository : GenericRepository<Projection>, IProjectionRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly List<string> TimesheeetProjectList = new List<string>() { "POC", "Pre-Sales", "OnBench" };
        public ProjectionRepository(RmsDevContext context, IHostingEnvironment env) : base(context)
        {
            _context = context;
            _env = env;

        }

        public async Task<ApiResponse<object>> GetEmployeeBy_SkillId_Exep(Employee_Skill_Exe_Dto Value)
        {
            try
            {
                //var result =await _context.AggrEmployeeSkillExpeCosts
                //                          .Where(x=>x.Skillid==Value.SkillId && x.Expname.ToLower().Trim()==Value.Experince.ToLower().Trim())
                //                          //.Select(x=> new { EmoloyeeId=x.Employeeid,EmployeeName=x.Employeename}).ToListAsync();
                //                          .ToListAsync();
                //var endResult=result.Select(x=> new { EmployeeId=x.Employeeid,EmployeeName=x.Employeename}).ToList();
                //if (result == null)
                //{
                //    return ApiResponse<object>.Error("Employee Not foun at given request",StatusCodes.Status404NotFound);
                //}
                //return ApiResponse<object>.Success(endResult);
            //try
            //{
            //var result =await _context.AggrEmployeeSkillExpeCosts
            //                          .Where(x=>x.Skillid==Value.SkillId && x.Expname.ToLower().Trim()==Value.Experince.ToLower().Trim())
            //                          //.Select(x=> new { EmoloyeeId=x.Employeeid,EmployeeName=x.Employeename}).ToListAsync();
            //                          .ToListAsync();







            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Error("Error in fetching records", StatusCodes.Status500InternalServerError, ex.Message);
            }
            // var endResult=result.Select(x=> new { EmployeeId=x.Employeeid,EmployeeName=x.Employeename}).ToList();
            //    if (result == null)
            //    {
            //        return ApiResponse<object>.Error("Employee Not foun at given request",StatusCodes.Status404NotFound);
            //    }
            //    return ApiResponse<object>.Success(endResult);

            //}
            //catch (Exception ex)
            //{
            //    return ApiResponse<object>.Error("Error in fetching records", StatusCodes.Status500InternalServerError, ex.Message);
            //}
            return null;
        }

        public async  Task<ApiResponse<object>> GetEmployeeProjectDetails_For_Projrction(int EmployeeId)
        {
            try
            {
                var result =await _context.AggregatedDataHrs.Where(x => x.Employeeid == EmployeeId).ToListAsync();
                if (result.Any(x => x.Contracstatus.ToLower().Trim() == "deployed"))
                {
                    result = result.Where(x => x.Contracstatus.ToLower().Trim() == "deployed" || x.Contracstatus.ToLower().Trim() == "over run").ToList();
                    return ApiResponse<object>.Success(result); 
                }
                else if (result.Any(x => x.Contracstatus.ToLower().Trim() == "over run"))
                {
                    result = result.Where(x => x.Contracstatus.ToLower().Trim() == "deployed" || x.Contracstatus.ToLower().Trim() == "over run").ToList();
                    return ApiResponse<object>.Success(result);

                }
                else 
                {
                    result = result.Where(x => x.Contracstatus.ToLower().Trim() == "bench").ToList();
                }
               return null;

            }
            catch (Exception ex)
            {
                return ApiResponse<object>.Error("An error occurd while accessin g data of employee",StatusCodes.Status500InternalServerError,ex.Message);
            }
        }

        public async Task<ApiResponse<ProjectionRequestResponseDto>> GetProjectionByIdAsync(int id)
        {
            try
            {
                // Fetch the projection data
                var result = await _context.Projections
                                           .Include(x => x.Customer)
                                           .Include(x => x.Projecthead)
                                           .Include(x => x.ProjectionEmployeeDeployements)
                                               .ThenInclude(x => x.DeployedEmployee)
                                           .Include(x => x.ProjectionEmployeeDeployements)
                                               .ThenInclude(x => x.Categorysubstatus)
                                           .Include(x => x.Status)
                                           .Include(x => x.Subpractice)
                                           .Include(x => x.Projectioninitialbillings)
                                           .FirstOrDefaultAsync(x => x.Projectionid == id);

                if (result == null)
                {
                    return ApiResponse<ProjectionRequestResponseDto>.Error("Projection not found", 404);
                }

                // Mapping ProjectionEmployeeDeployements to DeployedEmployees list
                var deployedEmployeesList = result.ProjectionEmployeeDeployements
                                                   .Select(emp => new DeployedEmployee
                                                   {
                                                       DeployedEmployeeId = emp.DeployedEmployeeId,
                                                       DeployedEmployeeName = emp.DeployedEmployee?.Employeename,
                                                       StartDate = emp.Startdate,
                                                       EndDate = emp.Enddate,
                                                   })
                                                   .ToList();

                var projectionInitialBilling = result.Projectioninitialbillings.Select(_ => new ProjectionInitialBilling
                {
                    Amount = _.Amount,
                    MonthYear = _.Monthyear,

                }).OrderBy(_ => Convert.ToDateTime(_.MonthYear)).ToList();

                // Mapping ProjectionRequestDto based on a different relation or data source
                // Assuming `ProjectionRequests` is a separate entity or dataset related to `Projections`
                var projectionRequestList = await _context.Projectionrequests // Assuming this exists in your database context
                                                          .Where(x => x.Projectionid == id) // Adjust this as needed
                                                          .Select(req => new ProjectionRequestDto
                                                          {
                                                              DeliveryAnchorId = req.Requestsentto,
                                                              DeliveryAnchorName = _context.Rmsemployees.Where(a => a.Employeeid == req.Requestsentto).Select(b => b.Employeename).FirstOrDefault().ToString(),  // Assuming relation
                                                              Status = req.Status,
                                                              Remarks = req.Remarks,
                                                              EmployeeId = req.Employeeid
                                                          })
                                                          .Distinct()
                                                          .ToListAsync();
                var projectionType = await _context.Projecttypes.Where(_ => _.Projecttypeid == result.Projecttypeid).Select(a => a.Projecttypename).FirstOrDefaultAsync();

                // Populate the ProjectionRequestResponseDto
                var endResult = new ProjectionRequestResponseDto
                {
                    CrmId = result.CrmDealsid,  // Mapping CrmId
                    Projectionid = result.Projectionid,
                    ProjectionNo = result.Projectionno,
                    ProjectionName = result.Projectionname,
                    ProjectionDescription = result.Projectiondescription,
                    CustomerId = result.Customerid,
                    CustomerName = result.Customer?.Companyname,
                    SubPracticeId = result.Subpracticeid ?? 0,
                    SubPracticeName = result.Subpractice?.Subpracticename,
                    ProjectHeadId = result.Projectheadid,
                    ProjectHeadName = result.Projecthead?.Employeename,
                    StartDate = result.Startdate,
                    EndDate = result.Enddate,
                    ProjectionCost = result.Projectioncost,
                    StatusId = result.Statusid,
                    StatusName = result.Status?.Statusname,
                    DeployedEmployees = deployedEmployeesList,
                    ProjectionRequest = projectionRequestList,
                    ProjectType = projectionType,
                    ProjectionInitialBilling = projectionInitialBilling,
                    CostsheetId = result.Costsheetid,

                };

                return ApiResponse<ProjectionRequestResponseDto>.Success(endResult, "Projection data fetched successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProjectionRequestResponseDto>.Error("An error occurred while fetching the projection", 500, ex.Message);
            }
        }


        
public async Task<ApiResponse<Projection>> UpsertProjectionAsync(ProjectionRequestResponseDto projection, JwtLoginDetailDto logindetails)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(); // Awaiting transaction start

            try
            {
                var createdUpdatedBy = await _context.Rmsemployees.FirstOrDefaultAsync(x => x.Userid == logindetails.TmcId);
                if (createdUpdatedBy == null)
                {
                    return ApiResponse<Projection>.Error("You are not authorized to create this record", StatusCodes.Status405MethodNotAllowed);
                }

                var accountManagerId = createdUpdatedBy.Employeeid;
                if (projection == null)
                {
                    return ApiResponse<Projection>.Error("Projection data cannot be null", StatusCodes.Status406NotAcceptable);
                }

                var projectionEntity = new Projection();

                // Inserting a new record
                if ((projection.Projectionid == null && (projection.DeployedEmployees == null || projection.DeployedEmployees.Count == 0)) || projection.Projectionid <= 0)
                {
                    if (projection.CustomerId <= 0)
                    {
                        return ApiResponse<Projection>.Error("Invalid CustomerId", StatusCodes.Status406NotAcceptable);
                    }

                    string projectionNumber = await GetMaxProjectionNumberAsync();
                    projectionEntity = new Projection
                    {
                        CrmDealsid = projection.CrmId,
                        Projectionno = projectionNumber,
                        Projectionname = string.IsNullOrWhiteSpace(projection.ProjectionName) ? projectionNumber : projection.ProjectionName,
                        Projectiondescription = projection.ProjectionDescription,
                        Customerid = projection.CustomerId,
                        Subpracticeid = projection.SubPracticeId,
                        Projectheadid = accountManagerId,
                        Startdate = projection.StartDate.Value,
                        Enddate = projection.EndDate.Value,
                        Projectioncost = projection.ProjectionCost,
                        Statusid = 2,
                        Createdby = createdUpdatedBy.Employeeid,
                        Createddate = DateTime.Now,
                        Projecttypeid = projection.ProjectTypeId,
                        Costsheetid = projection.CostsheetId,
                    };

                    _context.Projections.Add(projectionEntity);
                    await _context.SaveChangesAsync();

                    // Fetching the newly created ProjectionId
                    var newlyCreatedProjectionId = projectionEntity.Projectionid;

                    // Saving ProjectionInitialBilling entries
                    if (projection.ProjectionInitialBilling != null && projection.ProjectionInitialBilling.Count > 0)
                    {
                        var projectionInitialBillingEntities = projection.ProjectionInitialBilling.Select(billing => new Projectioninitialbilling
                        {
                            Projectionid = newlyCreatedProjectionId,
                            Monthyear = billing.MonthYear,
                            Amount = billing.Amount,

                        }).ToList();

                        await _context.Projectioninitialbillings.AddRangeAsync(projectionInitialBillingEntities);
                        await _context.SaveChangesAsync();
                    }


                    await transaction.CommitAsync();
                    return ApiResponse<Projection>.Success(projectionEntity, "Projection upserted successfully");
                }
                else if (projection.DeployedEmployees != null && projection.DeployedEmployees.Count > 0)
                {
                    // Handle Deployed Employees
                    var deployedEmployeeEntities = projection.DeployedEmployees.Select(employee => new ProjectionEmployeeDeployement
                    {
                        Projectionid = (int)projection.Projectionid,
                        DeployedEmployeeId = employee.DeployedEmployeeId,
                        Startdate = employee.StartDate ?? projection.StartDate,
                        Enddate = employee.EndDate ?? projection.EndDate,
                        Categorysubstatusid = employee.CategorySubStatusId,
                        Createdby = createdUpdatedBy.Employeeid,
                        Createddate = DateTime.Now
                    }).ToList();

                    await _context.ProjectionEmployeeDeployements.AddRangeAsync(deployedEmployeeEntities);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return ApiResponse<Projection>.Success(projectionEntity, "Projection upserted successfully");
                }
                else if (projection.ProjectionInitialBilling != null && projection.ProjectionInitialBilling.Count > 0)
                {
                    // Insert ProjectionInitialBilling entries if only ProjectionInitialBilling data is provided

                    var projectionInitialBillingEntities = projection.ProjectionInitialBilling.Select(billing => new Projectioninitialbilling
                    {
                        Projectionid = (int)projection.Projectionid,
                        Monthyear = billing.MonthYear,
                        Amount = billing.Amount,

                    }).ToList();

                    await _context.Projectioninitialbillings.AddRangeAsync(projectionInitialBillingEntities);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return ApiResponse<Projection>.Success(projectionEntity, "Projection initial billing upserted successfully");
                }

                // Updating an existing record
                else
                {
                    if (projection.CustomerId <= 0)
                    {
                        return ApiResponse<Projection>.Error("Invalid CustomerId", StatusCodes.Status400BadRequest);
                    }

                    var existingProjection = await _context.Projections.FindAsync(projection.Projectionid);
                    if (existingProjection == null)
                    {
                        return ApiResponse<Projection>.Error("Projection not found", StatusCodes.Status404NotFound);
                    }

                    existingProjection.CrmDealsid = projection.CrmId;
                    existingProjection.Projectionname = string.IsNullOrWhiteSpace(projection.ProjectionName) ? existingProjection.Projectionno : projection.ProjectionName;
                    existingProjection.Projectiondescription = projection.ProjectionDescription;
                    existingProjection.Customerid = projection.CustomerId;
                    existingProjection.Subpracticeid = projection.SubPracticeId;
                    existingProjection.Projectheadid = accountManagerId;
                    existingProjection.Startdate = projection.StartDate ?? DateTime.UtcNow;
                    existingProjection.Enddate = projection.EndDate.Value;
                    existingProjection.Projectioncost = projection.ProjectionCost;
                    existingProjection.Statusid = 2;
                    existingProjection.Projecttypeid = projection.ProjectTypeId;
                    existingProjection.Costsheetid = projection.CostsheetId;

                    _context.Projections.Update(existingProjection);
                    await _context.SaveChangesAsync();

                    // Update or add Deployed Employees if provided
                    if (projection.DeployedEmployees != null && projection.DeployedEmployees.Count > 0)
                    {
                        var deployedEmployeeEntities = projection.DeployedEmployees.Select(employee => new ProjectionEmployeeDeployement
                        {
                            Projectionid = existingProjection.Projectionid,
                            Startdate = employee.StartDate ?? existingProjection.Startdate,
                            Enddate = employee.EndDate ?? existingProjection.Enddate,
                            Categorysubstatusid = employee.CategorySubStatusId
                        }).ToList();

                        await _context.ProjectionEmployeeDeployements.AddRangeAsync(deployedEmployeeEntities);
                        await _context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                    return ApiResponse<Projection>.Success(existingProjection, "Projection updated successfully");
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ApiResponse<Projection>.Error("An error occurred while upserting the projection", StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public async Task<ApiResponse<List<ProjectionRequestResponseDto>>> GetProjectionListAsync()
        {
            try
            {
                var result = await _context.Projections.Include(x => x.Customer)
                                                     .Include(x => x.Projecthead)
                                                     .Include(x => x.ProjectionEmployeeDeployements).ThenInclude(x => x.DeployedEmployee)
                                                     .Include(x => x.ProjectionEmployeeDeployements).ThenInclude(x => x.Categorysubstatus).ThenInclude(x=>x.Categorystatus)
                                                     .Include(x => x.Status)
                                                     .Include(x => x.Subpractice).ThenInclude(x=>x.Practice)
                                                     .ToListAsync();
                var endResult = result.Select(x => new ProjectionRequestResponseDto
                {
                    Projectionid = x?.Projectionid,
                    ProjectionNo = x.Projectionno,
                    ProjectionName = x?.Projectionname,
                    ProjectionDescription = x?.Projectiondescription,
                    CustomerId = x.Customerid,
                    CustomerName = x?.Customer?.Companyname,
                    SubPracticeId = x.Subpracticeid.Value,
                    SubPracticeName = x?.Subpractice?.Subpracticename,
                    PracticeId=x?.Subpractice?.Practice?.Practiceid,
                    PracticeName = x?.Subpractice?.Practice?.Practicename,
                    ProjectHeadId = x?.Projectheadid,
                    ProjectHeadName = x?.Projecthead?.Employeename,
                    StartDate = x.Startdate,
                    EndDate = x.Enddate,
                    ProjectionCost = x?.Projectioncost,
                    StatusId = x?.Statusid,
                    StatusName = x?.Status?.Statusname,
                    CrmId = x.CrmDealsid,
                    DeployedEmployees = x.ProjectionEmployeeDeployements.Select(emp => new DeployedEmployee
                    {
                        DeployedEmployeeId = emp.DeployedEmployeeId,
                        DeployedEmployeeName = emp?.DeployedEmployee?.Employeename,
                        StartDate = emp?.Startdate,
                        EndDate = emp?.Enddate,
                        CategorySubStatusId = emp.Categorysubstatusid.Value,
                        CategorySubStatusName = emp?.Categorysubstatus?.Categorysubstatusname,
                        Remarks = emp?.Remarks,
                    }).ToList()
                }).ToList();
                return ApiResponse<List<ProjectionRequestResponseDto>>.Success(endResult);

            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProjectionRequestResponseDto>>.Error("An error occurred while upserting the projection", 500, ex.Message);
            }
        }

        public async Task<ApiResponse<object>> GetSkillNameExe_Collection()
        {
            //try
            //{
            //    var result=await _context.AggrEmployeeSkillExpeCosts.ToListAsync();
            //    var midResult=result.Select(x => new {SkillId=x.Skillid,Skill=x.Skillname,SkillExp=x.Expname }).ToList();
            //    var endResult = midResult.GroupBy( x => x.SkillId).Select(group => new
            //    { 
            //        SkillId = group.Key, 
            //        Skill = group.First().Skill,
            //        SkillExp = group.Select(x => x.SkillExp).ToList()   
            //    }).ToList();

            //    return ApiResponse<object>.Success(endResult);  

            //}
            //catch (Exception ex)
            //{ 
            //    return ApiResponse<object>.Error("internal server  error",StatusCodes.Status500InternalServerError,ex.Message);
            //}
            return null;
        }

        private async Task<string> GetMaxProjectionNumberAsync()
        {

            const string prefix = "PR/TDE/PROJN-";
            var projectionNumbers = await _context.Projections
                                                  .Select(x => x.Projectionno)
                                                  .ToListAsync();
            List<int> numbers = new List<int>();
            foreach (var projection in projectionNumbers)
            {
                var parts = projection.Split("-");
                if (parts.Length > 1 && int.TryParse(parts.Last(), out int number))
                {
                    numbers.Add(number);
                }
            }
            int maxNumber = numbers.Count > 0 ? numbers.Max() + 1 : 1;
            return $"{prefix}{maxNumber:D5}";
        }

        public async Task<IEnumerable<object>> GetProjectionRequestList(int EmployeeId)
        {
            try
            {

                var employeeDetail = await _context.Rmsemployees.FirstOrDefaultAsync(x => x.Employeeid == EmployeeId);
                string tmcId = employeeDetail.Userid;


                var result = await _context.Projectionrequests
                                                     .Include(_ => _.Projection)
                                                     .Where(_ => _.Requestsentto == employeeDetail.Employeeid)
                                                      .Select(p => new
                                                      {
                                                          ProjectionName = p.Projection.Projectionname,
                                                          ContractStartDate = p.Projection.Startdate,
                                                          ContractEndDate = p.Projection.Enddate,
                                                          Employeeid = tmcId,
                                                          ProjectionRequestId = p.Projectionrequestid,
                                                          Status = p.Status,
                                                          RequestFrom = _context.Rmsemployees
                                                          .Where(a => a.Employeeid == p.Requestsentby)
                                                          .Select(a => a.Employeename)
                                                           .FirstOrDefault()
                                                      }).ToListAsync();

                return result;


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching active projects.", ex);
            }
        }
    }
}
