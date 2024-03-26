namespace LangLa.IO
{
	public class Message
	{
		public sbyte CMD;

		private Reader _Reader;

		private Writer _Writer;

		public Message(sbyte cmd, byte[] data)
		{
			CMD = cmd;
			_Reader = new Reader(data);
		}

		public Message(sbyte var1)
		{
			CMD = var1;
			_Writer = new Writer();
		}

		public byte ReadUByte()
		{
			return _Reader.ReadUbyte();
		}

		public sbyte ReadByte()
		{
			return _Reader.ReadByte();
		}

		public short ReadShort()
		{
			return _Reader.ReadShort();
		}

		public ushort ReadUShort()
		{
			return _Reader.ReadUShort();
		}

		public int ReadInt()
		{
			return _Reader.ReadInt();
		}

		public string ReadString()
		{
			return _Reader.ReadString();
		}

		public bool ReadBool()
		{
			return _Reader.ReadBool();
		}

		public long ReadLong()
		{
			return _Reader.ReadLong();
		}

		public byte[] Read()
		{
			int lenght = ReadInt();
			return _Reader.ReadArrayByte(lenght);
		}

		public int available()
		{
			return _Reader.available();
		}

		public void WriteByte(sbyte value)
		{
			_Writer.writeByte(value);
		}

		public void WriteUShort(ushort value)
		{
			_Writer.writeUnsignedShort(value);
		}

		public void WriteInt(int value)
		{
			_Writer.writeInt(value);
		}

		public void WriteShort(short value)
		{
			_Writer.writeShort(value);
		}

		public void WriteUTF(string text)
		{
			_Writer.writeUTF(text);
		}

		public void WriteBool(bool x)
		{
			_Writer.writeBoolean(x);
		}

		public void WriteLong(long value)
		{
			_Writer.writeLong(value);
		}

		public void Wirte(sbyte[] data)
		{
			_Writer.Write(data);
		}

		public sbyte[] getData()
		{
			return _Writer.getData();
		}

		public void close()
		{
			if (_Writer != null)
			{
				_Writer.close();
			}
			if (_Reader != null)
			{
				_Reader.close();
			}
		}
	}
}
