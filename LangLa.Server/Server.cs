using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LangLa.Client;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.SqlConnection;

namespace LangLa.Server
{
	public class Server
	{
		public static sbyte RankCaoNhatServer;

		public static int RankAllServer;

		public static int DauTuAllServer;

		public static int TheThangAllServer;

		public static TcpListener? SocketServer;

		public static TcpListener? SokcetServer2;

		private static int x = 0;

		public static readonly sbyte ExpServer = 3;

		public static int SizeClient = 1;

		public static bool IsRunning = true;

		public static bool IsStop = false;

		public static bool LockDB = false;

		public static List<string> LogBug = new List<string>();

		public static readonly sbyte XMoneyServer = 1;

		public static readonly ConcurrentDictionary<int, LangLa.Client.Client> Clients = new ConcurrentDictionary<int, LangLa.Client.Client>();

		public static Character GetChar(string Name)
		{
			LangLa.Client.Client[] List = Clients.Values.ToArray();
			if (List != null)
			{
				for (int i = 0; i < List.Length; i++)
				{
					if (List[i] != null)
					{
						Character character = List[i].Character;
						if (List[i].isConnection && character != null && character.IsConnection && character.Info.Name.Equals(Name))
						{
							return character;
						}
					}
				}
			}
			return null;
		}

		public static Character GetChar(int UserId)
		{
			LangLa.Client.Client[] List = Clients.Values.ToArray();
			if (List != null)
			{
				for (int i = 0; i < List.Length; i++)
				{
					if (List[i] != null)
					{
						Character character = List[i].Character;
						if (List[i].isConnection && character != null && List[i].IdUser == UserId)
						{
							return character;
						}
					}
				}
			}
			return null;
		}

		public static void SendThongBaoFromServer(string Text)
		{
			foreach (LangLa.Client.Client c in Clients.Values)
			{
				if (c.isConnection)
				{
					Message i = new Message(22);
					i.WriteByte(2);
					i.WriteUTF("Hệ thống");
					i.WriteUTF(Text);
					c.AddMessage(i);
				}
			}
		}

		public static void SendChatTheGioi(string NameChar, string Text, bool IsLoa)
		{
			foreach (LangLa.Client.Client c in Clients.Values)
			{
				if (c.isConnection)
				{
					Message i = new Message(22);
					i.WriteByte((sbyte)(IsLoa ? 1 : 0));
					i.WriteUTF(NameChar);
					i.WriteUTF(Text);
					c.AddMessage(i);
				}
			}
		}

		public Server()
		{
			new Thread((ThreadStart)delegate
			{
				Util.ShowCustom("Server is ready now!", ConsoleColor.Green);
				while (!IsStop)
				{
					string text3 = Console.ReadLine();
					if (text3.Equals("stop"))
					{
						LockDB = true;
						IsRunning = false;
						long TimeEndServer = Util.CurrentTimeMillis() + 30000;
						foreach (LangLa.Client.Client current in Clients.Values)
						{
							if (current.isConnection)
							{
								Message message = new Message(-59);
								message.WriteUTF("Hệ thống");
								message.WriteUTF("Hệ thống sẽ bảo trì sau 1 phút nữa");
								current.AddMessage(message);
							}
						}
						new Task(delegate
						{
							while (Util.CurrentTimeMillis() <= TimeEndServer)
							{
								Thread.Sleep(10);
							}
							foreach (LangLa.Client.Client current2 in Clients.Values)
							{
								current2.close();
							}
							ConnectionDB.UpdateGiaToc();
							ConnectionDB.UpdateServer();
							IsStop = true;
						}).Start();
					}
					Thread.Sleep(1000);
				}
			}).Start();
			new Thread((ThreadStart)delegate
			{
				TcpListener tcpListener = new TcpListener(IPAddress.Parse(Program.AppSettings.IP), Program.AppSettings.PortEnd);
				tcpListener.Start();
				while (IsRunning)
				{
					Socket socket = tcpListener.AcceptSocket();
					try
					{
						NetworkStream stream = new NetworkStream(socket);
						TextReader textReader = new StreamReader(stream, Encoding.UTF8);
						string text2 = textReader.ReadLine();
						Console.WriteLine("Client says2: {0}", text2);
						if (text2 != null && text2.Length > 0)
						{
							ConnectionUser.CallBackCard(text2);
						}
						textReader.Close();
						textReader.Dispose();
					}
					catch (Exception e3)
					{
						Util.ShowErr(e3);
					}
					finally
					{
						socket.Close();
						socket.Dispose();
					}
				}
			}).Start();
			new Thread((ThreadStart)delegate
			{
				SocketServer = new TcpListener(IPAddress.Parse(Program.AppSettings.IP), Program.AppSettings.PortStart + 1);
				SocketServer.Start();
				while (IsRunning)
				{
					TcpClient tcpClient2 = SocketServer.AcceptTcpClient();
					string text = ((IPEndPoint)tcpClient2.Client.RemoteEndPoint).Address.ToString();
					tcpClient2.Close();
					tcpClient2.Dispose();
				}
			}).Start();
			long TimeUpdateGiaToc = Util.CurrentTimeMillis() + 3600000;
			new Thread((ThreadStart)delegate
			{
				while (IsRunning)
				{
					if (Util.CurrentTimeMillis() > TimeUpdateGiaToc)
					{
						try
						{
							lock (GiaTocManager.ListGiaTocs)
							{
								ConnectionDB.UpdateGiaToc();
							}
							ConnectionDB.UpdateServer();
						}
						catch (Exception e2)
						{
							Util.ShowErr(e2);
						}
						TimeUpdateGiaToc = Util.CurrentTimeMillis() + 3600000;
					}
					Thread.Sleep(1000);
				}
			}).Start();
			SokcetServer2 = new TcpListener(IPAddress.Parse(Program.AppSettings.IP), Program.AppSettings.PortStart);
			SokcetServer2.Start();
			while (IsRunning)
			{
				TcpClient tcpClient = SokcetServer2.AcceptTcpClient();
				if (!IsRunning)
				{
					tcpClient.Close();
					break;
				}
				lock (Clients)
				{
					string ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
					try
					{
						SizeClient++;
						LangLa.Client.Client client = new LangLa.Client.Client(tcpClient, SizeClient);
						if (Clients.TryAdd(SizeClient, client))
						{
							client.Ip = ip;
							client.Start();
						}
						else
						{
							client.Remove();
						}
					}
					catch (Exception e)
					{
						Util.ShowErr(e);
					}
					x++;
				}
			}
		}
	}
}
