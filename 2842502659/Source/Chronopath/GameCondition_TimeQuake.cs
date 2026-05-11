using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using VanillaPsycastsExpanded.Harmonist;
using VEF.Buildings;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000138 RID: 312
	[StaticConstructorOnStartup]
	public class GameCondition_TimeQuake : GameCondition_TimeSnow
	{
		// Token: 0x0600047C RID: 1148 RVA: 0x0001B6E4 File Offset: 0x000198E4
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			if (base.TicksPassed % 60 == 0)
			{
				foreach (Map map in base.AffectedMaps)
				{
					for (int i = 0; i < 2000; i++)
					{
						map.wildPlantSpawner.WildPlantSpawnerTick();
					}
					foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned.Where(new Func<Pawn, bool>(this.CanEffect)).ToList<Pawn>())
					{
						AbilityExtension_Age.Age(pawn, 1f);
					}
					foreach (Plant plant in map.listerThings.ThingsInGroup(27).OfType<Plant>().Where(new Func<Plant, bool>(this.CanEffect)).ToList<Plant>())
					{
						if (plant.Growth < 1f)
						{
							plant.Growth = 1f;
						}
						else if (plant.def.useHitPoints)
						{
							plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 0.01f * (float)plant.MaxHitPoints, 0f, -1f, null, null, null, 0, null, true, true, 2, true, false));
						}
						else
						{
							plant.Age = int.MaxValue;
						}
					}
					foreach (Building building in map.listerBuildings.allBuildingsColonist.Concat(map.listerBuildings.allBuildingsNonColonist).Where(new Func<Building, bool>(this.CanEffect)).ToList<Building>())
					{
						if (building.def.useHitPoints)
						{
							building.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 0.01f * (float)building.MaxHitPoints, 0f, -1f, null, null, null, 0, null, true, true, 2, true, false));
						}
					}
				}
			}
			if (base.TicksPassed % 300 == 250)
			{
				foreach (Map map2 in base.AffectedMaps)
				{
					Ability_RandomEvent.DoRandomEvent(map2);
				}
			}
			if (this.sustainer == null)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(VPE_DefOf.Psycast_Neuroquake_CastLoop, this.Pawn);
			}
			else
			{
				this.sustainer.Maintain();
			}
			Find.CameraDriver.shaker.DoShake(1.5f);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001BA0C File Offset: 0x00019C0C
		public override void End()
		{
			this.sustainer.End();
			SoundStarter.PlayOneShot(VPE_DefOf.Psycast_Neuroquake_CastEnd, this.Pawn);
			base.End();
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001BA34 File Offset: 0x00019C34
		private bool CanEffect(Thing thing)
		{
			return !thing.Position.InHorDistOf(this.Pawn.Position, this.SafeRadius);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001BA64 File Offset: 0x00019C64
		public override void GameConditionDraw(Map map)
		{
			base.GameConditionDraw(map);
			if (Find.Selector.IsSelected(this.Pawn))
			{
				GenDraw.DrawRadiusRing(this.Pawn.Position, this.SafeRadius, Color.yellow, null);
			}
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.Pawn.Position.ToVector3ShiftedWithAltitude(28), Quaternion.AngleAxis(0f, Vector3.up), Vector3.one * this.SafeRadius * 2f);
			Graphics.DrawMesh(MeshPool.plane10, matrix4x, GameCondition_TimeQuake.DistortionMat, 0);
		}

		// Token: 0x040001E2 RID: 482
		private static readonly Material DistortionMat = DistortedMaterialsPool.DistortedMaterial("Things/Mote/Black", "Things/Mote/PsycastDistortionMask", 1E-05f, 1f);

		// Token: 0x040001E3 RID: 483
		public float SafeRadius;

		// Token: 0x040001E4 RID: 484
		public Pawn Pawn;

		// Token: 0x040001E5 RID: 485
		private Sustainer sustainer;
	}
}
