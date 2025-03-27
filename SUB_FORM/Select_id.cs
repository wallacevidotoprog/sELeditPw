using sELedit.CORE.BASE;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace sELedit.NOVO
{
	public partial class Select_id : Form
	{
		public Select_id()
		{
			InitializeComponent();
		}

		public int retorn { get; set; }
		public int input { get; set; }
		public Thread AssetManagerLoad { get; private set; }
		SortedList valuePairs;

		private void Select_id_Load(object sender, EventArgs ex)
		{
			valuePairs = new SortedList();

			for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.ItemUse.Count; i++)
			{
				int v = List_categories.Items.Add("[" + int.Parse(sELeditCache.Instance.sELeditDatas.database.ItemUse.GetKey(i).ToString()) + "] - " + sELeditCache.Instance.sELeditDatas.eLC.Lists[int.Parse(sELeditCache.Instance.sELeditDatas.database.ItemUse.GetKey(i).ToString())].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1]);
				if (!valuePairs.ContainsKey(int.Parse(sELeditCache.Instance.sELeditDatas.database.ItemUse.GetKey(i).ToString())))
				{
					valuePairs.Add(int.Parse(sELeditCache.Instance.sELeditDatas.database.ItemUse.GetKey(i).ToString()), "x");
				}

			}

			List_categories.SelectedIndex = 0;

			if (input != 0)
			{
				Search_textbox.Text = input.ToString();
				AssetManagerLoad = new Thread(delegate () { xContinue_search_Click(); }); AssetManagerLoad.Start();

			}
		}

		private void List_categories_SelectedIndexChanged(object sender, EventArgs ex)
		{
			int ixaxa = List_categories.SelectedIndex;
			var Valor = List_categories.Items[ixaxa].ToString().Replace("[", "").Replace("]", "").Split('-');
			int l = int.Parse(Valor[0].Replace(" ", ""));
			Items_grid.Rows.Clear();

			if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
			{
				int pos = -1;
				int pos2 = -1;
				for (int i = 0; i < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields.Length; i++)
				{
					if (sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[i] == "Name")
					{
						pos = i;
					}
					if (sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[i] == "file_icon" || sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[i] == "file_icon1")
					{
						pos2 = i;
					}
					if (pos != -1 && pos2 != -1) { break; }
				}
				for (int e = 0; e < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length; e++)
				{
					if (sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[0] == "ID")
					{

						Bitmap img = Properties.Resources.blank;
						string path = Path.GetFileName(sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, pos2));
						if (sELeditCache.Instance.sELeditDatas.database.sourceBitmap != null && sELeditCache.Instance.sELeditDatas.database.ContainsKey(path))
						{
							if (sELeditCache.Instance.sELeditDatas.database.ContainsKey(path))
							{
								img = sELeditCache.Instance.sELeditDatas.database.images(path);
							}
						}


						Items_grid.Rows.Add(new object[] { img, sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, pos), sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, 0) });



					}
					else
					{

						Items_grid.Rows.Add(new object[] { Properties.Resources.unknown, sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, pos), 0 });

					}



				}
			}
			else
			{

			}
		}

		private void HideForm_button_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void Items_grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridView dgv = (DataGridView)sender;
			retorn = int.Parse(dgv.Rows[e.RowIndex].Cells[2].Value.ToString().Replace(" ", ""));
			this.Close();
		}

		private void xContinue_search_Click()
		{
			List_categories.Invoke((MethodInvoker)delegate ()
			{
				string id = Search_textbox.Text.Replace(" ", "");
				bool finsh = false;
				try
				{
					string value = "";

					for (int lf = 0; lf < valuePairs.Count; lf++)
					{
						for (int ef = 0; ef < sELeditCache.Instance.sELeditDatas.eLC.Lists[int.Parse(valuePairs.GetKey(lf).ToString())].elementValues.Length; ef++)
						{
							value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(int.Parse(valuePairs.GetKey(lf).ToString()), ef, 0);

							if (value.Contains(id))
							{
								List_categories.SelectedIndex = lf;
								Items_grid.ClearSelection();
								Items_grid.CurrentCell = Items_grid[0, ef];
								Items_grid.Rows[ef].Selected = true;
								Items_grid.FirstDisplayedScrollingRowIndex = ef;
								Items_grid.CurrentCell = Items_grid.Rows[ef].Cells[2];
								finsh = true;
								break;

							}



						}
						if (finsh)
						{
							break;
						}

					}
				}
				catch
				{
				}
			});
		}
		private void Continue_search_Click(object sender, EventArgs ex)
		{
			string id = Search_textbox.Text.Replace(" ", "");
			bool finsh = false;
			try
			{
				string value = "";

				for (int lf = 0; lf < valuePairs.Count; lf++)
				{
					for (int ef = 0; ef < sELeditCache.Instance.sELeditDatas.eLC.Lists[int.Parse(valuePairs.GetKey(lf).ToString())].elementValues.Length; ef++)
					{
						value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(int.Parse(valuePairs.GetKey(lf).ToString()), ef, 0);

						if (value.Contains(id))
						{
							List_categories.SelectedIndex = lf;
							Items_grid.ClearSelection();
							Items_grid.CurrentCell = Items_grid[0, ef];
							Items_grid.Rows[ef].Selected = true;
							Items_grid.FirstDisplayedScrollingRowIndex = ef;
							Items_grid.CurrentCell = Items_grid.Rows[ef].Cells[2];
							finsh = true;
							break;

						}



					}
					if (finsh)
					{
						break;
					}

				}
			}
			catch
			{
			}

		}
	}
}
