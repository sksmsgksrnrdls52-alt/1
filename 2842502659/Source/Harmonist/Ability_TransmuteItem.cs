using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x0200012B RID: 299
	[HotSwappable]
	public class Ability_TransmuteItem : Ability
	{
		// Token: 0x0600044D RID: 1101 RVA: 0x0001A10C File Offset: 0x0001830C
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Ability_TransmuteItem.<>c__DisplayClass0_0 CS$<>8__locals1 = new Ability_TransmuteItem.<>c__DisplayClass0_0();
				Thing thing = globalTargetInfo.Thing;
				Map map = thing.Map;
				CS$<>8__locals1.value = thing.MarketValue * (float)thing.stackCount;
				IntVec3 position = thing.Position;
				List<ThingDef> list = (from thingDef in DefDatabase<ThingDef>.AllDefs
				where this.IsValid(thingDef)
				let marketValue = thingDef.BaseMarketValue
				let count = Mathf.FloorToInt(CS$<>8__locals1.value / thingDef.BaseMarketValue)
				where marketValue <= CS$<>8__locals1.value
				where count <= thingDef.stackLimit
				where count >= 1
				select thingDef).ToList<ThingDef>();
				CS$<>8__locals1.maxWeight = list.Max(new Func<ThingDef, float>(CS$<>8__locals1.<Cast>g__WeightSelector|7));
				ThingDef thingDef2 = GenCollection.RandomElementByWeight<ThingDef>(list, (ThingDef thingDef) => CS$<>8__locals1.maxWeight - base.<Cast>g__WeightSelector|7(thingDef));
				thing.Destroy(0);
				thing = ThingMaker.MakeThing(thingDef2, null);
				thing.stackCount = Mathf.FloorToInt(CS$<>8__locals1.value / thingDef2.BaseMarketValue);
				GenSpawn.Spawn(thing, position, map, 0);
			}
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001A2AC File Offset: 0x000184AC
		private bool IsValid(ThingDef thingDef)
		{
			if (thingDef.category != 2)
			{
				return false;
			}
			if (thingDef.IsCorpse)
			{
				return false;
			}
			if (thingDef.MadeFromStuff)
			{
				return false;
			}
			if (thingDef.IsEgg)
			{
				return false;
			}
			if (thingDef == ThingDefOf.Apparel_CerebrexNode)
			{
				return false;
			}
			if (thingDef == VPE_DefOf.MechanoidTransponder)
			{
				return false;
			}
			if (thingDef.tradeTags != null)
			{
				if (GenCollection.Any<string>(thingDef.tradeTags, (string tag) => tag.Contains("CE") && tag.Contains("Ammo")))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001A330 File Offset: 0x00018530
		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return this.targetParams.CanTarget(target.Thing, this) && GenSight.LineOfSight(this.pawn.Position, target.Cell, this.pawn.Map, true, null, 0, 0);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001A380 File Offset: 0x00018580
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			if (target.Thing.MarketValue < 1f)
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.TooCheap"), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return target.Thing.def != ThingDefOf.Apparel_CerebrexNode;
		}
	}
}
