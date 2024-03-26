using System.Text;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.OOP;

namespace LangLa.Hander
{
	public static class ThuHander
	{
		public static void Write(Message msg, InfoThu Thu)
		{
			msg.WriteShort(Thu.Id);
			msg.WriteBool(Thu.IsReceived);
			msg.WriteUTF(Thu.Title);
			msg.WriteUTF(Thu.NameSender);
			msg.WriteUTF(Thu.Content);
			msg.WriteInt(Thu.Bac);
			msg.WriteInt(Thu.BacKhoa);
			msg.WriteInt(Thu.Vang);
			msg.WriteInt(Thu.VangKhoa);
			msg.WriteLong(Thu.Exp);
			msg.WriteInt((int)(Util.CurrentTimeMillis() / 1000 + 60));
			if (Thu.Item == null)
			{
				msg.WriteShort(-1);
			}
			else
			{
				ItemHander.WriteItem(msg, Thu.Item);
			}
		}

		public static void ReloadThu(Character _myChar)
		{
			if (_myChar.Thus.Count >= 120)
			{
				return;
			}
			lock (_myChar.Thus)
			{
				Message i = new Message(97);
				i.WriteShort((short)_myChar.Thus.Count);
				foreach (InfoThu t in _myChar.Thus)
				{
					Write(i, t);
				}
				_myChar.SendMessage(i);
			}
		}

		public static void CreateThuNapThe(Character _myChar, int Vang)
		{
			InfoThu infoThu = new InfoThu();
			infoThu.Id = (short)_myChar.Thus.Count;
			infoThu.Bac = 0;
			infoThu.BacKhoa = 0;
			infoThu.Vang = Vang;
			infoThu.VangKhoa = 0;
			infoThu.Exp = 0L;
			infoThu.Title = "Hệ thống";
			infoThu.NameSender = "Vàng Nạp thẻ";
			infoThu.Content = "";
			infoThu.TimeEnd = 90000L;
			_myChar.Thus.Add(infoThu);
			ReloadThu(_myChar);
			_myChar.SendMessage(UtilMessage.SendThongBaoNhacNho("Nạp vàng thành công\n Mở hộp thư để nhận"));
		}

		public static void NhanThu(Character _myChar, Message msg, bool IsNhanAll = false)
		{
			InfoThu infoThu = null;
			lock (_myChar.Thus)
			{
				if (!IsNhanAll)
				{
					short IdThu = msg.ReadShort();
					if (IdThu >= _myChar.Thus.Count)
					{
						return;
					}
					infoThu = _myChar.Thus[IdThu];
					if (infoThu == null || infoThu.IsReceived)
					{
						return;
					}
					if (infoThu.Item != null)
					{
						if (InventoryHander.AddItemBag(_myChar, infoThu.Item))
						{
							infoThu.IsReceived = true;
							infoThu.Item = null;
						}
					}
					else
					{
						infoThu.IsReceived = true;
					}
					if (infoThu.Exp > 0)
					{
						_myChar.UpdateExp(infoThu.Exp);
						infoThu.Exp = 0L;
					}
					if (infoThu.Bac > 0)
					{
						InventoryHander.AddBac(_myChar, infoThu.Bac, ThongBao: true);
						infoThu.Bac = 0;
					}
					if (infoThu.Vang > 0)
					{
						InventoryHander.AddVang(_myChar, infoThu.Vang, ThongBao: true);
						infoThu.Vang = 0;
					}
					if (infoThu.VangKhoa > 0)
					{
						InventoryHander.AddVangKhoa(_myChar, infoThu.VangKhoa, ThongBao: true);
						infoThu.VangKhoa = 0;
					}
					if (infoThu.BacKhoa > 0)
					{
						InventoryHander.AddBacKhoa(_myChar, infoThu.BacKhoa, ThongBao: true);
						infoThu.BacKhoa = 0;
					}
					ReloadThu(_myChar);
					return;
				}
				StringBuilder stringBuilder = new StringBuilder();
				int i = 0;
				foreach (InfoThu t in _myChar.Thus)
				{
					if (t.IsReceived)
					{
						continue;
					}
					if (t.Item != null)
					{
						if (InventoryHander.AddItemBag(_myChar, t.Item))
						{
							t.IsReceived = true;
							t.Item = null;
						}
					}
					else
					{
						t.IsReceived = true;
					}
					if (t.Exp > 0)
					{
						_myChar.UpdateExp(t.Exp);
						t.Exp = 0L;
					}
					if (t.Bac > 0)
					{
						InventoryHander.AddBac(_myChar, t.Bac, ThongBao: true);
						t.Bac = 0;
					}
					if (t.BacKhoa > 0)
					{
						InventoryHander.AddBacKhoa(_myChar, t.BacKhoa, ThongBao: true);
						t.BacKhoa = 0;
					}
					if (t.Vang > 0)
					{
						InventoryHander.AddVang(_myChar, t.Vang, ThongBao: true);
						t.Vang = 0;
					}
					if (t.VangKhoa > 0)
					{
						InventoryHander.AddVangKhoa(_myChar, t.VangKhoa, ThongBao: true);
						t.VangKhoa = 0;
					}
					i++;
				}
				string reqw = stringBuilder.ToString();
				if (!reqw.Equals(""))
				{
					_myChar.SendMessage(UtilMessage.SendThongBao(reqw, Util.WHITE));
				}
				ReloadThu(_myChar);
			}
		}

		public static void RemoveThu(Character _myChar, Message msg)
		{
			lock (_myChar.Thus)
			{
				short SizeRemove = msg.ReadShort();
				short[] Index = new short[SizeRemove];
				for (int i = 0; i < Index.Length; i++)
				{
					Index[i] = msg.ReadShort();
				}
				for (int j = 0; j < Index.Length; j++)
				{
					for (int k = 0; k < _myChar.Thus.Count; k++)
					{
						InfoThu thu = _myChar.Thus[k];
						if (thu.Id == Index[j] && thu.Item == null && thu.IsReceived)
						{
							_myChar.Thus.RemoveAt(k);
						}
					}
				}
				ReloadThu(_myChar);
			}
		}
	}
}
