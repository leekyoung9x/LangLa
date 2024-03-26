using LangLa.Hander;
using LangLa.IO;

namespace LangLa.SupportOOP
{
	public class ItemMap
	{
		public short Id;

		public int IdChar;

		public short Cx;

		public short Cy;

		public Item? item;

		public ItemMap(short Id, Item item)
		{
			this.Id = Id;
			this.item = item;
		}

		public void Write(Message msg)
		{
			msg.WriteInt(IdChar);
			msg.WriteShort(Id);
			msg.WriteShort(Cx);
			msg.WriteShort(Cy);
			ItemHander.WriteItem(msg, item);
		}
	}
}
