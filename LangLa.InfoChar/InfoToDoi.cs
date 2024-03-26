using System.Collections.Generic;
using LangLa.Hander;
using LangLa.IO;
using LangLa.OOP;

namespace LangLa.InfoChar
{
	public class InfoToDoi
	{
		public int Id;

		public int IdDoiTtruong;

		public bool IsLockNhom = true;

		public int IdCharDoiTuong;

		public string? NameDoiTruong;

		public List<Character>? Chars;

		public List<string>? MessageChat;

		private bool IsClose;

		public void CleanUp()
		{
			IsClose = true;
			Chars?.Clear();
			Chars = null;
			MessageChat?.Clear();
			MessageChat = null;
		}

		public void Remove(Character _myChar)
		{
			Character _myChar2 = _myChar;
			if (_myChar2.InfoGame.IsDoiTruong)
			{
				Chars.RemoveAt(0);
				if (Chars.Count > 0)
				{
					Chars[0].InfoGame.IsDoiTruong = true;
				}
				else
				{
					CleanUp();
				}
			}
			else
			{
				Chars.RemoveAt(Chars.FindIndex((Character s) => s.Info.IdUser == _myChar2.Info.IdUser));
				if (Chars.Count > 0)
				{
					Chars[0].InfoGame.IsDoiTruong = true;
				}
				else
				{
					CleanUp();
				}
			}
			if (IsClose)
			{
				return;
			}
			foreach (Character c in Chars)
			{
				if (_myChar2.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi > 0)
				{
					c.InfoGame.ToDoiKinhNghiem -= _myChar2.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi;
				}
				ToDoiHander.ToDoiForMe(c);
				c.SendMessage(UtilMessage.SendThongBao("Người chơi " + _myChar2.Info.Name + " đã rời khỏi tổ đội", Util.WHITE));
			}
		}

		public InfoToDoi(Character _myChar)
		{
			_myChar.InfoGame.IsDoiTruong = true;
			NameDoiTruong = _myChar.Info.Name;
			IdCharDoiTuong = _myChar.Info.IdUser;
			Chars = new List<Character>();
			MessageChat = new List<string>();
			Chars.Add(_myChar);
		}
	}
}
