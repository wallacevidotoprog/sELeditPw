
using sELedit.CORE.BASE;
using System;
using System.Globalization;

namespace sELedit
{
    class WEDDING_BOOKCARD_ESSENCE
    {
        public static string GetProps(int pos_item)
        {
            string line = "";
            try
            {
                string year = "0";
                string month = "0";
                string day = "0";
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[133].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[133].elementFields[k] == "year")
                    {
                        year = sELeditCache.Instance.sELeditDatas.eLC.GetValue(133, pos_item, k);
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[133].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[133].elementFields[k] == "month")
                    {
                        month = sELeditCache.Instance.sELeditDatas.eLC.GetValue(133, pos_item, k);
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[133].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[133].elementFields[k] == "day")
                    {
                        day = sELeditCache.Instance.sELeditDatas.eLC.GetValue(133, pos_item, k);
                        break;
                    }
                }
                line += "\n" + String.Format(Extensions.GetLocalization(7089), year, month, day);
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[133].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[133].elementFields[k] == "price")
                    {
                        string price = sELeditCache.Instance.sELeditDatas.eLC.GetValue(133, pos_item, k);
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

