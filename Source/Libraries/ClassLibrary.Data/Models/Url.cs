using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Data.Models
{
    public enum EnvironmentTypes
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

        [Required]
        public EnvironmentTypes Environment { get; set; }

        [Required]
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
