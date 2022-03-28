using ClassLibrary.Data;
using ClassLibrary.Mvc.Http;
using ClassLibrary.Mvc.Services.AppSettings;
using Microsoft.AspNetCore.Mvc;

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
            this.LoadMenu(Id);

            byte[] encryptionKey = _dbContext.Configuration.Select(x => x.EncryptionKey).FirstOrDefault()!.ToArray();
            ApplicationCookie cookie = new(_httpContextAccessor, encryptionKey);
            cookie.SetValue("session", "menuId", ViewBag.MenuId.ToString());

            return View();
        }
    }
}
