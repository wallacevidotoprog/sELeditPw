using sELedit.CORE.BASE;
using sELedit.CORE.MODEL;
using System;
using System.IO;
using System.Xml.Serialization;

namespace sELedit.CORE.IO
{
	public static class ReadFile
	{
		private static string FileData = Path.Combine(Directory.GetCurrentDirectory(), "DataConfig");

		private static string FileSettings = Path.Combine(FileData, "Settings.xml");



		public static bool ReadWriteSettings(IOAction action)
		{
			try
			{
				switch (action)
				{
					case IOAction.Read:
						if (!File.Exists(FileSettings)) return false;

						XmlSerializer deserializer = new XmlSerializer(typeof(Settings));
						using (StreamReader reader = new StreamReader(FileSettings))
						{
							var obj = deserializer.Deserialize(reader);
							sELeditCache.Instance.Settings = (Settings)obj;
						}
						return true;
						break;
					case IOAction.Write:
						if (!File.Exists(FileData))
						{
							return false;
						}
						break;

				}

				return false;

			}
			catch (Exception)
			{
				return false;
			}
		}
	}

	public enum IOAction
	{
		Read,
		Write
	}
}
