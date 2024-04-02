using System;
using LangLa.Data;
using LangLa.Model;
using LangLa.SqlConnection;
using Microsoft.Extensions.Configuration;
using LangLa.IO;

namespace LangLa
{
    public class Program
    {
        public static AppSettings AppSettings { get; set; }

        private static void Main(string[] args)
        {
            Util.ShowCustom("Start to run Lang La version 0.1", ConsoleColor.Cyan);
            IConfigurationRoot Configuration = null;
            Setup.BuildConfig(ref Configuration);
            AppSettings = Setup.GetConfigurationServer(Configuration);
            ConnectionItem.LoadItemTemplate();
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