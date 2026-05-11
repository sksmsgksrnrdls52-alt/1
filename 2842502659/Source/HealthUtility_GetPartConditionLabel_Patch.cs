using System;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200001F RID: 31
	[HarmonyPatch(typeof(HealthUtility), "GetPartConditionLabel")]
	public static class HealthUtility_GetPartConditionLabel_Patch
	{
		// Token: 0x06000056 RID: 86 RVA: 0x00003128 File Offset: 0x00001328
		private static void Postfix(ref Pair<string, Color> __result, Pawn pawn, BodyPartRecord part)
		{
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (hediff.def == VPE_DefOf.VPE_Sacrificed && hediff.Part == part)
				{
					__result = new Pair<string, Color>(__result.First, Color.grey);
				}
			}
		}
	}
}
