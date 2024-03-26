using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LangLa.Data;
using LangLa.Hander;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Server;
using LangLa.Template;

namespace LangLa.SupportOOP
{
	public class Mob
	{
		public short IdEntity;

		public short Id;

		public bool IsPainMiniMap;

		public string Name = "";

		public short Cx;

		public short Cy;

		public short Level;

		public short Speed;

		public sbyte IdClass;

		public sbyte Type;

		public int TimeThuHoach;

		public int Hp;

		public int HpFull;

		public int Exp;

		public sbyte LevelBoss;

		public int HpThuLinh;

		public int HpTinhAnh;

		public int ExpTinhAnh;

		public int ExpThuLinh;

		public long TimeDie;

		public Zone _Zone;

		public List<InfoEff> Effs = new List<InfoEff>();

		public InfoMob InfoMob = new InfoMob();

		public bool IsTrungDoc;

		public bool IsAnChoang;

		public bool IsLamCham;

		public bool IsSuyYeu;

		public bool IsBong;

		public long DelayAttack;

		public sbyte StatusAttack;

		public bool IsCaoThuNhanGia;

		public bool IsMobAiGiaToc;

		public bool IsMobVongLap;

		public bool IsMobKhuRungChet;

		public long LastTimeHoiPhucHp;

		public long LastTimePST;

		public int SetDameBoss;

		public Mob(Zone zone)
		{
			_Zone = zone;
		}

		public Message WriteTuongKhac()
		{
			Message i = new Message(13);
			i.WriteShort(IdEntity);
			short[] arrTuongKhac = InfoMob.ArrTuongKhac;
			foreach (short T in arrTuongKhac)
			{
				i.WriteShort(T);
			}
			return i;
		}

		public void SetDie(Character _myChar)
		{
			if (_myChar != null)
			{
				if (IsCaoThuNhanGia)
				{
					Mob mob = this;
					if (_Zone.Mobs.TryRemove(IdEntity, out mob))
					{
						LangLa.Server.Server.SendThongBaoFromServer(_myChar.Info.Name + " đã đánh bại được " + DataServer.ArrMobTempalte[Id].Name + " ở " + DataServer.ArrMapTemplate[_myChar.Info.MapId].Name);
					}
				}
				else if (IsMobKhuRungChet)
				{
					Mob mob3 = this;
					if (_Zone.Mobs.TryRemove(IdEntity, out mob3))
					{
						if ((_myChar.Info.MapId == 2 || _myChar.Info.MapId == 22) && mob3._Zone.Mobs.Values.Count == 0)
						{
							foreach (Character c3 in mob3._Zone.Chars.Values)
							{
								if (c3.IsConnection)
								{
									c3.SendMessage(UtilMessage.MsgUpdateHoatDong(IsLockNextmap: false));
								}
							}
						}
						foreach (Character c2 in _Zone.Chars.Values)
						{
							if (c2.IsConnection)
							{
								Message j = new Message(0);
								j.WriteShort(IdEntity);
								c2.SendMessage(j);
							}
						}
					}
				}
				else if (IsMobAiGiaToc)
				{
					Mob mob4 = this;
					if (_Zone.Mobs.TryRemove(IdEntity, out mob4))
					{
						if (_myChar.Info.MapId == 46)
						{
							if (mob4._Zone.Mobs.Values.Count == 0)
							{
								foreach (Character c in mob4._Zone.Chars.Values)
								{
									if (c.IsConnection)
									{
										c.SendMessage(UtilMessage.SendThongBao("Toàn bộ quái vật đã bị tiêu diệt hãy tiến về cửa tiếp theo", Util.WHITE));
										c.SendMessage(UtilMessage.SendThongBao("Bạn nhận được 2 điểm chuyên cần,2 điểm cống hiến gia tộc", Util.WHITE));
										c.Info.ChuyenCan += 2;
										c.InfoGame.GiaToc.ThanhViens.FirstOrDefault((GiaTocTemplate.ThanhVienGiaToc s) => s.Name.Equals(c.Info.Name)).CongHien += 2;
										c.SendMessage(UtilMessage.MsgUpdateHoatDong(IsLockNextmap: false));
									}
								}
							}
						}
						else if (_myChar.Info.MapId == 47 && mob4._Zone.Mobs.Values.Count == 12)
						{
							if (_myChar.InfoGame.GiaToc.AiGiaToc.IsRespawBigBoss)
							{
								_myChar.InfoGame.GiaToc.AiGiaToc.Close();
							}
							else
							{
								_myChar.InfoGame.GiaToc.AiGiaToc.CreateBigBoss();
								foreach (Character c5 in mob4._Zone.Chars.Values)
								{
									if (c5.IsConnection)
									{
										c5.SendMessage(UtilMessage.SendThongBao("Senju Hashirama đã được triệu hội ở phòng bùa chú", Util.WHITE));
									}
								}
							}
						}
						foreach (Character c4 in _Zone.Chars.Values)
						{
							if (c4.IsConnection)
							{
								Message k = new Message(0);
								k.WriteShort(IdEntity);
								c4.SendMessage(k);
							}
						}
					}
				}
				else if (IsMobVongLap)
				{
					Mob mob2 = this;
					if (_Zone.Mobs.TryRemove(IdEntity, out mob2) && mob2._Zone.Mobs.Values.Count == 0)
					{
						if (_myChar.InfoGame.CamThuat.CountVongLap == 17)
						{
							_myChar.InfoGame.CamThuat.SetWin();
						}
						else if (!_myChar.InfoGame.CamThuat.IsRespawBigBoss)
						{
							_myChar.InfoGame.CamThuat.ResawBigBoss();
						}
						else
						{
							for (short i = 0; i < _myChar.InfoGame.CamThuat.IdMobAdd; i++)
							{
								foreach (Character c7 in _Zone.Chars.Values)
								{
									if (c7.IsConnection)
									{
										Message l = new Message(0);
										l.WriteShort(i);
										c7.SendMessage(l);
									}
								}
							}
							sbyte CountVL = (sbyte)(_myChar.InfoGame.CamThuat.CountVongLap - 1);
							_myChar.InfoGame.CamThuat.IsResawMob1 = false;
							_myChar.InfoGame.CamThuat.TimeStart = Util.CurrentTimeMillis();
							_myChar.InfoGame.CamThuat.TimeHoatDong = 10000;
							_myChar.InfoGame.CamThuat.IsHoatDong = false;
							foreach (Character c6 in _Zone.Chars.Values)
							{
								if (c6.IsConnection)
								{
									c6.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(_myChar.InfoGame.CamThuat.TimeStart, _myChar.InfoGame.CamThuat.TimeHoatDong, _myChar.InfoGame.CamThuat.IsHoatDong));
									c6.SendMessage(UtilMessage.SendThongBao("Vòng lặp ảo tưởng lần " + CountVL + " sẽ bắt đầu sau 10 giây nữa", Util.WHITE));
								}
							}
						}
					}
				}
				DropItemMap(_myChar);
			}
			IsTrungDoc = false;
			IsAnChoang = false;
			IsLamCham = false;
			IsSuyYeu = false;
			IsBong = false;
			StatusAttack = 0;
			TimeDie = Util.CurrentTimeMillis() + 2000;
		}

		private bool IsCanAttack()
		{
			return !IsAnChoang;
		}

		public void Update()
		{
			try
			{
				for (int i = 0; i < Effs.Count; i++)
				{
					Effs[i].MobUpdate(this);
				}
				if (Hp <= 0 && TimeDie < Util.CurrentTimeMillis())
				{
					ResawInMap();
				}
				if (Hp <= 0)
				{
					return;
				}
				if (IsCanAttack())
				{
					AutoAttack();
				}
				if (InfoMob.PhanTramHoiPhucHp <= 0 || Hp >= HpFull || Util.CurrentTimeMillis() <= LastTimeHoiPhucHp)
				{
					return;
				}
				int HpUp = HpFull * InfoMob.PhanTramHoiPhucHp / 100;
				Hp += HpUp;
				if (Hp > HpFull)
				{
					Hp = HpFull;
				}
				foreach (Character c in _Zone.Chars.Values)
				{
					if (c.IsConnection)
					{
						c.SendMessage(MsgUpdateHp());
					}
				}
				LastTimeHoiPhucHp = Util.CurrentTimeMillis() + 500;
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
		}

		private Message MsgUpdateHp()
		{
			Message i = new Message(-36);
			i.WriteShort(IdEntity);
			i.WriteInt(Hp);
			return i;
		}

		private void DropItemMap(Character _myChar)
		{
			lock (_Zone.Chars)
			{
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 9)
				{
					if (Util.NextInt(0, 10) > 8)
					{
						if (_Zone.ID_ITEM_MAP >= 32766)
						{
							_Zone.ID_ITEM_MAP = 0;
						}
						ItemMap itemMap = new ItemMap(_Zone.ID_ITEM_MAP++, new Item((short)Util.NextInt(1, 2), IsLock: true));
						itemMap.IdChar = _myChar.Info.IdUser;
						itemMap.Cx = Cx;
						itemMap.Cy = Cy;
						if (InventoryHander.GetCountNotNullBag(_myChar) > 0)
						{
							TaskHander.UpCountTask(_myChar);
							InventoryHander.AddItemBag(_myChar, itemMap.item);
							itemMap = null;
						}
						else
						{
							_Zone.AddItemMap(itemMap);
						}
						if (_myChar.Task.IdRequire >= 3)
						{
							TaskHander.NextStep(_myChar);
						}
					}
				}
				else if (IsCaoThuNhanGia)
				{
					short[] ItemCaoThu = new short[5] { 217, 218, 278, 302, 315 };
					short ValueRandom = 2;
					switch (Id)
					{
					case 200:
						ValueRandom = 3;
						break;
					case 201:
						ValueRandom = 4;
						break;
					case 202:
						ValueRandom = 5;
						break;
					case 203:
						ValueRandom = 6;
						break;
					}
					for (int i = 0; i < ItemCaoThu.Length; i++)
					{
						Item item = new Item(ItemCaoThu[i], IsLock: false);
						item.Quantity = Util.NextInt(1, ValueRandom);
						ItemMap itemMap2 = new ItemMap(_Zone.ID_ITEM_MAP++, item);
						itemMap2.IdChar = _myChar.Info.IdUser;
						itemMap2.Cx = (short)Util.NextInt(Cx - 20, Cx + 20);
						itemMap2.Cy = Cy;
						_Zone.AddItemMap(itemMap2);
					}
					if (Id == 203)
					{
					}
					if (_myChar.Info.IdGiaToc != -1)
					{
						sbyte CountCamThuat = 0;
						switch (Id)
						{
						case 199:
							CountCamThuat = 1;
							break;
						case 200:
							CountCamThuat = 2;
							break;
						case 201:
							CountCamThuat = 3;
							break;
						case 202:
							CountCamThuat = 4;
							break;
						case 203:
							CountCamThuat = 5;
							break;
						}
						_myChar.InfoGame.GiaToc.SoLanMoCuaAi += CountCamThuat;
						string Text = _myChar.Info.Name + " đã tiêu diệt được " + DataServer.ArrMobTempalte[Id].Name + " tăng thêm " + CountCamThuat + " số lần vào cửa ải gia tộc";
						_myChar.InfoGame.GiaToc.AddLogGiaToc(Text);
						foreach (GiaTocTemplate.ThanhVienGiaToc c in _myChar.InfoGame.GiaToc.ThanhViens)
						{
							if (c._myChar != null && c._myChar.IsConnection)
							{
								c._myChar.SendMessage(UtilMessage.SendThongBao(Text, Util.YELLOW_MID));
							}
						}
					}
					LangLa.Server.Server.SendThongBaoFromServer(_myChar.Info.Name + " vừa tiêu diệt " + DataServer.ArrMobTempalte[Id].Name + " tại " + DataServer.ArrMapTemplate[_myChar.Info.MapId].Name);
				}
				else if (IsMobVongLap && Id == 238)
				{
					bool IsDrop = Util.NextInt(0, 1) == 0;
					short IdItem = 0;
					if (_myChar.InfoGame.CamThuat.CountVongLap > 0 && _myChar.InfoGame.CamThuat.CountVongLap <= 10)
					{
						IdItem = 435;
					}
					else if (_myChar.InfoGame.CamThuat.CountVongLap > 10 && _myChar.InfoGame.CamThuat.CountVongLap <= 15)
					{
						IdItem = 719;
					}
					else if (_myChar.InfoGame.CamThuat.CountVongLap > 15)
					{
						IdItem = 778;
					}
					if (IsDrop)
					{
						Item item2 = new Item(IdItem, IsLock: false);
						ItemMap itemMap3 = new ItemMap(_Zone.ID_ITEM_MAP++, item2);
						itemMap3.IdChar = _myChar.Info.IdUser;
						itemMap3.Cx = (short)Util.NextInt(Cx - 20, Cx + 20);
						itemMap3.Cy = Cy;
						_Zone.AddItemMap(itemMap3);
					}
				}
				else if (IsMobAiGiaToc && Id == 112)
				{
					if (Util.NextInt(0, 1) == 0)
					{
						Item item3 = new Item(870, IsLock: false);
						item3.Quantity = Util.NextInt(1, 2);
						ItemMap itemMap4 = new ItemMap(_Zone.ID_ITEM_MAP++, item3);
						itemMap4.IdChar = _myChar.Info.IdUser;
						itemMap4.Cx = (short)Util.NextInt(Cx - 20, Cx + 20);
						itemMap4.Cy = Cy;
						_Zone.AddItemMap(itemMap4);
					}
					if (_myChar.InfoGame.GiaToc.SoLanDiAiTrongNgay >= 2)
					{
						short VLRD = 20;
						if (_myChar.InfoGame.GiaToc.SoLanDiAiTrongNgay > 2)
						{
							VLRD = 50;
						}
						if (Util.NextInt(0, 100) < VLRD)
						{
							short IdRd = (short)((Util.NextInt(0, 2) == 0) ? 749 : 888);
							Item item4 = new Item(IdRd, IsLock: false);
							short DoTuLuyen = (short)((IdRd == 749) ? 22000 : 30000);
							StringBuilder stringBuilder = new StringBuilder();
							if (IdRd == 749)
							{
								stringBuilder.Append(128 + ",").Append(0 + "," + DoTuLuyen).Append(";")
									.Append(311 + "," + Util.NextInt(40, 90))
									.Append(";")
									.Append(0 + "," + Util.NextInt(900, 1200))
									.Append(1 + "," + Util.NextInt(900, 1200))
									.Append(";");
								switch (_myChar.Info.IdClass)
								{
								case 1:
									stringBuilder.Append(109 + "," + Util.NextInt(220, 260)).Append(";").Append(144 + "," + Util.NextInt(300, 400));
									break;
								case 2:
									stringBuilder.Append(110 + "," + Util.NextInt(220, 260)).Append(";").Append(115 + "," + Util.NextInt(300, 400));
									break;
								case 3:
									stringBuilder.Append(111 + "," + Util.NextInt(220, 260)).Append(";").Append(116 + "," + Util.NextInt(300, 400));
									break;
								case 4:
									stringBuilder.Append(112 + "," + Util.NextInt(220, 260)).Append(";").Append(117 + "," + Util.NextInt(300, 400));
									break;
								case 5:
									stringBuilder.Append(113 + "," + Util.NextInt(220, 260)).Append(";").Append(118 + "," + Util.NextInt(300, 400));
									break;
								}
							}
							else
							{
								stringBuilder.Append(128 + ",").Append(0 + "," + DoTuLuyen).Append(";")
									.Append(371 + "," + Util.NextInt(3, 8))
									.Append(";")
									.Append(311 + "," + Util.NextInt(90, 150))
									.Append(";")
									.Append(0 + "," + Util.NextInt(1400, 2300))
									.Append(";")
									.Append(1 + "," + Util.NextInt(1400, 2300))
									.Append(";");
								switch (_myChar.Info.IdClass)
								{
								case 1:
									stringBuilder.Append(109 + "," + Util.NextInt(261, 399)).Append(";").Append(144 + "," + Util.NextInt(401, 522));
									break;
								case 2:
									stringBuilder.Append(110 + "," + Util.NextInt(261, 399)).Append(";").Append(115 + "," + Util.NextInt(401, 522));
									break;
								case 3:
									stringBuilder.Append(111 + "," + Util.NextInt(261, 399)).Append(";").Append(116 + "," + Util.NextInt(401, 522));
									break;
								case 4:
									stringBuilder.Append(112 + "," + Util.NextInt(261, 399)).Append(";").Append(117 + "," + Util.NextInt(401, 522));
									break;
								case 5:
									stringBuilder.Append(113 + "," + Util.NextInt(261, 399)).Append(";").Append(118 + "," + Util.NextInt(401, 522));
									break;
								}
							}
							item4.Options = stringBuilder.ToString();
							ItemMap itemMap5 = new ItemMap(_Zone.ID_ITEM_MAP++, item4);
							itemMap5.IdChar = _myChar.Info.IdUser;
							itemMap5.Cx = (short)Util.NextInt(Cx - 20, Cx + 20);
							itemMap5.Cy = Cy;
							_Zone.AddItemMap(itemMap5);
						}
					}
					LangLa.Server.Server.SendThongBaoFromServer(_myChar.Info.Name + " vừa tiêu diệt " + DataServer.ArrMobTempalte[Id].Name + " tại " + DataServer.ArrMapTemplate[_myChar.Info.MapId].Name);
				}
				if (_myChar.Inventory.ItemBody[11] == null)
				{
				}
			}
		}

		private bool IsBigBoss()
		{
			return Id == 112 || Id == 238;
		}

		private void AutoAttack()
		{
			if (Speed <= 0 && !IsCaoThuNhanGia && !IsMobKhuRungChet && !IsMobAiGiaToc && !IsMobVongLap)
			{
				return;
			}
			long TimeNow = Util.CurrentTimeMillis();
			if (DelayAttack >= TimeNow)
			{
				return;
			}
			lock (_Zone.Chars)
			{
				Character cAnDame = null;
				short Range = (short)(50 + Speed);
				foreach (Character c2 in _Zone.Chars.Values)
				{
					if (c2.IsConnection && !c2.Info.IsDie && TimeNow > c2.InfoGame.TimeDelayMobAttack && Math.Abs(this.Cx - c2.Info.Cx) < Range && Math.Abs(this.Cy - c2.Info.Cy) < Range)
					{
						cAnDame = c2;
						break;
					}
				}
				if (cAnDame == null)
				{
					return;
				}
				int IdChar = cAnDame.Info.IdUser;
				int Dame = MobHander.GetDameAttackChar(this, cAnDame);
				if (cAnDame.InfoGame.IsUseSusanoo)
				{
					int DameHutMp = Dame * cAnDame.InfoGame.DungMpHutSatThuongSasunoo / 100;
					if (cAnDame.Point.Mp > DameHutMp)
					{
						Dame -= DameHutMp;
						PointHander.UpdateMpChar(cAnDame, -DameHutMp);
					}
				}
				if (IsBigBoss() && Id == 112)
				{
					Dame = (cAnDame.InfoGame.IsUseBiDuoc ? (cAnDame.Point.GetHpFull(cAnDame) * Util.NextInt(15, 25) / 100) : (cAnDame.Point.GetHpFull(cAnDame) * Util.NextInt(70, 85) / 100));
				}
				if (LevelBoss > 0 && !IsBigBoss())
				{
					Dame += cAnDame.Point.HpFull * 5 / 100;
				}
				if (SetDameBoss > 0)
				{
					Dame += SetDameBoss;
				}
				if (Dame < 0)
				{
					Dame = Level * 10;
				}
				Dame = Util.NextInt(Dame, (int)((double)Dame * 1.5));
				PointHander.UpdateHpChar(cAnDame, -Dame);
				bool IsDie = cAnDame.Point.Hp <= 0;
				int Hp = cAnDame.Point.Hp;
				int Mp = cAnDame.Point.Mp;
				short Cx = cAnDame.Info.Cx;
				short Cy = cAnDame.Info.Cy;
				foreach (Character c in _Zone.Chars.Values)
				{
					Message i = new Message(56);
					i.WriteShort(IdEntity);
					i.WriteInt(IdChar);
					c.SendMessage(i);
					i = new Message(55);
					i.WriteInt(IdChar);
					i.WriteInt(Mp);
					i.WriteInt(Hp);
					i.WriteBool(x: false);
					if (IsDie)
					{
						i.WriteShort(Cx);
						i.WriteShort(Cy);
						i.WriteUTF("");
					}
					c.SendMessage(i);
				}
				if (IsDie)
				{
					DelayAttack = TimeNow + 5000;
					StatusAttack = 0;
					cAnDame.SetDie();
					return;
				}
				short TimeDelay = (short)((StatusAttack == 0) ? Util.NextInt(5000, 7000) : 2300);
				if (LevelBoss > 0 && !IsBigBoss())
				{
					TimeDelay = 1750;
				}
				cAnDame.InfoGame.TimeDelayMobAttack = Util.CurrentTimeMillis() + TimeDelay;
				DelayAttack = Util.CurrentTimeMillis() + TimeDelay;
			}
		}

		private void ResawInMap()
		{
			if (LevelBoss == 1 || LevelBoss == 2)
			{
				if (LevelBoss == 1)
				{
					HpFull -= HpTinhAnh;
					Exp -= ExpTinhAnh;
				}
				else if (LevelBoss == 2)
				{
					HpFull -= HpThuLinh;
					Exp -= ExpThuLinh;
				}
				LevelBoss = 0;
			}
			Hp = HpFull;
			MobHander.SetPointRespawMob(this);
			lock (_Zone.Chars)
			{
				foreach (Character Char in _Zone.Chars.Values)
				{
					if (Char.IsConnection)
					{
						Message i = new Message(57);
						i.WriteShort(IdEntity);
						i.WriteShort(Level);
						i.WriteByte(IdClass);
						i.WriteInt(Hp);
						i.WriteInt(HpFull);
						i.WriteInt(Exp);
						i.WriteByte(LevelBoss);
						i.WriteByte(0);
						Char.SendMessage(i);
					}
				}
			}
		}

		public void Write(Message msg)
		{
			msg.WriteShort(IdEntity);
			msg.WriteBool(IsPainMiniMap);
			msg.WriteUTF(Name);
			msg.WriteShort(Id);
			msg.WriteShort(Cx);
			msg.WriteShort(Cy);
			msg.WriteShort(Level);
			msg.WriteByte(IdClass);
			msg.WriteByte((sbyte)((Hp <= 0) ? 4 : 0));
			msg.WriteInt(Hp);
			msg.WriteInt(HpFull);
			msg.WriteInt(Exp);
			msg.WriteByte(LevelBoss);
			WriteEff(msg);
		}

		public void WriteEff(Message msg)
		{
			msg.WriteByte((sbyte)Effs.Count);
			foreach (InfoEff eff in Effs)
			{
				msg.WriteShort(eff.Id);
				msg.WriteInt(eff.Value);
				msg.WriteLong(eff.TimeStart);
				msg.WriteInt(eff.TimeEnd);
			}
		}
	}
}
