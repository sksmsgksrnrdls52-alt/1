using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000070 RID: 112
	public class Hediff_BodiesConsumed : HediffWithComps
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00007BC7 File Offset: 0x00005DC7
		public override string Label
		{
			get
			{
				return base.Label + ": " + this.consumedBodies.ToString();
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.consumedBodies, "consumedBodies", 0, false);
		}

		// Token: 0x04000054 RID: 84
		public int consumedBodies;
	}
}
