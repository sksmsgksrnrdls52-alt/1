using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;
using VanillaPsycastsExpanded.Graphics;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000AF RID: 175
	[StaticConstructorOnStartup]
	public class Projectile_FrostRay : Projectile
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float num = Projectile_FrostRay.ArcHeightFactor(this) * GenMath.InverseParabola(base.DistanceCoveredFraction);
			float num2 = Vector3.Distance(Vector3Utility.Yto0(this.origin), Vector3Utility.Yto0(drawLoc));
			Vector3 vector = Vector3.Lerp(this.origin, drawLoc, 0.5f);
			vector.y += 5f;
			Vector3 vector2 = vector + new Vector3(0f, 0f, 1f) * num;
			if (this.def.projectile.shadowSize > 0f)
			{
				this.DrawShadow(vector, num);
			}
			base.Comps_PostDraw();
			Graphics.DrawMesh(MeshPool.GridPlane(new Vector2(5f, num2)), vector2, this.ExactRotation, (this.Graphic as Graphic_Animated).MatSingle, 0);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
		protected override void Tick()
		{
			base.Tick();
			if (this.sustainer == null || this.sustainer.Ended)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(VPE_DefOf.VPE_FrostRay_Sustainer, SoundInfo.InMap(this, 1));
			}
			this.sustainer.Maintain();
			Pawn pawn = this.launcher as Pawn;
			if (pawn != null && (pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_FrostRay, false) == null || pawn.Downed || pawn.Dead))
			{
				this.Destroy(0);
				return;
			}
			if (Gen.IsHashIntervalTick(this, 10))
			{
				ShootLine resultingLine = new ShootLine(IntVec3Utility.ToIntVec3(this.origin), IntVec3Utility.ToIntVec3(this.DrawPos));
				IEnumerable<IntVec3> enumerable = from x in resultingLine.Points()
				where x != resultingLine.Source
				select x;
				HashSet<Pawn> hashSet = new HashSet<Pawn>();
				foreach (IntVec3 intVec in enumerable)
				{
					foreach (Pawn item in GridsUtility.GetThingList(intVec, base.Map).OfType<Pawn>())
					{
						hashSet.Add(item);
					}
				}
				foreach (Pawn pawn2 in hashSet)
				{
					BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(this.launcher, pawn2, this.intendedTarget.Thing, this.launcher.def, this.def, this.targetCoverDef);
					Find.BattleLog.Add(battleLogEntry_RangedImpact);
					DamageInfo damageInfo;
					damageInfo..ctor(this.def.projectile.damageDef, (float)this.DamageAmount, this.ArmorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, null, this.equipmentDef, 0, this.intendedTarget.Thing, true, true, 2, true, false);
					pawn2.TakeDamage(damageInfo).AssociateWithLog(battleLogEntry_RangedImpact);
					HediffDef hediffDef;
					if (pawn2.CanReceiveHypothermia(out hediffDef))
					{
						HealthUtility.AdjustSeverity(pawn2, hediffDef, 0.013333333f);
					}
					HealthUtility.AdjustSeverity(pawn2, VPE_DefOf.VFEP_HypothermicSlowdown, 0.013333333f);
				}
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000CC30 File Offset: 0x0000AE30
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000CC34 File Offset: 0x0000AE34
		private void DrawShadow(Vector3 drawLoc, float height)
		{
			if (!(Projectile_FrostRay.shadowMaterial == null))
			{
				float num = this.def.projectile.shadowSize * Mathf.Lerp(1f, 0.6f, height);
				Vector3 vector;
				vector..ctor(num, 1f, num);
				Vector3 vector2;
				vector2..ctor(0f, -0.01f, 0f);
				Matrix4x4 matrix4x = default(Matrix4x4);
				matrix4x.SetTRS(drawLoc + vector2, Quaternion.identity, vector);
				Graphics.DrawMesh(MeshPool.plane10, matrix4x, Projectile_FrostRay.shadowMaterial, 0);
			}
		}

		// Token: 0x040000A1 RID: 161
		private static readonly Material shadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent);

		// Token: 0x040000A2 RID: 162
		public static Func<Projectile, float> ArcHeightFactor = (Func<Projectile, float>)Delegate.CreateDelegate(typeof(Func<Projectile, float>), null, AccessTools.Method(typeof(Projectile), "get_ArcHeightFactor", null, null));

		// Token: 0x040000A3 RID: 163
		private Sustainer sustainer;
	}
}
