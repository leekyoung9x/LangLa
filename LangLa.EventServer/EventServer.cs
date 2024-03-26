using System;
using System.Threading;

namespace LangLa.EventServer
{
	public static class EventServer
	{
		public static readonly short[] TimeOfMoney = new short[0];

		public static void Start()
		{
			Thread EventServer = new Thread((ThreadStart)delegate
			{
				while (true)
				{
					switch ((int)DateTime.Now.DayOfWeek)
					{
					case 0:
						EventSunday();
						break;
					case 1:
						EventModay();
						break;
					case 2:
						EventTuesday();
						break;
					case 3:
						EventWednesday();
						break;
					case 4:
						EventThursday();
						break;
					case 5:
						EventFriday();
						break;
					case 6:
						EventSaturday();
						break;
					}
					Thread.Sleep(1000);
				}
			});
			EventServer.Start();
		}

		private static void EventModay()
		{
			int Hour = DateTime.Now.Hour;
			switch (Hour)
			{
			case 6:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 9:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 12:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 14:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			case 15:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 18:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 19:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			}
		}

		private static void EventTuesday()
		{
			int Hour = DateTime.Now.Hour;
			switch (Hour)
			{
			case 6:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 9:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 12:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 14:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			case 15:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 18:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 19:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			}
		}

		private static void EventWednesday()
		{
			int Hour = DateTime.Now.Hour;
			switch (Hour)
			{
			case 6:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 9:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 12:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 14:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			case 15:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 18:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 19:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			}
		}

		private static void EventThursday()
		{
			int Hour = DateTime.Now.Hour;
			switch (Hour)
			{
			case 6:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 9:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 12:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 14:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			case 15:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 18:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 19:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			}
		}

		private static void EventFriday()
		{
			int Hour = DateTime.Now.Hour;
			switch (Hour)
			{
			case 6:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 9:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 12:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 14:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 19).Start();
				}
				break;
			case 15:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 18:
				break;
			case 19:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, -1).Start();
				}
				break;
			case 20:
				break;
			case 21:
				break;
			case 7:
			case 8:
			case 10:
			case 11:
			case 13:
			case 16:
			case 17:
				break;
			}
		}

		private static void EventSaturday()
		{
			int Hour = DateTime.Now.Hour;
			switch (Hour)
			{
			case 6:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 9:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 12:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 14:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			case 15:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 18:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 19:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			}
		}

		private static void EventSunday()
		{
			int Hour = DateTime.Now.Hour;
			switch (Hour)
			{
			case 6:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 9:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 12:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 14:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			case 15:
				if (!KhuRungChet.IsStartKhuRungChet && DateTime.Now.Minute == 50)
				{
					new KhuRungChet((sbyte)Hour, -1).Start();
				}
				break;
			case 19:
				if (!CaoThuNhanGia.IsStartDaiHoiNhanGia && DateTime.Now.Minute == 0)
				{
					new CaoThuNhanGia((sbyte)Hour, 14).Start();
				}
				break;
			}
		}
	}
}
