using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000041 RID: 65
	[HarmonyPatch(typeof(JoyUtility), "TryGainRecRoomThought")]
	public static class JoyUtility_TryGainRecRoomThought_Patch
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x0000547E File Offset: 0x0000367E
		public static void Prefix(Pawn pawn)
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = pawn;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005486 File Offset: 0x00003686
		public static void Postfix()
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}
	}
}
