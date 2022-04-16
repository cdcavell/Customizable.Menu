using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Customizable.Menu.Models.Configure
{
    public class IndexViewModel
    {
        public const string BindProperty = "Guid, EntityType, Description, EnvironmentType";

        [Required]
        public Guid Guid { get; set; } = Guid.Empty;
        [Required]
        public EntityTypes EntityType { get; set; } = EntityTypes.None;
        public string Description { get; set; } = string.Empty;
        public EnvironmentTypes? EnvironmentType { get; set; }
        public List<KeyValuePair<int, string>> EnvironmentTypeList { get; } = Url.GetEnumList();
        public List<KeyValuePair<int, string>> EntityTypeList { get; } = Entities.GetEnumList();
        public List<ClassLibrary.Data.Models.Menu> Menus { get; set; } = new();
        public short MaxMenuOrdinal { get; set; }
    }
}
