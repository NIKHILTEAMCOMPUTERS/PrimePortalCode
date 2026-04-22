using RMS.Service.Interfaces.Master;
using RMS.Service.Interfaces.RBAC;
using RMS.Service.Interfaces.Timesheets;

namespace RMS.Service.Interfaces
{
    public interface IUnitOfWork
    {
        ICountryRepository CountryRepository { get; }
        IStateRepository StateRepository { get; }
        IUserLoginLogRepository UserLoginLogRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        ISkillRepository SkillRepository { get; }
        ICurrencyRepository CurrencyRepository { get; } 
        IPaymentTermRepository PaymentTermRepository { get; }
        ICityRepository CityRepository { get; }
        IProjectModelRepository ProjectModelRepository { get; } 
        IProjectTypeRepository ProjectTypeRepository { get; }   
        IPracticeRepository PracticeRepository { get; }   
        ISubpracticeRepository SubpracticeRepository { get; }   
        IRoleRepository RoleRepository { get; }
        IAuthorizeRepository AuthorizeRepository { get; }
        IContractRepository ContractRepository { get; } 
        ICategorySubStatusRepository CategorySubStatusRepository { get; }
        ICategoryStatusRepository CategoryStatusRepository { get; } 
        IProjectAssignmentRepository ProjectAssignmentRepository { get; }   
        IPreSalesQuestionRepository PreSalesQuestionRepository { get; }
        ICostSheetRepository CostSheetRepository { get; }   
        IContractTypeRepository ContractTypeRepository { get; } 
        IContractBillinRepository ContractBillinRepository { get; }
        IOafRepository IOafRepository { get; }
        IVendorRepository VendorRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IBranchRepository BranchRepository { get; }
        IDesignationRepository DesignationRepository {  get; }
        ITimesheetRepository TimesheetRepository { get; }
       
        IAccountManagerRepository AccountManagerRepository { get; } 
       IReportRepository ReportRepository { get; }  
        IProjectionRepository ProjectionRepository { get; } 
       
      
        Task<bool> Complete();
        bool HasChanges();
    }
}