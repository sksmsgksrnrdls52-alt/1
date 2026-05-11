using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000047 RID: 71
	[HarmonyPatch(typeof(Toils_LayDown), "ApplyBedThoughts")]
	public static class Toils_LayDown_ApplyBedThoughts_Patch
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x0000556B File Offset: 0x0000376B
		public static void Prefix(Pawn actor)
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = actor;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00005573 File Offset: 0x00003773
		public static void Postfix()
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}
	}
}
