using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000E7 RID: 231
	public class WildPlantSpawner : PlantSpawner
	{
		// Token: 0x06000324 RID: 804 RVA: 0x00013A50 File Offset: 0x00011C50
		protected override ThingDef ChoosePlant(IntVec3 loc, Map map)
		{
			if (Rand.Chance(0.2f))
			{
				return null;
			}
			ThingDef thingDef;
			if (GenCollection.TryRandomElement<ThingDef>(DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef td)
			{
				PlantProperties plant = td.plant;
				return plant != null && plant.Sowable && !plant.IsTree;
			}), ref thingDef) && PlantUtility.CanEverPlantAt(thingDef, loc, map, true, true))
			{
				return thingDef;
			}
			return null;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00013AAC File Offset: 0x00011CAC
		protected override void SetupPlant(Plant plant, IntVec3 loc, Map map)
		{
			base.SetupPlant(plant, loc, map);
			plant.Growth = Mathf.Clamp(StatExtension.GetStatValue(ThingCompUtility.TryGetComp<CompAbilitySpawn>(this).pawn, StatDefOf.PsychicSensitivity, true, -1) - 1f, 0.1f, 1f);
		}
	}
}
