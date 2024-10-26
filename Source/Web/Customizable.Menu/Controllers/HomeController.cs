﻿using ClassLibrary.Data;
using ClassLibrary.Mvc.Http;
using ClassLibrary.Mvc.Services.AppSettings;
using Customizable.Menu.Models.Home;
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
        public IActionResult Index(Guid guid)
        {
            if (!_dbContext.HasAnyLinks())
                return RedirectToAction("Index", "Configure");

            this.LoadMenu(guid, false);
            ApplicationCookie cookie = new(_httpContextAccessor, _encryptionKey);
            cookie.SetValue("session", "menuGuid", ViewBag.MenuGuid.ToString());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetSiteList([Bind(IndexViewModel.BindProperty)] IndexViewModel model)
        {
            if (!ModelState.IsValid)
                return InvalidModelState();

            try
            {
                model.Menus = _dbContext.SortedMenuList()
                    .Where(menu => menu.Guid == model.Guid).ToList();

                model.Menus.ForEach(menu => menu.Sites.RemoveAll(site => site.Urls.Count == 0));

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
