using System;
using System.Threading;
using LangLa.Data;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SupportOOP;

namespace LangLa.EventServer
{
	public class CaoThuNhanGia
	{
		public static bool IsStartDaiHoiNhanGia;

		private sbyte HourStart;

		private sbyte HourEnd = 0;

		private sbyte MinuteEnd = 0;

		private sbyte HourNext;

		private Mob[] ArrCaoThuNhanGia = new Mob[5];

		private long TimeEnd;

		public CaoThuNhanGia(sbyte HourStart, sbyte HourNext)
		{
			TimeEnd = Util.CurrentTimeMillis() + 3600000;
			this.HourStart = HourStart;
			this.HourNext = HourNext;
			IsStartDaiHoiNhanGia = true;
			LangLa.Server.Server.SendThongBaoFromServer("Hoạt động cao thủ đã chính thức bắt đầu [" + DateTime.Now.ToString() + "]");
			LangLa.Server.Server.SendThongBaoFromServer("Cao thủ nhẫn giả đă bắt đầu xuất hiện ở " + DataServer.ArrMapTemplate[57].Name + ", " + DataServer.ArrMapTemplate[65].Name + "," + DataServer.ArrMapTemplate[87].Name + "," + DataServer.ArrMapTemplate[79].Name + "," + DataServer.ArrMapTemplate[73].Name + " đánh thắng sẽ có quà");
			for (int i = 0; i < 5; i++)
			{
				Mob mob = null;
				switch (i)
				{
				case 0:
					mob = new Mob(MapManager.Maps[57].Zones[Util.NextInt(0, 8)]);
					mob.IdEntity = (short)mob._Zone.Mobs.Values.Count;
					mob.Id = 199;
					mob.Cx = 958;
					mob.Cy = 443;
					mob.Level = 15;
					mob.LevelBoss = 10;
					mob.HpFull = 100000000;
					mob.Hp = mob.HpFull;
					mob.Exp = 800000;
					mob.IsCaoThuNhanGia = true;
					if (!mob._Zone.Mobs.TryAdd(mob.IdEntity, mob))
					{
						break;
					}
					lock (mob._Zone.Chars)
					{
						foreach (Character c2 in mob._Zone.Chars.Values)
						{
							if (c2.IsConnection)
							{
								Message k = new Message(1);
								mob.Write(k);
								c2.SendMessage(k);
							}
						}
					}
					break;
				case 1:
					mob = new Mob(MapManager.Maps[65].Zones[Util.NextInt(0, 8)]);
					mob.IdEntity = (short)mob._Zone.Mobs.Values.Count;
					mob.Id = 200;
					mob.Cx = 906;
					mob.Cy = 137;
					mob.Level = 25;
					mob.HpFull = 150000000;
					mob.Hp = mob.HpFull;
					mob.Exp = 1200000;
					mob.LevelBoss = 10;
					mob.IsCaoThuNhanGia = true;
					if (!mob._Zone.Mobs.TryAdd(mob.IdEntity, mob))
					{
						break;
					}
					lock (mob._Zone.Chars)
					{
						foreach (Character c4 in mob._Zone.Chars.Values)
						{
							if (c4.IsConnection)
							{
								Message n = new Message(1);
								mob.Write(n);
								c4.SendMessage(n);
							}
						}
					}
					break;
				case 2:
					mob = new Mob(MapManager.Maps[87].Zones[Util.NextInt(0, 8)]);
					mob.IdEntity = (short)mob._Zone.Mobs.Values.Count;
					mob.Id = 201;
					mob.Cx = 969;
					mob.Cy = 150;
					mob.Level = 35;
					mob.HpFull = 200000000;
					mob.Hp = mob.HpFull;
					mob.Exp = 17500000;
					mob.LevelBoss = 10;
					mob.IsCaoThuNhanGia = true;
					if (!mob._Zone.Mobs.TryAdd(mob.IdEntity, mob))
					{
						break;
					}
					lock (mob._Zone.Chars)
					{
						foreach (Character c5 in mob._Zone.Chars.Values)
						{
							if (c5.IsConnection)
							{
								Message m = new Message(1);
								mob.Write(m);
								c5.SendMessage(m);
							}
						}
					}
					break;
				case 3:
					mob = new Mob(MapManager.Maps[79].Zones[Util.NextInt(0, 8)]);
					mob.IdEntity = (short)mob._Zone.Mobs.Values.Count;
					mob.Id = 202;
					mob.Cx = 1036;
					mob.Cy = 141;
					mob.Level = 45;
					mob.HpFull = 250000000;
					mob.Hp = mob.HpFull;
					mob.Exp = 27500000;
					mob.LevelBoss = 10;
					mob.IsCaoThuNhanGia = true;
					if (!mob._Zone.Mobs.TryAdd(mob.IdEntity, mob))
					{
						break;
					}
					lock (mob._Zone.Chars)
					{
						foreach (Character c3 in mob._Zone.Chars.Values)
						{
							if (c3.IsConnection)
							{
								Message l = new Message(1);
								mob.Write(l);
								c3.SendMessage(l);
							}
						}
					}
					break;
				case 4:
					mob = new Mob(MapManager.Maps[73].Zones[Util.NextInt(0, 8)]);
					mob.IdEntity = (short)mob._Zone.Mobs.Values.Count;
					mob.Id = 203;
					mob.Cx = 751;
					mob.Cy = 178;
					mob.Level = 55;
					mob.HpFull = 300000000;
					mob.Hp = mob.HpFull;
					mob.Exp = 37500000;
					mob.LevelBoss = 10;
					mob.IsCaoThuNhanGia = true;
					if (!mob._Zone.Mobs.TryAdd(mob.IdEntity, mob))
					{
						break;
					}
					lock (mob._Zone.Chars)
					{
						foreach (Character c in mob._Zone.Chars.Values)
						{
							if (c.IsConnection)
							{
								Message j = new Message(1);
								mob.Write(j);
								c.SendMessage(j);
							}
						}
					}
					break;
				}
				ArrCaoThuNhanGia[i] = mob;
			}
			switch (HourStart)
			{
			case 9:
				HourEnd = 10;
				break;
			case 14:
				HourEnd = 15;
				break;
			case 19:
				HourEnd = 20;
				break;
			case 12:
				HourEnd = 13;
				break;
			}
		}

		public void Start()
		{
			new Thread((ThreadStart)delegate
			{
				while (IsStartDaiHoiNhanGia)
				{
					try
					{
						if (Util.CurrentTimeMillis() > TimeEnd)
						{
							IsStartDaiHoiNhanGia = false;
							LangLa.Server.Server.SendThongBaoFromServer("Hoạt động cao thủ đã kết thúc");
							if (HourNext == -1)
							{
								LangLa.Server.Server.SendThongBaoFromServer("Hẹn các cao thủ vào ngày mai");
							}
							else
							{
								LangLa.Server.Server.SendThongBaoFromServer("Hoạt động nhẫn giả tiếp theo sẽ diễn ra vào lúc " + HourNext + "h");
							}
							close();
							break;
						}
						sbyte b = 0;
						for (int i = 0; i < ArrCaoThuNhanGia.Length; i++)
						{
							if (ArrCaoThuNhanGia[i].Hp <= 0)
							{
								b++;
							}
							if (b == 5)
							{
								IsStartDaiHoiNhanGia = false;
								LangLa.Server.Server.SendThongBaoFromServer("Toàn bộ cao thủ nhẫn giả đã bị tiêu diệt Hoạt động cao thủ nhẫn giả đã kết thúc");
								if (HourNext == -1)
								{
									LangLa.Server.Server.SendThongBaoFromServer("Hẹn các cao thủ vào ngày mai");
								}
								else
								{
									LangLa.Server.Server.SendThongBaoFromServer("Hoạt động nhẫn giả tiếp theo sẽ diễn ra vào lúc " + HourNext + "h");
								}
								close();
								break;
							}
						}
						Thread.Sleep(1000);
					}
					catch (Exception e)
					{
						Util.ShowErr(e);
					}
				}
			}).Start();
		}

		private void close()
		{
			if (ArrCaoThuNhanGia == null)
			{
				return;
			}
			for (int i = 0; i < ArrCaoThuNhanGia.Length; i++)
			{
				if (ArrCaoThuNhanGia[i].Hp > 0)
				{
					Mob mob = ArrCaoThuNhanGia[i];
					foreach (Character c in mob._Zone.Chars.Values)
					{
						if (c.IsConnection)
						{
							Message j = new Message(0);
							j.WriteShort(mob.IdEntity);
							c.SendMessage(j);
						}
					}
					if (!mob._Zone.Mobs.TryRemove(mob.IdEntity, out mob))
					{
					}
				}
				ArrCaoThuNhanGia[i] = null;
			}
			ArrCaoThuNhanGia = null;
		}
	}
}
