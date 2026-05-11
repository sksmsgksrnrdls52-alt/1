using System;
using HarmonyLib;
using RimWorld;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000028 RID: 40
	[HarmonyPatch(typeof(Pawn_AbilityTracker), "Notify_TemporaryAbilitiesChanged")]
	public static class Pawn_AbilityTracker_Notify_TemporaryAbilitiesChanged_Patch
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003BC7 File Offset: 0x00001DC7
		public static void Postfix(Pawn_AbilityTracker __instance)
		{
			__instance.pawn.RecheckPaths();
		}
	}
}
