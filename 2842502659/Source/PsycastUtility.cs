using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LudeonTK;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000C0 RID: 192
	[StaticConstructorOnStartup]
	public static class PsycastUtility
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
		public static void RecheckPaths(this Pawn pawn)
		{
			Hediff_PsycastAbilities hediff_PsycastAbilities = pawn.Psycasts();
			if (hediff_PsycastAbilities != null)
			{
				if (hediff_PsycastAbilities.unlockedPaths != null)
				{
					foreach (PsycasterPathDef psycasterPathDef in hediff_PsycastAbilities.unlockedPaths.ToList<PsycasterPathDef>())
					{
						if (psycasterPathDef.ensureLockRequirement && !psycasterPathDef.CanPawnUnlock(pawn))
						{
							hediff_PsycastAbilities.previousUnlockedPaths.Add(psycasterPathDef);
							hediff_PsycastAbilities.unlockedPaths.Remove(psycasterPathDef);
						}
					}
				}
				if (hediff_PsycastAbilities.previousUnlockedPaths != null)
				{
					foreach (PsycasterPathDef psycasterPathDef2 in hediff_PsycastAbilities.previousUnlockedPaths.ToList<PsycasterPathDef>())
					{
						if (psycasterPathDef2.ensureLockRequirement)
						{
							if (psycasterPathDef2.CanPawnUnlock(pawn))
							{
								hediff_PsycastAbilities.previousUnlockedPaths.Remove(psycasterPathDef2);
								hediff_PsycastAbilities.unlockedPaths.Add(psycasterPathDef2);
							}
						}
						else
						{
							hediff_PsycastAbilities.previousUnlockedPaths.Remove(psycasterPathDef2);
							hediff_PsycastAbilities.unlockedPaths.Add(psycasterPathDef2);
						}
					}
				}
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000E8F0 File Offset: 0x0000CAF0
		public static bool IsEltexOrHasEltexMaterial(this ThingDef def)
		{
			if (def != null)
			{
				if (def != VPE_DefOf.VPE_Eltex)
				{
					if (def.costList != null)
					{
						if (GenCollection.Any<ThingDefCountClass>(def.costList, (ThingDefCountClass x) => x.thingDef == VPE_DefOf.VPE_Eltex))
						{
							return true;
						}
					}
					return PsycastUtility.eltexThings.Contains(def);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000E94B File Offset: 0x0000CB4B
		public static Hediff_PsycastAbilities Psycasts(this Pawn pawn)
		{
			object obj;
			if (pawn == null)
			{
				obj = null;
			}
			else
			{
				Pawn_HealthTracker health = pawn.health;
				if (health == null)
				{
					obj = null;
				}
				else
				{
					HediffSet hediffSet = health.hediffSet;
					obj = ((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant, false) : null);
				}
			}
			return (Hediff_PsycastAbilities)obj;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000E97C File Offset: 0x0000CB7C
		[DebugAction("Pawns", "Reset Psycasts", true, false, false, false, false, 0, false, actionType = 2, allowedGameStates = 10)]
		public static void ResetPsycasts(Pawn p)
		{
			Hediff_PsycastAbilities hediff_PsycastAbilities = p.Psycasts();
			if (hediff_PsycastAbilities == null)
			{
				return;
			}
			hediff_PsycastAbilities.Reset();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000E98E File Offset: 0x0000CB8E
		public static bool CanReceiveHypothermia(this Pawn pawn, out HediffDef hypothermiaHediff)
		{
			if (pawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid)
			{
				hypothermiaHediff = VPE_DefOf.HypothermicSlowdown;
				return true;
			}
			if (pawn.RaceProps.IsFlesh)
			{
				hypothermiaHediff = HediffDefOf.Hypothermia;
				return true;
			}
			hypothermiaHediff = null;
			return false;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000E9C5 File Offset: 0x0000CBC5
		public static T CreateDelegate<T>(this MethodInfo method) where T : Delegate
		{
			return (T)((object)method.CreateDelegate(typeof(T)));
		}

		// Token: 0x040000D7 RID: 215
		private static readonly HashSet<ThingDef> eltexThings = (from recipe in DefDatabase<RecipeDef>.AllDefs
		where GenCollection.Any<IngredientCount>(recipe.ingredients, (IngredientCount x) => x.IsFixedIngredient && x.FixedIngredient == VPE_DefOf.VPE_Eltex)
		select recipe.ProducedThingDef).ToHashSet<ThingDef>();
	}
}
