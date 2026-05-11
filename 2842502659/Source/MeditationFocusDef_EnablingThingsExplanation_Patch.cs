using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000048 RID: 72
	[HarmonyPatch(typeof(MeditationFocusDef), "EnablingThingsExplanation")]
	public static class MeditationFocusDef_EnablingThingsExplanation_Patch
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x0000557C File Offset: 0x0000377C
		public static void Postfix(Pawn pawn, MeditationFocusDef __instance, ref string __result)
		{
			Hediff_PsycastAbilities hediff_PsycastAbilities = pawn.Psycasts();
			if (hediff_PsycastAbilities != null && hediff_PsycastAbilities.unlockedMeditationFoci.Contains(__instance))
			{
				__result += "\n  - " + Translator.Translate("VPE.UnlockedByPoints") + ".";
			}
		}
	}
}
