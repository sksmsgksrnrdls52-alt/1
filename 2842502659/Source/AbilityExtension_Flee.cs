using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;
using Verse.AI.Group;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200007F RID: 127
	public class AbilityExtension_Flee : AbilityExtension_AbilityMod
	{
		// Token: 0x0600017D RID: 381 RVA: 0x000085F4 File Offset: 0x000067F4
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (!this.onlyHostile || !GenHostility.HostileTo(pawn, ability.pawn))
				{
					return;
				}
				Lord lord = LordUtility.GetLord(pawn);
				if (lord != null)
				{
					lord.RemovePawn(pawn);
				}
				pawn.jobs.EndCurrentJob(16, true, true);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.PanicFlee, ability.def.label, true, false, false, null, true, false, true);
			}
		}

		// Token: 0x04000065 RID: 101
		public bool onlyHostile = true;
	}
}
