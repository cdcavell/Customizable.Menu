using ClassLibrary.Data.Models;

namespace Customizable.Menu.Models
{
    public class IndexViewModel
    {
        public List<KeyValuePair<int, string>> EnvironmentTypes { get; } = Url.GetEnumList();
        public Guid MenuGuid { get; set; } = Guid.Empty;
        public List<ClassLibrary.Data.Models.Menu> Menus { get; set; } = new();

    }
}
