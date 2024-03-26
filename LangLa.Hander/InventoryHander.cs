using System;
using System.Collections.Generic;
using System.Linq;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class InventoryHander
	{
		public static bool UpdateVang(Character _myChar, int Vang, bool IsThongBao = false)
		{
			if (_myChar.Inventory.Vang < Vang)
			{
				return false;
			}
			_myChar.Inventory.Vang -= Vang;
			if (_myChar.Inventory.Vang < 0)
			{
				_myChar.Inventory.Vang = 0;
			}
			_myChar.TimeChar.SoVangTieuTrongNgay += Vang;
			_myChar.TimeChar.SoVangTieuTrongTuan += Vang;
			if (IsThongBao)
			{
				_myChar.SendMessage(MsgUpdateVang(_myChar.Inventory.Vang, ThongBao: true));
			}
			return true;
		}

		public static bool UpdateBac(Character _myChar, int Bac, bool ThongBao = false)
		{
			if (_myChar.Inventory.Bac < Bac)
			{
				return false;
			}
			_myChar.Inventory.Bac -= Bac;
			if (_myChar.Inventory.Bac < 0)
			{
				_myChar.Inventory.Bac = 0;
			}
			if (ThongBao)
			{
				_myChar.SendMessage(MsgUpdateBac(_myChar.Inventory.Bac, ThongBao));
			}
			return true;
		}

		public static bool UpdateBackhoa(Character _myChar, int BacKhoa, bool ThongBao = false)
		{
			if (_myChar.Inventory.BacKhoa < BacKhoa)
			{
				return false;
			}
			_myChar.Inventory.BacKhoa -= BacKhoa;
			if (_myChar.Inventory.BacKhoa < 0)
			{
				_myChar.Inventory.BacKhoa = 0;
			}
			if (ThongBao)
			{
				_myChar.SendMessage(MsgUpdateBacKhoa(_myChar.Inventory.BacKhoa, ThongBao));
			}
			return true;
		}

		public static bool UpdateVangKhoa(Character _myChar, int VangKhoa, bool IsThongBao = false)
		{
			if (_myChar.Inventory.VangKhoa < VangKhoa)
			{
				return false;
			}
			_myChar.Inventory.VangKhoa -= VangKhoa;
			if (_myChar.Inventory.VangKhoa < 0)
			{
				_myChar.Inventory.VangKhoa = 0;
			}
			if (IsThongBao)
			{
				_myChar.SendMessage(MsgUpdateVangKhoa(_myChar.Inventory.VangKhoa, IsThongBao));
			}
			return true;
		}

		public static bool UpdateVangBox(Character _myChar, int Vang)
		{
			if (_myChar.Inventory.VangBox < Vang)
			{
				return false;
			}
			_myChar.Inventory.VangBox -= Vang;
			if (_myChar.Inventory.VangBox < 0)
			{
				_myChar.Inventory.VangBox = 0;
			}
			return true;
		}

		public static bool UpdateBacBox(Character _myChar, int Bac)
		{
			if (_myChar.Inventory.BacBox < Bac)
			{
				return false;
			}
			_myChar.Inventory.BacBox -= Bac;
			if (_myChar.Inventory.BacBox < 0)
			{
				_myChar.Inventory.BacBox = 0;
			}
			return true;
		}

		public static bool UpdateBackhoaBox(Character _myChar, int BacKhoa)
		{
			if (_myChar.Inventory.BacKhoaBox < BacKhoa)
			{
				return false;
			}
			_myChar.Inventory.BacKhoaBox -= BacKhoa;
			if (_myChar.Inventory.BacKhoaBox < 0)
			{
				_myChar.Inventory.BacKhoaBox = 0;
			}
			return true;
		}

		public static bool UpdateVangKhoaBox(Character _myChar, int VangKhoa)
		{
			if (_myChar.Inventory.VangKhoaBox < VangKhoa)
			{
				return false;
			}
			_myChar.Inventory.VangKhoaBox -= VangKhoa;
			if (_myChar.Inventory.VangKhoaBox < 0)
			{
				_myChar.Inventory.VangKhoaBox = 0;
			}
			return true;
		}

		public static void AddBac(Character _myChar, int Bac, bool ThongBao = false)
		{
			int BacAdd = ((_myChar.Inventory.Bac + Bac < 0) ? int.MaxValue : (_myChar.Inventory.Bac + Bac));
			_myChar.Inventory.Bac = BacAdd;
			if (_myChar.Inventory.Bac >= int.MaxValue)
			{
				_myChar.Inventory.Bac = int.MaxValue;
			}
			_myChar.SendMessage(MsgUpdateBac(_myChar.Inventory.Bac, ThongBao));
		}

		public static void AddBacKhoa(Character _myChar, int BacKhoa, bool ThongBao = false)
		{
			int BacKhoaAdd = ((_myChar.Inventory.BacKhoa + BacKhoa < 0) ? int.MaxValue : (_myChar.Inventory.BacKhoa + BacKhoa));
			_myChar.Inventory.BacKhoa = BacKhoaAdd;
			if (_myChar.Inventory.BacKhoa >= int.MaxValue)
			{
				_myChar.Inventory.BacKhoa = int.MaxValue;
			}
			_myChar.SendMessage(MsgUpdateBacKhoa(_myChar.Inventory.BacKhoa, ThongBao));
		}

		public static void AddVang(Character _myChar, int Vang, bool ThongBao = false)
		{
			int VangAdd = ((_myChar.Inventory.Vang + Vang < 0) ? int.MaxValue : (_myChar.Inventory.Vang + Vang));
			_myChar.Inventory.Vang = VangAdd;
			if (_myChar.Inventory.Vang >= int.MaxValue)
			{
				_myChar.Inventory.Vang = int.MaxValue;
			}
			_myChar.SendMessage(MsgUpdateVang(_myChar.Inventory.Vang, ThongBao));
		}

		public static void AddVangKhoa(Character _myChar, int VangKhoa, bool ThongBao = false)
		{
			int VangKhoaAdd = ((_myChar.Inventory.VangKhoa + VangKhoa < 0) ? int.MaxValue : (_myChar.Inventory.VangKhoa + VangKhoa));
			_myChar.Inventory.VangKhoa = VangKhoaAdd;
			if (_myChar.Inventory.VangKhoa >= int.MaxValue)
			{
				_myChar.Inventory.VangKhoa = int.MaxValue;
			}
			_myChar.SendMessage(MsgUpdateVangKhoa(_myChar.Inventory.VangKhoa, ThongBao));
		}

		public static void AddBacBox(Character _myChar, int Bac, bool ThongBao = false)
		{
			int BacAdd = ((_myChar.Inventory.BacBox + Bac < 0) ? int.MaxValue : (_myChar.Inventory.BacBox + Bac));
			_myChar.Inventory.BacBox = BacAdd;
			if (_myChar.Inventory.BacBox >= int.MaxValue)
			{
				_myChar.Inventory.BacBox = int.MaxValue;
			}
		}

		public static void AddBacKhoaBox(Character _myChar, int BacKhoa, bool ThongBao = false)
		{
			int BacKhoaAdd = ((_myChar.Inventory.BacKhoaBox + BacKhoa < 0) ? int.MaxValue : (_myChar.Inventory.BacKhoaBox + BacKhoa));
			_myChar.Inventory.BacKhoaBox = BacKhoaAdd;
			if (_myChar.Inventory.BacKhoaBox >= int.MaxValue)
			{
				_myChar.Inventory.BacKhoaBox = int.MaxValue;
			}
		}

		public static void AddVangBox(Character _myChar, int Vang, bool ThongBao = false)
		{
			int VangAdd = ((_myChar.Inventory.VangBox + Vang < 0) ? int.MaxValue : (_myChar.Inventory.VangBox + Vang));
			_myChar.Inventory.VangBox = VangAdd;
			if (_myChar.Inventory.VangBox >= int.MaxValue)
			{
				_myChar.Inventory.VangBox = int.MaxValue;
			}
		}

		public static void AddVangKhoaBox(Character _myChar, int VangKhoa, bool ThongBao = false)
		{
			int VangKhoaAdd = ((_myChar.Inventory.VangKhoaBox + VangKhoa < 0) ? int.MaxValue : (_myChar.Inventory.VangKhoaBox + VangKhoa));
			_myChar.Inventory.VangKhoaBox = VangKhoaAdd;
			if (_myChar.Inventory.VangKhoaBox >= int.MaxValue)
			{
				_myChar.Inventory.VangKhoaBox = int.MaxValue;
			}
		}

		private static Message MsgUpdateVang(int Vang, bool ThongBao = false)
		{
			Message i = new Message(92);
			i.WriteInt((Vang >= int.MaxValue) ? int.MaxValue : Vang);
			i.WriteBool(ThongBao);
			return i;
		}

		private static Message MsgUpdateVangKhoa(int VangKhoa, bool ThongBao = false)
		{
			Message i = new Message(93);
			i.WriteInt((VangKhoa >= int.MaxValue) ? int.MaxValue : VangKhoa);
			i.WriteBool(ThongBao);
			return i;
		}

		private static Message MsgUpdateBac(int Bac, bool ThongBao = false)
		{
			Message i = new Message(90);
			i.WriteInt((Bac >= int.MaxValue) ? int.MaxValue : Bac);
			i.WriteBool(ThongBao);
			return i;
		}

		private static Message MsgUpdateBacKhoa(int BacKhoa, bool ThongBao = false)
		{
			Message i = new Message(91);
			i.WriteInt((BacKhoa >= int.MaxValue) ? int.MaxValue : BacKhoa);
			i.WriteBool(ThongBao);
			return i;
		}

		public static bool AddItemBag(Character _myChar, Item it, bool SendThongBao = true)
		{
			Item it2 = it;
			short COuntBag = GetCountNotNullBag(_myChar);
			if (COuntBag > 0)
			{
				if (it2.IsCongDon)
				{
					if (_myChar.Inventory.ItemBag.Any((Item s) => s != null && s.Id == it2.Id))
					{
						Item itCongDon = _myChar.Inventory.ItemBag.FirstOrDefault((Item s) => s != null && s.Id == it2.Id);
						itCongDon.Quantity += it2.Quantity;
						it2.Quantity = itCongDon.Quantity;
						it2.Index = itCongDon.Index;
						if (SendThongBao)
						{
							_myChar.SendMessage(UtilMessage.MsgAddItembag(itCongDon));
						}
						return true;
					}
					it2.Index = (short)_myChar.Inventory.ItemBag.FindIndex((Item s) => s == null);
					_myChar.Inventory.ItemBag[it2.Index] = it2;
				}
				else
				{
					it2.Index = (short)_myChar.Inventory.ItemBag.FindIndex((Item s) => s == null);
					_myChar.Inventory.ItemBag[it2.Index] = it2;
				}
				if (SendThongBao)
				{
					_myChar.SendMessage(UtilMessage.MsgAddItembag(it2));
				}
				return true;
			}
			return false;
		}

		public static void AddMultiItembag(Character _myChar, Item[] it2)
		{
			Item[] it3 = it2;
			if (GetCountNotNullBag(_myChar) <= it3.Length)
			{
				return;
			}
			int i;
			for (i = 0; i < it3.Length; i++)
			{
				if (it3[i].IsCongDon)
				{
					if (!_myChar.Inventory.ItemBag.Any((Item s) => s != null && s.Id == it3[i].Id))
					{
						it3[i].Index = (short)_myChar.Inventory.ItemBag.FindIndex((Item s) => s == null);
						_myChar.Inventory.ItemBag[it3[i].Index] = it3[i];
						continue;
					}
					Item itCongDon = _myChar.Inventory.ItemBag.FirstOrDefault((Item s) => s != null && s.Id == it3[i].Id);
					itCongDon.Quantity += it3[i].Quantity;
					it3[i].Quantity = itCongDon.Quantity;
					it3[i].Index = itCongDon.Index;
				}
				else
				{
					it3[i].Index = (short)_myChar.Inventory.ItemBag.FindIndex((Item s) => s == null);
					_myChar.Inventory.ItemBag[it3[i].Index] = it3[i];
				}
			}
			ShopHander.MsgBuyShopMuilti(_myChar, it3);
		}

		public static short GetCountNotNullBag(Character _myChar)
		{
			return (short)_myChar.Inventory.ItemBag.Count((Item s) => s == null);
		}

		public static short GetCountNotNUllBox(Character _myChar)
		{
			return (short)_myChar.Inventory.ItemBox.Count((Item s) => s == null);
		}

		public static void RemoveItemBag(Character _myChar, short Index, short[] MuiltiIndex = null)
		{
			if (MuiltiIndex != null)
			{
				foreach (short c in MuiltiIndex)
				{
					_myChar.Inventory.ItemBag[c].Quantity--;
					if (_myChar.Inventory.ItemBag[c].Quantity <= 0)
					{
						_myChar.SendMessage(UtilMessage.SetNullItembag(c));
						_myChar.Inventory.ItemBag[c] = null;
					}
				}
			}
			else
			{
				_myChar.Inventory.ItemBag[Index].Quantity--;
				if (_myChar.Inventory.ItemBag[Index].Quantity <= 0)
				{
					_myChar.SendMessage(UtilMessage.SetNullItembag(Index));
					_myChar.Inventory.ItemBag[Index] = null;
				}
			}
		}

		public static void SetNullItemBag(Character _myChar, short id)
		{
			if (_myChar.Inventory.ItemBag.Any((Item s) => s != null && s.Id == id))
			{
				short Index = (short)_myChar.Inventory.ItemBag.FindIndex((Item s) => s != null && s.Id == id);
				if (Index != -1)
				{
					RemoveItemBag(_myChar, Index);
					_myChar.SendMessage(UtilMessage.SetNullItembag(Index));
				}
			}
		}

		public static void SortItem(List<Item> ItemChar)
		{
			List<Item> items = new List<Item>();
			for (int i = 0; i < ItemChar.Count; i++)
			{
				if (ItemChar[i] != null)
				{
					items.Add(ItemChar[i]);
				}
				ItemChar[i] = null;
			}
			for (int var4 = 0; var4 < items.Count; var4++)
			{
				Item var6;
				if (!(var6 = items[var4]).IsCongDon)
				{
					continue;
				}
				int Quantity = 0;
				for (int var5 = items.Count - 1; var5 > var4; var5--)
				{
					Item var3;
					if ((var3 = items[var5]).Id == var6.Id && var3.IsLock == var6.IsLock && var3.HSD == var6.HSD)
					{
						Quantity += var3.Quantity;
						if (Quantity > 1000000000)
						{
							Quantity = (var6.Quantity = 1000000000);
						}
						else
						{
							var6.Quantity += Quantity;
						}
						items.RemoveAt(var5);
					}
				}
			}
			short var7 = 0;
			while (var7 < items.Count)
			{
				ItemChar[var7] = items[var7];
				ItemChar[var7].Index = var7++;
			}
		}

		public static void SortItem(Character _myChar, Message msg)
		{
			sbyte TypeSort = msg.ReadByte();
			if (TypeSort == 0)
			{
				lock (_myChar.Inventory.ItemBag)
				{
					SortItem(_myChar.Inventory.ItemBag);
				}
				_myChar.SendMessage(UtilMessage.MsgSortItem(TypeSort));
			}
			else
			{
				lock (_myChar.Inventory.ItemBox)
				{
					SortItem(_myChar.Inventory.ItemBox);
				}
				_myChar.SendMessage(UtilMessage.MsgSortItem(TypeSort));
			}
		}

		public static void TachItem(Character _myChar, Message msg)
		{
			short Index = msg.ReadShort();
			short SizeTach = msg.ReadShort();
			Item it = _myChar.Inventory.ItemBag[Index];
			if (it != null && it.Quantity > SizeTach && GetCountNotNullBag(_myChar) > 0)
			{
				it.Quantity -= SizeTach;
				Item itTach = ItemHander.CoppyItem(it, SizeTach);
				itTach.Index = (short)_myChar.Inventory.ItemBag.FindIndex((Item s) => s == null);
				_myChar.Inventory.ItemBag[itTach.Index] = itTach;
				_myChar.SendMessage(UtilMessage.MsgTachItem(Index, it.Quantity, itTach.Index, SizeTach));
			}
		}

		public static void MoneyBoxToBag(Character _myChar, Message msg)
		{
			sbyte Type = msg.ReadByte();
			int Money = msg.ReadInt();
			switch (Type)
			{
			case 0:
				if (Money > _myChar.Inventory.BacBox)
				{
					return;
				}
				UpdateBacBox(_myChar, Money);
				AddBac(_myChar, Money, ThongBao: true);
				break;
			case 1:
				if (Money > _myChar.Inventory.BacKhoaBox)
				{
					return;
				}
				UpdateBackhoaBox(_myChar, Money);
				AddBacKhoa(_myChar, Money, ThongBao: true);
				break;
			case 2:
				if (Money > _myChar.Inventory.VangBox)
				{
					return;
				}
				UpdateVangBox(_myChar, Money);
				AddVang(_myChar, Money, ThongBao: true);
				break;
			case 3:
				if (Money > _myChar.Inventory.VangKhoaBox)
				{
					return;
				}
				UpdateVangKhoaBox(_myChar, Money);
				AddVangKhoa(_myChar, Money, ThongBao: true);
				break;
			}
			WriteItemBox(_myChar);
		}

		public static void MoneyBagToBox(Character _myChar, Message msg)
		{
			sbyte Type = msg.ReadByte();
			int Money = msg.ReadInt();
			switch (Type)
			{
			case 0:
				if (Money > _myChar.Inventory.Bac)
				{
					return;
				}
				UpdateBac(_myChar, Money, ThongBao: true);
				AddBacBox(_myChar, Money);
				break;
			case 1:
				if (Money > _myChar.Inventory.BacKhoa)
				{
					return;
				}
				UpdateBackhoa(_myChar, Money, ThongBao: true);
				AddBacKhoaBox(_myChar, Money);
				break;
			case 2:
				if (Money > _myChar.Inventory.Vang)
				{
					return;
				}
				UpdateVang(_myChar, Money, IsThongBao: true);
				AddVangBox(_myChar, Money);
				break;
			case 3:
				if (Money > _myChar.Inventory.VangKhoa)
				{
					return;
				}
				UpdateVangKhoa(_myChar, Money, IsThongBao: true);
				AddVangKhoaBox(_myChar, Money);
				break;
			}
			WriteItemBox(_myChar);
		}

		private static bool AddItemToBox(Character _myChar, Item it)
		{
			Item it2 = it;
			short CountBox = GetCountNotNUllBox(_myChar);
			if (CountBox > 0)
			{
				if (it2.IsCongDon)
				{
					sbyte Index = (sbyte)_myChar.Inventory.ItemBox.FindIndex((Item s) => s != null && s.Id == it2.Id);
					if (Index == -1)
					{
						it2.Index = (short)_myChar.Inventory.ItemBox.FindIndex((Item s) => s == null);
						_myChar.Inventory.ItemBox[it2.Index] = it2;
					}
					else
					{
						Item itCongDon = _myChar.Inventory.ItemBox.FirstOrDefault((Item s) => s != null && s.Id == it2.Id);
						itCongDon.Quantity += it2.Quantity;
						it2.Quantity = itCongDon.Quantity;
						it2.Index = itCongDon.Index;
					}
					return true;
				}
				it2.Index = (short)_myChar.Inventory.ItemBox.FindIndex((Item s) => s == null);
				_myChar.Inventory.ItemBox[it2.Index] = it2;
				return true;
			}
			return false;
		}

		public static void MoRongBox(Character _myChar)
		{
			lock (_myChar.Inventory.ItemBox)
			{
				if (_myChar.Inventory.Vang >= 90)
				{
					if (_myChar.Inventory.ItemBox.Count + 9 > 127)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Rương đồ đã đã giới hạn", Util.WHITE));
						return;
					}
					UpdateVang(_myChar, 90, IsThongBao: true);
					_myChar.Inventory.ItemBox.AddRange(new List<Item>(new Item[9]));
					WriteItemBox(_myChar);
				}
			}
		}

		public static void ItemBagToBox(Character _myChar, Message msg)
		{
			short IndexBag = msg.ReadShort();
			if (IndexBag >= _myChar.Inventory.ItemBag.Count)
			{
				return;
			}
			Item itemToBox = _myChar.Inventory.ItemBag[IndexBag];
			if (itemToBox == null)
			{
				return;
			}
			lock (_myChar.Inventory.ItemBox)
			{
				if (AddItemToBox(_myChar, itemToBox))
				{
					_myChar.Inventory.ItemBag[IndexBag] = null;
					_myChar.SendMessage(MsgItemBagToBox(IndexBag, itemToBox.Index));
				}
			}
		}

		public static int FindQuantityAll(Character _myChar, short Id)
		{
			Item[] items = _myChar.Inventory.ItemBag.Where((Item s) => s != null && s.Id == Id).ToArray();
			if (items == null || items.Length == 0)
			{
				return 0;
			}
			int Quantity = 0;
			Item[] array = items;
			foreach (Item c in array)
			{
				Quantity += c.Quantity;
			}
			return Quantity;
		}

		public static void RemoveQuantityWhereId(Character _myChar, int Quantity, short Id)
		{
			int QuantityRemove = Quantity;
			for (int i = 0; i < _myChar.Inventory.ItemBag.Count; i++)
			{
				if (_myChar.Inventory.ItemBag[i] != null && _myChar.Inventory.ItemBag[i].Id == Id)
				{
					int QuantityBag = _myChar.Inventory.ItemBag[i].Quantity;
					_myChar.Inventory.ItemBag[i].Quantity -= QuantityRemove;
					if (_myChar.Inventory.ItemBag[i].Quantity <= 0)
					{
						_myChar.SendMessage(UtilMessage.SetNullItembag(_myChar.Inventory.ItemBag[i].Index));
						_myChar.Inventory.ItemBag[i] = null;
					}
					else
					{
						_myChar.SendMessage(UtilMessage.MsgUpdateItemBag(_myChar.Inventory.ItemBag[i]));
					}
					QuantityRemove -= QuantityBag;
					if (QuantityRemove <= 0)
					{
						break;
					}
				}
			}
		}

		private static Message MsgItemBoxTobag(short Index1, short Index2)
		{
			Message i = new Message(114);
			i.WriteShort(Index1);
			i.WriteShort(Index2);
			return i;
		}

		private static Message MsgItemBagToBox(short Index1, short Index2)
		{
			Message i = new Message(115);
			i.WriteShort(Index1);
			i.WriteShort(Index2);
			return i;
		}

		public static void ItemBoxToBag(Character _myChar, Message msg)
		{
			short Index = msg.ReadShort();
			Item itemToBag = _myChar.Inventory.ItemBox[Index];
			if (itemToBag == null)
			{
				return;
			}
			lock (_myChar.Inventory.ItemBag)
			{
				if (AddItemBag(_myChar, itemToBag, SendThongBao: false))
				{
					_myChar.Inventory.ItemBox[Index] = null;
					_myChar.SendMessage(MsgItemBoxTobag(Index, itemToBag.Index));
				}
			}
		}

		public static void ItemMoRongToBag(Character _myChar, Message msg)
		{
			sbyte Index2 = msg.ReadByte();
			Item itemMoRong = _myChar.Inventory.ItemMoRongTui[Index2];
			if (itemMoRong == null)
			{
				return;
			}
			short SizeDown = -1;
			switch (itemMoRong.Id)
			{
			case 185:
				SizeDown = 9;
				break;
			case 186:
				SizeDown = 18;
				break;
			case 187:
				SizeDown = 27;
				break;
			case 468:
				SizeDown = 36;
				break;
			}
			if (GetCountNotNullBag(_myChar) > SizeDown)
			{
				lock (_myChar.Inventory.ItemBag)
				{
					SortItem(_myChar.Inventory.ItemBag);
				}
				_myChar.SendMessage(UtilMessage.MsgSortItem(0));
				short SizeNew = (short)(_myChar.Inventory.ItemBag.Count - SizeDown);
				short SizeBag = (short)_myChar.Inventory.ItemBag.Count;
				for (int i = 0; i != SizeDown; i++)
				{
					_myChar.Inventory.ItemBag.RemoveAt(_myChar.Inventory.ItemBag.Count - 1);
				}
				_myChar.Inventory.ItemMoRongTui[Index2] = null;
				AddItemBag(_myChar, itemMoRong, SendThongBao: false);
				Message j = new Message(112);
				j.WriteByte(Index2);
				j.WriteShort((short)_myChar.Inventory.ItemBag.Count);
				_myChar.SendMessage(j);
			}
			else
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Cần trống ít nhất " + SizeDown + " ô hành trang", Util.WHITE));
			}
		}

		public static void WriteItemBox(Character _myChar)
		{
			Message i = new Message(122);
			i.WriteByte(50);
			i.WriteShort((short)_myChar.Inventory.ItemBox.Count);
			Item[] itemsBox = _myChar.Inventory.ItemBox.Where((Item s) => s != null).ToArray();
			i.WriteShort((short)itemsBox.Length);
			Item[] array = itemsBox;
			foreach (Item it in array)
			{
				ItemHander.WriteItem(i, it);
			}
			i.WriteBool(_myChar.Inventory.IsShowBox);
			i.WriteInt(_myChar.Inventory.BacBox);
			i.WriteInt(_myChar.Inventory.BacKhoaBox);
			i.WriteInt(_myChar.Inventory.VangBox);
			i.WriteInt(_myChar.Inventory.VangKhoaBox);
			_myChar.SendMessage(i);
		}

		public static void CreateGiaoDich(Character _myChar, Message msg)
		{
			string cGiaoDich = msg.ReadString();
			if (!_myChar.Info.IsActive)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Cần kích hoạt tài khoản", Util.WHITE));
				return;
			}
			Character _cGiaoDich = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(cGiaoDich));
			if (_cGiaoDich != null)
			{
				if (!_cGiaoDich.Info.IsActive)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Đối phương chưa kích hoạt tài khoản", Util.WHITE));
					return;
				}
				Message i = new Message(86);
				i.WriteUTF(_myChar.Info.Name);
				_cGiaoDich.SendMessage(i);
				_myChar.SendMessage(UtilMessage.SendThongBao("Đã gửi lời mời giao dịch tới " + cGiaoDich, Util.WHITE));
				_cGiaoDich.InfoGame.IdCharGiaoDich = _myChar.Info.IdUser;
				_myChar.InfoGame.IdCharGiaoDich = _cGiaoDich.Info.IdUser;
			}
		}

		public static void StartGiaoDich(Character _myChar, Message msg)
		{
			Character _myChar2 = _myChar;
			Character _cGiaoDich = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == _myChar2.InfoGame.IdCharGiaoDich);
			if (_cGiaoDich != null)
			{
				Message i = new Message(122);
				i.WriteByte(59);
				i.WriteUTF(_cGiaoDich.Info.Name);
				_myChar2.SendMessage(i);
				_myChar2.InfoGame.StatusGD = 1;
				i = new Message(122);
				i.WriteByte(59);
				i.WriteUTF(_myChar2.Info.Name);
				_cGiaoDich.SendMessage(i);
				_cGiaoDich.InfoGame.StatusGD = 1;
				short Speed = _cGiaoDich.TuongKhac.GetSpeedChar(_cGiaoDich);
				short Speed2 = _myChar2.TuongKhac.GetSpeedChar(_myChar2);
				{
					foreach (Character c in _myChar2.InfoGame.ZoneGame.Chars.Values)
					{
						if (c.IsConnection)
						{
							c.SendMessage(UtilMessage.UpdatePointMore(_cGiaoDich.Info.IdUser, 0, _cGiaoDich.Info.TaiPhu, Speed, 1));
							c.SendMessage(UtilMessage.UpdatePointMore(_myChar2.Info.IdUser, 0, _myChar2.Info.TaiPhu, Speed2, 1));
						}
					}
					return;
				}
			}
			Util.ShowLog("NULL C");
		}

		public static void HuyGiaoDich(Character _myChar)
		{
			Character _myChar2 = _myChar;
			if (_myChar2.InfoGame.StatusGD != 1)
			{
				return;
			}
			Character _cGiaoDich = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == _myChar2.InfoGame.IdCharGiaoDich);
			short Speed2 = _myChar2.TuongKhac.GetSpeedChar(_cGiaoDich);
			if (_cGiaoDich != null && _cGiaoDich.InfoGame.StatusGD == 1)
			{
				_cGiaoDich.SendMessage(MsgClearGD());
				_cGiaoDich.SendMessage(UtilMessage.SendThongBao(_myChar2.Info.Name + " đã hủy giao dịch", Util.YELLOW_MID));
				_cGiaoDich.InfoGame.CleanUpGD(null);
				short Speed = _cGiaoDich.TuongKhac.GetSpeedChar(_cGiaoDich);
				foreach (Character c2 in _cGiaoDich.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.UpdatePointMore(_cGiaoDich.Info.IdUser, 0, _cGiaoDich.Info.TaiPhu, Speed2, 0));
					}
				}
			}
			foreach (Character c in _myChar2.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.UpdatePointMore(_myChar2.Info.IdUser, 0, _myChar2.Info.TaiPhu, Speed2, 0));
				}
			}
			_myChar2.InfoGame.CleanUpGD(null);
		}

		public static void LockGiaoDich(Character _myChar, Message msg)
		{
			Character _myChar2 = _myChar;
			Character _cGiaoDich = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == _myChar2.InfoGame.IdCharGiaoDich);
			if (_myChar2.InfoGame.StatusGD == 1 && _cGiaoDich != null && _cGiaoDich.InfoGame.StatusGD == 1)
			{
				_myChar2.InfoGame.IsLockGD = true;
				if (_cGiaoDich.InfoGame.IsLockGD)
				{
					try
					{
						if (_cGiaoDich.InfoGame.BacGiaoDich > 0)
						{
							UpdateBac(_cGiaoDich, _cGiaoDich.InfoGame.BacGiaoDich);
							AddBac(_myChar2, _cGiaoDich.InfoGame.BacGiaoDich);
						}
						if (_myChar2.InfoGame.BacGiaoDich > 0)
						{
							UpdateBac(_myChar2, _myChar2.InfoGame.BacGiaoDich);
							AddBac(_cGiaoDich, _myChar2.InfoGame.BacGiaoDich);
						}
						Item[] itemGD = _cGiaoDich.InfoGame.ItemGD;
						foreach (Item j in itemGD)
						{
							_cGiaoDich.Inventory.ItemBag[j.Index] = null;
							_cGiaoDich.SendMessage(UtilMessage.SetNullItembag(j.Index));
							AddItemBag(_myChar2, j);
						}
						Item[] itemGD2 = _myChar2.InfoGame.ItemGD;
						foreach (Item i in itemGD2)
						{
							_myChar2.Inventory.ItemBag[i.Index] = null;
							_myChar2.SendMessage(UtilMessage.SetNullItembag(i.Index));
							AddItemBag(_cGiaoDich, i);
						}
						_cGiaoDich.SendMessage(MsgClearGD());
						_myChar2.SendMessage(MsgClearGD());
						_cGiaoDich.SendMessage(UtilMessage.SendThongBao("Giao dịch thành công", Util.YELLOW_MID));
						_myChar2.SendMessage(UtilMessage.SendThongBao("Giao dịch thành công", Util.YELLOW_MID));
						_myChar2.InfoGame.CleanUpGD(_cGiaoDich);
						return;
					}
					catch (Exception e)
					{
						Util.ShowErr(e);
						return;
					}
				}
				_myChar2.SendMessage(UtilMessage.SendThongBao("Xin chờ " + _cGiaoDich.Info.Name + " đồng ý", Util.WHITE));
			}
			else
			{
				Util.ShowLog("NULL C");
			}
		}

		public static Message MsgClearGD()
		{
			Message i = UtilMessage.Message123();
			i.WriteByte(-43);
			return i;
		}

		public static void ShowListItemGiaoDich(Character _myChar, Message msg)
		{
			Character _myChar2 = _myChar;
			Character _cGiaoDich = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == _myChar2.InfoGame.IdCharGiaoDich);
			if (_cGiaoDich == null || _myChar2.InfoGame.StatusGD != 1 || _cGiaoDich.InfoGame.StatusGD != 1)
			{
				return;
			}
			_myChar2.InfoGame.BacGiaoDich = msg.ReadInt();
			if (_myChar2.Inventory.Bac < _myChar2.InfoGame.BacGiaoDich)
			{
				return;
			}
			sbyte Size = msg.ReadByte();
			if (GetCountNotNullBag(_cGiaoDich) < Size)
			{
				_myChar2.SendMessage(UtilMessage.SendThongBao("Hành trang của " + _cGiaoDich.Info.Name + " không đủ ô trống", Util.WHITE));
				return;
			}
			_myChar2.InfoGame.ItemGD = new Item[Size];
			for (int j = 0; j < Size; j++)
			{
				_myChar2.InfoGame.ItemGD[j] = _myChar2.Inventory.ItemBag[msg.ReadShort()];
			}
			Message k = new Message(82);
			k.WriteInt(_myChar2.InfoGame.BacGiaoDich);
			k.WriteByte(Size);
			Item[] itemGD = _myChar2.InfoGame.ItemGD;
			foreach (Item i in itemGD)
			{
				ItemHander.WriteItem(k, i);
			}
			_cGiaoDich.SendMessage(k);
		}
	}
}
