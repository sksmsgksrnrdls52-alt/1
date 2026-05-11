using System;
using RimWorld;
using UnityEngine;
using VEF.Buildings;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x0200013B RID: 315
	[StaticConstructorOnStartup]
	public class TimeSphere : Thing
	{
		// Token: 0x06000486 RID: 1158 RVA: 0x0001BCC0 File Offset: 0x00019EC0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(base.Position, base.Map, this.Radius, true))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					Faction faction = thing.Faction;
					if (faction != null && !faction.IsPlayer && !FactionUtility.HostileTo(pawn.Faction, Faction.OfPlayer))
					{
						pawn.Faction.TryAffectGoodwillWith(Faction.OfPlayer, -75, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
					}
				}
			}
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001BD88 File Offset: 0x00019F88
		protected override void Tick()
		{
			if (Gen.IsHashIntervalTick(this, 60))
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(base.Position, base.Map, this.Radius, true))
				{
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						AbilityExtension_Age.Age(pawn, 1f);
					}
					Plant plant = thing as Plant;
					if (plant != null)
					{
						if (plant.Growth < 1f)
						{
							plant.Growth = 1f;
						}
						else if (plant.def.useHitPoints)
						{
							thing.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 0.01f * (float)thing.MaxHitPoints, 0f, -1f, null, null, null, 0, null, true, true, 2, true, false));
						}
						else
						{
							plant.Age = int.MaxValue;
						}
					}
					if (thing is Building && thing.def.useHitPoints)
					{
						thing.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 0.01f * (float)thing.MaxHitPoints, 0f, -1f, null, null, null, 0, null, true, true, 2, true, false));
					}
				}
			}
			if (this.sustainer == null)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(VPE_DefOf.VPE_TimeSphere_Sustainer, this);
			}
			else
			{
				this.sustainer.Maintain();
			}
			if (Find.TickManager.TicksGame >= this.startTick + this.Duration)
			{
				this.Destroy(0);
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001BF0C File Offset: 0x0001A10C
		public override void Destroy(DestroyMode mode = 0)
		{
			this.sustainer.End();
			base.Destroy(mode);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001BF20 File Offset: 0x0001A120
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			drawLoc.y = Altitudes.AltitudeFor(27);
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(drawLoc, Quaternion.AngleAxis(0f, Vector3.up), Vector3.one * this.Radius * 1.75f), TimeSphere.DistortionMat, 0);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001BF7C File Offset: 0x0001A17C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.Radius, "radius", 0f, false);
			Scribe_Values.Look<int>(ref this.Duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x040001E6 RID: 486
		private static readonly Material DistortionMat = DistortedMaterialsPool.DistortedMaterial("Things/Mote/Black", "Things/Mote/PsycastDistortionMask", 0.1f, 1.5f);

		// Token: 0x040001E7 RID: 487
		public int Duration;

		// Token: 0x040001E8 RID: 488
		public float Radius;

		// Token: 0x040001E9 RID: 489
		private int startTick;

		// Token: 0x040001EA RID: 490
		private Sustainer sustainer;
	}
}
