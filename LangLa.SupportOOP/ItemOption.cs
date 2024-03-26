using Newtonsoft.Json;

namespace LangLa.SupportOOP
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ItemOption
	{
		public int[] a;

		public ItemOption(string var1)
		{
			string[] var3 = var1.Split(",");
			a = new int[var3.Length];
			for (int var2 = 0; var2 < var3.Length; var2++)
			{
				a[var2] = int.Parse(var3[var2]);
			}
		}

		public int c(int var1)
		{
			return a[1] = var1;
		}

		public string g()
		{
			string var1 = "";
			for (int var2 = 0; var2 < a.Length; var2++)
			{
				var1 += a[var2];
				if (var2 < a.Length - 1)
				{
					var1 += ",";
				}
			}
			return var1;
		}
	}
}
