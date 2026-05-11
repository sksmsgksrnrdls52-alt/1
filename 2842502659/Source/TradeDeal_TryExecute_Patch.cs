using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000053 RID: 83
	[HarmonyPatch(typeof(TradeDeal), "TryExecute", new Type[]
	{
		typeof(bool)
	}, new ArgumentType[]
	{
		2
	})]
	public static class TradeDeal_TryExecute_Patch
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00005A2C File Offset: 0x00003C2C
		public static void Prefix(List<Tradeable> ___tradeables, out int __state)
		{
			__state = 0;
			foreach (Tradeable tradeable in ___tradeables)
			{
				__state += tradeable.ThingDef.GetEltexOrEltexMaterialCount() * tradeable.CountToTransferToDestination;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005A90 File Offset: 0x00003C90
		public static int GetEltexOrEltexMaterialCount(this ThingDef def)
		{
			if (def != null)
			{
				if (def == VPE_DefOf.VPE_Eltex)
				{
					return 1;
				}
				if (def.costList == null)
				{
					foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
					{
						if (recipeDef.ProducedThingDef == def)
						{
							IngredientCount ingredientCount = GenCollection.FirstOrDefault<IngredientCount>(recipeDef.ingredients, (IngredientCount x) => x.IsFixedIngredient && x.FixedIngredient == VPE_DefOf.VPE_Eltex);
							if (ingredientCount != null)
							{
								return (int)ingredientCount.GetBaseCount();
							}
						}
					}
					return 0;
				}
				ThingDefCountClass thingDefCountClass = GenCollection.FirstOrDefault<ThingDefCountClass>(def.costList, (ThingDefCountClass x) => x.thingDef == VPE_DefOf.VPE_Eltex);
				if (thingDefCountClass != null)
				{
					return thingDefCountClass.count;
				}
			}
			return 0;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00005B68 File Offset: 0x00003D68
		public static void Postfix(int __state, bool __result)
		{
			if (__state > 0 && __result && TradeSession.trader.Faction != Faction.OfEmpire && Faction.OfEmpire != null && Rand.Chance(0.5f))
			{
				Current.Game.GetComponent<GameComponent_PsycastsManager>().goodwillImpacts.Add(new GoodwillImpactDelayed
				{
					factionToImpact = Faction.OfEmpire,
					goodwillImpact = -__state,
					historyEvent = (TradeSession.giftMode ? VPE_DefOf.VPE_GiftedEltex : VPE_DefOf.VPE_SoldEltex),
					impactInTicks = Find.TickManager.TicksGame + (int)(60000f * Rand.Range(7f, 14f)),
					letterLabel = Translator.Translate("VPE.EmpireAngeredTitle"),
					letterDesc = TranslatorFormattedStringExtensions.Translate("VPE.EmpireAngeredDesc", TradeSession.giftMode ? Translator.Translate("VPE.Gifting") : Translator.Translate("VPE.Trading")),
					relationInfoKey = "VPE.FactionRelationReducedInfo"
				});
			}
		}
	}
}
