using System;
using System.Threading;
using System.Threading.Tasks;
using LangLa.Hander;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.EventServer
{
    public class KhuRungChet
    {
        public static bool IsStartKhuRungChet;

        public static long TimeStart;

        public static int TimeHoatDong;

        public static bool IsHoatDong;

        public Zone[] ZonesHoatDong = new Zone[7];

        public Zone[] ZonesHoatDong2 = new Zone[7];

        public static long[] TimeSetBossMap2 = new long[7];

        public static long[] TimeStartMap2 = new long[7];

        public static bool[] IsRespawBoss2 = new bool[7];

        private bool IsSpawQuai1;

        private long TimeRespawBoss1;

        private bool IsRespawBoss1;

        private short IdMobDie;

        private long TimeEnd;

        private bool IsSendThongBaoClose;

        private void Close()
        {
            IsStartKhuRungChet = false;
            TimeStart = -1L;
            TimeHoatDong = -1;
            for (int k = 0; k < TimeSetBossMap2.Length; k++)
            {
                TimeSetBossMap2[k] = 0L;
            }
            for (int j = 0; j < TimeStartMap2.Length; j++)
            {
                TimeStartMap2[j] = 0L;
            }
            for (int i = 0; i < IsRespawBoss2.Length; i++)
            {
                IsRespawBoss2[i] = false;
            }
            Zone[] zonesHoatDong = ZonesHoatDong;
            foreach (Zone z2 in zonesHoatDong)
            {
                foreach (Character c2 in z2.Chars.Values)
                {
                    if (c2.IsConnection)
                    {
                        z2.RemoveChar(c2);
                        c2.Info.MapId = 86;
                        c2.JoinMap(-1, -1);
                    }
                }
            }
            Zone[] zonesHoatDong2 = ZonesHoatDong2;
            foreach (Zone z in zonesHoatDong2)
            {
                foreach (Character c in z.Chars.Values)
                {
                    if (c.IsConnection)
                    {
                        z.RemoveChar(c);
                        c.Info.MapId = 86;
                        c.JoinMap(-1, -1);
                    }
                }
            }
            ZonesHoatDong = null;
            ZonesHoatDong2 = null;
        }

        public KhuRungChet(sbyte HourEnd, sbyte HourNext)
        {
            TimeEnd = Util.CurrentTimeMillis() + 3600000;
            IsStartKhuRungChet = true;
            TimeStart = Util.CurrentTimeMillis();
            TimeHoatDong = 600000;
            IsHoatDong = false;
            Map map = MapManager.Maps[2];
            for (int j = 0; j <= 6; j++)
            {
                ZonesHoatDong[j] = map.Zones[j];
            }
            map = MapManager.Maps[22];
            for (int i = 0; i <= 6; i++)
            {
                ZonesHoatDong2[i] = map.Zones[i];
            }
        }

        public void sendThongBaoAllZone(string Text)
        {
            Zone[] zonesHoatDong = ZonesHoatDong;
            foreach (Zone z2 in zonesHoatDong)
            {
                foreach (Character c2 in z2.Chars.Values)
                {
                    if (c2.IsConnection)
                    {
                        c2.SendMessage(UtilMessage.SendThongBao(Text, Util.YELLOW_MID));
                    }
                }
            }
            Zone[] zonesHoatDong2 = ZonesHoatDong2;
            foreach (Zone z in zonesHoatDong2)
            {
                foreach (Character c in z.Chars.Values)
                {
                    if (c.IsConnection)
                    {
                        c.SendMessage(UtilMessage.SendThongBao(Text, Util.YELLOW_MID));
                    }
                }
            }
        }

        public void Start()
        {
            new Thread((ThreadStart)delegate
            {
                while (IsStartKhuRungChet)
                {
                    try
                    {
                        if (!IsSendThongBaoClose && Util.CurrentTimeMillis() > TimeEnd)
                        {
                            IsSendThongBaoClose = true;
                            sendThongBaoAllZone("Hoạt động khu rừng chết sẽ kết thúc sau 10 giây nữa");
                            new Task(delegate
                            {
                                while (true)
                                {
                                    try
                                    {
                                        if (Util.CurrentTimeMillis() > TimeEnd + 15000)
                                        {
                                            IsStartKhuRungChet = false;
                                            Close();
                                            break;
                                        }
                                    }
                                    catch (Exception e2)
                                    {
                                        Util.ShowErr(e2);
                                        IsStartKhuRungChet = false;
                                        Close();
                                    }
                                    Thread.Sleep(100);
                                }
                            }).Start();
                            break;
                        }
                        if (!IsSpawQuai1 && Util.CurrentTimeMillis() - TimeStart > TimeHoatDong)
                        {
                            IsSpawQuai1 = true;
                            IsHoatDong = true;
                            TimeStart = Util.CurrentTimeMillis();
                            ResawMob2();
                            ResawMob1();
                            Zone[] zonesHoatDong = ZonesHoatDong;
                            foreach (Zone zone in zonesHoatDong)
                            {
                                foreach (Character current in zone.Chars.Values)
                                {
                                    if (current.IsConnection)
                                    {
                                        current.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(TimeStart, TimeHoatDong, IsHoatDong));
                                        foreach (Mob current2 in current.InfoGame.ZoneGame.Mobs.Values)
                                        {
                                            Message msg = new Message(1);
                                            current2.Write(msg);
                                            current.SendMessage(msg);
                                        }
                                    }
                                }
                            }
                            TimeRespawBoss1 = Util.CurrentTimeMillis() + 900000;
                        }
                        if (!IsRespawBoss1 && TimeRespawBoss1 != 0L && Util.CurrentTimeMillis() > TimeRespawBoss1)
                        {
                            IsRespawBoss1 = true;
                            ResawBoss1();
                        }
                        for (int j = 0; j < TimeSetBossMap2.Length; j++)
                        {
                            if (TimeSetBossMap2[j] != 0 && !IsRespawBoss2[j] && Util.CurrentTimeMillis() > TimeSetBossMap2[j])
                            {
                                IsRespawBoss2[j] = true;
                                ResawBoss2(ZonesHoatDong2[j]);
                            }
                        }
                        Thread.Sleep(100);
                    }
                    catch (Exception e)
                    {
                        Util.ShowErr(e);
                    }
                }
            }).Start();
        }

        private void ResawBoss1()
        {
            int Hp = 0;
            int Level = 0;
            for (int i = 0; i < ZonesHoatDong.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        Hp = 652422;
                        Level = Util.NextInt(5, 10);
                        break;
                    case 1:
                        Hp = 1057200;
                        Level = Util.NextInt(15, 20);
                        break;
                    case 2:
                        Hp = 1524000;
                        Level = Util.NextInt(25, 30);
                        break;
                    case 3:
                        Hp = 2040000;
                        Level = Util.NextInt(35, 40);
                        break;
                    case 4:
                        Hp = 3242000;
                        Level = Util.NextInt(45, 50);
                        break;
                    case 5:
                        Hp = 4558420;
                        Level = Util.NextInt(55, 60);
                        break;
                    case 6:
                        Hp = 6250000;
                        Level = Util.NextInt(65, 70);
                        break;
                }
                Zone zone = ZonesHoatDong[i];
                Mob mob = new Mob(zone);
                mob.Cx = 624;
                mob.Cy = 245;
                mob.Id = 80;
                mob.Hp = (mob.HpFull = Hp);
                mob.Level = (short)Level;
                mob.IsMobKhuRungChet = true;
                mob.Exp = mob.Hp / 7;
                mob.IdClass = (sbyte)Util.NextInt(1, 5);
                mob.IdEntity = IdMobDie++;
                if (!zone.Mobs.TryAdd(mob.IdEntity, mob))
                {
                    continue;
                }
                foreach (Character c in zone.Chars.Values)
                {
                    if (c.IsConnection)
                    {
                        Message j = new Message(1);
                        mob.Write(j);
                        c.SendMessage(j);
                    }
                }
            }
        }

        private void ResawBoss2(Zone zone)
        {
            int Hp = 0;
            int Level = 0;
            switch (zone.Id)
            {
                case 0:
                    Hp = 852422;
                    Level = Util.NextInt(5, 10);
                    break;
                case 1:
                    Hp = 1457200;
                    Level = Util.NextInt(15, 20);
                    break;
                case 2:
                    Hp = 2024000;
                    Level = Util.NextInt(25, 30);
                    break;
                case 3:
                    Hp = 2540000;
                    Level = Util.NextInt(35, 40);
                    break;
                case 4:
                    Hp = 3842000;
                    Level = Util.NextInt(45, 50);
                    break;
                case 5:
                    Hp = 5158420;
                    Level = Util.NextInt(55, 60);
                    break;
                case 6:
                    Hp = 7250000;
                    Level = Util.NextInt(65, 70);
                    break;
            }
            Mob mob = new Mob(zone);
            mob.Cx = 557;
            mob.Cy = 417;
            mob.Id = 80;
            mob.Hp = (mob.HpFull = Hp);
            mob.Level = (short)Level;
            mob.IsMobKhuRungChet = true;
            mob.Exp = mob.Hp / 7;
            mob.IdClass = (sbyte)Util.NextInt(1, 5);
            mob.IdEntity = IdMobDie++;
            if (!zone.Mobs.TryAdd(mob.IdEntity, mob))
            {
                return;
            }
            foreach (Character c in zone.Chars.Values)
            {
                if (c.IsConnection)
                {
                    Message i = new Message(1);
                    mob.Write(i);
                    c.SendMessage(i);
                }
            }
        }

        private void ResawMob2()
        {
            int Hp = 0;
            int Level = 0;
            short Cx = 0;
            short Cy = 0;
            for (int i = 0; i < ZonesHoatDong2.Length; i++)
            {
                Zone zone = ZonesHoatDong2[i];
                Cx = 0;
                Cy = 0;
                switch (i)
                {
                    case 0:
                        Hp = 55000;
                        Level = Util.NextInt(5, 10);
                        break;
                    case 1:
                        Hp = 75000;
                        Level = Util.NextInt(15, 20);
                        break;
                    case 2:
                        Hp = 127500;
                        Level = Util.NextInt(25, 30);
                        break;
                    case 3:
                        Hp = 136350;
                        Level = Util.NextInt(35, 40);
                        break;
                    case 4:
                        Hp = 182000;
                        Level = Util.NextInt(45, 50);
                        break;
                    case 5:
                        Hp = 252500;
                        Level = Util.NextInt(55, 60);
                        break;
                    case 6:
                        Hp = 325000;
                        Level = Util.NextInt(65, 70);
                        break;
                }
                for (int j = 0; j < 40; j++)
                {
                    Mob mob = new Mob(zone);
                    mob.Id = (short)((Util.NextInt(0, 1) == 0) ? 93 : 85);
                    mob.Hp = Hp;
                    mob.Exp = Hp / 7;
                    mob.HpFull = Hp;
                    mob.Level = (short)Level;
                    mob.IsMobKhuRungChet = true;
                    mob.IdClass = (sbyte)Util.NextInt(1, 5);
                    if (j < 10)
                    {
                        if (Cx > 1000)
                        {
                            Cx = 0;
                        }
                        mob.Cx = (short)(281 + Cx);
                        mob.Cy = 827;
                        Cx += 120;
                    }
                    else if (j >= 10 && j <= 20)
                    {
                        switch (j)
                        {
                            case 10:
                                mob.Cx = 88;
                                mob.Cy = 697;
                                break;
                            case 11:
                                mob.Cx = 163;
                                mob.Cy = 697;
                                break;
                            case 12:
                                mob.Cx = 243;
                                mob.Cy = 697;
                                break;
                            case 13:
                                mob.Cx = 390;
                                mob.Cy = 614;
                                break;
                            case 14:
                                mob.Cx = 468;
                                mob.Cy = 614;
                                break;
                            case 15:
                                mob.Cy = 571;
                                mob.Cx = 614;
                                break;
                            case 16:
                                mob.Cy = 693;
                                mob.Cx = 614;
                                break;
                            case 17:
                                mob.Cx = 779;
                                mob.Cy = 614;
                                break;
                            case 18:
                                mob.Cx = 969;
                                mob.Cy = 698;
                                break;
                            case 19:
                                mob.Cx = 1024;
                                mob.Cy = 698;
                                break;
                            case 20:
                                mob.Cx = 1096;
                                mob.Cy = 698;
                                break;
                        }
                    }
                    else if (j > 20 && j < 30)
                    {
                        switch (j)
                        {
                            case 21:
                                mob.Cx = 139;
                                mob.Cy = 418;
                                break;
                            case 22:
                                mob.Cx = 228;
                                mob.Cy = 418;
                                break;
                            case 23:
                                mob.Cx = 326;
                                mob.Cy = 418;
                                break;
                            case 24:
                                mob.Cx = 416;
                                mob.Cy = 418;
                                break;
                            case 25:
                                mob.Cx = 523;
                                mob.Cy = 418;
                                break;
                            case 26:
                                mob.Cx = 640;
                                mob.Cy = 418;
                                break;
                            case 27:
                                mob.Cx = 754;
                                mob.Cy = 418;
                                break;
                            case 28:
                                mob.Cx = 880;
                                mob.Cy = 418;
                                break;
                            case 29:
                                mob.Cx = 978;
                                mob.Cy = 418;
                                break;
                        }
                    }
                    else if (j >= 30 && j < 40)
                    {
                        switch (j)
                        {
                            case 30:
                                mob.Cx = 100;
                                mob.Cy = 135;
                                break;
                            case 31:
                                mob.Cx = 135;
                                mob.Cy = 135;
                                break;
                            case 32:
                                mob.Cx = 195;
                                mob.Cy = 135;
                                break;
                            case 33:
                                mob.Cx = 376;
                                mob.Cy = 225;
                                break;
                            case 34:
                                mob.Cx = 471;
                                mob.Cy = 225;
                                break;
                            case 35:
                                mob.Cx = 561;
                                mob.Cy = 225;
                                break;
                            case 36:
                                mob.Cx = 659;
                                mob.Cy = 225;
                                break;
                            case 37:
                                mob.Cx = 749;
                                mob.Cy = 225;
                                break;
                            case 38:
                                mob.Cx = 1009;
                                mob.Cy = 130;
                                break;
                            case 39:
                                mob.Cx = 1104;
                                mob.Cy = 130;
                                break;
                        }
                    }
                    mob.IdEntity = IdMobDie++;
                    MobHander.SetPointRespawMob(mob);
                    zone.Mobs.TryAdd(mob.IdEntity, mob);
                }
            }
        }

        private void ResawMob1()
        {
            int Hp = 0;
            int Level = 0;
            short Cx = 0;
            short Cy = 0;
            for (int i = 0; i < ZonesHoatDong.Length; i++)
            {
                Zone zone = ZonesHoatDong[i];
                Cx = 0;
                Cy = 0;
                switch (i)
                {
                    case 0:
                        Hp = 25000;
                        Level = Util.NextInt(5, 10);
                        break;
                    case 1:
                        Hp = 35000;
                        Level = Util.NextInt(15, 20);
                        break;
                    case 2:
                        Hp = 47500;
                        Level = Util.NextInt(25, 30);
                        break;
                    case 3:
                        Hp = 66350;
                        Level = Util.NextInt(35, 40);
                        break;
                    case 4:
                        Hp = 82000;
                        Level = Util.NextInt(45, 50);
                        break;
                    case 5:
                        Hp = 102500;
                        Level = Util.NextInt(55, 60);
                        break;
                    case 6:
                        Hp = 125000;
                        Level = Util.NextInt(65, 70);
                        break;
                }
                for (int j = 0; j < 30; j++)
                {
                    Mob mob = new Mob(zone);
                    mob.Id = 83;
                    mob.Hp = Hp;
                    mob.Exp = Hp / 7;
                    mob.HpFull = Hp;
                    mob.Level = (short)Level;
                    mob.IsMobKhuRungChet = true;
                    mob.IdClass = (sbyte)Util.NextInt(1, 5);
                    if (j < 10)
                    {
                        if (Cx > 1000)
                        {
                            Cx = 0;
                        }
                        mob.Cx = (short)(299 + Cx);
                        mob.Cy = 655;
                        Cx += 85;
                    }
                    else if (j >= 10 && j <= 20)
                    {
                        switch (j)
                        {
                            case 10:
                                mob.Cx = 103;
                                mob.Cy = 488;
                                break;
                            case 11:
                                mob.Cx = 203;
                                mob.Cy = 488;
                                break;
                            case 12:
                                mob.Cx = 303;
                                mob.Cy = 488;
                                break;
                            case 13:
                                mob.Cx = 527;
                                mob.Cy = 491;
                                break;
                            case 14:
                                mob.Cx = 575;
                                mob.Cy = 578;
                                break;
                            case 15:
                                mob.Cy = 579;
                                mob.Cx = 708;
                                break;
                            case 16:
                                mob.Cy = 517;
                                mob.Cx = 772;
                                break;
                            case 17:
                                mob.Cx = 920;
                                mob.Cy = 490;
                                break;
                            case 18:
                                mob.Cx = 1100;
                                mob.Cy = 444;
                                break;
                            case 19:
                                mob.Cx = 1200;
                                mob.Cy = 444;
                                break;
                            case 20:
                                mob.Cx = 1109;
                                mob.Cy = 444;
                                break;
                        }
                    }
                    else if (j > 20 && j < 30)
                    {
                        switch (j)
                        {
                            case 21:
                                mob.Cx = 71;
                                mob.Cy = 352;
                                break;
                            case 22:
                                mob.Cx = 157;
                                mob.Cy = 352;
                                break;
                            case 23:
                                mob.Cx = 291;
                                mob.Cy = 406;
                                break;
                            case 24:
                                mob.Cx = 316;
                                mob.Cy = 209;
                                break;
                            case 25:
                                mob.Cx = 509;
                                mob.Cy = 245;
                                break;
                            case 26:
                                mob.Cx = 650;
                                mob.Cy = 245;
                                break;
                            case 27:
                                mob.Cx = 1167;
                                mob.Cy = 323;
                                break;
                            case 28:
                                mob.Cx = 938;
                                mob.Cy = 323;
                                break;
                            case 29:
                                mob.Cx = 1035;
                                mob.Cy = 323;
                                break;
                        }
                    }
                    mob.IdEntity = IdMobDie++;
                    MobHander.SetPointRespawMob(mob);
                    zone.Mobs.TryAdd(mob.IdEntity, mob);
                }
            }
        }
    }
}
