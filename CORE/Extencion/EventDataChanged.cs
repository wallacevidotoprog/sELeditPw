using System;

namespace sELedit.CORE.Extencion
{
	public class EventDataChanged : EventArgs
	{
		public int ListIndex { get; set; }
		public int ElementIndex { get; set; }
		public int FieldIndex { get; set; }
		public string Value { get; set; }

		public string ListName { get; set; }
		public string ElementName { get; set; }
		public string FieldName { get; set; }

	}
}
