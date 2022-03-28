using ClassLibrary.Data;
using ClassLibrary.Mvc.Controllers;
using ClassLibrary.Mvc.Http;
using ClassLibrary.Mvc.Services.AppSettings;
using Customizable.Menu.Filters;
using Customizable.Menu.Models.AppSettings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Customizable.Menu.Controllers
{
    [ServiceFilter(typeof(SecurityFilterAttribute))]
    public abstract partial class ApplicationBaseController<T> : WebBaseController<ApplicationBaseController<T>> where T : ApplicationBaseController<T>
    {
        protected readonly AppSettings _appSettings;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ApplicationDbContext _dbContext;

        protected ApplicationBaseController(
            ILogger<T> logger,
            IAppSettingsService appSettingsService,
            IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext dbContext
        ) : base(logger)
        {
            _appSettings = appSettingsService.ToObject<AppSettings>();
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            byte[] encryptionKey = _dbContext.Configuration.Select(x => x.EncryptionKey).FirstOrDefault()!.ToArray();
            ApplicationCookie cookie = new(_httpContextAccessor, encryptionKey);

            Guid menuId = Guid.Empty;
            try
            {
                menuId = Guid.Parse(cookie.GetValue("session", "menuId"));
            }
            catch (ArgumentNullException) { }
            catch (FormatException) { }

            this.LoadMenu(menuId);
        }

        protected void LoadMenu(Guid menuId)
        {
            List<ClassLibrary.Data.Models.Menu> menuItems = ClassLibrary.Data.Models.Menu.List(_dbContext);

            if (menuId == Guid.Empty)
                menuId = menuItems.Select(x => x.Guid).FirstOrDefault();

            ViewBag.MenuId = menuId;
            ViewBag.MenuTitle = menuItems.Where(x => x.Guid == menuId).Select(x => x.Description).FirstOrDefault();
            ViewBag.MenuItems = menuItems.Where(x => x.Guid != menuId).ToList();

            _logger.LogDebug("Selected Menu Id: {menuId}", menuId);
        }
    }
}
