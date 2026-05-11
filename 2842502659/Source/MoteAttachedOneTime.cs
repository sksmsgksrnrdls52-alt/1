using System;
using VanillaPsycastsExpanded.Graphics;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200001B RID: 27
	public class MoteAttachedOneTime : MoteAttached, IAnimationOneTime
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00002EFB File Offset: 0x000010FB
		public int CurrentIndex()
		{
			return this.currentIndex;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002F04 File Offset: 0x00001104
		protected override void Tick()
		{
			base.Tick();
			if (Gen.IsHashIntervalTick(this, (this.Graphic.data as GraphicData_Animated).ticksPerFrame) && this.currentIndex < (this.Graphic as Graphic_Animated).SubGraphicCount)
			{
				this.currentIndex++;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002F5A File Offset: 0x0000115A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.currentIndex, "currentIndex", 0, false);
		}

		// Token: 0x04000012 RID: 18
		public bool shouldDestroy;

		// Token: 0x04000013 RID: 19
		public int currentIndex;
	}
}
