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
        protected readonly byte[] _encryptionKey;

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
            _encryptionKey = _dbContext.EncryptionKey();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ApplicationCookie cookie = new(_httpContextAccessor, _encryptionKey);

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
            List<ClassLibrary.Data.Models.Menu> menuItems = _dbContext
                .SortedMenuListNoTracking().ToList();

            if (menuId == Guid.Empty)
                menuId = menuItems.OrderBy(menu => menu.Ordinal)
                    .Select(menu => menu.Guid).FirstOrDefault();

            ViewBag.MenuId = menuId;
            ViewBag.MenuTitle = menuItems
                .Where(menu => menu.Guid == menuId)
                .OrderBy(menu => menu.Ordinal)
                .Select(menu => menu.Description)
                .FirstOrDefault();
            ViewBag.MenuItems = menuItems.Where(menu => menu.Guid != menuId).ToList();

            _logger.LogDebug("Selected Menu Id: {menuId}", menuId);
        }
    }
}
