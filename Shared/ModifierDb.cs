using System;
using System.IO;
using System.Diagnostics;

namespace NoxShared
{
	public class ModifierDb : NoxDb
	{
		public static bool test;

		static ModifierDb()
		{
			Debug.WriteLine("Begin Read", "ModifierDbRead");
			Debug.Indent();

			dbFile = "modifier.bin";
			using (StreamReader rdr = new StreamReader(CryptApi.DecryptStream(GetStream(), CryptApi.NoxCryptFormat.MODIFIER)))
			{
				while (rdr.BaseStream.Position < rdr.BaseStream.Length)
				{
					string line = rdr.ReadLine();
					if (line == "WEAPON_DEFINITIONS")
						Debug.WriteLine("reading weapons");
					else if (line == "ARMOR_DEFINITIONS")
						Debug.WriteLine("reading armor");
					else if (line == "EFFECTIVENESS")
						Debug.WriteLine("reading effectiveness");
					else if (line == "MATERIAL")
						Debug.WriteLine("reading materials");
					else if (line == "ENCHANTMENT")
						Debug.WriteLine("reading enchantments");
				}
			}

			Debug.Unindent();
			Debug.WriteLine("End Read", "ModifierDbRead");
		}
	}
}
