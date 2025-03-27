using sELedit.configs;
using sELedit.CORE.BASE;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace sELedit.NOVO
{
	public partial class RECIPES : Form
	{
		Encoding enc = Encoding.GetEncoding("Unicode");
		Thread AssetManagerLoad;
		public RECIPES()
		{
			InitializeComponent();

		}


		public int input { get; set; }
		public int GET { get; set; }

		private void RECIPES_Load(object sender, EventArgs ex)
		{
			AssetManagerLoad = new Thread(delegate ()
			{


				for (int e = 0; e < sELeditCache.Instance.sELeditDatas.eLC.Lists[69].elementValues.Length; e++)
				{
					dataGridView_elems.Invoke((MethodInvoker)delegate ()
					{

						dataGridView_elems.Rows.Add(new object[] { sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, e, 0), "", sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, e, 3) });

						Text = "RECIPES (" + e + " - " + sELeditCache.Instance.sELeditDatas.eLC.Lists[69].elementValues.Length.ToString() + " )";
					});
				}


			}); AssetManagerLoad.Start();
		}


		public Image img(int IdRecipe, int idIndex)
		{
			Image ig = null;

			int id_1 = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, idIndex, 8));
			bool fi = false;

			if (IdRecipe != 0)
			{
				try
				{
					string value = "";
					for (int L = 0; L < sELeditCache.Instance.sELeditDatas.database.ItemUse.Count; L++)
					{

						int La = int.Parse(sELeditCache.Instance.sELeditDatas.database.ItemUse.GetKey(L).ToString());
						int pos = 0;
						int posN = 0;

						for (int i = 0; i < sELeditCache.Instance.sELeditDatas.eLC.Lists[La].elementFields.Length; i++)
						{
							if (sELeditCache.Instance.sELeditDatas.eLC.Lists[La].elementFields[i] == "Name")
							{
								posN = i;
								//break;
							}
							if (sELeditCache.Instance.sELeditDatas.eLC.Lists[La].elementFields[i] == "file_icon")
							{
								pos = i;
								break;
							}

						}
						for (int ef = 0; ef < sELeditCache.Instance.sELeditDatas.eLC.Lists[La].elementValues.Length; ef++)
						{
							value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, pos);

							if (id_1 == int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, 0))/* || value.Contains(b.ToString())*/)
							{
								string path = Path.GetFileName(value);
								if (sELeditCache.Instance.sELeditDatas.database.sourceBitmap != null && sELeditCache.Instance.sELeditDatas.database.ContainsKey(path))
								{
									if (sELeditCache.Instance.sELeditDatas.database.ContainsKey(path))
									{
										ig = Extensions.ResizeImage(sELeditCache.Instance.sELeditDatas.database.images(path), 32, 32);

										fi = true;


										break;
									}
								}
							}
						}

						if (fi == true)
						{
							break;
						}
					}


				}
				catch (Exception edx)
				{


				}
			}


			if (ig == null)
			{
				ig = Properties.Resources.unknown;
			}

			return ig;
		}

		private void dataGridView_elems_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
		{
			int id = int.Parse(((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString());
			Image igg = img(int.Parse(((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString()), e.RowIndex);
			((TextAndImageCell)((DataGridView)sender).Rows[e.RowIndex].Cells[1]).Image = igg;


		}

		private void dataGridView_elems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			GET = int.Parse(((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString());
			this.Close();
		}
	}
}
