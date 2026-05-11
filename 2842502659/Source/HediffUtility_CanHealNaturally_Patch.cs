using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000020 RID: 32
	[HarmonyPatch(typeof(HediffUtility), "CanHealNaturally")]
	public static class HediffUtility_CanHealNaturally_Patch
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000031AC File Offset: 0x000013AC
		private static void Postfix(ref bool __result, Hediff_Injury hd)
		{
			if (__result)
			{
				__result = (hd.def != VPE_DefOf.VPE_Regenerating);
			}
		}
	}
}
