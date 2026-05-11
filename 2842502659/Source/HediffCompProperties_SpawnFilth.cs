using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000013 RID: 19
	public class HediffCompProperties_SpawnFilth : HediffCompProperties
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002A05 File Offset: 0x00000C05
		public HediffCompProperties_SpawnFilth()
		{
			this.compClass = typeof(HediffComp_SpawnFilth);
		}

		// Token: 0x0400000D RID: 13
		public ThingDef filthDef;

		// Token: 0x0400000E RID: 14
		public int intervalRate;

		// Token: 0x0400000F RID: 15
		public IntRange filthCount;
	}
}
