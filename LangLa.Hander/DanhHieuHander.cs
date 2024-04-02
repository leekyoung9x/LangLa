using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class DanhHieuHander
	{
		public static void SelectDanhHieu(Character _myChar, Message msg)
		{
			sbyte Select = msg.ReadByte();
		}

		public static void AddDanhHieu(Character _myChar, Item item)
		{
			InfoDanhHieu infoDanhHieu = new InfoDanhHieu();
			string[] Name = DataServer.ArrItemTemplate[item.Id].name.Split(" ");
			infoDanhHieu.Id = item.Id;
			for (int i = 2; i < Name.Length; i++)
			{
				if (i > 2)
				{
					infoDanhHieu.Name += " ";
				}
				infoDanhHieu.Name += Name[i];
			}
			infoDanhHieu.Time = -1;
			_myChar.DanhHieus.Add(infoDanhHieu);
			_myChar.SendMessage(MsgSendDanhHieu(_myChar));
		}

		private static Message MsgSendDanhHieu(Character _myChar)
		{
			Message i = UtilMessage.Message123();
			i.WriteByte(-75);
			i.WriteInt(_myChar.Info.IdUser);
			_myChar.WriteDanhHieu(i);
			i.WriteByte((sbyte)(_myChar.DanhHieus.Count - 1));
			return i;
		}
	}
}
