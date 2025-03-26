using System;
using System.Collections;
using System.Drawing;
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

		public static string ColorCod(Color color)
		{
			Convert.ToString((object)new int[3] { (int)color.R, (int)color.G, (int)color.B });
			return string.Format("^{0:x2}{1:x2}{2:x2}", (object)color.R, (object)color.G, (object)color.B).ToUpper();
		}


	}
}
