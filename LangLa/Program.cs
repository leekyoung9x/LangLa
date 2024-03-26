using LangLa.Data;
using LangLa.EventServer;
using LangLa.Model;
using LangLa.Server;
using LangLa.SqlConnection;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace LangLa
{
    public class Program
    {
        public static AppSettings AppSettings { get; set; }

        private static void Main(string[] args)
        {
            IConfigurationRoot Configuration = null;

            Setup.BuildConfig(ref Configuration);
            AppSettings = Setup.GetConfigurationServer(Configuration);
            DataServer.DataGame();
            ConnectionDB.LoadGiaToc();
            ConnectionDB.LoadShop();
            ConnectionDB.LoadTop();
            ConnectionDB.LoadDataServer();
            LangLa.EventServer.EventServer.Start();
            new LangLa.Server.Server();
        }
    }
}