using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200007C RID: 124
	[StaticConstructorOnStartup]
	public class CompSetStoneColour : ThingComp
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000084D2 File Offset: 0x000066D2
		public ThingDef KilledLeave
		{
			get
			{
				return this.rockDef;
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000084DA File Offset: 0x000066DA
		public void SetStoneColour(ThingDef thingDef)
		{
			this.rockDef = thingDef;
			this.color = this.rockDef.graphic.data.color;
			(this.parent as Pawn).Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008518 File Offset: 0x00006718
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.rockDef, "rockDef");
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
		}

		// Token: 0x04000063 RID: 99
		public Color color;

		// Token: 0x04000064 RID: 100
		private ThingDef rockDef;
	}
}
