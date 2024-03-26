using System.Linq;
using LangLa.Data;
using LangLa.EventServer;
using LangLa.Hander;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.InfoChar
{
	public class InfoInGame
	{
		public sbyte TypePk;

		public sbyte LevelPk;

		public int IdCharPk = -1;

		public bool IsCuuSat;

		public bool IsAnCuuSat;

		public int IdCuuSat;

		public int IdCharMoiCuuSat;

		public bool IsTyVo;

		public int IdTyVo;

		public sbyte Status;

		public sbyte StatusGD;

		public int IdCharGiaoDich;

		public int BacGiaoDich;

		public int VangGiaoDich;

		public bool IsLockGD;

		public Item[] ItemGD;

		public sbyte ID_Click_SHOP;

		public sbyte IdClickNpc;

		public bool IsOderMenu2;

		public sbyte IndexClickSelect;

		public Zone ZoneGame;

		public long TimeLoadMap;

		public Character CharFocus;

		public Mob MobFocus;

		public long TimeDelayMobAttack;

		public bool IsUseItemNotNpc;

		public bool IsUseItemNotNpcOder2;

		public short IndexItemNotNpc;

		public bool IsSuyYeu;

		public bool IsLamCham;

		public bool IsBong;

		public bool IsChoang;

		public int GaySuyYeu;

		public int GayBong;

		public int GayChoang;

		public int GayTrungDoc;

		public int GayLamCham;

		public bool IsUseByaKugan;

		public short ChinhXacByaKugan;

		public bool IsUseAnThanChiThuat;

		public short GayBongSkill;

		public short ChinhXacAnThanChiThuat;

		public short Speed;

		public bool UseAnPhanThanChiThuat;

		public short SpeedPhanThanChiThuat;

		public short NeTranhPhanThanChiThuat;

		public int GiamSuyYeu;

		public int GiamGayBong;

		public int GiamGayChoang;

		public bool UseSkillDoiHutMau;

		public short TangDameChiMangDoiHutMau;

		public int DameDoiHutMau;

		public long LastTimeDoiHutMauAttack;

		public bool UseSkillChimYeuThuat;

		public short DameChimYeuThuat;

		public short LamChamChimYeu;

		public long LastTimeChimYeuAttack;

		public bool UseSkillCuuViHinh;

		public short ChakraCuuViHinh;

		public sbyte TileGayChoangNuaGiayCuuViHinh;

		public bool UseTrangThaiHienNhan;

		public short BoQuaKhangTinhHienNhan;

		public short TieuHaoMpHienNhan;

		public long LastTimeTieuHaoMpHienNhan;

		public bool IsDinhThanhSatChakra;

		public bool IsDinhLoaToanLienThuLiKiem;

		public short ChinhXacLoaToanLienThuLiKiem;

		public bool IsDinhAnChuChiThuat;

		public short GiamKhangTatCaAnChuChiThuat;

		public short TrietTieuNeTranhGiamSatThuongAnChuChiThuat;

		public short GiamChakraAnChuChiThuat;

		public bool IsUseByaKuganThuy;

		public short ChinhXacByaKuganThuy;

		public bool UseChimKhongLo;

		public short HpChimKhongLo;

		public bool IsUseSusanoo;

		public short DungMpHutSatThuongSasunoo;

		public bool IsDinhThienChieu;

		public short SuyGiamNeTranhThienChieu;

		public bool IsUseBachHaoChiThuat;

		public short TimeDichChuyenChiThuatFromBachHaoChiThuat;

		public int GiamTrungDoc;

		public int GiamLamCham;

		public bool IsUseBiDuoc;

		public bool UseSusanoFromCaiTrang;

		public int DameAndChinhXacSusanoCaiTrang;

		public short ToDoiKinhNghiem;

		public int SKillGiaTocDameQuai;

		public int SkillGiaTocHp;

		public int SkillGiaTocTanCong;

		public short SkillGiaTocGiamSatThuong;

		public short SkillGiaTocNeTranh;

		public short SKillGiaTocChinhXac;

		public short SkillGiaTocChiMang;

		public short SkillGiaTocKhangTatCa;

		public short SkillGiaTocBoQuaKhangTinh;

		public short SkillGiaTocKhangPhong;

		public short SkillGiaTocGayChoang;

		public short SkillGiaTocTangTanCongLenPhong;

		public short SkillGiaTocKhangHoa;

		public short SkillGiaTocTangTanCongLenHoa;

		public short SkillGiaTocGayBong;

		public short SkillGiaTocKhangThuy;

		public short SkillGiaTocTangTanCongLenThuy;

		public short SkillGiaTocGayLamCham;

		public short SkillGiaTocKhangTho;

		public short SkillGiaTocTangTanCongLenTho;

		public short SkillGiaTocGayTrungDoc;

		public short SkillGiaTocKhangLoi;

		public short SkillGiaTocTangTanCongLenLoi;

		public short SkillGiaTocGaySuyYeu;

		public int SkillGiaTocMoiNuaGiayHoiHp;

		public int SkillGiaTocMoiNuaGiayHoiMp;

		public string NameOderToDoi;

		public bool IsDoiTruong;

		public long LastTimeNuaGiayHoiHp;

		public long LastTimeNuaGiayHoiMp;

		public InfoToDoi Todoi;

		public GiaTocTemplate GiaToc;

		public Character _CView;

		public bool IsVaoCamThuat;

		public CamThuat CamThuat;

		public Item[] CaiTrangBack;

		public void CleanUpGD(Character _cGD)
		{
			_cGD?.InfoGame.CleanUp();
			StatusGD = 0;
			IdCharGiaoDich = -1;
			BacGiaoDich = -1;
			VangGiaoDich = -1;
			IsLockGD = false;
			ItemGD = null;
		}

		public void CleanUpToDoi()
		{
			Todoi = null;
			IsDoiTruong = false;
			NameOderToDoi = null;
		}

		public void CleanUpUseItemNotNpc()
		{
			IsUseItemNotNpc = false;
			IsUseItemNotNpcOder2 = false;
			IndexItemNotNpc = -1;
		}

		public void CleanUpCuuSat()
		{
			IdCuuSat = -1;
			IdCharMoiCuuSat = -1;
			IsCuuSat = false;
			IsAnCuuSat = false;
		}

		public void CleanUpTyvo()
		{
			IdTyVo = -1;
			IsTyVo = false;
		}

		public void SetGiaToc(Character _myChar)
		{
			Character _myChar2 = _myChar;
			GiaTocManager.SetGiaTocMe(_myChar2);
			_myChar2.SendMessage(GiaTocHander.SendGiaTocMsg(_myChar2));
			GiaToc.ThanhViens.FirstOrDefault((GiaTocTemplate.ThanhVienGiaToc s) => s != null && s.IdUser == _myChar2.Info.IdUser).isOn = true;
			GiaToc.ThanhViens.FirstOrDefault((GiaTocTemplate.ThanhVienGiaToc s) => s != null && s.IdUser == _myChar2.Info.IdUser)._myChar = _myChar2;
			sbyte[] skillGiaToc = _myChar2.InfoGame.GiaToc.SkillGiaToc;
			foreach (sbyte c in skillGiaToc)
			{
				if (c != -1)
				{
					SkillClanTemplate skillClanTemplate = DataServer.ArrSkillClan[c];
					if (skillClanTemplate != null)
					{
						_myChar2.TuongKhac.GetPointFromSkillGiaToc(_myChar2, skillClanTemplate.Options);
					}
				}
			}
		}

		public void CleanUpGiaToc(int IdUser)
		{
			if (GiaToc != null)
			{
				GiaToc.ThanhViens.FirstOrDefault((GiaTocTemplate.ThanhVienGiaToc s) => s != null && s.IdUser == IdUser).isOn = false;
				GiaToc.ThanhViens.FirstOrDefault((GiaTocTemplate.ThanhVienGiaToc s) => s != null && s.IdUser == IdUser)._myChar = null;
				GiaToc = null;
			}
		}

		public void SetLevel(long exp, ref sbyte Level)
		{
			sbyte Level2 = 0;
			while (Level2 < DataServer.ArrExp.Length && exp >= DataServer.ArrExp[Level2])
			{
				exp -= DataServer.ArrExp[Level2];
				Level2++;
			}
			Level = Level2;
		}

		public void CleanUp()
		{
			TypePk = -1;
			LevelPk = -1;
			Status = -1;
			StatusGD = -1;
			ID_Click_SHOP = -1;
		}
	}
}
