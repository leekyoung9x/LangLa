using System;
using System.Text;
using LangLa.Data;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SqlConnection;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class ClickEventHander
	{
		public static readonly sbyte GHEP_DA = 81;

		public static readonly sbyte CUONG_HOA = 82;

		public static readonly sbyte NANG_CAP_BUA_NO = 83;

		public static readonly sbyte TACH_CUONG_HOA = 84;

		public static readonly sbyte DICH_CHUYEN_TRANG_BI = 85;

		public static readonly sbyte KHAM_NGOC = 87;

		public static readonly sbyte TACH_NGOC_KHAM = 90;

		public static readonly sbyte GHEP_CAI_TRANG = 94;

		public static readonly sbyte TACH_CAI_TRANG = 95;

		private static readonly int[] PowerViThu = new int[10] { 10000, 20000, 40000, 60000, 90000, 140000, 180000, 260000, 360000, 500000 };

		private static readonly string[] InfoItemThuVanMay = new string[24]
		{
			"600,1", "723,1", "478,1,0@450@168@15@152@35", "163,10000", "163,50000", "163,100000", "481,1,0@650@0@55@152@45", "301,1", "704,1", "688,1",
			"763,250", "468,1", "4,3", "6,2", "10,1", "623,1,209@45@150@10", "163,1000000", "687,50", "687,100", "790,1",
			"150,1", "151,1", "153,1", "154,1"
		};

		public static void ShowGiaoVatPhat(Character _myChar)
		{
			Message i = new Message(122);
			i.WriteByte(80);
			_myChar.SendMessage(i);
		}

		public static void HanderClick(Character _myChar, Message msg)
		{
			try
			{
				sbyte IdEvent = msg.ReadByte();
				sbyte Index = -1;
				while (msg.available() > 0)
				{
					Index = msg.ReadByte();
				}
				if (IdEvent >= 8 && IdEvent <= 17)
				{
					ShopHander.OpenShopBodyChar(_myChar, IdEvent, Index);
					_myChar.InfoGame.ID_Click_SHOP = IdEvent;
					return;
				}
				if (IdEvent == 18 || IdEvent == 19 || IdEvent == 30 || IdEvent == 37 || IdEvent == 38)
				{
					ShopHander.OpenShopNgoaiTrang(_myChar, IdEvent, -1);
					_myChar.InfoGame.ID_Click_SHOP = IdEvent;
					return;
				}
				switch (IdEvent)
				{
				case 0:
				case 1:
					ShopHander.OpennShopType0(_myChar, IdEvent);
					break;
				case 4:
				case 5:
					ShopHander.OpenShopNpc4(_myChar, -1, IdEvent);
					break;
				case 20:
				case 21:
				case 22:
				case 23:
				case 24:
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
					ShopHander.OpenShopHokage(_myChar, IdEvent, Index);
					break;
				case 36:
					ShopHander.OpenShopNgoaiTrang(_myChar, IdEvent, Index);
					break;
				case 6:
				case 7:
				case 39:
				case 40:
					ShopHander.OpenShopCuaHang(_myChar, IdEvent);
					break;
				case 50:
					InventoryHander.WriteItemBox(_myChar);
					break;
				case 54:
					GiaTocHander.ShowListGiaToc(_myChar);
					break;
				case 56:
					ShowTabHoatDong(_myChar);
					break;
				case 81:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(GHEP_DA));
					break;
				case 82:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(CUONG_HOA));
					break;
				case 83:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(NANG_CAP_BUA_NO));
					break;
				case 84:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(TACH_CUONG_HOA));
					break;
				case 85:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(DICH_CHUYEN_TRANG_BI));
					break;
				case 87:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(KHAM_NGOC));
					break;
				case 90:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(TACH_NGOC_KHAM));
					break;
				case 94:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(GHEP_CAI_TRANG));
					break;
				case 95:
					_myChar.SendMessage(UtilMessage.OpenTabEvent(TACH_CAI_TRANG));
					break;
				case 86:
					_myChar.SendMessage(UtilMessage.OpenTabKhoBau(_myChar));
					break;
				case 88:
					ShowTabPhucLoi(_myChar);
					break;
				case 92:
					if (_myChar.Task.Id == 0 && _myChar.Task.IdStep == 1)
					{
						TaskHander.NextStep(_myChar);
					}
					break;
				}
				_myChar.InfoGame.ID_Click_SHOP = IdEvent;
			}
			catch (Exception E)
			{
				Util.ShowErr(E);
			}
		}

		public static Message ShowTabThuVanMay()
		{
			Message j = new Message(122);
			j.WriteByte(74);
			j.WriteShort((short)InfoItemThuVanMay.Length);
			string[] infoItemThuVanMay = InfoItemThuVanMay;
			foreach (string s in infoItemThuVanMay)
			{
				string[] Info = s.Split(",");
				short IdItem = short.Parse(Info[0]);
				j.WriteShort(short.Parse(Info[0]));
				j.WriteBool(x: true);
				j.WriteLong(-1L);
				if (DataServer.ArrItemTemplate[IdItem].Type <= 16)
				{
					j.WriteByte(-1);
					j.WriteByte(0);
					if (Info.Length > 2)
					{
						string Op = "";
						string[] Option = Info[2].Split("@");
						for (int i = 0; i < Option.Length; i++)
						{
							if (i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12 || i == 14)
							{
								Op += ";";
							}
							if (i == 1 || i == 3 || i == 5 || i == 7 || i == 9 || i == 11 || i == 13)
							{
								Op += ",";
							}
							Op += short.Parse(Option[i]);
						}
						j.WriteUTF(Op);
					}
					else
					{
						j.WriteUTF("");
					}
				}
				else
				{
					j.WriteInt(int.Parse(Info[1]));
				}
				j.WriteShort(1);
			}
			return j;
		}

		public static void HanderClickThuVanMay(Character _myChar)
		{
			if (_myChar.Inventory.Vang < 25)
			{
				return;
			}
			if (InventoryHander.GetCountNotNullBag(_myChar) > 0)
			{
				short IndexRd = (short)Util.NextInt(0, InfoItemThuVanMay.Length - 1);
				if (InfoItemThuVanMay[IndexRd] == null)
				{
					return;
				}
				short IdItem = short.Parse(InfoItemThuVanMay[IndexRd].Split(",")[0]);
				int Quantity = int.Parse(InfoItemThuVanMay[IndexRd].Split(",")[1]);
				Message j = UtilMessage.Message123();
				j.WriteByte(-85);
				j.WriteShort(IdItem);
				j.WriteInt(Quantity);
				_myChar.SendMessage(j);
				string[] Info = InfoItemThuVanMay[IndexRd].Split(",");
				Item item = new Item(IdItem, IsLock: true);
				item.Quantity = Quantity;
				string Op = "";
				if (Info.Length > 2)
				{
					string[] Option = Info[2].Split("@");
					for (int i = 0; i < Option.Length; i++)
					{
						if (i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12 || i == 14)
						{
							Op += ";";
						}
						if (i == 1 || i == 3 || i == 5 || i == 7 || i == 9 || i == 11 || i == 13)
						{
							Op += ",";
						}
						Op += short.Parse(Option[i]);
					}
				}
				item.Options = Op;
				InventoryHander.AddItemBag(_myChar, item);
				InventoryHander.UpdateVang(_myChar, 25, IsThongBao: true);
			}
			else
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Cần trống ít nhất 1 ô hành trang", Util.WHITE));
			}
		}

		private static void ShowTabHoatDong(Character _myChar)
		{
			Message j = new Message(122);
			j.WriteByte(56);
			j.WriteUTF("Làng lá lậu\\Làng lá lậu\\Làng lá lậu");
			j.WriteByte(18);
			for (int i = 0; i < 18; i++)
			{
				j.WriteUTF("Làng lá lậu");
				j.WriteUTF("Làng lá lậu");
			}
			_myChar.SendMessage(j);
		}

		public static void ShowTabPhucLoi(Character _myChar)
		{
			int TimeOnl = _myChar.TimeChar.getMinutes(IsTrue: true);
			short Day = _myChar.TimeChar.getDay(IsTrue: true);
			bool[] QuaOnline = _myChar.TimeChar.IsCanNhanQuaOnlie;
			sbyte Level = _myChar.Info.Level;
			bool[] Qua7Day = _myChar.TimeChar.IsCanNhanQuaOnline7Day;
			bool[] QuaThangCap = _myChar.TimeChar.IsCanNhanQuaThangCap;
			bool[] QuaTieuNgay = _myChar.TimeChar.IsCanNhanQuaTieuNgay;
			bool[] QuaTieuTuan = _myChar.TimeChar.IsCanNhanQuaTieuTuan;
			bool[] QuaNapNgay = _myChar.TimeChar.IsCanNhanQuaNapNgay;
			bool[] QuaNapTuan = _myChar.TimeChar.IsCanNhanQuaNapTuan;
			bool[] QuaNapLienTuc = _myChar.TimeChar.IsCanNhanQuaNapLienTuc;
			bool[] QuaNap3Moc = _myChar.TimeChar.IsCanNhanQuaNap3Moc;
			bool[] QuaNapDon = _myChar.TimeChar.IsCanNhanQuaNapDon;
			bool[] QuaRank = _myChar.TimeChar.IsCanNhanQuaRank;
			bool[] QuaRankChung = _myChar.TimeChar.IsCanNhanQuaRankChung;
			bool[] QuaRankTatCa = _myChar.TimeChar.IsCanNhanQuaRankTatCa;
			bool[] GoiHaoHoa = _myChar.TimeChar.IsCanNhanQuaGoiHaoHoa;
			bool[] GoiChiTon = _myChar.TimeChar.IsCanNhanQuaGoiChiTon;
			bool[] DauTuTatCa = _myChar.TimeChar.IsCanNhanQuaDauTuTatCa;
			bool[] TheThangTatCa = _myChar.TimeChar.IsCanNhanQuaTheThangTatCa;
			bool IsBuyGoiHaoHoa = _myChar.Info.IsBuyGoiHaoHoa;
			bool IsBuyGoiChiTon = _myChar.Info.IsBuyGoiChiTon;
			int SoVangNap3Moc = _myChar.TimeChar.SoVangNap3Moc;
			int SoVangTieuNgay = _myChar.TimeChar.SoVangTieuTrongNgay;
			int SoVangTieuTrongTuan = _myChar.TimeChar.SoVangTieuTrongTuan;
			int SoVangNapTrongNgay = _myChar.TimeChar.SoVangNapTrongNgay;
			int SoVangNapTrongTuan = _myChar.TimeChar.SoVangNapTrongTuan;
			short SoNgayNapLienTuc = _myChar.TimeChar.SoNgayNapLienTuc;
			int SoVangNapDon = _myChar.TimeChar.SoVangNapDon;
			int TongSoVangDaNap = _myChar.TimeChar.TongSoVangDaNap;
			sbyte RankCaoNhatServer = LangLa.Server.Server.RankCaoNhatServer;
			int TongSoRankServer = LangLa.Server.Server.RankAllServer;
			int DauTuAllServer = LangLa.Server.Server.DauTuAllServer;
			int TheThangAllServer = LangLa.Server.Server.TheThangAllServer;
			Console.WriteLine("Seconds " + TimeOnl + " Day " + Day);
			Message k = new Message(122);
			k.WriteByte(88);
			k.WriteInt(TimeOnl * 60 * 1000);
			k.WriteByte((sbyte)Day);
			k.WriteInt(TongSoVangDaNap);
			k.WriteInt(TongSoRankServer);
			k.WriteInt(SoVangNapTrongNgay);
			k.WriteInt(SoVangNapTrongTuan);
			k.WriteInt(SoVangTieuNgay);
			k.WriteInt(SoVangTieuTrongTuan);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteInt(TheThangAllServer);
			k.WriteBool(_myChar.Info.IsBuyGoiHaoHoa);
			k.WriteBool(_myChar.Info.IsBuyGoiChiTon);
			k.WriteInt(DauTuAllServer);
			k.WriteByte(RankCaoNhatServer);
			k.WriteBool(x: true);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteByte(0);
			k.WriteByte((sbyte)SoNgayNapLienTuc);
			k.WriteLong(Util.CurrentTimeMillis() + 50000);
			k.WriteShort((short)ConnectionDB.ListItemShopPhucLoi.Length);
			for (short i = 0; i < ConnectionDB.ListItemShopPhucLoi.Length; i++)
			{
				string[] Item = ConnectionDB.ListItemShopPhucLoi[i].Split(",");
				k.WriteShort(i);
				short IdItem = short.Parse(Item[0]);
				k.WriteUTF(Item[2]);
				short Type = -1;
				if (i < 8)
				{
					Type = 0;
				}
				else if (i >= 8 && i < 15)
				{
					Type = 1;
				}
				else if (i >= 15 && i < 23)
				{
					Type = 2;
				}
				else if (i >= 23 && i < 30)
				{
					Type = 7;
				}
				else if (i >= 30 && i <= 36)
				{
					Type = 8;
				}
				else if (i > 36 && i < 43)
				{
					Type = 5;
				}
				else if (i >= 43 && i <= 51)
				{
					Type = 6;
				}
				else if (i > 51 && i <= 58)
				{
					Type = 19;
				}
				else if (i > 58 && i <= 67)
				{
					Type = 21;
				}
				else if (i > 67 && i <= 72)
				{
					Type = 22;
				}
				else if (i > 72 && i <= 82)
				{
					Type = 3;
				}
				else if (i >= 83 && i <= 88)
				{
					Type = 20;
				}
				else if (i > 88 && i <= 95)
				{
					Type = 4;
				}
				else if (i > 95 && i <= 107)
				{
					Type = 15;
				}
				else if (i > 107 && i <= 119)
				{
					Type = 16;
				}
				else if (i > 119 && i <= 123)
				{
					Type = 17;
				}
				else if (i > 123 && i <= 127)
				{
					Type = 18;
				}
				else
				{
					Console.WriteLine(" IIII " + i);
					Type = 17;
				}
				k.WriteShort(Type);
				Item it2 = new Item(IdItem, IsLock: true);
				it2.Quantity = int.Parse(Item[1]);
				it2.IdClass = -1;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("");
				if (Item.Length > 3)
				{
					string[] Option = Item[3].Split("@");
					for (int j = 0; j < Option.Length; j++)
					{
						if (j == 2 || j == 4 || j == 6 || j == 8 || j == 10 || j == 12 || j == 14)
						{
							stringBuilder.Append(";");
						}
						if (j == 1 || j == 3 || j == 5 || j == 7 || j == 9 || j == 11 || j == 13)
						{
							stringBuilder.Append(",");
						}
						stringBuilder.Append(short.Parse(Option[j]));
					}
				}
				it2.Options = stringBuilder.ToString();
				ItemHander.WriteItem(k, it2);
				it2 = null;
				if (i < 8)
				{
					if (i == 0 && TimeOnl >= 10 && !QuaOnline[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 1 && TimeOnl >= 20 && !QuaOnline[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 2 && TimeOnl >= 30 && !QuaOnline[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 3 && TimeOnl >= 60 && !QuaOnline[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 4 && TimeOnl >= 90 && !QuaOnline[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 5 && TimeOnl >= 120 && !QuaOnline[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 6 && TimeOnl >= 180 && !QuaOnline[6])
					{
						k.WriteBool(x: true);
					}
					else if (i == 7 && TimeOnl >= 240 && !QuaOnline[7])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i >= 8 && i < 15)
				{
					if (i == 8 && Day >= 1 && !Qua7Day[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 9 && Day >= 2 && !Qua7Day[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 10 && Day >= 3 && !Qua7Day[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 11 && Day >= 4 && !Qua7Day[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 12 && Day >= 5 && !Qua7Day[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 13 && Day >= 6 && !Qua7Day[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 14 && Day >= 7 && !Qua7Day[6])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i >= 15 && i < 23)
				{
					if (i == 15 && Level >= 10 && !QuaThangCap[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 16 && Level >= 20 && !QuaThangCap[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 17 && Level >= 30 && !QuaThangCap[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 18 && Level >= 35 && !QuaThangCap[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 19 && Level >= 40 && !QuaThangCap[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 20 && Level >= 45 && !QuaThangCap[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 21 && Level >= 50 && !QuaThangCap[6])
					{
						k.WriteBool(x: true);
					}
					else if (i == 22 && Level >= 55 && !QuaThangCap[7])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i >= 23 && i < 30)
				{
					if (i == 23 && SoVangTieuNgay >= 20 && !QuaTieuNgay[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 24 && SoVangTieuNgay >= 50 && !QuaTieuNgay[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 25 && SoVangTieuNgay >= 100 && !QuaTieuNgay[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 26 && SoVangTieuNgay >= 250 && !QuaTieuNgay[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 27 && SoVangTieuNgay >= 500 && !QuaTieuNgay[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 28 && SoVangTieuNgay >= 1000 && !QuaTieuNgay[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 29 && SoVangTieuNgay >= 1500 && !QuaTieuNgay[6])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i >= 30 && i <= 36)
				{
					if (i == 30 && SoVangTieuTrongTuan >= 80 && !QuaTieuTuan[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 31 && SoVangTieuTrongTuan >= 160 && !QuaTieuTuan[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 32 && SoVangTieuTrongTuan >= 220 && !QuaTieuTuan[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 33 && SoVangTieuTrongTuan >= 500 && !QuaTieuTuan[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 34 && SoVangTieuTrongTuan >= 1000 && !QuaTieuTuan[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 35 && SoVangTieuTrongTuan >= 1700 && !QuaTieuTuan[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 36 && SoVangTieuTrongTuan >= 2300 && !QuaTieuTuan[6])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 36 && i < 43)
				{
					if (i == 37 && SoVangNapTrongNgay >= 25 && !QuaNapNgay[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 38 && SoVangNapTrongNgay >= 75 && !QuaNapNgay[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 39 && SoVangNapTrongNgay >= 145 && !QuaNapNgay[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 40 && SoVangNapTrongNgay >= 250 && !QuaNapNgay[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 41 && SoVangNapTrongNgay >= 390 && !QuaNapNgay[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 42 && SoVangNapTrongNgay >= 500 && !QuaNapNgay[5])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i >= 43 && i <= 51)
				{
					if (i == 43 && SoVangNapTrongTuan >= 100 && !QuaNapTuan[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 44 && SoVangNapTrongTuan >= 400 && !QuaNapTuan[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 45 && SoVangNapTrongTuan >= 1000 && !QuaNapTuan[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 46 && SoVangNapTrongTuan >= 3000 && !QuaNapTuan[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 47 && SoVangNapTrongTuan >= 5000 && !QuaNapTuan[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 48 && SoVangNapTrongTuan >= 10000 && !QuaNapTuan[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 49 && SoVangNapTrongTuan >= 15000 && !QuaNapTuan[6])
					{
						k.WriteBool(x: true);
					}
					else if (i == 50 && SoVangNapTrongTuan >= 20000 && !QuaNapTuan[7])
					{
						k.WriteBool(x: true);
					}
					else if (i == 51 && SoVangNapTrongTuan >= 25000 && !QuaNapNgay[8])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 51 && i <= 58)
				{
					if (i == 52 && SoNgayNapLienTuc >= 1 && !QuaNapLienTuc[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 53 && SoNgayNapLienTuc >= 2 && !QuaNapLienTuc[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 54 && SoNgayNapLienTuc >= 3 && !QuaNapLienTuc[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 55 && SoNgayNapLienTuc >= 4 && !QuaNapLienTuc[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 56 && SoNgayNapLienTuc >= 5 && !QuaNapLienTuc[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 57 && SoNgayNapLienTuc >= 6 && !QuaNapLienTuc[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 58 && SoNgayNapLienTuc >= 7 && !QuaNapLienTuc[6])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 58 && i <= 67)
				{
					if (i > 58 && i <= 61 && SoVangNap3Moc >= 100 && !QuaNap3Moc[0])
					{
						k.WriteBool(x: true);
					}
					else if (i > 62 && i <= 64 && SoVangNap3Moc >= 500 && !QuaNap3Moc[1])
					{
						k.WriteBool(x: true);
					}
					else if (i > 64 && i <= 67 && SoVangNap3Moc >= 1000 && !QuaNap3Moc[2])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 67 && i <= 72)
				{
					if (i == 68 && SoVangNapDon >= 50 && !QuaNapDon[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 69 && SoVangNapDon >= 200 && !QuaNapDon[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 70 && SoVangNapDon >= 600 && !QuaNapDon[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 71 && SoVangNapDon >= 1300 && !QuaNapDon[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 72 && SoVangNapDon >= 2000 && !QuaNapDon[4])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 72 && i <= 82)
				{
					if (i == 73 && TongSoVangDaNap >= 10 && !QuaRank[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 74 && TongSoVangDaNap >= 50 && !QuaRank[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 75 && TongSoVangDaNap >= 100 && !QuaRank[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 76 && TongSoVangDaNap >= 500 && !QuaRank[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 77 && TongSoVangDaNap >= 1000 && !QuaRank[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 78 && TongSoVangDaNap >= 5000 && !QuaRank[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 79 && TongSoVangDaNap >= 20000 && !QuaRank[6])
					{
						k.WriteBool(x: true);
					}
					else if (i == 80 && TongSoVangDaNap >= 50000 && !QuaRank[7])
					{
						k.WriteBool(x: true);
					}
					else if (i == 81 && TongSoVangDaNap >= 120000 && !QuaRank[8])
					{
						k.WriteBool(x: true);
					}
					else if (i == 82 && TongSoVangDaNap >= 500000 && !QuaRank[9])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 82 && i <= 88)
				{
					if (i == 83 && RankCaoNhatServer >= 5 && !QuaRankChung[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 84 && RankCaoNhatServer >= 6 && !QuaRankChung[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 85 && RankCaoNhatServer >= 7 && !QuaRankChung[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 86 && RankCaoNhatServer >= 8 && !QuaRankChung[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 87 && RankCaoNhatServer >= 9 && !QuaRankChung[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 88 && RankCaoNhatServer >= 10 && !QuaRankChung[5])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 88 && i <= 95)
				{
					if (i == 89 && TongSoRankServer >= 50 && !QuaRankTatCa[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 90 && TongSoRankServer >= 100 && !QuaRankTatCa[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 91 && TongSoRankServer >= 200 && !QuaRankTatCa[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 92 && TongSoRankServer >= 300 && !QuaRankTatCa[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 93 && TongSoRankServer >= 500 && !QuaRankTatCa[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 94 && TongSoRankServer >= 1000 && !QuaRankTatCa[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 95 && TongSoRankServer >= 2000 && !QuaRankTatCa[6])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 95 && i <= 107)
				{
					if (i == 96 && Level >= 10 && IsBuyGoiHaoHoa && !GoiHaoHoa[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 97 && Level >= 15 && IsBuyGoiHaoHoa && !GoiHaoHoa[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 98 && Level >= 20 && IsBuyGoiHaoHoa && !GoiHaoHoa[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 99 && Level >= 25 && IsBuyGoiHaoHoa && !GoiHaoHoa[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 100 && Level >= 30 && IsBuyGoiHaoHoa && !GoiHaoHoa[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 101 && Level >= 35 && IsBuyGoiHaoHoa && !GoiHaoHoa[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 102 && Level >= 40 && IsBuyGoiHaoHoa && !GoiHaoHoa[6])
					{
						k.WriteBool(x: true);
					}
					else if (i == 103 && Level >= 45 && IsBuyGoiHaoHoa && !GoiHaoHoa[7])
					{
						k.WriteBool(x: true);
					}
					else if (i == 104 && Level >= 50 && IsBuyGoiHaoHoa && !GoiHaoHoa[8])
					{
						k.WriteBool(x: true);
					}
					else if (i == 105 && Level >= 55 && IsBuyGoiHaoHoa && !GoiHaoHoa[9])
					{
						k.WriteBool(x: true);
					}
					else if (i == 106 && Level >= 60 && IsBuyGoiHaoHoa && !GoiHaoHoa[10])
					{
						k.WriteBool(x: true);
					}
					else if (i == 107 && Level >= 65 && IsBuyGoiHaoHoa && !GoiHaoHoa[11])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 107 && i <= 119)
				{
					if (i == 108 && Level >= 10 && IsBuyGoiChiTon && !GoiChiTon[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 109 && Level >= 15 && IsBuyGoiChiTon && !GoiChiTon[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 110 && Level >= 20 && IsBuyGoiChiTon && !GoiChiTon[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 111 && Level >= 25 && IsBuyGoiChiTon && !GoiChiTon[3])
					{
						k.WriteBool(x: true);
					}
					else if (i == 112 && Level >= 30 && IsBuyGoiChiTon && !GoiChiTon[4])
					{
						k.WriteBool(x: true);
					}
					else if (i == 113 && Level >= 35 && IsBuyGoiChiTon && !GoiChiTon[5])
					{
						k.WriteBool(x: true);
					}
					else if (i == 114 && Level >= 40 && IsBuyGoiChiTon && !GoiChiTon[6])
					{
						k.WriteBool(x: true);
					}
					else if (i == 115 && Level >= 45 && IsBuyGoiChiTon && !GoiChiTon[7])
					{
						k.WriteBool(x: true);
					}
					else if (i == 116 && Level >= 50 && IsBuyGoiChiTon && !GoiChiTon[8])
					{
						k.WriteBool(x: true);
					}
					else if (i == 117 && Level >= 55 && IsBuyGoiChiTon && !GoiChiTon[9])
					{
						k.WriteBool(x: true);
					}
					else if (i == 118 && Level >= 60 && IsBuyGoiChiTon && !GoiChiTon[10])
					{
						k.WriteBool(x: true);
					}
					else if (i == 119 && Level >= 65 && IsBuyGoiChiTon && !GoiChiTon[11])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 119 && i <= 123)
				{
					if (i == 120 && DauTuAllServer >= 10 && !DauTuTatCa[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 121 && DauTuAllServer >= 100 && !DauTuTatCa[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 122 && DauTuAllServer >= 200 && !DauTuTatCa[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 123 && DauTuAllServer >= 500 && !DauTuTatCa[3])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else if (i > 123 && i <= 127)
				{
					if (i == 124 && TheThangAllServer >= 10 && !TheThangTatCa[0])
					{
						k.WriteBool(x: true);
					}
					else if (i == 125 && TheThangAllServer >= 100 && !TheThangTatCa[1])
					{
						k.WriteBool(x: true);
					}
					else if (i == 126 && TheThangAllServer >= 200 && !TheThangTatCa[2])
					{
						k.WriteBool(x: true);
					}
					else if (i == 127 && TheThangAllServer >= 500 && !TheThangTatCa[3])
					{
						k.WriteBool(x: true);
					}
					else
					{
						k.WriteBool(x: false);
					}
				}
				else
				{
					k.WriteBool(x: true);
				}
			}
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteLong(-1L);
			k.WriteInt(0);
			k.WriteLong(-1L);
			k.WriteLong(0L);
			k.WriteLong(0L);
			k.WriteLong(0L);
			k.WriteLong(-1L);
			_myChar.SendMessage(k);
		}
	}
}
