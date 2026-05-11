using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000068 RID: 104
	public class CompMelter : ThingComp
	{
		// Token: 0x0600012D RID: 301 RVA: 0x000071BC File Offset: 0x000053BC
		public override void CompTick()
		{
			base.CompTick();
			if (Gen.IsHashIntervalTick(this.parent, 60))
			{
				float ambientTemperature = this.parent.AmbientTemperature;
				if (ambientTemperature > 0f)
				{
					this.damageBuffer += ambientTemperature / 41.66f;
					if (this.damageBuffer >= 1f)
					{
						this.parent.HitPoints -= (int)this.damageBuffer;
						this.damageBuffer = 0f;
					}
					if (this.parent.HitPoints < 0)
					{
						FilthMaker.TryMakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_Water, 1, 0, true);
						this.parent.Destroy(0);
					}
				}
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000727B File Offset: 0x0000547B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.damageBuffer, "damageBuffer", 0f, false);
		}

		// Token: 0x04000051 RID: 81
		public float damageBuffer;
	}
}
