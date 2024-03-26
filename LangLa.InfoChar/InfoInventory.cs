using System.Collections.Generic;
using LangLa.SupportOOP;
using Newtonsoft.Json;

namespace LangLa.InfoChar
{
	[JsonObject(MemberSerialization.OptIn)]
	public class InfoInventory
	{
		[JsonProperty]
		public int Bac = int.MaxValue;

		[JsonProperty]
		public int BacKhoa = int.MaxValue;

		[JsonProperty]
		public int Vang = int.MaxValue;

		[JsonProperty]
		public int VangKhoa = int.MaxValue;

		[JsonProperty]
		public int BacBox;

		[JsonProperty]
		public int BacKhoaBox;

		[JsonProperty]
		public int VangBox;

		[JsonProperty]
		public int VangKhoaBox;

		[JsonProperty]
		public bool IsShowBox = true;

		public List<Item> ItemBag;

		public List<Item> ItemBox;

		public List<Item> ItemBody;

		public List<Item> ItemBody2;

		public List<Item> ItemVyThu;

		[JsonProperty]
		public Item[] ItemMoRongTui = new Item[3];

		public InfoInventory(bool IsNew)
		{
			ItemBag = new List<Item>(new Item[27]);
			Item it = new Item(28, IsLock: true);
			it.IdClass = -1;
			it.AddOption(2, 78);
			it.AddOption(3, 22);
			it.Index = 0;
			ItemBag[0] = it;
			ItemBody = new List<Item>(new Item[17]);
			ItemBody2 = new List<Item>(new Item[16]);
			ItemVyThu = new List<Item>(new Item[6]);
			ItemBox = new List<Item>(new Item[27]);
		}
	}
}
