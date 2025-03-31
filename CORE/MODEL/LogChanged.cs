using sELedit.CORE.BASE;
using System;
using System.Collections.Generic;

namespace sELedit.CORE.MODEL
{
	public class LogChanged
	{
		public event EventHandler LogChangedEvent;
		public List<LogChangedValues> LogChangedValues { get; set; } = new List<LogChangedValues>();

		public void Add(LogChangedType Type, int ListIndex, int ElementIndex, int FieldIndex, string Value)
		{
			LogChangedValues.Add(new LogChangedValues
			{
				Type = Type,
				ListIndex = ListIndex,
				ElementIndex = ElementIndex,
				FieldIndex = FieldIndex,
				Value = Value,
				ListName = sELeditCache.Instance.sELeditDatas.eLC.Lists[ListIndex].listName,
				ElementName = sELeditCache.Instance.sELeditDatas.eLC.Lists[ListIndex].elementTypes[ElementIndex],
				FieldName = sELeditCache.Instance.sELeditDatas.eLC.Lists[ListIndex].elementFields[FieldIndex]
			});
			LogChangedEvent?.Invoke(this, null);
		}
	}

	public class LogChangedValues
	{
		public LogChangedType Type { get; set; }
		public int ListIndex { get; set; }
		public int ElementIndex { get; set; }
		public int FieldIndex { get; set; }
		public string Value { get; set; }
		public string ListName { get; set; }
		public string ElementName { get; set; }
		public string FieldName { get; set; }


		public override string ToString()
		{
			return $"=> Type: {Type} - " +
				$"[ ListName: {ListName} >> ListIndex: {ListIndex} ], " +
				$"[ ElementName: {ElementName} >> ElementIndex: {ElementIndex} ], " +
				$"[ FieldName: {FieldName} >> FieldIndex: {FieldIndex} ]  " +
				$"Value: {Value}";
		}
	}
	public enum LogChangedType
	{
		ADD,
		REMOVE,
		UPDATE
	}

}
