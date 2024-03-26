using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class EffHander
	{
		public static void AddEff(Character _myChar, short idItem, sbyte TypeItem)
		{
			sbyte Ideff = -1;
			if (idItem >= 12 && idItem <= 16)
			{
				Ideff = (sbyte)(idItem + 1);
			}
			else if (idItem >= 17 && idItem <= 21)
			{
				Ideff = (sbyte)(idItem + 3);
			}
			else if (idItem == 219 || idItem == 220)
			{
				Ideff = (sbyte)(idItem - 201);
			}
			else if (idItem == 221 || idItem == 222)
			{
				Ideff = (sbyte)(idItem - 196);
			}
			else if (idItem >= 22 && idItem <= 27)
			{
				Ideff = (sbyte)(idItem - 22);
			}
			else if (idItem == 238 || idItem == 239)
			{
				Ideff = (sbyte)(idItem - 232);
			}
			else if (idItem == 240 || idItem == 241 || idItem == 242)
			{
				Ideff = (sbyte)(idItem - 197);
			}
			else if (idItem == 243)
			{
				Ideff = 50;
			}
			else if (idItem == 159 || idItem == 347 || idItem == 281)
			{
				Ideff = 42;
			}
			int Time = -1;
			int Value = -1;
			if (idItem == -1)
			{
				return;
			}
			Time = ((TypeItem == 23 || TypeItem == 22) ? 3000 : 1800000);
			switch (idItem)
			{
			case 159:
				Time = 18000000;
				Value = 50;
				_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += (short)Value;
				break;
			case 281:
				Time = 18000000;
				Value = 76;
				_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += (short)Value;
				break;
			case 347:
				Time = 18000000;
				Value = 100;
				_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai += (short)Value;
				break;
			}
			InfoEff Eff2 = null;
			foreach (InfoEff e in _myChar.Effs)
			{
				if (e.Id == Ideff)
				{
					e.TimeEnd += Time;
					if (e.TimeEnd >= int.MaxValue)
					{
						e.TimeEnd = int.MaxValue;
					}
					Eff2 = e;
					break;
				}
			}
			InfoEff infoEff = null;
			if (Eff2 == null)
			{
				infoEff = new InfoEff(Ideff, Time, Value, idItem);
				_myChar.Effs.Add(infoEff);
			}
			int Id = _myChar.Info.IdUser;
			int Vluae = infoEff?.Value ?? Eff2.Value;
			if (TypeItem != 22 && TypeItem != 23 && TypeItem != 24)
			{
				foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
				{
					c.SendMessage(UtilMessage.MsgSendEff(Id, Ideff, Vluae, Time));
				}
				return;
			}
			_myChar.SendMessage(UtilMessage.MsgSendEff(Id, Ideff, Vluae, Time));
		}

		public static void AddEffNotFocus(Character _myChar, short Id, short Value, int Seconds, bool IsSendAll = true)
		{
			bool IsAdd = true;
			for (int i = 0; i < _myChar.Effs.Count; i++)
			{
				if (_myChar.Effs[i].Id == Id)
				{
					_myChar.Effs[i].TimeStart = Util.CurrentTimeMillis();
					IsAdd = false;
					break;
				}
			}
			if (IsAdd)
			{
				InfoEff infoEff = new InfoEff(Id, Seconds, Value, -1);
				int IdC = _myChar.Info.IdUser;
				infoEff.IsAutoRemove = true;
				if (Id == 75 || Id == 12)
				{
					_myChar.InfoGame.IsChoang = true;
				}
				_myChar.Effs.Add(infoEff);
				if (IsSendAll)
				{
					foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
					{
						if (c.IsConnection)
						{
							c.SendMessage(UtilMessage.MsgSendEff(IdC, Id, Value, Seconds));
						}
					}
					return;
				}
				_myChar.SendMessage(UtilMessage.MsgSendEff(IdC, Id, Value, Seconds));
				return;
			}
			foreach (Character c2 in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c2.IsConnection)
				{
					c2.SendMessage(UtilMessage.MsgSendEff(_myChar.Info.IdUser, Id, Value, Seconds));
				}
			}
		}

		public static bool AddTimeEff(Character _myChar, InfoEff infoEff)
		{
			int Time = -1;
			short idItem = infoEff.IdItem;
			short num = idItem;
			if (num == 159 || num == 281 || num == 347)
			{
				Time = 18000000;
				infoEff.TimeEnd += Time;
			}
			if (infoEff.TimeEnd > 86400000)
			{
				infoEff.TimeEnd = 86400000;
			}
			if (Time != -1)
			{
				_myChar.SendMessage(UtilMessage.MsgSendEff(_myChar.Info.IdUser, infoEff.Id, infoEff.Value, infoEff.TimeEnd));
				return true;
			}
			return false;
		}

		public static void AddEffGayChoang(Character _myChar, Character _cAnDame, int Value)
		{
			if (_cAnDame.InfoGame.IsUseByaKugan)
			{
				return;
			}
			Value += _myChar.TuongKhac.GetPointGayChoang(_myChar);
			Value -= _cAnDame.TuongKhac.GetPointGiamGayChoang(_cAnDame);
			if (Util.NextInt(0, 1000) >= Value)
			{
				return;
			}
			int Time = ((Value < 150) ? 3000 : ((Value >= 150 && Value < 300) ? 5000 : 10000));
			Time = Util.NextInt(3000, Time);
			bool IsAdd = true;
			for (int i = 0; i < _cAnDame.Effs.Count; i++)
			{
				if (_cAnDame.Effs[i].Id != 12)
				{
					continue;
				}
				_cAnDame.Effs[i].TimeStart = Util.CurrentTimeMillis();
				IsAdd = false;
				foreach (Character c2 in _cAnDame.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 12, Value, Time));
					}
				}
				break;
			}
			if (!IsAdd)
			{
				return;
			}
			InfoEff infoEff = new InfoEff(12, Time, Value, -1);
			infoEff.IsAutoRemove = true;
			_cAnDame.Effs.Add(infoEff);
			foreach (Character c in _cAnDame.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 12, Value, Time));
				}
			}
		}

		public static void AddEffGayGaySuyYeu(Character _myChar, Character _cAnDame, int Value)
		{
			if (_cAnDame.InfoGame.IsUseByaKugan)
			{
				return;
			}
			Value += _myChar.TuongKhac.GetPointSuyYeu(_myChar);
			Value -= _cAnDame.TuongKhac.GetPointGiamSuyYeu(_cAnDame);
			if (Util.NextInt(0, 1000) >= Value || Value <= 0)
			{
				return;
			}
			int Time = ((Value < 150) ? 3000 : ((Value >= 150 && Value < 300) ? 5000 : 10000));
			Time = Util.NextInt(3000, Time);
			bool IsAdd = true;
			for (int i = 0; i < _cAnDame.Effs.Count; i++)
			{
				if (_cAnDame.Effs[i].Id != 8)
				{
					continue;
				}
				_cAnDame.Effs[i].TimeStart = Util.CurrentTimeMillis();
				IsAdd = false;
				foreach (Character c2 in _cAnDame.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 8, Value, Time));
					}
				}
				break;
			}
			if (!IsAdd)
			{
				return;
			}
			_cAnDame.InfoGame.IsSuyYeu = true;
			_cAnDame.WriteInfo();
			InfoEff infoEff = new InfoEff(8, Time, Value, -1);
			infoEff.IsAutoRemove = true;
			_cAnDame.Effs.Add(infoEff);
			foreach (Character c in _cAnDame.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 8, Value, Time));
				}
			}
		}

		public static void AddEffGayTrungDoc(Character _myChar, Character _cAnDame, int Value)
		{
			if (_cAnDame.InfoGame.IsUseByaKugan)
			{
				return;
			}
			Value += _myChar.TuongKhac.GetPointTrungDoc(_myChar);
			Value -= _cAnDame.TuongKhac.GetPointGiamTrungDoc(_cAnDame);
			if (Util.NextInt(0, 1000) >= Value || Value <= 0)
			{
				return;
			}
			int Time = ((Value < 150) ? 3000 : ((Value >= 150 && Value < 300) ? 5000 : 10000));
			Time = Util.NextInt(3000, Time);
			bool IsAdd = true;
			for (int i = 0; i < _cAnDame.Effs.Count; i++)
			{
				if (_cAnDame.Effs[i].Id != 9)
				{
					continue;
				}
				_cAnDame.Effs[i].TimeStart = Util.CurrentTimeMillis();
				IsAdd = false;
				foreach (Character c2 in _cAnDame.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 9, Value, Time));
					}
				}
				break;
			}
			if (!IsAdd)
			{
				return;
			}
			InfoEff infoEff = new InfoEff(9, Time, Value, -1);
			infoEff.IsAutoRemove = true;
			_cAnDame.Effs.Add(infoEff);
			foreach (Character c in _cAnDame.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 9, Value, Time));
				}
			}
		}

		public static void AddEffGayLamCham(Character _myChar, Character _cAnDame, int Value)
		{
			if (_cAnDame.InfoGame.IsUseByaKugan)
			{
				return;
			}
			Value += _myChar.TuongKhac.GetPointLamCham(_myChar);
			Value -= _cAnDame.TuongKhac.GetPointGiamLamCham(_cAnDame);
			if (Util.NextInt(0, 1000) >= Value || Value <= 0)
			{
				return;
			}
			int Time = ((Value < 150) ? 3000 : ((Value >= 150 && Value < 300) ? 5000 : 10000));
			Time = Util.NextInt(3000, Time);
			bool IsAdd = true;
			for (int i = 0; i < _cAnDame.Effs.Count; i++)
			{
				if (_cAnDame.Effs[i].Id != 10)
				{
					continue;
				}
				_cAnDame.Effs[i].TimeStart = Util.CurrentTimeMillis();
				IsAdd = false;
				foreach (Character c2 in _cAnDame.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 10, Value, Time));
					}
				}
				break;
			}
			if (!IsAdd)
			{
				return;
			}
			if (Value > 32767 || Value < 0)
			{
				Value = 32767;
			}
			_cAnDame.InfoGame.Speed = (short)(_cAnDame.Info.Speed / 2);
			_cAnDame.InfoGame.IsLamCham = true;
			InfoEff infoEff = new InfoEff(10, Time, Value, -1);
			infoEff.IsAutoRemove = true;
			_cAnDame.Effs.Add(infoEff);
			short Speed = _cAnDame.TuongKhac.GetSpeedChar(_cAnDame);
			foreach (Character c in _cAnDame.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 10, Value, Time));
					c.SendMessage(UtilMessage.UpdatePointMore(_cAnDame.Info.IdUser, _myChar.InfoGame.LevelPk, _myChar.Info.TaiPhu, Speed, _myChar.InfoGame.StatusGD));
				}
			}
		}

		public static void AddEffGayBong(Character _myChar, Character _cAnDame, int Value)
		{
			if (_cAnDame.InfoGame.IsUseByaKugan)
			{
				return;
			}
			Value += _myChar.TuongKhac.GetPointGayBong(_myChar);
			Value -= _cAnDame.TuongKhac.GetPointGiamGayBong(_cAnDame);
			if (Util.NextInt(0, 1000) >= Value || Value <= 0)
			{
				return;
			}
			int Time = ((Value < 150) ? 3000 : ((Value >= 150 && Value < 300) ? 5000 : 10000));
			Time = Util.NextInt(3000, Time);
			bool IsAdd = true;
			for (int i = 0; i < _cAnDame.Effs.Count; i++)
			{
				if (_cAnDame.Effs[i].Id != 11)
				{
					continue;
				}
				_cAnDame.Effs[i].TimeStart = Util.CurrentTimeMillis();
				IsAdd = false;
				foreach (Character c2 in _cAnDame.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 11, Value, Time));
					}
				}
				break;
			}
			if (!IsAdd)
			{
				return;
			}
			_cAnDame.InfoGame.IsBong = true;
			InfoEff infoEff = new InfoEff(11, Time, Value, -1);
			infoEff.IsAutoRemove = true;
			_cAnDame.Effs.Add(infoEff);
			foreach (Character c in _cAnDame.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgSendEff(_cAnDame.Info.IdUser, 11, Value, Time));
				}
			}
		}

		public static void AddEffGayBongMob(Character _myChar, Mob mob, int Value, int Dame)
		{
			Value += _myChar.TuongKhac.GetPointGayBong(_myChar);
			if (Util.NextInt(0, 1000) >= Value || Value <= 0)
			{
				return;
			}
			int Time = ((Value < 150) ? 3000 : ((Value >= 150 && Value < 300) ? 5000 : 10000));
			Time = Util.NextInt(3000, Time);
			bool IsAdd = true;
			for (int i = 0; i < mob.Effs.Count; i++)
			{
				if (mob.Effs[i].Id != 11)
				{
					continue;
				}
				mob.Effs[i].TimeStart = Util.CurrentTimeMillis();
				IsAdd = false;
				foreach (Character c2 in _myChar.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.MsgSendEffMob(mob.IdEntity, 11, 0, Time));
					}
				}
				break;
			}
			if (!IsAdd)
			{
				return;
			}
			InfoEff infoEff = new InfoEff(11, Time, 0, -1);
			infoEff.IsAutoRemove = true;
			mob.IsBong = true;
			mob.Effs.Add(infoEff);
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgSendEffMob(mob.IdEntity, 11, 0, Time));
				}
			}
		}

		public static void AddEffGayChoangMob(Character _myChar, Mob mob, int Value, int Dame)
		{
		}

		public static void AddEffGayLamChamMob(Character _myChar, Mob mob, int Value, int Dame)
		{
		}

		public static void AddEffGaySuyYeuMob(Character _myChar, Mob mob, int Value, int Dame)
		{
		}

		public static void AddEffGayTrungDocMob(Character _myChar, Mob mob, int Value, int Dame)
		{
			Value += _myChar.TuongKhac.GetPointTrungDoc(_myChar);
			if (Util.NextInt(0, 1000) >= Value || Value <= 0)
			{
				return;
			}
			int Time = ((Value < 150) ? 3000 : ((Value >= 150 && Value < 300) ? 5000 : 10000));
			Time = Util.NextInt(3000, Time);
			bool IsAdd = true;
			for (int i = 0; i < mob.Effs.Count; i++)
			{
				if (mob.Effs[i].Id != 9)
				{
					continue;
				}
				mob.Effs[i].TimeStart = Util.CurrentTimeMillis();
				IsAdd = false;
				foreach (Character c2 in _myChar.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.MsgSendEffMob(mob.IdEntity, 9, Dame / 2, Time));
					}
				}
				break;
			}
			if (!IsAdd)
			{
				return;
			}
			InfoEff infoEff = new InfoEff(9, Time, Dame / 2, -1);
			infoEff.IsAutoRemove = true;
			mob.IsTrungDoc = true;
			mob.Effs.Add(infoEff);
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgSendEffMob(mob.IdEntity, 9, Dame / 2, Time));
				}
			}
		}

		public static void AddEffMob(Mob mob, InfoEff infoEff)
		{
			bool IsAdd = true;
			foreach (InfoEff E in mob.Effs)
			{
				if (E.Id == infoEff.Id)
				{
					E.TimeStart = Util.CurrentTimeMillis();
					IsAdd = false;
					break;
				}
			}
			foreach (Character c in mob._Zone.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgSendEffMob(mob.IdEntity, infoEff.Id, infoEff.Value, infoEff.TimeEnd));
				}
			}
			if (IsAdd)
			{
				mob.Effs.Add(infoEff);
			}
		}

		public static void RemoveEffMob(Mob mob, InfoEff infoEff)
		{
			short IdMob = mob.IdEntity;
			short IdEff = infoEff.Id;
			mob.Effs.Remove(infoEff);
			foreach (Character c in mob._Zone.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgRemoveEffMob(IdMob, IdEff));
				}
			}
		}

		public static void RemoveEff(Character _myChar, InfoEff infoEff)
		{
			int IdChar = _myChar.Info.IdUser;
			if (infoEff == null)
			{
				return;
			}
			short Id = infoEff.Id;
			_myChar.Effs.Remove(infoEff);
			if (infoEff.Type == 0 || infoEff.Type == 6 || infoEff.Type == 7 || infoEff.Type == 23)
			{
				if (infoEff.Type == 23)
				{
					_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai -= (short)infoEff.Value;
				}
				_myChar.SendMessage(UtilMessage.MsgRemoveEff(IdChar, Id));
				return;
			}
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.MsgRemoveEff(IdChar, Id));
				}
			}
		}
	}
}
