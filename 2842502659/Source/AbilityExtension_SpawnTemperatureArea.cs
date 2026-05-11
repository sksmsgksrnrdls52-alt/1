using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000064 RID: 100
	public class AbilityExtension_SpawnTemperatureArea : AbilityExtension_AbilityMod
	{
		// Token: 0x06000121 RID: 289 RVA: 0x00006CA8 File Offset: 0x00004EA8
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				ability.pawn.Map.GetComponent<MapComponent_PsycastsManager>().temperatureZones.Add(new FixedTemperatureZone
				{
					fixedTemperature = this.fixedTemperature,
					radius = ability.GetRadiusForPawn(),
					center = globalTargetInfo.Cell,
					expiresIn = Find.TickManager.TicksGame + ability.GetDurationForPawn(),
					fleckToSpawn = this.fleckToSpawnInArea,
					spawnRate = this.spawnRate
				});
			}
		}

		// Token: 0x0400004D RID: 77
		public float fixedTemperature;

		// Token: 0x0400004E RID: 78
		public FleckDef fleckToSpawnInArea;

		// Token: 0x0400004F RID: 79
		public float spawnRate;
	}
}
