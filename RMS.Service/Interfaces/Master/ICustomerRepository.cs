using Microsoft.AspNetCore.Http;
using RMS.Data.Models;
using RMS.Entity.DTO;

namespace RMS.Service.Interfaces.Master
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        //new chnages 
        public Task<Customer> GetByName(string name);
        public Task<Response> AddCutomerAsync(CustomerRequestDto DtoObject,IFormFile CustomerLogo,JwtLoginDetailDto LoginDetails);
        public Task<Response> UpdateCustomerAsync(int ?customerId, CustomerRequestDto DtoObject, IFormFile CustomerLogo, JwtLoginDetailDto LoginDetails);
        //public Task<List<Customer>> GetAllCustomersWithDetailsAsync();
        //public Task<List<CustomerWithDetailsDTO>> GetAllCustomersWithDetailsAsync();
        //public Task<CustomerResposeDto> GetByIdWithDetailsAsync(int CustomerId);
        public Task<List<CustomerWithDetailsDto>> GetAllCustomersWithDetailsAsync();
        public Task<CustomerWithDetailsDto> GetCustomerWithDetailsAsync(int CustomerId);

        public Task<Response> SoftDeleteCustomer(int customerId);

        public Task<List<CustomerWithDetailsDto>> GetAllCustomersByDA(int DeliveryAnchorId);
        public Task<List<CustomerWithDetailsDto>> GetAllCustomersWithIdAsync(int employeeId);
    }
}