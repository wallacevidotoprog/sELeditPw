using System;
using System.Collections;
using System.Linq;

namespace sELedit.CORE.Extencion
{
	public static class ExCore
	{
		public static GroupedComboBox ComboBoxListItem(this GroupedComboBox cb, eListCollection collection)
		{
			ArrayList datas = new ArrayList();

			for (int i = 0; i < collection.Lists.Length; i++)
			{
				datas.Add(new
				{
					Display = collection.Lists[i].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1],
					Group = collection.Lists[i].listName.Split('_').Last(),
					Value = i
				});
			}
			cb.ValueMember = "Value";
			cb.DisplayMember = "Display";
			cb.GroupMember = "Group";
			cb.DataSource = datas;

			return cb;
		}




	}
}
