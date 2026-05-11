using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000069 RID: 105
	public class Hediff_IceBlock : Hediff_Overlay
	{
		// Token: 0x06000130 RID: 304 RVA: 0x000072A4 File Offset: 0x000054A4
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			IntVec3 facingCell = this.pawn.Rotation.FacingCell;
			int ticksToDisappear = HediffUtility.TryGetComp<HediffComp_Disappears>(this).ticksToDisappear;
			Job job = JobMaker.MakeJob(VPE_DefOf.VPE_StandFreeze);
			job.expiryInterval = ticksToDisappear;
			job.overrideFacing = this.pawn.Rotation;
			this.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(0), false);
			this.pawn.pather.StopDead();
			this.pawn.stances.SetStance(new Stance_Stand(ticksToDisappear, facingCell, null));
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00007341 File Offset: 0x00005541
		public override string OverlayPath
		{
			get
			{
				return "Effects/Frostshaper/IceBlock/IceBlock";
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007348 File Offset: 0x00005548
		public override void Draw()
		{
			Vector3 drawPos = this.pawn.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(28);
			Matrix4x4 matrix4x = default(Matrix4x4);
			float num = 1.5f;
			matrix4x.SetTRS(drawPos, Quaternion.identity, new Vector3(num, 1f, num));
			Graphics.DrawMesh(MeshPool.plane10, matrix4x, base.OverlayMat, 0, null, 0, this.MatPropertyBlock);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000073B0 File Offset: 0x000055B0
		public override void Tick()
		{
			base.Tick();
			HediffDef hediffDef;
			if (Gen.IsHashIntervalTick(this.pawn, 60) && this.pawn.CanReceiveHypothermia(out hediffDef))
			{
				HealthUtility.AdjustSeverity(this.pawn, hediffDef, 0.05f);
			}
		}
	}
}
