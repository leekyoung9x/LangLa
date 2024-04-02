using System;
using System.Linq;
using LangLa.Admin;
using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class CharacterHander
	{
		public static void GetInfo(Character Char, Message msg)
		{
			string NameGet = msg.ReadString();
			if (NameGet.Equals(Char.Info.Name))
			{
				Char.WriteInfo();
			}
			else if (Char.InfoGame._CView != null && Char.InfoGame._CView.IsConnection)
			{
				Char.SendMessage(Char.InfoGame._CView.MsgWriteInfo());
			}
		}

		public static void AnCaiTrang(Character _myChar, Message msg)
		{
			sbyte Var1 = msg.ReadByte();
			_myChar.Info.SelectCaiTrang = Var1;
			Message i = UtilMessage.Message123();
			i.WriteByte(-58);
			i.WriteByte(Var1);
			_myChar.SendMessage(i);
		}

		public static void AddMyFriend(Character _myChar, Message msg)
		{
			string NameAdd = msg.ReadString();
			Character _cAdd = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.IsConnection && s.Info.Name.Equals(NameAdd));
			if (_cAdd == null)
			{
			}
		}

		public static void AttackMob(Character Char, Message msg)
		{
			short IdSkill = msg.ReadShort();
			short IdMob = -1;
			while (msg.available() > 0)
			{
				IdMob = msg.ReadShort();
			}
			if (IdMob == -1)
			{
				SkillHander.SkillNotFocus(Char, IdSkill);
				return;
			}
			Mob mob = Char.InfoGame.ZoneGame.Mobs.Values.FirstOrDefault((Mob s) => s.Hp > 0 && s.IdEntity == IdMob);
			if (mob != null)
			{
				Skill skill = Char.Skill.Skills.FirstOrDefault((Skill s) => s.IdTemplate == IdSkill);
				if (skill != null)
				{
					SkillHander.UseSkill(Char, skill, mob);
				}
			}
		}

		public static void SendInfoMobAttack(Character _myChar, Message msg)
		{
			short IdMob = msg.ReadShort();
			Mob mob = _myChar.InfoGame.ZoneGame.Mobs.Values.FirstOrDefault((Mob s) => s.IdEntity == IdMob);
			if (mob != null)
			{
				_myChar.SendMessage(mob.WriteTuongKhac());
			}
		}

		public static void AttackChar(Character _myChar, Message msg)
		{
			short IdSkillAttack = msg.ReadShort();
			Character _cAnDame = ZoneHander.FindCharInZome(_myChar, msg.ReadInt());
			Skill skill = _myChar.Skill.Skills.FirstOrDefault((Skill s) => s.IdTemplate == IdSkillAttack);
			if (skill != null && _cAnDame != null)
			{
				SkillHander.UseSkill(_myChar, skill, null, _cAnDame);
			}
		}

		public static long GetExpFormLevel(int lv)
		{
			long i = 0L;
			for (int var3 = 0; var3 < DataServer.ArrExp.Length && var3 < lv; var3++)
			{
				i += DataServer.ArrExp[var3];
			}
			return i;
		}

		public static void ChatTheGioi(Character _myChar, Message msg)
		{
			bool IsLoa = msg.ReadBool();
			string Text = msg.ReadString();
			if (IsLoa && _myChar.Inventory.Vang < 2)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.YELLOW_MID));
				return;
			}
			if (IsLoa)
			{
				InventoryHander.UpdateVang(_myChar, 2, IsThongBao: true);
			}
			LangLa.Server.Server.SendChatTheGioi(_myChar.Info.Name, Text, IsLoa);
		}

		public static void HanderChatChar(Character _myChar, Message msg)
		{
			string TextChat = msg.ReadString();
			string NameChar = _myChar.Info.Name;

            if (TextChat.Contains("item") || TextChat.Contains("money") || TextChat.Contains("point"))
            {
                try
                {
                    AdminTool.ProcessChat(_myChar, TextChat);
                }
                catch (Exception e)
                {
	                Util.ShowErr(e);
                }
            } else
			{
                foreach (Character Char in _myChar.InfoGame.ZoneGame.Chars.Values)
                {
                    if (Char.IsConnection)
                    {
                        Message i = new Message(21);
                        i.WriteUTF(NameChar);
                        i.WriteUTF(TextChat);
                        Char.SendMessage(i);
                    }
                }
            }
		}

		private static void Reset(Character _myChar, Mob mob)
		{
			if (_myChar.InfoGame.ZoneGame.Mobs.TryRemove(mob.Id, out mob))
			{
				Message i = new Message(2);
				i.WriteShort(0);
				i.WriteByte(0);
				_myChar.SendMessage(i);
				i = new Message(0);
				i.WriteShort(mob.IdEntity);
				_myChar.SendMessage(i);
				mob = null;
			}
		}

		public static int GetDameTuongKhac(Character Char1, Character Char2, int Dame)
		{
			return Dame;
		}

		public static void TrieuHoiThuCuoi(Character _myChar, Message msg)
		{
			sbyte Id = msg.ReadByte();
			switch (Id)
			{
			case 16:
			{
				if (_myChar.Inventory.ItemBody[Id] == null || _myChar.Effs.Any((InfoEff s) => s.Type == 91))
				{
					break;
				}
				InfoEff infoEff = new InfoEff(100, 0, -1, _myChar.Inventory.ItemBody[Id].Id);
				infoEff.IsAutoRemove = true;
				_myChar.Effs.Add(infoEff);
				{
					foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
					{
						if (c.IsConnection)
						{
							c.SendMessage(UtilMessage.MsgSendEff(_myChar.Info.IdUser, 100, 1, 1));
						}
					}
					break;
				}
			}
			case 14:
				if (!_myChar.InfoGame.UseSusanoFromCaiTrang)
				{
					if (_myChar.TimeChar.LastTimeUseSkillSusanoCaiTrang < Util.CurrentTimeMillis())
					{
						short ValueRamdom = (short)Util.NextInt(100, 500);
						_myChar.InfoGame.UseSusanoFromCaiTrang = true;
						_myChar.InfoGame.DameAndChinhXacSusanoCaiTrang = ValueRamdom;
						EffHander.AddEffNotFocus(_myChar, 101, ValueRamdom, 120000);
						_myChar.TimeChar.LastTimeUseSkillSusanoCaiTrang = Util.CurrentTimeMillis() + 600000;
					}
					else
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Thời gian của kỹ năng chưa hồi", Util.WHITE));
					}
				}
				break;
			}
		}
	}
}
