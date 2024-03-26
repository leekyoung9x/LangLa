using LangLa.OOP;
using Newtonsoft.Json;

namespace LangLa.InfoChar
{
	[JsonObject(MemberSerialization.OptIn)]
	public class InfoPoint
	{
		[JsonProperty]
		public int Hp = 100;

		[JsonProperty]
		public int HpFull = 100;

		[JsonProperty]
		public int Mp = 100;

		[JsonProperty]
		public int MpFull = 100;

		[JsonProperty]
		public long Exp;

		[JsonProperty]
		public short PhanTramMpToiDa;

		[JsonProperty]
		public short PhanTramHpToiDa;

		[JsonProperty]
		public short DiemKyNang;

		[JsonProperty]
		public short DiemTiemNang;

		[JsonProperty]
		public sbyte PointCheckCrit;

		[JsonProperty]
		public short[] ArrayPoint;

		[JsonProperty]
		public sbyte[] PointUseSachKyNang = new sbyte[3];

		[JsonProperty]
		public sbyte[] PointUseSachTiemNang = new sbyte[3];

		[JsonProperty]
		public bool[] IsCanUseNhanThuatSaoChep = new bool[2];

		[JsonProperty]
		public sbyte PointGhepCaiTrang;

		public int GetHpFull(Character _myChar)
		{
			int HpF = HpFull;
			if (_myChar.Info.IdGiaToc != -1)
			{
				HpF += _myChar.InfoGame.SkillGiaTocHp;
			}
			if (_myChar.InfoGame.UseChimKhongLo)
			{
				HpF += _myChar.InfoGame.HpChimKhongLo;
			}
			if (PhanTramHpToiDa > 0)
			{
				HpF += HpF * PhanTramHpToiDa / 100;
			}
			return HpF;
		}

		public int GetMpFull()
		{
			int MpF = MpFull;
			if (PhanTramMpToiDa > 0)
			{
				MpF += MpF * PhanTramMpToiDa / 100;
			}
			return MpF;
		}

		public InfoPoint(sbyte IdClass)
		{
			ArrayPoint = new short[4];
			if (IdClass == 1 || IdClass == 5)
			{
				ArrayPoint[0] = 10;
				ArrayPoint[1] = 0;
				ArrayPoint[2] = 5;
				ArrayPoint[3] = 5;
			}
			else
			{
				ArrayPoint[0] = 0;
				ArrayPoint[1] = 0;
				ArrayPoint[2] = 15;
				ArrayPoint[3] = 5;
			}
		}
	}
}
