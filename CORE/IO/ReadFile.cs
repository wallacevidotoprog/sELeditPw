using sELedit.CORE.BASE;
using sELedit.CORE.Extencion;
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
				XmlSerializer xmls = new XmlSerializer(typeof(Settings));
				switch (action)
				{
					case IOAction.Read:
						if (!File.Exists(FileSettings)) return false;

						using (StreamReader reader = new StreamReader(FileSettings))
						{
							var obj = xmls.Deserialize(reader);
							sELeditCache.Instance.Settings = obj as Settings ?? new Settings();
							//sELeditCache.Instance.Settings = (Settings)obj;
						}
						return true;
						break;
					case IOAction.Write:
						using (StreamWriter writer = new StreamWriter(FileSettings))
						{
							xmls.Serialize(writer, sELeditCache.Instance.Settings);
						}
						return true;
						break;

				}

				return false;

			}
			catch (Exception e)
			{
				e.ErrorGet(false);
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
