using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000098 RID: 152
	public class MoteAttachedScaled : MoteAttached
	{
		// Token: 0x060001CD RID: 461 RVA: 0x0000A51C File Offset: 0x0000871C
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (!base.Destroyed && this.def.mote.growthRate != 0f)
			{
				this.linearScale = new Vector3(this.linearScale.x + this.def.mote.growthRate * deltaTime, this.linearScale.y, this.linearScale.z + this.def.mote.growthRate * deltaTime);
				this.linearScale.x = Mathf.Min(Mathf.Max(this.linearScale.x, 0.0001f), this.maxScale);
				this.linearScale.z = Mathf.Min(Mathf.Max(this.linearScale.z, 0.0001f), this.maxScale);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000A5FD File Offset: 0x000087FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.maxScale, "maxScale", 0f, false);
		}

		// Token: 0x0400007F RID: 127
		public float maxScale;
	}
}
