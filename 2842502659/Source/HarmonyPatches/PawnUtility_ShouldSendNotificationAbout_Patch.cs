using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded.HarmonyPatches
{
	// Token: 0x0200013F RID: 319
	[HarmonyPatch(typeof(PawnUtility), "ShouldSendNotificationAbout")]
	public static class PawnUtility_ShouldSendNotificationAbout_Patch
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x0001C0D9 File Offset: 0x0001A2D9
		public static void Postfix(ref bool __result, Pawn p)
		{
			if (__result && p.kindDef == VPE_DefOf.VPE_SummonedSkeleton)
			{
				__result = false;
			}
		}
	}
}
