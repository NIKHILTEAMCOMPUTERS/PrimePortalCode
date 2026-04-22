using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using RMS.Client.Utility;

namespace RMS.Client.Controllers.Account
{
    public class ProfileController : BaseController
    {
        private ApiManager apiManager;
        private IConfiguration _configuration;
        private IsoDateTimeConverter dateTimeConverter;
        public ProfileController(IConfiguration configuration, IWebHostEnvironment env)
              : base(configuration, env)
        {
            _configuration = configuration;
            dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
