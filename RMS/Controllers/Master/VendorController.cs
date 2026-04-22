using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Security.Claims;

namespace RMS.Controllers.Master
{
    public class VendorController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public VendorController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<Vendor>> Get()
        {

            return await _uow.VendorRepository.GetAllVendor();
        }

        [HttpGet("{id}")]
        public async Task<VendorDto> Get(int id)
        {

            return await _uow.VendorRepository.GetVendorWithDetailsAsync(id);
        }

        [HttpPost, Authorize]

        public async Task<IActionResult> Post([FromForm] JsonWithFileDto DtoObject)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var VendorObj = JsonConvert.DeserializeObject<VendorDto>(DtoObject.JasonData);

                var result = await _uow.VendorRepository.AddVendorAsync(VendorObj, LoginDetails);
                if (result.responseCode == 400)
                {
                    return BadRequest(result);

                }
                else
                {
                    return Ok(result);
                }
            }
        }

        [HttpPut("{VendorId:int}"), Authorize]
        public async Task<IActionResult> Put(int VendorId, [FromForm] JsonWithFileDto DtoObject)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var Vendorobj = JsonConvert.DeserializeObject<VendorDto>(DtoObject.JasonData);
            if (Vendorobj == null)
                return BadRequest("Vendor json requested data can not be Null");
            if (VendorId <= 0)
                return BadRequest("Vendor Id neither be less than zero or equal to zero/Null");
            if (VendorId != Vendorobj.Vendorid)
                return BadRequest("Vendor Id neither be less than zero or equal to zero/Null");

            var photos = DtoObject.Files;

            if (photos != null)
            {
                foreach (var file in photos)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".gif" && extension != ".png")
                    {
                        return BadRequest("Only image formats (jpg, png, gif) are accepted");
                    }
                }
            }

            IFormFile fileToUpload = null;
            if (photos != null && photos.Count > 0)
            {
                fileToUpload = photos[0];
            }

            var result = await _uow.VendorRepository.UpdateVendorAsync(VendorId, Vendorobj, fileToUpload, LoginDetails);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<Response> Delete(int id)
        {
            return await _uow.VendorRepository.SoftDeleteVendor(id);

        }


        // add vendor employee
        [HttpPost("AddVendorEmployee"), Authorize]

        public async Task<IActionResult> AddVendorEmployee([FromForm] JsonWithFileDto DtoObject)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var VendorObj = JsonConvert.DeserializeObject<employeeResponseDto>(DtoObject.JasonData);

                var result = await _uow.VendorRepository.AddVendorEmployeeAsync(VendorObj, LoginDetails);
                return Ok(result);

            }
        }
        [HttpPut("AddVendorEmployee/{EmployeeId:int}"), Authorize]
        public async Task<IActionResult> UpdateVendorEmployee(int EmployeeId, [FromForm] JsonWithFileDto DtoObject)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var Vendorobj = JsonConvert.DeserializeObject<employeeResponseDto>(DtoObject.JasonData);
            if (Vendorobj == null)
                return BadRequest("Employee json requested data can not be Null");
            if (EmployeeId <= 0)
                return BadRequest("Employee Id neither be less than zero or equal to zero/Null");
            if (EmployeeId != Vendorobj.Employeeid)
                return BadRequest("Employee Id neither be less than zero or equal to zero/Null");

            var photos = DtoObject.Files;

            if (photos != null)
            {
                foreach (var file in photos)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (extension != ".jpg" && extension != ".jpeg" && extension != ".gif" && extension != ".png")
                    {
                        return BadRequest("Only image formats (jpg, png, gif) are accepted");
                    }
                }
            }

            IFormFile fileToUpload = null;
            if (photos != null && photos.Count > 0)
            {
                fileToUpload = photos[0];
            }

            var result = await _uow.VendorRepository.UpdateVendorEmployeeAsync(EmployeeId, Vendorobj, fileToUpload, LoginDetails);
            return Ok(result);
        }

        [HttpDelete("DeleteVendorEmployee/{id:int}")]
        public async Task<Response> DeleteVendorEmployee(int id)
        {
            return await _uow.VendorRepository.SoftDeleteVendorEmployee(id);

        }

        [HttpGet("EmployeeGetById/{id:int}")]
        public async Task<employeeResponseDto> EmployeeGetById(int id)
        {

            return await _uow.VendorRepository.GetEmployeeDetail(id);
        }
    }

}
