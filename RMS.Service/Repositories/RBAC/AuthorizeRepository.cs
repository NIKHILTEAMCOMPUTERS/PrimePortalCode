using Microsoft.EntityFrameworkCore;
using RMS.Data.Models;
using RMS.Entity.DTO;
using RMS.Service.Interfaces.RBAC;

namespace RMS.Service.Repositories.RBAC
{
    public class AuthorizeRepository : GenericRepository<Employeerole>, IAuthorizeRepository
    {
        private readonly RmsDevContext _context;
        public AuthorizeRepository(RmsDevContext context): base(context)
        {
            _context = context;
        }

        public async Task<AuthorizeDto> Get(string employeeCode)
        {
            AuthorizeDto authorizeDtos = new AuthorizeDto();
            var res = await _context.Employeeroles
                .Include(c => c.Employee)
                .Include(c => c.Role).ThenInclude(c => c.Rolepages).ThenInclude(x => x.Page).ThenInclude(x => x.Module)
                .Where(c => c.Employee.Userid == employeeCode)
                .ToListAsync();

            if (res != null)
            {
                var AuthorizeRoles = new List<AuthorizeRoles>();
                foreach (var item in res)
                {
                    var authorizeRoles = new AuthorizeRoles();
                    authorizeRoles.RoleName = item.Role.Rolename;
                    List<AuthorizePages> authPages = new List<AuthorizePages>();

                    if (item.Role.Isadmin == true)
                    {
                        var adminPages = _context.Pages.AsNoTracking().Include(c => c.Module).Include(x=>x.Rolepages).Where(c => c.Isactive == true ).ToList();
                        foreach (var page in adminPages)
                        {
                            var authorizePages = new AuthorizePages();
                            authorizePages.ModuleName = page.Module == null ? null : page.Module.Modulename;
                            authorizePages.ModuleIcon = page.Module == null ? null : page.Module.Icon;
                            authorizePages.PageName = page.Pagename;
                            authorizePages.Controller = page.Controllername;
                            authorizePages.Action = page.Actionname;
                            authorizePages.PageIcon = page.Icon;
                            authorizePages.Isreadpermit = true;
                            authorizePages.Iswritepermit = true;
                            authorizePages.Isdeletepermit = true;
                            authorizePages.Isbillingpermit = true;
                            authorizePages.Isrowlevelpermit = item.Role.Isrowlevel;
                            authorizePages.Pagesequence = page.Rolepages.Where(x => x.Pageid == page.Pageid).FirstOrDefault().Pagesequence;
                            authPages.Add(authorizePages);
                        }

                        authorizeRoles.AuthorizePages = authPages;
                        AuthorizeRoles.Add(authorizeRoles);
                        break;
                    }

                    foreach (var pages in item.Role.Rolepages.Where(x=>x.Page.Isactive==true))
                    {
                        
                        if (pages.Page != null)
                        {
                            var authorizePages = new AuthorizePages();
                            authorizePages.ModuleName = pages.Page.Module == null ? null : pages.Page.Module.Modulename;
                            authorizePages.ModuleIcon = pages.Page.Module == null ? null : pages.Page.Module.Icon;
                            authorizePages.PageName = pages.Page.Pagename;
                            authorizePages.Controller = pages.Page.Controllername;
                            authorizePages.Action = pages.Page.Actionname;
                            authorizePages.PageIcon = pages.Page.Icon;
                            authorizePages.Isreadpermit = pages.Isreadpermit;
                            authorizePages.Iswritepermit = pages.Iswritepermit;
                            authorizePages.Isdeletepermit = pages.Isdeletepermit;
                            authorizePages.Isbillingpermit = pages.Isbillingpermit;
                            authorizePages.Isrowlevelpermit = item.Role.Isrowlevel;
                            authorizePages.Pagesequence = pages.Pagesequence;   
                            authPages.Add(authorizePages);
                        }
                    }

                    authorizeRoles.AuthorizePages = authPages;
                    AuthorizeRoles.Add(authorizeRoles);
                }

                authorizeDtos.AuthorizeRoles = AuthorizeRoles;
            }

            return authorizeDtos;
        }
    }
}