using System;
using System.Linq;
using LangLa.IO;
using LangLa.OOP;

namespace LangLa.Hander
{
	public static class PvPHander
	{
		public static void ChangeTypePk(Character _myChar, Message msg)
		{
			int ID = _myChar.Info.IdUser;
			sbyte TypePk = msg.ReadByte();
			foreach (Character Char in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (Char.IsConnection)
				{
					Message i = new Message(-15);
					i.WriteInt(ID);
					i.WriteByte(TypePk);
					Char.SendMessage(i);
				}
			}
			_myChar.InfoGame.TypePk = TypePk;
		}

		public static void CuuSat(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character _cCuuSat = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (_cCuuSat != null)
			{
				if (Math.Abs(_cCuuSat.Info.Level - _myChar.Info.Level) > 10)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Chênh lệnh cấp độ với đối thủ", Util.WHITE));
					return;
				}
				_cCuuSat.SendMessage(MsgCuuSat(_myChar.Info.IdUser, _cCuuSat.Info.IdUser));
				_cCuuSat.InfoGame.IsAnCuuSat = true;
				_cCuuSat.InfoGame.IdCharMoiCuuSat = _myChar.Info.IdUser;
				_myChar.SendMessage(MsgCuuSat(_myChar.Info.IdUser, _cCuuSat.Info.IdUser));
				_myChar.InfoGame.IsCuuSat = true;
				_myChar.InfoGame.IdCuuSat = _cCuuSat.Info.IdUser;
			}
		}

		private static Message MsgCuuSat(int IdMyChar, int IdCuuSat)
		{
			Message i = new Message(19);
			i.WriteInt(IdMyChar);
			i.WriteInt(IdCuuSat);
			return i;
		}

		private static Message MsgMoiTyVo(string Name)
		{
			Message i = new Message(32);
			i.WriteUTF(Name);
			return i;
		}

		public static Message MsgCloseCuuSat(int Id, bool IsCloseCuuSat)
		{
			Message i = new Message(18);
			i.WriteInt(Id);
			i.WriteBool(IsCloseCuuSat);
			return i;
		}

		public static void TyVo(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character _cTyVo = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (_cTyVo != null)
			{
				if (Math.Abs(_cTyVo.Info.Level - _myChar.Info.Level) > 10)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Chênh lệnh cấp độ với đối thủ", Util.WHITE));
					return;
				}
				_myChar.SendMessage(UtilMessage.SendThongBao("Đã gửi lời mời tỷ vỏ đến " + Name, Util.WHITE));
				_cTyVo.SendMessage(MsgMoiTyVo(_myChar.Info.Name));
			}
		}

		private static Message MsgStartTyVo(int IdChar, int IdTyVo)
		{
			Message i = new Message(31);
			i.WriteInt(IdChar);
			i.WriteByte(1);
			i.WriteInt(IdTyVo);
			i.WriteByte(1);
			return i;
		}

		public static Message MsgCloseTyVo(sbyte Type, int Id1, int Id2)
		{
			Message i = new Message(29);
			i.WriteByte(Type);
			i.WriteInt(Id1);
			i.WriteInt(Id2);
			return i;
		}

		public static void StartTyVo(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character _cTyVo = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (_cTyVo == null || Math.Abs(_cTyVo.Info.Level - _myChar.Info.Level) > 10)
			{
				return;
			}
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(MsgStartTyVo(_cTyVo.Info.IdUser, _myChar.Info.IdUser));
					c.SendMessage(MsgStartTyVo(_myChar.Info.IdUser, _cTyVo.Info.IdUser));
				}
			}
			_cTyVo.InfoGame.IsTyVo = true;
			_cTyVo.InfoGame.IdTyVo = _myChar.Info.IdUser;
			_myChar.InfoGame.IsTyVo = true;
			_myChar.InfoGame.IdTyVo = _cTyVo.Info.IdUser;
		}

		public static void TuChoiTyVo(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			_myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name))?.SendMessage(UtilMessage.SendThongBao(_myChar.Info.Name + " đã từ chối tỷ võ", Util.WHITE));
		}
	}
}
