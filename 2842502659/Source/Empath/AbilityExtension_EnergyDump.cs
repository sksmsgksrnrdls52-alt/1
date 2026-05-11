using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Empath
{
	// Token: 0x020000C7 RID: 199
	public class AbilityExtension_EnergyDump : AbilityExtension_AbilityMod
	{
		// Token: 0x06000299 RID: 665 RVA: 0x0000EC90 File Offset: 0x0000CE90
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (((needs != null) ? needs.rest : null) != null)
					{
						pawn.needs.rest.CurLevel = 1f;
						ability.pawn.needs.rest.CurLevel = 0f;
					}
				}
			}
		}
	}
}
