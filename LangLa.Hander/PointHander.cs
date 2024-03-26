using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class PointHander
	{
		public static void TayTiemNang(Character _myChar)
		{
			lock (_myChar.Point.ArrayPoint)
			{
				short NumAll = 0;
				short NumHp = 0;
				short NumChara = 0;
				short NumMp = 0;
				short NumDame = 0;
				sbyte IdClass = _myChar.Info.IdClass;
				for (int j = 0; j < _myChar.Point.ArrayPoint.Length; j++)
				{
					short NumUp = _myChar.Point.ArrayPoint[j];
					switch (j)
					{
					case 0:
						NumDame = NumUp;
						if (IdClass == 1 || IdClass == 5)
						{
							NumDame -= 10;
						}
						break;
					case 1:
						NumChara = NumUp;
						break;
					case 3:
						NumHp = NumUp;
						NumHp -= 5;
						break;
					case 2:
						NumMp = NumUp;
						NumMp = ((IdClass != 1 && IdClass != 5) ? ((short)(NumMp - 15)) : ((short)(NumMp - 5)));
						break;
					}
					NumAll += NumUp;
				}
				if (NumAll > 20)
				{
					for (int i = 0; i < _myChar.Point.ArrayPoint.Length; i++)
					{
						switch (i)
						{
						case 0:
							_myChar.Point.ArrayPoint[i] -= NumDame;
							break;
						case 1:
							_myChar.Point.ArrayPoint[i] -= NumChara;
							break;
						case 3:
							_myChar.Point.ArrayPoint[i] -= NumHp;
							break;
						case 2:
							_myChar.Point.ArrayPoint[i] -= NumMp;
							break;
						}
					}
					if (NumHp > 0)
					{
						_myChar.Point.HpFull -= NumHp * 9;
						UpdateHpFullChar(_myChar);
					}
					if (NumMp > 0)
					{
						_myChar.Point.MpFull -= NumMp * 9;
						_myChar.SendMessage(UtilMessage.UpdateMpFull_Me(_myChar.Point.GetMpFull(), _myChar.Point.Mp));
						foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
						{
							if (c.IsConnection && c.Info.IdUser != _myChar.Info.IdUser)
							{
								c.SendMessage(UtilMessage.SendMpFullMeInMap(_myChar.Info.IdUser, _myChar.Point.GetMpFull(), _myChar.Point.Mp));
							}
						}
					}
					if (NumChara > 0)
					{
						sbyte PointCrit = _myChar.Point.PointCheckCrit;
						while (NumChara > 0)
						{
							_myChar.Point.HpFull -= 2;
							_myChar.Point.MpFull -= 2;
							_myChar.TuongKhac.DameCoBan--;
							if (PointCrit == 0)
							{
								_myChar.TuongKhac.ChiMang--;
							}
							_myChar.TuongKhac.NeTranh--;
							NumChara--;
							PointCrit--;
							if (PointCrit < 0)
							{
								PointCrit = 2;
							}
						}
						if (_myChar.TuongKhac.ChiMang < 0)
						{
							_myChar.TuongKhac.ChiMang = 0;
						}
						_myChar.Point.PointCheckCrit = 0;
						UpdateHpFullChar(_myChar);
						UpdateMpFullChar(_myChar);
						_myChar.WriteInfo();
					}
					if (NumDame > 0)
					{
						_myChar.TuongKhac.DameCoBan -= NumDame * 2;
						_myChar.WriteInfo();
					}
					_myChar.Point.DiemTiemNang += (short)(NumAll - 20);
					_myChar.UpdateDataChar();
					_myChar.SendMessage(UtilMessage.SendThongBao("Tất cả điểm tiềm năng đã được tẩy sạch", Util.YELLOW_MID));
				}
				else
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn không có điểm tiềm năng nào nào", Util.WHITE));
				}
			}
		}

		public static void TayKyNang(Character _myChar)
		{
			lock (_myChar.Skill.Skills)
			{
				short PointKyNang = 0;
				int j = 0;
				if (_myChar.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi > 0 && _myChar.InfoGame.Todoi != null)
				{
					foreach (Character c2 in _myChar.InfoGame.Todoi.Chars)
					{
						if (c2.IsConnection && c2.Info.IdUser != _myChar.Info.IdUser)
						{
							c2.InfoGame.ToDoiKinhNghiem -= _myChar.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi;
						}
					}
				}
				for (int i = 0; i < _myChar.Skill.Skills.Length; i++)
				{
					Skill skill = _myChar.Skill.Skills[i];
					if (skill.Level <= 0 || i == 0)
					{
						continue;
					}
					j = 0;
					while (skill.Level > 0)
					{
						if (DataServer.ArrSkillTemplate[skill.IdTemplate].Type == 5 && j == 0)
						{
							Skill SkillBackPoint = InfoSkill.GetSkillAndLevel(skill.IdTemplate, skill.Level);
							string[] Value2 = SkillBackPoint.Options.Split(";");
							switch (_myChar.Info.IdClass)
							{
							case 1:
								_myChar.Point.HpFull -= short.Parse(Value2[0].Split(",")[1]);
								_myChar.TuongKhac.PhanTramPhanDon -= short.Parse(Value2[1].Split(",")[1]);
								_myChar.Info.Speed -= short.Parse(Value2[2].Split(",")[1]);
								_myChar.TuongKhac.TileTuongKhac -= short.Parse(Value2[3].Split(",")[1]);
								_myChar.TuongKhac.TangTanCongLenTho -= short.Parse(Value2[3].Split(",")[1]);
								break;
							case 2:
								_myChar.TuongKhac.ChiMang -= short.Parse(Value2[0].Split(",")[1]);
								_myChar.TuongKhac.ChinhXac -= short.Parse(Value2[1].Split(",")[1]);
								_myChar.Info.Speed -= short.Parse(Value2[2].Split(",")[1]);
								_myChar.TuongKhac.TileTuongKhac -= short.Parse(Value2[3].Split(",")[1]);
								_myChar.TuongKhac.TangTanCongLenThuy -= short.Parse(Value2[3].Split(",")[1]);
								break;
							case 3:
							{
								short s1 = short.Parse(Value2[0].Split(",")[1]);
								_myChar.TuongKhac.PhanTramKinhNghiemDanhQuai -= s1;
								_myChar.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi -= short.Parse(Value2[1].Split(",")[1]);
								_myChar.Point.PhanTramHpToiDa -= short.Parse(Value2[2].Split(",")[1]);
								_myChar.Info.Speed -= short.Parse(Value2[3].Split(",")[1]);
								_myChar.TuongKhac.TileTuongKhac -= short.Parse(Value2[4].Split(",")[1]);
								_myChar.TuongKhac.TangTanCongLenHoa -= short.Parse(Value2[4].Split(",")[1]);
								break;
							}
							case 4:
							{
								_myChar.Point.PhanTramMpToiDa -= short.Parse(Value2[0].Split(",")[1]);
								short KhangAll = short.Parse(Value2[1].Split(",")[1]);
								_myChar.TuongKhac.KhangPhong -= KhangAll;
								_myChar.TuongKhac.KhangLoi -= KhangAll;
								_myChar.TuongKhac.KhangTho -= KhangAll;
								_myChar.TuongKhac.KhangThuy -= KhangAll;
								_myChar.TuongKhac.KhangHoa -= KhangAll;
								_myChar.Info.Speed -= short.Parse(Value2[2].Split(",")[1]);
								_myChar.TuongKhac.TileTuongKhac -= short.Parse(Value2[3].Split(",")[1]);
								_myChar.TuongKhac.TangTanCongLenPhong -= short.Parse(Value2[3].Split(",")[1]);
								break;
							}
							case 5:
								_myChar.TuongKhac.DameCoBan -= short.Parse(Value2[0].Split(",")[1]);
								_myChar.TuongKhac.BoQuaNeTranh -= short.Parse(Value2[1].Split(",")[1]);
								_myChar.Info.Speed -= short.Parse(Value2[2].Split(",")[1]);
								_myChar.TuongKhac.TileTuongKhac -= short.Parse(Value2[3].Split(",")[1]);
								_myChar.TuongKhac.TangTanCongLenLoi -= short.Parse(Value2[3].Split(",")[1]);
								break;
							}
						}
						j++;
						_myChar.Skill.Skills[i].Level--;
						if (_myChar.Skill.Skills[i].Level == 0)
						{
							_myChar.Skill.Skills[i] = InfoSkill.GetSkillAndLevel(skill.IdTemplate, 0);
						}
						PointKyNang++;
					}
				}
				if (_myChar.TuongKhac.ChiMang < 0)
				{
					_myChar.TuongKhac.ChiMang = 0;
				}
				if (_myChar.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi > 0 && _myChar.InfoGame.Todoi != null)
				{
					foreach (Character c in _myChar.InfoGame.Todoi.Chars)
					{
						if (c.IsConnection && c.Info.IdUser != _myChar.Info.IdUser)
						{
							c.InfoGame.ToDoiKinhNghiem += _myChar.TuongKhac.PhanTramgKinhNghiemHoTroDongDoi;
						}
					}
				}
				if (PointKyNang > 0)
				{
					_myChar.Skill.TangPhanTramDameSkill = 0;
					_myChar.Skill.TangThoiGianTelietDichChuyenChiThuat = 0;
					_myChar.Point.DiemKyNang += PointKyNang;
					_myChar.WriteInfo();
					_myChar.MsgUpdateSkill();
					UpdateHpFullChar(_myChar);
					UpdateMpFullChar(_myChar);
					_myChar.SendMessage(UtilMessage.SendThongBao("Tất cả điểm kỹ năng đã được tẩy sạch", Util.YELLOW_MID));
				}
				else
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn không có điểm kỹ năng nào", Util.WHITE));
				}
			}
		}

		public static void UpdateHpChar(Character _myChar, int Hp)
		{
			int HpFull = _myChar.Point.GetHpFull(_myChar);
			_myChar.Point.Hp += Hp;
			if (_myChar.Point.Hp > HpFull)
			{
				_myChar.Point.Hp = HpFull;
			}
			if (_myChar.Point.Hp > int.MaxValue)
			{
				_myChar.Point.Hp = int.MaxValue;
			}
			if (_myChar.Point.Hp < 0)
			{
				_myChar.Point.Hp = 0;
			}
			int IdChar = _myChar.Info.IdUser;
			_myChar.SendMessage(UtilMessage.UpdateHp_Me(_myChar.Point.Hp, _myChar.Info.Cx, _myChar.Info.Cy));
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection && IdChar != c.Info.IdUser)
				{
					c.SendMessage(UtilMessage.SendHpMeInMap(IdChar, _myChar.Point.Hp, _myChar.Info.Cx, _myChar.Info.Cy));
				}
			}
		}

		public static void UpdateMpChar(Character _myChar, int Mp)
		{
			int MpFull = _myChar.Point.GetMpFull();
			_myChar.Point.Mp += Mp;
			if (_myChar.Point.Mp > MpFull)
			{
				_myChar.Point.Mp = MpFull;
			}
			if (_myChar.Point.Mp > int.MaxValue)
			{
				_myChar.Point.Mp = int.MaxValue;
			}
			if (_myChar.Point.Mp < 0)
			{
				_myChar.Point.Mp = 0;
			}
			int IdChar = _myChar.Info.IdUser;
			_myChar.SendMessage(UtilMessage.UpdateMp_Me(_myChar.Point.Mp));
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection && IdChar != c.Info.IdUser)
				{
					c.SendMessage(UtilMessage.SendMpMeInMap(IdChar, _myChar.Point.Mp));
				}
			}
		}

		public static void UpdateHpFullChar(Character _myChar)
		{
			int HpFull = _myChar.Point.GetHpFull(_myChar);
			if (_myChar.Point.Hp > HpFull)
			{
				_myChar.Point.Hp = HpFull;
			}
			_myChar.SendMessage(UtilMessage.UpdateHpFull_Me(HpFull, _myChar.Point.Hp));
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection && c.Info.IdUser != _myChar.Info.IdUser)
				{
					c.SendMessage(UtilMessage.SendHpFullMeInMap(_myChar.Info.IdUser, HpFull, _myChar.Point.Hp));
				}
			}
		}

		public static void UpdateMpFullChar(Character _myChar)
		{
			int MpFull = _myChar.Point.GetMpFull();
			if (_myChar.Point.Mp > MpFull)
			{
				_myChar.Point.Mp = MpFull;
			}
			_myChar.SendMessage(UtilMessage.UpdateMpFull_Me(MpFull, _myChar.Point.Mp));
			foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection && c.Info.IdUser != _myChar.Info.IdUser)
				{
					c.SendMessage(UtilMessage.SendMpFullMeInMap(_myChar.Info.IdUser, MpFull, _myChar.Point.Mp));
				}
			}
		}
	}
}
