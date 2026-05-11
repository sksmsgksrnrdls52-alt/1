using System;
using HarmonyLib;
using RimWorld;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000029 RID: 41
	[HarmonyPatch(typeof(Pawn_GeneTracker), "Notify_GenesChanged")]
	public static class Pawn_GeneTracker_Notify_GenesChanged_Patch
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003BD4 File Offset: 0x00001DD4
		public static void Postfix(Pawn_GeneTracker __instance)
		{
			__instance.pawn.RecheckPaths();
		}
	}
}
