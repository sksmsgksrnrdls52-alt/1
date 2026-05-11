using System;
using RimWorld.Planet;
using VEF.Abilities;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200006F RID: 111
	public class Ability_StealVitality : Ability
	{
		// Token: 0x0600014F RID: 335 RVA: 0x00007B98 File Offset: 0x00005D98
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			this.ApplyHediff(this.pawn, VPE_DefOf.VPE_GainedVitality, null, this.GetDurationForPawn(), 0f);
		}
	}
}
