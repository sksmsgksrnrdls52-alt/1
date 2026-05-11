using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000099 RID: 153
	[HarmonyPatch(typeof(Hediff_Psylink), "PostAdd")]
	public static class Hediff_Psylink_PostAdd
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x0000A624 File Offset: 0x00008824
		public static void Postfix(Hediff_Psylink __instance)
		{
			((Hediff_PsycastAbilities)__instance.pawn.health.AddHediff(VPE_DefOf.VPE_PsycastAbilityImplant, __instance.Part, null, null)).InitializeFromPsylink(__instance);
		}
	}
}
