namespace LangLa.InfoChar
{
	public class InfoChar
	{
		public int IdUser;

		public string? Name;

		public sbyte IdClass;

		public sbyte GioiTinh;

		public sbyte IdChar;

		public sbyte Rank;

		public sbyte SelectCaiTrang;

		public short MapId;

		public int TaiPhu;

		public int LevelCheTao;

		public short Speed;

		public sbyte SachChienDau;

		public sbyte PointPhanThanSach;

		public long ExpSach;

		public bool UsePhanThan;

		public int PointNap;

		public int HoatLuc;

		public short Cx;

		public short Cy;

		public sbyte Level;

		public bool IsDie;

		public short MapBackHome = -1;

		public bool IsBuyTheThang;

		public bool IsBuyTheVinhVien;

		public bool IsBuyGoiHaoHoa;

		public bool IsBuyGoiChiTon;

		public sbyte SelectDanhHieu;

		public short IdGiaToc = -1;

		public sbyte RoleGiaToc = -1;

		public short[] PointItemHokage = new short[10];

		public int PointCuongHoa;

		public int ChuyenCan;

		public bool IsActive;

		public bool IsLockCap;

		public InfoChar(sbyte select, string name)
		{
			Name = name;
			IdChar = select;
			GioiTinh = (sbyte)((select < 5) ? 1 : 0);
			Speed = 500;
			switch (select)
			{
			case 0:
			case 5:
				IdClass = 1;
				break;
			case 1:
			case 6:
				IdClass = 2;
				break;
			case 2:
			case 7:
				IdClass = 3;
				break;
			case 3:
			case 8:
				IdClass = 4;
				break;
			case 4:
				IdClass = 5;
				break;
			}
		}
	}
}
