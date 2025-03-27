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
}
