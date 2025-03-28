using sELedit.CORE.BASE;
using sELedit.Properties;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
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

		public static bool isNull(this object obj)
		{
			try
			{
				return obj == null ? true : false;
			}
			catch
			{
				return true;
			}
		}

		public static bool IsNull<T>(T obj)
		{
			try
			{
				return obj == null ? true : false;
			}
			catch
			{
				return true;
			}
		}

		public static Image GetImageItem(int ListIndex, int ElementIndex)
		{
			Image image = Resources.unknown;

			int indexIco = Array.FindIndex(sELeditCache.Instance.sELeditDatas.eLC.Lists[ListIndex].elementFields, x => x.ToUpper().StartsWith("FILE_ICON"));
			if (indexIco != -1)
			{
				string fileIco = sELeditCache.Instance.sELeditDatas.eLC.GetValue(ListIndex, ElementIndex, indexIco);
				string path = Path.GetFileName(fileIco);
				

				if (!string.IsNullOrEmpty(fileIco) && sELeditCache.Instance.sELeditDatas.database.ContainsKey(path))
				{
					return sELeditCache.Instance.sELeditDatas.database.images(path);
				}
				else
				{
					return sELeditCache.Instance.sELeditDatas.database.images("unknown.dds");
				}				
			}
			else
			{
				return image;
			}

		}


	}
}
