using System;
using HarmonyLib;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000084 RID: 132
	[HarmonyPatch(typeof(Pawn), "Kill")]
	public static class Pawn_Kill_Patch
	{
		// Token: 0x06000187 RID: 391 RVA: 0x0000883B File Offset: 0x00006A3B
		private static bool Prefix(Pawn __instance)
		{
			return __instance.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_DeathShield, false) == null;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008858 File Offset: 0x00006A58
		private static void Postfix(Pawn __instance, DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			if (__instance.Dead)
			{
				if (dinfo != null)
				{
					Pawn pawn = dinfo.Value.Instigator as Pawn;
					if (pawn != null)
					{
						Hediff_Ability hediff_Ability = pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_ControlledFrenzy, false) as Hediff_Ability;
						if (hediff_Ability != null)
						{
							pawn.psychicEntropy.TryAddEntropy(-10f, null, true, false);
							HediffUtility.TryGetComp<HediffComp_Disappears>(hediff_Ability).ticksToDisappear = hediff_Ability.ability.GetDurationForPawn();
						}
					}
				}
				Hediff firstHediffOfDef = __instance.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_IceBlock, false);
				if (firstHediffOfDef != null)
				{
					__instance.health.RemoveHediff(firstHediffOfDef);
				}
			}
		}
	}
}
