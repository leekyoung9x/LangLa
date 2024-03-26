using System.Text;

namespace LangLa.IO
{
	public class Writer
	{
		private sbyte[] buffer;

		private int posWrite;

		private int lenght;

		public static string k = " 0123456789+-*='\"\\/_?.,ˋˊ~ˀ:;|<>[]{}!@#$%^&*()aáàảãạâấầẩẫậăắằẳẵặbcdđeéèẻẽẹêếềểễệfghiíìỉĩịjklmnoóòỏõọôốồổỗộơớờởỡợpqrstuúùủũụưứừửữựvxyýỳỷỹỵzwAÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶBCDĐEÉÈẺẼẸÊẾỀỂỄỆFGHIÍÌỈĨỊJKLMNOÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢPQRSTUÚÙỦŨỤƯỨỪỬỮỰVXYÝỲỶỸỴZW";

		public Writer()
		{
			buffer = new sbyte[2048];
			lenght = 2048;
		}

		public Writer(int Size)
		{
			buffer = new sbyte[Size];
			lenght = Size;
		}

		public void writeSByte(sbyte value)
		{
			checkLenght(1);
			buffer[posWrite++] = value;
		}

		public void setLength(int lenght)
		{
			buffer = new sbyte[lenght];
			this.lenght = lenght;
		}

		public void writeSByteUncheck(sbyte value)
		{
			buffer[posWrite++] = value;
		}

		public void writeByte(sbyte value)
		{
			writeSByte(value);
		}

		public void writeByte(int value)
		{
			writeSByte((sbyte)value);
		}

		public void writeChar(char value)
		{
			writeSByte(0);
			writeSByte((sbyte)value);
		}

		public void writeUnsignedByte(byte value)
		{
			writeSByte((sbyte)value);
		}

		public void writeUnsignedByte(byte[] value)
		{
			checkLenght(value.Length);
			for (int i = 0; i < value.Length; i++)
			{
				writeSByteUncheck((sbyte)value[i]);
			}
		}

		public void writeSByte(sbyte[] value)
		{
			checkLenght(value.Length);
			for (int i = 0; i < value.Length; i++)
			{
				writeSByteUncheck(value[i]);
			}
		}

		public void writeShort(short value)
		{
			checkLenght(2);
			for (int num = 1; num >= 0; num--)
			{
				writeSByteUncheck((sbyte)(value >> num * 8));
			}
		}

		public void writeShort(int value)
		{
			checkLenght(2);
			short num = (short)value;
			for (int num2 = 1; num2 >= 0; num2--)
			{
				writeSByteUncheck((sbyte)(num >> num2 * 8));
			}
		}

		public void writeUnsignedShort(ushort value)
		{
			checkLenght(2);
			for (int num = 1; num >= 0; num--)
			{
				writeSByteUncheck((sbyte)(value >> num * 8));
			}
		}

		public void writeInt(int value)
		{
			checkLenght(4);
			for (int num = 3; num >= 0; num--)
			{
				writeSByteUncheck((sbyte)(value >> num * 8));
			}
		}

		public void checkLenght(int ltemp)
		{
			if (posWrite + ltemp > lenght)
			{
				sbyte[] array = new sbyte[lenght + 1024 + ltemp];
				for (int i = 0; i < lenght; i++)
				{
					array[i] = buffer[i];
				}
				buffer = null;
				buffer = array;
				lenght += 1024 + ltemp;
			}
		}

		public void writeLong(long value)
		{
			checkLenght(8);
			for (int num = 7; num >= 0; num--)
			{
				writeSByteUncheck((sbyte)(value >> num * 8));
			}
		}

		public void writeBoolean(bool value)
		{
			writeSByte((sbyte)(value ? 1 : 0));
		}

		public void Write(sbyte[] data)
		{
			checkLenght(data.Length);
			for (int i = 0; i < data.Length; i++)
			{
				writeSByteUncheck(data[i]);
			}
		}

		public void writeBool(bool value)
		{
			writeSByte((sbyte)(value ? 1 : 0));
		}

		public sbyte[] getData()
		{
			if (posWrite <= 0)
			{
				return null;
			}
			sbyte[] array = new sbyte[posWrite];
			for (int i = 0; i < posWrite; i++)
			{
				array[i] = buffer[i];
			}
			return array;
		}

		public short GetSize()
		{
			return (short)posWrite;
		}

		public void writeString(string value)
		{
			char[] array = value.ToCharArray();
			writeShort((short)array.Length);
			checkLenght(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				writeSByteUncheck((sbyte)array[i]);
			}
		}

		private void writeUTF2(string var1)
		{
			Encoding unicode = Encoding.Unicode;
			Encoding encoding = Encoding.GetEncoding(65001);
			byte[] bytes = unicode.GetBytes(var1);
			byte[] array = Encoding.Convert(unicode, encoding, bytes);
			writeShort((short)array.Length);
			checkLenght(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				sbyte value2 = (sbyte)array[i];
				writeSByteUncheck(value2);
			}
		}

		public void writeUTF(string var1)
		{
			if (var1.Length > 0 && var1.Length <= 255)
			{
				writeByte(var1.Length);
				for (int var3 = 0; var3 < var1.Length; var3++)
				{
					int var2;
					if ((var2 = k.IndexOf(var1[var3])) < 0)
					{
						var2 = 0;
					}
					writeByte(var2);
				}
			}
			else
			{
				writeByte(0);
				writeUTF2(var1);
			}
		}

		public void close()
		{
			buffer = null;
			posWrite = -1;
			lenght = -1;
		}
	}
}
