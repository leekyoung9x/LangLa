using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.EventServer
{
	public class CamThuat
	{
		public long TimeStart;

		public int TimeHoatDong;

		public bool IsHoatDong;

		private short MapJoint;

		private short ZoneJoint;

		private List<Mob> Mobs = new List<Mob>();

		public sbyte CountVongLap = 1;

		private short LevelAll;

		public bool IsResawMob1;

		private sbyte SizeChar;

		private bool IsRunning;

		public bool IsRespawBigBoss;

		private short CxBack;

		private short CyBack;

		public static readonly object _Lock = new object();

		private Zone Zone = new Zone(0);

		public short IdMobAdd;

		public long LastTimeCanResawMob;

		private long TimeCheckOut = Util.CurrentTimeMillis() + 180000;

		public CamThuat(Character _myChar, InfoToDoi ToDoi = null)
		{
			short LevelMax = 0;
			lock (_Lock)
			{
				TimeStart = Util.CurrentTimeMillis();
				IsHoatDong = false;
				TimeHoatDong = 10000;
				MapJoint = _myChar.Info.MapId;
				ZoneJoint = _myChar.InfoGame.ZoneGame.Id;
				CxBack = _myChar.Info.Cx;
				CyBack = _myChar.Info.Cy;
				if (ToDoi != null)
				{
					foreach (Character c in ToDoi.Chars)
					{
						if (c.IsConnection && c.Info.MapId == MapJoint && c.InfoGame.ZoneGame.Id == ZoneJoint)
						{
							if (LevelMax < c.Info.Level)
							{
								LevelMax = c.Info.Level;
							}
							LevelAll += c.Info.Level;
							Util.ShowLog("LEVEL ADD " + LevelAll + " LV C " + c.Info.Level);
							c.InfoGame.ZoneGame.RemoveChar(c);
							c.Info.MapId = 89;
							c.Info.Cx = 152;
							c.Info.Cy = 428;
							c.InfoGame.ZoneGame = Zone;
							c.JoinMap(-1, -1);
							c.InfoGame.IsVaoCamThuat = true;
							c.InfoGame.CamThuat = this;
							c.TimeChar.SoLanDiCamThuat--;
							c.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(TimeStart, TimeHoatDong, IsHoatDong));
						}
					}
					SizeChar = (sbyte)ToDoi.Chars.Count;
				}
				else
				{
					LevelAll += _myChar.Info.Level;
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.Info.MapId = 89;
					_myChar.Info.Cx = 152;
					_myChar.Info.Cy = 428;
					_myChar.InfoGame.ZoneGame = Zone;
					_myChar.JoinMap(-1, -1);
					_myChar.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(TimeStart, TimeHoatDong, IsHoatDong));
					SizeChar = 1;
					_myChar.InfoGame.IsVaoCamThuat = true;
					_myChar.InfoGame.CamThuat = this;
					_myChar.TimeChar.SoLanDiCamThuat--;
				}
				InitMob();
				LevelAll /= SizeChar;
				if (LevelAll < LevelMax)
				{
					LevelAll = LevelMax;
				}
				Util.ShowLog("LevelAll " + LevelAll);
			}
		}

		public void Start()
		{
			IsRunning = true;
			Update();
		}

		public void ResawBigBoss()
		{
			int Hp = 80000000;
			IsRespawBigBoss = true;
			if (LevelAll <= 0)
			{
				LevelAll = (sbyte)Util.NextInt(20, 40);
				Hp = 50000000;
			}
			if (LevelAll >= 20 && LevelAll <= 30)
			{
				Hp = 40000000;
			}
			else if (LevelAll >= 30 && LevelAll <= 40)
			{
				Hp = 80000000;
			}
			else if (LevelAll >= 40 && LevelAll <= 50)
			{
				Hp = 12000000;
			}
			else if (LevelAll >= 50 && LevelAll <= 60)
			{
				Hp = 14000000;
			}
			else if (LevelAll >= 60 && LevelAll <= 80)
			{
				Hp = 18000000;
			}
			int HpUp = 1;
			if (CountVongLap > 1)
			{
				HpUp = CountVongLap;
			}
			Hp = (int)((double)Util.NextInt(Hp, (int)((double)Hp * 1.5)) * ((HpUp > 2) ? 1.45 : ((double)HpUp))) * 2;
			if (Hp > int.MaxValue || Hp == 0 || Hp < 0)
			{
				Hp = 80000000;
			}
			Mob mob = new Mob(Zone);
			mob.Id = 238;
			mob.Cx = 900;
			mob.Cy = 137;
			mob.SetDameBoss = Util.NextInt(10, 20) * CountVongLap;
			mob.Hp = (mob.HpFull = Hp);
			mob.Exp = mob.Hp / 7;
			mob.Level = LevelAll;
			mob.LevelBoss = 10;
			mob.IsMobVongLap = true;
			mob.IdEntity = IdMobAdd++;
			if (Zone.Mobs.TryAdd(mob.IdEntity, mob))
			{
				foreach (Character c2 in Zone.Chars.Values)
				{
					c2.SendMessage(UtilMessage.SendThongBao("Kabuto đã bắt đầu xuát hiện", Util.WHITE));
				}
			}
			foreach (Character c in Zone.Chars.Values)
			{
				Message i = new Message(1);
				mob.Write(i);
				c.SendMessage(i);
			}
		}

		private void InitMob()
		{
			int Hp = 200000;
			if (LevelAll <= 0)
			{
				LevelAll = (sbyte)Util.NextInt(20, 40);
				Hp = 20000;
			}
			if (LevelAll > 0 && LevelAll <= 20)
			{
				Hp = 30000;
			}
			if (LevelAll > 20 && LevelAll <= 30)
			{
				Hp = 40000;
			}
			if (LevelAll > 31 && LevelAll <= 40)
			{
				Hp = 65000;
			}
			if (LevelAll > 40 && LevelAll <= 50)
			{
				Hp = 100000;
			}
			if (LevelAll > 50 && LevelAll < 60)
			{
				Hp = 150000;
			}
			if (LevelAll > 60 && LevelAll <= 80)
			{
				Hp = 200000;
			}
			else
			{
				LevelAll = 65;
				Hp = 200000;
			}
			Hp = (int)((double)Util.NextInt(Hp, (int)((double)Hp * 1.5)) * ((CountVongLap > 2) ? 1.45 : ((double)CountVongLap)));
			if (Hp > int.MaxValue || Hp <= 0)
			{
				Hp = Util.NextInt(50000000, 300000000);
			}
			int j = 60;
			for (int i = 0; i < 42; i++)
			{
				Mob mob = new Mob(Zone);
				mob.Id = 237;
				mob.Hp = (mob.HpFull = Hp);
				mob.Exp = mob.Hp / 7;
				mob.Level = LevelAll;
				mob.IdEntity = IdMobAdd++;
				mob.SetDameBoss = Util.NextInt(3, 5) * CountVongLap;
				mob.IsMobVongLap = true;
				if (i <= 14)
				{
					mob.Cx = (short)(200 + j);
					mob.Cy = 428;
					if (i == 14)
					{
						j = 60;
					}
				}
				if (i > 14 && i <= 28)
				{
					mob.Cx = (short)(60 + j);
					mob.Cy = 284;
					if (i == 28)
					{
						j = 60;
					}
				}
				if (i > 28)
				{
					mob.Cx = (short)(90 + j);
					mob.Cy = 137;
				}
				j += 60;
				if (Zone.Mobs.TryAdd(mob.IdEntity, mob))
				{
				}
			}
		}

		private void ResawVongLap()
		{
		}

		public void Update()
		{
			new Thread((ThreadStart)delegate
			{
				while (IsRunning)
				{
					if (Util.CurrentTimeMillis() > TimeCheckOut)
					{
						if (Zone.Chars.Values.Count <= 0)
						{
							Close();
							break;
						}
						TimeCheckOut = Util.CurrentTimeMillis() + 180000;
					}
					if (!IsResawMob1 && Util.CurrentTimeMillis() - TimeStart > TimeHoatDong)
					{
						IsResawMob1 = true;
						TimeHoatDong = 300000;
						TimeStart = Util.CurrentTimeMillis();
						if (CountVongLap > 1)
						{
							IdMobAdd = 0;
							InitMob();
							IsRespawBigBoss = false;
						}
						foreach (Character current in Zone.Chars.Values)
						{
							if (current.IsConnection)
							{
								if (CountVongLap > 1)
								{
									current.SetXY(152, 428);
									Zone.SetXyChar(current, when_move: false);
								}
								foreach (Mob current2 in Zone.Mobs.Values)
								{
									Message msg = new Message(1);
									current2.Write(msg);
									current.SendMessage(msg);
								}
								current.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(TimeStart, TimeHoatDong, IsHoatDong));
							}
						}
						CountVongLap++;
					}
					if (IsResawMob1 && Util.CurrentTimeMillis() - TimeStart > TimeHoatDong)
					{
						IsRunning = false;
						Close();
						break;
					}
					Zone.Update();
					Thread.Sleep(10);
				}
			}).Start();
		}

		public void SetWin()
		{
			foreach (Character c in Zone.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.SendThongBao("Chúc mừng bạn đã chiến thắng cấm thuật và nhận được 2 diểm chuyên cần", Util.WHITE));
					c.Info.ChuyenCan += 2;
				}
			}
			CreateTaskClose();
		}

		private void Close()
		{
			foreach (Character c in Zone.Chars.Values)
			{
				if (c.IsConnection)
				{
					c.SendMessage(UtilMessage.SendThongBao("Cấm thuật thất bại sẽ trở về sau 10 giây nữa", Util.WHITE));
					for (short i = 0; i < IdMobAdd; i++)
					{
						Message j = new Message(0);
						j.WriteShort(i);
						c.SendMessage(j);
					}
				}
			}
			CreateTaskClose();
		}

		private void CreateTaskClose()
		{
			long Time = Util.CurrentTimeMillis() + 10000;
			new Task(delegate
			{
				while (Util.CurrentTimeMillis() <= Time)
				{
					Thread.Sleep(10);
				}
				try
				{
					foreach (Character current in Zone.Chars.Values)
					{
						if (current.IsConnection)
						{
							current.InfoGame.IsVaoCamThuat = false;
							current.InfoGame.CamThuat = null;
							current.InfoGame.ZoneGame.RemoveChar(current);
							current.Info.MapId = MapJoint;
							current.Info.Cx = CxBack;
							current.Info.Cy = CyBack;
							current.JoinMap(-1, -1);
						}
					}
					IsRunning = false;
					Zone.Mobs.Clear();
					Zone.Chars.Clear();
					Zone.ItemMaps.Clear();
				}
				catch (Exception e)
				{
					Util.ShowErr(e);
					IsRunning = false;
					Zone.Mobs.Clear();
					Zone.Chars.Clear();
					Zone.ItemMaps.Clear();
				}
			}).Start();
		}
	}
}
