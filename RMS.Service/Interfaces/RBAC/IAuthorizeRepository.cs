using RMS.Data.Models;
using RMS.Entity.DTO;

namespace RMS.Service.Interfaces.RBAC
{
    public interface IAuthorizeRepository : IGenericRepository<Employeerole>
    {
        public Task<AuthorizeDto> Get(string employeeCode);
    }
}