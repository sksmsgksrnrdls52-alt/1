using System;
using HarmonyLib;
using RimWorld;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A3 RID: 163
	[HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), "RechargePsyfocus")]
	public static class Pawn_EntropyTracker_RechargePsyfocus_Postfix
	{
		// Token: 0x060001EA RID: 490 RVA: 0x0000AE30 File Offset: 0x00009030
		[HarmonyPrefix]
		public static void Prefix(Pawn_PsychicEntropyTracker __instance)
		{
			__instance.GainXpFromPsyfocus(1f - __instance.CurrentPsyfocus);
		}
	}
}
