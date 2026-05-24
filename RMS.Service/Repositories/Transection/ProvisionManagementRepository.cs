using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Transection;
using System.Globalization;
using System.Text.Json;

namespace RMS.Service.Repositories.Transection
{
    public class ProvisionManagementRepository : GenericRepository<Contractbillingprovesion>, IProvisionManagementRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProvisionManagementRepository(RmsDevContext context, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
            : base(context)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ProvisionManagementListDto>> GetProvisionList(string monthYear, JwtLoginDetailDto loginDetails)
        {
            var query = from p in _context.Contractbillingprovesions
                        join ce in _context.Contractemployees on p.Contractemployeeid equals ce.Contractemployeeid
                        join e in _context.Rmsemployees on ce.Employeeid equals e.Employeeid
                        join c in _context.Projectcontracts on ce.Contractid equals c.Contractid
                        join proj in _context.Projects on c.Projectid equals proj.Projectid into projGroup
                        from proj in projGroup.DefaultIfEmpty()
                        join cust in _context.Customers on proj.Customerid equals cust.Customerid into custGroup
                        from cust in custGroup.DefaultIfEmpty()
                        join pt in _context.Projecttypes on proj.Projecttypeid equals pt.Projecttypeid into ptGroup
                        from pt in ptGroup.DefaultIfEmpty()
                        join am in _context.Rmsemployees on proj.Accountmanagerid equals am.Employeeid into amGroup
                        from am in amGroup.DefaultIfEmpty()
                        join da in _context.Rmsemployees on c.Deliveryanchorid equals da.Employeeid into daGroup
                        from da in daGroup.DefaultIfEmpty()
                        join sp in _context.Subpractices on proj.Subpracticeid equals sp.Subpracticeid into spGroup
                        from sp in spGroup.DefaultIfEmpty()
                        join prac in _context.Practices on sp.Practiceid equals prac.Practiceid into pracGroup
                        from prac in pracGroup.DefaultIfEmpty()
                        where !p.Isdeleted
                              && (string.IsNullOrEmpty(monthYear) || p.Billingmonthyear == monthYear)
                        orderby p.Createddate descending
                        select new ProvisionManagementListDto
                        {
                            ProvisionId = p.Contractbillingprovesionid,
                            ResourceName = e.Employeename ?? "",
                            TmcId = e.Userid ?? "",
                            CustomerName = cust != null ? cust.Companyname : "",
                            ProjectName = proj != null ? proj.Projectname : "",
                            ContractNo = c.Contractno ?? "",
                            BillingMonthYear = p.Billingmonthyear ?? "",
                            ProvisionAmount = p.Costing ?? 0m,
                            PaidAmount = p.Recievedbillingamount ?? 0m,
                            RemainingAmount = (p.Costing ?? 0m) - (p.Recievedbillingamount ?? 0m),
                            CarryForwardCount = p.CarryForwardCount,
                            ProvisionStatus = p.ProvisionStatus ?? "Active",
                            CarryForwardFromId = p.CarryForwardFromId,
                            CreatedDate = p.Createddate,
                            EstimatedBillingDate = p.EstimatedBillingDate,
                            AccountManager = am != null ? am.Employeename : null,
                            ProjectType = pt != null ? pt.Projecttypename : null,
                            DeliveryAnchor = da != null ? da.Employeename : null,
                            InvoicePeriod = c.Invoiceperiod,
                            ContractStartDate = c.Contractstartdate,
                            ContractEndDate = c.Contractenddate,
                            ToplineBillingAmt = c.Amount,
                            Products = sp != null ? sp.Subpracticename : null,
                            Practice = prac != null ? prac.Practicename : null,
                            PoNumber = c.Ponumber,
                            ProjectNo = proj != null ? proj.Projectno : null,
                            CarriedForwardToMonth = (from t in _context.Contractbillingprovesions
                                                     where t.CarryForwardFromId == p.Contractbillingprovesionid
                                                           && !t.Isdeleted && t.ProvisionStatus != "Reversed"
                                                     orderby t.Createddate descending
                                                     select t.Billingmonthyear).FirstOrDefault(),
                            CarriedForwardToDate = (from t in _context.Contractbillingprovesions
                                                    where t.CarryForwardFromId == p.Contractbillingprovesionid
                                                          && !t.Isdeleted && t.ProvisionStatus != "Reversed"
                                                    orderby t.Createddate descending
                                                    select t.EstimatedBillingDate).FirstOrDefault(),
                            HasHistory = _context.Contractbillingprovisionhistories
                                .Any(h => h.Contractbillingprovesionid == p.Contractbillingprovesionid && !h.Isdeleted),
                            IsTerminalTarget = p.CarryForwardFromId != null &&
                                _context.Contractbillingprovesions
                                    .Any(s => s.Contractbillingprovesionid == p.CarryForwardFromId
                                           && s.CarryForwardCount >= 3
                                           && !s.Isdeleted)
                        };

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<Response> CarryForward(ProvisionActionDto dto, JwtLoginDetailDto loginDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var provision = await _context.Contractbillingprovesions
                    .FirstOrDefaultAsync(p => p.Contractbillingprovesionid == dto.ProvisionId && !p.Isdeleted);

                if (provision == null) return Fail(404, "Provision not found.");
                if (provision.ProvisionStatus == "Reversed" || provision.ProvisionStatus == "Settled")
                    return Fail(400, $"Cannot carry forward a {provision.ProvisionStatus} provision.");
                if (provision.ProvisionStatus == "CarriedForward")
                    return Fail(400, "This provision has already been carried forward.");
                if (provision.CarryForwardCount >= 3)
                    return Fail(400, "Maximum carry-forward limit (3) reached. Please reverse this provision.");

                var remaining = (provision.Costing ?? 0m) - (provision.Recievedbillingamount ?? 0m);
                if (remaining <= 0)
                    return Fail(400, "No remaining amount to carry forward.");

                var employeeId = await GetEmployeeIdAsync(loginDetails.TmcId);
                var nextMonthYear = !string.IsNullOrWhiteSpace(dto.TargetMonth)
                    ? ParseInputMonth(dto.TargetMonth)
                    : GetNextMonthYear(provision.Billingmonthyear);

                // Parse carry-forward date for EstimatedBillingDate on the target provision
                DateTime? cfBillingDate = DateTime.TryParseExact(dto.TargetMonth, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedCfDate)
                    ? parsedCfDate : (DateTime?)null;

                var snap = Snapshot(provision);

                // Mark original as CarriedForward and record this closer-date change
                provision.ProvisionStatus = "CarriedForward";
                provision.CarryForwardCount++;
                provision.Lastupdateby = employeeId;
                provision.Lastupdatedate = DateTime.Now;
                _context.Contractbillingprovesions.Update(provision);
                AddHistory(provision.Contractbillingprovesionid, "CARRIED_FORWARD",
                    loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                    snap, new { Status = "CarriedForward", TargetMonth = nextMonthYear, TargetDate = cfBillingDate?.ToString("dd/MM/yyyy"), AttachmentPath = dto.AttachmentPath }, employeeId);

                // Try to find an existing Active provision in the next month for the same employee
                var existing = await _context.Contractbillingprovesions
                    .FirstOrDefaultAsync(p => p.Contractemployeeid == provision.Contractemployeeid
                                           && p.Billingmonthyear == nextMonthYear
                                           && !p.Isdeleted
                                           && p.ProvisionStatus != "Reversed"
                                           && p.ProvisionStatus != "CarriedForward");

                int targetId;
                string message;

                if (existing != null)
                {
                    // Merge remaining into existing provision — do NOT touch its own CD count
                    var existingSnap = Snapshot(existing);
                    existing.Costing = (existing.Costing ?? 0m) + remaining;
                    existing.CarryForwardFromId = provision.Contractbillingprovesionid;
                    if (cfBillingDate.HasValue) existing.EstimatedBillingDate = cfBillingDate;
                    existing.Lastupdateby = employeeId;
                    existing.Lastupdatedate = DateTime.Now;
                    _context.Contractbillingprovesions.Update(existing);

                    AddHistory(existing.Contractbillingprovesionid, "CARRY_FORWARD_RECEIVED",
                        loginDetails.Name ?? loginDetails.TmcId,
                        $"₹{remaining:N0} added from provision #{provision.Contractbillingprovesionid}. {dto.Remark}".Trim(),
                        existingSnap, new { Costing = existing.Costing, EstimatedBillingDate = existing.EstimatedBillingDate?.ToString("dd/MM/yyyy") }, employeeId);

                    targetId = existing.Contractbillingprovesionid;
                    message = $"₹{remaining:N0} added to existing {nextMonthYear} provision.";
                }
                else
                {
                    // New provision starts with CD count 0 — its own changes are tracked independently
                    var newProvision = new Contractbillingprovesion
                    {
                        Contractemployeeid = provision.Contractemployeeid,
                        Billingmonthyear = nextMonthYear,
                        Costing = remaining,
                        Recievedbillingamount = 0,
                        EstimatedBillingDate = cfBillingDate ?? provision.EstimatedBillingDate,
                        Isactive = true,
                        Isdeleted = false,
                        Createddate = DateTime.Now,
                        Lastupdatedate = DateTime.Now,
                        Createdby = employeeId,
                        Lastupdateby = employeeId,
                        Statusid = provision.Statusid,
                        CarryForwardCount = 0,
                        CarryForwardFromId = provision.Contractbillingprovesionid,
                        ProvisionStatus = "Active"
                    };
                    _context.Contractbillingprovesions.Add(newProvision);
                    await _context.SaveChangesAsync();

                    AddHistory(newProvision.Contractbillingprovesionid, "CREATED",
                        loginDetails.Name ?? loginDetails.TmcId,
                        $"Carried forward from provision #{provision.Contractbillingprovesionid}. {dto.Remark}".Trim(),
                        null, new { Status = "Active", Amount = remaining, MonthYear = nextMonthYear, EstimatedBillingDate = newProvision.EstimatedBillingDate?.ToString("dd/MM/yyyy") }, employeeId);

                    targetId = newProvision.Contractbillingprovesionid;
                    message = $"Provision carried forward to {nextMonthYear} (billing date: {newProvision.EstimatedBillingDate?.ToString("dd/MM/yyyy") ?? "—"}).";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response
                {
                    responseCode = 200,
                    responseMessage = message,
                    data = new { targetProvisionId = targetId, nextMonthYear }
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Response> ReverseProvision(ProvisionActionDto dto, JwtLoginDetailDto loginDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var provision = await _context.Contractbillingprovesions
                    .FirstOrDefaultAsync(p => p.Contractbillingprovesionid == dto.ProvisionId && !p.Isdeleted);

                if (provision == null) return Fail(404, "Provision not found.");
                if (provision.ProvisionStatus == "Reversed") return Fail(400, "This provision is already reversed.");

                var employeeId = await GetEmployeeIdAsync(loginDetails.TmcId);
                var snap = Snapshot(provision);

                provision.ProvisionStatus = "Reversed";
                provision.Isrevised = true;
                provision.Lastupdateby = employeeId;
                provision.Lastupdatedate = DateTime.Now;
                _context.Contractbillingprovesions.Update(provision);

                AddHistory(provision.Contractbillingprovesionid, "REVERSED",
                    loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                    snap, new { Status = "Reversed" }, employeeId);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return provision details so the caller can send the reversal email
                return new Response
                {
                    responseCode = 200,
                    responseMessage = "Provision reversed successfully.",
                    data = new
                    {
                        provisionId = provision.Contractbillingprovesionid,
                        billingMonthYear = provision.Billingmonthyear,
                        provisionAmount = provision.Costing,
                        paidAmount = provision.Recievedbillingamount,
                        carryForwardCount = provision.CarryForwardCount
                    }
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<ProvisionHistoryDto>> GetProvisionHistory(int provisionId)
        {
            return await _context.Contractbillingprovisionhistories
                .Where(h => h.Contractbillingprovesionid == provisionId && !h.Isdeleted)
                .OrderByDescending(h => h.Createddate)
                .Select(h => new ProvisionHistoryDto
                {
                    HistoryId = h.Historyid,
                    ProvisionId = h.Contractbillingprovesionid ?? 0,
                    ActionType = h.ActionType ?? h.Approveraction ?? "",
                    ActionBy = h.ActionBy ?? "",
                    Remark = h.Remark,
                    OldValues = h.OldValues,
                    NewValues = h.NewValues,
                    CreatedDate = h.Createddate
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Response> UpdateEstimatedBillingDate(UpdateEstimatedBillingDateDto dto, JwtLoginDetailDto loginDetails)
        {
            var provision = await _context.Contractbillingprovesions
                .FirstOrDefaultAsync(p => p.Contractbillingprovesionid == dto.ProvisionId && !p.Isdeleted);

            if (provision == null) return Fail(404, "Provision not found.");
            if (provision.ProvisionStatus == "Reversed") return Fail(400, "Cannot edit a reversed provision.");

            var employeeId = await GetEmployeeIdAsync(loginDetails.TmcId);
            var snap = Snapshot(provision);

            provision.EstimatedBillingDate = dto.EstimatedBillingDate;
            provision.Lastupdateby = employeeId;
            provision.Lastupdatedate = DateTime.Now;
            _context.Contractbillingprovesions.Update(provision);

            AddHistory(provision.Contractbillingprovesionid, "UPDATED",
                loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                snap, new { EstimatedBillingDate = dto.EstimatedBillingDate?.ToString("dd/MM/yyyy") }, employeeId);

            await _context.SaveChangesAsync();
            return new Response { responseCode = 200, responseMessage = "Estimated billing date updated." };
        }

        public async Task<Response> UpdatePaidAmount(UpdatePaidAmountDto dto, JwtLoginDetailDto loginDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var provision = await _context.Contractbillingprovesions
                    .FirstOrDefaultAsync(p => p.Contractbillingprovesionid == dto.ProvisionId && !p.Isdeleted);

                if (provision == null) return Fail(404, "Provision not found.");
                if (provision.ProvisionStatus == "Reversed")
                    return Fail(400, "Cannot update payment for a reversed provision.");
                if (provision.ProvisionStatus == "Settled")
                    return Fail(400, "This provision is already fully settled.");

                var provisionAmt = provision.Costing ?? 0m;
                var alreadyPaid = provision.Recievedbillingamount ?? 0m;
                var currentRemaining = provisionAmt - alreadyPaid;

                if (dto.PaidAmount <= 0)
                    return Fail(400, "Payment amount must be greater than zero.");
                if (dto.PaidAmount > currentRemaining)
                    return Fail(400, $"Payment (₹{dto.PaidAmount:N0}) exceeds remaining balance (₹{currentRemaining:N0}).");

                var employeeId = await GetEmployeeIdAsync(loginDetails.TmcId);
                var snapBefore = Snapshot(provision);

                // ADD this payment to existing paid amount
                var newTotalPaid = alreadyPaid + dto.PaidAmount;
                var remaining = provisionAmt - newTotalPaid;

                provision.Recievedbillingamount = newTotalPaid;
                provision.Lastupdateby = employeeId;
                provision.Lastupdatedate = DateTime.Now;

                string msg;

                if (newTotalPaid >= provisionAmt)
                {
                    // Fully paid → Settled
                    provision.ProvisionStatus = "Settled";
                    _context.Contractbillingprovesions.Update(provision);
                    AddHistory(provision.Contractbillingprovesionid, "PAID",
                        loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                        snapBefore, new { PaymentAmount = dto.PaidAmount, TotalPaid = newTotalPaid, RemainingAmount = 0m, Status = "Settled" }, employeeId);
                    msg = "Provision fully paid and marked as Settled.";
                }
                else if (dto.RemainingAction == "carryforward")
                {
                    // Partial payment + carry forward remaining
                    if (provision.CarryForwardFromId.HasValue)
                    {
                        var sourceProvision = await _context.Contractbillingprovesions
                            .FirstOrDefaultAsync(s => s.Contractbillingprovesionid == provision.CarryForwardFromId && !s.Isdeleted);
                        if (sourceProvision != null && sourceProvision.CarryForwardCount >= 3)
                            return Fail(400, "Cannot change the closer date: the parent provision's 3 changes are already exhausted.");
                    }
                    if (provision.CarryForwardCount >= 3)
                        return Fail(400, "Maximum carry-forward limit (3) reached. Cannot carry forward.");

                    var nextMonthYear = !string.IsNullOrWhiteSpace(dto.CarryForwardDate)
                        ? ParseInputMonth(dto.CarryForwardDate)
                        : GetNextMonthYear(provision.Billingmonthyear);

                    // Parse the exact CF date to use as EstimatedBillingDate on the target provision
                    DateTime? cfBillingDate = DateTime.TryParseExact(dto.CarryForwardDate, "yyyy-MM-dd",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedCfDate)
                        ? parsedCfDate : (DateTime?)null;

                    provision.ProvisionStatus = "CarriedForward";
                    provision.CarryForwardCount++;
                    _context.Contractbillingprovesions.Update(provision);

                    AddHistory(provision.Contractbillingprovesionid, "PAID",
                        loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                        snapBefore, new { PaymentAmount = dto.PaidAmount, TotalPaid = newTotalPaid, RemainingAmount = remaining, Status = "CarriedForward" }, employeeId);

                    var snapAfterPay = Snapshot(provision);
                    AddHistory(provision.Contractbillingprovesionid, "CARRIED_FORWARD",
                        loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                        snapAfterPay, new { Status = "CarriedForward", TargetMonth = nextMonthYear, TargetDate = cfBillingDate?.ToString("dd/MM/yyyy") }, employeeId);

                    // Reverse any previous unpaid targets created from this provision (CF date is changing)
                    var prevTargets = await _context.Contractbillingprovesions
                        .Where(t => t.CarryForwardFromId == provision.Contractbillingprovesionid
                                 && !t.Isdeleted
                                 && t.ProvisionStatus != "Reversed"
                                 && (t.Recievedbillingamount == null || t.Recievedbillingamount == 0))
                        .ToListAsync();
                    foreach (var prev in prevTargets)
                    {
                        var prevSnap = Snapshot(prev);
                        prev.ProvisionStatus = "Reversed";
                        prev.Isrevised = true;
                        prev.Lastupdateby = employeeId;
                        prev.Lastupdatedate = DateTime.Now;
                        _context.Contractbillingprovesions.Update(prev);
                        AddHistory(prev.Contractbillingprovesionid, "AUTO_REVERSED",
                            loginDetails.Name ?? loginDetails.TmcId,
                            $"Superseded by updated carry-forward from provision #{provision.Contractbillingprovesionid}.",
                            prevSnap, new { Status = "Reversed" }, employeeId);
                    }
                    await _context.SaveChangesAsync();

                    // Find a non-CF, non-reversed Active provision in the target month (not one we just reversed)
                    var existing = await _context.Contractbillingprovesions
                        .FirstOrDefaultAsync(p => p.Contractemployeeid == provision.Contractemployeeid
                                               && p.Billingmonthyear == nextMonthYear
                                               && !p.Isdeleted
                                               && p.ProvisionStatus != "Reversed"
                                               && p.ProvisionStatus != "CarriedForward"
                                               && p.CarryForwardFromId != provision.Contractbillingprovesionid);

                    if (existing != null)
                    {
                        // Merge amount — do NOT touch target's own CD count
                        var existingSnap = Snapshot(existing);
                        existing.Costing = (existing.Costing ?? 0m) + remaining;
                        existing.CarryForwardFromId = provision.Contractbillingprovesionid;
                        if (cfBillingDate.HasValue) existing.EstimatedBillingDate = cfBillingDate;
                        existing.Lastupdateby = employeeId;
                        existing.Lastupdatedate = DateTime.Now;
                        _context.Contractbillingprovesions.Update(existing);
                        AddHistory(existing.Contractbillingprovesionid, "CARRY_FORWARD_RECEIVED",
                            loginDetails.Name ?? loginDetails.TmcId,
                            $"₹{remaining:N0} added from provision #{provision.Contractbillingprovesionid}. {dto.Remark}".Trim(),
                            existingSnap, new { Costing = existing.Costing, EstimatedBillingDate = existing.EstimatedBillingDate?.ToString("dd/MM/yyyy") }, employeeId);
                        msg = $"Payment recorded. Remaining ₹{remaining:N0} added to existing {nextMonthYear} provision.";
                    }
                    else
                    {
                        // New provision starts with CD count 0 — its own changes tracked independently
                        var newProvision = new Contractbillingprovesion
                        {
                            Contractemployeeid = provision.Contractemployeeid,
                            Billingmonthyear = nextMonthYear,
                            Costing = remaining,
                            Recievedbillingamount = 0,
                            EstimatedBillingDate = cfBillingDate ?? provision.EstimatedBillingDate,
                            Isactive = true,
                            Isdeleted = false,
                            Createddate = DateTime.Now,
                            Lastupdatedate = DateTime.Now,
                            Createdby = employeeId,
                            Lastupdateby = employeeId,
                            Statusid = provision.Statusid,
                            CarryForwardCount = 0,
                            CarryForwardFromId = provision.Contractbillingprovesionid,
                            ProvisionStatus = "Active"
                        };
                        _context.Contractbillingprovesions.Add(newProvision);
                        await _context.SaveChangesAsync();
                        AddHistory(newProvision.Contractbillingprovesionid, "CREATED",
                            loginDetails.Name ?? loginDetails.TmcId,
                            $"Carried forward from provision #{provision.Contractbillingprovesionid}. {dto.Remark}".Trim(),
                            null, new { Status = "Active", Amount = remaining, MonthYear = nextMonthYear, EstimatedBillingDate = newProvision.EstimatedBillingDate?.ToString("dd/MM/yyyy") }, employeeId);
                        msg = $"Payment recorded. Remaining ₹{remaining:N0} carried forward to {nextMonthYear} (billing date: {newProvision.EstimatedBillingDate?.ToString("dd/MM/yyyy") ?? "—"}).";
                    }
                }
                else if (dto.RemainingAction == "nullify")
                {
                    // Partial payment + nullify remaining
                    provision.ProvisionStatus = "Reversed";
                    provision.Isrevised = true;
                    _context.Contractbillingprovesions.Update(provision);

                    AddHistory(provision.Contractbillingprovesionid, "PAID",
                        loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                        snapBefore, new { PaymentAmount = dto.PaidAmount, TotalPaid = newTotalPaid, RemainingAmount = remaining, Status = "Reversed" }, employeeId);

                    var snapAfterPay = Snapshot(provision);
                    AddHistory(provision.Contractbillingprovesionid, "REVERSED",
                        loginDetails.Name ?? loginDetails.TmcId,
                        $"Remaining ₹{remaining:N0} nullified after partial payment. {dto.Remark}".Trim(),
                        snapAfterPay, new { Status = "Reversed" }, employeeId);

                    msg = $"Payment recorded. Remaining ₹{remaining:N0} has been nullified.";
                }
                else
                {
                    // Keep Active — just record payment
                    _context.Contractbillingprovesions.Update(provision);
                    AddHistory(provision.Contractbillingprovesionid, "PAID",
                        loginDetails.Name ?? loginDetails.TmcId, dto.Remark,
                        snapBefore, new { PaymentAmount = dto.PaidAmount, TotalPaid = newTotalPaid, RemainingAmount = remaining, Status = provision.ProvisionStatus }, employeeId);
                    msg = $"Payment of ₹{dto.PaidAmount:N0} recorded. Remaining: ₹{remaining:N0}";
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new Response { responseCode = 200, responseMessage = msg };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Called by the monthly background job.
        // Returns the list of auto-reversed provisions so the caller (EmployeeDataUpdateService) can send emails.
        public async Task<List<ProvisionManagementListDto>> ProcessMonthlyCarryForward()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var lastMonthYear = lastMonth.ToString("MMM-yy", CultureInfo.InvariantCulture);
            var autoReversed = new List<ProvisionManagementListDto>();

            var activeProvisions = await _context.Contractbillingprovesions
                .Where(p => !p.Isdeleted && p.Billingmonthyear == lastMonthYear && p.ProvisionStatus == "Active")
                .ToListAsync();

            var pending = activeProvisions
                .Where(p => (p.Costing ?? 0m) - (p.Recievedbillingamount ?? 0m) > 0)
                .ToList();

            foreach (var provision in pending)
            {
                using var tx = await _context.Database.BeginTransactionAsync();
                try
                {
                    var remaining = (provision.Costing ?? 0m) - (provision.Recievedbillingamount ?? 0m);
                    var snap = Snapshot(provision);

                    if (provision.CarryForwardCount >= 3)
                    {
                        provision.ProvisionStatus = "Reversed";
                        provision.Isrevised = true;
                        provision.Lastupdatedate = DateTime.Now;
                        _context.Contractbillingprovesions.Update(provision);

                        AddHistory(provision.Contractbillingprovesionid, "AUTO_REVERSED",
                            "System", "Auto-reversed: carry forward count exceeded 3 months.",
                            snap, new { Status = "Reversed" }, provision.Lastupdateby);

                        await _context.SaveChangesAsync();
                        await tx.CommitAsync();

                        autoReversed.Add(new ProvisionManagementListDto
                        {
                            ProvisionId = provision.Contractbillingprovesionid,
                            BillingMonthYear = provision.Billingmonthyear ?? "",
                            ProvisionAmount = provision.Costing ?? 0m,
                            PaidAmount = provision.Recievedbillingamount ?? 0m,
                            RemainingAmount = remaining,
                            CarryForwardCount = provision.CarryForwardCount,
                            ProvisionStatus = "Reversed"
                        });
                    }
                    else
                    {
                        var nextMonthYear = GetNextMonthYear(provision.Billingmonthyear);
                        provision.ProvisionStatus = "CarriedForward";
                        provision.Lastupdatedate = DateTime.Now;
                        _context.Contractbillingprovesions.Update(provision);

                        AddHistory(provision.Contractbillingprovesionid, "CARRIED_FORWARD",
                            "System", "Auto carry-forward by monthly cron job.",
                            snap, new { Status = "CarriedForward" }, provision.Lastupdateby);

                        var newProvision = new Contractbillingprovesion
                        {
                            Contractemployeeid = provision.Contractemployeeid,
                            Billingmonthyear = nextMonthYear,
                            Costing = remaining,
                            Recievedbillingamount = 0,
                            EstimatedBillingDate = provision.EstimatedBillingDate,
                            Isactive = true,
                            Isdeleted = false,
                            Createddate = DateTime.Now,
                            Lastupdatedate = DateTime.Now,
                            Createdby = provision.Lastupdateby,
                            Lastupdateby = provision.Lastupdateby,
                            Statusid = provision.Statusid,
                            CarryForwardCount = provision.CarryForwardCount + 1,
                            CarryForwardFromId = provision.Contractbillingprovesionid,
                            ProvisionStatus = "Active"
                        };
                        _context.Contractbillingprovesions.Add(newProvision);
                        await _context.SaveChangesAsync();

                        AddHistory(newProvision.Contractbillingprovesionid, "CREATED",
                            "System", $"Carried forward from provision #{provision.Contractbillingprovesionid} by monthly cron.",
                            null, new { Status = "Active", Amount = remaining, MonthYear = nextMonthYear },
                            provision.Lastupdateby);

                        await _context.SaveChangesAsync();
                        await tx.CommitAsync();
                    }
                }
                catch (Exception)
                {
                    await tx.RollbackAsync();
                    // Continue processing remaining provisions
                }
            }

            return autoReversed;
        }

        // -------------------------------------------------------
        // Private helpers
        // -------------------------------------------------------

        private void AddHistory(int provisionId, string actionType, string actionBy, string remark,
            object oldValues, object newValues, int updatedBy)
        {
            _context.Contractbillingprovisionhistories.Add(new Contractbillingprovisionhistory
            {
                Contractbillingprovesionid = provisionId,
                ActionType = actionType,
                ActionBy = actionBy,
                Remark = remark,
                OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
                Revisionnumber = 0,
                Isdeleted = false,
                Createddate = DateTime.Now,
                Lastupdatedate = DateTime.Now,
                Createdby = updatedBy,
                Lastupdateby = updatedBy
            });
        }

        private async Task<int> GetEmployeeIdAsync(string tmcId)
        {
            return await _context.Rmsemployees
                .Where(e => e.Userid == tmcId)
                .Select(e => e.Employeeid)
                .FirstOrDefaultAsync();
        }

        private static string ParseInputMonth(string inputMonth)
        {
            // Handle yyyy-MM-dd (from <input type="date">)
            if (DateTime.TryParseExact(inputMonth, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var d1))
                return d1.ToString("MMM-yy", CultureInfo.InvariantCulture);
            // Handle yyyy-MM (legacy from <input type="month">)
            if (DateTime.TryParseExact(inputMonth, "yyyy-MM",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var d2))
                return d2.ToString("MMM-yy", CultureInfo.InvariantCulture);
            return inputMonth;
        }

        private static string GetNextMonthYear(string billingMonthYear)
        {
            if (DateTime.TryParseExact(billingMonthYear, "MMM-yy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                return d.AddMonths(1).ToString("MMM-yy", CultureInfo.InvariantCulture);

            return DateTime.Now.AddMonths(1).ToString("MMM-yy", CultureInfo.InvariantCulture);
        }

        private static object Snapshot(Contractbillingprovesion p) => new
        {
            Status = p.ProvisionStatus,
            Amount = p.Costing,
            PaidAmount = p.Recievedbillingamount,
            CarryForwardCount = p.CarryForwardCount
        };

        private static Response Fail(int code, string message) =>
            new Response { responseCode = code, responseMessage = message };
    }
}
