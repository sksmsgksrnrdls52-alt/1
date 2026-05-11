using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000022 RID: 34
	[HarmonyPatch(typeof(PawnGenerator), "GenerateNewPawnInternal")]
	[HarmonyAfter(new string[]
	{
		"OskarPotocki.VEF"
	})]
	public class PawnGen_Patch
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00003374 File Offset: 0x00001574
		[HarmonyPostfix]
		public static void Postfix(Pawn __result, PawnGenerationRequest request)
		{
			if (__result == null || DevelopmentalStageExtensions.Newborn(request.AllowedDevelopmentalStages))
			{
				return;
			}
			PawnKindAbilityExtension_Psycasts modExtension = __result.kindDef.GetModExtension<PawnKindAbilityExtension_Psycasts>();
			CompAbilities comp = null;
			if (modExtension != null)
			{
				comp = __result.GetComp<CompAbilities>();
				if (modExtension.implantDef != null)
				{
					Hediff_Psylink hediff_Psylink = __result.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false) as Hediff_Psylink;
					if (hediff_Psylink == null)
					{
						hediff_Psylink = (HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, __result, __result.health.hediffSet.GetBrain()) as Hediff_Psylink);
						__result.health.AddHediff(hediff_Psylink, null, null, null);
					}
					Hediff_PsycastAbilities hediff_PsycastAbilities = __result.health.hediffSet.GetFirstHediffOfDef(modExtension.implantDef, false) as Hediff_PsycastAbilities;
					if (hediff_PsycastAbilities.psylink == null)
					{
						hediff_PsycastAbilities.InitializeFromPsylink(hediff_Psylink);
					}
					Func<AbilityDef, bool> <>9__0;
					foreach (PathUnlockData pathUnlockData in modExtension.unlockedPaths)
					{
						if (pathUnlockData.path.CanPawnUnlock(__result))
						{
							hediff_PsycastAbilities.UnlockPath(pathUnlockData.path);
							int num = pathUnlockData.unlockedAbilityCount.RandomInRange;
							IEnumerable<AbilityDef> enumerable = new List<AbilityDef>();
							int num2 = pathUnlockData.unlockedAbilityLevelRange.min;
							while (num2 < pathUnlockData.unlockedAbilityLevelRange.max && num2 < pathUnlockData.path.MaxLevel)
							{
								enumerable = enumerable.Concat(GenCollection.Except<AbilityDef>(pathUnlockData.path.abilityLevelsInOrder[num2 - 1], PsycasterPathDef.Blank));
								num2++;
							}
							List<AbilityDef> list = enumerable.ToList<AbilityDef>();
							for (;;)
							{
								IEnumerable<AbilityDef> source = list;
								Func<AbilityDef, bool> predicate;
								if ((predicate = <>9__0) == null)
								{
									predicate = (<>9__0 = ((AbilityDef ab) => ab.Psycast().PrereqsCompleted(comp)));
								}
								List<AbilityDef> list2;
								if (!GenCollection.Any<AbilityDef>(list2 = source.Where(predicate).ToList<AbilityDef>()) || num <= 0)
								{
									break;
								}
								num--;
								AbilityDef abilityDef = GenCollection.RandomElement<AbilityDef>(list2);
								comp.GiveAbility(abilityDef);
								hediff_PsycastAbilities.ChangeLevel(1, false);
								hediff_PsycastAbilities.points--;
								list.Remove(abilityDef);
							}
						}
					}
					int randomInRange = modExtension.statUpgradePoints.RandomInRange;
					hediff_PsycastAbilities.ChangeLevel(randomInRange);
					hediff_PsycastAbilities.points -= randomInRange;
					hediff_PsycastAbilities.ImproveStats(randomInRange);
				}
			}
			Storyteller storyteller = Find.Storyteller;
			if (((storyteller != null) ? storyteller.def : null) == VPE_DefOf.VPE_Basilicus && __result.RaceProps.intelligence >= 2 && Rand.Value < PsycastsMod.Settings.baseSpawnChance)
			{
				Hediff_Psylink hediff_Psylink2 = __result.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false) as Hediff_Psylink;
				if (hediff_Psylink2 == null)
				{
					hediff_Psylink2 = (HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, __result, __result.health.hediffSet.GetBrain()) as Hediff_Psylink);
					__result.health.AddHediff(hediff_Psylink2, null, null, null);
				}
				Hediff_PsycastAbilities hediff_PsycastAbilities2 = (__result.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant, false) as Hediff_PsycastAbilities) ?? (HediffMaker.MakeHediff(VPE_DefOf.VPE_PsycastAbilityImplant, __result, GenCollection.FirstOrFallback<BodyPartRecord>(__result.RaceProps.body.GetPartsWithDef(VPE_DefOf.Brain), null)) as Hediff_PsycastAbilities);
				if (hediff_PsycastAbilities2.psylink == null)
				{
					hediff_PsycastAbilities2.InitializeFromPsylink(hediff_Psylink2);
				}
				PsycasterPathDef psycasterPathDef = GenCollection.RandomElement<PsycasterPathDef>(from ppd in DefDatabase<PsycasterPathDef>.AllDefsListForReading
				where ppd.CanPawnUnlock(__result)
				select ppd);
				hediff_PsycastAbilities2.UnlockPath(psycasterPathDef);
				if (comp == null)
				{
					comp = __result.GetComp<CompAbilities>();
				}
				IEnumerable<AbilityDef> enumerable2 = psycasterPathDef.abilities.Except(from ab in comp.LearnedAbilities
				select ab.def);
				Func<AbilityDef, bool> <>9__3;
				do
				{
					IEnumerable<AbilityDef> source2 = enumerable2;
					Func<AbilityDef, bool> predicate2;
					if ((predicate2 = <>9__3) == null)
					{
						predicate2 = (<>9__3 = ((AbilityDef ab) => ab.GetModExtension<AbilityExtension_Psycast>().PrereqsCompleted(comp)));
					}
					AbilityDef abilityDef2;
					if (!GenCollection.TryRandomElement<AbilityDef>(source2.Where(predicate2), ref abilityDef2))
					{
						break;
					}
					comp.GiveAbility(abilityDef2);
					if (hediff_PsycastAbilities2.points <= 0)
					{
						hediff_PsycastAbilities2.ChangeLevel(1, false);
					}
					hediff_PsycastAbilities2.points--;
					enumerable2 = GenCollection.Except<AbilityDef>(enumerable2, abilityDef2);
				}
				while (Rand.Value < PsycastsMod.Settings.additionalAbilityChance);
			}
		}
	}
}
