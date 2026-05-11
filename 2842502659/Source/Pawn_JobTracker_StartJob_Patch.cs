using System;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000052 RID: 82
	[HarmonyPatch(typeof(Pawn_JobTracker), "StartJob")]
	public class Pawn_JobTracker_StartJob_Patch
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x00005A12 File Offset: 0x00003C12
		private static bool Prefix(Pawn_JobTracker __instance, Pawn ___pawn, Job newJob, JobTag? tag)
		{
			return ___pawn.CurJobDef != VPE_DefOf.VPE_StandFreeze;
		}
	}
}
