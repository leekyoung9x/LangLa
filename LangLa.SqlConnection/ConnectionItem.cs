using System;
using System.Collections.Generic;
using LangLa.BaseDatabase;
using LangLa.Data;
using LangLa.IO;
using LangLa.Manager;
using LangLa.SupportOOP;
using LangLa.Template;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace LangLa.SqlConnection
{
    public class ConnectionItem
    {
        public static void LoadItemTemplate()
        {
            var result = ItemTemplateManager.GetAll();

            DataServer.ArrItemTemplate = result;
        }
    }
}