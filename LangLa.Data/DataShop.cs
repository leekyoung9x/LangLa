using System.Collections.Generic;
using System.Text;
using LangLa.Template;

namespace LangLa.Data
{
	public static class DataShop
	{
		public static List<ShopTemplate>? ShopTemplates;

		public static readonly short[] ITEM_SHOP0 = new short[18]
		{
			22, 23, 24, 25, 26, 27, 238, 239, 240, 22,
			23, 24, 25, 26, 27, 238, 239, 240
		};

		public static readonly short[] MoneyShop0 = new short[18]
		{
			200, 400, 800, 1200, 1600, 2000, 2400, 2800, 3200, 200,
			400, 800, 1200, 1600, 2000, 2400, 2800, 3200
		};

		public static readonly string[] AoChoangStr = SetStringOptionAoChoang();

		public static readonly string[] TantoOptionStr = SetStringOptionTanto();

		public static readonly short[] MoneyAoChoang = new short[9] { 345, 575, 805, 1035, 2300, 2875, 3450, 4600, 6900 };

		public static readonly short[] BuyTinhThachBiKip = new short[3] { 420, 1680, 2520 };

		public static readonly string[] VuKhiStr = SetOptionVuKhi();

		public static readonly string[] BiKipStr = SetStringOptionBiKiep();

		public static readonly string[] DayThungStr = SetStringOptionPhuKien(0);

		public static readonly string[] MocSatStr = SetStringOptionPhuKien(1);

		public static readonly string[] TuiNhanGiaStr = SetStringOptionPhuKien(3);

		public static readonly string[] DaiTrangStr = SetStringOptionDaiTran();

		public static readonly string[] GiayStr = SetStringOptionDaiTran2();

		public static readonly string[] GiayStr2 = SetStringOptionDaiTran2(isGiay: true);

		public static readonly short[] IdShopCuaHang = new short[0];

		public static readonly short[] IdCaiTrangShop = new short[17]
		{
			292, 298, 326, 327, 429, 430, 460, 377, 374, 458,
			459, 373, 376, 431, 372, 375, 887
		};

		private static string[] SetOptionVuKhi(bool IsHokage = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			short IdTangCongHe = 22;
			short TangTanCongLenHe = 10;
			short ChinhXac = 25;
			short IdHieuUngKhongChe = 48;
			short HieuUngKhongChe = 5;
			short ChiMang = 30;
			short TanCongCobanCuaVuKhi = 35;
			short PhatHuyLucTanCongCoban = 100;
			short PhatHuyLucTanCongCoban2 = 150;
			short TanCong = 72;
			short TanCongQuai = 28;
			sbyte j = 0;
			for (int i = 0; i < 30; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("@");
				}
				if (i >= 25)
				{
					IdTangCongHe = 21;
				}
				stringBuilder.Append(2).Append(",").Append(TanCong)
					.Append(",")
					.Append(TanCong + 50)
					.Append(";")
					.Append(3)
					.Append(",")
					.Append(TanCongQuai)
					.Append(",")
					.Append(TanCongQuai + 20)
					.Append(";")
					.Append(20)
					.Append(",")
					.Append(ChinhXac)
					.Append(",")
					.Append(ChinhXac + 10)
					.Append(";")
					.Append(IdTangCongHe)
					.Append(",")
					.Append(TangTanCongLenHe)
					.Append(",")
					.Append(TangTanCongLenHe + 10)
					.Append(";")
					.Append(IdHieuUngKhongChe)
					.Append(",")
					.Append(HieuUngKhongChe)
					.Append(",")
					.Append(HieuUngKhongChe + 5);
				if (j > 0)
				{
					stringBuilder.Append(";").Append(28).Append(",")
						.Append(ChiMang)
						.Append(",")
						.Append(ChiMang + 15);
				}
				if (j > 1)
				{
					stringBuilder.Append(";").Append(31).Append(",")
						.Append(TanCongCobanCuaVuKhi)
						.Append(",")
						.Append(TanCongCobanCuaVuKhi + 30);
				}
				if (j > 2)
				{
					stringBuilder.Append(";").Append(41).Append(",")
						.Append(PhatHuyLucTanCongCoban)
						.Append(",")
						.Append(PhatHuyLucTanCongCoban + 50);
					PhatHuyLucTanCongCoban += 30;
				}
				if (j > 3)
				{
					stringBuilder.Append(";").Append(47).Append(",")
						.Append(PhatHuyLucTanCongCoban2)
						.Append(",")
						.Append(PhatHuyLucTanCongCoban2 + 50);
					PhatHuyLucTanCongCoban2 += 50;
				}
				ChinhXac += 25;
				TangTanCongLenHe += 10;
				TanCong += 50;
				TanCongQuai += 20;
				TanCongCobanCuaVuKhi += 30;
				j++;
				if (i == 5 || i == 11 || i == 17 || i == 23 || i == 29)
				{
					j = 0;
					TanCong = 72;
					TanCongQuai = 28;
					ChinhXac = 25;
					TangTanCongLenHe = 10;
					IdHieuUngKhongChe++;
					HieuUngKhongChe = 5;
					ChiMang += 15;
					TanCongCobanCuaVuKhi = 30;
					PhatHuyLucTanCongCoban = 100;
					PhatHuyLucTanCongCoban2 = 150;
					IdTangCongHe++;
				}
			}
			return stringBuilder.ToString().Split("@");
		}

		private static string[] SetStringOptionPhuKien(sbyte Type, bool IsHokage = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			short HpMp = 20;
			short IdKhangHe = 8;
			short IdKichCuongHoaKhangHe = 36;
			short KichAnKhangHeGiamSatThuong = 5;
			short HpToiDa = 100;
			byte TocDoDiChuyen = 60;
			sbyte ChiMang = 10;
			short HpToiDa2 = 20;
			sbyte TileHpToiDa = 15;
			short ValueKhangHe = 100;
			sbyte NuaGiayHoiPhucMp = 2;
			sbyte NeTranh = 10;
			sbyte KhangTatCa = 5;
			int j = 0;
			for (int i = 0; i < 30; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("@");
				}
				stringBuilder.Append(0).Append(",").Append(HpMp)
					.Append(",")
					.Append(HpMp + 10)
					.Append(";")
					.Append(1)
					.Append(",")
					.Append(HpMp)
					.Append(",")
					.Append(HpMp + 10)
					.Append(";")
					.Append(IdKhangHe)
					.Append(",")
					.Append(KichAnKhangHeGiamSatThuong)
					.Append(",")
					.Append(KichAnKhangHeGiamSatThuong + 5)
					.Append(";");
				switch (Type)
				{
				case 0:
					stringBuilder.Append(15).Append(",").Append(ChiMang)
						.Append(",")
						.Append(ChiMang + 10)
						.Append(";")
						.Append(27)
						.Append(",")
						.Append(NuaGiayHoiPhucMp);
					break;
				case 1:
					stringBuilder.Append(14).Append(",").Append(NeTranh)
						.Append(",")
						.Append(NeTranh + 10)
						.Append(";")
						.Append(27)
						.Append(",")
						.Append(NuaGiayHoiPhucMp);
					break;
				case 2:
					stringBuilder.Append(14).Append(",").Append(NeTranh)
						.Append(",")
						.Append(NeTranh + 10)
						.Append(";")
						.Append(27)
						.Append(",")
						.Append(NuaGiayHoiPhucMp);
					break;
				case 3:
					stringBuilder.Append(12).Append(",").Append(KhangTatCa)
						.Append(",")
						.Append(KhangTatCa + 5)
						.Append(";")
						.Append(27)
						.Append(",")
						.Append(NuaGiayHoiPhucMp);
					break;
				}
				if (j > 0)
				{
					stringBuilder.Append(";").Append(30).Append(",")
						.Append(HpToiDa);
					HpToiDa += 50;
				}
				if (j > 1)
				{
					stringBuilder.Append(";").Append(33).Append(",")
						.Append(TileHpToiDa);
					TileHpToiDa += 5;
				}
				if (j > 2)
				{
					stringBuilder.Append(";").Append(IdKichCuongHoaKhangHe).Append(",")
						.Append(ValueKhangHe);
					ValueKhangHe += 25;
				}
				if (j > 3)
				{
					stringBuilder.Append(";").Append(42).Append(",")
						.Append(25);
				}
				j++;
				HpMp = ((j <= 4) ? ((short)(HpMp + 20)) : ((short)(HpMp + 30)));
				HpToiDa2 += 20;
				KichAnKhangHeGiamSatThuong += 5;
				TocDoDiChuyen += 20;
				NeTranh += 10;
				NuaGiayHoiPhucMp += 2;
				KhangTatCa += 5;
				if (Type == 0 && (i == 4 || i == 9 || i == 14 || i == 19 || i == 24))
				{
					j = 0;
					IdKichCuongHoaKhangHe++;
					IdKhangHe++;
					HpMp = 20;
					KichAnKhangHeGiamSatThuong = 5;
					HpToiDa = 100;
					TileHpToiDa = 15;
					ValueKhangHe = 100;
					TocDoDiChuyen = 60;
					HpToiDa2 = 20;
					NuaGiayHoiPhucMp = 2;
				}
				else if (i == 5 || i == 11 || i == 17 || i == 23 || i == 29)
				{
					j = 0;
					IdKichCuongHoaKhangHe++;
					IdKhangHe++;
					HpMp = 20;
					KichAnKhangHeGiamSatThuong = 5;
					HpToiDa = 100;
					TileHpToiDa = 15;
					ValueKhangHe = 100;
					TocDoDiChuyen = 60;
					HpToiDa2 = 20;
					NeTranh = 10;
					NuaGiayHoiPhucMp = 2;
					KhangTatCa = 5;
				}
			}
			return stringBuilder.ToString().Split("@");
		}

		private static string[] SetStringOptionDaiTran2(bool isGiay = false, bool IsHokage = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			short HpMp = 20;
			short IdKhangHe = 8;
			short IdKichCuongHoaKhangHe = 36;
			short KichAnKhangHeGiamSatThuong = 5;
			sbyte KichCuongHoaHoiPhucHp = 2;
			short HpToiDa = 100;
			byte TocDoDiChuyen = 60;
			short HpToiDa2 = 20;
			sbyte TileHpToiDa = 15;
			short ValueKhangHe = 100;
			int j = 0;
			for (int i = 0; i < 30; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("@");
				}
				stringBuilder.Append(0).Append(",").Append(HpMp)
					.Append(",")
					.Append(HpMp + 10)
					.Append(";")
					.Append(1)
					.Append(",")
					.Append(HpMp)
					.Append(",")
					.Append(HpMp + 10)
					.Append(";")
					.Append(IdKhangHe)
					.Append(",")
					.Append(KichAnKhangHeGiamSatThuong)
					.Append(",")
					.Append(KichAnKhangHeGiamSatThuong + 5)
					.Append(";");
				if (!isGiay)
				{
					stringBuilder.Append(18).Append(",").Append(HpToiDa2)
						.Append(",")
						.Append(HpToiDa2 + 10)
						.Append(";")
						.Append(17)
						.Append(",")
						.Append(TocDoDiChuyen);
				}
				else
				{
					stringBuilder.Append(13).Append(",").Append(KichAnKhangHeGiamSatThuong)
						.Append(",")
						.Append(KichAnKhangHeGiamSatThuong + 5)
						.Append(";")
						.Append(26)
						.Append(",")
						.Append(KichCuongHoaHoiPhucHp);
				}
				if (j > 0)
				{
					stringBuilder.Append(";").Append(29).Append(",")
						.Append(HpToiDa);
					HpToiDa += 50;
				}
				if (j > 1)
				{
					stringBuilder.Append(";").Append(32).Append(",")
						.Append(TileHpToiDa);
					TileHpToiDa += 5;
				}
				if (j > 2)
				{
					stringBuilder.Append(";").Append(IdKichCuongHoaKhangHe).Append(",")
						.Append(ValueKhangHe);
					ValueKhangHe += 25;
				}
				if (j > 3)
				{
					stringBuilder.Append(";").Append(42).Append(",")
						.Append(25);
				}
				j++;
				HpMp = ((j <= 4) ? ((short)(HpMp + 20)) : ((short)(HpMp + 30)));
				HpToiDa2 += 20;
				KichCuongHoaHoiPhucHp += 2;
				KichAnKhangHeGiamSatThuong += 5;
				TocDoDiChuyen += 20;
				if (i == 5 || i == 11 || i == 17 || i == 23 || i == 29)
				{
					j = 0;
					IdKichCuongHoaKhangHe++;
					IdKhangHe++;
					HpMp = 20;
					KichCuongHoaHoiPhucHp = 2;
					KichAnKhangHeGiamSatThuong = 5;
					HpToiDa = 100;
					TileHpToiDa = 15;
					ValueKhangHe = 100;
					TocDoDiChuyen = 60;
					HpToiDa2 = 20;
				}
			}
			return stringBuilder.ToString().Split("@");
		}

		private static string[] SetStringOptionDaiTran(bool IsHokage = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			short HpMp = 20;
			short IdKhangHe = 8;
			short IdKichCuongHoaKhangHe = 36;
			short KichAnKhangHeGiamSatThuong = 5;
			sbyte KichCuongHoaHoiPhucHp = 2;
			short HpToiDa = 100;
			sbyte TileHpToiDa = 15;
			short ValueKhangHe = 100;
			int j = 0;
			for (int i = 0; i < 25; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("@");
				}
				stringBuilder.Append(0).Append(",").Append(HpMp)
					.Append(",")
					.Append(HpMp + 10)
					.Append(";")
					.Append(1)
					.Append(",")
					.Append(HpMp)
					.Append(",")
					.Append(HpMp + 10)
					.Append(";")
					.Append(IdKhangHe)
					.Append(",")
					.Append(KichAnKhangHeGiamSatThuong)
					.Append(",")
					.Append(KichAnKhangHeGiamSatThuong + 5)
					.Append(";")
					.Append(13)
					.Append(",")
					.Append(KichAnKhangHeGiamSatThuong)
					.Append(",")
					.Append(KichAnKhangHeGiamSatThuong + 5)
					.Append(";")
					.Append(26)
					.Append(",")
					.Append(KichCuongHoaHoiPhucHp);
				if (j > 0)
				{
					stringBuilder.Append(";").Append(29).Append(",")
						.Append(HpToiDa);
					HpToiDa += 50;
				}
				if (j > 1)
				{
					stringBuilder.Append(";").Append(32).Append(",")
						.Append(TileHpToiDa);
					TileHpToiDa += 5;
				}
				if (j > 2)
				{
					stringBuilder.Append(";").Append(IdKichCuongHoaKhangHe).Append(",")
						.Append(ValueKhangHe);
					ValueKhangHe += 25;
				}
				if (j > 3)
				{
					stringBuilder.Append(";").Append(42).Append(",")
						.Append(25);
				}
				j++;
				HpMp += 20;
				KichCuongHoaHoiPhucHp += 2;
				KichAnKhangHeGiamSatThuong += 5;
				if (i == 4 || i == 9 || i == 14 || i == 19 || i == 24)
				{
					j = 0;
					IdKichCuongHoaKhangHe++;
					IdKhangHe++;
					HpMp = 20;
					KichCuongHoaHoiPhucHp = 2;
					KichAnKhangHeGiamSatThuong = 5;
					HpToiDa = 100;
					TileHpToiDa = 15;
					ValueKhangHe = 100;
				}
			}
			return stringBuilder.ToString().Split("@");
		}

		private static string[] SetStringOptionAo()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 25; i++)
			{
			}
			return stringBuilder.ToString().Split("@");
		}

		private static string[] SetStringOptionTanto()
		{
			StringBuilder stringBuilder = new StringBuilder();
			short IdKichHe = -1;
			short NeTranhKhangAll = 70;
			short IdTangTanCongLeHe = -1;
			short IdKhangHe = -1;
			short TangTanCongLenHe = 200;
			short KhangHe = 30;
			short valueAdd = 20;
			short Tancong = 50;
			short ChinhXac = 60;
			sbyte GaySuyYeu = 6;
			short IdGaySuyYey = -1;
			short j = 0;
			for (int i = 0; i < 15; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("@");
				}
				if (i < 4)
				{
					IdKichHe = 131;
					IdTangTanCongLeHe = 154;
					IdKhangHe = 109;
					IdGaySuyYey = 168;
				}
				else if (i < 7)
				{
					IdKichHe = 132;
					IdTangTanCongLeHe = 155;
					IdKhangHe = 110;
					IdGaySuyYey = 169;
				}
				else if (i < 10)
				{
					IdKichHe = 133;
					IdTangTanCongLeHe = 156;
					IdKhangHe = 111;
					IdGaySuyYey = 170;
				}
				else if (i < 13)
				{
					IdKichHe = 134;
					IdTangTanCongLeHe = 157;
					IdKhangHe = 112;
					IdGaySuyYey = 171;
				}
				else
				{
					IdKichHe = 135;
					IdTangTanCongLeHe = 153;
					IdKhangHe = 108;
					IdGaySuyYey = 172;
				}
				stringBuilder.Append(IdKichHe).Append(",").Append(0)
					.Append(";")
					.Append(151)
					.Append(",")
					.Append(NeTranhKhangAll)
					.Append(",")
					.Append(NeTranhKhangAll + ((i == 2 || i == 5 || i == 8 || i == 11 || i == 14) ? 20 : 10))
					.Append(";")
					.Append(152)
					.Append(",")
					.Append(NeTranhKhangAll)
					.Append(",")
					.Append(NeTranhKhangAll + ((i == 2 || i == 5 || i == 8 || i == 11 || i == 14) ? 20 : 10))
					.Append(";")
					.Append(IdTangTanCongLeHe)
					.Append(",")
					.Append(TangTanCongLenHe)
					.Append(",")
					.Append(TangTanCongLenHe + valueAdd)
					.Append(";")
					.Append(158)
					.Append(",")
					.Append(3)
					.Append(";")
					.Append(IdKhangHe)
					.Append(",")
					.Append(KhangHe)
					.Append(",")
					.Append(KhangHe + 20);
				if (j >= 3)
				{
					j = 0;
					NeTranhKhangAll = 20;
					valueAdd = 50;
					GaySuyYeu = 6;
					Tancong = 50;
					TangTanCongLenHe = 200;
					KhangHe = 30;
				}
				else
				{
					if (j == 1 || j == 2)
					{
						stringBuilder.Append(";").Append(2).Append(",")
							.Append(Tancong)
							.Append(",")
							.Append(Tancong + 50)
							.Append(";")
							.Append(167)
							.Append(",")
							.Append(ChinhXac)
							.Append(",")
							.Append(ChinhXac + 20)
							.Append(";")
							.Append(IdGaySuyYey)
							.Append(",")
							.Append(GaySuyYeu)
							.Append(",")
							.Append(GaySuyYeu + 2);
						GaySuyYeu += 2;
						Tancong += 50;
					}
					if (j == 2)
					{
						NeTranhKhangAll += 30;
						KhangHe += 20;
						valueAdd += 50;
					}
					else
					{
						KhangHe += 30;
						NeTranhKhangAll += 20;
						valueAdd += 20;
					}
					TangTanCongLenHe += 50;
				}
				stringBuilder.Append(";").Append(148).Append(",")
					.Append(0);
				j++;
			}
			return stringBuilder.ToString().Split("@");
		}

		private static string[] SetStringOptionBiKiep()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int DoTuLuyen = 4000;
			short HpMp = 150;
			short KhangHe = 20;
			short Khache = 60;
			short IdKhacHe = 7;
			short IdKhangHe = 113;
			for (int i = 0; i < 15; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("@");
				}
				if (i < 4)
				{
					IdKhangHe = 109;
					IdKhacHe = 114;
				}
				else if (i < 7)
				{
					IdKhangHe = 110;
					IdKhacHe = 115;
				}
				else if (i < 10)
				{
					IdKhangHe = 111;
					IdKhacHe = 116;
				}
				else if (i < 13)
				{
					IdKhangHe = 112;
					IdKhacHe = 117;
				}
				else if (i < 16)
				{
					IdKhangHe = 108;
					IdKhacHe = 113;
				}
				stringBuilder.Append(128).Append(",").Append(0)
					.Append(",")
					.Append(DoTuLuyen)
					.Append(";")
					.Append(0)
					.Append(",")
					.Append(HpMp)
					.Append(",")
					.Append(HpMp + 50)
					.Append(";")
					.Append(1)
					.Append(",")
					.Append(HpMp)
					.Append(",")
					.Append(HpMp + 50)
					.Append(";")
					.Append(IdKhangHe)
					.Append(",")
					.Append(KhangHe)
					.Append(",")
					.Append(KhangHe + 10)
					.Append(";")
					.Append(IdKhacHe)
					.Append(",")
					.Append(Khache)
					.Append(",")
					.Append(Khache + 20);
				DoTuLuyen *= 2;
				HpMp *= 2;
				KhangHe *= 3;
				Khache *= 2;
				if (i == 2 || i == 5 || i == 8 || i == 11 || i == 14)
				{
					DoTuLuyen = 4000;
					HpMp = 150;
					KhangHe = 20;
					Khache = 60;
				}
			}
			return stringBuilder.ToString().Split("@");
		}

		private static string[] SetStringOptionAoChoang()
		{
			StringBuilder stringBuilder = new StringBuilder();
			short NuaGiayHpMp = 30;
			short Hp = 900;
			short TileHpMpToiDa = 75;
			short KhangTatCa = 140;
			short PhatHuyLucTanCongCoban = 50;
			sbyte More = 6;
			sbyte BoQuaKhangTinh = 2;
			sbyte GiamTanCongKhiBiChiMang = 5;
			for (int i = 0; i < 15; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("@");
				}
				stringBuilder.Append(136).Append(",").Append(NuaGiayHpMp)
					.Append(",")
					.Append(NuaGiayHpMp + 5)
					.Append(";")
					.Append(137)
					.Append(",")
					.Append(NuaGiayHpMp)
					.Append(",")
					.Append(NuaGiayHpMp + ((i > 4) ? 10 : 5))
					.Append(";");
				if (i >= 6)
				{
					stringBuilder.Append(149).Append(",").Append(BoQuaKhangTinh)
						.Append(",")
						.Append(BoQuaKhangTinh + 1)
						.Append(";")
						.Append(174)
						.Append(",")
						.Append(GiamTanCongKhiBiChiMang)
						.Append(",")
						.Append(GiamTanCongKhiBiChiMang + 1);
					if (i == 8)
					{
						stringBuilder.Append(311).Append(",").Append(140)
							.Append(",")
							.Append(150)
							.Append(";")
							.Append(346)
							.Append(",")
							.Append(200)
							.Append(",")
							.Append(210)
							.Append(";");
					}
					BoQuaKhangTinh++;
					GiamTanCongKhiBiChiMang++;
				}
				stringBuilder.Append(0).Append(",").Append(Hp)
					.Append(",")
					.Append(Hp + ((i > 4) ? 200 : 100))
					.Append(";")
					.Append(119)
					.Append(",")
					.Append(TileHpMpToiDa)
					.Append(",")
					.Append(TileHpMpToiDa + 15)
					.Append(";")
					.Append(120)
					.Append(",")
					.Append(TileHpMpToiDa)
					.Append(",")
					.Append(TileHpMpToiDa + 15)
					.Append(";")
					.Append(121)
					.Append(",")
					.Append(KhangTatCa)
					.Append(",")
					.Append(KhangTatCa + 20)
					.Append(";")
					.Append(122)
					.Append(",")
					.Append(PhatHuyLucTanCongCoban)
					.Append(",")
					.Append(PhatHuyLucTanCongCoban + ((i >= 7) ? 20 : 10))
					.Append(";");
				for (int j = 68; j <= 72; j++)
				{
					if (j > 68)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(j).Append(",").Append(More)
						.Append(",")
						.Append(More + 2);
				}
				NuaGiayHpMp = ((i <= 4) ? ((short)(NuaGiayHpMp + 5)) : ((short)(NuaGiayHpMp + 10)));
				TileHpMpToiDa += 15;
				if (i >= 6)
				{
					Hp += 300;
					KhangTatCa += 40;
					PhatHuyLucTanCongCoban += 20;
				}
				else
				{
					KhangTatCa += 30;
					PhatHuyLucTanCongCoban += 10;
					Hp += 100;
				}
				More += 2;
			}
			return stringBuilder.ToString().Split("@");
		}
	}
}
