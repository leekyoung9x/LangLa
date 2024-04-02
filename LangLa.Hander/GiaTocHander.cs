using System.Linq;
using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SqlConnection;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.Hander
{
	public static class GiaTocHander
	{
		public static void XinVaoGiaoToc(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			lock (GiaTocManager.ListGiaTocs)
			{
				GiaTocTemplate giaTocTemplate = GiaTocManager.ListGiaTocs.Values.FirstOrDefault((GiaTocTemplate s) => s.Name.Equals(Name));
				if (giaTocTemplate != null)
				{
					GiaTocTemplate.ThanhVienGiaToc thanhvienGiaToc = giaTocTemplate.ThanhViens.FirstOrDefault((GiaTocTemplate.ThanhVienGiaToc s) => s.ChucVu == 5);
					if (thanhvienGiaToc._myChar != null)
					{
						Message i = UtilMessage.Message123();
						i.WriteByte(-104);
						i.WriteUTF(_myChar.Info.Name);
						i.WriteShort(_myChar.Info.Level);
						i.WriteInt(_myChar.Info.TaiPhu);
						thanhvienGiaToc._myChar.SendMessage(i);
						_myChar.SendMessage(UtilMessage.SendThongBao("Gửi đơn gia nhập thành công", Util.WHITE));
					}
					else
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Tộc trưởng của gia tộc " + Name + " đã Offline", Util.WHITE));
					}
				}
				else
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Gia tộc không tồn tại", Util.WHITE));
				}
			}
		}

		public static void MoiVaoGiaToc(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character CharOder = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (CharOder != null)
			{
				Message i = UtilMessage.Message123();
				i.WriteByte(-105);
				i.WriteUTF(_myChar.Info.Name);
				i.WriteUTF(_myChar.InfoGame.GiaToc.Name);
				i.WriteByte(_myChar.Info.RoleGiaToc);
				CharOder.SendMessage(i);
				_myChar.SendMessage(UtilMessage.SendThongBao("Đã gửi lời mời gia tộc đến " + Name, Util.WHITE));
			}
		}

		public static void XinVaoGiaToc(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character CharOder = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (CharOder != null && CharOder.Info.RoleGiaToc >= 3 && CharOder.Info.RoleGiaToc <= 5)
			{
				Message i = UtilMessage.Message123();
				i.WriteByte(-104);
				i.WriteUTF(_myChar.Info.Name);
				i.WriteShort(_myChar.Info.Level);
				i.WriteInt(_myChar.Info.TaiPhu);
				CharOder.SendMessage(i);
				_myChar.SendMessage(UtilMessage.SendThongBao("Đã gửi lời mời gia tộc đến " + Name, Util.WHITE));
			}
		}

		public static void TuChoiMoiVaoGiaToc(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			LangLa.Server.Server.GetChar(Name)?.SendMessage(UtilMessage.SendThongBao(_myChar.Info.Name + " từ chối lời mời vào gia tộc của bạn", Util.WHITE));
		}

		public static void ChapNhanVoGiaToc(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character CharOder = LangLa.Server.Server.GetChar(Name);
			if (CharOder != null)
			{
				AddMember(CharOder, _myChar);
			}
		}

		public static void TuChoiVoGiaToc(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			_myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name))?.SendMessage(UtilMessage.SendThongBao(_myChar.Info.Name + " đã từ chối lời mời gia nhập gia tộc của bạn", Util.WHITE));
		}

		public static void ChapNhanMoiVoGiaToc(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character CharOder = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (CharOder != null && CharOder.Info.RoleGiaToc >= 3 && CharOder.Info.RoleGiaToc <= 5)
			{
				AddMember(_myChar, CharOder);
			}
		}

		private static void AddMember(Character _myChar, Character _cAdd)
		{
			if (_cAdd.Info.IdGiaToc == -1 && (_cAdd.Info.RoleGiaToc != 5 || _cAdd.Info.RoleGiaToc != 4 || _cAdd.Info.RoleGiaToc != 3))
			{
				return;
			}
			_cAdd.InfoGame.GiaToc.AddMember(_myChar);
			_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã gia nhập vào gia tộc " + _myChar.InfoGame.GiaToc.Name, Util.WHITE));
			_cAdd.SendMessage(UtilMessage.SendThongBao(_myChar.Info.Name + " đã chấp nhận lời mời vào gia tộc", Util.WHITE));
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(SendGiaTocMsg(_myChar));
				}
			}
		}

		public static void LeaveGiaToc(Character _myChar)
		{
			Character _myChar2 = _myChar;
			_myChar2.Info.IdGiaToc = -1;
			_myChar2.Info.RoleGiaToc = -1;
			sbyte[] skillGiaToc = _myChar2.InfoGame.GiaToc.SkillGiaToc;
			foreach (sbyte c2 in skillGiaToc)
			{
				if (c2 != -1)
				{
					_myChar2.TuongKhac.GetPointFromSkillGiaToc(_myChar2, DataServer.ArrSkillClan[c2].Options, IsDownPoint: true);
				}
			}
			_myChar2.InfoGame.GiaToc = null;
			_myChar2.InfoGame.GiaToc.ThanhViens.RemoveAt(_myChar2.InfoGame.GiaToc.ThanhViens.FindIndex((GiaTocTemplate.ThanhVienGiaToc s) => s != null && s._myChar != null && s._myChar.Info.IdUser == _myChar2.Info.IdUser));
			_myChar2.InfoGame.GiaToc.SizeThanhVien--;
			_myChar2.SendMessage(UtilMessage.ClearSceen());
			foreach (Character c in _myChar2.InfoGame.ZoneGame.Chars.Values)
			{
				Message i = new Message(-88);
				i.WriteInt(_myChar2.Info.IdUser);
				i.WriteUTF("");
				c.SendMessage(i);
			}
		}

		public static void ShowListGiaToc(Character _myChar)
		{
			if (_myChar.Info.IdGiaToc == -1)
			{
				Message i = new Message(122);
				i.WriteByte(91);
				lock (GiaTocManager.ListGiaTocs)
				{
					i.WriteShort((short)GiaTocManager.ListGiaTocs.Count);
					foreach (GiaTocTemplate G in GiaTocManager.ListGiaTocs.Values)
					{
						i.WriteUTF(G.NameTocTruong);
						i.WriteInt(G.Level);
						i.WriteInt((G.Exp > 0) ? (G.Exp / GiaTocManager.ExpGiaTocServer[G.Level] * 100) : 0);
						i.WriteInt(G.ThanhViens.Count);
						i.WriteInt(30);
						i.WriteUTF(G.Name);
					}
				}
				_myChar.SendMessage(i);
			}
			else
			{
				ShowGiaTocMe(_myChar);
			}
		}

		public static void AddCongHienBac(Character _myChar, Message msg)
		{
			int CongHien = msg.ReadInt();
			if (_myChar.Inventory.Bac < CongHien)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Bạn không đủ bạc", Util.WHITE));
			}
			else if (_myChar.Info.RoleGiaToc == 5)
			{
				InventoryHander.UpdateBac(_myChar, CongHien, ThongBao: true);
				if ((_myChar.InfoGame.GiaToc.NganSach += CongHien) > int.MaxValue)
				{
					_myChar.InfoGame.GiaToc.NganSach = int.MaxValue;
				}
				ShowGiaTocMe(_myChar);
			}
		}

		public static void RutCongHienBac(Character _myChar, Message msg)
		{
			int BacRut = msg.ReadInt();
			if (_myChar.InfoGame.GiaToc.NganSach < BacRut)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Ngân sách không đủ", Util.WHITE));
			}
			else if (_myChar.Info.RoleGiaToc == 5)
			{
				InventoryHander.AddBac(_myChar, BacRut, ThongBao: true);
				ShowGiaTocMe(_myChar);
			}
		}

		private static void ShowGiaTocMe(Character _myChar)
		{
			GiaTocTemplate giaTocTemplate = _myChar.InfoGame.GiaToc;
			Message j = new Message(122);
			j.WriteByte(54);
			j.WriteUTF(giaTocTemplate.Name);
			j.WriteUTF(giaTocTemplate.Title);
			j.WriteLong(giaTocTemplate.NgayThanhLap);
			j.WriteShort(giaTocTemplate.Level);
			j.WriteInt(giaTocTemplate.Exp);
			j.WriteInt(GiaTocManager.ExpGiaTocServer[giaTocTemplate.Level]);
			j.WriteInt(giaTocTemplate.CongHienTuan);
			j.WriteInt(giaTocTemplate.NganSach);
			j.WriteUTF(giaTocTemplate.Content);
			_myChar.InfoGame.GiaToc.SetNextDayMoCuaAi();
			j.WriteByte(giaTocTemplate.SoLanMoCuaAi);
			j.WriteShort((short)giaTocTemplate.ThanhViens.Count);
			foreach (GiaTocTemplate.ThanhVienGiaToc T in giaTocTemplate.ThanhViens)
			{
				j.WriteByte(T.ChucVu);
				j.WriteByte(T.ClassID);
				j.WriteByte(T.IdChar);
				j.WriteShort(T.Level);
				j.WriteUTF(T.Name);
				j.WriteInt(T.CongHien);
				j.WriteInt(T.CongHienTuan);
				j.WriteBool(T.isOn);
				j.WriteBool(T.IsLockChat);
			}
			j.WriteShort((short)giaTocTemplate.LogGiaTocs.Count);
			foreach (string u in giaTocTemplate.LogGiaTocs)
			{
				j.WriteUTF(u);
			}
			j.WriteShort((short)giaTocTemplate.ItemGiaToc.Count);
			foreach (Item i in giaTocTemplate.ItemGiaToc)
			{
				ItemHander.WriteItem(j, i);
			}
			sbyte[] Size = giaTocTemplate.SkillGiaToc.Where((sbyte s) => s != -1).ToArray();
			j.WriteByte((sbyte)Size.Length);
			sbyte[] array = Size;
			foreach (sbyte b in array)
			{
				j.WriteByte(b);
			}
			j.WriteLong(Util.CurrentTimeMillis() + 1);
			j.WriteShort(10);
			_myChar.SendMessage(j);
		}

		public static void CreateGiaToc(Character _myChar, Message msg)
		{
			string Name1 = msg.ReadString();
			string Name2 = msg.ReadString();
			if (_myChar.Info.IdGiaToc != -1)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã có gia tộc rồi", Util.WHITE));
			}
			else
			{
				if (LangLa.Server.Server.LockDB)
				{
					return;
				}
				lock (GiaTocManager.ListGiaTocs)
				{
					if (GiaTocManager.ListGiaTocs.Values.Any((GiaTocTemplate s) => s.Name.Equals(Name1)))
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Tên gia tộc đã tồn tại", Util.WHITE));
						return;
					}
					if (Name1.Length <= 5)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Tên gia tộc quá ngắn", Util.WHITE));
						return;
					}
					if (!Util.CheckNameString(Name1))
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Tên gia tộc không được chứa ký tự đặc biệt", Util.WHITE));
						return;
					}
					GiaTocManager.ID_GIA_TOC++;
					GiaTocTemplate G = new GiaTocTemplate((short)GiaTocManager.ID_GIA_TOC, Name1, _myChar);
					Util.ShowLog("ID " + GiaTocManager.ID_GIA_TOC);
					if (ConnectionDB.CreateGiaToc(G) && GiaTocManager.ListGiaTocs.TryAdd(G.ID, G))
					{
						_myChar.Info.IdGiaToc = G.ID;
						_myChar.Info.RoleGiaToc = 5;
						_myChar.SendMessage(UtilMessage.ClearSceen());
						Message i = new Message(-109);
						i.WriteUTF("Xin chúc mừng bạn đã chính thức trở thành tộc trưởng của gia tộc " + Name1);
						i.WriteByte(-1);
						_myChar.SendMessage(i);
						_myChar.InfoGame.GiaToc = G;
						InventoryHander.RemoveQuantityWhereId(_myChar, 1, 301);
					}
				}
			}
		}

		public static void UpdateSkillGiaToc(Character _myChar, Message msg)
		{
			sbyte Index = msg.ReadByte();
			if (_myChar.Info.IdGiaToc == -1 || _myChar.Info.RoleGiaToc != 5)
			{
				return;
			}
			SkillClanTemplate skillClan = DataServer.ArrSkillClan[Index];
			if (skillClan == null || _myChar.InfoGame.GiaToc.Level < skillClan.LevelNeed || _myChar.InfoGame.GiaToc.NganSach < skillClan.MoneyBuy || _myChar.InfoGame.GiaToc.SkillGiaToc[Index] != -1)
			{
				return;
			}
			_myChar.InfoGame.GiaToc.SkillGiaToc[Index] = Index;
			_myChar.InfoGame.GiaToc.NganSach -= skillClan.MoneyBuy;
			string Text = "Tộc trưởng " + _myChar.Info.Name + " khai mờ chiêu " + skillClan.Name + ",giảm " + skillClan.MoneyBuy + " bạc của ngân sách";
			_myChar.InfoGame.GiaToc.AddLogGiaToc(Text);
			foreach (GiaTocTemplate.ThanhVienGiaToc b in _myChar.InfoGame.GiaToc.ThanhViens)
			{
				if (b._myChar != null && b._myChar.IsConnection)
				{
					b._myChar.SendMessage(UtilMessage.SendThongBao(Text, Util.YELLOW_MID));
					b._myChar.TuongKhac.GetPointFromSkillGiaToc(b._myChar, skillClan.Options);
				}
			}
			ShowGiaTocMe(_myChar);
		}

		public static void ChangeRole(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			sbyte Role = msg.ReadByte();
		}

		public static void PhanPhatItem(Character _myChar, Message msg)
		{
			short Index = msg.ReadShort();
			if (_myChar.InfoGame.GiaToc.ItemGiaToc[Index] == null)
			{
				return;
			}
			short Id = _myChar.InfoGame.GiaToc.ItemGiaToc[Index].Id;
			int Quantity = _myChar.InfoGame.GiaToc.ItemGiaToc[Index].Quantity;
			string Text = "Tộc trưởng " + _myChar.Info.Name + " đã phân phát " + DataServer.ArrItemTemplate[Id].name + " cho toàn bộ thành viên gia tộc";
			_myChar.InfoGame.GiaToc.AddLogGiaToc(Text);
			foreach (GiaTocTemplate.ThanhVienGiaToc c in _myChar.InfoGame.GiaToc.ThanhViens)
			{
				if (c._myChar != null && c._myChar.IsConnection)
				{
					Item item = new Item(Id, IsLock: true);
					item.Quantity = Quantity;
					InfoThu infoThu = new InfoThu(item);
					infoThu.Title = "Hệ thống";
					infoThu.NameSender = "Vật phẩm gia tộc";
					infoThu.Content = "";
					infoThu.TimeEnd = 90000L;
					c._myChar.Thus.Add(infoThu);
					c._myChar.SendMessage(UtilMessage.SendThongBao(Text, Util.YELLOW_MID));
					ThuHander.ReloadThu(c._myChar);
				}
			}
			_myChar.InfoGame.GiaToc.ItemGiaToc.RemoveAt(Index);
			ShowGiaTocMe(_myChar);
		}

		public static void PhatLuongGiaToc(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			int BacPhat = msg.ReadInt();
			if (_myChar.InfoGame.GiaToc.NganSach < BacPhat)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Ngân sách không đủ", Util.YELLOW_MID));
				return;
			}
			byte Index = (byte)_myChar.InfoGame.GiaToc.ThanhViens.FindIndex((GiaTocTemplate.ThanhVienGiaToc s) => s.Name.Equals(Name));
			if (Index != -1)
			{
				Character Char = _myChar.InfoGame.GiaToc.ThanhViens[Index]._myChar;
				if (Char == null)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Người chơi không online", Util.YELLOW_MID));
					return;
				}
				_myChar.SendMessage(UtilMessage.SendThongBao("Gửi thành công", Util.YELLOW_MID));
				InventoryHander.AddBac(Char, BacPhat, ThongBao: true);
				Char.SendMessage(UtilMessage.SendThongBao("Bạn nhận được tiền lương từ gia tộc", Util.WHITE));
			}
		}

		public static void KickThanhVien(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			byte Index = (byte)_myChar.InfoGame.GiaToc.ThanhViens.FindIndex((GiaTocTemplate.ThanhVienGiaToc s) => s.Name.Equals(Name));
			if (Index == -1)
			{
				return;
			}
			string Text = "Tộc trưởng " + _myChar.Info.Name + " đã loại thành viên " + Name + " ra khỏi gia tộc";
			Character Char = _myChar.InfoGame.GiaToc.ThanhViens[Index]._myChar;
			_myChar.InfoGame.GiaToc.ThanhViens.RemoveAt(Index);
			_myChar.InfoGame.GiaToc.AddLogGiaToc(Text);
			ShowGiaTocMe(_myChar);
			if (Char != null)
			{
				Char.Info.IdGiaToc = -1;
				Char.Info.RoleGiaToc = -1;
				Char.InfoGame.GiaToc = null;
				foreach (GiaTocTemplate.ThanhVienGiaToc c in _myChar.InfoGame.GiaToc.ThanhViens)
				{
					if (c._myChar != null && c._myChar.IsConnection)
					{
						c._myChar.SendMessage(UtilMessage.SendThongBao(Text, Util.YELLOW_MID));
					}
				}
				sbyte[] skillGiaToc = _myChar.InfoGame.GiaToc.SkillGiaToc;
				foreach (sbyte s2 in skillGiaToc)
				{
					if (s2 != -1)
					{
						Char.TuongKhac.GetPointFromSkillGiaToc(Char, DataServer.ArrSkillClan[s2].Options, IsDownPoint: true);
					}
				}
				Char.SendMessage(UtilMessage.SendThongBao("Bạn đã bị loại khỏi gia tộc", Util.YELLOW_MID));
			}
			else
			{
				ConnectionDB.UpdateRoleThanhVien(Name);
			}
		}

		public static void BlockChat(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			byte Index = (byte)_myChar.InfoGame.GiaToc.ThanhViens.FindIndex((GiaTocTemplate.ThanhVienGiaToc s) => s.Name.Equals(Name));
			if (Index != -1)
			{
				bool IsLock = (_myChar.InfoGame.GiaToc.ThanhViens[Index].IsLockChat = !_myChar.InfoGame.GiaToc.ThanhViens[Index].IsLockChat);
				ShowGiaTocMe(_myChar);
				Character Char = _myChar.InfoGame.GiaToc.ThanhViens[Index]._myChar;
				if (Char != null && Char.IsConnection)
				{
					string Text = "Chat gia tộc đã được mở";
					Char.SendMessage(UtilMessage.SendThongBao(_myChar.InfoGame.GiaToc.ThanhViens[Index].IsLockChat ? "Bạn đã bị cấm chat gia tộc" : Text, Util.YELLOW_MID));
				}
			}
		}

		public static void HuyGiaToc(Character _myChar)
		{
			GiaTocTemplate Giatoc = _myChar.InfoGame.GiaToc;
			if (!GiaTocManager.ListGiaTocs.TryRemove(Giatoc.ID, out Giatoc) || !ConnectionDB.DeleteGiaToc(Giatoc.ID))
			{
				return;
			}
			sbyte[] skillGiaToc = Giatoc.SkillGiaToc;
			foreach (sbyte s in skillGiaToc)
			{
				if (s != -1)
				{
					_myChar.TuongKhac.GetPointFromSkillGiaToc(_myChar, DataServer.ArrSkillClan[s].Options, IsDownPoint: true);
				}
			}
			_myChar.Info.IdGiaToc = -1;
			_myChar.Info.RoleGiaToc = -1;
			_myChar.InfoGame.GiaToc = null;
			InventoryHander.UpdateVang(_myChar, 190, IsThongBao: true);
			_myChar.SendMessage(UtilMessage.ClearSceen());
			_myChar.SendMessage(UtilMessage.SendThongBao("Xóa bỏ gia tộc thành công", Util.YELLOW_MID));
		}

		public static void SendThongBao(Character _myChar, Message msg)
		{
			string Text = msg.ReadString();
			foreach (GiaTocTemplate.ThanhVienGiaToc c in _myChar.InfoGame.GiaToc.ThanhViens)
			{
				if (c._myChar != null && c._myChar.IsConnection)
				{
					c._myChar.SendMessage(UtilMessage.SendThongBao(Text, Util.YELLOW));
				}
			}
		}

		public static Message SendGiaTocMsg(Character _myChar)
		{
			Message i = new Message(-88);
			i.WriteInt(_myChar.Info.IdUser);
			i.WriteUTF(_myChar.InfoGame.GiaToc.Name);
			i.WriteUTF("");
			i.WriteByte(_myChar.Info.RoleGiaToc);
			return i;
		}

		public static void ShowTabCreateGiaToc(Character _myChar)
		{
			Message i = new Message(122);
			i.WriteByte(53);
			_myChar.SendMessage(i);
		}
	}
}
