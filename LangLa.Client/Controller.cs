using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LangLa.Data;
using LangLa.Hander;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Perform;
using LangLa.Server;
using LangLa.SqlConnection;

namespace LangLa.Client
{
	public class Controller : IPerformClient
	{
		private string _UserName;

		private string _PassWord;

		private Client _Client;

		private Thread? _ThreadRead;

		private Thread? _ThreadSend;

		private ConcurrentQueue<Message> _QueueMessage;

		private bool isConnection = true;

		private bool CheckKey;

		private bool CheckKey2;

		public Controller(Client client)
		{
			_Client = client;
			_QueueMessage = new ConcurrentQueue<Message>();
		}

		public void AddMessage(Message message)
		{
			lock (_QueueMessage)
			{
				_QueueMessage.Enqueue(message);
			}
		}

		public void Start(BinaryReader read, BinaryWriter writer)
		{
			BinaryReader read2 = read;
			BinaryWriter writer2 = writer;
			_ThreadRead = new Thread((ThreadStart)delegate
			{
				while (isConnection)
				{
					try
					{
						Message message = Listen(read2);
						if (message != null)
						{
							Action(message);
							message.close();
						}
					}
					catch (Exception e3)
					{
						Util.ShowErr(e3);
					}
				}
			});
			_ThreadSend = new Thread((ThreadStart)delegate
			{
				while (isConnection)
				{
					try
					{
						while (_QueueMessage.Count > 0)
						{
							if (_QueueMessage.TryDequeue(out Message result))
							{
								Perform(writer2, result);
								result.close();
							}
						}
					}
					catch (Exception e2)
					{
						if (_Client.isConnection)
						{
							_Client.close();
						}
						Util.ShowErr(e2);
					}
					Thread.Sleep(10);
				}
			});
			try
			{
				_ThreadRead.Start();
				_ThreadSend.Start();
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
		}

		public void Action(Message msg)
		{
			switch (msg.CMD)
			{
			case -127:
			case -113:
				CheckLogin(msg);
				break;
			case -122:
				msg.CMD = msg.ReadByte();
				if (msg.CMD == -127)
				{
					CheckLogin(msg, Check2: false, Check3: false, Check4: true);
				}
				else if (_Client.IsLoginGame)
				{
					MessageHander.ReadMessage122(_Client, msg);
				}
				break;
			case -124:
			case -123:
				msg.CMD = msg.ReadByte();
				if (msg.CMD == sbyte.MinValue)
				{
					CheckLogin(msg);
				}
				else if (msg.CMD == -127)
				{
					CheckLogin(msg, Check2: false, Check3: true);
				}
				else if (_Client.IsLoginGame)
				{
					MessageHander.ReadMessage123(_Client, msg);
				}
				break;
			default:
				if (_Client.IsLoginGame)
				{
					MessageHander.Perform(_Client, msg);
				}
				break;
			}
		}

		private void CheckLogin(Message msg, bool Check2 = false, bool Check3 = false, bool Check4 = false)
		{
			switch (msg.CMD)
			{
			case sbyte.MinValue:
			{
				sbyte select = msg.ReadByte();
				string name = msg.ReadString();
				if (!Util.CheckNameString(name))
				{
					_Client.AddMessage(UtilMessage.SendThongBao("Tên nhân vật không được chứa ký tự đặc biệt", Util.WHITE));
				}
				else if (name.Length <= 5)
				{
					_Client.AddMessage(UtilMessage.SendThongBao("Tên nhân vật quá ngắn", Util.WHITE));
				}
				else
				{
					if (_Client.Character != null || (_Client.ArrChar != null && !_Client.ArrChar.Any((string s) => s == null)) || !CheckKey || !CheckKey2)
					{
						break;
					}
					if (ConnectionUser.CheckNameUser(name))
					{
						_Client.Character = new Character(select, name, _Client);
						_Client.IsLoginGame = true;
						if (_Client.ArrChar != null)
						{
							_Client.Character.Index = (sbyte)_Client.ArrChar.ToList().FindIndex((string s) => s == null);
							if (_Client.Character.Index == -1)
							{
								_Client.Character.Index = 0;
							}
						}
						ConnectionUser.CreateDbChar(_Client.Character, _Client.ArrChar);
					}
					else
					{
						_Client.AddMessage(UtilMessage.SendThongBao("Tên nhân vật đã có người sử dụng", Util.WHITE));
					}
				}
				break;
			}
			case -127:
				if (Check3 && CheckKey2 && CheckKey)
				{
					string user = msg.ReadString();
					string pass = msg.ReadString();
					if (user.Length <= 0 || pass.Length <= 0)
					{
						_Client.AddMessage(UtilMessage.SendThongBao("Hãy nhập tài khoản và mật khẩu", Util.WHITE));
					}
					else
					{
						if (_Client.ArrChar != null)
						{
							break;
						}
						if (ConnectionUser.GetUser(_Client, user, pass))
						{
							if (LangLa.Server.Server.Clients.Values.ToList().FindIndex((Client s) => s.IdUser == _Client.IdUser && s.IsLoginGame) != -1)
							{
								LangLa.Server.Server.Clients.Values.FirstOrDefault((Client s) => s.IdUser == _Client.IdUser && s.IsLoginGame).close();
								_Client.AddMessage(UtilMessage.SendThongBao("Tài khoản được đăng nhập ở nơi khác", Util.WHITE));
								_Client.close();
							}
							else
							{
								Message i = UtilMessage.Message122();
								i.WriteByte(-113);
								i.Wirte(DataServer.Data_Game2Change);
								_Client.AddMessage(i);
								_Client.AddMessage(UtilMessage.SendTabChar(_Client));
							}
						}
						else
						{
							_Client.AddMessage(UtilMessage.SendThongBao("Tài khoản hoặc mật khẩu không chính xác", Util.RED_MID));
						}
					}
				}
				else if (Check4 && CheckKey && CheckKey2)
				{
					if (_Client.Character == null && _Client.ListChar != null)
					{
						sbyte IndexClickSelectChar = msg.ReadByte();
						if (IndexClickSelectChar <= _Client.ListChar.Count - 1)
						{
							Character _myChar = _Client.ListChar[IndexClickSelectChar];
							_Client.ListChar.Clear();
							_Client.ListChar = null;
							_Client.Character = _myChar;
							_Client.IsLoginGame = true;
							_myChar.SetSocket(_Client);
						}
					}
				}
				else
				{
					string Key = msg.ReadString();
					byte[] data = msg.Read();
					string Resource = Encoding.Default.GetString(data);
					if (Key.Equals("4563b0fde5eb26fccee6e6a6d5f6e784"))
					{
						CheckKey = true;
					}
					else
					{
						_Client.close();
					}
				}
				break;
			case -113:
				if (!CheckKey)
				{
					long Time = Util.CurrentTimeMillis() + 6000;
					new Task(delegate
					{
						while (isConnection)
						{
							if (Util.CurrentTimeMillis() > Time && !_Client.IsLoginGame)
							{
								try
								{
									if (isConnection)
									{
										_Client.AddMessage(UtilMessage.StopLoad());
									}
									break;
								}
								catch (Exception)
								{
									break;
								}
							}
							Thread.Sleep(100);
						}
					}).Start();
				}
				else
				{
					CheckKey2 = true;
				}
				break;
			}
		}

		public Message Listen(BinaryReader Read)
		{
			byte[] data = null;
			sbyte cmd = -1;
			try
			{
				cmd = Read.ReadSByte();
				int length = 0;
				bool canGiaiNen = false;
				switch (cmd)
				{
				case -84:
				case -83:
				case -82:
				case 123:
				case 124:
				case 125:
				{
					bool when_move = cmd == 123 || cmd == 124 || cmd == 125;
					bool xy = cmd == 123 || cmd == -84;
					bool send_x = cmd == 125 || cmd == -82;
					short x = 0;
					short y = 0;
					if (!_Client.IsLoginGame)
					{
						_Client.close();
						return null;
					}
					if (xy)
					{
						x = Util.readShort(Read.ReadInt16());
						y = Util.readShort(Read.ReadInt16());
						_Client.Character.SetXY(x, y);
					}
					else
					{
						if (send_x)
						{
							x = Read.ReadSByte();
						}
						else
						{
							y = Read.ReadSByte();
						}
						_Client.Character.SetXY(x, y, isNext: true);
					}
					if (x != 0 || y != 0)
					{
						_Client.Character.InfoGame.ZoneGame.UpdateXyChar(_Client.Character, when_move);
					}
					return null;
				}
				case sbyte.MinValue:
					cmd = Read.ReadSByte();
					length = ((Read.ReadSByte() << 24) & 0xFF) | ((Read.ReadSByte() << 16) & 0xFF) | ((Read.ReadSByte() << 8) & 0xFF) | (Read.ReadSByte() & 0xFF);
					canGiaiNen = true;
					break;
				case -80:
					cmd = Read.ReadSByte();
					length = ((Read.ReadByte() << 8) & 0xFF) | (Read.ReadByte() & 0xFF);
					canGiaiNen = true;
					break;
				default:
					length = ((Read.ReadByte() << 8) & 0xFF) | (Read.ReadByte() & 0xFF);
					break;
				}
				data = Read.ReadBytes(length);
				if (canGiaiNen)
				{
					data = Util.Decompress(data);
				}
				return new Message(cmd, data);
			}
			catch (Exception)
			{
				isConnection = false;
				if (_Client != null)
				{
					_Client.close();
				}
			}
			return null;
		}

		private void ReciveMultiData(BinaryWriter writer2, ConcurrentQueue<Message> messages)
		{
			Writer writer3 = new Writer();
			short SizeMsg = (short)messages.Count;
			while (messages.Count > 0 && isConnection)
			{
				if (!_QueueMessage.TryDequeue(out Message msg))
				{
					continue;
				}
				sbyte CMD = msg.CMD;
				sbyte[] Data = msg.getData();
				int SizeData = Data.Length;
				if (SizeData > 99)
				{
					if (SizeData > 90000)
					{
						Data = Array.ConvertAll(Util.Compress(Array.ConvertAll(Data, (sbyte s) => (byte)s)), (byte s) => (sbyte)s);
						SizeData = Data.Length;
						writer3.writeByte(-80);
						writer3.writeByte(CMD);
						writer3.writeByte(Data.Length >> 8);
						writer3.writeByte(Data.Length);
						sbyte[] array = Data;
						foreach (sbyte c2 in array)
						{
							writer3.writeByte(c2);
						}
					}
					else if (SizeData <= 32767)
					{
						writer3.writeByte(-80);
						writer3.writeByte(CMD);
						writer3.writeByte((sbyte)(Data.Length >> 8));
						writer3.writeByte((sbyte)Data.Length);
						sbyte[] array2 = Data;
						foreach (sbyte c3 in array2)
						{
							writer3.writeByte(c3);
						}
					}
					else
					{
						writer3.writeByte(-128);
						writer3.writeByte(CMD);
						writer3.writeByte(Data.Length >> 24);
						writer3.writeByte(Data.Length >> 16);
						writer3.writeByte(Data.Length >> 8);
						writer3.writeByte(Data.Length);
						sbyte[] array3 = Data;
						foreach (sbyte c4 in array3)
						{
							writer3.writeByte(c4);
						}
					}
				}
				else if (CMD == -84 || CMD == 123)
				{
					writer3.writeByte(CMD);
					sbyte[] array4 = Data;
					foreach (sbyte b in array4)
					{
						writer3.writeByte(b);
					}
				}
				else if (Data.Length <= 32767)
				{
					writer3.writeByte(CMD);
					writer3.writeByte(SizeData >> 8);
					writer3.writeByte(SizeData);
					sbyte[] array5 = Data;
					foreach (sbyte b2 in array5)
					{
						writer3.writeByte(b2);
					}
				}
				else
				{
					writer3.writeByte(-128);
					writer3.writeByte(CMD);
					writer3.writeByte(Data.Length >> 24);
					writer3.writeByte(Data.Length >> 16);
					writer3.writeByte(Data.Length >> 8);
					writer3.writeByte(Data.Length);
					sbyte[] array6 = Data;
					foreach (sbyte b3 in array6)
					{
						writer3.writeByte(b3);
					}
				}
				msg.close();
			}
			if (isConnection)
			{
				sbyte[] DataSend = writer3.getData();
				DataSend = Array.ConvertAll(Util.Compress(Array.ConvertAll(DataSend, (sbyte s) => (byte)s)), (byte s) => (sbyte)s);
				writer2.Write((sbyte)(-79));
				writer2.Write((sbyte)(SizeMsg >> 8));
				writer2.Write((sbyte)SizeMsg);
				writer2.Write((sbyte)(DataSend.Length >> 8));
				writer2.Write((sbyte)DataSend.Length);
				sbyte[] array7 = DataSend;
				foreach (sbyte c in array7)
				{
					writer2.Write(c);
				}
				writer2.Flush();
			}
			writer3.close();
		}

		private void RecieveData(BinaryWriter writer, Message msg)
		{
			sbyte CMD = msg.CMD;
			sbyte[] Data = msg.getData();
			int SizeData = Data.Length;
			try
			{
				if (SizeData > 99)
				{
					if (SizeData > 90000)
					{
						Data = Array.ConvertAll(Util.Compress(Array.ConvertAll(Data, (sbyte s) => (byte)s)), (byte s) => (sbyte)s);
						SizeData = Data.Length;
						writer.Write((sbyte)(-80));
						writer.Write(CMD);
						writer.Write((sbyte)(Data.Length >> 8));
						writer.Write((sbyte)Data.Length);
						sbyte[] array = Data;
						foreach (sbyte c in array)
						{
							writer.Write(c);
						}
					}
					else if (SizeData <= 32767)
					{
						writer.Write(CMD);
						writer.Write((sbyte)(Data.Length >> 8));
						writer.Write((sbyte)Data.Length);
						sbyte[] array2 = Data;
						foreach (sbyte c2 in array2)
						{
							writer.Write(c2);
						}
					}
					else
					{
						writer.Write(sbyte.MinValue);
						writer.Write(CMD);
						writer.Write((sbyte)(Data.Length >> 24));
						writer.Write((sbyte)(Data.Length >> 16));
						writer.Write((sbyte)(Data.Length >> 8));
						writer.Write((sbyte)Data.Length);
						sbyte[] array3 = Data;
						foreach (sbyte c3 in array3)
						{
							writer.Write(c3);
						}
					}
				}
				else if (CMD == -84 || CMD == 123)
				{
					writer.Write(CMD);
					sbyte[] array4 = Data;
					foreach (sbyte b in array4)
					{
						writer.Write(b);
					}
				}
				else
				{
					writer.Write(CMD);
					writer.Write((sbyte)(SizeData >> 8));
					writer.Write((sbyte)SizeData);
					sbyte[] array5 = Data;
					foreach (sbyte b2 in array5)
					{
						writer.Write(b2);
					}
				}
				writer.Flush();
			}
			catch (Exception)
			{
			}
		}

		public void Perform(BinaryWriter writer, Message msg)
		{
			RecieveData(writer, msg);
		}

		private void _SendMuiltiMessage(BinaryWriter writer, ConcurrentQueue<Message> QueueMsg)
		{
			ReciveMultiData(writer, QueueMsg);
		}

		public void close()
		{
			isConnection = false;
			_QueueMessage.Clear();
			_Client = null;
			_ThreadRead = null;
			_ThreadSend = null;
		}
	}
}
