using System;
using Verse;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x0200011D RID: 285
	public class CompProperties_Darkness : CompProperties
	{
		// Token: 0x06000416 RID: 1046 RVA: 0x000192F8 File Offset: 0x000174F8
		public CompProperties_Darkness()
		{
			this.compClass = typeof(CompDarkener);
		}

		// Token: 0x040001CB RID: 459
		public float darknessRange;
	}
}
