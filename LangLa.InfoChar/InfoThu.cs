using LangLa.SupportOOP;

namespace LangLa.InfoChar
{
	public class InfoThu
	{
		public short Id;

		public bool IsReceived;

		public string? NameSender = "";

		public string? Title = "";

		public string? Content = "";

		public int Bac;

		public int BacKhoa;

		public int Vang;

		public int VangKhoa;

		public long Exp;

		public long TimeEnd;

		public Item? Item;

		public InfoThu(Item item = null)
		{
			Item = item;
		}
	}
}
