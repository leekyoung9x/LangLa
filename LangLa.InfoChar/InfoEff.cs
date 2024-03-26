using LangLa.Data;
using LangLa.Hander;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;
using Newtonsoft.Json;

namespace LangLa.InfoChar
{
	[JsonObject(MemberSerialization.OptIn)]
	public class InfoEff
	{
		[JsonProperty]
		public short Id;

		[JsonProperty]
		public short IdItem;

		[JsonProperty]
		public int Value;

		[JsonProperty]
		public sbyte Type;

		[JsonProperty]
		public long TimeStart;

		[JsonProperty]
		public int TimeEnd;

		[JsonProperty]
		private long TimeSleep;

		public bool IsAutoRemove;

		private bool IsRunning = true;

		public InfoEff(short Id, int TimeEnd, int Value = -1, short IdItem = -1)
		{
			this.Id = Id;
			this.IdItem = IdItem;
			Type = (sbyte)DataServer.ArrEffTemplate[Id].Type;
			this.Value = ((Value == -1) ? GetValueFromId(Id) : Value);
			if ((Type == 6 || Type == 7) && Value > 3000)
			{
				Value = 3000;
			}
			if (Type == 91)
			{
				IsRunning = false;
			}
			this.TimeEnd = TimeEnd;
			TimeStart = Util.CurrentTimeMillis();
		}

		private static int GetValueFromId(int Id)
		{
			switch (Id)
			{
			case 0:
				return 7;
			case 1:
				return 27;
			case 2:
				return 36;
			case 3:
				return 45;
			case 4:
				return 54;
			case 5:
				return 63;
			case 6:
				return 72;
			case 7:
				return 81;
			case 13:
			case 20:
				return 50;
			case 14:
			case 21:
				return 160;
			case 15:
			case 22:
				return 370;
			case 16:
			case 23:
				return 580;
			case 17:
			case 24:
				return 790;
			case 18:
			case 25:
				return 940;
			case 19:
			case 26:
				return 1100;
			case 42:
				return 50;
			case 43:
				return 90;
			case 44:
				return 99;
			case 45:
				return 108;
			case 50:
				return 117;
			case 89:
				return 600;
			case 219:
			case 221:
				return 940;
			case 220:
			case 222:
				return 1100;
			default:
				return 0;
			}
		}

		private void ClearPointEff(Character _myChar)
		{
			switch (Type)
			{
			case 0:
			case 6:
			case 7:
				EffHander.RemoveEff(_myChar, this);
				break;
			case 1:
				EffHander.RemoveEff(_myChar, this);
				_myChar.InfoGame.IsSuyYeu = false;
				_myChar.WriteInfo();
				break;
			case 2:
				EffHander.RemoveEff(_myChar, this);
				break;
			case 3:
			{
				_myChar.InfoGame.IsLamCham = false;
				_myChar.InfoGame.Speed = (short)(_myChar.Info.Speed * 2);
				EffHander.RemoveEff(_myChar, this);
				short Speed = _myChar.TuongKhac.GetSpeedChar(_myChar);
				foreach (Character c2 in _myChar.InfoGame.ZoneGame.Chars.Values)
				{
					if (c2.IsConnection)
					{
						c2.SendMessage(UtilMessage.UpdatePointMore(_myChar.Info.IdUser, _myChar.InfoGame.LevelPk, _myChar.Info.TaiPhu, Speed, _myChar.InfoGame.StatusGD));
					}
				}
				EffHander.AddEffNotFocus(_myChar, 38, -1, 1500);
				break;
			}
			case 4:
				_myChar.InfoGame.IsBong = false;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 5:
			case 67:
				_myChar.InfoGame.IsChoang = false;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 47:
			case 48:
				_myChar.InfoGame.IsDinhAnChuChiThuat = false;
				_myChar.InfoGame.GiamKhangTatCaAnChuChiThuat = 0;
				_myChar.InfoGame.TrietTieuNeTranhGiamSatThuongAnChuChiThuat = 0;
				if (_myChar.InfoGame.GiamChakraAnChuChiThuat != 0)
				{
					_myChar.TuongKhac.UpdateChakra(_myChar, (short)(-_myChar.InfoGame.GiamChakraAnChuChiThuat));
				}
				_myChar.InfoGame.GiamChakraAnChuChiThuat = 0;
				break;
			case 17:
			case 19:
			case 63:
			case 85:
				EffHander.RemoveEff(_myChar, this);
				break;
			case 86:
				_myChar.InfoGame.IsDinhThanhSatChakra = false;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 45:
				_myChar.InfoGame.IsUseAnThanChiThuat = false;
				_myChar.InfoGame.GayBongSkill = 0;
				_myChar.InfoGame.ChinhXacAnThanChiThuat = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 43:
			{
				_myChar.InfoGame.UseAnPhanThanChiThuat = false;
				_myChar.InfoGame.NeTranhPhanThanChiThuat = 0;
				_myChar.InfoGame.SpeedPhanThanChiThuat = 0;
				short Speed2 = _myChar.TuongKhac.GetSpeedChar(_myChar);
				foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
				{
					if (c.IsConnection)
					{
						c.SendMessage(UtilMessage.UpdatePointMore(_myChar.Info.IdUser, _myChar.InfoGame.LevelPk, _myChar.Info.TaiPhu, Speed2, _myChar.InfoGame.StatusGD));
					}
				}
				EffHander.RemoveEff(_myChar, this);
				break;
			}
			case 49:
			case 50:
				_myChar.InfoGame.UseSkillChimYeuThuat = false;
				_myChar.InfoGame.DameChimYeuThuat = 0;
				_myChar.InfoGame.LamChamChimYeu = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 70:
				EffHander.RemoveEff(_myChar, this);
				break;
			case 54:
				_myChar.InfoGame.UseSkillDoiHutMau = false;
				_myChar.InfoGame.TangDameChiMangDoiHutMau = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 56:
			case 57:
				_myChar.InfoGame.UseTrangThaiHienNhan = false;
				_myChar.InfoGame.TieuHaoMpHienNhan = 0;
				_myChar.InfoGame.BoQuaKhangTinhHienNhan = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 12:
			case 65:
				_myChar.InfoGame.IsUseByaKugan = false;
				_myChar.InfoGame.ChinhXacByaKugan = 0;
				_myChar.InfoGame.IsUseByaKuganThuy = false;
				_myChar.InfoGame.ChinhXacByaKuganThuy = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 64:
				_myChar.InfoGame.IsUseByaKuganThuy = false;
				_myChar.InfoGame.ChinhXacByaKuganThuy = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 66:
			case 69:
				_myChar.TuongKhac.UpdateChakra(_myChar, (short)(-_myChar.InfoGame.ChakraCuuViHinh));
				_myChar.InfoGame.UseSkillCuuViHinh = false;
				_myChar.InfoGame.ChakraCuuViHinh = 0;
				_myChar.InfoGame.TileGayChoangNuaGiayCuuViHinh = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 87:
				_myChar.InfoGame.IsDinhLoaToanLienThuLiKiem = false;
				_myChar.InfoGame.ChinhXacLoaToanLienThuLiKiem = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 51:
			case 52:
				_myChar.InfoGame.UseChimKhongLo = false;
				_myChar.InfoGame.HpChimKhongLo = 0;
				PointHander.UpdateHpFullChar(_myChar);
				EffHander.RemoveEff(_myChar, this);
				break;
			case 60:
				_myChar.InfoGame.IsUseSusanoo = false;
				_myChar.InfoGame.DungMpHutSatThuongSasunoo = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 61:
			case 62:
				_myChar.InfoGame.IsUseBachHaoChiThuat = false;
				_myChar.InfoGame.TimeDichChuyenChiThuatFromBachHaoChiThuat = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 84:
				_myChar.InfoGame.IsDinhThienChieu = false;
				_myChar.InfoGame.SuyGiamNeTranhThienChieu = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			case 92:
				_myChar.InfoGame.UseSusanoFromCaiTrang = true;
				_myChar.InfoGame.DameAndChinhXacSusanoCaiTrang = 0;
				EffHander.RemoveEff(_myChar, this);
				break;
			}
		}

		private void ClearPointEffMob(Mob mob)
		{
			switch (Type)
			{
			case 4:
				mob.IsBong = false;
				break;
			case 2:
				mob.IsTrungDoc = false;
				break;
			case 3:
				mob.IsLamCham = false;
				break;
			case 5:
				mob.IsAnChoang = false;
				break;
			case 47:
			case 48:
				mob.InfoMob.IsDinhAnChuChiThuat = false;
				MobHander.RemovePointSkillAnChuChiThuat(mob);
				break;
			case 84:
				mob.InfoMob.IsDinhThienChieu = false;
				MobHander.RemovePointThienChieu(mob);
				break;
			case 87:
				mob.InfoMob.IsDinhLoaToanLienThuLiKiem = false;
				MobHander.RemovePointSkillLoaToanLienThuLiKiem(mob);
				break;
			}
		}

		public void MobUpdate(Mob mob)
		{
			if (!IsRunning)
			{
				return;
			}
			long Time = Util.CurrentTimeMillis();
			bool bool1 = Time - TimeStart > TimeEnd;
			bool bool2 = mob.Hp <= 0;
			if (bool1 || bool2)
			{
				IsRunning = false;
				if (bool2)
				{
					ClearPointEffMob(mob);
					for (int i = 0; i < mob.Effs.Count; i++)
					{
						EffHander.RemoveEffMob(mob, mob.Effs[i]);
					}
				}
				else
				{
					ClearPointEffMob(mob);
					EffHander.RemoveEffMob(mob, this);
				}
				return;
			}
			sbyte type = Type;
			sbyte b = type;
			if (b != 2 || Time <= TimeSleep)
			{
				return;
			}
			if (mob.Hp > 0)
			{
				bool IsCrit = false;
				int Dame = Util.NextInt((int)((double)Value * 0.7), Value);
				if (Util.NextInt(0, 100) < 2)
				{
					IsCrit = true;
					Dame *= 2;
				}
				mob.Hp -= Dame;
				if (mob.Hp <= 0)
				{
					EffHander.RemoveEffMob(mob, this);
					mob.SetDie(null);
				}
				short IdMob = mob.IdEntity;
				int Hp = mob.Hp;
				foreach (Character c in mob._Zone.Chars.Values)
				{
					if (c.IsConnection)
					{
						Message j = new Message(52);
						j.WriteShort(IdMob);
						j.WriteInt(Hp);
						j.WriteBool(IsCrit);
						c.SendMessage(j);
					}
				}
			}
			TimeSleep = Time + 350;
		}

		public void Update(Character _myChar)
		{
			if (!IsRunning)
			{
				return;
			}
			long Time = Util.CurrentTimeMillis();
			bool bool1 = Time - TimeStart > TimeEnd;
			bool bool2 = _myChar.Info.IsDie;
			if (bool1 || bool2)
			{
				IsRunning = false;
				if (bool2 || bool1)
				{
					ClearPointEff(_myChar);
				}
				if (bool1)
				{
					sbyte type = Type;
					sbyte b = type;
					if (b == 42)
					{
						_myChar.InfoGame.IsUseBiDuoc = false;
					}
					EffHander.RemoveEff(_myChar, this);
				}
				return;
			}
			switch (Type)
			{
			case 0:
			case 6:
			case 7:
				if (Time > TimeSleep)
				{
					if (_myChar.Info.IsDie)
					{
						EffHander.RemoveEff(_myChar, this);
					}
					if (_myChar.Point.Hp <= 0)
					{
						EffHander.RemoveEff(_myChar, this);
					}
					if (Type == 0 || Type == 6)
					{
						PointHander.UpdateHpChar(_myChar, Value);
					}
					if (Type == 0 || Type == 7)
					{
						PointHander.UpdateMpChar(_myChar, Value);
					}
					TimeSleep = Time + 500;
				}
				break;
			case 2:
				if (Time > TimeSleep)
				{
					PointHander.UpdateHpChar(_myChar, -Value);
					if (_myChar.Point.Hp <= 0)
					{
						_myChar.SetDie();
					}
					TimeSleep = Time + 350;
				}
				break;
			case 51:
				if (Time > TimeSleep)
				{
					PointHander.UpdateHpChar(_myChar, Value);
					TimeSleep = Time + 2000;
				}
				break;
			case 61:
				if (Time > TimeSleep)
				{
					int HpFull = _myChar.Point.GetHpFull(_myChar);
					int ValueUp = HpFull * Value / 100;
					if (_myChar.Point.Hp < HpFull)
					{
						PointHander.UpdateHpChar(_myChar, ValueUp);
					}
					TimeSleep = Time + 1000;
				}
				break;
			}
		}
	}
}
