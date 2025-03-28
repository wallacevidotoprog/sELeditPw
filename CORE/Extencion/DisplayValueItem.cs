namespace sELedit.CORE.Extencion
{
	public class DisplayValueItem
	{
		public string DisplayText { get; set; }
		public object RealValue { get; set; }

		public override string ToString()
		{
			return DisplayText;
		}
	}

	public class DisplayValueItemElem
	{
		public string DisplayText { get; set; }
		public object RealValue { get; set; }

		public override string ToString()
		{
			if (DisplayText.StartsWith("^"))
			{
				return DisplayText.Remove(0, 7);
			}
			return DisplayText;
		}
	}
}
