using System;
using System.Linq;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000E8 RID: 232
	public class TreeSpawner : PlantSpawner
	{
		// Token: 0x06000327 RID: 807 RVA: 0x00013AF1 File Offset: 0x00011CF1
		protected override int DurationTicks()
		{
			return GenTicks.SecondsToTicks(5f);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00013B00 File Offset: 0x00011D00
		protected override ThingDef ChoosePlant(IntVec3 loc, Map map)
		{
			ThingDef thingDef;
			if ((GenCollection.TryRandomElement<ThingDef>(map.Biome.AllWildPlants.Where(delegate(ThingDef td)
			{
				PlantProperties plant = td.plant;
				return plant != null && plant.IsTree;
			}), ref thingDef) || GenCollection.TryRandomElement<ThingDef>(map.Biome.AllWildPlants, ref thingDef)) && PlantUtility.CanEverPlantAt(thingDef, loc, map, true, true) && PlantUtility.AdjacentSowBlocker(thingDef, loc, map) == null)
			{
				return thingDef;
			}
			return null;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00013B72 File Offset: 0x00011D72
		protected override void SetupPlant(Plant plant, IntVec3 loc, Map map)
		{
			if (PlantUtility.AdjacentSowBlocker(plant.def, loc, map) != null)
			{
				plant.Destroy(0);
				return;
			}
			plant.Growth = 1f;
		}
	}
}
