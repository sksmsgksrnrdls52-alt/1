using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Graphics
{
	// Token: 0x020000D2 RID: 210
	public abstract class Graphic_FleckCollection : Graphic_Fleck
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x00010064 File Offset: 0x0000E264
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
				this.subGraphics = new Graphic_Fleck[0];
				return;
			}
			this.subGraphics = (from texture2D in list
			select (Graphic_Fleck)GraphicDatabase.Get(typeof(Graphic_Fleck), req.path + "/" + texture2D.name, req.shader, this.drawSize, this.color, this.colorTwo, this.data, req.shaderParameters, null)).ToArray<Graphic_Fleck>();
		}

		// Token: 0x0400015C RID: 348
		protected Graphic_Fleck[] subGraphics;
	}
}
