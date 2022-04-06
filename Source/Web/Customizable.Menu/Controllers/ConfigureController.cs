using ClassLibrary.Data;
using ClassLibrary.Mvc.Http;
using ClassLibrary.Mvc.Services.AppSettings;
using Customizable.Menu.Models.Configure;
using Microsoft.AspNetCore.Mvc;
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
            this.LoadMenu(Guid.Empty, true);
            ApplicationCookie cookie = new(_httpContextAccessor, _encryptionKey);
            cookie.SetValue("session", "menuGuid", ViewBag.MenuGuid.ToString());

            return View();
        }

        [HttpGet]
        public IActionResult GetMenuList()
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                IndexViewModel model = new();
                model.Menus = _dbContext.SortedMenuListNoTracking().ToList();
                model.MaxMenuOrdinal = _dbContext.MaxMenuOrdinal();

                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles  };
                return Json(model, options) ;
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ItemAdd(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                _dbContext.AddItem(model.EntityType, model.Description.Clean(), model.Guid, model.EnvironmentType);
                
                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
                return Json(Ok(), options);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ItemDelete(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                _dbContext.DeleteItem(model.EntityType, model.Guid);

                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
                return Json(Ok(), options);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ItemUpdate(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                _dbContext.UpdateItem(model.EntityType, model.Guid, model.Description.Clean(), model.EnvironmentType);

                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
                return Json(Ok(), options);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ItemUp(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                _dbContext.MoveUp(model.EntityType, model.Guid);

                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
                return Json(Ok(), options);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ItemDown(IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                _dbContext.MoveDown(model.EntityType, model.Guid);

                JsonSerializerOptions options = new() { ReferenceHandler = ReferenceHandler.IgnoreCycles };
                return Json(Ok(), options);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
    }
}
