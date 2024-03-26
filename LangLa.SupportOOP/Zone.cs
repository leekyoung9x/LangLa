using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using LangLa.Hander;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Server;

namespace LangLa.SupportOOP
{
	public class Zone
	{
		public sbyte Id;

		public ConcurrentDictionary<short, Mob> Mobs;

		public ConcurrentDictionary<int, Character> Chars;

		public ConcurrentDictionary<short, ItemMap> ItemMaps;

		public short ID_ITEM_MAP = 0;

		public long LastTimeRespawTinhAnh;

		public long LastTimeRespawThuLinh;

		public Zone(sbyte Id)
		{
			this.Id = Id;
			Mobs = new ConcurrentDictionary<short, Mob>();
			Chars = new ConcurrentDictionary<int, Character>();
			ItemMaps = new ConcurrentDictionary<short, ItemMap>();
		}

		public void Update()
		{
			if (Chars.Values.Count > 0)
			{
				Task.WhenAll(UpdateChar(), UpdateMob(), UpdateItemMap());
			}
		}

		public async Task UpdateChar()
		{
			Parallel.ForEach(Chars.Values, delegate(Character Char)
			{
				Char.Update();
			});
			await Task.Delay(5);
		}

		public async Task UpdateMob()
		{
			Parallel.ForEach(Mobs.Values, delegate(Mob mob)
			{
				mob.Update();
			});
			await Task.Delay(5);
		}

		public async Task UpdateItemMap()
		{
			await Task.Delay(5);
		}

		public void AddItemMap(ItemMap itemMap)
		{
			if (!ItemMaps.TryAdd(itemMap.Id, itemMap))
			{
				return;
			}
			foreach (Character Char in Chars.Values)
			{
				if (Char.IsConnection)
				{
					Char.SendMessage(UtilMessage.AddItemMap(itemMap));
				}
			}
		}

		public void AddChar(Character _myChar)
		{
			if (!Chars.TryAdd(_myChar.Info.IdUser, _myChar))
			{
				return;
			}
			foreach (Character _myChar2 in Chars.Values)
			{
				if (_myChar2.Info.IdUser != _myChar.Info.IdUser && _myChar2.IsConnection)
				{
					Message i = new Message(-102);
					i.WriteInt(_myChar.Info.IdUser);
					_myChar.Write(i);
					_myChar2.SendMessage(i);
					if (_myChar.Info.IdGiaToc != -1 && _myChar.InfoGame.GiaToc != null)
					{
						_myChar2.SendMessage(GiaTocHander.SendGiaTocMsg(_myChar));
					}
				}
			}
		}

		public void UpdateXyChar(Character _myChar, bool when_move)
		{
			short Cx = _myChar.Info.Cx;
			short Cy = _myChar.Info.Cy;
			int ID = _myChar.Info.IdUser;
			foreach (Character _myChar2 in Chars.Values)
			{
				if (_myChar2.IsConnection)
				{
					_myChar2.SendMessage(UtilMessage.SendXYChar(ID, Cx, Cy, when_move));
				}
			}
		}

		public void SetXyChar(Character _myChar, bool when_move)
		{
			short Cx = _myChar.Info.Cx;
			short Cy = _myChar.Info.Cy;
			int ID = _myChar.Info.IdUser;
			foreach (Character _myChar2 in Chars.Values)
			{
				if (_myChar2.IsConnection)
				{
					_myChar2.SendMessage(UtilMessage.MsgSetXy(ID, Cx, Cy));
				}
			}
		}

		public void RemoveChar(Character _myChar)
		{
			Character _myChar2 = _myChar;
			if (_myChar2.InfoGame.IsCuuSat)
			{
				Character cAnCuuSat2 = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == _myChar2.InfoGame.IdCuuSat);
				if (cAnCuuSat2 != null)
				{
					cAnCuuSat2.SendMessage(PvPHander.MsgCloseCuuSat(_myChar2.Info.IdUser, IsCloseCuuSat: true));
					cAnCuuSat2.InfoGame.CleanUpCuuSat();
				}
				_myChar2.InfoGame.CleanUpCuuSat();
				_myChar2.SendMessage(PvPHander.MsgCloseCuuSat(_myChar2.Info.IdUser, IsCloseCuuSat: true));
			}
			if (_myChar2.InfoGame.IsAnCuuSat)
			{
				Character cAnCuuSat = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == _myChar2.InfoGame.IdCharMoiCuuSat);
				if (cAnCuuSat != null)
				{
					cAnCuuSat.SendMessage(PvPHander.MsgCloseCuuSat(cAnCuuSat.Info.IdUser, IsCloseCuuSat: true));
					cAnCuuSat.InfoGame.CleanUpCuuSat();
				}
				_myChar2.InfoGame.CleanUpCuuSat();
				_myChar2.SendMessage(PvPHander.MsgCloseCuuSat(_myChar2.InfoGame.IdCharMoiCuuSat, IsCloseCuuSat: true));
			}
			if (_myChar2.InfoGame.IsTyVo)
			{
				Character cTyVo = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == _myChar2.InfoGame.IdTyVo);
				if (cTyVo != null)
				{
					foreach (Character c2 in cTyVo.InfoGame.ZoneGame.Chars.Values)
					{
						if (c2.IsConnection)
						{
							c2.SendMessage(PvPHander.MsgCloseTyVo(2, cTyVo.Info.IdUser, _myChar2.Info.IdUser));
						}
					}
					LangLa.Server.Server.SendThongBaoFromServer("Nhẫn giả " + _myChar2.Info.Name + " đã bỏ chạy khi tỷ võ với " + cTyVo.Info.Name);
					cTyVo.InfoGame.CleanUpTyvo();
				}
				_myChar2.InfoGame.CleanUpTyvo();
			}
			if (_myChar2.InfoGame.StatusGD == 1)
			{
				Character _cGiaoDich = _myChar2.InfoGame.ZoneGame.Chars.Values.FirstOrDefault((Character s) => s.Info.IdUser == _myChar2.InfoGame.IdCharGiaoDich);
				if (_cGiaoDich != null)
				{
					_cGiaoDich.SendMessage(InventoryHander.MsgClearGD());
					_cGiaoDich.SendMessage(UtilMessage.SendThongBao("Giao dịch thất bại", Util.YELLOW_MID));
					_cGiaoDich.InfoGame.CleanUpGD(null);
					short Speed = _cGiaoDich.TuongKhac.GetSpeedChar(_cGiaoDich);
					foreach (Character c in _cGiaoDich.InfoGame.ZoneGame.Chars.Values)
					{
						if (c.IsConnection)
						{
							c.SendMessage(UtilMessage.UpdatePointMore(_cGiaoDich.Info.IdUser, 0, _cGiaoDich.Info.TaiPhu, Speed, 0));
						}
					}
				}
				_myChar2.InfoGame.CleanUpGD(null);
			}
			if (!Chars.TryRemove(_myChar2.Info.IdUser, out _myChar2))
			{
				return;
			}
			if (_myChar2.Info.MapId == 89)
			{
				_myChar2.InfoGame.IsVaoCamThuat = false;
				_myChar2.InfoGame.CamThuat = null;
			}
			foreach (Character _myChar3 in Chars.Values)
			{
				if (_myChar3.IsConnection)
				{
					Message i = new Message(-101);
					i.WriteInt(_myChar2.Info.IdUser);
					_myChar3.SendMessage(i);
				}
			}
		}
	}
}
