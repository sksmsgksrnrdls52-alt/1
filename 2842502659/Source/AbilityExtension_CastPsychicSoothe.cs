using System;
using System.Collections.Generic;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A6 RID: 166
	public class AbilityExtension_CastPsychicSoothe : AbilityExtension_AbilityMod
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000B6FC File Offset: 0x000098FC
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			new List<GlobalTargetInfo>();
			foreach (Pawn pawn in ability.pawn.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (!pawn.Dead && pawn.gender == this.gender && pawn.needs != null && pawn.needs.mood != null)
				{
					ability.ApplyHediff(pawn);
				}
			}
		}

		// Token: 0x04000099 RID: 153
		public Gender gender;
	}
}
