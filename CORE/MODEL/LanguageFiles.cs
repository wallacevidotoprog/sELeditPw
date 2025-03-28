using System.Collections;
using System.Collections.Generic;

namespace sELedit.CORE.MODEL
{
	public class LanguageFiles
	{
		public SortedList<int, string> LangSelected { get; private set; }
		public Dictionary<string, SortedList<int, string>> ListLang { get; set; }

		public void SetLang(string lang)
		{
			if (!string.IsNullOrEmpty(lang) && ListLang.ContainsKey(lang))
			{
				LangSelected = ListLang[lang];
			}

		}
	}
}
