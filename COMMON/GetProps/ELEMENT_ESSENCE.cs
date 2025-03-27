
using sELedit.CORE.BASE;
using System;
using System.Globalization;

namespace sELedit
{
    class ELEMENT_ESSENCE
    {
        public static string GetProps(int pos_item)
        {
            string line = "";
            try
            {
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[27].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[27].elementFields[k] == "level")
                    {
                        line += "\n" + String.Format(Extensions.GetLocalization(7000), sELeditCache.Instance.sELeditDatas.eLC.GetValue(27, pos_item, k));
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[27].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[27].elementFields[k] == "price")
                    {
                        string price = sELeditCache.Instance.sELeditDatas.eLC.GetValue(27, pos_item, k);
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

