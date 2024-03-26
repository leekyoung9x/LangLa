using System.Collections.Generic;
using System.Linq;
using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;

namespace LangLa.Hander
{
	public static class ToDoiHander
	{
		public static void CreateToDoi(Character _myChar, Message msg)
		{
			string Name = "";
			while (msg.available() > 0)
			{
				Name = msg.ReadString();
			}
			if (!Name.Equals(""))
			{
				if (_myChar.InfoGame.Todoi == null)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn cần phải tạo tổ đội trước khi mời người khác", Util.WHITE));
					return;
				}
				Character CharOder = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
				if (CharOder != null)
				{
					if (CharOder.InfoGame.Todoi != null)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Người chơi " + Name + " đã có tổ đội", Util.WHITE));
						return;
					}
					Message i = new Message(41);
					i.WriteUTF(_myChar.Info.Name);
					CharOder.SendMessage(i);
					_myChar.SendMessage(UtilMessage.SendThongBao("Đã gửi lời mời đến " + Name, Util.WHITE));
				}
			}
			else if (_myChar.InfoGame.Todoi == null)
			{
				_myChar.InfoGame.Todoi = new InfoToDoi(_myChar);
				ToDoiForMe(_myChar);
			}
		}

		public static void ToDoiForMe(Character _myChar)
		{
			Message j = new Message(43);
			j.WriteBool(_myChar.InfoGame.Todoi.IsLockNhom);
			j.WriteByte((sbyte)_myChar.InfoGame.Todoi.Chars.Count);
			for (sbyte i = 0; i < _myChar.InfoGame.Todoi.Chars.Count; i++)
			{
				j.WriteByte(i);
				j.WriteByte(_myChar.InfoGame.Todoi.Chars[i].Info.IdChar);
				j.WriteShort(DataServer.ArrIconChar[_myChar.InfoGame.Todoi.Chars[i].Info.IdChar]);
				j.WriteUTF(_myChar.InfoGame.Todoi.Chars[i].Info.Name);
			}
			_myChar.SendMessage(j);
		}

		public static void ChapNhanVaoDoi(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character CharOder = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (CharOder == null || CharOder.InfoGame.Todoi == null)
			{
				return;
			}
			_myChar.InfoGame.Todoi = CharOder.InfoGame.Todoi;
			CharOder.InfoGame.Todoi.Chars.Add(_myChar);
			_myChar.SendMessage(UtilMessage.SendThongBao("Vào tổ đội của " + Name + " thành công", Util.WHITE));
			CharOder.SendMessage(UtilMessage.SendThongBao(_myChar.Info.Name + " đã chấp nhận vào đội của bạn ", Util.WHITE));
			foreach (Character c in CharOder.InfoGame.Todoi.Chars)
			{
				if (c.IsConnection)
				{
					if (_myChar.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi > 0 && _myChar.Info.IdUser != c.Info.IdUser)
					{
						c.InfoGame.ToDoiKinhNghiem += _myChar.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi;
					}
					if (c.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi > 0 && _myChar.Info.IdUser != c.Info.IdUser)
					{
						_myChar.InfoGame.ToDoiKinhNghiem += c.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi;
					}
					ToDoiForMe(c);
				}
			}
		}

		public static void ChatNhom(Character _myChar, Message msg)
		{
			string Text = msg.ReadString();
			string Name = _myChar.Info.Name;
			foreach (Character c in _myChar.InfoGame.Todoi.Chars)
			{
				if (c.IsConnection)
				{
					Message i = new Message(26);
					i.WriteUTF(Name);
					i.WriteUTF(Text);
					c.SendMessage(i);
				}
			}
		}

		public static void TuChoiVaoDoi(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			_myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name))?.SendMessage(UtilMessage.SendThongBao("Người chơi " + _myChar.Info.Name + " từ chối lời mời vào đội", Util.YELLOW_MID));
		}

		public static void ShowListTodoi(Character _myChar)
		{
			List<Character> ListToDoi = _myChar.InfoGame.ZoneGame.Chars.Values.Where((Character s) => s.IsConnection && s.InfoGame.IsDoiTruong).ToList();
			if (ListToDoi == null || ListToDoi.Count <= 0)
			{
				return;
			}
			Message i = new Message(45);
			i.WriteByte((sbyte)ListToDoi.Count);
			foreach (Character c in ListToDoi)
			{
				i.WriteBool(c.InfoGame.Todoi.IsLockNhom);
				i.WriteByte((sbyte)c.InfoGame.Todoi.Chars.Count);
				i.WriteByte(c.Info.IdChar);
				i.WriteByte(c.Info.IdChar);
				i.WriteShort(1);
				i.WriteUTF(c.InfoGame.Todoi.NameDoiTruong);
			}
			_myChar.SendMessage(i);
		}

		public static void NhuongDoiTruong(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character CharOder = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (CharOder == null)
			{
				return;
			}
			sbyte Index = (sbyte)_myChar.InfoGame.Todoi.Chars.FindIndex((Character s) => s.Info.IdUser == CharOder.Info.IdUser);
			_myChar.InfoGame.Todoi.Chars[Index] = _myChar;
			_myChar.InfoGame.Todoi.Chars[0] = CharOder;
			_myChar.InfoGame.IsDoiTruong = false;
			CharOder.InfoGame.IsDoiTruong = true;
			CharOder.SendMessage(UtilMessage.SendThongBao(_myChar.Info.Name + " Đã nhường nhóm trưởng cho bạn", Util.WHITE));
			foreach (Character c in _myChar.InfoGame.Todoi.Chars)
			{
				ToDoiForMe(c);
			}
		}

		public static void KichMember(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character cKich = _myChar.InfoGame.Todoi.Chars.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (cKich == null)
			{
				return;
			}
			_myChar.InfoGame.Todoi.Chars.RemoveAt(_myChar.InfoGame.Todoi.Chars.FindIndex((Character s) => s.Info.IdUser == cKich.Info.IdUser));
			cKich.InfoGame.CleanUpToDoi();
			Message i = new Message(44);
			i.WriteByte(0);
			cKich.SendMessage(i);
			cKich.SendMessage(UtilMessage.SendThongBao("Bạn đã bị kich khỏi đội", Util.WHITE));
			cKich.InfoGame.ToDoiKinhNghiem = 0;
			foreach (Character c in _myChar.InfoGame.Todoi.Chars)
			{
				if (cKich.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi > 0)
				{
					c.InfoGame.ToDoiKinhNghiem -= cKich.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi;
				}
				ToDoiForMe(c);
			}
		}

		public static void LeaveToDoi(Character _myChar)
		{
			Character _myChar2 = _myChar;
			_myChar2.InfoGame.Todoi.Chars.RemoveAt(_myChar2.InfoGame.Todoi.Chars.FindIndex((Character s) => s.Info.IdUser == _myChar2.Info.IdUser));
			if (_myChar2.InfoGame.Todoi.Chars.Count > 0)
			{
				foreach (Character c in _myChar2.InfoGame.Todoi.Chars)
				{
					if (_myChar2.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi > 0)
					{
						c.InfoGame.ToDoiKinhNghiem -= _myChar2.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi;
					}
					ToDoiForMe(c);
				}
			}
			else
			{
				_myChar2.InfoGame.Todoi.CleanUp();
			}
			_myChar2.InfoGame.ToDoiKinhNghiem = 0;
			_myChar2.InfoGame.IsDoiTruong = false;
			_myChar2.InfoGame.Todoi = null;
			Message i = new Message(44);
			i.WriteByte(0);
			_myChar2.SendMessage(i);
		}
	}
}
