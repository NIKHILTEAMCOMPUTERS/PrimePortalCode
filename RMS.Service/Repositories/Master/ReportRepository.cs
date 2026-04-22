using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RMS.Data.DTOs;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.Collections;

namespace RMS.Service.Repositories.Master
{
    public class ReportRepository : IReportRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportRepository(RmsDevContext context, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        #region [Reporting section]
        public async Task<List<rptContractBillingDto>> GetReportByMonthYearProcedure(string monthyear)
        {
            try
            {
                var result = await _context.rptContractBillingDtos
                                                    .FromSqlRaw("SELECT * FROM rptcontractbillingmonthyear({0})", monthyear)
                                                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<IEnumerable> GetReportByMonthYearProcedure_DA_Wise(string monthyear)
        //{
        //    try
        //    {
        //        //var endresult = new List<rptContratBillingDA_WiseDto>();
        //        var result = await _context.rptContractBillingDtos
        //                                            .FromSqlRaw("SELECT * FROM rptcontractbillingmonthyear({0})", monthyear)
        //                                            .ToListAsync();
        //        result = result.Where(x =>x.DAname != null).Distinct().ToList();

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


        public async Task<IEnumerable> GetReportByMonthYearProcedure_DA_Wise(string monthyear)
        {
            try
            {
                // Fetch contract billing data
                var result = await _context.rptContractBillingDtos
                                           .FromSqlRaw("SELECT * FROM rptcontractbillingmonthyear({0})", monthyear)
                                           .ToListAsync();

                // Get the current month and year
                var currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var nextMonthStart = currentMonthStart.AddMonths(1);

                // Fetch projection data
                var projectionData = await _context.CrmAggregatedDataFromAllTables.ToListAsync();
            

                // Filter projection data
                var filteredProjections = projectionData
                    .Where(deal =>
                        deal.Stageposition == 5 && // Only include Stage 5
                        (
                            (deal.ExpectedClose != null && deal.ExpectedClose >= currentMonthStart && deal.ExpectedClose < nextMonthStart) || // Check ExpectedClose
                            (deal.ExpectedClose == null && deal.CreatedAt >= currentMonthStart && deal.CreatedAt < nextMonthStart) // Check CreatedAt if ExpectedClose is null
                        ))
                    .ToList();

                // Calculate total projection amount
                var totalProjectionAmount = filteredProjections.Sum(deal => deal.Amount);

                // Group and process contract billing data
                var groupedResult = result
                    .Where(x => !string.IsNullOrEmpty(x.DAname)) // Filter non-null DAname
                    .GroupBy(x => x.DAname)
                    .Select(g => new
                    {
                        DAname = g.Key,
                        TAndMCount = g.Where(p => p.ProjectType == "T&M").Select(p => p.projectno).Distinct().Count(),
                        FixedBidCount = g.Where(p => p.ProjectType == "Fixed Bid").Select(p => p.projectno).Distinct().Count(),
                        PresalesCount = g.Where(p => p.ProjectType == "Presales").Select(p => p.projectno).Distinct().Count(),
                        InternalCount = g.Where(p => p.ProjectType == "Internal").Select(p => p.projectno).Distinct().Count(),
                        ProjectCount = g.Select(x => x.projectno).Distinct().Count(),
                        TotalActualBilling = g.Sum(x => x.actualbilling ?? 0), // Handle null values safely
                        TotalProvisionBilling = g.Sum(x => x.provesionbilling ?? 0), // Handle null values safely
                    })
                    .ToList();

                // Add total projection row
                var totalProjectionRow = new
                {
                    DAname = "Total Projection",
                    TAndMCount = 0, // Empty for totals
                    FixedBidCount = 0,
                    PresalesCount = 0,
                    InternalCount = 0,
                    ProjectCount = 0,
                    TotalActualBilling = 0.0m,
                    TotalProvisionBilling = 0.0m,
                    TotalProjectionAmount = totalProjectionAmount // Show the total projection amount here
                };

                // Append total projection row
                var finalResult = groupedResult.Cast<object>().ToList(); // Convert groupedResult to object list
                finalResult.Add(totalProjectionRow);

                return finalResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        public async Task<IEnumerable> GetRptProvisionHistory(string monthyear, int contractid)
        {
            try
            {
                var result = await _context.RevisionDetailsDtos
                                                    .FromSqlRaw("SELECT * FROM getProvision_Revision({0},{1})", monthyear, contractid)
                                                    .ToListAsync();

                var groupedResult = result.GroupBy(x => new
                {
                    x.TMC,
                    x.ResourceName,
                    x.ProjectName,
                    x.ProjectNo,
                    x.ContractNo,
                    x.ContractId,
                    x.PONumber,
                    x.DeliveryAnchor,                    
                }
                                                   )
                                                            .Select(group => new
                                                            {
                                                                Item = group.Key,
                                                                ItemDetails = group.Select(item => new
                                                                {
                                                                    item.RevisionNumber,
                                                                    item.ProvisionAmount,
                                                                    item.CreatedDate,
                                                                    item.EstimatedBillingDate,
                                                                    item.ApprovalStatus,
                                                                }).ToList()
                                                            });


                return groupedResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<rptContractBillingProvisionDto>> GetReportByMonthYearProcedureProvision(string monthyear, int? DeliveryAnchorId )
        {
            try
            {
                var result = DeliveryAnchorId != null ? await _context.rptContractBillingProvisionDtos
                                                    .FromSqlRaw("SELECT * FROM GetRptMonthForBilled({0},{1})", monthyear, DeliveryAnchorId)
                                                    .ToListAsync()
                                                    : await _context.rptContractBillingProvisionDtos
                                                    .FromSqlRaw("SELECT * FROM GetRptMonthForBilled({0},{1})", monthyear, null)
                                                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw;



            }
        }
        public async Task<List<rptContractBillingActualMapperDto>> GetReportByMonthYearProcedureActualBilling(string monthyear, int? DeliveryAnchorId)
        {
            var mappedResult=new List<rptContractBillingActualMapperDto>();
            
            try
            {
        


                var result = DeliveryAnchorId!=null? await _context.rptContractBillingActualDtos
                                                    .FromSqlRaw("SELECT * FROM getrptmonthforbilled_tobebilled_actual({0},{1})", monthyear, DeliveryAnchorId)
                                                    .ToListAsync()
                                                    : await _context.rptContractBillingActualDtos
                                                    .FromSqlRaw("SELECT * FROM getrptmonthforbilled_tobebilled_actual({0},{1})", monthyear,null)
                                                    .ToListAsync();
                mappedResult = result.Select(r=> new rptContractBillingActualMapperDto
                {
                    contractbillingid=r.contractbillingid,
                    EmpId=r.EmpId,
                    TMC=r.TMC,
                    ResourceName = r.ResourceName,
                    CCname = r.CCname,
                    ProjectType = r.ProjectType,
                    CustomerName = r.CustomerName,
                    DAname = r.DAname,
                    SPname = r.SPname  ,
                    Pname = r.Pname,
                    Cno = r.Cno,
                    POno=r.POno,
                    contractstartdate=r.contractstartdate,
                    contractenddate=r.contractenddate,
                    invoiceperiod = r.invoiceperiod,
                    billing=r.billing,
                    projectname= r.projectname,
                    projectno= r.projectno,
                    billingmonth=r.billingmonth,
                    contractemployeeid = r.contractemployeeid,
                    isbilled = r.isbilled,
                    istobebilled = r.istobebilled,
                    estimatedbillingdate = r.estimatedbillingdate,
                     BillingStatus =  r.isbilled==true? "isbilled" : (r.istobebilled==true? "istobebilled" : null),
                    //BillingStatus = (r.statusid == 4 || r.statusid == 5) ? null: r.isbilled==true? "isbilled": r.istobebilled==true && r.statusid!=2  ? "istobebilled": null,
                    //BillingStatus=(r.isrevised==false)
                    //                                  ?
                    //                                  (r.statusid==2 && r.isbilled==true)?"Isbilled"
                    //                                    : (r.statusid == 2 && r.istobebilled == true) ? "Istobebilled"
                    //                                         : (r.statusid ==5)?"Approve"
                    //                                            : (r.statusid == 4)?"Reject": "Draft"
                    //                                  : (r.statusid == 5) ? "Approve": (r.statusid == 4) ?"Reject": "Draft",
                    StatusId=r.statusid,

                    DocumentUrl = string.IsNullOrEmpty(r.documenturl) ? "" 
                                  : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{r.documenturl}",
                }).Distinct().ToList();
                return mappedResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<BillingDropdownMapperDto>> BillingDropDown()
        {
            var mapperResult= new List<BillingDropdownMapperDto>(); 
            try
            {
               
        
                var result = await _context.BillingDropDownDtos.FromSqlRaw("select * from BillingDropDown()").ToListAsync();
                mapperResult=result.Select(r=>new BillingDropdownMapperDto
                {
                    Text=r.texts== "isbilled"?"Billed":(r.texts == "istobebilled"? "TobeBilled":""),
                    Value= r.texts
                }).ToList();  

                return mapperResult;    

            }
            catch (Exception ex) 
            {
                throw;
            }
        }
        public async Task<List<ResourceInfoForHRDto>> GetResourcesInfoForHR()
        {
            var result = new List<ResourceInfoForHRDto>();
            try
            {
                // result = await _context.ResourceInfoForHRDtos.FromSqlRaw("select * from GetResourcesInfoForHR()").ToListAsync();
                result = await _context.ResourceInfoForHRDtos.FromSqlRaw("select * from getresourcesinfoforhr_new()").ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion[Reporting section]



        #region mailer
        public async Task<List<MonthWiseDAReport>> GetReportByMonthYearProcedure_DA_WiseForMailer(string monthyear)
        {
            try
            {
                List<MonthWiseDAReport> monthWiseDAReports = new List<MonthWiseDAReport>();
                //var endresult = new List<rptContratBillingDA_WiseDto>();
                var result = await _context.rptContractBillingDtos
                                                    .FromSqlRaw("SELECT * FROM mailing_rptcontractbillingmonthyear({0})", monthyear)
                                                    .ToListAsync();
                result = result.Where(x => x.DAname != null).Distinct().ToList();


                var groupedResult = result
                                        .GroupBy(x => x.DAname)
                                                .Select(g => new
                                                {
                                                    DAname = g.Key,
                                                    TAndMCount = g.Where(p => p.ProjectType == "T&M").Select(p => p.projectno).Distinct().Count(),
                                                    FixedBidCount = g.Where(p => p.ProjectType == "Fixed Bid").Select(p => p.projectno).Distinct().Count(),
                                                    PresalesCount = g.Where(p => p.ProjectType == "Presales").Select(p => p.projectno).Distinct().Count(),
                                                    InternalCount = g.Where(p => p.ProjectType == "Internal").Select(p => p.projectno).Distinct().Count(),
                                                    ProjectCount = g.Select(x => x.projectno).Distinct().Count(),
                                                    TotalPendingProvisions = g.Where(x => x.provesionbilling != null && x.provisionstatusid == 2).Sum(x => x.provesionbilling),
                                                    TotalActualBilling = g.Sum(x => x.actualbilling),
                                                    TotalProvisionBilling = g.Where(x => x.provesionbilling != null && x.provisionstatusid == 5).Sum(x => x.provesionbilling) // Sum of provision billing, considering non-null values
                                                }).OrderBy(x => x.DAname);

                foreach (var g in groupedResult)
                {
                    // MonthWiseDAReport data = new MonthWiseDAReport();
                    //g.DAname = data.DAname
                    monthWiseDAReports.Add(new MonthWiseDAReport
                    {
                        DAname = g.DAname,
                        TAndMCount = g.TAndMCount,
                        FixedBidCount = g.FixedBidCount,
                        PresalesCount = g.PresalesCount,
                        InternalCount = g.InternalCount,
                        ProjectCount = g.ProjectCount,
                        TotalActualBilling = g.TotalActualBilling,
                        TotalProvisionBilling = g.TotalProvisionBilling,
                        TotalPendingProvisions = g.TotalPendingProvisions

                    });
                }

                //if(groupedResult!=null || groupedResult.Count>0)
                //{
                //    foreach (var item in groupedResult)
                //    {
                //        endresult.Add( new rptContratBillingDA_WiseDto
                //        {
                //            CCname=item.Key.CCname==null ? null : item.Key.CCname,
                //            ProjectType=item.Key.ProjectType==null ? null : item.Key.ProjectType,
                //            CustomerName=item.Key.CustomerName==null ? null : item.Key.CustomerName,
                //            DAname=item.Key.DAname==null ? null : item.Key.DAname,
                //            Cno = item.Key.Cno == null ? null : item.Key.Cno,
                //            POno = item.Key.POno == null ? null : item.Key.POno,
                //            contractstartdate = item.Key.contractstartdate == null ? null : item.Key.contractstartdate ,
                //            contractenddate = item.Key.contractenddate == null ? null : item.Key.contractenddate,
                //            projectname = item.Key.projectname == null ? null : item.Key.projectname ,
                //            projectno = item.Key.projectno == null ? null : item.Key.projectno ,
                //            billingmonth = item.Key.billingmonth == null ? null : item.Key.billingmonth,
                //            totalActualBilling=item.TotalActualBilling,
                //            totalProvisionBilling=item.TotalProvisionBilling
                //        });

                //    }

                //}

                return monthWiseDAReports;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}