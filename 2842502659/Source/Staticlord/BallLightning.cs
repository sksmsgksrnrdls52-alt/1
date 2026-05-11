using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x0200010B RID: 267
	public class BallLightning : AbilityProjectile
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x0001690C File Offset: 0x00014B0C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ticksTillAttack = 180;
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00016924 File Offset: 0x00014B24
		protected override void Tick()
		{
			base.Tick();
			if (!base.Spawned)
			{
				return;
			}
			this.ticksTillAttack--;
			if (this.ticksTillAttack <= 0)
			{
				this.currentTargets.Clear();
				foreach (Thing thing in (from t in GenRadial.RadialDistinctThingsAround(IntVec3Utility.ToIntVec3(this.ExactPosition), base.Map, this.ability.GetRadiusForPawn(), true)
				where GenHostility.HostileTo(t, this.launcher)
				select t).Take(Mathf.FloorToInt(this.ability.GetPowerForPawn())))
				{
					this.currentTargets.Add(thing);
					BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(this.launcher, thing, thing, this.def, VPE_DefOf.VPE_Bolt, this.targetCoverDef);
					thing.TakeDamage(new DamageInfo(DamageDefOf.Flame, 12f, 5f, Vector3Utility.AngleToFlat(this.DrawPos, thing.DrawPos), this, null, null, 0, null, true, true, 2, true, false)).AssociateWithLog(battleLogEntry_RangedImpact);
					thing.TakeDamage(new DamageInfo(DamageDefOf.EMP, 20f, 5f, Vector3Utility.AngleToFlat(this.DrawPos, thing.DrawPos), this, null, null, 0, null, true, true, 2, true, false)).AssociateWithLog(battleLogEntry_RangedImpact);
					SoundStarter.PlayOneShot(VPE_DefOf.VPE_BallLightning_Zap, thing);
				}
				this.ticksTillAttack = 60;
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00016AA0 File Offset: 0x00014CA0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			Vector3 vector = Vector3Utility.Yto0(drawLoc) + Vector3Utility.RotatedBy(new Vector3(1f, 0f, 0f), Vector3Utility.AngleToFlat(this.origin, this.destination));
			Graphic graphic = VPE_DefOf.VPE_ChainBolt.graphicData.Graphic;
			foreach (Thing thing in this.currentTargets)
			{
				Vector3 vector2 = Vector3Utility.Yto0(thing.DrawPos);
				Vector3 vector3;
				vector3..ctor(graphic.drawSize.x, 1f, (vector2 - vector).magnitude);
				Matrix4x4 matrix4x = Matrix4x4.TRS(vector + (vector2 - vector) / 2f + Vector3.up * (this.def.Altitude - 0.018292684f), Quaternion.LookRotation(vector2 - vector), vector3);
				Graphics.DrawMesh(MeshPool.plane10, matrix4x, graphic.MatSingle, 0);
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00016BD0 File Offset: 0x00014DD0
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			GenExplosion.DoExplosion(base.Position, base.Map, this.def.projectile.explosionRadius, this.def.projectile.damageDef, this.launcher, this.DamageAmount, this.ArmorPenetration, this.def.projectile.soundExplode, this.equipmentDef, this.def, this.intendedTarget.Thing, this.def.projectile.postExplosionSpawnThingDef, this.def.projectile.postExplosionSpawnChance, this.def.projectile.postExplosionSpawnThingCount, this.def.projectile.postExplosionGasType, new float?(this.def.projectile.postExplosionSpawnChance), 255, this.def.projectile.applyDamageToExplosionCellsNeighbors, this.def.projectile.preExplosionSpawnThingDef, this.def.projectile.preExplosionSpawnChance, this.def.projectile.preExplosionSpawnThingCount, this.def.projectile.explosionChanceToStartFire, this.def.projectile.explosionDamageFalloff, new float?(Vector3Utility.AngleToFlat(this.origin, this.destination)), null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			base.Impact(hitThing, blockedByShield);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00016D3F File Offset: 0x00014F3F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksTillAttack, "ticksTillAttack", 0, false);
			Scribe_Collections.Look<Thing>(ref this.currentTargets, "currentTargets", 3, Array.Empty<object>());
		}

		// Token: 0x040001B7 RID: 439
		private const int WARMUP = 180;

		// Token: 0x040001B8 RID: 440
		private List<Thing> currentTargets = new List<Thing>();

		// Token: 0x040001B9 RID: 441
		private int ticksTillAttack = -1;
	}
}
