using ClassLibrary.Data;
using ClassLibrary.Data.Models;

namespace Customizable.Menu.Models.Home
{
    public class IndexViewModel
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public List<ClassLibrary.Data.Models.Menu> Menus { get; set; } = new();
    }
}
