using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace System.Linq
{
    public static class IQueryableExtensions
    {
        public static byte[] EncryptionKey(this ApplicationDbContext dbContext)
        {
            return dbContext.Configuration
               .OrderBy(config => config.Guid).Take(1)
               .Select(config => config.EncryptionKey)
               .Single().ToArray();
        }

        public static IQueryable<Menu> SortedMenuList(this ApplicationDbContext dbContext)
        {
            return dbContext.Menu
                .Include(menu => menu.Sites.OrderBy(site => site.Ordinal))
                .ThenInclude(site => site.Urls.OrderBy(url => url.Environment))
                .OrderBy(menu => menu.Ordinal);
        }

        public static IQueryable<Menu> SortedMenuListNoTracking(this ApplicationDbContext dbContext)
        {
            return dbContext.SortedMenuList()
                .AsNoTrackingWithIdentityResolution();
        }

        public static bool HasAnyLinks(this ApplicationDbContext dbContext)
        {
            long count = dbContext.SortedMenuListNoTracking()
                .Select(menu => menu.Sites).FirstOrDefault()!
                .Select(site => site.Urls).FirstOrDefault()!
                .Select(url => url.Link)
                .Where(link => !string.IsNullOrEmpty(link))
                .LongCount();

            if (count > 0)
                return true;

            return false;
        }

        public static Menu MenuItem(this ApplicationDbContext dbContext, Guid guid)
        {
            return dbContext.Menu
                .Include(menu => menu.Sites.OrderBy(site => site.Ordinal))
                .ThenInclude(site => site.Urls.OrderBy(url => url.Environment))
                .Where(menu => menu.Guid == guid)
                .FirstOrDefault() ?? new();
        }

        public static Menu MenuItemNoTracking(this ApplicationDbContext dbContext, Guid guid)
        {
            return dbContext.Menu.AsNoTrackingWithIdentityResolution()
                .Include(menu => menu.Sites.OrderBy(site => site.Ordinal))
                .ThenInclude(site => site.Urls.OrderBy(url => url.Environment))
                .Where(menu => menu.Guid == guid)
                .FirstOrDefault() ?? new();
        }

        public static Site SiteItem(this ApplicationDbContext dbContext, Guid guid)
        {
            return dbContext.Site
                .Include(site => site.Urls.OrderBy(url => url.Environment))
                .Where(site => site.Guid == guid)
                .FirstOrDefault() ?? new();
        }

        public static Site SiteItemNoTracking(this ApplicationDbContext dbContext, Guid guid)
        {
            return dbContext.Site.AsNoTrackingWithIdentityResolution()
                .Include(site => site.Urls.OrderBy(url => url.Environment))
                .Where(site => site.Guid == guid)
                .FirstOrDefault() ?? new();
        }

        public static Url UrlItem(this ApplicationDbContext dbContext, Guid guid)
        {
            return dbContext.Url
                .Where(url => url.Guid == guid)
                .FirstOrDefault() ?? new();
        }

        public static Url UrlItemNoTracking(this ApplicationDbContext dbContext, Guid guid)
        {
            return dbContext.Url.AsNoTrackingWithIdentityResolution()
                .Where(url => url.Guid == guid)
                .FirstOrDefault() ?? new();
        }


        public static void DeleteItem(this ApplicationDbContext dbContext, EntityTypes entityType, Guid guid)
        {
            IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            using (dbContextTransaction)
            {
                try
                {
                    switch (entityType)
                    {
                        case EntityTypes.Menu:
                            ClassLibrary.Data.Models.Menu menu = dbContext.MenuItem(guid);
                            menu.Delete(dbContext);
                            break;
                        case EntityTypes.Site:
                            Site site = dbContext.SiteItem(guid);
                            site.Delete(dbContext);
                            break;
                        case EntityTypes.Url:
                            Url url = dbContext.UrlItem(guid);
                            url.Delete(dbContext);
                            break;
                        default:
                            throw new ArgumentException($"Invalid Entity Type: {entityType}");
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public static void UpdateItem(this ApplicationDbContext dbContext, EntityTypes entityType, Guid guid, string description, EnvironmentTypes? environment)
        {
            IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            using (dbContextTransaction)
            {
                try
                {
                    switch (entityType)
                    {
                        case EntityTypes.Menu:
                            ClassLibrary.Data.Models.Menu menu = dbContext.MenuItem(guid);
                            menu.Description = description.Clean();
                            menu.AddUpdate(dbContext);
                            break;
                        case EntityTypes.Site:
                            Site site = dbContext.SiteItem(guid);
                            site.Description = description.Clean();
                            site.AddUpdate(dbContext);
                            break;
                        case EntityTypes.Url:
                            if (environment.HasValue)
                                if (!Enum.IsDefined(typeof(EnvironmentTypes), environment))
                                {
                                    Url url = dbContext.UrlItem(guid);
                                    url.Link = description.Clean();
                                    url.Environment = (EnvironmentTypes)environment;
                                    url.AddUpdate(dbContext);
                                    break;
                                }
                            throw new ArgumentException($"Invalid Environment Type: {environment}");
                        default:
                            throw new ArgumentException($"Invalid Entity Type: {entityType}");
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        public static void MoveUp(this ApplicationDbContext dbContext, EntityTypes entityType, Guid guid)
        {
            IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            using (dbContextTransaction)
            {
                try
                {
                    switch (entityType)
                    {
                        case EntityTypes.Menu:
                            MoveMenuUp(dbContext, guid);
                            break;
                        case EntityTypes.Site:
                            MoveSiteUp(dbContext, guid);
                            break;
                        case EntityTypes.Url:
                            MoveUrlUp(dbContext, guid);
                            break;
                        default:
                            throw new ArgumentException($"Invalid Entity Type: {entityType}");
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        private static void MoveMenuUp(this ApplicationDbContext dbContext, Guid guid)
        {
            Menu? menuA = dbContext.Menu
                .Where(menu => menu.Guid == guid)
                .FirstOrDefault();

            if ((menuA != null) && (menuA.Guid != Guid.Empty))
            {
                Menu? menuB = dbContext.Menu
                    .Where(menu => menu.Ordinal < menuA.Ordinal)
                    .OrderByDescending(menu => menu.Ordinal)
                    .FirstOrDefault();

                if ((menuB != null) && (menuB.Guid != Guid.Empty))
                {
                    short newOrdinalA = menuB.Ordinal;
                    short newOrdinalB = menuA.Ordinal;

                    menuA.Ordinal= newOrdinalA;
                    menuA.AddUpdate(dbContext);

                    menuB.Ordinal = newOrdinalB;
                    menuB.AddUpdate(dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        private static void MoveSiteUp(this ApplicationDbContext dbContext, Guid guid)
        {
            Site? siteA = dbContext.Site
                .Where(site => site.Guid == guid)
                .FirstOrDefault();

            if ((siteA != null) && (siteA.Guid != Guid.Empty))
            {
                Site? siteB = dbContext.Site
                    .Where(site => site.MenuGuid == siteA.MenuGuid)
                    .Where(site => site.Ordinal < siteA.Ordinal)
                    .OrderByDescending(site => site.Ordinal)
                    .FirstOrDefault();

                if ((siteB != null) && (siteB.Guid != Guid.Empty))
                {
                    short newOrdinalA = siteB.Ordinal;
                    short newOrdinalB = siteA.Ordinal;

                    siteA.Ordinal = newOrdinalA;
                    siteA.AddUpdate(dbContext);

                    siteB.Ordinal = newOrdinalB;
                    siteB.AddUpdate(dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        private static void MoveUrlUp(this ApplicationDbContext dbContext, Guid guid)
        {
            Url? urlA = dbContext.Url
                .Where(url => url.Guid == guid)
                .FirstOrDefault();

            if ((urlA != null) && (urlA.Guid != Guid.Empty))
            {
                Url? urlB = dbContext.Url
                    .Where(url => url.SiteGuid == urlA.SiteGuid)
                    .Where(url => url.Environment < urlA.Environment)
                    .OrderByDescending(url => url.Environment) 
                    .FirstOrDefault();

                if ((urlB != null) && (urlB.Guid != Guid.Empty))
                {
                    EnvironmentTypes newEnvironmentA = urlB.Environment;
                    EnvironmentTypes newEnvironmentB = urlA.Environment;

                    urlA.Environment = newEnvironmentA;
                    urlA.AddUpdate(dbContext);

                    urlB.Environment = newEnvironmentB;
                    urlB.AddUpdate(dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        public static void MoveDown(this ApplicationDbContext dbContext, EntityTypes entityType, Guid guid)
        {
            IDbContextTransaction dbContextTransaction = dbContext.Database.BeginTransaction();
            using (dbContextTransaction)
            {
                try
                {
                    switch (entityType)
                    {
                        case EntityTypes.Menu:
                            MoveMenuDown(dbContext, guid);
                            break;
                        case EntityTypes.Site:
                            MoveSiteDown(dbContext, guid);
                            break;
                        case EntityTypes.Url:
                            MoveUrlDown(dbContext, guid);
                            break;
                        default:
                            throw new ArgumentException($"Invalid Entity Type: {entityType}");
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
        }

        private static void MoveMenuDown(this ApplicationDbContext dbContext, Guid guid)
        {
            Menu? menuA = dbContext.Menu
                .Where(menu => menu.Guid == guid)
                .FirstOrDefault();

            if ((menuA != null) && (menuA.Guid != Guid.Empty))
            {
                Menu? menuB = dbContext.Menu
                    .Where(menu => menu.Ordinal > menuA.Ordinal)
                    .OrderBy(menu => menu.Ordinal)
                    .FirstOrDefault();

                if (menuB != null)
                {
                    short newOrdinalA = menuB.Ordinal;
                    short newOrdinalB = menuA.Ordinal;

                    menuA.Ordinal = newOrdinalA;
                    menuA.AddUpdate(dbContext);

                    menuB.Ordinal = newOrdinalB;
                    menuB.AddUpdate(dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        private static void MoveSiteDown(this ApplicationDbContext dbContext, Guid guid)
        {
            Site? siteA = dbContext.Site
                .Where(site => site.Guid == guid)
                .FirstOrDefault();

            if (siteA != null)
            {
                Site? siteB = dbContext.Site
                    .Where(site => site.MenuGuid == siteA.MenuGuid)
                    .Where(site => site.Ordinal > siteA.Ordinal)
                    .OrderBy(site => site.Ordinal)
                    .FirstOrDefault();

                if ((siteB != null) && (siteB.Guid != Guid.Empty))
                {
                    short newOrdinalA = siteB.Ordinal;
                    short newOrdinalB = siteA.Ordinal;

                    siteA.Ordinal = newOrdinalA;
                    siteA.AddUpdate(dbContext);

                    siteB.Ordinal = newOrdinalB;
                    siteB.AddUpdate(dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        private static void MoveUrlDown(this ApplicationDbContext dbContext, Guid guid)
        {
            Url? urlA = dbContext.Url
                .Where(url => url.Guid == guid)
                .FirstOrDefault();

            if ((urlA != null) && (urlA.Guid != Guid.Empty))
            {
                Url? urlB = dbContext.Url
                    .Where(url => url.SiteGuid == urlA.SiteGuid)
                    .Where(url => url.Environment > urlA.Environment)
                    .OrderBy(url => url.Environment)
                    .FirstOrDefault();

                if ((urlB != null) && (urlB.Guid != Guid.Empty))
                {
                    EnvironmentTypes newEnvironmentA = urlB.Environment;
                    EnvironmentTypes newEnvironmentB = urlA.Environment;

                    urlA.Environment = newEnvironmentA;
                    urlA.AddUpdate(dbContext);

                    urlB.Environment = newEnvironmentB;
                    urlB.AddUpdate(dbContext);

                    dbContext.SaveChanges();
                }
            }
        }

        public static short MaxMenuOrdinal(this ApplicationDbContext dbContext)
        {
            return dbContext.Configuration
                .Max(config => config.MaxMenuOrdinal);
        }
    }
}
