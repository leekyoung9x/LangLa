using LangLa.Client;
using LangLa.Hander;
using LangLa.OOP;
using LangLa.SqlConnection;
using LangLa.SupportOOP;

namespace LangLa.IO
{
	public static class UtilMessage
	{
		public static Message Message125()
		{
			return new Message(-125);
		}

		public static Message Message124()
		{
			return new Message(-124);
		}

		public static Message Message123()
		{
			return new Message(-123);
		}

		public static Message Message122()
		{
			return new Message(-122);
		}

		public static Message e()
		{
			return new Message(-112);
		}

		public static Message Message111()
		{
			return new Message(-111);
		}

		public static Message SendThongBao(string text, sbyte color)
		{
			Message i = new Message(color);
			i.WriteUTF(text);
			return i;
		}

		public static Message UpdateHp_Me(int Hp, short Cx, short Cy)
		{
			Message i = new Message(66);
			i.WriteInt(Hp);
			if (Hp <= 0)
			{
				i.WriteShort(Cx);
				i.WriteShort(Cy);
				i.WriteUTF("");
			}
			return i;
		}

		public static Message SendHpFullMeInMap(int IdChar, int HpFull, int Hp)
		{
			Message i = new Message(71);
			i.WriteInt(IdChar);
			i.WriteInt(HpFull);
			i.WriteInt(Hp);
			return i;
		}

		public static Message SendMpFullMeInMap(int IdChar, int MpFull, int Mp)
		{
			Message i = new Message(69);
			i.WriteInt(IdChar);
			i.WriteInt(MpFull);
			i.WriteInt(Mp);
			return i;
		}

		public static Message SendHpMeInMap(int IdChar, int Hp, short Cx, short Cy)
		{
			Message i = new Message(70);
			i.WriteInt(IdChar);
			i.WriteInt(Hp);
			if (Hp <= 0)
			{
				i.WriteShort(Cx);
				i.WriteShort(Cy);
				i.WriteUTF("");
			}
			return i;
		}

		public static Message SendMpMeInMap(int IdChar, int Mp)
		{
			Message i = new Message(68);
			i.WriteInt(IdChar);
			i.WriteInt(Mp);
			return i;
		}

		public static Message UpdateMp_Me(int Mp)
		{
			Message i = new Message(64);
			i.WriteInt(Mp);
			return i;
		}

		public static Message SendDameHpChar(int IdChar, int Mp, int Hp, bool IsCrit, short Cx, short Cy)
		{
			Message i = new Message(55);
			i.WriteInt(IdChar);
			i.WriteInt(Mp);
			i.WriteInt(Hp);
			i.WriteBool(IsCrit);
			i.WriteShort(Cx);
			i.WriteShort(Cy);
			i.WriteUTF("");
			return i;
		}

		public static Message UpdateHpFull_Me(int HpFull, int Hp)
		{
			Message i = new Message(67);
			i.WriteInt(HpFull);
			i.WriteInt(Hp);
			return i;
		}

		public static Message UpdateMpFull_Me(int MpFull, int Mp)
		{
			Message i = new Message(65);
			i.WriteInt(MpFull);
			i.WriteInt(Mp);
			return i;
		}

		public static Message MsgAddItembag(Item it)
		{
			Message i = new Message(109);
			ItemHander.WriteItem(i, it);
			return i;
		}

		public static Message MsgUpdateItemBag(Item it)
		{
			Message i = new Message(-4);
			ItemHander.WriteItem(i, it);
			return i;
		}

		public static Message MsgTachItem(short IndexItemTach, int Quantity, short IndexItemNew, int QuantityItemNew)
		{
			Message i = new Message(118);
			i.WriteShort(IndexItemTach);
			i.WriteInt(Quantity);
			i.WriteShort(IndexItemNew);
			i.WriteInt(QuantityItemNew);
			return i;
		}

		public static Message MsgUpdateSachChienDau(sbyte SachChienDau)
		{
			Message i = Message123();
			i.WriteByte(-67);
			i.WriteByte(SachChienDau);
			return i;
		}

		public static Message MsgSortItem(sbyte Type)
		{
			Message i = new Message(117);
			i.WriteByte(Type);
			return i;
		}

		public static Message MsgAddMultiItem(Item[] it)
		{
			Message i = new Message(120);
			i.WriteShort((short)it.Length);
			foreach (Item it2 in it)
			{
				ItemHander.WriteItem(i, it2);
			}
			return i;
		}

		public static Message SetNullItembag(short Index)
		{
			Message i = new Message(110);
			i.WriteShort(Index);
			return i;
		}

		public static Message ClearSceen()
		{
			Message i = Message123();
			i.WriteByte(-43);
			return i;
		}

		public static Message OpenMenu(short id, string Menu)
		{
			Message i = new Message(54);
			i.WriteShort(id);
			i.WriteUTF(Menu);
			return i;
		}

		public static Message OpenMenuNotNpc(short IndexItem, string Menu)
		{
			Message i = new Message(-25);
			i.WriteShort(IndexItem);
			i.WriteUTF(Menu);
			return i;
		}

		public static Message OderMenuSelect(string Text1, string Text2)
		{
			Message i = new Message(5);
			i.WriteUTF(Text1);
			i.WriteUTF(Text2);
			return i;
		}

		public static Message OpenTabEvent(sbyte Type)
		{
			Message i = new Message(122);
			i.WriteByte(Type);
			if (Type == 87)
			{
				i.WriteByte(1);
			}
			return i;
		}

		public static Message OpenTabKhoBau(Character _myChar)
		{
			Message i = new Message(122);
			i.WriteByte(86);
			i.WriteBool(x: false);
			i.WriteInt(1);
			return i;
		}

		public static Message SendThongBaoNhacNho(string Text)
		{
			Message i = new Message(-109);
			i.WriteUTF(Text);
			i.WriteByte(116);
			return i;
		}

		public static Message SetMaxLevelCaiTrang(sbyte ValueAdd)
		{
			Message i = Message123();
			i.WriteByte(-25);
			i.WriteByte((sbyte)(17 + ValueAdd));
			return i;
		}

		public static Message MsgSendEff(int IdC, short IdEff, int Value, int Seconds)
		{
			Message i = new Message(50);
			i.WriteInt(IdC);
			i.WriteShort(IdEff);
			i.WriteInt(Value);
			i.WriteLong(Util.CurrentTimeMillis());
			i.WriteInt(Seconds);
			return i;
		}

		public static Message MsgSendEffMob(short IdMob, short IdEff, int Value, int TimeEnd)
		{
			Message i = new Message(15);
			i.WriteShort(IdMob);
			i.WriteShort(IdEff);
			i.WriteInt(Value);
			i.WriteLong(Util.CurrentTimeMillis());
			i.WriteInt(TimeEnd);
			return i;
		}

		public static Message MsgRemoveEffMob(short IdMob, short IdEff)
		{
			Message i = new Message(16);
			i.WriteShort(IdMob);
			i.WriteShort(IdEff);
			return i;
		}

		public static Message MsgRemoveEff(int IdUser, short Id)
		{
			Message i = new Message(51);
			i.WriteInt(IdUser);
			i.WriteShort(Id);
			return i;
		}

		public static Message UpdatePointMore(int IdChar, sbyte LevelPk, int TaiPhu, short Speed, sbyte SatatusGD)
		{
			Message i = new Message(33);
			i.WriteInt(IdChar);
			i.WriteByte(LevelPk);
			i.WriteInt(TaiPhu);
			i.WriteShort(Speed);
			i.WriteByte(SatatusGD);
			return i;
		}

		public static Message UpdateBodyChar(Character _myChar)
		{
			Message i = new Message(-99);
			i.WriteInt(_myChar.Info.IdUser);
			_myChar.WriteBody(i);
			return i;
		}

		public static Message ViewInfoChar(Character _myChar)
		{
			Message i = new Message(34);
			i.WriteUTF(_myChar.Info.Name);
			i.WriteLong(_myChar.Point.Exp);
			i.WriteByte(_myChar.Info.IdChar);
			i.WriteByte(_myChar.Info.IdClass);
			i.WriteByte(_myChar.Info.IdClass);
			i.WriteByte(_myChar.Info.GioiTinh);
			i.WriteByte(_myChar.Info.SachChienDau);
			_myChar.WriteBody(i);
			_myChar.WriteBody2(i);
			i.WriteShort((short)_myChar.Skill.Skills.Length);
			Skill[] skills = _myChar.Skill.Skills;
			foreach (Skill s in skills)
			{
				i.WriteShort(s.Index);
			}
			if (_myChar.Info.IdGiaToc == -1)
			{
				i.WriteUTF("");
			}
			else
			{
				i.WriteUTF(_myChar.InfoGame.GiaToc.Name);
				i.WriteUTF("");
				i.WriteByte(_myChar.Info.RoleGiaToc);
			}
			_myChar.WriteDanhHieu(i);
			i.WriteByte(0);
			i.WriteByte(0);
			i.WriteByte(0);
			return i;
		}

		public static Message SendTabTrangBiLucDao()
		{
			Message i = new Message(122);
			i.WriteByte(100);
			return i;
		}

		public static Message SendTabTrangHienNhan()
		{
			Message i = new Message(122);
			i.WriteByte(64);
			return i;
		}

		public static Message UseItemCanTime(int Time, string Name, sbyte var, int IdChar, short ap = 0)
		{
			Message i = new Message(4);
			i.WriteInt(Time);
			i.WriteUTF(Name);
			i.WriteByte(var);
			i.WriteInt(IdChar);
			i.WriteShort(ap);
			return i;
		}

		public static Message DropItemMobToMap(short IdMob, ItemMap itemMap)
		{
			Message i = new Message(60);
			i.WriteShort(IdMob);
			i.WriteInt(itemMap.IdChar);
			i.WriteShort(itemMap.Id);
			i.WriteShort(itemMap.Cx);
			i.WriteShort(itemMap.Cy);
			ItemHander.WriteItem(i, itemMap.item);
			return i;
		}

		public static Message SendXYChar(int ID_Char, short Cx, short Cy, bool when_move)
		{
			Message i = (when_move ? new Message(123) : new Message(-84));
			i.WriteInt(ID_Char);
			i.WriteShort(Cx);
			i.WriteShort(Cy);
			return i;
		}

		public static Message AddItemMap(ItemMap item)
		{
			Message i = new Message(111);
			i.WriteInt(item.IdChar);
			i.WriteInt(item.IdChar);
			i.WriteShort(item.Id);
			i.WriteShort(item.Cx);
			i.WriteShort(item.Cy);
			ItemHander.WriteItem(i, item.item);
			return i;
		}

		public static Message AddMultiItemMap(ItemMap[] items)
		{
			Message i = Message123();
			i.WriteByte(-119);
			return i;
		}

		public static Message MsgUpdateRank(int IdChar, sbyte Rank)
		{
			Message i = Message123();
			i.WriteByte(-72);
			i.WriteInt(IdChar);
			i.WriteByte(Rank);
			return i;
		}

		public static Message MsgUpdateHoatDong(bool IsLockNextmap)
		{
			Message i = Message123();
			i.WriteByte(-79);
			i.WriteBool(IsLockNextmap);
			return i;
		}

		public static Message MsgUpdateTimeHoatDong(long TimeStart, int TimeHoatDong, bool IsHoatDong)
		{
			Message i = Message123();
			i.WriteByte(-80);
			i.WriteLong(TimeStart);
			i.WriteInt(TimeHoatDong);
			i.WriteBool(IsHoatDong);
			return i;
		}

		public static Message UpdateIndexItemBody(Item item)
		{
			Message i = new Message(-21);
			ItemHander.WriteItem(i, item);
			return i;
		}

		public static Message SendItemBodyTobag(sbyte IndexBody, sbyte IndexBag)
		{
			Message i = new Message(113);
			i.WriteByte(IndexBody);
			i.WriteShort(IndexBag);
			return i;
		}

		public static Message MsgSetXy(int IdChar, short X, short Y)
		{
			Message i = new Message(102);
			i.WriteInt(IdChar);
			i.WriteShort(X);
			i.WriteShort(Y);
			return i;
		}

		public static Message SendTabTrangByakugan()
		{
			Message i = new Message(122);
			i.WriteByte(66);
			return i;
		}

		public static Message SendTabTrangRinnegan()
		{
			Message i = new Message(122);
			i.WriteByte(67);
			return i;
		}

		public static Message SendTabTrangSharingan()
		{
			Message i = new Message(122);
			i.WriteByte(65);
			return i;
		}

		public static Message SendTabDoiBuaNo()
		{
			Message i = new Message(122);
			i.WriteByte(97);
			return i;
		}

		public static Message StopLoad()
		{
			Message i = new Message(-122);
			i.WriteByte(-117);
			return i;
		}

		public static Message SetNullItemFromType(sbyte Type, short Index)
		{
			Message i = new Message(-16);
			i.WriteByte(Type);
			i.WriteShort(Index);
			return i;
		}

		public static Message ClearMessage()
		{
			Message i = new Message(-86);
			i.WriteByte(0);
			return i;
		}

		public static Message SendTabChar(LangLa.Client.Client client)
		{
			Message j = Message122();
			j.WriteByte(sbyte.MinValue);
			if (client.ArrChar == null)
			{
				j.WriteByte(0);
				return j;
			}
			client.ListChar = ConnectionUser.GetCharDb(client.IdUser, client.QuantityChar);
			j.WriteByte((sbyte)client.ListChar.Count);
			int i = 0;
			foreach (Character Char in client.ListChar)
			{
				j.WriteInt(client.IdUser);
				j.WriteByte(0);
				j.WriteUTF(Char.Info.Name);
				j.WriteByte(Char.Info.IdChar);
				j.WriteByte(Char.Info.GioiTinh);
				j.WriteByte(Char.Info.IdClass);
				j.WriteByte(0);
				j.WriteByte(0);
				j.WriteShort(Char.Info.Speed);
				j.WriteInt(Char.Point.Hp);
				j.WriteInt(Char.Point.HpFull);
				j.WriteInt(Char.Point.Mp);
				j.WriteInt(Char.Point.MpFull);
				j.WriteLong(Char.Point.Exp);
				j.WriteShort(Char.Info.Cx);
				j.WriteShort(Char.Info.Cy);
				j.WriteByte(0);
				Char.WriteBody(j);
				Char.WirteEff(j);
				j.WriteByte(0);
				j.WriteByte(0);
				j.WriteByte(0);
				j.WriteByte(0);
			}
			return j;
		}
	}
}
