using System.Text;
using LangLa.Data;
using LangLa.IO;
using LangLa.Model;
using LangLa.Template;

namespace LangLa.SupportOOP
{
	public class Item
	{
		public short Id;

		public sbyte Type;

		public int Quantity;

		public bool IsCongDon;

		public bool IsLock = true;

		public long HSD = -1L;

		public sbyte IdClass = (sbyte)Util.NextInt(1, 5);

		public sbyte Level;

		public string Options;

		public short Index;

		public string OptionsCTBack = "";

		public sbyte CountBuaNo;

		public ItemOption[] L(bool IsSet)
		{
			if (Options != null && Options.Length > 0)
			{
				string[] var1;
				ItemOption[] var2 = new ItemOption[(var1 = Options.Split(";")).Length];
				for (int var3 = 0; var3 < var1.Length; var3++)
				{
					var2[var3] = new ItemOption(var1[var3]);
				}
				return var2;
			}
			return null;
		}

		public Item(short Id, bool IsLock, bool SetOptionAuto = false, int Quantity = 1, string Options = "")
		{
			this.Id = Id;
			this.IsLock = IsLock;
			this.Quantity = Quantity;
			this.Options = Options;
			item_template itemTemplate = DataServer.ArrItemTemplate[Id];
			Type = itemTemplate.type;
			IsCongDon = itemTemplate.is_cong_don;
			if (SetOptionAuto && Type <= 16)
			{
				switch (Type)
				{
				case 0:
					setOptionsVuKhi(DataServer.ArrItemTemplate[Id].level_need);
					break;
				case 16:
					AddOption(340, Id);
					break;
				}
			}
		}

		public void AddOption(short ID, int Value, int Value2 = -1)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (Options.Length > 0)
			{
				stringBuilder.Append(Options).Append(";");
			}
			stringBuilder.Append(ID).Append(",").Append(Value);
			if (Value2 != -1)
			{
				stringBuilder.Append(",").Append(Value2);
			}
			Options = stringBuilder.ToString();
		}

		private void setOptionsVuKhi(int level)
		{
			AddOption(2, 50 * level / 10, 50 + 50 * level / 10);
			AddOption(3, level * 2, level * 2 + 10);
			AddOption(20, level * 2, level * 2 + 10);
			if (level < 60)
			{
				if (IdClass == 5)
				{
					AddOption(21, level, level + 10);
				}
				else
				{
					AddOption((short)(IdClass + 21), level, level + 10);
				}
			}
			else if (IdClass == 5)
			{
				AddOption(21, level * 2, (level + 10) * 2);
			}
			else
			{
				AddOption((short)(IdClass + 21), (level + 10) * 2);
			}
			if (level >= 10)
			{
				AddOption((short)(IdClass - 1 + 48), 5 * (level / 10));
			}
			if (level >= 20)
			{
				AddOption(28, level / 10 * 30);
			}
			if (level >= 30)
			{
				AddOption(31, 100 + level / 10 * 50);
			}
			if (level >= 40)
			{
				if (level >= 60)
				{
					AddOption(41, 120);
				}
				else if (level >= 50)
				{
					AddOption(41, 110);
				}
				else if (level >= 40)
				{
					AddOption(41, 95);
				}
			}
			if (level >= 50)
			{
				if (level >= 60)
				{
					AddOption(47, 220);
				}
				else if (level >= 50)
				{
					AddOption(47, 200);
				}
			}
		}

		private void setOptionsTrangBiPhuKien(int level)
		{
			AddOption(0, 20 * (level / 10), 20 * (level / 10) + 10);
			AddOption(1, 20 * (level / 10), 20 * (level / 10) + 10);
			int num1 = 5 * (level / 10);
			int num2 = 5 * (level / 10 + 1);
			int num3 = 20 + 20 * (level / 10 - 1);
			int num4 = 30 + 20 * (level / 10 - 1);
			if (IdClass == 5)
			{
				AddOption(7, num1, num2);
			}
			else
			{
				AddOption((short)(IdClass + 7), num1, num2);
			}
			if (Type == 9)
			{
				AddOption(12, num1, num2);
			}
			else if (Type == 7 || Type == 5)
			{
				AddOption(14, num1, num2);
			}
			else if (Type == 3)
			{
				AddOption(15, num1, num2);
			}
			else if (Type == 0 || Type == 2 || Type == 4 || Type == 6)
			{
				AddOption(13, num1, num2);
			}
			else if (Type == 8)
			{
				AddOption(18, num3, num4);
			}
			if (Type == 8)
			{
				AddOption(17, 40 + 20 * (level / 10));
			}
			else if (Type == 0 || Type == 2 || Type == 4 || Type == 6)
			{
				AddOption(26, 2 * (level / 10));
			}
			else
			{
				AddOption(27, 2 * (level / 10));
			}
			if (level >= 20)
			{
				num1 = 50 + (level / 10 - 1) * 50;
				if (Type == 0 || Type == 2 || Type == 4 || Type == 6 || Type == 8)
				{
					AddOption(29, num1);
				}
				else
				{
					AddOption(30, num1);
				}
			}
			if (level >= 30)
			{
				num1 = 5 * (level / 10);
				if (Type == 0 || Type == 2 || Type == 4 || Type == 6 || Type == 8)
				{
					AddOption(32, num1);
				}
				else
				{
					AddOption(33, num1);
				}
			}
			if (level >= 40)
			{
				num1 = 100 + 25 * (level / 10 - 4);
				if (IdClass == 5)
				{
					AddOption(35, num1);
				}
				else
				{
					AddOption((short)(IdClass + 35), num1);
				}
			}
			if (level >= 50)
			{
				num1 = 5 * (level / 10);
				AddOption(42, num1);
			}
		}
	}
}
