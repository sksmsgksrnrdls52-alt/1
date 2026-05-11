using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000083 RID: 131
	[HarmonyPatch(typeof(Pawn_HealthTracker), "AddHediff", new Type[]
	{
		typeof(Hediff),
		typeof(BodyPartRecord),
		typeof(DamageInfo?),
		typeof(DamageWorker.DamageResult)
	})]
	public static class Pawn_HealthTracker_AddHediff_Patch
	{
		// Token: 0x06000186 RID: 390 RVA: 0x00008811 File Offset: 0x00006A11
		public static bool Prefix(Pawn_HealthTracker __instance, Pawn ___pawn, Hediff hediff, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
		{
			return hediff.def != HediffDefOf.Hypothermia || !___pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_IceShield, false);
		}
	}
}
