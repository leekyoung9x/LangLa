using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LangLa.Hander;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.EventServer
{
	public class AiGiaToc
	{
		public Zone[] Zone = new Zone[2];

		private GiaTocTemplate _GiaToc;

		public bool IsRunning;

		public bool IsDoneAi1;

		public bool IsResawMob1;

		public bool IsRespawBigBoss;

		public long TimeStart;

		public int TimeHoatDong;

		public bool IsHoatDong;

		private long TimeEnd;

		private sbyte LevelMob = 0;

		private short SizeChar;

		private short LevelAll;

		private short IdMobAdd;

		public AiGiaToc(Character _myChar, GiaTocTemplate GiaToc, short MapJoint, sbyte ZoneJoin)
		{
			_GiaToc = GiaToc;
			GiaToc.SoLanDiAiTrongNgay++;
			GiaToc.SoLanMoCuaAi--;
			GiaToc.TimeMoCuaAi = DateTime.Now.ToString();
			GiaToc.IsMoCuaAi = true;
			GiaToc.AiGiaToc = this;
			IsHoatDong = false;
			TimeHoatDong = 180000;
			TimeEnd = Util.CurrentTimeMillis() + 3600000;
			TimeStart = Util.CurrentTimeMillis();
			for (sbyte i = 0; i < Zone.Length; i++)
			{
				Zone[i] = new Zone(i);
			}
			SetCharJoinMap(_myChar, MapJoint, ZoneJoin);
		}

		public void Start()
		{
			Update();
		}

		public void Close()
		{
			for (int i = 0; i < Zone.Length; i++)
			{
				Zone Zone2 = Zone[i];
				foreach (Character c in Zone2.Chars.Values)
				{
					if (c.IsConnection)
					{
						c.SendMessage(UtilMessage.SendThongBao("Bạn nhận được 2 điểm chuyên cần,2 điểm cống hiến gia tộc", Util.WHITE));
						c.SendMessage(UtilMessage.SendThongBao("Sẽ tự động về trường sau 15 giây nữa", Util.WHITE));
						c.Info.ChuyenCan += 2;
						c.InfoGame.GiaToc.ThanhViens.FirstOrDefault((GiaTocTemplate.ThanhVienGiaToc s) => s.Name.Equals(c.Info.Name)).CongHien += 2;
					}
				}
			}
			CreateTaskClose();
		}

		private void SetLose()
		{
			for (int i = 0; i < Zone.Length; i++)
			{
				Zone Zone2 = Zone[i];
				foreach (Character c in Zone2.Chars.Values)
				{
					if (c.IsConnection)
					{
						c.SendMessage(UtilMessage.SendThongBao("Ải gia tộc thất bại", Util.WHITE));
					}
				}
			}
			CreateTaskClose();
		}

		private void CreateTaskClose()
		{
			long Time = Util.CurrentTimeMillis() + 15000;
			new Task(delegate
			{
				while (true)
				{
					try
					{
						if (Util.CurrentTimeMillis() > Time)
						{
							for (int i = 0; i < Zone.Length; i++)
							{
								Zone zone = Zone[i];
								foreach (Character current in zone.Chars.Values)
								{
									if (current.IsConnection)
									{
										current.InfoGame.ZoneGame.RemoveChar(current);
										current.Info.MapId = 86;
										current.Info.Cx = 590;
										current.Info.Cy = 644;
										current.JoinMap(-1, -1);
									}
								}
								zone.Chars.Clear();
								zone.Mobs.Clear();
								zone.ItemMaps.Clear();
							}
							Zone = null;
							int num = 3000;
							if (_GiaToc.SoLanDiAiTrongNgay > 1)
							{
								num *= _GiaToc.SoLanDiAiTrongNgay;
							}
							_GiaToc.UpdateExp(num);
							_GiaToc.IsMoCuaAi = false;
							_GiaToc = null;
							IsRunning = false;
							break;
						}
					}
					catch (Exception e)
					{
						Util.ShowErr(e);
						Zone = null;
						int num2 = 3000;
						if (_GiaToc.SoLanDiAiTrongNgay > 1)
						{
							num2 *= _GiaToc.SoLanDiAiTrongNgay;
						}
						_GiaToc.UpdateExp(num2);
						_GiaToc.IsMoCuaAi = false;
						_GiaToc = null;
						IsRunning = false;
						break;
					}
					Thread.Sleep(10);
				}
			}).Start();
		}

		private void SetCharJoinMap(Character _myChar, short MapJoint, sbyte ZoneJoin)
		{
			short LevelMax = 0;
			lock (_GiaToc.ThanhViens)
			{
				foreach (GiaTocTemplate.ThanhVienGiaToc c in _GiaToc.ThanhViens)
				{
					if (c.isOn && c._myChar != null)
					{
						LevelAll += c._myChar.Info.Level;
						SizeChar++;
					}
				}
			}
			LevelAll /= SizeChar;
			_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
			_myChar.Info.MapId = 46;
			_myChar.Info.Cx = 101;
			_myChar.Info.Cy = 417;
			_myChar.JoinMap(-1, -1);
			_myChar.SendMessage(UtilMessage.MsgUpdateHoatDong(IsLockNextmap: true));
			_myChar.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(TimeStart, TimeHoatDong, IsHoatDong));
		}

		public void CreateBigBoss()
		{
			IsRespawBigBoss = true;
			int Hp = 1200000000;
			if (LevelAll <= 0)
			{
				LevelAll = (short)Util.NextInt(20, 40);
				Hp = 75000000;
			}
			if (LevelAll >= 20 && LevelAll <= 30)
			{
				Hp = 120000000;
			}
			if (LevelAll >= 30 && LevelAll <= 40)
			{
				Hp = 180000000;
			}
			if (LevelAll >= 40 && LevelAll <= 50)
			{
				Hp = 240000000;
			}
			if (LevelAll >= 50 && LevelAll <= 60)
			{
				Hp = 360000000;
			}
			Hp = ((LevelAll < 60 || LevelAll > 80) ? 120000000 : 400000000);
			int HpUp2 = 0;
			double HpUp = 1.0;
			if (_GiaToc.SoLanDiAiTrongNgay > 1)
			{
				HpUp = (double)_GiaToc.SoLanDiAiTrongNgay * 1.3;
			}
			Hp = Util.NextInt(Hp, (int)((double)Hp * HpUp));
			if (Hp <= 0)
			{
				Hp = 120000000;
			}
			Mob mob = new Mob(Zone[1]);
			mob.Id = 112;
			mob.Cx = 2440;
			mob.Cy = 771;
			mob.IdEntity = IdMobAdd++;
			mob.Hp = (mob.HpFull = Hp);
			mob.IsMobAiGiaToc = true;
			mob.Level = LevelAll;
			mob.Exp = mob.Hp / 7;
			if (!Zone[1].Mobs.TryAdd(mob.IdEntity, mob))
			{
				return;
			}
			foreach (Character c in Zone[1].Chars.Values)
			{
				if (c.IsConnection)
				{
					Message i = new Message(1);
					mob.Write(i);
					c.SendMessage(i);
				}
			}
		}

		private void CreateMob()
		{
			int Hp = 0;
			short ValueNeTranh = 0;
			sbyte ValuePhanSatThuong = 0;
			sbyte ValueHoiPhucHp = 0;
			if (LevelAll <= 0)
			{
				LevelAll = (short)Util.NextInt(20, 40);
				Hp = 450000;
				ValueNeTranh = 500;
				ValuePhanSatThuong = 5;
				ValueHoiPhucHp = 3;
			}
			if (LevelAll >= 20 && LevelAll <= 30)
			{
				Hp = 550000;
				ValueNeTranh = 600;
				ValuePhanSatThuong = 6;
				ValueHoiPhucHp = 4;
			}
			else if (LevelAll >= 30 && LevelAll <= 40)
			{
				Hp = 750000;
				ValueNeTranh = 700;
				ValuePhanSatThuong = 7;
				ValueHoiPhucHp = 5;
			}
			else if (LevelAll >= 40 && LevelAll <= 50)
			{
				Hp = 850000;
				ValueNeTranh = 900;
				ValuePhanSatThuong = 8;
				ValueHoiPhucHp = 6;
			}
			else if (LevelAll >= 50 && LevelAll <= 60)
			{
				Hp = 1000000;
				ValueNeTranh = 1200;
				ValuePhanSatThuong = 9;
				ValueHoiPhucHp = 7;
			}
			else if (LevelAll >= 60 && LevelAll <= 80)
			{
				Hp = 1400000;
				ValueNeTranh = 1900;
				ValuePhanSatThuong = 10;
				ValueHoiPhucHp = 8;
			}
			short Cx = 0;
			double HpMob = 1.0;
			if (_GiaToc.SoLanDiAiTrongNgay > 1)
			{
				HpMob = (double)_GiaToc.SoLanDiAiTrongNgay * 1.2;
			}
			Hp = Util.NextInt(Hp, (int)((double)Hp * HpMob));
			if (Hp <= 0)
			{
				Hp = int.MaxValue;
			}
			ValueNeTranh = (short)Util.NextInt(ValueNeTranh, ValueNeTranh + 150);
			short MaxMob = 33;
			for (sbyte i = 0; i < Zone.Length; i++)
			{
				if (i == 1)
				{
					IdMobAdd = 0;
					MaxMob = 71;
					Hp = (int)((double)Hp * 1.3);
					Cx = 0;
				}
				for (int j = 0; j < MaxMob; j++)
				{
					if (j == 24)
					{
						Cx = 0;
					}
					Mob mob = new Mob(Zone[i]);
					if (i == 0)
					{
						mob.Id = 122;
					}
					else if (j <= 15)
					{
						mob.Id = 129;
					}
					else if (j > 16 && j <= 40)
					{
						mob.Id = 128;
					}
					else if (j > 41 && j <= 59)
					{
						mob.Id = 127;
					}
					else if (j >= 60 && j <= 70)
					{
						mob.Id = 130;
					}
					else
					{
						mob.Id = 128;
					}
					mob.Level = LevelAll;
					mob.IdEntity = IdMobAdd++;
					mob.Hp = (mob.HpFull = Hp);
					mob.Exp = mob.Hp / 7;
					mob.IdClass = (sbyte)Util.NextInt(1, 5);
					mob.IsMobAiGiaToc = true;
					MobHander.SetPointRespawMob(mob, IsNew: false, IsSetThuLinh: false);
					if (i == 0)
					{
						if (j <= 24)
						{
							mob.Cx = (short)(686 + Cx);
							mob.Cy = 363;
						}
						else
						{
							mob.Cx = (short)(3680 + Cx);
							mob.Cy = 800;
						}
					}
					else
					{
						if (j == 16 || j == 42 || j == 60)
						{
							Cx = 0;
						}
						if (j <= 15)
						{
							mob.Cx = (short)(198 + Cx);
							mob.Cy = 873;
							MobHander.AddValueNeTranh(mob, ValueNeTranh);
						}
						else if (j > 15 && j <= 40)
						{
							mob.Cx = (short)(93 + Cx);
							mob.Cy = 600;
							mob.InfoMob.PhanTramPhanSatThuong = ValuePhanSatThuong;
						}
						else if (j > 41 && j <= 59)
						{
							mob.Cx = (short)(78 + Cx);
							mob.Cy = 302;
							mob.InfoMob.PhanTramHoiPhucHp = ValueHoiPhucHp;
						}
						else if (j >= 60 && j <= 70)
						{
							mob.Cx = (short)(2158 + Cx);
							mob.Cy = 372;
							mob.IsMobAiGiaToc = false;
							mob.Speed = 0;
						}
					}
					Cx = ((j < 60) ? ((short)(Cx + (short)Util.NextInt(60, 100))) : ((short)(Cx + 130)));
					Zone[i].Mobs.TryAdd(mob.IdEntity, mob);
				}
			}
		}

		public void Update()
		{
			IsRunning = true;
			new Thread((ThreadStart)delegate
			{
				while (IsRunning)
				{
					Parallel.ForEach(Zone, delegate(Zone s)
					{
						s.Update();
					});
					if (!IsResawMob1 && Util.CurrentTimeMillis() - TimeStart > TimeHoatDong)
					{
						IsResawMob1 = true;
						IsHoatDong = true;
						TimeStart = Util.CurrentTimeMillis();
						CreateMob();
						lock (Zone[0].Chars)
						{
							Zone[] zone = Zone;
							foreach (Zone zone2 in zone)
							{
								if (zone2.Id == 0)
								{
									foreach (Character current in zone2.Chars.Values)
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
							}
						}
					}
					if (Util.CurrentTimeMillis() > TimeEnd)
					{
						SetLose();
						break;
					}
					Thread.Sleep(15);
				}
			}).Start();
		}
	}
}
