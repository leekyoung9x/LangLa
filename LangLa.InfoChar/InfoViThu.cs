using System.Collections.Generic;

namespace LangLa.InfoChar
{
	public class InfoViThu
	{
		public class ViThu
		{
			public sbyte Id;

			public sbyte Level;

			public string? Options;
		}

		public List<ViThu> VyThus = new List<ViThu>();

		public sbyte MaxCoutViThu = 1;
	}
}
