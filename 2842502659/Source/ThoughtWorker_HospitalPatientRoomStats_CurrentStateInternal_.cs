using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000044 RID: 68
	[HarmonyPatch(typeof(ThoughtWorker_HospitalPatientRoomStats), "CurrentStateInternal")]
	public static class ThoughtWorker_HospitalPatientRoomStats_CurrentStateInternal_Patch
	{
		// Token: 0x060000C9 RID: 201 RVA: 0x000054AE File Offset: 0x000036AE
		public static void Prefix(Pawn p)
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = p;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000054B6 File Offset: 0x000036B6
		public static void Postfix()
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}
	}
}
