using System;
using VEF.Abilities;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000086 RID: 134
	public class CompProperties_UseEffect_Psytrainer : CompProperties_UseEffectGiveAbility
	{
		// Token: 0x0600018D RID: 397 RVA: 0x0000896F File Offset: 0x00006B6F
		public CompProperties_UseEffect_Psytrainer()
		{
			this.compClass = typeof(CompPsytrainer);
		}
	}
}
