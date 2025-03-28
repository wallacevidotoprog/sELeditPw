﻿//using PWDE.Element_Editor_Classes;
using eELedit;
using Newtonsoft.Json;
using sELedit.CORE.MODEL;
using sELedit.gShop;
using sELedit.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using tasks;

namespace sELedit
{

	public class CacheSave
	{
		[JsonIgnore]
		public Bitmap sourceBitmap = null;

		[JsonIgnore]
		public bool started = false;

		[JsonIgnore]
		public SortedDictionary<string, Bitmap> imagesChache = new SortedDictionary<string, Bitmap>();

		public bool ContainsKey(string path)
		{
			return imageposition != null && imageposition.ContainsKey(path);
		}

		public Bitmap images(string name)
		{
			if (sourceBitmap != null)
			{
				if (imageposition.ContainsKey(name))
				{
					if (imagesChache != null && imagesChache.ContainsKey(name))
					{
						return imagesChache[name];
					}
					int w = 32;
					int h = 32;
					Point d = imageposition[name];
					Bitmap pageBitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
					using (Graphics graphics = Graphics.FromImage(pageBitmap))
					{
						graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, w, h), new Rectangle(d.X, d.Y, w, h), GraphicsUnit.Pixel);
					}
					if (imagesChache == null || imagesChache != null && !imagesChache.ContainsKey(name))
					{
						imagesChache[name] = pageBitmap;
					}
					return pageBitmap;
				}

			}
			return new Bitmap(new Bitmap(Resources.blank));
		}



		public LanguageFiles LanguageFiles = null;
		public SortedList LocalizationText = null; // TROCA PELO DE CIMA

		public PCKs pckCongigs = null;
		public PCKs pckSurfaces = null;


		public int rows = 0;
		public int cols = 0;
		public SortedList<int, String> imagesx = null;
		public SortedList<string, Point> imageposition = null;
		public SortedList<int, int> item_color = null;
		//public SortedList item_color;
		public SortedList<int, string> item_desc = null;
		//public SortedList<string, string> arrTheme = null;
		public List<string> arrTheme = null;
		//public SortedList<int, ItemDupe> task_recipes = null;
		public SortedList<int, ItemDupe> task_items = null;
		public string[] task_items_list = null;
		public SortedList monsters_npcs_mines = null;
		public SortedList titles = null;
		public SortedList homeitems = null;
		public SortedList InstanceList = null;
		public string[] buff_str = null;
		//public string[] item_ext_desc = null;
		public SortedList<int, string> item_ext_desc = null;
		public string[] skillstr = null;
		public string[] world_targets = null;
		public SortedList addonslist = null;

		public SortedList _wepon = null;
		public SortedList _armor = null;
		public SortedList _decoration = null;
		public SortedList _suite = null;
		public ATaskTempl[] Tasks = null;
		public FileGshop Gshop = null;
		public FileGshop GshopEvent = null;
		public SortedList ItemUse = null;
		//public SortedList<int, Image> ImageTask = null;
		public ImageList ImageTask;
	}
}
