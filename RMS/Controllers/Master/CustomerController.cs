using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Extensions;
using RMS.Service.Interfaces;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace RMS.Controllers.Master
{
    public class CustomerController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public CustomerController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerWithDetailsDto>> Get()
        {
           //return await _uow.CustomerRepository.GetAsync();
            return await _uow.CustomerRepository.GetAllCustomersWithDetailsAsync();
        }

        [HttpGet("CustomerByDeliveryAnchor/{DeliveryAnchorId:int}")]
        public async Task<IEnumerable<CustomerWithDetailsDto>> GetCustomerByDeliveryAnchorId(int DeliveryAnchorId)
        {
            return await _uow.CustomerRepository.GetAllCustomersByDA(DeliveryAnchorId);
        }

        [HttpGet("{id}")]
        public async Task<CustomerWithDetailsDto> Get(int id)
        {
            //return await _uow.CustomerRepository.GetByIdAsync(id);
            return await _uow.CustomerRepository.GetCustomerWithDetailsAsync(id);
        }

        [HttpPost,Authorize]
      
        public async Task<IActionResult> Post([FromForm] JsonWithFileDto DtoObject)
        {
            JwtLoginDetailDto Userdetails = new JwtLoginDetailDto();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");
            else
            {
                var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
                var CustomerObj  = JsonConvert.DeserializeObject<CustomerRequestDto>(DtoObject.JasonData);
                var photos = DtoObject.Files;
                if (photos != null)
                {
                    foreach (var files in photos)
                    {
                        var extension = Path.GetExtension(files.FileName);
                        if (extension != ".jpg" && extension != ".jpeg" && extension != ".gif" && extension != ".png")
                        {
                            return BadRequest("Only image formats (jpg, png, gif) are accepted ");
                        }
                    }
                }
                IFormFile fileToUpload = null;
                if (photos != null && photos.Count > 0)
                {
                    fileToUpload = photos[0];
                }
                var result = await _uow.CustomerRepository.AddCutomerAsync(CustomerObj, fileToUpload, LoginDetails);
                return Ok(result);
               
            }
        }
        [HttpPut("{CustomerId:int}"), Authorize]
        public async Task<IActionResult> Put(int CustomerId, [FromForm] JsonWithFileDto DtoObject)
        {     

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                return BadRequest("Authentication Fails");

            var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
            var CustomerObj = JsonConvert.DeserializeObject<CustomerRequestDto>(DtoObject.JasonData);
            if (CustomerObj == null)
                return BadRequest("Customer json requested data can not be Null");
            if (CustomerId <= 0)
                return BadRequest("Customer Id neither be less than zero or equal to zero/Null");
            if (CustomerId != CustomerObj.CustomerId)
                return BadRequest("Customer Id neither be less than zero or equal to zero/Null");

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

            var result = await _uow.CustomerRepository.UpdateCustomerAsync(CustomerId, CustomerObj, fileToUpload, LoginDetails);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<Response> Delete(int id)
        {
           return await _uow.CustomerRepository.SoftDeleteCustomer(id);
            
        }











        //[HttpPost("UpdateCustomer/{id:int}"),Authorize]
        //public async Task<IActionResult> UpdateCustomer(int id, [FromForm] JsonWithFileDto DtoObject)
        //{
        //    IFormFile fileToUpload;
        //    if (id <=0)
        //        return BadRequest("Customer Id neither be less tahn zero or equal to zero");
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity == null)
        //        return BadRequest("Authentication Fails");

        //    var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
        //    var CustomerObj = JsonConvert.DeserializeObject<CustomerRequestDto>(DtoObject.JasonData);

        //    var photos = DtoObject.Files;
        //    if (photos != null)
        //    {
        //        foreach (var files in photos)
        //        {
        //            var extension = Path.GetExtension(files.FileName);
        //            if (extension != ".jpg" && extension != ".jpeg" && extension != ".gif" && extension != ".png")
        //            {
        //                return BadRequest("Only image formats (jpg, png, gif) are accepted ");
        //            }
        //        }
        //    }
        //    if (photos[0] != null)
        //    {
        //        fileToUpload = photos[0];
        //    }


        //    var result = await _uow.CustomerRepository.UpdateCustomerAsync(id, CustomerObj, fileToUpload=null, LoginDetails);
        //    return Ok(result);
        //}

        //[HttpPost("UpdateCustomer/{id:int}"), Authorize]
        //public async Task<IActionResult> UpdateCustomer(int id, [FromForm] JsonWithFileDto DtoObject)
        //{
        //    if (id <= 0)
        //        return BadRequest("Customer Id neither be less than zero or equal to zero");

        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity == null)
        //        return BadRequest("Authentication Fails");

        //    var LoginDetails = AuthenticUserDetails.GetCurrentUserDetails(identity);
        //    var CustomerObj = JsonConvert.DeserializeObject<CustomerRequestDto>(DtoObject.JasonData);

        //    var photos = DtoObject.Files;

        //    if (photos != null)
        //    {
        //        foreach (var file in photos)
        //        {
        //            var extension = Path.GetExtension(file.FileName);
        //            if (extension != ".jpg" && extension != ".jpeg" && extension != ".gif" && extension != ".png")
        //            {
        //                return BadRequest("Only image formats (jpg, png, gif) are accepted");
        //            }
        //        }
        //    }

        //    IFormFile fileToUpload = null;
        //    if (photos != null && photos.Count > 0)
        //    {
        //        fileToUpload = photos[0];
        //    }

        //    var result = await _uow.CustomerRepository.UpdateCustomerAsync(id, CustomerObj, fileToUpload, LoginDetails);
        //    return Ok(result);
        //}
        [HttpGet("EmployeeActiveProjects/{employeeId:int}")]
        public async Task<IEnumerable<CustomerWithDetailsDto>> GetCustomersWithEmployee(int employeeId)
        {
            return await _uow.CustomerRepository.GetAllCustomersWithIdAsync(employeeId);
        }

    }
}