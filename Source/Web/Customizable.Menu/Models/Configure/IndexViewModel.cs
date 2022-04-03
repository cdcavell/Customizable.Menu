using ClassLibrary.Data;
using ClassLibrary.Data.Models;

namespace Customizable.Menu.Models.Configure
{
    public class IndexViewModel
    {
        public List<KeyValuePair<int, string>> EnvironmentTypeList { get; } = Url.GetEnumList();
        public List<KeyValuePair<int, string>> EntityTypeList { get; } = Entities.GetEnumList();
        public List<ClassLibrary.Data.Models.Menu> Menus { get; set; } = new();
        public Guid Guid { get; set; } = Guid.Empty;
        public EntityTypes EntityType { get; set; } = EntityTypes.None;
        public string Description { get; set; } = string.Empty;
        public EnvironmentTypes? EnvironmentType { get; set; }
    }
}
