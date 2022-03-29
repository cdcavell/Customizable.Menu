using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Data.Models
{
    public enum Environment
    {
        Development,
        Staging,
        Production,
        Public
    }

    [Table("Url")]
    public class Url : DataModel<Site>
    {
        #region properties

        [DataType(DataType.Text)]
        public string Environment { get; set; } = string.Empty;

        [DataType(DataType.Text)]
        [MinLength(1, ErrorMessage = "Link value must contain at least 1 character.")]
        [MaxLength(256, ErrorMessage = "Link value cannot exceed 256 characters.")]
        public string Link { get; set; } = string.Empty;

        #endregion

        #region relationships

        public Guid SiteId { get; set; }
        public Site Site { get; set; } = new();

        #endregion
    }
}
