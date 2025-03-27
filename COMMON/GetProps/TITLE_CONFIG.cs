using sELedit.CORE.BASE;

namespace sELedit
{
    class TITLE_CONFIG
    {
        public static string GetProps(int pos)
        {
            string line = "";
            try
            {
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "Name")
                    {
                        line += sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        break;
                    }
                }
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "desc")
                    {
                        string desc = sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        if (desc != "")
                        {
                            line += "\n" + desc;
                        }
                        break;
                    }
                }
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "condition")
                    {
                        string condition = sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        if (condition != "")
                        {
                            line += "\n" + condition;
                        }
                        break;
                    }
                }
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "phy_damage")
                    {
                        string phy_damage = sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        if (phy_damage != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7390) + " +" + phy_damage;
                        }
                        break;
                    }
                }
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "magic_damage")
                    {
                        string magic_damage = sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        if (magic_damage != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7391) + " +" + magic_damage;
                        }
                        break;
                    }
                }
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "phy_defence")
                    {
                        string phy_defence = sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        if (phy_defence != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7392) + " +" + phy_defence;
                        }
                        break;
                    }
                }
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "magic_defence")
                    {
                        string magic_defence = sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        if (magic_defence != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7393) + " +" + magic_defence;
                        }
                        break;
                    }
                }
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "armor")
                    {
                        string armor = sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        if (armor != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7394) + " +" + armor;
                        }
                        break;
                    }
                }
                for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields.Length; t++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[169].elementFields[t] == "attack")
                    {
                        string attack = sELeditCache.Instance.sELeditDatas.eLC.GetValue(169, pos, t);
                        if (attack != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7395) + " +" + attack;
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

