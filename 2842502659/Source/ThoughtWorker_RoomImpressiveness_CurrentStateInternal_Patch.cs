using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000045 RID: 69
	[HarmonyPatch(typeof(ThoughtWorker_RoomImpressiveness), "CurrentStateInternal")]
	public static class ThoughtWorker_RoomImpressiveness_CurrentStateInternal_Patch
	{
		// Token: 0x060000CB RID: 203 RVA: 0x000054BE File Offset: 0x000036BE
		public static void Prefix(Pawn p)
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = p;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000054C6 File Offset: 0x000036C6
		public static void Postfix()
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}
	}
}
