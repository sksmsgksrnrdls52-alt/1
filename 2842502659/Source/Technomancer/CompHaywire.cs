using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F7 RID: 247
	public class CompHaywire : ThingComp
	{
		// Token: 0x0600036B RID: 875 RVA: 0x0001557C File Offset: 0x0001377C
		public void GoHaywire(int duration)
		{
			this.ticksLeft = Mathf.Max(duration, this.ticksLeft);
			HaywireManager.HaywireThings.Add(this.parent);
			if (this.effecter == null)
			{
				this.effecter = VPE_DefOf.VPE_Haywire.Spawn(this.parent, this.parent.Map, 1f);
				this.effecter.Trigger(this.parent, this.parent, -1);
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00015600 File Offset: 0x00013800
		public override void CompTick()
		{
			base.CompTick();
			if (this.ticksLeft > 0)
			{
				this.effecter.EffectTick(this.parent, this.parent);
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					HaywireManager.HaywireThings.Remove(this.parent);
					this.effecter.Cleanup();
					this.effecter = null;
				}
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00015677 File Offset: 0x00013877
		public override void PostDeSpawn(Map map, DestroyMode mode = 0)
		{
			base.PostDeSpawn(map, mode);
			if (HaywireManager.HaywireThings.Contains(this.parent))
			{
				HaywireManager.HaywireThings.Remove(this.parent);
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000156A4 File Offset: 0x000138A4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.ticksLeft > 0)
			{
				HaywireManager.HaywireThings.Add(this.parent);
				this.effecter = VPE_DefOf.VPE_Haywire.Spawn(this.parent, this.parent.Map, 1f);
				this.effecter.Trigger(this.parent, this.parent, -1);
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0001571B File Offset: 0x0001391B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "haywireTicksLeft", 0, false);
		}

		// Token: 0x040001A4 RID: 420
		private int ticksLeft;

		// Token: 0x040001A5 RID: 421
		private Effecter effecter;
	}
}
