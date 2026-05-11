using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Graphics
{
	// Token: 0x020000CC RID: 204
	public class Graphic_AnimatedMote : Graphic_Animated
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x0000F880 File Offset: 0x0000DA80
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			if (GenText.NullOrEmpty(req.path))
			{
				throw new ArgumentNullException("folderPath");
			}
			if (req.shader == null)
			{
				throw new ArgumentNullException("shader");
			}
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			List<Texture2D> list = (from x in ContentFinder<Texture2D>.GetAllInFolder(req.path)
			where !x.name.EndsWith(Graphic_Single.MaskSuffix)
			orderby x.name
			select x).ToList<Texture2D>();
			if (GenList.NullOrEmpty<Texture2D>(list))
			{
				Log.Error("Collection cannot init: No textures found at path " + req.path);
				this.subGraphics = new Graphic[]
				{
					BaseContent.BadGraphic
				};
				return;
			}
			List<Graphic> list2 = new List<Graphic>();
			foreach (IGrouping<string, Texture2D> grouping in from s in list
			group s by s.name.Split(new char[]
			{
				'_'
			})[0])
			{
				List<Texture2D> list3 = grouping.ToList<Texture2D>();
				string text = req.path + "/" + grouping.Key;
				bool flag = false;
				for (int i = list3.Count - 1; i >= 0; i--)
				{
					if (list3[i].name.Contains("_east") || list3[i].name.Contains("_north") || list3[i].name.Contains("_west") || list3[i].name.Contains("_south"))
					{
						list3.RemoveAt(i);
						flag = true;
					}
				}
				if (list3.Count > 0)
				{
					foreach (Texture2D texture2D in list3)
					{
						list2.Add(GraphicDatabase.Get(typeof(Graphic_Mote), req.path + "/" + texture2D.name, req.shader, this.drawSize, this.color, this.colorTwo, this.data, req.shaderParameters, null));
					}
				}
				if (flag)
				{
					list2.Add(GraphicDatabase.Get(typeof(Graphic_Multi), text, req.shader, this.drawSize, this.color, this.colorTwo, this.data, req.shaderParameters, null));
				}
			}
			this.subGraphics = list2.ToArray();
		}
	}
}
