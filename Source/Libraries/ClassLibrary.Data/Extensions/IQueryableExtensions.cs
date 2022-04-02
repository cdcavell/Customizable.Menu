using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
