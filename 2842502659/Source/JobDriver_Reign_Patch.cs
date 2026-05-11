using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000040 RID: 64
	[HarmonyPatch]
	public static class JobDriver_Reign_Patch
	{
		// Token: 0x060000C0 RID: 192 RVA: 0x0000542F File Offset: 0x0000362F
		[HarmonyTargetMethod]
		public static MethodBase GetMethod()
		{
			return typeof(JobDriver_Reign).GetMethods(AccessTools.all).Last((MethodInfo x) => x.Name.Contains("<MakeNewToils>"));
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00005469 File Offset: 0x00003669
		public static void Prefix(JobDriver_Reign __instance)
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = __instance.pawn;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00005476 File Offset: 0x00003676
		public static void Postfix()
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}
	}
}
