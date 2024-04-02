using System.Collections.Generic;
using System.Text;
using LangLa.Data;
using LangLa.IO;
using LangLa.Model;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.Hander
{
	public static class CombineHander
	{
		private static Message MsgGhepData(int Bac, int BacKhoa, short[] Index, Item itNew)
		{
			Message i = new Message(108);
			i.WriteInt(Bac);
			i.WriteInt(BacKhoa);
			i.WriteByte((sbyte)Index.Length);
			foreach (short s in Index)
			{
				i.WriteShort(s);
			}
			ItemHander.WriteItem(i, itNew);
			return i;
		}

		private static Message MsgCuongHoa(bool IsThanhCong, bool var1, int Bac, int BacKhoa, short[] IndexDa, Item itNew, sbyte TypeTemp, short IndexBua)
		{
			Message i = new Message(107);
			i.WriteBool(IsThanhCong);
			i.WriteBool(x: false);
			i.WriteInt(Bac);
			i.WriteInt(BacKhoa);
			i.WriteByte((sbyte)IndexDa.Length);
			foreach (short s in IndexDa)
			{
				i.WriteShort(s);
			}
			i.WriteShort(IndexBua);
			ItemHander.WriteItem(i, itNew);
			i.WriteByte(TypeTemp);
			return i;
		}

		private static Message MsgTachCuongHoa(Item item, sbyte TypeTemp)
		{
			Message i = new Message(105);
			ItemHander.WriteItem(i, item);
			i.WriteByte(TypeTemp);
			return i;
		}

		private static Message MsgNangCapBuaNo(Item item, sbyte TypeTemp, short[] Index)
		{
			Message i = new Message(106);
			ItemHander.WriteItem(i, item);
			i.WriteByte(TypeTemp);
			i.WriteByte((sbyte)Index.Length);
			foreach (short s in Index)
			{
				i.WriteShort(s);
			}
			return i;
		}

		private static Message MsgKhamNgoc(Item item, short[] Index, sbyte TypeTemp)
		{
			Message i = new Message(-46);
			i.WriteByte((sbyte)Index.Length);
			foreach (short c in Index)
			{
				i.WriteShort(c);
			}
			ItemHander.WriteItem(i, item);
			i.WriteByte(TypeTemp);
			return i;
		}

		private static Message MsgGhepCaiTrang(Item it, sbyte[] TypeTemp, short[] Index)
		{
			Message j = new Message(-50);
			j.WriteByte((sbyte)Index.Length);
			for (int i = 0; i < Index.Length; i++)
			{
				j.WriteByte(TypeTemp[i]);
				if (i == 0)
				{
					ItemHander.WriteItem(j, it);
				}
				else
				{
					j.WriteShort(Index[i]);
				}
			}
			return j;
		}

		private static Message MsgShowOptionTachCT(Item[] it)
		{
			Message j = new Message(-52);
			j.WriteByte((sbyte)it.Length);
			foreach (Item i in it)
			{
				ItemHander.WriteItem(j, i);
			}
			return j;
		}

		private static Message MsgTachCaiTrang(sbyte Type, short Index, Item it)
		{
			Message i = new Message(-51);
			i.WriteByte(Type);
			i.WriteShort(Index);
			ItemHander.WriteItem(i, it);
			return i;
		}

		private static Message MsgDichChuyenTrangBi(Item item, sbyte Type1, Item item1, sbyte Type2, short Index)
		{
			Message i = new Message(104);
			ItemHander.WriteItem(i, item);
			i.WriteByte(Type1);
			ItemHander.WriteItem(i, item1);
			i.WriteByte(Type2);
			i.WriteShort(Index);
			return i;
		}

		private static Message MsgTachKhamNgoc(Item item, sbyte Index)
		{
			Message i = new Message(-47);
			ItemHander.WriteItem(i, item);
			i.WriteByte(Index);
			return i;
		}

		public static void HanderGhepData(Character _myChar, Message msg)
		{
			bool IsDungBacKhoa = msg.ReadBool();
			sbyte Size = msg.ReadByte();
			long TiLeThanhCong = 0L;
			int LevelNangCap = 0;
			short[] Index = new short[Size];
			for (int j = 0; j < Size; j++)
			{
				Index[j] = msg.ReadShort();
				if (Index[j] >= _myChar.Inventory.ItemBag.Count || _myChar.Inventory.ItemBag[Index[j]] == null)
				{
					return;
				}
				int Id = _myChar.Inventory.ItemBag[Index[j]].Id;
				if (Id >= DataServer.DiemGhepDa.Length)
				{
					return;
				}
				TiLeThanhCong += DataServer.DiemGhepDa[Id];
			}
			LevelNangCap = DataServer.DiemGhepDa.Length - 1;
			while (LevelNangCap >= 0 && TiLeThanhCong <= DataServer.DiemGhepDa[LevelNangCap])
			{
				LevelNangCap--;
			}
			if (TiLeThanhCong == 0)
			{
				return;
			}
			int BacGhepDa = ((LevelNangCap + 1 <= DataServer.BacGhepDa.Length) ? DataServer.BacGhepDa[LevelNangCap + 1] : DataServer.BacGhepDa[DataServer.BacGhepDa.Length - 1]);
			if (!((!IsDungBacKhoa) ? (_myChar.Inventory.Bac > BacGhepDa) : (_myChar.Inventory.BacKhoa > BacGhepDa)))
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ bạc", Util.WHITE));
				return;
			}
			bool IsThanhCong = TiLeThanhCong > Util.NextInt(0, 100);
			if (IsDungBacKhoa)
			{
				InventoryHander.UpdateBackhoa(_myChar, BacGhepDa);
			}
			else
			{
				InventoryHander.UpdateBac(_myChar, BacGhepDa);
			}
			Item it = (IsThanhCong ? new Item((short)(LevelNangCap + 1), IsLock: true) : new Item((short)LevelNangCap, IsLock: true));
			for (int i = 0; i < Index.Length; i++)
			{
				_myChar.Inventory.ItemBag[Index[i]] = null;
			}
			InventoryHander.AddItemBag(_myChar, it, SendThongBao: false);
			_myChar.SendMessage(MsgGhepData(_myChar.Inventory.Bac, _myChar.Inventory.BacKhoa, Index, it));
		}

		public static void HanderCuongHoaTrangBi(Character _myChar, Message msg)
		{
			sbyte TypeTemp = msg.ReadByte();
			short Index1 = msg.ReadShort();
			short Index2 = msg.ReadShort();
			sbyte Size = msg.ReadByte();
			short[] IndexDa = new short[Size];
			long TileThanhCong = 0L;
			int BacNangCap = 0;
			long DiemNangCap = 0L;
			for (int j = 0; j < IndexDa.Length; j++)
			{
				IndexDa[j] = msg.ReadShort();
				if (IndexDa[j] >= _myChar.Inventory.ItemBag.Count)
				{
					return;
				}
				int Id = _myChar.Inventory.ItemBag[IndexDa[j]].Id;
				if (Id >= DataServer.DiemGhepDa.Length)
				{
					return;
				}
				TileThanhCong += DataServer.DiemGhepDa[Id];
			}
			Item itemNangCap = _myChar.GetItemFromType(TypeTemp, Index1);
			if (itemNangCap == null)
			{
				return;
			}
			if (itemNangCap.Type == 1 && itemNangCap.Level <= 19)
			{
				BacNangCap = DataServer.BacNangCapVuKhi[itemNangCap.Level + 1];
				DiemNangCap = DataServer.DiemNangCapVuKhi[itemNangCap.Level + 1];
			}
			else if ((itemNangCap.Type == 0 || itemNangCap.Type == 2 || itemNangCap.Type == 4 || itemNangCap.Type == 6 || itemNangCap.Type == 8) && itemNangCap.Level <= 19)
			{
				BacNangCap = DataServer.BacNangCapTrangBi[itemNangCap.Level + 1];
				DiemNangCap = DataServer.DiemNangCapTrangBi[itemNangCap.Level + 1];
			}
			else if ((itemNangCap.Type == 3 || itemNangCap.Type == 5 || itemNangCap.Type == 7 || itemNangCap.Type == 9) && itemNangCap.Level <= 19)
			{
				BacNangCap = DataServer.BacNangCapPhuKien[itemNangCap.Level + 1];
				DiemNangCap = DataServer.DiemNangCapPhuKien[itemNangCap.Level + 1];
			}
			if (_myChar.Inventory.BacKhoa < BacNangCap)
			{
				return;
			}
			if (DiemNangCap == 0)
			{
				DiemNangCap = 1L;
			}
			InventoryHander.UpdateBackhoa(_myChar, BacNangCap);
			TileThanhCong = TileThanhCong * 100 / DiemNangCap;
			if (Index2 != -1)
			{
				TileThanhCong += 3;
				_myChar.Inventory.ItemBag[Index2] = null;
			}
			bool IsThanhCong = TileThanhCong > 0;
			for (int i = 0; i < IndexDa.Length; i++)
			{
				_myChar.Inventory.ItemBag[IndexDa[i]] = null;
			}
			if (IsThanhCong)
			{
				string OptionRemove = itemNangCap.Options;
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(itemNangCap, _myChar, IsDownPoint: true, OptionRemove);
				}
				ItemOptionHander.UpOptionCuongHoa(itemNangCap);
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(itemNangCap, _myChar);
				}
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 10)
				{
					TaskHander.NextStep(_myChar);
				}
				_myChar.Info.PointCuongHoa += ((BacNangCap / 10 >= 0) ? (BacNangCap / 10) : 0);
				if (itemNangCap.Level >= 15)
				{
					LangLa.Server.Server.SendThongBaoFromServer("Chúc mừng nhẫn giả " + _myChar.Info.Name + " vừa cường hóa trang bị " + DataServer.ArrItemTemplate[itemNangCap.Id].name + "+" + itemNangCap.Level);
				}
			}
			else if (itemNangCap.Level >= 15)
			{
				LangLa.Server.Server.SendThongBaoFromServer("Nhẫn giả " + _myChar.Info.Name + " vừa cường hóa thất bại trang bị " + DataServer.ArrItemTemplate[itemNangCap.Id].name + " +" + (itemNangCap.Level + 1));
			}
			_myChar.SendMessage(MsgCuongHoa(IsThanhCong, var1: false, _myChar.Inventory.Bac, _myChar.Inventory.BacKhoa, IndexDa, itemNangCap, TypeTemp, Index2));
		}

		public static void HanderTachCuongHoa(Character _myChar, Message msg)
		{
			sbyte TypeTemp = msg.ReadByte();
			short Index = msg.ReadShort();
			Item itemTach = _myChar.GetItemFromType(TypeTemp, Index);
			if (itemTach == null)
			{
				return;
			}
			sbyte LevelItem = itemTach.Level;
			int BacUp = 0;
			long Point = 0L;
			int PointGhepDa = 0;
			int[] BacArr = null;
			long[] PointArr = null;
			if (itemTach.Type == 1)
			{
				BacArr = DataServer.BacNangCapVuKhi;
				PointArr = DataServer.DiemNangCapVuKhi;
			}
			else if (itemTach.Type == 0 || itemTach.Type == 2 || itemTach.Type == 4 || itemTach.Type == 6 || itemTach.Type == 8)
			{
				BacArr = DataServer.BacNangCapTrangBi;
				PointArr = DataServer.DiemNangCapTrangBi;
			}
			else if (itemTach.Type == 3 || itemTach.Type == 5 || itemTach.Type == 7 || itemTach.Type == 9)
			{
				BacArr = DataServer.BacNangCapPhuKien;
				PointArr = DataServer.DiemNangCapPhuKien;
			}
			if (BacArr == null || PointArr == null)
			{
				return;
			}
			for (int j = 0; j < LevelItem; j++)
			{
				BacUp += BacArr[j];
				Point += PointArr[j];
			}
			List<Item> itemsBackDa = new List<Item>();
			Point /= 3;
			for (short i = 0; i < DataServer.DiemGhepDa.Length; i++)
			{
				if (Point >= DataServer.DiemGhepDa[i])
				{
					Item item = new Item(i, IsLock: true);
					itemsBackDa.Add(item);
					if (itemsBackDa.Count >= 16)
					{
						break;
					}
				}
			}
			if (InventoryHander.GetCountNotNullBag(_myChar) < itemsBackDa.Count)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Hành trang không đủ " + itemsBackDa.Count, Util.WHITE));
				return;
			}
			string OptionRemove = itemTach.Options;
			if (TypeTemp == 2)
			{
				_myChar.TuongKhac.GetPointFromItem(itemTach, _myChar, IsDownPoint: true, OptionRemove);
			}
			ItemOptionHander.DonwOptionItem(itemTach, itemTach.Level);
			if (TypeTemp == 2)
			{
				_myChar.TuongKhac.GetPointFromItem(itemTach, _myChar);
			}
			InventoryHander.AddBac(_myChar, BacUp, ThongBao: true);
			InventoryHander.AddMultiItembag(_myChar, itemsBackDa.ToArray());
			_myChar.SendMessage(MsgTachCuongHoa(itemTach, TypeTemp));
		}

		public static void HanderNangCapBuaNo(Character _myChar, Message msg)
		{
			sbyte IndexSelect = msg.ReadByte();
			sbyte TypeTemp = msg.ReadByte();
			short Index = msg.ReadShort();
			short[] IndexTinhThach = new short[msg.ReadByte()];
			int ValueAdd = 0;
			for (int k = 0; k < IndexTinhThach.Length; k++)
			{
				IndexTinhThach[k] = msg.ReadShort();
				if (IndexTinhThach[k] == -1 || IndexTinhThach[k] >= _myChar.Inventory.ItemBag.Count)
				{
					return;
				}
				ValueAdd += _myChar.Inventory.ItemBag[IndexTinhThach[k]].Quantity / 10;
			}
			Item BuaNo = _myChar.GetItemFromType(TypeTemp, Index);
			if (BuaNo == null || ValueAdd <= 0)
			{
				return;
			}
			short IdBuaNo = BuaNo.Id;
			short MaxValue = 0;
			switch (IdBuaNo)
			{
			case 134:
				if (ValueAdd > 500)
				{
					ValueAdd = 500;
				}
				MaxValue = 500;
				break;
			case 602:
				if (ValueAdd > 1000)
				{
					ValueAdd = 1000;
				}
				MaxValue = 1000;
				break;
			case 811:
				if (ValueAdd > 1500)
				{
					ValueAdd = 1500;
				}
				MaxValue = 1500;
				if (BuaNo.CountBuaNo > 0)
				{
					MaxValue += (short)(BuaNo.CountBuaNo * 150);
				}
				break;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string OptionRemove = BuaNo.Options;
			string[] Option = BuaNo.Options.Split(";");
			short Value2 = 0;
			for (int j = 0; j < Option.Length; j++)
			{
				string[] Value = Option[j].Split(",");
				short IdOption = short.Parse(Value[0]);
				short ValueOption = short.Parse(Value[1]);
				if (j > 0)
				{
					stringBuilder.Append(";");
				}
				if (IdOption >= 53 && IdOption <= 62)
				{
					if ((IndexSelect == 0 && IdOption >= 53 && IdOption <= 57) || (IndexSelect == 1 && IdOption >= 58 && IdOption <= 62))
					{
						ValueOption += (short)ValueAdd;
						if (ValueOption > MaxValue)
						{
							ValueOption = MaxValue;
						}
					}
					Value2 += ValueOption;
				}
				stringBuilder.Append(IdOption).Append(",").Append(ValueOption);
				if (Value.Length > 2)
				{
					stringBuilder.Append(",").Append(MaxValue);
				}
			}
			BuaNo.Options = stringBuilder.ToString();
			int k2 = MaxValue * 2 / 16;
			BuaNo.Level = (sbyte)(Value2 / 200);
			if (BuaNo.Level < 0)
			{
				BuaNo.Level = 0;
			}
			if (TypeTemp == 2)
			{
				_myChar.TuongKhac.GetPointFromItem(BuaNo, _myChar, IsDownPoint: true, OptionRemove);
				_myChar.TuongKhac.GetPointFromItem(BuaNo, _myChar);
			}
			_myChar.SendMessage(MsgNangCapBuaNo(BuaNo, TypeTemp, IndexTinhThach));
			for (int i = 0; i < IndexTinhThach.Length; i++)
			{
				_myChar.Inventory.ItemBag[IndexTinhThach[i]] = null;
			}
		}

		public static int getIdOptionNgoc(int var0)
		{
			return var0 switch
			{
				406 => 199, 
				407 => 200, 
				408 => 201, 
				409 => 202, 
				410 => 203, 
				411 => 204, 
				412 => 205, 
				413 => 206, 
				826 => 344, 
				827 => 345, 
				_ => -1, 
			};
		}

		public static void HanderKhamNgoc(Character _myChar, Message msg)
		{
			sbyte TypeTemp = msg.ReadByte();
			short Index = msg.ReadShort();
			short[] IndexNgoc = new short[msg.ReadByte()];
			int Quantity = 0;
			short IdKham = -1;
			for (int k = 0; k < IndexNgoc.Length; k++)
			{
				IndexNgoc[k] = msg.ReadShort();
				if (IndexNgoc[k] == -1 || IndexNgoc[k] >= _myChar.Inventory.ItemBag.Count)
				{
					return;
				}
				Quantity += _myChar.Inventory.ItemBag[IndexNgoc[k]].Quantity;
				IdKham = _myChar.Inventory.ItemBag[IndexNgoc[k]].Id;
			}
			if (Quantity == 0)
			{
				return;
			}
			Item ItemKham = _myChar.GetItemFromType(TypeTemp, Index);
			if (ItemKham == null)
			{
				return;
			}
			sbyte MaxNgocKham = 1;
			string[] OptionsCheck = ItemKham.Options.Split(";");
			item_template itemTemplate = DataServer.ArrItemTemplate[ItemKham.Id];
			MaxNgocKham = ((itemTemplate.level_need >= 50) ? ((sbyte)(MaxNgocKham + 4)) : ((itemTemplate.level_need < 40) ? ((sbyte)(MaxNgocKham + (sbyte)((itemTemplate.level_need < 30) ? 1 : 2))) : ((sbyte)(MaxNgocKham + 3))));
			sbyte Count = 0;
			for (int n = 0; n < OptionsCheck.Length; n++)
			{
				short Id3 = short.Parse(OptionsCheck[n].Split(",")[0]);
				ItemOptionTemplate itemOptionTemplate2 = DataServer.ArrItemOptionTemplate[Id3];
				if (itemOptionTemplate2.Type == 8)
				{
					Count++;
				}
				if (Count >= MaxNgocKham)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Trang bị đã khảm tối đa ngọc", Util.WHITE));
					return;
				}
			}
			int QantityUpLevel = 0;
			sbyte Level = -1;
			short ValueAdd = 0;
			int var3 = 17;
			int IdOption = getIdOptionNgoc(IdKham);
			bool check = false;
			for (int m = 0; m < OptionsCheck.Length; m++)
			{
				short Id2 = short.Parse(OptionsCheck[m].Split(",")[0]);
				if (Id2 == IdOption)
				{
					check = true;
					break;
				}
			}
			string OptionRemove = ItemKham.Options;
			string OptionNew = ItemKham.Options;
			if (!check)
			{
				OptionNew = OptionNew + ";" + IdOption + ",0";
			}
			string[] Options2 = OptionNew.Split(";");
			int[] PointNgoc = DataServer.NgocKhamPoint;
			StringBuilder stringBuilder = new StringBuilder();
			for (int l = 0; l < Options2.Length; l++)
			{
				if (l > 0)
				{
					stringBuilder.Append(";");
				}
				short Id = short.Parse(Options2[l].Split(",")[0]);
				short ValueItem = short.Parse(Options2[l].Split(",")[1]);
				ItemOptionTemplate itemOptionTemplate3 = DataServer.ArrItemOptionTemplate[Id];
				if (itemOptionTemplate3.Type == 8 && Id == IdOption)
				{
					ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[Id];
					string[] OptionsAdds = itemOptionTemplate.Options.Split(";");
					for (int j = 1; j <= OptionsAdds.Length; j++)
					{
						if (Quantity >= PointNgoc[j] && j <= 16)
						{
							Level = (sbyte)j;
							QantityUpLevel += PointNgoc[j];
							ValueAdd += short.Parse(OptionsAdds[j - 1]);
							Quantity -= PointNgoc[j];
						}
					}
					stringBuilder.Append(Id).Append(",").Append(ValueAdd)
						.Append(",")
						.Append(-1)
						.Append(",")
						.Append(Level);
				}
				else
				{
					stringBuilder.Append(Id).Append(",").Append(ValueItem);
					if (Id >= 199 && Id <= 206 && Options2[l].Length > 2)
					{
						stringBuilder.Append(",").Append(Options2[l].Split(",")[2]).Append(",")
							.Append(Options2[l].Split(",")[3]);
					}
				}
			}
			ItemKham.Options = stringBuilder.ToString();
			if (TypeTemp == 2)
			{
				_myChar.TuongKhac.GetPointFromItem(ItemKham, _myChar, IsDownPoint: true, OptionRemove);
				_myChar.TuongKhac.GetPointFromItem(ItemKham, _myChar);
			}
			for (int i = 0; i < IndexNgoc.Length; i++)
			{
				_myChar.Inventory.ItemBag[IndexNgoc[i]] = null;
			}
			_myChar.SendMessage(MsgKhamNgoc(ItemKham, IndexNgoc, TypeTemp));
		}

		public static void ShowOptionCaiTrangTach(Character _myChar, Message msg)
		{
			sbyte Type = msg.ReadByte();
			short Index = msg.ReadShort();
			Item itemTach = _myChar.GetItemFromType(Type, Index);
			if (itemTach != null && itemTach.OptionsCTBack != null)
			{
				string[] ItemBack = itemTach.OptionsCTBack.Split("#");
				_myChar.InfoGame.CaiTrangBack = null;
				if (_myChar.InfoGame.CaiTrangBack == null)
				{
					_myChar.InfoGame.CaiTrangBack = new Item[ItemBack.Length];
				}
				for (int i = 0; i < ItemBack.Length; i++)
				{
					short Id = short.Parse(ItemBack[i].Split("@")[0]);
					Item item = new Item(Id, IsLock: true);
					item.IdClass = -1;
					item.Options = ItemBack[i].Split("@")[1];
					_myChar.InfoGame.CaiTrangBack[i] = item;
				}
				_myChar.SendMessage(MsgShowOptionTachCT(_myChar.InfoGame.CaiTrangBack));
			}
		}

		public static void HanderGhepCaiTrang(Character _myChar, Message msg)
		{
			sbyte SizeGhep = msg.ReadByte();
			sbyte[] TypeTemp = new sbyte[SizeGhep];
			short[] IndexCt = new short[SizeGhep];
			for (int j = 0; j < SizeGhep; j++)
			{
				TypeTemp[j] = msg.ReadByte();
				IndexCt[j] = msg.ReadShort();
			}
			Item CaiTrangUp = _myChar.GetItemFromType(TypeTemp[0], IndexCt[0]);
			if (CaiTrangUp == null)
			{
				return;
			}
			sbyte TYpeItem = TypeTemp[0];
			string OptionRemove = CaiTrangUp.Options;
			Item[] itGhep = new Item[SizeGhep];
			for (int i = 0; i < SizeGhep; i++)
			{
				itGhep[i] = _myChar.GetItemFromType(TypeTemp[i], IndexCt[i]);
				if (i != 0)
				{
					_myChar.SetNullItemFromType(TypeTemp[i], IndexCt[i]);
				}
			}
			if (TYpeItem == 2)
			{
				_myChar.TuongKhac.GetPointFromItem(CaiTrangUp, _myChar, IsDownPoint: true, OptionRemove);
			}
			ItemOptionHander.AddOptionCaiTrang(CaiTrangUp, itGhep);
			if (TYpeItem == 2)
			{
				_myChar.TuongKhac.GetPointFromItem(CaiTrangUp, _myChar);
			}
			_myChar.SendMessage(MsgGhepCaiTrang(CaiTrangUp, TypeTemp, IndexCt));
		}

		public static void HanderTachCaiTrang(Character _myChar, Message msg)
		{
			sbyte Type = msg.ReadByte();
			short Index = msg.ReadShort();
			Item itemTach = _myChar.GetItemFromType(Type, Index);
			if (itemTach == null || _myChar.InfoGame.CaiTrangBack == null)
			{
				return;
			}
			if (InventoryHander.GetCountNotNullBag(_myChar) > _myChar.InfoGame.CaiTrangBack.Length)
			{
				if (Type == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(itemTach, _myChar, IsDownPoint: true, itemTach.Options);
				}
				_myChar.SetNullItemFromType(Type, Index);
				string[] ItemBack = itemTach.OptionsCTBack.Split("#");
				_myChar.SendMessage(MsgTachCaiTrang(Type, Index, _myChar.InfoGame.CaiTrangBack[0]));
				InventoryHander.AddMultiItembag(_myChar, _myChar.InfoGame.CaiTrangBack);
				_myChar.InfoGame.CaiTrangBack = null;
			}
			else
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Hành trang không đủ ô trống", Util.WHITE));
			}
		}

		public static void HanderDichChuyenTrangBi(Character _myChar, Message msg)
		{
			sbyte TypeTemp1 = msg.ReadByte();
			short Index1 = msg.ReadShort();
			sbyte TypeTemp2 = msg.ReadByte();
			short Index2 = msg.ReadShort();
			short Index3 = msg.ReadShort();
			Item item1 = _myChar.GetItemFromType(TypeTemp1, Index1);
			Item item2 = _myChar.GetItemFromType(TypeTemp2, Index2);
			Item item3 = _myChar.Inventory.ItemBag[Index3];
			if (item1 != null && item2 != null && item3 != null)
			{
				string Option = item1.Options;
				if (TypeTemp1 == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item1, _myChar, IsDownPoint: true, Option);
				}
				ItemOptionHander.DichChuyenOptionTrangBi(item1, item2);
				if (TypeTemp1 == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item1, _myChar);
				}
				_myChar.Inventory.ItemBag[Index3] = null;
				_myChar.SendMessage(MsgDichChuyenTrangBi(item1, TypeTemp1, item2, TypeTemp2, Index3));
			}
			else
			{
				Util.ShowLog("FAIL");
			}
		}

		public static short GetIdItemFromOptionDa(short IdOption)
		{
			return IdOption switch
			{
				199 => 406, 
				200 => 407, 
				201 => 408, 
				202 => 409, 
				203 => 410, 
				204 => 411, 
				205 => 412, 
				206 => 413, 
				344 => 826, 
				345 => 827, 
				_ => -1, 
			};
		}

		public static void HanderTachNgocKham(Character _myChar, Message msg)
		{
			sbyte TypeTemp = msg.ReadByte();
			short Index = msg.ReadShort();
			short i2 = msg.ReadShort();
			Item item = _myChar.GetItemFromType(TypeTemp, Index);
			if (item == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int Money = 0;
			string ItemBack = "";
			sbyte CountNgoc = 0;
			ItemOption[] var11;
			if ((var11 = item.L(IsSet: true)) == null)
			{
				return;
			}
			for (int var12 = 0; var12 < var11.Length; var12++)
			{
				if (var11[var12].a.Length > 2)
				{
					int var13 = var11[var12].a[3];
					int Quantity = 0;
					for (int var10 = 0; var10 <= var13; var10++)
					{
						Money += DataServer.NgocKhamPoint[var10];
						Quantity += DataServer.NgocKhamPoint[var10];
					}
					if (CountNgoc != 0)
					{
						ItemBack += ";";
					}
					ItemBack = ItemBack + var11[var12].a[0] + "," + Quantity;
					CountNgoc++;
				}
				else
				{
					if (var12 > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(var11[var12].g());
				}
			}
			if (Money > 600)
			{
				Money = 600;
			}
			if (_myChar.Inventory.Vang < Money)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.WHITE));
			}
			else if (InventoryHander.GetCountNotNullBag(_myChar) > CountNgoc)
			{
				string[] Item = ItemBack.Split(";");
				Item[] items = new Item[Item.Length];
				for (int i = 0; i < Item.Length; i++)
				{
					Item item2 = new Item(GetIdItemFromOptionDa(short.Parse(Item[i].Split(",")[0])), IsLock: true);
					item2.Quantity = short.Parse(Item[i].Split(",")[1]);
					items[i] = item2;
				}
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar, IsDownPoint: true);
				}
				InventoryHander.AddMultiItembag(_myChar, items);
				item.Options = stringBuilder.ToString();
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar);
				}
				InventoryHander.UpdateVang(_myChar, Money, IsThongBao: true);
				_myChar.SendMessage(MsgTachKhamNgoc(item, TypeTemp));
			}
			else
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Hành trang không đủ ô trống", Util.WHITE));
			}
		}

		private static Message MsgUpVuKhiHienNhan(Item item, sbyte TypeTemp, short[] Index)
		{
			Message i = new Message(-35);
			ItemHander.WriteItem(i, item);
			i.WriteByte(TypeTemp);
			i.WriteByte((sbyte)Index.Length);
			foreach (short s in Index)
			{
				i.WriteShort(s);
			}
			return i;
		}

		public static void CheckUpVuKhiNew(Character _myChar, Message msg)
		{
			sbyte TypeTemp = msg.ReadByte();
			short Index = msg.ReadShort();
			short[] IndexNgoc = new short[msg.ReadByte()];
			int Quantity = 0;
			for (int i = 0; i < IndexNgoc.Length; i++)
			{
				IndexNgoc[i] = msg.ReadShort();
				Quantity += _myChar.Inventory.ItemBag[IndexNgoc[i]].Quantity;
			}
			switch (_myChar.InfoGame.ID_Click_SHOP)
			{
			case 64:
				UpVuKhiHienNhan(_myChar, TypeTemp, Index, IndexNgoc, Quantity);
				break;
			case 65:
				UpdateVuKhiSharingan(_myChar, TypeTemp, Index, IndexNgoc, Quantity);
				break;
			case 66:
				UpdateVuKhiByakugan(_myChar, TypeTemp, Index, IndexNgoc, Quantity);
				break;
			case 67:
				UpdateVuKhiRinnegan(_myChar, TypeTemp, Index, IndexNgoc, Quantity);
				break;
			}
		}

		private static void UpdateVuKhiSharingan(Character _myChar, sbyte TypeTemp, short Index, short[] IndexNgoc, int Quantity)
		{
			Item item = _myChar.GetItemFromType(TypeTemp, Index);
			if (item == null)
			{
				return;
			}
			item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
			int BacUp = 15000000;
			int QuantityUp = itemTemplate.level_need / 10 * 100;
			if (itemTemplate.level_need / 10 == 5)
			{
				BacUp = 20000000;
			}
			else if (itemTemplate.level_need / 10 == 6)
			{
				BacUp = 40000000;
				QuantityUp = 700;
			}
			if (_myChar.Inventory.Bac < BacUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ bạc", Util.YELLOW_MID));
			}
			else if (Quantity < QuantityUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ " + DataServer.ArrItemTemplate[_myChar.Inventory.ItemBag[IndexNgoc[0]].Id].name, Util.YELLOW_MID));
			}
			else if (_myChar.Inventory.ItemBag[IndexNgoc[0]].Id == 565 && itemTemplate.level_need >= 40)
			{
				string Options = item.Options;
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar, IsDownPoint: true, Options);
				}
				ItemOptionHander.UpOptionSharingan(item);
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar);
				}
				for (int i = 0; i < IndexNgoc.Length; i++)
				{
					_myChar.Inventory.ItemBag[IndexNgoc[i]] = null;
				}
				InventoryHander.UpdateBac(_myChar, BacUp, ThongBao: true);
				_myChar.SendMessage(MsgUpVuKhiHienNhan(item, TypeTemp, IndexNgoc));
				LangLa.Server.Server.SendThongBaoFromServer("Nhẫn giả " + _myChar.Info.Name + " vừa cường hóa thành công " + itemTemplate.name + " trở thành trang bị Sharingan");
			}
		}

		private static void UpdateVuKhiRinnegan(Character _myChar, sbyte TypeTemp, short Index, short[] IndexNgoc, int Quantity)
		{
			Item item = _myChar.GetItemFromType(TypeTemp, Index);
			if (item == null)
			{
				return;
			}
			item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
			int BacUp = 25000000;
			int QuantityUp = itemTemplate.level_need / 10 * 100;
			if (itemTemplate.level_need / 10 == 5)
			{
				BacUp = 30000000;
			}
			else if (itemTemplate.level_need / 10 == 6)
			{
				BacUp = 40000000;
				QuantityUp = 700;
			}
			if (_myChar.Inventory.Bac < BacUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ bạc", Util.YELLOW_MID));
			}
			else if (Quantity < QuantityUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ " + DataServer.ArrItemTemplate[_myChar.Inventory.ItemBag[IndexNgoc[0]].Id].name, Util.YELLOW_MID));
			}
			else if (_myChar.Inventory.ItemBag[IndexNgoc[0]].Id == 567 && itemTemplate.level_need >= 40)
			{
				string Optionback = item.Options;
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar, IsDownPoint: true, Optionback);
				}
				ItemOptionHander.UpOptionRinneGan(item);
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar);
				}
				for (int i = 0; i < IndexNgoc.Length; i++)
				{
					_myChar.Inventory.ItemBag[IndexNgoc[i]] = null;
				}
				InventoryHander.UpdateBac(_myChar, BacUp, ThongBao: true);
				_myChar.SendMessage(MsgUpVuKhiHienNhan(item, TypeTemp, IndexNgoc));
				LangLa.Server.Server.SendThongBaoFromServer("Nhẫn giả " + _myChar.Info.Name + " vừa cường hóa thành công " + itemTemplate.name + " trở thành trang bị RinneGan");
			}
		}

		private static void UpdateVuKhiByakugan(Character _myChar, sbyte TypeTemp, short Index, short[] IndexNgoc, int Quantity)
		{
			Item item = _myChar.GetItemFromType(TypeTemp, Index);
			if (item == null)
			{
				return;
			}
			item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
			int BacUp = 25000000;
			int QuantityUp = itemTemplate.level_need / 10 * 100;
			if (itemTemplate.level_need / 10 == 5)
			{
				BacUp = 30000000;
			}
			else if (itemTemplate.level_need / 10 == 6)
			{
				BacUp = 40000000;
				QuantityUp = 700;
			}
			if (_myChar.Inventory.Bac < BacUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ bạc", Util.YELLOW_MID));
			}
			else if (Quantity < QuantityUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ " + DataServer.ArrItemTemplate[_myChar.Inventory.ItemBag[IndexNgoc[0]].Id].name, Util.YELLOW_MID));
			}
			else if (_myChar.Inventory.ItemBag[IndexNgoc[0]].Id == 563 && itemTemplate.level_need >= 40)
			{
				string Optionback = item.Options;
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar, IsDownPoint: true, Optionback);
				}
				ItemOptionHander.UpOptionByaKugan(item);
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar);
				}
				for (int i = 0; i < IndexNgoc.Length; i++)
				{
					_myChar.Inventory.ItemBag[IndexNgoc[i]] = null;
				}
				InventoryHander.UpdateBac(_myChar, BacUp, ThongBao: true);
				_myChar.SendMessage(MsgUpVuKhiHienNhan(item, TypeTemp, IndexNgoc));
				LangLa.Server.Server.SendThongBaoFromServer("Nhẫn giả " + _myChar.Info.Name + " vừa cường hóa thành công " + itemTemplate.name + " trở thành trang bị Byakugan");
			}
		}

		public static void UpdateVuKhiLucDao(Character _myChar, Message msg)
		{
			sbyte TypeTemp = msg.ReadByte();
			short Index = msg.ReadShort();
			short[] IndexNgoc = new short[msg.ReadByte()];
			int Quantity = 0;
			for (int j = 0; j < IndexNgoc.Length; j++)
			{
				IndexNgoc[j] = msg.ReadShort();
				Quantity += _myChar.Inventory.ItemBag[IndexNgoc[j]].Quantity;
			}
			Item itemUp = _myChar.GetItemFromType(TypeTemp, Index);
			if (itemUp == null)
			{
				return;
			}
			item_template itemTemplate = DataServer.ArrItemTemplate[itemUp.Id];
			short IdNgoc = _myChar.Inventory.ItemBag[IndexNgoc[0]].Id;
			if (itemTemplate.type == 1 && IdNgoc != 353)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Yêu cầu Ngọc Myoboku", Util.YELLOW_MID));
				return;
			}
			if ((itemTemplate.type == 5 || itemTemplate.type == 6 || itemTemplate.type == 9) && IdNgoc != 563)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Yêu cầu Ngọc Byakugan", Util.YELLOW_MID));
				return;
			}
			if ((itemTemplate.type == 2 || itemTemplate.type == 7 || itemTemplate.type == 8) && IdNgoc != 565)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Yêu cầu Ngọc Sharingan", Util.YELLOW_MID));
				return;
			}
			if ((itemTemplate.type == 0 || itemTemplate.type == 4 || itemTemplate.type == 3) && IdNgoc != 567)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Yêu cầu Ngọc Rinnegan", Util.YELLOW_MID));
				return;
			}
			int QuantityUp = itemTemplate.level_need / 10 * 10 + 150;
			int BacUp = itemTemplate.level_need / 10 * 10000000 + 15000000;
			if (_myChar.Inventory.Bac < BacUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ bạc khóa", Util.YELLOW_MID));
				return;
			}
			if (Quantity < QuantityUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ " + DataServer.ArrItemTemplate[_myChar.Inventory.ItemBag[IndexNgoc[0]].Id].name, Util.YELLOW_MID));
				return;
			}
			InventoryHander.UpdateBackhoa(_myChar, BacUp);
			for (int i = 0; i < IndexNgoc.Length; i++)
			{
				_myChar.Inventory.ItemBag[IndexNgoc[i]] = null;
			}
			string OptionsBack = itemUp.Options;
			if (TypeTemp == 2)
			{
				_myChar.TuongKhac.GetPointFromItem(itemUp, _myChar, IsDownPoint: true, OptionsBack);
			}
			ItemOptionHander.UpOptionLucDao(itemUp, _myChar.Info.Name);
			if (TypeTemp == 2)
			{
				_myChar.TuongKhac.GetPointFromItem(itemUp, _myChar);
			}
			_myChar.SendMessage(MsgUpVuKhiHienNhan(itemUp, TypeTemp, IndexNgoc));
		}

		private static void UpVuKhiHienNhan(Character _myChar, sbyte TypeTemp, short Index, short[] IndexNgoc, int Quantity)
		{
			Item item = _myChar.GetItemFromType(TypeTemp, Index);
			if (item == null)
			{
				return;
			}
			item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
			int BacUp = 25000000;
			int QuantityUp = itemTemplate.level_need / 10 * 100;
			if (itemTemplate.level_need / 10 == 5)
			{
				BacUp = 30000000;
			}
			else if (itemTemplate.level_need / 10 == 6)
			{
				BacUp = 40000000;
				QuantityUp = 700;
			}
			if (_myChar.Inventory.Bac < BacUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ bạc", Util.YELLOW_MID));
			}
			else if (Quantity < QuantityUp)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ " + DataServer.ArrItemTemplate[_myChar.Inventory.ItemBag[IndexNgoc[0]].Id].name, Util.YELLOW_MID));
			}
			else if (_myChar.Inventory.ItemBag[IndexNgoc[0]].Id == 353 && itemTemplate.level_need >= 40)
			{
				string Options = item.Options;
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar, IsDownPoint: true, Options);
				}
				ItemOptionHander.UpOptionHienNhan(item);
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(item, _myChar);
				}
				for (int i = 0; i < IndexNgoc.Length; i++)
				{
					_myChar.Inventory.ItemBag[IndexNgoc[i]] = null;
				}
				InventoryHander.UpdateBac(_myChar, BacUp, ThongBao: true);
				_myChar.SendMessage(MsgUpVuKhiHienNhan(item, TypeTemp, IndexNgoc));
				LangLa.Server.Server.SendThongBaoFromServer("Nhẫn giả " + _myChar.Info.Name + " vừa cường hóa thành công " + itemTemplate.name + " trở thành trang bị HienNhan");
			}
		}
	}
}
