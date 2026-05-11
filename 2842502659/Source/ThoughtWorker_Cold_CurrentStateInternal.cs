using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000088 RID: 136
	[HarmonyPatch(typeof(ThoughtWorker_Cold), "CurrentStateInternal")]
	public static class ThoughtWorker_Cold_CurrentStateInternal
	{
		// Token: 0x06000191 RID: 401 RVA: 0x00008AC0 File Offset: 0x00006CC0
		public static void Postfix(Pawn p, ref ThoughtState __result)
		{
			if (p.health.hediffSet.HasHediff(VPE_DefOf.VPE_IceShield, false))
			{
				__result = false;
			}
		}
	}
}
