using System.IO;

namespace LangLa.IO
{
	public class Reader
	{
		public BinaryReader _ReadMessage;

		private MemoryStream memoryStream;

		public Reader(byte[] data)
		{
			memoryStream = new MemoryStream(data);
			_ReadMessage = new BinaryReader(memoryStream);
		}

		public int available()
		{
			return (int)(_ReadMessage.BaseStream.Length - _ReadMessage.BaseStream.Position);
		}

		public sbyte ReadByte()
		{
			return _ReadMessage.ReadSByte();
		}

		public byte ReadUbyte()
		{
			return _ReadMessage.ReadByte();
		}

		public ushort ReadUShort()
		{
			return Util.readUshort(_ReadMessage.ReadUInt16());
		}

		public short ReadShort()
		{
			return Util.readShort(_ReadMessage.ReadInt16());
		}

		public int ReadInt()
		{
			return Util.readInt(_ReadMessage.ReadInt32());
		}

		public string ReadUTF()
		{
			return _ReadMessage.ReadString();
		}

		public string ReadString()
		{
			return Util.readString(_ReadMessage);
		}

		public byte[] ReadArrayByte(int lenght)
		{
			return _ReadMessage.ReadBytes(lenght);
		}

		public long ReadLong()
		{
			return Util.readLong(_ReadMessage.ReadInt64());
		}

		public bool ReadBool()
		{
			return _ReadMessage.ReadBoolean();
		}

		public void close()
		{
			memoryStream.Close();
			memoryStream.Dispose();
			memoryStream = null;
			_ReadMessage.Dispose();
			_ReadMessage.Close();
			_ReadMessage = null;
		}
	}
}
