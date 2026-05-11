using System;
using System.Collections.Generic;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000056 RID: 86
	public class MapComponent_PsycastsManager : MapComponent
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00005E59 File Offset: 0x00004059
		public MapComponent_PsycastsManager(Map map) : base(map)
		{
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005E84 File Offset: 0x00004084
		public override void MapComponentTick()
		{
			base.MapComponentTick();
			for (int i = this.temperatureZones.Count - 1; i >= 0; i--)
			{
				FixedTemperatureZone fixedTemperatureZone = this.temperatureZones[i];
				if (Find.TickManager.TicksGame >= fixedTemperatureZone.expiresIn)
				{
					this.temperatureZones.RemoveAt(i);
				}
				else
				{
					fixedTemperatureZone.DoEffects(this.map);
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005EE8 File Offset: 0x000040E8
		public bool TryGetOverridenTemperatureFor(IntVec3 cell, out float result)
		{
			for (int i = 0; i < this.temperatureZones.Count; i++)
			{
				FixedTemperatureZone fixedTemperatureZone = this.temperatureZones[i];
				if ((float)IntVec3Utility.DistanceToSquared(cell, fixedTemperatureZone.center) <= fixedTemperatureZone.radius * fixedTemperatureZone.radius)
				{
					result = fixedTemperatureZone.fixedTemperature;
					return true;
				}
			}
			for (int j = 0; j < this.blizzardSources.Count; j++)
			{
				Hediff_BlizzardSource hediff_BlizzardSource = this.blizzardSources[j];
				float radiusForPawn = hediff_BlizzardSource.ability.GetRadiusForPawn();
				if (IntVec3Utility.DistanceTo(cell, hediff_BlizzardSource.pawn.Position) <= radiusForPawn * radiusForPawn)
				{
					result = -60f;
					return true;
				}
			}
			result = -1f;
			return false;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005F98 File Offset: 0x00004198
		public override void MapComponentUpdate()
		{
			base.MapComponentUpdate();
			for (int i = this.hediffsToDraw.Count - 1; i >= 0; i--)
			{
				Hediff_Overlay hediff_Overlay = this.hediffsToDraw[i];
				if (hediff_Overlay.pawn == null || !hediff_Overlay.pawn.health.hediffSet.hediffs.Contains(hediff_Overlay))
				{
					this.hediffsToDraw.RemoveAt(i);
				}
				else
				{
					Pawn pawn = hediff_Overlay.pawn;
					if (((pawn != null) ? pawn.MapHeld : null) != null)
					{
						hediff_Overlay.Draw();
					}
				}
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006020 File Offset: 0x00004220
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<FixedTemperatureZone>(ref this.temperatureZones, "temperatureZones", 2, Array.Empty<object>());
			Scribe_Collections.Look<Hediff_BlizzardSource>(ref this.blizzardSources, "blizzardSources", 3, Array.Empty<object>());
			Scribe_Collections.Look<Hediff_Overlay>(ref this.hediffsToDraw, "hediffsToDraw", 3, Array.Empty<object>());
			if (Scribe.mode == 4)
			{
				if (this.temperatureZones == null)
				{
					this.temperatureZones = new List<FixedTemperatureZone>();
				}
				if (this.blizzardSources == null)
				{
					this.blizzardSources = new List<Hediff_BlizzardSource>();
				}
				if (this.hediffsToDraw == null)
				{
					this.hediffsToDraw = new List<Hediff_Overlay>();
				}
				this.temperatureZones.RemoveAll((FixedTemperatureZone x) => x == null);
				this.blizzardSources.RemoveAll((Hediff_BlizzardSource x) => x == null);
				this.hediffsToDraw.RemoveAll((Hediff_Overlay x) => x == null);
			}
		}

		// Token: 0x04000043 RID: 67
		public List<FixedTemperatureZone> temperatureZones = new List<FixedTemperatureZone>();

		// Token: 0x04000044 RID: 68
		public List<Hediff_BlizzardSource> blizzardSources = new List<Hediff_BlizzardSource>();

		// Token: 0x04000045 RID: 69
		public List<Hediff_Overlay> hediffsToDraw = new List<Hediff_Overlay>();
	}
}
