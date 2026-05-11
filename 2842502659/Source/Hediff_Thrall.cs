using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200005C RID: 92
	public class Hediff_Thrall : HediffWithComps
	{
		// Token: 0x06000103 RID: 259 RVA: 0x00006565 File Offset: 0x00004765
		public override void Tick()
		{
			base.Tick();
			int num = Find.TickManager.TicksGame % 60;
		}
	}
}
