using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000133 RID: 307
	public class Ability_Finish : Ability
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x0001AFC4 File Offset: 0x000191C4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				UnfinishedThing unfinishedThing = globalTargetInfo.Thing as UnfinishedThing;
				if (unfinishedThing != null)
				{
					List<Thing> ingredients = unfinishedThing.ingredients;
					RecipeDef recipe = unfinishedThing.Recipe;
					ThingDef stuff = unfinishedThing.Stuff;
					IntVec3 position = unfinishedThing.Position;
					Pawn pawn = unfinishedThing.Creator ?? this.pawn;
					Thing thing;
					if (unfinishedThing.def.MadeFromStuff)
					{
						thing = ingredients.First((Thing ing) => ing.def == stuff);
					}
					else if (GenList.NullOrEmpty<Thing>(ingredients))
					{
						thing = null;
					}
					else if (recipe.productHasIngredientStuff)
					{
						thing = ingredients[0];
					}
					else if (GenCollection.Any<ThingDefCountClass>(recipe.products, (ThingDefCountClass x) => x.thingDef.MadeFromStuff))
					{
						thing = GenCollection.RandomElementByWeight<Thing>(from x in ingredients
						where x.def.IsStuff
						select x, (Thing x) => (float)x.stackCount);
					}
					else
					{
						thing = GenCollection.RandomElementByWeight<Thing>(ingredients, (Thing x) => (float)x.stackCount);
					}
					List<Thing> list = GenRecipe.MakeRecipeProducts(recipe, pawn, ingredients, thing, unfinishedThing.BoundWorkTable as IBillGiver, null, null, null).ToList<Thing>();
					ingredients.ForEach(delegate(Thing t)
					{
						recipe.Worker.ConsumeIngredient(t, recipe, this.pawn.Map);
					});
					Bill_ProductionWithUft boundBill = unfinishedThing.BoundBill;
					if (boundBill != null)
					{
						boundBill.Notify_IterationCompleted(pawn, ingredients);
					}
					recipe.Worker.ConsumeIngredient(unfinishedThing, recipe, this.pawn.Map);
					RecordsUtility.Notify_BillDone(pawn, list);
					if (list.Count == 0)
					{
						return;
					}
					if (recipe.WorkAmountForStuff(stuff) >= 10000f)
					{
						TaleRecorder.RecordTale(TaleDefOf.CompletedLongCraftingProject, new object[]
						{
							pawn,
							MinifyUtility.GetInnerIfMinified(list[0]).def
						});
					}
					Find.QuestManager.Notify_ThingsProduced(pawn, list);
					foreach (Thing thing2 in list)
					{
						if (!GenPlace.TryPlaceThing(thing2, position, this.pawn.Map, 1, null, null, null, 1))
						{
							Log.Error(string.Format("Could not drop recipe product {0} near {1}", thing2, position));
						}
					}
				}
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001B2B0 File Offset: 0x000194B0
		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return base.CanHitTarget(target) && target.Thing is UnfinishedThing;
		}
	}
}
