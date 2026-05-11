using System;
using HarmonyLib;
using RimWorld;
using VEF.Abilities;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000024 RID: 36
	[HarmonyPatch(typeof(PawnGroupMaker), "CanGenerateFrom")]
	public static class PawnGroupMaker_CanGenerateFrom_Patch
	{
		// Token: 0x0600005C RID: 92 RVA: 0x0000383C File Offset: 0x00001A3C
		public static void Postfix(ref bool __result, PawnGroupMaker __instance, PawnGroupMakerParms parms)
		{
			if (__result && __instance is PawnGroupMaker_PsycasterRaid)
			{
				RaidStrategyDef raidStrategy = parms.raidStrategy;
				bool flag;
				if (((raidStrategy != null) ? raidStrategy.Worker : null) is RaidStrategyWorker_ImmediateAttack_Psycasters && __instance.options != null)
				{
					flag = __instance.options.Exists((PawnGenOption x) => x.kind.HasModExtension<PawnKindAbilityExtension>());
				}
				else
				{
					flag = false;
				}
				__result = flag;
			}
		}
	}
}
