using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000134 RID: 308
	public class Ability_MaturePlants : Ability
	{
		// Token: 0x0600046E RID: 1134 RVA: 0x0001B2D4 File Offset: 0x000194D4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (Plant plant in targets.SelectMany((GlobalTargetInfo target) => GenRadial.RadialDistinctThingsAround(target.Cell, target.Map, this.GetRadiusForPawn(), true)).OfType<Plant>().Distinct<Plant>())
			{
				plant.Growth += plant.GrowthRate * (3.5f / plant.def.plant.growDays);
				plant.DirtyMapMesh(plant.Map);
			}
		}
	}
}
