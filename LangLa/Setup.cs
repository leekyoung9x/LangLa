using LangLa.Model;
using Microsoft.Extensions.Configuration;

namespace LangLa
{
    public static class Setup
    {
        public static void BuildConfig(ref IConfigurationRoot Configuration)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                        .AddJsonFile("Configs/appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static AppSettings GetConfigurationServer(IConfigurationRoot Configuration)
        {
            AppSettings appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            return appSettings;
        }
    }
}