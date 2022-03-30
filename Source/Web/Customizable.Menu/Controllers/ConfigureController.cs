using ClassLibrary.Data;
using ClassLibrary.Mvc.Services.AppSettings;
using Customizable.Menu.Models.Configure;
using Microsoft.AspNetCore.Mvc;

namespace Customizable.Menu.Controllers
{
    public class ConfigureController : ApplicationBaseController<ConfigureController>
    {
        public ConfigureController(
            ILogger<ConfigureController> logger,
            IAppSettingsService appSettingsService,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext
        ) : base(logger, appSettingsService, httpContextAccessor, dbContext)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
