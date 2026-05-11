using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200003A RID: 58
	public class FixedTemperatureZone : IExposable
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00004B30 File Offset: 0x00002D30
		public void DoEffects(Map map)
		{
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.center, this.radius, true))
			{
				if (Rand.Value < this.spawnRate)
				{
					this.ThrowFleck(intVec, map, 2.3f);
					if (this.fixedTemperature < 0f)
					{
						map.snowGrid.AddDepth(intVec, 0.1f);
					}
				}
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004BBC File Offset: 0x00002DBC
		public void ThrowFleck(IntVec3 c, Map map, float size)
		{
			Vector3 vector = c.ToVector3Shifted();
			if (GenView.ShouldSpawnMotesAt(vector, map, true))
			{
				vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
				if (GenGrid.InBounds(vector, map))
				{
					FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, map, this.fleckToSpawn, Rand.Range(4f, 6f) * size);
					dataStatic.rotationRate = Rand.Range(-3f, 3f);
					dataStatic.velocityAngle = (float)Rand.Range(0, 360);
					dataStatic.velocitySpeed = 0.12f;
					map.flecks.CreateFleck(dataStatic);
				}
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004C74 File Offset: 0x00002E74
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.center, "center", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<int>(ref this.expiresIn, "expiresIn", 0, false);
			Scribe_Values.Look<float>(ref this.fixedTemperature, "fixedTemperature", 0f, false);
			Scribe_Values.Look<float>(ref this.spawnRate, "spawnRate", 0f, false);
			Scribe_Defs.Look<FleckDef>(ref this.fleckToSpawn, "fleckSpawn");
		}

		// Token: 0x04000029 RID: 41
		public IntVec3 center;

		// Token: 0x0400002A RID: 42
		public float radius;

		// Token: 0x0400002B RID: 43
		public int expiresIn;

		// Token: 0x0400002C RID: 44
		public float fixedTemperature;

		// Token: 0x0400002D RID: 45
		public FleckDef fleckToSpawn;

		// Token: 0x0400002E RID: 46
		public float spawnRate;
	}
}
