using System;

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
	}
}
