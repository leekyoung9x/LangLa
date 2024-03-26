using System;
using System.Linq;
using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.Hander
{
	public static class SkillHander
	{
		public static void Write(Message msg, Skill[] skills, short IdSkillFocus = -1)
		{
			msg.WriteShort((IdSkillFocus == -1) ? skills[0].Id : IdSkillFocus);
			msg.WriteShort((short)skills.Length);
			foreach (Skill skill in skills)
			{
				if (skill != null)
				{
					msg.WriteShort(skill.Id);
				}
			}
		}

		public static void SkillNotFocus(Character _myChar, short IdSkillTemplate)
		{
			Skill skill = _myChar.Skill.Skills.FirstOrDefault((Skill s) => s.IdTemplate == IdSkillTemplate);
			if (skill == null)
			{
				return;
			}
			string[] Options = skill.Options.Split(";");
			short Value = -1;
			short Value3 = -1;
			short IdEff = -1;
			int Seconds = -1;
			switch (_myChar.Info.IdClass)
			{
			case 1:
				switch (IdSkillTemplate)
				{
				case 10:
					if (_myChar.Effs.Any((InfoEff s) => s.Type == 91))
					{
						InfoEff infoEff3 = _myChar.Effs.FirstOrDefault((InfoEff s) => s.Type == 91);
						EffHander.RemoveEff(_myChar, infoEff3);
					}
					Seconds = int.Parse(Options[0].Split(",")[1]);
					Value = short.Parse(Options[1].Split(",")[1]);
					Value3 = short.Parse(Options[2].Split(",")[1]);
					_myChar.InfoGame.UseChimKhongLo = true;
					_myChar.InfoGame.HpChimKhongLo += Value3;
					PointHander.UpdateHpFullChar(_myChar);
					EffHander.AddEffNotFocus(_myChar, 59, Value, Seconds);
					EffHander.AddEffNotFocus(_myChar, 60, Value3, Seconds, IsSendAll: false);
					break;
				case 7:
					Value = short.Parse(Options[0].Split(",")[1]);
					_myChar.InfoGame.IsUseSusanoo = true;
					_myChar.InfoGame.DungMpHutSatThuongSasunoo += Value;
					EffHander.AddEffNotFocus(_myChar, 68, _myChar.InfoGame.DungMpHutSatThuongSasunoo, 20000);
					break;
				case 31:
					Value = short.Parse(Options[0].Split(",")[1]);
					_myChar.InfoGame.IsUseBachHaoChiThuat = true;
					_myChar.InfoGame.TimeDichChuyenChiThuatFromBachHaoChiThuat += short.Parse(Options[1].Split(",")[1]);
					EffHander.AddEffNotFocus(_myChar, 69, Value, 10000);
					EffHander.AddEffNotFocus(_myChar, 70, _myChar.InfoGame.TimeDichChuyenChiThuatFromBachHaoChiThuat, 10000);
					break;
				}
				break;
			case 2:
				if (_myChar.Effs.Any((InfoEff s) => s.Type == 55))
				{
					InfoEff infoEff2 = _myChar.Effs.FirstOrDefault((InfoEff s) => s.Type == 55);
					EffHander.RemoveEff(_myChar, infoEff2);
				}
				if (_myChar.Effs.Any((InfoEff s) => s.Type == 54))
				{
					InfoEff infoEff = _myChar.Effs.FirstOrDefault((InfoEff s) => s.Type == 54);
					EffHander.RemoveEff(_myChar, infoEff);
				}
				Seconds = int.Parse(Options[0].Split(",")[1]);
				Value = short.Parse(Options[1].Split(",")[1]);
				Value3 = short.Parse(Options[2].Split(",")[1]);
				_myChar.InfoGame.UseSkillDoiHutMau = true;
				_myChar.InfoGame.LastTimeDoiHutMauAttack = Util.CurrentTimeMillis();
				_myChar.InfoGame.DameDoiHutMau = Value3;
				_myChar.InfoGame.TangDameChiMangDoiHutMau = Value;
				EffHander.AddEffNotFocus(_myChar, 62, Value, Seconds);
				EffHander.AddEffNotFocus(_myChar, 63, Value3, Seconds, IsSendAll: false);
				break;
			case 3:
				if (IdSkillTemplate == 13)
				{
					short Seconds2 = short.Parse(Options[0].Split(",")[1]);
					short Value2 = short.Parse(Options[1].Split(",")[1]);
					EffHander.AddEffNotFocus(_myChar, 72, 0, Seconds2);
					EffHander.AddEffNotFocus(_myChar, 73, Value2, Seconds2);
					_myChar.InfoGame.IsUseByaKuganThuy = true;
					_myChar.InfoGame.ChinhXacByaKuganThuy += Value2;
				}
				break;
			case 4:
				switch (IdSkillTemplate)
				{
				case 19:
				{
					if (_myChar.InfoGame.IsLamCham)
					{
						EffHander.RemoveEff(_myChar, _myChar.Effs.FirstOrDefault((InfoEff s) => s.Id == 10));
						_myChar.InfoGame.IsLamCham = false;
						_myChar.InfoGame.Speed += (short)(_myChar.Info.Speed * 2);
						short Speed2 = _myChar.TuongKhac.GetSpeedChar(_myChar);
						foreach (Character c2 in _myChar.InfoGame.ZoneGame.Chars.Values)
						{
							if (c2.IsConnection)
							{
								c2.SendMessage(UtilMessage.UpdatePointMore(_myChar.Info.IdUser, _myChar.InfoGame.LevelPk, _myChar.Info.TaiPhu, Speed2, _myChar.InfoGame.StatusGD));
							}
						}
					}
					if (_myChar.InfoGame.IsBong)
					{
						_myChar.InfoGame.IsBong = false;
						EffHander.RemoveEff(_myChar, _myChar.Effs.FirstOrDefault((InfoEff s) => s.Id == 11));
					}
					if (_myChar.Effs.Any((InfoEff s) => s.Id == 9))
					{
						EffHander.RemoveEff(_myChar, _myChar.Effs.FirstOrDefault((InfoEff s) => s.Id == 9));
					}
					if (_myChar.Effs.Any((InfoEff s) => s.Id == 8))
					{
						_myChar.InfoGame.IsSuyYeu = false;
						EffHander.RemoveEff(_myChar, _myChar.Effs.FirstOrDefault((InfoEff s) => s.Id == 8));
					}
					if (_myChar.Effs.Any((InfoEff s) => s.Id == 12))
					{
						EffHander.RemoveEff(_myChar, _myChar.Effs.FirstOrDefault((InfoEff s) => s.Id == 12));
					}
					_myChar.InfoGame.IsUseByaKugan = true;
					short Seconds3 = short.Parse(Options[0].Split(",")[1]);
					EffHander.AddEffNotFocus(_myChar, 31, 0, Seconds3);
					EffHander.AddEffNotFocus(_myChar, 73, short.Parse(Options[1].Split(",")[1]), Seconds3);
					_myChar.InfoGame.ChinhXacByaKugan = short.Parse(Options[1].Split(",")[1]);
					break;
				}
				case 22:
					Seconds = int.Parse(Options[0].Split(",")[1]);
					Value = short.Parse(Options[1].Split(",")[1]);
					Value3 = short.Parse(Options[2].Split(",")[1]);
					_myChar.InfoGame.IsUseAnThanChiThuat = true;
					_myChar.InfoGame.GayBongSkill += Value;
					_myChar.InfoGame.ChinhXacAnThanChiThuat += Value3;
					_myChar.WriteInfo();
					EffHander.AddEffNotFocus(_myChar, 53, Value, Seconds);
					EffHander.AddEffNotFocus(_myChar, 54, Value3, Seconds, IsSendAll: false);
					break;
				case 23:
					Seconds = int.Parse(Options[0].Split(",")[1]);
					Value = short.Parse(Options[1].Split(",")[1]);
					Value3 = short.Parse(Options[2].Split(",")[1]);
					_myChar.InfoGame.UseSkillChimYeuThuat = true;
					_myChar.InfoGame.DameChimYeuThuat += Value;
					_myChar.InfoGame.LamChamChimYeu += Value3;
					EffHander.AddEffNotFocus(_myChar, 57, Value, Seconds);
					EffHander.AddEffNotFocus(_myChar, 58, Value3, Seconds, IsSendAll: false);
					break;
				}
				break;
			case 5:
				if (skill.IdTemplate == 25)
				{
					short Value4 = short.Parse(Options[0].Split(",")[1]);
					sbyte Value5 = sbyte.Parse(Options[1].Split(",")[1]);
					_myChar.InfoGame.UseSkillCuuViHinh = true;
					_myChar.InfoGame.ChakraCuuViHinh += Value4;
					_myChar.InfoGame.TileGayChoangNuaGiayCuuViHinh += Value5;
					_myChar.TuongKhac.UpdateChakra(_myChar, Value4);
					EffHander.AddEffNotFocus(_myChar, 74, Value4, 10000);
					EffHander.AddEffNotFocus(_myChar, 77, Value5, 10000);
					break;
				}
				IdEff = (short)((skill.IdTemplate != 29) ? 51 : 64);
				Seconds = (short)((skill.IdTemplate != 29) ? 15 : 33);
				if (IdEff == 51)
				{
					_myChar.InfoGame.UseAnPhanThanChiThuat = true;
					_myChar.InfoGame.SpeedPhanThanChiThuat += short.Parse(Options[0].Split(",")[1]);
					_myChar.InfoGame.NeTranhPhanThanChiThuat += short.Parse(Options[1].Split(",")[1]);
					EffHander.AddEffNotFocus(_myChar, 51, _myChar.InfoGame.SpeedPhanThanChiThuat, 15000);
					EffHander.AddEffNotFocus(_myChar, 52, _myChar.InfoGame.NeTranhPhanThanChiThuat, 15000);
					short Speed = _myChar.TuongKhac.GetSpeedChar(_myChar);
					foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
					{
						if (c.IsConnection)
						{
							c.SendMessage(UtilMessage.UpdatePointMore(_myChar.Info.IdUser, _myChar.InfoGame.LevelPk, _myChar.Info.TaiPhu, Speed, _myChar.InfoGame.StatusGD));
						}
					}
				}
				else
				{
					_myChar.InfoGame.UseTrangThaiHienNhan = true;
					_myChar.InfoGame.BoQuaKhangTinhHienNhan += short.Parse(Options[1].Split(",")[1]);
					_myChar.InfoGame.TieuHaoMpHienNhan += short.Parse(Options[2].Split(",")[1]);
					EffHander.AddEffNotFocus(_myChar, 64, _myChar.InfoGame.BoQuaKhangTinhHienNhan, Seconds * 1000);
					EffHander.AddEffNotFocus(_myChar, 65, _myChar.InfoGame.TieuHaoMpHienNhan, Seconds * 1000);
				}
				break;
			}
			PointHander.UpdateMpChar(_myChar, -skill.MpUse);
		}

		public static void UpdatePointSkill(Character _myChar, Message msg)
		{
			short ID_Skill = msg.ReadShort();
			if (_myChar.Point.DiemKyNang <= 0)
			{
				return;
			}
			Skill SkillUP = InfoSkill.GetSkill(ID_Skill);
			SkillTemplate skillTemplate = DataServer.GetSkillTemplate(ID_Skill);
			if (_myChar.Skill == null || SkillUP.Level + 1 >= skillTemplate.LevelMax)
			{
				return;
			}
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
				if (_myChar.Skill.Skills[i] == null || _myChar.Skill.Skills[i].IdTemplate != ID_Skill)
				{
					continue;
				}
				Skill SkilF = InfoSkill.GetCloneSkill(ID_Skill, (sbyte)(_myChar.Skill.Skills[i].Level + 1));
				if (SkilF == null)
				{
					return;
				}
				_myChar.Skill.Skills[i] = SkilF;
				if (_myChar.Info.Level < DataServer.ArrSkillTemplate[SkilF.IdTemplate].LevelNeed)
				{
					return;
				}
				if (skillTemplate.Type == 5)
				{
					_myChar.TuongKhac.GetPointFromSkill(_myChar, _myChar.Skill.Skills[i]);
				}
				if (SkilF.IdTemplate == 0 || SkilF.IdTemplate == 6 || SkilF.IdTemplate == 12 || SkilF.IdTemplate == 18 || SkilF.IdTemplate == 24)
				{
					_myChar.Skill.TangPhanTramDameSkill = 0;
					_myChar.Skill.TangPhanTramDameSkill = short.Parse(SkilF.Options.Split(";")[4].Split(",")[1]);
				}
				if (SkilF.IdTemplate == 7)
				{
					_myChar.Skill.TangThoiGianTelietDichChuyenChiThuat = 0;
					short Seconds = short.Parse(SkilF.Options.Split(";")[1].Split(",")[1]);
					_myChar.Skill.TangThoiGianTelietDichChuyenChiThuat = Seconds;
				}
				break;
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
			_myChar.Point.DiemKyNang--;
			_myChar.WriteInfo();
			_myChar.MsgUpdateSkill();
			if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 11)
			{
				TaskHander.NextStep(_myChar);
			}
		}

		public static void SetSkillFocus(Character _myhar, Message msg)
		{
			short Id = msg.ReadShort();
			_myhar.Skill.IdFocus = _myhar.Skill.Skills.ToList().FirstOrDefault((Skill s) => s.IdTemplate == Id).Id;
			if (_myhar.Skill.IdFocus != -1)
			{
				Message i = new Message(126);
				Write(i, _myhar.Skill.Skills, _myhar.Skill.IdFocus);
				_myhar.SendMessage(i);
			}
		}

		public static bool CheckCanAttackChar(Character Char1, Character Char2)
		{
			if ((Char1.InfoGame.TypePk == 2 && Char2.InfoGame.TypePk == 2) || Char1.InfoGame.TypePk == 3 || Char2.InfoGame.TypePk == 3)
			{
				return true;
			}
			return false;
		}

		public static void UseSkill(Character Char, Skill skill, Mob mob = null, Character Char2 = null)
		{
			if (mob != null)
			{
				UseSkillAttackMob(Char, skill, mob);
			}
			if (Char2 != null)
			{
				UseSkillAttackChar(Char, skill, Char2);
			}
		}

		public static bool IsTuongKhac(sbyte ID1, sbyte ID2)
		{
			return (ID1 == 1 && ID2 == 2) || (ID1 == 2 && ID2 == 3) || (ID1 == 3 && ID2 == 4) || (ID1 == 4 && ID2 == 5) || (ID1 == 5 && ID2 == 1);
		}

		private static int GetDameSkillAttackMob(Skill skill, ref int GayChoang, ref int GayBong, ref int GaySuyYeu, ref int GayTrungDoc, ref int GayLamCham)
		{
			string[] Options = skill.Options.Split(";");
			int dame = 0;
			string[] array = Options;
			foreach (string Op in array)
			{
				string[] ValueAll = Op.Split(",");
				short Id = short.Parse(ValueAll[0]);
				int check = int.Parse(ValueAll[1]);
				switch (Id)
				{
				case 2:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
					dame += check;
					break;
				case 3:
				case 89:
					dame += check;
					break;
				case 69:
					GayTrungDoc += check;
					break;
				case 71:
				case 179:
					GayBong += check;
					break;
				case 68:
					GaySuyYeu += check;
					break;
				case 70:
				case 188:
					GayLamCham += check;
					break;
				case 72:
					GayChoang += check;
					break;
				}
			}
			return dame;
		}

		public static void PhanThanAttack(Character _myChar, Mob mob, int dame)
		{
			lock (_myChar.InfoGame.ZoneGame.Chars)
			{
				mob.Hp -= dame;
				short IdEntityMob = mob.IdEntity;
				int IdUse = _myChar.Info.IdUser;
				foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
				{
					if (c.IsConnection)
					{
						Message i = new Message(-19);
						i.WriteInt(IdUse);
						i.WriteShort(IdEntityMob);
						c.SendMessage(i);
						i = new Message(52);
						i.WriteShort(IdEntityMob);
						i.WriteInt(mob.Hp);
						i.WriteBool(x: false);
						c.SendMessage(i);
					}
				}
				if (mob.Hp <= 0)
				{
					mob.Hp = 0;
					mob.SetDie(_myChar);
					TaskHander.CheckDoneTaskKillMob(_myChar, mob.Id);
				}
			}
		}

		private static void UseSkillAttackMob(Character Char, Skill skill, Mob mob)
		{
			Skill skill2 = skill;
			PointHander.UpdateMpChar(Char, -skill2.MpUse);
			if (skill2.TimeDelay > Util.CurrentTimeMillis())
			{
				return;
			}
			int ValueGayChoang = 0;
			int ValueGayLamCham = 0;
			int ValueGayTrungDoc = 0;
			int ValueGayBong = 0;
			int ValueGaySuyYeu = 0;
			int Dame = GetDameSkillAttackMob(skill2, ref ValueGayChoang, ref ValueGayBong, ref ValueGaySuyYeu, ref ValueGayTrungDoc, ref ValueGayLamCham);
			sbyte IdClassChar = Char.Info.IdClass;
			sbyte IdClassMob = mob.IdClass;
			if (Char.Skill.TangPhanTramDameSkill > 0 && (skill2.IdTemplate == 2 || skill2.IdTemplate == 8 || skill2.IdTemplate == 14 || skill2.IdTemplate == 20 || skill2.IdTemplate == 26))
			{
				Dame += Dame * Char.Skill.TangPhanTramDameSkill / 100;
			}
			short Cx = Char.Info.Cx;
			short Cy = Char.Info.Cy;
			short Idm1 = mob.IdEntity;
			long ExpUp = mob.Exp;
			int IdChar = Char.Info.IdUser;
			int ExpUp2 = 1;
			if (Char.IsMapPhuBan())
			{
				ExpUp2 = 200;
			}
			ExpUp += ExpUp * ((Char.TuongKhac.PhanTramKinhNghiemDanhQuai + Char.InfoGame.ToDoiKinhNghiem) * LangLa.Server.Server.ExpServer * ExpUp2) / 100;
			if (ExpUp < 0)
			{
				ExpUp = 0L;
			}
			for (int i = 0; i < skill2.MaxTarget; i++)
			{
				if (i == 1)
				{
					mob = Char.InfoGame.ZoneGame.Mobs.Values.FirstOrDefault((Mob s) => s.Hp > 0 && Idm1 != s.IdEntity && s.Speed > 0 && Math.Abs(s.Cx - Cx) <= skill2.RangeNgang && Math.Abs(s.Cy - Cy) <= skill2.RangeDoc);
					if (mob == null)
					{
						break;
					}
				}
				Dame = Char.TuongKhac.GetDameAttackMob(Char, mob.IdClass, Dame, IsTuongKhac(Char.Info.IdClass, mob.IdClass));
				MobHander.GetDameAttackMob(Char, mob, ref Dame);
				if (Dame < 0)
				{
					Dame = 0;
				}
				if (Dame > 0)
				{
					Dame = Util.NextInt((int)((double)Dame * 0.8), Dame);
				}
				if (mob.IsTrungDoc && Dame > 0 && (double)Dame * 3.5 < 2147483647.0)
				{
					Dame = Util.NextInt(Dame, (int)((double)Dame * 3.5));
				}
				if (mob.IsBong)
				{
					Dame *= 2;
				}
				if (Dame < 0)
				{
					Dame = 0;
				}
				mob.Hp -= Dame;
				bool IsDie = (mob.Hp = ((mob.Hp > 0) ? mob.Hp : 0)) <= 0;
				ZoneHander.SendAttackMobInZone(Char, mob.IdEntity, mob.Hp, skill2.Index, IsCrit: false, Char.Point.Exp + ExpUp, IsDie);
				if (IsDie)
				{
					mob.Hp = 0;
					if (Char.InfoGame.Todoi != null && Char.InfoGame.Todoi.Chars.Count > 1)
					{
						ExpUp /= Char.InfoGame.Todoi.Chars.Count;
						if (ExpUp > 0)
						{
							foreach (Character c in Char.InfoGame.Todoi.Chars)
							{
								if (c.IsConnection && c.Info.IdUser != Char.Info.IdUser && c.Info.MapId == Char.Info.MapId && c.InfoGame.ZoneGame.Id == Char.InfoGame.ZoneGame.Id)
								{
									c.UpdateExp(ExpUp);
								}
							}
						}
					}
					Char.UpdateExp(ExpUp);
					mob.SetDie(Char);
					TaskHander.CheckDoneTaskKillMob(Char, mob.Id);
					continue;
				}
				if (Dame > 0 && mob.InfoMob.PhanTramPhanSatThuong > 0 && mob.LastTimePST < Util.CurrentTimeMillis())
				{
					int DamePST = Dame * mob.InfoMob.PhanTramPhanSatThuong / 100;
					if (DamePST > 0)
					{
						PointHander.UpdateHpChar(Char, -DamePST);
					}
					mob.LastTimePST = Util.CurrentTimeMillis() + 500;
				}
				EffHander.AddEffGayBongMob(Char, mob, ValueGayBong, Dame);
				EffHander.AddEffGayChoangMob(Char, mob, ValueGayChoang, Dame);
				EffHander.AddEffGaySuyYeuMob(Char, mob, ValueGaySuyYeu, Dame);
				if (Dame > 0)
				{
					EffHander.AddEffGayTrungDocMob(Char, mob, ValueGayTrungDoc, Dame);
				}
				EffHander.AddEffGayLamChamMob(Char, mob, ValueGayLamCham, Dame);
				mob.StatusAttack = 1;
				switch (skill2.IdTemplate)
				{
				case 4:
				{
					short Value38 = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
					short Value40 = short.Parse(skill2.Options.Split(";")[2].Split(",")[1]);
					InfoEff infoEff8 = new InfoEff(55, 10000, Value38, -1);
					infoEff8.IsAutoRemove = true;
					InfoEff infoEff9 = new InfoEff(56, 10000, Value40, -1);
					infoEff9.IsAutoRemove = true;
					mob.InfoMob.IsDinhAnChuChiThuat = true;
					mob.InfoMob.GiamKhangTatCa -= Value38;
					mob.InfoMob.TrietTieuNeTranhGiamSatThuong -= Value40;
					EffHander.AddEffMob(mob, infoEff8);
					EffHander.AddEffMob(mob, infoEff9);
					MobHander.AddValueGiamKhangTatCa(mob, (short)(-Value38));
					MobHander.AddValueGiamSatThuong(mob, (short)(-Value40));
					MobHander.AddValueNeTranh(mob, (short)(-Value40));
					if (Char.Skill.Skills.ToList().FindIndex((Skill s) => s.IdTemplate == 1 && s.Level > 0) != -1)
					{
						Skill skill3 = Char.Skill.Skills.FirstOrDefault((Skill s) => s.IdTemplate == 1);
						short Value39 = short.Parse(skill3.Options.Split(";")[0].Split(",")[1]);
						mob.InfoMob.GiamKhangTatCa -= Value39;
						MobHander.AddValueGiamKhangTatCa(mob, (short)(-Value39));
					}
					break;
				}
				case 11:
				{
					short Seconds3 = short.Parse(skill2.Options.Split(";")[2].Split(",")[1]);
					if (Char.Skill.TangThoiGianTelietDichChuyenChiThuat > 0)
					{
						Seconds3 += Char.Skill.TangThoiGianTelietDichChuyenChiThuat;
					}
					if (Char.InfoGame.IsUseBachHaoChiThuat)
					{
						Seconds3 += Char.InfoGame.TimeDichChuyenChiThuatFromBachHaoChiThuat;
					}
					InfoEff infoEff3 = new InfoEff(36, Seconds3, -1, -1);
					infoEff3.IsAutoRemove = true;
					EffHander.AddEffMob(mob, infoEff3);
					break;
				}
				case 32:
				{
					short Seconds = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
					InfoEff infoEff = new InfoEff(71, Seconds, -1, -1);
					infoEff.IsAutoRemove = true;
					EffHander.AddEffMob(mob, infoEff);
					break;
				}
				case 33:
				{
					short Seconds4 = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
					InfoEff infoEff4 = new InfoEff(78, Seconds4, -1, -1);
					infoEff4.IsAutoRemove = true;
					EffHander.AddEffMob(mob, infoEff4);
					break;
				}
				case 36:
				{
					short Value36 = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
					mob.InfoMob.IsDinhLoaToanLienThuLiKiem = true;
					mob.InfoMob.ChinhXacLoaToanLienThuLiKiem -= Value36;
					MobHander.AddValueChinhXac(mob, (short)(-Value36));
					InfoEff infoEff5 = new InfoEff(96, 15000, Value36, -1);
					infoEff5.IsAutoRemove = true;
					EffHander.AddEffMob(mob, infoEff5);
					break;
				}
				case 37:
				{
					short Seconds5 = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
					InfoEff infoEff6 = new InfoEff(95, Seconds5, -1, -1);
					infoEff6.IsAutoRemove = true;
					EffHander.AddEffMob(mob, infoEff6);
					break;
				}
				case 38:
				{
					short Value37 = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
					MobHander.AddValueNeTranh(mob, (short)(-Value37));
					InfoEff infoEff7 = new InfoEff(93, 15000, -1, -1);
					infoEff7.IsAutoRemove = true;
					EffHander.AddEffMob(mob, infoEff7);
					mob.InfoMob.IsDinhThienChieu = true;
					mob.InfoMob.SuyGiamNeTranhThienChieu -= Value37;
					break;
				}
				case 39:
				{
					short Seconds2 = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
					InfoEff infoEff2 = new InfoEff(94, Seconds2, -1, -1);
					infoEff2.IsAutoRemove = true;
					EffHander.AddEffMob(mob, infoEff2);
					break;
				}
				}
				if (i == 0 && Char.InfoGame.UseSkillDoiHutMau && !IsDie && Char.InfoGame.LastTimeDoiHutMauAttack < Util.CurrentTimeMillis())
				{
					mob.Hp -= Char.InfoGame.DameDoiHutMau;
					if (mob.Hp <= 0)
					{
						Char.UpdateExp(ExpUp);
						mob.SetDie(Char);
						TaskHander.CheckDoneTaskKillMob(Char, mob.Id);
					}
					foreach (Character c2 in Char.InfoGame.ZoneGame.Chars.Values)
					{
						if (c2.IsConnection)
						{
							Message j = new Message(-43);
							j.WriteInt(IdChar);
							j.WriteShort(mob.IdEntity);
							c2.SendMessage(j);
							j = new Message(52);
							j.WriteShort(mob.IdEntity);
							j.WriteInt(mob.Hp);
							j.WriteBool(x: false);
							c2.SendMessage(j);
						}
					}
					PointHander.UpdateHpChar(Char, Char.InfoGame.DameDoiHutMau);
					Char.InfoGame.LastTimeDoiHutMauAttack = Util.CurrentTimeMillis() + 3000;
				}
				else if (i == 0 && Char.InfoGame.UseSkillChimYeuThuat && !IsDie && Char.InfoGame.LastTimeChimYeuAttack < Util.CurrentTimeMillis())
				{
					foreach (Character c3 in Char.InfoGame.ZoneGame.Chars.Values)
					{
						if (c3.IsConnection)
						{
							Message k = new Message(-43);
							k.WriteInt(IdChar);
							k.WriteShort(mob.IdEntity);
							c3.SendMessage(k);
							k = new Message(52);
							k.WriteShort(mob.IdEntity);
							k.WriteInt(mob.Hp);
							k.WriteBool(x: false);
							c3.SendMessage(k);
						}
					}
					EffHander.AddEffGayLamChamMob(Char, mob, Char.InfoGame.LamChamChimYeu, Dame);
					Char.InfoGame.LastTimeChimYeuAttack = Util.CurrentTimeMillis() + 3000;
				}
				else
				{
					if (!Char.InfoGame.IsUseByaKuganThuy || Util.NextInt(0, 100) >= 50)
					{
						continue;
					}
					mob.Hp -= Dame;
					bool IsDie2 = (mob.Hp = ((mob.Hp > 0) ? mob.Hp : 0)) <= 0;
					ZoneHander.SendAttackMobInZone(Char, mob.IdEntity, mob.Hp, skill2.Index, IsCrit: false, Char.Point.Exp + ExpUp, IsDie2);
					if (!IsDie2)
					{
						continue;
					}
					mob.Hp = 0;
					if (Char.InfoGame.Todoi != null && Char.InfoGame.Todoi.Chars.Count > 1)
					{
						ExpUp /= Char.InfoGame.Todoi.Chars.Count;
						if (ExpUp > 0)
						{
							foreach (Character c4 in Char.InfoGame.Todoi.Chars)
							{
								if (c4.IsConnection && c4.Info.IdUser != Char.Info.IdUser && c4.Info.MapId == Char.Info.MapId && c4.InfoGame.ZoneGame.Id == Char.InfoGame.ZoneGame.Id)
								{
									c4.UpdateExp(ExpUp);
								}
							}
						}
					}
					Char.UpdateExp(ExpUp);
					mob.SetDie(Char);
					TaskHander.CheckDoneTaskKillMob(Char, mob.Id);
				}
			}
			if (Char.InfoGame.IsUseAnThanChiThuat)
			{
				Char.InfoGame.IsUseAnThanChiThuat = false;
				Char.InfoGame.GayBongSkill = 0;
				EffHander.RemoveEff(Char, Char.Effs.FirstOrDefault((InfoEff s) => s.Id == 53));
				EffHander.RemoveEff(Char, Char.Effs.FirstOrDefault((InfoEff s) => s.Id == 54));
			}
		}

		private static int GetDameSkillAttackChar(Skill skill, ref int GayChoang, ref int GayBong, ref int GaySuyYeu, ref int GayTrungDoc, ref int GayLamCham)
		{
			string[] Options = skill.Options.Split(";");
			int dame = 0;
			string[] array = Options;
			foreach (string Op in array)
			{
				string[] ValueAll = Op.Split(",");
				short Id = short.Parse(ValueAll[0]);
				int check = int.Parse(ValueAll[1]);
				switch (Id)
				{
				case 2:
				case 73:
				case 74:
				case 75:
				case 76:
				case 77:
				case 78:
					dame += check;
					break;
				case 69:
					GayTrungDoc += check;
					break;
				case 71:
				case 179:
					GayBong += check;
					break;
				case 68:
					GaySuyYeu += check;
					break;
				case 70:
				case 188:
					GayLamCham += check;
					break;
				case 72:
					GayChoang += check;
					break;
				case 3:
					dame += check;
					break;
				}
			}
			return dame;
		}

		private static bool IsCanAttackChar(Character _myChar, Character _cAndAme)
		{
			bool Level = Math.Abs(_myChar.Info.Level - _cAndAme.Info.Level) < 10 && _cAndAme.InfoGame.Status == 0;
			return (_myChar.InfoGame.TypePk == 2 && _cAndAme.InfoGame.TypePk == 2 && Level) || (_myChar.InfoGame.TypePk == 3 && Level) || (_cAndAme.InfoGame.TypePk == 3 && Level) || (_cAndAme.InfoGame.IsAnCuuSat && _myChar.InfoGame.IsCuuSat) || (_cAndAme.InfoGame.IsCuuSat && _myChar.InfoGame.IsAnCuuSat) || (_cAndAme.InfoGame.IsTyVo && _myChar.InfoGame.IsTyVo && _myChar.InfoGame.IdTyVo == _cAndAme.Info.IdUser && _cAndAme.InfoGame.IdTyVo == _myChar.Info.IdUser);
		}

		private static void UseSkillAttackChar(Character _myChar, Skill skill, Character _cAnDame)
		{
			Character _myChar2 = _myChar;
			Skill skill2 = skill;
			if (!IsCanAttackChar(_myChar2, _cAnDame))
			{
				return;
			}
			int IdChar2 = _cAnDame.Info.IdUser;
			int IdChar3 = _myChar2.Info.IdUser;
			short Cx2 = _cAnDame.Info.Cx;
			short Cy2 = _cAnDame.Info.Cy;
			sbyte LevelChar = _myChar2.Info.Level;
			int DameUpFromChar = 0;
			for (int i = 0; i < skill2.MaxTarget; i++)
			{
				if (i == 1)
				{
					_cAnDame = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.IsConnection && !s.Info.IsDie && s.Info.IdUser != IdChar2 && s.Info.IdUser != IdChar3 && IsCanAttackChar(_myChar2, s) && Math.Abs(s.Info.Cx - Cx2) <= skill2.RangeNgang && Math.Abs(s.Info.Cy - Cy2) <= skill2.RangeDoc);
					if (_cAnDame == null || !IsCanAttackChar(_myChar2, _cAnDame))
					{
						break;
					}
				}
				short NT2 = (short)(_cAnDame.TuongKhac.GetNeTranh(_cAnDame) - _myChar2.TuongKhac.BoQuaNeTranh + _myChar2.TuongKhac.GetChinhXac(_myChar2));
				int NT = NT2;
				int ValueGayChoang = 0;
				int ValueGayLamCham = 0;
				int ValueGayTrungDoc = 0;
				int ValueGayBong = 0;
				int ValueGaySuyYeu = 0;
				bool NeTranh = false;
				bool IsCrit = false;
				if (NT > 0)
				{
					short ValueRamdom = 10;
					if (NT > 300)
					{
						ValueRamdom = 17;
					}
					else if (NT > 500)
					{
						ValueRamdom = 24;
					}
					else if (NT > 1000)
					{
						ValueRamdom = 44;
					}
					else if (NT > 1500)
					{
						ValueRamdom = 66;
					}
					else if (NT > 2000)
					{
						ValueRamdom = 77;
					}
					else if (NT > 3000)
					{
						ValueRamdom = 88;
					}
					NeTranh = Util.NextInt(0, 100) < ValueRamdom;
				}
				if (!NeTranh)
				{
					DameUpFromChar = GetDameSkillAttackChar(skill2, ref ValueGayChoang, ref ValueGayBong, ref ValueGaySuyYeu, ref ValueGayTrungDoc, ref ValueGayLamCham);
					if (_myChar2.Skill.TangPhanTramDameSkill > 0 && (skill2.IdTemplate == 2 || skill2.IdTemplate == 8 || skill2.IdTemplate == 14 || skill2.IdTemplate == 20 || skill2.IdTemplate == 26))
					{
						DameUpFromChar += DameUpFromChar * _myChar2.Skill.TangPhanTramDameSkill / 100;
					}
					_cAnDame.TuongKhac.DownPointHieuUng(_cAnDame, ref ValueGayChoang, ref ValueGayLamCham, ref ValueGayBong, ref ValueGaySuyYeu, ref ValueGayTrungDoc);
					DameUpFromChar += _myChar2.TuongKhac.GetDameAttackChar(_myChar2, _cAnDame, 0);
					if (DameUpFromChar > 0)
					{
						DameUpFromChar = Util.NextInt((int)((double)DameUpFromChar * 0.8), DameUpFromChar);
					}
					IsCrit = _myChar2.TuongKhac.GetCritAttackChar(ref DameUpFromChar, _myChar2, _cAnDame);
					if (_cAnDame.InfoGame.IsBong)
					{
						DameUpFromChar = (int)((double)DameUpFromChar * 1.45);
					}
					if (skill2.IdTemplate == 34)
					{
						short ValueUp = short.Parse(skill2.Options.Split(";")[0].Split(",")[1]);
						int Dame2 = _cAnDame.Point.GetHpFull(_myChar2) * ValueUp / 100;
						DameUpFromChar += Dame2;
					}
					if (_cAnDame.InfoGame.IsUseSusanoo)
					{
						int DameHutMp = DameUpFromChar * _cAnDame.InfoGame.DungMpHutSatThuongSasunoo / 100;
						if (_cAnDame.Point.Mp > DameHutMp)
						{
							DameUpFromChar -= DameHutMp;
							PointHander.UpdateMpChar(_cAnDame, -DameHutMp);
						}
					}
					if (DameUpFromChar < 0)
					{
						DameUpFromChar = 0;
					}
					_cAnDame.Point.Hp -= DameUpFromChar;
					if (_cAnDame.Point.Hp <= 0)
					{
						_cAnDame.SetDie();
					}
					else
					{
						switch (skill2.IdTemplate)
						{
						case 11:
						{
							short Seconds11 = short.Parse(skill2.Options.Split(";")[2].Split(",")[1]);
							if (_myChar2.Skill.TangThoiGianTelietDichChuyenChiThuat > 0)
							{
								Seconds11 += _myChar2.Skill.TangThoiGianTelietDichChuyenChiThuat;
							}
							if (_myChar2.InfoGame.IsUseBachHaoChiThuat)
							{
								Seconds11 += _myChar2.InfoGame.TimeDichChuyenChiThuatFromBachHaoChiThuat;
							}
							EffHander.AddEffNotFocus(_cAnDame, 36, 0, Seconds11);
							break;
						}
						case 4:
						{
							short Value3 = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
							short Value4 = short.Parse(skill2.Options.Split(";")[2].Split(",")[1]);
							_cAnDame.InfoGame.IsDinhAnChuChiThuat = true;
							_cAnDame.InfoGame.GiamKhangTatCaAnChuChiThuat -= Value3;
							_cAnDame.InfoGame.TrietTieuNeTranhGiamSatThuongAnChuChiThuat -= Value4;
							EffHander.AddEffNotFocus(_cAnDame, 55, Value3, 10000);
							EffHander.AddEffNotFocus(_cAnDame, 56, Value4, 10000);
							if (_myChar2.Skill.Skills.ToList().FindIndex((Skill s) => s.IdTemplate == 1 && s.Level > 0) != -1)
							{
								Skill skill3 = _myChar2.Skill.Skills.FirstOrDefault((Skill s) => s.IdTemplate == 1);
								short Value5 = short.Parse(skill3.Options.Split(";")[0].Split(",")[1]);
								short Value6 = short.Parse(skill3.Options.Split(";")[1].Split(",")[1]);
								_cAnDame.InfoGame.TrietTieuNeTranhGiamSatThuongAnChuChiThuat -= Value5;
								_cAnDame.InfoGame.GiamChakraAnChuChiThuat -= Value6;
								_cAnDame.TuongKhac.UpdateChakra(_cAnDame, _cAnDame.InfoGame.GiamChakraAnChuChiThuat);
								EffHander.AddEffNotFocus(_cAnDame, 76, _cAnDame.InfoGame.GiamChakraAnChuChiThuat, 10000);
							}
							break;
						}
						case 32:
							EffHander.AddEffNotFocus(_cAnDame, 71, 0, short.Parse(skill2.Options.Split(";")[1].Split(",")[1]));
							break;
						case 33:
							EffHander.AddEffNotFocus(_cAnDame, 78, 0, short.Parse(skill2.Options.Split(";")[1].Split(",")[1]));
							break;
						case 36:
						{
							_cAnDame.InfoGame.IsDinhLoaToanLienThuLiKiem = true;
							short Value2 = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
							_cAnDame.InfoGame.ChinhXacLoaToanLienThuLiKiem -= Value2;
							EffHander.AddEffNotFocus(_cAnDame, 96, Value2, 15000);
							break;
						}
						case 37:
							_cAnDame.InfoGame.IsDinhThanhSatChakra = true;
							EffHander.AddEffNotFocus(_cAnDame, 95, 0, short.Parse(skill2.Options.Split(";")[1].Split(",")[1]));
							break;
						case 38:
						{
							short Value = short.Parse(skill2.Options.Split(";")[1].Split(",")[1]);
							EffHander.AddEffNotFocus(_cAnDame, 93, Value, 15000);
							_cAnDame.InfoGame.IsDinhThienChieu = true;
							_cAnDame.InfoGame.SuyGiamNeTranhThienChieu -= Value;
							break;
						}
						case 39:
							EffHander.AddEffNotFocus(_cAnDame, 94, 0, short.Parse(skill2.Options.Split(";")[1].Split(",")[1]));
							break;
						}
						EffHander.AddEffGayBong(_myChar2, _cAnDame, ValueGayBong);
						EffHander.AddEffGayChoang(_myChar2, _cAnDame, ValueGayChoang);
						EffHander.AddEffGayGaySuyYeu(_myChar2, _cAnDame, ValueGaySuyYeu);
						EffHander.AddEffGayTrungDoc(_myChar2, _cAnDame, ValueGayTrungDoc);
						EffHander.AddEffGayLamCham(_myChar2, _cAnDame, ValueGayLamCham);
						if (_myChar2.InfoGame.UseSkillCuuViHinh && Util.NextInt(0, 100) < _myChar2.InfoGame.TileGayChoangNuaGiayCuuViHinh)
						{
							EffHander.AddEffNotFocus(_cAnDame, 75, 0, 500);
						}
					}
				}
				_myChar2.Point.Mp -= skill2.MpUse;
				if (_myChar2.Point.Mp <= 0)
				{
					_myChar2.Point.Mp = 0;
				}
				int IdChar = _cAnDame.Info.IdUser;
				int Hp = _cAnDame.Point.Hp;
				int Mp = _cAnDame.Point.Mp;
				short Cx = _cAnDame.Info.Cx;
				short Cy = _cAnDame.Info.Cy;
				foreach (Character c3 in _myChar2.InfoGame.ZoneGame.Chars.Values)
				{
					if (c3.IsConnection)
					{
						Message l = new Message(20);
						l.WriteInt(_myChar2.Info.IdUser);
						l.WriteInt(_myChar2.Point.Mp);
						l.WriteShort(skill2.Index);
						l.WriteInt(_cAnDame.Info.IdUser);
						c3.SendMessage(l);
						c3.SendMessage(UtilMessage.SendDameHpChar(IdChar, Mp, Hp, IsCrit, Cx, Cy));
					}
				}
				if (i == 0 && _myChar2.InfoGame.UseSkillDoiHutMau && _cAnDame.Point.Hp > 0 && _myChar2.InfoGame.LastTimeDoiHutMauAttack < Util.CurrentTimeMillis())
				{
					PointHander.UpdateHpChar(_cAnDame, -_myChar2.InfoGame.DameDoiHutMau);
					if (_cAnDame.Point.Hp <= 0)
					{
						_cAnDame.SetDie();
					}
					foreach (Character c in _myChar2.InfoGame.ZoneGame.Chars.Values)
					{
						if (c.IsConnection)
						{
							Message j = new Message(-44);
							j.WriteInt(_myChar2.Info.IdUser);
							j.WriteInt(_cAnDame.Info.IdUser);
							c.SendMessage(j);
						}
					}
					PointHander.UpdateHpChar(_myChar2, _myChar2.InfoGame.DameDoiHutMau);
					_myChar2.InfoGame.LastTimeDoiHutMauAttack = Util.CurrentTimeMillis() + 2500;
				}
				else if (i == 0 && _myChar2.InfoGame.UseSkillChimYeuThuat && _cAnDame.Point.Hp > 0 && _myChar2.InfoGame.LastTimeChimYeuAttack < Util.CurrentTimeMillis())
				{
					foreach (Character c2 in _myChar2.InfoGame.ZoneGame.Chars.Values)
					{
						if (c2.IsConnection)
						{
							Message k = new Message(-44);
							k.WriteInt(_myChar2.Info.IdUser);
							k.WriteInt(_cAnDame.Info.IdUser);
							c2.SendMessage(k);
						}
					}
					EffHander.AddEffGayLamCham(_myChar2, _cAnDame, ValueGayLamCham + _myChar2.InfoGame.LamChamChimYeu);
					_myChar2.InfoGame.LastTimeChimYeuAttack = Util.CurrentTimeMillis() + 3500;
				}
				else if (_myChar2.InfoGame.UseTrangThaiHienNhan && _cAnDame.Point.Hp > _myChar2.InfoGame.TieuHaoMpHienNhan && _myChar2.InfoGame.LastTimeTieuHaoMpHienNhan < Util.CurrentTimeMillis())
				{
					PointHander.UpdateMpChar(_cAnDame, -_myChar2.InfoGame.TieuHaoMpHienNhan);
					_myChar2.InfoGame.LastTimeTieuHaoMpHienNhan = Util.CurrentTimeMillis() + 3000;
				}
				else if (_myChar2.InfoGame.IsUseByaKuganThuy && DameUpFromChar > 0 && Util.NextInt(0, 100) < 50)
				{
					_cAnDame.Point.Hp -= DameUpFromChar;
					if (_cAnDame.Point.Hp <= 0)
					{
						_cAnDame.SetDie();
						continue;
					}
					EffHander.AddEffGayBong(_myChar2, _cAnDame, ValueGayBong);
					EffHander.AddEffGayChoang(_myChar2, _cAnDame, ValueGayChoang);
					EffHander.AddEffGayGaySuyYeu(_myChar2, _cAnDame, ValueGaySuyYeu);
					EffHander.AddEffGayTrungDoc(_myChar2, _cAnDame, ValueGayTrungDoc);
					EffHander.AddEffGayLamCham(_myChar2, _cAnDame, ValueGayLamCham);
				}
			}
			if (_myChar2.InfoGame.IsUseAnThanChiThuat)
			{
				_myChar2.InfoGame.IsUseAnThanChiThuat = false;
				_myChar2.InfoGame.GayBongSkill = 0;
				EffHander.RemoveEff(_myChar2, _myChar2.Effs.FirstOrDefault((InfoEff s) => s.Id == 53));
				EffHander.RemoveEff(_myChar2, _myChar2.Effs.FirstOrDefault((InfoEff s) => s.Id == 54));
			}
		}
	}
}
