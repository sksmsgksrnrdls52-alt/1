using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using VEF;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B4 RID: 180
	[StaticConstructorOnStartup]
	public class Hediff_Overshield : Hediff_Overlay
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000CF94 File Offset: 0x0000B194
		public override string OverlayPath
		{
			get
			{
				return "Other/ForceField";
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000CF9B File Offset: 0x0000B19B
		public virtual Color OverlayColor
		{
			get
			{
				return Color.yellow;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000CFA4 File Offset: 0x0000B1A4
		public override void Tick()
		{
			base.Tick();
			if (this.pawn.Map != null)
			{
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.pawn.Position, this.pawn.Map, this.OverlaySize + 1f, true))
				{
					Projectile projectile = thing as Projectile;
					if (projectile != null && this.CanDestroyProjectile(projectile))
					{
						this.DestroyProjectile(projectile);
					}
				}
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000D038 File Offset: 0x0000B238
		protected virtual void DestroyProjectile(Projectile projectile)
		{
			Effecter effecter = new Effecter(VPE_DefOf.Interceptor_BlockedProjectilePsychic);
			effecter.Trigger(new TargetInfo(projectile.Position, this.pawn.Map, false), TargetInfo.Invalid, -1);
			effecter.Cleanup();
			this.lastInterceptAngle = Vector3Utility.AngleToFlat(projectile.ExactPosition, GenThing.TrueCenter(this.pawn));
			this.lastInterceptTicks = Find.TickManager.TicksGame;
			this.drawInterceptCone = true;
			projectile.Destroy(0);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000D0B4 File Offset: 0x0000B2B4
		public unsafe virtual bool CanDestroyProjectile(Projectile projectile)
		{
			IntVec3 item = IntVec3Utility.ToIntVec3(Vector3Utility.Yto0(*NonPublicFields.Projectile_origin.Invoke(projectile)));
			return Vector3.Distance(Vector3Utility.Yto0(projectile.ExactPosition), Vector3Utility.Yto0(this.pawn.DrawPos)) <= this.OverlaySize && !GenRadial.RadialCellsAround(this.pawn.Position, this.OverlaySize, true).ToList<IntVec3>().Contains(item);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000D12C File Offset: 0x0000B32C
		public override void Draw()
		{
			Vector3 drawPos = this.pawn.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(28);
			float currentAlpha = this.GetCurrentAlpha();
			if (currentAlpha > 0f)
			{
				Color overlayColor = this.OverlayColor;
				overlayColor.a *= currentAlpha;
				this.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, overlayColor);
				Matrix4x4 matrix4x = default(Matrix4x4);
				matrix4x.SetTRS(drawPos, Quaternion.identity, new Vector3(this.OverlaySize * 2f * 1.1601562f, 1f, this.OverlaySize * 2f * 1.1601562f));
				Graphics.DrawMesh(MeshPool.plane10, matrix4x, base.OverlayMat, 0, null, 0, this.MatPropertyBlock);
			}
			float currentConeAlpha_RecentlyIntercepted = this.GetCurrentConeAlpha_RecentlyIntercepted();
			if (currentConeAlpha_RecentlyIntercepted > 0f)
			{
				Color overlayColor2 = this.OverlayColor;
				overlayColor2.a *= currentConeAlpha_RecentlyIntercepted;
				this.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, overlayColor2);
				Matrix4x4 matrix4x2 = default(Matrix4x4);
				matrix4x2.SetTRS(drawPos, Quaternion.Euler(0f, this.lastInterceptAngle - 90f, 0f), new Vector3(this.OverlaySize * 2f * 1.1601562f, 1f, this.OverlaySize * 2f * 1.1601562f));
				Graphics.DrawMesh(MeshPool.plane10, matrix4x2, Hediff_Overshield.ForceFieldConeMat, 0, null, 0, this.MatPropertyBlock);
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000D292 File Offset: 0x0000B492
		private float GetCurrentAlpha()
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(this.GetCurrentAlpha_Idle(), this.GetCurrentAlpha_Selected()), this.GetCurrentAlpha_RecentlyIntercepted()), this.GetCurrentAlpha_RecentlyActivated()), Hediff_Overshield.minAlpha);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000D2C8 File Offset: 0x0000B4C8
		private float GetCurrentAlpha_Idle()
		{
			if (Find.Selector.IsSelected(this.pawn))
			{
				return 0f;
			}
			return Mathf.Lerp(Hediff_Overshield.minIdleAlpha, 0.11f, (Mathf.Sin((float)(Gen.HashCombineInt(this.pawn.thingIDNumber, 96804938) % 100) + Time.realtimeSinceStartup * Hediff_Overshield.idlePulseSpeed) + 1f) / 2f);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000D334 File Offset: 0x0000B534
		private float GetCurrentAlpha_Selected()
		{
			float num = Mathf.Max(2f, Hediff_Overshield.idlePulseSpeed);
			if (!Find.Selector.IsSelected(this.pawn))
			{
				return 0f;
			}
			return Mathf.Lerp(0.2f, 0.62f, (Mathf.Sin((float)(Gen.HashCombineInt(this.pawn.thingIDNumber, 35990913) % 100) + Time.realtimeSinceStartup * num) + 1f) / 2f);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D3AC File Offset: 0x0000B5AC
		private float GetCurrentAlpha_RecentlyIntercepted()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.09f;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
		private float GetCurrentAlpha_RecentlyActivated()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 50f) * 0.09f;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D41C File Offset: 0x0000B61C
		private float GetCurrentConeAlpha_RecentlyIntercepted()
		{
			if (!this.drawInterceptCone)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.82f;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D464 File Offset: 0x0000B664
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastInterceptTicks, "lastInterceptTicks", 0, false);
			Scribe_Values.Look<float>(ref this.lastInterceptAngle, "lastInterceptTicks", 0f, false);
			Scribe_Values.Look<bool>(ref this.drawInterceptCone, "drawInterceptCone", false, false);
		}

		// Token: 0x040000A5 RID: 165
		private int lastInterceptTicks = -999999;

		// Token: 0x040000A6 RID: 166
		private float lastInterceptAngle;

		// Token: 0x040000A7 RID: 167
		private bool drawInterceptCone;

		// Token: 0x040000A8 RID: 168
		public static float idlePulseSpeed = 3f;

		// Token: 0x040000A9 RID: 169
		public static float minIdleAlpha = 0.05f;

		// Token: 0x040000AA RID: 170
		public static float minAlpha = 0.2f;

		// Token: 0x040000AB RID: 171
		private static Material ForceFieldConeMat = MaterialPool.MatFrom("Other/ForceFieldCone", ShaderDatabase.MoteGlow);
	}
}
