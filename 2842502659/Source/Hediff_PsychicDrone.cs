using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000097 RID: 151
	[StaticConstructorOnStartup]
	public class Hediff_PsychicDrone : Hediff_Overlay
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000A271 File Offset: 0x00008471
		public override string OverlayPath
		{
			get
			{
				return "Effects/Archotechist/PsychicDrone/PsychicDroneEnergyField";
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000A278 File Offset: 0x00008478
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.maintainedMotes.Add(this.SpawnMoteAttached(VPE_DefOf.VPE_PsycastAreaEffectMaintained, this.ability.GetRadiusForPawn(), 0f));
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000A2A8 File Offset: 0x000084A8
		public override void Tick()
		{
			base.Tick();
			this.curAngle += 0.015f;
			if (this.curAngle > 360f)
			{
				this.curAngle = 0f;
			}
			foreach (Mote mote in this.maintainedMotes)
			{
				mote.Maintain();
			}
			Pawn pawn;
			if (Find.TickManager.TicksGame % 180 == 0 && GenCollection.TryRandomElement<Pawn>(from x in GenRadial.RadialDistinctThingsAround(this.pawn.Position, this.pawn.Map, this.ability.GetRadiusForPawn(), true).OfType<Pawn>()
			where !this.affectedPawns.Contains(x) && !x.InMentalState && GenHostility.HostileTo(x, this.pawn) && x.RaceProps.IsFlesh
			select x, ref pawn))
			{
				MentalStateDef mentalStateDef = Rand.Bool ? VPE_DefOf.VPE_Wander_Sad : MentalStateDefOf.Berserk;
				if (pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateDef, null, false, false, false, null, false, false, true))
				{
					this.affectedPawns.Add(pawn);
				}
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000A3C0 File Offset: 0x000085C0
		public override void Draw()
		{
			Vector3 drawPos = this.pawn.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(28);
			Matrix4x4 matrix4x = default(Matrix4x4);
			float num = this.ability.GetRadiusForPawn() * 2f;
			matrix4x.SetTRS(drawPos, Quaternion.AngleAxis(this.curAngle, Vector3.up), new Vector3(num, 1f, num));
			Graphics.DrawMesh(MeshPool.plane10, matrix4x, base.OverlayMat, 0, null, 0, this.MatPropertyBlock);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000A440 File Offset: 0x00008640
		public Mote SpawnMoteAttached(ThingDef moteDef, float scale, float rotationRate)
		{
			MoteAttachedScaled moteAttachedScaled = MoteMaker.MakeAttachedOverlay(this.pawn, moteDef, Vector3.zero, 1f, -1f) as MoteAttachedScaled;
			moteAttachedScaled.maxScale = scale;
			moteAttachedScaled.rotationRate = rotationRate;
			if (moteAttachedScaled.def.mote.needsMaintenance)
			{
				moteAttachedScaled.Maintain();
			}
			return moteAttachedScaled;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000A495 File Offset: 0x00008695
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.affectedPawns, "affectedPawns", 3, Array.Empty<object>());
			Scribe_Collections.Look<Mote>(ref this.maintainedMotes, "maintainedMotes", 3, Array.Empty<object>());
		}

		// Token: 0x0400007C RID: 124
		private float curAngle;

		// Token: 0x0400007D RID: 125
		private List<Mote> maintainedMotes = new List<Mote>();

		// Token: 0x0400007E RID: 126
		private List<Pawn> affectedPawns = new List<Pawn>();
	}
}
