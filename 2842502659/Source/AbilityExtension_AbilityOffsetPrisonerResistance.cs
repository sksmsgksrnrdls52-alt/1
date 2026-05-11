using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200005F RID: 95
	public class AbilityExtension_AbilityOffsetPrisonerResistance : AbilityExtension_AbilityMod
	{
		// Token: 0x06000114 RID: 276 RVA: 0x0000698C File Offset: 0x00004B8C
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					float num = this.offset * StatExtension.GetStatValue(pawn, StatDefOf.PsychicSensitivity, true, -1);
					pawn.guest.resistance = Mathf.Max(pawn.guest.resistance + num, 0f);
				}
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006A04 File Offset: 0x00004C04
		public override bool CanApplyOn(LocalTargetInfo target, Ability ability, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return pawn != null && pawn.IsPrisonerOfColony && (pawn == null || pawn.guest.resistance >= float.Epsilon) && !pawn.Downed && this.Valid(new GlobalTargetInfo[]
			{
				target.ToGlobalTargetInfo(target.Thing.Map)
			}, ability, false);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00006A74 File Offset: 0x00004C74
		public override bool Valid(GlobalTargetInfo[] targets, Ability ability, bool throwMessages = false)
		{
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null && !AbilityUtility.ValidateHasResistance(pawn, throwMessages, null))
				{
					return false;
				}
			}
			return base.Valid(targets, ability, throwMessages);
		}

		// Token: 0x04000049 RID: 73
		public float offset;
	}
}
