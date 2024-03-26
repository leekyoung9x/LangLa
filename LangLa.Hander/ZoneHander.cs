using System.Linq;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class ZoneHander
	{
		public static bool SetZoneMap(Character _myChar, Map map, sbyte ZoneId = -1)
		{
			if (ZoneId != -1)
			{
				_myChar.InfoGame.ZoneGame = map.Zones[ZoneId];
				_myChar.InfoGame.ZoneGame.AddChar(_myChar);
				return true;
			}
			sbyte MaxZone = 25;
			if (map.Id == 2 || map.Id == 22)
			{
				MaxZone = 40;
				if (_myChar.Info.Level < 10)
				{
					if (map.Zones[0].Chars.Values.Count >= 40)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						_myChar.SendMessage(UtilMessage.SendThongBao("Khu nhi đồng dã đầy người", Util.WHITE));
						return false;
					}
					_myChar.InfoGame.ZoneGame = map.Zones[0];
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
				if (_myChar.Info.Level >= 10 && _myChar.Info.Level <= 20)
				{
					if (map.Zones[1].Chars.Values.Count >= 40)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						_myChar.SendMessage(UtilMessage.SendThongBao("Khu 1x dã đầy người", Util.WHITE));
						return false;
					}
					_myChar.InfoGame.ZoneGame = map.Zones[1];
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
				if (_myChar.Info.Level > 20 && _myChar.Info.Level <= 30)
				{
					if (map.Zones[2].Chars.Values.Count >= 40)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						_myChar.SendMessage(UtilMessage.SendThongBao("Khu 2x dã đầy người", Util.WHITE));
						return false;
					}
					_myChar.InfoGame.ZoneGame = map.Zones[2];
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
				if (_myChar.Info.Level > 30 && _myChar.Info.Level <= 40)
				{
					if (map.Zones[3].Chars.Values.Count >= 40)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						_myChar.SendMessage(UtilMessage.SendThongBao("Khu 3x dã đầy người", Util.WHITE));
						return false;
					}
					_myChar.InfoGame.ZoneGame = map.Zones[3];
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
				if (_myChar.Info.Level > 40 && _myChar.Info.Level <= 50)
				{
					if (map.Zones[4].Chars.Values.Count >= 40)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						_myChar.SendMessage(UtilMessage.SendThongBao("Khu 4x dã đầy người", Util.WHITE));
						return false;
					}
					_myChar.InfoGame.ZoneGame = map.Zones[4];
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
				if (_myChar.Info.Level > 50 && _myChar.Info.Level <= 60)
				{
					if (map.Zones[5].Chars.Values.Count >= 40)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						_myChar.SendMessage(UtilMessage.SendThongBao("Khu 5x dã đầy người", Util.WHITE));
						return false;
					}
					_myChar.InfoGame.ZoneGame = map.Zones[5];
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
				if (_myChar.Info.Level > 60 && _myChar.Info.Level <= 70)
				{
					if (map.Zones[6].Chars.Values.Count >= 40)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						_myChar.SendMessage(UtilMessage.SendThongBao("Khu 6x dã đầy người", Util.WHITE));
						return false;
					}
					_myChar.InfoGame.ZoneGame = map.Zones[6];
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
			}
			else
			{
				if (map.Id == 46 || map.Id == 47)
				{
					if (_myChar.Info.IdGiaToc == -1)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						return false;
					}
					if (!_myChar.InfoGame.GiaToc.IsMoCuaAi)
					{
						_myChar.SendMessage(UtilMessage.MsgSetXy(_myChar.Info.IdUser, _myChar.Info.Cx, _myChar.Info.Cy));
						return false;
					}
					_myChar.InfoGame.ZoneGame = _myChar.InfoGame.GiaToc.AiGiaToc.Zone[(map.Id != 46) ? 1 : 0];
					if (_myChar.InfoGame.ZoneGame.Id == 0)
					{
						_myChar.SendMessage(UtilMessage.MsgUpdateHoatDong(!_myChar.InfoGame.GiaToc.AiGiaToc.IsDoneAi1));
						_myChar.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(_myChar.InfoGame.GiaToc.AiGiaToc.TimeStart, _myChar.InfoGame.GiaToc.AiGiaToc.TimeHoatDong, _myChar.InfoGame.GiaToc.AiGiaToc.IsHoatDong));
					}
					else
					{
						_myChar.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(_myChar.InfoGame.GiaToc.AiGiaToc.TimeStart, _myChar.InfoGame.GiaToc.AiGiaToc.TimeHoatDong, _myChar.InfoGame.GiaToc.AiGiaToc.IsHoatDong));
					}
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
				if (map.Id == 89)
				{
					_myChar.InfoGame.ZoneGame.AddChar(_myChar);
					return true;
				}
			}
			for (int i = 0; i < map.Zones.Length; i++)
			{
				lock (map.Zones[i].Chars)
				{
					if (map.Zones[i].Chars.Values.Count < MaxZone)
					{
						_myChar.InfoGame.ZoneGame = map.Zones[i];
						_myChar.InfoGame.ZoneGame.AddChar(_myChar);
						return true;
					}
				}
			}
			return false;
		}

		public static void SendAttackMobInZone(Character Char, short Id, int Hp, short INDEX_SKILL, bool IsCrit, long Exp, bool IsSendExp)
		{
			int ID_CHAR = Char.Info.IdUser;
			int MPS = Char.Point.Mp;
			foreach (Character Char2 in Char.InfoGame.ZoneGame.Chars.Values)
			{
				if (Char.IsConnection)
				{
					Message i = new Message(61);
					i.WriteInt(ID_CHAR);
					i.WriteInt(MPS);
					i.WriteShort(INDEX_SKILL);
					i.WriteShort(Id);
					Char2.SendMessage(i);
					i = new Message(52);
					i.WriteShort(Id);
					i.WriteInt(Hp);
					i.WriteBool(IsCrit);
					Char2.SendMessage(i);
				}
			}
		}

		public static void OpenTabZone(Character _myChar)
		{
			Zone[] zones = MapManager.Maps[_myChar.Info.MapId].Zones;
			Message j = new Message(-6);
			j.WriteByte((sbyte)zones.Length);
			j.WriteByte(_myChar.InfoGame.ZoneGame.Id);
			j.WriteByte((sbyte)_myChar.InfoGame.ZoneGame.Chars.Values.Count);
			sbyte i = 0;
			j.WriteByte((sbyte)zones.Length);
			Zone[] array = zones;
			foreach (Zone z in array)
			{
				j.WriteByte(i);
				j.WriteByte((sbyte)z.Chars.Values.Count);
				i++;
			}
			_myChar.SendMessage(j);
		}

		private static Message MsgPickItemMap(short IdItemMap, int IdChar, Item item)
		{
			Message i = new Message(59);
			i.WriteShort(IdItemMap);
			i.WriteInt(IdChar);
			ItemHander.WriteItem(i, item);
			return i;
		}

		private static Message MsgRemoveItemMap(short IdItemMap)
		{
			Message i = new Message(58);
			i.WriteShort(IdItemMap);
			return i;
		}

		private static Message MsgRemoveVupItemPick(int IdChar, short IdItemMap, short Itemid)
		{
			Message i = new Message(-13);
			i.WriteInt(IdChar);
			i.WriteShort(IdItemMap);
			i.WriteShort(Itemid);
			return i;
		}

		public static void PickItemMap(Character _myChar, Message msg)
		{
			short IdItemMap = msg.ReadShort();
			lock (_myChar.InfoGame.ZoneGame.ItemMaps)
			{
				if (!_myChar.InfoGame.ZoneGame.ItemMaps.Values.Any((ItemMap s) => s.Id == IdItemMap))
				{
					return;
				}
				ItemMap itemMap = _myChar.InfoGame.ZoneGame.ItemMaps.Values.FirstOrDefault((ItemMap s) => s.Id == IdItemMap);
				if (itemMap.IdChar != _myChar.Info.IdUser)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Không thể nhặt vật phẩm của người khác", Util.YELLOW_MID));
				}
				else
				{
					if (InventoryHander.GetCountNotNullBag(_myChar) <= 0 || !_myChar.InfoGame.ZoneGame.ItemMaps.TryRemove(itemMap.Id, out itemMap))
					{
						return;
					}
					_myChar.InfoGame.ZoneGame.ID_ITEM_MAP--;
					if (!InventoryHander.AddItemBag(_myChar, itemMap.item, SendThongBao: false))
					{
						return;
					}
					_myChar.SendMessage(MsgPickItemMap(itemMap.Id, _myChar.Info.IdUser, itemMap.item));
					_myChar.SendMessage(MsgRemoveVupItemPick(_myChar.Info.IdUser, itemMap.Id, itemMap.item.Id));
					{
						foreach (Character c in _myChar.InfoGame.ZoneGame.Chars.Values)
						{
							if (c.IsConnection && c.Info.IdUser != _myChar.Info.IdUser)
							{
								c.SendMessage(MsgRemoveItemMap(itemMap.Id));
							}
						}
						return;
					}
				}
			}
		}

		public static Character FindCharInZome(Character _myChar, int Id)
		{
			return _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.IsConnection && s.Info.IdUser == Id);
		}

		public static void ChangeZone(Character _myChar, Message msg)
		{
			sbyte ZoneID = msg.ReadByte();
			MapManager.Maps[_myChar.Info.MapId]?.ChangeZone(_myChar, ZoneID);
		}
	}
}
