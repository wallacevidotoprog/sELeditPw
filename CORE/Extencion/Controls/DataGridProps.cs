using sELedit.configs;
using sELedit.CORE.BASE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace sELedit.CORE.Extencion.Controls
{
	public partial class DataGridProps : UserControl
	{
		public event EventHandler<EventDataChanged> DataChanged;

		private bool EnableSelectionItem { get; set; } = true;

		private int listSelectedIndex = -1;
		private DataGridView dataGridView_elems = null;
		private Dictionary<string, List<(int Index, object[] Values, string[] Fields)>> keyValuePairs;
		private Dictionary<string, bool> tabStatus;
		public string TabSelect = string.Empty;

		public DataGridProps(int listSelectedIndex, DataGridView dataGridView_elems, Dictionary<string, List<(int Index, object[] Values, string[] Fields)>> keyValuePairs)
		{
			InitializeComponent();
			EnableSelectionItem = false;
			this.listSelectedIndex = listSelectedIndex;
			this.dataGridView_elems = dataGridView_elems;
			this.keyValuePairs = keyValuePairs;
			StartTabs();
			SetPropsGrid("default");
			EnableSelectionItem = true;
		}

		private void StartTabs()
		{
			tabStatus = new Dictionary<string, bool>();

			foreach (var item in keyValuePairs)
			{
				tabStatus.Add(item.Key, item.Key == "default" ? true : false);
			}
		}

		public void SetPropsGrid(string tab)
		{
			TabSelect = tab;
			try
			{

				if (!EnableSelectionItem)
				{
					int indexDG = 0;
					dataGridView_itemProps.Rows.Clear();
					foreach (var tb in keyValuePairs[tab.ToLower()])
					{
						dataGridView_itemProps.Rows.Add(new object[] {
							new DisplayValueItem{
								DisplayText=sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementFields[tb.Index].Replace("_"," ").ToUpper(),
								RealValue = sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementFields[tb.Index].Replace("_"," ").ToUpper()},

							new DisplayValueItem{
								DisplayText=sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementTypes[tb.Index],
								RealValue = sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementTypes[tb.Index]},

							new DisplayValueItem{
								DisplayText=sELeditCache.Instance.sELeditDatas.eLC.GetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, tb.Index),
								RealValue = sELeditCache.Instance.sELeditDatas.eLC.GetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, tb.Index) }
						});

						dataGridView_itemProps.Rows[indexDG].HeaderCell.Value = tb.Index.ToString();
						indexDG++;
					}
					dataGridView_itemProps.PerformLayout();
					dataGridView_itemProps.ResumeLayout();
				}
			}
			catch (Exception ex)
			{
				ex.ErrorGet();
			}

		}

		public void SetID(int value)
		{

			int indexID = Array.FindIndex(sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementFields, x => x.ToUpper() == "ID");
			if (indexID != -1)
			{
				sELeditCache.Instance.sELeditDatas.eLC.SetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, indexID, value.ToString());
				RefreshGrid(Change.ID, value.ToString());
			}
		}
		public void SetName(string value)
		{
			int indexName = Array.FindIndex(sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementFields, x => x.ToUpper() == "NAME");
			var tt = sELeditCache.Instance.sELeditDatas.eLC.GetValue(listSelectedIndex, indexName, indexName);
			if (sELeditCache.Instance.sELeditDatas.eLC.GetValue(listSelectedIndex, indexName, indexName) == value)
			{
				return;
			}
			if (indexName != -1)
			{
				sELeditCache.Instance.sELeditDatas.eLC.SetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, indexName, value);
				RefreshGrid(Change.NAME, value);
			}
		}

		public void SetIco(string value)
		{
			int indexIco = Array.FindIndex(sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementFields, x => x.ToUpper().StartsWith("FILE_ICON"));
			if (indexIco != -1)
			{
				sELeditCache.Instance.sELeditDatas.eLC.SetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, indexIco, value);
				RefreshGrid(Change.ICO, value);
			}
		}

		private void RefreshGrid(Change change, string value)
		{
			EnableSelectionItem = false;

			foreach (DataGridViewRow item in dataGridView_itemProps.Rows)
			{
				if (item.Cells[0].Value.ToString().ToUpper() == change.ToString())
				{
					item.Cells[2].Value = new DisplayValueItem { DisplayText = value, RealValue = value }; break;
				}
			}
			EnableSelectionItem = true;

			//SetPropsGrid(TabSelect);
		}

		private void change_value(object sender, DataGridViewCellEventArgs ea)
		{
			if (!EnableSelectionItem) return;
			try
			{
				if (sELeditCache.Instance.sELeditDatas.eLC != null && ea.ColumnIndex > -1 && ea.RowIndex > -1)
				{
					int l = listSelectedIndex;
					int f = ea.RowIndex;

					string _set = string.Empty;

					if (dataGridView_itemProps.Rows[ea.RowIndex].Cells[2].Value.ToString().Contains("[") && dataGridView_itemProps.Rows[ea.RowIndex].Cells[2].Value.ToString().Contains("]"))
					{
						var _set_v = Convert.ToString((dataGridView_itemProps.Rows[ea.RowIndex].Cells[2].Value as DisplayValueItem)?.RealValue.ToString()).Replace("[", "").Replace("] ", "").Split('-');
						_set = _set_v[0].Replace(" ", "");

					}
					else
					{
						_set = Convert.ToString((dataGridView_itemProps.Rows[ea.RowIndex].Cells[2].Value as DisplayValueItem)?.RealValue.ToString());
					}


					if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
					{
						EnableSelectionItem = false;
						int[] selIndices = gridSelectedIndices(dataGridView_elems);
						for (int e = 0; e < selIndices.Length; e++)
						{

							sELeditCache.Instance.sELeditDatas.eLC.SetValue(l, selIndices[e], f, _set);//-------------------------------------------------------set value


							DataChanged?.Invoke(this, new EventDataChanged
							{
								ListIndex = l,
								ElementIndex = selIndices[e],
								FieldIndex = f,
								Value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, selIndices[e], f),
								FieldName = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[f],
							});

							for (int a = 0; a < selIndices.Length; a++)
							{
								if (dataGridView_itemProps.Rows[a].Cells[0].Value.ToString() == "ID" || dataGridView_itemProps.Rows[a].Cells[0].Value.ToString() == "Name" || dataGridView_itemProps.Rows[a].Cells[0].Value.ToString() == "file_icon" || dataGridView_itemProps.Rows[a].Cells[0].Value.ToString() == "file_icon1")
								{
									// change the values in the listbox depending on new name & id

									// Find Position for Name
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


									//Bitmap img = Properties.Resources.blank;
									//string path = Path.GetFileName(sELeditCache.Instance.sELeditDatas.sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, selIndices[e], pos2));
									//if (database.sourceBitmap != null && database.ContainsKey(path))
									//{
									//	if (database.ContainsKey(path))
									//	{
									//		img = database.images(path);
									//	}
									//}

									/// -------------- vai enviar um evento
									//dataGridView_elems.Rows[selIndices[e]].Cells[0].Value = sELeditCache.Instance.sELeditDatas.sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, selIndices[e], 0);
									//dataGridView_elems.Rows[selIndices[e]].Cells[1].Value = img;
									//dataGridView_elems.Rows[selIndices[e]].Cells[2].Value = sELeditCache.Instance.sELeditDatas.sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, selIndices[e], pos);
								}

							}

						}
						EnableSelectionItem = true;

					}
					else
					{
						//TALK_PROCs check which item was changed by field name
						//string fieldName = dataGridView_item[0, ea.RowIndex].Value.ToString();

						//if (fieldName == "id_talk")
						//{
						//	conversationList.talk_procs[r].id_talk = Convert.ToInt32(dataGridView_item.CurrentCell.Value);
						//	return;
						//}
						//if (fieldName == "text")
						//{
						//	conversationList.talk_procs[r].SetText(dataGridView_item.CurrentCell.Value.ToString());
						//	return;
						//}
						//if (fieldName.StartsWith("window_") && fieldName.EndsWith("_id"))
						//{
						//	int q = Convert.ToInt32(fieldName.Replace("window_", "").Replace("_id", ""));
						//	conversationList.talk_procs[r].windows[q].id = Convert.ToInt32(dataGridView_item.CurrentCell.Value);
						//	return;
						//}
						//if (fieldName.StartsWith("window_") && fieldName.Contains("option_") && fieldName.EndsWith("_param"))
						//{
						//	string[] s = fieldName.Replace("window_", "").Replace("_option_", ";").Replace("_param", "").Split(new char[] { ';' });
						//	int q = Convert.ToInt32(s[0]);
						//	int c = Convert.ToInt32(s[1]);
						//	conversationList.talk_procs[r].windows[q].options[c].param = Convert.ToInt32(dataGridView_item.CurrentCell.Value);
						//	return;
						//}
						//if (fieldName.StartsWith("window_") && fieldName.Contains("option_") && fieldName.EndsWith("_text"))
						//{
						//	string[] s = fieldName.Replace("window_", "").Replace("_option_", ";").Replace("_text", "").Split(new char[] { ';' });
						//	int q = Convert.ToInt32(s[0]);
						//	int c = Convert.ToInt32(s[1]);
						//	conversationList.talk_procs[r].windows[q].options[c].SetText(dataGridView_item.CurrentCell.Value.ToString());
						//	return;
						//}
						//if (fieldName.StartsWith("window_") && fieldName.Contains("option_") && fieldName.EndsWith("_id"))
						//{
						//	string[] s = fieldName.Replace("window_", "").Replace("_option_", ";").Replace("_id", "").Split(new char[] { ';' });
						//	int q = Convert.ToInt32(s[0]);
						//	int c = Convert.ToInt32(s[1]);
						//	conversationList.talk_procs[r].windows[q].options[c].id = Convert.ToInt32(dataGridView_item.CurrentCell.Value);
						//	return;
						//}
						//if (fieldName.StartsWith("window_") && fieldName.EndsWith("_id_parent"))
						//{
						//	int q = Convert.ToInt32(fieldName.Replace("window_", "").Replace("_id_parent", ""));
						//	conversationList.talk_procs[r].windows[q].id_parent = Convert.ToInt32(dataGridView_item.CurrentCell.Value);
						//	return;
						//}
						//if (fieldName.StartsWith("window_") && fieldName.EndsWith("_talk_text"))
						//{
						//	int q = Convert.ToInt32(fieldName.Replace("window_", "").Replace("_talk_text", ""));
						//	conversationList.talk_procs[r].windows[q].SetText(dataGridView_item.CurrentCell.Value.ToString());
						//	dataGridView_item[1, ea.RowIndex].Value = "wstring:" + conversationList.talk_procs[r].windows[q].talk_text_len;
						//	return;
						//}
					}

				}
			}
			catch (Exception exs)
			{
				EnableSelectionItem = true;
				MessageBox.Show("CHANGING ERROR!\nFailed changing value, this value seems to be invalid.\n" + exs.Message);
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

		private void button_SetValue_Click(object sender, EventArgs e)
		{


			string value = textBox_SetValue.Visible == true ? textBox_SetValue.Text : numericUpDown_SetValue.Visible == true ? numericUpDown_SetValue.Value.ToString() : null;

			if (string.IsNullOrEmpty(value)) return;

			int ind = Convert.ToInt32(dataGridView_itemProps.Rows[dataGridView_itemProps.CurrentCell.RowIndex].HeaderCell.Value);
			sELeditCache.Instance.sELeditDatas.eLC.SetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, ind, value);
			dataGridView_itemProps.Rows[dataGridView_itemProps.CurrentCell.RowIndex].Cells[2].Value = value;


			DataChanged?.Invoke(this, new EventDataChanged
			{
				ListIndex = listSelectedIndex,
				ElementIndex = dataGridView_elems.CurrentCell.RowIndex,
				FieldIndex = ind,
				Value = sELeditCache.Instance.sELeditDatas.eLC.GetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, ind),
				FieldName = sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementFields[ind],
			});
		}

		private void dataGridView_itemProps_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{

			string type = sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementTypes[Convert.ToInt32(dataGridView_itemProps.Rows[e.RowIndex].HeaderCell.Value)];


			switch (type)
			{
				case "int16":
				case "int32":
				case "int64":
					textBox_SetValue.Visible = false;
					numericUpDown_SetValue.Visible = true;
					numericUpDown_SetValue.SetDecimalPlaces = 0;
					numericUpDown_SetValue.SetThousandsSeparator = false;
					numericUpDown_SetValue.MaximalValue = Int32.MaxValue;
					numericUpDown_SetValue.MinimalValue = 0;
					numericUpDown_SetValue.Increment = 1;
					numericUpDown_SetValue.Value = Convert.ToDecimal((dataGridView_itemProps.Rows[e.RowIndex].Cells[2].Value as DisplayValueItem)?.RealValue);

					panel_action.Visible = true;
					break;
				case "float":
				case "double":
					textBox_SetValue.Visible = false;
					numericUpDown_SetValue.Visible = true;
					numericUpDown_SetValue.SetDecimalPlaces = 6;
					numericUpDown_SetValue.SetThousandsSeparator = true;
					numericUpDown_SetValue.MaximalValue = decimal.Parse("1,000000");
					numericUpDown_SetValue.MinimalValue = decimal.Parse("0,000000");
					numericUpDown_SetValue.Increment = 0.25M;
					numericUpDown_SetValue.Value = Convert.ToDecimal((dataGridView_itemProps.Rows[e.RowIndex].Cells[2].Value as DisplayValueItem)?.RealValue);

					panel_action.Visible = true;
					break;
				case "wstring:64":
				case "string:32":
				case "string:128":
				case "string:":
					numericUpDown_SetValue.Visible = false;
					textBox_SetValue.Visible = true;
					textBox_SetValue.Text = (string)(dataGridView_itemProps.Rows[e.RowIndex].Cells[2].Value as DisplayValueItem)?.RealValue;

					panel_action.Visible = true;
					break;

			}
		}

		private void dataGridView_item_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
		{
			DataGridView dgv = (DataGridView)sender;
			if (listSelectedIndex != 54 && listSelectedIndex != 40)
			{

				if (dgv.Rows.Count > 0)
				{

					try
					{
						string ID_ITEM = sELeditCache.Instance.sELeditDatas.eLC.GetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, 0).ToString();

						//int indexName = Array.FindIndex(sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].elementFields, x => x.ToUpper() == "NAME");
						string NAME_ITEM = dgv.Rows[e.RowIndex].Cells[0].Value.ToString().ToLower().Replace(" ", "_");//sELeditCache.Instance.sELeditDatas.eLC.GetValue(listSelectedIndex, dataGridView_elems.CurrentCell.RowIndex, indexName);

						string TYPE_ITEM = dgv.Rows[e.RowIndex].Cells[1].Value.ToString().ToLower().Replace(" ", "_");

						if (ID_ITEM != "0")
						{

							bool fi = false;
							bool fini = false;



							switch (NAME_ITEM)
							{
								case string x when ((x.EndsWith("_id_addon") || x.StartsWith("skills_") || x.StartsWith("after_death") || x.StartsWith("skill_hp") || x.EndsWith("_id_unique") || x.EndsWith("_id_rand")) && TYPE_ITEM.StartsWith("int32")):

									dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem
									{
										DisplayText = "[" + ID_ITEM + "] - " + EQUIPMENT_ADDON.GetAddon(ID_ITEM.ToString()),
										RealValue = ID_ITEM
									};
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

														//if (ID_ITEM == sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, 0))
														//{
														//	if (sELeditCache.Instance.sELeditDatas.database.sourceBitmap != null && sELeditCache.Instance.sELeditDatas.database.ContainsKey(Path.GetFileName(sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, pos))))
														//	{
														//		if (sELeditCache.Instance.sELeditDatas.database.ContainsKey(Path.GetFileName(sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, pos))))
														//		{
														//			if (dgv.Rows[e.RowIndex].Cells[2].Value.ToString() == "0")
														//			{
														//				((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[2]).Image = Extensions.ResizeImage(null, 0, 0);
														//			}
														//			else
														//			{
														//				((TextAndImageCell)dgv.Rows[e.RowIndex].Cells[2]).Image = Extensions.ResizeImage(sELeditCache.Instance.sELeditDatas.database.images(Path.GetFileName(sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, pos))), 18, 18);
														//			}

														//			dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem
														//			{
														//				DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(La, ef, posN),
														//				RealValue = ID_ITEM
														//			};

														//			fi = true;

														//			Color clr;
														//			try
														//			{ clr = Helper.getByID(sELeditCache.Instance.sELeditDatas.database.item_color[int.Parse(ID_ITEM.ToString())]); }
														//			catch (Exception)
														//			{ clr = Color.White; }

														//			dgv.Rows[e.RowIndex].Cells[2].Style.ForeColor = clr;
														//			break;
														//		}
														//	}
														//}
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
											dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + Extensions.DecodingCharacterComboId(ID_ITEM.ToString()), RealValue = ID_ITEM };
											break;
										}
									}
									break;

								case "proc_type":
									for (int k = 0; k < sELeditCache.Instance.sELeditDatas.eLC.Lists[3].elementFields.Length; k++)
									{
										if (sELeditCache.Instance.sELeditDatas.eLC.Lists[3].elementFields[k] == "proc_type")
										{
											dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + Extensions.Get_proc_type(ID_ITEM), RealValue = ID_ITEM };
											break;
										}
									}
									break;

								case "id_major_type":
									for (int l = 0; l < sELeditCache.Instance.sELeditDatas.eLC.Lists.Length; l++)
									{
										string major = sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1].Replace("ESSENCE", "MAJOR_TYPE");
										string conf = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1];
										if (major == conf)
										{
											for (int m = 0; m < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length; m++)
											{
												if (int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 0)) == int.Parse(ID_ITEM))
												{
													dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 1), RealValue = ID_ITEM };
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
										string major = sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1].Replace("ESSENCE", "SUB_TYPE");
										string conf = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1];
										if (major == conf)
										{
											for (int m = 0; m < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length; m++)
											{
												if (int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 0)) == int.Parse(ID_ITEM))
												{
													dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 1), RealValue = ID_ITEM };
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
									dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + EQUIPMENT_ADDON.GetAddon(ID_ITEM), RealValue = ID_ITEM };
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
										string major = sELeditCache.Instance.sELeditDatas.eLC.Lists[listSelectedIndex].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1].Replace("ESSENCE", "TYPE");
										string conf = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1];
										if (major == conf)
										{
											for (int m = 0; m < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues.Length; m++)
											{
												if (int.Parse(sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 0)) == int.Parse(ID_ITEM))
												{
													dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, m, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(54, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(41, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(40, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(42, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(43, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(44, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(46, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(45, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(47, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem
												{
													DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(48, xe, 1),
													RealValue = ID_ITEM
												};
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(49, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(50, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(52, xe, 1), RealValue = ID_ITEM };
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
												dgv.Rows[e.RowIndex].Cells[2].Value = new DisplayValueItem { DisplayText = "[" + ID_ITEM + "] - " + sELeditCache.Instance.sELeditDatas.eLC.GetValue(53, xe, 1), RealValue = ID_ITEM };
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
						exd.ErrorGet();

					}



				}

			}

		}
	}

	enum Change
	{
		NONE, ID, NAME, ICO
	}
}
