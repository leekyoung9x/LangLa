using System;
using System.Collections.Concurrent;
using System.Linq;
using LangLa.IO;
using LangLa.OOP;
using LangLa.Template;

namespace LangLa.Manager
{
	public static class GiaTocManager
	{
		public static ConcurrentDictionary<short, GiaTocTemplate>? ListGiaTocs;

		public static int ID_GIA_TOC = 0;

		public static readonly int[] ExpGiaTocServer = new int[15]
		{
			0, 1500, 3000, 5000, 7000, 10000, 13000, 17000, 21000, 26000,
			31000, 37000, 43000, 50000, 57000
		};

		public static void SetGiaTocMe(Character _myChar)
		{
			Character _myChar2 = _myChar;
			try
			{
				_myChar2.InfoGame.GiaToc = ListGiaTocs.Values.FirstOrDefault((GiaTocTemplate s) => s.ID == _myChar2.Info.IdGiaToc);
			}
			catch (Exception e)
			{
				Util.ShowErr(e);
				_myChar2.Info.IdGiaToc = -1;
				_myChar2.Info.RoleGiaToc = -1;
			}
		}
	}
}
