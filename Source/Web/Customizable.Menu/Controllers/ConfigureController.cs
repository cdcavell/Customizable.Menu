using ClassLibrary.Data;
using ClassLibrary.Mvc.Services.AppSettings;
using Customizable.Menu.Models.Configure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetMenuList(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                model.Menus = _dbContext.SortedMenuListNoTracking().ToList();

                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles  };
                return Json(model, options) ;
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
            
        }
    }
}
