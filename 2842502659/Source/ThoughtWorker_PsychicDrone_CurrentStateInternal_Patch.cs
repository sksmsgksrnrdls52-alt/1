using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200004E RID: 78
	[HarmonyPatch(typeof(ThoughtWorker_PsychicDrone), "CurrentStateInternal")]
	public static class ThoughtWorker_PsychicDrone_CurrentStateInternal_Patch
	{
		// Token: 0x060000DD RID: 221 RVA: 0x000058B5 File Offset: 0x00003AB5
		public static void Postfix(Pawn p, ref ThoughtState __result)
		{
			if (__result.StageIndex != 0 && p.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsychicSoothe, false) != null)
			{
				__result = ThoughtState.ActiveAtStage(0);
			}
		}
	}
}
