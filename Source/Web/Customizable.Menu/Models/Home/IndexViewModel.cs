using ClassLibrary.Data;
using ClassLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Customizable.Menu.Models.Home
{
    public class IndexViewModel
    {
        public const string BindProperty = "Guid";     

        [Required]
        public Guid Guid { get; set; } = Guid.Empty;
        public List<ClassLibrary.Data.Models.Menu> Menus { get; set; } = new();
        public List<KeyValuePair<int, string>> EnvironmentTypeList { get; } = Url.GetEnumList();
        public List<KeyValuePair<int, string>> EntityTypeList { get; } = Entities.GetEnumList();
    }
}
