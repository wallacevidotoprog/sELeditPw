using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace sELedit.CORE.LOGSYSTEM
{
	public static class LogSistem
	{
		public static string dir = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "LOG";
		public static string fileLog = Path.Combine(dir, "LOGSISTEM.log");
		public static void InitLogger()
		{
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			LogWrite($"# => LOG ETERPHARMAPRO  {DateTime.Now} ");

		}

		public static void LogWrite(string msg)
		{
			if (!File.Exists(fileLog))
			{
				using (TextWriter tw = new StreamWriter(fileLog, false, Encoding.Default))
				{
					tw.Close();
				}
			}
			using (StreamWriter sw = File.AppendText(fileLog))
			{
				sw.WriteLine(msg);
			}
		}
		public static void LogWriteLog(TypeLog typeLog, string title, string msgErr, object dataErro)
		{

			InitLogger();

			string msg = string.Empty;
			//= $"=>\t{string.PadRight(typeLog.ToString(), 7)}" +
			//$"\t\t" +
			//$"{ExtensionsDefault.PadRight(title.ToUpper(), title.Length > 15 ? title.Length : 15)}" +
			//$"\t\t" +
			//$"{msgErr}" +
			//$"\n" +
			//$"{dataErro.ToString().Trim()}\n\n";
			LogWrite(msg);
		}

		public static List<string[]> LogGet()
		{
			List<string[]> tempLog = null;
			if (File.Exists(fileLog))
			{
				tempLog = new List<string[]>();
				using (StreamReader sr = new StreamReader(fileLog))
				{
					while (!sr.EndOfStream)
					{
						string temp = sr.ReadLine();
						if (temp != "" && temp != null)
						{
							tempLog.Add(temp.Split('-').Select(x => x.Trim().Replace("\t", null)).ToArray());
						}
					}
					sr.Close();
				}
			}

			return tempLog;
		}
	}

	public enum TypeLog
	{
		NONE, ALERT, INFO, WARNING, ERROR
	}
}
