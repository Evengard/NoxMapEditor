using System;
using System.IO;
using Microsoft.Win32;
using MessageBox = System.Windows.Forms.MessageBox;

namespace NoxShared
{
	public class NoxDb
	{
		protected static string noxPath;
		protected static string dbFile;//set this before calling GetStream()

		protected static FileStream GetStream() {return File.OpenRead(noxPath + dbFile);}

		static NoxDb()
		{
			RegistryKey installPathKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Westwood\\Nox");
			if (installPathKey == null)
			{
				MessageBox.Show("Could not find Nox's install path in the registry. Reinstall Nox to fix this.", "Error");
				return;
			}
			noxPath = (string) installPathKey.GetValue("InstallPath");
			noxPath = noxPath.Substring(0, noxPath.LastIndexOf("\\")+1);
			string filePath = noxPath + "thing.bin";
			NoxBinaryReader rdr = null;
			try
			{
				rdr = new NoxBinaryReader(File.OpenRead(filePath), CryptApi.NoxCryptFormat.THING);
			}
			catch (FileNotFoundException)
			{
				MessageBox.Show(String.Format("Could not access database '{0}' in the Nox game directory.", dbFile), "Error");
				return;
			}
		}
	}
}
