using LangLa.Data;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;
using LangLa.SupportOOP;

namespace LangLa.Hander
{
	public static class ViThuHander
	{
		public static bool UseSkillViThu(Character _myChar, Item item)
		{
			Item bodyVT = _myChar.Inventory.ItemBody[10];
			if (bodyVT == null)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Cần trang bị vĩ thú", Util.YELLOW_MID));
				return false;
			}
			if (bodyVT.Id >= 476 && bodyVT.Id <= 484)
			{
				if (_myChar.ViThu.VyThus.Count < _myChar.ViThu.MaxCoutViThu)
				{
					sbyte IdRd = (sbyte)Util.NextInt(13, 19);
					string OptionAdd = DataServer.ArrSkillViThu[IdRd - 13].Options;
					InfoViThu.ViThu viThu = new InfoViThu.ViThu();
					viThu.Id = IdRd;
					viThu.Options = OptionAdd;
					_myChar.ViThu.VyThus.Add(viThu);
					ReLoadSkillViThu(_myChar);
					_myChar.SendMessage(UtilMessage.SendThongBao("Học thành công skill vĩ thú " + DataServer.ArrSkillViThu[IdRd - 13].Name, Util.YELLOW_MID));
					return true;
				}
				_myChar.SendMessage(UtilMessage.SendThongBao("Cần mở rộng ô vĩ thú", Util.YELLOW_MID));
				return false;
			}
			_myChar.SendMessage(UtilMessage.SendThongBao("Cần trang bị vĩ thú", Util.YELLOW_MID));
			return false;
		}

		public static void DownPointFromItemViThu(Character _myChar)
		{
			foreach (InfoViThu.ViThu V in _myChar.ViThu.VyThus)
			{
				_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: true, V.Options);
			}
		}

		public static void UpPointFromItemViThu(Character _myChar)
		{
			foreach (InfoViThu.ViThu V in _myChar.ViThu.VyThus)
			{
				_myChar.TuongKhac.GetPointFromItem(null, _myChar, IsDownPoint: false, V.Options);
			}
		}

		public static void RemoveSkillViThu(Character _myChar, Message msg)
		{
			sbyte Index = msg.ReadByte();
			bool IsDungVang = msg.ReadBool();
		}

		public static void UpSkillViThu(Character _myChar, Message msg)
		{
			sbyte Index = msg.ReadByte();
			bool IsDungVang = msg.ReadBool();
		}

		public static void MoRongSkillViThu(Character _myChar, Message msg)
		{
			sbyte Var1 = msg.ReadByte();
			if (_myChar.Inventory.Vang < 2000)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("Không đủ vàng", Util.WHITE));
				return;
			}
			if (_myChar.ViThu.MaxCoutViThu == 6)
			{
				_myChar.SendMessage(UtilMessage.SendThongBao("đã mở rộng tối đa 6 ô skill", Util.WHITE));
				return;
			}
			_myChar.ViThu.MaxCoutViThu = 6;
			_myChar.SendMessage(UtilMessage.SendThongBao("mở rộng thành công 5 ô skill", Util.WHITE));
			InventoryHander.UpdateVang(_myChar, 2000, IsThongBao: true);
		}

		private static void ReLoadSkillViThu(Character _myChar)
		{
			Message i = UtilMessage.Message123();
			i.WriteByte(-29);
			_myChar.WriteSkillViThu(i);
			_myChar.SendMessage(i);
		}
	}
}
