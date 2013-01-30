using System;
using System.Collections;
using System.IO;
using System.Text;

namespace NoxShared
{
	public class AppConsole
	{
		public class ConsoleEventArgs : EventArgs
		{
			public string Line;
			public ConsoleEventArgs(string line)
			{
				Line = line;
			}
		}
		public delegate void ConsoleEvent(object sender, ConsoleEventArgs e);
		public static event ConsoleEvent LineWritten;

		//private static Stack stack = new Stack();
		private static StreamWriter log = new StreamWriter("ConsoleLog.txt", true, Encoding.UTF8);

		public static bool TimeStamp = true;
		public static bool Echo = true;

		public static void WriteLine(string format, params object[] args)
		{
			string timestamp = String.Format("[{0:yyyy-MM-dd HH:mm:ss}] ", DateTime.Now);
			string line = (TimeStamp ? timestamp : "") + String.Format(format, args);
			//stack.Push(line);
			log.WriteLine(line);
			log.Flush();
			if (Echo) System.Console.WriteLine(line);
			if (LineWritten != null) LineWritten(null, new ConsoleEventArgs(line));
		}
/*
		/// <summary>
		/// returns the last N lines
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>
		public static string[] GetLines(int lines)
		{
			ArrayList list = new ArrayList();
			foreach (string line in stack)
				if (lines-- > 0)
					list.Add(line);
			return (string[]) list.ToArray(typeof(string));
		}

		/// <summary>
		/// Retrieve the specified lines from the history
		/// </summary>
		/// <param name="start">The index back to start from</param>
		/// <param name="lines">How many lines to retrieve</param>
		/// <returns></returns>
		public static string[] GetLines(int start, int lines)
		{
			ArrayList list = new ArrayList();
			foreach (string line in stack)
				if (start-- <= 0 && lines-- > 0)
					list.Add(line);
			return (string[]) list.ToArray(typeof(string));
		}

		public static int LineCount
		{
			get
			{
				return stack.Count;
			}
		}
*/
	}
}
