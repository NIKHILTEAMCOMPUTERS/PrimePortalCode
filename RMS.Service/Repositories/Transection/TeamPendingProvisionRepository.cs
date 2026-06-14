using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Transection;
using System.Text.Json;

namespace RMS.Service.Repositories.Transection
{
    public class TeamPendingProvisionRepository : ITeamPendingProvisionRepository
    {
        private readonly RmsDevContext _context;

        public TeamPendingProvisionRepository(RmsDevContext context)
        {
            _context = context;
        }

        public async Task<List<TeamPendingProvisionListDto>> GetPendingList(JwtLoginDetailDto loginDetails)
        {
            var today = DateTime.Today;

            var query = from p in _context.Contractbillingprovesions
                        join ce in _context.Contractemployees on p.Contractemployeeid equals ce.Contractemployeeid
                        join e in _context.Rmsemployees on ce.Employeeid equals e.Employeeid
                        join c in _context.Projectcontracts on ce.Contractid equals c.Contractid
                        join proj in _context.Projects on c.Projectid equals proj.Projectid into projG
                        from proj in projG.DefaultIfEmpty()
                        join cust in _context.Customers on proj.Customerid equals cust.Customerid into custG
                        from cust in custG.DefaultIfEmpty()
                        join pt in _context.Projecttypes on proj.Projecttypeid equals pt.Projecttypeid into ptG
                        from pt in ptG.DefaultIfEmpty()
                        join am in _context.Rmsemployees on proj.Accountmanagerid equals am.Employeeid into amG
                        from am in amG.DefaultIfEmpty()
                        join da in _context.Rmsemployees on c.Deliveryanchorid equals da.Employeeid into daG
                        from da in daG.DefaultIfEmpty()
                        join sp in _context.Subpractices on proj.Subpracticeid equals sp.Subpracticeid into spG
                        from sp in spG.DefaultIfEmpty()
                        join prac in _context.Practices on sp.Practiceid equals prac.Practiceid into pracG
                        from prac in pracG.DefaultIfEmpty()
                        join track in _context.TeamProvisionTrackings on p.Contractbillingprovesionid equals track.ProvisionId into trackG
                        from track in trackG.Where(t => !t.IsDeleted).DefaultIfEmpty()
                        where !p.Isdeleted
                           && p.ProvisionStatus != "Reversed"
                           && p.ProvisionStatus != "Settled"
                           && (p.Costing ?? 0m) > 0
                        orderby p.Createddate ascending
                        select new TeamPendingProvisionListDto
                        {
                            ProvisionId = p.Contractbillingprovesionid,
                            TeamTrackingId = track != null ? (int?)track.Id : null,
                            ResourceName = e.Employeename ?? "",
                            TmcId = e.Userid ?? "",
                            CustomerName = cust != null ? cust.Companyname : "",
                            ProjectName = proj != null ? proj.Projectname : "",
                            ContractNo = c.Contractno ?? "",
                            PoNumber = c.Ponumber ?? "",
                            BillingMonthYear = p.Billingmonthyear ?? "",
                            ProvisionAmount = p.Costing ?? 0m,
                            ProvisionStatus = p.ProvisionStatus ?? "Active",
                            CreatedDate = p.Createddate,
                            DaysPending = (today - p.Createddate.Date).Days,
                            AccountManager = am != null ? am.Employeename : null,
                            DeliveryAnchor = da != null ? da.Employeename : null,
                            Practice = prac != null ? prac.Practicename : null,
                            Products = sp != null ? sp.Subpracticename : null,
                            ProjectType = pt != null ? pt.Projecttypename : null,
                            CloserDate = track != null ? track.CloserDate : null,
                            DocumentNo = track != null ? track.DocumentNo : null,
                            BilledAmount = track != null ? track.BilledAmount : 0m,
                            HasHistory = _context.TeamProvisionHistories
                                .Any(h => h.ProvisionId == p.Contractbillingprovesionid && !h.IsDeleted)
                        };

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Response> UpdateCloserDate(TeamUpdateCloserDateDto dto, JwtLoginDetailDto loginDetails)
        {
            var employeeId = await GetEmployeeIdAsync(loginDetails.TmcId);
            var track = await GetOrCreateTracking(dto.ProvisionId, employeeId);

            var oldDate = track.CloserDate;
            track.CloserDate = dto.CloserDate;
            track.LastUpdatedBy = employeeId;
            track.LastUpdatedDate = DateTime.Now;
            _context.TeamProvisionTrackings.Update(track);

            AddHistory(track.Id, dto.ProvisionId, "CLOSER_DATE_CHANGED",
                loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                new { CloserDate = oldDate?.ToString("dd/MM/yyyy") },
                new { CloserDate = dto.CloserDate?.ToString("dd/MM/yyyy") },
                employeeId);

            await _context.SaveChangesAsync();
            return Ok($"Closer date updated to {dto.CloserDate?.ToString("dd/MM/yyyy") ?? "—"}.");
        }

        public async Task<Response> UpdateDocumentNo(TeamUpdateDocumentNoDto dto, JwtLoginDetailDto loginDetails)
        {
            if (string.IsNullOrWhiteSpace(dto.MonthYear) || string.IsNullOrWhiteSpace(dto.PoNumber))
                return Fail(400, "Month/Year and PO Number are required.");

            var employeeId = await GetEmployeeIdAsync(loginDetails.TmcId);

            var provisions = await (from p in _context.Contractbillingprovesions
                                    join ce in _context.Contractemployees on p.Contractemployeeid equals ce.Contractemployeeid
                                    join c in _context.Projectcontracts on ce.Contractid equals c.Contractid
                                    where !p.Isdeleted
                                       && p.Billingmonthyear == dto.MonthYear
                                       && c.Ponumber == dto.PoNumber
                                       && p.ProvisionStatus != "Reversed"
                                    select p.Contractbillingprovesionid).ToListAsync();

            if (!provisions.Any())
                return Fail(404, $"No provisions found for {dto.MonthYear} with PO '{dto.PoNumber}'.");

            foreach (var provId in provisions)
            {
                var track = await GetOrCreateTracking(provId, employeeId);
                var oldDoc = track.DocumentNo;
                track.DocumentNo = dto.DocumentNo;
                track.LastUpdatedBy = employeeId;
                track.LastUpdatedDate = DateTime.Now;
                _context.TeamProvisionTrackings.Update(track);

                AddHistory(track.Id, provId, "DOCUMENT_NO_UPDATED",
                    loginDetails.Name ?? loginDetails.TmcId, null,
                    new { DocumentNo = oldDoc },
                    new { DocumentNo = dto.DocumentNo },
                    employeeId);
            }

            await _context.SaveChangesAsync();
            return Ok($"Document No updated for {provisions.Count} provision(s).");
        }

        public async Task<Response> RecordBilled(TeamRecordBilledDto dto, JwtLoginDetailDto loginDetails)
        {
            if (dto.BilledAmount <= 0)
                return Fail(400, "Billed amount must be greater than zero.");

            var provision = await _context.Contractbillingprovesions
                .FirstOrDefaultAsync(p => p.Contractbillingprovesionid == dto.ProvisionId && !p.Isdeleted);
            if (provision == null) return Fail(404, "Provision not found.");

            var employeeId = await GetEmployeeIdAsync(loginDetails.TmcId);
            var track = await GetOrCreateTracking(dto.ProvisionId, employeeId);

            var provAmount = provision.Costing ?? 0m;
            var newTotal = track.BilledAmount + dto.BilledAmount;
            if (newTotal > provAmount)
                return Fail(400, $"Total billed (₹{newTotal:N0}) would exceed provision amount (₹{provAmount:N0}).");

            var oldTotal = track.BilledAmount;
            track.BilledAmount = newTotal;
            track.LastUpdatedBy = employeeId;
            track.LastUpdatedDate = DateTime.Now;
            _context.TeamProvisionTrackings.Update(track);

            AddHistory(track.Id, dto.ProvisionId, "BILLED",
                loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                new { TotalBilled = oldTotal, Remaining = provAmount - oldTotal },
                new { ThisPayment = dto.BilledAmount, TotalBilled = newTotal, Remaining = provAmount - newTotal },
                employeeId);

            await _context.SaveChangesAsync();
            return Ok($"₹{dto.BilledAmount:N0} recorded. Total billed: ₹{newTotal:N0}.");
        }

        public async Task<List<TeamProvisionHistoryDto>> GetHistory(int provisionId)
        {
            return await _context.TeamProvisionHistories
                .Where(h => h.ProvisionId == provisionId && !h.IsDeleted)
                .OrderByDescending(h => h.CreatedDate)
                .Select(h => new TeamProvisionHistoryDto
                {
                    Id = h.Id,
                    ProvisionId = h.ProvisionId,
                    ActionType = h.ActionType,
                    ActionBy = h.ActionBy,
                    OldValues = h.OldValues,
                    NewValues = h.NewValues,
                    Remark = h.Remark,
                    CreatedDate = h.CreatedDate
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // ── helpers ──────────────────────────────────────────────────────────

        private async Task<TeamProvisionTracking> GetOrCreateTracking(int provisionId, int employeeId)
        {
            var track = await _context.TeamProvisionTrackings
                .FirstOrDefaultAsync(t => t.ProvisionId == provisionId && !t.IsDeleted);

            if (track == null)
            {
                track = new TeamProvisionTracking
                {
                    ProvisionId = provisionId,
                    BilledAmount = 0m,
                    IsDeleted = false,
                    CreatedBy = employeeId,
                    CreatedDate = DateTime.Now,
                    LastUpdatedBy = employeeId,
                    LastUpdatedDate = DateTime.Now
                };
                _context.TeamProvisionTrackings.Add(track);
                await _context.SaveChangesAsync();
            }
            return track;
        }

        private void AddHistory(int trackingId, int provisionId, string actionType, string actionBy,
            string? remark, object? oldValues, object? newValues, int createdBy)
        {
            _context.TeamProvisionHistories.Add(new TeamProvisionHistory
            {
                TeamTrackingId = trackingId,
                ProvisionId = provisionId,
                ActionType = actionType,
                ActionBy = actionBy,
                Remark = remark,
                OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
                IsDeleted = false,
                CreatedBy = createdBy,
                CreatedDate = DateTime.Now
            });
        }

        private async Task<int> GetEmployeeIdAsync(string tmcId)
        {
            return await _context.Rmsemployees
                .Where(e => e.Userid == tmcId)
                .Select(e => e.Employeeid)
                .FirstOrDefaultAsync();
        }

        private static Response Ok(string message) =>
            new Response { responseCode = 200, responseMessage = message };

        private static Response Fail(int code, string message) =>
            new Response { responseCode = code, responseMessage = message };
    }
}
