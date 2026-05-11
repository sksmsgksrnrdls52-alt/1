using System;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000DE RID: 222
	public class CompProperties_PlaySound : CompProperties
	{
		// Token: 0x060002FD RID: 765 RVA: 0x000133D7 File Offset: 0x000115D7
		public CompProperties_PlaySound()
		{
			this.compClass = typeof(Comp_PlaySound);
		}

		// Token: 0x0400018D RID: 397
		public SoundDef sustainer;

		// Token: 0x0400018E RID: 398
		public SoundDef endSound;
	}
}
