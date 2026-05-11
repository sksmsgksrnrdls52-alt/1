using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000089 RID: 137
	[HarmonyPatch(typeof(VerbProperties), "AdjustedMeleeDamageAmount", new Type[]
	{
		typeof(Verb),
		typeof(Pawn)
	})]
	public static class VerbProperties_AdjustedMeleeDamageAmount_Patch
	{
		// Token: 0x06000192 RID: 402 RVA: 0x00008AE6 File Offset: 0x00006CE6
		private static void Postfix(ref float __result, Verb ownerVerb, Pawn attacker)
		{
			if (attacker != null && VerbProperties_AdjustedMeleeDamageAmount_Patch.multiplyByPawnMeleeSkill)
			{
				__result *= (float)attacker.skills.GetSkill(SkillDefOf.Melee).Level / 10f * StatExtension.GetStatValue(attacker, StatDefOf.PsychicSensitivity, true, -1);
			}
		}

		// Token: 0x0400006C RID: 108
		public static bool multiplyByPawnMeleeSkill;
	}
}
