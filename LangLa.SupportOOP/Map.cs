using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LangLa.EventServer;
using LangLa.Hander;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.Server;

namespace LangLa.SupportOOP
{
	public class Map
	{
		public short Id;

		public Zone[] Zones;

		public List<WayPoint> WayPoints;

		public Thread ThreadUpdate;

		private static readonly sbyte MAX_COUNT = 24;

		private bool IsRuning = true;

		public Map(short Id)
		{
			this.Id = Id;
			WayPoints = new List<WayPoint>();
		}

		public void Update()
		{
			ThreadUpdate = new Thread(Run);
			ThreadUpdate.Start();
		}

		private void Run()
		{
			while (IsRuning)
			{
				long Start = Util.CurrentTimeMillis();
				try
				{
					Parallel.ForEach(Zones, delegate(Zone s)
					{
						s.Update();
					});
				}
				catch (Exception e)
				{
					LangLa.Server.Server.LogBug.Add(e.StackTrace);
					Util.ShowErr(e);
				}
				long End = 100 - (Util.CurrentTimeMillis() - Start);
				Thread.Sleep((int)((End <= 0) ? 100 : End));
			}
		}

		public void Close()
		{
		}

		public void LoginGame(Character _MyChar)
		{
			lock (Zones)
			{
				for (int i = 0; i < Zones.Length; i++)
				{
					if (Zones[i] == null)
					{
						Util.ShowLog("DONE NULL");
					}
					else if (Zones[i].Chars == null)
					{
						Util.ShowLog("NULL CHAR");
					}
					else if (Zones[i].Chars.Values.Count < 25)
					{
						Zones[i].AddChar(_MyChar);
						_MyChar.InfoGame.ZoneGame = Zones[i];
						break;
					}
				}
			}
		}

		public void ChangeZone(Character _myChar, sbyte ZoneID)
		{
			if (ZoneID >= Zones.Length)
			{
				return;
			}
			Zone zone = Zones[ZoneID];
			if (_myChar.InfoGame.ZoneGame.Id != ZoneID)
			{
				if (zone.Chars.Values.Count > 24)
				{
					_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
					_myChar.SendMessage(UtilMessage.SendThongBao("Khu vực đã đầy", Util.WHITE));
				}
				else
				{
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.JoinMap(-1, ZoneID);
				}
			}
		}

		public void NextMap(Character _MyChar)
		{
			short Cx = _MyChar.Info.Cx;
			short Cy = _MyChar.Info.Cy;
			WayPoint _WayPoint = null;
			short MapNext2 = -1;
			foreach (WayPoint wp in WayPoints)
			{
				if (_WayPoint == null || Util.getRange(Cx, Cy, wp.Cx, wp.Cy) < Util.getRange(Cx, Cy, _WayPoint.Cx, _WayPoint.Cy))
				{
					_WayPoint = wp;
				}
			}
			if (_WayPoint != null)
			{
				short MapNext = _WayPoint.MapNext;
				MapNext2 = MapNext;
				foreach (WayPoint wpNext in MapManager.Maps[MapNext].WayPoints)
				{
					if (wpNext.MapNext == _MyChar.Info.MapId)
					{
						_WayPoint = wpNext;
						break;
					}
				}
				if (_WayPoint == null)
				{
					return;
				}
				_MyChar.InfoGame.ZoneGame.RemoveChar(_MyChar);
				TaskHander.CheckDoneTaskNextMap(_MyChar, MapNext2);
				_MyChar.Info.MapId = MapNext2;
				sbyte ZoneSetKhuRungChet = _MyChar.InfoGame.ZoneGame.Id;
				if (MapNext2 == 22)
				{
					_MyChar.Info.Cx = (short)Util.NextInt(122, 287);
					_MyChar.Info.Cy = 827;
					if (KhuRungChet.TimeSetBossMap2[ZoneSetKhuRungChet] == 0)
					{
						KhuRungChet.TimeSetBossMap2[ZoneSetKhuRungChet] = Util.CurrentTimeMillis() + 900000;
					}
				}
				else if (MapNext == 47)
				{
					_MyChar.Info.Cx = 135;
					_MyChar.Info.Cy = 873;
				}
				else
				{
					_MyChar.Info.Cx = _WayPoint.Cx;
					_MyChar.Info.Cy = _WayPoint.Cy;
				}
				_MyChar.JoinMap(_MyChar.Info.MapId, -1);
				switch (MapNext2)
				{
				case 22:
					_MyChar.SendMessage(UtilMessage.MsgUpdateHoatDong(IsLockNextmap: true));
					if (KhuRungChet.TimeStartMap2[ZoneSetKhuRungChet] == 0)
					{
						KhuRungChet.TimeStartMap2[ZoneSetKhuRungChet] = Util.CurrentTimeMillis();
					}
					_MyChar.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(KhuRungChet.TimeStartMap2[ZoneSetKhuRungChet], KhuRungChet.TimeHoatDong, KhuRungChet.IsHoatDong));
					break;
				case 47:
					_MyChar.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(_MyChar.InfoGame.GiaToc.AiGiaToc.TimeStart, _MyChar.InfoGame.GiaToc.AiGiaToc.TimeHoatDong, _MyChar.InfoGame.GiaToc.AiGiaToc.IsHoatDong));
					break;
				}
			}
			else
			{
				_MyChar.SetXY((short)((Cx > 500) ? (Cx - 50) : (Cx + 50)), Cy);
			}
		}
	}
}
