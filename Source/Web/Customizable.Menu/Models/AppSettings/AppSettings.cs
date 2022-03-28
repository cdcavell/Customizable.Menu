namespace Customizable.Menu.Models.AppSettings
{
    public class AppSettings : ClassLibrary.Mvc.Services.AppSettings.Models.AppSettings
    {
        public AppSettings(IConfiguration configuration) : base(configuration) { }

        public ConnectionStrings ConnectionStrings { get; set; } = new ConnectionStrings();
    }
}
