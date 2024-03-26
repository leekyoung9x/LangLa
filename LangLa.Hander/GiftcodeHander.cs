using System.Collections.Generic;
using System.Linq;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class GiftcodeHander
	{
		private static readonly object _Lock_Code = new object();

		public static void CheckGiftCode(Character _myChar, Message msg)
		{
			lock (_Lock_Code)
			{
				string Code = msg.ReadString();
				if (_myChar.GiftCode.GiftDaNhan.Any((string s) => s.Equals(Code)))
				{
					_myChar.SendMessage(UtilMessage.SendThongBao("Bạn đã nhận mã quà tặng này", Util.WHITE));
				}
				else
				{
					if (!_myChar.Info.IsActive)
					{
						return;
					}
					if (Code.Equals("langla2"))
					{
						if (InventoryHander.GetCountNotNullBag(_myChar) < 30)
						{
							_myChar.SendMessage(UtilMessage.SendThongBao("Cần trống ít nhất 30 ô hành trang", Util.YELLOW_MID));
							return;
						}
						_myChar.GiftCode.GiftDaNhan.Add(Code);
						_myChar.TimeChar.SoLanDiCamThuat++;
						Item item = new Item(428, IsLock: false);
						item.Quantity = 10000;
						item.IdClass = _myChar.Info.IdClass;
						InfoThu infoThu = new InfoThu(item);
						infoThu.Id = (short)_myChar.Thus.Count;
						infoThu.Bac = 0;
						infoThu.BacKhoa = 0;
						infoThu.Vang = 0;
						infoThu.VangKhoa = 0;
						infoThu.Exp = 0L;
						infoThu.Title = "Hệ thống";
						infoThu.NameSender = "LangLa2";
						infoThu.Content = "";
						infoThu.TimeEnd = 90000L;
						_myChar.Thus.Add(infoThu);
						ThuHander.ReloadThu(_myChar);
						_myChar.SendMessage(UtilMessage.ClearSceen());
						_myChar.SendMessage(UtilMessage.SendThongBao("Mã quà tặng hợp lệ,vui lòng mở hộp thư để nhận thưởng", Util.YELLOW_MID));
						CreateQuaKichHoatTest2(_myChar);
					}
					else
					{
						_myChar.SendMessage(UtilMessage.SendThongBao("Mã quà tặng không chính xác", Util.WHITE));
					}
				}
			}
		}

		public static void CreateQuaKichHoatTest2(Character _myChar)
		{
			if (InventoryHander.GetCountNotNullBag(_myChar) < 30)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Cần trống ít nhất 30 ô hành trang", Util.YELLOW_MID));
				return;
			}
			_myChar.TimeChar.IsNhanQuaKichHoat = true;
			List<Item> ItemKichHoat = new List<Item>();
			short[] IdItem = new short[19]
			{
				174, 175, 179, 216, 217, 218, 248, 278, 302, 315,
				329, 428, 134, 353, 563, 565, 567, 600, 724
			};
			for (int j = 0; j < IdItem.Length; j++)
			{
				Item item2 = new Item(IdItem[j], IsLock: false);
				if (IdItem[j] == 134)
				{
					item2.Quantity = 1;
					switch (_myChar.Info.IdClass)
					{
					case 1:
						item2.Options = "54,0,500;62,0,500";
						break;
					case 2:
						item2.Options = "55,0,500;58,0,500";
						break;
					case 3:
						item2.Options = "56,0,500;59,0,500";
						break;
					case 4:
						item2.Options = "57,0,500;60,0,500";
						break;
					case 5:
						item2.Options = "53,0,500;61,0,500";
						break;
					}
				}
				else if (item2.Id == 353 || item2.Id == 563 || item2.Id == 565 || item2.Id == 567)
				{
					item2.Quantity = 10000;
				}
				else if (item2.Id == 600 || item2.Id == 724)
				{
					item2.Quantity = 1;
				}
				else
				{
					item2.Quantity = 1000;
				}
				ItemKichHoat.Add(item2);
			}
			for (int i = 0; i < 10; i++)
			{
				Item item = new Item((short)(i + 417), IsLock: false);
				ItemKichHoat.Add(item);
			}
			InventoryHander.AddMultiItembag(_myChar, ItemKichHoat.ToArray());
			InfoThu infoThu = new InfoThu();
			infoThu.Id = (short)_myChar.Thus.Count;
			infoThu.Bac = int.MaxValue;
			infoThu.BacKhoa = int.MaxValue;
			infoThu.Vang = int.MaxValue;
			infoThu.VangKhoa = int.MaxValue;
			infoThu.Exp = 0L;
			infoThu.Title = "Hệ thống";
			infoThu.NameSender = "Quà trải nghiệm Làng lá private";
			infoThu.Content = "";
			infoThu.TimeEnd = 90000L;
			_myChar.Thus.Add(infoThu);
		}
	}
}
