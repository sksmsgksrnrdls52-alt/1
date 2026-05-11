using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000E2 RID: 226
	public class Ability_EssenceTransfer : Ability
	{
		// Token: 0x0600030A RID: 778 RVA: 0x00013634 File Offset: 0x00011834
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Pawn pawn = targets[0].Thing as Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = targets[1].Thing as Pawn;
				if (pawn2 != null)
				{
					Pawn pawn3 = this.curTarget;
					if (pawn3 != null && !pawn3.Dead && !pawn3.Discarded && !pawn3.Destroyed)
					{
						using (List<Hediff_Essence>.Enumerator enumerator = this.curTarget.health.hediffSet.hediffs.OfType<Hediff_Essence>().ToList<Hediff_Essence>().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Hediff_Essence hediff_Essence = enumerator.Current;
								this.curTarget.health.RemoveHediff(hediff_Essence);
							}
							goto IL_B7;
						}
					}
					this.curTarget = null;
					IL_B7:
					Hediff_Essence hediff_Essence2 = (Hediff_Essence)HediffMaker.MakeHediff(VPE_DefOf.VPE_Essence, pawn2, null);
					hediff_Essence2.EssenceOf = pawn;
					pawn2.health.AddHediff(hediff_Essence2, null, null, null);
					this.curTarget = pawn2;
					return;
				}
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00013740 File Offset: 0x00011940
		public override float GetRangeForPawn()
		{
			if (this.currentTargetingIndex == 1)
			{
				return 99999f;
			}
			return base.GetRangeForPawn();
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00013757 File Offset: 0x00011957
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.curTarget, "curTarget", false);
		}

		// Token: 0x04000192 RID: 402
		private Pawn curTarget;
	}
}
