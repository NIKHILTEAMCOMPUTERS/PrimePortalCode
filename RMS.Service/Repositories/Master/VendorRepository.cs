using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

namespace RMS.Service.Repositories.Master
{
    public class VendorRepository : GenericRepository<Vendor>, IVendorRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VendorRepository(RmsDevContext context, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<Response> AddVendorAsync(VendorDto DtoObject, JwtLoginDetailDto LoginDetails)
        {
           
           
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var matchingContracts = await _context.Vendors.AsNoTracking()
                            .Where(c => c.Vendorcode == DtoObject.Vendorcode && c.Isactive == true)
                            .ToListAsync();

                    if(matchingContracts.Count > 0) {
                        return new Response { responseCode = 400, responseMessage = "Vendor Code Already Exist Please Use Unique Code" };
                        
                    }

                    var vendor = new Vendor
                    {
                        Vendorcode = DtoObject.Vendorcode,
                        Vendorname = DtoObject.Vendorname,
                        Vendorcontact = DtoObject.Vendorcontact,
                        Email = DtoObject.Email,
                        Phone1 = DtoObject.Phone1,
                        Phone2 = DtoObject.Phone2,
                        Pannumber = DtoObject.Pannumber,
                        Currencyid = DtoObject.Currencyid,
                        Gstnumber = DtoObject.Gstnumber,
                        Paymenttermid = DtoObject.Paymenttermid,
                        Cityid = DtoObject.Cityid,
                        Zipcode = DtoObject.Zipcode,
                        Address1 = DtoObject.Address1,
                        Address2 = DtoObject.Address2,
                        Createdby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync(),
                        Createdate = DateTime.Now,

                    };
                    _context.Vendors.Add(vendor);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new Response { responseCode = 200, responseMessage = "Data Inserted Succesfully" };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        public async Task<List<Vendor>> GetAllVendor()
        {
            var result = new List<Vendor>();
            try
            {

                result = await _context.Vendors.AsNoTracking().Where(x => x.Isdeleted != true && x.Isactive != false)
                                           .Include(x => x.City)
                                                    .ThenInclude(city => city.State)
                                                    .ThenInclude(state => state.Country)
                                                    .Include(x => x.Rmsemployees)
                                                    .ToListAsync();

                return result;


            }
            catch (Exception e)
            {
                throw;

            }

        }
        public async Task<VendorDto> GetVendorWithDetailsAsync(int VendorId)
        {
            var result = new VendorDto();
            try
            {
                result = await _context.Vendors.AsNoTracking().Where(x => x.Isdeleted != true && x.Isactive != false)
                     .Include(currency => currency.Currency)
                     .Include(x => x.City)
                           .ThenInclude(city => city.State)
                           .ThenInclude(state => state.Country)
                     .Select(vendor => new VendorDto
                     {
                         Vendorid = vendor.Vendorid,
                         Vendorcode = string.IsNullOrEmpty(vendor.Vendorcode) ? vendor.Vendorcode : vendor.Vendorcode,
                         Vendorname = vendor.Vendorname,
                         Vendorcontact = vendor.Vendorcontact,
                         Pannumber = vendor.Pannumber,
                         Gstnumber = vendor.Gstnumber,
                         Email = vendor.Email,
                         Phone1 = vendor.Phone1 ?? "",
                         Phone2 = vendor.Phone2 ?? "",
                         Address1 = vendor.Address1 ?? "",
                         Address2 = vendor.Address2 ?? "",
                         Zipcode = vendor.Zipcode ?? "",
                         PaymentTerm = vendor.Paymentterm == null ? null : new PaymentTermDto
                         {
                             PaymentTermId = vendor.Paymentterm.Paymenttermid,
                             PaymentTermName = vendor.Paymentterm.Paymenttermname,
                         },
                         Currency = vendor.Currency == null ? null : new CurrencyDto
                         {
                             CurrencyId = vendor.Currency.Currencyid,
                             CurrencyName = vendor.Currency.Currencyname
                         },

                         City = vendor.City == null ? null : new CityInfo
                         {
                             CityId = vendor.City.Cityid,
                             CityName = vendor.City.Cityname,
                             State = vendor.City.State == null ? null : new StateInfo
                             {
                                 StateId = vendor.City.Stateid,
                                 StateName = vendor.City.State.Statename,
                                 Country = vendor.City.State.Country == null ? null : new CountryDto
                                 {
                                     CountryId = vendor.City.State.Countryid,
                                     CountryName = vendor.City.State.Country.Countryname,
                                 }
                             }

                         },
                         Employees = _context.Rmsemployees.AsNoTracking().Include(x => x.Department).Include(x => x.Branch)
                         .Include(x => x.Subpractice)
                        //.Where(employee => employee.Udf2 == VendorId && employee.Isactive == true && employee.Isdeleted != true)
                        .Where(employee => employee.Udf2 == VendorId)
                        .ToList(),

                     }).FirstOrDefaultAsync(x => x.Vendorid == VendorId);

                return result;

            }
            catch (Exception e)
            {
                throw;

            };
        }

        public async Task<Response> UpdateVendorAsync(int? VendorId, VendorDto DtoObject, IFormFile CustomerLogo, JwtLoginDetailDto LoginDetails)
        {
            string wwwPath = _env.WebRootPath;


            var vendorToUpdate = await _context.Vendors.AsNoTracking().FirstOrDefaultAsync(c => c.Vendorid == VendorId);
            if (vendorToUpdate == null)
                return new Response { responseCode = 404, responseMessage = $"Vendor not found at given VendorId: {VendorId} " };

            // Handle logo upload (similar to AddCustomerAsync method)
            string basePath = Path.Combine(this._env.WebRootPath, @"CustomerLogo\" + LoginDetails.TmcId.ToString());
            string databasePath = @"\CustomerLogo\" + LoginDetails.TmcId.ToString();
            string databsefilePath = "";

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            
            // Update properties
            vendorToUpdate.Vendorname = DtoObject.Vendorname ?? vendorToUpdate.Vendorname;
            vendorToUpdate.Vendorcontact = DtoObject.Vendorcontact ?? vendorToUpdate.Vendorcontact;
            vendorToUpdate.Email = DtoObject.Email ?? vendorToUpdate.Email;
            vendorToUpdate.Phone1 = DtoObject.Phone1 ?? vendorToUpdate.Phone1;
            vendorToUpdate.Phone2 = DtoObject.Phone2 ?? vendorToUpdate.Phone2;
            vendorToUpdate.Pannumber = DtoObject.Pannumber ?? vendorToUpdate.Pannumber;
            vendorToUpdate.Currencyid = DtoObject.Currencyid <= 0 ? vendorToUpdate.Currencyid : DtoObject.Currencyid;
            vendorToUpdate.Gstnumber = DtoObject.Gstnumber ?? vendorToUpdate.Gstnumber;
            vendorToUpdate.Paymenttermid = DtoObject.Paymenttermid <= 0 ? vendorToUpdate.Paymenttermid : DtoObject.Paymenttermid;
            vendorToUpdate.Currencyid = DtoObject.Currencyid <= 0 ? vendorToUpdate.Currencyid : DtoObject.Currencyid;
            vendorToUpdate.Cityid = DtoObject.Cityid <= 0 ? vendorToUpdate.Cityid : DtoObject.Cityid;
            vendorToUpdate.Zipcode = DtoObject.Zipcode ?? vendorToUpdate.Zipcode;
            vendorToUpdate.Lastupdatedby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync();
            vendorToUpdate.Lastupdatedate = DateTime.Now;
            _context.Vendors.Update(vendorToUpdate);

            try
            {
                await _context.SaveChangesAsync();
                return new Response { responseCode = 200, responseMessage = "Data Updated Successfully" };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private Task<bool> IsOldImagesExists(object customerId, string wwwPath, string v)
        {
            throw new NotImplementedException();
        }


        public async Task<Response> SoftDeleteVendor(int vendorId)
        {
            try
            {
                if (vendorId <= 0)
                {
                    throw new Exception("VendorId cannot be less than or equal to zero.");
                }
                var existingVendor = await _context.Vendors.AsNoTracking()
                                                     .FirstOrDefaultAsync(x => x.Vendorid == vendorId);
                if (existingVendor == null)
                {
                    throw new Exception($"Vendor does not exist at the given Id :: {vendorId}");
                }
                existingVendor.Isdeleted = true;
                existingVendor.Isactive = false;
                _context.Vendors.Update(existingVendor);
                await _context.SaveChangesAsync();
                return new Response
                {
                    responseMessage = "Successfully Deleted ",
                    responseCode = 200
                };
            }
            catch (Exception e)
            {


                return new Response
                {
                    responseMessage = e.Message,
                    responseCode = 500
                };
            }
        }

        public async Task<Response> AddVendorEmployeeAsync(employeeResponseDto DtoObject, JwtLoginDetailDto LoginDetails)
        {


            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string newContractno = null;

                    if (DtoObject.Udf2 != null && DtoObject.Udf2 > 0)
                    {
                        var prefix = "TDE/TRE-";

                        var matchingContracts = await _context.Rmsemployees.AsNoTracking()
                            .Where(c => c.Userid.StartsWith(prefix))
                            .ToListAsync();

                        int nextNumber = 1;

                        if (matchingContracts.Any())
                        {
                            var lastContract = matchingContracts
                                .Select(c => int.TryParse(c.Userid.Replace("TDE/TRE-", ""), out int numericValue)
                                    ? numericValue
                                    : int.MinValue)
                                .Max();

                            nextNumber = lastContract + 1;
                        }

                        newContractno = $"{prefix}{nextNumber:D4}";
                    }
                    else if (!string.IsNullOrEmpty(DtoObject.TmcId))
                    {
                        newContractno = DtoObject.TmcId;
                    }

                    var vendor = new Rmsemployee
                { 

                    Employeename = DtoObject.Employeename,
                    Udf2 = DtoObject.Udf2,                              // this will uses for add vendor id in employee table
                    Userid = DtoObject.Udf2 != null && DtoObject.Udf2 > 0
                       ? !string.IsNullOrEmpty(DtoObject.TmcId) ? DtoObject.TmcId : newContractno
                       : !string.IsNullOrEmpty(DtoObject.TmcId) ? DtoObject.TmcId : null,
                    // TMC
                    Companyemail = DtoObject.email,
                        Contactno = DtoObject.Contactno,
                        Branchid = DtoObject.Branchid,
                        Createdby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync(),
                        Createdon = DateOnly.FromDateTime(DateTime.Now),
                        Isactive = true,
                        Isdeleted = false,
                        Subpracticeid = DtoObject.subpracticeid,
                        Designationid = DtoObject.Designationid,
                        Departmentid = 22,
                        Dateofjoining = DtoObject.Dateofjoining,
                        Sbuid = 1,
                    };
                    _context.Rmsemployees.Add(vendor);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new Response { responseCode = 200, responseMessage = "Data Inserted Succesfully" };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        public async Task<Response> UpdateVendorEmployeeAsync(int? EmployeeId, employeeResponseDto DtoObject, IFormFile CustomerLogo, JwtLoginDetailDto LoginDetails)
        {
            string wwwPath = _env.WebRootPath;


            var vendorToUpdate = await _context.Rmsemployees.AsNoTracking().FirstOrDefaultAsync(c => c.Employeeid == EmployeeId);
            if (vendorToUpdate == null)
                return new Response { responseCode = 404, responseMessage = $"Employee not found at given EmployeeId: {EmployeeId} " };

            // Handle logo upload (similar to AddCustomerAsync method)
            string basePath = Path.Combine(this._env.WebRootPath, @"CustomerLogo\" + LoginDetails.TmcId.ToString());
            string databasePath = @"\CustomerLogo\" + LoginDetails.TmcId.ToString();
            string databsefilePath = "";

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            // Update properties
            vendorToUpdate.Employeename = DtoObject.Employeename ?? vendorToUpdate.Employeename;
            vendorToUpdate.Contactno = DtoObject.Contactno ?? vendorToUpdate.Contactno;
            vendorToUpdate.Companyemail = DtoObject.email ?? vendorToUpdate.Companyemail;
            vendorToUpdate.Userid = DtoObject.TmcId ?? vendorToUpdate.Userid;
            vendorToUpdate.Branchid = DtoObject.Branchid <= 0 ? vendorToUpdate.Branchid : DtoObject.Branchid;
            vendorToUpdate.Subpracticeid = DtoObject.subpracticeid <= 0 ? vendorToUpdate.Subpracticeid : DtoObject.subpracticeid;
            vendorToUpdate.Designationid = DtoObject.Designationid <= 0 ? vendorToUpdate.Designationid : DtoObject.Designationid;
            vendorToUpdate.Departmentid = DtoObject.Departmentid <= 0 ? vendorToUpdate.Departmentid : DtoObject.Departmentid;
            vendorToUpdate.Dateofjoining = DtoObject.Dateofjoining;
            _context.Rmsemployees.Update(vendorToUpdate);

            try
            {
                await _context.SaveChangesAsync();
                return new Response { responseCode = 200, responseMessage = "Data Updated Successfully" };
            }
            catch (Exception e)
            {
                throw;
            }
        }

       // this method is used to toggle the vendor from active to inactive state 
        public async Task<Response> SoftDeleteVendorEmployee(int employeeId)
        {
            try
            {
                if (employeeId <= 0)
                {
                    throw new Exception("EmployeeId cannot be less than or equal to zero.");
                }
                var existingVendor = await _context.Rmsemployees.FirstOrDefaultAsync(x => x.Employeeid == employeeId);
                if (existingVendor == null)
                {
                    throw new Exception($"Employee does not exist at the given Id :: {employeeId}");
                }
                //existingVendor.Isdeleted = true;
                //existingVendor.Isactive = false;
                existingVendor.Isdeleted = !existingVendor.Isdeleted;

                // Toggle the IsActive flag
                existingVendor.Isactive = !existingVendor.Isactive;
                _context.Rmsemployees.Update(existingVendor);
                await _context.SaveChangesAsync();
                return new Response
                {
                    responseMessage = "Successfully Deleted ",
                    responseCode = 200
                };
            }
            catch (Exception e)
            {


                return new Response
                {
                    responseMessage = e.Message,
                    responseCode = 500
                };
            }
        }
        public async Task<employeeResponseDto> GetEmployeeDetail(int Id)
        {
            var result = new employeeResponseDto();
            try
            {
                result = await _context.Rmsemployees.AsNoTracking()
                    //.Where(x => x.Isdeleted != true && x.Isactive != false)
                    .Include(x => x.Subpractice).ThenInclude(Subpractice => Subpractice.Practice)
                     .Select(Rmsemployee => new employeeResponseDto
                     {

                         Employeeid = Rmsemployee.Employeeid,
                         Employeename = Rmsemployee.Employeename,
                         TmcId = Rmsemployee.Userid,
                         Departmentid = Rmsemployee.Departmentid,
                         Designationid = Rmsemployee.Designationid,
                         email = Rmsemployee.Companyemail,
                         Contactno = Rmsemployee.Contactno ?? "",
                         Dateofjoining = Rmsemployee.Dateofjoining ?? "",
                         Branchid = Rmsemployee.Branchid,
                         subpracticeid = Rmsemployee.Subpractice.Subpracticeid,
                         Subpractice = Rmsemployee.Subpractice == null ? null : new Subpractice
                         {
                             Subpracticeid = Rmsemployee.Subpractice.Subpracticeid,
                             Subpracticename = Rmsemployee.Subpractice.Subpracticename,
                             Practice = Rmsemployee.Subpractice.Practice == null ? null : new Practice
                             {
                                 Practiceid = (int)Rmsemployee.Subpractice.Practiceid
                             }
                         },
                          Vendor = Rmsemployee.Udf2 == null ? null : new Vendor
                          {
                              Vendorid = Rmsemployee.Udf2Navigation.Vendorid,
                              Vendorname = Rmsemployee.Udf2Navigation.Vendorname,
                          } 
                         
                       
                     }).FirstOrDefaultAsync(x => x.Employeeid == Id);

                return result;

            }
            catch (Exception e)
            {
                throw;

            };
        }
    }
}
