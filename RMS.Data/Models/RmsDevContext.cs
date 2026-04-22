using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RMS.Data.Models;

public partial class RmsDevContext : DbContext
{
    public RmsDevContext(DbContextOptions<RmsDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acprojecthistory> Acprojecthistories { get; set; }

    public virtual DbSet<ActualProvisionBillingView> ActualProvisionBillingViews { get; set; }

    public virtual DbSet<AggregatedDataEmployee> AggregatedDataEmployees { get; set; }

    public virtual DbSet<AggregatedDataHr> AggregatedDataHrs { get; set; }

    public virtual DbSet<AggregatedDataHrTemp> AggregatedDataHrTemps { get; set; }

    public virtual DbSet<BillingConsumedAmountView> BillingConsumedAmountViews { get; set; }

    public virtual DbSet<BillingDataView> BillingDataViews { get; set; }

    public virtual DbSet<BillingDataViewLogic> BillingDataViewLogics { get; set; }

    public virtual DbSet<BillingDataViewNew> BillingDataViewNews { get; set; }

    public virtual DbSet<BillingDataViewOld> BillingDataViewOlds { get; set; }

    public virtual DbSet<BillingDataViewOldOld> BillingDataViewOldOlds { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Categoryofactivity> Categoryofactivities { get; set; }

    public virtual DbSet<Categorystatus> Categorystatuses { get; set; }

    public virtual DbSet<Categorysubstatus> Categorysubstatuses { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Contractbilling> Contractbillings { get; set; }

    public virtual DbSet<Contractbillingactualhistory> Contractbillingactualhistories { get; set; }

    public virtual DbSet<Contractbillingprovesion> Contractbillingprovesions { get; set; }

    public virtual DbSet<ContractbillingprovesionToContractbillingHistory> ContractbillingprovesionToContractbillingHistories { get; set; }

    public virtual DbSet<Contractbillingprovisionhistory> Contractbillingprovisionhistories { get; set; }

    public virtual DbSet<Contractemployee> Contractemployees { get; set; }

    public virtual DbSet<Contractemployeedeploymentdate> Contractemployeedeploymentdates { get; set; }

    public virtual DbSet<Contractline> Contractlines { get; set; }

    public virtual DbSet<Contractpresalesresponse> Contractpresalesresponses { get; set; }

    public virtual DbSet<Contracttype> Contracttypes { get; set; }

    public virtual DbSet<Costsheet> Costsheets { get; set; }

    public virtual DbSet<Costsheetdetail> Costsheetdetails { get; set; }

    public virtual DbSet<CostsheetdetailHistory> CostsheetdetailHistories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CrmAggregatedDataFromAllTable> CrmAggregatedDataFromAllTables { get; set; }

    public virtual DbSet<CrmDeal> CrmDeals { get; set; }

    public virtual DbSet<CrmDealReason> CrmDealReasons { get; set; }

    public virtual DbSet<CrmDealStage> CrmDealStages { get; set; }

    public virtual DbSet<CrmDealType> CrmDealTypes { get; set; }

    public virtual DbSet<CrmLeadSource> CrmLeadSources { get; set; }

    public virtual DbSet<CrmSalesAccount> CrmSalesAccounts { get; set; }

    public virtual DbSet<CrmTerritory> CrmTerritories { get; set; }

    public virtual DbSet<CrmUser> CrmUsers { get; set; }

    public virtual DbSet<Crmproject> Crmprojects { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Customercontactdetail> Customercontactdetails { get; set; }

    public virtual DbSet<Customertype> Customertypes { get; set; }

    public virtual DbSet<DataForEmployeeReport> DataForEmployeeReports { get; set; }

    public virtual DbSet<Deliveryhead> Deliveryheads { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Designation> Designations { get; set; }

    public virtual DbSet<EmployeeOnBenchView> EmployeeOnBenchViews { get; set; }

    public virtual DbSet<Employeeprojecthistory> Employeeprojecthistories { get; set; }

    public virtual DbSet<Employeerole> Employeeroles { get; set; }

    public virtual DbSet<Employeeskill> Employeeskills { get; set; }

    public virtual DbSet<Extendedcontract> Extendedcontracts { get; set; }

    public virtual DbSet<Flag> Flags { get; set; }

    public virtual DbSet<ForeslosureProjectcontractHistory> ForeslosureProjectcontractHistories { get; set; }

    public virtual DbSet<Initialsplitcontractbilling> Initialsplitcontractbillings { get; set; }

    public virtual DbSet<MaxNum> MaxNums { get; set; }

    public virtual DbSet<Milestonedetail> Milestonedetails { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Oaf> Oafs { get; set; }

    public virtual DbSet<OafExtendedHistory> OafExtendedHistories { get; set; }

    public virtual DbSet<Oafchecklist> Oafchecklists { get; set; }

    public virtual DbSet<Oafline> Oaflines { get; set; }

    public virtual DbSet<Oldproject> Oldprojects { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<Paymentmethod> Paymentmethods { get; set; }

    public virtual DbSet<Paymentterm> Paymentterms { get; set; }

    public virtual DbSet<Practice> Practices { get; set; }

    public virtual DbSet<Practicehead> Practiceheads { get; set; }

    public virtual DbSet<Presalesquestionmaster> Presalesquestionmasters { get; set; }

    public virtual DbSet<Probillapprl> Probillapprls { get; set; }

    public virtual DbSet<ProbillapprlOld> ProbillapprlOlds { get; set; }

    public virtual DbSet<Probillapprldetail> Probillapprldetails { get; set; }

    public virtual DbSet<ProbillapprldetailOld> ProbillapprldetailOlds { get; set; }

    public virtual DbSet<Probillapprlstage> Probillapprlstages { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectOld> ProjectOlds { get; set; }

    public virtual DbSet<Projectcontract> Projectcontracts { get; set; }

    public virtual DbSet<Projectemployeeassignment> Projectemployeeassignments { get; set; }

    public virtual DbSet<Projection> Projections { get; set; }

    public virtual DbSet<ProjectionEmployeeDeployement> ProjectionEmployeeDeployements { get; set; }

    public virtual DbSet<Projectioninitialbilling> Projectioninitialbillings { get; set; }

    public virtual DbSet<Projectionrequest> Projectionrequests { get; set; }

    public virtual DbSet<Projectmodel> Projectmodels { get; set; }

    public virtual DbSet<Projecttype> Projecttypes { get; set; }

    public virtual DbSet<Rmsemployee> Rmsemployees { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolepage> Rolepages { get; set; }

    public virtual DbSet<Sbu> Sbus { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Skillcosting> Skillcostings { get; set; }

    public virtual DbSet<Skilltag> Skilltags { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Subpractice> Subpractices { get; set; }

    public virtual DbSet<Temp> Temps { get; set; }

    public virtual DbSet<TempContract> TempContracts { get; set; }

    public virtual DbSet<TempContractLine> TempContractLines { get; set; }

    public virtual DbSet<TempContractbilling1> TempContractbillings1 { get; set; }

    public virtual DbSet<TempDadatum> TempDadata { get; set; }

    public virtual DbSet<TempProject> TempProjects { get; set; }

    public virtual DbSet<Tempcontractbilling> Tempcontractbillings { get; set; }

    public virtual DbSet<Tempcontractbillingprovesion> Tempcontractbillingprovesions { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<Templatedetail> Templatedetails { get; set; }

    public virtual DbSet<Timesheetdetail> Timesheetdetails { get; set; }

    public virtual DbSet<Timesheetdetailold> Timesheetdetailolds { get; set; }

    public virtual DbSet<Timesheetheader> Timesheetheaders { get; set; }

    public virtual DbSet<Timesheetold> Timesheetolds { get; set; }

    public virtual DbSet<Userloginlog> Userloginlogs { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acprojecthistory>(entity =>
        {
            entity.HasKey(e => e.Acprojecthistoryid).HasName("acprojecthistory_pkey");

            entity.ToTable("acprojecthistory");

            entity.Property(e => e.Acprojecthistoryid).HasColumnName("acprojecthistoryid");
            entity.Property(e => e.Acmanagerid).HasColumnName("acmanagerid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Startdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startdate");

            entity.HasOne(d => d.Acmanager).WithMany(p => p.Acprojecthistories)
                .HasForeignKey(d => d.Acmanagerid)
                .HasConstraintName("acprojecthistory_acmanagerid_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Acprojecthistories)
                .HasForeignKey(d => d.Projectid)
                .HasConstraintName("acprojecthistory_projectid_fkey");
        });

        modelBuilder.Entity<ActualProvisionBillingView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("actual_provision_billing_view");

            entity.Property(e => e.Accountmanager).HasColumnName("accountmanager");
            entity.Property(e => e.Actualbilling)
                .HasPrecision(10, 2)
                .HasColumnName("actualbilling");
            entity.Property(e => e.Actualestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actualestimatedbillingdate");
            entity.Property(e => e.Amount)
                .HasPrecision(19, 4)
                .HasColumnName("amount");
            entity.Property(e => e.BillingYear).HasColumnName("billing_year");
            entity.Property(e => e.Billingdate).HasColumnName("billingdate");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasMaxLength(255)
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatus).HasColumnName("contractstatus");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Daname).HasColumnName("daname");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Emppractice)
                .HasMaxLength(255)
                .HasColumnName("emppractice");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Empsubpractice)
                .HasMaxLength(255)
                .HasColumnName("empsubpractice");
            entity.Property(e => e.Invoiceperiod).HasColumnName("invoiceperiod");
            entity.Property(e => e.Ponumber)
                .HasMaxLength(255)
                .HasColumnName("ponumber");
            entity.Property(e => e.Projectname)
                .HasMaxLength(255)
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projectpractice)
                .HasMaxLength(255)
                .HasColumnName("projectpractice");
            entity.Property(e => e.Projectsubpractice)
                .HasMaxLength(255)
                .HasColumnName("projectsubpractice");
            entity.Property(e => e.Projecttype)
                .HasMaxLength(255)
                .HasColumnName("projecttype");
            entity.Property(e => e.Provesionbilling)
                .HasPrecision(10, 2)
                .HasColumnName("provesionbilling");
            entity.Property(e => e.Provisionestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("provisionestimatedbillingdate");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<AggregatedDataEmployee>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("aggregated_data_employee");

            entity.Property(e => e.Billablenonbillable).HasColumnName("billablenonbillable");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Companyname).HasColumnName("companyname");
            entity.Property(e => e.Contracstatus).HasColumnName("contracstatus");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatusid).HasColumnName("contractstatusid");
            entity.Property(e => e.Da).HasColumnName("da");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Function).HasColumnName("function");
            entity.Property(e => e.Globalstatus).HasColumnName("globalstatus");
            entity.Property(e => e.Practice).HasColumnName("practice");
            entity.Property(e => e.Practiceid).HasColumnName("practiceid");
            entity.Property(e => e.Projectname)
                .HasMaxLength(255)
                .HasColumnName("projectname");
            entity.Property(e => e.Projecttypename)
                .HasMaxLength(255)
                .HasColumnName("projecttypename");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Subpractice)
                .HasMaxLength(255)
                .HasColumnName("subpractice");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<AggregatedDataHr>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("aggregated_data_hr");

            entity.Property(e => e.Billablenonbillable).HasColumnName("billablenonbillable");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Companyname).HasColumnName("companyname");
            entity.Property(e => e.Contracstatus).HasColumnName("contracstatus");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Da).HasColumnName("da");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Function).HasColumnName("function");
            entity.Property(e => e.Globalstatus).HasColumnName("globalstatus");
            entity.Property(e => e.Practice).HasColumnName("practice");
            entity.Property(e => e.Practiceid).HasColumnName("practiceid");
            entity.Property(e => e.Projectname)
                .HasColumnType("character varying")
                .HasColumnName("projectname");
            entity.Property(e => e.Projecttypename)
                .HasColumnType("character varying")
                .HasColumnName("projecttypename");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Subpractice)
                .HasMaxLength(255)
                .HasColumnName("subpractice");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<AggregatedDataHrTemp>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("aggregated_data_hr_temp");

            entity.Property(e => e.Billablenonbillable).HasColumnName("billablenonbillable");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Companyname).HasColumnName("companyname");
            entity.Property(e => e.Contracstatus).HasColumnName("contracstatus");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Function).HasColumnName("function");
            entity.Property(e => e.Globalstatus).HasColumnName("globalstatus");
            entity.Property(e => e.Practice)
                .HasMaxLength(255)
                .HasColumnName("practice");
            entity.Property(e => e.Practiceid).HasColumnName("practiceid");
            entity.Property(e => e.Projectname)
                .HasMaxLength(255)
                .HasColumnName("projectname");
            entity.Property(e => e.Projecttypename)
                .HasMaxLength(255)
                .HasColumnName("projecttypename");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Subpractice)
                .HasMaxLength(255)
                .HasColumnName("subpractice");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
            entity.Property(e => e.Udf3).HasColumnName("udf3");
        });

        modelBuilder.Entity<BillingConsumedAmountView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("billing_consumed_amount_view");

            entity.Property(e => e.Accountmanager).HasColumnName("accountmanager");
            entity.Property(e => e.Actualbilling).HasColumnName("actualbilling");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Billingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("billingdate");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.ConsumedAmountTill1stApr).HasColumnName("consumed_amount_till_1st_apr");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasColumnType("character varying")
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatus).HasColumnName("contractstatus");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Daname).HasColumnName("daname");
            entity.Property(e => e.Emppractice)
                .HasMaxLength(255)
                .HasColumnName("emppractice");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Empsubpractice)
                .HasMaxLength(255)
                .HasColumnName("empsubpractice");
            entity.Property(e => e.Ponumber)
                .HasColumnType("character varying")
                .HasColumnName("ponumber");
            entity.Property(e => e.Projectname)
                .HasColumnType("character varying")
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projectpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectpractice");
            entity.Property(e => e.Projectsubpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectsubpractice");
            entity.Property(e => e.Projecttype)
                .HasColumnType("character varying")
                .HasColumnName("projecttype");
            entity.Property(e => e.Provesionbilling).HasColumnName("provesionbilling");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<BillingDataView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("billing_data_view");

            entity.Property(e => e.Accountmanager).HasColumnName("accountmanager");
            entity.Property(e => e.Actualbilling).HasColumnName("actualbilling");
            entity.Property(e => e.Actualestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actualestimatedbillingdate");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BillingYear).HasColumnName("billing_year");
            entity.Property(e => e.Billingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("billingdate");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasColumnType("character varying")
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatus).HasColumnName("contractstatus");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Daname).HasColumnName("daname");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Emppractice)
                .HasMaxLength(255)
                .HasColumnName("emppractice");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Empsubpractice)
                .HasMaxLength(255)
                .HasColumnName("empsubpractice");
            entity.Property(e => e.Invoiceperiod).HasColumnName("invoiceperiod");
            entity.Property(e => e.Ponumber)
                .HasColumnType("character varying")
                .HasColumnName("ponumber");
            entity.Property(e => e.Projectname)
                .HasColumnType("character varying")
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projectpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectpractice");
            entity.Property(e => e.Projectsubpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectsubpractice");
            entity.Property(e => e.Projecttype)
                .HasColumnType("character varying")
                .HasColumnName("projecttype");
            entity.Property(e => e.Provesionbilling).HasColumnName("provesionbilling");
            entity.Property(e => e.Provisionestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("provisionestimatedbillingdate");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<BillingDataViewLogic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("billing_data_view_logic");

            entity.Property(e => e.Accountmanager).HasColumnName("accountmanager");
            entity.Property(e => e.Actualbilling).HasColumnName("actualbilling");
            entity.Property(e => e.Actualestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actualestimatedbillingdate");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BillingYear).HasColumnName("billing_year");
            entity.Property(e => e.Billingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("billingdate");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasColumnType("character varying")
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatus).HasColumnName("contractstatus");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Daname).HasColumnName("daname");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Emppractice)
                .HasMaxLength(255)
                .HasColumnName("emppractice");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Empsubpractice)
                .HasMaxLength(255)
                .HasColumnName("empsubpractice");
            entity.Property(e => e.Invoiceperiod).HasColumnName("invoiceperiod");
            entity.Property(e => e.Ponumber)
                .HasColumnType("character varying")
                .HasColumnName("ponumber");
            entity.Property(e => e.Projectname)
                .HasColumnType("character varying")
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projectpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectpractice");
            entity.Property(e => e.Projectsubpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectsubpractice");
            entity.Property(e => e.Projecttype)
                .HasColumnType("character varying")
                .HasColumnName("projecttype");
            entity.Property(e => e.Provesionbilling).HasColumnName("provesionbilling");
            entity.Property(e => e.Provisionestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("provisionestimatedbillingdate");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<BillingDataViewNew>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("billing_data_view_new");

            entity.Property(e => e.Accountmanager).HasColumnName("accountmanager");
            entity.Property(e => e.Actualbilling).HasColumnName("actualbilling");
            entity.Property(e => e.Actualestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actualestimatedbillingdate");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Billingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("billingdate");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasColumnType("character varying")
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatus).HasColumnName("contractstatus");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Daname).HasColumnName("daname");
            entity.Property(e => e.Emppractice)
                .HasMaxLength(255)
                .HasColumnName("emppractice");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Empsubpractice)
                .HasMaxLength(255)
                .HasColumnName("empsubpractice");
            entity.Property(e => e.Ponumber)
                .HasColumnType("character varying")
                .HasColumnName("ponumber");
            entity.Property(e => e.Projectname)
                .HasColumnType("character varying")
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projectpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectpractice");
            entity.Property(e => e.Projectsubpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectsubpractice");
            entity.Property(e => e.Projecttype)
                .HasColumnType("character varying")
                .HasColumnName("projecttype");
            entity.Property(e => e.Provesionbilling).HasColumnName("provesionbilling");
            entity.Property(e => e.Provisionestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("provisionestimatedbillingdate");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<BillingDataViewOld>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("billing_data_view_old");

            entity.Property(e => e.Accountmanager).HasColumnName("accountmanager");
            entity.Property(e => e.Actualbilling).HasColumnName("actualbilling");
            entity.Property(e => e.Actualestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actualestimatedbillingdate");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BillingYear).HasColumnName("billing_year");
            entity.Property(e => e.Billingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("billingdate");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasColumnType("character varying")
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatus).HasColumnName("contractstatus");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Daname).HasColumnName("daname");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Emppractice)
                .HasMaxLength(255)
                .HasColumnName("emppractice");
            entity.Property(e => e.Empsubpractice)
                .HasMaxLength(255)
                .HasColumnName("empsubpractice");
            entity.Property(e => e.Invoiceperiod).HasColumnName("invoiceperiod");
            entity.Property(e => e.Ponumber)
                .HasColumnType("character varying")
                .HasColumnName("ponumber");
            entity.Property(e => e.Projectname)
                .HasColumnType("character varying")
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projectpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectpractice");
            entity.Property(e => e.Projectsubpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectsubpractice");
            entity.Property(e => e.Projecttype)
                .HasColumnType("character varying")
                .HasColumnName("projecttype");
            entity.Property(e => e.Provesionbilling).HasColumnName("provesionbilling");
            entity.Property(e => e.Provisionestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("provisionestimatedbillingdate");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<BillingDataViewOldOld>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("billing_data_view_old_old");

            entity.Property(e => e.Accountmanager).HasColumnName("accountmanager");
            entity.Property(e => e.Actualbilling).HasColumnName("actualbilling");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BillingYear).HasColumnName("billing_year");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasColumnType("character varying")
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Daname).HasColumnName("daname");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Emppractice)
                .HasMaxLength(255)
                .HasColumnName("emppractice");
            entity.Property(e => e.Empsubpractice)
                .HasMaxLength(255)
                .HasColumnName("empsubpractice");
            entity.Property(e => e.Estimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("estimatedbillingdate");
            entity.Property(e => e.Invoiceperiod).HasColumnName("invoiceperiod");
            entity.Property(e => e.Ponumber)
                .HasColumnType("character varying")
                .HasColumnName("ponumber");
            entity.Property(e => e.Projectname)
                .HasColumnType("character varying")
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projectpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectpractice");
            entity.Property(e => e.Projectsubpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectsubpractice");
            entity.Property(e => e.Projecttype)
                .HasColumnType("character varying")
                .HasColumnName("projecttype");
            entity.Property(e => e.Provesionbilling).HasColumnName("provesionbilling");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Branchid).HasName("branch_pkey");

            entity.ToTable("branch");

            entity.Property(e => e.Branchid).HasColumnName("branchid");
            entity.Property(e => e.Branchcode).HasColumnName("branchcode");
            entity.Property(e => e.Branchname).HasColumnName("branchname");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<Categoryofactivity>(entity =>
        {
            entity.HasKey(e => e.Categoryofactivityid).HasName("categoryofactivity_pkey");

            entity.ToTable("categoryofactivity");

            entity.Property(e => e.Categoryofactivityid)
                .HasDefaultValueSql("nextval('categoryofactivity_id_seq'::regclass)")
                .HasColumnName("categoryofactivityid");
            entity.Property(e => e.Categoryofactivityname)
                .HasMaxLength(100)
                .HasColumnName("categoryofactivityname");
        });

        modelBuilder.Entity<Categorystatus>(entity =>
        {
            entity.HasKey(e => e.Categorystatusid).HasName("categorystatus_pkey");

            entity.ToTable("categorystatus");

            entity.Property(e => e.Categorystatusid).HasColumnName("categorystatusid");
            entity.Property(e => e.Categorystatusdescription).HasColumnName("categorystatusdescription");
            entity.Property(e => e.Categorystatusname).HasColumnName("categorystatusname");
            entity.Property(e => e.Categorysubstatusid)
                .ValueGeneratedOnAdd()
                .HasColumnName("categorysubstatusid");
            entity.Property(e => e.Categorysubstatusname).HasColumnName("categorysubstatusname");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Status).WithMany(p => p.Categorystatuses)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("categorystatus_statusid_fkey");
        });

        modelBuilder.Entity<Categorysubstatus>(entity =>
        {
            entity.HasKey(e => e.Categorysubstatusid).HasName("categorysubstatus_pkey");

            entity.ToTable("categorysubstatus");

            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Categorystatusid).HasColumnName("categorystatusid");
            entity.Property(e => e.Categorysubstatusdescription).HasColumnName("categorysubstatusdescription");
            entity.Property(e => e.Categorysubstatusname).HasColumnName("categorysubstatusname");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Categorystatus).WithMany(p => p.Categorysubstatuses)
                .HasForeignKey(d => d.Categorystatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categorysubstatus_categorystatusid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Categorysubstatuses)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("categorysubstatus_statusid_fkey");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Cityid).HasName("city_pkey");

            entity.ToTable("city");

            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Citycode).HasColumnName("citycode");
            entity.Property(e => e.Cityname).HasColumnName("cityname");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Stateid).HasColumnName("stateid");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.Stateid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("city_stateid_fkey");
        });

        modelBuilder.Entity<Contractbilling>(entity =>
        {
            entity.HasKey(e => e.Contractbillingid).HasName("contractbilling_pkey");

            entity.ToTable("contractbilling");

            entity.Property(e => e.Contractbillingid).HasColumnName("contractbillingid");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Costing)
                .HasPrecision(10, 2)
                .HasColumnName("costing");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Documenturl).HasColumnName("documenturl");
            entity.Property(e => e.Estimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("estimatedbillingdate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isbilled)
                .HasDefaultValueSql("false")
                .HasColumnName("isbilled");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isfromprovision)
                .HasDefaultValueSql("false")
                .HasColumnName("isfromprovision");
            entity.Property(e => e.Isrevised)
                .HasDefaultValueSql("false")
                .HasColumnName("isrevised");
            entity.Property(e => e.Isswaped)
                .HasDefaultValueSql("false")
                .HasColumnName("isswaped");
            entity.Property(e => e.Istobebilled)
                .HasDefaultValueSql("false")
                .HasColumnName("istobebilled");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.Statusid)
                .HasDefaultValueSql("2")
                .HasColumnName("statusid");
            entity.Property(e => e.Swapingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("swapingdate");

            entity.HasOne(d => d.Contractemployee).WithMany(p => p.Contractbillings)
                .HasForeignKey(d => d.Contractemployeeid)
                .HasConstraintName("fk_contractbilling_contractemployeeid");

            entity.HasOne(d => d.Status).WithMany(p => p.Contractbillings)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("contractbilling_statusid_fkey");
        });

        modelBuilder.Entity<Contractbillingactualhistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("contractbillingactualhistory");

            entity.Property(e => e.Approveraction).HasColumnName("approveraction");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractbillingid).HasColumnName("contractbillingid");
            entity.Property(e => e.Costing).HasColumnName("costing");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Estimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("estimatedbillingdate");
            entity.Property(e => e.Historyid)
                .ValueGeneratedOnAdd()
                .HasColumnName("historyid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Lastupdatedby).HasColumnName("lastupdatedby");
            entity.Property(e => e.Oldcosting).HasColumnName("oldcosting");
            entity.Property(e => e.Oldestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("oldestimatedbillingdate");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.Revisionnumber).HasColumnName("revisionnumber");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Contractbilling).WithMany()
                .HasForeignKey(d => d.Contractbillingid)
                .HasConstraintName("contractbillingactualhistory_contractbillingid_fkey");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany()
                .HasForeignKey(d => d.Createdby)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contractbillingactualhistory_createdby_fkey");

            entity.HasOne(d => d.LastupdatedbyNavigation).WithMany()
                .HasForeignKey(d => d.Lastupdatedby)
                .HasConstraintName("contractbillingactualhistory_lastupdatedby_fkey");

            entity.HasOne(d => d.Status).WithMany()
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("contractbillingactualhistory_statusid_fkey");
        });

        modelBuilder.Entity<Contractbillingprovesion>(entity =>
        {
            entity.HasKey(e => e.Contractbillingprovesionid).HasName("contractbillingprovesion_pkey");

            entity.ToTable("contractbillingprovesion");

            entity.Property(e => e.Contractbillingprovesionid).HasColumnName("contractbillingprovesionid");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Costing)
                .HasPrecision(10, 2)
                .HasColumnName("costing");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Documenturl).HasColumnName("documenturl");
            entity.Property(e => e.EstimatedBillingDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("estimatedBillingDate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isbilled).HasColumnName("isbilled");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isfromactual)
                .HasDefaultValueSql("false")
                .HasColumnName("isfromactual");
            entity.Property(e => e.Isrevised).HasColumnName("isrevised");
            entity.Property(e => e.Isswaped)
                .HasDefaultValueSql("false")
                .HasColumnName("isswaped");
            entity.Property(e => e.Istobebilled)
                .HasDefaultValueSql("false")
                .HasColumnName("istobebilled");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Recievedbillingamount).HasColumnName("recievedbillingamount");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Swapingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("swapingdate");

            entity.HasOne(d => d.Contractemployee).WithMany(p => p.Contractbillingprovesions)
                .HasForeignKey(d => d.Contractemployeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contractbillingprovesion_contractemployeeid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Contractbillingprovesions)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("contractbillingprovesion_statusid_fkey");
        });

        modelBuilder.Entity<ContractbillingprovesionToContractbillingHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contractbillingprovesion_to_contractbilling_history_pkey");

            entity.ToTable("contractbillingprovesion_to_contractbilling_history");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualCosting).HasColumnName("actual_costing");
            entity.Property(e => e.ActualEstimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actual_estimatedbillingdate");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractbillingid).HasColumnName("contractbillingid");
            entity.Property(e => e.Contractbillingprovesionid).HasColumnName("contractbillingprovesionid");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.ProvisionCosting).HasColumnName("provision_costing");
            entity.Property(e => e.ProvisionEstimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("provision_estimatedbillingdate");

            entity.HasOne(d => d.Contractbilling).WithMany(p => p.ContractbillingprovesionToContractbillingHistories)
                .HasForeignKey(d => d.Contractbillingid)
                .HasConstraintName("contractbillingprovesion_to_contractbill_contractbillingid_fkey");

            entity.HasOne(d => d.Contractbillingprovesion).WithMany(p => p.ContractbillingprovesionToContractbillingHistories)
                .HasForeignKey(d => d.Contractbillingprovesionid)
                .HasConstraintName("contractbillingprovesion_to_con_contractbillingprovesionid_fkey");

            entity.HasOne(d => d.Contractemployee).WithMany(p => p.ContractbillingprovesionToContractbillingHistories)
                .HasForeignKey(d => d.Contractemployeeid)
                .HasConstraintName("contractbillingprovesion_to_contractbil_contractemployeeid_fkey");
        });

        modelBuilder.Entity<Contractbillingprovisionhistory>(entity =>
        {
            entity.HasKey(e => e.Historyid).HasName("contractbillingprovisionhistory_pkey");

            entity.ToTable("contractbillingprovisionhistory");

            entity.Property(e => e.Historyid).HasColumnName("historyid");
            entity.Property(e => e.Approveraction).HasColumnName("approveraction");
            entity.Property(e => e.Contractbillingprovesionid).HasColumnName("contractbillingprovesionid");
            entity.Property(e => e.Costing).HasColumnName("costing");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Estimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("estimatedbillingdate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isrevised).HasColumnName("isrevised");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Oldcosting).HasColumnName("oldcosting");
            entity.Property(e => e.Probillapprldetailid).HasColumnName("probillapprldetailid");
            entity.Property(e => e.Probillapprlid).HasColumnName("probillapprlid");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.Revisionnumber).HasColumnName("revisionnumber");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Contractbillingprovesion).WithMany(p => p.Contractbillingprovisionhistories)
                .HasForeignKey(d => d.Contractbillingprovesionid)
                .HasConstraintName("contractbillingprovisionhistory_contractbillingprovesionid_fkey");

            entity.HasOne(d => d.Probillapprldetail).WithMany(p => p.Contractbillingprovisionhistories)
                .HasForeignKey(d => d.Probillapprldetailid)
                .HasConstraintName("contractbillingprovisionhistory_probillapprldetailid_fkey");

            entity.HasOne(d => d.Probillapprl).WithMany(p => p.Contractbillingprovisionhistories)
                .HasForeignKey(d => d.Probillapprlid)
                .HasConstraintName("contractbillingprovisionhistory_probillapprlid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Contractbillingprovisionhistories)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("contractbillingprovisionhistory_statusid_fkey");
        });

        modelBuilder.Entity<Contractemployee>(entity =>
        {
            entity.HasKey(e => e.Contractemployeeid).HasName("contractemployee_pkey");

            entity.ToTable("contractemployee");

            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Contractid).HasColumnName("contractid");
            entity.Property(e => e.Costsheetdetailid).HasColumnName("costsheetdetailid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Empxvalue).HasColumnName("empxvalue");
            entity.Property(e => e.Lastupdatedby).HasColumnName("lastupdatedby");
            entity.Property(e => e.Lastupdateon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdateon");

            entity.HasOne(d => d.Categorysubstatus).WithMany(p => p.Contractemployees)
                .HasForeignKey(d => d.Categorysubstatusid)
                .HasConstraintName("fk_contractemployee_categorysubstatusid");

            entity.HasOne(d => d.Contract).WithMany(p => p.Contractemployees)
                .HasForeignKey(d => d.Contractid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contractemployee_contractid");

            entity.HasOne(d => d.Costsheetdetail).WithMany(p => p.Contractemployees)
                .HasForeignKey(d => d.Costsheetdetailid)
                .HasConstraintName("contractemployee_costsheetdetailid_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.Contractemployees)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_contractemployee_employeeid");
        });

        modelBuilder.Entity<Contractemployeedeploymentdate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contractemployeedeploymentdates_pkey");

            entity.ToTable("contractemployeedeploymentdates");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Startdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startdate");

            entity.HasOne(d => d.Categorysubstatus).WithMany(p => p.Contractemployeedeploymentdates)
                .HasForeignKey(d => d.Categorysubstatusid)
                .HasConstraintName("contractemployeedeploymentdates_categorysubstatusid_fkey");

            entity.HasOne(d => d.Contractemployee).WithMany(p => p.Contractemployeedeploymentdates)
                .HasForeignKey(d => d.Contractemployeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contractemployeedeploymentdates_contractemployeeid_fkey");
        });

        modelBuilder.Entity<Contractline>(entity =>
        {
            entity.HasKey(e => e.Contractlineid).HasName("contractline_pkey");

            entity.ToTable("contractline");

            entity.Property(e => e.Contractlineid).HasColumnName("contractlineid");
            entity.Property(e => e.Contractid).HasColumnName("contractid");
            entity.Property(e => e.Lineamount).HasColumnName("lineamount");
            entity.Property(e => e.Linedescription1).HasColumnName("linedescription1");
            entity.Property(e => e.Linedescription2).HasColumnName("linedescription2");
            entity.Property(e => e.Lineno).HasColumnName("lineno");

            entity.HasOne(d => d.Contract).WithMany(p => p.Contractlines)
                .HasForeignKey(d => d.Contractid)
                .HasConstraintName("fk_contractline_contractid");
        });

        modelBuilder.Entity<Contractpresalesresponse>(entity =>
        {
            entity.HasKey(e => e.Responseid).HasName("contractpresalesresponse_pkey");

            entity.ToTable("contractpresalesresponse");

            entity.Property(e => e.Responseid).HasColumnName("responseid");
            entity.Property(e => e.Clientresponse).HasColumnName("clientresponse");
            entity.Property(e => e.Contractid).HasColumnName("contractid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isextra).HasColumnName("isextra");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.Refresponse).HasColumnName("refresponse");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Contract).WithMany(p => p.Contractpresalesresponses)
                .HasForeignKey(d => d.Contractid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contractpresalesresponse_contractid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Contractpresalesresponses)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("contractpresalesresponse_statusid_fkey");
        });

        modelBuilder.Entity<Contracttype>(entity =>
        {
            entity.HasKey(e => e.Contracttypeid).HasName("contracttype_pkey");

            entity.ToTable("contracttype");

            entity.HasIndex(e => e.Contracttypename, "contracttype_contracttypename_key").IsUnique();

            entity.Property(e => e.Contracttypeid).HasColumnName("contracttypeid");
            entity.Property(e => e.Contracttypename).HasColumnName("contracttypename");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<Costsheet>(entity =>
        {
            entity.HasKey(e => e.Costsheetid).HasName("costsheet_pkey");

            entity.ToTable("costsheet");

            entity.Property(e => e.Costsheetid).HasColumnName("costsheetid");
            entity.Property(e => e.Costsheetname).HasColumnName("costsheetname");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<Costsheetdetail>(entity =>
        {
            entity.HasKey(e => e.Costsheetdetailid).HasName("costsheetdetail_pkey");

            entity.ToTable("costsheetdetail");

            entity.Property(e => e.Costsheetdetailid).HasColumnName("costsheetdetailid");
            entity.Property(e => e.Costsheetid).HasColumnName("costsheetid");
            entity.Property(e => e.Customerprice).HasColumnName("customerprice");
            entity.Property(e => e.Perioddays).HasColumnName("perioddays");
            entity.Property(e => e.Requiredresource).HasColumnName("requiredresource");
            entity.Property(e => e.Skillcost).HasColumnName("skillcost");
            entity.Property(e => e.Skillexperince).HasColumnName("skillexperince");
            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Totalcost).HasColumnName("totalcost");
            entity.Property(e => e.Totalprice).HasColumnName("totalprice");
            entity.Property(e => e.Xvalue).HasColumnName("xvalue");

            entity.HasOne(d => d.Costsheet).WithMany(p => p.Costsheetdetails)
                .HasForeignKey(d => d.Costsheetid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_costsheetdetail_costsheetid");

            entity.HasOne(d => d.Skill).WithMany(p => p.Costsheetdetails)
                .HasForeignKey(d => d.Skillid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_costsheetdetail_skillid");
        });

        modelBuilder.Entity<CostsheetdetailHistory>(entity =>
        {
            entity.HasKey(e => e.Costsheetdetailid).HasName("costsheetdetail_history_pkey");

            entity.ToTable("costsheetdetail_history");

            entity.Property(e => e.Costsheetdetailid).HasColumnName("costsheetdetailid");
            entity.Property(e => e.Costsheetid).HasColumnName("costsheetid");
            entity.Property(e => e.Customerprice).HasColumnName("customerprice");
            entity.Property(e => e.Perioddays).HasColumnName("perioddays");
            entity.Property(e => e.Requiredresource).HasColumnName("requiredresource");
            entity.Property(e => e.Skillcost).HasColumnName("skillcost");
            entity.Property(e => e.Skillexperince).HasColumnName("skillexperince");
            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Totalcost).HasColumnName("totalcost");
            entity.Property(e => e.Totalprice).HasColumnName("totalprice");
            entity.Property(e => e.Version).HasColumnName("version");
            entity.Property(e => e.Xvalue).HasColumnName("xvalue");

            entity.HasOne(d => d.Costsheet).WithMany(p => p.CostsheetdetailHistories)
                .HasForeignKey(d => d.Costsheetid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("costsheetdetail_history_costsheetid_fkey");

            entity.HasOne(d => d.Skill).WithMany(p => p.CostsheetdetailHistories)
                .HasForeignKey(d => d.Skillid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("costsheetdetail_history_skillid_fkey");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Countryid).HasName("country_pkey");

            entity.ToTable("country");

            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Countrycode)
                .HasMaxLength(255)
                .HasColumnName("countrycode");
            entity.Property(e => e.Countryname).HasColumnName("countryname");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<CrmAggregatedDataFromAllTable>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("crm_aggregated_data_from_all_tables");

            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.BaseCurrencyAmount)
                .HasPrecision(18, 2)
                .HasColumnName("base_currency_amount");
            entity.Property(e => e.ClosedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("closed_date");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Createdby)
                .HasMaxLength(255)
                .HasColumnName("createdby");
            entity.Property(e => e.Creatoremail)
                .HasMaxLength(255)
                .HasColumnName("creatoremail");
            entity.Property(e => e.Creatormobile)
                .HasMaxLength(255)
                .HasColumnName("creatormobile");
            entity.Property(e => e.CrmDealsid).HasColumnName("crm_dealsid");
            entity.Property(e => e.Crmid).HasColumnName("crmid");
            entity.Property(e => e.Dealname)
                .HasColumnType("character varying")
                .HasColumnName("dealname");
            entity.Property(e => e.ExpectedClose)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expected_close");
            entity.Property(e => e.Projectionid).HasColumnName("projectionid");
            entity.Property(e => e.Salesaccountname)
                .HasColumnType("character varying")
                .HasColumnName("salesaccountname");
            entity.Property(e => e.Satgeforcasttype)
                .HasMaxLength(255)
                .HasColumnName("satgeforcasttype");
            entity.Property(e => e.StageUpdatedTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("stage_updated_time");
            entity.Property(e => e.Stagename)
                .HasMaxLength(255)
                .HasColumnName("stagename");
            entity.Property(e => e.Stageposition).HasColumnName("stageposition");
            entity.Property(e => e.Territoryname)
                .HasMaxLength(255)
                .HasColumnName("territoryname");
            entity.Property(e => e.Website)
                .HasColumnType("character varying")
                .HasColumnName("website");
        });

        modelBuilder.Entity<CrmDeal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crm_deals_pkey");

            entity.ToTable("crm_deals");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.BaseCurrencyAmount)
                .HasPrecision(18, 2)
                .HasColumnName("base_currency_amount");
            entity.Property(e => e.ClosedDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("closed_date");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreaterId).HasColumnName("creater_id");
            entity.Property(e => e.CrmDealStagesid).HasColumnName("crm_deal_stagesid");
            entity.Property(e => e.CrmDealsid).HasColumnName("crm_dealsid");
            entity.Property(e => e.CrmLeadSourcesid).HasColumnName("crm_lead_sourcesid");
            entity.Property(e => e.CrmSalesAccountsid).HasColumnName("crm_sales_accountsid");
            entity.Property(e => e.CrmTerritoriesid).HasColumnName("crm_territoriesid");
            entity.Property(e => e.CurrencyId).HasColumnName("currency_id");
            entity.Property(e => e.ExpectedClose)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expected_close");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.StageUpdatedTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("stage_updated_time");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdaterId).HasColumnName("updater_id");
        });

        modelBuilder.Entity<CrmDealReason>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crm_deal_reasons_pkey");

            entity.ToTable("crm_deal_reasons");

            entity.HasIndex(e => e.CrmDealReasonsid, "crm_deal_reasons_crm_deal_reasonsid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CrmDealReasonsid).HasColumnName("crm_deal_reasonsid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");
        });

        modelBuilder.Entity<CrmDealStage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crm_deal_stages_pkey");

            entity.ToTable("crm_deal_stages");

            entity.HasIndex(e => e.CrmDealStagesid, "crm_deal_stages_crm_deal_stagesid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ChoiceType).HasColumnName("choice_type");
            entity.Property(e => e.CrmDealStagesid).HasColumnName("crm_deal_stagesid");
            entity.Property(e => e.DealPipelineId).HasColumnName("deal_pipeline_id");
            entity.Property(e => e.ForecastType)
                .HasMaxLength(255)
                .HasColumnName("forecast_type");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.Probability).HasColumnName("probability");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<CrmDealType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crm_deal_types_pkey");

            entity.ToTable("crm_deal_types");

            entity.HasIndex(e => e.CrmDealTypesid, "crm_deal_types_crm_deal_typesid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CrmDealTypesid).HasColumnName("crm_deal_typesid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");
        });

        modelBuilder.Entity<CrmLeadSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crm_lead_sources_pkey");

            entity.ToTable("crm_lead_sources");

            entity.HasIndex(e => e.CrmLeadSourcesid, "crm_lead_sources_crm_lead_sourcesid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CrmLeadSourcesid).HasColumnName("crm_lead_sourcesid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");
        });

        modelBuilder.Entity<CrmSalesAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crm_sales_accounts_pkey");

            entity.ToTable("crm_sales_accounts");

            entity.HasIndex(e => e.CrmSalesAccountsid, "crm_sales_accounts_crm_sales_accountsid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Avatar)
                .HasColumnType("character varying")
                .HasColumnName("avatar");
            entity.Property(e => e.CrmSalesAccountsid).HasColumnName("crm_sales_accountsid");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.OpenDealsAmount)
                .HasPrecision(18, 2)
                .HasColumnName("open_deals_amount");
            entity.Property(e => e.OpenDealsCount).HasColumnName("open_deals_count");
            entity.Property(e => e.Website)
                .HasColumnType("character varying")
                .HasColumnName("website");
            entity.Property(e => e.WonDealsAmount)
                .HasPrecision(18, 2)
                .HasColumnName("won_deals_amount");
            entity.Property(e => e.WonDealsCount).HasColumnName("won_deals_count");
        });

        modelBuilder.Entity<CrmTerritory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crm_territories_pkey");

            entity.ToTable("crm_territories");

            entity.HasIndex(e => e.CrmTerritoriesid, "crm_territories_crm_territoriesid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CrmTerritoriesid).HasColumnName("crm_territoriesid");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Position).HasColumnName("position");
        });

        modelBuilder.Entity<CrmUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("crm_users_pkey");

            entity.ToTable("crm_users");

            entity.HasIndex(e => e.CrmUsersid, "crm_users_crm_usersid_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CrmUsersid).HasColumnName("crm_usersid");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(255)
                .HasColumnName("display_name");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.MobileNumber)
                .HasMaxLength(255)
                .HasColumnName("mobile_number");
            entity.Property(e => e.WorkNumber)
                .HasMaxLength(255)
                .HasColumnName("work_number");
        });

        modelBuilder.Entity<Crmproject>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("crmprojects");

            entity.Property(e => e.ActualCustomerPoValue).HasColumnName("actual_customer_po_value");
            entity.Property(e => e.AsignToTmc).HasColumnName("asign_to_tmc");
            entity.Property(e => e.BilledAmount).HasColumnName("billed_amount");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CustomerCategory).HasColumnName("customer_category");
            entity.Property(e => e.CustomerEmailId1).HasColumnName("customer_email_id_1");
            entity.Property(e => e.CustomerPoNo).HasColumnName("customer_po_no");
            entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");
            entity.Property(e => e.PaymentTermsCode).HasColumnName("payment_terms_code");
            entity.Property(e => e.ProjectDate).HasColumnName("project_date");
            entity.Property(e => e.ProjectNo).HasColumnName("project_no");
            entity.Property(e => e.ProjectSubmitDatetime).HasColumnName("project_submit_datetime");
            entity.Property(e => e.RequestedDeliveryDate).HasColumnName("requested_delivery_date");
            entity.Property(e => e.SalespersonCode).HasColumnName("salesperson_code");
            entity.Property(e => e.SellToCustomerNo).HasColumnName("sell_to_customer_no");
            entity.Property(e => e.ShortcutDimension12Code).HasColumnName("shortcut_dimension_12_code");
            entity.Property(e => e.ShortcutDimension14Code).HasColumnName("shortcut_dimension_14_code");
            entity.Property(e => e.ShortcutDimension1Code).HasColumnName("shortcut_dimension_1_code");
            entity.Property(e => e.ShortcutDimension2Code).HasColumnName("shortcut_dimension_2_code");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Currencyid).HasName("currency_pkey");

            entity.ToTable("currency");

            entity.HasIndex(e => e.Abbreviation, "currency_abbreviation_key").IsUnique();

            entity.HasIndex(e => e.Currencyname, "currency_currencyname_key").IsUnique();

            entity.Property(e => e.Currencyid).HasColumnName("currencyid");
            entity.Property(e => e.Abbreviation).HasColumnName("abbreviation");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Currencyname).HasColumnName("currencyname");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("customer_pkey");

            entity.ToTable("customer");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Address1).HasColumnName("address1");
            entity.Property(e => e.Address2).HasColumnName("address2");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Chainname).HasColumnName("chainname");
            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Companyemail).HasColumnName("companyemail");
            entity.Property(e => e.Companylogourl).HasColumnName("companylogourl");
            entity.Property(e => e.Companyname).HasColumnName("companyname");
            entity.Property(e => e.Contact).HasColumnName("contact");
            entity.Property(e => e.Countryregioncode).HasColumnName("countryregioncode");
            entity.Property(e => e.Createdby)
                .HasDefaultValueSql("0")
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Currencyid).HasColumnName("currencyid");
            entity.Property(e => e.Customercode).HasColumnName("customercode");
            entity.Property(e => e.Customertypeid).HasColumnName("customertypeid");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Globaldimension10code).HasColumnName("globaldimension10code");
            entity.Property(e => e.Globaldimension11code).HasColumnName("globaldimension11code");
            entity.Property(e => e.Globaldimension12code).HasColumnName("globaldimension12code");
            entity.Property(e => e.Globaldimension13code).HasColumnName("globaldimension13code");
            entity.Property(e => e.Globaldimension14code).HasColumnName("globaldimension14code");
            entity.Property(e => e.Globaldimension9code).HasColumnName("globaldimension9code");
            entity.Property(e => e.Gstnumber).HasColumnName("gstnumber");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Lastupdateby)
                .HasDefaultValueSql("0")
                .HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Oldcustomercode).HasColumnName("oldcustomercode");
            entity.Property(e => e.Pannumber).HasColumnName("pannumber");
            entity.Property(e => e.Paymentmethodid).HasColumnName("paymentmethodid");
            entity.Property(e => e.Paymenttermid).HasColumnName("paymenttermid");
            entity.Property(e => e.Phone1).HasColumnName("phone1");
            entity.Property(e => e.Phone2).HasColumnName("phone2");
            entity.Property(e => e.Statecode).HasColumnName("statecode");
            entity.Property(e => e.Zipcode).HasColumnName("zipcode");

            entity.HasOne(d => d.City).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Cityid)
                .HasConstraintName("customer_cityid_fkey");

            entity.HasOne(d => d.Currency).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Currencyid)
                .HasConstraintName("customer_currencyid_fkey");

            entity.HasOne(d => d.Customertype).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Customertypeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_customertypeid_fkey");

            entity.HasOne(d => d.Paymentmethod).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Paymentmethodid)
                .HasConstraintName("customer_paymentmethodid_fkey");

            entity.HasOne(d => d.Paymentterm).WithMany(p => p.Customers)
                .HasForeignKey(d => d.Paymenttermid)
                .HasConstraintName("customer_paymenttermid_fkey");
        });

        modelBuilder.Entity<Customercontactdetail>(entity =>
        {
            entity.HasKey(e => e.Customercontactdetailid).HasName("customercontactdetail_pkey");

            entity.ToTable("customercontactdetail");

            entity.Property(e => e.Customercontactdetailid).HasColumnName("customercontactdetailid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Emailid).HasColumnName("emailid");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Mobile).HasColumnName("mobile");

            entity.HasOne(d => d.Customer).WithMany(p => p.Customercontactdetails)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customercontactdetail_customerid_fkey");
        });

        modelBuilder.Entity<Customertype>(entity =>
        {
            entity.HasKey(e => e.Customertypeid).HasName("customertype_pkey");

            entity.ToTable("customertype");

            entity.HasIndex(e => e.Customertypename, "customertype_customertypename_key").IsUnique();

            entity.Property(e => e.Customertypeid).HasColumnName("customertypeid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Customertypename).HasColumnName("customertypename");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<DataForEmployeeReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("data_for_employee_report");

            entity.Property(e => e.Ade).HasColumnName("ade");
            entity.Property(e => e.Billablenonbillable).HasColumnName("billablenonbillable");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatusid).HasColumnName("contractstatusid");
            entity.Property(e => e.Currentmonthbilling).HasColumnName("currentmonthbilling");
            entity.Property(e => e.Customernames).HasColumnName("customernames");
            entity.Property(e => e.Da).HasColumnName("da");
            entity.Property(e => e.Dateofjoining).HasColumnName("dateofjoining");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Exe).HasColumnName("exe");
            entity.Property(e => e.Flag).HasColumnName("flag");
            entity.Property(e => e.Function).HasColumnName("function");
            entity.Property(e => e.Practice).HasColumnName("practice");
            entity.Property(e => e.Projectnames).HasColumnName("projectnames");
            entity.Property(e => e.Projecttypes).HasColumnName("projecttypes");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Reportinghead).HasColumnName("reportinghead");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Subpractice)
                .HasColumnType("character varying")
                .HasColumnName("subpractice");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Workexedays).HasColumnName("workexedays");
        });

        modelBuilder.Entity<Deliveryhead>(entity =>
        {
            entity.HasKey(e => e.Deliveryheadid).HasName("deliveryhead_pkey");

            entity.ToTable("deliveryhead");

            entity.Property(e => e.Deliveryheadid)
                .HasDefaultValueSql("nextval('practicehead_practiceheadid_seq'::regclass)")
                .HasColumnName("deliveryheadid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Practiceid).HasColumnName("practiceid");

            entity.HasOne(d => d.Employee).WithMany(p => p.Deliveryheads)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("deliveryhead_employeeid_fkey");

            entity.HasOne(d => d.Practice).WithMany(p => p.Deliveryheads)
                .HasForeignKey(d => d.Practiceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("deliveryhead_practiceid_fkey");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Departmentid).HasName("department_pkey");

            entity.ToTable("department");

            entity.HasIndex(e => e.Departmentname, "department_departmentname_key").IsUnique();

            entity.Property(e => e.Departmentid).HasColumnName("departmentid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Departmentname).HasColumnName("departmentname");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<Designation>(entity =>
        {
            entity.HasKey(e => e.Designationid).HasName("designation_pkey");

            entity.ToTable("designation");

            entity.HasIndex(e => e.Designationname, "designation_designationname_key").IsUnique();

            entity.Property(e => e.Designationid).HasColumnName("designationid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Designationname).HasColumnName("designationname");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<EmployeeOnBenchView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("employee_on_bench_view");

            entity.Property(e => e.Accountmanager).HasColumnName("accountmanager");
            entity.Property(e => e.Actualbilling).HasColumnName("actualbilling");
            entity.Property(e => e.Actualestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actualestimatedbillingdate");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BillingYear).HasColumnName("billing_year");
            entity.Property(e => e.Billingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("billingdate");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasColumnType("character varying")
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Contractstatus).HasColumnName("contractstatus");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Daname).HasColumnName("daname");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Emppractice)
                .HasMaxLength(255)
                .HasColumnName("emppractice");
            entity.Property(e => e.Empstatus).HasColumnName("empstatus");
            entity.Property(e => e.Empsubpractice)
                .HasMaxLength(255)
                .HasColumnName("empsubpractice");
            entity.Property(e => e.Invoiceperiod).HasColumnName("invoiceperiod");
            entity.Property(e => e.Ponumber)
                .HasColumnType("character varying")
                .HasColumnName("ponumber");
            entity.Property(e => e.Projectname)
                .HasColumnType("character varying")
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projectpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectpractice");
            entity.Property(e => e.Projectsubpractice)
                .HasColumnType("character varying")
                .HasColumnName("projectsubpractice");
            entity.Property(e => e.Projecttype)
                .HasColumnType("character varying")
                .HasColumnName("projecttype");
            entity.Property(e => e.Provesionbilling).HasColumnName("provesionbilling");
            entity.Property(e => e.Provisionestimatedbillingdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("provisionestimatedbillingdate");
            entity.Property(e => e.Resourcename).HasColumnName("resourcename");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Employeeprojecthistory>(entity =>
        {
            entity.HasKey(e => e.Historyid).HasName("employeeprojecthistory_pkey");

            entity.ToTable("employeeprojecthistory");

            entity.Property(e => e.Historyid).HasColumnName("historyid");
            entity.Property(e => e.Actiontakenby).HasColumnName("actiontakenby");
            entity.Property(e => e.Actiontakenon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actiontakenon");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Effectiveenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("effectiveenddate");
            entity.Property(e => e.Effectivestartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("effectivestartdate");

            entity.HasOne(d => d.Categorysubstatus).WithMany(p => p.Employeeprojecthistories)
                .HasForeignKey(d => d.Categorysubstatusid)
                .HasConstraintName("employeeprojecthistory_categorysubstatusid_fkey");

            entity.HasOne(d => d.Contractemployee).WithMany(p => p.Employeeprojecthistories)
                .HasForeignKey(d => d.Contractemployeeid)
                .HasConstraintName("employeeprojecthistory_contractemployeeid_fkey");
        });

        modelBuilder.Entity<Employeerole>(entity =>
        {
            entity.HasKey(e => e.Employeeroleid).HasName("employeerole_pkey");

            entity.ToTable("employeerole");

            entity.Property(e => e.Employeeroleid).HasColumnName("employeeroleid");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeeroles)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_employeerole_rmsemployee");

            entity.HasOne(d => d.Role).WithMany(p => p.Employeeroles)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("fk_employeerole_roleid");
        });

        modelBuilder.Entity<Employeeskill>(entity =>
        {
            entity.HasKey(e => e.Employeeskillid).HasName("employeeskill_pkey");

            entity.ToTable("employeeskill");

            entity.Property(e => e.Employeeskillid).HasColumnName("employeeskillid");
            entity.Property(e => e.Certificationurl).HasColumnName("certificationurl");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Experinceinmonths).HasColumnName("experinceinmonths");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isprimary).HasColumnName("isprimary");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Managerrating).HasColumnName("managerrating");
            entity.Property(e => e.Selfreting).HasColumnName("selfreting");
            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Employee).WithMany(p => p.Employeeskills)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employeeskill_employeeid_fkey");

            entity.HasOne(d => d.Skill).WithMany(p => p.Employeeskills)
                .HasForeignKey(d => d.Skillid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employeeskill_skillid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Employeeskills)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("employeeskill_statusid_fkey");
        });

        modelBuilder.Entity<Extendedcontract>(entity =>
        {
            entity.HasKey(e => e.Extensionid).HasName("extendedcontracts_pkey");

            entity.ToTable("extendedcontracts");

            entity.Property(e => e.Extensionid).HasColumnName("extensionid");
            entity.Property(e => e.Createdby)
                .HasMaxLength(100)
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Extendedcontractid).HasColumnName("extendedcontractid");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Lastupdatedby)
                .HasMaxLength(100)
                .HasColumnName("lastupdatedby");
            entity.Property(e => e.Oldcontractid).HasColumnName("oldcontractid");

            entity.HasOne(d => d.Oldcontract).WithMany(p => p.Extendedcontracts)
                .HasForeignKey(d => d.Oldcontractid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_oldcontract");
        });

        modelBuilder.Entity<Flag>(entity =>
        {
            entity.HasKey(e => e.Flagid).HasName("flag_pkey");

            entity.ToTable("flag");

            entity.HasIndex(e => e.Flagname, "flag_flagname_key").IsUnique();

            entity.Property(e => e.Flagid).HasColumnName("flagid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Flagname).HasColumnName("flagname");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<ForeslosureProjectcontractHistory>(entity =>
        {
            entity.HasKey(e => e.Historyid).HasName("foreslosure_projectcontract_history_pkey");

            entity.ToTable("foreslosure_projectcontract_history");

            entity.Property(e => e.Historyid).HasColumnName("historyid");
            entity.Property(e => e.Changedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("changedate");
            entity.Property(e => e.Changedby).HasColumnName("changedby");
            entity.Property(e => e.Contractid).HasColumnName("contractid");
            entity.Property(e => e.Newcontractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("newcontractenddate");
            entity.Property(e => e.Oldcontractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("oldcontractenddate");

            entity.HasOne(d => d.Contract).WithMany(p => p.ForeslosureProjectcontractHistories)
                .HasForeignKey(d => d.Contractid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("foreslosure_projectcontract_history_contractid_fkey");
        });

        modelBuilder.Entity<Initialsplitcontractbilling>(entity =>
        {
            entity.HasKey(e => e.Contractbillingid).HasName("initialsplitcontractbilling_pkey");

            entity.ToTable("initialsplitcontractbilling");

            entity.Property(e => e.Contractbillingid).HasColumnName("contractbillingid");
            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Costing)
                .HasPrecision(10, 2)
                .HasColumnName("costing");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");

            entity.HasOne(d => d.Contractemployee).WithMany(p => p.Initialsplitcontractbillings)
                .HasForeignKey(d => d.Contractemployeeid)
                .HasConstraintName("initialsplitcontractbilling_contractemployeeid_fkey");
        });

        modelBuilder.Entity<MaxNum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("max_num");

            entity.Property(e => e.Coalesce).HasColumnName("coalesce");
        });

        modelBuilder.Entity<Milestonedetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("milestonedetails_pkey");

            entity.ToTable("milestonedetails");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Days).HasColumnName("days");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Milestonedesc).HasColumnName("milestonedesc");
            entity.Property(e => e.Oafid).HasColumnName("oafid");
            entity.Property(e => e.Percentage).HasColumnName("percentage");

            entity.HasOne(d => d.Oaf).WithMany(p => p.Milestonedetails)
                .HasForeignKey(d => d.Oafid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("milestonedetails_oafid_fkey");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Moduleid).HasName("module_pkey");

            entity.ToTable("module");

            entity.Property(e => e.Moduleid).HasColumnName("moduleid");
            entity.Property(e => e.Createdby)
                .HasDefaultValueSql("0")
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdatedby)
                .HasDefaultValueSql("0")
                .HasColumnName("lastupdatedby");
            entity.Property(e => e.Lastupdateddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdateddate");
            entity.Property(e => e.Modulename).HasColumnName("modulename");
        });

        modelBuilder.Entity<Oaf>(entity =>
        {
            entity.HasKey(e => e.Oafid).HasName("oaf_pkey");

            entity.ToTable("oaf");

            entity.Property(e => e.Oafid).HasColumnName("oafid");
            entity.Property(e => e.Accountmanagerid).HasColumnName("accountmanagerid");
            entity.Property(e => e.Advanceamount).HasColumnName("advanceamount");
            entity.Property(e => e.Advancepercent).HasColumnName("advancepercent");
            entity.Property(e => e.Clientcoordinator).HasColumnName("clientcoordinator");
            entity.Property(e => e.CommittedClientBillingDate).HasColumnName("committed_client_billing_date");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno).HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Costattachment).HasColumnName("costattachment");
            entity.Property(e => e.Costsheetid).HasColumnName("costsheetid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Deliveryanchorid).HasColumnName("deliveryanchorid");
            entity.Property(e => e.Emailattachment).HasColumnName("emailattachment");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isextended).HasColumnName("isextended");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Milestones).HasColumnName("milestones");
            entity.Property(e => e.Oafno).HasColumnName("oafno");
            entity.Property(e => e.Orderdescription).HasColumnName("orderdescription");
            entity.Property(e => e.Poattachment).HasColumnName("poattachment");
            entity.Property(e => e.Ponumber).HasColumnName("ponumber");
            entity.Property(e => e.Potermscondition).HasColumnName("potermscondition");
            entity.Property(e => e.Povalue).HasColumnName("povalue");
            entity.Property(e => e.Projectdescription).HasColumnName("projectdescription");
            entity.Property(e => e.Projectmodel).HasColumnName("projectmodel");
            entity.Property(e => e.Projectname).HasColumnName("projectname");
            entity.Property(e => e.Projecttypeid).HasColumnName("projecttypeid");
            entity.Property(e => e.Proposalattachment).HasColumnName("proposalattachment");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Subpracticeid).HasColumnName("subpracticeid");
            entity.Property(e => e.Udf1).HasColumnName("udf1");
            entity.Property(e => e.Udf2).HasColumnName("udf2");
            entity.Property(e => e.Udf3).HasColumnName("udf3");
            entity.Property(e => e.Xvalue).HasColumnName("xvalue");

            entity.HasOne(d => d.Accountmanager).WithMany(p => p.OafAccountmanagers)
                .HasForeignKey(d => d.Accountmanagerid)
                .HasConstraintName("oaf_accountmanagerid_fkey");

            entity.HasOne(d => d.Costsheet).WithMany(p => p.Oafs)
                .HasForeignKey(d => d.Costsheetid)
                .HasConstraintName("fk_status_costsheetid");

            entity.HasOne(d => d.Customer).WithMany(p => p.Oafs)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_oaf_customerid");

            entity.HasOne(d => d.Deliveryanchor).WithMany(p => p.OafDeliveryanchors)
                .HasForeignKey(d => d.Deliveryanchorid)
                .HasConstraintName("fk_status_deliveryanchorid");

            entity.HasOne(d => d.Projecttype).WithMany(p => p.Oafs)
                .HasForeignKey(d => d.Projecttypeid)
                .HasConstraintName("fk_status_projecttypeid");

            entity.HasOne(d => d.Status).WithMany(p => p.Oafs)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("fk_status_statusid");

            entity.HasOne(d => d.Subpractice).WithMany(p => p.Oafs)
                .HasForeignKey(d => d.Subpracticeid)
                .HasConstraintName("fk_status_subpracticeid");
        });

        modelBuilder.Entity<OafExtendedHistory>(entity =>
        {
            entity.HasKey(e => e.Historyid).HasName("oaf_extended_history_pkey");

            entity.ToTable("oaf_extended_history");

            entity.Property(e => e.Historyid).HasColumnName("historyid");
            entity.Property(e => e.Accountmanagerid).HasColumnName("accountmanagerid");
            entity.Property(e => e.Advanceamount).HasColumnName("advanceamount");
            entity.Property(e => e.Advancepercent).HasColumnName("advancepercent");
            entity.Property(e => e.Clientcoordinator).HasColumnName("clientcoordinator");
            entity.Property(e => e.CommittedClientBillingDate).HasColumnName("committed_client_billing_date");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno).HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Costattachment).HasColumnName("costattachment");
            entity.Property(e => e.Costsheetid).HasColumnName("costsheetid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Deliveryanchorid).HasColumnName("deliveryanchorid");
            entity.Property(e => e.Emailattachment).HasColumnName("emailattachment");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isextended).HasColumnName("isextended");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Milestones).HasColumnName("milestones");
            entity.Property(e => e.Oafid).HasColumnName("oafid");
            entity.Property(e => e.Oafno).HasColumnName("oafno");
            entity.Property(e => e.Orderdescription).HasColumnName("orderdescription");
            entity.Property(e => e.Poattachment).HasColumnName("poattachment");
            entity.Property(e => e.Ponumber).HasColumnName("ponumber");
            entity.Property(e => e.Potermscondition).HasColumnName("potermscondition");
            entity.Property(e => e.Povalue).HasColumnName("povalue");
            entity.Property(e => e.Projectdescription).HasColumnName("projectdescription");
            entity.Property(e => e.Projectmodel).HasColumnName("projectmodel");
            entity.Property(e => e.Projectname).HasColumnName("projectname");
            entity.Property(e => e.Projecttypeid).HasColumnName("projecttypeid");
            entity.Property(e => e.Proposalattachment).HasColumnName("proposalattachment");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Revisionnumber).HasColumnName("revisionnumber");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Subpracticeid).HasColumnName("subpracticeid");
            entity.Property(e => e.Udf1).HasColumnName("udf1");
            entity.Property(e => e.Udf2).HasColumnName("udf2");
            entity.Property(e => e.Udf3).HasColumnName("udf3");
            entity.Property(e => e.Xvalue).HasColumnName("xvalue");

            entity.HasOne(d => d.Accountmanager).WithMany(p => p.OafExtendedHistoryAccountmanagers)
                .HasForeignKey(d => d.Accountmanagerid)
                .HasConstraintName("oaf_extended_history_accountmanagerid_fkey");

            entity.HasOne(d => d.Costsheet).WithMany(p => p.OafExtendedHistories)
                .HasForeignKey(d => d.Costsheetid)
                .HasConstraintName("oaf_extended_history_costsheetid_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.OafExtendedHistories)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("oaf_extended_history_customerid_fkey");

            entity.HasOne(d => d.Deliveryanchor).WithMany(p => p.OafExtendedHistoryDeliveryanchors)
                .HasForeignKey(d => d.Deliveryanchorid)
                .HasConstraintName("oaf_extended_history_deliveryanchorid_fkey");

            entity.HasOne(d => d.Oaf).WithMany(p => p.OafExtendedHistories)
                .HasForeignKey(d => d.Oafid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("oaf_extended_history_oafid_fkey");

            entity.HasOne(d => d.Projecttype).WithMany(p => p.OafExtendedHistories)
                .HasForeignKey(d => d.Projecttypeid)
                .HasConstraintName("oaf_extended_history_projecttypeid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.OafExtendedHistories)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("oaf_extended_history_statusid_fkey");

            entity.HasOne(d => d.Subpractice).WithMany(p => p.OafExtendedHistories)
                .HasForeignKey(d => d.Subpracticeid)
                .HasConstraintName("oaf_extended_history_subpracticeid_fkey");
        });

        modelBuilder.Entity<Oafchecklist>(entity =>
        {
            entity.HasKey(e => e.Oafchecklistid).HasName("oafchecklist_pkey");

            entity.ToTable("oafchecklist");

            entity.Property(e => e.Oafchecklistid).HasColumnName("oafchecklistid");
            entity.Property(e => e.Clientresponse).HasColumnName("clientresponse");
            entity.Property(e => e.Isextra).HasColumnName("isextra");
            entity.Property(e => e.Oafid).HasColumnName("oafid");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Oaf).WithMany(p => p.Oafchecklists)
                .HasForeignKey(d => d.Oafid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_oafchecklist_oafid");

            entity.HasOne(d => d.Status).WithMany(p => p.Oafchecklists)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("fk_oafchecklist_statusid");
        });

        modelBuilder.Entity<Oafline>(entity =>
        {
            entity.HasKey(e => e.Oaflineid).HasName("oaflines_pkey");

            entity.ToTable("oaflines");

            entity.Property(e => e.Oaflineid).HasColumnName("oaflineid");
            entity.Property(e => e.Lineamount).HasColumnName("lineamount");
            entity.Property(e => e.Linedescription1).HasColumnName("linedescription1");
            entity.Property(e => e.Linedescription2).HasColumnName("linedescription2");
            entity.Property(e => e.Lineno).HasColumnName("lineno");
            entity.Property(e => e.Oafid).HasColumnName("oafid");

            entity.HasOne(d => d.Oaf).WithMany(p => p.Oaflines)
                .HasForeignKey(d => d.Oafid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_oaflines_oafid");
        });

        modelBuilder.Entity<Oldproject>(entity =>
        {
            entity.HasKey(e => e.Projectid).HasName("project_pkey");

            entity.ToTable("oldproject");

            entity.Property(e => e.Projectid)
                .HasDefaultValueSql("nextval('project_projectid_seq'::regclass)")
                .HasColumnName("projectid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Projectdiscription).HasColumnName("projectdiscription");
            entity.Property(e => e.Projectname).HasColumnName("projectname");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Status).WithMany(p => p.Oldprojects)
                .HasForeignKey(d => d.Statusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("project_statusid_fkey");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.Pageid).HasName("page_pkey");

            entity.ToTable("page");

            entity.Property(e => e.Pageid).HasColumnName("pageid");
            entity.Property(e => e.Actionname).HasColumnName("actionname");
            entity.Property(e => e.Controllername).HasColumnName("controllername");
            entity.Property(e => e.Createdby)
                .HasDefaultValueSql("0")
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdatedby)
                .HasDefaultValueSql("0")
                .HasColumnName("lastupdatedby");
            entity.Property(e => e.Lastupdateddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdateddate");
            entity.Property(e => e.Moduleid).HasColumnName("moduleid");
            entity.Property(e => e.Pagename).HasColumnName("pagename");

            entity.HasOne(d => d.Module).WithMany(p => p.Pages)
                .HasForeignKey(d => d.Moduleid)
                .HasConstraintName("fk_page_moduleid");
        });

        modelBuilder.Entity<Paymentmethod>(entity =>
        {
            entity.HasKey(e => e.Paymentmethodid).HasName("paymentmethod_pkey");

            entity.ToTable("paymentmethod");

            entity.HasIndex(e => e.Paymentmethodname, "paymentmethod_paymentmethodname_key").IsUnique();

            entity.Property(e => e.Paymentmethodid).HasColumnName("paymentmethodid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Paymentmethodname).HasColumnName("paymentmethodname");
        });

        modelBuilder.Entity<Paymentterm>(entity =>
        {
            entity.HasKey(e => e.Paymenttermid).HasName("paymentterm_pkey");

            entity.ToTable("paymentterm");

            entity.HasIndex(e => e.Paymenttermname, "paymentterm_paymenttermname_key").IsUnique();

            entity.Property(e => e.Paymenttermid).HasColumnName("paymenttermid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Paymenttermname).HasColumnName("paymenttermname");
        });

        modelBuilder.Entity<Practice>(entity =>
        {
            entity.HasKey(e => e.Practiceid).HasName("practice_pkey");

            entity.ToTable("practice");

            entity.Property(e => e.Practiceid)
                .ValueGeneratedNever()
                .HasColumnName("practiceid");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Practicename)
                .HasMaxLength(255)
                .HasColumnName("practicename");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Status).WithMany(p => p.Practices)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("practice_statusid_fkey");
        });

        modelBuilder.Entity<Practicehead>(entity =>
        {
            entity.HasKey(e => e.Practiceheadid).HasName("practicehead_pkey");

            entity.ToTable("practicehead");

            entity.Property(e => e.Practiceheadid).HasColumnName("practiceheadid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Practiceid).HasColumnName("practiceid");

            entity.HasOne(d => d.Employee).WithMany(p => p.Practiceheads)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("practicehead_employeeid_fkey");

            entity.HasOne(d => d.Practice).WithMany(p => p.Practiceheads)
                .HasForeignKey(d => d.Practiceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("practicehead_practiceid_fkey");
        });

        modelBuilder.Entity<Presalesquestionmaster>(entity =>
        {
            entity.HasKey(e => e.Questionid).HasName("pesalesquestionmaster_pkey");

            entity.ToTable("presalesquestionmaster");

            entity.Property(e => e.Questionid)
                .HasDefaultValueSql("nextval('pesalesquestionmaster_questionid_seq'::regclass)")
                .HasColumnName("questionid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Practiceid).HasColumnName("practiceid");
            entity.Property(e => e.Question).HasColumnName("question");
            entity.Property(e => e.Refrenceresponse).HasColumnName("refrenceresponse");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Practice).WithMany(p => p.Presalesquestionmasters)
                .HasForeignKey(d => d.Practiceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pesalesquestionmaster_practiceid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Presalesquestionmasters)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("pesalesquestionmaster_statusid_fkey");
        });

        modelBuilder.Entity<Probillapprl>(entity =>
        {
            entity.HasKey(e => e.Probillapprlid).HasName("probillapprl_pkey1");

            entity.ToTable("probillapprl");

            entity.Property(e => e.Probillapprlid)
                .HasDefaultValueSql("nextval('probillapprl_probillapprlid_seq1'::regclass)")
                .HasColumnName("probillapprlid");
            entity.Property(e => e.Contractbillingprovesionid).HasColumnName("contractbillingprovesionid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Currentstageid).HasColumnName("currentstageid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Contractbillingprovesion).WithMany(p => p.Probillapprls)
                .HasForeignKey(d => d.Contractbillingprovesionid)
                .HasConstraintName("probillapprl_contractbillingprovesionid_fkey1");

            entity.HasOne(d => d.Currentstage).WithMany(p => p.Probillapprls)
                .HasForeignKey(d => d.Currentstageid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probillapprl_currentstageid_fkey1");

            entity.HasOne(d => d.Status).WithMany(p => p.Probillapprls)
                .HasForeignKey(d => d.Statusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probillapprl_statusid_fkey1");
        });

        modelBuilder.Entity<ProbillapprlOld>(entity =>
        {
            entity.HasKey(e => e.Probillapprlid).HasName("probillapprl_pkey");

            entity.ToTable("probillapprlOLD");

            entity.Property(e => e.Probillapprlid)
                .HasDefaultValueSql("nextval('probillapprl_probillapprlid_seq'::regclass)")
                .HasColumnName("probillapprlid");
            entity.Property(e => e.Contractbillingprovesionid).HasColumnName("contractbillingprovesionid");
            entity.Property(e => e.Currentstageid).HasColumnName("currentstageid");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Contractbillingprovesion).WithMany(p => p.ProbillapprlOlds)
                .HasForeignKey(d => d.Contractbillingprovesionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probillapprl_contractbillingprovesionid_fkey");

            entity.HasOne(d => d.Currentstage).WithMany(p => p.ProbillapprlOlds)
                .HasForeignKey(d => d.Currentstageid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probillapprl_currentstageid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ProbillapprlOlds)
                .HasForeignKey(d => d.Statusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probillapprl_statusid_fkey");
        });

        modelBuilder.Entity<Probillapprldetail>(entity =>
        {
            entity.HasKey(e => e.Probillapprldetailid).HasName("probillapprldetail_pkey");

            entity.ToTable("probillapprldetail");

            entity.Property(e => e.Probillapprldetailid)
                .HasDefaultValueSql("nextval('probillapprldetail_probillapprldetail_seq'::regclass)")
                .HasColumnName("probillapprldetailid");
            entity.Property(e => e.Actiontakenon)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actiontakenon");
            entity.Property(e => e.Isactiontaken).HasColumnName("isactiontaken");
            entity.Property(e => e.Probillapprlid).HasColumnName("probillapprlid");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.Stageid).HasColumnName("stageid");

            entity.HasOne(d => d.Probillapprl).WithMany(p => p.Probillapprldetails)
                .HasForeignKey(d => d.Probillapprlid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probillapprldetail_probillapprlid_fkey");

            entity.HasOne(d => d.Stage).WithMany(p => p.Probillapprldetails)
                .HasForeignKey(d => d.Stageid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probillapprldetail_stageid_fkey");
        });

        modelBuilder.Entity<ProbillapprldetailOld>(entity =>
        {
            entity.HasKey(e => e.Probillapprldetail).HasName("probbillapprldetail_pkey");

            entity.ToTable("probillapprldetailOLD");

            entity.Property(e => e.Probillapprldetail)
                .HasDefaultValueSql("nextval('probbillapprldetail_billingapprovaldetailid_seq'::regclass)")
                .HasColumnName("probillapprldetail");
            entity.Property(e => e.Actiontakenon)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("actiontakenon");
            entity.Property(e => e.Isactiontaken).HasColumnName("isactiontaken");
            entity.Property(e => e.Probillapprlid).HasColumnName("probillapprlid");
            entity.Property(e => e.Remark).HasColumnName("remark");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Probillapprl).WithMany(p => p.ProbillapprldetailOlds)
                .HasForeignKey(d => d.Probillapprlid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probbillapprldetail_probillapprlid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ProbillapprldetailOlds)
                .HasForeignKey(d => d.Statusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("probbillapprldetail_statusid_fkey");
        });

        modelBuilder.Entity<Probillapprlstage>(entity =>
        {
            entity.HasKey(e => e.Probillapprlstageid).HasName("probillapprlstage_pkey");

            entity.ToTable("probillapprlstage");

            entity.Property(e => e.Probillapprlstageid).HasColumnName("probillapprlstageid");
            entity.Property(e => e.Practiceheadid).HasColumnName("practiceheadid");
            entity.Property(e => e.Stageapproverid).HasColumnName("stageapproverid");
            entity.Property(e => e.Stagename)
                .HasMaxLength(255)
                .HasColumnName("stagename");
            entity.Property(e => e.Stageorder).HasColumnName("stageorder");

            entity.HasOne(d => d.Practicehead).WithMany(p => p.Probillapprlstages)
                .HasForeignKey(d => d.Practiceheadid)
                .HasConstraintName("probillapprlstage_practiceheadid_fkey");

            entity.HasOne(d => d.Stageapprover).WithMany(p => p.Probillapprlstages)
                .HasForeignKey(d => d.Stageapproverid)
                .HasConstraintName("probillapprlstage_stageapproverid_fkey");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Projectid).HasName("project_pkey2");

            entity.ToTable("project");

            entity.Property(e => e.Projectid)
                .HasDefaultValueSql("nextval('project_projectid_seq2'::regclass)")
                .HasColumnName("projectid");
            entity.Property(e => e.Accountmanagerid).HasColumnName("accountmanagerid");
            entity.Property(e => e.Billingcycledate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("billingcycledate");
            entity.Property(e => e.CommittedClientBillingDate).HasColumnName("committed_client_billing_date");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Lastupdatedby)
                .HasDefaultValueSql("0")
                .HasColumnName("lastupdatedby");
            entity.Property(e => e.Projectdescription).HasColumnName("projectdescription");
            entity.Property(e => e.Projectname)
                .HasMaxLength(255)
                .HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projecttypeid).HasColumnName("projecttypeid");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Subpracticeid).HasColumnName("subpracticeid");

            entity.HasOne(d => d.Accountmanager).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Accountmanagerid)
                .HasConstraintName("project_accountmanagerid_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("fk_project_customerid");

            entity.HasOne(d => d.Projecttype).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Projecttypeid)
                .HasConstraintName("fk_project_projecttypeid");

            entity.HasOne(d => d.Status).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("fk_project_statusid");

            entity.HasOne(d => d.Subpractice).WithMany(p => p.Projects)
                .HasForeignKey(d => d.Subpracticeid)
                .HasConstraintName("fk_project_subpracticeid");
        });

        modelBuilder.Entity<ProjectOld>(entity =>
        {
            entity.HasKey(e => e.Projectid).HasName("project_pkey1");

            entity.ToTable("projectOld");

            entity.Property(e => e.Projectid)
                .HasDefaultValueSql("nextval('project_projectid_seq1'::regclass)")
                .HasColumnName("projectid");
            entity.Property(e => e.Contactpersonname).HasColumnName("contactpersonname");
            entity.Property(e => e.Contractenddate).HasColumnName("contractenddate");
            entity.Property(e => e.Contractno).HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate).HasColumnName("contractstartdate");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Ponumber).HasColumnName("ponumber");
            entity.Property(e => e.Projectdescription).HasColumnName("projectdescription");
            entity.Property(e => e.Projectheadid).HasColumnName("projectheadid");
            entity.Property(e => e.Projectmodelid).HasColumnName("projectmodelid");
            entity.Property(e => e.Projectname).HasColumnName("projectname");
            entity.Property(e => e.Projecttypeid).HasColumnName("projecttypeid");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Subpractiseid).HasColumnName("subpractiseid");

            entity.HasOne(d => d.Customer).WithMany(p => p.ProjectOldCustomers)
                .HasForeignKey(d => d.Customerid)
                .HasConstraintName("project_customerid_fkey1");

            entity.HasOne(d => d.Projectmodel).WithMany(p => p.ProjectOldProjectmodels)
                .HasForeignKey(d => d.Projectmodelid)
                .HasConstraintName("project_projectmodelid_fkey");

            entity.HasOne(d => d.Projecttype).WithMany(p => p.ProjectOldProjecttypes)
                .HasForeignKey(d => d.Projecttypeid)
                .HasConstraintName("project_projecttypeid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ProjectOlds)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("project_statusid_fkey1");

            entity.HasOne(d => d.Subpractise).WithMany(p => p.ProjectOldSubpractises)
                .HasForeignKey(d => d.Subpractiseid)
                .HasConstraintName("project_subpractiseid_fkey");
        });

        modelBuilder.Entity<Projectcontract>(entity =>
        {
            entity.HasKey(e => e.Contractid).HasName("contract_pkey");

            entity.ToTable("projectcontract");

            entity.Property(e => e.Contractid)
                .HasDefaultValueSql("nextval('contract_contractid_seq'::regclass)")
                .HasColumnName("contractid");
            entity.Property(e => e.Amount)
                .HasPrecision(19, 4)
                .HasColumnName("amount");
            entity.Property(e => e.Attachment).HasColumnName("attachment");
            entity.Property(e => e.Contactnumber).HasColumnName("contactnumber");
            entity.Property(e => e.Contactpersonname)
                .HasMaxLength(255)
                .HasColumnName("contactpersonname");
            entity.Property(e => e.Contractenddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractenddate");
            entity.Property(e => e.Contractno)
                .HasMaxLength(255)
                .HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("contractstartdate");
            entity.Property(e => e.Costsheetid).HasColumnName("costsheetid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Deliveryanchorid).HasColumnName("deliveryanchorid");
            entity.Property(e => e.Invoiceperiod).HasColumnName("invoiceperiod");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isforeclosure)
                .HasDefaultValueSql("false")
                .HasColumnName("isforeclosure");
            entity.Property(e => e.Isprojectestimationdone)
                .HasDefaultValueSql("false")
                .HasColumnName("isprojectestimationdone");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Oafid).HasColumnName("oafid");
            entity.Property(e => e.Ponumber)
                .HasMaxLength(255)
                .HasColumnName("ponumber");
            entity.Property(e => e.Povalue).HasColumnName("povalue");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Udf1).HasColumnName("udf1");

            entity.HasOne(d => d.Deliveryanchor).WithMany(p => p.Projectcontracts)
                .HasForeignKey(d => d.Deliveryanchorid)
                .HasConstraintName("fk_projectcontract_deliveryanchorid");

            entity.HasOne(d => d.Oaf).WithMany(p => p.Projectcontracts)
                .HasForeignKey(d => d.Oafid)
                .HasConstraintName("projectcontract_oafid_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Projectcontracts)
                .HasForeignKey(d => d.Projectid)
                .HasConstraintName("contract_projectid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Projectcontracts)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("contract_statusid_fkey");
        });

        modelBuilder.Entity<Projectemployeeassignment>(entity =>
        {
            entity.HasKey(e => e.Projectemployeeassignmentid).HasName("projectemployeeassigment_pkey");

            entity.ToTable("projectemployeeassignment");

            entity.Property(e => e.Projectemployeeassignmentid)
                .HasDefaultValueSql("nextval('projectemployeeassigment_projectemployeeassigmentid_seq'::regclass)")
                .HasColumnName("projectemployeeassignmentid");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Startdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startdate");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Categorysubstatus).WithMany(p => p.Projectemployeeassignments)
                .HasForeignKey(d => d.Categorysubstatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projectemployeeassigment_categorysubstatusid_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.Projectemployeeassignments)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projectemployeeassigment_employeeid_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Projectemployeeassignments)
                .HasForeignKey(d => d.Projectid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projectemployeeassigment_projectid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Projectemployeeassignments)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("projectemployeeassigment_statusid_fkey");
        });

        modelBuilder.Entity<Projection>(entity =>
        {
            entity.HasKey(e => e.Projectionid).HasName("projection_pkey");

            entity.ToTable("projection");

            entity.Property(e => e.Projectionid).HasColumnName("projectionid");
            entity.Property(e => e.Costsheetid).HasColumnName("costsheetid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.CrmDealsid).HasColumnName("crm_dealsid");
            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Lastupdatedby)
                .HasDefaultValueSql("0")
                .HasColumnName("lastupdatedby");
            entity.Property(e => e.Projectheadid).HasColumnName("projectheadid");
            entity.Property(e => e.Projectioncost).HasColumnName("projectioncost");
            entity.Property(e => e.Projectiondescription).HasColumnName("projectiondescription");
            entity.Property(e => e.Projectionname)
                .HasMaxLength(255)
                .HasColumnName("projectionname");
            entity.Property(e => e.Projectionno).HasColumnName("projectionno");
            entity.Property(e => e.Projecttypeid).HasColumnName("projecttypeid");
            entity.Property(e => e.Startdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startdate");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Subpracticeid).HasColumnName("subpracticeid");

            entity.HasOne(d => d.Costsheet).WithMany(p => p.Projections)
                .HasForeignKey(d => d.Costsheetid)
                .HasConstraintName("fk_projection_costsheet");

            entity.HasOne(d => d.CrmDeals).WithMany(p => p.Projections)
                .HasForeignKey(d => d.CrmDealsid)
                .HasConstraintName("projection_crm_dealsid_fkey");

            entity.HasOne(d => d.Customer).WithMany(p => p.Projections)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projection_customerid_fkey");

            entity.HasOne(d => d.Projecthead).WithMany(p => p.Projections)
                .HasForeignKey(d => d.Projectheadid)
                .HasConstraintName("projection_projectheadid_fkey");

            entity.HasOne(d => d.Projecttype).WithMany(p => p.Projections)
                .HasForeignKey(d => d.Projecttypeid)
                .HasConstraintName("projection_projecttypeid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Projections)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("projection_statusid_fkey");

            entity.HasOne(d => d.Subpractice).WithMany(p => p.Projections)
                .HasForeignKey(d => d.Subpracticeid)
                .HasConstraintName("projection_subpracticeid_fkey");
        });

        modelBuilder.Entity<ProjectionEmployeeDeployement>(entity =>
        {
            entity.HasKey(e => e.ProjectionEmployeeDeployementId).HasName("projection_employee_deployement_pkey");

            entity.ToTable("projection_employee_deployement");

            entity.Property(e => e.ProjectionEmployeeDeployementId)
                .HasDefaultValueSql("nextval('projection_employee_deployeme_projection_employee_deployeme_seq'::regclass)")
                .HasColumnName("projection_employee_deployement_id");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.DeployedEmployeeId).HasColumnName("deployed_employee_id");
            entity.Property(e => e.Enddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Projectionid).HasColumnName("projectionid");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Startdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startdate");

            entity.HasOne(d => d.Categorysubstatus).WithMany(p => p.ProjectionEmployeeDeployements)
                .HasForeignKey(d => d.Categorysubstatusid)
                .HasConstraintName("projection_employee_deployement_categorysubstatusid_fkey");

            entity.HasOne(d => d.DeployedEmployee).WithMany(p => p.ProjectionEmployeeDeployements)
                .HasForeignKey(d => d.DeployedEmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projection_employee_deployement_deployed_employee_id_fkey");

            entity.HasOne(d => d.Projection).WithMany(p => p.ProjectionEmployeeDeployements)
                .HasForeignKey(d => d.Projectionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("projection_employee_deployement_projectionid_fkey");
        });

        modelBuilder.Entity<Projectioninitialbilling>(entity =>
        {
            entity.HasKey(e => e.Projectioninitialbillingid).HasName("projectioninitialbilling_pkey");

            entity.ToTable("projectioninitialbilling");

            entity.Property(e => e.Projectioninitialbillingid).HasColumnName("projectioninitialbillingid");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Monthyear)
                .HasMaxLength(7)
                .HasColumnName("monthyear");
            entity.Property(e => e.Projectionid).HasColumnName("projectionid");

            entity.HasOne(d => d.Projection).WithMany(p => p.Projectioninitialbillings)
                .HasForeignKey(d => d.Projectionid)
                .HasConstraintName("projectioninitialbilling_projectionid_fkey");
        });

        modelBuilder.Entity<Projectionrequest>(entity =>
        {
            entity.HasKey(e => e.Projectionrequestid).HasName("projectionrequests_pkey");

            entity.ToTable("projectionrequests");

            entity.Property(e => e.Projectionrequestid).HasColumnName("projectionrequestid");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Lastupdatedby).HasColumnName("lastupdatedby");
            entity.Property(e => e.Projectionid).HasColumnName("projectionid");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Requestsentby).HasColumnName("requestsentby");
            entity.Property(e => e.Requestsentto).HasColumnName("requestsentto");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Projection).WithMany(p => p.Projectionrequests)
                .HasForeignKey(d => d.Projectionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_projection");
        });

        modelBuilder.Entity<Projectmodel>(entity =>
        {
            entity.HasKey(e => e.Projectmodelid).HasName("projectmodel_pkey");

            entity.ToTable("projectmodel");

            entity.Property(e => e.Projectmodelid).HasColumnName("projectmodelid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Projectmodelname)
                .HasMaxLength(255)
                .HasColumnName("projectmodelname");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Status).WithMany(p => p.Projectmodels)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("projectmodel_statusid_fkey");
        });

        modelBuilder.Entity<Projecttype>(entity =>
        {
            entity.HasKey(e => e.Projecttypeid).HasName("projecttype_pkey");

            entity.ToTable("projecttype");

            entity.Property(e => e.Projecttypeid).HasColumnName("projecttypeid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Projecttypename)
                .HasMaxLength(255)
                .HasColumnName("projecttypename");
            entity.Property(e => e.Statusid).HasColumnName("statusid");

            entity.HasOne(d => d.Status).WithMany(p => p.Projecttypes)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("projecttype_statusid_fkey");
        });

        modelBuilder.Entity<Rmsemployee>(entity =>
        {
            entity.HasKey(e => e.Employeeid).HasName("employee_pkey");

            entity.ToTable("rmsemployee");

            entity.HasIndex(e => e.Userid, "employee_userid_key").IsUnique();

            entity.Property(e => e.Employeeid)
                .HasDefaultValueSql("nextval('employee_employeeid_seq'::regclass)")
                .HasColumnName("employeeid");
            entity.Property(e => e.Ade).HasColumnName("ade");
            entity.Property(e => e.Baseoffice).HasColumnName("baseoffice");
            entity.Property(e => e.Branchid).HasColumnName("branchid");
            entity.Property(e => e.Categorysubstatusid).HasColumnName("categorysubstatusid");
            entity.Property(e => e.Companyemail).HasColumnName("companyemail");
            entity.Property(e => e.Contactno).HasColumnName("contactno");
            entity.Property(e => e.Costcenter).HasColumnName("costcenter");
            entity.Property(e => e.Createdby)
                .HasDefaultValueSql("0")
                .HasColumnName("createdby");
            entity.Property(e => e.Createdon)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("createdon");
            entity.Property(e => e.Dateofbirth).HasColumnName("dateofbirth");
            entity.Property(e => e.Dateofjoining).HasColumnName("dateofjoining");
            entity.Property(e => e.Departmentid).HasColumnName("departmentid");
            entity.Property(e => e.Designationid).HasColumnName("designationid");
            entity.Property(e => e.Employeename).HasColumnName("employeename");
            entity.Property(e => e.Employeeregion).HasColumnName("employeeregion");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdatedby)
                .HasDefaultValueSql("0")
                .HasColumnName("lastupdatedby");
            entity.Property(e => e.Lastupdatedon)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("lastupdatedon");
            entity.Property(e => e.Reportheadid).HasColumnName("reportheadid");
            entity.Property(e => e.Resignationdate).HasColumnName("resignationdate");
            entity.Property(e => e.Sbu).HasColumnName("sbu");
            entity.Property(e => e.Sbuid).HasColumnName("sbuid");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Subpracticeid).HasColumnName("subpracticeid");
            entity.Property(e => e.Udf1).HasColumnName("udf1");
            entity.Property(e => e.Udf2).HasColumnName("udf2");
            entity.Property(e => e.Udf3).HasColumnName("udf3");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Workexedays).HasColumnName("workexedays");
            entity.Property(e => e.Workexperience).HasColumnName("workexperience");

            entity.HasOne(d => d.Branch).WithMany(p => p.Rmsemployees)
                .HasForeignKey(d => d.Branchid)
                .HasConstraintName("employee_branchid_fkey");

            entity.HasOne(d => d.Categorysubstatus).WithMany(p => p.Rmsemployees)
                .HasForeignKey(d => d.Categorysubstatusid)
                .HasConstraintName("fk_rmsemployee_categorysubstatusid");

            entity.HasOne(d => d.Department).WithMany(p => p.Rmsemployees)
                .HasForeignKey(d => d.Departmentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_departmentid_fkey");

            entity.HasOne(d => d.Designation).WithMany(p => p.Rmsemployees)
                .HasForeignKey(d => d.Designationid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("employee_designationid_fkey");

            entity.HasOne(d => d.SbuNavigation).WithMany(p => p.Rmsemployees)
                .HasForeignKey(d => d.Sbuid)
                .HasConstraintName("employee_sbuid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Rmsemployees)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("employee_statusid_fkey");

            entity.HasOne(d => d.Subpractice).WithMany(p => p.Rmsemployees)
                .HasForeignKey(d => d.Subpracticeid)
                .HasConstraintName("fk_rmsemployee_subpracticeid");

            entity.HasOne(d => d.Udf2Navigation).WithMany(p => p.Rmsemployees)
                .HasForeignKey(d => d.Udf2)
                .HasConstraintName("fk_vendor_udf2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("role_pkey");

            entity.ToTable("role");

            entity.HasIndex(e => e.Rolename, "role_rolename_key").IsUnique();

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isadmin)
                .HasDefaultValueSql("false")
                .HasColumnName("isadmin");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isrowlevel)
                .HasDefaultValueSql("false")
                .HasColumnName("isrowlevel");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Rolename).HasColumnName("rolename");
        });

        modelBuilder.Entity<Rolepage>(entity =>
        {
            entity.HasKey(e => e.Rolepageid).HasName("rolepage_pkey");

            entity.ToTable("rolepage");

            entity.Property(e => e.Rolepageid).HasColumnName("rolepageid");
            entity.Property(e => e.Isbillingpermit)
                .HasDefaultValueSql("false")
                .HasColumnName("isbillingpermit");
            entity.Property(e => e.Isdeletepermit).HasColumnName("isdeletepermit");
            entity.Property(e => e.Isreadpermit).HasColumnName("isreadpermit");
            entity.Property(e => e.Iswritepermit).HasColumnName("iswritepermit");
            entity.Property(e => e.Pageid).HasColumnName("pageid");
            entity.Property(e => e.Pagesequence).HasColumnName("pagesequence");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Page).WithMany(p => p.Rolepages)
                .HasForeignKey(d => d.Pageid)
                .HasConstraintName("fk_rolepage_pageid");

            entity.HasOne(d => d.Role).WithMany(p => p.Rolepages)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("fk_rolepage_roleid");
        });

        modelBuilder.Entity<Sbu>(entity =>
        {
            entity.HasKey(e => e.Sbuid).HasName("sbu_pkey");

            entity.ToTable("sbu");

            entity.Property(e => e.Sbuid).HasColumnName("sbuid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Sbucode).HasColumnName("sbucode");
            entity.Property(e => e.Sbudesc).HasColumnName("sbudesc");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Skillid).HasName("skill_pkey");

            entity.ToTable("skill");

            entity.HasIndex(e => e.Skillname, "skill_skillname_key").IsUnique();

            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Skillname).HasColumnName("skillname");
        });

        modelBuilder.Entity<Skillcosting>(entity =>
        {
            entity.HasKey(e => e.Skillcostingid).HasName("skillcosting_pkey");

            entity.ToTable("skillcosting");

            entity.Property(e => e.Skillcostingid).HasColumnName("skillcostingid");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Expname).HasColumnName("expname");
            entity.Property(e => e.Fromexpmonth).HasColumnName("fromexpmonth");
            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Toexpmonth).HasColumnName("toexpmonth");

            entity.HasOne(d => d.Skill).WithMany(p => p.Skillcostings)
                .HasForeignKey(d => d.Skillid)
                .HasConstraintName("fk_skillcosting_skillid");
        });

        modelBuilder.Entity<Skilltag>(entity =>
        {
            entity.HasKey(e => e.Tagid).HasName("skilltag_pkey");

            entity.ToTable("skilltag");

            entity.HasIndex(e => e.Tagname, "skilltag_tagname_key").IsUnique();

            entity.Property(e => e.Tagid).HasColumnName("tagid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Skillid).HasColumnName("skillid");
            entity.Property(e => e.Tagname).HasColumnName("tagname");

            entity.HasOne(d => d.Skill).WithMany(p => p.Skilltags)
                .HasForeignKey(d => d.Skillid)
                .HasConstraintName("fk_skilltag_skillid");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Stateid).HasName("state_pkey");

            entity.ToTable("state");

            entity.Property(e => e.Stateid).HasColumnName("stateid");
            entity.Property(e => e.Countryid).HasColumnName("countryid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Statecode).HasColumnName("statecode");
            entity.Property(e => e.Statename).HasColumnName("statename");

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.Countryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_state_countryid");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("status_pkey");

            entity.ToTable("status");

            entity.HasIndex(e => e.Statuscode, "status_statuscode_key").IsUnique();

            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Statuscode).HasColumnName("statuscode");
            entity.Property(e => e.Statusdiscription).HasColumnName("statusdiscription");
            entity.Property(e => e.Statusname).HasColumnName("statusname");
        });

        modelBuilder.Entity<Subpractice>(entity =>
        {
            entity.HasKey(e => e.Subpracticeid).HasName("subpractise_pkey");

            entity.ToTable("subpractice");

            entity.Property(e => e.Subpracticeid)
                .ValueGeneratedNever()
                .HasColumnName("subpracticeid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Practiceid).HasColumnName("practiceid");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Subpracticename)
                .HasMaxLength(255)
                .HasColumnName("subpracticename");

            entity.HasOne(d => d.Practice).WithMany(p => p.Subpractices)
                .HasForeignKey(d => d.Practiceid)
                .HasConstraintName("subpractise_practiceid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Subpractices)
                .HasForeignKey(d => d.Statusid)
                .HasConstraintName("subpractise_statusid_fkey");
        });

        modelBuilder.Entity<Temp>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("temp");

            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<TempContract>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("temp_contract");

            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Contractenddate).HasColumnName("contractenddate");
            entity.Property(e => e.Contractno).HasColumnName("contractno");
            entity.Property(e => e.Contractstartdate).HasColumnName("contractstartdate");
            entity.Property(e => e.Contracttypeid).HasColumnName("contracttypeid");
            entity.Property(e => e.Invoiceperiod).HasColumnName("invoiceperiod");
            entity.Property(e => e.Ponumber).HasColumnName("ponumber");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Serviceperiod).HasColumnName("serviceperiod");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<TempContractLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("temp_contract_line_pkey");

            entity.ToTable("temp_contract_line");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contractid).HasColumnName("contractid");
            entity.Property(e => e.Contractno).HasColumnName("contractno");
            entity.Property(e => e.Lineamount).HasColumnName("lineamount");
            entity.Property(e => e.Linedescription1).HasColumnName("linedescription1");
            entity.Property(e => e.Linedescription2).HasColumnName("linedescription2");
            entity.Property(e => e.Lineno).HasColumnName("lineno");
        });

        modelBuilder.Entity<TempContractbilling1>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("temp_contractbilling");

            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Contractid).HasColumnName("contractid");
            entity.Property(e => e.Contractno).HasColumnName("contractno");
            entity.Property(e => e.Costing).HasColumnName("costing");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Tmc).HasColumnName("tmc");
        });

        modelBuilder.Entity<TempDadatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("temp_dadata");

            entity.Property(e => e.Contractid).HasColumnName("contractid");
            entity.Property(e => e.Deliveryanchorid).HasColumnName("deliveryanchorid");
        });

        modelBuilder.Entity<TempProject>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("temp_project");

            entity.Property(e => e.Customerid).HasColumnName("customerid");
            entity.Property(e => e.Customername).HasColumnName("customername");
            entity.Property(e => e.Projectdescription).HasColumnName("projectdescription");
            entity.Property(e => e.Projectname).HasColumnName("projectname");
            entity.Property(e => e.Projectno).HasColumnName("projectno");
            entity.Property(e => e.Projecttypeid).HasColumnName("projecttypeid");
            entity.Property(e => e.Subpracticeid).HasColumnName("subpracticeid");
        });

        modelBuilder.Entity<Tempcontractbilling>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tempcontractbilling");

            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractbillingid).HasColumnName("contractbillingid");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Costing).HasColumnName("costing");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate).HasColumnName("createddate");
            entity.Property(e => e.Documenturl).HasColumnName("documenturl");
            entity.Property(e => e.Estimatedbillingdate).HasColumnName("estimatedbillingdate");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isbilled).HasColumnName("isbilled");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Istobebilled).HasColumnName("istobebilled");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate).HasColumnName("lastupdatedate");
        });

        modelBuilder.Entity<Tempcontractbillingprovesion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tempcontractbillingprovesion");

            entity.Property(e => e.Billingmonthyear).HasColumnName("billingmonthyear");
            entity.Property(e => e.Contractbillingprovesionid).HasColumnName("contractbillingprovesionid");
            entity.Property(e => e.Contractemployeeid).HasColumnName("contractemployeeid");
            entity.Property(e => e.Costing).HasColumnName("costing");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate).HasColumnName("createddate");
            entity.Property(e => e.Documenturl).HasColumnName("documenturl");
            entity.Property(e => e.EstimatedBillingDate).HasColumnName("estimatedBillingDate");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isbilled).HasColumnName("isbilled");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Isrevised).HasColumnName("isrevised");
            entity.Property(e => e.Istobebilled).HasColumnName("istobebilled");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate).HasColumnName("lastupdatedate");
            entity.Property(e => e.Recievedbillingamount).HasColumnName("recievedbillingamount");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Templateid).HasName("template_pkey");

            entity.ToTable("template");

            entity.Property(e => e.Templateid).HasColumnName("templateid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Templatename).HasColumnName("templatename");
            entity.Property(e => e.Totalhours).HasColumnName("totalhours");

            entity.HasOne(d => d.Employee).WithMany(p => p.Templates)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("template_employeeid_fkey");
        });

        modelBuilder.Entity<Templatedetail>(entity =>
        {
            entity.HasKey(e => e.Templatedetailid).HasName("templatedetail_pkey");

            entity.ToTable("templatedetail");

            entity.Property(e => e.Templatedetailid).HasColumnName("templatedetailid");
            entity.Property(e => e.Activity).HasColumnName("activity");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Departmentid).HasColumnName("departmentid");
            entity.Property(e => e.Hours).HasColumnName("hours");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Templateid).HasColumnName("templateid");

            entity.HasOne(d => d.Department).WithMany(p => p.Templatedetails)
                .HasForeignKey(d => d.Departmentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("templatedetail_departmentid_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Templatedetails)
                .HasForeignKey(d => d.Projectid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("templatedetail_projectid_fkey");

            entity.HasOne(d => d.Template).WithMany(p => p.Templatedetails)
                .HasForeignKey(d => d.Templateid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("templatedetail_templateid_fkey");
        });

        modelBuilder.Entity<Timesheetdetail>(entity =>
        {
            entity.HasKey(e => e.Timesheetdetailid).HasName("timesheetdetail_pkey1");

            entity.ToTable("timesheetdetail");

            entity.Property(e => e.Timesheetdetailid)
                .HasDefaultValueSql("nextval('timesheetdetail_timesheetdetailid_seq1'::regclass)")
                .HasColumnName("timesheetdetailid");
            entity.Property(e => e.Activity).HasColumnName("activity");
            entity.Property(e => e.Benchstatus).HasColumnName("benchstatus");
            entity.Property(e => e.Categoryofactivityid).HasColumnName("categoryofactivityid");
            entity.Property(e => e.Contractid).HasColumnName("contractid");
            entity.Property(e => e.Createdby)
                .HasDefaultValueSql("0")
                .HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Dayhours)
                .HasPrecision(3, 2)
                .HasColumnName("dayhours");
            entity.Property(e => e.Departmentid).HasColumnName("departmentid");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Isdrafted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdrafted");
            entity.Property(e => e.Lastupdateby)
                .HasDefaultValueSql("0")
                .HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Timesheetdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timesheetdate");
            entity.Property(e => e.Timesheetid).HasColumnName("timesheetid");

            entity.HasOne(d => d.Categoryofactivity).WithMany(p => p.Timesheetdetails)
                .HasForeignKey(d => d.Categoryofactivityid)
                .HasConstraintName("fk_categoryofactivity");

            entity.HasOne(d => d.Contract).WithMany(p => p.Timesheetdetails)
                .HasForeignKey(d => d.Contractid)
                .HasConstraintName("timesheetdetail_contractid_fkey");

            entity.HasOne(d => d.Department).WithMany(p => p.Timesheetdetails)
                .HasForeignKey(d => d.Departmentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheetdetail_departmentid_fkey1");

            entity.HasOne(d => d.Timesheet).WithMany(p => p.Timesheetdetails)
                .HasForeignKey(d => d.Timesheetid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheetdetail_timesheetid_fkey1");
        });

        modelBuilder.Entity<Timesheetdetailold>(entity =>
        {
            entity.HasKey(e => e.Timesheetdetailid).HasName("timesheetdetail_pkey");

            entity.ToTable("timesheetdetailold");

            entity.Property(e => e.Timesheetdetailid)
                .HasDefaultValueSql("nextval('timesheetdetail_timesheetdetailid_seq'::regclass)")
                .HasColumnName("timesheetdetailid");
            entity.Property(e => e.Activity).HasColumnName("activity");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Departmentid).HasColumnName("departmentid");
            entity.Property(e => e.Hours).HasColumnName("hours");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.Timesheetid).HasColumnName("timesheetid");

            entity.HasOne(d => d.Department).WithMany(p => p.Timesheetdetailolds)
                .HasForeignKey(d => d.Departmentid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheetdetail_departmentid_fkey");

            entity.HasOne(d => d.Project).WithMany(p => p.Timesheetdetailolds)
                .HasForeignKey(d => d.Projectid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheetdetail_projectid_fkey");

            entity.HasOne(d => d.Timesheet).WithMany(p => p.Timesheetdetailolds)
                .HasForeignKey(d => d.Timesheetid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheetdetail_timesheetid_fkey");
        });

        modelBuilder.Entity<Timesheetheader>(entity =>
        {
            entity.HasKey(e => e.Timesheetid).HasName("timesheetheader_pkey");

            entity.ToTable("timesheetheader");

            entity.Property(e => e.Timesheetid).HasColumnName("timesheetid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Monthyear).HasColumnName("monthyear");

            entity.HasOne(d => d.Employee).WithMany(p => p.Timesheetheaders)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheetheader_employeeid_fkey");
        });

        modelBuilder.Entity<Timesheetold>(entity =>
        {
            entity.HasKey(e => e.Timesheetid).HasName("timesheet_pkey");

            entity.ToTable("timesheetold");

            entity.Property(e => e.Timesheetid)
                .HasDefaultValueSql("nextval('timesheet_timesheetid_seq'::regclass)")
                .HasColumnName("timesheetid");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.Isactive)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdrafted).HasColumnName("isdrafted");
            entity.Property(e => e.Lastupdateby).HasColumnName("lastupdateby");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Timesheetdate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("timesheetdate");
            entity.Property(e => e.Totalhours).HasColumnName("totalhours");

            entity.HasOne(d => d.Employee).WithMany(p => p.Timesheetolds)
                .HasForeignKey(d => d.Employeeid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("timesheet_employeeid_fkey");
        });

        modelBuilder.Entity<Userloginlog>(entity =>
        {
            entity.HasKey(e => e.Userloginlogid).HasName("userloginlogs_pkey");

            entity.ToTable("userloginlogs");

            entity.Property(e => e.Userloginlogid).HasColumnName("userloginlogid");
            entity.Property(e => e.Browserclient).HasColumnName("browserclient");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(45)
                .HasColumnName("ipaddress");
            entity.Property(e => e.Jwttoken).HasColumnName("jwttoken");
            entity.Property(e => e.Logindatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("logindatetime");
            entity.Property(e => e.Loginstatus)
                .HasMaxLength(100)
                .HasColumnName("loginstatus");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Vendorid).HasName("vendor_pkey");

            entity.ToTable("vendor");

            entity.Property(e => e.Vendorid).HasColumnName("vendorid");
            entity.Property(e => e.Address1).HasColumnName("address1");
            entity.Property(e => e.Address2).HasColumnName("address2");
            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.Createdate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdate");
            entity.Property(e => e.Createdby)
                .HasDefaultValueSql("0")
                .HasColumnName("createdby");
            entity.Property(e => e.Currencyid).HasColumnName("currencyid");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Gstnumber).HasColumnName("gstnumber");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("true")
                .HasColumnName("isactive");
            entity.Property(e => e.Isdeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("isdeleted");
            entity.Property(e => e.Lastupdatedate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdatedate");
            entity.Property(e => e.Lastupdatedby)
                .HasDefaultValueSql("0")
                .HasColumnName("lastupdatedby");
            entity.Property(e => e.Pannumber).HasColumnName("pannumber");
            entity.Property(e => e.Paymenttermid).HasColumnName("paymenttermid");
            entity.Property(e => e.Phone1).HasColumnName("phone1");
            entity.Property(e => e.Phone2).HasColumnName("phone2");
            entity.Property(e => e.Udf1).HasColumnName("udf1");
            entity.Property(e => e.Udf2).HasColumnName("udf2");
            entity.Property(e => e.Udf3).HasColumnName("udf3");
            entity.Property(e => e.Vendorcode).HasColumnName("vendorcode");
            entity.Property(e => e.Vendorcontact).HasColumnName("vendorcontact");
            entity.Property(e => e.Vendorname).HasColumnName("vendorname");
            entity.Property(e => e.Zipcode).HasColumnName("zipcode");

            entity.HasOne(d => d.City).WithMany(p => p.Vendors)
                .HasForeignKey(d => d.Cityid)
                .HasConstraintName("vendor_cityid_fkey");

            entity.HasOne(d => d.Currency).WithMany(p => p.Vendors)
                .HasForeignKey(d => d.Currencyid)
                .HasConstraintName("vendor_currencyid_fkey");

            entity.HasOne(d => d.Paymentterm).WithMany(p => p.Vendors)
                .HasForeignKey(d => d.Paymenttermid)
                .HasConstraintName("vendor_paymenttermid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
