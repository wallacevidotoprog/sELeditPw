using sELedit.CORE.IO;
using sELedit.CORE.MODEL;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sELedit.CORE.BASE
{
	public class sELeditCache
	{
		private static readonly Lazy<sELeditCache> _instance = new Lazy<sELeditCache>(() => new sELeditCache());

		public static sELeditCache Instance => _instance.Value;

		public ToolStripProgressBar progressBar;

		public Settings Settings { get; set; }

		public sELeditModel sELeditDatas { get; set; } = new sELeditModel();

		private sELeditCache()
		{


		}

		public void Start()
		{
			if (ReadFile.ReadWriteSettings(IOAction.Read))
			{
				Loads();
			}
		}





		#region LOADS
		private void Loads()
		{

			if (Settings != null)
			{
				if (Settings.CheckFileExists(nameof(Settings.ElementsDataPath)))
				{
					LoadElementData();

				}
				//if (Settings.CheckFileExists(nameof(Settings.TasksDataPath)) )
				//{
				//	Thread tasks = new Thread(new ThreadStart(ReadTask));
				//	tasks.Start();

				//}
				//if (Settings.CheckFileExists(nameof(Settings.GshopDataPath)) )
				//{
				//	Thread gshop = new Thread(new ThreadStart(ReadShops));
				//	gshop.Start();

				//}

			}
		}
		private async Task LoadElementData()
		{
			if (Settings != null && Settings.CheckFileExists(nameof(Settings.ElementsDataPath)))
			{
				try
				{
					var progress = new Progress<int>(value =>
					{
						progressBar.Value = value;
					});

					sELeditDatas.eLC = await Task.Run(() => new eListCollection(Settings.ElementsDataPath, progress));

					SortedList ItemUse = new SortedList();
					for (int i = 0; i < sELeditDatas.eLC.Lists.Length; i++)
					{
						if (sELeditDatas.eLC.Lists[i].itemUse == true)
						{
							if (!ItemUse.ContainsKey(i))
							{
								ItemUse.Add(i, i);
							}

						}

					}
					MainWindow.eLC = sELeditDatas.eLC;
					EventGlobal.Publish(sELeditDatas.eLC);

					//database.ItemUse = ItemUse;
					//if (sELeditDatas.eLC.ConfigFile != null)
					//{
					//	string[] referencefiles = Directory.GetFiles(Application.StartupPath + "\\configs", "references.txt");
					//	if (referencefiles.Length > 0)
					//	{
					//		StreamReader sr = new StreamReader(referencefiles[0]);
					//		char[] chars = { ';', ',' };
					//		string[] x;
					//		xrefs = new string[sELeditDatas.eLC.Lists.Length][];
					//		string line;
					//		while (!sr.EndOfStream)
					//		{
					//			line = sr.ReadLine();
					//			if (!line.StartsWith("#"))
					//			{
					//				x = line.Split(chars);
					//				if (int.Parse(x[0]) < sELeditDatas.eLC.Lists.Length)
					//				{
					//					xrefs[int.Parse(x[0])] = x;
					//				}
					//			}
					//		}
					//		this.toolStripSeparator6.Visible = true;
					//		// this.xrefItemToolStripMenuItem.Visible = true;
					//	}
					//}

					//if (sELeditDatas.eLC.ConversationListIndex > -1 && sELeditDatas.eLC.Lists.Length > sELeditDatas.eLC.ConversationListIndex)
					//{
					//	conversationList = new eListConversation((byte[])sELeditDatas.eLC.Lists[sELeditDatas.eLC.ConversationListIndex].elementValues[0][0]);
					//}

					//}





				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message + "\n\n\n" + e);

				}
			}


		}
		//private Task ReadTask()
		//{
		//	if (File.Exists(XmlData.TasksDataPath))
		//	{
		//		try
		//		{
		//			ATaskTempl[] Tasks = null;
		//			FileStream input = File.OpenRead(XmlData.TasksDataPath);
		//			BinaryReader binaryStream = new BinaryReader(input);
		//			TASK_PACK_HEADER tph = new TASK_PACK_HEADER(binaryStream);
		//			if (tph.magic == -1819966623 || tph.magic == 0)
		//			{
		//				if (!GlobalData.Versions.Contains(tph.version))
		//				{
		//					binaryStream.Close();
		//					input.Close();


		//				}
		//				else
		//				{

		//					GlobalData.NewID = 0;
		//					GlobalData.version = tph.version;

		//					int[] pOffs = new int[tph.item_count];
		//					for (int i = 0; i < tph.item_count; i++)
		//					{
		//						pOffs[i] = binaryStream.ReadInt32();
		//					}
		//					Tasks = new ATaskTempl[tph.item_count];

		//					IProgress<int> progress = new Progress<int>(value =>
		//					{

		//					});
		//					if (true)
		//					{
		//						var p = 0;
		//						Parallel.For(0, tph.item_count, i =>
		//						{
		//							byte[] bytes = null;
		//							lock (binaryStream)
		//							{
		//								binaryStream.BaseStream.Seek(pOffs[i], SeekOrigin.Begin);
		//								int count = ((i < pOffs.Length - 1)
		//												? pOffs[i + 1]
		//												: (int)binaryStream.BaseStream.Length) - pOffs[i];
		//								bytes = binaryStream.ReadBytes(count);
		//							}

		//							using (var ms = new MemoryStream(bytes))
		//							using (var br = new BinaryReader(ms))
		//							{
		//								if (true)
		//								{
		//									try
		//									{
		//										Tasks[i] = new ATaskTempl(tph.version, br);
		//									}
		//									catch (Exception e)
		//									{
		//										//MessageBox.Show(String.Format(GetLocalization(521), i));
		//										//if (Debug)
		//										//    Extensions.WriteLog("Error load task! Task index: " + i);
		//									}
		//								}
		//								else
		//									Tasks[i] = new ATaskTempl(tph.version, br);
		//							}

		//							Interlocked.Increment(ref p);
		//							if (p % 100 == 0)
		//							{
		//								progress.Report(p / 2);
		//								Application.DoEvents();
		//							}
		//						}); //3.5 мс для 146 версии
		//					}
		//					else
		//					{
		//						for (int i = 0; i < tph.item_count; i++)
		//						{
		//							// Tasks[i] = new ATaskTempl(tph.version, binaryStream, pOffs[i]);
		//							if (i % 100 == 0)
		//							{
		//								progress.Report(i / 2);
		//								Application.DoEvents();
		//							}
		//						}
		//					}

		//					binaryStream.Close();
		//					input.Close();

		//					Tasks = Tasks.Where(it => it != null).ToArray();

		//					void add_node(ATaskTempl[] tasks, TreeNodeCollection nodes, int GMIconIndex)
		//					{
		//						for (var i = 0; i < tasks.Length; i++)
		//						{
		//							tasks[i].AddNode(nodes, GMIconIndex);
		//							if (tasks[i].pSub.Length > 0)
		//								add_node(tasks[i].pSub, nodes[i].Nodes, GMIconIndex);
		//						}
		//					}



		//				}
		//			}
		//			else
		//			{
		//				binaryStream.Close();
		//				input.Close();
		//			}

		//			database.Tasks = Tasks;
		//		}
		//		catch (Exception ex)
		//		{


		//		}

		//	}
		//	else
		//	{
		//		database.Tasks = null;
		//	}
		//}
		//private Task ReadShops()
		//{
		//	try
		//	{
		//		FileGshop FileGshop = new FileGshop();
		//		FileGshop.ReadFile(XmlData.GshopDataPath, 0);
		//		database.Gshop = FileGshop;
		//	}
		//	catch (Exception ex)
		//	{


		//	}
		//	try
		//	{
		//		FileGshop FileGshop1 = new FileGshop();
		//		FileGshop1.ReadFile(XmlData.Gshop1DataPath, 0);
		//		database.GshopEvent = FileGshop1;
		//	}
		//	catch (Exception ex)
		//	{


		//	}
		//}

		#endregion

	}
}
