using sELedit.CORE.BASE;
using sELedit.CORE.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace sELedit.configs
{
	public partial class Configs : Form
	{
		public Configs()
		{
			InitializeComponent();

			// this.BackgroundImage = configs.ImgsFiles.TrueStretchImage(Resources._base, this.Width, this.Height);
		}
		public string elementData { get; set; }
		public string configPCK { get; set; }
		public string surfacePCK { get; set; }
		public string tasksData { get; set; }
		public string gshop { get; set; }
		public string gshop1 { get; set; }
		public bool isModified { get; set; }


		Settings XmlData;
		// Tasks
		string caminho = Path.Combine(Application.StartupPath, "Settings.xml");

		private void Elements_data_search_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				//openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				openFileDialog.Filter = "elements.data|*.data|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					elementData = openFileDialog.FileName.ToString();
					Elements_path_textbox.Text = elementData;
				}
			}
		}

		private void Surfaces_pck_search_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				//openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				openFileDialog.Filter = "surface.pck|*.pck|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					surfacePCK = openFileDialog.FileName.ToString();
					Surfaces_path_textbox.Text = surfacePCK;
				}
			}
		}

		private void Configs_search_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				//openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				openFileDialog.Filter = "configs.pck|*.pck|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					configPCK = openFileDialog.FileName.ToString();
					Configs_path.Text = configPCK;
				}
			}
		}

		private void Configs_Load(object sender, EventArgs e)
		{

			if (sELeditCache.Instance.Settings == null)
			{
				sELeditCache.Instance.Settings = new CORE.MODEL.Settings();
			}

			Elements_path_textbox.Text = sELeditCache.Instance.Settings.ElementsDataPath;
			Configs_path.Text = sELeditCache.Instance.Settings.ConfigsPckPath;
			Surfaces_path_textbox.Text = sELeditCache.Instance.Settings.SurfacesPckPath;
			textBox_Tasks.Text = sELeditCache.Instance.Settings.TasksDataPath;
			textBox_gshop.Text = sELeditCache.Instance.Settings.GshopDataPath;
			textBox_gshop1.Text = sELeditCache.Instance.Settings.Gshop1DataPath;


		}

		private void Exit_button_Click(object sender, EventArgs e)
		{
			isModified = false;
			this.Close();
		}

		private void Accept_button_Click(object sender, EventArgs e)
		{
			var campos = new Dictionary<string, (Func<string> getValorAntigo, Action<string> setValorNovo, TextBox textBox)>
			{
				{ "elementData", (() => sELeditCache.Instance.Settings.ElementsDataPath, val => sELeditCache.Instance.Settings.ElementsDataPath = val, Elements_path_textbox) },
				{ "surfacePCK", (() => sELeditCache.Instance.Settings.SurfacesPckPath, val => sELeditCache.Instance.Settings.SurfacesPckPath = val, Surfaces_path_textbox) },
				{ "configPCK", (() => sELeditCache.Instance.Settings.ConfigsPckPath, val => sELeditCache.Instance.Settings.ConfigsPckPath = val, Configs_path) },
				{ "tasksData", (() => sELeditCache.Instance.Settings.TasksDataPath, val => sELeditCache.Instance.Settings.TasksDataPath = val, textBox_Tasks) },
				{ "gshop", (() => sELeditCache.Instance.Settings.GshopDataPath, val => sELeditCache.Instance.Settings.GshopDataPath = val, textBox_gshop) },
				{ "gshop1", (() => sELeditCache.Instance.Settings.Gshop1DataPath, val => sELeditCache.Instance.Settings.Gshop1DataPath = val, textBox_gshop1) }
			};

			foreach (var campo in campos)
			{
				if (campo.Value.getValorAntigo() != campo.Value.textBox.Text)
				{
					isModified = true;					
					campo.Value.setValorNovo(campo.Value.textBox.Text);
				}
			}

			if (isModified)
			{
				ReadFile.ReadWriteSettings(IOAction.Write);
			}
			//elementData = Elements_path_textbox.Text;
			//surfacePCK = Surfaces_path_textbox.Text;
			//configPCK = Configs_path.Text;
			//tasksData = textBox_Tasks.Text;
			//gshop = textBox_gshop.Text;
			//gshop1 = textBox_gshop1.Text;

			//XmlData.ElementsDataPath = elementData;
			//XmlData.ConfigsPckPath = configPCK;
			//XmlData.SurfacesPckPath = surfacePCK;
			//XmlData.TasksDataPath = tasksData;
			//XmlData.GshopDataPath = gshop;
			//XmlData.Gshop1DataPath = gshop1;



			//using (var writer = new StringWriter())
			//{
			//	new XmlSerializer(XmlData.GetType()).Serialize(writer, XmlData);
			//	File.Delete(caminho);
			//	StreamWriter sw3 = new StreamWriter(caminho, true, Encoding.UTF8);
			//	sw3.Write(writer.ToString());
			//	sw3.Close();
			//}
			//this.isModified = true;
			this.Close();



		}

		private void button_Tasks_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				openFileDialog.Filter = "tasks.data|*.data|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					tasksData = openFileDialog.FileName.ToString();
					textBox_Tasks.Text = tasksData;
				}
			}
		}

		private void button_gshop_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				openFileDialog.Filter = "gshop.data|*.data|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					gshop = openFileDialog.FileName.ToString();
					textBox_gshop.Text = gshop;
				}
			}
		}

		private void button_gshop1_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				openFileDialog.Filter = "gshop1.data|*.data|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					gshop1 = openFileDialog.FileName.ToString();
					textBox_gshop1.Text = gshop1;
				}
			}
		}
	}
}
