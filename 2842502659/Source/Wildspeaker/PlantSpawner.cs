using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000E5 RID: 229
	[HarmonyPatch]
	public abstract class PlantSpawner : GroundSpawner
	{
		// Token: 0x06000319 RID: 793 RVA: 0x0001388C File Offset: 0x00011A8C
		protected override void Spawn(Map map, IntVec3 loc)
		{
			if (this.plantDef == null)
			{
				return;
			}
			Plant plant = (Plant)GenSpawn.Spawn(this.plantDef, loc, map, 0);
			this.SetupPlant(plant, loc, map);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000138C0 File Offset: 0x00011AC0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.secondarySpawnTick = Find.TickManager.TicksGame + this.DurationTicks() + Rand.RangeInclusive(-60, 120);
				this.filthSpawnMTB = float.PositiveInfinity;
				Rand.PushState(Find.TickManager.TicksGame);
				this.plantDef = this.ChoosePlant(base.Position, map);
				Rand.PopState();
			}
			if (!this.CheckSpawnLoc(base.Position, map) || this.plantDef == null)
			{
				this.Destroy(0);
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001394C File Offset: 0x00011B4C
		protected virtual bool CheckSpawnLoc(IntVec3 loc, Map map)
		{
			if (GridsUtility.GetTerrain(loc, map).fertility == 0f)
			{
				return false;
			}
			List<Thing> thingList = GridsUtility.GetThingList(loc, map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				Thing thing = thingList[i];
				if (thing is Plant)
				{
					if (thing.def.plant.IsTree)
					{
						return false;
					}
					thing.Destroy(0);
				}
				if (EdificeUtility.IsEdifice(thing.def))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600031C RID: 796
		protected abstract ThingDef ChoosePlant(IntVec3 loc, Map map);

		// Token: 0x0600031D RID: 797 RVA: 0x000139C4 File Offset: 0x00011BC4
		protected virtual void SetupPlant(Plant plant, IntVec3 loc, Map map)
		{
		}

		// Token: 0x0600031E RID: 798 RVA: 0x000139C6 File Offset: 0x00011BC6
		protected virtual int DurationTicks()
		{
			return GenTicks.SecondsToTicks(3f);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000139D2 File Offset: 0x00011BD2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDef, "plantDef");
		}

		// Token: 0x04000194 RID: 404
		private ThingDef plantDef;
	}
}
