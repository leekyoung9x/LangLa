using System.Collections.Generic;
using System.Linq;
using System.Text;
using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.Model;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SqlConnection;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.Hander
{
	public static class ShopHander
	{
		private static readonly short[] ValueOnline = new short[8] { 10, 20, 30, 60, 90, 120, 180, 240 };

		private static readonly sbyte[] ValueOnline7Day = new sbyte[7] { 1, 2, 3, 4, 5, 6, 7 };

		private static readonly sbyte[] ValueLevelThangCap = new sbyte[7] { 10, 20, 30, 35, 40, 45, 50 };

		private static readonly short[] ValueTieuNgay = new short[7] { 20, 50, 100, 250, 500, 1000, 1500 };

		private static readonly short[] ValueTieuTuan = new short[7] { 80, 150, 220, 500, 1000, 1700, 2300 };

		private static readonly short[] ValueNapNgay = new short[6] { 25, 75, 145, 250, 390, 500 };

		private static readonly short[] ValueNapTuan = new short[9] { 100, 400, 1000, 3000, 5000, 10000, 15000, 20000, 25000 };

		private static readonly short[] ValueDayNapLienTuc = new short[7] { 1, 2, 3, 4, 5, 6, 7 };

		private static readonly short[] ValueNap3Moc = new short[3] { 100, 500, 1000 };

		private static readonly short[] ValueNapDon = new short[5] { 50, 200, 600, 1300, 2000 };

		private static readonly int[] ValueQuaRank = new int[10] { 10, 50, 100, 500, 1000, 5000, 20000, 50000, 120000, 500000 };

		private static readonly sbyte[] ValueRankChung = new sbyte[6] { 5, 6, 7, 8, 9, 10 };

		private static readonly short[] ValueRankTatCa = new short[7] { 50, 100, 200, 300, 500, 1000, 2000 };

		private static readonly short[] ValueGoiHaoHoaAndChiTon = new short[11]
		{
			10, 15, 20, 25, 30, 35, 40, 45, 50, 55,
			60
		};

		private static readonly short[] ValueDauTuallAndTheThangAllServer = new short[4] { 10, 100, 200, 500 };

		public static readonly string[] Hokage = new string[10] { "Đai trán", "Áo", "Bao tay", "Quần", "Giày", "Vũ khí", "Dây thừng", "Móc sắt", "Ống tiêu", "Túi nhẫn giả" };

		public static void OpenShopNpc4(Character _myChar, short IdNpc, sbyte Type)
		{
			Message j = new Message(122);
			j.WriteByte(Type);
			j.WriteShort((short)(DataShop.ITEM_SHOP0.Length / 2));
			int i = 0;
			for (i = ((Type != 4) ? (DataShop.ITEM_SHOP0.Length / 2) : 0); i < DataShop.ITEM_SHOP0.Length; i++)
			{
				j.WriteShort((short)i);
				j.WriteShort(DataShop.ITEM_SHOP0[i]);
				j.WriteBool(x: true);
				j.WriteLong(-1L);
				j.WriteUTF("");
				j.WriteInt(0);
				j.WriteInt(0);
				j.WriteInt(0);
				j.WriteInt((Type != 4) ? DataShop.MoneyShop0[i] : 0);
				j.WriteInt((Type == 4) ? DataShop.MoneyShop0[i] : 0);
			}
			_myChar.InfoGame.ID_Click_SHOP = 4;
			_myChar.SendMessage(j);
		}

		public static void OpenShopHokage(Character _myChar, sbyte Type, sbyte IdClass = -1)
		{
			ItemShop[] itemShops2 = null;
			sbyte GioiTinh = (sbyte)((Type > 25) ? 2 : _myChar.Info.GioiTinh);
			sbyte Down = 1;
			if (Type == 20)
			{
				GioiTinh = 2;
			}
			if (Type == 25)
			{
				itemShops2 = DataShop.ShopTemplates.FirstOrDefault((ShopTemplate s) => s.TypeShop == Type).ItemShops.Where((ItemShop s) => s.IdClass == IdClass && DataServer.ArrItemTemplate[s.IdItem].type == 8 && s.GioiTinh == GioiTinh).ToArray();
				Down = 3;
			}
			else
			{
				itemShops2 = DataShop.ShopTemplates.FirstOrDefault((ShopTemplate s) => s.TypeShop == Type).ItemShops.Where((ItemShop s) => s.IdClass == IdClass && s.GioiTinh == GioiTinh).ToArray();
			}
			Message m2 = new Message(122);
			m2.WriteByte(Type);
			m2.WriteByte(IdClass);
			m2.WriteShort((short)(itemShops2.Length / Down));
			for (int i = 0; i < itemShops2.Length / Down; i++)
			{
				m2.WriteShort(itemShops2[i].IdBuy);
				m2.WriteShort(itemShops2[i].IdItem);
				m2.WriteLong(itemShops2[i].HSD);
				m2.WriteUTF(itemShops2[i].Options);
				m2.WriteInt(0);
				m2.WriteInt(0);
				m2.WriteInt(0);
				m2.WriteInt(0);
				m2.WriteInt(itemShops2[i].BacBuy);
				m2.WriteInt(itemShops2[i].MoneyHokage);
			}
			_myChar.SendMessage(m2);
			_myChar.InfoGame.ID_Click_SHOP = Type;
		}

		public static void OpennShopType0(Character _myChar, sbyte Type)
		{
			ItemShop[] itemShop = DataShop.ShopTemplates.FirstOrDefault((ShopTemplate s) => s.TypeShop == Type).ItemShops.ToArray();
			Message j = new Message(122);
			j.WriteByte(Type);
			j.WriteShort((short)itemShop.Length);
			ItemShop[] array = itemShop;
			foreach (ItemShop i in array)
			{
				j.WriteShort(i.IdBuy);
				j.WriteShort(i.IdItem);
				j.WriteBool(i.IsLock);
				j.WriteLong(-1L);
				j.WriteUTF("");
				j.WriteInt(0);
				j.WriteInt(0);
				j.WriteInt(0);
				j.WriteInt(i.BacKhoaBuy);
				j.WriteInt(i.BacBuy);
			}
			_myChar.SendMessage(j);
		}

		public static void BuyPhucLoi(Character _myChar, sbyte Type)
		{
			short VangBuy = -1;
			bool IsDaMua = false;
			switch (Type)
			{
			case -63:
				VangBuy = 200;
				if (_myChar.Info.IsBuyGoiHaoHoa)
				{
					IsDaMua = true;
				}
				break;
			case -62:
				VangBuy = 300;
				if (_myChar.Info.IsBuyGoiChiTon)
				{
					IsDaMua = true;
				}
				break;
			case -66:
				VangBuy = 100;
				break;
			case -65:
				VangBuy = 300;
				break;
			}
			if (VangBuy != -1 && !IsDaMua && InventoryHander.UpdateVang(_myChar, VangBuy))
			{
				switch (Type)
				{
				case -63:
					_myChar.Info.IsBuyGoiHaoHoa = true;
					break;
				case -62:
					_myChar.Info.IsBuyGoiChiTon = true;
					break;
				}
				LangLa.Server.Server.DauTuAllServer++;
				ClickEventHander.ShowTabPhucLoi(_myChar);
			}
		}

		public static void ClickButtonShopPhucLoi(Character _myChar, Message msg)
		{
			short Id = msg.ReadShort();
			bool IsOk = false;
			int j = 0;
			if (Id < 8)
			{
				int Minute = _myChar.TimeChar.getMinutes(IsTrue: true);
				bool[] QuanOnline = _myChar.TimeChar.IsCanNhanQuaOnlie;
				short[] valueOnline = ValueOnline;
				foreach (short c in valueOnline)
				{
					if (Minute >= c && !QuanOnline[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id >= 8 && Id < 15)
			{
				short Day = _myChar.TimeChar.getDay(IsTrue: true);
				bool[] Qua7Day = _myChar.TimeChar.IsCanNhanQuaOnline7Day;
				sbyte[] valueOnline7Day = ValueOnline7Day;
				foreach (sbyte c2 in valueOnline7Day)
				{
					if (Day >= c2 && !Qua7Day[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id >= 15 && Id < 23)
			{
				sbyte Level = _myChar.Info.Level;
				bool[] QuaThangCap = _myChar.TimeChar.IsCanNhanQuaThangCap;
				sbyte[] valueLevelThangCap = ValueLevelThangCap;
				foreach (sbyte c3 in valueLevelThangCap)
				{
					if (Level >= c3 && !QuaThangCap[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id >= 23 && Id < 30)
			{
				int SoVangTieuTrongNgay = _myChar.TimeChar.SoVangTieuTrongNgay;
				bool[] QuaTieuNgay = _myChar.TimeChar.IsCanNhanQuaTieuNgay;
				short[] valueTieuNgay = ValueTieuNgay;
				foreach (short c4 in valueTieuNgay)
				{
					if (SoVangTieuTrongNgay >= c4 && !QuaTieuNgay[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id >= 30 && Id <= 36)
			{
				int SoVangTieuTrongTuan = _myChar.TimeChar.SoVangTieuTrongTuan;
				bool[] QuaTieuTuan = _myChar.TimeChar.IsCanNhanQuaTieuTuan;
				short[] valueTieuTuan = ValueTieuTuan;
				foreach (short c5 in valueTieuTuan)
				{
					if (SoVangTieuTrongTuan >= c5 && !QuaTieuTuan[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 36 && Id < 43)
			{
				int SoVangNapTrongNgay = _myChar.TimeChar.SoVangNapTrongNgay;
				bool[] QuaNapNgay = _myChar.TimeChar.IsCanNhanQuaNapNgay;
				short[] valueNapNgay = ValueNapNgay;
				foreach (short c7 in valueNapNgay)
				{
					if (SoVangNapTrongNgay >= c7 && !QuaNapNgay[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id >= 43 && Id <= 51)
			{
				int SoVangNapTrongTuan = _myChar.TimeChar.SoVangNapTrongTuan;
				bool[] QuaNapTuan = _myChar.TimeChar.IsCanNhanQuaNapTuan;
				short[] valueNapTuan = ValueNapTuan;
				foreach (short c8 in valueNapTuan)
				{
					if (SoVangNapTrongTuan >= c8 && !QuaNapTuan[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 51 && Id <= 58)
			{
				int SoNgayNapLienTuc = _myChar.TimeChar.SoNgayNapLienTuc;
				bool[] QuaNapLienTuc = _myChar.TimeChar.IsCanNhanQuaNapLienTuc;
				short[] valueDayNapLienTuc = ValueDayNapLienTuc;
				foreach (short c9 in valueDayNapLienTuc)
				{
					if (SoNgayNapLienTuc >= c9 && !QuaNapLienTuc[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 58 && Id <= 67)
			{
				int SoVangNap3Moc = _myChar.TimeChar.SoVangNap3Moc;
				bool[] QuaNap3Moc = _myChar.TimeChar.IsCanNhanQuaNap3Moc;
				short[] valueNap3Moc = ValueNap3Moc;
				foreach (short c10 in valueNap3Moc)
				{
					if (SoVangNap3Moc >= c10 && !QuaNap3Moc[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 67 && Id <= 72)
			{
				int SoVangNapDon = _myChar.TimeChar.SoVangNapDon;
				bool[] QuaNapDoen = _myChar.TimeChar.IsCanNhanQuaNapDon;
				short[] valueNapDon = ValueNapDon;
				foreach (short c11 in valueNapDon)
				{
					if (SoVangNapDon >= c11 && !QuaNapDoen[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 72 && Id <= 82)
			{
				int TongSoVangDaNap = _myChar.TimeChar.TongSoVangDaNap;
				bool[] QuaRank = _myChar.TimeChar.IsCanNhanQuaRank;
				int[] valueQuaRank = ValueQuaRank;
				foreach (int c13 in valueQuaRank)
				{
					if (TongSoVangDaNap >= c13 && !QuaRank[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 82 && Id <= 88)
			{
				sbyte RankCaoNhatServer = LangLa.Server.Server.RankCaoNhatServer;
				bool[] QuaRankAllServer = _myChar.TimeChar.IsCanNhanQuaRankChung;
				sbyte[] valueRankChung = ValueRankChung;
				foreach (sbyte c14 in valueRankChung)
				{
					if (RankCaoNhatServer >= c14 && !QuaRankAllServer[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 88 && Id <= 95)
			{
				int TongSoRankServer = LangLa.Server.Server.RankAllServer;
				bool[] QuaRankAllServer2 = _myChar.TimeChar.IsCanNhanQuaRankTatCa;
				short[] valueRankTatCa = ValueRankTatCa;
				foreach (short c15 in valueRankTatCa)
				{
					if (TongSoRankServer >= c15 && !QuaRankAllServer2[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 95 && Id <= 107)
			{
				if (!_myChar.Info.IsBuyGoiHaoHoa)
				{
					return;
				}
				sbyte Level3 = _myChar.Info.Level;
				bool[] QuaGoiHaoHoa = _myChar.TimeChar.IsCanNhanQuaGoiHaoHoa;
				short[] valueGoiHaoHoaAndChiTon = ValueGoiHaoHoaAndChiTon;
				foreach (short c16 in valueGoiHaoHoaAndChiTon)
				{
					if (Level3 >= c16 && !QuaGoiHaoHoa[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 107 && Id <= 119)
			{
				if (!_myChar.Info.IsBuyGoiChiTon)
				{
					return;
				}
				sbyte Level2 = _myChar.Info.Level;
				bool[] QuaGoiChiTon = _myChar.TimeChar.IsCanNhanQuaGoiChiTon;
				short[] valueGoiHaoHoaAndChiTon2 = ValueGoiHaoHoaAndChiTon;
				foreach (short c17 in valueGoiHaoHoaAndChiTon2)
				{
					if (Level2 >= c17 && !QuaGoiChiTon[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 119 && Id <= 123)
			{
				int DauTuAllServer = LangLa.Server.Server.DauTuAllServer;
				bool[] QuaDauTuAllServer = _myChar.TimeChar.IsCanNhanQuaDauTuTatCa;
				short[] valueDauTuallAndTheThangAllServer = ValueDauTuallAndTheThangAllServer;
				foreach (short c12 in valueDauTuallAndTheThangAllServer)
				{
					if (DauTuAllServer >= c12 && !QuaDauTuAllServer[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			else if (Id > 123 && Id <= 127)
			{
				int TheThangAllServer = LangLa.Server.Server.TheThangAllServer;
				bool[] QuaTheThangAllServer = _myChar.TimeChar.IsCanNhanQuaTheThangTatCa;
				short[] valueDauTuallAndTheThangAllServer2 = ValueDauTuallAndTheThangAllServer;
				foreach (short c6 in valueDauTuallAndTheThangAllServer2)
				{
					if (TheThangAllServer >= c6 && !QuaTheThangAllServer[j])
					{
						IsOk = true;
						break;
					}
					j++;
				}
			}
			if (!IsOk)
			{
				return;
			}
			string[] Info = ConnectionDB.ListItemShopPhucLoi[Id].Split(",");
			int Quantity = int.Parse(Info[1]);
			short IdItem = short.Parse(Info[0]);
			Item it = new Item(IdItem, IsLock: true);
			if (Info.Length > 2)
			{
				it.Quantity = Quantity;
			}
			if (Info.Length > 3)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string[] Op = Info[3].Split("@");
				for (int i = 0; i < Op.Length; i++)
				{
					if (i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12 || i == 14)
					{
						stringBuilder.Append(";");
					}
					if (i == 1 || i == 3 || i == 5 || i == 7 || i == 9 || i == 11 || i == 13)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(Op[i]);
				}
				it.Options = stringBuilder.ToString();
			}
			if (InventoryHander.GetCountNotNullBag(_myChar) > 0)
			{
				InventoryHander.AddItemBag(_myChar, it);
				NhanQuaPhucLoi(_myChar, Id);
				return;
			}
			if (_myChar.Thus.Count >= 120)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Số lượng thư đã đạt giới hạn ", Util.WHITE));
				return;
			}
			InfoThu infoThu = new InfoThu(it);
			infoThu.Title = "Quà phúc lợi";
			infoThu.Id = (short)_myChar.Thus.Count;
			infoThu.TimeEnd = 99999L;
			infoThu.Item = it;
			_myChar.Thus.Add(infoThu);
			ThuHander.ReloadThu(_myChar);
			_myChar.SendMessage(UtilMessage.SendThongBao("Hành trang không đủ ô trống vật phẩm đã được gửi vào thư", Util.WHITE));
			NhanQuaPhucLoi(_myChar, Id);
		}

		private static void NhanQuaPhucLoi(Character _myChar, short i)
		{
			if (i < 8)
			{
				_myChar.TimeChar.IsCanNhanQuaOnlie[i] = true;
			}
			else if (i >= 8 && i < 15)
			{
				_myChar.TimeChar.IsCanNhanQuaOnline7Day[i - 8] = true;
			}
			else if (i >= 15 && i < 23)
			{
				_myChar.TimeChar.IsCanNhanQuaThangCap[i - 15] = true;
			}
			else if (i >= 23 && i < 30)
			{
				_myChar.TimeChar.IsCanNhanQuaTieuNgay[i - 23] = true;
			}
			else if (i >= 30 && i <= 36)
			{
				_myChar.TimeChar.IsCanNhanQuaTieuTuan[i - 30] = true;
			}
			else if (i > 36 && i < 43)
			{
				_myChar.TimeChar.IsCanNhanQuaNapNgay[i - 37] = true;
			}
			else if (i >= 43 && i <= 51)
			{
				_myChar.TimeChar.IsCanNhanQuaNapTuan[i - 43] = true;
			}
			else if (i > 51 && i <= 58)
			{
				_myChar.TimeChar.IsCanNhanQuaNapLienTuc[i - 52] = true;
			}
			else if (i > 58 && i <= 67)
			{
				_myChar.TimeChar.IsCanNhanQuaNap3Moc[i - 59] = true;
			}
			else if (i > 67 && i <= 72)
			{
				_myChar.TimeChar.IsCanNhanQuaNapDon[i - 68] = true;
			}
			else if (i > 72 && i <= 82)
			{
				_myChar.TimeChar.IsCanNhanQuaRank[i - 73] = true;
			}
			else if (i >= 83 && i <= 88)
			{
				_myChar.TimeChar.IsCanNhanQuaRankChung[i - 83] = true;
			}
			else if (i > 88 && i <= 95)
			{
				_myChar.TimeChar.IsCanNhanQuaRankTatCa[i - 89] = true;
			}
			else if (i > 95 && i <= 107)
			{
				_myChar.TimeChar.IsCanNhanQuaGoiHaoHoa[i - 96] = true;
			}
			else if (i > 107 && i <= 119)
			{
				_myChar.TimeChar.IsCanNhanQuaGoiChiTon[i - 108] = true;
			}
			else if (i > 119 && i <= 123)
			{
				_myChar.TimeChar.IsCanNhanQuaDauTuTatCa[i - 120] = true;
			}
			else if (i > 123 && i <= 127)
			{
				_myChar.TimeChar.IsCanNhanQuaTheThangTatCa[i - 124] = true;
			}
			Message j = new Message(-123);
			j.WriteByte(-70);
			j.WriteShort(i);
			_myChar.SendMessage(j);
		}

		public static void OpenShopCuaHang(Character _myChar, sbyte Type)
		{
			Message m = new Message(122);
			m.WriteByte(Type);
			if (Type == 40)
			{
				m.WriteShort(0);
				m.WriteLong(-1L);
				m.WriteLong(-1L);
				_myChar.SendMessage(m);
				return;
			}
			List<ItemShop> shopTemplates = DataShop.ShopTemplates.FirstOrDefault((ShopTemplate s) => s.TypeShop == Type).ItemShops;
			if (shopTemplates == null)
			{
				return;
			}
			if (Type == 39)
			{
				m.WriteShort(10);
				int l = 0;
				bool[] CanBuy = _myChar.TimeChar.IsCanBuyShopKhuRank;
				for (short i = 0; i < 10; i++)
				{
					m.WriteShort(shopTemplates[l].IdBuy);
					m.WriteShort(-1);
					m.WriteBool(x: true);
					m.WriteLong(-1L);
					m.WriteUTF("");
					m.WriteInt(0);
					m.WriteInt(shopTemplates[l].VangBuy);
					m.WriteInt(0);
					m.WriteInt(0);
					m.WriteInt(0);
					m.WriteBool(CanBuy[i]);
					for (int k = 0; k < 3; k++)
					{
						m.WriteShort(shopTemplates[l].IdItem);
						m.WriteBool(x: true);
						m.WriteLong(-1L);
						if (DataServer.ArrItemTemplate[shopTemplates[l].IdItem].type <= 16)
						{
							m.WriteByte(-1);
							m.WriteByte(0);
							m.WriteUTF(shopTemplates[l].Options);
						}
						else
						{
							m.WriteInt(shopTemplates[l].Quantity);
						}
						m.WriteShort(-1);
						l++;
					}
				}
				_myChar.SendMessage(m);
				return;
			}
			m.WriteShort((short)shopTemplates.Count);
			for (short j = 0; j < shopTemplates.Count; j++)
			{
				m.WriteShort(shopTemplates[j].IdBuy);
				m.WriteShort(shopTemplates[j].IdItem);
				m.WriteBool(x: true);
				m.WriteLong(-1L);
				m.WriteUTF(shopTemplates[j].Options);
				switch (Type)
				{
				case 6:
					m.WriteInt(0);
					m.WriteInt(shopTemplates[j].VangBuy);
					m.WriteInt(0);
					m.WriteInt(0);
					m.WriteInt(0);
					break;
				case 7:
					m.WriteInt(0);
					m.WriteInt(0);
					m.WriteInt(shopTemplates[j].VangKhoaBuy);
					m.WriteInt(0);
					m.WriteInt(0);
					break;
				}
			}
			_myChar.SendMessage(m);
		}

		public static void OpenShopBodyChar(Character _myChar, sbyte Type, sbyte IdClass = -1)
		{
			Character _myChar2 = _myChar;
			List<ItemShop> shopTemplate = null;
			Message k = new Message(122);
			k.WriteByte(Type);
			sbyte TypeShow = 0;
			sbyte IdClassShow = ((IdClass != -1) ? IdClass : _myChar2.Info.IdClass);
			sbyte IdClassFind = ((IdClass != -1) ? IdClass : _myChar2.Info.IdClass);
			sbyte GioiTinh = _myChar2.Info.GioiTinh;
			if (Type != 8)
			{
				IdClassFind = 0;
			}
			else
			{
				GioiTinh = 2;
			}
			switch (Type)
			{
			case 8:
				TypeShow = 1;
				break;
			case 9:
				TypeShow = 0;
				break;
			case 10:
				TypeShow = 2;
				break;
			case 11:
				TypeShow = 4;
				break;
			case 12:
				TypeShow = 6;
				break;
			case 13:
				TypeShow = 8;
				break;
			case 14:
				TypeShow = 3;
				GioiTinh = 2;
				IdClassFind = 0;
				break;
			case 15:
				TypeShow = 5;
				GioiTinh = 2;
				IdClassFind = 0;
				break;
			case 16:
				TypeShow = 7;
				GioiTinh = 2;
				IdClassFind = 0;
				break;
			case 17:
				TypeShow = 9;
				GioiTinh = 2;
				IdClassFind = 0;
				break;
			}
			k.WriteByte(IdClassShow);
			sbyte MaxSize = 5;
			if (Type == 8 || Type == 11 || Type == 12 || Type == 13 || Type == 15 || Type == 16 || Type == 17)
			{
				MaxSize = 6;
			}
			int j = _myChar2.Info.IdClass - 1;
			if (IdClass != -1)
			{
				j = IdClass - 1;
			}
			if (j != 0)
			{
				j *= MaxSize;
			}
			shopTemplate = ((Type < 9 || Type > 13) ? DataShop.ShopTemplates.FirstOrDefault((ShopTemplate s) => s.TypeShop == Type).ItemShops.Where((ItemShop s) => s.IdClass == IdClassShow).ToList() : DataShop.ShopTemplates.FirstOrDefault((ShopTemplate s) => s.TypeShop == Type).ItemShops.Where((ItemShop s) => s.IdClass == IdClassShow && s.GioiTinh == _myChar2.Info.GioiTinh).ToList());
			k.WriteShort((short)shopTemplate.Count);
			for (int i = 0; i < shopTemplate.Count; i++)
			{
				k.WriteShort(shopTemplate[i].IdBuy);
				k.WriteShort(shopTemplate[i].IdItem);
				k.WriteLong(-1L);
				k.WriteUTF(shopTemplate[i].Options);
				k.WriteInt(0);
				k.WriteInt(0);
				k.WriteInt(0);
				k.WriteInt(0);
				k.WriteInt(shopTemplate[i].BacBuy);
				k.WriteInt(0);
				j++;
			}
			_myChar2.SendMessage(k);
			_myChar2.InfoGame.ID_Click_SHOP = Type;
		}

		public static void OpenShopNgoaiTrang(Character _myChar, sbyte Type, sbyte IdClass = -1)
		{
			if (Type == 36)
			{
				ItemShop[] itemShops2 = DataShop.ShopTemplates.FirstOrDefault((ShopTemplate s) => s.TypeShop == Type).ItemShops.Where((ItemShop s) => s.IdClass == IdClass).ToArray();
				Message m2 = new Message(122);
				m2.WriteByte(Type);
				m2.WriteByte(IdClass);
				m2.WriteShort((short)itemShops2.Length);
				ItemShop[] array = itemShops2;
				foreach (ItemShop i in array)
				{
					m2.WriteShort(i.IdBuy);
					m2.WriteShort(i.IdItem);
					m2.WriteLong(i.HSD);
					m2.WriteUTF(i.Options);
					m2.WriteInt(i.TinhThachBuy);
					m2.WriteInt(0);
					m2.WriteInt(0);
					m2.WriteInt(0);
					m2.WriteInt(0);
					m2.WriteInt(0);
				}
				_myChar.SendMessage(m2);
			}
			else
			{
				ItemShop[] itemShops = DataShop.ShopTemplates.FirstOrDefault((ShopTemplate s) => s.TypeShop == Type).ItemShops.ToArray();
				sbyte Down = 1;
				if (Type == 19)
				{
					Down = 2;
				}
				Message k = new Message(122);
				k.WriteByte(Type);
				k.WriteShort((short)((short)itemShops.Length / Down));
				for (int j = 0; j < itemShops.Length / Down; j++)
				{
					k.WriteShort(itemShops[j].IdBuy);
					k.WriteShort(itemShops[j].IdItem);
					k.WriteBool(x: true);
					k.WriteLong(itemShops[j].HSD);
					k.WriteUTF(itemShops[j].Options);
					k.WriteInt(itemShops[j].TinhThachBuy);
					k.WriteInt(0);
					k.WriteInt(0);
					k.WriteInt(0);
					k.WriteInt(0);
				}
				_myChar.SendMessage(k);
				_myChar.InfoGame.ID_Click_SHOP = Type;
			}
		}

		public static void CharSale(Character _myChar, Message msg)
		{
			short Index = msg.ReadShort();
			bool var = msg.ReadBool();
			Item itemSale = _myChar.Inventory.ItemBag[Index];
			if (itemSale != null)
			{
				int BacKhoa = 0;
				string[] Options = itemSale.Options.Split(";");
				if (itemSale.Type <= 16 && Options.Length > 1)
				{
					BacKhoa = Options.Length;
				}
				if (itemSale.Type == 34)
				{
					BacKhoa = 50000;
				}
				InventoryHander.AddBacKhoa(_myChar, BacKhoa);
				_myChar.Inventory.ItemBag[Index] = null;
				_myChar.SendMessage(MsgCharSale(_myChar.Inventory.BacKhoa, _myChar.Inventory.Bac, Index));
			}
		}

		private static bool CheckPointHokage(Character _myChar, short Point, sbyte IdEvent)
		{
			sbyte Index = -1;
			switch (IdEvent)
			{
			case 20:
				Index = 5;
				break;
			case 21:
				Index = 0;
				break;
			case 22:
				Index = 1;
				break;
			case 23:
				Index = 2;
				break;
			case 24:
				Index = 3;
				break;
			case 25:
				Index = 4;
				break;
			case 26:
				Index = 6;
				break;
			case 27:
				Index = 7;
				break;
			case 28:
				Index = 8;
				break;
			case 29:
				Index = 9;
				break;
			}
			if (_myChar.Info.PointItemHokage[Index] < Point)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Còn thiếu " + (Point - _myChar.Info.PointItemHokage[Index]) + " điểm Hokage " + Hokage[Index], Util.YELLOW_MID));
				return false;
			}
			return true;
		}

		public static void BuyShop(Character _myChar, Message msg)
		{
			short ID_Buy = msg.ReadShort();
			short Quantity = msg.ReadShort();
			bool IsOk = false;
			int BuyVang = 0;
			int BuyBac = 0;
			int BuyBacKhoa = 0;
			int BuyVangKhoa = 0;
			int BuyTinhThach = 0;
			string Options = "";
			short IdItemCreate = -1;
			sbyte IdClass = -1;
			bool IsLock = false;
			long HSD = -1L;
			short PointHokage = 0;
			bool IsCanBuy = true;
			Item it = null;
			if (_myChar.InfoGame.ID_Click_SHOP != 5 && _myChar.InfoGame.ID_Click_SHOP != 4)
			{
				foreach (ShopTemplate s2 in DataShop.ShopTemplates)
				{
					if (IsOk)
					{
						break;
					}
					foreach (ItemShop j in s2.ItemShops)
					{
						if (j.IdBuy == ID_Buy)
						{
							BuyBac = j.BacBuy;
							BuyVang = j.VangBuy;
							BuyVangKhoa = j.VangKhoaBuy;
							BuyBacKhoa = j.BacKhoaBuy;
							BuyTinhThach = j.TinhThachBuy;
							Options = j.Options;
							IdClass = j.IdClass;
							HSD = j.HSD;
							PointHokage = (short)j.MoneyHokage;
							IdItemCreate = j.IdItem;
							IsLock = j.IsLock;
							IsOk = true;
							break;
						}
					}
				}
				if (IdItemCreate == -1)
				{
					return;
				}
			}
			else if (_myChar.InfoGame.ID_Click_SHOP == 4 || _myChar.InfoGame.ID_Click_SHOP == 5)
			{
				IdItemCreate = DataShop.ITEM_SHOP0[ID_Buy];
			}
			item_template itemTemplate = DataServer.ArrItemTemplate[IdItemCreate];
			if (InventoryHander.GetCountNotNullBag(_myChar) < Quantity && !itemTemplate.is_cong_don)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Hành trang không đủ chỗ trống", Util.WHITE));
				return;
			}
			sbyte iD_Click_SHOP = _myChar.InfoGame.ID_Click_SHOP;
			sbyte b = iD_Click_SHOP;
			if ((uint)(b - 4) <= 1u)
			{
				int Monney = DataShop.MoneyShop0[ID_Buy];
				if (_myChar.InfoGame.ID_Click_SHOP != 5 || _myChar.Inventory.BacKhoa >= Monney)
				{
					InventoryHander.UpdateBackhoa(_myChar, Monney);
					if (_myChar.InfoGame.ID_Click_SHOP != 4 || _myChar.Inventory.Bac >= Monney)
					{
						InventoryHander.UpdateBac(_myChar, Monney);
						it = new Item(IdItemCreate, IsLock: true);
						it.Quantity = Quantity;
						InventoryHander.AddItemBag(_myChar, it, SendThongBao: false);
						MsgBuyShop(_myChar, it);
						TaskHander.CheckBuyItem(_myChar, it);
					}
				}
			}
			if ((_myChar.InfoGame.ID_Click_SHOP >= 20 && _myChar.InfoGame.ID_Click_SHOP <= 29 && !CheckPointHokage(_myChar, PointHokage, _myChar.InfoGame.ID_Click_SHOP)) || !IsOk)
			{
				return;
			}
			short QuantityItemList = CheckItemListBuy(_myChar, ID_Buy);
			if (InventoryHander.GetCountNotNullBag(_myChar) < QuantityItemList)
			{
				return;
			}
			if (BuyBac > 0)
			{
				if (_myChar.Inventory.Bac < BuyBac * Quantity)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn không đủ Bạc để mua vật phẩm này", Util.WHITE));
					return;
				}
				InventoryHander.UpdateBac(_myChar, BuyBac * Quantity);
			}
			if (BuyBacKhoa > 0)
			{
				if (_myChar.Inventory.BacKhoa < BuyBacKhoa * Quantity)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn không đủ Bạc khóa để mua vật phẩm này", Util.WHITE));
					return;
				}
				InventoryHander.UpdateBackhoa(_myChar, BuyBacKhoa * Quantity);
			}
			if (BuyVang > 0)
			{
				if (_myChar.Inventory.Vang < BuyVang * Quantity)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn không đủ Vàng để mua vật phẩm này", Util.WHITE));
					return;
				}
				InventoryHander.UpdateVang(_myChar, BuyVang * Quantity);
			}
			if (BuyVangKhoa > 0)
			{
				if (_myChar.Inventory.VangKhoa < BuyVangKhoa * Quantity)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn không đủ Vàng khóa để mua vật phẩm này", Util.WHITE));
					return;
				}
				InventoryHander.UpdateVangKhoa(_myChar, BuyVangKhoa * Quantity);
			}
			if (BuyTinhThach > 0)
			{
				if (InventoryHander.FindQuantityAll(_myChar, 160) < BuyTinhThach)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn không đủ Tinh thạch để mua vật phẩm này", Util.WHITE));
					return;
				}
				InventoryHander.RemoveQuantityWhereId(_myChar, BuyTinhThach, 160);
			}
			if (QuantityItemList == 3)
			{
				bool[] CanBuy = _myChar.TimeChar.IsCanBuyShopKhuRank;
				sbyte IndexBool = (sbyte)((ID_Buy - 159) / 3);
				if (_myChar.TimeChar.IsCanBuyShopKhuRank[IndexBool])
				{
					return;
				}
				_myChar.TimeChar.IsCanBuyShopKhuRank[IndexBool] = true;
				Item[] it2 = new Item[3];
				bool IsBreak = false;
				int c = ID_Buy;
				for (int i = 0; i < 3; i++)
				{
					foreach (ShopTemplate s in DataShop.ShopTemplates)
					{
						foreach (ItemShop i2 in s.ItemShops)
						{
							if (i2.IdBuy == c)
							{
								Item it3 = new Item(i2.IdItem, i2.IsLock);
								it3.IdClass = i2.IdClass;
								it3.Options = i2.Options;
								it3.Quantity = i2.Quantity;
								RandomOption(it3, i2.Options);
								it2[i] = it3;
								IsBreak = true;
								c++;
								break;
							}
						}
					}
				}
				InventoryHander.AddMultiItembag(_myChar, it2);
			}
			else
			{
				it = new Item(IdItemCreate, IsLock);
				it.IdClass = IdClass;
				it.Quantity = Quantity;
				it.HSD = ((HSD != -1) ? (Util.CurrentTimeMillis() + HSD) : HSD);
				RandomOption(it, Options);
				InventoryHander.AddItemBag(_myChar, it, SendThongBao: false);
				TaskHander.CheckBuyItem(_myChar, it);
				MsgBuyShop(_myChar, it);
			}
		}

		private static short CheckItemListBuy(Character _myChar, short ItemId)
		{
			if (ItemId == 159 || ItemId == 162 || ItemId == 165 || ItemId == 168 || ItemId == 171 || ItemId == 174 || ItemId == 177 || ItemId == 180 || ItemId == 183 || ItemId == 186)
			{
				if (InventoryHander.GetCountNotNullBag(_myChar) < 3)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Cần ít nhất 3 ô trống hành trang", Util.WHITE));
				}
				return 3;
			}
			return 0;
		}

		private static void RandomOption(Item it, string Options)
		{
			if (Options.Equals(""))
			{
				return;
			}
			string[] op = Options.Split(";");
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < op.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(";");
				}
				string[] Value = op[i].Split(",");
				short Id = short.Parse(Value[0]);
				int Value2 = int.Parse(Value[1]);
				if (Id == 128)
				{
					stringBuilder.Append(Id).Append(",").Append(Value2)
						.Append(",")
						.Append(int.Parse(Value[2]));
					continue;
				}
				int Value3 = -1;
				if (Value.Length > 2)
				{
					Value3 = int.Parse(Value[2]);
					if (Value2 < Value3)
					{
						Value2 = Util.NextInt(Value2, Value3);
					}
				}
				stringBuilder.Append(Id).Append(",").Append(Value2);
			}
			it.Options = stringBuilder.ToString();
		}

		private static Message MsgCharSale(int BacKhoa, int Bac, short Index)
		{
			Message i = new Message(119);
			i.WriteInt(BacKhoa);
			i.WriteInt(Bac);
			i.WriteShort(Index);
			return i;
		}

		public static void MsgBuyShop(Character _myChar, Item it)
		{
			Message i = new Message(121);
			i.WriteInt(_myChar.Inventory.Bac);
			i.WriteInt(_myChar.Inventory.BacKhoa);
			i.WriteInt(_myChar.Inventory.Vang);
			i.WriteInt(_myChar.Inventory.VangKhoa);
			i.WriteShort(1);
			ItemHander.WriteItem(i, it);
			_myChar.SendMessage(i);
		}

		public static void MsgBuyShopMuilti(Character _myChar, Item[] it)
		{
			Message j = new Message(121);
			j.WriteInt(_myChar.Inventory.Bac);
			j.WriteInt(_myChar.Inventory.BacKhoa);
			j.WriteInt(_myChar.Inventory.Vang);
			j.WriteInt(_myChar.Inventory.VangKhoa);
			j.WriteShort((short)it.Length);
			foreach (Item i in it)
			{
				ItemHander.WriteItem(j, i);
			}
			_myChar.SendMessage(j);
		}
	}
}
