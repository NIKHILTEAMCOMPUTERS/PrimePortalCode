using Microsoft.AspNetCore.Http;
using RMS.Data.Models;
using RMS.Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Service.Interfaces.Master
{
    public interface IVendorRepository : IGenericRepository<Vendor>
    {
        public Task<Response> AddVendorAsync(VendorDto DtoObject, JwtLoginDetailDto LoginDetails);
        public Task<List<Vendor>> GetAllVendor();
        public Task<VendorDto> GetVendorWithDetailsAsync(int VendorId);
        public Task<Response> UpdateVendorAsync(int? VendorId, VendorDto DtoObject, IFormFile CustomerLogo, JwtLoginDetailDto LoginDetails);
        public Task<Response> SoftDeleteVendor(int vendorId);
        public Task<Response> AddVendorEmployeeAsync(employeeResponseDto DtoObject, JwtLoginDetailDto LoginDetails);
        public Task<Response> UpdateVendorEmployeeAsync(int? EmployeeId, employeeResponseDto DtoObject, IFormFile CustomerLogo, JwtLoginDetailDto LoginDetails);

        public Task<Response> SoftDeleteVendorEmployee(int employeeId); 
        public Task<employeeResponseDto> GetEmployeeDetail(int Id);
    }
}
