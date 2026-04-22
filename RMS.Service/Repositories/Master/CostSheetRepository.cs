using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.Runtime.InteropServices;

namespace RMS.Service.Repositories.Master
{
    public class CostSheetRepository : GenericRepository<Costsheet>, ICostSheetRepository
    {
        private readonly RmsDevContext _context;
        public CostSheetRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Response> DeleteCostsheet(int CostSheetId)
        {
            // Ensure platform-agnostic transaction management
            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);

            bool detailsDeleted = false;
            bool historiesDeleted = false;
            bool headerDeleted = false;

            try
            {
                // Validate CostSheetId
                if (CostSheetId <= 0)
                {
                    throw new Exception($"Invalid CostSheetId: {CostSheetId}. No CostSheet exists.");
                }

                // Fetch the cost sheet with its related details and histories
                var costSheetHeader = await _context.Costsheets
                    .Include(details => details.Costsheetdetails)
                    .Include(history => history.CostsheetdetailHistories)
                    .FirstOrDefaultAsync(x => x.Costsheetid == CostSheetId);

                if (costSheetHeader == null)
                {
                    throw new Exception($"CostSheet does not exist for the given ID: {CostSheetId}");
                }

                // Step 1: Delete related CostSheetDetails first
                _context.Costsheetdetails.RemoveRange(costSheetHeader.Costsheetdetails);
                await _context.SaveChangesAsync();
                detailsDeleted = true;

                // Step 2: Delete related CostSheetHistories if details are deleted
                if (detailsDeleted)
                {
                    _context.CostsheetdetailHistories.RemoveRange(costSheetHeader.CostsheetdetailHistories);
                    await _context.SaveChangesAsync();
                    historiesDeleted = true;
                }

                // Step 3: Delete the CostSheet header if histories are deleted
                if (historiesDeleted)
                {
                    _context.Costsheets.Remove(costSheetHeader);
                    await _context.SaveChangesAsync();
                    headerDeleted = true;
                }

                // Commit transaction if all deletions succeed
                if (headerDeleted)
                {
                    await transaction.CommitAsync();
                    return new Response { responseCode = 200, responseMessage = "Deleted Successfully" };
                }

                // Handle failure in case some parts were not deleted
                throw new Exception("Error during deletion: not all related records were deleted.");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Concurrency issues (e.g., if another user modifies the data concurrently)
                await transaction.RollbackAsync();
                throw new Exception("Concurrency conflict detected while deleting CostSheet", ex);
            }
            catch (Exception ex)
            {
                // Rollback transaction in case of any exception
                await transaction.RollbackAsync();
                throw new Exception($"Error in deleting CostSheet: {ex.Message}", ex);
            }
        }


        public async Task<Costsheet> GetByName(string name)
        {
            return await _context.Costsheets.AsNoTracking().Where(c=>c.Costsheetname == name).FirstOrDefaultAsync();
        }



        public async Task<CostSheetDto> GetCostSheetById(int id)
        {
            try
            {
                var result = await _context.Costsheets.AsNoTracking()
                                          .Include(x => x.Costsheetdetails).ThenInclude(x => x.Skill)
                                          .Include(x => x.Oafs)
                                          .Include(x => x.CostsheetdetailHistories).ThenInclude(x => x.Skill)
                                          .Select(x => new CostSheetDto
                                          {
                                              Costsheetid = x.Costsheetid,
                                              Costsheetname = x.Costsheetname,
                                              Costsheetdetails = x.Costsheetdetails.Select(y => new CostsheetdetailDto
                                              {

                                                  Costsheetdetailid = y.Costsheetdetailid,
                                                  Costsheetid = y.Costsheetid,
                                                  Skillid = y.Skillid,
                                                  skillexperience = y.Skillexperince,
                                                  Requiredresource = y.Requiredresource,
                                                  Skillcost = y.Skillcost,
                                                  Skillname = y.Skill.Skillname,
                                                  Xvalue = y.Xvalue,
                                                  Perioddays = y.Perioddays,
                                                  Customerprice = y.Customerprice,
                                                  Totalcost = y.Totalcost,
                                                  Totalprice = y.Totalprice

                                              }).ToList(),
                                              AverageXvalue = x.Costsheetdetails.Average(d => d.Xvalue ?? 0),
                                              Oafid = x.Oafs.Any() == true ? x.Oafs.FirstOrDefault().Oafid : null,
                                              Orderdescription = x.Oafs.Any() == true ? x.Oafs.FirstOrDefault().Orderdescription : null,
                                              CostsheetHistory = x.CostsheetdetailHistories.GroupBy(y => new { y.Costsheetid, y.Version })
                                                                    .Select(g => new GroupedCostsheetDetailDto
                                                                    {
                                                                        Costsheetid = g.Key.Costsheetid,
                                                                        Version = g.Key.Version,
                                                                        Costsheetdetails = g.Select(h => new CostsheetdetailDto
                                                                        {
                                                                            Costsheetdetailid = h.Costsheetdetailid,
                                                                            Costsheetid = h.Costsheetid,
                                                                            Skillid = h.Skillid,
                                                                            skillexperience = h.Skillexperince,
                                                                            Requiredresource = h.Requiredresource,
                                                                            Skillcost = h.Skillcost,
                                                                            Skillname = h.Skill.Skillname,
                                                                            Xvalue = h.Xvalue,
                                                                            Perioddays = h.Perioddays,
                                                                            Customerprice = h.Customerprice,
                                                                            Totalcost = h.Totalcost,
                                                                            Totalprice = h.Totalprice
                                                                        }).ToList()
                                                                    }).ToList()

                                          }).FirstOrDefaultAsync(x => x.Costsheetid == id);


                return result;
                return null;

            }
            catch (Exception ex)
            {
                throw;
            }
        }     

        public async Task<List<CostSheetDto>> GetCostSheetWithDetails()
        {
            try
            {
                var result= await _context.Costsheets.AsNoTracking()
                                          .Include(x => x.Costsheetdetails)
                                          .ThenInclude(x=>x.Skill)                                          
                                          .Include(x => x.Oafs)
                                          .Select(x=> new CostSheetDto {
                                                    Costsheetid=x.Costsheetid,
                                                    Costsheetname=x.Costsheetname,
                                                    Costsheetdetails=x.Costsheetdetails.Select(y=> new CostsheetdetailDto {

                                                                                                Costsheetdetailid=y.Costsheetdetailid,
                                                                                                Costsheetid=y.Costsheetid,
                                                                                                Skillid=y.Skillid,
                                                        skillexperience = y.Skillexperince,
                                                                                                Requiredresource=y.Requiredresource,
                                                                                                Skillcost = y.Skillcost,
                                                                                                Skillname=y.Skill.Skillname,
                                                        Xvalue=y.Xvalue,
                                                        Perioddays = y.Perioddays   ,
                                                        Customerprice=y.Customerprice,
                                                        Totalcost=y.Totalcost,
                                                        Totalprice=y.Totalprice,    
                                                    }).ToList(),
                                              AverageXvalue = x.Costsheetdetails.Average(d => d.Xvalue ?? 0),
                                              Oafid = x.Oafs.Any() == true ? x.Oafs.FirstOrDefault().Oafid : null,
                                              Orderdescription = x.Oafs.Any() == true ? x.Oafs.FirstOrDefault().Orderdescription : null,
                                              TotalAmount = x.Costsheetdetails.Sum(_ => _.Totalprice ?? 0)


                                          }).ToListAsync();  
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        //public async Task<CostSheetDto> UpsertCostsheet(CostSheetDto value, JwtLoginDetailDto logindetails)
        //{
        //    int costsheetid;
        //    await using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var existingCostsheet = await _context.Costsheets
        //                                      .FirstOrDefaultAsync(x => x.Costsheetname.ToLower().Trim() == value.Costsheetname.ToLower().Trim());

        //        if (value.Costsheetid is null or <= 0)
        //        {
        //            if (existingCostsheet != null)
        //            {
        //                throw new Exception($"Costsheet with the same name already exists: {value.Costsheetname}");
        //            }

        //            var createdBy = await _context.Rmsemployees.AsNoTracking()
        //                                 .Where(x => x.Userid == logindetails.TmcId)
        //                                 .Select(x => x.Employeeid)
        //                                 .FirstOrDefaultAsync();

        //            var costSheetHeader = new Costsheet
        //            {
        //                Costsheetname = value.Costsheetname,
        //                Createdby = createdBy,
        //                Createddate = DateTime.Now
        //            };

        //            await _context.Costsheets.AddAsync(costSheetHeader);
        //            await _context.SaveChangesAsync();

        //            var costsheetDetailsList = value.Costsheetdetails.Select(item => new Costsheetdetail
        //            {
        //                Costsheetid = costSheetHeader.Costsheetid,
        //                Skillid = item.Skillid,
        //                Skillexperince = item.skillexperience,
        //                Requiredresource = item.Requiredresource,
        //                Skillcost = item.Skillcost,
        //                Xvalue = item.Xvalue,
        //                Perioddays = item.Perioddays,
        //                Customerprice = item.Customerprice,
        //                Totalcost = item.Totalcost,
        //                Totalprice = item.Totalprice
        //            }).ToList();

        //            await _context.Costsheetdetails.AddRangeAsync(costsheetDetailsList);
        //            await _context.SaveChangesAsync();
        //            costsheetid= costSheetHeader.Costsheetid;
        //        }
        //        else
        //        {
        //            var costSheetHeader = await _context.Costsheets
        //                                                .Include(details => details.Costsheetdetails)
        //                                                .FirstOrDefaultAsync(x => x.Costsheetid == value.Costsheetid);

        //            if (costSheetHeader == null)
        //            {
        //                throw new Exception($"CostSheet does not exist with the given ID: {value.Costsheetid}");
        //            }

        //            var lastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
        //                                    .Where(x => x.Userid == logindetails.TmcId)
        //                                    .Select(x => x.Employeeid)
        //                                    .FirstOrDefaultAsync();

        //            costSheetHeader.Costsheetname = value.Costsheetname;
        //            costSheetHeader.Lastupdatedate = DateTime.Now;
        //            costSheetHeader.Lastupdateby = lastUpdatedBy;
        //            #region[insert new record for the costsheet detail history]
        //            var maxVersion = await _context.CostsheetdetailHistories.AsNoTracking()
        //                                                                    .Where(x => x.Costsheetid == costSheetHeader.Costsheetid)
        //                                                                    .MaxAsync(x => x.Version);

        //            await _context.CostsheetdetailHistories.AddRangeAsync(costSheetHeader.Costsheetdetails.Select(item => new CostsheetdetailHistory
        //            {
        //                Costsheetid = costSheetHeader.Costsheetid,
        //                Skillid = item.Skillid,
        //                Skillexperince = item.Skillexperince,
        //                Requiredresource = item.Requiredresource,
        //                Skillcost = item.Skillcost,
        //                Xvalue = item.Xvalue,
        //                Perioddays = item.Perioddays,
        //                Customerprice = item.Customerprice,
        //                Totalcost = item.Totalcost,
        //                Totalprice = item.Totalprice,
        //                Version= maxVersion == null ? 1 : maxVersion+1
        //            }).ToList());
        //            #endregion[end region]

        //            _context.Costsheetdetails.RemoveRange(costSheetHeader.Costsheetdetails);
        //            await _context.SaveChangesAsync();

        //            var costsheetDetailsList = value.Costsheetdetails.Select(item => new Costsheetdetail
        //            {
        //                Costsheetid = costSheetHeader.Costsheetid,
        //                Skillid = item.Skillid,
        //                Skillexperince = item.skillexperience,
        //                Requiredresource = item.Requiredresource,
        //                Skillcost = item.Skillcost,
        //                Xvalue = item.Xvalue,
        //                Perioddays = item.Perioddays,
        //                Customerprice = item.Customerprice,
        //                Totalcost = item.Totalcost,
        //                Totalprice = item.Totalprice
        //            }).ToList();

        //            await _context.Costsheetdetails.AddRangeAsync(costsheetDetailsList);
        //            await _context.SaveChangesAsync();
        //            costsheetid = costSheetHeader.Costsheetid;
        //        }

        //        await transaction.CommitAsync();
        //        return await GetCostSheetById(costsheetid);
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        throw new Exception("Error in upserting CostSheet", ex);
        //    }
        //}
        public async Task<CostSheetDto> UpsertCostsheet(CostSheetDto value, JwtLoginDetailDto logindetails)
        {
            int costsheetid;
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingCostsheet = await _context.Costsheets
                                          .FirstOrDefaultAsync(x => x.Costsheetname.ToLower().Trim() == value.Costsheetname.ToLower().Trim());

                if (value.Costsheetid is null or <= 0)
                {
                    if (existingCostsheet != null)
                    {
                        throw new Exception($"Costsheet with the same name already exists: {value.Costsheetname}");
                    }

                    var createdBy = await _context.Rmsemployees.AsNoTracking()
                                         .Where(x => x.Userid == logindetails.TmcId)
                                         .Select(x => x.Employeeid)
                                         .FirstOrDefaultAsync();

                    var costSheetHeader = new Costsheet
                    {
                        Costsheetname = value.Costsheetname,
                        Createdby = createdBy,
                        Createddate = DateTime.Now
                    };

                    await _context.Costsheets.AddAsync(costSheetHeader);
                    await _context.SaveChangesAsync();

                    var costsheetDetailsList = new List<Costsheetdetail>();
                    foreach (var item in value.Costsheetdetails)
                    {
                        if (item.Requiredresource > 1)
                        {
                            // If Requiredresource is greater than 1, create multiple entries
                            for (int i = 0; i < item.Requiredresource; i++)
                            {
                                costsheetDetailsList.Add(new Costsheetdetail
                                {
                                    Costsheetid = costSheetHeader.Costsheetid,
                                    Skillid = item.Skillid,
                                    Skillexperince = item.skillexperience,
                                    Requiredresource = 1, // Each entry is counted as a single resource
                                    Skillcost = item.Skillcost,
                                    Xvalue = item.Xvalue,
                                    Perioddays = item.Perioddays,
                                    Customerprice = item.Customerprice / item.Requiredresource, // Divide by number of resources
                                    Totalcost = item.Totalcost / item.Requiredresource,         // Divide by number of resources
                                    Totalprice = item.Totalprice / item.Requiredresource        // Divide by number of resources
                                });
                            }
                        }
                        else
                        {
                            // If Requiredresource is 1, add a single entry
                            costsheetDetailsList.Add(new Costsheetdetail
                            {
                                Costsheetid = costSheetHeader.Costsheetid,
                                Skillid = item.Skillid,
                                Skillexperince = item.skillexperience,
                                Requiredresource = 1, // Single resource
                                Skillcost = item.Skillcost,
                                Xvalue = item.Xvalue,
                                Perioddays = item.Perioddays,
                                Customerprice = item.Customerprice, // No division needed
                                Totalcost = item.Totalcost,         // No division needed
                                Totalprice = item.Totalprice        // No division needed
                            });
                        }
                    }

                    await _context.Costsheetdetails.AddRangeAsync(costsheetDetailsList);
                    await _context.SaveChangesAsync();
                    costsheetid = costSheetHeader.Costsheetid;
                }
                else
                {
                    var costSheetHeader = await _context.Costsheets
                                                        .Include(details => details.Costsheetdetails)
                                                        .FirstOrDefaultAsync(x => x.Costsheetid == value.Costsheetid);

                    if (costSheetHeader == null)
                    {
                        throw new Exception($"CostSheet does not exist with the given ID: {value.Costsheetid}");
                    }

                    var lastUpdatedBy = await _context.Rmsemployees.AsNoTracking()
                                            .Where(x => x.Userid == logindetails.TmcId)
                                            .Select(x => x.Employeeid)
                                            .FirstOrDefaultAsync();

                    costSheetHeader.Costsheetname = value.Costsheetname;
                    costSheetHeader.Lastupdatedate = DateTime.Now;
                    costSheetHeader.Lastupdateby = lastUpdatedBy;

                    #region[insert new record for the costsheet detail history]
                    var maxVersion = await _context.CostsheetdetailHistories.AsNoTracking()
                                                                            .Where(x => x.Costsheetid == costSheetHeader.Costsheetid)
                                                                            .MaxAsync(x => x.Version);

                    await _context.CostsheetdetailHistories.AddRangeAsync(costSheetHeader.Costsheetdetails.Select(item => new CostsheetdetailHistory
                    {
                        Costsheetid = costSheetHeader.Costsheetid,
                        Skillid = item.Skillid,
                        Skillexperince = item.Skillexperince,
                        Requiredresource = item.Requiredresource,
                        Skillcost = item.Skillcost,
                        Xvalue = item.Xvalue,
                        Perioddays = item.Perioddays,
                        Customerprice = item.Customerprice,
                        Totalcost = item.Totalcost,
                        Totalprice = item.Totalprice,
                        Version = maxVersion == null ? 1 : maxVersion + 1
                    }).ToList());
                    #endregion[end region]

                    _context.Costsheetdetails.RemoveRange(costSheetHeader.Costsheetdetails);
                    await _context.SaveChangesAsync();

                    var costsheetDetailsList = new List<Costsheetdetail>();
                    foreach (var item in value.Costsheetdetails)
                    {
                        if (item.Requiredresource > 1)
                        {
                            // If Requiredresource is greater than 1, create multiple entries
                            for (int i = 0; i < item.Requiredresource; i++)
                            {
                                costsheetDetailsList.Add(new Costsheetdetail
                                {
                                    Costsheetid = costSheetHeader.Costsheetid,
                                    Skillid = item.Skillid,
                                    Skillexperince = item.skillexperience,
                                    Requiredresource = 1, // Each entry is counted as a single resource
                                    Skillcost = item.Skillcost,
                                    Xvalue = item.Xvalue,
                                    Perioddays = item.Perioddays,
                                    Customerprice = item.Customerprice / item.Requiredresource, // Divide by number of resources
                                    Totalcost = item.Totalcost / item.Requiredresource,         // Divide by number of resources
                                    Totalprice = item.Totalprice / item.Requiredresource        // Divide by number of resources
                                });
                            }
                        }
                        else
                        {
                            // If Requiredresource is 1, add a single entry
                            costsheetDetailsList.Add(new Costsheetdetail
                            {
                                Costsheetid = costSheetHeader.Costsheetid,
                                Skillid = item.Skillid,
                                Skillexperince = item.skillexperience,
                                Requiredresource = 1, // Single resource
                                Skillcost = item.Skillcost,
                                Xvalue = item.Xvalue,
                                Perioddays = item.Perioddays,
                                Customerprice = item.Customerprice, // No division needed
                                Totalcost = item.Totalcost,         // No division needed
                                Totalprice = item.Totalprice        // No division needed
                            });
                        }
                    }

                    await _context.Costsheetdetails.AddRangeAsync(costsheetDetailsList);
                    await _context.SaveChangesAsync();
                    costsheetid = costSheetHeader.Costsheetid;
                }

                await transaction.CommitAsync();
                return await GetCostSheetById(costsheetid);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error in upserting CostSheet", ex);
            }
        }



        private Costsheetdetail CreateCostSheetDetail(CostsheetdetailDto item, int costSheetId)
        {
            return new Costsheetdetail
            {
                Costsheetid = costSheetId,
                Skillid = item.Skillid,
                Skillexperince = item.skillexperience,
                Requiredresource = item.Requiredresource,
                Skillcost = item.Skillcost,
                Xvalue=item.Xvalue,
                Perioddays=item.Perioddays,
                Customerprice=item.Customerprice,
                Totalcost=item.Totalcost,
                Totalprice=item.Totalprice
            };
        }     
        public async Task<bool> CheckCostsheet(int? CostSheetid, string CostSheetName)
        {
            try
            {

                if (CostSheetid > 0)
                {
                    var result = await _context.Costsheets.AsNoTracking().AnyAsync(x => x.Costsheetname == CostSheetName
                                          && x.Costsheetid == CostSheetid);
                    if (result == true)
                    {
                        return false;
                    }
                    else
                    {
                        return await _context.Costsheets.AsNoTracking().AnyAsync(x => x.Costsheetname == CostSheetName);
                    }

                }
                else
                {
                    return await _context.Costsheets.AsNoTracking().AnyAsync(x => x.Costsheetname == CostSheetName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}