using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Perform;
using LangLa.Server;

namespace LangLa.Client
{
	public class Client
	{
		private TcpClient _Socket;

		private BinaryReader _Reader;

		private BinaryWriter _Writer;

		private IPerformClient _Perform;

		public sbyte QuantityChar;

		public int IdUser;

		public string[] ArrChar;

		public Character Character;

		public List<Character> ListChar;

		public bool IsLoginGame;

		public bool isConnection;

		public int IdClient;

		public int Money;

		public int TotalMoney;

		public string Ip;

		public Client(TcpClient Socket, int Id)
		{
			try
			{
				_Socket = Socket;
				_Reader = new BinaryReader(_Socket.GetStream());
				_Writer = new BinaryWriter(_Socket.GetStream());
				_Perform = new Controller(this);
				IdClient = Id;
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
				_Socket.Close();
				_Reader.Close();
				_Writer.Close();
			}
		}

		public void AddMessage(Message msg)
		{
			_Perform.AddMessage(msg);
		}

		public void Start()
		{
			_Perform.Start(_Reader, _Writer);
			isConnection = true;
		}

		public void close()
		{
			Client client = this;
			if (!LangLa.Server.Server.Clients.TryRemove(IdClient, out client))
			{
				return;
			}
			isConnection = false;
			LangLa.Server.Server.SizeClient--;
			try
			{
				if (Character != null)
				{
					Character.CleanUp();
					Character = null;
				}
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
			}
			finally
			{
				if (_Perform != null)
				{
					_Perform.close();
					_Perform = null;
				}
				if (_Socket != null)
				{
					_Socket.Close();
					_Socket = null;
				}
				if (_Reader != null)
				{
					_Reader.Close();
					_Reader.Dispose();
					_Reader = null;
				}
				if (_Writer != null)
				{
					_Writer.Close();
					_Writer.Dispose();
					_Writer = null;
				}
			}
			Util.ShowLog("SIZE CHAR " + LangLa.Server.Server.Clients.Values.Count);
		}

		public void Remove()
		{
			if (_Socket != null)
			{
				_Socket.Close();
				_Socket = null;
			}
			if (_Reader != null)
			{
				_Reader.Close();
				_Reader.Dispose();
				_Reader = null;
			}
			if (_Writer != null)
			{
				_Writer.Close();
				_Writer.Dispose();
				_Writer = null;
			}
			if (_Perform != null)
			{
				_Perform.close();
				_Perform = null;
			}
		}
	}
}
