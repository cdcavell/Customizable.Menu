using ClassLibrary.Data.Models;

namespace Customizable.Menu.Models.Configure
{
    public class SiteViewModel
    {
        public Guid Guid { get; set; } = Guid.Empty;
        public Site Site { get; set; } = new();
    }
}
