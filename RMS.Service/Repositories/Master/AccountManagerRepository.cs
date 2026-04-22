using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.Collections;
using System.Transactions;

namespace RMS.Service.Repositories.Master
{
    public class AccountManagerRepository : GenericRepository<Rmsemployee>, IAccountManagerRepository
    {
        private readonly RmsDevContext _context;
        public AccountManagerRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AccountManagerDetailsDto>> GetAccountManagerList()
        {
            // If you don't log/handle, no need for try/catch; let it bubble up.
            return await _context.Rmsemployees
                .AsNoTracking()
                .Where(emp =>
                    emp.Udf3 != "2" &&
                    (
                        // Group related business rules explicitly
                        (emp.Sbuid == 1 && emp.Departmentid == 8) ||
                        (emp.Sbuid == 22 && emp.Departmentid == 4) ||
                        (emp.Sbuid == 1 && emp.Departmentid == 94) ||
                        (emp.Sbuid == 55 && emp.Departmentid == 8)
                    )
                )
                .Select(x => new AccountManagerDetailsDto
                {
                    AccountManagerId = x.Employeeid,
                    AccountManagerName = x.Employeename
                })
                .ToListAsync();
        }

        public async Task<IEnumerable> GetDAList()
        {

            try
            {
                var DaIds = await _context.Projectcontracts.AsNoTracking().Select(x => x.Deliveryanchorid).ToListAsync();
                if (DaIds.Any())
                {

                    var filterResult = _context.Rmsemployees.AsNoTracking().Where(x => DaIds.Contains(x.Employeeid))
                                               .Select(x => new { x.Employeeid, x.Employeename }).ToList().Distinct().OrderBy(x => x.Employeename);
                    return filterResult;
                }
                else
                {
                    throw new Exception("No DA are found");
                }

            }
            catch (Exception ex)
            {
                throw;

            }
        }


        public async Task<Response> ProvisionBillingSubmission(List<ProvisionBillingSubmittingDataDto> obj)
        {
            // Begin transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var objItem in obj)
                {
                    var existingContractBillingProvision = await _context.Contractbillingprovesions.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Contractbillingprovesionid == objItem.ProvisionBillingId);

                    if (existingContractBillingProvision != null)
                    {

                        existingContractBillingProvision.Isbilled = true;
                        existingContractBillingProvision.Recievedbillingamount = objItem.BilledAmount;
                        _context.Contractbillingprovesions.UpdateRange(existingContractBillingProvision);
                    }
                    else
                    {
                        continue;
                    }


                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new Response { responseMessage = "Updated Succesfully", responseCode = 200 };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // DA dropdown based on PracticeId (Testing Pending)
        public async Task<IEnumerable> GetFilteredDAList(int practiceId)
        {

            try
            {
                var DaIds = await _context.Projectcontracts.AsNoTracking().Select(x => x.Deliveryanchorid).ToListAsync();
                if (!DaIds.Any())
                    throw new Exception("No DA are found");

                var employeeId = await _context.Deliveryheads.AsNoTracking()
                            .Where(dh => dh.Practiceid == practiceId && DaIds.Contains(dh.Employeeid))
                            .Select(dh => dh.Employeeid)
                            .ToListAsync();

                if (!employeeId.Any())
                    throw new Exception("No DA are found");

                var filterResult = await _context.Rmsemployees.AsNoTracking()
                           .Where(e => employeeId.Contains(e.Employeeid))
                           .Select(e => new { e.Employeeid, e.Employeename }).Distinct().OrderBy(e => e.Employeename).ToListAsync();

                return filterResult;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //nick

        public async Task<IEnumerable> PracticewiseDA(int practiceId)
        {
            try
            {
                // Step 1: Get Employee IDs with Role ID = 3 (Delivery Anchors)
                var daEmployeeIds = await _context.Employeeroles.AsNoTracking()
                                        .Where(x => x.Roleid == 3)
                                        .Select(x => x.Employeeid)
                                        .ToListAsync();

                if (!daEmployeeIds.Any())
                    throw new Exception("No Delivery Anchors found based on role.");

                // Step 2: Filter RMSEmployees with the obtained DA Employee IDs
                var validDaIds = await _context.Rmsemployees.AsNoTracking()
                                        .Where(emp => daEmployeeIds.Contains(emp.Employeeid))
                                        .Select(emp => emp.Employeeid)
                                        .ToListAsync();

                if (!validDaIds.Any())
                    throw new Exception("No matching employees found for Delivery Anchors.");

                // Step 3: Get Employee IDs from DeliveryHeads(its an deliveryanchor table name wrongly added) based on PracticeId
                var deliveryHeadEmployeeIds = await _context.Deliveryheads.AsNoTracking()
                                                  .Where(dh => dh.Practiceid == practiceId && validDaIds.Contains(dh.Employeeid))
                                                  .Select(dh => dh.Employeeid)
                                                  .ToListAsync();

                if (!deliveryHeadEmployeeIds.Any())
                    throw new Exception("No Delivery Anchors found for the specified practice.");

                // Step 4: Get the filtered DA Employee details from RMSEmployees
                var result = await _context.Rmsemployees.AsNoTracking()
                                .Where(emp => deliveryHeadEmployeeIds.Contains(emp.Employeeid))
                                .Select(emp => new { emp.Employeeid, emp.Employeename })
                                .Distinct()
                                .OrderBy(emp => emp.Employeename)
                                .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Delivery Anchors.", ex);
            }
        }

    }
}