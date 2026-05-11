using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x0200012C RID: 300
	public class Ability_TransmuteStone : Ability
	{
		// Token: 0x06000453 RID: 1107 RVA: 0x0001A3F4 File Offset: 0x000185F4
		public unsafe override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Ability_TransmuteStone.<>c__DisplayClass2_0 CS$<>8__locals1;
				CS$<>8__locals1.map = globalTargetInfo.Map;
				Find.World.NaturalRockTypesIn(CS$<>8__locals1.map.Tile);
				CS$<>8__locals1.naturalRockDefs = *Ability_TransmuteStone.allNaturalRockDefs.Invoke(Find.World);
				CS$<>8__locals1.chosenRock = GenCollection.RandomElement<ThingDef>(CS$<>8__locals1.naturalRockDefs);
				foreach (IntVec3 intVec in GenRadial.RadialCellsAround(globalTargetInfo.Cell, this.GetRadiusForPawn(), true))
				{
					foreach (Thing thing in GenList.ListFullCopy<Thing>(GridsUtility.GetThingList(intVec, CS$<>8__locals1.map)))
					{
						if (thing.def.IsNonResourceNaturalRock)
						{
							Ability_TransmuteStone.<Cast>g__Replace|2_1(thing, CS$<>8__locals1.chosenRock, null, ref CS$<>8__locals1);
						}
						else
						{
							foreach (ThingDef thingDef in CS$<>8__locals1.naturalRockDefs)
							{
								if (thingDef.building.mineableThing != null && thingDef.building.mineableThing == thing.def)
								{
									Ability_TransmuteStone.<Cast>g__Replace|2_1(thing, CS$<>8__locals1.chosenRock.building.mineableThing, null, ref CS$<>8__locals1);
								}
								else if (thingDef.building.smoothedThing != null && thingDef.building.smoothedThing == thing.def)
								{
									Ability_TransmuteStone.<Cast>g__Replace|2_1(thing, CS$<>8__locals1.chosenRock.building.smoothedThing, null, ref CS$<>8__locals1);
								}
								else
								{
									ThingDef mineableThing = thingDef.building.mineableThing;
									ThingDef thingDef2;
									if (mineableThing == null)
									{
										thingDef2 = null;
									}
									else
									{
										ThingDefCountClass thingDefCountClass = mineableThing.butcherProducts[0];
										thingDef2 = ((thingDefCountClass != null) ? thingDefCountClass.thingDef : null);
									}
									if (thingDef2 == thing.def)
									{
										Ability_TransmuteStone.<Cast>g__Replace|2_1(thing, CS$<>8__locals1.chosenRock.building.mineableThing.butcherProducts[0].thingDef, null, ref CS$<>8__locals1);
									}
									else if (thing.Stuff != null)
									{
										ThingDef stuff = thing.Stuff;
										ThingDef mineableThing2 = thingDef.building.mineableThing;
										object obj;
										if (mineableThing2 == null)
										{
											obj = null;
										}
										else
										{
											ThingDefCountClass thingDefCountClass2 = mineableThing2.butcherProducts[0];
											obj = ((thingDefCountClass2 != null) ? thingDefCountClass2.thingDef : null);
										}
										if (stuff == obj)
										{
											Ability_TransmuteStone.<Cast>g__Replace|2_1(thing, null, CS$<>8__locals1.chosenRock.building.mineableThing.butcherProducts[0].thingDef, ref CS$<>8__locals1);
										}
									}
								}
							}
						}
					}
					TerrainGrid terrainGrid = CS$<>8__locals1.map.terrainGrid;
					TerrainDef terrainDef = terrainGrid.TerrainAt(intVec);
					terrainGrid.SetTerrain(intVec, Ability_TransmuteStone.<Cast>g__NewTerrain|2_0(terrainDef, ref CS$<>8__locals1));
					terrainDef = terrainGrid.UnderTerrainAt(intVec);
					if (terrainDef != null)
					{
						terrainGrid.SetUnderTerrain(intVec, Ability_TransmuteStone.<Cast>g__NewTerrain|2_0(terrainDef, ref CS$<>8__locals1));
					}
				}
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001A75C File Offset: 0x0001895C
		[CompilerGenerated]
		internal static TerrainDef <Cast>g__NewTerrain|2_0(TerrainDef terrain, ref Ability_TransmuteStone.<>c__DisplayClass2_0 A_1)
		{
			string text = terrain.defName;
			foreach (ThingDef thingDef in GenCollection.Except<ThingDef>(A_1.naturalRockDefs, A_1.chosenRock))
			{
				if (text.StartsWith(thingDef.defName))
				{
					text = text.Replace(thingDef.defName, A_1.chosenRock.defName);
				}
			}
			return TerrainDef.Named(text);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0001A7E0 File Offset: 0x000189E0
		[CompilerGenerated]
		internal static void <Cast>g__Replace|2_1(Thing thing, ThingDef def = null, ThingDef stuff = null, ref Ability_TransmuteStone.<>c__DisplayClass2_0 A_3)
		{
			ThingOwner holdingOwner = thing.holdingOwner;
			IntVec3 position = thing.Position;
			Rot4 rotation = thing.Rotation;
			if (def == null)
			{
				def = thing.def;
			}
			if (stuff == null)
			{
				stuff = thing.Stuff;
			}
			Thing thing2 = ThingMaker.MakeThing(def, stuff);
			List<Designation> list = GenList.ListFullCopy<Designation>(A_3.map.designationManager.AllDesignationsOn(thing));
			thing.Destroy(0);
			if (position.IsValid)
			{
				GenSpawn.Spawn(thing2, position, A_3.map, rotation, 0, false, false);
			}
			else
			{
				if (holdingOwner == null)
				{
					Log.Warning(string.Format("[VPE] Attempting to replace unspawned and unheld thing {0}", thing));
					return;
				}
				if (!holdingOwner.TryAdd(thing2, true))
				{
					Log.Error(string.Format("[VPE] Failed to add {0} to {1}", thing2, holdingOwner));
				}
			}
			foreach (Designation designation in list)
			{
				A_3.map.designationManager.AddDesignation(new Designation(thing2, designation.def, null));
			}
		}

		// Token: 0x040001D8 RID: 472
		private static readonly AccessTools.FieldRef<World, List<ThingDef>> allNaturalRockDefs = AccessTools.FieldRefAccess<World, List<ThingDef>>("allNaturalRockDefs");

		// Token: 0x040001D9 RID: 473
		private static readonly AccessTools.FieldRef<Thing, Graphic> graphicInt = AccessTools.FieldRefAccess<Thing, Graphic>("graphicInt");
	}
}
