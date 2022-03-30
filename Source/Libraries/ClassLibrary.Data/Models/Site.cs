using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Data.Models
{
    [Table("Site")]
    public class Site : DataModel<Site>
    {
        #region properties

        [Required]
        public short Ordinal { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [MinLength(1, ErrorMessage = "Description value must contain at least 1 characters.")]
        [MaxLength(256, ErrorMessage = "Description value cannot exceed 256 characters.")]
        public string Description { get; set; } = string.Empty;

        #endregion

        #region relationships

        public Guid MenuId { get; set; }
        public Menu Menu { get; set; } = new();

        public List<Url> Urls { get; set; } = new();

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
            if (this.Ordinal < 1)
            {
                short maxOrdinal = 0;
                if (dbContext.Site.Where(x => x.MenuId == this.MenuId).Any()) maxOrdinal = dbContext.Site.Where(x => x.MenuId == this.MenuId).Max(x => x.Ordinal);
                this.Ordinal = Convert.ToInt16(maxOrdinal + 1);
            }

            foreach (var url in this.Urls)
                url.AddUpdate(dbContext);
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

        #endregion
    }
}
