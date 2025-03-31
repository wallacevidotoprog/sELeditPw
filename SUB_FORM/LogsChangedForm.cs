using sELedit.CORE.BASE;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace sELedit.SUB_FORM
{
	public partial class LogsChangedForm : Form
	{
		public LogsChangedForm()
		{
			InitializeComponent();

			LogsLoad();

			sELeditCache.Instance.LogChanged.LogChangedEvent += LogChanged_LogChangedEvent;
		}

		private void LogChanged_LogChangedEvent(object sender, EventArgs e)
		{
			richTextBox_log.Clear();
			LogsLoad();
		}

		private void LogsLoad()
		{
			foreach (var item in sELeditCache.Instance.LogChanged.LogChangedValues)
			{
				richTextBox_log.AppendText(item.ToString() + "\n");
			}
		}

		private void HighlightKeywords()
		{
			string[] keywords = new string[] { "ADD", "REMOVE", "UPDATE" };

			foreach (var item in keywords)
			{
				int startIndex = 0;

				while ((startIndex = richTextBox_log.Text.IndexOf(item, startIndex)) != -1)
				{
					richTextBox_log.Select(startIndex, item.Length);

					switch (item)
					{
						case "ADD":
							richTextBox_log.SelectionColor = Color.Green;
							break;
						case "REMOVE":
							richTextBox_log.SelectionColor = Color.Red;
							break;
						case "UPDATE":
							richTextBox_log.SelectionColor = Color.Blue;
							break;
						default:
							break;
					}
					startIndex += item.Length;
				}
			}
			richTextBox_log.SelectionStart = richTextBox_log.Text.Length;
			richTextBox_log.SelectionColor = Color.Black;
		}

		private void richTextBox_log_TextChanged(object sender, System.EventArgs e)
		{
			HighlightKeywords();
		}
	}
}
