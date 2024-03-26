using System;
using System.Collections.Generic;
using System.Linq;
using LangLa.Data;
using LangLa.EventServer;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.SupportOOP;
using Newtonsoft.Json;

namespace LangLa.Template
{
	[JsonObject(MemberSerialization.OptIn)]
	public class GiaTocTemplate
	{
		[JsonObject(MemberSerialization.OptIn)]
		public class ThanhVienGiaToc
		{
			[JsonProperty]
			public sbyte ChucVu;

			[JsonProperty]
			public sbyte ClassID;

			[JsonProperty]
			public int IdUser;

			[JsonProperty]
			public sbyte IdChar;

			[JsonProperty]
			public string? Name;

			[JsonProperty]
			public bool IsLockChat;

			[JsonProperty]
			public int CongHien;

			[JsonProperty]
			public int CongHienTuan;

			[JsonProperty]
			public bool isOn;

			[JsonProperty]
			public sbyte Level;

			public Character? _myChar;
		}

		[JsonProperty]
		public short ID;

		[JsonProperty]
		public string? Name;

		[JsonProperty]
		public string? NameTocTruong;

		[JsonProperty]
		public string Content = "";

		[JsonProperty]
		public string Title = "";

		[JsonProperty]
		public byte Level;

		[JsonProperty]
		public int Exp;

		[JsonProperty]
		public int MaxExp;

		[JsonProperty]
		public byte SizeThanhVien;

		[JsonProperty]
		public int NganSach;

		[JsonProperty]
		public long NgayThanhLap;

		[JsonProperty]
		public int CongHienTuan;

		[JsonProperty]
		public byte LuotThuNapTrongNgay;

		[JsonProperty]
		public long TimeReSetThuNap;

		[JsonProperty]
		public sbyte SoLanMoCuaAi = 1;

		[JsonProperty]
		public string TimeMoCuaAi = DateTime.Now.ToString();

		[JsonProperty]
		public short SoLanDiAiTrongNgay;

		public List<Item> ItemGiaToc = new List<Item>();

		public List<string> LogGiaTocs = new List<string>();

		public List<ThanhVienGiaToc> ThanhViens = new List<ThanhVienGiaToc>();

		public sbyte[]? SkillGiaToc;

		public bool IsMoCuaAi;

		public AiGiaToc AiGiaToc;

		public void AddMember(Character _cAdd)
		{
			_cAdd.Info.IdGiaToc = ID;
			_cAdd.Info.RoleGiaToc = 2;
			_cAdd.InfoGame.GiaToc = this;
			SizeThanhVien++;
			ThanhVienGiaToc thanhVienGiaToc = new ThanhVienGiaToc();
			thanhVienGiaToc.ChucVu = 2;
			thanhVienGiaToc.ClassID = _cAdd.Info.IdClass;
			thanhVienGiaToc.IdUser = _cAdd.Info.IdUser;
			thanhVienGiaToc.IdChar = _cAdd.Info.IdChar;
			thanhVienGiaToc.Name = _cAdd.Info.Name;
			thanhVienGiaToc.isOn = true;
			thanhVienGiaToc.Level = _cAdd.Info.Level;
			thanhVienGiaToc._myChar = _cAdd;
			ThanhViens.Add(thanhVienGiaToc);
			sbyte[] skillGiaToc = SkillGiaToc;
			foreach (sbyte s in skillGiaToc)
			{
				if (s != -1)
				{
					_cAdd.TuongKhac.GetPointFromSkillGiaToc(_cAdd, DataServer.ArrSkillClan[s].Options);
				}
			}
		}

		public void SetNextDayMoCuaAi()
		{
			DateTime aDateTime = DateTime.Now;
			DateTime y2K = Convert.ToDateTime(TimeMoCuaAi.ToString().Split(" ")[0]);
			int Day = (aDateTime - y2K).Days;
			if (Day > 0)
			{
				SoLanDiAiTrongNgay = 0;
				SoLanMoCuaAi = 1;
			}
		}

		public GiaTocTemplate()
		{
		}

		public void ChatGiaToc(Character _myChar, Message msg)
		{
			Character _myChar2 = _myChar;
			string Text = msg.ReadString();
			if (ThanhViens.FirstOrDefault((ThanhVienGiaToc s) => s.Name.Equals(_myChar2.Info.Name)).IsLockChat)
			{
				_myChar2.SendMessage(UtilMessage.SendThongBao("Bạn đã bị cấm chat", Util.YELLOW_MID));
				return;
			}
			string Name = _myChar2.Info.Name;
			lock (ThanhViens)
			{
				foreach (ThanhVienGiaToc c in ThanhViens)
				{
					if (c != null && c._myChar != null && c._myChar.IsConnection)
					{
						Message i = new Message(25);
						i.WriteUTF(Name);
						i.WriteUTF(Text);
						c._myChar.SendMessage(i);
					}
				}
			}
		}

		public void AddLogGiaToc(string Text)
		{
			if (LogGiaTocs.Count >= 30)
			{
				LogGiaTocs.RemoveAt(LogGiaTocs.Count - 1);
			}
			string AddNew = DateTime.Now.ToString() + " : " + Text;
			LogGiaTocs.Add(AddNew);
		}

		public GiaTocTemplate(short Id, string Name, Character _myChar)
		{
			ID = Id;
			this.Name = Name;
			Level = 1;
			NameTocTruong = _myChar.Info.Name;
			SizeThanhVien = 1;
			NgayThanhLap = Util.CurrentTimeMillis();
			SkillGiaToc = new sbyte[DataServer.ArrSkillClan.Length];
			for (int i = 0; i < SkillGiaToc.Length; i++)
			{
				SkillGiaToc[i] = -1;
			}
			ThanhVienGiaToc thanhVienGiaToc = new ThanhVienGiaToc
			{
				ChucVu = 5,
				ClassID = _myChar.Info.IdClass,
				IdUser = _myChar.Info.IdUser,
				IdChar = _myChar.Info.IdChar,
				Name = _myChar.Info.Name,
				isOn = true,
				Level = _myChar.Info.Level,
				_myChar = _myChar
			};
			ThanhViens.Add(thanhVienGiaToc);
		}

		public void UpdateExp(int Exp)
		{
			this.Exp += Exp;
			byte LevelUp = Level;
			if (Level >= 14)
			{
				return;
			}
			while (this.Exp > GiaTocManager.ExpGiaTocServer[LevelUp])
			{
				LevelUp++;
				if (LevelUp == 14)
				{
					break;
				}
				this.Exp -= GiaTocManager.ExpGiaTocServer[LevelUp];
			}
			if (LevelUp <= 0)
			{
				return;
			}
			Level = LevelUp;
			foreach (ThanhVienGiaToc t in ThanhViens)
			{
				if (t._myChar != null && t._myChar.IsConnection)
				{
					t._myChar.SendMessage(UtilMessage.SendThongBao("Gia tộc của bạn vừa được nâng cấp lên Level " + Level, Util.YELLOW_MID));
				}
			}
		}
	}
}
