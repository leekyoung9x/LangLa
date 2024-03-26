using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.Manager;
using LangLa.Server;
using LangLa.SupportOOP;
using LangLa.Template;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace LangLa.SqlConnection
{
	public static class ConnectionDB
	{
		private static readonly sbyte[] TypeShop = new sbyte[31]
		{
			0, 1, 6, 7, 39, 8, 9, 10, 11, 12,
			13, 14, 15, 16, 17, 18, 19, 30, 37, 36,
			38, 26, 27, 28, 29, 20, 21, 22, 23, 24,
			25
		};

		private static readonly object Lock_CreateGiatoc = new object();

		private static readonly object _Lock_UpdateRoleTV = new object();

		private static object _lock_Create = new object();

		private static sbyte[] ShopTxt = new sbyte[14]
		{
			8, 9, 10, 11, 12, 13, 14, 15, 16, 17,
			18, 30, 37, 38
		};

		private static readonly int[] MoneyVuKhi = new int[7] { 30000, 62000, 150000, 300000, 500000, 800000, 1111111 };

		private static readonly int[] MoneyPhuKienDTMSOT = new int[6] { 7500, 15000, 36000, 90000, 150000, 300000 };

		private static readonly int[] MoneyPhuKienTuiNhanGia = new int[6] { 10500, 21000, 45000, 135000, 225000, 450000 };

		private static readonly int[] MoneyShopPhuKien = new int[6] { 6000, 12000, 30000, 67500, 112500, 225000 };

		private static readonly string[] IdShopCuaHangKhuVang = new string[70]
		{
			"156", "157", "158", "166", "168", "185", "186", "187", "159", "347",
			"162", "569", "169", "170", "301", "371", "619", "182", "285", "404",
			"782", "500", "683", "593", "498", "568", "551", "161", "277", "571",
			"603", "300", "156", "166", "168", "159", "161", "277", "267", "266",
			"643,3", "404", "163,100000", "644,5", "300", "163,100000", "277,2", "571", "163,300000", "785,3",
			"499,10", "163,500000", "5,10", "347,3", "163,1000000", "435", "267", "163,1000000", "295", "182,20",
			"163,3000000", "296", "500,10", "163,5000000", "297", "428,30", "163,7000000", "688,2", "468", "814"
		};

		private static readonly short[] MoneyCuaHangVang = new short[33]
		{
			32, 58, 98, 10, 20, 14, 48, 190, 8, 16,
			100, 30, 50, 50, 250, 55, 10, 5, 5, 5,
			2, 3, 1, 20, 20, 5, 15, 99, 69, 20,
			15, 200, 10
		};

		private static readonly short[] MoneyCuaHangVangKhoa = new short[8] { 100, 15, 30, 15, 99, 30, 9, 15 };

		private static readonly short[] MoneyCuaHangKhuRank = new short[30]
		{
			1, 1, 1, 10, 10, 10, 30, 30, 30, 50,
			50, 50, 100, 100, 100, 150, 150, 150, 300, 300,
			300, 500, 500, 500, 800, 800, 800, 1000, 1000, 1000
		};

		private static readonly short[] IdShopDuocPham = new short[12]
		{
			12, 13, 14, 15, 16, 219, 17, 18, 19, 20,
			21, 221
		};

		private static readonly short[] MoneyDuocPham = new short[6] { 200, 400, 600, 800, 1000, 1200 };

		public static readonly string[] ListItemShopPhucLoi = CreateShopPhucLoi();

		public static void LoadGiaToc()
		{
			GiaTocManager.ListGiaTocs = new ConcurrentDictionary<short, GiaTocTemplate>();
			string Text = "SELECT * from giatoc";
			MySqlConnection con = Connection.getConnection();
			MySqlCommand cmd = null;
			MySqlDataReader read = null;
			try
			{
				con.Open();
				cmd = con.CreateCommand();
				cmd.CommandText = Text;
				read = cmd.ExecuteReader();
				while (read.Read())
				{
					GiaTocTemplate giaToc = new GiaTocTemplate();
					giaToc = JsonConvert.DeserializeObject<GiaTocTemplate>(read.GetString("info"));
					giaToc.ItemGiaToc = JsonConvert.DeserializeObject<List<Item>>(read.GetString("items"));
					giaToc.LogGiaTocs = JsonConvert.DeserializeObject<List<string>>(read.GetString("loggiatocs"));
					giaToc.ThanhViens = JsonConvert.DeserializeObject<List<GiaTocTemplate.ThanhVienGiaToc>>(read.GetString("thanhviens"));
					giaToc.SkillGiaToc = JsonConvert.DeserializeObject<sbyte[]>(read.GetString("skills"));
					GiaTocManager.ListGiaTocs.TryAdd(giaToc.ID, giaToc);
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Close();
				con.Dispose();
				cmd?.Dispose();
				read?.DisposeAsync();
			}
			int Id = 0;
			foreach (GiaTocTemplate c in GiaTocManager.ListGiaTocs.Values)
			{
				if (c.ID > Id)
				{
					Id = c.ID;
				}
			}
			GiaTocManager.ID_GIA_TOC = Id++;
			Util.ShowLog("GIA TOC SIZE " + GiaTocManager.ListGiaTocs.Values.Count);
		}

		private static sbyte SetLevel(long exp)
		{
			sbyte Level2 = 0;
			while (Level2 < DataServer.ArrExp.Length && exp >= DataServer.ArrExp[Level2])
			{
				exp -= DataServer.ArrExp[Level2];
				Level2++;
			}
			return Level2;
		}

		public static void LoadTop()
		{
			TopManager.InfoTopLevels = new List<InfoTop>();
			TopManager.InfoTopTaiPhus = new List<InfoTop>();
			TopManager.InfoTopCuongHoa = new List<InfoTop>();
			TopManager.InfoTopNhiDong = new List<InfoTop>();
			TopManager.InfoTopNapNhieu = new List<InfoTop>();
			MySqlConnection con = Connection.getConnection();
			MySqlCommand cmd = null;
			MySqlDataReader read = null;
			string[] Text = new string[5] { "SELECT Name, Cast(JSON_EXTRACT(Point, '$.Exp')as UNSIGNED) AS Exp, Cast(JSON_EXTRACT(Info, '$.IdClass')as UNSIGNED) AS IdClass, Cast(JSON_EXTRACT(Info, '$.IdGiaToc')as SIGNED) AS IdGiaToc FROM characters WHERE Cast(JSON_EXTRACT(Point, '$.Exp')as SIGNED) >9567889496 ORDER BY Exp DESC LIMIT 100", "SELECT Name, Cast(JSON_EXTRACT(Info, '$.TaiPhu')as UNSIGNED) AS TaiPhu, Cast(JSON_EXTRACT(Info, '$.IdClass')as UNSIGNED) AS IdClass, Cast(JSON_EXTRACT(Info, '$.IdGiaToc')as SIGNED) AS IdGiaToc FROM characters WHERE Cast(JSON_EXTRACT(Info, '$.TaiPhu')as SIGNED) >0 ORDER BY TaiPhu DESC LIMIT 100;", "SELECT Name, Cast(JSON_EXTRACT(Info, '$.PointCuongHoa')as UNSIGNED) AS PointCuongHoa, Cast(JSON_EXTRACT(Info, '$.IdClass')as UNSIGNED) AS IdClass, Cast(JSON_EXTRACT(Info, '$.IdGiaToc')as SIGNED) AS IdGiaToc FROM characters WHERE Cast(JSON_EXTRACT(Info, '$.PointCuongHoa')as SIGNED) >0 ORDER BY PointCuongHoa DESC LIMIT 100", "SELECT Name, Cast(JSON_EXTRACT(Point, '$.Exp')as UNSIGNED) AS Exp, Cast(JSON_EXTRACT(Info, '$.IdClass')as UNSIGNED) AS IdClass, Cast(JSON_EXTRACT(Info, '$.IdGiaToc')as SIGNED) AS IdGiaToc FROM characters WHERE Cast(JSON_EXTRACT(Point, '$.Exp')as SIGNED) <=9567889496 ORDER BY Exp DESC LIMIT 100", "SELECT Name, Cast(JSON_EXTRACT(TimeChar, '$.TongSoVangDaNap')as UNSIGNED) AS TongSoVangDaNap,Cast(JSON_EXTRACT(TimeChar, '$.TimeNap')as UNSIGNED) AS TimeNap, Cast(JSON_EXTRACT(Info, '$.IdClass')as UNSIGNED) AS IdClass, Cast(JSON_EXTRACT(Info, '$.IdGiaToc')as SIGNED) AS IdGiaToc FROM characters WHERE Cast(JSON_EXTRACT(TimeChar, '$.TongSoVangDaNap')as UNSIGNED) >0 ORDER BY TongSoVangDaNap DESC,TimeNap DESC LIMIT 100" };
			try
			{
				con.Open();
				cmd = con.CreateCommand();
				for (int i = 0; i < Text.Length; i++)
				{
					cmd.CommandText = Text[i];
					read = cmd.ExecuteReader();
					while (read.Read())
					{
						switch (i)
						{
						case 0:
						{
							InfoTop infoTop = new InfoTop();
							infoTop.Name = read.GetString("Name");
							infoTop.Exp = read.GetInt64("Exp");
							infoTop.IdClass = (sbyte)read.GetInt16("IdClass");
							short IdGiaToc = read.GetInt16("IdGiaToc");
							infoTop.Level = SetLevel(infoTop.Exp);
							if (IdGiaToc == -1)
							{
								infoTop.NameGiaToc = "";
							}
							else
							{
								infoTop.NameGiaToc = GiaTocManager.ListGiaTocs.Values.FirstOrDefault((GiaTocTemplate s) => s.ID == IdGiaToc).Name;
							}
							TopManager.InfoTopLevels.Add(infoTop);
							break;
						}
						case 1:
						{
							InfoTop infoTop2 = new InfoTop();
							infoTop2.Name = read.GetString("Name");
							infoTop2.TaiPhu = read.GetInt32("TaiPhu");
							infoTop2.IdClass = (sbyte)read.GetInt16("IdClass");
							short IdGiaToc2 = read.GetInt16("IdGiaToc");
							if (IdGiaToc2 == -1)
							{
								infoTop2.NameGiaToc = "";
							}
							else
							{
								infoTop2.NameGiaToc = GiaTocManager.ListGiaTocs.Values.FirstOrDefault((GiaTocTemplate s) => s.ID == IdGiaToc2).Name;
							}
							TopManager.InfoTopTaiPhus.Add(infoTop2);
							break;
						}
						case 2:
						{
							InfoTop infoTop3 = new InfoTop();
							infoTop3.Name = read.GetString("Name");
							infoTop3.PointCuongHoa = read.GetInt32("PointCuongHoa");
							infoTop3.IdClass = (sbyte)read.GetInt16("IdClass");
							short IdGiaToc3 = read.GetInt16("IdGiaToc");
							if (IdGiaToc3 == -1)
							{
								infoTop3.NameGiaToc = "";
							}
							else
							{
								infoTop3.NameGiaToc = GiaTocManager.ListGiaTocs.Values.FirstOrDefault((GiaTocTemplate s) => s.ID == IdGiaToc3).Name;
							}
							TopManager.InfoTopCuongHoa.Add(infoTop3);
							break;
						}
						case 3:
						{
							InfoTop infoTop4 = new InfoTop();
							infoTop4.Name = read.GetString("Name");
							infoTop4.Exp = read.GetInt64("Exp");
							infoTop4.IdClass = (sbyte)read.GetInt16("IdClass");
							short IdGiaToc4 = read.GetInt16("IdGiaToc");
							infoTop4.Level = SetLevel(infoTop4.Exp);
							if (IdGiaToc4 == -1)
							{
								infoTop4.NameGiaToc = "";
							}
							else
							{
								infoTop4.NameGiaToc = GiaTocManager.ListGiaTocs.Values.FirstOrDefault((GiaTocTemplate s) => s.ID == IdGiaToc4).Name;
							}
							TopManager.InfoTopNhiDong.Add(infoTop4);
							break;
						}
						case 4:
						{
							InfoTop infoTop5 = new InfoTop();
							infoTop5.Name = read.GetString("Name");
							infoTop5.NapNhieu = read.GetInt32("TongSoVangDaNap");
							infoTop5.TimeNap = read.GetInt64("TimeNap");
							infoTop5.IdClass = (sbyte)read.GetInt16("IdClass");
							short IdGiaToc5 = read.GetInt16("IdGiaToc");
							if (IdGiaToc5 == -1)
							{
								infoTop5.NameGiaToc = "";
							}
							else
							{
								infoTop5.NameGiaToc = GiaTocManager.ListGiaTocs.Values.FirstOrDefault((GiaTocTemplate s) => s.ID == IdGiaToc5).Name;
							}
							TopManager.InfoTopNapNhieu.Add(infoTop5);
							break;
						}
						}
					}
					read.Close();
					read.DisposeAsync();
					cmd.Dispose();
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Close();
				con.Dispose();
				cmd?.Dispose();
				read?.DisposeAsync();
			}
			LoadTopGiaToc();
		}

		public static void UpdateServer()
		{
			MySqlConnection con = Connection.getConnection();
			MySqlCommand cmd = null;
			string Text = "Update serverll set id ='1' ,dautuallServer =@1,thethangallserver =@2,rankcaonhatserver=@3 where id ='1'";
			try
			{
				con.Open();
				cmd = con.CreateCommand();
				cmd.CommandText = Text;
				cmd.Parameters.AddWithValue("@1", LangLa.Server.Server.DauTuAllServer);
				cmd.Parameters.AddWithValue("@3", LangLa.Server.Server.TheThangAllServer);
				cmd.Parameters.AddWithValue("@2", LangLa.Server.Server.RankCaoNhatServer);
				cmd.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Close();
				con.Dispose();
				cmd?.Dispose();
			}
		}

		public static void LoadDataServer()
		{
			MySqlConnection con = Connection.getConnection();
			MySqlCommand cmd = null;
			MySqlDataReader Read = null;
			string Text = "SELECT * from serverll";
			try
			{
				con.Open();
				cmd = con.CreateCommand();
				cmd.CommandText = Text;
				Read = cmd.ExecuteReader();
				if (Read.Read())
				{
					LangLa.Server.Server.DauTuAllServer = Read.GetInt32("dautuallserver");
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Close();
				con.Dispose();
				cmd?.Dispose();
				Read?.Close();
				Read?.DisposeAsync();
			}
		}

		private static void LoadTopGiaToc()
		{
			List<InfoTop> infoTops2 = new List<InfoTop>();
			foreach (GiaTocTemplate T in GiaTocManager.ListGiaTocs.Values)
			{
				InfoTop infoTop = new InfoTop();
				infoTop.NameGiaToc = T.Name;
				infoTop.LevelGiaToc = (sbyte)T.Level;
				infoTop.ThanhVien = (sbyte)T.ThanhViens.Count;
				infoTop.NameTocTruong = T.NameTocTruong;
				infoTop.ExpGiaToc = T.Exp;
				T.SoLanDiAiTrongNgay = 1;
				infoTops2.Add(infoTop);
			}
			TopManager.InfoTopGiaToc = (from s in infoTops2
				orderby s.LevelGiaToc descending, s.ExpGiaToc descending
				select s).ToList();
		}

		public static void UpdateRoleThanhVien(string Name)
		{
			lock (_Lock_UpdateRoleTV)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = "UPDATE characters SET Info = JSON_SET( Info, '$.IdGiaToc', -1 ),Info = JSON_SET( Info, '$.RoleGiaToc',-1 ) WHERE Name =@1";
					cmd.Parameters.AddWithValue("@1", Name);
					cmd.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					Util.ShowErr(e);
				}
				finally
				{
					con.Close();
					cmd?.Dispose();
				}
			}
		}

		public static bool DeleteGiaToc(int id)
		{
			lock (_lock_Create)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				string Text = "Delete from giatoc where id =@1";
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = Text;
					cmd.Parameters.AddWithValue("@1", id);
					cmd.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					Util.ShowErr(e);
					return false;
				}
				finally
				{
					con.Close();
					con.Dispose();
					cmd?.Dispose();
				}
				return true;
			}
		}

		public static bool CreateGiaToc(GiaTocTemplate GiaToc)
		{
			lock (Lock_CreateGiatoc)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				string Text = "INSERT INTO Giatoc set id =@1 ,Name=@2,Info=@3,items=@4,loggiatocs=@5,thanhviens=@6,skills=@7";
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = Text;
					cmd.Parameters.AddWithValue("@1", JsonConvert.SerializeObject(GiaToc.ID));
					cmd.Parameters.AddWithValue("@2", JsonConvert.SerializeObject(GiaToc.Name));
					cmd.Parameters.AddWithValue("@3", JsonConvert.SerializeObject(GiaToc));
					cmd.Parameters.AddWithValue("@4", JsonConvert.SerializeObject(GiaToc.ItemGiaToc));
					cmd.Parameters.AddWithValue("@5", JsonConvert.SerializeObject(GiaToc.LogGiaTocs));
					cmd.Parameters.AddWithValue("@6", JsonConvert.SerializeObject(GiaToc.ThanhViens));
					cmd.Parameters.AddWithValue("@7", JsonConvert.SerializeObject(GiaToc.SkillGiaToc));
					cmd.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					Util.ShowErr(e);
					return false;
				}
				finally
				{
					con.Close();
					con.Dispose();
					cmd?.Dispose();
				}
				return true;
			}
		}

		public static void UpdateGiaToc()
		{
			MySqlConnection con = Connection.getConnection();
			MySqlCommand cmd = null;
			string Text = "Update Giatoc set Name=@1,Info=@2,items=@3,loggiatocs=@4,thanhviens=@5,skills=@6 where id = @7";
			try
			{
				con.Open();
				cmd = con.CreateCommand();
				cmd.CommandText = Text;
				foreach (GiaTocTemplate GiaToc in GiaTocManager.ListGiaTocs.Values)
				{
					cmd.Parameters.AddWithValue("@1", JsonConvert.SerializeObject(GiaToc.Name));
					cmd.Parameters.AddWithValue("@2", JsonConvert.SerializeObject(GiaToc));
					cmd.Parameters.AddWithValue("@3", JsonConvert.SerializeObject(GiaToc.ItemGiaToc));
					cmd.Parameters.AddWithValue("@4", JsonConvert.SerializeObject(GiaToc.LogGiaTocs));
					cmd.Parameters.AddWithValue("@5", JsonConvert.SerializeObject(GiaToc.ThanhViens));
					cmd.Parameters.AddWithValue("@6", JsonConvert.SerializeObject(GiaToc.SkillGiaToc));
					cmd.Parameters.AddWithValue("@7", JsonConvert.SerializeObject(GiaToc.ID));
					cmd.ExecuteNonQuery();
					cmd.Dispose();
					cmd.Parameters.Clear();
					cmd.CommandText = Text;
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Close();
				con.Dispose();
				cmd?.Dispose();
			}
		}

		public static void LoadShop()
		{
			DataShop.ShopTemplates = new List<ShopTemplate>();
			int i = 0;
			string Text = "SELECT * from listshop where TypeShop =@1 and cansale =@2";
			MySqlConnection con = Connection.getConnection();
			MySqlCommand cmd = null;
			MySqlDataReader read = null;
			try
			{
				con.Open();
				cmd = con.CreateCommand();
				cmd.CommandText = Text;
				for (int j = 0; j < TypeShop.Length; j++)
				{
					cmd.Parameters.AddWithValue("@1", TypeShop[j]);
					cmd.Parameters.AddWithValue("@2", 1);
					read = cmd.ExecuteReader();
					ShopTemplate shopTemplate = new ShopTemplate();
					shopTemplate.TypeShop = TypeShop[j];
					while (read.Read())
					{
						ItemShop itemShop = new ItemShop();
						itemShop.IdBuy = read.GetInt16("id");
						itemShop.IdItem = read.GetInt16("itemid");
						itemShop.Quantity = read.GetInt32("quantity");
						itemShop.IdClass = read.GetSByte("idclass");
						itemShop.GioiTinh = read.GetSByte("gioitinh");
						itemShop.IsLock = read.GetBoolean("islock");
						itemShop.HSD = read.GetInt32("HSD");
						itemShop.TinhThachBuy = read.GetInt32("tinhthachbuy");
						itemShop.VangBuy = read.GetInt32("vangbuy");
						itemShop.VangKhoaBuy = read.GetInt32("vangkhoabuy");
						itemShop.BacBuy = read.GetInt32("bacbuy");
						itemShop.BacKhoaBuy = read.GetInt32("backhoabuy");
						itemShop.MoneyHokage = read.GetInt32("moneyhokage");
						itemShop.Options = read.GetString("options");
						shopTemplate.ItemShops.Add(itemShop);
						i++;
					}
					DataShop.ShopTemplates.Add(shopTemplate);
					cmd.Dispose();
					read.DisposeAsync();
					cmd.Parameters.Clear();
					cmd.CommandText = Text;
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Close();
				con.Dispose();
				cmd?.Dispose();
				read?.DisposeAsync();
			}
			Util.ShowLog("SHOP SIZE " + DataShop.ShopTemplates.Count);
		}

		private static void LoadShopTxt()
		{
			for (int i = 0; i < ShopTxt.Length; i++)
			{
				ShopTemplate shopTemplate3 = new ShopTemplate();
				List<ItemShop> itemShops2 = new List<ItemShop>();
				shopTemplate3.TypeShop = ShopTxt[i];
				string[] InfoShops = null;
				string[] InfoShops2 = null;
				switch (ShopTxt[i])
				{
				case 8:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\ShopBinhKhi").Split("#");
					break;
				case 9:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\DaiTranBoy").Split("#");
					InfoShops2 = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\DaiTranGirl").Split("#");
					break;
				case 10:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\AoBoy").Split("#");
					InfoShops2 = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\AoGirl").Split("#");
					break;
				case 11:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\BaoTayBoy").Split("#");
					InfoShops2 = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\BaoTayGirl").Split("#");
					break;
				case 12:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\QuanBoy").Split("#");
					InfoShops2 = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\QuanGirl").Split("#");
					break;
				case 13:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\GiayBoy").Split("#");
					InfoShops2 = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\GiayGirl").Split("#");
					break;
				case 14:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\DayThung").Split("#");
					break;
				case 15:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\MocSat").Split("#");
					break;
				case 16:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\OngTieu").Split("#");
					break;
				case 17:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\TuiNhanGia").Split("#");
					break;
				case 18:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\ShopNgoaiTrang").Split("#");
					break;
				case 30:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\ShopNgoaiTrang").Split("#");
					break;
				case 37:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\ShopNgoaiTrang").Split("#");
					break;
				case 38:
					InfoShops = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\ShopNgoaiTrang").Split("#");
					break;
				}
				string[] array = InfoShops;
				foreach (string s7 in array)
				{
					ItemShop itemShop6 = JsonConvert.DeserializeObject<ItemShop>(s7);
					itemShops2.Add(itemShop6);
				}
				if (InfoShops2 != null)
				{
					string[] array2 = InfoShops2;
					foreach (string s6 in array2)
					{
						ItemShop itemShop5 = JsonConvert.DeserializeObject<ItemShop>(s6);
						itemShops2.Add(itemShop5);
					}
				}
				switch (ShopTxt[i])
				{
				case 8:
				{
					int m = 0;
					int x2 = 0;
					sbyte k;
					for (k = 1; k <= 5; k++)
					{
						m = 0;
						DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.Type == 1 && s.IdClass == k).ToList().ForEach(delegate(ItemTemplate s)
						{
							ItemShop itemShop9 = new ItemShop();
							ItemShop itemShop10 = new ItemShop();
							itemShop9.IdItem = s.Id;
							itemShop9.IdBuy = 888;
							itemShop10.GioiTinh = 2;
							itemShop9.IdClass = k;
							itemShop10.IsLock = true;
							itemShop9.Options = itemShops2[x2].Options;
							itemShop9.BacBuy = itemShops2[x2].BacBuy;
							shopTemplate3.ItemShops.Add(itemShop9);
							int num8 = m;
							m = num8 + 1;
							num8 = x2;
							x2 = num8 + 1;
						});
					}
					break;
				}
				case 18:
					shopTemplate3.ItemShops = itemShops2.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 12).ToList();
					break;
				case 30:
					shopTemplate3.ItemShops = itemShops2.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 15).ToList();
					break;
				case 37:
					shopTemplate3.ItemShops = itemShops2.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 10).ToList();
					break;
				case 38:
					shopTemplate3.ItemShops = itemShops2.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 14).ToList();
					break;
				default:
					shopTemplate3.ItemShops = itemShops2;
					break;
				}
				DataShop.ShopTemplates.Add(shopTemplate3);
			}
			string Shop = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\Shop36");
			string[] Shops = Shop.Split("#");
			ShopTemplate shopTemplate = new ShopTemplate();
			shopTemplate.TypeShop = 36;
			string[] array3 = Shops;
			foreach (string s5 in array3)
			{
				ItemShop itemShop4 = JsonConvert.DeserializeObject<ItemShop>(s5);
				shopTemplate.ItemShops.Add(itemShop4);
			}
			DataShop.ShopTemplates.Add(shopTemplate);
			string[] ShopHokages = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\ShopHokage").Split("#");
			List<ItemShop> itemShops = new List<ItemShop>();
			string[] array4 = ShopHokages;
			foreach (string s4 in array4)
			{
				ItemShop itemShop3 = JsonConvert.DeserializeObject<ItemShop>(s4);
				itemShops.Add(itemShop3);
			}
			string[] ShopHokageGirls = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\ShopHokageGirl").Split("#");
			List<ItemShop> itemShopGirls = new List<ItemShop>();
			string[] array5 = ShopHokageGirls;
			foreach (string s3 in array5)
			{
				ItemShop itemShop2 = JsonConvert.DeserializeObject<ItemShop>(s3);
				itemShopGirls.Add(itemShop2);
			}
			sbyte[] Ss = new sbyte[10] { 26, 27, 28, 29, 20, 21, 22, 23, 24, 25 };
			short IDS = 1905;
			int c = 0;
			short PointHokage = 300;
			List<ItemShop> itemShops3 = new List<ItemShop>();
			string[] ShopHokages2 = File.ReadAllText("D:\\Project LL\\Project LL\\ServerSG188\\data\\BinhKhiHokage").Split("#");
			string[] array6 = ShopHokages2;
			foreach (string s2 in array6)
			{
				ItemShop itemShop = JsonConvert.DeserializeObject<ItemShop>(s2);
				itemShops3.Add(itemShop);
			}
			sbyte[] array7 = Ss;
			foreach (sbyte x in array7)
			{
				ShopTemplate shopTemplate2 = new ShopTemplate();
				shopTemplate2.TypeShop = x;
				c = 0;
				PointHokage = 300;
				switch (x)
				{
				case 20:
				{
					int l = 0;
					int x3 = 0;
					sbyte j;
					for (j = 1; j <= 5; j++)
					{
						l = 0;
						DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.Type == 1 && s.IdClass == j).ToList().ForEach(delegate(ItemTemplate s)
						{
							ItemShop itemShop7 = new ItemShop();
							ItemShop itemShop8 = new ItemShop();
							itemShop7.IdItem = s.Id;
							itemShop7.IdBuy = 888;
							itemShop7.GioiTinh = 2;
							itemShop7.IdClass = j;
							itemShop8.IsLock = true;
							itemShop7.Options = itemShops3[x3].Options;
							itemShop7.BacBuy = itemShops3[x3].BacBuy;
							shopTemplate2.ItemShops.Add(itemShop7);
							int num7 = l;
							l = num7 + 1;
							num7 = x3;
							x3 = num7 + 1;
						});
					}
					break;
				}
				case 21:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 0).ToList();
					shopTemplate2.ItemShops.AddRange(itemShopGirls.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 0));
					break;
				case 22:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 2).ToList();
					shopTemplate2.ItemShops.AddRange(itemShopGirls.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 2));
					break;
				case 23:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 4).ToList();
					shopTemplate2.ItemShops.AddRange(itemShopGirls.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 4));
					break;
				case 24:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 6).ToList();
					shopTemplate2.ItemShops.AddRange(itemShopGirls.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 6));
					break;
				case 25:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 8).ToList();
					shopTemplate2.ItemShops.AddRange(itemShopGirls.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 8));
					break;
				case 26:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 3).ToList();
					break;
				case 27:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 5).ToList();
					break;
				case 28:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 7).ToList();
					break;
				case 29:
					shopTemplate2.ItemShops = itemShops.Where((ItemShop s) => DataServer.ArrItemTemplate[s.IdItem].Type == 9).ToList();
					break;
				}
				DataShop.ShopTemplates.Add(shopTemplate2);
			}
		}

		public static void CreateItemShopVuKhi()
		{
			sbyte MaxSize = 6;
			int c = 0;
			string Text = "INSERT INTO itemshop set TypeShop =@1,CanSale =@2 ,ItemId = @3,Idclass = @4,GioiTinh = @5,IsLock = @6,HSD = @7,TinhThachBuy = @8,VangBuy = @9,VangKhoaBuy = @10,BacBuy =@11  ,BacKhoaBuy = @12,Options = @13";
			MySqlConnection con = Connection.getConnection();
			try
			{
				con.Open();
				MySqlCommand cmd = con.CreateCommand();
				cmd.CommandText = Text;
				for (int i = 1; i <= 5; i++)
				{
					c = 0;
					int j = i - 1;
					if (i != -1)
					{
						j = i - 1;
					}
					if (j != 0)
					{
						j *= MaxSize;
					}
					DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.Type == 1 && s.IdClass == i).ToList().ForEach(delegate(ItemTemplate s)
					{
						cmd.Parameters.AddWithValue("@1", 8);
						cmd.Parameters.AddWithValue("@2", 1);
						cmd.Parameters.AddWithValue("@3", s.Id);
						cmd.Parameters.AddWithValue("@4", DataServer.ArrItemTemplate[s.Id].IdClass);
						cmd.Parameters.AddWithValue("@5", DataServer.ArrItemTemplate[s.Id].GioiTinh);
						cmd.Parameters.AddWithValue("@6", 1);
						cmd.Parameters.AddWithValue("@7", -1);
						cmd.Parameters.AddWithValue("@8", 0);
						cmd.Parameters.AddWithValue("@9", 0);
						cmd.Parameters.AddWithValue("@10", 0);
						cmd.Parameters.AddWithValue("@11", MoneyVuKhi[c]);
						cmd.Parameters.AddWithValue("@12", 0);
						cmd.Parameters.AddWithValue("@13", DataShop.VuKhiStr[j]);
						cmd.ExecuteNonQuery();
						cmd.Dispose();
						cmd.Parameters.Clear();
						cmd.CommandText = Text;
						int num = c;
						c = num + 1;
						j++;
					});
				}
				cmd.Dispose();
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Close();
				con.Dispose();
			}
		}

		public static void CreateItemShopPhuKien()
		{
			sbyte MaxSize = 6;
			int c = 0;
			string Text = "INSERT INTO itemshop set CanSale =@1 ,Typeshop = @2,ItemId = @3,Idclass = @4,GioiTinh = @5,IsLock = @6,HSD = @7,TinhThachBuy = @8,VangBuy = @9,VangKhoaBuy = @10,BacBuy =@11  ,BacKhoaBuy = @12,Options = @13";
			MySqlConnection con = Connection.getConnection();
			try
			{
				con.Open();
				sbyte TypeShow = -1;
				string[] Options = null;
				int o = 0;
				MySqlCommand cmd = con.CreateCommand();
				try
				{
					cmd.CommandText = Text;
					sbyte j = 5;
					for (int x = 14; x <= 17; x++)
					{
						o = 0;
						if (x == 16 || x == 17)
						{
							j = 6;
						}
						switch (x)
						{
						case 14:
							TypeShow = 3;
							Options = DataShop.DayThungStr;
							break;
						case 15:
							TypeShow = 5;
							Options = DataShop.MocSatStr;
							break;
						case 16:
							TypeShow = 7;
							Options = DataShop.MocSatStr;
							break;
						case 17:
							TypeShow = 9;
							Options = DataShop.TuiNhanGiaStr;
							break;
						}
						for (int i = 1; i <= 5; i++)
						{
							c = 0;
							int k = i - 1;
							if (i != -1)
							{
								k = i - 1;
							}
							if (k != 0)
							{
								k *= j;
							}
							DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.Type == TypeShow).ToList().ForEach(delegate(ItemTemplate s)
							{
								cmd.Parameters.AddWithValue("@1", 1);
								cmd.Parameters.AddWithValue("@2", x);
								cmd.Parameters.AddWithValue("@3", s.Id);
								cmd.Parameters.AddWithValue("@4", i);
								cmd.Parameters.AddWithValue("@5", 2);
								cmd.Parameters.AddWithValue("@6", 1);
								cmd.Parameters.AddWithValue("@7", -1);
								cmd.Parameters.AddWithValue("@8", 0);
								cmd.Parameters.AddWithValue("@9", 0);
								cmd.Parameters.AddWithValue("@10", 0);
								if (x == 17)
								{
									cmd.Parameters.AddWithValue("@11", MoneyPhuKienTuiNhanGia[c]);
								}
								else
								{
									cmd.Parameters.AddWithValue("@11", MoneyPhuKienDTMSOT[c]);
								}
								cmd.Parameters.AddWithValue("@12", 0);
								cmd.Parameters.AddWithValue("@13", Options[k]);
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								cmd.CommandText = Text;
								int num = o;
								o = num + 1;
								num = c;
								c = num + 1;
								k++;
							});
						}
					}
					cmd.Dispose();
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Close();
				con.Dispose();
			}
		}

		public static void CreateShopTrangPhuc()
		{
			sbyte MaxSize = 6;
			int c = 0;
			string Text = "INSERT INTO itemshop set CanSale =@1 ,Typeshop = @2,ItemId = @3,Idclass = @4,GioiTinh = @5,IsLock = @6,HSD = @7,TinhThachBuy = @8,VangBuy = @9,VangKhoaBuy = @10,BacBuy =@11  ,BacKhoaBuy = @12,Options = @13";
			MySqlConnection con = Connection.getConnection();
			try
			{
				con.Open();
				sbyte TypeShow = -1;
				string[] Options = null;
				sbyte IdClass = 0;
				MySqlCommand cmd = con.CreateCommand();
				try
				{
					cmd.CommandText = Text;
					sbyte j = 5;
					for (int x = 9; x <= 13; x++)
					{
						if (x == 16 || x == 17)
						{
							j = 6;
						}
						switch (x)
						{
						case 9:
							TypeShow = 0;
							IdClass = 0;
							Options = DataShop.DaiTrangStr;
							break;
						case 10:
							TypeShow = 2;
							Options = DataShop.DaiTrangStr;
							break;
						case 11:
							TypeShow = 4;
							Options = DataShop.GiayStr2;
							break;
						case 12:
							TypeShow = 6;
							Options = DataShop.GiayStr;
							break;
						case 13:
							TypeShow = 8;
							Options = DataShop.GiayStr;
							break;
						}
						for (int i = 1; i <= 5; i++)
						{
							c = 0;
							int k = i - 1;
							if (i != -1)
							{
								k = i - 1;
							}
							if (k != 0)
							{
								k *= j;
							}
							Util.ShowLog("L " + k);
							DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.Type == TypeShow).ToList().ForEach(delegate(ItemTemplate s)
							{
								cmd.Parameters.AddWithValue("@1", 1);
								cmd.Parameters.AddWithValue("@2", x);
								cmd.Parameters.AddWithValue("@3", s.Id);
								cmd.Parameters.AddWithValue("@4", i);
								cmd.Parameters.AddWithValue("@5", DataServer.ArrItemTemplate[s.Id].GioiTinh);
								cmd.Parameters.AddWithValue("@6", 1);
								cmd.Parameters.AddWithValue("@7", -1);
								cmd.Parameters.AddWithValue("@8", 0);
								cmd.Parameters.AddWithValue("@9", 0);
								cmd.Parameters.AddWithValue("@10", 0);
								cmd.Parameters.AddWithValue("@11", MoneyShopPhuKien[c]);
								cmd.Parameters.AddWithValue("@12", 0);
								cmd.Parameters.AddWithValue("@13", Options[k]);
								cmd.ExecuteNonQuery();
								cmd.Parameters.Clear();
								cmd.CommandText = Text;
								Util.ShowLog("NAME " + DataServer.ArrItemTemplate[s.Id].Name + " OPION " + MoneyPhuKienDTMSOT[c]);
								int num = c;
								c = num + 1;
								if (MoneyShopPhuKien.Length - 1 <= c)
								{
									c = 0;
								}
								k++;
								if (Options.Length - 1 <= k)
								{
									k = 0;
								}
							});
						}
					}
					cmd.Dispose();
				}
				finally
				{
					if (cmd != null)
					{
						((IDisposable)cmd).Dispose();
					}
				}
				con.Dispose();
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				con.Clone();
				con.Dispose();
			}
		}

		public static void CreateShopVang()
		{
			string Text = "INSERT INTO itemshop set CanSale =@1 ,Typeshop = @2,ItemId = @3,Quantity =@4,Idclass = @5,GioiTinh = @6,IsLock = @7,HSD = @8,TinhThachBuy = @9,VangBuy = @10,VangKhoaBuy = @11,BacBuy =@12  ,BacKhoaBuy = @13,Options = @14";
			using MySqlConnection con = Connection.getConnection();
			con.Open();
			using MySqlCommand cmd = con.CreateCommand();
			cmd.CommandText = Text;
			int j = 0;
			int c = 0;
			sbyte Type = -1;
			for (int i = 0; i < IdShopCuaHangKhuVang.Length; i++)
			{
				int BuyVang = 0;
				int BuyVangKhoa = 0;
				int Quantity = 1;
				string Options = "";
				string[] Item = IdShopCuaHangKhuVang[i].Split(",");
				short Id = short.Parse(Item[0]);
				switch (j)
				{
				case 32:
					c = 0;
					break;
				case 40:
					c = 0;
					break;
				}
				if (j < 32)
				{
					Type = 6;
				}
				else if (j >= 32 && j < 40)
				{
					Type = 7;
				}
				else if (j >= 40)
				{
					Type = 39;
				}
				switch (Type)
				{
				case 6:
					BuyVang = MoneyCuaHangVang[c];
					break;
				case 7:
					BuyVangKhoa = MoneyCuaHangVangKhoa[c];
					break;
				case 39:
					BuyVang = MoneyCuaHangKhuRank[c];
					break;
				}
				Quantity = ((Item.Length > 1) ? int.Parse(Item[1]) : Quantity);
				if (i == IdShopCuaHangKhuVang.Length - 1)
				{
					Options = "0,1000,2000;1,1000,20000;2,50,100;258,50,100;172,10,15;168,10,15;306,50,100;307,50,100";
				}
				j++;
				c++;
				Util.ShowLog("TYPE " + Type + " ID  " + Id + " QUANTITY " + Quantity + " OPTIONS " + Options + "BUY VANG " + BuyVang + " BUY VANG KHOA " + BuyVangKhoa);
				cmd.Parameters.AddWithValue("@1", 1);
				cmd.Parameters.AddWithValue("@2", Type);
				cmd.Parameters.AddWithValue("@3", Id);
				cmd.Parameters.AddWithValue("@4", Quantity);
				cmd.Parameters.AddWithValue("@5", -1);
				cmd.Parameters.AddWithValue("@6", DataServer.ArrItemTemplate[Id].GioiTinh);
				cmd.Parameters.AddWithValue("@7", 1);
				cmd.Parameters.AddWithValue("@8", -1);
				cmd.Parameters.AddWithValue("@9", 0);
				cmd.Parameters.AddWithValue("@10", BuyVang);
				cmd.Parameters.AddWithValue("@11", BuyVangKhoa);
				cmd.Parameters.AddWithValue("@12", 0);
				cmd.Parameters.AddWithValue("@13", 0);
				cmd.Parameters.AddWithValue("@14", Options);
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				cmd.CommandText = Text;
			}
			cmd.Dispose();
		}

		public static void CreateShopDuocPham()
		{
			try
			{
				string Text = "INSERT INTO itemshop set CanSale =@1 ,Typeshop = @2,ItemId = @3,Quantity =@4,Idclass = @5,GioiTinh = @6,IsLock = @7,HSD = @8,TinhThachBuy = @9,VangBuy = @10,VangKhoaBuy = @11,BacBuy =@12  ,BacKhoaBuy = @13,Options = @14";
				using MySqlConnection con = Connection.getConnection();
				con.Open();
				int l = 0;
				using MySqlCommand cmd = con.CreateCommand();
				int k = 0;
				for (int i = 0; i < 2; i++)
				{
					l = 0;
					for (int j = 0; j < IdShopDuocPham.Length; j++)
					{
						if (l == 6)
						{
							l = 0;
						}
						cmd.CommandText = Text;
						cmd.Parameters.AddWithValue("@1", 1);
						cmd.Parameters.AddWithValue("@2", i);
						cmd.Parameters.AddWithValue("@3", IdShopDuocPham[j]);
						cmd.Parameters.AddWithValue("@4", 1);
						cmd.Parameters.AddWithValue("@5", -1);
						cmd.Parameters.AddWithValue("@6", 2);
						cmd.Parameters.AddWithValue("@7", 1);
						cmd.Parameters.AddWithValue("@8", -1);
						cmd.Parameters.AddWithValue("@9", 0);
						cmd.Parameters.AddWithValue("@10", 0);
						cmd.Parameters.AddWithValue("@11", 0);
						cmd.Parameters.AddWithValue("@12", (i == 0) ? MoneyDuocPham[l] : 0);
						cmd.Parameters.AddWithValue("@13", (i == 1) ? MoneyDuocPham[l] : 0);
						cmd.Parameters.AddWithValue("@14", "");
						cmd.ExecuteNonQuery();
						cmd.Parameters.Clear();
						cmd.CommandText = Text;
						cmd.Dispose();
						Util.ShowLog("I_ " + i + " ID " + IdShopDuocPham[j]);
						l++;
						k++;
					}
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
		}

		public static void ReloadBiKip()
		{
			string Text = "INSERT INTO itemshop set CanSale =@1 ,Typeshop = @2,ItemId = @3,Quantity =@4,Idclass = @5,GioiTinh = @6,IsLock = @7,HSD = @8,TinhThachBuy = @9,VangBuy = @10,VangKhoaBuy = @11,BacBuy =@12  ,BacKhoaBuy = @13,Options = @14";
			using MySqlConnection con = Connection.getConnection();
			con.Open();
			using MySqlCommand cmd = con.CreateCommand();
			List<ItemTemplate> itemTemplates = DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.IdClass > 0 && s.Type == 11).ToList();
			itemTemplates = itemTemplates.OrderBy((ItemTemplate s) => s.IdClass).ToList();
			int x = 0;
			for (int i = 0; i < itemTemplates.Count; i++)
			{
				cmd.CommandText = Text;
				cmd.Parameters.AddWithValue("@1", 1);
				cmd.Parameters.AddWithValue("@2", 19);
				cmd.Parameters.AddWithValue("@3", itemTemplates[i].Id);
				cmd.Parameters.AddWithValue("@4", 1);
				cmd.Parameters.AddWithValue("@5", itemTemplates[i].IdClass);
				cmd.Parameters.AddWithValue("@6", itemTemplates[i].GioiTinh);
				cmd.Parameters.AddWithValue("@7", 1);
				cmd.Parameters.AddWithValue("@8", -1);
				cmd.Parameters.AddWithValue("@9", DataShop.BuyTinhThachBiKip[x]);
				cmd.Parameters.AddWithValue("@10", 0);
				cmd.Parameters.AddWithValue("@11", 0);
				cmd.Parameters.AddWithValue("@12", 0);
				cmd.Parameters.AddWithValue("@13", 0);
				cmd.Parameters.AddWithValue("@14", DataShop.BiKipStr[i]);
				cmd.ExecuteNonQuery();
				cmd.Parameters.Clear();
				cmd.CommandText = Text;
				cmd.Dispose();
				x++;
				if (x >= 3)
				{
					x = 0;
				}
			}
		}

		public static void CreateShopAll()
		{
			string Text = "INSERT INTO listshop set CanSale =@1 ,Typeshop = @2,ItemId = @3,Quantity =@4,Idclass = @5,GioiTinh = @6,IsLock = @7,HSD = @8,TinhThachBuy = @9,VangBuy = @10,VangKhoaBuy = @11,BacBuy =@12  ,BacKhoaBuy = @13,MoneyHokage =@14,Options = @15";
			int i = 0;
			using MySqlConnection con = Connection.getConnection();
			con.Open();
			using MySqlCommand cmd = con.CreateCommand();
			foreach (ShopTemplate Shop in DataShop.ShopTemplates)
			{
				Thread.Sleep(500);
				foreach (ItemShop ItemShop in Shop.ItemShops)
				{
					cmd.CommandText = Text;
					cmd.Parameters.AddWithValue("@1", 1);
					cmd.Parameters.AddWithValue("@2", Shop.TypeShop);
					cmd.Parameters.AddWithValue("@3", ItemShop.IdItem);
					cmd.Parameters.AddWithValue("@4", ItemShop.Quantity);
					cmd.Parameters.AddWithValue("@5", ItemShop.IdClass);
					cmd.Parameters.AddWithValue("@6", ItemShop.GioiTinh);
					cmd.Parameters.AddWithValue("@7", 1);
					cmd.Parameters.AddWithValue("@8", ItemShop.HSD);
					cmd.Parameters.AddWithValue("@9", ItemShop.TinhThachBuy);
					cmd.Parameters.AddWithValue("@10", ItemShop.VangBuy);
					cmd.Parameters.AddWithValue("@11", ItemShop.VangKhoaBuy);
					cmd.Parameters.AddWithValue("@12", ItemShop.BacBuy);
					cmd.Parameters.AddWithValue("@13", ItemShop.BacKhoaBuy);
					cmd.Parameters.AddWithValue("@14", ItemShop.MoneyHokage);
					cmd.Parameters.AddWithValue("@15", ItemShop.Options);
					cmd.ExecuteNonQuery();
					cmd.Parameters.Clear();
					cmd.CommandText = Text;
					cmd.Dispose();
					i++;
				}
			}
		}

		public static void CreateShopNgoaiTrang()
		{
			string Text = "INSERT INTO itemshop set CanSale =@1 ,Typeshop = @2,ItemId = @3,Quantity =@4,Idclass = @5,GioiTinh = @6,IsLock = @7,HSD = @8,TinhThachBuy = @9,VangBuy = @10,VangKhoaBuy = @11,BacBuy =@12  ,BacKhoaBuy = @13,Options = @14";
			sbyte[] TypeItem = new sbyte[5] { 18, 30, 19, 37, 38 };
			short[] IdAoChoang = new short[9] { 128, 129, 130, 131, 132, 133, 535, 124, 345 };
			Util.ShowLog("DONE ");
			using MySqlConnection con = Connection.getConnection();
			sbyte TypeFind = -1;
			List<ItemTemplate> itemTemplates = null;
			con.Open();
			short ValueCt = 5;
			short TinhThachBuyCaiTrang = 500;
			int TinhTachBuyThuNuoi = 500;
			using MySqlCommand cmd = con.CreateCommand();
			for (int i = 0; i < TypeItem.Length; i++)
			{
				sbyte Type = TypeItem[i];
				switch (Type)
				{
				case 18:
					TypeFind = 12;
					itemTemplates = DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.LevelNeed >= 15 && s.GioiTinh == 2 && s.Type == TypeFind && s.IdClass == 0).ToList();
					itemTemplates = itemTemplates.OrderBy((ItemTemplate s) => s.LevelNeed).ToList();
					break;
				case 30:
					itemTemplates = DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.Id == 570 || s.Id == 237 || s.Id == 703).ToList();
					break;
				case 19:
					itemTemplates = DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.IdClass > 0 && s.Type == 11).ToList();
					itemTemplates = itemTemplates.OrderBy((ItemTemplate s) => s.IdClass).ToList();
					break;
				case 37:
					itemTemplates = DataServer.ArrItemTemplate.Where((ItemTemplate s) => s.GioiTinh == 2 && s.Type == 10 && s.IdClass == 0).ToList();
					itemTemplates = itemTemplates.OrderBy((ItemTemplate s) => s.LevelNeed).ToList();
					break;
				}
				short Size = (short)((Type != 38) ? itemTemplates.Count : DataShop.IdCaiTrangShop.Length);
				if (Type == 30)
				{
					Size *= 5;
				}
				Util.ShowLog("DONE TYPE" + Type + " SIZE " + Size);
				short x = 0;
				for (int j = 0; j < Size; j++)
				{
					int IdItem = 0;
					string Option = "";
					int MoneyBuy = 0;
					sbyte IdClass = -1;
					sbyte GioiTinh = -1;
					switch (Type)
					{
					case 30:
						IdItem = itemTemplates[x].Id;
						IdClass = itemTemplates[x].IdClass;
						GioiTinh = itemTemplates[x].GioiTinh;
						break;
					case 38:
						IdItem = DataShop.IdCaiTrangShop[j];
						break;
					default:
						IdItem = itemTemplates[j].Id;
						IdClass = itemTemplates[j].IdClass;
						GioiTinh = itemTemplates[j].GioiTinh;
						break;
					}
					switch (Type)
					{
					case 18:
						Option = DataShop.AoChoangStr[j];
						MoneyBuy = DataShop.MoneyAoChoang[j];
						break;
					case 30:
						Option = DataShop.TantoOptionStr[j];
						MoneyBuy = DataShop.BuyTinhThachBiKip[x];
						break;
					case 19:
						Option = DataShop.BiKipStr[j];
						MoneyBuy = DataShop.BuyTinhThachBiKip[x];
						break;
					case 37:
						MoneyBuy = TinhTachBuyThuNuoi;
						Option = "255," + ValueCt;
						break;
					case 38:
						Option = "255," + ValueCt;
						break;
					}
					cmd.CommandText = Text;
					cmd.Parameters.AddWithValue("@1", 1);
					cmd.Parameters.AddWithValue("@2", Type);
					cmd.Parameters.AddWithValue("@3", IdItem);
					cmd.Parameters.AddWithValue("@4", 1);
					cmd.Parameters.AddWithValue("@5", IdClass);
					cmd.Parameters.AddWithValue("@6", GioiTinh);
					cmd.Parameters.AddWithValue("@7", 1);
					cmd.Parameters.AddWithValue("@8", -1);
					cmd.Parameters.AddWithValue("@9", (Type == 38) ? TinhThachBuyCaiTrang : MoneyBuy);
					cmd.Parameters.AddWithValue("@10", 0);
					cmd.Parameters.AddWithValue("@11", 0);
					cmd.Parameters.AddWithValue("@12", 0);
					cmd.Parameters.AddWithValue("@13", 0);
					cmd.Parameters.AddWithValue("@14", Option);
					cmd.ExecuteNonQuery();
					cmd.Parameters.Clear();
					cmd.CommandText = Text;
					cmd.Dispose();
					Util.ShowLog("DONE J" + j);
					if (Type == 30 || Type == 19)
					{
						x++;
						if (x >= 3)
						{
							x = 0;
						}
						continue;
					}
					switch (Type)
					{
					case 38:
						if (j == DataShop.IdCaiTrangShop.Length - 2)
						{
							TinhThachBuyCaiTrang += 500;
							ValueCt += 25;
						}
						else
						{
							TinhThachBuyCaiTrang += 100;
							ValueCt += 5;
						}
						break;
					case 37:
						TinhTachBuyThuNuoi += 200;
						if (j == DataShop.IdCaiTrangShop.Length - 2)
						{
							TinhThachBuyCaiTrang += 500;
							ValueCt += 25;
						}
						else
						{
							TinhThachBuyCaiTrang += 100;
							ValueCt += 5;
						}
						break;
					}
				}
			}
		}

		private static string[] CreateShopPhucLoi()
		{
			return new string[128]
			{
				"464,1,10 phút,255@30", "192,10,20 phút", "163,50000,30 phút", "192,10,60 phút", "163,150000,90 phút", "192,30,120 phút", "163,500000,180 phút", "192,60,240 phút", "267,1,1 ngày", "643,2,2 ngày",
				"277,1,3 ngày", "644,3,4 ngày", "285,2,5 ngày", "161,1,6 ngày", "192,100,7 ngày", "347,1,Cấp 10", "517,1,Cấp 20,137@20@174@5@255@47", "169,1,Cấp 30", "162,1,Cấp 35", "158,1,Cấp 40",
				"177,1,Cấp 45", "435,1,Cấp 50", "688,1,Cấp 55", "163,50000,20 Vàng", "192,50,50 Vàng", "163,150000,100 Vàng", "192,150,250 Vàng", "163,500000,500 Vàng", "192,500,1000 Vàng", "812,1,1500 Vàng,0@1000@1@1000",
				"163,200000,80 Vàng", "192,100,160 Vàng", "163,500000,220 Vàng", "192,220,500 Vàng", "163,1500000,1000 Vàng", "161,20,1700 Vàng", "277,50,2300 Vàng", "163,100000,Môc 25 vàng", "163,200000,Mốc 75 vàng", "163,300000,Mốc 145 vàng",
				"163,500000,Mốc 250 vàng", "163,900000,Mốc 390 vàng", "163,2000000,Mốc 500 vàng", "161,1,Mốc 100 vàng", "277,2,Mốc 400 vàng", "163,1100000,Mốc 1000 vàng", "277,10,Mốc 3000 vàng", "163,5000000,Mốc 5000 Vàng", "277,25,Mốc 10000 vàng", "163,10000000,Mốc 15000 vàng",
				"277,50,Mốc 20000 vàng", "163,20000000,Mốc 25000 vàng", "277,1,Ngày 1", "428,2,Ngày 2", "464,1,Ngày 3,255@30", "437,1,Ngày 4", "161,2,Ngày 5", "277,5,Ngày 6", "463,1,Ngày 7,255@75", "277,1,Mốc 100 Vàng",
				"277,3,Mốc 100 Vàng ", "277,5,Mốc 100 Vàng", "161,1,Mốc 500 Vàng", "161,3,Mốc 500 Vàng", "161,5,Mốc 500 Vàng", "231,1,Mốc 1000 Vàng", "231,3,Mốc 1000 Vàng", "231,5,Mốc 1000 Vàng", "435,1,Mốc 50 Vàng", "435,3,Mốc 200 Vàng",
				"435,8,Mốc 600 Vàng", "719,1,Mốc 1300 Vàng", "778,1,Mốc 2000 Vàng", "417,1,10 Vàng", "418,1, 50 Vàng", "419,1,100 Vàng", "420,1, 500 Vàng", "421,1,1000 Vàng", "422,1,5000 Vàng", "423,1,20000 Vàng",
				"424,1,50000 Vàng", "425,1,120000 Vàng", "426,1,500000 Vàng", "192,100,Rank 5", "192,500,Rank 6", "192,1000,Rank 7", "192,2000,Rank 8", "192,5000,Rank 9", "466,1,Sập Game :)),0@5000@0@5000@2@500@63@100@92@10@173@200@371@10", "428,1,50 Rank",
				"428,3,100 Rank", "428,5,200 Rank", "428,7,300 Rank", "428,10,500 Rank", "428,20,1000 Rank", "428,50,2000 Rank", "326,1,Cấp 10,209@10", "327,1,Cấp 15,209@15", "518,1,Cấp 20,209@20", "520,1,Cấp 25,209@25",
				"521,1,Cấp 30,209@30", "524,1,Cấp 35,209@35", "525,1,Cấp 40,209@40", "527,1,Cấp 45,209@45", "528,1,Cấp 50,209@50", "529,1,Cấp 55,209@55", "530,1,Cấp 60,209@60", "623,1,Cấp 65,209@65", "513,1,Cấp 10,209@10@0@100@1@100@2@10", "514,1,Cấp 15,209@15@0@200@1@200@2@20",
				"515,1,Cấp 20,209@20@0@300@1@300@2@30", "653,1,Cấp 25,209@25@0@400@1@400@2@40", "662,1,Cấp 30,209@30@0@500@1@500@2@50", "672,1,Cấp 35,209@35@0@600@1@600@2@60", "677,1,Cấp 40,209@40@0@700@1@700@2@70", "682,1,Cấp 45,209@45@0@800@1@800@2@80", "702,1,Cấp 50,209@50@0@900@1@900@2@90", "726,1,Cấp 55,209@55@0@1000@1@1000@2@100", "886,1,Cấp 60,209@60@0@1100@1@1100@2@110", "775,1,Cấp 62,209@65@0@1200@1@1200@2@120",
				"277,5,10 Người mua", "277,10,100 Người mua", "277,20,200 Người mua", "277,40,500 Ngươi mua", "277,5,10 Người mua", "277,10,100 Người mua", "277,20,200 Người mua", "277,40,500 Ngươi mua"
			};
		}
	}
}
