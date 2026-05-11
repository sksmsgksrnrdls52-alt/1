using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A1 RID: 161
	[HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), "GainPsyfocus_NewTemp")]
	public static class Pawn_EntropyTracker_GainPsyfocus_Postfix
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x0000ADCC File Offset: 0x00008FCC
		public static void Postfix(Pawn_PsychicEntropyTracker __instance, int delta, Thing focus = null)
		{
			float gain = MeditationUtility.PsyfocusGainPerTick(__instance.Pawn, focus) * (float)delta;
			__instance.GainXpFromPsyfocus(gain);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public static void GainXpFromPsyfocus(this Pawn_PsychicEntropyTracker __instance, float gain)
		{
			Pawn pawn = __instance.Pawn;
			if (pawn == null)
			{
				return;
			}
			Hediff_PsycastAbilities hediff_PsycastAbilities = pawn.Psycasts();
			if (hediff_PsycastAbilities == null)
			{
				return;
			}
			hediff_PsycastAbilities.GainExperience(gain * 100f * PsycastsMod.Settings.XPPerPercent, true);
		}
	}
}
