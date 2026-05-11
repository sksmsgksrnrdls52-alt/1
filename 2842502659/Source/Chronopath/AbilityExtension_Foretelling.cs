using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x0200012E RID: 302
	public class AbilityExtension_Foretelling : AbilityExtension_GiveInspiration
	{
		// Token: 0x0600045D RID: 1117 RVA: 0x0001AAD8 File Offset: 0x00018CD8
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			if (Rand.Chance(0.5f))
			{
				base.Cast(targets, ability);
				return;
			}
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemoryFast(VPE_DefOf.VPE_Future, null);
				}
			}
		}
	}
}
