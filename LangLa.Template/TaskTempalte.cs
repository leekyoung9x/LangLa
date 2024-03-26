using System.Linq;
using LangLa.Data;
using LangLa.SupportOOP;

namespace LangLa.Template
{
	public class TaskTempalte
	{
		public class StepTask
		{
			public sbyte Id;

			public string? Name;

			public short IdItem;

			public short IdNpc;

			public short IdMob;

			public short IdMap;

			public short Cx;

			public short Cy;

			public short MaxRequire;

			public string? Str1;

			public string? StrItem;
		}

		public string? Name;

		public short LevelNeed;

		public short IdNpc;

		public short IdMap;

		public short Cx;

		public short Cy;

		public string? Str1;

		public string? Str2;

		public string? Str3;

		public int Exp;

		public int Bac;

		public int BacKhoa;

		public int VangKhoa;

		public string? StrItem;

		public StepTask[]? TaskStep;

		public Item GetItemTask(sbyte IdClass, sbyte GioiTinh)
		{
			if (StrItem != null && StrItem.Length > 0)
			{
				string[] InfoItem = StrItem.Split("@");
				Item it = new Item(short.Parse(InfoItem[0]), bool.Parse(InfoItem[1]));
				it.HSD = long.Parse(InfoItem[2]);
				it.Quantity = int.Parse(InfoItem[3]);
				it.IdClass = sbyte.Parse(InfoItem[4]);
				it.Level = sbyte.Parse(InfoItem[5]);
				if (InfoItem.Length > 6)
				{
					it.Options = InfoItem[6];
				}
				ItemTemplate itemTemplate = DataServer.ArrItemTemplate[it.Id];
				if (itemTemplate.GioiTinh != 2 && itemTemplate.GioiTinh != GioiTinh)
				{
					ItemTemplate itemTemplate2 = DataServer.ArrItemTemplate.FirstOrDefault((ItemTemplate s) => s.GioiTinh == GioiTinh && s.Type == it.Type && s.LevelNeed == itemTemplate.LevelNeed);
					if (itemTemplate2 != null)
					{
						it.Id = itemTemplate2.Id;
					}
				}
				if (it.Type == 13)
				{
					switch (IdClass)
					{
					case 1:
						it.Options = "54,0,500;62,0,500";
						break;
					case 2:
						it.Options = "55,0,500;58,0,500";
						break;
					case 3:
						it.Options = "56,0,500;59,0,500";
						break;
					case 4:
						it.Options = "57,0,500;60,0,500";
						break;
					case 5:
						it.Options = "53,0,500;61,0,500";
						break;
					}
				}
				return it;
			}
			return null;
		}
	}
}
