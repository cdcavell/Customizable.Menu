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
    }
}
