using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Data.Models
{
    [Table("Site")]
    public class Site : DataModel<Site>
    {
        #region properties

        [DataType(DataType.Text)]
        [MinLength(3, ErrorMessage = "Description value must contain at least 3 characters.")]
        [MaxLength(256, ErrorMessage = "Description value cannot exceed 256 characters.")]
        public string Description { get; set; } = string.Empty;

        #endregion

        #region relationships

        public Guid MenuId { get; set; }
        public Menu Menu { get; set; } = new();

        public List<Url> Urls { get; set; } = new();

        #endregion
    }
}
