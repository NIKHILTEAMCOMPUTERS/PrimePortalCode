using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace RMS.Service.Repositories.Master
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly RmsDevContext _context;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerRepository(RmsDevContext context, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<Response> AddCutomerAsync(CustomerRequestDto DtoObject, IFormFile CustomerLogo, JwtLoginDetailDto LoginDetails)
        {
            //string basePath = Path.Combine(this._env.WebRootPath, @"CustomerLogo\" + LoginDetails.UserId.ToString());
            //string databasePath = @"\CustomerLogo\" + LoginDetails.UserId.ToString();
            string basePath = Path.Combine(this._env.WebRootPath, @"CustomerLogo\" + LoginDetails.TmcId.ToString());
            string databasePath = @"\CustomerLogo\" + LoginDetails.TmcId.ToString();
            string databsefilePath = "";
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            if (CustomerLogo != null)
            {
                string uniqueFileName = null;
                var fileName = Path.GetFileName(CustomerLogo.FileName);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                var filePath = Path.Combine(basePath, uniqueFileName);
                databsefilePath = Path.Combine(databasePath, uniqueFileName);
                var extension = Path.GetExtension(CustomerLogo.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CustomerLogo.CopyToAsync(stream);
                    }

                }

            }

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var customer = new Customer
                    {
                        Customertypeid = DtoObject.Customertypeid,
                        Firstname = DtoObject.Firstname,
                        Lastname = DtoObject.Lastname,
                        Companyname = DtoObject.Companyname,
                        Companylogourl = string.IsNullOrEmpty(databsefilePath) ? null : databsefilePath,
                        Companyemail = DtoObject.Companyemail,
                        Phone1 = DtoObject.Phone1,
                        Phone2 = DtoObject.Phone2,
                        Pannumber = DtoObject.Pannumber,
                        Currencyid = DtoObject.Currencyid,
                        Gstnumber = DtoObject.Gstnumber,
                        Paymenttermid = DtoObject.Paymenttermid,
                        Cityid = DtoObject.Cityid,
                        Zipcode = DtoObject.Zipcode,
                        Address1 = DtoObject.Address1,
                        Paymentmethodid = 1,
                        Customercode = DtoObject.Customercode,
                        Address2 = DtoObject.Address2,
                        Createdby = await _context.Rmsemployees.AsNoTracking().Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync(),
                        Createddate = DateTime.Now,

                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    //here data insertion is perforemd for alternate contancts 
                    if (DtoObject.Customercontactdetails != null)
                    {
                        foreach (var item in DtoObject.Customercontactdetails)
                        {
                            _context.Customercontactdetails.Add(new Customercontactdetail
                            {
                                Customerid = customer.Customerid,
                                Firstname = item.Firstname,
                                Lastname = item.Lastname,
                                Mobile = item.Mobile,
                                Emailid = item.Emailid
                            });
                            await _context.SaveChangesAsync();
                        }
                    }

                    //here a default contact detals is always inserted same as the customertable 
                    customer.Customercontactdetails.Add(new Customercontactdetail
                    {
                        Customerid = customer.Customerid,
                        Firstname = customer.Firstname,
                        Lastname = customer.Lastname,
                        Mobile = customer.Phone1,
                        Emailid = customer.Companyemail
                    });

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    return new Response { responseCode = 200, responseMessage = "Data Inserted Succesfully",
                        data = new
                        {
                            CustomerId = customer.Customerid,
                            CustomerName = customer.Companyname == null ? customer.Firstname + " " + customer.Lastname : customer.Companyname

                        }
                    };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
        public async Task<Response> UpdateCustomerAsync(int? customerId, CustomerRequestDto DtoObject, IFormFile CustomerLogo, JwtLoginDetailDto LoginDetails)
        {
            string wwwPath = _env.WebRootPath;


            var customerToUpdate = await _context.Customers.Include(c => c.Customercontactdetails)
                                                           .FirstOrDefaultAsync(c => c.Customerid == customerId);
            if (customerToUpdate == null)
                return new Response { responseCode = 404, responseMessage = $"Customer not found at given CustomerId: {customerId} " };

            // Handle logo upload (similar to AddCustomerAsync method)
            string basePath = Path.Combine(this._env.WebRootPath, @"CustomerLogo\" + LoginDetails.TmcId.ToString());
            string databasePath = @"\CustomerLogo\" + LoginDetails.TmcId.ToString();
            string databsefilePath = "";

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            if (CustomerLogo != null)
            {
                bool IsOldDeletd = await IsOldImagesExists(customerId, wwwPath, LoginDetails.TmcId.ToString());



                string uniqueFileName = null;
                var fileName = Path.GetFileName(CustomerLogo.FileName);
                uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                var filePath = Path.Combine(basePath, uniqueFileName);
                databsefilePath = Path.Combine(databasePath, uniqueFileName);
                var extension = Path.GetExtension(CustomerLogo.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CustomerLogo.CopyToAsync(stream);
                    }

                }
            }
            // Update properties
            customerToUpdate.Customertypeid = DtoObject.Customertypeid <= 0 ? customerToUpdate.Customertypeid : DtoObject.Customertypeid;
            customerToUpdate.Firstname = DtoObject.Firstname ?? customerToUpdate.Firstname;
            customerToUpdate.Lastname = DtoObject.Lastname ?? customerToUpdate.Lastname;
            customerToUpdate.Companyname = DtoObject.Companyname ?? customerToUpdate.Companyname;
            customerToUpdate.Companyemail = DtoObject.Companyemail ?? customerToUpdate.Companyemail;
            customerToUpdate.Companylogourl = String.IsNullOrEmpty(databsefilePath) ? customerToUpdate.Companylogourl : databsefilePath;
            customerToUpdate.Phone1 = DtoObject.Phone1 ?? customerToUpdate.Phone1;
            customerToUpdate.Phone2 = DtoObject.Phone2 ?? customerToUpdate.Phone2;
            customerToUpdate.Pannumber = DtoObject.Pannumber ?? customerToUpdate.Pannumber;
            customerToUpdate.Currencyid = DtoObject.Currencyid <= 0 ? customerToUpdate.Currencyid : DtoObject.Currencyid;
            customerToUpdate.Gstnumber = DtoObject.Gstnumber ?? customerToUpdate.Gstnumber;
            customerToUpdate.Paymenttermid = DtoObject.Paymenttermid <= 0 ? customerToUpdate.Paymenttermid : DtoObject.Paymenttermid;
            customerToUpdate.Currencyid = DtoObject.Currencyid <= 0 ? customerToUpdate.Currencyid : DtoObject.Currencyid;
            customerToUpdate.Cityid = DtoObject.Cityid <= 0 ? customerToUpdate.Cityid : DtoObject.Cityid;
            customerToUpdate.Zipcode = DtoObject.Zipcode ?? customerToUpdate.Zipcode;
            customerToUpdate.Lastupdateby = await _context.Rmsemployees.Where(x => x.Userid == LoginDetails.TmcId).Select(x => x.Employeeid).FirstOrDefaultAsync();
            customerToUpdate.Lastupdatedate = DateTime.Now;
            customerToUpdate.Address1 = DtoObject.Address1 ?? customerToUpdate.Address1;
            customerToUpdate.Address2 = DtoObject.Address2 ?? customerToUpdate.Address2;

            _context.Customers.Update(customerToUpdate);

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
        public async Task<List<CustomerWithDetailsDto>> GetAllCustomersWithDetailsAsync()
        {
            var result = new List<CustomerWithDetailsDto>();
            try
            {
                //var result= await _context.Customers
                //                            //.Include(c => c.Customercontactdetails)
                //                            .Include(x => x.City)
                //                                    .ThenInclude(city => city.State) 
                //                                    .ThenInclude(state=>state.Country)
                //                                    .ToListAsync();

                result = await _context.Customers.AsNoTracking().Where(x => x.Isdeleted != true && x.Isactive != false)
                        .Include(currency => currency.Currency)
                        .Include(paymentTe => paymentTe.Paymentterm)
                        .Include(x => x.City)
                        //.ThenInclude(city => city.State)
                        //.ThenInclude(state => state.Country)
                        .Include(projects => projects.Projects)
                              .ThenInclude(empAssignment => empAssignment.Projectemployeeassignments)
                              .ThenInclude(emp => emp.Employee)
                        //.Include(projects => projects.Projects)
                        //      .ThenInclude(contr => contr.Projectcontracts)
                        //.Include(projects => projects.Projects)
                        //      .ThenInclude(subPrac => subPrac.Subpractice)
                        //            .ThenInclude(prac => prac.Practice)
                        .Include(projects => projects.Projects)
                              .ThenInclude(projStatus => projStatus.Status)

                       .Select(customer => new CustomerWithDetailsDto
                       {
                           CustomerId = customer.Customerid,
                           CustomerFirstName = customer.Firstname,
                           CustomerLastName = customer.Lastname,
                           CustomerCompanyName = string.IsNullOrEmpty(customer.Companyname)
                                         ? (customer.Firstname + " " + customer.Lastname)
                                         : customer.Companyname,
                           //CustomerCompanyLogoUrl = customer.Companylogourl,
                           CustomerCompanyLogoUrl = string.IsNullOrEmpty(customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{customer.Companylogourl}",
                           PAN = customer.Pannumber,
                           GST = customer.Gstnumber,
                           CustomerEmail = customer.Companyemail,
                           CustomerPhone1 = customer.Phone1 ?? "",
                           CustomerAddress1 = customer.Address1 ?? "",
                           ZipCode = customer.Zipcode ?? "",
                           PaymentTerm = customer.Paymentterm == null ? null : new PaymentTermDto
                           {
                               PaymentTermId = customer.Paymentterm.Paymenttermid,
                               PaymentTermName = customer.Paymentterm.Paymenttermname
                           },
                           Currency = customer.Currency == null ? null : new CurrencyDto
                           {
                               CurrencyId = customer.Currency.Currencyid,
                               CurrencyName = customer.Currency.Currencyname
                           },


                           City = customer.City == null ? null : new CityInfo
                           {
                               CityId = customer.City.Cityid,
                               CityName = customer.City.Cityname,
                               //State = customer.City.State == null ? null : new StateInfo
                               //{
                               //    StateId = customer.City.State.Stateid,
                               //    StateName = customer.City.State.Statename,
                               //    Country = customer.City.State.Country == null ? null : new CountryDto
                               //    {
                               //        CountryId = customer.City.State.Country.Countryid,
                               //        CountryName = customer.City.State.Country.Countryname
                               //    }
                               //}
                           },
                           TotalProjects = customer.Projects.Count(),
                           ProjectsInfo = customer.Projects.Select(proj => new ProjectInfoDto
                           {
                               ProjectId = proj.Projectid,
                               //ProjectName = $"" + (string.IsNullOrEmpty(proj.Projectname) ? customer.Companyname : proj.Projectname) +
                               //               "-" + customer.Projects.Select(prac => prac.Subpractice.Practice.Practicename),
                               ProjectName = proj.Projectname ?? "",
                               //TotalEmployees = proj.Projectemployeeassignments.Count(),
                               TotalEmployees = proj.Projectcontracts.SelectMany(a => a.Contractemployees)
                                .Count(b => (b.Categorysubstatusid == 5 || b.Categorysubstatusid == 6 || b.Categorysubstatusid == 4 || b.Categorysubstatusid == 3 || b.Categorysubstatusid == 2) && b.Contractid == proj.Projectcontracts
                                .Select(c => c.Contractid).FirstOrDefault()),
                               TotalAmount = proj.Projectcontracts.Sum(contract => contract.Amount),
                               //ProjectType = proj.Projecttype.Projecttypename,
                               //ProjrectSubPractice = proj.Subpractice.Subpracticename,
                               //ProjectPractice = proj.Subpractice.Practice.Practicename,
                               ProjectStatus = proj.Status.Statusname,
                               //ContractInfo = proj.Projectcontracts.Select(contract => new ContractDto
                               //{
                               //    StartDate = contract.Contractstartdate,
                               //    EndDate = contract.Contractenddate,
                               //    Amount = contract.Amount
                               //}).ToList()


                           }).ToList()
                       }).ToListAsync();

                return result;
                //return null;

            }
            catch (Exception e)
            {
                throw;

            }

        }

        public async Task<Customer> GetByName(string name)
        {
            return await _context.Customers.AsNoTracking().Where(c => c.Firstname == name).FirstOrDefaultAsync();
        }

        public async Task<CustomerWithDetailsDto> GetCustomerWithDetailsAsync(int CustomerId)
        {
            // Customer result=new Customer(); 



            var result = new CustomerWithDetailsDto();
            try
            {
                //result = await _context.Customers
                //                            .Include(c => c.Customercontactdetails)
                //                            .Include(x => x.City)
                //                                    .ThenInclude(city => city.State)
                //                                    .ThenInclude(state => state.Country)
                //                                    .FirstOrDefaultAsync(x => x.Customerid == CustomerId);
                //var respose = new CustomerResposeDto
                //{
                //    CustomerId=result.Customerid,
                //    CustomerTypeId=result.Customerid,
                //    CompanyName=result.Companyname,
                //    LastName=result.Lastname,
                //    FirstName=result.Firstname,
                //    CompanyLogoUrl=result.Companylogourl,
                //    CompanyEmail=result.Companyemail,
                //    Phone1 = result.Phone1,
                //    Phone2 = result.Phone2,
                //    PANNumber= result.Pannumber,
                //    CurrencyId=result.Currencyid,
                //    GSTNumber= result.Gstnumber,
                //    PaymentTermId=result.Paymenttermid,
                //    CityId=result.Cityid,
                //    StateId=result.City.State.Stateid,
                //    CountryId=result.City.State.Country.Countryid,
                //    ZipCode=result.Zipcode,
                //    Address1=result.Address1,   
                //    Address2=result.Address2,
                //    CustomerCode=result.Customercode,
                //    Contact=result.Contact,
                //    Amount = result.Amount,
                //    PaymentMethodId=result.Paymentmethodid,
                //};
                //return respose;
                result = await _context.Customers.AsNoTracking().Where(x => x.Isdeleted != true && x.Isactive != false)
                     .Include(currency => currency.Currency)
                     .Include(x => x.City)
                           .ThenInclude(city => city.State)
                           .ThenInclude(state => state.Country)
                     .Include(projects => projects.Projects)
                           .ThenInclude(empAssignment => empAssignment.Projectemployeeassignments)
                           .ThenInclude(emp => emp.Employee)
                     .Include(projects => projects.Projects)
                           .ThenInclude(contr => contr.Projectcontracts)
                     .Include(projects => projects.Projects)
                           .ThenInclude(subPrac => subPrac.Subpractice)
                                 .ThenInclude(prac => prac.Practice)
                     .Include(projects => projects.Projects)
                           .ThenInclude(projStatus => projStatus.Status)
                     .Select(customer => new CustomerWithDetailsDto
                     {
                         CustomerId = customer.Customerid,
                         CustomerFirstName = string.IsNullOrEmpty(customer.Firstname) ? customer.Companyname : customer.Firstname,
                         CustomerLastName = customer.Lastname,
                         CustomerCompanyName = string.IsNullOrEmpty(customer.Companyname)
                                       ? (customer.Firstname + " " + customer.Lastname)
                                       : customer.Companyname,
                         //CustomerCompanyLogoUrl = customer.Companylogourl,
                         CustomerCompanyLogoUrl = string.IsNullOrEmpty(customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{customer.Companylogourl}",

                         PAN = customer.Pannumber,
                         GST = customer.Gstnumber,
                         CustomerEmail = customer.Companyemail,
                         CustomerPhone1 = customer.Phone2 ?? "",
                         CustomerPhone2 = customer.Phone1 ?? "",
                         CustomerAddress1 = customer.Address1 ?? "",
                         CustomerAddress2 = customer.Address2 ?? "",
                         ZipCode = customer.Zipcode ?? "",
                         countryregioncode = customer.Countryregioncode ?? "",

                         PaymentTerm = customer.Paymentterm == null ? null : new PaymentTermDto
                         {
                             PaymentTermId = customer.Paymentterm.Paymenttermid,
                             PaymentTermName = customer.Paymentterm.Paymenttermname,
                         },
                         Currency = customer.Currency == null ? null : new CurrencyDto
                         {
                             CurrencyId = customer.Currency.Currencyid,
                             CurrencyName = customer.Currency.Currencyname
                         },

                         City = customer.City == null ? null : new CityInfo
                         {
                             CityId = customer.City.Cityid,
                             CityName = customer.City.Cityname,
                             State = customer.City.State == null ? null : new StateInfo
                             {
                                 StateId = customer.City.Stateid,
                                 StateName = customer.City.State.Statename,
                                 Country = customer.City.State.Country == null ? null : new CountryDto
                                 {
                                     CountryId = customer.City.State.Countryid,
                                     CountryName = customer.City.State.Country.Countryname,
                                 }
                             }

                         },
                         TotalProjects = customer.Projects.Count(),
                         ProjectsInfo = customer.Projects.Select(proj => new ProjectInfoDto
                         {
                             ProjectId = proj.Projectid,
                             ProjectName = proj.Projectname ?? "",
                             TotalEmployees = proj.Projectcontracts.SelectMany(a => a.Contractemployees)
                            .Count(b => (b.Categorysubstatusid == 5 || b.Categorysubstatusid == 6 || b.Categorysubstatusid == 4 || b.Categorysubstatusid == 3 || b.Categorysubstatusid == 2) && b.Contractid == proj.Projectcontracts
                            .Select(c => c.Contractid).FirstOrDefault()),
                             TotalAmount = proj.Projectcontracts.Sum(contract => contract.Amount),
                             ProjectType = proj.Projecttype.Projecttypename,
                             ProjrectSubPractice = proj.Subpractice.Subpracticename,
                             ProjectPractice = proj.Subpractice.Practice.Practicename,
                             ProjectStatus = proj.Status.Statusname,
                             ContractInfo = proj.Projectcontracts.Select(contract => new ContractDto
                             {
                                 StartDate = contract.Contractstartdate,
                                 EndDate = contract.Contractenddate,
                                 Amount = contract.Amount
                             }).ToList()


                         }).ToList(),
                         CustomerCode = customer.Customercode,
                     }).FirstOrDefaultAsync(x => x.CustomerId == CustomerId);

                return result;

            }
            catch (Exception e)
            {
                throw;

            };
        }

        private async Task<bool> IsOldImagesExists(int? id, string rootPath, string UserId)
        {
            List<string> errlist = new List<string>();
            var result = await _context.Customers.AsNoTracking().AsNoTracking().FirstOrDefaultAsync(x => x.Customerid == id);
            if (result != null)
            {
                var imageFilefordeletion = rootPath + result.Companylogourl;
                FileInfo file = new FileInfo(imageFilefordeletion);
                if (file.Exists)
                {
                    file.Delete();
                }
                return true;
            }

            else
                return false;
        }

        public async Task<Response> SoftDeleteCustomer(int customerId)
        {
            try
            {
                if (customerId <= 0)
                {
                    throw new Exception("CustomerId cannot be less than or equal to zero.");
                }
                var existingCustomer = await _context.Customers
                                                     .FirstOrDefaultAsync(x => x.Customerid == customerId);
                if (existingCustomer == null)
                {
                    throw new Exception($"Customer does not exist at the given Id :: {customerId}");
                }
                existingCustomer.Isdeleted = true;
                existingCustomer.Isactive = false;
                _context.Customers.Update(existingCustomer);
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

        //old caode till 18-01-2024
        //public async Task<List<CustomerWithDetailsDto>> GetAllCustomersByDA(int DeliveryAnchorId)
        //{
        //    var result = new List<CustomerWithDetailsDto>();
        //    try
        //    {
        //        var res = await _context.Customers.Where(x => x.Isdeleted != true && x.Isactive != false)
        //                .Include(currency => currency.Currency)
        //                .Include(paymentTe => paymentTe.Paymentterm)
        //                .Include(x => x.City)
        //                //.ThenInclude(city => city.State)
        //                //.ThenInclude(state => state.Country)
        //                .Include(projects => projects.Projects)
        //                      .ThenInclude(empAssignment => empAssignment.Projectemployeeassignments)
        //                      .ThenInclude(emp => emp.Employee)
        //                .Include(projects => projects.Projects)
        //                      .ThenInclude(contr => contr.Projectcontracts)
        //                //.Include(projects => projects.Projects)
        //                //      .ThenInclude(subPrac => subPrac.Subpractice)
        //                //            .ThenInclude(prac => prac.Practice)
        //                .Include(projects => projects.Projects)
        //                      .ThenInclude(projStatus => projStatus.Status)
        //                .Where(customer =>
        //                customer.Projects.Any(project =>
        //                    project.Projectcontracts.Any(contract =>
        //                        contract.Deliveryanchorid == DeliveryAnchorId)))
        //            .ToListAsync();

        //        return res.Select(customer => new CustomerWithDetailsDto
        //              {
        //                  CustomerId = customer.Customerid,
        //                  CustomerFirstName = customer.Firstname,
        //                  CustomerLastName = customer.Lastname,
        //                  CustomerCompanyName = string.IsNullOrEmpty(customer.Companyname)
        //                                 ? (customer.Firstname + " " + customer.Lastname)
        //                                 : customer.Companyname,
        //                  //CustomerCompanyLogoUrl = customer.Companylogourl,
        //                  CustomerCompanyLogoUrl = string.IsNullOrEmpty(customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{customer.Companylogourl}",
        //                  PAN = customer.Pannumber,
        //                  GST = customer.Gstnumber,
        //                  CustomerEmail = customer.Companyemail,
        //                  CustomerPhone1 = customer.Phone1 ?? "",
        //                  CustomerAddress1 = customer.Address1 ?? "",
        //                  ZipCode = customer.Zipcode ?? "",
        //                  PaymentTerm = customer.Paymentterm == null ? null : new PaymentTermDto
        //                  {
        //                      PaymentTermId = customer.Paymentterm.Paymenttermid,
        //                      PaymentTermName = customer.Paymentterm.Paymenttermname
        //                  },
        //                  Currency = customer.Currency == null ? null : new CurrencyDto
        //                  {
        //                      CurrencyId = customer.Currency.Currencyid,
        //                      CurrencyName = customer.Currency.Currencyname
        //                  },
        //                  City = customer.City == null ? null : new CityInfo
        //                  {
        //                      CityId = customer.City.Cityid,
        //                      CityName = customer.City.Cityname,
        //                      //State = customer.City.State == null ? null : new StateInfo
        //                      //{
        //                      //    StateId = customer.City.State.Stateid,
        //                      //    StateName = customer.City.State.Statename,
        //                      //    Country = customer.City.State.Country == null ? null : new CountryDto
        //                      //    {
        //                      //        CountryId = customer.City.State.Country.Countryid,
        //                      //        CountryName = customer.City.State.Country.Countryname
        //                      //    }
        //                      //}
        //                  },
        //                  TotalProjects = customer.Projects.Count(),
        //                  ProjectsInfo = customer.Projects.Select(proj => new ProjectInfoDto
        //                  {
        //                      ProjectId = proj.Projectid,
        //                      //ProjectName = $"" + (string.IsNullOrEmpty(proj.Projectname) ? customer.Companyname : proj.Projectname) +
        //                      //               "-" + customer.Projects.Select(prac => prac.Subpractice.Practice.Practicename),
        //                      ProjectName = proj.Projectname ?? "",
        //                      //TotalEmployees = proj.Projectemployeeassignments.Count(),
        //                      TotalEmployees = proj.Projectcontracts.SelectMany(a => a.Contractemployees)
        //                    .Count(b => (b.Categorysubstatusid == 5 || b.Categorysubstatusid == 6 || b.Categorysubstatusid == 4 || b.Categorysubstatusid == 3 || b.Categorysubstatusid == 2) && b.Contractid == proj.Projectcontracts
        //                    .Select(c => c.Contractid).FirstOrDefault()),
        //                      TotalAmount = proj.Projectcontracts.Sum(contract => contract.Amount),
        //                      //ProjectType = proj.Projecttype.Projecttypename,
        //                      //ProjrectSubPractice = proj.Subpractice.Subpracticename,
        //                      //ProjectPractice = proj.Subpractice.Practice.Practicename,
        //                      ProjectStatus = proj.Status.Statusname,
        //                      //ContractInfo = proj.Projectcontracts.Select(contract => new ContractDto
        //                      //{
        //                      //    StartDate = contract.Contractstartdate,
        //                      //    EndDate = contract.Contractenddate,
        //                      //    Amount = contract.Amount
        //                      //}).ToList()


        //                  }).ToList()
        //              }).ToList();
        //    }
        //    catch (Exception e)
        //    {
        //        throw;

        //    }

        //}

        public async Task<List<CustomerWithDetailsDto>> GetAllCustomersByDA(int DeliveryAnchorId)
        {

            var result = new List<CustomerWithDetailsDto>();
            try
            {

                result = await _context.Customers.AsNoTracking().Where(x => x.Isdeleted != true && x.Isactive != false)
                .Where(customer => customer.Projects.Any(project => project.Projectcontracts.Any(contract =>
                          contract.Deliveryanchorid == DeliveryAnchorId)))
                        .Include(currency => currency.Currency)
                        .Include(paymentTe => paymentTe.Paymentterm)
                        .Include(x => x.City)

                        .Include(projects => projects.Projects)
                              .ThenInclude(empAssignment => empAssignment.Projectemployeeassignments)
                              .ThenInclude(emp => emp.Employee)

                        .Include(projects => projects.Projects)
                              .ThenInclude(projStatus => projStatus.Status)

                       .Select(customer => new CustomerWithDetailsDto
                       {
                           CustomerId = customer.Customerid,
                           CustomerFirstName = customer.Firstname,
                           CustomerLastName = customer.Lastname ?? "",
                           CustomerCompanyName = string.IsNullOrEmpty(customer.Companyname)
                                         ? (customer.Firstname + " " + customer.Lastname)
                                         : customer.Companyname,
                           //CustomerCompanyLogoUrl = customer.Companylogourl,
                           CustomerCompanyLogoUrl = string.IsNullOrEmpty(customer.Companylogourl) ? "" : $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{customer.Companylogourl}",
                           PAN = customer.Pannumber ?? "",
                           GST = customer.Gstnumber ?? "",
                           CustomerEmail = customer.Companyemail ?? "",
                           CustomerPhone1 = customer.Phone1 ?? "",
                           CustomerAddress1 = customer.Address1 ?? "",
                           ZipCode = customer.Zipcode ?? "",
                           PaymentTerm = customer.Paymentterm == null ? null : new PaymentTermDto
                           {
                               PaymentTermId = customer.Paymentterm.Paymenttermid,
                               PaymentTermName = customer.Paymentterm.Paymenttermname
                           },
                           Currency = customer.Currency == null ? null : new CurrencyDto
                           {
                               CurrencyId = customer.Currency.Currencyid,
                               CurrencyName = customer.Currency.Currencyname
                           },

                          
                           City = customer.City == null ? null : new CityInfo
                           {
                               CityId = customer.City.Cityid,
                               CityName = customer.City.Cityname,
                               //State = customer.City.State == null ? null : new StateInfo
                               //{
                               //    StateId = customer.City.State.Stateid,
                               //    StateName = customer.City.State.Statename,
                               //    Country = customer.City.State.Country == null ? null : new CountryDto
                               //    {
                               //        CountryId = customer.City.State.Country.Countryid,
                               //        CountryName = customer.City.State.Country.Countryname
                               //    }
                               //}
                           },
                           TotalProjects = customer.Projects.Count(),
                           ProjectsInfo = customer.Projects.Select(proj => new ProjectInfoDto
                           {
                               ProjectId = proj.Projectid,
                               //ProjectName = $"" + (string.IsNullOrEmpty(proj.Projectname) ? customer.Companyname : proj.Projectname) +
                               //               "-" + customer.Projects.Select(prac => prac.Subpractice.Practice.Practicename),
                               ProjectName = proj.Projectname ?? "",
                               //TotalEmployees = proj.Projectemployeeassignments.Count(),
                               TotalEmployees = proj.Projectcontracts.SelectMany(a => a.Contractemployees)
                                .Count(b => (b.Categorysubstatusid == 5 || b.Categorysubstatusid == 6 || b.Categorysubstatusid == 4 || b.Categorysubstatusid == 3 || b.Categorysubstatusid == 2) && b.Contractid == proj.Projectcontracts
                                .Select(c => c.Contractid).FirstOrDefault()),
                               TotalAmount = proj.Projectcontracts.Sum(contract => contract.Amount) ?? 0,
                               //ProjectType = proj.Projecttype.Projecttypename,
                               //ProjrectSubPractice = proj.Subpractice.Subpracticename,
                               //ProjectPractice = proj.Subpractice.Practice.Practicename,
                               ProjectStatus = proj.Status.Statusname ?? "",
                               //ContractInfo = proj.Projectcontracts.Select(contract => new ContractDto
                               //{
                               //    StartDate = contract.Contractstartdate,
                               //    EndDate = contract.Contractenddate,
                               //    Amount = contract.Amount
                               //}).ToList()


                           }).ToList()
                       }).ToListAsync();

                return result;
                //return null;

            }
            catch (Exception e)
            {
                throw;

            }
        }
        public async Task<List<CustomerWithDetailsDto>> GetAllCustomersWithIdAsync(int employeeId)
        {
            var result = new List<CustomerWithDetailsDto>();
            try
            {
                result = await _context.Customers.AsNoTracking()
                        .Include(projects => projects.Projects)
                              .ThenInclude(proj => proj.Projectcontracts)
                              .ThenInclude(contract => contract.Contractemployees)
                              .Where(customer => customer.Projects.Any(proj =>
                              proj.Projectcontracts.Any(contract =>
                              contract.Contractemployees.Any(emp => emp.Employeeid == employeeId) &&
                              contract.Statusid != 6)))

                       .Select(customer => new CustomerWithDetailsDto
                       {
                           CustomerId = customer.Customerid,
                           CustomerCompanyName = string.IsNullOrEmpty(customer.Companyname)
                                         ? (customer.Firstname + " " + customer.Lastname)
                                         : customer.Companyname,

                       }).ToListAsync();

                return result;

            }
            catch (Exception e)
            {
                throw;

            }

        }
    }
}
