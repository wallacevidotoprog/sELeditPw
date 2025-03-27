﻿
using sELedit.CORE.BASE;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace sELedit
{
    class ARMOR_ESSENCE
    {
        private static Random random;

        public static string GetProps(int pos_item)
        {
            string line = "";
            try
            {
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "id_sub_type")
                    {
                        string id_sub_type = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k);
                        for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[5].elementValues.Length; t++)
                        {
                            if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(5, t, 0) == id_sub_type)
                            {
                                for (int a = 0; a < sELeditCache.Instance.sELeditDatas.eLC.Lists[5].elementFields.Length; a++)
                                {
                                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[5].elementFields[a] == "Name")
                                    {
                                        line += "\n" + sELeditCache.Instance.sELeditDatas.eLC.GetValue(5, t, a);
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "level")
                    {
                        line += "\n" + String.Format(Extensions.GetLocalization(7000), sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k));
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                    {
                        if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "mp_enhance_low")
                        {
                            string hp_enhance_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 2);
                            if (hp_enhance_low != "0")
                            {
                                line += "\n" + Extensions.GetLocalization(7006) + " +" + hp_enhance_low;
                            }
                            string mp_enhance_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k);
                            if (mp_enhance_low != "0")
                            {
                                line += "\n" + Extensions.GetLocalization(7007) + " +" + mp_enhance_low;
                            }
                            string armor_enhance_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 4);
                            if (armor_enhance_low != "0")
                            {
                                line += "\n" + Extensions.GetLocalization(7008) + " +" + armor_enhance_low;
                            }
                            break;
                        }
                    }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "defence_low")
                    {
                        string defence_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k);
                        if (defence_low != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7009) + " +" + defence_low;
                        }
                        string magic_defences_1_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 2);
                        if (magic_defences_1_low != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7010) + " +" + magic_defences_1_low;
                        }
                        string magic_defences_2_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 4);
                        if (magic_defences_2_low != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7011) + " +" + magic_defences_2_low;
                        }
                        string magic_defences_3_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 6);
                        if (magic_defences_3_low != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7012) + " +" + magic_defences_3_low;
                        }
                        string magic_defences_4_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 8);
                        if (magic_defences_4_low != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7013) + " +" + magic_defences_4_low;
                        }
                        string magic_defences_5_low = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 10);
                        if (magic_defences_5_low != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7014) + " +" + magic_defences_5_low;
                        }
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "durability_min")
                    {
                        line += "\n" + Extensions.GetLocalization(7015) + " " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k) + "/" + sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 1);
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "character_combo_id")
                    {
                        line += Extensions.DecodingCharacterComboId(sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k));
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "require_level")
                    {
                        string require_level = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k);
                        if (require_level != "0")
                        {
                            line += "\n" + String.Format(Extensions.GetLocalization(7018), require_level);
                        }
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "require_strength")
                    {
                        string require_strength = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k);
                        if (require_strength != "0")
                        {
                            line += "\n" + String.Format(Extensions.GetLocalization(7019), require_strength);
                        }
                        string require_agility = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 1);
                        if (require_agility != "0")
                        {
                            line += "\n" + String.Format(Extensions.GetLocalization(7020), require_agility);
                        }
                        string require_energy = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 2);
                        if (require_energy != "0")
                        {
                            line += "\n" + String.Format(Extensions.GetLocalization(7021), require_energy);
                        }
                        string require_tili = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k + 3);
                        if (require_tili != "0")
                        {
                            line += "\n" + String.Format(Extensions.GetLocalization(7022), require_tili);
                        }
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "require_reputation")
                    {
                        string require_reputation = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k);
                        if (require_reputation != "0")
                        {
                            line += "\n" + String.Format(Extensions.GetLocalization(7023), require_reputation);
                        }
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "fixed_props")
                    {
                        if ("0" == sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k))
                        {
                            string[] probability_addon_numX = new string[5];
                            int nu = 0;
                            for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; t++)
                            {
                                if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[t] == "probability_addon_num" + nu)
                                {

                                    probability_addon_numX[nu] = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, t);
                                    nu++;
                                    if (nu == 5) { break; }
                                }
                            }
                            int adds = 0;
                            int Min = 9;int Max = 4;
                            for (int i = 0; i < probability_addon_numX.Length; i++)
                            {
                                string sda = probability_addon_numX[i];
                                if (probability_addon_numX[i].StartsWith("1"))
                                {
                                    adds = i;
                                    break;
                                }
                                else
                                {
                                    
                                    if (Min == 9 && probability_addon_numX[i] != "0,000000")
                                    {
                                        Min = i;
                                    }
                                    if (probability_addon_numX[i] != "0,000000")
                                    {
                                        Max = i;
                                    }
                                }
                            }

                            if (adds == 0)
                            {
                                random = new Random();
                                adds = random.Next(Min, Max);

                            }

                            string[] idAdds = new string[33];
                            nu = 0;

                            for (int t = 1; t < 33; t++)
                            {
                                for (int a = 0; a < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; a++)
                                {
                                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[a] == "addons_" + t + "_id_addon")
                                    {
                                        string id_addon = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, a);
                                        if (id_addon != "0")
                                        {
                                            idAdds[nu] = "\n" + "^4286f4" + EQUIPMENT_ADDON.GetAddon(id_addon) + "^FFFFFF"; nu++;
                                        }
                                        break;

                                    }
                                }
                            }
                            idAdds = idAdds.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                            random = new Random();
                            for (int i = 0; i < adds; i++)
                            {
                                
                                int xs = random.Next(0,idAdds.Length);
                                line += idAdds[random.Next(0,idAdds.Length)]; 
                            }
                        }

                        else if ("0" != sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k))
                        {
                            string probability_addon_num0 = "0";
                            for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; t++)
                            {
                                if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[t] == "probability_addon_num0")
                                {
                                    probability_addon_num0 = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, t);
                                    break;
                                }
                            }
                            if (probability_addon_num0 != "1")
                            {
                                for (int t = 1; t < 33; t++)
                                {
                                    for (int a = 0; a < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; a++)
                                    {
                                        if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[a] == "addons_" + t + "_id_addon")
                                        {
                                            string id_addon = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, a);
                                            if (id_addon != "0")
                                            {
                                                line += "\n" + "^4286f4" + EQUIPMENT_ADDON.GetAddon(id_addon) + "^FFFFFF";
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        //if ("0" != sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k))
                        //{
                        //    string probability_addon_num0 = "0";
                        //    for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; t++)
                        //    {
                        //        if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[t] == "probability_addon_num0")
                        //        {
                        //            probability_addon_num0 = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, t);
                        //            break;
                        //        }
                        //    }
                        //    if (probability_addon_num0 != "1")
                        //    {
                        //        for (int t = 1; t < 33; t++)
                        //        {
                        //            for (int a = 0; a < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; a++)
                        //            {
                        //                if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[a] == "addons_" + t + "_id_addon")
                        //                {
                        //                    string id_addon = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, a);
                        //                    if (id_addon != "0")
                        //                    {
                        //                        line += "\n" + "^4286f4" + EQUIPMENT_ADDON.GetAddon(id_addon) + "^FFFFFF";
                        //                    }
                        //                    break;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        break;
                    }
                }
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields.Length; k++)
                {
                    if (sELeditCache.Instance.sELeditDatas.eLC.Lists[6].elementFields[k] == "price")
                    {
                        string price = sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, k);
                        if (price != "0")
                        {
                            line += "\n" + Extensions.GetLocalization(7024) + " " + Convert.ToInt32(price).ToString("N0", CultureInfo.CreateSpecificCulture("zh-CN"));
                        }
                        break;
                    }
                }
                bool Suc = false; int[] IdCombo = new int[12]; int[] IdAddons = new int[11];
                for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementValues.Length; k++)
                {
                    for (int a = 1; a < 13; a++)
                    {
                        for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields.Length; t++)
                        {
                            if (sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields[t] == "equipments_" + a + "_id")
                            {
                                if (Convert.ToInt32(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, t)) == Convert.ToInt32(sELeditCache.Instance.sELeditDatas.eLC.GetValue(6, pos_item, 0)))
                                {
                                    Suc = true;
                                    string name = "";
                                    string max_equips = "0";
                                    IdCombo[0] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 3));
                                    IdCombo[1] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 4));
                                    IdCombo[2] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 5));
                                    IdCombo[3] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 6));
                                    IdCombo[4] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 7));
                                    IdCombo[5] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 8));
                                    IdCombo[6] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 9));
                                    IdCombo[7] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 10));
                                    IdCombo[8] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 11));
                                    IdCombo[9] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 12));
                                    IdCombo[10] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 13));
                                    IdCombo[11] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 14));

                                    IdAddons[0] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,15));
                                    IdAddons[1] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,16));
                                    IdAddons[2] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,17));
                                    IdAddons[3] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,18));
                                    IdAddons[4] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,19));
                                    IdAddons[5] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,20));
                                    IdAddons[6] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,21));
                                    IdAddons[7] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,22));
                                    IdAddons[8] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,23));
                                    IdAddons[9] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,24));
                                    IdAddons[10] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k,25));


                                    for (int n = 0; n < sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields.Length; n++)
                                    {
                                        if (sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields[n] == "Name")
                                        {
                                            name = sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, n);
                                            break;
                                        }
                                    }
                                    for (int n = 0; n < sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields.Length; n++)
                                    {
                                        if (sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields[n] == "max_equips")
                                        {
                                            max_equips = sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, n);
                                            break;
                                        }
                                    }
                                    line += "\n\n ^FBDA20" + name + " (" + max_equips + ")\n";


                                    for (int i = 0; i < IdCombo.Length; i++)
                                    {
                                        if (IdCombo[i] != 0)
                                        {
                                            line += "^00FF00 [" + IdCombo[i].ToString() + "] - " + Extensions.GetNameItem(IdCombo[i]) + "\n";
                                        }

                                    }
                                    line += "\n";

                                    int add = 2;
                                    for (int i = 0; i < IdAddons.Length; i++)
                                    {
                                        if (IdAddons[i] != 0)
                                        {
                                            line += "^4286f4(" + add.ToString() + ") - " + sELeditCache.Instance.sELeditDatas.database._suite[IdAddons[i]] /*Extensions.GetNameItem(IdCombo[i])*/ + "\n";
                                        }
                                        add++;
                                    }







                                    break;
                                }
                                break;
                            }
                        }
                    }
                    if (Suc == true) break;
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
