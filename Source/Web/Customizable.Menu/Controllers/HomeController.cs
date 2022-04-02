using ClassLibrary.Data;
using ClassLibrary.Mvc.Http;
using ClassLibrary.Mvc.Services.AppSettings;
using Customizable.Menu.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Customizable.Menu.Controllers
{
    public class HomeController : ApplicationBaseController<HomeController>
    {
        public HomeController(
            ILogger<HomeController> logger,
            IAppSettingsService appSettingsService,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext
        ) : base(logger, appSettingsService, httpContextAccessor, dbContext)
        {
        }

        [HttpGet]
        public IActionResult Index(Guid Id)
        {
            this.LoadMenu(Id, false);
            ApplicationCookie cookie = new(_httpContextAccessor, _encryptionKey);
            cookie.SetValue("session", "menuGuid", ViewBag.MenuGuid.ToString());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetSiteList(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                model.Menus = _dbContext.SortedMenuListNoTracking()
                    .Where(menu => menu.Guid == model.MenuGuid).ToList();

                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
                return Json(model, options);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }

        }
    }
}
