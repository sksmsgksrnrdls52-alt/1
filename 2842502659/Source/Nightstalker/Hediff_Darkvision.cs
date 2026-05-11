using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x02000123 RID: 291
	[HarmonyPatch]
	public class Hediff_Darkvision : HediffWithComps
	{
		// Token: 0x06000433 RID: 1075 RVA: 0x000197CE File Offset: 0x000179CE
		[HarmonyPatch(typeof(ThoughtUtility), "NullifyingHediff")]
		[HarmonyPostfix]
		public static void NullDarkness(ThoughtDef def, Pawn pawn, ref Hediff __result)
		{
			if (def == VPE_DefOf.EnvironmentDark && __result == null && Hediff_Darkvision.DarkvisionPawns.Contains(pawn))
			{
				__result = pawn.health.hediffSet.hediffs.OfType<Hediff_Darkvision>().FirstOrDefault<Hediff_Darkvision>();
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00019808 File Offset: 0x00017A08
		[HarmonyPatch(typeof(StatPart_Glow), "ActiveFor")]
		[HarmonyPostfix]
		public static void NoDarkPenalty(Thing t, ref bool __result)
		{
			if (__result)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null && Hediff_Darkvision.DarkvisionPawns.Contains(pawn))
				{
					__result = false;
				}
			}
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00019834 File Offset: 0x00017A34
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			Hediff_Darkvision.DarkvisionPawns.Add(this.pawn);
			foreach (BodyPartRecord bodyPartRecord in from part in this.pawn.RaceProps.body.AllParts
			where part.def == BodyPartDefOf.Eye
			select part)
			{
				this.pawn.health.AddHediff(VPE_DefOf.VPE_Darkvision_Display, bodyPartRecord, null, null);
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x000198E8 File Offset: 0x00017AE8
		public override void PostRemoved()
		{
			base.PostRemoved();
			Hediff_Darkvision.DarkvisionPawns.Remove(this.pawn);
			Hediff firstHediffOfDef;
			while ((firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_Darkvision_Display, false)) != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
		}

		// Token: 0x040001D5 RID: 469
		public static HashSet<Pawn> DarkvisionPawns = new HashSet<Pawn>();
	}
}
