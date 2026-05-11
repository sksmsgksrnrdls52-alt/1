using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B3 RID: 179
	public class Hediff_GuardianSkipBarrier : Hediff_Overshield
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000CE68 File Offset: 0x0000B068
		public override Color OverlayColor
		{
			get
			{
				return new ColorInt(79, 141, 247).ToColor;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000CE8E File Offset: 0x0000B08E
		public override float OverlaySize
		{
			get
			{
				return 9f;
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000CE95 File Offset: 0x0000B095
		protected override void DestroyProjectile(Projectile projectile)
		{
			base.DestroyProjectile(projectile);
			this.AddEntropy();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000CEA4 File Offset: 0x0000B0A4
		public override void PostTick()
		{
			base.PostTick();
			this.AddEntropy();
			if (this.sustainer == null || this.sustainer.Ended)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(VPE_DefOf.VPE_GuardianSkipbarrier_Sustainer, SoundInfo.InMap(this.pawn, 1));
			}
			this.sustainer.Maintain();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000CEFE File Offset: 0x0000B0FE
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (!this.sustainer.Ended)
			{
				Sustainer sustainer = this.sustainer;
				if (sustainer == null)
				{
					return;
				}
				sustainer.End();
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000CF24 File Offset: 0x0000B124
		private void AddEntropy()
		{
			if (Find.TickManager.TicksGame % 10 == 0)
			{
				this.pawn.psychicEntropy.TryAddEntropy(1f, null, true, true);
			}
			if (this.pawn.psychicEntropy.EntropyValue >= this.pawn.psychicEntropy.MaxEntropy)
			{
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x040000A4 RID: 164
		private Sustainer sustainer;
	}
}
