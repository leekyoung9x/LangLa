namespace LangLa.SupportOOP
{
	public class WayPoint
	{
		public short MapHere;

		public short MapNext;

		public short l;

		public short m;

		public short n;

		public short o;

		public bool IsNext;

		public short Cx;

		public short Cy;

		public int p = 0;

		public int q = 0;

		public void Create(short s1, short s2, short s3, short s4, short s5, short s6, short s7, short s8)
		{
			MapHere = s1;
			MapNext = s2;
			l = (short)(s3 - 5);
			m = (short)(s4 - 5);
			n = (short)(s5 + 5);
			o = (short)(s6 + 5);
			Cx = (short)(s3 + (s5 - s3) / 2);
			Cy = s6;
			p = s7;
			q = s8;
		}
	}
}
