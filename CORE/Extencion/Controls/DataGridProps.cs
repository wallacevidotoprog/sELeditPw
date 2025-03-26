using sELedit.CORE.BASE;
using sELedit.CORE.BASE.CONTROLLERS;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace sELedit.CORE.Extencion.Controls
{
	public partial class DataGridProps : UserControl
	{
		private bool EnableSelectionItem { get; set; } = true;

		private int listSelectedIndex = -1;
		private DataGridView dataGridView_elems = null;

		public DataGridProps(int listSelectedIndex, DataGridView dataGridView_elems)
		{
			InitializeComponent();
			this.listSelectedIndex = listSelectedIndex;
			this.dataGridView_elems = dataGridView_elems;
			change_item();
		}


		private void change_item()
		{

			if (EnableSelectionItem)
			{
				int l = listSelectedIndex;
				if (dataGridView_elems.CurrentCell == null) { return; }
				int e = dataGridView_elems.CurrentCell.RowIndex;
				int scroll = dataGridView_itemProps.FirstDisplayedScrollingRowIndex;
				dataGridView_itemProps.SuspendLayout();
				dataGridView_itemProps.Rows.Clear();
				//proctypeLocation = 0;
				//proctypeLocationvak = 0;
				try
				{
					if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
					{
						if (e > -1)
						{
							dataGridView_itemProps.Enabled = false;



							var x = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues[e];
							var x1 = sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[e];
							var x21 = sELeditCache.Instance.sELeditDatas.eLC.Lists[l];


							CategoryControlManager generator = new CategoryControlManager();
							var groupedValues = generator.GroupElementValuesByCategory(sELeditCache.Instance.sELeditDatas.eLC.Lists[l]);
							var controls = generator.CreateControlsFromGroupedValues(groupedValues);




							for (int f = 0; f < sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementValues[e].Length; f++)
							{
								dataGridView_itemProps.Rows.Add(new string[] { sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementFields[f], sELeditCache.Instance.sELeditDatas.eLC.Lists[l].elementTypes[f], sELeditCache.Instance.sELeditDatas.eLC.GetValue(l, e, f) });
								dataGridView_itemProps.Rows[f].HeaderCell.Value = f.ToString();
							}
							dataGridView_itemProps.Enabled = true;
							dataGridView_itemProps.PerformLayout();
							dataGridView_itemProps.ResumeLayout();
						}
					}
					else
					{
						//if (e > -1)
						//{
						//	dataGridView_item.Rows.Add(new string[] { "id_talk", "int32", conversationList.talk_procs[e].id_talk.ToString() });
						//	dataGridView_item.Rows.Add(new string[] { "text", "wstring:128", conversationList.talk_procs[e].GetText() });
						//	for (int q = 0; q < conversationList.talk_procs[e].num_window; q++)
						//	{
						//		dataGridView_item.Rows.Add(new string[] { "window_" + q + "_id", "int32", conversationList.talk_procs[e].windows[q].id.ToString() });
						//		dataGridView_item.Rows.Add(new string[] { "window_" + q + "_id_parent", "int32", conversationList.talk_procs[e].windows[q].id_parent.ToString() });
						//		dataGridView_item.Rows.Add(new string[] { "window_" + q + "_talk_text", "wstring:" + conversationList.talk_procs[e].windows[q].talk_text_len, conversationList.talk_procs[e].windows[q].GetText() });
						//		for (int c = 0; c < conversationList.talk_procs[e].windows[q].num_option; c++)
						//		{
						//			dataGridView_item.Rows.Add(new string[] { "window_" + q + "_option_" + c + "_param", "int32", conversationList.talk_procs[e].windows[q].options[c].param.ToString() });
						//			dataGridView_item.Rows.Add(new string[] { "window_" + q + "_option_" + c + "_text", "wstring:128", conversationList.talk_procs[e].windows[q].options[c].GetText() });
						//			dataGridView_item.Rows.Add(new string[] { "window_" + q + "_option_" + c + "_id", "int32", conversationList.talk_procs[e].windows[q].options[c].id.ToString() });
						//		}
						//	}
						//}
					}
					if (scroll > -1)
					{
						dataGridView_itemProps.FirstDisplayedScrollingRowIndex = scroll;
					}
				}
				catch { }
			}
		}

		private void change_value(object sender, DataGridViewCellEventArgs ea)
		{
			//if (EnableSelectionItem) return;
			try
			{
				if (sELeditCache.Instance.sELeditDatas.eLC != null && ea.ColumnIndex > -1 && ea.RowIndex > -1)
				{
					int l = listSelectedIndex;
					int f = ea.RowIndex;

					string _set = string.Empty;

					if (dataGridView_itemProps.Rows[ea.RowIndex].Cells[2].Value.ToString().Contains("[") && dataGridView_itemProps.Rows[ea.RowIndex].Cells[2].Value.ToString().Contains("]"))
					{
						var _set_v = Convert.ToString(dataGridView_itemProps.Rows[ea.RowIndex].Cells[2].Value.ToString()).Replace("[", "").Replace("] ", "").Split('-');
						_set = _set_v[0].Replace(" ", "");
					}
					else
					{
						_set = Convert.ToString(dataGridView_itemProps.Rows[ea.RowIndex].Cells[2].Value.ToString());
					}


					if (true)
					{

					}
					if (l != sELeditCache.Instance.sELeditDatas.eLC.ConversationListIndex)
					{
						EnableSelectionItem = false;
						int[] selIndices = gridSelectedIndices(dataGridView_elems);
						for (int e = 0; e < selIndices.Length; e++)
						{
							sELeditCache.Instance.sELeditDatas.eLC.SetValue(l, selIndices[e], f, _set);//-------------------------------------------------------set value


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
	}
}
