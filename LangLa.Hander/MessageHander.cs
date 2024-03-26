using System;
using LangLa.Client;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Server;

namespace LangLa.Hander
{
	public static class MessageHander
	{
		public static void Perform(LangLa.Client.Client _Client, Message msg)
		{
			sbyte CMD = msg.CMD;
			try
			{
				switch (CMD)
				{
				case -95:
					_Client.Character.WriteDataBag();
					break;
				case 5:
					NpcHander.ReadOderMenuSelect(_Client.Character, msg);
					break;
				case 8:
					TaskHander.CheckHaiVatPhat(_Client.Character, msg);
					break;
				case 19:
					PvPHander.CuuSat(_Client.Character, msg);
					break;
				case 30:
					PvPHander.TuChoiTyVo(_Client.Character, msg);
					break;
				case 31:
					PvPHander.StartTyVo(_Client.Character, msg);
					break;
				case 32:
					PvPHander.TyVo(_Client.Character, msg);
					break;
				case -25:
					NpcHander.Read25(_Client.Character, msg);
					break;
				case -22:
					TopHander.ShowTop(_Client.Character, msg);
					break;
				case -35:
					CombineHander.CheckUpVuKhiNew(_Client.Character, msg);
					break;
				case -50:
					CombineHander.HanderGhepCaiTrang(_Client.Character, msg);
					break;
				case -51:
					CombineHander.HanderTachCaiTrang(_Client.Character, msg);
					break;
				case -52:
					CombineHander.ShowOptionCaiTrangTach(_Client.Character, msg);
					break;
				case -47:
					CombineHander.HanderTachNgocKham(_Client.Character, msg);
					break;
				case -46:
					CombineHander.HanderKhamNgoc(_Client.Character, msg);
					break;
				case -15:
					PvPHander.ChangeTypePk(_Client.Character, msg);
					break;
				case -6:
					ZoneHander.OpenTabZone(_Client.Character);
					break;
				case -7:
					ZoneHander.ChangeZone(_Client.Character, msg);
					break;
				case -56:
				{
					short Index = msg.ReadShort();
					TaskHander.AddItemTask(_Client.Character);
					break;
				}
				case 10:
					TaskHander.CheckClickDoneTask(_Client.Character, msg);
					break;
				case 11:
				case 12:
					TaskHander.CheckClickNhanNhiemVu(_Client.Character);
					break;
				case 13:
					CharacterHander.SendInfoMobAttack(_Client.Character, msg);
					break;
				case 14:
					SkillHander.UpdatePointSkill(_Client.Character, msg);
					break;
				case 20:
					CharacterHander.AttackChar(_Client.Character, msg);
					break;
				case 21:
					CharacterHander.HanderChatChar(_Client.Character, msg);
					break;
				case 22:
					CharacterHander.ChatTheGioi(_Client.Character, msg);
					break;
				case 25:
					if (_Client.Character.Info.IdGiaToc != -1)
					{
						_Client.Character.InfoGame.GiaToc.ChatGiaToc(_Client.Character, msg);
					}
					break;
				case 26:
					if (_Client.Character.InfoGame.Todoi != null)
					{
						ToDoiHander.ChatNhom(_Client.Character, msg);
					}
					break;
				case 34:
				{
					string Name = msg.ReadString();
					Character CView2 = LangLa.Server.Server.GetChar(Name);
					if (CView2 == null)
					{
						_Client.AddMessage(UtilMessage.SendThongBao("Người chơi không online", Util.WHITE));
						break;
					}
					CView2.SendMessage(UtilMessage.SendThongBao(_Client.Character.Info.Name + " đang xem thông tin về bạn", Util.YELLOW_MID));
					_Client.Character.SendMessage(UtilMessage.ViewInfoChar(CView2));
					_Client.Character.InfoGame._CView = CView2;
					break;
				}
				case 38:
					ToDoiHander.TuChoiVaoDoi(_Client.Character, msg);
					break;
				case 39:
					ToDoiHander.ChapNhanVaoDoi(_Client.Character, msg);
					break;
				case 41:
					ToDoiHander.CreateToDoi(_Client.Character, msg);
					break;
				case 42:
					if (_Client.Character.InfoGame.Todoi != null)
					{
						_Client.Character.InfoGame.Todoi.IsLockNhom = false;
					}
					break;
				case 43:
					if (_Client.Character.InfoGame.Todoi != null)
					{
						ToDoiHander.ToDoiForMe(_Client.Character);
					}
					break;
				case 44:
					if (_Client.Character.InfoGame.Todoi != null)
					{
						ToDoiHander.LeaveToDoi(_Client.Character);
					}
					break;
				case 45:
					ToDoiHander.ShowListTodoi(_Client.Character);
					break;
				case 46:
					if (_Client.Character.InfoGame.Todoi != null && _Client.Character.InfoGame.IsDoiTruong)
					{
						ToDoiHander.NhuongDoiTruong(_Client.Character, msg);
					}
					break;
				case 47:
					if (_Client.Character.InfoGame.Todoi != null)
					{
						ToDoiHander.KichMember(_Client.Character, msg);
					}
					break;
				case 48:
					if (_Client.Character.Info.IsDie)
					{
						_Client.Character.BackHome();
					}
					break;
				case 49:
					if (_Client.Character.Info.IsDie)
					{
						_Client.Character.SetLive();
					}
					break;
				case 53:
					NpcHander.HanderMenu(_Client.Character, msg);
					break;
				case 54:
					NpcHander.OpenMenu(_Client.Character, msg);
					break;
				case 59:
					ZoneHander.PickItemMap(_Client.Character, msg);
					break;
				case 61:
					CharacterHander.AttackMob(_Client.Character, msg);
					break;
				case 62:
					_Client.Character.UpdatePoint(msg);
					break;
				case 63:
					CharacterHander.GetInfo(_Client.Character, msg);
					break;
				case 81:
					InventoryHander.LockGiaoDich(_Client.Character, msg);
					break;
				case 82:
					InventoryHander.ShowListItemGiaoDich(_Client.Character, msg);
					break;
				case 83:
					InventoryHander.HuyGiaoDich(_Client.Character);
					break;
				case 85:
					InventoryHander.StartGiaoDich(_Client.Character, msg);
					break;
				case 86:
					InventoryHander.CreateGiaoDich(_Client.Character, msg);
					break;
				case 88:
					ThuHander.RemoveThu(_Client.Character, msg);
					break;
				case 96:
					ThuHander.NhanThu(_Client.Character, msg);
					break;
				case 107:
					CombineHander.HanderCuongHoaTrangBi(_Client.Character, msg);
					break;
				case 104:
					CombineHander.HanderDichChuyenTrangBi(_Client.Character, msg);
					break;
				case 105:
					CombineHander.HanderTachCuongHoa(_Client.Character, msg);
					break;
				case 106:
					CombineHander.HanderNangCapBuaNo(_Client.Character, msg);
					break;
				case 108:
					CombineHander.HanderGhepData(_Client.Character, msg);
					break;
				case 112:
					InventoryHander.ItemMoRongToBag(_Client.Character, msg);
					break;
				case 113:
					ItemHander.ItemBodyTobag(_Client.Character, msg);
					break;
				case 114:
					InventoryHander.ItemBoxToBag(_Client.Character, msg);
					break;
				case 115:
					InventoryHander.ItemBagToBox(_Client.Character, msg);
					break;
				case 116:
					ItemHander.UseItem(_Client.Character, msg);
					break;
				case 117:
					InventoryHander.SortItem(_Client.Character, msg);
					break;
				case 118:
					InventoryHander.TachItem(_Client.Character, msg);
					break;
				case 119:
					ShopHander.CharSale(_Client.Character, msg);
					break;
				case 121:
					ShopHander.BuyShop(_Client.Character, msg);
					break;
				case 122:
					ClickEventHander.HanderClick(_Client.Character, msg);
					break;
				case 126:
					SkillHander.SetSkillFocus(_Client.Character, msg);
					break;
				case sbyte.MaxValue:
					_Client.Character.NextMap();
					break;
				default:
					Util.ShowWarring("CMD Controler " + CMD + " Chưa làm");
					break;
				}
			}
			catch (Exception var)
			{
				Util.ShowErr(var);
			}
		}

		public static void ReadMessage122(LangLa.Client.Client _Client, Message msg)
		{
			sbyte cMD = msg.CMD;
			sbyte b = cMD;
			Util.ShowWarring("CMD 122 " + msg.CMD + " Chưa làm");
		}

		public static void ReadMessage123(LangLa.Client.Client _Client, Message msg)
		{
			Util.ShowWarring("CMD 123 " + msg.CMD);
			switch (msg.CMD)
			{
			case -17:
				CombineHander.UpdateVuKhiLucDao(_Client.Character, msg);
				break;
			case -18:
				ViThuHander.MoRongSkillViThu(_Client.Character, msg);
				break;
			case -26:
				ViThuHander.RemoveSkillViThu(_Client.Character, msg);
				break;
			case -45:
				InventoryHander.MoneyBoxToBag(_Client.Character, msg);
				break;
			case -46:
				InventoryHander.MoneyBagToBox(_Client.Character, msg);
				break;
			case -82:
				InventoryHander.MoRongBox(_Client.Character);
				break;
			case -48:
				ItemHander.DoiBuiNo(_Client.Character, msg);
				break;
			case -50:
				ThuHander.NhanThu(_Client.Character, null, IsNhanAll: true);
				break;
			case -58:
				CharacterHander.AnCaiTrang(_Client.Character, msg);
				break;
			case -68:
				GiaTocHander.UpdateSkillGiaToc(_Client.Character, msg);
				break;
			case -66:
			case -65:
			case -63:
			case -62:
				ShopHander.BuyPhucLoi(_Client.Character, msg.CMD);
				break;
			case -73:
				_Client.Character.ShowThongTin(msg);
				break;
			case -70:
				ShopHander.ClickButtonShopPhucLoi(_Client.Character, msg);
				break;
			case -19:
				CharacterHander.TrieuHoiThuCuoi(_Client.Character, msg);
				break;
			case -92:
				GiaTocHander.XinVaoGiaoToc(_Client.Character, msg);
				break;
			case -90:
				if (_Client.Character.Info.IdGiaToc != -1 && _Client.Character.Info.RoleGiaToc == 5)
				{
					GiaTocHander.PhanPhatItem(_Client.Character, msg);
				}
				break;
			case -91:
				if (_Client.Character.Info.IdGiaToc != -1 && _Client.Character.Info.RoleGiaToc == 5)
				{
					GiaTocHander.BlockChat(_Client.Character, msg);
				}
				break;
			case -93:
				if (_Client.Character.Info.IdGiaToc != -1 && _Client.Character.Info.RoleGiaToc == 5)
				{
					GiaTocHander.SendThongBao(_Client.Character, msg);
				}
				break;
			case -94:
				if (_Client.Character.InfoGame.GiaToc != null)
				{
					GiaTocHander.RutCongHienBac(_Client.Character, msg);
				}
				break;
			case -95:
				if (_Client.Character.InfoGame.GiaToc != null)
				{
					GiaTocHander.AddCongHienBac(_Client.Character, msg);
				}
				break;
			case -96:
				if (_Client.Character.Info.IdGiaToc != -1)
				{
					GiaTocHander.LeaveGiaToc(_Client.Character);
				}
				break;
			case -97:
				if (_Client.Character.Info.IdGiaToc != -1 && _Client.Character.Info.RoleGiaToc == 5)
				{
					GiaTocHander.KickThanhVien(_Client.Character, msg);
				}
				break;
			case -98:
				if (_Client.Character.Info.IdGiaToc != -1 && _Client.Character.Info.RoleGiaToc == 5)
				{
					GiaTocHander.PhatLuongGiaToc(_Client.Character, msg);
				}
				break;
			case -99:
				if (_Client.Character.Info.IdGiaToc != -1 && _Client.Character.Info.RoleGiaToc == 5)
				{
					GiaTocHander.ChangeRole(_Client.Character, msg);
				}
				break;
			case -110:
				GiftcodeHander.CheckGiftCode(_Client.Character, msg);
				break;
			case -100:
				GiaTocHander.ChapNhanMoiVoGiaToc(_Client.Character, msg);
				break;
			case -101:
				GiaTocHander.ChapNhanVoGiaToc(_Client.Character, msg);
				break;
			case -102:
				GiaTocHander.TuChoiMoiVaoGiaToc(_Client.Character, msg);
				break;
			case -103:
				GiaTocHander.TuChoiVoGiaToc(_Client.Character, msg);
				break;
			case -104:
				GiaTocHander.XinVaoGiaToc(_Client.Character, msg);
				break;
			case -105:
				GiaTocHander.MoiVaoGiaToc(_Client.Character, msg);
				break;
			case -106:
				GiaTocHander.CreateGiaToc(_Client.Character, msg);
				break;
			case -85:
				ClickEventHander.HanderClickThuVanMay(_Client.Character);
				break;
			default:
				Util.ShowWarring("CMD 123 " + msg.CMD + " Chưa làm");
				break;
			}
		}
	}
}
