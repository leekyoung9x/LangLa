using System;
using System.Collections.Generic;
using System.Linq;
using LangLa.Client;
using LangLa.Hander;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SupportOOP;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace LangLa.SqlConnection
{
	public static class ConnectionUser
	{
		private static readonly object _Lock_User = new object();

		private static readonly object _Lock_GetChar = new object();

		private static readonly object _Lock_CreateChar = new object();

		private static readonly object _Lock_CheckName = new object();

		private static readonly object _Lock_UpdateChar = new object();

		private static readonly object _Lock_CallCard = new object();

		private static readonly object _Lock_UpdateMoney = new object();

		public static bool GetUser(LangLa.Client.Client _Client, string uu, string pp)
		{
			lock (_Lock_User)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				MySqlDataReader read = null;
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = "SELECT * from users where username = @1 and pass = @2";
					cmd.Parameters.AddWithValue("@1", uu);
					cmd.Parameters.AddWithValue("@2", pp);
					read = cmd.ExecuteReader();
					if (read.Read())
					{
						_Client.IdUser = read.GetInt32("id");
						_Client.QuantityChar = read.GetSByte("quantitychar");
						_Client.Money = read.GetInt32("Money");
						_Client.TotalMoney = read.GetInt32("Totalmoney");
						string NameArr = read.GetString("arrnaem");
						if (!NameArr.Equals(""))
						{
							_Client.ArrChar = JsonConvert.DeserializeObject<string[]>(read.GetString("arrnaem"));
						}
						return true;
					}
					return false;
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
			}
			return false;
		}

		public static bool CheckNameUser(string Name)
		{
			bool IsOk = true;
			lock (_Lock_CheckName)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = "SELECT ID_USER from characters where Name =@1";
					cmd.Parameters.AddWithValue("@1", Name);
					int result = Convert.ToInt32(cmd.ExecuteScalar());
					if (result > 0)
					{
						IsOk = false;
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
			return IsOk;
		}

		public static List<Character> GetCharDb(int IdUser, int Quantity)
		{
			lock (_Lock_GetChar)
			{
				List<Character> characters = new List<Character>();
				string Text = "SELECT * from characters where id_user = @1";
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				MySqlDataReader read = null;
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = Text;
					cmd.Parameters.AddWithValue("@1", IdUser);
					read = cmd.ExecuteReader();
					try
					{
						while (read.Read())
						{
							Character Char = new Character();
							Char.Index = read.GetSByte("index_char");
							Char.Task = JsonConvert.DeserializeObject<TaskChar>(read.GetString("task"));
							Char.Info = JsonConvert.DeserializeObject<LangLa.InfoChar.InfoChar>(read.GetString("info"));
							Char.Inventory = JsonConvert.DeserializeObject<InfoInventory>(read.GetString("inventory"));
							Char.TimeChar = JsonConvert.DeserializeObject<InfoCharTime>(read.GetString("timeChar"));
							Char.Inventory.ItemBody = JsonConvert.DeserializeObject<List<Item>>(read.GetString("itembody"));
							Char.Inventory.ItemBody2 = JsonConvert.DeserializeObject<List<Item>>(read.GetString("itembody2"));
							Char.Inventory.ItemBox = JsonConvert.DeserializeObject<List<Item>>(read.GetString("itembox"));
							Char.Inventory.ItemBag = JsonConvert.DeserializeObject<List<Item>>(read.GetString("itembag"));
							Char.Inventory.ItemVyThu = JsonConvert.DeserializeObject<List<Item>>(read.GetString("itemvythu"));
							Char.Skill = JsonConvert.DeserializeObject<InfoSkill>(read.GetString("skill"));
							Char.Point = JsonConvert.DeserializeObject<InfoPoint>(read.GetString("point"));
							Char.Effs = JsonConvert.DeserializeObject<List<InfoEff>>(read.GetString("listEffs"));
							Char.TuongKhac = JsonConvert.DeserializeObject<InfoTuongKhac>(read.GetString("tuongkhac"));
							Char.Thus = JsonConvert.DeserializeObject<List<InfoThu>>(read.GetString("thu"));
							Char.GiftCode = JsonConvert.DeserializeObject<InfoGiftCode>(read.GetString("Code"));
							Char.Friends = JsonConvert.DeserializeObject<List<InfoFriend>>(read.GetString("friends"));
							Char.Enemies = JsonConvert.DeserializeObject<List<InfoEnemy>>(read.GetString("enemies"));
							Char.DanhHieus = JsonConvert.DeserializeObject<List<InfoDanhHieu>>(read.GetString("danhhieu"));
							Char.ViThu = JsonConvert.DeserializeObject<InfoViThu>(read.GetString("vithu"));
							characters.Add(Char);
						}
					}
					catch (Exception e2)
					{
						Util.ShowErr(e2);
					}
					finally
					{
						read.DisposeAsync();
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
					read?.DisposeAsync();
				}
				return characters;
			}
		}

		public static void CreateDbChar(Character _myChar, string[] ArrChar)
		{
			lock (_Lock_CreateChar)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					if (ArrChar == null)
					{
						ArrChar = new string[3];
					}
					sbyte Index = (sbyte)ArrChar.ToList().FindIndex((string s) => s == null);
					if (Index == -1)
					{
						Index = 0;
					}
					cmd.CommandText = "INSERT INTO Characters set ID_USER = @1,Index_Char = @2,Name =@3,Task =@4,Info =@5,TimeChar =@6,Inventory =@7,Itembody=@8,Itembody2=@9,ItemBag=@10,ItemBox=@11,Itemvythu=@12,Skill=@13,ListEffs =@14,Point=@15,TuongKhac=@16,Thu=@17,Code=@18,Friends=@19,Enemies=@20,Danhhieu=@21,ViThu=@22";
					cmd.Parameters.AddWithValue("@1", _myChar.Info.IdUser);
					cmd.Parameters.AddWithValue("@2", Index);
					cmd.Parameters.AddWithValue("@3", _myChar.Info.Name);
					cmd.Parameters.AddWithValue("@4", JsonConvert.SerializeObject(_myChar.Task));
					cmd.Parameters.AddWithValue("@5", JsonConvert.SerializeObject(_myChar.Info));
					cmd.Parameters.AddWithValue("@6", JsonConvert.SerializeObject(_myChar.TimeChar));
					cmd.Parameters.AddWithValue("@7", JsonConvert.SerializeObject(_myChar.Inventory));
					cmd.Parameters.AddWithValue("@8", JsonConvert.SerializeObject(_myChar.Inventory.ItemBody));
					cmd.Parameters.AddWithValue("@9", JsonConvert.SerializeObject(_myChar.Inventory.ItemBody2));
					cmd.Parameters.AddWithValue("@10", JsonConvert.SerializeObject(_myChar.Inventory.ItemBag));
					cmd.Parameters.AddWithValue("@11", JsonConvert.SerializeObject(_myChar.Inventory.ItemBox));
					cmd.Parameters.AddWithValue("@12", JsonConvert.SerializeObject(_myChar.Inventory.ItemVyThu));
					cmd.Parameters.AddWithValue("@13", JsonConvert.SerializeObject(_myChar.Skill));
					cmd.Parameters.AddWithValue("@14", JsonConvert.SerializeObject(_myChar.Effs));
					cmd.Parameters.AddWithValue("@15", JsonConvert.SerializeObject(_myChar.Point));
					cmd.Parameters.AddWithValue("@16", JsonConvert.SerializeObject(_myChar.TuongKhac));
					cmd.Parameters.AddWithValue("@17", JsonConvert.SerializeObject(_myChar.Thus));
					cmd.Parameters.AddWithValue("@18", JsonConvert.SerializeObject(_myChar.GiftCode));
					cmd.Parameters.AddWithValue("@19", JsonConvert.SerializeObject(_myChar.Friends));
					cmd.Parameters.AddWithValue("@20", JsonConvert.SerializeObject(_myChar.Enemies));
					cmd.Parameters.AddWithValue("@21", JsonConvert.SerializeObject(_myChar.DanhHieus));
					cmd.Parameters.AddWithValue("@22", JsonConvert.SerializeObject(_myChar.ViThu));
					cmd.ExecuteNonQuery();
					cmd.Dispose();
					cmd.Parameters.Clear();
					cmd.CommandText = "UPDATE users Set quantitychar =@23 ,arrnaem =@24 where id =@25";
					cmd.Parameters.AddWithValue("@23", Index + 1);
					ArrChar[Index] = _myChar.Info.Name;
					cmd.Parameters.AddWithValue("@24", JsonConvert.SerializeObject(ArrChar));
					cmd.Parameters.AddWithValue("@25", _myChar.Info.IdUser);
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
		}

		public static void UpdateChar(Character _myChar)
		{
			lock (_Lock_UpdateChar)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = "Update Characters set Task =@4,Info =@5,TimeChar =@6,Inventory =@7,Itembody=@8,Itembody2=@9,ItemBag=@10,ItemBox=@11,Itemvythu=@12,Skill=@13,ListEffs =@14,Point=@15,TuongKhac=@16,Thu=@17,Code=@18,Friends=@19,Enemies=@20,Danhhieu=@21,Vithu=@22 where ID_USER =@23 and Index_char = @24";
					cmd.Parameters.AddWithValue("@4", JsonConvert.SerializeObject(_myChar.Task));
					cmd.Parameters.AddWithValue("@5", JsonConvert.SerializeObject(_myChar.Info));
					cmd.Parameters.AddWithValue("@6", JsonConvert.SerializeObject(_myChar.TimeChar));
					cmd.Parameters.AddWithValue("@7", JsonConvert.SerializeObject(_myChar.Inventory));
					cmd.Parameters.AddWithValue("@8", JsonConvert.SerializeObject(_myChar.Inventory.ItemBody));
					cmd.Parameters.AddWithValue("@9", JsonConvert.SerializeObject(_myChar.Inventory.ItemBody2));
					cmd.Parameters.AddWithValue("@10", JsonConvert.SerializeObject(_myChar.Inventory.ItemBag));
					cmd.Parameters.AddWithValue("@11", JsonConvert.SerializeObject(_myChar.Inventory.ItemBox));
					cmd.Parameters.AddWithValue("@12", JsonConvert.SerializeObject(_myChar.Inventory.ItemVyThu));
					cmd.Parameters.AddWithValue("@13", JsonConvert.SerializeObject(_myChar.Skill));
					cmd.Parameters.AddWithValue("@14", JsonConvert.SerializeObject(_myChar.Effs));
					cmd.Parameters.AddWithValue("@15", JsonConvert.SerializeObject(_myChar.Point));
					cmd.Parameters.AddWithValue("@16", JsonConvert.SerializeObject(_myChar.TuongKhac));
					cmd.Parameters.AddWithValue("@17", JsonConvert.SerializeObject(_myChar.Thus));
					cmd.Parameters.AddWithValue("@18", JsonConvert.SerializeObject(_myChar.GiftCode));
					cmd.Parameters.AddWithValue("@19", JsonConvert.SerializeObject(_myChar.Friends));
					cmd.Parameters.AddWithValue("@20", JsonConvert.SerializeObject(_myChar.Enemies));
					cmd.Parameters.AddWithValue("@21", JsonConvert.SerializeObject(_myChar.DanhHieus));
					cmd.Parameters.AddWithValue("@22", JsonConvert.SerializeObject(_myChar.ViThu));
					cmd.Parameters.AddWithValue("@23", _myChar.Info.IdUser);
					cmd.Parameters.AddWithValue("@24", _myChar.Index);
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
		}

		public static void CallBackCard(string Text)
		{
			if (Text.Length <= 0)
			{
				return;
			}
			int ID_USER = -1;
			try
			{
				ID_USER = int.Parse(Text);
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			Character _myChar = LangLa.Server.Server.GetChar(ID_USER);
			if (_myChar == null)
			{
				return;
			}
			lock (_Lock_CallCard)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlDataReader Read = null;
				MySqlCommand cmd = null;
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = "SELECT Money,TotalMoney from users where ID = @1";
					cmd.Parameters.AddWithValue("@1", ID_USER);
					Read = cmd.ExecuteReader();
					if (Read.Read())
					{
						int MoneyNew = Read.GetInt32("Money") / 100 * LangLa.Server.Server.XMoneyServer;
						int TotalMoney = Read.GetInt32("TotalMoney") / 100;
						if (MoneyNew > 0)
						{
							DownMoney(ID_USER);
							_myChar.TimeChar.TongSoVangDaNap = TotalMoney;
							_myChar.TimeChar.TimeNap = Util.CurrentTimeMillis();
							_myChar.Info.IsActive = true;
							ThuHander.CreateThuNapThe(_myChar, MoneyNew);
						}
					}
				}
				catch (Exception E)
				{
					Util.ShowErr(E);
				}
				finally
				{
					con.Close();
					con.Dispose();
					Read?.Close();
					Read?.DisposeAsync();
					cmd?.Dispose();
				}
			}
		}

		public static void DownMoney(int IdUser)
		{
			lock (_Lock_UpdateMoney)
			{
				MySqlConnection con = Connection.getConnection();
				MySqlCommand cmd = null;
				try
				{
					con.Open();
					cmd = con.CreateCommand();
					cmd.CommandText = "UPDATE users set Money ='0' where ID = @1";
					cmd.Parameters.AddWithValue("@1", IdUser);
					cmd.ExecuteNonQuery();
				}
				catch (Exception E)
				{
					Util.ShowErr(E);
				}
				finally
				{
					con.Close();
					con.Dispose();
					cmd?.Dispose();
				}
			}
		}
	}
}
