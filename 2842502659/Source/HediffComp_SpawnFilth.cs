using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000014 RID: 20
	public class HediffComp_SpawnFilth : HediffComp
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002A1D File Offset: 0x00000C1D
		public HediffCompProperties_SpawnFilth Props
		{
			get
			{
				return this.props as HediffCompProperties_SpawnFilth;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A2C File Offset: 0x00000C2C
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (base.Pawn.Spawned && Gen.IsHashIntervalTick(base.Pawn, this.Props.intervalRate))
			{
				FilthMaker.TryMakeFilth(base.Pawn.Position, base.Pawn.Map, this.Props.filthDef, this.Props.filthCount.RandomInRange, 0, true);
			}
		}
	}
}
