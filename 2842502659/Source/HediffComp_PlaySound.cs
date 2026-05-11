using System;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000BB RID: 187
	public class HediffComp_PlaySound : HediffComp
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000DB3D File Offset: 0x0000BD3D
		public HediffCompProperties_PlaySound Props
		{
			get
			{
				return (HediffCompProperties_PlaySound)this.props;
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.Props.sustainer != null)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = SoundStarter.TrySpawnSustainer(this.Props.sustainer, SoundInfo.InMap(base.Pawn, 1));
				}
				this.sustainer.Maintain();
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000DBB4 File Offset: 0x0000BDB4
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (this.Props.sustainer != null && !this.sustainer.Ended)
			{
				Sustainer sustainer = this.sustainer;
				if (sustainer != null)
				{
					sustainer.End();
				}
			}
			if (this.Props.endSound != null)
			{
				SoundStarter.PlayOneShot(this.Props.endSound, base.Pawn);
			}
		}

		// Token: 0x040000B3 RID: 179
		private Sustainer sustainer;
	}
}
