using System.Linq;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;

namespace LangLa.Hander
{
	public static class FriendEnemyHander
	{
		public static void AddFriend(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			Character CharOder = _myChar.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.Name.Equals(Name));
			if (CharOder != null)
			{
				InfoFriend infoFriend = new InfoFriend();
				infoFriend.Name = Name;
				infoFriend.b = 2;
				infoFriend.IsOn = true;
				Message i = new Message(77);
				i.WriteUTF(Name);
				i.WriteByte(1);
				CharOder.SendMessage(i);
				_myChar.SendMessage(UtilMessage.SendThongBao("Đã gửi lời mời kết bạn đến " + Name, Util.WHITE));
			}
		}

		public static void RemoveFriend(Character _myChar, Message msg)
		{
			string Name = msg.ReadString();
			if (_myChar.Friends.Any((InfoFriend s) => s.Name.Equals(Name)))
			{
				_myChar.Friends.RemoveAt(_myChar.Friends.FindIndex((InfoFriend s) => s.Name.Equals(Name)));
				Message i = new Message(76);
				i.WriteUTF(Name);
				_myChar.SendMessage(i);
			}
		}
	}
}
