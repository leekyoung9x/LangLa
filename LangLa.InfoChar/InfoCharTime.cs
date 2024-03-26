using System;

namespace LangLa.InfoChar
{
	public class InfoCharTime
	{
		public bool[] IsCanNhanQuaOnlie = new bool[8];

		public bool[] IsCanBuyRankShop = new bool[10];

		public bool[] IsCanNhanQuaOnline7Day = new bool[7];

		public bool[] IsCanNhanQuaThangCap = new bool[8];

		public bool[] IsCanNhanQuaTieuNgay = new bool[7];

		public bool[] IsCanNhanQuaTieuTuan = new bool[7];

		public bool[] IsCanNhanQuaNapNgay = new bool[6];

		public bool[] IsCanNhanQuaNapTuan = new bool[9];

		public bool[] IsCanNhanQuaNapLienTuc = new bool[7];

		public bool[] IsCanNhanQuaNap3Moc = new bool[3];

		public bool[] IsCanNhanQuaNapDon = new bool[5];

		public bool[] IsCanNhanQuaRank = new bool[10];

		public bool[] IsCanNhanQuaRankChung = new bool[6];

		public bool[] IsCanNhanQuaRankTatCa = new bool[7];

		public bool[] IsCanNhanQuaGoiHaoHoa = new bool[12];

		public bool[] IsCanNhanQuaGoiChiTon = new bool[12];

		public bool[] IsCanNhanQuaDauTuTatCa = new bool[4];

		public bool[] IsCanNhanQuaTheThangTatCa = new bool[4];

		public bool[] IsCanBuyShopKhuRank = new bool[10];

		public string TimeCreateAccout = DateTime.Now.ToString();

		public string TimeInGame;

		public int TimeOnline;

		public int SoVangNapTrongNgay;

		public int SoVangNapTrongTuan;

		public int SoVangTieuTrongNgay;

		public int SoVangTieuTrongTuan;

		public int SoVangNap3Moc;

		public short SoNgayNapLienTuc;

		public short DayOnlineLienTuc;

		public int TongSoVangDaNap;

		public int SoVangHienCo;

		public long TimeNap;

		public sbyte SoLanDiCamThuat;

		public int SoVangNapDon;

		public bool IsNhanQuaKichHoat;

		public bool[] IsCanUseLenhBaiCamThuat = new bool[2];

		public long LastTimeUseSkillSusanoCaiTrang;

		public InfoCharTime()
		{
			SoLanDiCamThuat = 1;
			TimeInGame = DateTime.Now.ToString();
		}

		public int getMinutes(bool IsTrue)
		{
			return (short)(TimeOnline + DateTime.Now.Subtract(Convert.ToDateTime(TimeInGame)).Minutes);
		}

		public short getDay(bool IsTrue)
		{
			return (short)(DayOnlineLienTuc + DateTime.Now.Subtract(Convert.ToDateTime(TimeInGame)).Days);
		}

		public void SetLogOut()
		{
			TimeOnline = getMinutes(IsTrue: true);
			DayOnlineLienTuc = getDay(IsTrue: true);
			DateTime aDateTime = DateTime.Now;
			Console.WriteLine(TimeInGame);
			DateTime y2K = Convert.ToDateTime(TimeInGame.ToString().Split(" ")[0]);
			int Day = (aDateTime - y2K).Days;
			if (Day > 0)
			{
				SoLanDiCamThuat = 1;
				for (int n = 0; n < IsCanNhanQuaOnlie.Length; n++)
				{
					IsCanNhanQuaOnlie[n] = false;
				}
				for (int m = 0; m < IsCanNhanQuaNapNgay.Length; m++)
				{
					IsCanNhanQuaNapNgay[m] = false;
				}
				for (int l = 0; l < IsCanUseLenhBaiCamThuat.Length; l++)
				{
					IsCanUseLenhBaiCamThuat[l] = false;
				}
				SoVangNapTrongNgay = 0;
				TimeOnline = 0;
				if (Day == 1)
				{
					for (int k = 0; k < IsCanNhanQuaTieuNgay.Length; k++)
					{
						IsCanNhanQuaTieuNgay[k] = false;
					}
					SoVangTieuTrongNgay = 0;
					DayOnlineLienTuc++;
				}
				else
				{
					SoNgayNapLienTuc--;
					DayOnlineLienTuc--;
					if (DayOnlineLienTuc <= 0)
					{
						DayOnlineLienTuc = 1;
					}
				}
				if (Day >= 7)
				{
					for (int j = 0; j < IsCanNhanQuaTieuTuan.Length; j++)
					{
						IsCanNhanQuaTieuTuan[j] = false;
					}
					for (int i = 0; i < IsCanNhanQuaNapTuan.Length; i++)
					{
						IsCanNhanQuaNapTuan[i] = false;
					}
					SoVangNapTrongTuan = 0;
					SoVangTieuTrongTuan = 0;
				}
			}
			TimeInGame = aDateTime.ToString();
		}

		public void SetTime()
		{
			DateTime aDateTime = DateTime.Now;
			Console.WriteLine(TimeInGame);
			DateTime y2K = Convert.ToDateTime(TimeInGame.Split(" ")[0]);
			int Day = (aDateTime - y2K).Days;
			if (Day > 0)
			{
				SoLanDiCamThuat = 1;
				for (int n = 0; n < IsCanNhanQuaOnlie.Length; n++)
				{
					IsCanNhanQuaOnlie[n] = false;
				}
				for (int m = 0; m < IsCanNhanQuaNapNgay.Length; m++)
				{
					IsCanNhanQuaNapNgay[m] = false;
				}
				for (int l = 0; l < IsCanUseLenhBaiCamThuat.Length; l++)
				{
					IsCanUseLenhBaiCamThuat[l] = false;
				}
				SoVangNapTrongNgay = 0;
				TimeOnline = 0;
				if (Day == 1)
				{
					for (int k = 0; k < IsCanNhanQuaTieuNgay.Length; k++)
					{
						IsCanNhanQuaTieuNgay[k] = false;
					}
					SoVangTieuTrongNgay = 0;
					DayOnlineLienTuc++;
				}
				else
				{
					SoNgayNapLienTuc--;
					DayOnlineLienTuc--;
					if (DayOnlineLienTuc <= 0)
					{
						DayOnlineLienTuc = 1;
					}
				}
				if (Day >= 7)
				{
					for (int j = 0; j < IsCanNhanQuaTieuTuan.Length; j++)
					{
						IsCanNhanQuaTieuTuan[j] = false;
					}
					for (int i = 0; i < IsCanNhanQuaNapTuan.Length; i++)
					{
						IsCanNhanQuaNapTuan[i] = false;
					}
					SoVangNapTrongTuan = 0;
					SoVangTieuTrongTuan = 0;
				}
			}
			TimeInGame = aDateTime.ToString();
		}
	}
}
