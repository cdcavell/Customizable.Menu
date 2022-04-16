using ClassLibrary.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Customizable.Menu.Models.Configure
{
    public class SiteViewModel
    {
        public const string BindProperty = "Guid, Site";

        [Required]
        public Guid Guid { get; set; } = Guid.Empty;
        [Required]
        public Site Site { get; set; } = new();
    }
}
