
using sELedit.CORE.BASE;
using System;
using System.Globalization;

namespace sELedit
{
	class INC_SKILL_ABILITY_ESSENCE
	{
		public static string GetProps(int pos_item)
		{
			string line = "";
			try
			{
				for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[130].elementFields.Length; k++)
				{
					if (sELeditCache.Instance.sELeditDatas.eLC.Lists[130].elementFields[k] == "level_required")
					{
						line += "\n" + String.Format(Extensions.GetLocalization(7087), sELeditCache.Instance.sELeditDatas.eLC.GetValue(130, pos_item, k));
						break;
					}
				}
				for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[130].elementFields.Length; k++)
				{
					if (sELeditCache.Instance.sELeditDatas.eLC.Lists[130].elementFields[k] == "inc_ratio")
					{
						line += "\n" + String.Format(Extensions.GetLocalization(7088), Convert.ToSingle(sELeditCache.Instance.sELeditDatas.eLC.GetValue(130, pos_item, k)).ToString("P0"));
						break;
					}
				}
				for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[130].elementFields.Length; k++)
				{
					if (sELeditCache.Instance.sELeditDatas.eLC.Lists[130].elementFields[k] == "price")
					{
						string price = sELeditCache.Instance.sELeditDatas.eLC.GetValue(130, pos_item, k);
						if (price != "0")
						{
							line += "\n" + Extensions.GetLocalization(7024) + " " + Convert.ToInt32(price).ToString("N0", CultureInfo.CreateSpecificCulture("zh-CN"));
						}
						break;
					}
				}
			}
			catch
			{
				line = "";
			}
			return line;
		}
	}
}

