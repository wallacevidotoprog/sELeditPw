using sELedit.configs;
using sELedit.CORE.BASE;
using sELedit.CORE.BASE.CONTROLLERS;
using sELedit.CORE.Extencion;
using sELedit.CORE.Extencion.Controls;
using sELedit.DDSReader.Utils;
using sELedit.NOVO;
using sELedit.Properties;
using sELedit.SUB_FORM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace sELedit
{

	public partial class MainWindow : Form
	{
		#region NEW

		private DataGridProps dgp;
		private bool isLoadedDataGridProps = false;

		public Cursor CursG;
		public Cursor CursH;

		public Configs configs;

		private bool SetPropsGridLoad = false;

		#endregion


		private IToolType customTooltype;
		ToolTip t = new ToolTip();
		ToolTip_addons t2 = new ToolTip_addons();
		SurfacesChanger _SurfacesChanger;
		public Select_id select;
		public ClassMaskWindow eClassMask;
		public RECIPES rec;

		public ProcTypeGenerator procType;
		public set_ADDONS setADD;
		public _major_sub major_Sub;

		public bool ColorEdit { get; private set; }

		//private PCKs pck;


		public MainWindow()
		{
			InitializeComponent();

			SetIcoMouse();

			sELeditCache.Instance.progressBar = toolStripProgressBar_loads;
			sELeditCache.Instance.Start();
			sELeditCache.Instance.StartFilesLoads();

			EventGlobal.Subscribe<eListCollection>(ElementDataLoaded);

		}

		private void ElementDataLoaded(eListCollection collection)
		{
			comboBox_lists.Items.Clear();
			comboBox_lists.ComboBoxListItem(collection);

			if (collection.Lists[0].listOffset.Length > 0)
				this.Text = " sELedit NanoTech (" + sELeditCache.Instance.Settings.ElementsDataPath + " [ Version: " + collection.VersionData + " Key:" + collection.Version.ToString() + BitConverter.ToUInt32(collection.Lists[0].listOffset, 0) + " ])";
		}

		private void SetIcoMouse()
		{
			var CursG = AdvancedCursorsFromEmbededResources.Create(Properties.Resources.Game);
			var CursH = AdvancedCursorsFromEmbededResources.Create(Properties.Resources.Hand);

			Cursor = CursG;

			dataGridView_elems.Cursor = CursH; pictureBox_icon.Cursor = CursH; numericUpDownEx_ID.Cursor = CursH; textBox_NAME.Cursor = CursH; pictureBox_color_Item.Cursor = CursH;
		}



		private void toolStripButton_Config_Click(object sender, EventArgs e)
		{
			configs = new Configs();
			configs.ShowDialog(this);

			if (configs.isModified)
			{
				sELeditCache.Instance.sELeditDatas.eLC = null;
				sELeditCache.Instance.sELeditDatas.database.Tasks = null;
				sELeditCache.Instance.sELeditDatas.database.Gshop = null;
				sELeditCache.Instance.sELeditDatas.database.GshopEvent = null;
				sELeditCache.Instance.Start();
				sELeditCache.Instance.StartFilesLoads();
			}
		}

		#region save

		private void saveElementedataToolStripMenuItem_Click(object sender, EventArgs e)
		{

			if (sELeditCache.Instance.Settings.CheckFileExists(nameof(sELeditCache.Instance.Settings.ElementsDataPath)))
			{
				try
				{

					Cursor = Cursors.AppStarting;
					//progressBar_progress.Style = ProgressBarStyle.Marquee;
					File.Copy(sELeditCache.Instance.Settings.ElementsDataPath, sELeditCache.Instance.Settings.ElementsDataPath + ".bak", true);
					if (sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex > -1 && sELeditCache.Instance.sELeditDatas.eLC.Lists.Length > sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
					{
						sELeditCache.Instance.sELeditDatas.eLC.Lists[sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex].elementValues[0][0] = sELeditCache.Instance.sELeditDatas.conversationList.GetBytes();
					}
					sELeditCache.Instance.sELeditDatas.eLC.Save(sELeditCache.Instance.Settings.ElementsDataPath);
					//progressBar_progress.Style = ProgressBarStyle.Continuous;
					Cursor = Cursors.Default;
				}
				catch
				{
					MessageBox.Show("SAVING ERROR!\nThis error mostly occurs of configuration and elements.data mismatch");
					//progressBar_progress.Style = ProgressBarStyle.Continuous;
					Cursor = Cursors.Default;
				}
			}
		}

		private void saveGshop12ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//if (sELeditCache.Instance.sELeditDatas.database.Gshop != null)
			//{
			//    SalveShops.SaveGshopData(sELeditCache.Instance.sELeditDatas.database.Gshop, XmlData.GshopDataPath);
			//    SalveShops.SaveGshopSevData(sELeditCache.Instance.sELeditDatas.database.Gshop, XmlData.GshopDataPath);
			//}
			//if (sELeditCache.Instance.sELeditDatas.database.GshopEvent != null)
			//{
			//    SalveShops.SaveGshopData(sELeditCache.Instance.sELeditDatas.database.GshopEvent, XmlData.Gshop1DataPath);
			//    SalveShops.SaveGshopSevData(sELeditCache.Instance.sELeditDatas.database.GshopEvent, XmlData.Gshop1DataPath);
			//}


		}

		private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveElementedataToolStripMenuItem_Click(null, null);
		}

		private void click_save(object sender, EventArgs e)
		{

			SaveFileDialog eSave = new SaveFileDialog();
			eSave.InitialDirectory = Environment.CurrentDirectory;
			eSave.Filter = "Elements File (*.data)|*.data|All Files (*.*)|*.*";
			if (eSave.ShowDialog() == DialogResult.OK && eSave.FileName != "")
			{
				try
				{
					Cursor = Cursors.AppStarting;
					//progressBar_progress.Style = ProgressBarStyle.Marquee;
					//File.Copy(eSave.FileName, eSave.FileName + ".bak", true);
					if (sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex > -1 && sELeditCache.Instance.sELeditDatas.eLC.Lists.Length > sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
					{
						sELeditCache.Instance.sELeditDatas.eLC.Lists[sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex].elementValues[0][0] = sELeditCache.Instance.sELeditDatas.conversationList.GetBytes();
					}
					sELeditCache.Instance.sELeditDatas.eLC.Save(eSave.FileName);
					//progressBar_progress.Style = ProgressBarStyle.Continuous;
					Cursor = Cursors.Default;
				}
				catch
				{
					MessageBox.Show("SAVING ERROR!\nThis error mostly occurs of configuration and elements.data mismatch");
					//progressBar_progress.Style = ProgressBarStyle.Continuous;
					Cursor = Cursors.Default;
				}
			}
		}

		#endregion

		private void attAddonsItemToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(new ups.exyt()).Show(this);
		}

		private void attAddonsSuiteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(new ups.BuscaAddonsSuite()).ShowDialog(this);
		}



		private void change_list(object sender, EventArgs ea)// click no dataElement
		{

			pictureBox_icon.BackgroundImage = sELeditCache.Instance.sELeditDatas?.database?.images("unknown.dds") ?? Resources.unknown;
			addItemRecipeToolStripMenuItem.Visible = false;
			dataGridView_elems.Columns[1].Visible = sELeditCache.Instance.sELeditDatas.eLC.Lists[(int)comboBox_lists.SelectedValue].isIcon;


			if (comboBox_lists.SelectedIndex > -1)
			{
				int l = (int)comboBox_lists.SelectedValue;
				dataGridView_elems.Rows.Clear();

				if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
				{

					int pos = Array.FindIndex(sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields, x => x.ToUpper() == "NAME"); ;



					for (int e = 0; e < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length; e++)
					{
						string value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, pos);
						dataGridView_elems.Rows.Add(new object[] {
							sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, 0),
							Resources.unknown,
							new DisplayValueItemElem
							{
								DisplayText=value,
								RealValue=value
							}
						});

					}
				}
				change_item(null, null);
			}
		}

		private void change_item(object sender, EventArgs ea)
		{
			try
			{
				var groupedValues = CategoryControlManager.GroupElementValuesByCategory(sELeditCache.Instance.sELeditDatas.eLC.Lists[(int)comboBox_lists.SelectedValue]);
				var controls = CategoryControlManager.CreateControlsFromGroupedValues(groupedValues);

				if (dgp != null)
				{
					dgp.DataChanged -= Dgp_DataChanged;
					dgp.Loaded -= Dgp_Loaded;
				}


				isLoadedDataGridProps = false;

				panel_center.Controls.Clear();

				dgp = new DataGridProps(
					(int)comboBox_lists.SelectedValue,
					dataGridView_elems.CurrentCell.RowIndex,
					groupedValues)
				{ Dock = DockStyle.Fill };

				dgp.DataChanged += Dgp_DataChanged;
				dgp.Loaded += Dgp_Loaded;

				panel_center.Controls.Add(dgp);

				panel_buts.Controls.Clear();
				foreach (var item in controls)
				{
					item.Click += (s, e) => dgp.SetPropsGrid(item.Text);
					panel_buts.Controls.Add(item);
				}
				SetTopValues();
			}
			catch (Exception ex)
			{
				ex.ErrorGet();
			}
		}

		private void Dgp_Loaded(object sender, bool e) => isLoadedDataGridProps = e;

		private void SetTopValues()
		{
			eList list = sELeditCache.Instance.sELeditDatas.eLC.Lists[(int)comboBox_lists.SelectedValue];

			int indexIco = Array.FindIndex(list.elementFields, x => x.ToUpper().StartsWith("FILE_ICON"));
			if (indexIco != -1)
			{
				Image tempImg = ExCore.GetImageItem((int)comboBox_lists.SelectedValue, dataGridView_elems.CurrentCell.RowIndex) ?? Resources.unknown;

				pictureBox_icon.BackgroundImage = tempImg;
				pictureBox_icon.Visible = true;
			}
			else
			{
				pictureBox_icon.Visible = false;
			}


			int indexID = Array.FindIndex(list.elementFields, x => x.ToUpper() == "ID");
			if (indexID != -1)
			{
				int id = Convert.ToInt32(sELeditCache.Instance.sELeditDatas.eLC.GetValue((int)comboBox_lists.SelectedValue, dataGridView_elems.CurrentCell.RowIndex, indexID));
				numericUpDownEx_ID.Value = id;
			}

			SetPropsGridLoad = true;
			int indexName = Array.FindIndex(list.elementFields, x => x.ToUpper() == "NAME");
			if (indexName != -1)
			{
				string name = sELeditCache.Instance.sELeditDatas.eLC.GetValue((int)comboBox_lists.SelectedValue, dataGridView_elems.CurrentCell.RowIndex, indexName);
				textBox_NAME.Text = name;
				Color colorSelected = Color.Black;
				try
				{
					int ID = int.Parse(dataGridView_elems.Rows[dataGridView_elems.CurrentCell.RowIndex].Cells[0].Value.ToString());

					if (!ExCore.IsNull(sELeditCache.Instance.sELeditDatas?.database?.item_color) && sELeditCache.Instance.sELeditDatas.database.item_color.ContainsKey(ID))
					{
						colorSelected = Helper.getByID(sELeditCache.Instance.sELeditDatas.database.item_color[ID]);
					}
					else
					{
						colorSelected = Color.White;
					}
				}

				catch (Exception e)
				{ e.ErrorGet(false); colorSelected = Color.White; }
				textBox_NAME.ForeColor = Color.Black;
			}
			SetPropsGridLoad = false;
		}

		private void Dgp_DataChanged(object sender, EventDataChanged e)
		{
			SetTopValues();
			GridElem(e);

		}

		private void GridElem(EventDataChanged e)
		{
			if (!string.IsNullOrEmpty(e.FieldName) && e.FieldName.ToUpper() == "ID" || e.FieldName.ToUpper() == "NAME" || e.FieldName.ToUpper().StartsWith("FILE_ICON"))
			{
				switch (e.FieldName.ToUpper())
				{

					case "ID":
						dataGridView_elems.Rows[dataGridView_elems.CurrentCell.RowIndex].Cells[0].Value = e.Value;
						break;
					case "NAME":
						dataGridView_elems.Rows[dataGridView_elems.CurrentCell.RowIndex].Cells[2].Value =
							new DisplayValueItemElem { DisplayText = e.Value, RealValue = e.Value };
						break;
					default:
						if (e.FieldName.ToUpper().StartsWith("FILE_ICON"))
						{
							Image tempIMG = ExCore.GetImageItem(e.ListIndex, e.ElementIndex);
							dataGridView_elems.Rows[dataGridView_elems.CurrentCell.RowIndex].Cells[1].Value = tempIMG;
							pictureBox_icon.BackgroundImage = tempIMG;
						}
						break;
				}
			}
			return;
		}




		#region MouseTitle

		private void dataGridView_recipes_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				int IdListRecipe = int.Parse(((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString().Replace("[", "").Replace("]", "").Split(new string[] { " - " }, StringSplitOptions.None)[0].ToString().Replace(" ", ""));

				IntPtr handle = ((Control)sender).Handle;
				this.t.ShowToolTip(handle, IdListRecipe, 69);
			}
			catch (Exception)
			{


			}
		}

		private void dataGridView_recipes_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			if ((t != null) && t.Visible)
			{
				this.t.Hide();
			}
		}


		#endregion


		#region TOP_PROPS
		private void pictureBox_icon_Click(object sender, EventArgs e)
		{
			int indexIco = Array.FindIndex(sELeditCache.Instance.sELeditDatas.eLC.Lists[(int)comboBox_lists.SelectedValue].elementFields, x => x.ToUpper().StartsWith("FILE_ICON"));
			string fileIco = sELeditCache.Instance.sELeditDatas.eLC.GetValue((int)comboBox_lists.SelectedValue, dataGridView_elems.CurrentCell.RowIndex, indexIco);


			_SurfacesChanger = new SurfacesChanger();
			_SurfacesChanger.SET = !string.IsNullOrEmpty(fileIco) ? fileIco : "unknown.dds";
			_SurfacesChanger.ShowDialog(this);

			string retur = _SurfacesChanger.GET;

			if (!isLoadedDataGridProps)
			{
				return;
			}
			if (retur != null)
			{
				dgp.SetIco(retur);
			}



		}

		private void textBox_NAME_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (sELeditCache.Instance.sELeditDatas.eLC.Lists[comboBox_lists.SelectedIndex].itemUse)
			{
				try
				{
					(new _ListNamesColor(textBox_NAME.Text, Color.Black, comboBox_lists.SelectedIndex, textBox_NAME.Width, Cursor.Position, int.Parse(numericUpDownEx_ID.Value.ToString()))).ShowDialog(this);
					textBox_NAME.ForeColor = Helper.getByID(sELeditCache.Instance.sELeditDatas.database.item_color[int.Parse(numericUpDownEx_ID.Value.ToString())]);
				}
				catch (Exception ex)
				{
					ex.ErrorGet();
				}

			}
		}

		private void pictureBox_color_Item_Click(object sender, EventArgs e)
		{
			ColorEdit = true;
			textBox_NAME.ForeColor = Color.Black;
			ColorDialog colorDialog = new ColorDialog();
			string textBox_ColorCod = "";
			colorDialog.FullOpen = true;
			try
			{
				colorDialog.Color = Color.FromArgb(int.Parse(textBox_ColorCod.Substring(1, 6), NumberStyles.HexNumber));
			}
			catch
			{
				colorDialog.Color = Color.Black;
			}
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				var colorcod = ExCore.ColorCod(colorDialog.Color);
				textBox_ColorCod = colorcod;
				Clipboard.SetText(colorcod);

				string COR = textBox_ColorCod;

				if (textBox_NAME.Text.StartsWith("^"))
				{
					textBox_NAME.Text = textBox_NAME.Text.Remove(0, 7);
				}
				ColorEdit = false;
				textBox_NAME.Text = textBox_NAME.Text.Insert(0, textBox_ColorCod);
				textBox_NAME.ForeColor = Extensions.ColorHex(textBox_NAME.Text);
			}
		}

		private void pictureBox_icon_MouseEnter(object sender, EventArgs e)
		{
			pictureBox_icon.Image = Resources.bloco_select;
		}

		private void pictureBox_icon_MouseLeave(object sender, EventArgs e)
		{
			pictureBox_icon.Image = null;
		}

		#endregion

		private void pictureBox_BOX_Click(object sender, EventArgs e)
		{
			if (comboBox_lists.SelectedIndex == 54)
			{
				NOVO.ForjaShop shop54;

				//#region shop54

				//Npc_MAKER[] _Npc_MAKER = new Npc_MAKER[8];
				//Itens_Npc_MAKER[] _Itens_Npc_MAKER = new Itens_Npc_MAKER[32];
				//int lineI = 0; int lineT = 0;

				//int _lineI = 1; int _lineT = 1;
				//int[] listItem;


				////for (int i = 0; i < dataGridView_item.Rows.Count; i++)
				////{
				////	string name = dataGridView_item.Rows[i].Cells[0].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace("[", "").Replace("]", "").Replace(" ", "");
				////	string type = dataGridView_item.Rows[i].Cells[1].Value.ToString();
				////	string value = dataGridView_item.Rows[i].Cells[2].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace("[", "").Replace("]", "").Replace(" ", "");

				////	if (name.StartsWith("pages_" + _lineT) && name.EndsWith("_page_title"))
				////	{
				////		_Npc_MAKER[lineT] = new Npc_MAKER();
				////		_Npc_MAKER[lineT].Title = value;
				////		_Npc_MAKER[lineT].IdItem = listItem = new int[32];

				////	}
				////	if (name.StartsWith("pages_" + _lineT + "_id_goods_" + _lineI))
				////	{
				////		_Npc_MAKER[lineT].IdItem[_lineI - 1] = int.Parse(value);

				////		lineI++;
				////		_lineI++;

				////		if (_lineI == 32)
				////		{
				////			_lineI = 1;
				////			lineT++;
				////			_lineT++;
				////		}
				////	}



				////}

				//if (_Npc_MAKER != null)
				//{
				//	shop54 = new ForjaShop(_Npc_MAKER);
				//	shop54.ShowDialog(this);
				//	_Npc_MAKER = shop54.Npc_MAKER;


				//	lineI = 0; lineT = 0;

				//	_lineI = 1; _lineT = 1;

				//	for (int i = 0; i < dataGridView_item.Rows.Count; i++)
				//	{
				//		string name = dataGridView_item.Rows[i].Cells[0].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace("[", "").Replace("]", "").Replace(" ", "");
				//		string type = dataGridView_item.Rows[i].Cells[1].Value.ToString();
				//		string value = dataGridView_item.Rows[i].Cells[2].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace("[", "").Replace("]", "").Replace(" ", "");

				//		if (name.StartsWith("pages_" + _lineT) && name.EndsWith("_page_title"))
				//		{
				//			dataGridView_item.Rows[i].Cells[2].Value = _Npc_MAKER[lineT].Title;
				//		}
				//		if (name.StartsWith("pages_" + _lineT + "_id_goods_" + _lineI))
				//		{
				//			dataGridView_item.Rows[i].Cells[2].Value = _Npc_MAKER[lineT].IdItem[_lineI - 1];

				//			lineI++;
				//			_lineI++;

				//			if (_lineI == 32)
				//			{
				//				_lineI = 1;
				//				lineT++;
				//				_lineT++;
				//			}
				//		}



				//	}

				//	dataGridView_item.Update();
				//	dataGridView_item.Refresh();

				//}

				//#endregion

			}

			if (comboBox_lists.SelectedIndex == 40)
			{
				NOVO.SELL shop40;

				#region shop40



				//Npc_MAKER[] _Npc_MAKER = new Npc_MAKER[8];
				//Itens_Npc_MAKER[] _Itens_Npc_MAKER = new Itens_Npc_MAKER[32];
				//int lineI = 0; int lineT = 0;

				//int _lineI = 1; int _lineT = 1;
				//int[] listItem;


				//for (int i = 0; i < dataGridView_item.Rows.Count; i++)
				//{
				//	string name = dataGridView_item.Rows[i].Cells[0].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace("[", "").Replace("]", "").Replace(" ", "");
				//	string type = dataGridView_item.Rows[i].Cells[1].Value.ToString();
				//	string value = dataGridView_item.Rows[i].Cells[2].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace("[", "").Replace("]", "").Replace(" ", "");

				//	if (name.StartsWith("pages_" + _lineT) && name.EndsWith("_page_title"))
				//	{
				//		_Npc_MAKER[lineT] = new Npc_MAKER();
				//		_Npc_MAKER[lineT].Title = value;
				//		_Npc_MAKER[lineT].IdItem = listItem = new int[32];

				//	}
				//	if (name.StartsWith("pages_" + _lineT) && name.EndsWith(_lineI + "_id"))
				//	{
				//		_Npc_MAKER[lineT].IdItem[_lineI - 1] = int.Parse(value);

				//		lineI++;
				//		_lineI++;

				//		if (_lineI == 32)
				//		{
				//			_lineI = 1;
				//			lineT++;
				//			_lineT++;
				//		}
				//	}



				//}

				//if (_Npc_MAKER != null)
				//{
				//	shop40 = new SELL(_Npc_MAKER);
				//	shop40.ShowDialog(this);
				//	_Npc_MAKER = shop40.Npc_MAKER;


				//	lineI = 0; lineT = 0;

				//	_lineI = 1; _lineT = 1;

				//	for (int i = 0; i < dataGridView_item.Rows.Count; i++)
				//	{
				//		string name = dataGridView_item.Rows[i].Cells[0].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace("[", "").Replace("]", "").Replace(" ", "");

				//		if (name.StartsWith("pages_" + _lineT) && name.EndsWith("_page_title"))
				//		{
				//			dataGridView_item.Rows[i].Cells[2].Value = _Npc_MAKER[lineT].Title;
				//		}
				//		if (name.StartsWith("pages_" + _lineT) && name.EndsWith(_lineI + "_id"))
				//		{
				//			dataGridView_item.Rows[i].Cells[2].Value = _Npc_MAKER[lineT].IdItem[_lineI - 1];

				//			lineI++;
				//			_lineI++;

				//			if (_lineI == 32)
				//			{
				//				_lineI = 1;
				//				lineT++;
				//				_lineT++;
				//			}
				//		}



				//	}
				//	dataGridView_item.Refresh();


			}

			#endregion
		}

		#region Grids

		private void add_Returne(int ID)
		{
			if (ID != 0)
			{
				#region RECIPE
				Encoding enc = Encoding.GetEncoding("Unicode");

				dataGridView_recipes.Rows.Clear();
				string id_RP = null;
				for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[69].elementValues.Length; k++)
				{
					id_RP = sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, k, 0);
					byte[] id_name = enc.GetBytes(sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, k, 3));
					var id_1 = sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, k, 8);
					var id_2 = sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, k, 10);
					var id_3 = sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, k, 12);
					var id_4 = sELeditCache.Instance.sELeditDatas.eLC.GetValue(69, k, 14);
					//string itens = "";
					if (ID == int.Parse(id_1) || ID == int.Parse(id_2) || ID == int.Parse(id_3))
					{
						if (int.Parse(id_1) != 0)
						{
							dataGridView_recipes.Rows.Add(new object[] { id_RP, enc.GetString(id_name), id_1, id_2, id_3, id_4 });

						}
					}

				}
				dataGridView_recipes.ClearSelection();
				// NPC
				// maker 
				dataGridView_npcs.Rows.Clear();
				foreach (DataGridViewRow item in dataGridView_recipes.Rows)
				{
					var IdForMaker = dataGridView_recipes.Rows[item.Index].Cells[0].Value.ToString();
					if (IdForMaker != null)
					{

						for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[54].elementValues.Length; k++)
						{
							for (int f = 0; f < sELeditCache.Instance.sELeditDatas.eLC.Lists[54].elementValues[k].Length; f++)
							{
								var a = sELeditCache.Instance.sELeditDatas.eLC.Lists[54].elementFields[f];

								if (a.Contains("_id_goods"))
								{
									var c = sELeditCache.Instance.sELeditDatas.eLC.GetValue(54, k, f);
									if (c == IdForMaker)
									{
										var idMk = sELeditCache.Instance.sELeditDatas.eLC.GetValue(54, k, 0);
										for (int kk = 0; kk < sELeditCache.Instance.sELeditDatas.eLC.Lists[57].elementValues.Length; kk++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(57, kk, 26) == idMk)
											{
												dataGridView_npcs.Rows.Add(new object[] { sELeditCache.Instance.sELeditDatas.eLC.GetValue(57, kk, 0), sELeditCache.Instance.sELeditDatas.eLC.GetValue(57, kk, 1), "MAKER", IdForMaker });
											}

										}

									}
								}

							}
						}
					}
				}
				dataGridView_npcs.ClearSelection();

				//SELL
				//var IdForMaker = dataGridView_recipes.Rows[item.Index].Cells[0].Value.ToString();

				if (ID != null || (ID != 0))
				{

					for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[40].elementValues.Length; k++)
					{
						for (int f = 0; f < sELeditCache.Instance.sELeditDatas.eLC.Lists[40].elementValues[k].Length; f++)
						{
							var a = sELeditCache.Instance.sELeditDatas.eLC.Lists[40].elementFields[f];

							if (a.EndsWith("_id"))
							{
								var c = sELeditCache.Instance.sELeditDatas.eLC.GetValue(40, k, f);

								if (c == ID.ToString())
								{
									var idMk = sELeditCache.Instance.sELeditDatas.eLC.GetValue(40, k, 0);
									for (int kk = 0; kk < sELeditCache.Instance.sELeditDatas.eLC.Lists[57].elementValues.Length; kk++)
									{
										if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(57, kk, 12) == idMk)
										{
											dataGridView_npcs.Rows.Add(new object[] { sELeditCache.Instance.sELeditDatas.eLC.GetValue(57, kk, 0), sELeditCache.Instance.sELeditDatas.eLC.GetValue(57, kk, 1), "SELL", ID });
										}

									}

								}
							}

						}
					}
				}




				dataGridView_npcs.ClearSelection();


				#endregion

				#region SUITE
				string line = "";
				dataGridView_SUITE.Rows.Clear();
				int[] IdCombo = new int[12];
				bool Suc = false;
				for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementValues.Length; k++)
				{
					for (int a = 1; a < 13; a++)
					{
						for (int t = 0; t < sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields.Length; t++)
						{
							if (sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields[t] == "equipments_" + a + "_id")
							{
								if (Convert.ToInt32(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, t)) == Convert.ToInt32(ID/*sELeditCache.Instance.sELeditDatas.eLC.GetValue(3, pos_item, 0)*/))
								{
									int xtx = 3;
									for (int i = 0; i < 12; i++)
									{
										IdCombo[i] = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, xtx)); xtx++;
									}

									Suc = true;
									string id = "";
									string name = "";
									string max_equips = "0";

									for (int n = 0; n < sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields.Length; n++)
									{
										if (sELeditCache.Instance.sELeditDatas.eLC.Lists[90].elementFields[n] == "Name")
										{
											name = sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, n);
											id = sELeditCache.Instance.sELeditDatas.eLC.GetValue(90, k, 0);
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
									line += id + " - " + name + " (" + max_equips + ")";

									dataGridView_SUITE.Rows.Add(new object[] { line });
									xtx = 0;
									for (int i = 1; i < 13; i++)
									{
										dataGridView_SUITE.Rows[0].Cells[i].Value = Extensions.IdImageItem(IdCombo[xtx]); xtx++;

									}




									break;
								}
								break;
							}
							if (Suc == true) break;
						}
						if (Suc == true) break;
					}
					if (Suc == true) break;
				}
				dataGridView_SUITE.ClearSelection();
				#endregion

				#region desc
				// System.NullReferenceException: 'Referência de objeto não definida para uma instância de um objeto.'
				//for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.ItemUse.Count; i++)
				//{
				//	if (int.Parse(sELeditCache.Instance.sELeditDatas.database.ItemUse.GetKey(i).ToString()) == comboBox_lists.SelectedIndex)
				//	{
				//		if (sELeditCache.Instance.sELeditDatas.database.item_ext_desc.ContainsKey(ID.ToString()))
				//		{
				//			SetText(sELeditCache.Instance.sELeditDatas.database.item_ext_desc[ID.ToString()].ToString());

				//		}

				//	}
				//}
				#endregion

				#region Task
				//dataGridView_tasks.Rows.Clear();
				//dataGridView1.Rows.Clear();
				//if (sELeditCache.Instance.sELeditDatas.database.Tasks != null)
				//{
				//	for (int t = 0; t < sELeditCache.Instance.sELeditDatas.database.Tasks.Length; t++)
				//	{
				//		for (int m = 0; m < sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems.Length; m++)
				//		{
				//			for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems.Length; i++)
				//			{
				//				if (sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_ulItemTemplId == ID)
				//				{

				//					dataGridView_tasks.Rows.Add(new object[] { sELeditCache.Instance.sELeditDatas.database.Tasks[t].ID, sELeditCache.Instance.sELeditDatas.database.Tasks[t].Name, sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_ulItemNum + " UN", Convert.ToDecimal(sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_fProb * 100) + "%  " });
				//				}
				//			}
				//		}
				//	}


				//	if (comboBox_lists.SelectedIndex == 74)
				//	{

				//		dataGridView1.Rows.Clear();
				//		for (int t = 0; t < sELeditCache.Instance.sELeditDatas.database.Tasks.Length; t++)
				//		{
				//			if (sELeditCache.Instance.sELeditDatas.database.Tasks[t].ID == int.Parse(dataGridView_item.Rows[4].Cells[2].Value.ToString().Replace("[", "").Replace("]", "").Split(new string[] { " - " }, StringSplitOptions.None)[0].ToString().Replace(" ", "")))
				//			{
				//				for (int m = 0; m < sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems.Length; m++)
				//				{
				//					for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems.Length; i++)
				//					{

				//						dataGridView1.Rows.Add(new object[] { sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_ulItemTemplId, sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_ulItemNum + " UN", Convert.ToDecimal(sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_fProb * 100) + "%  " });

				//					}
				//					break;
				//				}
				//				break;
				//			}

				//		}
				//	}

				//}
				//else
				//{
				//	dataGridView1.Visible = false;
				//}
				//dataGridView_tasks.ClearSelection();

				#endregion

				#region GSHOP
				//dataGridView_gshop.Rows.Clear();
				//if (sELeditCache.Instance.sELeditDatas.database.Gshop != null)
				//{
				//	for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.Gshop.List_items.Count; i++)
				//	{
				//		if (sELeditCache.Instance.sELeditDatas.database.Gshop.List_items[i].Id == ID)
				//		{
				//			try
				//			{
				//				dataGridView_gshop.Rows.Add(new object[] { "GSHOP", sELeditCache.Instance.sELeditDatas.database.Gshop.List_categories[sELeditCache.Instance.sELeditDatas.database.Gshop.List_items[i].Item_category].Category_name, sELeditCache.Instance.sELeditDatas.database.Gshop.List_categories[sELeditCache.Instance.sELeditDatas.database.Gshop.List_items[i].Item_category].Sub_categories[sELeditCache.Instance.sELeditDatas.database.Gshop.List_items[i].Item_sub_category], sELeditCache.Instance.sELeditDatas.database.Gshop.List_items[i].Amount, Convert.ToDecimal(sELeditCache.Instance.sELeditDatas.database.Gshop.List_items[i].Sales[0].Price) / 100 });

				//			}
				//			catch (Exception)
				//			{


				//			}

				//		}
				//	}
				//}
				//dataGridView_gshop.ClearSelection();
				//if (sELeditCache.Instance.sELeditDatas.database.GshopEvent != null)
				//{
				//	for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_items.Count; i++)
				//	{
				//		if (sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_items[i].Id == ID)
				//		{
				//			try
				//			{
				//				dataGridView_gshop.Rows.Add(new object[] { "GSHOP EVENT", sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_categories[sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_items[i].Item_category].Category_name, sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_categories[sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_items[i].Item_category].Sub_categories[sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_items[i].Item_sub_category], sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_items[i].Amount, Convert.ToDecimal(sELeditCache.Instance.sELeditDatas.database.GshopEvent.List_items[i].Sales[0].Price) / 100 });

				//			}
				//			catch (Exception)
				//			{


				//			}

				//		}
				//	}
				//}
				//dataGridView_gshop.ClearSelection();
				#endregion
			}

		}


		public int[] gridSelectedIndices(DataGridView grd)
		{
			List<int> inx = new List<int>();
			Int32 selectedRowCount = grd.Rows.GetRowCount(DataGridViewElementStates.Selected);
			if (selectedRowCount > 0)
			{
				for (int i = 0; i < selectedRowCount; i++)
				{
					inx.Add(grd.SelectedRows[i].Index);
				}
			}
			inx.Sort();
			int[] arr = inx.ToArray();
			return arr;
		}

		private void click_SetValue(object sender, EventArgs e)
		{
			//if (dgV != null)
			//{
			//	string valueSet = string.Empty;

			//	//if (typeItem.StartsWith("int"))
			//	//{
			//	//	valueSet = Convert.ToInt32(numericUpDownEx_value.Value.ToString()).ToString(); ;
			//	//}
			//	//if (typeItem.StartsWith("float"))
			//	//{
			//	//	valueSet = numericUpDownEx_value.Value.ToString();

			//	//}
			//	//if (typeItem.StartsWith("string") || typeItem.StartsWith("wstring"))
			//	//{
			//	//	valueSet = textBox_SetValue.Text;
			//	//}

			//	int l = comboBox_lists.SelectedIndex;
			//	if (l > -1 && l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
			//	{
			//		ArrayList SelectedCellsIndexes = new ArrayList();
			//		int[] selIndices = gridSelectedIndices(dataGridView_elems);
			//		for (int i = 0; i < dgV.SelectedCells.Count; i++)
			//		{
			//			bool check = true;
			//			for (int k = 0; k < SelectedCellsIndexes.Count; k++)
			//			{
			//				if ((int)SelectedCellsIndexes[k] == dgV.SelectedCells[i].RowIndex)
			//				{
			//					check = false;
			//					break;
			//				}
			//			}
			//			if (check)
			//				SelectedCellsIndexes.Add(dgV.SelectedCells[i].RowIndex);
			//		}
			//		SelectedCellsIndexes.Sort();
			//		for (int i = 0; i < SelectedCellsIndexes.Count; i++)
			//		{

			//			dgV.Rows[(int)SelectedCellsIndexes[i]].Cells[2].Value = valueSet;
			//		}
			//		for (int i = 0; i < selIndices.Length; i++)
			//		{
			//			for (int f = 0; f < SelectedCellsIndexes.Count; f++)
			//			{

			//				sELeditCache.Instance.sELeditDatas.eLC.SetValue(l, selIndices[i], int.Parse(dgV.Rows[(int)SelectedCellsIndexes[i]].Cells[3].Value.ToString()), valueSet);

			//				// (int)SelectedCellsIndexes[f]


			//				if (dgV.Rows[(int)SelectedCellsIndexes[f]].Cells[0].Value.ToString() == "Name")
			//				{
			//					int pos = -1;
			//					int pos2 = -1;
			//					for (int p = 0; p < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields.Length; p++)
			//					{
			//						if (sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[p] == "Name")
			//						{
			//							pos = p;
			//						}
			//						if (sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[p] == "file_icon" || sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[p] == "file_icon1")
			//						{
			//							pos2 = p;
			//						}
			//						if (pos != -1 && pos2 != -1)
			//						{
			//							break;
			//						}
			//					}
			//					if (sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[0] == "ID")
			//					{
			//						Bitmap img = Properties.Resources.unknown;
			//						string path = Path.GetFileName(sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, selIndices[i], pos2));
			//						if (sELeditCache.Instance.sELeditDatas.database.sourceBitmap != null && sELeditCache.Instance.sELeditDatas.database.ContainsKey(path))
			//						{
			//							if (sELeditCache.Instance.sELeditDatas.database.ContainsKey(path))
			//							{
			//								img = sELeditCache.Instance.sELeditDatas.database.images(path);
			//							}
			//						}
			//						dataGridView_elems.Rows[selIndices[i]].Cells[0].Value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, selIndices[i], 0);
			//						dataGridView_elems.Rows[selIndices[i]].Cells[1].Value = img;
			//						dataGridView_elems.Rows[selIndices[i]].Cells[2].Value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, selIndices[i], pos);
			//					}
			//					else
			//					{
			//						dataGridView_elems.Rows[selIndices[i]].Cells[0].Value = "";
			//						dataGridView_elems.Rows[selIndices[i]].Cells[1].Value = Properties.Resources.unknown;
			//						dataGridView_elems.Rows[selIndices[i]].Cells[2].Value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, selIndices[i], pos);
			//					}
			//				}
			//			}
			//		}
			//	}
			//}

		}

		private void dataGridView_elems_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs ee)
		{
			if (!(sELeditCache.Instance.sELeditDatas?.database?.item_color is null))
			{

				Color colorSelected = Color.White;
				try
				{
					int ID = int.Parse(dataGridView_elems.Rows[ee.RowIndex].Cells[0].Value.ToString());
					if (sELeditCache.Instance.sELeditDatas.database.item_color.ContainsKey(ID))
					{
						colorSelected = Helper.getByID(sELeditCache.Instance.sELeditDatas.database.item_color[ID]);
					}
					else
					{
						colorSelected = Color.White;
					}
				}

				catch (Exception e)
				{ e.ErrorGet(false); colorSelected = Color.White; }


				try
				{
					DisplayValueItemElem temp = ((DisplayValueItemElem)dataGridView_elems.Rows[ee.RowIndex].Cells[2].Value);
					if (temp.RealValue.ToString().StartsWith("^"))
					{
						colorSelected = Extensions.ColorHex(temp.RealValue.ToString());
					}
				}
				catch (Exception ex) { ex.ErrorGet(false); colorSelected = Color.White; }


				dataGridView_elems.Rows[ee.RowIndex].Cells[2].Style.ForeColor = colorSelected;
				dataGridView_elems.Rows[ee.RowIndex].Cells[2].Style.SelectionForeColor = colorSelected;


				if (sELeditCache.Instance.sELeditDatas.eLC.Lists[(int)comboBox_lists.SelectedValue].isIcon)
				{
					try
					{

						dataGridView_elems.Rows[ee.RowIndex].Cells[1].Value = ExCore.GetImageItem((int)comboBox_lists.SelectedValue, ee.RowIndex);

					}
					catch (Exception ex) { ex.ErrorGet(false); }
				}




			}
		}

		private void dataGridView_item_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			DataGridView dgv = (DataGridView)sender;
			if (comboBox_lists.SelectedIndex != 54 && comboBox_lists.SelectedIndex != 40)
			{

				if (dgv.Rows.Count > 0)
				{

					try
					{
						string ID_ITEM = Extensions.GetIdItemFromGDV(dgv.Rows[e.RowIndex].Cells[2].Value.ToString()).ToString();
						string NAME_ITEM = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
						string TYPE_ITEM = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();

						if (ID_ITEM != "0")
						{

							bool fi = false;
							bool fini = false;



							switch (NAME_ITEM)
							{
								case string x when ((x.EndsWith("_id_addon") || x.StartsWith("skills_") || x.StartsWith("after_death") || x.StartsWith("skill_hp") || x.EndsWith("_id_unique") || x.EndsWith("_id_rand")) && TYPE_ITEM.StartsWith("int32")):
									dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + EQUIPMENT_ADDON.GetAddon(ID_ITEM.ToString());
									break;

								case string x when (x.StartsWith("addons_") && x.EndsWith("_id")):
									break;

								case string x when (x != ("character_combo_id") && x.EndsWith("element_id") || x.EndsWith("id_to_make") || x.StartsWith("id_upgrade_equip") || x.StartsWith("id_drop")
									|| (x.StartsWith("materials_") && x.EndsWith("_id")) || (x.StartsWith("equipments_") && x.EndsWith("_id")) || (x.StartsWith("drop_matters") && x.EndsWith("_id"))):

									if (ID_ITEM != "0")
									{
										try
										{
											for (int L = 0; L < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length; L++)
											{
												if (sELeditCache.Instance.sELeditDatas.eLC.Lists[L].itemUse == true)
												{
													int La = L;
													int pos = 0;
													int posN = 0;
													for (int i = 0; i < sELeditCache.Instance.sELeditDatas.eLC.Lists[La].elementFields.Length; i++)
													{
														if (sELeditCache.Instance.sELeditDatas.eLC.Lists[La].elementFields[i] == "Name")
														{
															posN = i;

														}
														if (sELeditCache.Instance.sELeditDatas.eLC.Lists[La].elementFields[i] == "file_icon")
														{
															pos = i;
															break;
														}

													}

													for (int ef = 0; ef < sELeditCache.Instance.sELeditDatas.eLC.Lists[La].elementValues.Length; ef++)
													{

														if (ID_ITEM == sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, 0))
														{
															if (sELeditCache.Instance.sELeditDatas.database.sourceBitmap != null && sELeditCache.Instance.sELeditDatas.database.ContainsKey(Path.GetFileName(sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, pos))))
															{
																if (sELeditCache.Instance.sELeditDatas.database.ContainsKey(Path.GetFileName(sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, pos))))
																{
																	//if (dgv.Rows[e.RowIndex].Cells[2].Value.ToString() == "0")
																	//{
																	//    ((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[2]).Image = Extensions.ResizeImage(null, 0, 0);
																	//}
																	//else
																	//{
																	((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[2]).Image = Extensions.ResizeImage(sELeditCache.Instance.sELeditDatas.database.images(Path.GetFileName(sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, pos))), 18, 18);
																	//}

																	dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, posN);
																	fi = true;

																	Color clr;
																	try
																	{ clr = Helper.getByID(sELeditCache.Instance.sELeditDatas.database.item_color[int.Parse(ID_ITEM.ToString())]); }
																	catch (Exception)
																	{ clr = Color.White; }

																	dgv.Rows[e.RowIndex].Cells[2].Style.ForeColor = clr;
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


										}
										catch (Exception ex)
										{

											//MessageBox.Show(ex.Message + "\n" + linha);
										}
									}
									break;

								case "character_combo_id":
									for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[3].elementFields.Length; k++)
									{
										if (sELeditCache.Instance.sELeditDatas.eLC.Lists[3].elementFields[k] == "character_combo_id")
										{
											dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + Extensions.DecodingCharacterComboId(ID_ITEM.ToString());
											break;
										}
									}
									break;

								case "proc_type":
									for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[3].elementFields.Length; k++)
									{
										if (sELeditCache.Instance.sELeditDatas.eLC.Lists[3].elementFields[k] == "proc_type")
										{
											dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + Extensions.Get_proc_type(ID_ITEM);
											break;
										}
									}
									break;

								case "id_major_type":
									for (int l = 0; l < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length; l++)
									{
										string major = sELeditCache.Instance.sELeditDatas.eLC.Lists[comboBox_lists.SelectedIndex].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1].Replace("ESSENCE", "MAJOR_TYPE");
										string conf = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1];
										if (major == conf)
										{
											for (int m = 0; m < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length; m++)
											{
												if (int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 0)) == int.Parse(ID_ITEM))
												{
													dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 1);
													fini = true;
												}


												if (fini)
												{
													break;
												}
											}
											if (fini)
											{
												break;
											}

										}
									}
									break;

								case "id_sub_type":
									for (int l = 0; l < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length; l++)
									{
										string major = sELeditCache.Instance.sELeditDatas.eLC.Lists[comboBox_lists.SelectedIndex].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1].Replace("ESSENCE", "SUB_TYPE");
										string conf = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1];
										if (major == conf)
										{
											for (int m = 0; m < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length; m++)
											{
												if (int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 0)) == int.Parse(ID_ITEM))
												{
													dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 1);
													fini = true;
												}

												if (fini)
												{
													break;
												}
											}
											if (fini)
											{
												break;
											}

										}
									}
									break;

								case string x when (x.StartsWith("addon_") && !x.EndsWith("rate")):
									dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + EQUIPMENT_ADDON.GetAddon(ID_ITEM);
									break;

								case "id_tasks_":
									for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.Tasks.Length; i++)
									{
										if (sELeditCache.Instance.sELeditDatas.database.Tasks[i].ID == int.Parse(ID_ITEM))
										{
											dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.database.Tasks[i].Name;
											break;
										}

									}
									break;

								case "id_type":
									for (int l = 0; l < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length; l++)
									{
										string major = sELeditCache.Instance.sELeditDatas.eLC.Lists[comboBox_lists.SelectedIndex].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1].Replace("ESSENCE", "TYPE");
										string conf = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1];
										if (major == conf)
										{
											for (int m = 0; m < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length; m++)
											{
												if (int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 0)) == int.Parse(ID_ITEM))
												{
													dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 1);
													fini = true;
												}

												if (fini)
												{
													break;
												}
											}
											if (fini)
											{
												break;
											}

										}
									}
									break;

								case string x when (x.StartsWith("task_lists_") && x.EndsWith("_id")):
									for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.Tasks.Length; i++)
									{
										if (sELeditCache.Instance.sELeditDatas.database.Tasks[i].ID == int.Parse(ID_ITEM))
										{

											ImageList imageList1 = new ImageList();
											string[] arquivos = Directory.GetFiles(Application.StartupPath + @"\images", "*.png", SearchOption.TopDirectoryOnly);
											for (int fd = 0; fd < arquivos.Length; fd++)
											{
												imageList1.Images.Add(Image.FromFile(arquivos[fd]));

											}



											string asds = sELeditCache.Instance.sELeditDatas.database.Tasks[i].m_ulType.ToString();
											((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[2]).Image = Extensions.ResizeImage(imageList1.Images[5], 32, 21);
											//MessageBox.Show(sELeditCache.Instance.sELeditDatas.database.Tasks[i].m_ulType);
											dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.database.Tasks[i].Name;
											break;
										}

									}
									break;

								#region 57

								case "id_make_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[54].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(54, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(54, xe, 1);
												break;
											}




										}
									}
									break;
								case "id_buy_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[41].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(41, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(41, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_sell_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[40].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(40, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(40, xe, 1);
												break;
											}

										}
									}
									break;
								case "id_repair_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[42].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(42, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(42, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_install_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[43].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(43, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(43, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_uninstall_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[44].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(44, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(44, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_task_out_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[46].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(46, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(46, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_task_in_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[45].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(45, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(45, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_task_matter_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[47].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(47, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(47, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_skill_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[48].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(48, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(48, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_heal_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[49].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(49, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(49, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_transmit_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[50].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(50, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(50, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_proxy_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[52].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(52, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(52, xe, 1);
												break;
											}
										}
									}
									break;
								case "id_storage_service":
									{
										for (int xe = 0; xe < sELeditCache.Instance.sELeditDatas.eLC.Lists[53].elementValues.Length; xe++)
										{
											if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(53, xe, 0) == ID_ITEM)
											{
												dgv.Rows[e.RowIndex].Cells[2].Value = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(53, xe, 1);
												break;
											}
										}
									}
									break;
								#endregion


								default:
									break;
							}
						}

					}

					catch (Exception exd)
					{


					}



				}

			}

		}

		private void dataGridView_item_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
		{
			DataGridView dgv = (DataGridView)sender;
			if (comboBox_lists.SelectedIndex == 54 || comboBox_lists.SelectedIndex == 40)
			{

				try
				{
					int ID = Extensions.GetIdItemFromGDV(dgv.Rows[e.RowIndex].Cells[2].Value.ToString());
					string NameP = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
					string Type = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();

					if (ID != 0)
					{
						switch (NameP)
						{
							case string x when (x.StartsWith("pages_") && Type.StartsWith("int32") && comboBox_lists.SelectedIndex == 54):

								((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[2]).Image = Extensions.ResizeImage(Extensions.IdImageRecipe(ID, out int idiTEM), 18, 18);
								dgv.Rows[e.RowIndex].Cells[2].Value = Extensions.SetIdNameRecipeFromGDV(ID.ToString());

								break;
							case string x when (x.StartsWith("pages_") && x.EndsWith("_id")):
								((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[2]).Image = Extensions.ResizeImage(Extensions.IdImageItem(ID), 18, 18);
								dgv.Rows[e.RowIndex].Cells[2].Value = Extensions.SetIdNameItemFromGDV(ID.ToString());
								break;
							default:
								break;
						}

					}
					else
					{
						Image x = null;
						((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[2]).Image = Extensions.ResizeImage(x, 0, 0);


					}

				}
				catch (Exception de)
				{


				}



			}
		}

		private void dataGridView_elems_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					var hti = dataGridView_elems.HitTest(e.X, e.Y);
					dataGridView_elems.ClearSelection();
					dataGridView_elems.Rows[hti.RowIndex].Selected = true;

				}
			}
			catch (Exception)
			{


			}
		}


		private void dataGridView_elems_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			if (customTooltype != null)
				customTooltype.Close();
		}
		private void dataGridView_elems_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			// int Id = Extensions.GetIdItemFromGDV(((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString());
			try
			{
				if (customTooltype != null)
					customTooltype.Close();
			}
			catch { }

			InfoTool ift = null;
			try
			{
				int l = comboBox_lists.SelectedIndex;
				int xe = e.RowIndex;
				int Id = Extensions.GetIdItemFromGDV(((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString());
				if (Id > 0)
				{
					ift = Extensions.GetItemPropsByILP(Id, 0, l, xe);
				}
				if (ift == null)
				{
					string text = Extensions.GetItemProps(Id, 0);
					text += Extensions.ItemDesc(Id);
					this.dataGridView_elems.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = text;
				}
				else
				{
					ift.description = Extensions.ItemDesc(Id);// Extensions.ColorClean(Extensions.ItemDesc(Id));
					customTooltype = new IToolType(ift);
					customTooltype.Show(this);
				}
			}
			catch
			{
			}


		}
		private void dataGridView_item_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (comboBox_lists.SelectedIndex == 74)
			{
				try
				{
					if (customTooltype != null)
						customTooltype.Close();

					int IdListRecipe = Extensions.GetIdItemFromGDV(((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString());
					InfoTool ift = null;
					ift = Extensions.GetItemPropsFromID(IdListRecipe);
					if (ift == null)
					{
						string text = Extensions.GetItemProps(IdListRecipe, 0);
						text += Extensions.ItemDesc(IdListRecipe);
						//this.dataGridView_item.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = text;
					}
					else
					{
						ift.description = Extensions.ItemDesc(IdListRecipe);
						customTooltype = new IToolType(ift);
						customTooltype.Show(this);
					}
				}
				catch (Exception)
				{


				}
			}
			else
			{
				try
				{
					if (customTooltype != null)
						customTooltype.Close();
				}
				catch { }
				if (e.ColumnIndex >= 0 && e.ColumnIndex == 2 && e.RowIndex > -1)
				{
					InfoTool ift = null;
					try
					{

						int Id = Extensions.GetIdItemFromGDV(((DataGridView)sender).Rows[e.RowIndex].Cells[2].Value.ToString());

						string a = null;// dataGridView_item.Rows[e.RowIndex].Cells[0].Value.ToString();
						var cfd = a.Split(new string[] { "_" }, StringSplitOptions.None);

						if (Id > 0)
						{

							if (a != ("character_combo_id") && a.EndsWith("element_id") || a.EndsWith("id_to_make") || a.StartsWith("id_upgrade_equip") || a.StartsWith("id_drop")
								|| (a.StartsWith("materials_") && a.EndsWith("_id")) || (a.StartsWith("equipments_") && a.EndsWith("_id")) || (a.StartsWith("pages_") && a.EndsWith("_id")))
							{
								ift = Extensions.GetItemPropsFromID(Id);
							}
							if (a.StartsWith("pages_") && int.TryParse(cfd[cfd.Length - 1], out int x) == true)
							{
								int idx;
								Extensions.IdImageRecipe(Id, out idx, true);
								ift = Extensions.GetItemPropsFromID(idx);
							}


						}
						if (ift == null)
						{
							string text = Extensions.GetItemProps(Id, 0);
							text += Extensions.ItemDesc(Id);
							//this.dataGridView_item.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = text;
						}
						else
						{
							ift.description = Extensions.ItemDesc(Id);// Extensions.ColorClean(Extensions.ItemDesc(Id));
							customTooltype = new IToolType(ift);
							customTooltype.Show(this);
						}





					}
					catch
					{
						//MessageBox.Show(esxfd.Message);
					}
				}
			}
		}
		private void dataGridView_elems_CellMouseLeave_1(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (customTooltype != null)
					customTooltype.Close();
			}
			catch { }
		}
		private void dataGridView_elems_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (customTooltype != null)
					customTooltype.Close();
			}
			catch { }
			try
			{
				int ID = Extensions.GetIdItemFromGDV(((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString());
				int l = comboBox_lists.SelectedIndex;
				int xe = e.RowIndex;

				if (ID > 0)
				{
					InfoTool ift = null;
					try
					{

						if (ID > 0)
						{
							ift = Extensions.GetItemPropsByILP(ID, 0, l, xe);
						}
						if (ift == null)
						{
							string text = Extensions.GetItemProps(ID, 0);
							text += Extensions.ItemDesc(ID);
						}
						else
						{
							ift.description = Extensions.ColorClean(Extensions.ItemDesc(ID));
							customTooltype = new IToolType(ift);
							customTooltype.Show(this);
						}




					}
					catch
					{
					}
				}
			}
			catch { }



		}

		private void dataGridView_item_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
		{


			if (e.ColumnIndex == 2)
			{
				// dataGridView_item.Refresh();
				//dataGridView_item.CancelEdit();

			}


		}

		private void dataGridView_item_DoubleClick(object sender, EventArgs e)
		{
			DataGridView gridView = (DataGridView)sender;
			SendKeys.Send("{ESC}");
			string retur = string.Empty;
			try
			{
				string a = gridView.Rows[gridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
				//string b = gridView.Rows[gridView.SelectedCells[0].RowIndex].Cells[1].Value.ToString();
				string c = gridView.Rows[gridView.SelectedCells[0].RowIndex].Cells[2].Value.ToString();


				if (a.EndsWith("_id_to_make") ||
					(a.StartsWith("materials_") && a.EndsWith("_id")) ||
					a.StartsWith("element_id") ||
					a.StartsWith("id_upgrade_equip") ||
					a.StartsWith("id_drop_after_damaged") ||
					(comboBox_lists.SelectedIndex == 90 && a.StartsWith("equipments_")) ||
					(a.StartsWith("drop_matters_") && a.EndsWith("_id")) ||
					(a.StartsWith("pages_") && a.EndsWith("_id")))
				{
					select = new Select_id();
					select.input = Extensions.GetIdItemFromGDV(c);
					select.ShowDialog(this);
					retur = select.retorn.ToString();

				}
				if (a.StartsWith("character_combo_id"))
				{
					eClassMask = new ClassMaskWindow();

					eClassMask.input = Extensions.GetIdItemFromGDV(c);
					eClassMask.ShowDialog(this);
					retur = eClassMask.GET.ToString();
				}
				if (a.Contains("_id_goods_"))
				{
					rec = new RECIPES();

					rec.input = Extensions.GetIdItemFromGDV(c);
					rec.ShowDialog(this);
					retur = rec.GET.ToString();
				}
				if (a == "proc_type")
				{
					procType = new ProcTypeGenerator();


					procType.input = Extensions.GetIdItemFromGDV(c);
					procType.ShowDialog(this);
					retur = procType.GET.ToString();
				}
				if (a.EndsWith("_id_rand") || a.EndsWith("_id_addon") || a.EndsWith("_id_unique") || a.EndsWith("addon_"))
				{
					setADD = new set_ADDONS();

					setADD.input = Extensions.GetIdItemFromGDV(c);
					setADD.gINDEX = comboBox_lists.SelectedIndex;
					setADD.ShowDialog(this);
					retur = setADD.GET.ToString();
				}
				if (a.StartsWith("file_model_right") || a.StartsWith("file_model_left") || a.StartsWith("file_matter") || a.StartsWith("file_model"))
				{
					//var result = Extensions.ViewerSKI(c);

					var fds = new Previews.SkiViewerModel(c);
					fds.FILE = c;
					//fds.SKI = result.Item1;
					//fds._texturesBytes = result.Item2;
					fds.Show(this);





				}



				if (a.StartsWith("id_major_type") || a.StartsWith("id_sub_type"))
				{
					major_Sub = new _major_sub();

					major_Sub.ID = Extensions.GetIdItemFromGDV(c);
					major_Sub.LIST = comboBox_lists.SelectedIndex;
					major_Sub.TYPE = a;
					major_Sub.ShowDialog(this);
					retur = major_Sub.GET.ToString();
				}





				if (int.Parse(retur) != 0)
				{
					gridView.Rows[gridView.SelectedCells[0].RowIndex].Cells[2].Value = retur;
				}
				SendKeys.Send("{ESC}");
			}
			catch { }
			finally
			{
				SendKeys.Send("{ESC}");
			}
		}

		private void dataGridView_recipes_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
		{

			for (int i = 2; i < 6; i++)
			{
				if (Extensions.GetIdItemFromGDV(dataGridView_recipes.Rows[e.RowIndex].Cells[i].Value.ToString()) > 0)
				{
					Color clr = Color.White;
					string s_i_ts = "";
					try
					{
						bool verificar = false;
						try
						{
							string ts = dataGridView_recipes.Rows[e.RowIndex].Cells[i].Value.ToString();

							int i_ts = ts.IndexOf("^");
							if (i_ts != -1)
							{
								s_i_ts = ts.Substring(i_ts, 7);
								clr = Extensions.ColorHex(ts.Substring(i_ts, ts.Length - i_ts));
								verificar = true;
							}


						}
						catch { }

						if (verificar) { }
						else
						{
							clr = Helper.getByID(sELeditCache.Instance.sELeditDatas.database.item_color[Extensions.GetIdItemFromGDV(dataGridView_recipes.Rows[e.RowIndex].Cells[i].Value.ToString())]);
						}
					}
					catch (Exception ex)
					{ clr = Color.White; }

					dataGridView_recipes.Rows[e.RowIndex].Cells[i].Style.ForeColor = clr;

					((TextAndImageCell)dataGridView_recipes.Rows[e.RowIndex].Cells[i]).Image =
					 Extensions.ResizeImage(Extensions.IdImageItem(Extensions.GetIdItemFromGDV(dataGridView_recipes.Rows[e.RowIndex].Cells[i].Value.ToString())), 32, 32);
					dataGridView_recipes.Rows[e.RowIndex].Cells[i].Value = Extensions.SetIdNameItemFromGDV(Extensions.GetIdItemFromGDV(dataGridView_recipes.Rows[e.RowIndex].Cells[i].Value.ToString()).ToString());


				}
			}


		}

		private void dataGridView_item_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			if ((t != null) && t.Visible)
			{
				this.t.Hide();
			}
		}

		private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
		{
			DataGridView dgv = (DataGridView)sender;
			string a, tt; int b = 0;
			var tt_1 = dgv.Rows[e.RowIndex].Cells[0].Value.ToString().Replace("[", "").Replace("]", "").Split('-');
			a = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
			int ret;
			tt = tt_1[0];
			if (int.TryParse(tt, out ret))
			{
				b = int.Parse(tt_1[0].Replace(" ", ""));
			}


			if (b != 0)
			{
				try
				{
					((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[0]).Image = Extensions.ResizeImage(Extensions.IdImageItem(int.Parse(tt_1[0].Replace(" ", ""))), 32, 32);
					dgv.Rows[e.RowIndex].Cells[0].Value = "[" + tt_1[0].Replace(" ", "") + "] - " + Extensions.IdNameItem(int.Parse(tt_1[0].Replace(" ", "")));

					Color clr;
					try
					{ clr = Helper.getByID(sELeditCache.Instance.sELeditDatas.database.item_color[int.Parse(b.ToString())]); }
					catch (Exception)
					{ clr = Color.White; }

					dgv.Rows[e.RowIndex].Cells[0].Style.ForeColor = clr;

				}
				catch (Exception ex)
				{


				}


			}
		}

		private void dataGridView_tasks_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (sELeditCache.Instance.sELeditDatas.database.Tasks != null
)
				{

					dataGridView1.Rows.Clear();
					for (int t = 0; t < sELeditCache.Instance.sELeditDatas.database.Tasks.Length; t++)
					{
						if (sELeditCache.Instance.sELeditDatas.database.Tasks[t].ID == int.Parse(dataGridView_tasks.Rows[e.RowIndex].Cells[0].Value.ToString().Replace("[", "").Replace("]", "").Split(new string[] { " - " }, StringSplitOptions.None)[0].ToString().Replace(" ", "")))
						{
							for (int m = 0; m < sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems.Length; m++)
							{
								for (int i = 0; i < sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems.Length; i++)
								{

									dataGridView1.Rows.Add(new object[] { sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_ulItemTemplId, sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_ulItemNum + " UN", Convert.ToDecimal(sELeditCache.Instance.sELeditDatas.database.Tasks[t].m_Award_S.m_CandItems[m].m_AwardItems[i].m_fProb * 100) + "%  " });

								}
								break;
							}
							break;
						}

					}
				}
			}
			catch (Exception)
			{


			}
		}

		private void dataGridView_SUITE_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
		{
			if ((t2 != null) && t2.Visible)
			{
				this.t2.Hide();
			}
		}

		private void dataGridView_SUITE_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				int IdListRecipe = int.Parse(dataGridView_SUITE.Rows[e.RowIndex].Cells[0].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace(" ", ""));
				IntPtr handle = ((Control)sender).Handle;
				this.t2.ShowToolTip(handle, IdListRecipe);
			}
			catch (Exception)
			{


			}
		}

		private void dataGridView_SUITE_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			int idRecipe = int.Parse(dataGridView_SUITE.Rows[e.RowIndex].Cells[0].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None)[0].Replace(" ", ""));

			comboBox_lists.SelectedIndex = 90;

			foreach (DataGridViewRow item in dataGridView_elems.Rows)
			{
				var a = item.Cells[0].Value;
				if (item.Cells[0].Value.ToString() == idRecipe.ToString())
				{
					//dataGridView_item.Rows.Clear();
					dataGridView_elems.Rows[item.Index].Selected = true;
					dataGridView_elems.CurrentCell = dataGridView_elems.Rows[item.Index].Cells[0];
					change_item(null, null);
					break;
				}
			}
		}

		private void dataGridView_npcs_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			int idRecipe = int.Parse(dataGridView_npcs.Rows[e.RowIndex].Cells[0].Value.ToString());

			comboBox_lists.SelectedIndex = 57;

			foreach (DataGridViewRow item in dataGridView_elems.Rows)
			{
				var a = item.Cells[0].Value;
				if (item.Cells[0].Value.ToString() == idRecipe.ToString())
				{
					//dataGridView_item.Rows.Clear();
					dataGridView_elems.Rows[item.Index].Selected = true;
					dataGridView_elems.CurrentCell = dataGridView_elems.Rows[item.Index].Cells[0];
					change_item(null, null);
					break;
				}
			}
		}

		private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				textBox_search.Clear();
				textBox_search.Focus();
				textBox_search.Text = Extensions.GetIdItemFromGDV(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString()).ToString();
				SendKeys.SendWait("{ENTER}");
			}
			catch { }
		}

		private void dataGridView_recipes_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{

			int idRecipe = int.Parse(dataGridView_recipes.Rows[e.RowIndex].Cells[0].Value.ToString());

			comboBox_lists.SelectedIndex = 69;
			//dataGridView_item.Rows.Clear();

			foreach (DataGridViewRow item in dataGridView_elems.Rows)
			{
				var a = item.Cells[0].Value;
				if (item.Cells[0].Value.ToString() == idRecipe.ToString())
				{

					dataGridView_elems.Rows[item.Index].Selected = true;
					dataGridView_elems.CurrentCell = dataGridView_elems.Rows[item.Index].Cells[0];
					change_item(null, null);
					break;
				}
			}

		}
		#endregion

		#region Search
		private void textBox_search_KeyPress(object sender, KeyPressEventArgs xe)
		{
			CheckBox checkBox_SearchMatchCase = new CheckBox();
			CheckBox checkBox_SearchAll = new CheckBox();
			CheckBox checkBox_SearchExactMatching = new CheckBox();

			if (xe.KeyChar == (char)Keys.Enter)
			{
				string id = textBox_search.Text;
				if (!checkBox_SearchMatchCase.Checked)
					id = id.ToLower();
				string value = "";
				int l = comboBox_lists.SelectedIndex;
				if (l < 0) { l = 0; }
				int f = 0;
				//if (dataGridView_item.CurrentCell != null)
				//	f = dataGridView_item.CurrentCell.RowIndex + 1;
				if (f < 0) { f = 0; }
				if (sELeditCache.Instance.sELeditDatas.eLC != null && sELeditCache.Instance.sELeditDatas.eLC.Lists != null)
				{
					//EnableSelectionItem = false;
					int ftmp = f;
					if (checkBox_SearchAll.Checked)
					{
						int e = dataGridView_elems.CurrentCell.RowIndex;
						if (e < 0) { e = 0; }
						int etmp = e;
						for (int lf = l; lf < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length; lf++)
						{
							for (int ef = etmp; ef < sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementValues.Length; ef++)
							{
								for (int ff = ftmp; ff < sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementFields.Length; ff++)
								{
									if (checkBox_SearchExactMatching.Checked)
									{
										if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, ff) == id)
										{
											comboBox_lists.SelectedIndex = lf;
											dataGridView_elems.ClearSelection();
											//EnableSelectionItem = true;
											dataGridView_elems.CurrentCell = dataGridView_elems[0, ef];
											dataGridView_elems.Rows[ef].Selected = true;
											dataGridView_elems.FirstDisplayedScrollingRowIndex = ef;
											//dataGridView_item.CurrentCell = dataGridView_item.Rows[ff].Cells[2];
											return;
										}
									}
									else
									{
										value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, ff);
										if (!checkBox_SearchMatchCase.Checked)
											value = value.ToLower();
										if (value.Contains(id))
										{
											comboBox_lists.SelectedIndex = lf;
											dataGridView_elems.ClearSelection();
											//EnableSelectionItem = true;
											dataGridView_elems.CurrentCell = dataGridView_elems[0, ef];
											dataGridView_elems.Rows[ef].Selected = true;
											dataGridView_elems.FirstDisplayedScrollingRowIndex = ef;
											//dataGridView_item.CurrentCell = dataGridView_item.Rows[ff].Cells[2];
											return;
										}
									}
								}
								ftmp = 0;
							}
							etmp = 0;
						}
						etmp = e;
						ftmp = f;
						for (int lf = 0; lf < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length && lf <= l; lf++)
						{
							for (int ef = 0; ef < sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementValues.Length; ef++)
							{
								for (int ff = 0; ff < sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementFields.Length; ff++)
								{
									if (checkBox_SearchExactMatching.Checked)
									{
										if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, ff) == id)
										{
											comboBox_lists.SelectedIndex = lf;
											dataGridView_elems.ClearSelection();
											//EnableSelectionItem = true;
											dataGridView_elems.CurrentCell = dataGridView_elems[0, ef];
											dataGridView_elems.Rows[ef].Selected = true;
											dataGridView_elems.FirstDisplayedScrollingRowIndex = ef;
											//dataGridView_item.CurrentCell = dataGridView_item.Rows[ff].Cells[2];
											return;
										}
									}
									else
									{
										value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, ff);
										if (!checkBox_SearchMatchCase.Checked)
											value = value.ToLower();
										if (value.Contains(id))
										{
											comboBox_lists.SelectedIndex = lf;
											dataGridView_elems.ClearSelection();
											//EnableSelectionItem = true;
											dataGridView_elems.CurrentCell = dataGridView_elems[0, ef];
											dataGridView_elems.Rows[ef].Selected = true;
											dataGridView_elems.FirstDisplayedScrollingRowIndex = ef;
											//dataGridView_item.CurrentCell = dataGridView_item.Rows[ff].Cells[2];
											return;
										}
									}
								}
								ftmp = 0;
							}
							etmp = 0;
						}
					}
					else
					{
						int e = dataGridView_elems.CurrentCell.RowIndex + 1;
						if (e < 0) { e = 0; }
						int etmp = e;
						for (int lf = l; lf < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length; lf++)
						{
							int pos = 0;
							for (int i = 0; i < sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementFields.Length; i++)
							{
								if (sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementFields[i] == "Name")
								{
									pos = i;
									break;
								}
							}
							for (int ef = etmp; ef < sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementValues.Length; ef++)
							{
								if (checkBox_SearchExactMatching.Checked)
								{
									if (id == sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, 0) || sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, pos) == id)
									{
										comboBox_lists.SelectedIndex = lf;
										dataGridView_elems.ClearSelection();
										//EnableSelectionItem = true;
										dataGridView_elems.CurrentCell = dataGridView_elems[0, ef];
										dataGridView_elems.Rows[ef].Selected = true;
										dataGridView_elems.FirstDisplayedScrollingRowIndex = ef;
										return;
									}
								}
								else
								{
									value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, pos);
									if (!checkBox_SearchMatchCase.Checked)
										value = value.ToLower();
									if (id == sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, 0) || value.Contains(id))
									{
										comboBox_lists.SelectedIndex = lf;
										dataGridView_elems.ClearSelection();
										//EnableSelectionItem = true;
										dataGridView_elems.CurrentCell = dataGridView_elems[0, ef];
										dataGridView_elems.Rows[ef].Selected = true;
										dataGridView_elems.FirstDisplayedScrollingRowIndex = ef;
										change_item(null, null);
										return;
									}
								}
							}
							etmp = 0;
						}
						etmp = e;
						for (int lf = 0; lf < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length && lf <= l; lf++)
						{
							int pos = 0;
							for (int i = 0; i < sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementFields.Length; i++)
							{
								if (sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementFields[i] == "Name")
								{
									pos = i;
									break;
								}
							}
							for (int ef = 0; ef < sELeditCache.Instance.sELeditDatas.eLC.Lists[lf].elementValues.Length; ef++)
							{
								if (checkBox_SearchExactMatching.Checked)
								{
									if (id == sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, 0) || sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, pos) == id)
									{
										comboBox_lists.SelectedIndex = lf;
										dataGridView_elems.ClearSelection();
										//EnableSelectionItem = true;
										dataGridView_elems.CurrentCell = dataGridView_elems[0, ef];
										dataGridView_elems.Rows[ef].Selected = true;
										dataGridView_elems.FirstDisplayedScrollingRowIndex = ef;
										return;
									}
								}
								else
								{
									value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, pos);
									if (!checkBox_SearchMatchCase.Checked)
										value = value.ToLower();
									if (id == sELeditCache.Instance.sELeditDatas.eLC.GetValue(lf, ef, 0) || value.Contains(id))
									{
										comboBox_lists.SelectedIndex = lf;
										dataGridView_elems.ClearSelection();
										//EnableSelectionItem = true;
										dataGridView_elems.CurrentCell = dataGridView_elems[0, ef];
										dataGridView_elems.Rows[ef].Selected = true;
										dataGridView_elems.FirstDisplayedScrollingRowIndex = ef;
										return;
									}
								}
							}
							etmp = 0;
						}
					}
					//EnableSelectionItem = true;
					MessageBox.Show("Search reached End without Result!");
				}
			}
		}

		private void textBox_search_enter(object sender, EventArgs e)
		{
			if (textBox_search.Text == "ID or NAME")
			{
				textBox_search.Clear();
			}
		}

		private void textBox_search_leave(object sender, EventArgs e)
		{
			if (textBox_search.Text == "")
			{
				textBox_search.Text = "ID or NAME";
			}
		}

		private void textBox_value_enter(object sender, EventArgs e)
		{
			//if (textBox_SetValue.Text == "Set Value")
			//{
			//	textBox_SetValue.Clear();
			//}
		}

		private void textBox_value_leave(object sender, EventArgs e)
		{
			//if (textBox_SetValue.Text == "")
			//{
			//	textBox_SetValue.Text = "Set Value";
			//}
		}
		#endregion

		public void SetText(string Text)
		{
			try
			{
				string output;
				output = Text.Replace("\\r", "\n");
				output = output.Replace("\\", "\n");
				List<string> colors = new List<string>();
				List<int> Symbol = new List<int>();
				for (int Index = 0; Index < output.Length; ++Index)
				{
					int b = output.IndexOf("^", Index);
					if (b >= 0)
					{
						colors.Add(output.Substring(b + 1, 6));
						output = output.Remove(b, 7);
						Symbol.Add(b);
					}
				}
				richTextBox_DESC_POS.Text = output;
				for (int b = 0; b < Symbol.Count; ++b)
				{
					richTextBox_DESC_POS.Select(Symbol[b], richTextBox_DESC_POS.Text.Length);
					Color col = ColorTranslator.FromHtml("#" + colors[b]);
					richTextBox_DESC_POS.SelectionColor = col;
				}

			}
			catch
			{
				richTextBox_DESC_POS.Text = "Text parse error";
			}
		}


		private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			//try
			//{
			//	if (AssetManagerLoad.ThreadState == System.Threading.ThreadState.Running)
			//	{
			//		AssetManagerLoad.Abort();

			//	}
			//}
			//catch (Exception)
			//{


			//}

		}

		#region contextMenuStrip_items

		private void addItemRecipeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			////DataGridView gridView = new DataGridView();
			//var gridView = dataGridView_elems;

			//int idItem = int.Parse(gridView.Rows[gridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
			//int idListItem = comboBox_lists.SelectedIndex;

			//int idListReceipe = 0;
			//for (int i = 0; i < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length; i++)
			//{
			//	//RECIPE_ESSENCE
			//	if (sELeditCache.Instance.sELeditDatas.eLC.Lists[i].listName.EndsWith("RECIPE_ESSENCE"))
			//	{
			//		idListReceipe = i;
			//		break;
			//	}
			//}

			//int idClone = int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(idListReceipe, sELeditCache.Instance.sELeditDatas.eLC.Lists[idListReceipe].elementValues.Length - 1, 0));

			//string NameClone = gridView.Rows[gridView.SelectedCells[2].RowIndex].Cells[2].Value.ToString();

			//Encoding enc = Encoding.GetEncoding("Unicode");
			//string type = "wstring:64";
			///// Encoding enc = Encoding.GetEncoding("Unicode");
			//byte[] target = new byte[Convert.ToInt32(type.Substring(8))];
			//byte[] source = enc.GetBytes(NameClone);
			//if (target.Length > source.Length)
			//{
			//	Array.Copy(source, target, source.Length);
			//}
			//else
			//{
			//	Array.Copy(source, target, target.Length);
			//}

			//int IDITEM;

			//if (idItem != 0)
			//{
			//	if (dataGridView_elems.RowCount > 0)
			//	{
			//		if (idListReceipe != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
			//		{
			//			int[] selIndices = gridSelectedIndices(dataGridView_elems);
			//			EnableSelectionList = false;
			//			EnableSelectionItem = false;

			//			IDITEM = int.Parse(dataGridView_elems.Rows[dataGridView_elems.RowCount - 1].Cells[0].Value.ToString());
			//			for (int i = 0; i < selIndices.Length; i++)
			//			{
			//				object[] o = new object[sELeditCache.Instance.sELeditDatas.eLC.Lists[idListReceipe].elementValues[0].Length];
			//				sELeditCache.Instance.sELeditDatas.eLC.Lists[idListReceipe].elementValues[0].CopyTo(o, 0);

			//				o[0] = idClone + 1;
			//				o[3] = target;
			//				o[8] = idItem;
			//				o[9] = float.Parse("1");
			//				for (int jj = 10; jj < sELeditCache.Instance.sELeditDatas.eLC.Lists[69].elementTypes.Length; jj++)
			//				{
			//					string ts = sELeditCache.Instance.sELeditDatas.eLC.Lists[69].elementTypes[jj].ToString();
			//					if (ts == "int32")
			//					{
			//						o[jj] = int.Parse("0");
			//					}
			//					if (ts == "float")
			//					{
			//						o[jj] = float.Parse("0");
			//					}

			//				}
			//				sELeditCache.Instance.sELeditDatas.eLC.Lists[idListReceipe].AddItem(o);

			//				break;
			//			}
			//			EnableSelectionList = true;
			//			EnableSelectionItem = true;

			//			if (MessageBox.Show("Deseja Ir a " + sELeditCache.Instance.sELeditDatas.eLC.Lists[69].listName + " ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			//			{
			//				comboBox_lists.SelectedIndex = 69;
			//				dataGridView_elems.CurrentCell = dataGridView_elems.Rows[dataGridView_elems.Rows.Count - 1].Cells[0];
			//				change_item(null, null);
			//			}


			//		}
			//		else
			//		{
			//			MessageBox.Show("Operation not supported in List " + sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex.ToString());
			//		}
			//	}
			//}

			//int idid = 0;
			//try
			//{
			//	idid = int.Parse(gridView.Rows[gridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
			//}
			//catch (Exception)
			//{

			//	//throw;
			//}

			//add_Returne(idid);
		}

		private void addItemsToolStripMenuItem_Click(object sender, EventArgs ee)
		{
			int l = comboBox_lists.SelectedIndex;
			if (dataGridView_elems.RowCount >= 1)
			{
				if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
				{
					string[] fileNames = null;
					OpenFileDialog eLoad = new OpenFileDialog();
					eLoad.Filter = "All Files (*.*)|*.*";
					eLoad.Multiselect = true;
					if (eLoad.ShowDialog() == DialogResult.OK && File.Exists(eLoad.FileName))
					{
						//EnableSelectionList = false;
						//EnableSelectionItem = false;
						fileNames = eLoad.FileNames;
						try
						{
							Cursor = Cursors.AppStarting;
							//progressBar_progress.Style = ProgressBarStyle.Continuous;
							//cpb2.Maximum = fileNames.Length;
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
								if (pos != -1 && pos2 != -1)
								{
									break;
								}
							}
							for (int i = 0; i < fileNames.Length; i++)
							{
								int e = dataGridView_elems.RowCount - 1;
								object[] o = new object[sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues[e].Length];
								sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues[e].CopyTo(o, 0);
								sELeditCache.Instance.sELeditDatas.eLC.Lists[l].AddItem(o);
								sELeditCache.Instance.sELeditDatas.eLC.Lists[l].ImportItem(fileNames[i], e + 1);
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
									dataGridView_elems.Rows.Add(new object[] { sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, 0), img, sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, pos) });
								}
								else
								{
									dataGridView_elems.Rows.Add(new object[] { 0, Properties.Resources.unknown, sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, pos) });
								}
								////cpb2.Value++;
							}
							Cursor = Cursors.Default;
						}
						catch
						{
							MessageBox.Show("IMPORT ERROR!\nCheck if the item version matches the elements.data version and is imported to the correct list!");
							Cursor = Cursors.Default;
						}
						comboBox_lists.Items[l] = "[" + l + "]: " + sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName + " (" + sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length + ")";
						//cpb2.Value = 0;
						//progressBar_progress.Style = ProgressBarStyle.Continuous;
						dataGridView_elems.ClearSelection();
						//EnableSelectionList = true;
						//EnableSelectionItem = true;
						dataGridView_elems.Rows[dataGridView_elems.RowCount - 1].Selected = true;
						dataGridView_elems.FirstDisplayedScrollingRowIndex = dataGridView_elems.RowCount - 1;
						change_item(null, null);
					}
				}
				else
				{
					MessageBox.Show("Operation not supported in List " + sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex.ToString());
				}
			}
		}

		private void cloneItemToolStripMenuItem_Click(object sender, EventArgs ex)
		{
			int l = comboBox_lists.SelectedIndex;
			int IDITEM;

			if (dataGridView_elems.RowCount > 0)
			{
				if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
				{
					int[] selIndices = gridSelectedIndices(dataGridView_elems);
					//EnableSelectionList = false;
					//EnableSelectionItem = false;
					int NewSelectedCount = 0;

					IDITEM = int.Parse(dataGridView_elems.Rows[dataGridView_elems.RowCount - 1].Cells[0].Value.ToString());
					for (int i = 0; i < selIndices.Length; i++)
					{
						object[] o = new object[sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues[selIndices[i]].Length];
						sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues[selIndices[i]].CopyTo(o, 0);
						o[0] = IDITEM + 1;


						sELeditCache.Instance.sELeditDatas.eLC.Lists[l].AddItem(o);
						dataGridView_elems.Rows.Add(new object[]
						{
							dataGridView_elems.Rows[selIndices[i]].Cells[0].Value,
							dataGridView_elems.Rows[selIndices[i]].Cells[1].Value,
							dataGridView_elems.Rows[selIndices[i]].Cells[2].Value
						});
						NewSelectedCount++;
					}
					//change_list(sender, ea);
					comboBox_lists.Items[l] = "[" + l + "]: " + sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName + " (" + sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length + ")";
					dataGridView_elems.ClearSelection();
					for (int i = NewSelectedCount; i > 0; i--)
					{
						dataGridView_elems.Rows[dataGridView_elems.RowCount - i].Selected = true;
						dataGridView_elems.FirstDisplayedScrollingRowIndex = dataGridView_elems.RowCount - 1;
					}
					//EnableSelectionList = true;
					//EnableSelectionItem = true;
					change_item(null, null);



				}
				else
				{
					MessageBox.Show("Operation not supported in List " + sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex.ToString());
				}
			}
		}

		private void deleteItemToolStripMenuItem_Click(object sender, EventArgs ee)
		{

			int l = comboBox_lists.SelectedIndex;
			int[] selIndices = gridSelectedIndices(dataGridView_elems);
			if (dataGridView_elems.RowCount > 0 && selIndices.Length > 0)
			{
				if (selIndices.Length < dataGridView_elems.RowCount)
				{
					if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
					{
						//EnableSelectionList = false;
						//EnableSelectionItem = false;
						for (int i = selIndices.Length - 1; i > -1; i--)
						{
							sELeditCache.Instance.sELeditDatas.eLC.Lists[l].RemoveItem(selIndices[i]);
							dataGridView_elems.Rows.RemoveAt(selIndices[i]);
						}
						comboBox_lists.Items[l] = "[" + l + "]: " + sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName + " (" + sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length + ")";
						//EnableSelectionList = true;
						//EnableSelectionItem = true;
						change_item(null, null);
					}
					else
					{
						MessageBox.Show("Operation not supported in List " + sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex.ToString());
					}
				}
				else
				{
					MessageBox.Show("Cannot delete all items in list!");
				}
			}
		}

		private void exportItemToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dataGridView_elems.RowCount > 0)
			{
				int l = comboBox_lists.SelectedIndex;
				int[] selIndices = gridSelectedIndices(dataGridView_elems);
				if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
				{
					if (dataGridView_elems.RowCount > 0 && selIndices.Length > 0)
					{
						FolderBrowserDialog eSave = new FolderBrowserDialog();
						if (eSave.ShowDialog() == DialogResult.OK && Directory.Exists(eSave.SelectedPath))
						{
							try
							{
								Cursor = Cursors.AppStarting;
								//progressBar_progress.Style = ProgressBarStyle.Continuous;
								//cpb2.Maximum = selIndices.Length;
								for (int i = 0; i < selIndices.Length; i++)
								{
									sELeditCache.Instance.sELeditDatas.eLC.Lists[l].ExportItem(eSave.SelectedPath + "\\" + selIndices[i], selIndices[i]);
									//cpb2.Value++;
								}
								Cursor = Cursors.Default;
								MessageBox.Show("Export complete!");
							}
							catch
							{
								MessageBox.Show("EXPORT ERROR!\nExporting item to unicode text file failed!");
								Cursor = Cursors.Default;
							}
							//cpb2.Value = 0;
							//progressBar_progress.Style = ProgressBarStyle.Continuous;
						}
					}
				}
				else
				{
					MessageBox.Show("Operation not supported in List " + sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex.ToString());
				}
			}
		}

		private void replaceItemToolStripMenuItem_Click(object sender, EventArgs ee)
		{
			int l = comboBox_lists.SelectedIndex;
			int e = dataGridView_elems.CurrentRow.Index;
			if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
			{
				if (l > -1 && e > -1)
				{
					OpenFileDialog eLoad = new OpenFileDialog();
					eLoad.Filter = "All Files (*.*)|*.*";
					if (eLoad.ShowDialog() == DialogResult.OK && File.Exists(eLoad.FileName))
					{
						try
						{
							Cursor = Cursors.AppStarting;
							sELeditCache.Instance.sELeditDatas.eLC.Lists[l].ImportItem(eLoad.FileName, e);
							change_list(null, null);
							dataGridView_elems.Rows[e].Selected = true;
							Cursor = Cursors.Default;
						}
						catch
						{
							MessageBox.Show("IMPORT ERROR!\nCheck if the item version matches the elements.data version and is imported to the correct list!");
							Cursor = Cursors.Default;
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Operation not supported in List " + sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex.ToString());
			}
		}

		private void moveItemsToTopToolStripMenuItem_Click(object sender, EventArgs ee)
		{
			int l = comboBox_lists.SelectedIndex;
			int[] selIndices = gridSelectedIndices(dataGridView_elems);
			if (selIndices[0] > 0 && selIndices.Length > 0 && l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
			{
				//EnableSelectionItem = false;
				object[][] temp = new object[sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length][];
				for (int i = 0; i < selIndices.Length; i++)
				{
					Array.Copy(sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues, selIndices[i], temp, i, 1);
				}
				Array.Copy(sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues, 0, temp, selIndices.Length, selIndices[0]);
				for (int i = selIndices.Length - 1; i > -1; i--)
				{
					sELeditCache.Instance.sELeditDatas.eLC.Lists[l].RemoveItem(selIndices[i]);
				}
				Array.Copy(sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues, 0, temp, selIndices.Length, sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length);
				sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues = temp;

				change_list(null, null);

				dataGridView_elems.ClearSelection();
				for (int i = 0; i < selIndices.Length; i++)
				{
					dataGridView_elems.Rows[i].Selected = true;
					dataGridView_elems.FirstDisplayedScrollingRowIndex = i;
				}
				//EnableSelectionItem = true;
			}
		}

		private void moveItemsToEndToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int l = comboBox_lists.SelectedIndex;
			int[] selIndices = gridSelectedIndices(dataGridView_elems);
			if (selIndices[0] < dataGridView_elems.RowCount - 1 && selIndices.Length > 0 && l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
			{
				//EnableSelectionItem = false;
				object[][] temp = new object[sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length][];
				for (int i = 0; i < selIndices.Length; i++)
				{
					Array.Copy(sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues, selIndices[i], temp, dataGridView_elems.RowCount - selIndices.Length + i, 1);
				}
				Array.Copy(sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues, 0, temp, selIndices.Length, selIndices[0]);
				for (int i = selIndices.Length - 1; i > -1; i--)
				{
					sELeditCache.Instance.sELeditDatas.eLC.Lists[l].RemoveItem(selIndices[i]);
				}
				Array.Copy(sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues, 0, temp, 0, sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length);
				sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues = temp;

				change_list(null, null);

				dataGridView_elems.ClearSelection();
				for (int i = dataGridView_elems.RowCount - selIndices.Length; i < dataGridView_elems.RowCount; i++)
				{
					dataGridView_elems.Rows[i].Selected = true;
					dataGridView_elems.FirstDisplayedScrollingRowIndex = i;
				}
				//EnableSelectionItem = true;
			}
		}

		#endregion

		#region cab
		private void numericUpDownEx_ID_ValueChanged(object sender, EventArgs e)
		{
			if (!isLoadedDataGridProps)
			{
				return;
			}
			dgp.SetID((int)numericUpDownEx_ID.Value);
		}

		private void textBox_NAME_TextChanged(object sender, EventArgs e)
		{
			if (!isLoadedDataGridProps)
			{
				return;
			}
			if (!ColorEdit)
			{
				dgp.SetName(textBox_NAME.Text);
			}
		}
		#endregion
		private void richTextBox_DESC_POS_MouseDoubleClick(object sender, MouseEventArgs e)
		{

			(new item_ext_desc(int.Parse(dataGridView_elems.Rows[dataGridView_elems.CurrentCell.RowIndex].Cells[0].Value.ToString()))).ShowDialog(this);
			richTextBox_DESC_POS.Clear();
			if (sELeditCache.Instance.sELeditDatas.database.item_ext_desc.ContainsKey(int.Parse(dataGridView_elems.Rows[dataGridView_elems.CurrentCell.RowIndex].Cells[0].Value.ToString())))
			{

				SetText(sELeditCache.Instance.sELeditDatas.database.item_ext_desc[(int)dataGridView_elems.Rows[dataGridView_elems.CurrentCell.RowIndex].Cells[0].Value]);
			}
		}

		private void toolStripButton_log_Click(object sender, EventArgs e) => (new LogsChangedForm()).Show();
	}

}

