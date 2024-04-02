using System;
using System.Collections.Generic;
using LangLa.BaseDatabase;
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
        // public static void LoadItemTemplate()
        // {
        //     List<ItemTemplate> result = new List<ItemTemplate>();
        //     string Text = "SELECT * FROM item_template;";
        //     MySqlConnection con = Connection.getConnection();
        //     MySqlCommand cmd = null;
        //     MySqlDataReader read = null;
        //
        //     try
        //     {
        //         con.Open();
        //         cmd = con.CreateCommand();
        //         cmd.CommandText = Text;
        //         read = cmd.ExecuteReader();
        //         while (read.Read())
        //         {
        //             ItemTemplate itemTemplate = new ItemTemplate();
        //
        //             itemTemplate.Id = read.GetInt16("id");
        //             itemTemplate.Name = read.GetString("name");
        //             itemTemplate.Detail = read.GetString("detail");
        //             itemTemplate.IsCongDon = read.GetBoolean("is_cong_don");
        //             itemTemplate.GioiTinh = (sbyte)read.GetInt16("gioi_tinh");
        //             itemTemplate.Type = (sbyte)read.GetInt16("type");
        //             itemTemplate.IdClass = (sbyte)read.GetInt16("id_class");
        //             itemTemplate.IdIcon = read.GetInt16("id_icon");
        //             itemTemplate.LevelNeed = read.GetInt16("level_need");
        //             itemTemplate.TaiPhuNeed = read.GetInt32("tai_phu_need");
        //             itemTemplate.IdMob = read.GetInt16("id_mob");
        //             itemTemplate.IdChar = read.GetInt16("id_char");
        //             
        //             result.Add(itemTemplate);
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Util.ShowErr(e);
        //     }
        //     finally
        //     {
        //         con.Close();
        //         con.Dispose();
        //         cmd?.Dispose();
        //         read?.DisposeAsync();
        //     }
        //
        //     Util.ShowInfo(string.Format("Load item template DB success ({0})", result.Count));
        // } 
        
        public static void LoadItemTemplate()
        {
            var result = ItemTemplateManager.GetAll();
        }
    }
}