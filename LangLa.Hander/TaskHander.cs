using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;
using LangLa.Template;

namespace LangLa.Hander
{
	public static class TaskHander
	{
		public static void AddItemTask(Character _myChar)
		{
			if (_myChar.Info.Level >= _myChar.Task.LevelNeed && _myChar.Task.Id == 6 && _myChar.Task.IdStep != -1 && _myChar.Task.IdStep <= 2)
			{
				TaskTempalte.StepTask taskTempalte = DataServer.ArrTaskTemplate[_myChar.Task.Id].TaskStep[_myChar.Task.IdStep];
				if ((_myChar.Task.IdStep == 0 || _myChar.Task.IdStep == 1 || _myChar.Task.IdStep == 2) && _myChar.Info.MapId == 83 && _myChar.Info.Cx == taskTempalte.Cx && _myChar.Info.Cy == taskTempalte.Cy)
				{
					InventoryHander.AddItemBag(_myChar, new Item(380, IsLock: true));
				}
			}
		}

		public static void CheckUseItem(Character _myChar, Item item)
		{
			Character _myChar2 = _myChar;
			switch (_myChar2.Task.Id)
			{
				case 0:
					if (_myChar2.Task.IdStep == 0 && item.Id == 28)
					{
						NextStep(_myChar2);
					}
					break;
				case 3:
					if (_myChar2.Task.IdStep == 1 && item.Id == 22)
					{
						NextStep(_myChar2);
					}
					break;
				case 8:
					if (_myChar2.Task.IdStep == 9 && item.Id == DataServer.ArrItemTemplate.FirstOrDefault(s => s.id_class == _myChar2.Info.IdClass && s.type == 1 && s.level_need == 10).id)
					{
						NextStep(_myChar2);
					}
					break;
			}
		}

		public static void CheckBuyItem(Character _myChar, Item it)
		{
			if (_myChar.Info.Level >= _myChar.Task.LevelNeed)
			{
				short id = _myChar.Task.Id;
				short num = id;
				if (num == 3 && _myChar.Task.IdStep == 0 && it.Id == 22)
				{
					NextStep(_myChar);
				}
			}
		}

		public static void CheckHaiVatPhat(Character _myChar, Message msg)
		{
			Character _myChar2 = _myChar;
			short ID = msg.ReadShort();
			if (ID >= DataServer.ArrMobTempalte.Length)
			{
				return;
			}
			long time = DataServer.ArrMobTempalte[ID].TimeThuHoac + Util.CurrentTimeMillis();
			bool IsFale = true;
			short id = _myChar2.Task.Id;
			short num = id;
			if (num == 2 && _myChar2.Task.IdStep == 0)
			{
				new Task(delegate
				{
					while (IsFale)
					{
						if (time < Util.CurrentTimeMillis())
						{
							if (_myChar2.InfoGame != null)
							{
								Message message3 = new Message(7);
								message3.WriteInt(_myChar2.Info.IdUser);
								_myChar2.SendMessage(message3);
								InventoryHander.AddItemBag(_myChar2, new Item(194, IsLock: true));
								if (_myChar2.Task.IdRequire == 2)
								{
									NextStep(_myChar2);
									break;
								}
								UpCountTask(_myChar2);
							}
							IsFale = false;
							break;
						}
						Thread.Sleep(10);
					}
				}).Start();
			}
			if (_myChar2.Info.MapId == 47 && ID >= 60)
			{
				if (_myChar2.InfoGame.IsUseBiDuoc)
				{
					_myChar2.SendMessage(UtilMessage.ClearSceen());
					return;
				}
				bool IsAddEffBiDuoc = Util.NextInt(0, 10) > 1;
				new Task(delegate
				{
					while (IsFale)
					{
						if (time < Util.CurrentTimeMillis())
						{
							if (_myChar2.IsConnection)
							{
								Message message2 = new Message(7);
								message2.WriteInt(_myChar2.Info.IdUser);
								_myChar2.SendMessage(message2);
								if (IsAddEffBiDuoc)
								{
									InfoEff item = new InfoEff(49, 300000, -1, -1)
									{
										IsAutoRemove = true
									};
									_myChar2.InfoGame.IsUseBiDuoc = true;
									_myChar2.SendMessage(UtilMessage.MsgSendEff(_myChar2.Info.IdUser, 49, 0, 300000));
									_myChar2.Effs.Add(item);
								}
							}
							break;
						}
						Thread.Sleep(10);
					}
				}).Start();
				return;
			}
			new Task(delegate
			{
				while (IsFale)
				{
					if (time < Util.CurrentTimeMillis())
					{
						if (_myChar2.IsConnection)
						{
							Message message = new Message(7);
							message.WriteInt(_myChar2.Info.IdUser);
							_myChar2.SendMessage(message);
							InventoryHander.AddItemBag(_myChar2, new Item(194, IsLock: true));
						}
						break;
					}
					Thread.Sleep(10);
				}
			}).Start();
		}

		public static void CheckClickDoneTask(Character _myChar, Message msg)
		{
			sbyte var1 = -1;
			while (msg.available() > 0)
			{
				var1 = msg.ReadByte();
				short[] var2 = new short[var1];
				for (int i = 0; i < var1; i++)
				{
					var2[i] = msg.ReadShort();
				}
			}
			if (_myChar.Info.Level < _myChar.Task.LevelNeed || !NpcHander.CheckRangeClickNpc(_myChar, DataServer.ArrTaskTemplate[_myChar.Task.Id].IdNpc))
			{
				return;
			}
			switch (_myChar.Task.Id)
			{
				case 0:
					if (_myChar.Task.IdStep == 3)
					{
						NextTask(_myChar);
					}
					break;
				case 1:
					if (_myChar.Task.IdStep == 1)
					{
						if (var1 != -1)
						{
							_myChar.SendMessage(UtilMessage.ClearSceen());
							InventoryHander.SetNullItemBag(_myChar, 378);
							NextTask(_myChar);
						}
						else
						{
							ClickEventHander.ShowGiaoVatPhat(_myChar);
						}
					}
					break;
				case 2:
					if (_myChar.Task.IdStep == 1)
					{
						if (var1 != -1)
						{
							_myChar.SendMessage(UtilMessage.ClearSceen());
							InventoryHander.SetNullItemBag(_myChar, 194);
							NextTask(_myChar);
						}
						else
						{
							ClickEventHander.ShowGiaoVatPhat(_myChar);
						}
					}
					break;
				case 3:
					if (_myChar.Task.IdStep == 4)
					{
						if (var1 != -1)
						{
							_myChar.SendMessage(UtilMessage.ClearSceen());
							InventoryHander.SetNullItemBag(_myChar, 379);
							NextTask(_myChar);
						}
						else
						{
							ClickEventHander.ShowGiaoVatPhat(_myChar);
						}
					}
					break;
				case 4:
					if (_myChar.Task.IdStep == 2)
					{
						NextTask(_myChar);
					}
					break;
				case 5:
					if (_myChar.Task.IdStep == 1)
					{
						NextTask(_myChar);
					}
					break;
				case 6:
					if (_myChar.Task.IdStep == 3)
					{
						NextTask(_myChar);
					}
					break;
				case 7:
					if (_myChar.Task.IdStep == 1)
					{
						NextTask(_myChar);
					}
					break;
				case 8:
					if (_myChar.Task.IdStep == 12)
					{
						NextTask(_myChar);
					}
					break;
				case 9:
					if (_myChar.Task.IdStep == 11)
					{
						NextTask(_myChar);
					}
					break;
				case 10:
					if (_myChar.Task.IdStep == 1)
					{
						NextTask(_myChar);
					}
					break;
				case 11:
					if (_myChar.Task.IdStep == 3)
					{
						NextTask(_myChar);
					}
					break;
				case 12:
					if (_myChar.Task.IdStep == 3)
					{
						NextTask(_myChar);
					}
					break;
				case 13:
					if (_myChar.Task.IdStep == 7)
					{
						NextTask(_myChar);
					}
					break;
			}
		}

		public static void CheckClickNhanNhiemVu(Character _myChar)
		{
			short IdNpc = DataServer.ArrTaskTemplate[_myChar.Task.Id].IdNpc;
			if (_myChar.Task.IdStep >= 0)
			{
				IdNpc = DataServer.ArrTaskTemplate[_myChar.Task.Id].TaskStep[_myChar.Task.IdStep].IdNpc;
			}
			if (NpcHander.CheckRangeClickNpc(_myChar, IdNpc))
			{
				NextStep(_myChar);
			}
		}

		public static void CheckDoneTaskNextMap(Character _myChar, short MapNext)
		{
			if (_myChar.Info.Level >= _myChar.Task.LevelNeed)
			{
				short id = _myChar.Task.Id;
				short num = id;
				if (num == 8 && _myChar.Task.IdStep == 2 && MapNext == 86)
				{
					NextStep(_myChar);
				}
			}
		}

		public static void CheckDoneTaskKillMob(Character _myChar, short ID_MOB)
		{
			if (_myChar.Info.Level < _myChar.Task.LevelNeed)
			{
				return;
			}
			switch (_myChar.Task.Id)
			{
				case 0:
					{
						sbyte idStep = _myChar.Task.IdStep;
						sbyte b = idStep;
						if (b == 2 && ID_MOB == 153)
						{
							if (_myChar.Task.IdRequire >= 4)
							{
								NextStep(_myChar);
							}
							else
							{
								UpCountTask(_myChar);
							}
						}
						break;
					}
				case 1:
					if (_myChar.Task.IdStep == 0 && ID_MOB == DataServer.ArrTaskTemplate[_myChar.Task.Id].TaskStep[_myChar.Task.IdStep].IdMob)
					{
						if (_myChar.Task.IdRequire >= 9)
						{
							NextStep(_myChar);
						}
						else if (Util.NextInt(0, 2) == 1)
						{
							Item item2 = new Item(378, IsLock: true);
							InventoryHander.AddItemBag(_myChar, item2);
							UpCountTask(_myChar);
						}
					}
					break;
				case 3:
					if (_myChar.Task.IdStep == 3 && ID_MOB == DataServer.ArrTaskTemplate[_myChar.Task.Id].TaskStep[_myChar.Task.IdStep].IdMob)
					{
						if (_myChar.Task.IdRequire >= DataServer.ArrTaskTemplate[_myChar.Task.Id].TaskStep[_myChar.Task.IdStep].MaxRequire - 1)
						{
							NextStep(_myChar);
						}
						else if (Util.NextInt(0, 5) == 1)
						{
							Item item = new Item(379, IsLock: true);
							InventoryHander.AddItemBag(_myChar, item);
							UpCountTask(_myChar);
						}
					}
					break;
				case 5:
					if (_myChar.Task.IdStep == 0 && ID_MOB == DataServer.ArrTaskTemplate[_myChar.Task.Id].TaskStep[_myChar.Task.IdStep].IdMob)
					{
						if (_myChar.Task.IdRequire >= 9)
						{
							NextStep(_myChar);
						}
						else
						{
							UpCountTask(_myChar);
						}
					}
					break;
				case 7:
					if (_myChar.Task.IdStep == 0 && ID_MOB == DataServer.ArrTaskTemplate[_myChar.Task.Id].TaskStep[_myChar.Task.IdStep].IdMob)
					{
						if (_myChar.Task.IdRequire >= 19)
						{
							NextStep(_myChar);
						}
						else
						{
							UpCountTask(_myChar);
						}
					}
					break;
				case 13:
					if (_myChar.Task.IdStep == 6 && ID_MOB == DataServer.ArrTaskTemplate[_myChar.Task.Id].TaskStep[_myChar.Task.IdStep].IdMob)
					{
						if (_myChar.Task.IdRequire >= 9)
						{
							NextStep(_myChar);
						}
						else
						{
							UpCountTask(_myChar);
						}
					}
					break;
			}
		}

		public static void ShowTrongTruyenTask(Character _myChar, short steep = -1)
		{
			sbyte Steep = (sbyte)((_myChar.Task.IdStep == -1) ? (-1) : (-2));
			if (steep != -1)
			{
				Steep = (sbyte)steep;
			}
			ShowTrongTruyenTask(_myChar, Steep);
		}

		public static void ShowTrongTruyenTask(Character _myChar, sbyte Steep)
		{
			Message i = new Message(12);
			i.WriteByte(Steep);
			_myChar.SendMessage(i);
		}

		public static void UpCountTask(Character _myChar)
		{
			_myChar.Task.IdRequire++;
			Message i = new Message(103);
			i.WriteShort(_myChar.Task.Id);
			i.WriteByte(_myChar.Task.IdStep);
			i.WriteUShort(_myChar.Task.IdRequire);
			_myChar.SendMessage(i);
		}

		public static void NextStep(Character _myChar)
		{
			Character _myChar2 = _myChar;
			if (_myChar2.Task.Id == 8 && _myChar2.Task.IdStep == 8)
			{
				Item it = new Item(DataServer.ArrItemTemplate.FirstOrDefault(s => s.id_class == _myChar2.Info.IdClass && s.type == 1 && s.level_need == 10).id, IsLock: true, SetOptionAuto: false);
				it.IdClass = _myChar2.Info.IdClass;
				it.AddOption(2, 100);
				it.AddOption(3, 30);
				it.AddOption(20, 30);
				it.AddOption(22, 20);
				it.AddOption(48, 5);
				InventoryHander.AddItemBag(_myChar2, it);
			}
			_myChar2.Task.IdStep++;
			Message i = new Message(103);
			i.WriteShort(_myChar2.Task.Id);
			i.WriteByte(_myChar2.Task.IdStep);
			i.WriteUShort(_myChar2.Task.IdRequire);
			_myChar2.SendMessage(i);
		}

		public static void NextTask(Character _myChar)
		{
			TaskTempalte DataTask = DataServer.ArrTaskTemplate[_myChar.Task.Id];
			if (DataTask.Bac > 0)
			{
				InventoryHander.AddBac(_myChar, DataTask.Bac);
			}
			if (DataTask.BacKhoa > 0)
			{
				InventoryHander.AddBacKhoa(_myChar, DataTask.BacKhoa, ThongBao: true);
			}
			if (DataTask.Exp > 0)
			{
				_myChar.UpdateExp(DataTask.Exp);
			}
			if (DataTask.StrItem.Length > 0)
			{
				InventoryHander.AddItemBag(_myChar, DataTask.GetItemTask(_myChar.Info.IdClass, _myChar.Info.GioiTinh));
			}
			_myChar.Task.IdRequire = 0;
			_myChar.Task.IdStep = -1;
			_myChar.Task.Id++;
			_myChar.Task.LevelNeed = (sbyte)DataServer.ArrTaskTemplate[_myChar.Task.Id].LevelNeed;
			Message i = new Message(103);
			i.WriteShort(_myChar.Task.Id);
			i.WriteByte(_myChar.Task.IdStep);
			i.WriteUShort(_myChar.Task.IdRequire);
			_myChar.SendMessage(i);
		}
	}
}
