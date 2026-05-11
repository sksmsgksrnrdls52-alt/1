using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000082 RID: 130
	[HarmonyPatch(typeof(HediffSet), "BleedRateTotal", 1)]
	public static class HediffSet_BleedRateTotal_Patch
	{
		// Token: 0x06000185 RID: 389 RVA: 0x000087EB File Offset: 0x000069EB
		public static void Postfix(ref float __result, HediffSet __instance)
		{
			if (__result > 0f && ((__instance != null) ? __instance.GetFirstHediffOfDef(VPE_DefOf.VPE_BlockBleeding, false) : null) != null)
			{
				__result = 0f;
			}
		}
	}
}
