using System.Collections.Generic;
using System.Linq;
using LangLa.InfoChar;
using LangLa.IO;
using LangLa.Manager;
using LangLa.OOP;

namespace LangLa.Hander
{
	public static class TopHander
	{
		public static void ShowTop(Character _myChar, Message msg)
		{
			sbyte Index = msg.ReadByte();
			sbyte Index2 = msg.ReadByte();
			sbyte Index3 = -1;
			while (msg.available() > 0)
			{
				Index3 = msg.ReadByte();
			}
			switch (Index)
			{
			case 0:
				ShowTopPower(_myChar, Index2);
				break;
			case 2:
				ShowTopTaiPhu(_myChar, Index2);
				break;
			case 4:
				ShopTopGiaToc(_myChar);
				break;
			case 6:
				ShowTopNhiDong(_myChar);
				break;
			case 8:
				ShopTopCuongHoa(_myChar);
				break;
			case 9:
				ShowTopNapNhieu(_myChar);
				break;
			case 1:
			case 3:
			case 5:
			case 7:
				break;
			}
		}

		private static void ShowTopNapNhieu(Character _myChar)
		{
			Character _myChar2 = _myChar;
			List<InfoTop> infoTops = TopManager.InfoTopNapNhieu;
			lock (TopManager.InfoTopNapNhieu)
			{
				if (infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					sbyte Index = (sbyte)infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name));
					if (Index != -1)
					{
						infoTops[Index].NapNhieu = _myChar2.TimeChar.TongSoVangDaNap;
						infoTops = (TopManager.InfoTopNapNhieu = (from s in infoTops
							orderby s.NapNhieu descending, s.TimeNap descending
							select s).ToList());
					}
				}
				if (TopManager.InfoTopNapNhieu.Count < 100 && _myChar2.TimeChar.TongSoVangDaNap > 0 && !infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					InfoTop infoTop = new InfoTop();
					infoTop.IdClass = _myChar2.Info.IdClass;
					infoTop.Name = _myChar2.Info.Name;
					infoTop.NapNhieu = _myChar2.TimeChar.TongSoVangDaNap;
					infoTop.TimeNap = _myChar2.TimeChar.TimeNap;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop);
					infoTops = (TopManager.InfoTopNapNhieu = (from s in infoTops
						orderby s.NapNhieu descending, s.TimeNap descending
						select s).ToList());
				}
				else if (infoTops.Count == 100 && infoTops.FindIndex((InfoTop s) => s.NapNhieu < _myChar2.TimeChar.TongSoVangDaNap && !s.Name.Equals(_myChar2.Info.Name)) != -1)
				{
					infoTops.RemoveAt(infoTops.Count - 1);
					InfoTop infoTop2 = new InfoTop();
					infoTop2.IdClass = _myChar2.Info.IdClass;
					infoTop2.Name = _myChar2.Info.Name;
					infoTop2.NapNhieu = _myChar2.TimeChar.TongSoVangDaNap;
					infoTop2.TimeNap = _myChar2.TimeChar.TimeNap;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop2.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop2);
					infoTops = (TopManager.InfoTopNapNhieu = (from s in infoTops
						orderby s.NapNhieu descending, s.TimeNap descending
						select s).ToList());
				}
			}
			Message j = new Message(-22);
			j.WriteBool(x: true);
			j.WriteByte((sbyte)infoTops.Count);
			sbyte i = 0;
			foreach (InfoTop T in infoTops)
			{
				j.WriteByte(i);
				j.WriteUTF(T.Name);
				j.WriteShort(0);
				j.WriteLong(T.NapNhieu);
				j.WriteByte(T.IdClass);
				j.WriteUTF(T.NameGiaToc);
				i++;
			}
			_myChar2.SendMessage(j);
		}

		private static void ShowTopNhiDong(Character _myChar)
		{
			Character _myChar2 = _myChar;
			List<InfoTop> infoTops = TopManager.InfoTopNhiDong;
			lock (TopManager.InfoTopNhiDong)
			{
				if (infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					sbyte Index = (sbyte)infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name));
					if (Index != -1)
					{
						infoTops[Index].Level = _myChar2.Info.Level;
						infoTops[Index].Exp = _myChar2.Point.Exp;
						infoTops = (TopManager.InfoTopNhiDong = infoTops.OrderByDescending((InfoTop s) => s.Exp).ToList());
					}
				}
				if (TopManager.InfoTopNhiDong.Count < 100 && _myChar2.Info.Level > 0 && _myChar2.Info.Level <= 50 && !infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					InfoTop infoTop3 = new InfoTop();
					infoTop3.IdClass = _myChar2.Info.IdClass;
					infoTop3.Level = _myChar2.Info.Level;
					infoTop3.Name = _myChar2.Info.Name;
					infoTop3.Exp = _myChar2.Point.Exp;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop3.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop3);
					infoTops = (TopManager.InfoTopNhiDong = infoTops.OrderByDescending((InfoTop s) => s.Exp).ToList());
				}
				else if (infoTops.Count == 100 && infoTops.FindIndex((InfoTop s) => s.Level < _myChar2.Info.Level && !s.Name.Equals(_myChar2.Info.Name) && _myChar2.Info.Level <= 50) != -1)
				{
					infoTops.RemoveAt(infoTops.Count - 1);
					InfoTop infoTop2 = new InfoTop();
					infoTop2.IdClass = _myChar2.Info.IdClass;
					infoTop2.Level = _myChar2.Info.Level;
					infoTop2.Name = _myChar2.Info.Name;
					infoTop2.Exp = _myChar2.Point.Exp;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop2.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop2);
					infoTops = (TopManager.InfoTopNhiDong = infoTops.OrderByDescending((InfoTop s) => s.Exp).ToList());
				}
				if (infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)) && _myChar2.Info.Level > 50)
				{
					sbyte IndexRemove = (sbyte)infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name));
					if (IndexRemove != -1)
					{
						infoTops.RemoveAt(IndexRemove);
						List<InfoTop> infoTopset1 = TopManager.InfoTopLevels;
						if (infoTopset1.Count == 100)
						{
							infoTopset1.RemoveAt(infoTopset1.Count - 1);
						}
						InfoTop infoTop = new InfoTop();
						infoTop.IdClass = _myChar2.Info.IdClass;
						infoTop.Level = _myChar2.Info.Level;
						infoTop.Name = _myChar2.Info.Name;
						infoTop.Exp = _myChar2.Point.Exp;
						if (_myChar2.Info.IdGiaToc != -1)
						{
							infoTop.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
						}
						infoTopset1.Add(infoTop);
						infoTopset1 = infoTopset1.OrderByDescending((InfoTop s) => s.Exp).ToList();
						TopManager.InfoTopLevels = infoTopset1;
					}
				}
			}
			Message j = new Message(-22);
			j.WriteBool(x: true);
			j.WriteByte((sbyte)infoTops.Count);
			sbyte i = 0;
			foreach (InfoTop T in infoTops)
			{
				j.WriteByte(i);
				j.WriteUTF(T.Name);
				j.WriteShort(T.Level);
				j.WriteLong(0L);
				j.WriteByte(T.IdClass);
				j.WriteUTF(T.NameGiaToc);
				i++;
			}
			_myChar2.SendMessage(j);
		}

		private static void ShopTopGiaToc(Character _myChar)
		{
			Character _myChar2 = _myChar;
			List<InfoTop> infoTops = TopManager.InfoTopGiaToc;
			lock (TopManager.InfoTopGiaToc)
			{
				if (_myChar2.Info.IdGiaToc != -1 && infoTops.Any((InfoTop s) => s.NameGiaToc.Equals(_myChar2.InfoGame.GiaToc.Name)) && infoTops.Any((InfoTop s) => s.Level < _myChar2.InfoGame.GiaToc.Level))
				{
					sbyte IndexUpdate2 = (sbyte)infoTops.FindIndex((InfoTop s) => s.NameGiaToc.Equals(_myChar2.InfoGame.GiaToc.Name));
					infoTops[IndexUpdate2].LevelGiaToc = (sbyte)_myChar2.InfoGame.GiaToc.Level;
					infoTops[IndexUpdate2].ExpGiaToc = _myChar2.InfoGame.GiaToc.Exp;
					infoTops = (TopManager.InfoTopGiaToc = (from s in infoTops
						orderby s.LevelGiaToc descending, s.ExpGiaToc descending
						select s).ToList());
				}
				else if (_myChar2.Info.IdGiaToc != -1 && infoTops.Any((InfoTop s) => s.NameGiaToc.Equals(_myChar2.InfoGame.GiaToc.Name)) && infoTops.Any((InfoTop s) => s.Level < _myChar2.InfoGame.GiaToc.Level) && infoTops.Any((InfoTop s) => s.Exp < _myChar2.InfoGame.GiaToc.Exp))
				{
					sbyte IndexUpdate = (sbyte)infoTops.FindIndex((InfoTop s) => s.NameGiaToc.Equals(_myChar2.InfoGame.GiaToc.Name));
					infoTops[IndexUpdate].LevelGiaToc = (sbyte)_myChar2.InfoGame.GiaToc.Level;
					infoTops[IndexUpdate].ExpGiaToc = _myChar2.InfoGame.GiaToc.Exp;
					infoTops = (TopManager.InfoTopGiaToc = (from s in infoTops
						orderby s.LevelGiaToc descending, s.ExpGiaToc descending
						select s).ToList());
				}
				if (infoTops.Count < 100 && _myChar2.Info.IdGiaToc != -1 && !infoTops.Any((InfoTop s) => s.NameGiaToc.Equals(_myChar2.InfoGame.GiaToc.Name)))
				{
					InfoTop infoTop = new InfoTop();
					infoTop.Name = _myChar2.Info.Name;
					infoTop.LevelGiaToc = (sbyte)_myChar2.InfoGame.GiaToc.Level;
					infoTop.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					infoTop.ExpGiaToc = _myChar2.InfoGame.GiaToc.Exp;
					infoTop.ThanhVien = (sbyte)_myChar2.InfoGame.GiaToc.ThanhViens.Count;
					infoTop.NameTocTruong = _myChar2.InfoGame.GiaToc.NameTocTruong;
					infoTops.Add(infoTop);
					TopManager.InfoTopGiaToc = (from s in infoTops
						orderby s.LevelGiaToc descending, s.ExpGiaToc descending
						select s).ToList();
					infoTops = TopManager.InfoTopGiaToc;
				}
				else if (infoTops.Count == 100 && _myChar2.Info.IdGiaToc != -1 && infoTops.FindIndex((InfoTop s) => s.LevelGiaToc < _myChar2.InfoGame.GiaToc.Level && !s.NameGiaToc.Equals(_myChar2.InfoGame.GiaToc.Name)) != -1)
				{
					infoTops.RemoveAt(infoTops.Count - 1);
					InfoTop infoTop2 = new InfoTop();
					infoTop2.Name = _myChar2.Info.Name;
					infoTop2.LevelGiaToc = (sbyte)_myChar2.InfoGame.GiaToc.Level;
					infoTop2.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					infoTop2.ExpGiaToc = _myChar2.InfoGame.GiaToc.Exp;
					infoTop2.ThanhVien = (sbyte)_myChar2.InfoGame.GiaToc.ThanhViens.Count;
					infoTop2.NameTocTruong = _myChar2.InfoGame.GiaToc.NameTocTruong;
					infoTops.Add(infoTop2);
					TopManager.InfoTopGiaToc = (from s in infoTops
						orderby s.LevelGiaToc descending, s.ExpGiaToc descending
						select s).ToList();
					infoTops = TopManager.InfoTopGiaToc;
				}
			}
			Message j = new Message(-30);
			j.WriteBool(x: true);
			j.WriteByte((sbyte)infoTops.Count);
			sbyte i = 0;
			foreach (InfoTop T in infoTops)
			{
				int Exp = (int)((T.ExpGiaToc <= 0) ? 0.0 : (double.Parse(T.ExpGiaToc.ToString()) / (double)GiaTocManager.ExpGiaTocServer[T.LevelGiaToc] * 100.0));
				j.WriteUTF(T.NameTocTruong);
				j.WriteInt(T.LevelGiaToc);
				j.WriteInt(Exp);
				j.WriteInt(T.ThanhVien);
				j.WriteInt(30);
				j.WriteUTF(T.NameGiaToc);
				i++;
			}
			_myChar2.SendMessage(j);
		}

		private static void ShowTopTaiPhu(Character _myChar, sbyte Index2)
		{
			Character _myChar2 = _myChar;
			List<InfoTop> infoTops = null;
			infoTops = ((Index2 != 0) ? TopManager.InfoTopTaiPhus.Where((InfoTop s) => s.IdClass == Index2).ToList() : TopManager.InfoTopTaiPhus);
			lock (TopManager.InfoTopTaiPhus)
			{
				if (infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					sbyte Index3 = (sbyte)infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name) && s.TaiPhu < _myChar2.Info.TaiPhu);
					if (Index3 != -1)
					{
						infoTops[Index3].TaiPhu = _myChar2.Info.TaiPhu;
						infoTops = (TopManager.InfoTopTaiPhus = infoTops.OrderByDescending((InfoTop s) => s.TaiPhu).ToList());
					}
					else
					{
						infoTops.RemoveAt(infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)));
						TopManager.InfoTopTaiPhus = infoTops;
					}
				}
				if (TopManager.InfoTopTaiPhus.Count < 100 && _myChar2.Info.TaiPhu > 0 && !infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					InfoTop infoTop = new InfoTop();
					infoTop.IdClass = _myChar2.Info.IdClass;
					infoTop.Name = _myChar2.Info.Name;
					infoTop.TaiPhu = _myChar2.Info.TaiPhu;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop);
					infoTops = (TopManager.InfoTopTaiPhus = infoTops.OrderByDescending((InfoTop s) => s.TaiPhu).ToList());
				}
				else if (infoTops.Count == 100 && infoTops.FindIndex((InfoTop s) => s.TaiPhu < _myChar2.Info.TaiPhu && !s.Name.Equals(_myChar2.Info.Name)) != -1)
				{
					infoTops.RemoveAt(infoTops.Count - 1);
					InfoTop infoTop2 = new InfoTop();
					infoTop2.IdClass = _myChar2.Info.IdClass;
					infoTop2.Name = _myChar2.Info.Name;
					infoTop2.TaiPhu = _myChar2.Info.TaiPhu;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop2.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop2);
					infoTops = (TopManager.InfoTopTaiPhus = infoTops.OrderByDescending((InfoTop s) => s.TaiPhu).ToList());
				}
			}
			Message j = new Message(-22);
			j.WriteBool(x: true);
			j.WriteByte((sbyte)infoTops.Count);
			sbyte i = 0;
			foreach (InfoTop T in infoTops)
			{
				j.WriteByte(i);
				j.WriteUTF(T.Name);
				j.WriteShort(0);
				j.WriteLong(T.TaiPhu);
				j.WriteByte(T.IdClass);
				j.WriteUTF(T.NameGiaToc);
				i++;
			}
			_myChar2.SendMessage(j);
		}

		private static void ShowTopPower(Character _myChar, sbyte Index2)
		{
			Character _myChar2 = _myChar;
			List<InfoTop> infoTops = null;
			infoTops = ((Index2 != 0) ? TopManager.InfoTopLevels.Where((InfoTop s) => s.IdClass == Index2).ToList() : TopManager.InfoTopLevels);
			lock (TopManager.InfoTopLevels)
			{
				if (infoTops.FindIndex((InfoTop s) => s.Exp < _myChar2.Point.Exp) != -1 && infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)) != -1)
				{
					sbyte IndexAddNew = (sbyte)infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name));
					if (IndexAddNew != -1)
					{
						infoTops[IndexAddNew].Level = _myChar2.Info.Level;
						infoTops[IndexAddNew].Exp = _myChar2.Point.Exp;
						infoTops = (TopManager.InfoTopLevels = infoTops.OrderByDescending((InfoTop s) => s.Exp).ToList());
					}
				}
				if (TopManager.InfoTopLevels.Count < 100 && _myChar2.Info.Level > 50 && !infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					InfoTop infoTop = new InfoTop();
					infoTop.IdClass = _myChar2.Info.IdClass;
					infoTop.Level = _myChar2.Info.Level;
					infoTop.Name = _myChar2.Info.Name;
					infoTop.Exp = _myChar2.Point.Exp;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop);
					infoTops = (TopManager.InfoTopLevels = infoTops.OrderByDescending((InfoTop s) => s.Exp).ToList());
				}
				else if (infoTops.Count == 100 && infoTops.FindIndex((InfoTop s) => s.Level < _myChar2.Info.Level) != -1 && infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)) == -1)
				{
					infoTops.RemoveAt(infoTops.Count - 1);
					InfoTop infoTop2 = new InfoTop();
					infoTop2.IdClass = _myChar2.Info.IdClass;
					infoTop2.Level = _myChar2.Info.Level;
					infoTop2.Name = _myChar2.Info.Name;
					infoTop2.Exp = _myChar2.Point.Exp;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop2.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop2);
					infoTops = (TopManager.InfoTopLevels = infoTops.OrderByDescending((InfoTop s) => s.Exp).ToList());
				}
			}
			Message j = new Message(-22);
			j.WriteBool(x: true);
			j.WriteByte((sbyte)infoTops.Count);
			sbyte i = 0;
			foreach (InfoTop T in infoTops)
			{
				j.WriteByte(i);
				j.WriteUTF(T.Name);
				j.WriteShort(T.Level);
				j.WriteLong(0L);
				j.WriteByte(T.IdClass);
				j.WriteUTF(T.NameGiaToc);
				i++;
			}
			_myChar2.SendMessage(j);
		}

		private static void ShopTopCuongHoa(Character _myChar)
		{
			Character _myChar2 = _myChar;
			List<InfoTop> infoTops = TopManager.InfoTopCuongHoa;
			lock (TopManager.InfoTopCuongHoa)
			{
				if (infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					sbyte Index = (sbyte)infoTops.FindIndex((InfoTop s) => s.Name.Equals(_myChar2.Info.Name));
					if (Index != -1)
					{
						infoTops[Index].PointCuongHoa = _myChar2.Info.PointCuongHoa;
						infoTops = (TopManager.InfoTopCuongHoa = infoTops.OrderByDescending((InfoTop s) => s.PointCuongHoa).ToList());
					}
				}
				if (TopManager.InfoTopCuongHoa.Count < 100 && _myChar2.Info.PointCuongHoa > 0 && !infoTops.Any((InfoTop s) => s.Name.Equals(_myChar2.Info.Name)))
				{
					InfoTop infoTop = new InfoTop();
					infoTop.Name = _myChar2.Info.Name;
					infoTop.IdClass = _myChar2.Info.IdClass;
					infoTop.PointCuongHoa = _myChar2.Info.PointCuongHoa;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop);
					infoTops = (TopManager.InfoTopCuongHoa = infoTops.OrderByDescending((InfoTop s) => s.PointCuongHoa).ToList());
				}
				else if (infoTops.Count == 100 && infoTops.FindIndex((InfoTop s) => s.PointCuongHoa < _myChar2.Info.PointCuongHoa && !s.Name.Equals(_myChar2.Info.Name)) != -1)
				{
					infoTops.RemoveAt(infoTops.Count - 1);
					InfoTop infoTop2 = new InfoTop();
					infoTop2.Name = _myChar2.Info.Name;
					infoTop2.IdClass = _myChar2.Info.IdClass;
					infoTop2.PointCuongHoa = _myChar2.Info.PointCuongHoa;
					if (_myChar2.Info.IdGiaToc != -1)
					{
						infoTop2.NameGiaToc = _myChar2.InfoGame.GiaToc.Name;
					}
					infoTops.Add(infoTop2);
					infoTops = (TopManager.InfoTopCuongHoa = infoTops.OrderByDescending((InfoTop s) => s.PointCuongHoa).ToList());
				}
			}
			Message j = new Message(-22);
			j.WriteBool(x: true);
			j.WriteByte((sbyte)infoTops.Count);
			sbyte i = 0;
			foreach (InfoTop T in infoTops)
			{
				j.WriteByte(i);
				j.WriteUTF(T.Name);
				j.WriteShort(0);
				j.WriteLong(T.PointCuongHoa);
				j.WriteByte(T.IdClass);
				j.WriteUTF(T.NameGiaToc);
				i++;
			}
			_myChar2.SendMessage(j);
		}
	}
}
