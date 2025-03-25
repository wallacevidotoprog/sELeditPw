using sELedit.CORE.MODEL;
using sELedit.gShop;
using sELedit.Properties;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using tasks;
using sELedit.CORE.IO;

namespace sELedit.CORE.BASE
{
	public class sELeditCache
	{
		private static readonly Lazy<sELeditCache> _instance = new Lazy<sELeditCache>(() => new sELeditCache());

		public static sELeditCache Instance => _instance.Value;


		public Settings Settings { get; set; }

		private sELeditCache()
		{
			ReadFile.ReadWriteSettings(IOAction.Read);
		}






		#region LOADS
		void Loads()
		{
			
			if (Settings != null)
			{
				if (File.Exists(Settings?.ElementsDataPath))
				{
					Thread element = new Thread(new ThreadStart(LoadElementData));
					element.Start();

				}
				if (File.Exists(Settings?.TasksDataPath))
				{
					Thread tasks = new Thread(new ThreadStart(ReadTask));
					tasks.Start();

				}
				if (File.Exists(Settings?.GshopDataPath))
				{
					Thread gshop = new Thread(new ThreadStart(ReadShops));
					gshop.Start();

				}

			}			
		}

		private Task ReadTask()
		{
			if (File.Exists(XmlData.TasksDataPath))
			{
				try
				{
					ATaskTempl[] Tasks = null;
					FileStream input = File.OpenRead(XmlData.TasksDataPath);
					BinaryReader binaryStream = new BinaryReader(input);
					TASK_PACK_HEADER tph = new TASK_PACK_HEADER(binaryStream);
					if (tph.magic == -1819966623 || tph.magic == 0)
					{
						if (!GlobalData.Versions.Contains(tph.version))
						{
							binaryStream.Close();
							input.Close();


						}
						else
						{

							GlobalData.NewID = 0;
							GlobalData.version = tph.version;

							int[] pOffs = new int[tph.item_count];
							for (int i = 0; i < tph.item_count; i++)
							{
								pOffs[i] = binaryStream.ReadInt32();
							}
							Tasks = new ATaskTempl[tph.item_count];

							IProgress<int> progress = new Progress<int>(value =>
							{

							});
							if (true)
							{
								var p = 0;
								Parallel.For(0, tph.item_count, i =>
								{
									byte[] bytes = null;
									lock (binaryStream)
									{
										binaryStream.BaseStream.Seek(pOffs[i], SeekOrigin.Begin);
										int count = ((i < pOffs.Length - 1)
														? pOffs[i + 1]
														: (int)binaryStream.BaseStream.Length) - pOffs[i];
										bytes = binaryStream.ReadBytes(count);
									}

									using (var ms = new MemoryStream(bytes))
									using (var br = new BinaryReader(ms))
									{
										if (true)
										{
											try
											{
												Tasks[i] = new ATaskTempl(tph.version, br);
											}
											catch (Exception e)
											{
												//MessageBox.Show(String.Format(GetLocalization(521), i));
												//if (Debug)
												//    Extensions.WriteLog("Error load task! Task index: " + i);
											}
										}
										else
											Tasks[i] = new ATaskTempl(tph.version, br);
									}

									Interlocked.Increment(ref p);
									if (p % 100 == 0)
									{
										progress.Report(p / 2);
										Application.DoEvents();
									}
								}); //3.5 мс для 146 версии
							}
							else
							{
								for (int i = 0; i < tph.item_count; i++)
								{
									// Tasks[i] = new ATaskTempl(tph.version, binaryStream, pOffs[i]);
									if (i % 100 == 0)
									{
										progress.Report(i / 2);
										Application.DoEvents();
									}
								}
							}

							binaryStream.Close();
							input.Close();

							Tasks = Tasks.Where(it => it != null).ToArray();

							void add_node(ATaskTempl[] tasks, TreeNodeCollection nodes, int GMIconIndex)
							{
								for (var i = 0; i < tasks.Length; i++)
								{
									tasks[i].AddNode(nodes, GMIconIndex);
									if (tasks[i].pSub.Length > 0)
										add_node(tasks[i].pSub, nodes[i].Nodes, GMIconIndex);
								}
							}



						}
					}
					else
					{
						binaryStream.Close();
						input.Close();
					}

					database.Tasks = Tasks;
				}
				catch (Exception ex)
				{


				}

			}
			else
			{
				database.Tasks = null;
			}
		}
		private Task ReadShops()
		{
			try
			{
				FileGshop FileGshop = new FileGshop();
				FileGshop.ReadFile(XmlData.GshopDataPath, 0);
				database.Gshop = FileGshop;
			}
			catch (Exception ex)
			{


			}
			try
			{
				FileGshop FileGshop1 = new FileGshop();
				FileGshop1.ReadFile(XmlData.Gshop1DataPath, 0);
				database.GshopEvent = FileGshop1;
			}
			catch (Exception ex)
			{


			}
		}
		void LoadElementData()
		{

			string file = XmlData.ElementsDataPath;
			comboBox_lists.Invoke((MethodInvoker)delegate
			{
				if (File.Exists(file))
				{
					try
					{
						eLC = new eListCollection(file, ref cpb2);

						SortedList ItemUse = new SortedList();
						for (int i = 0; i < eLC.Lists.Length; i++)
						{
							if (eLC.Lists[i].itemUse == true)
							{
								if (!ItemUse.ContainsKey(i))
								{
									ItemUse.Add(i, i);
								}

							}

						}
						database.ItemUse = ItemUse;
						if (eLC.ConfigFile != null)//Вроде работает
						{
							string[] referencefiles = Directory.GetFiles(Application.StartupPath + "\\configs", "references.txt");
							if (referencefiles.Length > 0)
							{
								StreamReader sr = new StreamReader(referencefiles[0]);
								char[] chars = { ';', ',' };
								string[] x;
								xrefs = new string[eLC.Lists.Length][];
								string line;
								while (!sr.EndOfStream)
								{
									line = sr.ReadLine();
									if (!line.StartsWith("#"))
									{
										x = line.Split(chars);
										if (int.Parse(x[0]) < eLC.Lists.Length)
										{
											xrefs[int.Parse(x[0])] = x;
										}
									}
								}
								this.toolStripSeparator6.Visible = true;
								// this.xrefItemToolStripMenuItem.Visible = true;
							}
						}

						if (eLC.ConversationListIndex > -1 && eLC.Lists.Length > eLC.ConversationListIndex)
						{
							conversationList = new eListConversation((byte[])eLC.Lists[eLC.ConversationListIndex].elementValues[0][0]);
						}

						dataGridView_item.Rows.Clear();
						comboBox_lists.Items.Clear();

						for (int l = 0; l < eLC.Lists.Length; l++)
						{


							comboBox_lists.Items.Add("[" + l + "] " + eLC.Lists[l].listName.Split(new string[] { " - " }, StringSplitOptions.None)[1] + " (" + eLC.Lists[l].elementValues.Length + ")");



						}
						string timestamp = "";
						if (eLC.Lists[0].listOffset.Length > 0)
							// timestamp = ", Timestamp: " + timestamp_to_string(BitConverter.ToUInt32(eLC.Lists[0].listOffset, 0));
							this.Text = " sELedit NanoTech (" + file + " [Version: " + eLC.VersionData + " Key:" + eLC.Version.ToString() + timestamp + "])";
						ElementsPath = file;

						cpb2.Value = 0;


						comboBox_lists.SelectedIndex = 0;

					}
					catch (Exception e)
					{
						MessageBox.Show(e.Message + "\n\n\n" + e);

					}
				}
			});

		}
		#endregion

	}
}
