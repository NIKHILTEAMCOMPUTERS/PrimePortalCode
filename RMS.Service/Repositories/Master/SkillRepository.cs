using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.Master;
using System.Diagnostics;
using System.Transactions;

namespace RMS.Service.Repositories.Master
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        private readonly RmsDevContext _context;
        public SkillRepository(RmsDevContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Response> AddSkill(SkillRequestDto obj)
        {
            try 
            {
                var result = await _context.Database.ExecuteSqlRawAsync("SELECT insert_user_Skill(@p0, @p1, @p2)", obj.SkillName,obj.SkillId,obj.ActionType);
                if (result == 1) { return new Response { responseCode = 200, responseMessage = "Record Inserted Succesfully" }; }
                else return new Response { responseCode = 200, responseMessage = "Record Inserted Succesfully" };

            }
            catch 
            {
                throw;
            }
        }

        public async Task<Response> DeleteSkill(SkillRequestDto obj)
        {

            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync("SELECT insert_user_Skill(@p0, @p1, @p2)", obj.SkillName, obj.SkillId, obj.ActionType);
                if (result == 1) { return new Response { responseCode = 200, responseMessage = "Record Delete Succesfully" }; }
                else return new Response { responseCode = 200, responseMessage = "Record Inserted Succesfully" };

            }
            catch
            {
                throw;
            }
        }

        public async Task<Skill> GetByName(string name)
        {
            return await _context.Skills.AsNoTracking().Where(c=>c.Skillname == name).FirstOrDefaultAsync();
        }

        public async Task<Response> UpdateSkill(SkillRequestDto obj)
        {

            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync("SELECT insert_user_Skill(@p0, @p1, @p2)", obj.SkillName, obj.SkillId, obj.ActionType);
                if (result == 1) { return new Response { responseCode = 200, responseMessage = "Record Updated Succesfully" }; }
                else return new Response { responseCode = 200, responseMessage = "Record Inserted Succesfully" };

            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Skill>> GetSkills()
        {
            try
            {
                var result=await _context.Skills.AsNoTracking()
                                          .Include(c=>c.Skillcostings)
                                          .ToListAsync();
                return result;

            }
            catch
            {
                throw;
            }
        }
        public async Task<Skill> GetSkillById(int id)
        {
            try
            {
                var result = await _context.Skills.AsNoTracking()
                                          .Include(c => c.Skillcostings)
                                          .FirstOrDefaultAsync(x=>x.Skillid==id);
                return result;

            }
            catch
            {
                throw;
            }
        }
        public async Task<Response> UpsertAssignEmployeeSkill(List<EmployeeskillDto> value, JwtLoginDetailDto logindetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var createdBy = await _context.Rmsemployees.AsNoTracking()
                                     .Where(x => x.Userid == logindetails.TmcId)
                                     .Select(x => x.Employeeid)
                                     .FirstOrDefaultAsync();

                if (createdBy == null)
                {
                    return new Response { responseCode = 404, responseMessage = "User not found." };
                }

                var newSkills = new List<Employeeskill>();
                var updatedSkills = new List<Employeeskill>();

                foreach (var item in value)
                {
                    if (item.Employeeskillid <= 0 || item.Employeeskillid == null)
                    {
                        newSkills.Add(new Employeeskill
                        {
                            Skillid = item.Skillid,
                            Employeeid = item.Employeeid,
                            Experinceinmonths = item.Experinceinmonths,
                            Certificationurl = item.Certificationurl,
                            Isprimary = item.Isprimary ?? false,
                            Managerrating = item.Managerrating ?? 0,
                            Selfreting = item.Selfreting ?? 0,
                            Createdby = createdBy,
                            Createddate = DateTime.Now,
                        });
                    }
                    else
                    {
                        var existingEmployeeSkill = await _context.Employeeskills
                                                         .FirstOrDefaultAsync(x => x.Employeeskillid == item.Employeeskillid);

                        if (existingEmployeeSkill == null)
                        {
                            return new Response { responseMessage = "Employee skill not found", responseCode = 404 };
                        }

                        existingEmployeeSkill.Skillid = item.Skillid;
                        existingEmployeeSkill.Experinceinmonths = item.Experinceinmonths?? existingEmployeeSkill.Experinceinmonths;
                        existingEmployeeSkill.Certificationurl = item.Certificationurl?? existingEmployeeSkill.Certificationurl;
                        existingEmployeeSkill.Isprimary = item.Isprimary ?? false;
                        existingEmployeeSkill.Managerrating = item.Managerrating ?? (existingEmployeeSkill.Managerrating??0);
                        existingEmployeeSkill.Selfreting = item.Selfreting ?? (existingEmployeeSkill.Selfreting??0);
                        existingEmployeeSkill.Lastupdatedate = DateTime.Now;
                        existingEmployeeSkill.Lastupdateby = createdBy;
                        updatedSkills.Add(existingEmployeeSkill);
                    }
                }

                if (newSkills.Count > 0)
                {
                    await _context.Employeeskills.AddRangeAsync(newSkills);
                }

                if (updatedSkills.Count > 0)
                {
                    _context.Employeeskills.UpdateRange(updatedSkills);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Response { responseCode = 200, responseMessage = "Data processed successfully!" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
               
                return new Response { responseCode = 500, responseMessage = "An error occurred while processing the data." };
            }
        }



        public async Task<Response> DeleteEmployeeSkill(int id, JwtLoginDetailDto logindetails)
        {
            try
            {
                var existingSkills = await _context.Employeeskills.Where(x=>x.Employeeskillid==id).FirstOrDefaultAsync();
                _context.Employeeskills.Remove(existingSkills);
                _context.SaveChanges();
                return new Response() { responseCode = 200, responseMessage = "Deleted Succesfully" };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
