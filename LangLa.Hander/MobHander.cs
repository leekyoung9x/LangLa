using System.Linq;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class MobHander
	{
		private static readonly short ValueTK = 8;

		public static void SetPointRespawMob(Mob mob, bool IsNew = false, bool IsSetThuLinh = true)
		{
			if (!IsNew)
			{
				for (int i = 0; i < mob.InfoMob.ArrTuongKhac.Length; i++)
				{
					mob.InfoMob.ArrTuongKhac[i] = 0;
				}
				mob.IdClass = (sbyte)Util.NextInt(1, 5);
			}
			mob.InfoMob.PhanTramPhanSatThuong = 0;
			mob.InfoMob.PhanTramHoiPhucHp = 0;
			if (IsSetThuLinh)
			{
				bool IsCreateTinhAnh = false;
				bool IsCreateThuLinh = false;
				lock (mob._Zone.Mobs)
				{
					if (mob._Zone.Mobs.Values.ToList().FindIndex((Mob s) => s.LevelBoss == 1 || s.LevelBoss == 2) == -1 && mob.LevelBoss == 0)
					{
						sbyte Rd = (sbyte)Util.NextInt(0, 10);
						if (Rd > 9 && Util.CurrentTimeMillis() > mob._Zone.LastTimeRespawTinhAnh)
						{
							IsCreateThuLinh = true;
							mob._Zone.LastTimeRespawTinhAnh = Util.CurrentTimeMillis() + 600000;
						}
						else if (Rd > 6 && Util.CurrentTimeMillis() > mob._Zone.LastTimeRespawTinhAnh)
						{
							IsCreateTinhAnh = true;
							mob._Zone.LastTimeRespawTinhAnh = Util.CurrentTimeMillis() + 300000;
						}
					}
				}
				if (IsCreateTinhAnh)
				{
					mob.LevelBoss = 1;
					mob.HpTinhAnh = mob.HpFull * Util.NextInt(5, 10);
					mob.HpFull += mob.HpTinhAnh;
					mob.Exp += mob.HpFull / 10;
					mob.Hp = mob.HpFull;
				}
				else if (IsCreateThuLinh)
				{
					mob.LevelBoss = 2;
					mob.HpThuLinh = mob.HpFull * Util.NextInt(10, 20);
					mob.HpFull += mob.HpThuLinh;
					mob.Exp += mob.HpFull / 10;
					mob.Hp = mob.HpFull;
				}
			}
			switch (mob.IdClass)
			{
			case 1:
				AddValueKhangPhong(mob, (short)(mob.Level * ValueTK));
				AddValueTuongKhacTho(mob, (short)(mob.Level * ValueTK));
				break;
			case 2:
				AddValueKhangLoi(mob, (short)(mob.Level * ValueTK));
				AddValueTuongKhacThuy(mob, (short)(mob.Level * ValueTK));
				break;
			case 3:
				AddValueKhangTho(mob, (short)(mob.Level * ValueTK));
				AddValueTuongKhacHoa(mob, (short)(mob.Level * ValueTK));
				break;
			case 4:
				AddValueKhangThuy(mob, (short)(mob.Level * ValueTK));
				AddValueTuongKhacPhong(mob, (short)(mob.Level * ValueTK));
				break;
			case 5:
				AddValueKhangHoa(mob, (short)(mob.Level * ValueTK));
				AddValueTuongKhacLoi(mob, (short)(mob.Level * ValueTK));
				break;
			}
		}

		public static void RemovePointSkillAnChuChiThuat(Mob mob)
		{
			for (int i = 15; i < 20; i++)
			{
				mob.InfoMob.ArrTuongKhac[i] -= mob.InfoMob.GiamKhangTatCa;
			}
			mob.InfoMob.ArrTuongKhac[20] -= mob.InfoMob.TrietTieuNeTranhGiamSatThuong;
			mob.InfoMob.ArrTuongKhac[21] -= mob.InfoMob.TrietTieuNeTranhGiamSatThuong;
			mob.InfoMob.GiamKhangTatCa = 0;
			mob.InfoMob.TrietTieuNeTranhGiamSatThuong = 0;
			mob.InfoMob.TrietTieuNeTranhGiamSatThuong = 0;
		}

		public static void RemovePointSkillLoaToanLienThuLiKiem(Mob mob)
		{
			mob.InfoMob.ArrTuongKhac[1] -= mob.InfoMob.ChinhXacLoaToanLienThuLiKiem;
			mob.InfoMob.ArrTuongKhac[2] -= mob.InfoMob.ChinhXacLoaToanLienThuLiKiem;
			mob.InfoMob.ChinhXacLoaToanLienThuLiKiem = 0;
		}

		public static void RemovePointThienChieu(Mob mob)
		{
			mob.InfoMob.ArrTuongKhac[21] -= mob.InfoMob.SuyGiamNeTranhThienChieu;
			mob.InfoMob.SuyGiamNeTranhThienChieu = 0;
		}

		public static int GetDameAttackMob(Character _myChar, Mob mob, ref int Dame)
		{
			switch (mob.IdClass)
			{
			case 1:
				if (_myChar.Info.IdClass == 5)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[19];
				}
				if (mob.InfoMob.IsDinhAnChuChiThuat)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[19];
				}
				break;
			case 2:
				if (_myChar.Info.IdClass == 1)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[15];
				}
				if (mob.InfoMob.IsDinhAnChuChiThuat)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[15];
				}
				break;
			case 3:
				if (_myChar.Info.IdClass == 2)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[16];
				}
				if (mob.InfoMob.IsDinhAnChuChiThuat)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[16];
				}
				break;
			case 4:
				if (_myChar.Info.IdClass == 3)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[17];
				}
				if (mob.InfoMob.IsDinhAnChuChiThuat)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[17];
				}
				break;
			case 5:
				if (_myChar.Info.IdClass == 4)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[18];
				}
				if (mob.InfoMob.IsDinhAnChuChiThuat)
				{
					Dame -= mob.InfoMob.ArrTuongKhac[18];
				}
				break;
			}
			Dame -= mob.InfoMob.ArrTuongKhac[20];
			int ChinhXac = _myChar.TuongKhac.GetChinhXac(_myChar);
			int NeTranh = mob.InfoMob.ArrTuongKhac[21] - ChinhXac;
			if (NeTranh > 0 && NeTranh >= Util.NextInt(0, 1000))
			{
				Dame = 0;
			}
			return Dame;
		}

		public static int GetDameAttackChar(Mob mob, Character _cAnDame)
		{
			if (mob.InfoMob.IsDinhLoaToanLienThuLiKiem)
			{
				return 1;
			}
			int Dame = mob.Level * 10;
			switch (_cAnDame.Info.IdClass)
			{
			case 1:
				if (mob.IdClass == 5)
				{
					Dame += mob.InfoMob.ArrTuongKhac[6];
				}
				Dame -= _cAnDame.TuongKhac.KhangLoi;
				break;
			case 2:
				if (mob.IdClass == 1)
				{
					Dame += mob.InfoMob.ArrTuongKhac[7];
				}
				Dame -= _cAnDame.TuongKhac.KhangTho;
				break;
			case 3:
				if (mob.IdClass == 2)
				{
					Dame += mob.InfoMob.ArrTuongKhac[8];
				}
				Dame -= _cAnDame.TuongKhac.KhangThuy;
				break;
			case 4:
				if (mob.IdClass == 3)
				{
					Dame += mob.InfoMob.ArrTuongKhac[9];
				}
				Dame -= _cAnDame.TuongKhac.KhangHoa;
				break;
			case 5:
				if (mob.IdClass == 4)
				{
					Dame += mob.InfoMob.ArrTuongKhac[10];
				}
				Dame -= _cAnDame.TuongKhac.KhangPhong;
				break;
			}
			return Dame;
		}

		public static void AddValueKhangLoi(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[15] += Value;
		}

		public static void AddValueKhangTho(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[16] += Value;
		}

		public static void AddValueKhangThuy(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[17] += Value;
		}

		public static void AddValueKhangHoa(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[18] += Value;
		}

		public static void AddValueKhangPhong(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[19] += Value;
		}

		public static void AddValueTuongKhacLoi(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[5] += Value;
		}

		public static void AddValueTuongKhacTho(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[6] += Value;
		}

		public static void AddValueTuongKhacThuy(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[7] += Value;
		}

		public static void AddValueTuongKhacHoa(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[8] += Value;
		}

		public static void AddValueTuongKhacPhong(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[9] += Value;
		}

		public static void AddValueGiamSatThuong(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[20] += Value;
		}

		public static void AddValueGiamKhangTatCa(Mob mob, short Value)
		{
			for (int i = 15; i < 20; i++)
			{
				mob.InfoMob.ArrTuongKhac[i] += Value;
			}
		}

		public static void AddValueNeTranh(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[21] += Value;
		}

		public static void AddValueChinhXac(Mob mob, short Value)
		{
			mob.InfoMob.ArrTuongKhac[2] += Value;
			mob.InfoMob.ArrTuongKhac[1] += Value;
		}
	}
}
