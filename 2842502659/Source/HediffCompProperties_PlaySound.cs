using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000BA RID: 186
	public class HediffCompProperties_PlaySound : HediffCompProperties
	{
		// Token: 0x06000266 RID: 614 RVA: 0x0000DB25 File Offset: 0x0000BD25
		public HediffCompProperties_PlaySound()
		{
			this.compClass = typeof(HediffComp_PlaySound);
		}

		// Token: 0x040000B1 RID: 177
		public SoundDef sustainer;

		// Token: 0x040000B2 RID: 178
		public SoundDef endSound;
	}
}
