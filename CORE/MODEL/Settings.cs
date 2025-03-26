using System;
using System.Reflection;

namespace sELedit.CORE.MODEL
{
	[Serializable]
	public class Settings
	{
		public string Version { get; set; }

		public string ElementsDataPath { get; set; }

		public string TasksDataPath { get; set; }

		public string SurfacesPckPath { get; set; }

		public string ConfigsPckPath { get; set; }

		public string GshopDataPath { get; set; }

		public string Gshop1DataPath { get; set; }

		public string ConfigPckPath { get; set; }

		public string SurfacePckPath { get; set; }






		public bool CheckFileExists(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName)) return false;

			Type type = this.GetType();

			PropertyInfo property = type.GetProperty(propertyName);

			if (property == null)
			{
				return false;
			}

			var value = property.GetValue(this) as string;

			if (string.IsNullOrEmpty(value))
			{
				return false;
			}

			return System.IO.File.Exists(value);
		}
	}


}
