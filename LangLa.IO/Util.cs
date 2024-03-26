using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace LangLa.IO
{
	public static class Util
	{
		public static readonly sbyte YELLOW = -108;

		public static readonly sbyte WHITE = -107;

		public static readonly sbyte YELLOW_MID = -106;

		public static readonly sbyte RED_MID = -105;

		public static readonly string regex = " 0123456789+-*='\"\\/_?.,ˋˊ~ˀ:;|<>[]{}!@#$%^&*()aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW ";

		private static Random random = new Random();

		private static Regex regexItem = new Regex("^[a-zA-Z0-9 ]*$");

		private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public static bool CheckNameString(string yourString)
		{
			if (!regexItem.IsMatch(yourString))
			{
				return false;
			}
			return true;
		}

		public static int getRange(int var0, int var1, int var2, int var3)
		{
			var0 = positive(var0 - var2);
			var1 = positive(var1 - var3);
			return (var0 > var1) ? var0 : var1;
		}

		private static int positive(int var0)
		{
			return (var0 > 0) ? var0 : (-var0);
		}

		public static long CurrentTimeMillis()
		{
			return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}

		public static int NextInt(int min, int max)
		{
			return random.Next(min, max + 1);
		}

		public static short readShort(short k)
		{
			byte[] b = BitConverter.GetBytes(k);
			return BitConverter.ToInt16(new byte[2]
			{
				b[1],
				b[0]
			}, 0);
		}

		public static ushort readUshort(ushort k)
		{
			byte[] b = BitConverter.GetBytes(k);
			return BitConverter.ToUInt16(new byte[2]
			{
				b[1],
				b[0]
			}, 0);
		}

		public static int readInt(int k)
		{
			byte[] b = BitConverter.GetBytes(k);
			return BitConverter.ToInt32(new byte[4]
			{
				b[3],
				b[2],
				b[1],
				b[0]
			}, 0);
		}

		public static string readString(BinaryReader read)
		{
			byte k = read.ReadByte();
			if (k == 0)
			{
				short l = readShort(read.ReadInt16());
				if (l <= 0)
				{
					return "";
				}
				byte[] text = new byte[l];
				for (int i = 0; i < l; i++)
				{
					text[i] = read.ReadByte();
				}
				return Encoding.UTF8.GetString(text);
			}
			StringBuilder str = new StringBuilder();
			for (int j = 0; j < k; j++)
			{
				byte xx = read.ReadByte();
				str.Append(regex[xx]);
			}
			return str.ToString();
		}

		public static byte[] Decompress(byte[] data)
		{
			Inflater Inflater = new Inflater();
			Inflater.SetInput(data);
			MemoryStream stream = new MemoryStream();
			byte[] var3 = new byte[4096];
			while (!Inflater.IsFinished)
			{
				int var4 = Inflater.Inflate(var3);
				stream.Write(var3, 0, var4);
			}
			byte[] result = stream.ToArray();
			stream.Close();
			return result;
		}

		public static byte[] Compress(byte[] data)
		{
			MemoryStream output = new MemoryStream();
			Deflater deflater = new Deflater();
			deflater.SetLevel(9);
			deflater.SetInput(data);
			deflater.Finish();
			byte[] var3 = new byte[4096];
			while (!deflater.IsFinished)
			{
				output.Write(var3, 0, deflater.Deflate(var3));
			}
			deflater.Flush();
			return output.ToArray();
		}

		public static long readLong(long k)
		{
			byte[] b = BitConverter.GetBytes(k);
			return BitConverter.ToInt64(new byte[8]
			{
				b[7],
				b[6],
				b[5],
				b[4],
				b[3],
				b[2],
				b[1],
				b[0]
			}, 0);
		}

		public static void ShowErr(Exception e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(e.StackTrace);
			Console.ResetColor();
		}

		public static void ShowWarring(string text)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(text);
			Console.ResetColor();
		}

		public static void ShowLog(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(text);
			Console.ResetColor();
		}
	}
}
