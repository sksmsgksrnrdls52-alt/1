using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200003D RID: 61
	public class MoteBetween : Mote
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00005210 File Offset: 0x00003410
		public float LifetimeFraction
		{
			get
			{
				return base.AgeSecs / this.def.mote.Lifespan;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005229 File Offset: 0x00003429
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero, false);
			this.link2 = new MoteAttachLink(b, Vector3.zero, false);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000524F File Offset: 0x0000344F
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.UpdatePositionAndRotation(ref drawLoc);
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005264 File Offset: 0x00003464
		protected void UpdatePositionAndRotation(ref Vector3 drawPos)
		{
			if (this.link1.Linked && this.link2.Linked)
			{
				if (!this.link1.Target.ThingDestroyed)
				{
					this.link1.UpdateDrawPos();
				}
				if (!this.link2.Target.ThingDestroyed)
				{
					this.link2.UpdateDrawPos();
				}
				Vector3 lastDrawPos = this.link1.LastDrawPos;
				Vector3 lastDrawPos2 = this.link2.LastDrawPos;
				this.exactPosition = lastDrawPos + (lastDrawPos2 - lastDrawPos) * this.LifetimeFraction;
				if (this.def.mote.rotateTowardsTarget)
				{
					this.exactRotation = Vector3Utility.AngleToFlat(lastDrawPos, lastDrawPos2) + 90f;
				}
			}
			this.exactPosition.y = Altitudes.AltitudeFor(this.def.altitudeLayer);
			drawPos = this.exactPosition;
		}

		// Token: 0x0400003B RID: 59
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}
