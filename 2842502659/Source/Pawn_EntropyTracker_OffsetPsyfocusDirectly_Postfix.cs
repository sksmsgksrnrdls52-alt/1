using System;
using HarmonyLib;
using RimWorld;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A2 RID: 162
	[HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), "OffsetPsyfocusDirectly")]
	public static class Pawn_EntropyTracker_OffsetPsyfocusDirectly_Postfix
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x0000AE1F File Offset: 0x0000901F
		[HarmonyPostfix]
		public static void Postfix(Pawn_PsychicEntropyTracker __instance, float offset)
		{
			if (offset > 0f)
			{
				__instance.GainXpFromPsyfocus(offset);
			}
		}
	}
}
