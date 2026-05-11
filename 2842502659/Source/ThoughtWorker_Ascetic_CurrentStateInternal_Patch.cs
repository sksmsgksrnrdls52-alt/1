using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000042 RID: 66
	[HarmonyPatch(typeof(ThoughtWorker_Ascetic), "CurrentStateInternal")]
	public static class ThoughtWorker_Ascetic_CurrentStateInternal_Patch
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x0000548E File Offset: 0x0000368E
		public static void Prefix(Pawn p)
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = p;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00005496 File Offset: 0x00003696
		public static void Postfix()
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}
	}
}
