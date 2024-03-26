using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LangLa.Data;
using LangLa.EventServer;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;
using LangLa.Server;
using LangLa.SqlConnection;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.Hander
{
	public static class NpcHander
	{
		private static readonly string Text = "Nói Chuyện";

		private static readonly string NpcLopHocSay = "Tẩy tiềm năng;Tẩy kỹ năng;Luyện bí kiếp;Đổi bí kiếp;Đổi bùa nổ;Trang bị lục đạo";

		private static readonly string oderPhu = "Làng lá;Làng sương mù;Làng mây;Làng đá;Làng cát;Làng cỏ;Làng mưa";

		public static bool CheckRangeClickNpc(Character _myChar, short IdNpc)
		{
			NpcTemplate[] npc = DataServer.ArrMapTemplate[_myChar.Info.MapId].ArrNpc;
			if (npc != null)
			{
				NpcTemplate[] array = npc;
				foreach (NpcTemplate i in array)
				{
					if (i.id == IdNpc && Math.Abs(i.cx - _myChar.Info.Cx) < 50)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static void OpenMenu(Character _myChar, Message msg)
		{
			short IdNpc = msg.ReadShort();
			NpcTemplate npc = DataServer.ArrMapTemplate[_myChar.Info.MapId].ArrNpc[IdNpc];
			if (Math.Abs(npc.cx - _myChar.Info.Cx) <= 50)
			{
				switch (npc.id)
				{
				case 2:
					Npc2(_myChar, Hander: false, -1);
					break;
				case 3:
					Npc3(_myChar, Hander: false, -1);
					break;
				case 4:
					Npc4(_myChar, Hander: false, -1);
					break;
				case 5:
					Npc5(_myChar, Hander: false, -1);
					break;
				case 6:
					Npc6(_myChar, Hander: false, -1);
					break;
				case 7:
					Npc7(_myChar, Hander: false, -1);
					break;
				case 8:
					Npc8(_myChar, Hander: false, -1);
					break;
				case 9:
					NpcSuKien(_myChar, Hander: false, -1);
					break;
				case 21:
					Npc21(_myChar, Hander: false, -1);
					break;
				case 28:
					NpcThoRen(_myChar, Hander: false, -1);
					break;
				case 29:
					NpcTrangPhuc(_myChar, Hander: false, -1);
					break;
				case 31:
					NpcTrangBiHokage(_myChar, Hander: false, -1);
					break;
				case 32:
					Npc32(_myChar, Hander: false, -1);
					break;
				case 40:
					Npc40(_myChar, Hander: false, -1);
					break;
				case 47:
					NpcRuongDo(_myChar, Hander: false, -1);
					break;
				case 58:
					Npc58(_myChar, Hander: false, -1);
					break;
				case 91:
					NpcPhuKien(_myChar, Hander: false, -1);
					break;
				case 92:
					NpcNgoaiTrang(_myChar, Hander: false, -1);
					break;
				case 96:
					NpcBinhKhi(_myChar, Hander: false, -1);
					break;
				case 97:
					Npc97(_myChar, Hander: false, -1);
					break;
				case 99:
					Npc99(_myChar, Hander: false, -1);
					break;
				case 94:
					NpcKonoha(_myChar, Hander: false, -1);
					break;
				case 101:
					NpcNguoiDanDuong(_myChar, Hander: false, -1);
					break;
				case 105:
					Npc105(_myChar, Hander: false, -1);
					break;
				}
				_myChar.InfoGame.IdClickNpc = (sbyte)npc.id;
			}
		}

		public static void HanderMenu(Character _myChar, Message msg)
		{
			short Idnpc = msg.ReadShort();
			sbyte Index = msg.ReadByte();
			sbyte Index2 = -1;
			while (msg.available() > 0)
			{
				Index2 = msg.ReadByte();
			}
			if (CheckRangeClickNpc(_myChar, Idnpc))
			{
				switch (Idnpc)
				{
				case 2:
					Npc2(_myChar, Hander: true, Index);
					break;
				case 3:
					Npc3(_myChar, Hander: true, Index);
					break;
				case 4:
					Npc4(_myChar, Hander: true, Index);
					break;
				case 5:
					Npc5(_myChar, Hander: true, Index);
					break;
				case 6:
					Npc6(_myChar, Hander: true, Index);
					break;
				case 7:
					Npc7(_myChar, Hander: true, Index);
					break;
				case 8:
					Npc8(_myChar, Hander: true, Index);
					break;
				case 9:
					NpcSuKien(_myChar, Hander: true, Index);
					break;
				case 21:
					Npc21(_myChar, Hander: true, Index);
					break;
				case 28:
					NpcThoRen(_myChar, Hander: true, Index);
					break;
				case 29:
					NpcTrangPhuc(_myChar, Hander: true, Index);
					break;
				case 31:
					NpcTrangBiHokage(_myChar, Hander: true, Index);
					break;
				case 32:
					Npc32(_myChar, Hander: true, Index);
					break;
				case 40:
					Npc40(_myChar, Hander: true, Index);
					break;
				case 47:
					NpcRuongDo(_myChar, Hander: true, Index);
					break;
				case 58:
					Npc58(_myChar, Hander: true, Index);
					break;
				case 91:
					NpcPhuKien(_myChar, Hander: true, Index);
					break;
				case 92:
					NpcNgoaiTrang(_myChar, Hander: true, Index);
					break;
				case 96:
					NpcBinhKhi(_myChar, Hander: true, Index);
					break;
				case 97:
					Npc97(_myChar, Hander: true, Index);
					break;
				case 99:
					Npc99(_myChar, Hander: true, Index);
					break;
				case 94:
					NpcKonoha(_myChar, Hander: true, Index);
					break;
				case 101:
					NpcNguoiDanDuong(_myChar, Hander: true, Index);
					break;
				case 105:
					Npc105(_myChar, Hander: true, Index);
					break;
				}
				_myChar.InfoGame.IsOderMenu2 = false;
				_myChar.InfoGame.IdClickNpc = (sbyte)Idnpc;
				_myChar.InfoGame.IndexClickSelect = Index;
			}
		}

		private static void Npc2(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			Character _myChar2 = _myChar;
			if (!Hander)
			{
				string Menu = ":-?Nói chuyện";
				if (_myChar2.Task.Id == 4 && _myChar2.Task.IdStep == 0)
				{
					_myChar2.SendMessage(UtilMessage.OpenMenu(2, Menu));
					TaskHander.ShowTrongTruyenTask(_myChar2, _myChar2.Task.IdStep);
				}
			}
			else
			{
				if (Select != 0 || _myChar2.Task.Id != 4 || _myChar2.Task.IdStep != 1)
				{
					return;
				}
				Mob mob = new Mob(null);
				mob.Id = 221;
				mob.Name = _myChar2.Info.Name;
				mob.IdEntity = (short)(-_myChar2.Info.IdUser);
				mob.IsPainMiniMap = false;
				mob.Level = 0;
				mob.Hp = 100;
				mob.HpFull = 100;
				mob.Cx = 1041;
				mob.Cy = 119;
				Message i = new Message(1);
				mob.Write(i);
				_myChar2.SendMessage(i);
				bool isOk = true;
				long time = Util.CurrentTimeMillis();
				short Cx = 839;
				short Cy = 118;
				i = new Message(2);
				i.WriteShort(0);
				i.WriteByte(6);
				_myChar2.SendMessage(i);
				sbyte Count = 0;
				short Delay = 0;
				new Task(delegate
				{
					while (isOk)
					{
						if (time < Util.CurrentTimeMillis())
						{
							if (Count == 10)
							{
								if (_myChar2.IsConnection)
								{
									Message message = new Message(0);
									message.WriteShort(mob.IdEntity);
									_myChar2.SendMessage(message);
									TaskHander.NextStep(_myChar2);
								}
								isOk = false;
								break;
							}
							if (Count == 0 || Count == 1)
							{
								Delay = 5000;
							}
							else if (Count == 2)
							{
								Cx = 680;
								Cy = 119;
								Delay = 4700;
							}
							else if (Count == 3)
							{
								Cx = 613;
								Cy = 242;
								Delay = 4500;
							}
							else if (Count == 4)
							{
								Cx = 501;
								Cy = 317;
								Delay = 4200;
							}
							else if (Count == 5)
							{
								Cx = 569;
								Cy = 463;
								Delay = 4000;
							}
							else if (Count == 6)
							{
								Cx = 438;
								Cy = 555;
								Delay = 3800;
							}
							else if (Count == 7)
							{
								Cx = 295;
								Cy = 511;
								Delay = 3600;
							}
							else if (Count == 8)
							{
								Cx = 191;
								Cy = 511;
								Delay = 1500;
							}
							else if (Count == 9)
							{
								Cx = 87;
								Cy = 511;
								Delay = 2000;
							}
							if (Count == 0)
							{
								Message message2 = new Message(-12);
								message2.WriteShort(mob.IdEntity);
								message2.WriteUTF("Hãy đi theo sau em");
								_myChar2.SendMessage(message2);
							}
							if (_myChar2.IsConnection)
							{
								_myChar2.SetXY(Cx, Cy);
								Message message3 = new Message(-1);
								message3.WriteShort(mob.IdEntity);
								message3.WriteShort(Cx);
								message3.WriteShort(Cy);
								_myChar2.SendMessage(message3);
							}
							time = Util.CurrentTimeMillis() + Delay;
							Count++;
						}
						if (!_myChar2.IsConnection)
						{
							break;
						}
						if (_myChar2.Info.IsDie)
						{
							Message message4 = new Message(0);
							message4.WriteShort(mob.IdEntity);
							_myChar2.SendMessage(message4);
							message4 = new Message(2);
							message4.WriteShort(0);
							message4.WriteByte(0);
							_myChar2.SendMessage(message4);
							_myChar2.SendMessage(UtilMessage.SendThongBao("Nhiệm vụ thất bại", Util.WHITE));
							_myChar2.Task.IdStep = 0;
							break;
						}
						Thread.Sleep(10);
					}
				}).Start();
			}
		}

		private static void Npc3(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu;
				if (_myChar.Task.Id == 4 && _myChar.Task.IdStep == 2)
				{
					Menu = ":-!Tìm Udon";
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 0)
				{
					Menu = ":-?Nói chuyện";
				}
				else if (_myChar.Task.Id == 4 && _myChar.Task.IdStep == -1)
				{
					Menu = ":-!Tìm Udon";
				}
				Menu = Text;
				_myChar.SendMessage(UtilMessage.OpenMenu(3, Menu));
			}
			else if (Select == 0)
			{
				if (_myChar.Task.Id == 4 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 2))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 0)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
					TaskHander.NextStep(_myChar);
				}
			}
		}

		private static void Npc4(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 3) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 0))
				{
					Menu = ":-?Nói chuyện";
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 1)
				{
					Menu = ":-?Xin hãy nhận em làm đệ tử" + NpcLopHocSay;
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 1)
				{
					Menu = NpcLopHocSay;
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(4, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 3) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 0))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 1)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
					TaskHander.NextStep(_myChar);
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 1)
				{
					PointHander.TayTiemNang(_myChar);
				}
				break;
			case 1:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 1)
				{
					PointHander.TayKyNang(_myChar);
				}
				break;
			case 4:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 1)
				{
					_myChar.SendMessage(UtilMessage.SendTabDoiBuaNo());
				}
				break;
			case 5:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 1)
				{
					_myChar.SendMessage(UtilMessage.SendTabTrangBiLucDao());
				}
				break;
			case 2:
			case 3:
				break;
			}
		}

		private static void Npc5(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 7) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 4))
				{
					Menu = ":-?Nói chuyện";
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 5)
				{
					Menu = ":-?Xin hãy nhận em làm đệ tử";
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 5)
				{
					Menu = NpcLopHocSay;
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(5, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 7) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 4))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 5)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
					TaskHander.NextStep(_myChar);
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 5)
				{
					PointHander.TayTiemNang(_myChar);
				}
				break;
			case 1:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 5)
				{
					PointHander.TayKyNang(_myChar);
				}
				break;
			case 4:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 5)
				{
					_myChar.SendMessage(UtilMessage.SendTabDoiBuaNo());
				}
				break;
			case 5:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 5)
				{
					_myChar.SendMessage(UtilMessage.SendTabTrangBiLucDao());
				}
				break;
			case 2:
			case 3:
				break;
			}
		}

		private static void Npc6(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 5) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 2))
				{
					Menu = ":-?Nói chuyện";
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 3)
				{
					Menu = ":-?Xin hãy nhận em làm đệ tử";
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 3)
				{
					Menu = NpcLopHocSay;
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(6, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 5) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 2))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 3)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
					TaskHander.NextStep(_myChar);
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 3)
				{
					PointHander.TayTiemNang(_myChar);
				}
				break;
			case 1:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 3)
				{
					PointHander.TayKyNang(_myChar);
				}
				break;
			case 4:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 3)
				{
					_myChar.SendMessage(UtilMessage.SendTabDoiBuaNo());
				}
				break;
			case 5:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 3)
				{
					_myChar.SendMessage(UtilMessage.SendTabTrangBiLucDao());
				}
				break;
			case 2:
			case 3:
				break;
			}
		}

		private static void Npc7(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 4) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 1))
				{
					Menu = ":-?Nói chuyện";
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 2)
				{
					Menu = ":-?Xin hãy nhận em làm đệ tử";
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 2)
				{
					Menu = NpcLopHocSay;
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(7, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 4) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 1))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 2)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
					TaskHander.NextStep(_myChar);
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 2)
				{
					PointHander.TayTiemNang(_myChar);
				}
				break;
			case 1:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 2)
				{
					PointHander.TayKyNang(_myChar);
				}
				break;
			case 4:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 2)
				{
					_myChar.SendMessage(UtilMessage.SendTabDoiBuaNo());
				}
				break;
			case 5:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 2)
				{
					_myChar.SendMessage(UtilMessage.SendTabTrangBiLucDao());
				}
				break;
			case 2:
			case 3:
				break;
			}
		}

		private static void Npc8(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 6) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 3))
				{
					Menu = ":-?Nói chuyện";
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 4)
				{
					Menu = ":-?Xin hãy nhận em làm đệ tử";
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 4)
				{
					Menu = NpcLopHocSay;
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(8, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if ((_myChar.Task.Id == 8 && _myChar.Task.IdStep == 6) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 3))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 8 && _myChar.Info.IdClass == 4)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
					TaskHander.NextStep(_myChar);
				}
				else if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 4)
				{
					PointHander.TayTiemNang(_myChar);
				}
				break;
			case 1:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 4)
				{
					PointHander.TayKyNang(_myChar);
				}
				break;
			case 4:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 4)
				{
					_myChar.SendMessage(UtilMessage.SendTabDoiBuaNo());
				}
				break;
			case 5:
				if (_myChar.Task.Id >= 9 && _myChar.Info.IdClass == 4)
				{
					_myChar.SendMessage(UtilMessage.SendTabTrangBiLucDao());
				}
				break;
			case 2:
			case 3:
				break;
			}
		}

		private static void NpcSuKien(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Đổi bùa nổ";
			}
			else if (Select == 0)
			{
			}
		}

		private static void NpcTrangPhuc(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Đai Trán;Áo;Bao tay;Quần;Giày";
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 3)
				{
					Menu = ":-?Nói chuyện;Đai Trán;Áo;Bao tay;Quần;Giày";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(29, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 3)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else
				{
					ShopHander.OpenShopBodyChar(_myChar, 9, -1);
				}
				break;
			case 1:
				ShopHander.OpenShopBodyChar(_myChar, 10, -1);
				break;
			case 2:
				ShopHander.OpenShopBodyChar(_myChar, 11, -1);
				break;
			case 3:
				ShopHander.OpenShopBodyChar(_myChar, 12, -1);
				break;
			case 4:
				ShopHander.OpenShopBodyChar(_myChar, 13, -1);
				break;
			}
		}

		private static void NpcTrangBiHokage(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Binh khí;Dây thừng;Móc sắt;Ống tiêu;Túi nhẫn giả;Đai trán;Áo;Bao tay;Quần;Giày";
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 7)
				{
					Menu = ":-?Nói chuyện";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(31, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 7)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else
				{
					ShopHander.OpenShopHokage(_myChar, 20, _myChar.Info.IdClass);
				}
				break;
			case 1:
				ShopHander.OpenShopHokage(_myChar, 26, _myChar.Info.IdClass);
				break;
			case 2:
				ShopHander.OpenShopHokage(_myChar, 27, _myChar.Info.IdClass);
				break;
			case 3:
				ShopHander.OpenShopHokage(_myChar, 28, _myChar.Info.IdClass);
				break;
			case 4:
				ShopHander.OpenShopHokage(_myChar, 29, _myChar.Info.IdClass);
				break;
			case 5:
				ShopHander.OpenShopHokage(_myChar, 21, _myChar.Info.IdClass);
				break;
			case 6:
				ShopHander.OpenShopHokage(_myChar, 22, _myChar.Info.IdClass);
				break;
			case 7:
				ShopHander.OpenShopHokage(_myChar, 23, _myChar.Info.IdClass);
				break;
			case 8:
				ShopHander.OpenShopHokage(_myChar, 24, _myChar.Info.IdClass);
				break;
			case 9:
				ShopHander.OpenShopHokage(_myChar, 25, _myChar.Info.IdClass);
				break;
			}
		}

		private static void Npc32(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Gia tộc;Cấm thuật Izanami;Trang bị";
				_myChar.SendMessage(UtilMessage.OpenMenu(32, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				_myChar.SendMessage(UtilMessage.OderMenuSelect("", "Thành lập;Xin vào gia tộc;Mở cửa ải gia tộc;Vào ải gia tộc;Xóa bỏ gia tộc(190 vàng)"));
				break;
			case 1:
				_myChar.SendMessage(UtilMessage.OderMenuSelect("", "Tham gia"));
				break;
			case 2:
				_myChar.SendMessage(UtilMessage.OderMenuSelect("", "Trang bị Sharingan;Trang bị Rinnegan;Trang bị Byakugan"));
				break;
			}
		}

		private static void Npc40(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			Character _myChar2 = _myChar;
			if (!Hander)
			{
				string Menu = Text;
				if (_myChar2.Task.Id == 12 && _myChar2.Task.IdStep == 0)
				{
					Menu = ":-?Kẻ địch lộ diện";
				}
				if (_myChar2.Task.Id == 12 && (_myChar2.Task.IdStep == 1 || _myChar2.Task.IdStep == 2))
				{
					Menu = ":-?Nói chuyện";
				}
				_myChar2.SendMessage(UtilMessage.OpenMenu(40, Menu));
			}
			else
			{
				if (Select != 0 || _myChar2.Task.Id != 12 || (_myChar2.Task.IdStep != 0 && _myChar2.Task.IdStep != 1 && _myChar2.Task.IdStep != 1 && _myChar2.Task.IdStep != 2))
				{
					return;
				}
				if (_myChar2.Task.IdStep == 2)
				{
					Mob mob = new Mob(_myChar2.InfoGame.ZoneGame);
					mob.Id = 222;
					mob.Name = "";
					mob.IdEntity = (short)(_myChar2.InfoGame.ZoneGame.Mobs.Values.Count + 1);
					mob.IsPainMiniMap = false;
					mob.Level = 0;
					mob.Speed = 0;
					mob.Hp = 5000;
					mob.Level = 5;
					mob.HpFull = 1000;
					mob.Cx = _myChar2.Info.Cx;
					mob.Cy = _myChar2.Info.Cy;
					Message i = new Message(1);
					mob.Write(i);
					_myChar2.SendMessage(i);
					if (!_myChar2.InfoGame.ZoneGame.Mobs.TryAdd(mob.Id, mob))
					{
						return;
					}
					long Time = Util.CurrentTimeMillis();
					long TimeReset = Util.CurrentTimeMillis() + 60000;
					bool IsFalse = true;
					bool IsReset = false;
					i = new Message(2);
					i.WriteShort(0);
					i.WriteByte(6);
					_myChar2.SendMessage(i);
					new Task(delegate
					{
						while (IsFalse)
						{
							while (Time < Util.CurrentTimeMillis())
							{
								if (_myChar2.InfoGame != null)
								{
									if (Math.Abs(mob.Cx - _myChar2.Info.Cx) <= 200 || Math.Abs(mob.Cy - _myChar2.Info.Cy) <= 200)
									{
										Message message = new Message(56);
										message.WriteShort(mob.IdEntity);
										message.WriteInt(_myChar2.Info.IdUser);
										_myChar2.SendMessage(message);
									}
									if (Math.Abs(mob.Cx - _myChar2.Info.Cx) >= 400 || Math.Abs(mob.Cy - _myChar2.Info.Cy) >= 200)
									{
										Reset(mob);
										_myChar2.SendMessage(UtilMessage.SendThongBao("! Nhiệm vụ thất bại", Util.YELLOW_MID));
										IsReset = true;
										break;
									}
									if (mob.Hp <= 0)
									{
										Reset(mob);
										TaskHander.NextStep(_myChar2);
										IsReset = true;
										break;
									}
								}
								else
								{
									Reset(mob);
									IsReset = true;
								}
								Time = Util.CurrentTimeMillis() + 1000;
							}
							if (TimeReset < Util.CurrentTimeMillis() || IsReset || _myChar2.InfoGame == null)
							{
								Reset(mob);
								IsFalse = false;
							}
							Thread.Sleep(100);
						}
					}).Start();
				}
				else
				{
					TaskHander.ShowTrongTruyenTask(_myChar2, _myChar2.Task.IdStep);
				}
			}
		}

		private static void Reset(Mob mob)
		{
			foreach (Character c in mob._Zone.Chars.Values)
			{
				if (c.IsConnection)
				{
					Message i = new Message(2);
					i.WriteShort(0);
					i.WriteByte(0);
					c.SendMessage(i);
					i = new Message(0);
					i.WriteShort(mob.IdEntity);
					c.SendMessage(i);
				}
			}
			if (mob._Zone.Mobs.TryRemove(mob.Id, out mob))
			{
				mob = null;
			}
		}

		private static void NpcRuongDo(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Rương dồ;Ở lại nơi này";
				_myChar.SendMessage(UtilMessage.OpenMenu(47, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				InventoryHander.WriteItemBox(_myChar);
				break;
			case 1:
				if (_myChar.Task.Id == 10 && _myChar.Task.IdStep == 0)
				{
					TaskHander.NextStep(_myChar);
				}
				_myChar.Info.MapBackHome = _myChar.Info.MapId;
				_myChar.SendMessage(UtilMessage.SendThongBao("Nơi này sẽ là địa điểm quay trở về khi bạn bị trọng thương", Util.YELLOW_MID));
				break;
			}
		}

		private static void Npc58(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if (_myChar.Task.Id == 10 && _myChar.Task.IdStep == -1)
				{
					Menu = ":-?Trợ giúp làng sương mù";
				}
				else if (_myChar.Task.Id == 10 && _myChar.Task.IdStep == 1)
				{
					Menu = ":-?Nói chuyện";
				}
				else if (_myChar.Task.Id == 11 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 3))
				{
					Menu = ((_myChar.Task.IdStep == -1) ? ":-?Bắt kẻ nghe lén" : ":-!Bắt kẻ nghe lén");
				}
				else if (_myChar.Task.Id == 12 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 3))
				{
					Menu = ((_myChar.Task.IdStep == -1) ? ":-?Kẻ địch lộ diện" : ":-!Kẻ địch lộ diện");
				}
				else if ((_myChar.Task.Id == 13 && _myChar.Task.IdStep == -1) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 5) || (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 7))
				{
					Menu = ":-?Cấp cứu viện binh";
					if (_myChar.Task.IdStep == 7)
					{
						Menu = ":-!Cấp cứu viện binh";
					}
				}
				else if (_myChar.Task.Id == 14 && _myChar.Task.IdStep == -1)
				{
					Menu = ":-?Giải cứu Inari";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(58, Menu));
			}
			else
			{
				if (Select != 0 || ((_myChar.Task.Id != 10 || (_myChar.Task.IdStep != -1 && _myChar.Task.IdStep != 1)) && (_myChar.Task.Id != 11 || (_myChar.Task.IdStep != -1 && _myChar.Task.IdStep != 3)) && (_myChar.Task.Id != 12 || (_myChar.Task.IdStep != -1 && _myChar.Task.IdStep != 3)) && (_myChar.Task.Id != 13 || (_myChar.Task.IdStep != -1 && _myChar.Task.IdStep != 5 && _myChar.Task.IdStep != 7)) && (_myChar.Task.Id != 14 || _myChar.Task.IdStep != -1)))
				{
					return;
				}
				if (_myChar.Task.Id == 13 && _myChar.Task.IdStep == 5)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
					return;
				}
				if (_myChar.Task.Id == 11 && _myChar.Task.IdStep == -1)
				{
					Item item = new Item(383, IsLock: true);
					item.Quantity = 3;
					InventoryHander.AddItemBag(_myChar, item);
				}
				TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
			}
		}

		private static void NpcPhuKien(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Dây thừng;Móc sắt;Ống tiêu;Túi Nhẫn giả";
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 5)
				{
					Menu = ":-?Nói chuyện;Dây thừng;Móc sắt;Ống tiêu;Túi Nhẫn giả";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(91, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 5)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else
				{
					ShopHander.OpenShopBodyChar(_myChar, 14, -1);
				}
				break;
			case 1:
				ShopHander.OpenShopBodyChar(_myChar, 15, -1);
				break;
			case 2:
				ShopHander.OpenShopBodyChar(_myChar, 16, -1);
				break;
			case 3:
				ShopHander.OpenShopBodyChar(_myChar, 17, -1);
				break;
			}
		}

		private static void NpcNgoaiTrang(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Áo choàng(theo hệ);Áo choàng;Tanto;Bí kiếp;Thú nuôi;Cải trang";
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 2)
				{
					Menu = ":-?Nói chuyện;Áo choàng(theo hệ);Áo choàng;Tanto;Bí kiếp;Thú nuôi;Cải trang";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(92, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 2)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else
				{
					ShopHander.OpenShopNgoaiTrang(_myChar, 36, _myChar.Info.IdClass);
				}
				break;
			case 1:
				ShopHander.OpenShopNgoaiTrang(_myChar, 18, -1);
				break;
			case 2:
				ShopHander.OpenShopNgoaiTrang(_myChar, 30, -1);
				break;
			case 3:
				ShopHander.OpenShopNgoaiTrang(_myChar, 19, -1);
				break;
			case 4:
				ShopHander.OpenShopNgoaiTrang(_myChar, 37, -1);
				break;
			case 5:
				ShopHander.OpenShopNgoaiTrang(_myChar, 38, -1);
				break;
			case 6:
				break;
			}
		}

		private static void NpcBinhKhi(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Binh khí(Lôi);Binh khí(Thổ);Binh khí(Thủy);Binh khí(Hỏa);Binh khí(Phong)";
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 4)
				{
					Menu = ":-?Nói chuyện;Binh khí(Lôi);Binh khí(Thổ);Binh khí(Thủy);Binh khí(Hỏa);Binh khí(Phong)";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(96, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 4)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else
				{
					ShopHander.OpenShopBodyChar(_myChar, 8, 1);
				}
				break;
			case 1:
				ShopHander.OpenShopBodyChar(_myChar, 8, 2);
				break;
			case 2:
				ShopHander.OpenShopBodyChar(_myChar, 8, 3);
				break;
			case 3:
				ShopHander.OpenShopBodyChar(_myChar, 8, 4);
				break;
			case 4:
				ShopHander.OpenShopBodyChar(_myChar, 8, 5);
				break;
			}
		}

		private static void Npc21(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 1)
				{
					Menu = ":-?Nói chuyện";
					_myChar.SendMessage(UtilMessage.OpenMenu(21, Menu));
					return;
				}
				string NameAdd = "";
				NameAdd = ((!_myChar.Info.IsLockCap) ? "Mở khóa cấp(25 vàng)" : "Khóa cấp(25 vàng)");
				Menu = "Đổi 20 vàng Nạp;Đổi 50 vàng Nạp;Đổi hết vàng Nạp;Quà kích hoạt;Thử vận may(25 vàng);Mở rộng túi;" + NameAdd;
				_myChar.SendMessage(UtilMessage.OderMenuSelect("Money : " + _myChar.TimeChar.SoVangHienCo, Menu));
			}
			else if (Select == 0 && _myChar.Task.Id == 9 && _myChar.Task.IdStep == 1)
			{
				TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
			}
		}

		private static void NpcThoRen(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Ghép đá;Cường hóa;Nâng cấp bùa nổ;Tách cường hóa;Dịch chuyển trang bị;Khảm ngọc;Tách ngọc khảm;Ghép cải trang;Tách cải trang";
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == -1)
				{
					Menu = ":-?Nhiệm vụ đầu tiên;Ghép đá;Cường hóa;Nâng cấp bùa nổ;Tách cường hóa;Dịch chuyển trang bị;Khảm ngọc;Tách ngọc khảm;Ghép cải trang;Tách cải trang";
				}
				else if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 8)
				{
					Menu = ":-?Nói chuyện;Ghép đá;Cường hóa;Nâng cấp bùa nổ;Tách cường hóa;Dịch chuyển trang bị;Khảm ngọc;Tách ngọc khảm;Ghép cải trang;Tách cải trang";
				}
				else if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 11)
				{
					Menu = ":-!Nhiệm vụ đầu tiên;Ghép đá;Cường hóa;Nâng cấp bùa nổ;Tách cường hóa;Dịch chuyển trang bị;Khảm ngọc;Tách ngọc khảm;Ghép cải trang;Tách cải trang";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(28, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if (_myChar.Task.Id == 9 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 8))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 11)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else
				{
					_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.GHEP_DA));
				}
				break;
			case 1:
				_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.CUONG_HOA));
				break;
			case 2:
				_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.NANG_CAP_BUA_NO));
				break;
			case 3:
				_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.TACH_CUONG_HOA));
				break;
			case 4:
				_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.DICH_CHUYEN_TRANG_BI));
				break;
			case 5:
				_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.KHAM_NGOC));
				break;
			case 6:
				_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.TACH_NGOC_KHAM));
				break;
			case 7:
				_myChar.SendMessage(UtilMessage.SetMaxLevelCaiTrang(_myChar.Point.PointGhepCaiTrang));
				_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.GHEP_CAI_TRANG));
				break;
			case 8:
				_myChar.SendMessage(UtilMessage.OpenTabEvent(ClickEventHander.TACH_CAI_TRANG));
				break;
			}
		}

		private static void Npc97(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 0)
				{
					Menu = ":-?Nói chuyện";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(97, Menu));
			}
			else if (Select == 0 && _myChar.Task.Id == 9 && _myChar.Task.IdStep == 0)
			{
				TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
			}
		}

		private static void Npc99(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Quán Ăn;Quán Ắn(Bạc Khóa)";
				if (_myChar.Task.Id == 3 && _myChar.Task.IdStep == -1)
				{
					Menu = ":-?Làm người tốt bụng;Quán Ăn;Quán Ắn(Bạc Khóa)";
				}
				else if (_myChar.Task.Id == 3 && _myChar.Task.IdStep == 2)
				{
					Menu = ":-?Nói chuyện;Quán Ăn;Quán Ắn(Bạc Khóa)";
				}
				else if (_myChar.Task.Id == 3 && _myChar.Task.IdStep == 4)
				{
					Menu = ":-!Làm người tốt bụng;Quán Ăn;Quán Ắn(Bạc Khóa)";
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 1)
				{
					Menu = ":-?Nói chuyện;Quán Ăn;Quán Ắn(Bạc Khóa)";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(99, Menu));
				return;
			}
			switch (Select)
			{
			case 0:
				if (_myChar.Task.Id == 3 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 2 || _myChar.Task.IdStep == 3 || _myChar.Task.IdStep == 4))
				{
					if (_myChar.Task.IdStep == 2)
					{
						TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
					}
					else
					{
						TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
					}
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 1)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
				}
				else
				{
					ShopHander.OpenShopNpc4(_myChar, 0, 4);
				}
				break;
			case 1:
				ShopHander.OpenShopNpc4(_myChar, 0, 4);
				break;
			case 2:
				ShopHander.OpenShopNpc4(_myChar, 0, 5);
				break;
			}
		}

		private static void NpcKonoha(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				if (_myChar.Task.Id == 0 && _myChar.Task.IdStep == -1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-?Luyện tập"));
				}
				else if (_myChar.Task.Id == 0 && _myChar.Task.IdStep == 3)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-!Luyện tập"));
				}
				else if (_myChar.Task.Id == 1 && _myChar.Task.IdStep == -1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-?Bắt cừu về"));
				}
				else if (_myChar.Task.Id == 1 && _myChar.Task.IdStep == 1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-!Bắt cừu về"));
				}
				else if (_myChar.Task.Id == 2 && _myChar.Task.IdStep == -1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-?Hái thuốc trị thương"));
				}
				else if (_myChar.Task.Id == 2 && _myChar.Task.IdStep == 1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-!Hái thuốc trị thương"));
				}
				else if (_myChar.Task.Id == 5 && _myChar.Task.IdStep == -1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-?Xua đuổi dơi rừng"));
				}
				else if (_myChar.Task.Id == 5 && _myChar.Task.IdStep == 1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-!Xua đuổi dơi rừng"));
				}
				else if (_myChar.Task.Id == 6 && _myChar.Task.IdStep == -1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-?Cảnh báo dân làng"));
				}
				else if (_myChar.Task.Id == 6 && _myChar.Task.IdStep == 3)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-!Cảnh báo dân làng"));
				}
				else if (_myChar.Task.Id == 7 && _myChar.Task.IdStep == -1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-?Xua đuổi hầu từ"));
				}
				else if (_myChar.Task.Id == 7 && _myChar.Task.IdStep == 1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-!Xua đuổi hầu từ"));
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == -1)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-?Nhẫn giả học viên"));
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 12)
				{
					_myChar.SendMessage(UtilMessage.OpenMenu(94, ":-!Nhẫn giả học viên"));
				}
			}
			else if (Select == 0)
			{
				if (_myChar.Task.Id == 0 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 3))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else if (_myChar.Task.Id == 1 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 1))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else if (_myChar.Task.Id == 2 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 1))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else if (_myChar.Task.Id == 5 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 1))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else if (_myChar.Task.Id == 6 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 3))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else if (_myChar.Task.Id == 7 && (_myChar.Task.IdStep == -1 || _myChar.Task.IdStep == 1))
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == -1)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
				else if (_myChar.Task.Id == 8 && _myChar.Task.IdStep == 12)
				{
					TaskHander.ShowTrongTruyenTask(_myChar, (short)(-1));
				}
			}
		}

		private static void NpcNguoiDanDuong(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = Text;
				if (_myChar.Task.Id == 9 && _myChar.Task.IdStep == 6)
				{
					Menu = ":-?Nói chuyện";
				}
				_myChar.SendMessage(UtilMessage.OpenMenu(101, Menu));
			}
			else if (Select == 0 && _myChar.Task.Id == 9 && _myChar.Task.IdStep == 6)
			{
				TaskHander.ShowTrongTruyenTask(_myChar, _myChar.Task.IdStep);
			}
		}

		private static void Npc105(Character _myChar, bool Hander = false, sbyte Select = -1)
		{
			if (!Hander)
			{
				string Menu = "Vũ khí hiền nhân";
				_myChar.SendMessage(UtilMessage.OpenMenu(105, Menu));
			}
			else if (Select == 0)
			{
				_myChar.SendMessage(UtilMessage.SendTabTrangHienNhan());
				_myChar.InfoGame.ID_Click_SHOP = 64;
			}
		}

		public static void ReadOderMenuSelect(Character _myChar, Message msg)
		{
			sbyte Select = msg.ReadByte();
			switch (_myChar.InfoGame.IdClickNpc)
			{
			case 21:
				if (InventoryHander.GetCountNotNullBag(_myChar) <= 0)
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Cần trống ít nhất 1 ô hành trang để mở rộng", Util.WHITE));
					break;
				}
				if (_myChar.InfoGame.IsOderMenu2)
				{
					Item item = null;
					switch (Select)
					{
					case 0:
						if (_myChar.Inventory.Vang < 10)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.YELLOW_MID));
							break;
						}
						if (_myChar.Inventory.ItemBag.Count + 9 > 900)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("ô túi đã đạt tối đa", Util.YELLOW_MID));
							break;
						}
						item = new Item(185, IsLock: true);
						InventoryHander.AddItemBag(_myChar, item);
						InventoryHander.AddItemBag(_myChar, item);
						ItemHander.MsgUseItemBag(_myChar, item);
						InventoryHander.UpdateVang(_myChar, 10, IsThongBao: true);
						_myChar.SendMessage(UtilMessage.ClearSceen());
						_myChar.SendMessage(UtilMessage.SendThongBao("Mở rộng ô túi thành công", Util.YELLOW_MID));
						break;
					case 1:
						if (_myChar.Inventory.Vang < 18)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.YELLOW_MID));
							break;
						}
						if (_myChar.Inventory.ItemBag.Count + 18 > 900)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("ô túi đã đạt tối đa", Util.YELLOW_MID));
							break;
						}
						item = new Item(186, IsLock: true);
						InventoryHander.AddItemBag(_myChar, item);
						InventoryHander.AddItemBag(_myChar, item);
						ItemHander.MsgUseItemBag(_myChar, item);
						InventoryHander.UpdateVang(_myChar, 18, IsThongBao: true);
						_myChar.SendMessage(UtilMessage.ClearSceen());
						_myChar.SendMessage(UtilMessage.SendThongBao("Mở rộng ô túi thành công", Util.YELLOW_MID));
						break;
					case 2:
						if (_myChar.Inventory.Vang < 25)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.YELLOW_MID));
							break;
						}
						if (_myChar.Inventory.ItemBag.Count + 27 > 900)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("ô túi đã đạt tối đa", Util.YELLOW_MID));
							break;
						}
						item = new Item(187, IsLock: true);
						InventoryHander.AddItemBag(_myChar, item);
						ItemHander.MsgUseItemBag(_myChar, item);
						InventoryHander.AddItemBag(_myChar, item);
						InventoryHander.UpdateVang(_myChar, 25, IsThongBao: true);
						_myChar.SendMessage(UtilMessage.ClearSceen());
						_myChar.SendMessage(UtilMessage.SendThongBao("Mở rộng ô túi thành công", Util.YELLOW_MID));
						break;
					case 3:
						if (_myChar.Inventory.Vang < 32)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.YELLOW_MID));
							break;
						}
						if (_myChar.Inventory.ItemBag.Count + 36 > 900)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("ô túi đã đạt tối đa", Util.YELLOW_MID));
							break;
						}
						item = new Item(468, IsLock: true);
						InventoryHander.AddItemBag(_myChar, item);
						ItemHander.MsgUseItemBag(_myChar, item);
						InventoryHander.RemoveItemBag(_myChar, item.Index);
						InventoryHander.UpdateVang(_myChar, 32, IsThongBao: true);
						_myChar.SendMessage(UtilMessage.ClearSceen());
						_myChar.SendMessage(UtilMessage.SendThongBao("Mở rộng ô túi thành công", Util.YELLOW_MID));
						break;
					}
					_myChar.InfoGame.IsOderMenu2 = false;
					break;
				}
				switch (Select)
				{
				case 0:
					if (_myChar.TimeChar.SoVangHienCo >= 20000)
					{
						ConnectionUser.DownMoney(_myChar.Info.IdUser);
						_myChar.TimeChar.TongSoVangDaNap += 200 * LangLa.Server.Server.XMoneyServer;
						_myChar.TimeChar.TimeNap = Util.CurrentTimeMillis();
						_myChar.TimeChar.SoVangHienCo -= 20000;
						InventoryHander.AddVang(_myChar, 200 * LangLa.Server.Server.XMoneyServer, ThongBao: true);
						if (!_myChar.Info.IsActive)
						{
							_myChar.Info.IsActive = true;
							_myChar.SendMessage(UtilMessage.SendThongBao("Kích hoạt tài khoản thành công", Util.YELLOW_MID));
						}
						_myChar.SendMessage(UtilMessage.ClearSceen());
					}
					else
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Số tiền không đủ", Util.YELLOW_MID));
					}
					break;
				case 1:
					if (_myChar.TimeChar.SoVangHienCo >= 50000)
					{
						ConnectionUser.DownMoney(_myChar.Info.IdUser);
						_myChar.TimeChar.TongSoVangDaNap += 500 * LangLa.Server.Server.XMoneyServer;
						_myChar.TimeChar.TimeNap = Util.CurrentTimeMillis();
						_myChar.TimeChar.SoVangHienCo -= 50000;
						InventoryHander.AddVang(_myChar, 500 * LangLa.Server.Server.XMoneyServer, ThongBao: true);
						if (!_myChar.Info.IsActive)
						{
							_myChar.Info.IsActive = true;
							_myChar.SendMessage(UtilMessage.SendThongBao("Kích hoạt tài khoản thành công", Util.YELLOW_MID));
						}
						_myChar.SendMessage(UtilMessage.ClearSceen());
					}
					else
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Số tiền không đủ", Util.YELLOW_MID));
					}
					break;
				case 2:
					if (_myChar.TimeChar.SoVangHienCo >= 1000)
					{
						if (!_myChar.Info.IsActive && _myChar.TimeChar.SoVangHienCo >= 20000)
						{
							_myChar.Info.IsActive = true;
							_myChar.SendMessage(UtilMessage.SendThongBao("Kích hoạt tài khoản thành công", Util.YELLOW_MID));
						}
						_myChar.TimeChar.TongSoVangDaNap += _myChar.TimeChar.SoVangHienCo / 100 * LangLa.Server.Server.XMoneyServer;
						_myChar.TimeChar.TimeNap = Util.CurrentTimeMillis();
						ConnectionUser.DownMoney(_myChar.Info.IdUser);
						_myChar.TimeChar.SoVangHienCo = 0;
						InventoryHander.AddVang(_myChar, _myChar.TimeChar.SoVangHienCo / 100 * LangLa.Server.Server.XMoneyServer, ThongBao: true);
						_myChar.SendMessage(UtilMessage.ClearSceen());
					}
					else
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Cần ít nhất là 1.000 Money", Util.YELLOW_MID));
					}
					break;
				case 3:
					if (!_myChar.Info.IsActive)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Tài khoản này hiện chưa được kích hoạt", Util.YELLOW_MID));
					}
					else if (_myChar.TimeChar.IsNhanQuaKichHoat)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã nhận gói quà kích hoạt rồi", Util.YELLOW_MID));
					}
					else
					{
						GiftcodeHander.CreateQuaKichHoatTest2(_myChar);
					}
					break;
				case 4:
					if (_myChar.Inventory.Vang < 25)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.YELLOW_MID));
					}
					else
					{
						_myChar.SendMessage(ClickEventHander.ShowTabThuVanMay());
					}
					break;
				case 5:
				{
					string Menu = "Mở thêm 9 ô(10 vàng);Mở thêm 18 ô( 18 vàng);Mở thêm 27 ô (25 vàng);Mở thêm 36 ô (32 vàng)";
					_myChar.SendMessage(UtilMessage.OderMenuSelect("Tối đa " + 900 + " ô túi", Menu));
					_myChar.InfoGame.IsOderMenu2 = true;
					break;
				}
				case 6:
					if (_myChar.Inventory.Vang < 25)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.YELLOW_MID));
					}
					else if (_myChar.Info.IsLockCap)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Đã mở khóa cấp", Util.YELLOW_MID));
						_myChar.SendMessage(UtilMessage.ClearSceen());
						_myChar.Info.IsLockCap = false;
					}
					else
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Đã khóa cấp bạn sẽ không nhận được kinh nghiệm khi đánh quái", Util.YELLOW_MID));
						_myChar.Info.IsLockCap = true;
						_myChar.SendMessage(UtilMessage.ClearSceen());
						InventoryHander.UpdateVang(_myChar, 25, IsThongBao: true);
					}
					break;
				}
				break;
			case 32:
				switch (_myChar.InfoGame.IndexClickSelect)
				{
				case 0:
					switch (Select)
					{
					case 0:
						if (_myChar.Info.IdGiaToc != -1)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã vào gia tộc rồi", Util.WHITE));
						}
						else if (_myChar.Inventory.ItemBag.FindIndex((Item s) => s != null && s.Id == 301) != -1)
						{
							GiaTocHander.ShowTabCreateGiaToc(_myChar);
						}
						else
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Cần 1 gia tộc lệnh", Util.WHITE));
						}
						break;
					case 1:
					{
						if (_myChar.Info.IdGiaToc != -1)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã vào gia tộc rồi", Util.WHITE));
							break;
						}
						Message j = new Message(122);
						j.WriteByte(55);
						_myChar.SendMessage(j);
						break;
					}
					case 2:
						if (_myChar.Info.IdGiaToc == -1)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Cần vào gia tộc", Util.WHITE));
						}
						else if (_myChar.Info.RoleGiaToc != 5)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Chỉ tộc trưởng mới có thể mở", Util.WHITE));
						}
						else if (_myChar.InfoGame.GiaToc.SoLanMoCuaAi <= 0)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Số lần vào ải gia tộc hôm nay đã hết", Util.WHITE));
						}
						else if (_myChar.InfoGame.GiaToc.ThanhViens.Count < 10)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Yêu cầu số lượng thành viên gia tộc phải trên 10 người", Util.WHITE));
						}
						else
						{
							new AiGiaToc(_myChar, _myChar.InfoGame.GiaToc, _myChar.Info.MapId, _myChar.InfoGame.ZoneGame.Id).Start();
						}
						break;
					case 3:
						if (_myChar.Info.IdGiaToc != -1)
						{
							if (!_myChar.InfoGame.GiaToc.IsMoCuaAi)
							{
								_myChar.SendMessage(UtilMessage.SendThongBao("Cửa ai gia tộc chưa mỏ", Util.WHITE));
								break;
							}
							_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
							_myChar.Info.MapId = 46;
							_myChar.Info.Cx = 97;
							_myChar.Info.Cy = 417;
							_myChar.JoinMap(-1, -1);
							_myChar.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(_myChar.InfoGame.GiaToc.AiGiaToc.TimeStart, _myChar.InfoGame.GiaToc.AiGiaToc.TimeHoatDong, _myChar.InfoGame.GiaToc.AiGiaToc.IsHoatDong));
							_myChar.SendMessage(UtilMessage.MsgUpdateHoatDong((_myChar.InfoGame.ZoneGame.Mobs.Values.Count != 0) ? true : false));
						}
						break;
					case 4:
						if (_myChar.Info.IdGiaToc == -1)
						{
							break;
						}
						if (_myChar.Info.RoleGiaToc != 5)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Chỉ tộc trưởng mới có thể xóa gia tộc", Util.WHITE));
							break;
						}
						if (_myChar.InfoGame.GiaToc.ThanhViens.Count > 2)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Vẫn còn thành viên trong gia tộc", Util.WHITE));
							break;
						}
						if (_myChar.Inventory.Vang < 190)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.WHITE));
							break;
						}
						lock (GiaTocManager.ListGiaTocs)
						{
							GiaTocHander.HuyGiaToc(_myChar);
							break;
						}
					}
					break;
				case 1:
					if (_myChar.TimeChar.SoLanDiCamThuat <= 0)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Số lần vào cấm thuật hôm này đã hết", Util.WHITE));
						break;
					}
					if (_myChar.InfoGame.Todoi != null)
					{
						if (!_myChar.InfoGame.IsDoiTruong)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Chỉ đội trưởng mới có thể mở", Util.WHITE));
							break;
						}
						StringBuilder stringBuilder = new StringBuilder();
						int CountCT = _myChar.InfoGame.Todoi.Chars.Count;
						for (int i = 0; i < _myChar.InfoGame.Todoi.Chars.Count; i++)
						{
							Character Char2 = _myChar.InfoGame.Todoi.Chars[i];
							if (!Char2.IsConnection)
							{
								continue;
							}
							if (Char2.InfoGame.IsVaoCamThuat)
							{
								_myChar.SendMessage(UtilMessage.SendThongBao("Người chơi " + Char2.Info.Name + " hiện đang trong cấm thuật", Util.WHITE));
								return;
							}
							if (Char2.TimeChar.SoLanDiCamThuat <= 0)
							{
								CountCT--;
								if (i > 0)
								{
									stringBuilder.Append(",");
								}
								stringBuilder.Append(_myChar.InfoGame.Todoi.Chars[i].Info.Name);
							}
						}
						string check = stringBuilder.ToString();
						if (CountCT != _myChar.InfoGame.Todoi.Chars.Count)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Người chơi " + check + " đã hết lượt tham gia cấm thuật", Util.WHITE));
							break;
						}
						lock (CamThuat._Lock)
						{
							new CamThuat(_myChar, _myChar.InfoGame.Todoi).Start();
							break;
						}
					}
					lock (CamThuat._Lock)
					{
						new CamThuat(_myChar, _myChar.InfoGame.Todoi).Start();
						break;
					}
				case 2:
					switch (Select)
					{
					case 0:
						_myChar.SendMessage(UtilMessage.SendTabTrangSharingan());
						_myChar.InfoGame.ID_Click_SHOP = 65;
						break;
					case 1:
						_myChar.SendMessage(UtilMessage.SendTabTrangRinnegan());
						_myChar.InfoGame.ID_Click_SHOP = 67;
						break;
					case 2:
						_myChar.SendMessage(UtilMessage.SendTabTrangByakugan());
						_myChar.InfoGame.ID_Click_SHOP = 66;
						break;
					}
					break;
				}
				break;
			}
		}

		public static void Read25(Character _myChar, Message msg)
		{
			short var = msg.ReadShort();
			sbyte var2 = msg.ReadByte();
			sbyte var3 = -1;
			while (msg.available() > 0)
			{
				var3 = msg.ReadByte();
			}
			if (!_myChar.InfoGame.IsUseItemNotNpc)
			{
				return;
			}
			if (_myChar.InfoGame.IsUseItemNotNpcOder2)
			{
				switch (var2)
				{
				case 0:
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.InfoGame.CleanUpUseItemNotNpc();
					_myChar.Info.MapId = 75;
					_myChar.JoinMap(-1, -1);
					break;
				case 1:
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.InfoGame.CleanUpUseItemNotNpc();
					_myChar.Info.MapId = 60;
					_myChar.Info.Cx = (short)Util.NextInt(475, 691);
					_myChar.Info.Cy = 431;
					_myChar.JoinMap(-1, -1);
					break;
				case 2:
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.InfoGame.CleanUpUseItemNotNpc();
					_myChar.Info.MapId = 69;
					_myChar.Info.Cx = (short)Util.NextInt(498, 668);
					_myChar.Info.Cy = 627;
					_myChar.JoinMap(-1, -1);
					break;
				case 3:
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.InfoGame.CleanUpUseItemNotNpc();
					_myChar.Info.MapId = 85;
					_myChar.Info.Cx = (short)Util.NextInt(782, 992);
					_myChar.Info.Cy = 562;
					_myChar.JoinMap(-1, -1);
					break;
				case 4:
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.InfoGame.CleanUpUseItemNotNpc();
					_myChar.Info.MapId = 59;
					_myChar.Info.Cx = (short)Util.NextInt(550, 774);
					_myChar.Info.Cy = 498;
					_myChar.JoinMap(-1, -1);
					break;
				case 5:
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.InfoGame.CleanUpUseItemNotNpc();
					_myChar.Info.MapId = 68;
					_myChar.Info.Cx = (short)Util.NextInt(1130, 1303);
					_myChar.Info.Cy = 416;
					_myChar.JoinMap(-1, -1);
					break;
				case 6:
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.InfoGame.CleanUpUseItemNotNpc();
					_myChar.Info.MapId = 102;
					_myChar.Info.Cx = (short)Util.NextInt(295, 502);
					_myChar.Info.Cy = 639;
					_myChar.JoinMap(-1, -1);
					break;
				}
				return;
			}
			switch (var2)
			{
			case 0:
				_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
				_myChar.InfoGame.CleanUpUseItemNotNpc();
				_myChar.Info.Cx = (short)Util.NextInt(491, 677);
				_myChar.Info.Cy = 644;
				_myChar.Info.MapId = 86;
				_myChar.JoinMap(-1, -1);
				break;
			case 1:
				_myChar.SendMessage(UtilMessage.OpenMenuNotNpc(_myChar.InfoGame.IndexItemNotNpc, oderPhu));
				_myChar.InfoGame.IsUseItemNotNpcOder2 = true;
				break;
			case 2:
				if (KhuRungChet.IsStartKhuRungChet)
				{
					if (KhuRungChet.IsHoatDong)
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Thời gian báo danh đã kết thúc", Util.YELLOW_MID));
						break;
					}
					_myChar.InfoGame.ZoneGame.RemoveChar(_myChar);
					_myChar.InfoGame.CleanUpUseItemNotNpc();
					_myChar.Info.Cx = (short)Util.NextInt(73, 222);
					_myChar.Info.Cy = 827;
					_myChar.Info.MapId = 2;
					_myChar.JoinMap(-1, -1);
					_myChar.SendMessage(UtilMessage.MsgUpdateHoatDong(IsLockNextmap: true));
					_myChar.SendMessage(UtilMessage.MsgUpdateTimeHoatDong(KhuRungChet.TimeStart, KhuRungChet.TimeHoatDong, KhuRungChet.IsHoatDong));
				}
				else
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Chưa đến thời gian diễn ra hoạt động", Util.YELLOW_MID));
				}
				break;
			case 3:
				_myChar.SendMessage(UtilMessage.SendThongBao("Đang hoàn thiện", Util.WHITE));
				break;
			case 4:
				_myChar.SendMessage(UtilMessage.SendThongBao("Đang hoàn thiện", Util.WHITE));
				break;
			}
		}
	}
}
