using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.Hander
{
	public static class ItemHander
	{
		public static Item CoppyItem(Item item, int Quantity = 1)
		{
			Item it = new Item(item.Id, item.IsLock);
			it.Quantity = Quantity;
			it.IdClass = item.IdClass;
			it.IsCongDon = item.IsCongDon;
			it.HSD = item.HSD;
			it.Index = item.Index;
			it.Level = item.Level;
			it.Options = item.Options;
			return it;
		}

		public static void WriteItemBody(Message msg, Item it)
		{
			if (it.Id > 0)
			{
				msg.WriteShort(it.Id);
				msg.WriteBool(it.IsLock);
				msg.WriteLong(it.HSD);
				msg.WriteByte(it.IdClass);
				msg.WriteByte(it.Level);
				msg.WriteUTF(it.Options);
			}
		}

		public static void WriteItem(Message msg, Item it)
		{
			msg.WriteShort(it.Id);
			if (it.Id >= 0)
			{
				msg.WriteBool(it.IsLock);
				msg.WriteLong(it.HSD);
				if (it.Type <= 16)
				{
					msg.WriteByte(it.IdClass);
					msg.WriteByte(it.Level);
					msg.WriteUTF(it.Options);
				}
				else
				{
					msg.WriteInt(it.Quantity);
				}
				if (it.Type == 99)
				{
					msg.WriteUTF(it.Options);
				}
				msg.WriteShort(it.Index);
			}
		}

		public static void MsgUseItemBag(Character _myChar, Item it)
		{
			Message i = new Message(116);
			i.WriteShort(it.Index);
			i.WriteBool(it.IsLock);
			if (it.Type == 28)
			{
				byte Num = 0;
				switch (it.Id)
				{
				case 185:
					Num = 9;
					break;
				case 186:
					Num = 18;
					break;
				case 187:
					Num = 27;
					break;
				case 468:
					Num = 36;
					break;
				}
				_myChar.Inventory.ItemBag.AddRange(new List<Item>(new Item[Num]));
				i.WriteShort((short)_myChar.Inventory.ItemBag.Count);
			}
			else
			{
				int Quantity = it.Quantity;
				if (Quantity > 1)
				{
					Quantity = it.Quantity - 1;
				}
				i.WriteInt(Quantity);
			}
			_myChar.SendMessage(i);
		}

		public static void UseItem(Character _myChar, Message msg)
		{
			short Index = msg.ReadShort();
			if (Index < 0 || Index >= _myChar.Inventory.ItemBag.Count || _myChar.Inventory.ItemBag[Index] == null)
			{
				return;
			}
			Item itUse = _myChar.Inventory.ItemBag[Index];
			ItemTemplate itemTemplate = DataServer.ArrItemTemplate[itUse.Id];
			if (itemTemplate.Type != 34 && (itemTemplate.LevelNeed > _myChar.Info.Level || (itemTemplate.GioiTinh != 2 && itemTemplate.GioiTinh != _myChar.Info.GioiTinh) || (itemTemplate.IdClass != 0 && itemTemplate.IdClass != _myChar.Info.IdClass) || (itemTemplate.Type == 12 && _myChar.Info.TaiPhu < itemTemplate.TaiPhuNeed)))
			{
				return;
			}
			Item it = null;
			bool IsItemMore = false;
			if (itUse != null)
			{
				if (itUse.Type <= 16)
				{
					if (!itUse.IsLock)
					{
						itUse.IsLock = true;
					}
					MsgUseItemBag(_myChar, itUse);
					itUse.IsLock = true;
					itUse.Index = itemTemplate.Type;
					it = _myChar.Inventory.ItemBody[itUse.Index];
					_myChar.TuongKhac.DownPointKichAn(_myChar, itUse);
					_myChar.Inventory.ItemBody[itUse.Index] = itUse;
					_myChar.TuongKhac.UpPointKichAn(_myChar, itUse);
					_myChar.TuongKhac.GetPointFromItem(itUse, _myChar);
					foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
					{
						if (c.IsConnection && c.Info.IdUser != _myChar.Info.IdUser)
						{
							c.SendMessage(UtilMessage.UpdateBodyChar(_myChar));
						}
					}
				}
				else
				{
					IsItemMore = true;
					switch (itUse.Type)
					{
					case 22:
					case 23:
						if (!_myChar.InfoGame.IsDinhThanhSatChakra && !_myChar.InfoGame.IsChoang)
						{
							EffHander.AddEff(_myChar, itUse.Id, DataServer.ArrItemTemplate[itUse.Id].Type);
							MsgUseItemBag(_myChar, itUse);
							InventoryHander.RemoveItemBag(_myChar, Index);
						}
						break;
					case 24:
						EffHander.AddEff(_myChar, itUse.Id, DataServer.ArrItemTemplate[itUse.Id].Type);
						MsgUseItemBag(_myChar, itUse);
						InventoryHander.RemoveItemBag(_myChar, Index);
						break;
					case 33:
						UseItemRank(_myChar, itUse, Index);
						break;
					case 28:
						if (_myChar.Inventory.ItemMoRongTui.Any((Item s) => s == null))
						{
							sbyte Index2 = (sbyte)_myChar.Inventory.ItemMoRongTui.ToList().FindIndex((Item s) => s == null);
							if (Index2 == -1)
							{
								Index2 = 0;
							}
							if (Index2 >= _myChar.Inventory.ItemMoRongTui.Length)
							{
								return;
							}
							MsgUseItemBag(_myChar, itUse);
							InventoryHander.RemoveItemBag(_myChar, Index);
							itUse.Index = Index2;
							_myChar.Inventory.ItemMoRongTui[Index2] = itUse;
						}
						else
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Đã đạt tối đa ô mở rộng túi", Util.WHITE));
						}
						break;
					case 27:
						_myChar.SendMessage(UtilMessage.OpenMenuNotNpc(itUse.Index, "Trường Konoha Gakuen;Làng;Khu rừng chết;Đại chiến nhẫn giả lần III;Đại hội nhẫn giả"));
						_myChar.InfoGame.IsUseItemNotNpc = true;
						_myChar.InfoGame.IsUseItemNotNpcOder2 = false;
						_myChar.InfoGame.IndexItemNotNpc = itUse.Index;
						break;
					case 29:
						UseItemType29(_myChar, itUse);
						break;
					case 30:
						UseSachKyNang(_myChar, itUse);
						break;
					case 34:
						UseDanhHieu(_myChar, itUse);
						break;
					case 100:
						UseItemType100(_myChar, itUse);
						break;
					default:
						_myChar.SendMessage(UtilMessage.SendThongBao("Vật phẩm này chưa thể sử dụng :D", Util.WHITE));
						break;
					}
				}
				if (it != null)
				{
					_myChar.TuongKhac.GetPointFromItem(it, _myChar, IsDownPoint: true);
					it.Index = Index;
					_myChar.Inventory.ItemBag[Index] = it;
					if (it.Type == 16)
					{
						InfoEff infoEff = _myChar.Effs.FirstOrDefault((InfoEff s) => s.Type == 91);
						if (infoEff != null)
						{
							EffHander.RemoveEff(_myChar, infoEff);
						}
					}
				}
				else if (!IsItemMore)
				{
					InventoryHander.RemoveItemBag(_myChar, Index);
				}
				TaskHander.CheckUseItem(_myChar, itUse);
			}
			else
			{
				Util.ShowLog("DONE NULL ITEM " + Index);
			}
		}

		private static void UseTeleport(Character _myChar)
		{
		}

		public static void ItemBodyTobag(Character _myChar, Message msg)
		{
			sbyte IndexBody = msg.ReadByte();
			Item it = _myChar.Inventory.ItemBody[IndexBody];
			if (it == null || !InventoryHander.AddItemBag(_myChar, it, SendThongBao: false))
			{
				return;
			}
			if (it.Type == 16)
			{
				InfoEff infoEff = _myChar.Effs.FirstOrDefault((InfoEff s) => s.Type == 91);
				if (infoEff != null)
				{
					EffHander.RemoveEff(_myChar, infoEff);
				}
			}
			_myChar.TuongKhac.DownPointKichAn(_myChar, it);
			_myChar.Inventory.ItemBody[IndexBody] = null;
			_myChar.TuongKhac.UpPointKichAn(_myChar, it);
			_myChar.SendMessage(UtilMessage.SendItemBodyTobag(IndexBody, (sbyte)it.Index));
			_myChar.TuongKhac.GetPointFromItem(it, _myChar, IsDownPoint: true);
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection && c.Info.IdUser != _myChar.Info.IdUser)
				{
					c.SendMessage(UtilMessage.UpdateBodyChar(_myChar));
				}
			}
			PointHander.UpdateHpFullChar(_myChar);
			PointHander.UpdateMpFullChar(_myChar);
		}

		private static void UseDanhHieu(Character _myChar, Item item)
		{
			Item item2 = item;
			if (_myChar.DanhHieus.Any((InfoDanhHieu s) => s.Id == item2.Id))
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã sỡ hữu danh hiệu này", Util.WHITE));
				return;
			}
			DanhHieuHander.AddDanhHieu(_myChar, item2);
			MsgUseItemBag(_myChar, item2);
			InventoryHander.RemoveItemBag(_myChar, item2.Index);
		}

		private static void UseItemRank(Character _myChar, Item item, short IndexRemove)
		{
			sbyte RankChar = _myChar.Info.Rank;
			bool IsOk = true;
			if (RankChar == 0 && item.Id != 417)
			{
				IsOk = false;
			}
			else if (RankChar == 1 && item.Id != 418)
			{
				IsOk = false;
			}
			else if (RankChar == 2 && item.Id != 419)
			{
				IsOk = false;
			}
			else if (RankChar == 3 && item.Id != 420)
			{
				IsOk = false;
			}
			else if (RankChar == 4 && item.Id != 421)
			{
				IsOk = false;
			}
			else if (RankChar == 5 && item.Id != 422)
			{
				IsOk = false;
			}
			else if (RankChar == 6 && item.Id != 423)
			{
				IsOk = false;
			}
			else if (RankChar == 7 && item.Id != 424)
			{
				IsOk = false;
			}
			else if (RankChar == 8 && item.Id != 425)
			{
				IsOk = false;
			}
			else if (RankChar == 9 && item.Id != 426)
			{
				IsOk = false;
			}
			if (!IsOk)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Hãy sử dụng rank theo từng bậc", Util.WHITE));
			}
			else
			{
				if (_myChar.Info.Rank >= 10)
				{
					return;
				}
				_myChar.Info.Rank++;
				switch (_myChar.Info.Rank)
				{
				case 1:
					InventoryHander.AddItemBag(_myChar, new Item(161, IsLock: true));
					break;
				case 2:
					InventoryHander.AddItemBag(_myChar, new Item(277, IsLock: true));
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 5;
					break;
				case 3:
					InventoryHander.AddItemBag(_myChar, new Item(266, IsLock: true, SetOptionAuto: false, 5));
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 10;
					break;
				case 4:
					InventoryHander.AddItemBag(_myChar, new Item(347, IsLock: true, SetOptionAuto: false, 5));
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 10;
					break;
				case 5:
					InventoryHander.AddItemBag(_myChar, new Item(7, IsLock: true));
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 20;
					break;
				case 6:
					InventoryHander.AddItemBag(_myChar, new Item(277, IsLock: true, SetOptionAuto: false, 15));
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 20;
					break;
				case 7:
					InventoryHander.AddItemBag(_myChar, new Item(161, IsLock: true, SetOptionAuto: false, 10));
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 30;
					break;
				case 8:
					InventoryHander.AddItemBag(_myChar, new Item(152, IsLock: true));
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 30;
					break;
				case 9:
					InventoryHander.AddItemBag(_myChar, new Item(155, IsLock: true));
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 40;
					break;
				case 10:
				{
					InventoryHander.AddItemBag(_myChar, new Item(467, IsLock: true));
					Item item2 = new Item(463, IsLock: true, SetOptionAuto: false, 1, "209,75");
					item2.IdClass = -1;
					InventoryHander.AddItemBag(_myChar, item2);
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += 40;
					break;
				}
				}
				MsgUseItemBag(_myChar, item);
				InventoryHander.RemoveItemBag(_myChar, IndexRemove);
				int IdChar = _myChar.Info.IdUser;
				sbyte Rank = _myChar.Info.Rank;
				foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
				{
					if (c.IsConnection)
					{
						c.SendMessage(UtilMessage.MsgUpdateRank(IdChar, Rank));
					}
				}
			}
		}

		public static void ItemMoRongTuiToBag(Character _myChar, Message msg)
		{
		}

		private static void UseItemType29(Character _myChar, Item item)
		{
			Item item2 = item;
			switch (item2.Id)
			{
			case 150:
			case 151:
			case 152:
				UseSachKyNang(_myChar, item2);
				break;
			case 159:
			case 281:
			case 347:
				if (_myChar.Effs.Any((InfoEff s) => s.Type == 23 && item2.Id != s.IdItem))
				{
					InfoEff infoEff = _myChar.Effs.FirstOrDefault((InfoEff s) => s != null && s.Type == 23);
					if (infoEff != null)
					{
						EffHander.RemoveEff(_myChar, infoEff);
					}
				}
				if (_myChar.Effs.Any((InfoEff s) => s.IdItem == item2.Id))
				{
					EffHander.AddTimeEff(_myChar, _myChar.Effs.FirstOrDefault((InfoEff s) => s.IdItem == item2.Id));
				}
				else
				{
					EffHander.AddEff(_myChar, item2.Id, DataServer.ArrItemTemplate[item2.Id].Type);
				}
				MsgUseItemBag(_myChar, item2);
				InventoryHander.RemoveItemBag(_myChar, item2.Index);
				break;
			}
		}

		private static void UseSachKyNang(Character _myChar, Item item)
		{
			sbyte ValuePointAdd = -1;
			bool IsCanAdd = false;
			sbyte Index = -1;
			switch (item.Id)
			{
			case 150:
			case 151:
			case 152:
			{
				Index = (ValuePointAdd = (sbyte)((item.Id == 150) ? 1 : ((item.Id == 151) ? 2 : 3)));
				Index--;
				sbyte[] PointAdSachKyNang = _myChar.Point.PointUseSachKyNang;
				if ((item.Id == 150 && PointAdSachKyNang[Index] < 3) || (item.Id == 151 && PointAdSachKyNang[Index] < 2) || (item.Id == 152 && PointAdSachKyNang[Index] < 1))
				{
					_myChar.Point.PointUseSachKyNang[Index]++;
					_myChar.Point.DiemKyNang += ValuePointAdd;
					MsgUseItemBag(_myChar, item);
					InventoryHander.RemoveItemBag(_myChar, item.Index);
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn nhận được " + ValuePointAdd + " điểm kỹ năng", Util.WHITE));
				}
				else
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Giới hạn sử dụng đã đạt tối đa", Util.WHITE));
				}
				break;
			}
			case 153:
			case 154:
			case 155:
			{
				Index = (ValuePointAdd = (sbyte)((item.Id == 153) ? 10 : ((item.Id == 154) ? 20 : 30)));
				Index /= 10;
				Index--;
				sbyte[] PointAdSachTiemNang = _myChar.Point.PointUseSachTiemNang;
				if ((item.Id == 153 && PointAdSachTiemNang[Index] < 3) || (item.Id == 154 && PointAdSachTiemNang[Index] < 2) || (item.Id == 155 && PointAdSachTiemNang[Index] < 1))
				{
					_myChar.Point.DiemTiemNang += ValuePointAdd;
					_myChar.Point.PointUseSachTiemNang[Index]++;
					MsgUseItemBag(_myChar, item);
					InventoryHander.RemoveItemBag(_myChar, item.Index);
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn nhận được " + ValuePointAdd + " điểm tiềm năng", Util.WHITE));
				}
				else
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Giới hạn sử dụng đã đạt tối đa", Util.WHITE));
				}
				break;
			}
			}
		}

		private static void UseItemType100(Character _myChar, Item item)
		{
			Character _myChar2 = _myChar;
			Item item3 = null;
			switch (item.Id)
			{
			case 161:
				item3 = new Item(160, IsLock: true);
				item3.Quantity = 100;
				if (InventoryHander.AddItemBag(_myChar2, item3))
				{
					MsgUseItemBag(_myChar2, item);
					InventoryHander.RemoveItemBag(_myChar2, item.Index);
				}
				break;
			case 163:
			{
				int QuantityAddBac = item.Quantity;
				if (_myChar2.Info.IsActive)
				{
					InventoryHander.AddBac(_myChar2, int.MaxValue, ThongBao: true);
					InventoryHander.AddBacKhoa(_myChar2, int.MaxValue, ThongBao: true);
					InventoryHander.AddVang(_myChar2, int.MaxValue, ThongBao: true);
					InventoryHander.AddVangKhoa(_myChar2, int.MaxValue, ThongBao: true);
				}
				else
				{
					InventoryHander.AddBacKhoa(_myChar2, QuantityAddBac, ThongBao: true);
				}
				_myChar2.SendMessage(UtilMessage.SetNullItembag(item.Index));
				_myChar2.Inventory.ItemBag[item.Index] = null;
				break;
			}
			case 277:
				item3 = new Item((short)Util.NextInt(4, 6), IsLock: true);
				if (InventoryHander.AddItemBag(_myChar2, item3))
				{
					MsgUseItemBag(_myChar2, item);
					InventoryHander.RemoveItemBag(_myChar2, item.Index);
				}
				break;
			case 231:
				item3 = new Item(176, IsLock: true);
				item3.Quantity = 100;
				if (InventoryHander.AddItemBag(_myChar2, item3))
				{
					MsgUseItemBag(_myChar2, item);
					InventoryHander.RemoveItemBag(_myChar2, item.Index);
				}
				break;
			case 329:
			{
				Item item2 = DataServer.ItemCaiTrang[Util.NextInt(0, DataServer.ItemCaiTrang.Length - 1)];
				item3 = new Item(item2.Id, IsLock: true);
				item3.IdClass = -1;
				short LevelNeed = DataServer.ArrItemTemplate[item3.Id].LevelNeed;
				short ValueOp = 0;
				item3.Options = "209," + ((LevelNeed >= 10 && LevelNeed <= 20) ? ((short)Util.NextInt(5, 10)) : ((LevelNeed > 20 && LevelNeed <= 35) ? ((short)Util.NextInt(15, 25)) : ((LevelNeed <= 36 || LevelNeed > 45) ? ((short)Util.NextInt(36, 50)) : ((short)Util.NextInt(26, 35))))) + ";2,50;5,5";
				if (InventoryHander.AddItemBag(_myChar2, item3))
				{
					MsgUseItemBag(_myChar2, item);
					InventoryHander.RemoveItemBag(_myChar2, item.Index);
				}
				break;
			}
			case 428:
				item3 = new Item((short)Util.NextInt(406, 413), IsLock: true);
				if (InventoryHander.AddItemBag(_myChar2, item3))
				{
					MsgUseItemBag(_myChar2, item);
					InventoryHander.RemoveItemBag(_myChar2, item.Index);
				}
				break;
			case 704:
			case 790:
				UseNhanThuatSaoChep(_myChar2, item);
				break;
			case 435:
			case 719:
			case 778:
				UseSachChienDau(_myChar2, item);
				break;
			case 380:
			case 383:
			{
				string Info = ((item.Id == 383) ? "Đang cắm " : "Đang đặt bẫy");
				_myChar2.SendMessage(UtilMessage.UseItemCanTime(7000, Info + " " + DataServer.ArrItemTemplate[item.Id].Name, 0, _myChar2.Info.IdUser, item.Id));
				bool IsOk = true;
				long Time = Util.CurrentTimeMillis() + 7000;
				new Task(delegate
				{
					while (true)
					{
						if (Time < Util.CurrentTimeMillis())
						{
							if (!_myChar2.IsConnection || !_myChar2.IsConnection)
							{
								return;
							}
							if ((_myChar2.Task.Id == 6 && (_myChar2.Task.IdStep == 0 || _myChar2.Task.IdStep == 1 || _myChar2.Task.IdStep == 2)) || (_myChar2.Task.Id == 11 && (_myChar2.Task.IdStep == 0 || _myChar2.Task.IdStep == 1 || _myChar2.Task.IdStep == 2)))
							{
								TaskHander.NextStep(_myChar2);
							}
							{
								foreach (Character current in _myChar2.InfoGame.ZoneGame.Chars.Values)
								{
									if (current.IsConnection)
									{
										Message message = new Message(7);
										message.WriteInt(_myChar2.Info.IdUser);
										current.SendMessage(message);
									}
								}
								return;
							}
						}
						if (_myChar2.IsConnection && _myChar2.Info.IsDie)
						{
							break;
						}
						Thread.Sleep(10);
					}
					_myChar2.SendMessage(UtilMessage.ClearSceen());
				}).Start();
				break;
			}
			case 599:
			case 600:
			case 723:
			case 736:
				UseSachHocSkill(_myChar2, item);
				break;
			case 174:
			case 175:
			case 179:
			case 216:
			case 217:
			case 218:
			case 248:
			case 278:
			case 302:
			case 315:
				UseLenhBaiHokage(_myChar2, item);
				break;
			case 498:
			case 568:
				UseLenhBaiCamThuat(_myChar2, item);
				break;
			case 688:
				UseSkillViThu(_myChar2, item);
				break;
			case 870:
				UseUpBuaNo(_myChar2, item);
				break;
			case 294:
				UseUpOptionBiKiep(_myChar2, item);
				break;
			default:
				_myChar2.SendMessage(UtilMessage.SendThongBao("Vật phẩm này chưa thể sử dụng :D", Util.WHITE));
				break;
			}
		}

		private static void UseUpOptionBiKiep(Character _myChar, Item item)
		{
			if (_myChar.Inventory.ItemBody[11] == null)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Cần trang bị bí kiếp", Util.WHITE));
				return;
			}
			ItemOption[] itemOptions = _myChar.Inventory.ItemBody[11].L(IsSet: true);
			StringBuilder stringBuilder = new StringBuilder();
			sbyte Level = 0;
			for (int i = 0; i < itemOptions.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(";");
				}
				if (itemOptions[i].a[0] == 128)
				{
					if (itemOptions[i].a[1] == itemOptions[i].a[2])
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Bí kiếp đã đạt tối đa độ tu luyện", Util.WHITE));
						return;
					}
					itemOptions[i].a[1] += 25;
					int var2 = itemOptions[i].a[1];
					short var1 = (short)(itemOptions[i].a[2] / 18);
					if (itemOptions[i].a[1] > itemOptions[i].a[2])
					{
						itemOptions[i].a[1] = itemOptions[i].a[2];
						Level = 18;
					}
					else
					{
						while (var2 > var1)
						{
							Level++;
							var2 -= var1;
						}
					}
					stringBuilder.Append(itemOptions[i].g());
				}
				else
				{
					stringBuilder.Append(itemOptions[i].g());
				}
			}
			if (Level != _myChar.Inventory.ItemBody[11].Level)
			{
				if (Level == 4)
				{
					_myChar.TuongKhac.GetPointFromItem(_myChar.Inventory.ItemBody[11], _myChar, IsDownPoint: true);
					switch (_myChar.Inventory.ItemBody[11].Id)
					{
					case 749:
						stringBuilder.Append(";").Append(26 + "," + Util.NextInt(120, 200));
						break;
					case 888:
						stringBuilder.Append(";").Append(26 + "," + Util.NextInt(222, 400));
						break;
					default:
						stringBuilder.Append(";").Append(26 + "," + Util.NextInt(50, 100));
						break;
					}
					switch (_myChar.Inventory.ItemBody[11].Id)
					{
					case 749:
						stringBuilder.Append(";").Append(27 + "," + Util.NextInt(120, 200));
						break;
					case 888:
						stringBuilder.Append(";").Append(27 + "," + Util.NextInt(222, 400));
						break;
					default:
						stringBuilder.Append(";").Append(27 + "," + Util.NextInt(50, 100));
						break;
					}
					_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: false, stringBuilder.ToString());
				}
				if (Level == 8)
				{
					_myChar.TuongKhac.GetPointFromItem(_myChar.Inventory.ItemBody[11], _myChar, IsDownPoint: true);
					switch (_myChar.Inventory.ItemBody[11].Id)
					{
					case 749:
						stringBuilder.Append(";").Append(28 + "," + Util.NextInt(30, 40));
						break;
					case 888:
						stringBuilder.Append(";").Append(28 + "," + Util.NextInt(50, 60));
						break;
					default:
						stringBuilder.Append(";").Append(28 + "," + Util.NextInt(10, 20));
						break;
					}
					_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: false, stringBuilder.ToString());
				}
				if (Level == 12)
				{
					_myChar.TuongKhac.GetPointFromItem(_myChar.Inventory.ItemBody[11], _myChar, IsDownPoint: true);
					switch (_myChar.Inventory.ItemBody[11].Id)
					{
					case 749:
						stringBuilder.Append(";").Append(31 + "," + Util.NextInt(100, 200));
						break;
					case 888:
						stringBuilder.Append(";").Append(31 + "," + Util.NextInt(300, 400));
						break;
					default:
						stringBuilder.Append(";").Append(31 + "," + Util.NextInt(50, 100));
						break;
					}
					_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: false, stringBuilder.ToString());
				}
				if (Level == 14)
				{
					_myChar.TuongKhac.GetPointFromItem(_myChar.Inventory.ItemBody[11], _myChar, IsDownPoint: true);
					switch (_myChar.Inventory.ItemBody[11].Id)
					{
					case 749:
						stringBuilder.Append(";").Append(Util.NextInt(48, 52) + "," + Util.NextInt(11, 21));
						break;
					case 888:
						stringBuilder.Append(";").Append(Util.NextInt(48, 52) + "," + Util.NextInt(20, 30));
						break;
					default:
						stringBuilder.Append(";").Append(Util.NextInt(48, 52) + "," + Util.NextInt(5, 10));
						break;
					}
					_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: false, stringBuilder.ToString());
				}
				if (Level == 18)
				{
					_myChar.TuongKhac.GetPointFromItem(_myChar.Inventory.ItemBody[11], _myChar, IsDownPoint: true);
					switch (_myChar.Inventory.ItemBody[11].Id)
					{
					case 749:
						stringBuilder.Append(";").Append(323 + "," + Util.NextInt(100, 200));
						break;
					case 888:
						stringBuilder.Append(";").Append(323 + "," + Util.NextInt(200, 400));
						break;
					default:
						stringBuilder.Append(";").Append(323 + "," + Util.NextInt(50, 100));
						break;
					}
					_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: false, stringBuilder.ToString());
				}
			}
			_myChar.Inventory.ItemBody[11].Options = stringBuilder.ToString();
			_myChar.Inventory.ItemBody[11].Level = Level;
			_myChar.SendMessage(UtilMessage.UpdateIndexItemBody(_myChar.Inventory.ItemBody[11]));
			MsgUseItemBag(_myChar, item);
			InventoryHander.RemoveItemBag(_myChar, item.Index);
			_myChar.SendMessage(UtilMessage.SendThongBao("Nhận được 25 độ luyện ", Util.WHITE));
		}

		private static void UseUpBuaNo(Character _myChar, Item item)
		{
			if (_myChar.Inventory.ItemBody[13] == null)
			{
				return;
			}
			if (_myChar.Inventory.ItemBody[13].Id != 811)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Chỉ sử dụng được với bùa nổ siêu cấp", Util.YELLOW_MID));
				return;
			}
			if (_myChar.Inventory.ItemBody[13].CountBuaNo >= 4)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Chỉ được sử dụng tối đa 4 cái", Util.YELLOW_MID));
				return;
			}
			short IdOption1 = -1;
			short IdOption2 = -1;
			switch (_myChar.Info.IdClass)
			{
			case 1:
				IdOption1 = 54;
				IdOption2 = 62;
				break;
			case 2:
				IdOption1 = 55;
				IdOption2 = 58;
				break;
			case 3:
				IdOption1 = 56;
				IdOption2 = 59;
				break;
			case 4:
				IdOption1 = 57;
				IdOption2 = 60;
				break;
			case 5:
				IdOption1 = 53;
				IdOption2 = 61;
				break;
			}
			ItemOption[] itemOptions = _myChar.Inventory.ItemBody[13].L(IsSet: true);
			StringBuilder stringBuilder = new StringBuilder();
			string Options2 = _myChar.Inventory.ItemBody[13].Options;
			for (int i = 0; i < itemOptions.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(";");
				}
				if ((itemOptions[i].a[0] == IdOption1 && itemOptions[i].a[1] < 1500) || (itemOptions[i].a[0] == IdOption2 && itemOptions[i].a[1] < 1500))
				{
					return;
				}
				if (itemOptions[i].a[0] == IdOption1 || itemOptions[i].a[0] == IdOption2)
				{
					itemOptions[i].a[2] += 150;
				}
				stringBuilder.Append(itemOptions[i].g());
			}
			_myChar.Inventory.ItemBody[13].CountBuaNo++;
			_myChar.Inventory.ItemBody[13].Options = stringBuilder.ToString();
			_myChar.SendMessage(UtilMessage.UpdateIndexItemBody(_myChar.Inventory.ItemBody[13]));
			MsgUseItemBag(_myChar, item);
			InventoryHander.RemoveItemBag(_myChar, item.Index);
			_myChar.SendMessage(UtilMessage.SendThongBao("Sử dụng thành công siêu bùa nổ", Util.YELLOW_MID));
		}

		private static void UseSkillViThu(Character _myChar, Item item)
		{
			if (ViThuHander.UseSkillViThu(_myChar, item))
			{
				MsgUseItemBag(_myChar, item);
				InventoryHander.RemoveItemBag(_myChar, item.Index);
			}
		}

		private static void UseLenhBaiCamThuat(Character _myChar, Item item)
		{
			if (item.Id == 498)
			{
				if (_myChar.TimeChar.IsCanUseLenhBaiCamThuat[0])
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Mỗi ngày chỉ được sử dụng 1 lần", Util.WHITE));
					return;
				}
				_myChar.TimeChar.IsCanUseLenhBaiCamThuat[0] = true;
				_myChar.TimeChar.SoLanDiCamThuat++;
				MsgUseItemBag(_myChar, item);
				InventoryHander.RemoveItemBag(_myChar, item.Index);
				_myChar.SendMessage(UtilMessage.SendThongBao("Tăng thêm 1 lần vào cấm thuật ", Util.YELLOW_MID));
			}
			else if (_myChar.TimeChar.IsCanUseLenhBaiCamThuat[1])
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Mỗi ngày chỉ được sử dụng 1 lần", Util.WHITE));
			}
			else
			{
				_myChar.TimeChar.IsCanUseLenhBaiCamThuat[1] = true;
				_myChar.TimeChar.SoLanDiCamThuat += 2;
				MsgUseItemBag(_myChar, item);
				InventoryHander.RemoveItemBag(_myChar, item.Index);
				_myChar.SendMessage(UtilMessage.SendThongBao("Tăng thêm 2 lần vào cấm thuật", Util.WHITE));
			}
		}

		private static void UseNhanThuatSaoChep(Character _myChar, Item item)
		{
			if (item.Id == 704)
			{
				if (_myChar.Point.IsCanUseNhanThuatSaoChep[0])
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Chỉ được sử dụng 1 lần", Util.WHITE));
					return;
				}
				_myChar.Point.IsCanUseNhanThuatSaoChep[0] = true;
				_myChar.Point.PointGhepCaiTrang = 1;
				MsgUseItemBag(_myChar, item);
				InventoryHander.RemoveItemBag(_myChar, item.Index);
				_myChar.SendMessage(UtilMessage.SendThongBao("Đã mở thêm giới hạn khi ghép cải trang", Util.YELLOW_MID));
			}
			else if (item.Id == 790)
			{
				if (!_myChar.Point.IsCanUseNhanThuatSaoChep[0])
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Cần sử dụng Nhẫn thuật sơ cấp trước", Util.WHITE));
					return;
				}
				if (_myChar.Point.IsCanUseNhanThuatSaoChep[1])
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Chỉ được sử dụng 1 lần", Util.WHITE));
					return;
				}
				_myChar.Point.IsCanUseNhanThuatSaoChep[1] = true;
				_myChar.Point.PointGhepCaiTrang = 2;
				MsgUseItemBag(_myChar, item);
				InventoryHander.RemoveItemBag(_myChar, item.Index);
				_myChar.SendMessage(UtilMessage.SendThongBao("Đã mở thêm giới hạn khi ghép cải trang", Util.YELLOW_MID));
			}
		}

		private static void UseLenhBaiHokage(Character _myChar, Item item)
		{
			sbyte Index = -1;
			switch (item.Id)
			{
			case 218:
				Index = 5;
				break;
			case 179:
				Index = 0;
				break;
			case 248:
				Index = 1;
				break;
			case 175:
				Index = 2;
				break;
			case 174:
				Index = 3;
				break;
			case 216:
				Index = 4;
				break;
			case 278:
				Index = 6;
				break;
			case 302:
				Index = 7;
				break;
			case 315:
				Index = 8;
				break;
			case 217:
				Index = 9;
				break;
			}
			if (Index != -1)
			{
				_myChar.Info.PointItemHokage[Index] += 5;
				if (_myChar.Info.PointItemHokage[Index] < 0)
				{
					_myChar.Info.PointItemHokage[Index] = short.MaxValue;
				}
				_myChar.SendMessage(UtilMessage.SendThongBao("Tăng thành công 5 điểm hokage " + ShopHander.Hokage[Index], Util.WHITE));
				MsgUseItemBag(_myChar, item);
				InventoryHander.RemoveItemBag(_myChar, item.Index);
			}
		}

		private static void UseSachHocSkill(Character _myChar, Item item)
		{
			switch (item.Id)
			{
			case 599:
			{
				if (item.Quantity < 1000)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Cần đủ 1000 mảnh huyết kế giới hạn ", Util.WHITE));
					break;
				}
				_myChar.Inventory.ItemBag[item.Index] = null;
				_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
				Item item2 = new Item(600, IsLock: true);
				if (!InventoryHander.AddItemBag(_myChar, item2))
				{
				}
				break;
			}
			case 736:
			{
				if (item.Quantity < 3000)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Cần đủ 3000 mảnn giấy đặc biệt ", Util.WHITE));
					break;
				}
				if (_myChar.Inventory.Bac < 1000000)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Cần 1.000.000 Bạc ", Util.WHITE));
					break;
				}
				_myChar.Inventory.ItemBag[item.Index] = null;
				_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
				InventoryHander.UpdateBac(_myChar, 1000000, ThongBao: true);
				Item item3 = new Item(723, IsLock: true);
				if (!InventoryHander.AddItemBag(_myChar, item3))
				{
				}
				break;
			}
			case 600:
				switch (_myChar.Info.IdClass)
				{
				case 1:
					if (_myChar.Info.GioiTinh == 1)
					{
						if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 7))
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
							return;
						}
						_myChar.Inventory.ItemBag[item.Index] = null;
						_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
						Skill skill8 = InfoSkill.GetCloneSkill(7, 0);
						List<Skill> skills8 = _myChar.Skill.Skills.ToList();
						skills8.Add(skill8);
						_myChar.Skill.Skills = skills8.ToArray();
					}
					else
					{
						if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 31))
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
							return;
						}
						_myChar.Inventory.ItemBag[item.Index] = null;
						_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
						Skill skill9 = InfoSkill.GetCloneSkill(31, 0);
						List<Skill> skills9 = _myChar.Skill.Skills.ToList();
						skills9.Add(skill9);
						_myChar.Skill.Skills = skills9.ToArray();
					}
					break;
				case 2:
					if (_myChar.Info.GioiTinh == 1)
					{
						if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 32))
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
							return;
						}
						_myChar.Inventory.ItemBag[item.Index] = null;
						_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
						Skill skill6 = InfoSkill.GetCloneSkill(32, 0);
						List<Skill> skills6 = _myChar.Skill.Skills.ToList();
						skills6.Add(skill6);
						_myChar.Skill.Skills = skills6.ToArray();
					}
					else
					{
						if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 1))
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
							return;
						}
						_myChar.Inventory.ItemBag[item.Index] = null;
						_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
						Skill skill7 = InfoSkill.GetCloneSkill(1, 0);
						List<Skill> skills7 = _myChar.Skill.Skills.ToList();
						skills7.Add(skill7);
						_myChar.Skill.Skills = skills7.ToArray();
					}
					break;
				case 3:
					if (_myChar.Info.GioiTinh == 1)
					{
						if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 34))
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
							return;
						}
						_myChar.Inventory.ItemBag[item.Index] = null;
						_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
						Skill skill4 = InfoSkill.GetCloneSkill(34, 0);
						List<Skill> skills4 = _myChar.Skill.Skills.ToList();
						skills4.Add(skill4);
						_myChar.Skill.Skills = skills4.ToArray();
					}
					else
					{
						if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 13))
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
							return;
						}
						_myChar.Inventory.ItemBag[item.Index] = null;
						_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
						Skill skill5 = InfoSkill.GetCloneSkill(13, 0);
						List<Skill> skills5 = _myChar.Skill.Skills.ToList();
						skills5.Add(skill5);
						_myChar.Skill.Skills = skills5.ToArray();
					}
					break;
				case 4:
					if (_myChar.Info.GioiTinh == 1)
					{
						if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 19))
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
							return;
						}
						_myChar.Inventory.ItemBag[item.Index] = null;
						_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
						Skill skill2 = InfoSkill.GetCloneSkill(19, 0);
						List<Skill> skills = _myChar.Skill.Skills.ToList();
						skills.Add(skill2);
						_myChar.Skill.Skills = skills.ToArray();
					}
					else
					{
						if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 33))
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
							return;
						}
						_myChar.Inventory.ItemBag[item.Index] = null;
						_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
						Skill skill3 = InfoSkill.GetCloneSkill(33, 0);
						List<Skill> skills3 = _myChar.Skill.Skills.ToList();
						skills3.Add(skill3);
						_myChar.Skill.Skills = skills3.ToArray();
					}
					break;
				case 5:
				{
					if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 25))
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
						return;
					}
					_myChar.Inventory.ItemBag[item.Index] = null;
					_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
					Skill skill13 = InfoSkill.GetCloneSkill(25, 0);
					List<Skill> skills13 = _myChar.Skill.Skills.ToList();
					skills13.Add(skill13);
					_myChar.Skill.Skills = skills13.ToArray();
					break;
				}
				}
				_myChar.SendMessage(UtilMessage.ClearSceen());
				_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã đánh thức huyết kế giới hạn", Util.YELLOW_MID));
				_myChar.MsgUpdateSkill();
				break;
			case 723:
				if (_myChar.Skill.Skills.Length < 7)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn chưa đủ yêu cầu để học kỹ năng này", Util.WHITE));
					break;
				}
				switch (_myChar.Info.IdClass)
				{
				case 1:
				{
					if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 38))
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
						return;
					}
					_myChar.Inventory.ItemBag[item.Index] = null;
					_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
					Skill skill = InfoSkill.GetCloneSkill(38, 0);
					List<Skill> skills2 = _myChar.Skill.Skills.ToList();
					skills2.Add(skill);
					_myChar.Skill.Skills = skills2.ToArray();
					break;
				}
				case 2:
				{
					if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 39))
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
						return;
					}
					_myChar.Inventory.ItemBag[item.Index] = null;
					_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
					Skill skill10 = InfoSkill.GetCloneSkill(39, 0);
					List<Skill> skills10 = _myChar.Skill.Skills.ToList();
					skills10.Add(skill10);
					_myChar.Skill.Skills = skills10.ToArray();
					break;
				}
				case 3:
				{
					if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 37))
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
						return;
					}
					_myChar.Inventory.ItemBag[item.Index] = null;
					_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
					Skill skill11 = InfoSkill.GetCloneSkill(37, 0);
					List<Skill> skills11 = _myChar.Skill.Skills.ToList();
					skills11.Add(skill11);
					_myChar.Skill.Skills = skills11.ToArray();
					break;
				}
				case 4:
				{
					if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 40))
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
						return;
					}
					_myChar.Inventory.ItemBag[item.Index] = null;
					_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
					Skill skill12 = InfoSkill.GetCloneSkill(40, 0);
					List<Skill> skills12 = _myChar.Skill.Skills.ToList();
					skills12.Add(skill12);
					_myChar.Skill.Skills = skills12.ToArray();
					break;
				}
				case 5:
				{
					if (_myChar.Skill.Skills.Any((Skill s) => s.IdTemplate == 36))
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có kỹ năng này rồi", Util.WHITE));
						return;
					}
					_myChar.Inventory.ItemBag[item.Index] = null;
					_myChar.SendMessage(UtilMessage.SetNullItembag(item.Index));
					Skill skill14 = InfoSkill.GetCloneSkill(36, 0);
					List<Skill> skills14 = _myChar.Skill.Skills.ToList();
					skills14.Add(skill14);
					_myChar.Skill.Skills = skills14.ToArray();
					break;
				}
				}
				_myChar.SendMessage(UtilMessage.ClearSceen());
				_myChar.SendMessage(UtilMessage.SendThongBao("Bạn học được nhẫn thuật đặc biệt", Util.YELLOW_MID));
				_myChar.MsgUpdateSkill();
				break;
			}
		}

		public static void DoiBuiNo(Character _myChar, Message msg)
		{
			sbyte TypeTemp = msg.ReadByte();
			short Index = msg.ReadShort();
			Item BuaNo = _myChar.GetItemFromType(TypeTemp, Index);
			if (BuaNo == null || BuaNo.Type != 13)
			{
				return;
			}
			if (_myChar.Inventory.Bac < 1000000)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ bạc", Util.WHITE));
				return;
			}
			bool Check = false;
			short Value2 = 0;
			short Value3 = 0;
			string[] Option = BuaNo.Options.Split(";");
			for (int i = 0; i < 2; i++)
			{
				string[] Value = Option[i].Split(",");
				short Value4 = short.Parse(Value[1]);
				short Value5 = short.Parse(Value[2]);
				Value2 += Value4;
				Value3 += Value5;
			}
			if (Value2 < Value3)
			{
				return;
			}
			short IdOption1 = -1;
			short IdOption2 = -1;
			switch (_myChar.Info.IdClass)
			{
			case 1:
				IdOption1 = 54;
				IdOption2 = 62;
				break;
			case 2:
				IdOption1 = 55;
				IdOption2 = 58;
				break;
			case 3:
				IdOption1 = 56;
				IdOption2 = 59;
				break;
			case 4:
				IdOption1 = 57;
				IdOption2 = 60;
				break;
			case 5:
				IdOption1 = 53;
				IdOption2 = 61;
				break;
			}
			if (InventoryHander.GetCountNotNullBag(_myChar) > 0)
			{
				Item BuaNo2 = new Item((short)((BuaNo.Id == 134) ? 602 : 811), IsLock: true);
				BuaNo2.IdClass = _myChar.Info.IdClass;
				string OptionBuaNo = "";
				switch (BuaNo2.Id)
				{
				case 602:
					OptionBuaNo = ",0,1000;" + IdOption2 + ",0,1000;2,100;5,50;149,3;122,5";
					break;
				case 811:
					OptionBuaNo = ",0,1500;" + IdOption2 + ",0,1500;2,150;5,150;167,150;149,3;122,15";
					break;
				}
				BuaNo2.Options = IdOption1 + OptionBuaNo;
				_myChar.SetNullItemFromType(TypeTemp, Index);
				_myChar.SendMessage(UtilMessage.ClearSceen());
				_myChar.SendMessage(UtilMessage.SetNullItemFromType(TypeTemp, Index));
				if (TypeTemp == 2)
				{
					_myChar.TuongKhac.GetPointFromItem(BuaNo, _myChar, IsDownPoint: true);
					_myChar.TuongKhac.GetPointFromItem(BuaNo2, _myChar);
					_myChar.Inventory.ItemBody[BuaNo.Index] = BuaNo2;
					_myChar.SendMessage(UtilMessage.UpdateIndexItemBody(BuaNo2));
					_myChar.SendMessage(UtilMessage.SendThongBao("Nâng cấp thành công bùa nổ", Util.YELLOW_MID));
				}
				else
				{
					InventoryHander.AddItemBag(_myChar, BuaNo2);
				}
				InventoryHander.UpdateBac(_myChar, 1000000, ThongBao: true);
			}
		}

		private static void UseSachChienDau(Character _myChar, Item item)
		{
			if (_myChar.Info.SachChienDau < 18 && (_myChar.Info.SachChienDau == 17 || item.Id != 778) && (_myChar.Info.SachChienDau == 16 || item.Id != 719))
			{
				if (_myChar.Info.SachChienDau > 0)
				{
					_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: true, _myChar.TuongKhac.OptionSachChienDau);
				}
				item.Quantity--;
				MsgUseItemBag(_myChar, item);
				if (item.Quantity <= 0)
				{
					InventoryHander.RemoveItemBag(_myChar, item.Index);
				}
				_myChar.Info.SachChienDau++;
				if (_myChar.Info.SachChienDau == 17)
				{
					_myChar.TuongKhac.OptionSachChienDau += ";311,150";
				}
				else if (_myChar.Info.SachChienDau == 18)
				{
					_myChar.TuongKhac.OptionSachChienDau += ";311,150";
				}
				ItemOptionHander.UpOptionNotItem(ref _myChar.TuongKhac.OptionSachChienDau, _myChar.Info.SachChienDau);
				_myChar.SendMessage(UtilMessage.MsgUpdateSachChienDau(_myChar.Info.SachChienDau));
				_myChar.SendMessage(UtilMessage.SendThongBao("Sử dụng thành công sách chiến đấu Level thực lực tăng mạnh", Util.WHITE));
				_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: false, _myChar.TuongKhac.OptionSachChienDau);
			}
		}
	}
}
