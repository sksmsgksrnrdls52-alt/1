using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000010 RID: 16
	public class HediffComp_SpawnMote : HediffComp
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000291A File Offset: 0x00000B1A
		public HediffCompProperties_SpawnMote Props
		{
			get
			{
				return this.props as HediffCompProperties_SpawnMote;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002928 File Offset: 0x00000B28
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.spawnedMote == null)
			{
				this.spawnedMote = MoteMaker.MakeAttachedOverlay(base.Pawn, this.Props.moteDef, this.Props.offset, 1f, -1f);
				MoteAttachedScaled moteAttachedScaled = this.spawnedMote as MoteAttachedScaled;
				if (moteAttachedScaled != null)
				{
					moteAttachedScaled.maxScale = this.Props.maxScale;
				}
			}
			if (this.spawnedMote.def.mote.needsMaintenance)
			{
				this.spawnedMote.Maintain();
			}
			base.CompPostTick(ref severityAdjustment);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000029B7 File Offset: 0x00000BB7
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Mote>(ref this.spawnedMote, "spawnedMote", false);
		}

		// Token: 0x0400000C RID: 12
		public Mote spawnedMote;
	}
}
