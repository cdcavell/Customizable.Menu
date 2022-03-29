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

        [Range(1, 5, ErrorMessage = "Valid integer range is 1 - 5")]
        public short Ordinal { get; set; }
        [DataType(DataType.Text)]
        [MinLength(3, ErrorMessage = "Description value must contain at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Description value cannot exceed 20 characters.")]
        public string Description { get; set; } = String.Empty;

        #endregion

        #region relationships

        public List<Site> Sites { get; set; } = new();

        #endregion

        #region instant methods

        public override void AddUpdate(ApplicationDbContext dbContext)
        {
            if (this.IsNew)
            {
                short maxOrdinal = 0; 
                if (dbContext.Menu.Any()) maxOrdinal = dbContext.Menu.Max(x => x.Ordinal);
                this.Ordinal = Convert.ToInt16(maxOrdinal + 1);
            }
                

            if (this.Ordinal < 1 || this.Ordinal > 5)
                throw new ValidationException($"{this.Ordinal} is not a value between 1 - 5.");

            base.AddUpdate(dbContext);
        }

        public override void Delete(ApplicationDbContext dbContext)
        {
            short ordinal = this.Ordinal;

            base.Delete(dbContext);

            foreach (Menu menuItem in dbContext.Menu.Where(x => x.Ordinal > ordinal).ToList())
            {
                menuItem.Ordinal = Convert.ToInt16(menuItem.Ordinal - 1);
                menuItem.AddUpdate(dbContext);
            }
        }

        #endregion

        #region staic methods

        public static List<Menu> List(ApplicationDbContext dbContext)
        {
            List<Menu> queryResult = new();
            if (dbContext.Database.CanConnect())
            {
                queryResult = dbContext.Menu.AsNoTracking()
                    .Include("Sites")
                    .Include("Sites.Urls")
                    .OrderBy(x => x.Ordinal)
                    .ToList();
            }

            return queryResult;
        }

        #endregion
    }
}
