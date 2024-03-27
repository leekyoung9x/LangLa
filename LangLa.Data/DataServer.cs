using System;
using System.IO;
using System.Linq;
using System.Text;
using LangLa.Hander;
using LangLa.IO;
using LangLa.Manager;
using LangLa.SupportOOP;
using LangLa.Template;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LangLa.Data
{
	public static class DataServer
	{
		public static readonly sbyte[]? Data_Game = _ReadArrDataGame1();

		public static readonly sbyte[]? Data_Game2 = _ReadArrDataGame2();

		public static sbyte[]? Data_Game2Change;

		public static Skill[]? ArrSkill;

		public static SkillTemplate[]? ArrSkillTemplate;

		public static MapTemplate[]? ArrMapTemplate;

		public static MobTemplate[]? ArrMobTempalte;

		public static ItemTemplate[]? ArrItemTemplate;

		public static TaskTempalte[]? ArrTaskTemplate;

		public static ItemOptionTemplate[]? ArrItemOptionTemplate;

		public static SkillClanTemplate[]? ArrSkillClan;

		public static SkillClanTemplate[]? ArrSkillViThu;

		public static EffTemplate[]? ArrEffTemplate;

		public static long[]? ArrExp;

		public static short[][]? DoubleWaypoint;

		public static long[]? DiemGhepDa;

		public static long[]? DiemNangCapVuKhi;

		public static long[]? DiemNangCapTrangBi;

		public static long[]? DiemNangCapPhuKien;

		public static int[]? BacGhepDa;

		public static int[]? BacNangCapVuKhi;

		public static int[]? BacNangCapTrangBi;

		public static int[]? BacNangCapPhuKien;

		public static int[]? NgocKhamPoint;

		public static short[]? ArrIconChar;

		public static Item[]? ItemCaiTrang;

		public static SkillTemplate GetSkillTemplate(short id)
		{
			return ArrSkillTemplate[id];
		}

		private static void _ReadDataGame1()
		{
			Message m2 = new Message(0, Util.Decompress(Array.ConvertAll(Data_Game, (sbyte s) => (byte)s)));
			sbyte dataIconChar = m2.ReadByte();
			ArrIconChar = new short[dataIconChar];
			for (int k = 0; k < dataIconChar; k++)
			{
				ArrIconChar[k] = m2.ReadShort();
			}
			sbyte dataNameClass = m2.ReadByte();
			for (int n = 0; n < dataNameClass; n++)
			{
				string name = m2.ReadString();
			}
			sbyte dataNameChar = m2.ReadByte();
			for (int i11 = 0; i11 < dataNameChar; i11++)
			{
				m2.ReadString();
				m2.ReadByte();
				m2.ReadShort();
			}
			byte dataAr = m2.ReadUByte();
			for (int i10 = 0; i10 < dataAr; i10++)
			{
				sbyte id = m2.ReadByte();
				string name2 = m2.ReadString();
				int c = m2.ReadInt();
				int exp = m2.ReadInt();
				int vangK = m2.ReadInt();
				int bac = m2.ReadInt();
				int bacK = m2.ReadInt();
				string strI = m2.ReadString();
			}
			short task = m2.ReadShort();
			ArrTaskTemplate = new TaskTempalte[task];
			for (int i9 = 0; i9 < task; i9++)
			{
				TaskTempalte taskTempalte = new TaskTempalte();
				taskTempalte.Name = m2.ReadString();
				taskTempalte.LevelNeed = m2.ReadShort();
				taskTempalte.IdNpc = m2.ReadShort();
				taskTempalte.IdMap = m2.ReadShort();
				taskTempalte.Cx = m2.ReadShort();
				taskTempalte.Cy = m2.ReadShort();
				taskTempalte.Str1 = m2.ReadString();
				taskTempalte.Str2 = m2.ReadString();
				taskTempalte.Str3 = m2.ReadString();
				taskTempalte.Exp = m2.ReadInt();
				taskTempalte.VangKhoa = m2.ReadInt();
				taskTempalte.Bac = m2.ReadInt();
				taskTempalte.BacKhoa = m2.ReadInt();
				taskTempalte.StrItem = m2.ReadString();
				taskTempalte.TaskStep = new TaskTempalte.StepTask[m2.ReadByte()];
				for (int j2 = 0; j2 < taskTempalte.TaskStep.Length; j2++)
				{
					TaskTempalte.StepTask stepTask = new TaskTempalte.StepTask();
					stepTask.Id = m2.ReadByte();
					stepTask.Name = m2.ReadString();
					stepTask.IdItem = m2.ReadShort();
					stepTask.IdNpc = m2.ReadShort();
					stepTask.IdMob = m2.ReadShort();
					stepTask.IdMap = m2.ReadShort();
					stepTask.Cx = m2.ReadShort();
					stepTask.Cy = m2.ReadShort();
					stepTask.MaxRequire = m2.ReadShort();
					stepTask.Str1 = m2.ReadString();
					stepTask.StrItem = m2.ReadString();
					taskTempalte.TaskStep[j2] = stepTask;
				}
				ArrTaskTemplate[i9] = taskTempalte;
			}
			
			Util.ShowInfo(string.Format("Load task template success ({0})", task));
			
			byte taskNgay = m2.ReadUByte();
			for (int i8 = 0; i8 < taskNgay; i8++)
			{
				m2.ReadByte();
				m2.ReadString();
				m2.ReadShort();
			}
			short MapTemplate = m2.ReadShort();
			ArrMapTemplate = new MapTemplate[MapTemplate];
			for (int i7 = 0; i7 < MapTemplate; i7++)
			{
				MapTemplate mapTemplate = new MapTemplate();
				mapTemplate.Name = m2.ReadString();
				mapTemplate.TypeBlockMap = m2.ReadUByte();
				mapTemplate.Type = m2.ReadByte();
				try
                {
                    string JsonNpc = null;
                    string JsonMob = null;
					string pathNpc = string.Format(@"{0}\Npc\{1}.json", Program.AppSettings.DataPath, i7);
					string pathMob = string.Format(@"{0}\Mob\{1}.json", Program.AppSettings.DataPath, i7);
                    string jArr = string.Format(@"{0}\ArrMap\{1}.bin", Program.AppSettings.DataPath, i7);
                    string pathArrayMap = string.Format(@"{0}\ArrMap\arr_map_{1}.bin", Program.AppSettings.DataPath, i7);
                    if (File.Exists(pathNpc))
					{
						JsonNpc = File.ReadAllText(pathNpc);
					}
					if (File.Exists(pathMob))
					{
						JsonMob = File.ReadAllText(pathMob);
					}
					if (JsonNpc != null)
					{
						mapTemplate.ArrNpc = JsonConvert.DeserializeObject<NpcTemplate[]>(JsonConvert.DeserializeObject<JArray>(JsonNpc).ToString());
					}
					if (JsonMob != null)
					{
						mapTemplate.ArrMob = JsonConvert.DeserializeObject<MobTemplate[]>(JsonConvert.DeserializeObject<JArray>(JsonMob).ToString());
					}
					mapTemplate.NotBlock = true;
					if (File.Exists(pathArrayMap))
					{
						mapTemplate.NotBlock = false;
						mapTemplate.ArrMap = Array.ConvertAll(File.ReadAllBytes(pathArrayMap), (byte s) => (sbyte)s);
					}
				}
				catch (Exception e)
				{
					Util.ShowErr(e);
				}
				ArrMapTemplate[i7] = mapTemplate;
			}
			
			Util.ShowInfo(string.Format("Load map template success ({0})", MapTemplate));
            
			short ItemOptionTemplate = m2.ReadShort();
			for (int i6 = 0; i6 < ItemOptionTemplate; i6++)
			{
				string Name4 = m2.ReadString();
				sbyte Type4 = m2.ReadByte();
				sbyte Level = m2.ReadByte();
				string Options = m2.ReadString();
			}
			
			Util.ShowInfo(string.Format("Load item option template success ({0})", ItemOptionTemplate));
			
			sbyte EffTemplate = m2.ReadByte();
			for (int i5 = 0; i5 < EffTemplate; i5++)
			{
				string Name3 = m2.ReadString();
				string Detail3 = m2.ReadString();
				byte Type3 = m2.ReadUByte();
				short IdIcon2 = m2.ReadShort();
				short IdMob2 = m2.ReadShort();
			}
			
			Util.ShowInfo(string.Format("Load item effect mob template success ({0})", EffTemplate));
			
			short ItemTemplate = m2.ReadShort();
			for (int i4 = 0; i4 < ItemTemplate; i4++)
			{
				string Name2 = m2.ReadString();
				string Detail2 = m2.ReadString();
				bool IsCongDon = m2.ReadBool();
				sbyte GioiTinh = m2.ReadByte();
				sbyte Type2 = m2.ReadByte();
				sbyte IdClass = m2.ReadByte();
				short IdIcon = m2.ReadShort();
				byte LevelNeed = m2.ReadUByte();
				ushort taiPhuNeed = m2.ReadUShort();
				short IdMob = m2.ReadShort();
				short IdChar = m2.ReadShort();
			}
			
			Util.ShowInfo(string.Format("Load item template success ({0})", ItemTemplate));
			
			short MobTemplate = m2.ReadShort();
			ArrMobTempalte = new MobTemplate[MobTemplate];
			for (int i3 = 0; i3 < MobTemplate; i3++)
			{
				MobTemplate mobTemplate = new MobTemplate();
				mobTemplate.g = m2.ReadShort();
				mobTemplate.Name = m2.ReadString();
				mobTemplate.Detail = m2.ReadString();
				mobTemplate.SpeedMove = m2.ReadUByte();
				mobTemplate.Type = m2.ReadByte();
				mobTemplate.SpeedMoveByte = m2.ReadByte();
				mobTemplate.b = m2.ReadByte();
				mobTemplate.IndexData = m2.ReadShort();
				mobTemplate.TimeThuHoac = m2.ReadShort();
				mobTemplate.Utf = m2.ReadString();
				mobTemplate.Utf2 = m2.ReadString();
				ArrMobTempalte[i3] = mobTemplate;
			}
			
			Util.ShowInfo(string.Format("Load mob template success ({0})", MobTemplate));
			
			short NpcTemplate = m2.ReadShort();
			for (int i2 = 0; i2 < NpcTemplate; i2++)
			{
				string Name = m2.ReadString();
				string Detail = m2.ReadString();
				short IndexData = m2.ReadShort();
				int Hp = m2.ReadInt();
				int Mp = m2.ReadInt();
				short g = m2.ReadShort();
			}
			
			Util.ShowInfo(string.Format("Load npc template success ({0})", NpcTemplate));
			
			short SkillTemplate = m2.ReadShort();
			ArrSkillTemplate = new SkillTemplate[SkillTemplate];
			for (int m = 0; m < SkillTemplate; m++)
			{
				SkillTemplate skillTemplate = new SkillTemplate();
				skillTemplate.Name = m2.ReadString();
				skillTemplate.Detail = m2.ReadString();
				skillTemplate.LevelNeed = m2.ReadShort();
				skillTemplate.IdChar = m2.ReadByte();
				skillTemplate.LevelMax = m2.ReadByte();
				skillTemplate.Type = m2.ReadByte();
				skillTemplate.IdIcon = m2.ReadShort();
				ArrSkillTemplate[m] = skillTemplate;
			}
			
			Util.ShowInfo(string.Format("Load skill template success ({0})", SkillTemplate));
			
			short Skill = m2.ReadShort();
			ArrSkill = new Skill[Skill];
			for (short l = 0; l < Skill; l++)
			{
				Skill skill = new Skill();
				skill.Id = m2.ReadShort();
				skill.IdTemplate = m2.ReadShort();
				skill.Level = m2.ReadByte();
				skill.LevelNeed = m2.ReadUByte();
				skill.MpUse = m2.ReadShort();
				skill.CoolDown = m2.ReadInt();
				skill.RangeNgang = m2.ReadShort();
				skill.RangeDoc = m2.ReadShort();
				skill.MaxTarget = m2.ReadByte();
				skill.Options = m2.ReadString();
				skill.Index = l;
				ArrSkill[l] = skill;
			}
			
			Util.ShowInfo(string.Format("Load skill success ({0})", Skill));
			
			byte SkillClan = m2.ReadUByte();
			ArrSkillClan = new SkillClanTemplate[SkillClan];
			for (int j = 0; j < SkillClan; j++)
			{
				SkillClanTemplate skillClanTemplate = new SkillClanTemplate();
				skillClanTemplate.Name = m2.ReadString();
				skillClanTemplate.Detail = m2.ReadString();
				skillClanTemplate.LevelNeed = m2.ReadUByte();
				skillClanTemplate.Options = m2.ReadString();
				skillClanTemplate.IdIcon = m2.ReadShort();
				skillClanTemplate.MoneyBuy = m2.ReadInt();
				ArrSkillClan[j] = skillClanTemplate;
			}
			
			Util.ShowInfo(string.Format("Load skill clan template success ({0})", SkillClan));
			
			sbyte DataTypeBody = m2.ReadByte();
			for (int i = 0; i < DataTypeBody; i++)
			{
				sbyte Type = m2.ReadByte();
			}
			
			Util.ShowInfo(string.Format("Load data body type success ({0})", DataTypeBody));
			
			_ReadMove(m2);
		}

		private static void _ReadMove(Message m)
		{
			short HashTable1 = m.ReadShort();
			for (int i = 0; i < HashTable1; i++)
			{
				short s3 = m.ReadShort();
				for (int j2 = 0; j2 < s3; j2++)
				{
					short s5 = m.ReadShort();
					byte b1 = m.ReadUByte();
					byte b3 = m.ReadUByte();
					m.ReadShort();
					m.ReadShort();
				}
			}
			short HashTable2 = m.ReadShort();
			for (int i22 = 0; i22 < HashTable2; i22++)
			{
				m.ReadShort();
				m.ReadShort();
				m.ReadShort();
			}
			short HashTable3 = m.ReadShort();
			for (int i21 = 0; i21 < HashTable3; i21++)
			{
				short s4 = m.ReadShort();
				for (int j9 = 0; j9 < s4; j9++)
				{
					short s6 = m.ReadShort();
					byte b2 = m.ReadUByte();
					byte b4 = m.ReadUByte();
					m.ReadShort();
					m.ReadShort();
				}
			}
			short HashTable4 = m.ReadShort();
			for (int i20 = 0; i20 < HashTable4; i20++)
			{
				m.ReadShort();
				m.ReadShort();
				m.ReadShort();
			}
			byte e = m.ReadUByte();
			for (int i19 = 0; i19 < e; i19++)
			{
				m.ReadShort();
				m.ReadShort();
				m.ReadShort();
				m.ReadByte();
			}
			Read1(m);
			byte f = m.ReadUByte();
			short f2 = m.ReadShort();
			for (int i18 = 0; i18 < f2; i18++)
			{
				m.ReadByte();
				for (int j8 = 0; j8 < f; j8++)
				{
					if (m.ReadShort() != 0)
					{
						m.ReadByte();
						m.ReadByte();
						m.ReadByte();
						m.ReadByte();
					}
				}
			}
			short f3 = m.ReadShort();
			for (int i17 = 0; i17 < f3; i17++)
			{
				sbyte bl = m.ReadByte();
				for (int j7 = 0; j7 < bl; j7++)
				{
					m.ReadShort();
				}
			}
			short f4 = m.ReadShort();
			for (int i16 = 0; i16 < f4; i16++)
			{
				m.ReadString();
				m.ReadString();
				m.ReadString();
			}
			Read2(m);
			Read3(m);
			Read4(m);
			Read5(m);
			Read6(m);
			short s2 = m.ReadShort();
			for (int i15 = 0; i15 < s2; i15++)
			{
				m.ReadShort();
				m.ReadShort();
				sbyte k2 = m.ReadByte();
				for (int j6 = 0; j6 < k2; j6++)
				{
					sbyte l4 = m.ReadByte();
					for (int l3 = 0; l3 < l4; l3++)
					{
						m.ReadByte();
					}
				}
				sbyte k3 = m.ReadByte();
				for (int j5 = 0; j5 < k3; j5++)
				{
					sbyte k4 = m.ReadByte();
					for (int l2 = 0; l2 < k4; l2++)
					{
						m.ReadShort();
						sbyte kx = m.ReadByte();
						if (kx >= 30)
						{
							m.ReadShort();
							m.ReadShort();
						}
						else if (kx >= 20)
						{
							m.ReadShort();
						}
						else if (kx >= 10)
						{
							m.ReadShort();
						}
					}
				}
			}
			sbyte SizeServer = m.ReadByte();
			for (int i14 = 0; i14 < SizeServer; i14++)
			{
				m.ReadString();
				for (int j4 = 0; j4 < m.ReadByte(); j4++)
				{
					short Id = m.ReadShort();
					string Name = m.ReadString();
					string Ip = m.ReadString();
					short Port = m.ReadShort();
					short PortCheck = m.ReadShort();
				}
			}
			int au = m.ReadInt();
			for (int i13 = 0; i13 < au; i13++)
			{
				m.ReadShort();
			}
			int BacKhoaGhepDa2 = m.ReadInt();
			BacGhepDa = new int[BacKhoaGhepDa2];
			for (int i12 = 0; i12 < BacKhoaGhepDa2; i12++)
			{
				BacGhepDa[i12] = m.ReadInt();
			}
			int BacKhoaNangCapVuKhi = m.ReadInt();
			BacNangCapVuKhi = new int[BacKhoaNangCapVuKhi];
			for (int i11 = 0; i11 < BacKhoaNangCapVuKhi; i11++)
			{
				BacNangCapVuKhi[i11] = m.ReadInt();
			}
			int BacKhoaNangCapTrangBi = m.ReadInt();
			BacNangCapTrangBi = new int[BacKhoaNangCapTrangBi];
			for (int i10 = 0; i10 < BacKhoaNangCapTrangBi; i10++)
			{
				BacNangCapTrangBi[i10] = m.ReadInt();
			}
			int BacKhoaNangCapPhuKien = m.ReadInt();
			BacNangCapPhuKien = new int[BacKhoaNangCapPhuKien];
			for (int i9 = 0; i9 < BacKhoaNangCapPhuKien; i9++)
			{
				BacNangCapPhuKien[i9] = m.ReadInt();
			}
			int DiemNangCapGhepDa = m.ReadInt();
			DiemGhepDa = new long[DiemNangCapGhepDa];
			for (int i8 = 0; i8 < DiemNangCapGhepDa; i8++)
			{
				DiemGhepDa[i8] = m.ReadLong();
			}
			int DiemNangCapVuKhi2 = m.ReadInt();
			DiemNangCapVuKhi = new long[DiemNangCapVuKhi2];
			for (int i7 = 0; i7 < DiemNangCapVuKhi2; i7++)
			{
				DiemNangCapVuKhi[i7] = m.ReadLong();
			}
			int DiemNangCapTrangBi2 = m.ReadInt();
			DiemNangCapTrangBi = new long[DiemNangCapTrangBi2];
			for (int i6 = 0; i6 < DiemNangCapTrangBi2; i6++)
			{
				DiemNangCapTrangBi[i6] = m.ReadLong();
			}
			int DiemNangCapPhuKien2 = m.ReadInt();
			DiemNangCapPhuKien = new long[DiemNangCapPhuKien2];
			for (int i5 = 0; i5 < DiemNangCapPhuKien2; i5++)
			{
				DiemNangCapPhuKien[i5] = m.ReadLong();
			}
			int SoluongDaKham = m.ReadInt();
			NgocKhamPoint = new int[SoluongDaKham];
			for (int i4 = 0; i4 < SoluongDaKham; i4++)
			{
				NgocKhamPoint[i4] = m.ReadInt();
			}
			int dataTreoCho = m.ReadInt();
			for (int i3 = 0; i3 < dataTreoCho; i3++)
			{
				m.ReadString();
			}
			int dataTreoCho2 = m.ReadInt();
			for (int i2 = 0; i2 < dataTreoCho2; i2++)
			{
				m.ReadString();
			}
			int Exps = m.ReadInt();
			ArrExp = new long[Exps];
			for (int n = 0; n < Exps; n++)
			{
				ArrExp[n] = m.ReadLong();
			}
			sbyte GiftQuaySo = m.ReadByte();
			for (int l = 0; l < GiftQuaySo; l++)
			{
				int x = m.ReadInt();
				for (int j3 = 0; j3 < x; j3++)
				{
					m.ReadInt();
				}
			}
			sbyte sizeSkillClan = m.ReadByte();
			ArrSkillViThu = new SkillClanTemplate[sizeSkillClan];
			for (int k = 0; k < sizeSkillClan; k++)
			{
				SkillClanTemplate skillClanTemplate = new SkillClanTemplate();
				skillClanTemplate.Id = m.ReadByte();
				skillClanTemplate.Name = m.ReadString();
				skillClanTemplate.Detail = m.ReadString();
				skillClanTemplate.LevelNeed = m.ReadUByte();
				skillClanTemplate.Options = m.ReadString();
				skillClanTemplate.IdIcon = m.ReadShort();
				ArrSkillViThu[k] = skillClanTemplate;
			}
			sbyte TypeArr = m.ReadByte();
			_ReadDataGame2();
			CreateMap();
			Data_Game2Change = DataGameChange2();
			ItemTemplate[] ItemCT = ArrItemTemplate.Where((ItemTemplate s) => s.Type == 14).ToArray().ToArray();
			ItemCaiTrang = new Item[ItemCT.Length];
			for (int j = 0; j < ItemCT.Length; j++)
			{
				Item item = new Item(ItemCT[j].Id, IsLock: true);
				item.IdClass = -1;
				short LevelNeed = ItemCT[j].LevelNeed;
				short ValueOp = 0;
				item.Options = "209," + ((LevelNeed >= 10 && LevelNeed <= 20) ? ((short)Util.NextInt(5, 10)) : ((LevelNeed > 20 && LevelNeed <= 35) ? ((short)Util.NextInt(15, 25)) : ((LevelNeed <= 36 || LevelNeed > 45) ? ((short)Util.NextInt(36, 50)) : ((short)Util.NextInt(26, 35)))));
				ItemCaiTrang[j] = item;
			}
		}

		private static void _ReadDataGame2()
		{
			Message l = new Message(0, Array.ConvertAll(Data_Game2, (sbyte s) => (byte)s));
			short ido = l.ReadShort();
			ArrItemOptionTemplate = new ItemOptionTemplate[ido];
			for (int j = 0; j < ido; j++)
			{
				ItemOptionTemplate itemOptionTemplate = new ItemOptionTemplate();
				itemOptionTemplate.Id = (short)j;
				itemOptionTemplate.Name = l.ReadString();
				itemOptionTemplate.Type = l.ReadByte();
				itemOptionTemplate.Level = l.ReadByte();
				itemOptionTemplate.Options = l.ReadString();
				ArrItemOptionTemplate[j] = itemOptionTemplate;
			}
			sbyte effTemp = l.ReadByte();
			ArrEffTemplate = new EffTemplate[effTemp];
			for (int k = 0; k < effTemp; k++)
			{
				EffTemplate effTemplate = new EffTemplate();
				effTemplate.Name = l.ReadString();
				effTemplate.Detail = l.ReadString();
				effTemplate.Type = l.ReadUByte();
				effTemplate.IdIcon = l.ReadShort();
				effTemplate.IdMob = l.ReadShort();
				ArrEffTemplate[k] = effTemplate;
			}
			Util.ShowInfo(string.Format("Load effect template success ({0})", effTemp));
			short itemtemp = (short)(l.ReadShort() + 1);
			ArrItemTemplate = new ItemTemplate[itemtemp];
			for (short i = 0; i < itemtemp; i++)
			{
				if (i == ArrItemTemplate.Length - 1)
				{
					ItemTemplate itemTemplate2 = new ItemTemplate();
					itemTemplate2.Id = i;
					itemTemplate2.Name = "danh hiệu Black Pink";
					itemTemplate2.Detail = "16728232";
					itemTemplate2.IsCongDon = false;
					itemTemplate2.GioiTinh = 2;
					itemTemplate2.Type = 34;
					itemTemplate2.IdClass = -1;
					itemTemplate2.IdIcon = 806;
					itemTemplate2.LevelNeed = 0;
					itemTemplate2.TaiPhuNeed = 0;
					itemTemplate2.IdMob = 0;
					itemTemplate2.IdChar = 0;
					ArrItemTemplate[i] = itemTemplate2;
				}
				else
				{
					ItemTemplate itemTemplate = new ItemTemplate();
					itemTemplate.Id = i;
					itemTemplate.Name = l.ReadString();
					itemTemplate.Detail = l.ReadString();
					itemTemplate.IsCongDon = l.ReadBool();
					itemTemplate.GioiTinh = l.ReadByte();
					itemTemplate.Type = l.ReadByte();
					itemTemplate.IdClass = l.ReadByte();
					itemTemplate.IdIcon = l.ReadShort();
					itemTemplate.LevelNeed = l.ReadUByte();
					itemTemplate.TaiPhuNeed = l.ReadUShort();
					itemTemplate.IdMob = l.ReadShort();
					itemTemplate.IdChar = l.ReadShort();
					ArrItemTemplate[i] = itemTemplate;
				}
			}
			Util.ShowInfo(string.Format("Load Item Template success ({0})", itemtemp));
			Read1(l);
			Read2(l);
			Read3(l);
			Read4(l);
			Read5(l);
			Read6(l);
		}

		public static void DataGame()
		{
			Console.OutputEncoding = Encoding.UTF8;
			_ReadDataGame1();
		}

		private static void Read1(Message m)
		{
			sbyte e1 = m.ReadByte();
			for (int i = 0; i < e1; i++)
			{
				sbyte e2 = m.ReadByte();
				for (int j = 0; j < e2; j++)
				{
					m.ReadByte();
				}
			}
		}

		private static void Read2(Message m)
		{
			short f5 = m.ReadShort();
			for (int i = 0; i < f5; i++)
			{
				m.ReadByte();
				m.ReadByte();
				m.ReadByte();
				m.ReadByte();
				m.ReadBool();
				m.ReadBool();
				sbyte l = m.ReadByte();
				for (int k = 0; k < l; k++)
				{
					m.ReadShort();
				}
				sbyte k2 = m.ReadByte();
				for (int j = 0; j < k2; j++)
				{
					sbyte k3 = m.ReadByte();
					for (int n = 0; n < k3; n++)
					{
						m.ReadShort();
					}
				}
			}
		}

		private static void Read3(Message m)
		{
			short Size = m.ReadShort();
			
			for (int x = 0; x < Size; x++)
			{
				m.ReadByte();
				byte l1 = m.ReadUByte();
				for (int x2 = 0; x2 < l1; x2++)
				{
					m.ReadShort();
					short l2 = m.ReadByte();
					if (l2 >= 30)
					{
						l2 -= 30;
						m.ReadShort();
						m.ReadShort();
					}
					else if (l2 >= 20)
					{
						l2 -= 20;
						m.ReadShort();
					}
					else if (l2 >= 10)
					{
						l2 -= 10;
						m.ReadShort();
					}
				}
			}
		}

		private static void Read4(Message m)
		{
			short f7 = m.ReadShort();

			for (int i = 0; i < f7; i++)
			{
				m.ReadByte();
				byte k = m.ReadUByte();
				for (int j = 0; j < k; j++)
				{
					m.ReadShort();
					m.ReadByte();
					sbyte l = m.ReadByte();
					if (l >= 30)
					{
						m.ReadByte();
						m.ReadByte();
					}
					else if (l >= 20)
					{
						m.ReadByte();
					}
					else if (l >= 10)
					{
						m.ReadByte();
					}
				}
			}
		}

		private static void Read5(Message m)
		{
			short f8 = m.ReadShort();
			for (int i = 0; i < f8; i++)
			{
				m.ReadShort();
				m.ReadShort();
				m.ReadByte();
				byte x = m.ReadUByte();
				for (int j = 0; j < x; j++)
				{
					byte x2 = m.ReadUByte();
					for (int k = 0; k < x2; k++)
					{
						m.ReadByte();
						sbyte l2 = m.ReadByte();
						if (l2 >= 30)
						{
							m.ReadShort();
							m.ReadByte();
							m.ReadByte();
							m.ReadShort();
							m.ReadByte();
							m.ReadByte();
						}
						else if (l2 >= 20)
						{
							m.ReadShort();
							m.ReadByte();
							m.ReadByte();
						}
						else if (l2 >= 10)
						{
							m.ReadShort();
							m.ReadByte();
							m.ReadByte();
						}
					}
				}
			}
		}

		private static void Read6(Message m)
		{
			short f9 = m.ReadShort();
			DoubleWaypoint = new short[f9][];
			for (int index = 0; index < DoubleWaypoint.Length; index++)
			{
				DoubleWaypoint[index] = new short[14];
				DoubleWaypoint[index][0] = m.ReadShort();
				DoubleWaypoint[index][1] = m.ReadShort();
				DoubleWaypoint[index][2] = m.ReadShort();
				DoubleWaypoint[index][3] = m.ReadShort();
				DoubleWaypoint[index][4] = m.ReadShort();
				DoubleWaypoint[index][10] = m.ReadShort();
				DoubleWaypoint[index][11] = m.ReadShort();
				DoubleWaypoint[index][5] = m.ReadShort();
				DoubleWaypoint[index][6] = m.ReadShort();
				DoubleWaypoint[index][7] = m.ReadShort();
				DoubleWaypoint[index][8] = m.ReadShort();
				DoubleWaypoint[index][9] = m.ReadShort();
				DoubleWaypoint[index][12] = m.ReadShort();
				DoubleWaypoint[index][13] = m.ReadShort();
			}
		}

		private static sbyte[] _ReadArrDataGame1()
		{
			byte[] DataGame = File.ReadAllBytes(string.Format(@"{0}\arr_data_game.bin", Program.AppSettings.DataPath));
			return Array.ConvertAll(DataGame, (byte s) => (sbyte)s);
		}

		private static sbyte[] _ReadArrDataGame2()
		{
			byte[] DataGame = File.ReadAllBytes(string.Format(@"{0}\arr_data_game2.bin", Program.AppSettings.DataPath));
			return Array.ConvertAll(DataGame, (byte s) => (sbyte)s);
		}

		public static sbyte[] DataGameChange2()
		{
			sbyte[] Data2 = _ReadArrDataGame2();
			Writer writer = new Writer(Data2.Length);
			Message m2 = new Message(0, Array.ConvertAll(Data2, (sbyte s) => (byte)s));
			short Size2 = m2.ReadShort();
			writer.writeShort(Size2);
			for (int i2 = 0; i2 < Size2; i2++)
			{
				writer.writeUTF(m2.ReadString());
				writer.writeByte(m2.ReadByte());
				writer.writeByte(m2.ReadByte());
				writer.writeUTF(m2.ReadString());
			}
			sbyte effTemp = m2.ReadByte();
			writer.writeByte(effTemp);
			for (int n = 0; n < effTemp; n++)
			{
				writer.writeUTF(m2.ReadString());
				writer.writeUTF(m2.ReadString());
				writer.writeByte(m2.ReadUByte());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
			}
			short itemtemp = (short)(m2.ReadShort() + 1);
			writer.writeShort(itemtemp);
			for (short m = 0; m < itemtemp; m++)
			{
				if (m == itemtemp - 1)
				{
					writer.writeUTF("danh hiệu Black Pink");
					writer.writeUTF("-38476");
					writer.writeBool(value: false);
					writer.writeByte(2);
					writer.writeByte(34);
					writer.writeByte(-1);
					writer.writeShort(1143);
					writer.writeByte(0);
					writer.writeShort(0);
					writer.writeShort(0);
					writer.writeShort(0);
					break;
				}
				writer.writeUTF(m2.ReadString());
				writer.writeUTF(m2.ReadString());
				writer.writeBool(m2.ReadBool());
				writer.writeByte(m2.ReadByte());
				writer.writeByte(m2.ReadByte());
				writer.writeByte(m2.ReadByte());
				writer.writeShort(m2.ReadShort());
				writer.writeByte(m2.ReadUByte());
				writer.writeShort(m2.ReadUShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
			}
			sbyte e1 = m2.ReadByte();
			writer.writeByte(e1);
			for (int l = 0; l < e1; l++)
			{
				sbyte e2 = m2.ReadByte();
				writer.writeByte(e2);
				for (int j6 = 0; j6 < e2; j6++)
				{
					writer.writeByte(m2.ReadByte());
				}
			}
			short f5 = m2.ReadShort();
			writer.writeShort(f5);
			for (int k = 0; k < f5; k++)
			{
				writer.writeByte(m2.ReadByte());
				writer.writeByte(m2.ReadByte());
				writer.writeByte(m2.ReadByte());
				writer.writeByte(m2.ReadByte());
				writer.writeBool(m2.ReadBool());
				writer.writeBool(m2.ReadBool());
				sbyte k3 = m2.ReadByte();
				writer.writeByte(k3);
				for (int j5 = 0; j5 < k3; j5++)
				{
					writer.writeShort(m2.ReadShort());
				}
				sbyte k4 = m2.ReadByte();
				writer.writeByte(k4);
				for (int j4 = 0; j4 < k4; j4++)
				{
					sbyte k5 = m2.ReadByte();
					writer.writeByte(k5);
					for (int l4 = 0; l4 < k5; l4++)
					{
						writer.writeShort(m2.ReadShort());
					}
				}
			}
			short Size = m2.ReadShort();
			writer.writeShort(Size);
			for (int x2 = 0; x2 < Size; x2++)
			{
				writer.writeByte(m2.ReadByte());
				byte l5 = m2.ReadUByte();
				writer.writeByte(l5);
				for (int x4 = 0; x4 < l5; x4++)
				{
					writer.writeShort(m2.ReadShort());
					short l7 = m2.ReadByte();
					writer.writeByte(l7);
					if (l7 >= 30)
					{
						l7 -= 30;
						writer.writeShort(m2.ReadShort());
						writer.writeShort(m2.ReadShort());
					}
					else if (l7 >= 20)
					{
						l7 -= 20;
						writer.writeShort(m2.ReadShort());
					}
					else if (l7 >= 10)
					{
						l7 -= 10;
						writer.writeShort(m2.ReadShort());
					}
				}
			}
			short f6 = m2.ReadShort();
			writer.writeShort(f6);
			for (int j = 0; j < f6; j++)
			{
				writer.writeByte(m2.ReadByte());
				byte k2 = m2.ReadUByte();
				writer.writeByte(k2);
				for (int j3 = 0; j3 < k2; j3++)
				{
					writer.writeShort(m2.ReadShort());
					writer.writeByte(m2.ReadByte());
					sbyte l3 = m2.ReadByte();
					writer.writeByte(l3);
					if (l3 >= 30)
					{
						writer.writeByte(m2.ReadByte());
						writer.writeByte(m2.ReadByte());
					}
					else if (l3 >= 20)
					{
						writer.writeByte(m2.ReadByte());
					}
					else if (l3 >= 10)
					{
						writer.writeByte(m2.ReadByte());
					}
				}
			}
			short f7 = m2.ReadShort();
			writer.writeShort(f7);
			for (int i = 0; i < f7; i++)
			{
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeByte(m2.ReadByte());
				byte x = m2.ReadUByte();
				writer.writeByte(x);
				for (int j2 = 0; j2 < x; j2++)
				{
					byte x3 = m2.ReadUByte();
					writer.writeByte(x3);
					for (int l2 = 0; l2 < x3; l2++)
					{
						writer.writeByte(m2.ReadByte());
						sbyte l6 = m2.ReadByte();
						writer.writeByte(l6);
						if (l6 >= 30)
						{
							writer.writeShort(m2.ReadShort());
							writer.writeByte(m2.ReadByte());
							writer.writeByte(m2.ReadByte());
							writer.writeShort(m2.ReadShort());
							writer.writeByte(m2.ReadByte());
							writer.writeByte(m2.ReadByte());
						}
						else if (l6 >= 20)
						{
							writer.writeShort(m2.ReadShort());
							writer.writeByte(m2.ReadByte());
							writer.writeByte(m2.ReadByte());
						}
						else if (l6 >= 10)
						{
							writer.writeShort(m2.ReadShort());
							writer.writeByte(m2.ReadByte());
							writer.writeByte(m2.ReadByte());
						}
					}
				}
			}
			short f8 = m2.ReadShort();
			writer.writeShort(f8);
			for (int index = 0; index < DoubleWaypoint.Length; index++)
			{
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
				writer.writeShort(m2.ReadShort());
			}
			return writer.getData();
		}

		public static void CreateMap()
		{
			short Size = (short)ArrMapTemplate.Length;
			MapManager.Maps = new Map[Size];
			for (short i = 0; i < Size; i++)
			{
				Map map = new Map(i);
				map.Zones = new Zone[9];
				if (!ArrMapTemplate[i].NotBlock)
				{
					for (sbyte j = 0; j < map.Zones.Length; j++)
					{
						Zone zone = new Zone(j);
						if (ArrMapTemplate[i].ArrMob != null)
						{
							for (int k = 0; k < ArrMapTemplate[i].ArrMob.Length; k++)
							{
								Mob mob = new Mob(zone);
								mob.Id = ArrMapTemplate[i].ArrMob[k].id;
								mob.Cx = ArrMapTemplate[i].ArrMob[k].cx;
								mob.Cy = ArrMapTemplate[i].ArrMob[k].cy;
								mob.Exp = ArrMapTemplate[i].ArrMob[k].exp;
								mob.Hp = ArrMapTemplate[i].ArrMob[k].hp;
								mob.Speed = ArrMobTempalte[mob.Id].SpeedMove;
								mob.Type = ArrMapTemplate[i].ArrMob[k].Type;
								mob.TimeThuHoach = ArrMapTemplate[i].ArrMob[k].TimeThuHoac;
								mob.HpFull = mob.Hp;
								mob.IsPainMiniMap = ArrMapTemplate[i].ArrMob[k].paintMiniMap;
								mob.IdEntity = (short)k;
								mob.IdClass = (sbyte)Util.NextInt(1, 5);
								mob.Level = ArrMapTemplate[i].ArrMob[k].level;
								mob.LevelBoss = ArrMapTemplate[i].ArrMob[k].levelBoss;
								MobHander.SetPointRespawMob(mob, IsNew: true);
								zone.Mobs.TryAdd(mob.IdEntity, mob);
							}
						}
						map.Zones[j] = zone;
					}
					for (int x = 0; x < DoubleWaypoint.Length; x++)
					{
						WayPoint wayPoint = null;
						if (DoubleWaypoint[x][0] == i)
						{
							wayPoint = new WayPoint();
							wayPoint.Create(DoubleWaypoint[x][0], DoubleWaypoint[x][5], DoubleWaypoint[x][1], DoubleWaypoint[x][2], DoubleWaypoint[x][3], DoubleWaypoint[x][4], DoubleWaypoint[x][10], DoubleWaypoint[x][11]);
							wayPoint.IsNext = true;
							map.WayPoints.Add(wayPoint);
						}
						else if (DoubleWaypoint[x][5] == map.Id)
						{
							wayPoint = new WayPoint();
							wayPoint.IsNext = false;
							wayPoint.Create(DoubleWaypoint[x][5], DoubleWaypoint[x][0], DoubleWaypoint[x][6], DoubleWaypoint[x][7], DoubleWaypoint[x][8], DoubleWaypoint[x][9], DoubleWaypoint[x][12], DoubleWaypoint[x][13]);
							map.WayPoints.Add(wayPoint);
						}
					}
					map.Update();
				}
				MapManager.Maps[i] = map;
			}
		}
	}
}
