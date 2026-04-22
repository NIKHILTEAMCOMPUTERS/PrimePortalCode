using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RMS.Data.Models;
using RMS.Service.Interfaces;
using RMS.Service.Interfaces.Extentions;
using RMS.Service.Interfaces.Master;
using RMS.Service.Interfaces.RBAC;
using RMS.Service.Interfaces.Timesheets;
using RMS.Service.Repositories.Master;
using RMS.Service.Repositories.RBAC;
using RMS.Service.Repositories.Timesheets;

namespace RMS.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeStatusService _employeeStatusService;
        private readonly ILogger<EmployeeRepository> _logger;
        IConfiguration _config;


        public UnitOfWork(RmsDevContext context, IHostingEnvironment env, IConfiguration configuration, IHttpContextAccessor httpContextAccessor,IEmployeeStatusService employeeStatusService, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _employeeStatusService = employeeStatusService;
            _logger = logger;
            _config = configuration;

        }

        public ICountryRepository CountryRepository => new CountryRepository(_context);
        public IStateRepository StateRepository => new StateRepository(_context);
        public IUserLoginLogRepository UserLoginLogRepository => new UserLoginLogRepository(_context);  
        public ICustomerRepository CustomerRepository => new CustomerRepository(_context,_env, _httpContextAccessor);       
        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_context,_logger);
        public ISkillRepository SkillRepository => new SkillRepository(_context);   
        public ICurrencyRepository CurrencyRepository => new CurrencyRepository(_context);
        public ICityRepository CityRepository => new CityRepository(_context);      
        public IPaymentTermRepository PaymentTermRepository => new PaymentTermRepository(_context);
        public IProjectRepository ProjectRepository => new ProjectRepository(_context, _env);
        public IProjectModelRepository ProjectModelRepository=> new ProjectModelRepository(_context);   
        public IProjectTypeRepository  ProjectTypeRepository => new ProjectTypeRepository(_context);    
        public IPracticeRepository PracticeRepository => new PracticeRepository(_context);  
        public IRoleRepository RoleRepository => new RoleRepository(_context);
        public IAuthorizeRepository AuthorizeRepository => new AuthorizeRepository(_context);
        public IContractRepository ContractRepository=> new ContractRepository(_context, _env, _httpContextAccessor,_employeeStatusService);
        public ISubpracticeRepository SubpracticeRepository => new SubPracticeRepository(_context);
        public ICategoryStatusRepository CategoryStatusRepository => new CategoryStatusRepository(_context);
        public ICategorySubStatusRepository CategorySubStatusRepository => new CategorySubStatusRepository(_context);
        public IProjectAssignmentRepository ProjectAssignmentRepository => new ProjectAssignmentRepository(_context, _env);
        public IPreSalesQuestionRepository PreSalesQuestionRepository=> new PreSalesQuestionRepository(_context);   
        public ICostSheetRepository CostSheetRepository => new CostSheetRepository(_context);   
        public IContractTypeRepository  ContractTypeRepository => new ContractTypeRepository(_context);
        public IContractBillinRepository ContractBillinRepository=>new ContractBillinRepository(_context, _env, _httpContextAccessor);
        public IOafRepository IOafRepository => new OafRepository(_context, _env, _httpContextAccessor);
        public IVendorRepository VendorRepository => new VendorRepository(_context, _env, _httpContextAccessor);
        public IDepartmentRepository DepartmentRepository => new DepartmentRepository(_context, _env, _httpContextAccessor);
        public IBranchRepository BranchRepository => new BranchRepository(_context, _env, _httpContextAccessor);

        public IDesignationRepository DesignationRepository => new DesignationRepository(_context, _env, _httpContextAccessor);


       
        public ITimesheetRepository TimesheetRepository => new TimesheetRepository(_context, _config, _env);
        public IAccountManagerRepository AccountManagerRepository => new AccountManagerRepository(_context); 
        public IReportRepository ReportRepository => new ReportRepository(_context, _env, _httpContextAccessor);
        public IProjectionRepository ProjectionRepository=>new ProjectionRepository(_context,_env);   
       

    

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
