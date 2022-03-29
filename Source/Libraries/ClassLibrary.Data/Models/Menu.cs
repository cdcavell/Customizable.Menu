using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace ClassLibrary.Data.Models
{
    [Table("Menu")]
    public class Menu : DataModel<Menu>
    {
        #region properties

        [Required]
        [Range(1, 5, ErrorMessage = "Valid integer range is 1 - 5")]
        public short Ordinal { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MinLength(1, ErrorMessage = "Description value must contain at least 1 character.")]
        [MaxLength(20, ErrorMessage = "Description value cannot exceed 20 characters.")]
        public string Description { get; set; } = String.Empty;

        #endregion

        #region relationships

        public List<Site> Sites { get; set; } = new();

        #endregion

        #region instant methods

        public override void AddUpdate(ApplicationDbContext dbContext)
        {
            var dbContextTransaction = dbContext.Database.CurrentTransaction;
            if (dbContextTransaction == null)
            {
                dbContextTransaction = dbContext.Database.BeginTransaction();
                using (dbContextTransaction)
                {
                    try
                    {
                        InternalAddUpdate(dbContext);
                        base.AddUpdate(dbContext);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                InternalAddUpdate(dbContext);
                base.AddUpdate(dbContext);
            }
        }

        private void InternalAddUpdate(ApplicationDbContext dbContext)
        {
            if (this.IsNew)
            {
                short maxOrdinal = 0;
                if (dbContext.Menu.Any()) maxOrdinal = dbContext.Menu.Max(x => x.Ordinal);
                this.Ordinal = Convert.ToInt16(maxOrdinal + 1);
            }

            if (this.Ordinal < 1 || this.Ordinal > 5)
                throw new ValidationException($"{this.Ordinal} is not a value between 1 - 5.");
        }


        public override void Delete(ApplicationDbContext dbContext)
        {
            short ordinal = this.Ordinal;

            var dbContextTransaction = dbContext.Database.CurrentTransaction;
            if (dbContextTransaction == null)
            {
                dbContextTransaction = dbContext.Database.BeginTransaction();
                using (dbContextTransaction)
                {
                    try
                    {
                        base.Delete(dbContext);
                        InternalReorg(dbContext, ordinal);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                base.Delete(dbContext);
                InternalReorg(dbContext, ordinal);
            }
        }

        #endregion

        #region staic methods

        private static void InternalReorg(ApplicationDbContext dbContext, short ordinal)
        {
            foreach (Menu menuItem in dbContext.Menu.Where(x => x.Ordinal > ordinal).ToList())
            {
                menuItem.Ordinal = Convert.ToInt16(menuItem.Ordinal - 1);
                menuItem.AddUpdate(dbContext);
            }
        }

        public static List<Menu> ReadOnlyList(ApplicationDbContext dbContext)
        {
            List<Menu> queryResult = new();
            if (dbContext.Database.CanConnect())
            {
                queryResult = dbContext.Menu.AsNoTrackingWithIdentityResolution()
                    .Include("Sites")
                    .Include("Sites.Urls")
                    .OrderBy(x => x.Ordinal)
#if DEBUG
                    //TODO: Development Only
                    .LogRecords(x => x.Ordinal.Equals(2))
                    .LogAllRecords()
#endif
                    .ToList();
            }

            return queryResult;
        }

        public static List<Menu> List(ApplicationDbContext dbContext)
        {
            List<Menu> queryResult = new();
            if (dbContext.Database.CanConnect())
            {
                queryResult = dbContext.Menu
                    .Include("Sites")
                    .Include("Sites.Urls")
                    .OrderBy(x => x.Ordinal)
#if DEBUG
                    //TODO: Development Only
                    .LogRecords(x => x.Ordinal.Equals(2))
                    .LogAllRecords()
#endif

                    .ToList();
            }

            return queryResult;
        }

        #endregion
    }
}
