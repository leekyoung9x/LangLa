using System;
using LangLa.Data;
using LangLa.Hander;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.InfoChar
{
	public class InfoTuongKhac
	{
		public int DameCoBan;

		public int DameLenQuai;

		public short PhatHuyLucDanhCoban;

		public short ChinhXac;

		public short BoQuaNeTranh;

		public int ChiMang;

		public int ValueAddDameFromChiMang;

		public bool IsMaxCrit;

		public short TangDameChiMang;

		public short TangDameTatCa;

		public short TangTanCongLenLoi;

		public short TangTanCongLenTho;

		public short TangTanCongLenThuy;

		public short TangTanCongLenHoa;

		public short TangTanCongLenPhong;

		public short GaySuyYeu;

		public short GayTrungDoc;

		public short GayLamCham;

		public short GayBong;

		public short GayChoang;

		public short BoQuaKhangTinh;

		public short KhangLoi;

		public short KhangTho;

		public short KhangThuy;

		public short KhangHoa;

		public short KhangPhong;

		public short GiamSatThuong;

		public short NeTranh;

		public short PhongChiMang;

		public short TileTuongKhac;

		public short TileGiamTuongKhac;

		public short GiamSuyYeu;

		public short GiamTrungDoc;

		public short GiamLamCham;

		public short GiamGayBong;

		public short GiamGayChoang;

		public short GiamChiMang;

		public short SatThuongChuyenThanhHoiPhucHp;

		public short PhanTramKinhNghiemDanhQuai;

		public short PhanTramPhanDon;

		public short PhanTramBoQuaKhangTinh;

		public short PhanTramgTileChoangNuaGiay;

		public short PhanTramgSatThuongChuyenThanhHoiHp;

		public short PhanTramSatThuongChieu;

		public short PhanTramgKinhNghiemHoTroDongDoi;

		public short ChakaraFromItem;

		public int NuaGiayHoiHp;

		public int NuaGiayHoiMp;

		public string OptionSachChienDau = "207,0;208,0";

		public void Update(Character _myChar)
		{
			int Hp = NuaGiayHoiHp + _myChar.InfoGame.SkillGiaTocMoiNuaGiayHoiHp;
			if (Hp > 0 && Util.CurrentTimeMillis() > _myChar.InfoGame.LastTimeNuaGiayHoiHp)
			{
				int HpFull = _myChar.Point.GetHpFull(_myChar);
				if (_myChar.Point.Hp < HpFull)
				{
					PointHander.UpdateHpChar(_myChar, Hp);
				}
				_myChar.InfoGame.LastTimeNuaGiayHoiHp = Util.CurrentTimeMillis() + 500;
			}
			int Mp = NuaGiayHoiMp + _myChar.InfoGame.SkillGiaTocMoiNuaGiayHoiMp;
			if (Mp > 0 && Util.CurrentTimeMillis() > _myChar.InfoGame.LastTimeNuaGiayHoiMp)
			{
				int MpFull = _myChar.Point.GetMpFull();
				if (_myChar.Point.Mp < MpFull)
				{
					PointHander.UpdateMpChar(_myChar, Mp);
				}
				_myChar.InfoGame.LastTimeNuaGiayHoiMp = Util.CurrentTimeMillis() + 500;
			}
		}

		public void GetPointFromSkill(Character Char, Skill skill)
		{
			if (skill == null)
			{
				return;
			}
			string[] Value = skill.Options.Split(";");
			if (skill.Level > 1)
			{
				Skill SkillBackPoint = InfoSkill.GetSkillAndLevel(skill.IdTemplate, (sbyte)(skill.Level - 1));
				string[] Value2 = SkillBackPoint.Options.Split(";");
				switch (Char.Info.IdClass)
				{
				case 1:
					Char.Point.HpFull -= short.Parse(Value2[0].Split(",")[1]);
					PhanTramPhanDon -= short.Parse(Value2[1].Split(",")[1]);
					Char.Info.Speed -= short.Parse(Value2[2].Split(",")[1]);
					TileTuongKhac -= short.Parse(Value2[3].Split(",")[1]);
					Char.TuongKhac.TangTanCongLenTho -= short.Parse(Value2[3].Split(",")[1]);
					break;
				case 2:
					if (skill.IdTemplate == 3)
					{
						ChiMang -= short.Parse(Value2[0].Split(",")[1]);
						ChinhXac -= short.Parse(Value2[1].Split(",")[1]);
						Char.Info.Speed -= short.Parse(Value2[2].Split(",")[1]);
						TileTuongKhac -= short.Parse(Value2[3].Split(",")[1]);
						Char.TuongKhac.TangTanCongLenThuy -= short.Parse(Value2[3].Split(",")[1]);
					}
					break;
				case 3:
					PhanTramKinhNghiemDanhQuai -= short.Parse(Value2[0].Split(",")[1]);
					PhanTramgKinhNghiemHoTroDongDoi -= short.Parse(Value2[1].Split(",")[1]);
					Char.Point.PhanTramHpToiDa -= short.Parse(Value2[2].Split(",")[1]);
					Char.Info.Speed -= short.Parse(Value2[3].Split(",")[1]);
					TileTuongKhac -= short.Parse(Value2[4].Split(",")[1]);
					Char.TuongKhac.TangTanCongLenHoa -= short.Parse(Value2[4].Split(",")[1]);
					break;
				case 4:
				{
					Char.Point.PhanTramMpToiDa -= short.Parse(Value2[0].Split(",")[1]);
					short KhangAll2 = short.Parse(Value2[1].Split(",")[1]);
					KhangPhong -= KhangAll2;
					KhangLoi -= KhangAll2;
					KhangTho -= KhangAll2;
					KhangThuy -= KhangAll2;
					KhangHoa -= KhangAll2;
					Char.Info.Speed -= short.Parse(Value2[2].Split(",")[1]);
					short Value3 = short.Parse(Value2[3].Split(",")[1]);
					TileTuongKhac -= Value3;
					Char.TuongKhac.TangTanCongLenPhong -= Value3;
					break;
				}
				case 5:
					DameCoBan -= short.Parse(Value2[0].Split(",")[1]);
					BoQuaNeTranh -= short.Parse(Value2[1].Split(",")[1]);
					Char.Info.Speed -= short.Parse(Value2[2].Split(",")[1]);
					TileTuongKhac -= short.Parse(Value2[3].Split(",")[1]);
					Char.TuongKhac.TangTanCongLenLoi -= short.Parse(Value2[3].Split(",")[1]);
					break;
				}
			}
			switch (Char.Info.IdClass)
			{
			case 1:
				Char.Point.HpFull += short.Parse(Value[0].Split(",")[1]);
				PhanTramPhanDon += short.Parse(Value[1].Split(",")[1]);
				Char.Info.Speed += short.Parse(Value[2].Split(",")[1]);
				TileTuongKhac += short.Parse(Value[3].Split(",")[1]);
				Char.TuongKhac.TangTanCongLenTho += short.Parse(Value[3].Split(",")[1]);
				PointHander.UpdateHpFullChar(Char);
				break;
			case 2:
				if (skill.IdTemplate == 3)
				{
					ChiMang += short.Parse(Value[0].Split(",")[1]);
					ChinhXac += short.Parse(Value[1].Split(",")[1]);
					Char.Info.Speed += short.Parse(Value[2].Split(",")[1]);
					TileTuongKhac += short.Parse(Value[3].Split(",")[1]);
					Char.TuongKhac.TangTanCongLenThuy += short.Parse(Value[3].Split(",")[1]);
				}
				break;
			case 3:
				PhanTramKinhNghiemDanhQuai += short.Parse(Value[0].Split(",")[1]);
				PhanTramgKinhNghiemHoTroDongDoi += short.Parse(Value[1].Split(",")[1]);
				Char.Point.PhanTramHpToiDa += short.Parse(Value[2].Split(",")[1]);
				Char.Info.Speed += short.Parse(Value[3].Split(",")[1]);
				TileTuongKhac += short.Parse(Value[4].Split(",")[1]);
				Char.TuongKhac.TangTanCongLenHoa += short.Parse(Value[4].Split(",")[1]);
				PointHander.UpdateHpFullChar(Char);
				break;
			case 4:
			{
				Char.Point.PhanTramMpToiDa += short.Parse(Value[0].Split(",")[1]);
				short KhangAll = short.Parse(Value[1].Split(",")[1]);
				KhangPhong += KhangAll;
				KhangLoi += KhangAll;
				KhangTho += KhangAll;
				KhangThuy += KhangAll;
				KhangHoa += KhangAll;
				Char.Info.Speed += short.Parse(Value[2].Split(",")[1]);
				TileTuongKhac += short.Parse(Value[3].Split(",")[1]);
				Char.TuongKhac.TangTanCongLenPhong += short.Parse(Value[3].Split(",")[1]);
				PointHander.UpdateMpFullChar(Char);
				break;
			}
			case 5:
				DameCoBan += short.Parse(Value[0].Split(",")[1]);
				BoQuaNeTranh += short.Parse(Value[1].Split(",")[1]);
				Char.Info.Speed += short.Parse(Value[2].Split(",")[1]);
				TileTuongKhac += short.Parse(Value[3].Split(",")[1]);
				Char.TuongKhac.TangTanCongLenLoi += short.Parse(Value[3].Split(",")[1]);
				break;
			}
			if (ChiMang < 0)
			{
				ChiMang = 0;
			}
		}

		private static int[] ArrIntType(sbyte Type)
		{
			return Type switch
			{
				0 => new int[2] { 3, 4 }, 
				2 => new int[2] { 7, 8 }, 
				3 => new int[2] { 0, 4 }, 
				4 => new int[2] { 0, 3 }, 
				5 => new int[2] { 6, 9 }, 
				6 => new int[2] { 5, 9 }, 
				7 => new int[2] { 2, 8 }, 
				8 => new int[2] { 2, 7 }, 
				9 => new int[2] { 6, 5 }, 
				_ => null, 
			};
		}

		private static Item a(int var0, int var1, Character var2)
		{
			if (var1 == 0)
			{
				for (var1 = 0; var1 < var2.Inventory.ItemBody.Count; var1++)
				{
					if (var2.Inventory.ItemBody[var1] != null && var2.Inventory.ItemBody[var1].Type == var0)
					{
						return var2.Inventory.ItemBody[var1];
					}
				}
			}
			else
			{
				for (var1 = 0; var1 < var2.Inventory.ItemBody2.Count; var1++)
				{
					if (var2.Inventory.ItemBody2[var1] != null && var2.Inventory.ItemBody2[var1].Type == var0)
					{
						return var2.Inventory.ItemBody2[var1];
					}
				}
			}
			return null;
		}

		private static bool CheckCanAddValue(Character _myChar, Item item, sbyte TypeOption, ref sbyte Count)
		{
			switch (TypeOption)
			{
			case 2:
			{
				if (item.Type == 1)
				{
					return item.Type == 1 && (_myChar.Info.IdClass == item.IdClass || (_myChar.Inventory.ItemBody[15] != null && _myChar.Inventory.ItemBody[15].IdClass == _myChar.Info.IdClass && _myChar.Info.IdClass == item.IdClass));
				}
				if (item.Type == 15)
				{
					return item.Type == 15 && _myChar.Inventory.ItemBody[1] != null && _myChar.Inventory.ItemBody[1].IdClass == _myChar.Info.IdClass;
				}
				int[] Arr = ArrIntType(item.Type);
				for (sbyte i = 0; i < Arr.Length; i++)
				{
					Item item2;
					if (Count != i && (item2 = a(Arr[i], 0, _myChar)) != null && (item2.IdClass == _myChar.Info.IdClass || item2.Id == 881))
					{
						Count = i;
						return true;
					}
				}
				return false;
			}
			case 3:
				return item.Level >= 4;
			case 4:
				return item.Level >= 8;
			case 5:
				return item.Level >= 12;
			case 6:
				return item.Level >= 14;
			case 7:
				return item.Level >= 16;
			case 10:
				return item.Level >= 17;
			case 11:
				return item.Level >= 18;
			case 16:
				return item.Level >= 19;
			case 19:
				return item.Level >= 16;
			default:
				return true;
			}
		}

		public void DownPointKichAn(Character _myChar, Item item)
		{
			int[] ArrIndex = ArrIntType(item.Type);
			if (ArrIndex == null)
			{
				return;
			}
			for (int i = 0; i < ArrIndex.Length; i++)
			{
				if (_myChar.Inventory.ItemBody[ArrIndex[i]] != null)
				{
					GetPointFromItem(_myChar.Inventory.ItemBody[ArrIndex[i]], _myChar, IsDownPoint: true, null, IsDownKichAn: true);
				}
			}
		}

		public void UpPointKichAn(Character _myChar, Item item)
		{
			int[] ArrIndex = ArrIntType(item.Type);
			if (ArrIndex == null)
			{
				return;
			}
			for (int i = 0; i < ArrIndex.Length; i++)
			{
				if (_myChar.Inventory.ItemBody[ArrIndex[i]] != null)
				{
					GetPointFromItem(_myChar.Inventory.ItemBody[ArrIndex[i]], _myChar, IsDownPoint: false, null, IsDownKichAn: true);
				}
			}
		}

		private short GetMaxDoTuLuyen(int Id)
		{
			return Id switch
			{
				_ => 0, 
			};
		}

		public void AddOptionBiKip(Character _myChar, Item item)
		{
			ItemOption[] itemOptions = item.L(IsSet: true);
			for (int i = 0; i < itemOptions.Length; i++)
			{
			}
		}

		public void GetPointFromSkillGiaToc(Character _myChar, string Options, bool IsDownPoint = false)
		{
			string[] ArrOptions = Options.Split(";");
			int Hp = _myChar.InfoGame.SkillGiaTocHp;
			string[] array = ArrOptions;
			foreach (string Op in array)
			{
				string[] ValueAll = Op.Split(",");
				short Id = short.Parse(ValueAll[0]);
				int check = int.Parse(ValueAll[1]);
				int Value = ((!IsDownPoint) ? check : (check * -1));
				switch (Id)
				{
				case 0:
					_myChar.InfoGame.SkillGiaTocHp += Value;
					break;
				case 2:
					_myChar.InfoGame.SkillGiaTocTanCong += Value;
					break;
				case 3:
					_myChar.InfoGame.SKillGiaTocDameQuai += Value;
					break;
				case 108:
					_myChar.InfoGame.SkillGiaTocKhangLoi += (short)Value;
					break;
				case 109:
					_myChar.InfoGame.SkillGiaTocKhangTho += (short)Value;
					break;
				case 110:
					_myChar.InfoGame.SkillGiaTocKhangThuy += (short)Value;
					break;
				case 111:
					_myChar.InfoGame.SkillGiaTocKhangHoa += (short)Value;
					break;
				case 112:
					_myChar.InfoGame.SkillGiaTocKhangPhong += (short)Value;
					break;
				case 113:
					_myChar.InfoGame.SkillGiaTocTangTanCongLenLoi += (short)Value;
					break;
				case 114:
					_myChar.InfoGame.SkillGiaTocTangTanCongLenTho += (short)Value;
					break;
				case 115:
					_myChar.InfoGame.SkillGiaTocTangTanCongLenThuy += (short)Value;
					break;
				case 116:
					_myChar.InfoGame.SkillGiaTocTangTanCongLenHoa += (short)Value;
					break;
				case 117:
					_myChar.InfoGame.SkillGiaTocTangTanCongLenPhong += (short)Value;
					break;
				case 123:
					_myChar.InfoGame.SkillGiaTocGaySuyYeu += (short)Value;
					break;
				case 124:
					_myChar.InfoGame.SkillGiaTocGayTrungDoc += (short)Value;
					break;
				case 125:
					_myChar.InfoGame.SkillGiaTocGayLamCham += (short)Value;
					break;
				case 126:
					_myChar.InfoGame.SkillGiaTocGayBong += (short)Value;
					break;
				case 127:
					_myChar.InfoGame.SkillGiaTocGayChoang += (short)Value;
					break;
				case 136:
					_myChar.InfoGame.SkillGiaTocMoiNuaGiayHoiHp += Value;
					break;
				case 137:
					_myChar.InfoGame.SkillGiaTocMoiNuaGiayHoiMp += Value;
					break;
				case 149:
					_myChar.InfoGame.SkillGiaTocBoQuaKhangTinh += (short)Value;
					break;
				case 152:
					_myChar.InfoGame.SkillGiaTocKhangTatCa += (short)Value;
					break;
				case 161:
					_myChar.InfoGame.SkillGiaTocNeTranh += (short)Value;
					break;
				case 166:
					_myChar.InfoGame.SkillGiaTocChiMang += (short)Value;
					break;
				case 167:
					_myChar.InfoGame.SKillGiaTocChinhXac += (short)Value;
					break;
				case 173:
					_myChar.InfoGame.SkillGiaTocGiamSatThuong += (short)Value;
					break;
				}
			}
			if (Hp != _myChar.InfoGame.SkillGiaTocHp)
			{
				PointHander.UpdateHpFullChar(_myChar);
			}
		}

		public void GetPointFromItem(Item it, Character _myChar, bool IsDownPoint = false, string OptionBack = null, bool IsDownKichAn = false)
		{
			string Op2 = ((OptionBack != null) ? OptionBack : it.Options);
			string[] Options = Op2.Split(";");
			if (Options.Length == 0 || (it != null && it.Options.Equals("") && Options == null))
			{
				return;
			}
			short Chakara = 0;
			int Hp = 0;
			int Mp = 0;
			short TileHpToiDa = 0;
			short TileMpToiDa = 0;
			short TangPhanTramDame = 0;
			sbyte IdClass = _myChar.Info.IdClass;
			sbyte CountKichAn = -1;
			string[] array = Options;
			foreach (string Op in array)
			{
				string[] ValueAll = Op.Split(",");
				short Id = -1;
				try
				{
					Id = short.Parse(ValueAll[0]);
				}
				catch (Exception e)
				{
					if (it != null)
					{
						Util.ShowWarring("ITEM LOI " + DataServer.ArrItemTemplate[it.Id].name + " OPTIONS " + it.Options);
					}
					else
					{
						Util.ShowWarring(" OPTIONS LOI " + OptionBack);
					}
					Util.ShowErr(e);
				}
				if (it != null)
				{
					ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[Id];
					if (!CheckCanAddValue(_myChar, it, itemOptionTemplate.Type, ref CountKichAn) || (IsDownKichAn && itemOptionTemplate.Type != 2))
					{
						continue;
					}
				}
				int check = int.Parse(ValueAll[1]);
				int Value = ((!IsDownPoint) ? check : (check * -1));
				switch (Id)
				{
				case 34:
				case 47:
				case 122:
				case 361:
					PhatHuyLucDanhCoban += (short)Value;
					break;
				case 32:
				case 79:
				case 119:
					TileHpToiDa += (short)Value;
					break;
				case 33:
				case 80:
				case 120:
					TileMpToiDa += (short)Value;
					break;
				case 0:
				case 18:
				case 29:
				case 106:
				case 175:
				case 192:
				case 202:
				case 253:
					Hp += Value;
					break;
				case 1:
				case 19:
				case 30:
				case 107:
					Mp += Value;
					break;
				case 4:
					BoQuaNeTranh += (short)Value;
					break;
				case 5:
				case 15:
				case 28:
				case 63:
				case 144:
				case 166:
				case 203:
				case 362:
					ChiMang += Value;
					break;
				case 6:
					SatThuongChuyenThanhHoiPhucHp += (short)Value;
					break;
				case 2:
				case 31:
				case 78:
				case 146:
				case 184:
				case 199:
				case 208:
					DameCoBan += Value;
					break;
				case 3:
				case 207:
					DameLenQuai += Value;
					break;
				case 7:
				case 35:
				case 58:
				case 82:
				case 108:
					if (IdClass == 2)
					{
						TileGiamTuongKhac += (short)Value;
					}
					KhangLoi += (short)Value;
					break;
				case 8:
				case 36:
				case 59:
				case 83:
				case 109:
					if (IdClass == 3)
					{
						TileGiamTuongKhac += (short)Value;
					}
					KhangTho += (short)Value;
					break;
				case 9:
				case 37:
				case 60:
				case 84:
				case 110:
					if (IdClass == 4)
					{
						TileGiamTuongKhac += (short)Value;
					}
					KhangThuy += (short)Value;
					break;
				case 10:
				case 38:
				case 61:
				case 85:
				case 111:
					if (IdClass == 5)
					{
						TileGiamTuongKhac += (short)Value;
					}
					KhangHoa += (short)Value;
					break;
				case 11:
				case 39:
				case 62:
				case 86:
				case 112:
					if (IdClass == 1)
					{
						TileGiamTuongKhac += (short)Value;
					}
					KhangPhong += (short)Value;
					break;
				case 12:
				case 40:
				case 81:
				case 121:
				case 152:
				case 201:
				case 258:
					TileGiamTuongKhac += (short)Value;
					KhangLoi += (short)Value;
					KhangTho += (short)Value;
					KhangThuy += (short)Value;
					KhangHoa += (short)Value;
					KhangPhong += (short)Value;
					break;
				case 13:
					GiamSatThuong += (short)Value;
					break;
				case 14:
					NeTranh += (short)Value;
					if (NeTranh < 0)
					{
						NeTranh = 0;
					}
					break;
				case 20:
					ChinhXac += (short)Value;
					if (ChinhXac < 0)
					{
						ChinhXac = 0;
					}
					break;
				case 21:
				case 53:
				case 113:
				case 153:
				case 350:
					if (IdClass == 5)
					{
						TileTuongKhac += (short)Value;
					}
					TangTanCongLenLoi += (short)Value;
					break;
				case 22:
				case 54:
				case 114:
				case 154:
				case 351:
					if (IdClass == 1)
					{
						TileTuongKhac += (short)Value;
					}
					TangTanCongLenTho += (short)Value;
					break;
				case 23:
				case 55:
				case 115:
				case 155:
				case 352:
					if (IdClass == 2)
					{
						TileTuongKhac += (short)Value;
					}
					TangTanCongLenThuy += (short)Value;
					break;
				case 24:
				case 56:
				case 116:
				case 156:
				case 353:
					if (IdClass == 3)
					{
						TileTuongKhac += (short)Value;
					}
					TangTanCongLenHoa += (short)Value;
					break;
				case 25:
				case 57:
				case 117:
				case 157:
				case 354:
					if (IdClass == 3)
					{
						TileTuongKhac += (short)Value;
					}
					TangTanCongLenPhong += (short)Value;
					break;
				case 209:
				case 255:
					Chakara += (short)Value;
					ChakaraFromItem += (short)Value;
					break;
				case 149:
					BoQuaKhangTinh += (short)Value;
					break;
				case 41:
				case 95:
				case 306:
				case 309:
					TangDameChiMang += (short)Value;
					break;
				case 42:
				case 43:
				case 44:
				case 45:
				case 46:
				case 174:
					PhongChiMang += (short)Value;
					break;
				case 344:
				case 346:
				case 348:
					GiamChiMang += (short)Value;
					break;
				case 48:
				case 68:
				case 123:
				case 168:
				case 185:
				case 259:
					GaySuyYeu += (short)Value;
					break;
				case 49:
				case 69:
				case 124:
				case 169:
				case 186:
				case 260:
					GayTrungDoc += (short)Value;
					break;
				case 50:
				case 70:
				case 125:
				case 170:
				case 187:
				case 261:
					GayLamCham += (short)Value;
					break;
				case 51:
				case 71:
				case 126:
				case 171:
				case 188:
				case 262:
					GayBong += (short)Value;
					break;
				case 52:
				case 72:
				case 127:
				case 172:
				case 189:
				case 263:
					GayChoang += (short)Value;
					break;
				case 289:
				case 355:
					GiamSuyYeu += (short)Value;
					break;
				case 290:
				case 356:
					GiamTrungDoc += (short)Value;
					break;
				case 291:
				case 357:
					GiamLamCham += (short)Value;
					break;
				case 292:
				case 358:
					GiamGayBong += (short)Value;
					break;
				case 293:
				case 359:
					GiamGayChoang += (short)Value;
					break;
				case 26:
				case 136:
				case 143:
				case 200:
				case 256:
					NuaGiayHoiHp += (short)Value;
					break;
				case 27:
				case 137:
				case 257:
					NuaGiayHoiMp += (short)Value;
					break;
				}
				_myChar.Info.TaiPhu += Value;
			}
			if (ChiMang < 0)
			{
				ChiMang = 0;
			}
			if (IsMaxCrit && ChiMang <= 3000)
			{
				int Value3 = ChiMang + 3000;
				DameCoBan -= ValueAddDameFromChiMang * 2;
				ValueAddDameFromChiMang = 0;
				IsMaxCrit = false;
			}
			if (ChiMang > 3000)
			{
				IsMaxCrit = true;
				if (ValueAddDameFromChiMang > 0 && IsDownPoint)
				{
					DameCoBan -= ValueAddDameFromChiMang * 2;
					ValueAddDameFromChiMang = 0;
				}
				int Value2 = ChiMang - 3000;
				DameCoBan += Value2 * 2;
				ValueAddDameFromChiMang += Value2;
			}
			if (Chakara > 0 || Chakara < 0 || Hp > 0 || Hp < 0 || Mp > 0 || Mp < 0 || TileHpToiDa < 0 || TileHpToiDa > 0 || TileMpToiDa < 0 || TileMpToiDa > 0)
			{
				if (Chakara > 0)
				{
					sbyte PointCrit = _myChar.Point.PointCheckCrit;
					while (Chakara > 0)
					{
						_myChar.Point.HpFull += 2;
						_myChar.Point.MpFull += 2;
						DameCoBan++;
						if (PointCrit == 2)
						{
							ChiMang++;
						}
						NeTranh++;
						Chakara--;
						PointCrit++;
						if (PointCrit == 3)
						{
							PointCrit = 0;
						}
					}
					_myChar.Point.PointCheckCrit = PointCrit;
				}
				else
				{
					sbyte PointCrit2 = _myChar.Point.PointCheckCrit;
					while (Chakara < 0)
					{
						_myChar.Point.HpFull -= 2;
						_myChar.Point.MpFull -= 2;
						DameCoBan--;
						if (PointCrit2 == 0)
						{
							ChiMang--;
						}
						NeTranh--;
						Chakara++;
						PointCrit2--;
						if (PointCrit2 < 0)
						{
							PointCrit2 = 2;
						}
					}
					_myChar.Point.PointCheckCrit = PointCrit2;
				}
				_myChar.Point.PhanTramHpToiDa += TileHpToiDa;
				_myChar.Point.PhanTramMpToiDa += TileMpToiDa;
				_myChar.Point.HpFull += Hp;
				_myChar.Point.MpFull += Mp;
				PointHander.UpdateHpFullChar(_myChar);
				PointHander.UpdateMpFullChar(_myChar);
				_myChar.WriteInfo();
			}
			_myChar.SendMessage(UtilMessage.UpdatePointMore(_myChar.Info.IdUser, _myChar.InfoGame.LevelPk, _myChar.Info.TaiPhu, GetSpeedChar(_myChar), _myChar.InfoGame.StatusGD));
		}

		public void UpdateChakra(Character _myChar, short Chakara)
		{
			if (Chakara > 0)
			{
				sbyte PointCrit2 = _myChar.Point.PointCheckCrit;
				while (Chakara > 0)
				{
					_myChar.Point.HpFull += 2;
					_myChar.Point.MpFull += 2;
					DameCoBan++;
					if (PointCrit2 == 2)
					{
						ChiMang++;
					}
					NeTranh++;
					Chakara--;
					PointCrit2++;
					if (PointCrit2 == 3)
					{
						PointCrit2 = 0;
					}
				}
				_myChar.Point.PointCheckCrit = PointCrit2;
			}
			else
			{
				sbyte PointCrit = _myChar.Point.PointCheckCrit;
				while (Chakara < 0)
				{
					_myChar.Point.HpFull -= 2;
					_myChar.Point.MpFull -= 2;
					DameCoBan--;
					if (PointCrit == 0)
					{
						ChiMang--;
					}
					NeTranh--;
					Chakara++;
					PointCrit--;
					if (PointCrit < 0)
					{
						PointCrit = 2;
					}
					if (ChiMang < 0)
					{
						ChiMang = 0;
					}
				}
				_myChar.Point.PointCheckCrit = PointCrit;
				if (_myChar.Point.Hp > _myChar.Point.HpFull)
				{
					_myChar.Point.Hp = _myChar.Point.HpFull;
				}
				if (_myChar.Point.Mp > _myChar.Point.MpFull)
				{
					_myChar.Point.Mp = _myChar.Point.MpFull;
				}
			}
			PointHander.UpdateHpFullChar(_myChar);
			PointHander.UpdateMpFullChar(_myChar);
		}

		public int GetDameAttackMob(Character _myChar, sbyte IdClassMob, int Dame, bool IsTuongKhac)
		{
			int Dame2 = Dame;
			Dame2 += GetDameCoBan(_myChar) + GetDameLenQuai(_myChar);
			if (IdClassMob == 1)
			{
				Dame2 += GetTangTanCongLenLoi(_myChar);
			}
			if (IdClassMob == 2)
			{
				Dame2 += GetTangTanCongLenTho(_myChar);
			}
			if (IdClassMob == 3)
			{
				Dame2 += GetTangTanCongLenThuy(_myChar);
			}
			if (IdClassMob == 4)
			{
				Dame2 += GetTangTanCongLenHoa(_myChar);
			}
			if (IdClassMob == 4)
			{
				Dame2 += GetTangTanCongLenPhong(_myChar);
			}
			if (IsTuongKhac)
			{
				Dame2 += GetTileTuongKhac(_myChar);
				Dame2 = (int)((double)Dame2 * 1.25);
			}
			return (Dame2 > 0) ? Dame2 : 0;
		}

		public int GetDameAttackChar(Character _myChar, Character _cAnDame, int Dame)
		{
			int Dame2 = Dame;
			sbyte IdClassC1 = _myChar.Info.IdClass;
			sbyte IdClassC2 = _cAnDame.Info.IdClass;
			Dame2 += GetDameCoBan(_myChar);
			if (SkillHander.IsTuongKhac(IdClassC1, IdClassC2))
			{
				Dame2 += Dame2 * 25 / 100;
				Dame2 += GetTileTuongKhac(_myChar);
				Dame2 -= _cAnDame.TuongKhac.GetGiamTileTuongKhac(_cAnDame);
			}
			switch (IdClassC2)
			{
			case 1:
				Dame2 += TangTanCongLenLoi;
				break;
			case 2:
				Dame2 += TangTanCongLenTho;
				break;
			case 3:
				Dame2 += TangTanCongLenThuy;
				break;
			case 4:
				Dame2 += TangTanCongLenHoa;
				break;
			case 5:
				Dame2 += TangTanCongLenPhong;
				break;
			}
			if (Util.NextInt(0, 100) < GetBoQuaKhangTinh(_myChar))
			{
				return Dame2;
			}
			int KhangLoi = _cAnDame.TuongKhac.GetKhangLoi(_cAnDame);
			int KhangTho = _cAnDame.TuongKhac.GetKhangTho(_cAnDame);
			int KhangThuy = _cAnDame.TuongKhac.GetKhangThuy(_cAnDame);
			int KhangHoa = _cAnDame.TuongKhac.GetKhangHoa(_cAnDame);
			int KhangPhong = _cAnDame.TuongKhac.GetKhangPhong(_cAnDame);
			switch (IdClassC2)
			{
			case 1:
				Dame2 -= ((KhangLoi > 0) ? (KhangLoi * 4) : KhangLoi);
				break;
			case 2:
				Dame2 -= ((KhangTho > 0) ? (KhangTho * 4) : KhangTho);
				break;
			case 3:
				Dame2 -= ((KhangThuy > 0) ? (KhangThuy * 4) : KhangThuy);
				break;
			case 4:
				Dame2 -= ((KhangHoa > 0) ? (KhangHoa * 4) : KhangHoa);
				break;
			case 5:
				Dame2 -= ((KhangPhong > 0) ? (KhangPhong * 4) : KhangPhong);
				break;
			}
			return Dame2;
		}

		public void DownPointHieuUng(Character _myChar, ref int GayChoang, ref int GayLamCham, ref int GayBong, ref int GaySuyYeu, ref int GayTrungDoc)
		{
			GayChoang -= GetPointGiamGayChoang(_myChar);
			GayLamCham -= GetPointGiamLamCham(_myChar);
			GayBong -= GetPointGiamGayBong(_myChar);
			GaySuyYeu -= GetPointGiamSuyYeu(_myChar);
			GayTrungDoc -= GetPointGiamTrungDoc(_myChar);
		}

		public bool GetCritAttackChar(ref int Dame, Character _myChar, Character _cAnDame)
		{
			int Cm = ChiMang;
			bool IsCrit = Util.NextInt(0, 4200) < Cm;
			if (IsCrit)
			{
				short DameCrit = GetDameKhiCrit(_myChar, TangDameChiMang);
				int DameUp = DameCrit - _cAnDame.TuongKhac.PhongChiMang;
				if (DameUp > 0)
				{
					Dame += Dame * 50 / 100;
					Dame += Dame * DameCrit / 100;
				}
				else
				{
					Dame += Dame * 50 / 100;
				}
				if (_cAnDame.TuongKhac.GiamChiMang > 0)
				{
					Dame -= _cAnDame.TuongKhac.GiamChiMang * 5;
				}
			}
			int GiamSatThuong = _cAnDame.TuongKhac.GetGiamSatThuong(_cAnDame);
			Dame -= ((GiamSatThuong > 0) ? Util.NextInt(GiamSatThuong, (int)((double)GiamSatThuong * 2.5)) : GiamSatThuong);
			return IsCrit;
		}

		public int GetDameIsCrit(Character _myChar, int Dame)
		{
			return Dame;
		}

		public short GetDameKhiCrit(Character _myChar, short Dame)
		{
			short DameCrit = Dame;
			if (_myChar.InfoGame.UseSkillDoiHutMau)
			{
				DameCrit += _myChar.InfoGame.TangDameChiMangDoiHutMau;
			}
			return DameCrit;
		}

		public int GetDameCoBan(Character _myChar)
		{
			int Dame = DameCoBan;
			if (PhatHuyLucDanhCoban > 0)
			{
				Dame += Dame * PhatHuyLucDanhCoban / 100;
			}
			if (_myChar.InfoGame.UseSkillChimYeuThuat)
			{
				Dame += _myChar.InfoGame.DameChimYeuThuat;
			}
			if (_myChar.InfoGame.UseSusanoFromCaiTrang)
			{
				Dame += _myChar.InfoGame.DameAndChinhXacSusanoCaiTrang;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				Dame += _myChar.InfoGame.SkillGiaTocTanCong;
			}
			if (_myChar.InfoGame.IsSuyYeu)
			{
				Dame /= 2;
			}
			return Dame;
		}

		public short GetPointSuyYeu(Character _myChar)
		{
			short GSY = GaySuyYeu;
			if (_myChar.InfoGame.GaySuyYeu != 0)
			{
				GSY += (short)_myChar.InfoGame.GaySuyYeu;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				GSY += _myChar.InfoGame.SkillGiaTocGaySuyYeu;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointTrungDoc(Character _myChar)
		{
			short GSY = GayTrungDoc;
			if (_myChar.InfoGame.GayTrungDoc != 0)
			{
				GSY += (short)_myChar.InfoGame.GayTrungDoc;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				GSY += _myChar.InfoGame.SkillGiaTocGayTrungDoc;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointLamCham(Character _myChar)
		{
			short GSY = GayLamCham;
			if (_myChar.InfoGame.GayLamCham != 0)
			{
				GSY += (short)_myChar.InfoGame.GayLamCham;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				GSY += _myChar.InfoGame.SkillGiaTocGayLamCham;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointGayBong(Character _myChar)
		{
			short GSY = GayBong;
			if (_myChar.InfoGame.GayBong != 0)
			{
				GSY += (short)_myChar.InfoGame.GayBong;
			}
			if (_myChar.InfoGame.IsUseAnThanChiThuat)
			{
				GSY += _myChar.InfoGame.GayBongSkill;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				GSY += _myChar.InfoGame.SkillGiaTocGayLamCham;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointGayChoang(Character _myChar)
		{
			short GSY = GayChoang;
			if (_myChar.InfoGame.GayChoang != 0)
			{
				GSY += (short)_myChar.InfoGame.GayChoang;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				GSY += _myChar.InfoGame.SkillGiaTocGayChoang;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointGiamSuyYeu(Character _myChar)
		{
			short GSY = GiamSuyYeu;
			if (_myChar.InfoGame.GiamSuyYeu != 0)
			{
				GSY += (short)_myChar.InfoGame.GiamSuyYeu;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointGiamTrungDoc(Character _myChar)
		{
			short GSY = GiamTrungDoc;
			if (_myChar.InfoGame.GiamTrungDoc != 0)
			{
				GSY += (short)_myChar.InfoGame.GiamTrungDoc;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointGiamLamCham(Character _myChar)
		{
			short GSY = GiamLamCham;
			if (_myChar.InfoGame.GiamLamCham != 0)
			{
				GSY += (short)_myChar.InfoGame.GiamLamCham;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointGiamGayBong(Character _myChar)
		{
			short GSY = GiamGayBong;
			if (_myChar.InfoGame.GiamGayBong != 0)
			{
				GSY += (short)_myChar.InfoGame.GiamGayBong;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetPointGiamGayChoang(Character _myChar)
		{
			short GSY = GiamGayChoang;
			if (_myChar.InfoGame.GiamGayChoang != 0)
			{
				GSY += (short)_myChar.InfoGame.GiamGayChoang;
			}
			if (GSY > short.MaxValue || GSY < 0)
			{
				GSY = 0;
			}
			return GSY;
		}

		public short GetSpeedChar(Character _myChar)
		{
			short Speed = _myChar.Info.Speed;
			if (_myChar.InfoGame.IsLamCham)
			{
				Speed -= _myChar.InfoGame.Speed;
			}
			if (_myChar.InfoGame.IsUseAnThanChiThuat)
			{
				Speed += _myChar.InfoGame.SpeedPhanThanChiThuat;
			}
			return Speed;
		}

		public short GetChinhXac(Character _myChar)
		{
			short ChinhXac2 = ChinhXac;
			if (_myChar.InfoGame.IsUseByaKugan)
			{
				ChinhXac2 += _myChar.InfoGame.ChinhXacByaKugan;
			}
			if (_myChar.InfoGame.IsDinhLoaToanLienThuLiKiem)
			{
				ChinhXac2 += _myChar.InfoGame.ChinhXacLoaToanLienThuLiKiem;
			}
			if (_myChar.InfoGame.IsUseByaKuganThuy)
			{
				ChinhXac2 += _myChar.InfoGame.ChinhXacByaKuganThuy;
			}
			if (_myChar.InfoGame.IsUseAnThanChiThuat)
			{
				ChinhXac2 += _myChar.InfoGame.ChinhXacAnThanChiThuat;
			}
			if (_myChar.InfoGame.UseSusanoFromCaiTrang)
			{
				ChinhXac2 += (short)_myChar.InfoGame.DameAndChinhXacSusanoCaiTrang;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				ChinhXac2 += _myChar.InfoGame.SKillGiaTocChinhXac;
			}
			if (ChinhXac2 < 0)
			{
				ChinhXac2 = 0;
			}
			return ChinhXac2;
		}

		public short GetBoQuaKhangTinh(Character _myChar)
		{
			short BoQuaKhangTinh2 = BoQuaKhangTinh;
			if (_myChar.InfoGame.UseTrangThaiHienNhan)
			{
				BoQuaKhangTinh2 += _myChar.InfoGame.BoQuaKhangTinhHienNhan;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				BoQuaKhangTinh2 += _myChar.InfoGame.SkillGiaTocBoQuaKhangTinh;
			}
			return BoQuaKhangTinh2;
		}

		public short GetNeTranh(Character _myChar)
		{
			short NeTranh2 = NeTranh;
			if (_myChar.InfoGame.UseAnPhanThanChiThuat)
			{
				NeTranh2 += _myChar.InfoGame.NeTranhPhanThanChiThuat;
			}
			if (_myChar.InfoGame.IsDinhAnChuChiThuat)
			{
				NeTranh2 += _myChar.InfoGame.TrietTieuNeTranhGiamSatThuongAnChuChiThuat;
			}
			if (_myChar.InfoGame.IsDinhThienChieu)
			{
				NeTranh2 += _myChar.InfoGame.SuyGiamNeTranhThienChieu;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				NeTranh2 += _myChar.InfoGame.SkillGiaTocNeTranh;
			}
			if (NeTranh2 < 0)
			{
				NeTranh2 = 0;
			}
			return NeTranh2;
		}

		private short GetKhangLoi(Character _myChar)
		{
			short KhangLoi2 = KhangLoi;
			if (_myChar.InfoGame.IsDinhAnChuChiThuat)
			{
				KhangLoi2 += _myChar.InfoGame.GiamKhangTatCaAnChuChiThuat;
			}
			if (_myChar.InfoGame.IsSuyYeu)
			{
				KhangLoi2 /= 2;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				KhangLoi2 += _myChar.InfoGame.SkillGiaTocKhangLoi;
				KhangLoi2 += _myChar.InfoGame.SkillGiaTocKhangTatCa;
			}
			if (KhangLoi2 < 0)
			{
				KhangLoi2 = 0;
			}
			return KhangLoi2;
		}

		private short GetKhangTho(Character _myChar)
		{
			short KhangTho2 = KhangTho;
			if (_myChar.InfoGame.IsDinhAnChuChiThuat)
			{
				KhangTho2 += _myChar.InfoGame.GiamKhangTatCaAnChuChiThuat;
			}
			if (_myChar.InfoGame.IsSuyYeu)
			{
				KhangTho2 /= 2;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				KhangTho2 += _myChar.InfoGame.SkillGiaTocKhangTho;
				KhangTho2 += _myChar.InfoGame.SkillGiaTocKhangTatCa;
			}
			if (KhangTho2 < 0)
			{
				KhangTho2 = 0;
			}
			return KhangTho2;
		}

		private short GetKhangThuy(Character _myChar)
		{
			short KhangThuy2 = KhangThuy;
			if (_myChar.InfoGame.IsDinhAnChuChiThuat)
			{
				KhangThuy2 += _myChar.InfoGame.GiamKhangTatCaAnChuChiThuat;
			}
			if (_myChar.InfoGame.IsSuyYeu)
			{
				KhangThuy2 /= 2;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				KhangThuy2 += _myChar.InfoGame.SkillGiaTocKhangThuy;
				KhangThuy2 += _myChar.InfoGame.SkillGiaTocKhangTatCa;
			}
			if (KhangThuy2 < 0)
			{
				KhangThuy2 = 0;
			}
			return KhangThuy2;
		}

		private short GetKhangHoa(Character _myChar)
		{
			short KhangHoa2 = KhangHoa;
			if (_myChar.InfoGame.IsDinhAnChuChiThuat)
			{
				KhangHoa2 += _myChar.InfoGame.GiamKhangTatCaAnChuChiThuat;
			}
			if (_myChar.InfoGame.IsSuyYeu)
			{
				KhangHoa2 /= 2;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				KhangHoa2 += _myChar.InfoGame.SkillGiaTocKhangHoa;
				KhangHoa2 += _myChar.InfoGame.SkillGiaTocKhangTatCa;
			}
			if (KhangHoa2 < 0)
			{
				KhangHoa2 = 0;
			}
			return KhangHoa2;
		}

		private short GetKhangPhong(Character _myChar)
		{
			short KhangPhong2 = KhangPhong;
			if (_myChar.InfoGame.IsDinhAnChuChiThuat)
			{
				KhangPhong2 += _myChar.InfoGame.GiamKhangTatCaAnChuChiThuat;
			}
			if (_myChar.InfoGame.IsSuyYeu)
			{
				KhangPhong2 /= 2;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				KhangPhong2 += _myChar.InfoGame.SkillGiaTocKhangPhong;
				KhangPhong2 += _myChar.InfoGame.SkillGiaTocKhangTatCa;
			}
			if (KhangPhong2 < 0)
			{
				KhangPhong2 = 0;
			}
			return KhangPhong2;
		}

		private short GetGiamSatThuong(Character _myChar)
		{
			short GiamSatThuong2 = GiamSatThuong;
			if (_myChar.InfoGame.IsDinhAnChuChiThuat)
			{
				GiamSatThuong2 += _myChar.InfoGame.TrietTieuNeTranhGiamSatThuongAnChuChiThuat;
			}
			if (_myChar.Info.IdGiaToc != -1)
			{
				GiamSatThuong2 += _myChar.InfoGame.SkillGiaTocGiamSatThuong;
			}
			if (GiamSatThuong2 < 0)
			{
				GiamSatThuong2 = 0;
			}
			return GiamSatThuong2;
		}

		private short GetGiamTileTuongKhac(Character _myChar)
		{
			short GiamTileTuongKhac2 = TileGiamTuongKhac;
			if (_myChar.Info.IdGiaToc != -1)
			{
				switch (_myChar.Info.IdClass)
				{
				case 1:
					GiamTileTuongKhac2 += _myChar.InfoGame.SkillGiaTocKhangPhong;
					break;
				case 2:
					GiamTileTuongKhac2 += _myChar.InfoGame.SkillGiaTocKhangLoi;
					break;
				case 3:
					GiamTileTuongKhac2 += _myChar.InfoGame.SkillGiaTocKhangTho;
					break;
				case 4:
					GiamTileTuongKhac2 += _myChar.InfoGame.SkillGiaTocKhangThuy;
					break;
				case 5:
					GiamTileTuongKhac2 += _myChar.InfoGame.SkillGiaTocKhangHoa;
					break;
				}
				GiamTileTuongKhac2 += _myChar.InfoGame.SkillGiaTocKhangTatCa;
			}
			if (_myChar.InfoGame.IsSuyYeu)
			{
				GiamTileTuongKhac2 /= 2;
			}
			if (GiamTileTuongKhac2 < 0)
			{
				GiamTileTuongKhac2 = 0;
			}
			return GiamTileTuongKhac2;
		}

		private short GetTangTanCongLenLoi(Character _myChar)
		{
			short TangTanCongLenLoi2 = TangTanCongLenLoi;
			if (TangTanCongLenLoi2 < 0)
			{
				TangTanCongLenLoi2 = 0;
			}
			return TangTanCongLenLoi2;
		}

		private short GetTangTanCongLenTho(Character _myChar)
		{
			short TangTanCongLenTho2 = TangTanCongLenLoi;
			if (_myChar.Info.IdGiaToc != -1)
			{
				TangTanCongLenTho2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenTho;
			}
			if (TangTanCongLenTho2 < 0)
			{
				TangTanCongLenTho2 = 0;
			}
			return TangTanCongLenTho2;
		}

		private short GetTangTanCongLenThuy(Character _myChar)
		{
			short TangTanCongLenThuy2 = TangTanCongLenThuy;
			if (_myChar.Info.IdGiaToc != -1)
			{
				TangTanCongLenThuy2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenThuy;
			}
			if (TangTanCongLenThuy2 < 0)
			{
				TangTanCongLenThuy2 = 0;
			}
			return TangTanCongLenThuy2;
		}

		private short GetTangTanCongLenHoa(Character _myChar)
		{
			short TangTanCongLenHoa2 = TangTanCongLenHoa;
			if (_myChar.Info.IdGiaToc != -1)
			{
				TangTanCongLenHoa2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenHoa;
			}
			if (TangTanCongLenHoa2 < 0)
			{
				TangTanCongLenHoa2 = 0;
			}
			return TangTanCongLenHoa2;
		}

		private short GetTangTanCongLenPhong(Character _myChar)
		{
			short TangTanCongLenPhong2 = TangTanCongLenPhong;
			if (_myChar.Info.IdGiaToc != -1)
			{
				TangTanCongLenPhong2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenPhong;
			}
			if (TangTanCongLenPhong2 < 0)
			{
				TangTanCongLenPhong2 = 0;
			}
			return TangTanCongLenPhong2;
		}

		private short GetTileTuongKhac(Character _myChar)
		{
			short TileTuongKhac2 = TileTuongKhac;
			if (_myChar.Info.IdGiaToc != -1)
			{
				switch (_myChar.Info.IdClass)
				{
				case 1:
					TileTuongKhac2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenTho;
					break;
				case 2:
					TileTuongKhac2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenThuy;
					break;
				case 3:
					TileTuongKhac2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenHoa;
					break;
				case 4:
					TileTuongKhac2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenPhong;
					break;
				case 5:
					TileTuongKhac2 += _myChar.InfoGame.SkillGiaTocTangTanCongLenLoi;
					break;
				}
			}
			if (TileTuongKhac2 < 0)
			{
				TileTuongKhac2 = 0;
			}
			return TileTuongKhac2;
		}

		public int GetDameLenQuai(Character _myChar)
		{
			int DameLenQuai2 = DameLenQuai;
			if (_myChar.Info.IdGiaToc != -1)
			{
				DameLenQuai2 += _myChar.InfoGame.SKillGiaTocDameQuai;
			}
			if (DameLenQuai2 < 0)
			{
				DameLenQuai2 = 0;
			}
			return DameLenQuai2;
		}

		private short GetChiMang(Character _myChar)
		{
			int ChiMang2 = ChiMang;
			if (_myChar.Info.IdGiaToc != -1)
			{
				ChiMang2 += _myChar.InfoGame.SkillGiaTocChiMang;
			}
			if (ChiMang2 > 3000)
			{
				ChiMang2 = 3000;
			}
			return (short)ChiMang2;
		}

		public void Write(Character _myChar, Message msg, short Speed)
		{
			msg.WriteInt(GetDameCoBan(_myChar));
			msg.WriteInt(GetDameLenQuai(_myChar));
			msg.WriteShort(GetChinhXac(_myChar));
			msg.WriteShort(BoQuaNeTranh);
			msg.WriteShort(GetChiMang(_myChar));
			msg.WriteShort(GetDameKhiCrit(_myChar, TangDameChiMang));
			msg.WriteShort(GetTangTanCongLenLoi(_myChar));
			msg.WriteShort(GetTangTanCongLenTho(_myChar));
			msg.WriteShort(GetTangTanCongLenThuy(_myChar));
			msg.WriteShort(GetTangTanCongLenHoa(_myChar));
			msg.WriteShort(GetTangTanCongLenPhong(_myChar));
			msg.WriteShort(GetPointSuyYeu(_myChar));
			msg.WriteShort(GetPointTrungDoc(_myChar));
			msg.WriteShort(GetPointLamCham(_myChar));
			msg.WriteShort(GetPointGayBong(_myChar));
			msg.WriteShort(GetPointGayChoang(_myChar));
			msg.WriteShort(GetBoQuaKhangTinh(_myChar));
			msg.WriteShort(GetKhangLoi(_myChar));
			msg.WriteShort(GetKhangTho(_myChar));
			msg.WriteShort(GetKhangThuy(_myChar));
			msg.WriteShort(GetKhangHoa(_myChar));
			msg.WriteShort(GetKhangPhong(_myChar));
			msg.WriteShort(GetGiamSatThuong(_myChar));
			msg.WriteShort(Speed);
			msg.WriteShort(GetNeTranh(_myChar));
			msg.WriteShort(PhanTramPhanDon);
			msg.WriteShort(PhongChiMang);
			msg.WriteShort(GetTileTuongKhac(_myChar));
			msg.WriteShort(GetGiamTileTuongKhac(_myChar));
			msg.WriteShort(GetPointGiamSuyYeu(_myChar));
			msg.WriteShort(GetPointGiamTrungDoc(_myChar));
			msg.WriteShort(GetPointGiamLamCham(_myChar));
			msg.WriteShort(GetPointGiamGayBong(_myChar));
			msg.WriteShort(GetPointGiamGayChoang(_myChar));
			msg.WriteShort(GiamChiMang);
		}
	}
}
