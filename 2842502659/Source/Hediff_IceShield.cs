using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000AD RID: 173
	public class Hediff_IceShield : Hediff_Overlay
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000C5DA File Offset: 0x0000A7DA
		public override float OverlaySize
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			foreach (Hediff hediff in (from x in this.pawn.health.hediffSet.hediffs
			where x.def == HediffDefOf.Hypothermia || x.def == VPE_DefOf.VFEP_HypothermicSlowdown || x.def == VPE_DefOf.HypothermicSlowdown
			select x).ToList<Hediff>())
			{
				this.pawn.health.RemoveHediff(hediff);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000C680 File Offset: 0x0000A880
		public override string OverlayPath
		{
			get
			{
				return "Effects/Frostshaper/FrostShield/Frostshield";
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C688 File Offset: 0x0000A888
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			Pawn pawn = dinfo.Instigator as Pawn;
			HediffDef hediffDef;
			if (pawn != null && Vector3.Distance(pawn.DrawPos, this.pawn.DrawPos) <= this.OverlaySize && pawn.CanReceiveHypothermia(out hediffDef))
			{
				HealthUtility.AdjustSeverity(pawn, hediffDef, 0.05f);
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		public override void Draw()
		{
			Vector3 drawPos = this.pawn.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(28);
			Matrix4x4 matrix4x = default(Matrix4x4);
			matrix4x.SetTRS(drawPos, Quaternion.identity, new Vector3(this.OverlaySize, 1f, this.OverlaySize));
			Graphics.DrawMesh(MeshPool.plane10, matrix4x, base.OverlayMat, 0, null, 0, this.MatPropertyBlock);
		}
	}
}
