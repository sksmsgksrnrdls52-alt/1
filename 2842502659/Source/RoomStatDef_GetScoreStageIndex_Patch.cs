using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200003F RID: 63
	[HarmonyPatch(typeof(RoomStatDef), "GetScoreStageIndex")]
	public static class RoomStatDef_GetScoreStageIndex_Patch
	{
		// Token: 0x060000BF RID: 191 RVA: 0x000053F5 File Offset: 0x000035F5
		public static void Postfix(RoomStatDef __instance, ref int __result)
		{
			if (RoomStatDef_GetScoreStageIndex_Patch.forPawn != null && RoomStatDef_GetScoreStageIndex_Patch.forPawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_Hallucination, false) != null)
			{
				__result = __instance.scoreStages.Count - 1;
			}
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}

		// Token: 0x0400003D RID: 61
		public static Pawn forPawn;
	}
}
