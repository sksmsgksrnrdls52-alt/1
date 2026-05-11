using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000043 RID: 67
	[HarmonyPatch(typeof(ThoughtWorker_Greedy), "CurrentStateInternal")]
	public static class ThoughtWorker_Greedy_CurrentStateInternal_Patch
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x0000549E File Offset: 0x0000369E
		public static void Prefix(Pawn p)
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = p;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000054A6 File Offset: 0x000036A6
		public static void Postfix()
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}
	}
}
