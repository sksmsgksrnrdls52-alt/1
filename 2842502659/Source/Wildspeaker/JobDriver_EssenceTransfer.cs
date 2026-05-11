using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000E3 RID: 227
	public class JobDriver_EssenceTransfer : JobDriver
	{
		// Token: 0x0600030E RID: 782 RVA: 0x00013778 File Offset: 0x00011978
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return ReservationUtility.Reserve(this.pawn, this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0001379B File Offset: 0x0001199B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_EssenceTransfer.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_EssenceTransfer.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x000137AB File Offset: 0x000119AB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.restStartTick, "restStartTick", 0, false);
		}

		// Token: 0x04000193 RID: 403
		private int restStartTick;
	}
}
