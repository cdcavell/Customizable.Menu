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

            base.AddUpdate(dbContext);
            foreach (var url in this.Urls)
                url.AddUpdate(dbContext);
        }


        public override void Delete(ApplicationDbContext dbContext)
        {
            short ordinal = this.Ordinal;
            Guid guid = this.Guid;

            var dbContextTransaction = dbContext.Database.CurrentTransaction;
            if (dbContextTransaction == null)
            {
                dbContextTransaction = dbContext.Database.BeginTransaction();
                using (dbContextTransaction)
                {
                    try
                    {
                        base.Delete(dbContext);
                        InternalReorg(dbContext, guid, ordinal);
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
                InternalReorg(dbContext, guid, ordinal);
            }
        }

        #endregion

        #region staic methods

        private static void InternalReorg(ApplicationDbContext dbContext, Guid guid, short ordinal)
        {
            foreach (Site siteItem in dbContext.Site.Where(x => x.Guid == guid).Where(x => x.Ordinal > ordinal).ToList())
            {
                siteItem.Ordinal = Convert.ToInt16(siteItem.Ordinal - 1);
                siteItem.AddUpdate(dbContext);
            }
        }

        #endregion
    }
}
