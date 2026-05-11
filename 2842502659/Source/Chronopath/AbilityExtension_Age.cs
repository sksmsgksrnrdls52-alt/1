using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x0200012F RID: 303
	public class AbilityExtension_Age : AbilityExtension_AbilityMod
	{
		// Token: 0x0600045F RID: 1119 RVA: 0x0001AB4C File Offset: 0x00018D4C
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			if (this.casterYears != null)
			{
				AbilityExtension_Age.Age(ability.pawn, this.casterYears.Value);
			}
			if (this.targetYears == null)
			{
				return;
			}
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					AbilityExtension_Age.Age(pawn, this.targetYears.Value);
				}
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001ABCC File Offset: 0x00018DCC
		public override bool CanApplyOn(LocalTargetInfo target, Ability ability, bool throwMessages = false)
		{
			if (!base.CanApplyOn(target, ability, throwMessages))
			{
				return false;
			}
			if (this.targetYears == null)
			{
				return true;
			}
			Pawn pawn = target.Thing as Pawn;
			return pawn != null && pawn.RaceProps.IsFlesh && pawn.RaceProps.Humanlike;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001AC28 File Offset: 0x00018E28
		public static void Age(Pawn pawn, float years)
		{
			pawn.ageTracker.AgeBiologicalTicks += (long)Mathf.FloorToInt(years * 3600000f);
			if (years < 0f)
			{
				List<HediffGiverSetDef> hediffGiverSets = pawn.def.race.hediffGiverSets;
				if (hediffGiverSets == null)
				{
					return;
				}
				float num = (float)pawn.ageTracker.AgeBiologicalYears / pawn.def.race.lifeExpectancy;
				foreach (HediffGiverSetDef hediffGiverSetDef in hediffGiverSets)
				{
					foreach (HediffGiver hediffGiver in hediffGiverSetDef.hediffGivers)
					{
						HediffGiver_Birthday hediffGiver_Birthday = hediffGiver as HediffGiver_Birthday;
						if (hediffGiver_Birthday != null && hediffGiver_Birthday.ageFractionChanceCurve.Evaluate(num) <= 0f)
						{
							Hediff firstHediffOfDef;
							while ((firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediffGiver_Birthday.hediff, false)) != null)
							{
								pawn.health.RemoveHediff(firstHediffOfDef);
							}
						}
					}
				}
			}
			if ((float)pawn.ageTracker.AgeBiologicalYears > pawn.def.race.lifeExpectancy * 1.1f && (pawn.genes == null || pawn.genes.HediffGiversCanGive(VPE_DefOf.HeartAttack)))
			{
				BodyPartRecord bodyPartRecord = GenCollection.FirstOrDefault<BodyPartRecord>(pawn.RaceProps.body.AllParts, (BodyPartRecord p) => p.def == BodyPartDefOf.Heart);
				Hediff hediff = HediffMaker.MakeHediff(VPE_DefOf.HeartAttack, pawn, bodyPartRecord);
				hediff.Severity = 1.1f;
				pawn.health.AddHediff(hediff, bodyPartRecord, null, null);
			}
		}

		// Token: 0x040001DC RID: 476
		public float? casterYears;

		// Token: 0x040001DD RID: 477
		public float? targetYears;
	}
}
