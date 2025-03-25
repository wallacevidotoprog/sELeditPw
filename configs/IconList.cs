//(c)Rey35
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace sELedit.configs
{
	public class IconList
	{
		//public IconList()
		//{
		//}

		//public Size IconSize;
		//int RowCount;
		//int ColumnCount;
		//public Dictionary<string, Image> Icons;

		//public IconList(package.FileInfo[] FileTable, string SurfacesParh, string IconListName)
		//{
		//	CreatIconList(FileTable, SurfacesParh, IconListName);
		//}

		//private void CreatIconList(package.FileInfo[] FileTable, string SurfacesParh, string IconListName)
		//{
		//	StreamReader sr_txt = new StreamReader(package.Functions.GetFile(FileTable, SurfacesParh, "-pw", IconListName + ".txt"), Encoding.GetEncoding("GBK"));
		//	string line;
		//	List<string> IconsName = new List<string>();
		//	while (!sr_txt.EndOfStream)
		//	{
		//		line = sr_txt.ReadLine();
		//		if (line != "" && !line.StartsWith("/") && !line.StartsWith("#"))
		//			IconsName.Add(line.ToLower());
		//	}
		//	IconSize = new Size(Convert.ToInt32(IconsName[0]), Convert.ToInt32(IconsName[1]));
		//	RowCount = Convert.ToInt32(IconsName[2]);
		//	ColumnCount = Convert.ToInt32(IconsName[3]);
		//	IconsName.RemoveRange(0, 4);
		//	sr_txt.Close();
		//	MemoryStream ms_img = package.Functions.GetFile(FileTable, SurfacesParh, "-pw", IconListName + ".dds");
		//	BinaryReader binaryStream = new BinaryReader(ms_img);
		//	byte[] bytes = new byte[ms_img.Length];
		//	bytes = binaryStream.ReadBytes(bytes.Length);
		//	DDS.DDSImage dds = new DDS.DDSImage(bytes);
		//	Bitmap iconlist_img = dds.BitmapImage;
		//	bytes = null;
		//	ms_img.Close();
		//	binaryStream.Close();
		//	int icon_count = 0;
		//	Icons = new Dictionary<string, Image>();
		//	for (int row_i = 0; row_i <= RowCount && icon_count < IconsName.Count; row_i++)
		//	{
		//		for (int column_i = 0; column_i < ColumnCount && icon_count < IconsName.Count; column_i++)
		//		{
		//			Image icon = iconlist_img.Clone(new Rectangle(IconSize.Width * column_i, IconSize.Height * row_i, IconSize.Width, IconSize.Height), iconlist_img.PixelFormat);
		//			Image result = new Bitmap(GlobalProgramData.ElementIconWidth, GlobalProgramData.ElementIconHeight);
		//			using (Graphics g = Graphics.FromImage(result))
		//			{
		//				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
		//				g.DrawImage(icon, 0, 0, GlobalProgramData.ElementIconWidth, GlobalProgramData.ElementIconHeight);
		//				g.Dispose();
		//			}
		//			Icons.Add(IconsName[icon_count], result);
		//			icon_count++;
		//		}
		//	}
		//}
	}
}
//(c)Rey35