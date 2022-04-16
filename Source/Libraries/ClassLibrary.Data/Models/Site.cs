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

        [NotMapped]
        public List<KeyValuePair<int, string>> AvailableEnvironments
        {
            get
            {
                List<KeyValuePair<int, string>> returnList = Url.GetEnumList();
                foreach (Url url in this.Urls)
                    returnList.Remove(new KeyValuePair<int, string>((int)url.Environment, url.Environment.ToString()));

                return returnList;
            }
        }

        #endregion

        #region relationships

        public Guid MenuGuid { get; set; }
        [ForeignKey(nameof(MenuGuid))]
        public Menu Menu { get; set; } = new();

        [InverseProperty(nameof(Url.Site))]
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
                if (dbContext.Site.Where(x => x.MenuGuid == this.MenuGuid).Any()) 
                    maxOrdinal = dbContext.Site
                        .Where(x => x.MenuGuid == this.MenuGuid)
                        .Max(x => x.Ordinal);

                this.Ordinal = Convert.ToInt16(maxOrdinal + 1);
            }

            base.AddUpdate(dbContext);
            foreach (var url in this.Urls)
                url.AddUpdate(dbContext);
        }


        public override void Delete(ApplicationDbContext dbContext)
        {
            short ordinal = this.Ordinal;
            Guid guid = this.MenuGuid;

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

        private static void InternalReorg(ApplicationDbContext dbContext, Guid menuGuid, short ordinal)
        {
            foreach (Site siteItem in dbContext.Site.Where(x => x.Guid == menuGuid).Where(x => x.Ordinal > ordinal).ToList())
            {
                siteItem.Ordinal = Convert.ToInt16(siteItem.Ordinal - 1);
                siteItem.AddUpdate(dbContext);
            }
        }

        #endregion
    }
}
