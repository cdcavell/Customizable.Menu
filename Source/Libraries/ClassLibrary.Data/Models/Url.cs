using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary.Data.Models
{
    public enum EnvironmentTypes
    {
        Development = 0,
        Staging = 1,
        Production = 2,
        Public = 3
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

        public Guid SiteGuid { get; set; }
        [ForeignKey(nameof(SiteGuid))]
        public Site Site { get; set; } = new();

        #endregion

        #region static methods

        public static List<KeyValuePair<int, string>> GetEnumList()
        {
            List<KeyValuePair<int, string>> enumList = new();
            foreach (var item in Enum.GetValues(typeof(EnvironmentTypes)))
                enumList.Add(new KeyValuePair<int, string>((int)item, item.ToString() ?? string.Empty));

            return enumList;
        }

        #endregion
    }
}
