using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200005D RID: 93
	public class Hediff_GroupLink : Hediff_Overlay
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00006583 File Offset: 0x00004783
		public override string OverlayPath
		{
			get
			{
				return "Other/ForceField";
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000658A File Offset: 0x0000478A
		public virtual Color OverlayColor
		{
			get
			{
				return new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.5f);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000065B4 File Offset: 0x000047B4
		public override float OverlaySize
		{
			get
			{
				return this.ability.GetRadiusForPawn();
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000065C1 File Offset: 0x000047C1
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.LinkAllPawnsAround();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x000065D0 File Offset: 0x000047D0
		public void LinkAllPawnsAround()
		{
			foreach (Pawn item in from x in GenRadial.RadialDistinctThingsAround(this.pawn.Position, this.pawn.Map, this.ability.GetRadiusForPawn(), true).OfType<Pawn>()
			where x.RaceProps.Humanlike && x != this.pawn
			select x)
			{
				if (!this.linkedPawns.Contains(item))
				{
					this.linkedPawns.Add(item);
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006668 File Offset: 0x00004868
		private void UnlinkAll()
		{
			for (int i = this.linkedPawns.Count - 1; i >= 0; i--)
			{
				this.linkedPawns.RemoveAt(i);
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006699 File Offset: 0x00004899
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.UnlinkAll();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000066A8 File Offset: 0x000048A8
		public override void Tick()
		{
			base.Tick();
			for (int i = this.linkedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.linkedPawns[i];
				if (pawn.Map != this.pawn.Map || IntVec3Utility.DistanceTo(pawn.Position, this.pawn.Position) > this.ability.GetRadiusForPawn())
				{
					this.linkedPawns.RemoveAt(i);
				}
			}
			if (!GenCollection.Any<Pawn>(this.linkedPawns))
			{
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006740 File Offset: 0x00004940
		public override void Draw()
		{
			Vector3 drawPos = this.pawn.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(28);
			Color overlayColor = this.OverlayColor;
			this.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, overlayColor);
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x.SetTRS(drawPos, Quaternion.identity, new Vector3(this.OverlaySize * 2f * 1.1601562f, 1f, this.OverlaySize * 2f * 1.1601562f));
			Graphics.DrawMesh(MeshPool.plane10, matrix4x, base.OverlayMat, 0, null, 0, this.MatPropertyBlock);
			foreach (Pawn pawn in this.linkedPawns)
			{
				GenDraw.DrawLineBetween(pawn.DrawPos, this.pawn.DrawPos, 5, 0.2f);
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006838 File Offset: 0x00004A38
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.linkedPawns, "linkedPawns", 3, Array.Empty<object>());
		}

		// Token: 0x04000048 RID: 72
		public List<Pawn> linkedPawns = new List<Pawn>();
	}
}
