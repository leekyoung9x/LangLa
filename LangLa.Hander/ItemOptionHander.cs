using System.Collections.Generic;
using System.Linq;
using System.Text;
using LangLa.Data;
using LangLa.IO;
using LangLa.Model;
using LangLa.Server;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.Hander
{
	public static class ItemOptionHander
	{
		public static void UpOptionCuongHoa(Item item)
		{
			if (item.Options.Length > 0)
			{
				string[] Options = item.Options.Split(";");
				sbyte LevelAdd = item.Level;
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < Options.Length; i++)
				{
					string[] Value = Options[i].Split(",");
					short Id = short.Parse(Value[0]);
					int Value2 = int.Parse(Value[1]);
					ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[Id];
					if (itemOptionTemplate.Type != 8)
					{
						if (itemOptionTemplate.Options.Equals(""))
						{
							stringBuilder.Append(";").Append(Id).Append(",")
								.Append(Value2);
							continue;
						}
						if (i > 0)
						{
							stringBuilder.Append(";");
						}
						string[] OptionNangCap = itemOptionTemplate.Options.Split(";");
						stringBuilder.Append(Id).Append(",").Append(Value2 + int.Parse(OptionNangCap[LevelAdd]));
					}
					else
					{
						stringBuilder.Append(";").Append(Id).Append(",")
							.Append(Value2)
							.Append(",")
							.Append(short.Parse(Value[2]))
							.Append(",")
							.Append(short.Parse(Value[3]));
					}
				}
				item.Options = stringBuilder.ToString();
			}
			item.Level++;
		}

		public static void DonwOptionItem(Item item, sbyte Level)
		{
			if (item.Options.Length <= 0)
			{
				return;
			}
			string[] Options = item.Options.Split(";");
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < Options.Length; j++)
			{
				string[] Value = Options[j].Split(",");
				short Id = short.Parse(Value[0]);
				int Value2 = int.Parse(Value[1]);
				ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[Id];
				if (itemOptionTemplate.Type != 8)
				{
					if (itemOptionTemplate.Options.Equals(""))
					{
						stringBuilder.Append(";").Append(Id).Append(",")
							.Append(Value2);
						continue;
					}
					if (j > 0)
					{
						stringBuilder.Append(";");
					}
					string[] OptionNangCap = itemOptionTemplate.Options.Split(";");
					for (int i = 0; i < Level; i++)
					{
						Value2 -= int.Parse(OptionNangCap[i]);
					}
					stringBuilder.Append(Id).Append(",").Append(Value2);
				}
				else
				{
					stringBuilder.Append(";").Append(Id).Append(",")
						.Append(Value2)
						.Append(",")
						.Append(short.Parse(Value[2]))
						.Append(",")
						.Append(short.Parse(Value[3]));
				}
			}
			item.Level = 0;
			item.Options = stringBuilder.ToString();
		}

		public static void UpOptionNotItem(ref string Options2, sbyte Level)
		{
			if (Options2.Length <= 0)
			{
				return;
			}
			string[] Options3 = Options2.Split(";");
			sbyte LevelAdd = Level;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < Options3.Length; i++)
			{
				string[] Value = Options3[i].Split(",");
				short Id = short.Parse(Value[0]);
				int Value2 = int.Parse(Value[1]);
				ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[Id];
				if (itemOptionTemplate.Type != 8)
				{
					if (itemOptionTemplate.Options.Equals(""))
					{
						stringBuilder.Append(";").Append(Id).Append(",")
							.Append(Value2);
						continue;
					}
					if (i > 0)
					{
						stringBuilder.Append(";");
					}
					string[] OptionNangCap = itemOptionTemplate.Options.Split(";");
					stringBuilder.Append(Id).Append(",").Append(Value2 + int.Parse(OptionNangCap[LevelAdd]));
				}
				else
				{
					stringBuilder.Append(";").Append(Id).Append(",")
						.Append(Value2)
						.Append(",")
						.Append(short.Parse(Value[2]))
						.Append(",")
						.Append(short.Parse(Value[3]));
				}
			}
			Options2 = stringBuilder.ToString();
		}

		public static void AddOptionCaiTrang(Item item, Item[] item1)
		{
			List<ItemOption> Optiuons = new List<ItemOption>();
			string NameItem = DataServer.ArrItemTemplate[item.Id].name;
			List<ItemOption> OptiosBack = new List<ItemOption>();
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder3 = new StringBuilder();
			for (int i = 0; i < item1.Length; i++)
			{
				item_template itemTemplate = DataServer.ArrItemTemplate[item1[i].Id];
				if (!itemTemplate.name.Equals(NameItem))
				{
					if (i > 0)
					{
						stringBuilder.Append(";");
					}
					ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate.FirstOrDefault((ItemOptionTemplate s) => s.Name.Equals(itemTemplate.name));
					stringBuilder.Append(itemOptionTemplate.Id).Append(",").Append(0);
				}
				if (i > 0)
				{
					stringBuilder3.Append("#");
				}
				stringBuilder3.Append(item1[i].Id).Append("@");
				ItemOption[] var4;
				if ((var4 = item1[i].L(IsSet: true)) != null)
				{
					for (int var5 = 0; var5 < var4.Length; var5++)
					{
						bool var6 = true;
						for (int var7 = 0; var7 < Optiuons.Count; var7++)
						{
							if (Optiuons[var7].a[0] == var4[var5].a[0])
							{
								Optiuons[var7].c(Optiuons[var7].a[1] + var4[var5].a[1]);
								var6 = false;
								break;
							}
						}
						if (var6)
						{
							Optiuons.Add(var4[var5]);
						}
						if (var5 > 0)
						{
							stringBuilder3.Append(";");
						}
						stringBuilder3.Append(var4[var5].g());
					}
				}
				item.OptionsCTBack += stringBuilder3.ToString();
				stringBuilder3.Clear();
			}
			item.Level += (sbyte)item1.Length;
			if (item.Level >= 18)
			{
				item.Level = 18;
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			int j = 0;
			foreach (ItemOption o in Optiuons)
			{
				if (j > 0)
				{
					stringBuilder2.Append(";");
				}
				stringBuilder2.Append(o.g());
				j++;
			}
			item.Options = stringBuilder2.ToString() + stringBuilder.ToString();
		}

		public static void DichChuyenOptionTrangBi(Item item1, Item item2)
		{
			ItemOption[] itemOptions = item1.L(IsSet: true);
			ItemOption[] itemOptions2 = item2.L(IsSet: true);
			for (int i = 0; i < itemOptions.Length; i++)
			{
				for (int j = 0; j < itemOptions2.Length; j++)
				{
					if (itemOptions[i].a[0] == itemOptions2[j].a[0])
					{
						itemOptions2[j].a[1] = itemOptions[i].a[1];
					}
				}
			}
			StringBuilder stringBuilder1 = new StringBuilder();
			int k = 0;
			ItemOption[] array = itemOptions2;
			foreach (ItemOption o in array)
			{
				if (k > 0)
				{
					stringBuilder1.Append(";");
				}
				stringBuilder1.Append(o.g());
				k++;
			}
			item2.Level = item1.Level;
			item2.Options = stringBuilder1.ToString();
			DonwOptionItem(item1, item1.Level);
		}

		public static void UpOptionHienNhan(Item item)
		{
			item.IsLock = true;
			ItemOption[] var2 = item.L(IsSet: true);
			string[] var3 = new string[6] { "", "168,10", "169,10", "170,10", "171,10", "172,10" };
			string[] var4 = new string[6] { "", "259,80", "260,80", "261,80", "262,80", "263,80" };
			bool var5 = false;
			bool var6 = false;
			StringBuilder stringBuilder = new StringBuilder();
			for (int var7 = 0; var7 < var2.Length; var7++)
			{
				if (var2[var7].a[0] == 148)
				{
					continue;
				}
				if (var7 > 0)
				{
					stringBuilder.Append(";");
				}
				ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[var2[var7].a[0]];
				if (itemOptionTemplate.Type == 2 && !var5)
				{
					stringBuilder.Append(var3[item.IdClass]).Append(";").Append("254,10")
						.Append(";");
					var5 = true;
				}
				stringBuilder.Append(var2[var7].g());
				if (var6)
				{
					continue;
				}
				item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
				if (itemTemplate.level_need / 10 == 4 && itemOptionTemplate.Type == 6)
				{
					stringBuilder.Append(";").Append("47,150");
					var6 = true;
				}
				if (itemOptionTemplate.Type == 7)
				{
					if (itemTemplate.level_need / 10 >= 5)
					{
						stringBuilder.Append(";").Append("252,5").Append(";")
							.Append(var4[item.IdClass]);
					}
					if (itemTemplate.level_need / 10 >= 6)
					{
						stringBuilder.Append(";").Append("286,300");
					}
					var6 = true;
				}
			}
			stringBuilder.Append(";").Append("165,0");
			item.Options = stringBuilder.ToString();
		}

		public static void UpOptionSharingan(Item item)
		{
			item.IsLock = true;
			ItemOption[] var2 = item.L(IsSet: true);
			item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
			string[] var3 = null;
			if (itemTemplate.type == 8)
			{
				var3 = new string[4] { "161,85", "173,20", "253,2000", "159,0" };
			}
			else if (itemTemplate.type == 2)
			{
				var3 = new string[4] { "162,1", "173,20", "253,2000", "159,0" };
			}
			else if (itemTemplate.type == 7)
			{
				var3 = new string[4] { "167,70", "173,20", "253,2000", "159,0" };
			}
			if (var3 == null)
			{
				return;
			}
			bool var4 = false;
			bool var5 = false;
			StringBuilder stringBuilder = new StringBuilder();
			for (int var6 = 0; var6 < var2.Length; var6++)
			{
				if (var2[var6].a[0] == 148)
				{
					continue;
				}
				if (var6 > 0)
				{
					stringBuilder.Append(";");
				}
				ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[var2[var6].a[0]];
				if (itemOptionTemplate.Type == 2 && !var4)
				{
					stringBuilder.Append(var3[0]).Append(";").Append(var3[1])
						.Append(";");
					var4 = true;
				}
				stringBuilder.Append(var2[var6].g());
				if (var5)
				{
					continue;
				}
				if (itemTemplate.level_need / 10 == 4 && itemOptionTemplate.Type == 6)
				{
					stringBuilder.Append(";").Append("42,20");
					var5 = true;
				}
				if (itemOptionTemplate.Type != 7)
				{
					continue;
				}
				if (itemTemplate.level_need / 10 >= 5)
				{
					stringBuilder.Append(";").Append(var3[2]);
				}
				if (itemTemplate.level_need / 10 >= 6)
				{
					switch (itemTemplate.type)
					{
					case 6:
						stringBuilder.Append(";").Append("323,200");
						break;
					case 7:
						stringBuilder.Append(";").Append("324,250");
						break;
					case 8:
						stringBuilder.Append(";").Append("304,300");
						break;
					case 9:
						stringBuilder.Append(";").Append("310,250");
						break;
					}
				}
				var5 = true;
			}
			stringBuilder.Append(";").Append("159,0");
			item.Options = stringBuilder.ToString();
		}

		public static void UpOptionRinneGan(Item item)
		{
			item.IsLock = true;
			ItemOption[] var2 = item.L(IsSet: true);
			item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
			string[] var3 = null;
			if (itemTemplate.type == 0)
			{
				var3 = new string[4] { "256,4", "173,20", "253,2000", "164,0" };
			}
			else if (itemTemplate.type == 4)
			{
				var3 = new string[4] { "257,4", "173,20", "253,2000", "164,0" };
			}
			else if (itemTemplate.type == 3)
			{
				var3 = new string[4] { "258,25", "173,20", "253,2000", "164,0" };
			}
			if (var3 == null)
			{
				return;
			}
			bool var4 = false;
			bool var5 = false;
			StringBuilder stringBuilder = new StringBuilder();
			for (int var6 = 0; var6 < var2.Length; var6++)
			{
				if (var2[var6].a[0] == 148)
				{
					continue;
				}
				if (var6 > 0)
				{
					stringBuilder.Append(";");
				}
				ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[var2[var6].a[0]];
				if (itemOptionTemplate.Type == 2 && !var4)
				{
					stringBuilder.Append(var3[0]).Append(";").Append(var3[1])
						.Append(";");
					var4 = true;
				}
				stringBuilder.Append(var2[var6].g());
				if (var5)
				{
					continue;
				}
				if (itemTemplate.level_need / 10 == 4 && itemOptionTemplate.Type == 6)
				{
					stringBuilder.Append(";").Append("42,20");
					var5 = true;
				}
				if (itemOptionTemplate.Type != 7)
				{
					continue;
				}
				if (itemTemplate.level_need / 10 >= 5)
				{
					stringBuilder.Append(";").Append(var3[2]);
				}
				if (itemTemplate.level_need / 10 >= 6)
				{
					switch (itemTemplate.type)
					{
					case 6:
						stringBuilder.Append(";").Append("323,200");
						break;
					case 7:
						stringBuilder.Append(";").Append("324,250");
						break;
					case 8:
						stringBuilder.Append(";").Append("304,300");
						break;
					case 9:
						stringBuilder.Append(";").Append("310,250");
						break;
					}
				}
				var5 = true;
			}
			stringBuilder.Append(";").Append("164,0");
			item.Options = stringBuilder.ToString();
		}

		public static void UpOptionByaKugan(Item item)
		{
			item.IsLock = true;
			ItemOption[] var2 = item.L(IsSet: true);
			item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
			string[] var3 = null;
			if (itemTemplate.type == 5)
			{
				var3 = new string[4] { "166,60", "173,20", "253,2000", "163,0" };
			}
			else if (itemTemplate.type == 6)
			{
				var3 = new string[4] { "174,2", "173,20", "253,2000", "163,0" };
			}
			else if (itemTemplate.type == 9)
			{
				var3 = new string[4] { "255,2", "173,20", "253,2000", "163,0" };
			}
			if (var3 == null)
			{
				return;
			}
			bool var4 = false;
			bool var5 = false;
			StringBuilder stringBuilder = new StringBuilder();
			for (int var6 = 0; var6 < var2.Length; var6++)
			{
				if (var2[var6].a[0] == 148)
				{
					continue;
				}
				if (var6 > 0)
				{
					stringBuilder.Append(";");
				}
				ItemOptionTemplate itemOptionTemplate = DataServer.ArrItemOptionTemplate[var2[var6].a[0]];
				if (itemOptionTemplate.Type == 2 && !var4)
				{
					stringBuilder.Append(var3[0]).Append(";").Append(var3[1])
						.Append(";");
					var4 = true;
				}
				stringBuilder.Append(var2[var6].g());
				if (var5)
				{
					continue;
				}
				if (itemTemplate.level_need / 10 == 4 && itemOptionTemplate.Type == 6)
				{
					stringBuilder.Append(";").Append("42,20");
					var5 = true;
				}
				if (itemOptionTemplate.Type != 7)
				{
					continue;
				}
				if (itemTemplate.level_need / 10 >= 5)
				{
					stringBuilder.Append(";").Append(var3[2]);
				}
				if (itemTemplate.level_need / 10 >= 6)
				{
					switch (itemTemplate.type)
					{
					case 6:
						stringBuilder.Append(";").Append("323,200");
						break;
					case 7:
						stringBuilder.Append(";").Append("324,250");
						break;
					case 8:
						stringBuilder.Append(";").Append("304,300");
						break;
					case 9:
						stringBuilder.Append(";").Append("310,250");
						break;
					}
				}
				var5 = true;
			}
			stringBuilder.Append(";").Append("163,0");
			item.Options = stringBuilder.ToString();
		}

		public static void UpOptionLucDao(Item item, string Name)
		{
			ItemOption[] var2 = item.L(IsSet: true);
			string[] var3 = null;
			item_template itemTemplate = DataServer.ArrItemTemplate[item.Id];
			if (itemTemplate.type == 5)
			{
				var3 = new string[2] { "259,10", "326,25" };
			}
			else if (itemTemplate.type == 6)
			{
				var3 = new string[2] { "253,3500", "323,45" };
			}
			else if (itemTemplate.type == 9)
			{
				var3 = new string[2] { "261,10", "304,25" };
			}
			else if (itemTemplate.type == 0)
			{
				var3 = new string[2] { "253,3500", "304,25" };
			}
			else if (itemTemplate.type == 4)
			{
				var3 = new string[2] { "253,3500", "323,45" };
			}
			else if (itemTemplate.type == 3)
			{
				var3 = new string[2] { "263,10", "304,25" };
			}
			else if (itemTemplate.type == 8)
			{
				var3 = new string[2] { "262,10", "324,30" };
			}
			else if (itemTemplate.type == 2)
			{
				var3 = new string[2] { "253,3500", "323,45" };
			}
			else if (itemTemplate.type == 7)
			{
				var3 = new string[2] { "260,10", "326,25" };
			}
			else if (itemTemplate.type == 1)
			{
				var3 = new string[2] { "252,5", "304,45" };
			}
			if (var3 == null)
			{
				return;
			}
			bool var4 = false;
			StringBuilder stringBuilder = new StringBuilder();
			sbyte ValueTanCongLucDao = 0;
			for (int var5 = 0; var5 < var2.Length; var5++)
			{
				if (var2[var5].a[0] != 148 && var2[var5].a[0] != 159 && var2[var5].a[0] != 163 && var2[var5].a[0] != 164 && var2[var5].a[0] != 165)
				{
					if (var5 > 0)
					{
						stringBuilder.Append(";");
					}
					stringBuilder.Append(var2[var5].g());
				}
			}
			if (itemTemplate.level_need / 10 == 4)
			{
				stringBuilder.Append(";").Append(var3[0]);
				ValueTanCongLucDao = 6;
			}
			if (itemTemplate.level_need / 10 >= 6 && !var4)
			{
				stringBuilder.Append(";").Append(Util.NextInt(350, 359) + ",40");
				ValueTanCongLucDao = 8;
				var4 = true;
			}
			if (itemTemplate.level_need / 10 >= 5 && !var4)
			{
				stringBuilder.Append(";").Append(var3[1]);
				ValueTanCongLucDao = 7;
			}
			stringBuilder.Append(";").Append("361," + ValueTanCongLucDao);
			item.Options = stringBuilder.ToString();
			LangLa.Server.Server.SendThongBaoFromServer("Chúc mừng nhẫn giả " + Name + " vừa cường hóa " + itemTemplate.name + " trở thành trang bị Lục đạo " + ValueTanCongLucDao + "% tấn công cơ bản");
		}
	}
}
