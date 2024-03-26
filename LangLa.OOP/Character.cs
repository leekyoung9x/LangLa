using System;
using System.Collections.Generic;
using System.Linq;
using LangLa.Client;
using LangLa.Data;
using LangLa.Hander;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.Manager;
using LangLa.Server;
using LangLa.SqlConnection;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.OOP
{
	public class Character
	{
		public sbyte Index;

		public bool IsConnection = true;

		private LangLa.Client.Client _Client;

		public LangLa.InfoChar.InfoChar Info;

		public InfoInventory Inventory;

		public InfoPoint Point;

		public InfoInGame InfoGame;

		public InfoTuongKhac TuongKhac;

		public InfoSkill Skill;

		public InfoGiftCode GiftCode;

		public TaskChar Task;

		public List<InfoEff> Effs;

		public List<InfoThu> Thus;

		public List<InfoFriend> Friends;

		public List<InfoEnemy> Enemies;

		public List<InfoDanhHieu> DanhHieus;

		public InfoViThu ViThu;

		public InfoCharTime TimeChar;

		private long TimeAttackPhanThan;

		private long LastTimeAutoUpdate;

		public bool IsActive;

		public void Update()
		{
			if (!IsConnection)
			{
				return;
			}
			try
			{
				for (int i = 0; i < Effs.Count; i++)
				{
					Effs[i].Update(this);
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
				_Client.close();
			}
			finally
			{
				TuongKhac.Update(this);
				if (Util.CurrentTimeMillis() > LastTimeAutoUpdate)
				{
					LastTimeAutoUpdate = Util.CurrentTimeMillis() + 3600000;
					ConnectionUser.UpdateChar(this);
				}
			}
		}

		public void SendMessage(Message msg)
		{
			if (IsConnection)
			{
				_Client.AddMessage(msg);
			}
		}

		public Character()
		{
		}

		public Character(sbyte select, string name, LangLa.Client.Client _Client)
		{
			this._Client = _Client;
			Info = new LangLa.InfoChar.InfoChar(select, name);
			Info.IdUser = _Client.IdUser;
			Inventory = new InfoInventory(IsNew: true);
			Point = new InfoPoint(Info.IdClass);
			Task = new TaskChar();
			InfoGame = new InfoInGame();
			Effs = new List<InfoEff>();
			Thus = new List<InfoThu>();
			Friends = new List<InfoFriend>();
			Enemies = new List<InfoEnemy>();
			Skill = new InfoSkill(Info.IdClass, Info.GioiTinh);
			TuongKhac = new InfoTuongKhac();
			TimeChar = new InfoCharTime();
			GiftCode = new InfoGiftCode();
			DanhHieus = new List<InfoDanhHieu>();
			ViThu = new InfoViThu();
			Task.Id = 0;
			Task.IdStep = -1;
			Info.Cx = 555;
			Info.Cy = 555;
			Info.MapId = 75;
			TuongKhac.DameCoBan = 10;
			IsConnection = true;
			WriteMe();
		}

		private void Reset()
		{
			TuongKhac.DameCoBan = 0;
			TuongKhac.PhatHuyLucDanhCoban = 0;
			TuongKhac.ChiMang = 0;
			TuongKhac.ChakaraFromItem = 0;
			TuongKhac.NeTranh = 0;
			TuongKhac.ChinhXac = 0;
			TuongKhac.BoQuaNeTranh = 0;
			TuongKhac.TangDameChiMang = 0;
			TuongKhac.TangTanCongLenLoi = 0;
			TuongKhac.TangTanCongLenHoa = 0;
			TuongKhac.TangTanCongLenTho = 0;
			TuongKhac.TangTanCongLenThuy = 0;
			TuongKhac.TangTanCongLenPhong = 0;
			TuongKhac.GaySuyYeu = 0;
			TuongKhac.GayTrungDoc = 0;
			TuongKhac.GayChoang = 0;
			TuongKhac.GayLamCham = 0;
			TuongKhac.GayBong = 0;
			TuongKhac.BoQuaKhangTinh = 0;
			TuongKhac.KhangLoi = 0;
			TuongKhac.KhangHoa = 0;
			TuongKhac.KhangThuy = 0;
			TuongKhac.KhangTho = 0;
			TuongKhac.KhangPhong = 0;
			TuongKhac.GiamSatThuong = 0;
			Info.Speed = 500;
			TuongKhac.PhanTramPhanDon = 0;
			TuongKhac.PhongChiMang = 0;
			TuongKhac.TileTuongKhac = 0;
			TuongKhac.GiamChiMang = 0;
			Info.TaiPhu = 0;
			TuongKhac.TileGiamTuongKhac = 0;
			TuongKhac.GiamSuyYeu = 0;
			TuongKhac.GiamTrungDoc = 0;
			TuongKhac.GiamGayBong = 0;
			TuongKhac.GiamTrungDoc = 0;
			TuongKhac.GiamGayChoang = 0;
			TuongKhac.NuaGiayHoiHp = 0;
			TuongKhac.NuaGiayHoiMp = 0;
			TuongKhac.IsMaxCrit = false;
			TuongKhac.DameLenQuai = 0;
			Point.PhanTramHpToiDa = 0;
			Point.PhanTramMpToiDa = 0;
			Point.Hp = (Point.HpFull = 100);
			Point.Mp = (Point.MpFull = 100);
			TuongKhac.DameCoBan = 10;
		}

		public void WriteMe()
		{
			InfoGame.SetLevel(Point.Exp, ref Info.Level);
			Message j = UtilMessage.Message122();
			j.WriteByte(-127);
			j.WriteUTF("");
			j.WriteInt(Info.IdUser);
			j.WriteUTF(Info.Name);
			j.WriteByte(Info.GioiTinh);
			j.WriteByte(Info.IdChar);
			j.WriteByte(Info.IdClass);
			j.WriteByte(Info.IdClass);
			j.WriteByte(InfoGame.TypePk);
			j.WriteByte(InfoGame.LevelPk);
			j.WriteInt(Info.TaiPhu);
			j.WriteShort(Info.Speed);
			j.WriteByte(Info.SachChienDau);
			j.WriteInt(Point.Hp);
			j.WriteInt(Point.GetHpFull(this));
			j.WriteInt(Point.Mp);
			j.WriteInt(Point.GetMpFull());
			j.WriteLong(Point.Exp);
			j.WriteInt(Inventory.Bac);
			j.WriteInt(Inventory.BacKhoa);
			j.WriteInt(Inventory.Vang);
			j.WriteInt(Inventory.VangKhoa);
			j.WriteShort(Task.Id);
			j.WriteByte(Task.IdStep);
			j.WriteUShort(Task.IdRequire);
			j.WriteInt(Info.HoatLuc);
			j.WriteInt(Info.PointNap);
			j.WriteShort((short)Inventory.ItemBag.Count);
			Item[] SizeMoRong = Inventory.ItemMoRongTui.Where((Item s) => s != null).ToArray();
			j.WriteByte((sbyte)SizeMoRong.Length);
			int i = 0;
			Item[] array = SizeMoRong;
			foreach (Item c in array)
			{
				j.WriteShort(c.Id);
				j.WriteBool(c.IsLock);
				j.WriteByte((sbyte)i);
				i++;
			}
			WriteBody(j);
			WriteBody2(j);
			WriteBag(j);
			WirteEff(j);
			j.WriteShort((short)Thus.Count);
			foreach (InfoThu thu in Thus)
			{
				ThuHander.Write(j, thu);
			}
			j.WriteShort((short)Friends.Count);
			foreach (InfoFriend fr in Friends)
			{
				j.WriteUTF(fr.Name);
				j.WriteByte(2);
				j.WriteBool(fr.IsOn);
			}
			Enemies.Clear();
			j.WriteShort((short)Enemies.Count);
			SkillHander.Write(j, Skill.Skills, -1);
			WriteDanhHieu(j);
			j.WriteByte((sbyte)(DanhHieus.Count - 1));
			j.WriteByte(Info.Rank);
			j.WriteByte(Info.SelectCaiTrang);
			j.WriteInt(-1);
			j.WriteByte(-1);
			j.WriteBool(x: false);
			WriteSkillViThu(j);
			_Client.AddMessage(j);
			MapManager.Maps[Info.MapId].LoginGame(this);
			InfoGame.TimeDelayMobAttack = Util.CurrentTimeMillis() + 5000;
			j = new Message(-104);
			j.WriteShort(0);
			_Client.AddMessage(j);
			JoinMap(-1, -1, IsNew: true);
			TimeChar.SetTime();
			LastTimeAutoUpdate = Util.CurrentTimeMillis() + 3600000;
			if (Info.IdGiaToc != -1)
			{
				InfoGame.SetGiaToc(this);
			}
		}

		public void WriteSkillViThu(Message m)
		{
			m.WriteByte((sbyte)ViThu.VyThus.Count);
			foreach (InfoViThu.ViThu v in ViThu.VyThus)
			{
				m.WriteByte(v.Id);
				m.WriteByte(v.Level);
			}
		}

		public void WriteDanhHieu(Message m)
		{
			m.WriteByte((sbyte)DanhHieus.Count);
			for (int i = 0; i < DanhHieus.Count; i++)
			{
				string Name = DanhHieus[i].Name;
				m.WriteUTF(Name);
				m.WriteInt(DanhHieus[i].Time);
				if (Name.Equals(" "))
				{
					m.WriteInt(DanhHieus[i].SetDetail);
				}
			}
		}

		public void WriteInfo()
		{
			Message j = new Message(63);
			j.WriteInt(Info.LevelCheTao);
			j.WriteInt(Info.HoatLuc);
			j.WriteByte(Info.SachChienDau);
			j.WriteShort(this.Point.DiemKyNang);
			j.WriteShort(this.Point.DiemTiemNang);
			j.WriteBool(x: false);
			j.WriteByte(0);
			int i = 0;
			short[] arrayPoint = this.Point.ArrayPoint;
			foreach (short Point in arrayPoint)
			{
				short Poin2 = Point;
				if (i == 1)
				{
					Poin2 += TuongKhac.ChakaraFromItem;
					Poin2 += InfoGame.ChakraCuuViHinh;
					Poin2 += InfoGame.GiamChakraAnChuChiThuat;
				}
				j.WriteShort(Poin2);
				i++;
			}
			TuongKhac.Write(this, j, Info.Speed);
			SendMessage(j);
		}

		public Message MsgWriteInfo()
		{
			Message j = new Message(63);
			j.WriteInt(Info.LevelCheTao);
			j.WriteInt(Info.HoatLuc);
			j.WriteByte(Info.SachChienDau);
			j.WriteShort(this.Point.DiemKyNang);
			j.WriteShort(this.Point.DiemTiemNang);
			j.WriteBool(x: true);
			j.WriteByte(0);
			int i = 0;
			short[] arrayPoint = this.Point.ArrayPoint;
			foreach (short Point in arrayPoint)
			{
				short Poin2 = Point;
				if (i == 1)
				{
					Poin2 += TuongKhac.ChakaraFromItem;
					Poin2 += InfoGame.ChakraCuuViHinh;
				}
				j.WriteShort(Poin2);
				i++;
			}
			TuongKhac.Write(this, j, Info.Speed);
			return j;
		}

		public void WriteDataBag()
		{
			Message i = new Message(-95);
			i.WriteInt(Inventory.Bac);
			WriteBag(i);
			i.WriteByte(0);
			i.WriteByte(0);
			i.WriteInt(0);
			SendMessage(i);
		}

		public Message WriteThongTinMe()
		{
			Message i = new Message(-123);
			i.WriteByte(-73);
			i.WriteInt(Point.GetHpFull(this));
			i.WriteInt(Point.Hp);
			i.WriteInt(Point.GetMpFull());
			i.WriteInt(Point.Mp);
			i.WriteByte(InfoGame.LevelPk);
			i.WriteShort((short)((TuongKhac.PhanTramKinhNghiemDanhQuai + InfoGame.ToDoiKinhNghiem) * LangLa.Server.Server.ExpServer));
			i.WriteInt(Info.TaiPhu);
			i.WriteShort(0);
			i.WriteShort(0);
			i.WriteLong(0L);
			i.WriteInt(0);
			i.WriteInt(0);
			i.WriteInt(0);
			i.WriteShort(0);
			i.WriteLong(0L);
			i.WriteInt(0);
			short[] pointItemHokage = Info.PointItemHokage;
			foreach (short s in pointItemHokage)
			{
				i.WriteShort(s);
			}
			i.WriteByte(Point.PointUseSachTiemNang[0]);
			i.WriteByte(Point.PointUseSachTiemNang[1]);
			i.WriteByte(Point.PointUseSachTiemNang[2]);
			i.WriteByte(Point.PointUseSachKyNang[0]);
			i.WriteByte(Point.PointUseSachKyNang[1]);
			i.WriteByte(Point.PointUseSachKyNang[2]);
			i.WriteByte(0);
			i.WriteByte(28);
			i.WriteByte(29);
			i.WriteByte(30);
			return i;
		}

		public void ShowThongTin(Message msg)
		{
			string NameFind = msg.ReadString();
			if (Info.Name.Equals(NameFind))
			{
				SendMessage(WriteThongTinMe());
			}
			else if (InfoGame._CView != null && InfoGame._CView.IsConnection)
			{
				SendMessage(InfoGame._CView.WriteThongTinMe());
			}
		}

		public void NextMap()
		{
			MapManager.Maps[Info.MapId].NextMap(this);
		}

		public void JoinMap(short MapJoint = -1, sbyte ZoneId = -1, bool IsNew = false)
		{
			short MapJoint2 = ((MapJoint == -1) ? Info.MapId : MapJoint);
			InfoGame.TimeDelayMobAttack = Util.CurrentTimeMillis() + 5000;
			Message i = new Message(-104);
			i.WriteShort(0);
			_Client.AddMessage(i);
			LoginMap(ZoneId, IsNew);
		}

		public void LoginMap(sbyte ZoneId = -1, bool IsLogin = false)
		{
			short MapJoint2 = Info.MapId;
			Message i = new Message(-111);
			i.WriteByte(3);
			i.WriteShort(MapJoint2);
			i.Wirte(DataServer.ArrMapTemplate[MapJoint2].ArrMap);
			_Client.AddMessage(i);
			Map map = MapManager.Maps[MapJoint2];
			if (!IsLogin && !ZoneHander.SetZoneMap(this, map, ZoneId))
			{
				_Client.close();
			}
			i = new Message(-103);
			i.WriteShort(InfoGame.ZoneGame.Id);
			i.WriteShort(map.Id);
			i.WriteShort(Info.Cx);
			i.WriteShort(Info.Cy);
			lock (InfoGame.ZoneGame.ItemMaps)
			{
				i.WriteShort((short)InfoGame.ZoneGame.ItemMaps.Values.Count);
				foreach (ItemMap ItemMap in InfoGame.ZoneGame.ItemMaps.Values)
				{
					ItemMap.Write(i);
				}
			}
			lock (InfoGame.ZoneGame.Chars)
			{
				Character[] Csend = InfoGame.ZoneGame.Chars.Values.Where((Character s) => s.IsConnection && s.Info.IdUser != Info.IdUser).ToArray();
				i.WriteByte((sbyte)Csend.Length);
				Character[] array = Csend;
				foreach (Character Char in array)
				{
					i.WriteInt(Char.Info.IdUser);
					Char.Write(i);
				}
			}
			lock (InfoGame.ZoneGame.Mobs)
			{
				Mob[] MSend = InfoGame.ZoneGame.Mobs.Values.ToArray();
				i.WriteUShort((ushort)MSend.Length);
				Mob[] array2 = MSend;
				foreach (Mob mob in array2)
				{
					mob.Write(i);
				}
			}
			NpcTemplate[] npc = DataServer.ArrMapTemplate[MapJoint2].ArrNpc;
			if (npc != null)
			{
				i.WriteShort((short)DataServer.ArrMapTemplate[MapJoint2].ArrNpc.Length);
				NpcTemplate[] arrNpc = DataServer.ArrMapTemplate[MapJoint2].ArrNpc;
				foreach (NpcTemplate npcTemplate in arrNpc)
				{
					i.WriteByte(npcTemplate.status);
					i.WriteShort(npcTemplate.id);
					i.WriteShort(npcTemplate.cx);
					i.WriteShort(npcTemplate.cy);
				}
			}
			else
			{
				i.WriteShort(0);
			}
			i.WriteByte(InfoGame.TypePk);
			i.WriteLong(0L);
			i.WriteInt(0);
			i.WriteBool(x: false);
			i.WriteBool(x: false);
			_Client.AddMessage(i);
			lock (InfoGame.ZoneGame.Chars)
			{
				Character[] Csend2 = InfoGame.ZoneGame.Chars.Values.Where((Character s) => s.IsConnection && s.Info.IdUser != Info.IdUser).ToArray();
				Character[] array3 = Csend2;
				foreach (Character x in array3)
				{
					if (x.IsConnection && x.Info.IdGiaToc != -1)
					{
						SendMessage(GiaTocHander.SendGiaTocMsg(x));
					}
				}
			}
		}

		public void WriteBody(Message m)
		{
			int SizeBody = Inventory.ItemBody.Count((Item s) => s != null);
			m.WriteByte((sbyte)SizeBody);
			foreach (Item it in Inventory.ItemBody)
			{
				if (it != null)
				{
					ItemHander.WriteItemBody(m, it);
				}
			}
		}

		public void WriteBag(Message m)
		{
			int SizeBag = Inventory.ItemBag.Count((Item s) => s != null);
			m.WriteShort((short)SizeBag);
			foreach (Item it in Inventory.ItemBag)
			{
				if (it != null)
				{
					ItemHander.WriteItem(m, it);
				}
			}
		}

		public void WriteBody2(Message m)
		{
			int SizeBody2 = Inventory.ItemBody2.Count((Item s) => s != null);
			m.WriteByte((sbyte)SizeBody2);
			foreach (Item it in Inventory.ItemBody2)
			{
				if (it != null)
				{
					ItemHander.WriteItemBody(m, it);
				}
			}
		}

		public void WirteEff(Message m)
		{
			m.WriteByte((sbyte)Effs.Count);
			foreach (InfoEff eff in Effs)
			{
				m.WriteShort(eff.Id);
				m.WriteInt(eff.Value);
				m.WriteLong(eff.TimeStart);
				m.WriteInt(eff.TimeEnd);
			}
		}

		public void SendItemBag()
		{
			Message i = new Message(83);
			i.WriteInt(Inventory.Bac);
			WriteBag(i);
			SendMessage(i);
		}

		public void UpdatePoint(Message msg)
		{
			short NumDown = 0;
			short NumAll = 0;
			short NumHp = 0;
			short NumChara = 0;
			short NumMp = 0;
			short NumDame = 0;
			short[] ArrCheck = new short[Point.ArrayPoint.Length];
			for (int j = 0; j < Point.ArrayPoint.Length; j++)
			{
				NumDown = msg.ReadShort();
				if (NumDown > Point.ArrayPoint[j])
				{
					short NumUp = (short)(NumDown - Point.ArrayPoint[j]);
					switch (j)
					{
					case 0:
						NumDame = NumUp;
						break;
					case 1:
						NumUp -= TuongKhac.ChakaraFromItem;
						NumChara = NumUp;
						break;
					case 3:
						NumHp = NumUp;
						break;
					case 2:
						NumMp = NumUp;
						break;
					}
					NumAll += NumUp;
				}
				ArrCheck[j] = NumDown;
				if (j == 1)
				{
					ArrCheck[j] = (short)(NumDown - TuongKhac.ChakaraFromItem);
				}
			}
			if (NumAll > Point.DiemTiemNang)
			{
				return;
			}
			Point.DiemTiemNang -= NumAll;
			UpdateDataChar();
			for (int i = 0; i < ArrCheck.Length; i++)
			{
				Point.ArrayPoint[i] = ArrCheck[i];
			}
			if (NumHp > 0)
			{
				Point.HpFull += NumHp * 9;
				PointHander.UpdateHpFullChar(this);
			}
			if (NumMp > 0)
			{
				Point.MpFull += NumMp * 9;
				PointHander.UpdateMpFullChar(this);
			}
			if (NumChara > 0)
			{
				sbyte PointCrit = Point.PointCheckCrit;
				while (NumChara > 0)
				{
					Point.HpFull += 2;
					Point.MpFull += 2;
					TuongKhac.DameCoBan++;
					if (PointCrit == 2)
					{
						TuongKhac.ChiMang++;
					}
					TuongKhac.NeTranh++;
					NumChara--;
					PointCrit++;
					if (PointCrit == 3)
					{
						PointCrit = 0;
					}
				}
				Point.PointCheckCrit = PointCrit;
				PointHander.UpdateHpFullChar(this);
				PointHander.UpdateMpFullChar(this);
				WriteInfo();
			}
			if (NumDame > 0)
			{
				TuongKhac.DameCoBan += NumDame * 2;
				WriteInfo();
			}
			if (Task.Id == 8 && Task.IdStep == 10)
			{
				TaskHander.NextStep(this);
			}
		}

		public void MsgUpdateSkill()
		{
			Message i = new Message(126);
			SkillHander.Write(i, Skill.Skills, -1);
			SendMessage(i);
		}

		public void UpdateDataChar()
		{
			Message i = new Message(-49);
			i.WriteShort(Point.DiemTiemNang);
			i.WriteShort(Point.DiemKyNang);
			i.WriteByte(0);
			i.WriteByte(0);
			i.WriteByte(0);
			i.WriteBool(x: true);
			i.WriteInt(Info.PointNap);
			SendMessage(i);
		}

		public void Write(Message msg)
		{
			if (IsConnection)
			{
				msg.WriteByte(InfoGame.Status);
				msg.WriteUTF(Info.Name);
				msg.WriteByte(Info.IdChar);
				msg.WriteByte(Info.GioiTinh);
				msg.WriteByte(Info.IdClass);
				msg.WriteByte(InfoGame.TypePk);
				msg.WriteByte(InfoGame.LevelPk);
				msg.WriteShort(Info.Speed);
				msg.WriteInt(Point.Hp);
				msg.WriteInt(Point.GetHpFull(this));
				msg.WriteInt(Point.Mp);
				msg.WriteInt(Point.GetMpFull());
				msg.WriteLong(Point.Exp);
				msg.WriteShort(Info.Cx);
				msg.WriteShort(Info.Cy);
				msg.WriteByte(InfoGame.StatusGD);
				WriteBody(msg);
				WirteEff(msg);
				WriteDanhHieu(msg);
				msg.WriteByte(Info.SelectDanhHieu);
				msg.WriteByte(Info.Rank);
				msg.WriteByte(Info.SelectCaiTrang);
			}
		}

		public Item GetItemFromType(sbyte Type, short Index)
		{
			return Type switch
			{
				0 => Inventory.ItemBag[Index], 
				1 => Inventory.ItemBox[Index], 
				2 => Inventory.ItemBody[Index], 
				3 => Inventory.ItemBody2[Index], 
				_ => null, 
			};
		}

		public void SetNullItemFromType(sbyte Type, short Index)
		{
			switch (Type)
			{
			case 0:
				Inventory.ItemBag[Index] = null;
				break;
			case 1:
				Inventory.ItemBox[Index] = null;
				break;
			case 2:
				Inventory.ItemBody[Index] = null;
				break;
			case 3:
				Inventory.ItemBody2[Index] = null;
				break;
			}
		}

		public void SetSocket(LangLa.Client.Client client)
		{
			_Client = client;
			if (_Client.TotalMoney >= 20000)
			{
				Info.IsActive = true;
			}
			TimeChar.SoVangHienCo = _Client.Money;
			InfoGame = new InfoInGame();
			IsConnection = true;
			WriteMe();
		}

		public void SetXY(short Cx, short Cy, bool isNext = false)
		{
			Info.Cx = (short)(isNext ? (Info.Cx + Cx) : Cx);
			Info.Cy = (short)(isNext ? (Info.Cy + Cy) : Cy);
		}

		public bool IsMapPhuBan()
		{
			return Info.MapId == 89 || Info.MapId == 2 || Info.MapId == 22 || Info.MapId == 46 || Info.MapId == 47;
		}

		public void UpdateExp(long Exp)
		{
			if (Info.IsLockCap)
			{
				return;
			}
			Point.Exp += Exp;
			long var2 = Point.Exp;
			sbyte var3 = 0;
			while (var3 < DataServer.ArrExp.Length && var2 >= DataServer.ArrExp[var3])
			{
				var2 -= DataServer.ArrExp[var3];
				var3++;
			}
			if (var3 > Info.Level)
			{
				short PointUp = (short)(var3 - Info.Level);
				Point.DiemKyNang += PointUp;
				Point.DiemTiemNang += (short)(PointUp * 4);
				Info.Level = var3;
				{
					foreach (Character c in InfoGame.ZoneGame.Chars.Values)
					{
						if (c.IsConnection)
						{
							Message i = new Message(94);
							i.WriteLong(Point.Exp);
							i.WriteInt(Info.IdUser);
							c.SendMessage(i);
						}
					}
					return;
				}
			}
			Message j = new Message(94);
			j.WriteLong(Point.Exp);
			j.WriteInt(Info.IdUser);
			SendMessage(j);
		}

		public void SetDie()
		{
			if (InfoGame.IsCuuSat)
			{
				Character cAnCuuSat2 = InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == InfoGame.IdCuuSat);
				if (cAnCuuSat2 != null)
				{
					cAnCuuSat2.SendMessage(PvPHander.MsgCloseCuuSat(Info.IdUser, IsCloseCuuSat: true));
					cAnCuuSat2.InfoGame.CleanUpCuuSat();
				}
				SendMessage(PvPHander.MsgCloseCuuSat(Info.IdUser, IsCloseCuuSat: true));
			}
			if (InfoGame.IsAnCuuSat)
			{
				Character cAnCuuSat = InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == InfoGame.IdCharMoiCuuSat);
				if (cAnCuuSat != null)
				{
					cAnCuuSat.SendMessage(PvPHander.MsgCloseCuuSat(cAnCuuSat.Info.IdUser, IsCloseCuuSat: true));
					cAnCuuSat.InfoGame.CleanUpCuuSat();
				}
				SendMessage(PvPHander.MsgCloseCuuSat(InfoGame.IdCharMoiCuuSat, IsCloseCuuSat: true));
			}
			if (InfoGame.IsTyVo)
			{
				Character cTyVo = InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == InfoGame.IdTyVo);
				if (cTyVo != null)
				{
					foreach (Character c2 in cTyVo.InfoGame.ZoneGame.Chars.Values)
					{
						if (c2.IsConnection)
						{
							c2.SendMessage(PvPHander.MsgCloseTyVo(2, cTyVo.Info.IdUser, Info.IdUser));
						}
					}
					cTyVo.InfoGame.CleanUpTyvo();
				}
				foreach (Character c in InfoGame.ZoneGame.Chars.Values)
				{
					if (c.IsConnection)
					{
						SendMessage(PvPHander.MsgCloseTyVo(2, InfoGame.IdTyVo, Info.IdUser));
					}
				}
				InfoGame.CleanUpTyvo();
			}
			Point.Hp = 0;
			Info.IsDie = true;
			InfoGame.TimeDelayMobAttack = Util.CurrentTimeMillis() + 5000;
		}

		public void SetLive()
		{
			if (Inventory.VangKhoa > 0)
			{
				InventoryHander.UpdateVangKhoa(this, 1);
			}
			else
			{
				if (Inventory.Vang <= 0)
				{
					SendMessage(UtilMessage.SendThongBao("Bạn không đủ vàng để thực hiện", Util.YELLOW_MID));
					return;
				}
				InventoryHander.UpdateVang(this, 1);
			}
			int Hp = (Point.Hp = Point.GetHpFull(this));
			int Mp = (Point.Mp = Point.GetMpFull());
			Info.IsDie = false;
			int ID = Info.IdUser;
			foreach (Character c in InfoGame.ZoneGame.Chars.Values)
			{
				if (c.IsConnection)
				{
					Message i = new Message(49);
					i.WriteInt(ID);
					i.WriteInt(Hp);
					i.WriteInt(Mp);
					c.SendMessage(i);
				}
			}
		}

		public void BackHome()
		{
			Info.MapId = (short)((Info.MapBackHome == -1) ? 75 : Info.MapBackHome);
			short Cx = -1;
			short Cy = -1;
			switch (Info.MapId)
			{
			case 75:
				Cx = (short)Util.NextInt(336, 504);
				Cy = 226;
				break;
			case 59:
				Cx = (short)Util.NextInt(1090, 1302);
				Cy = 114;
				break;
			case 60:
				Cx = (short)Util.NextInt(111, 477);
				Cy = 262;
				break;
			case 68:
				Cx = (short)Util.NextInt(1302, 1528);
				Cy = 286;
				break;
			case 69:
				Cx = (short)Util.NextInt(537, 687);
				Cy = 423;
				break;
			case 85:
				Cx = (short)Util.NextInt(184, 326);
				Cy = 112;
				break;
			case 102:
				Cx = (short)Util.NextInt(1088, 1252);
				Cy = 660;
				break;
			}
			Info.Cx = Cx;
			Info.Cy = Cy;
			int Hp = (Point.Hp = Point.HpFull);
			int Mp = (Point.Mp = Point.MpFull);
			Info.IsDie = false;
			int ID = Info.IdUser;
			foreach (Character c in InfoGame.ZoneGame.Chars.Values)
			{
				Message i = new Message(49);
				i.WriteInt(ID);
				i.WriteInt(Hp);
				i.WriteInt(Mp);
				c.SendMessage(i);
			}
			InfoGame.ZoneGame.RemoveChar(this);
			JoinMap(-1, -1);
		}

		public void LoginGame()
		{
		}

		public void CleanUp()
		{
			IsConnection = false;
			if (InfoGame == null)
			{
				return;
			}
			try
			{
				if (InfoGame.UseSkillCuuViHinh)
				{
					TuongKhac.UpdateChakra(this, (short)(-InfoGame.ChakraCuuViHinh));
				}
				if (InfoGame.IsDinhAnChuChiThuat)
				{
					TuongKhac.UpdateChakra(this, (short)(-InfoGame.GiamChakraAnChuChiThuat));
				}
				for (int i = 0; i < Effs.Count; i++)
				{
					if (Effs[i].IsAutoRemove)
					{
						Effs.RemoveAt(i);
					}
				}
				if (Info.MapId == 2 || Info.MapId == 2 || Info.MapId == 46 || Info.MapId == 47 || Info.MapId == 89)
				{
					Info.MapId = 86;
					Info.Cx = 578;
					Info.Cy = 644;
				}
				if (InfoGame.Todoi != null)
				{
					InfoGame.Todoi.Remove(this);
				}
				if (Task.Id == 4 && Task.IdStep == 1)
				{
					Task.IdStep = 0;
				}
				if (Info.IsDie)
				{
					SetLive();
				}
				if (Info.IdGiaToc != -1)
				{
					InfoGame.CleanUpGiaToc(Info.IdUser);
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				TimeChar.SetLogOut();
				InfoGame.ZoneGame.RemoveChar(this);
				ConnectionUser.UpdateChar(this);
				InfoGame = null;
				_Client = null;
			}
		}
	}
}
