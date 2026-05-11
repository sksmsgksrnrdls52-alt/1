using System;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000DF RID: 223
	public class Comp_PlaySound : ThingComp
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060002FE RID: 766 RVA: 0x000133EF File Offset: 0x000115EF
		public CompProperties_PlaySound Props
		{
			get
			{
				return (CompProperties_PlaySound)this.props;
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x000133FC File Offset: 0x000115FC
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.sustainer == null || this.sustainer.Ended)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(this.Props.sustainer, SoundInfo.InMap(this.parent, 1));
			}
			if (this.Props.sustainer != null)
			{
				this.sustainer.Maintain();
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00013471 File Offset: 0x00011671
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.cell = this.parent.Position;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0001348C File Offset: 0x0001168C
		public override void PostDeSpawn(Map map, DestroyMode mode = 0)
		{
			base.PostDeSpawn(map, mode);
			if (this.Props.sustainer != null && !this.sustainer.Ended)
			{
				Sustainer sustainer = this.sustainer;
				if (sustainer != null)
				{
					sustainer.End();
				}
			}
			SoundDef endSound = this.Props.endSound;
			if (endSound == null)
			{
				return;
			}
			SoundStarter.PlayOneShot(endSound, new TargetInfo(this.cell, map, false));
		}

		// Token: 0x0400018F RID: 399
		private Sustainer sustainer;

		// Token: 0x04000190 RID: 400
		private IntVec3 cell;
	}
}
