using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200006B RID: 107
	public class JobDriver_StandFreeze : JobDriver
	{
		// Token: 0x0600013F RID: 319 RVA: 0x00007834 File Offset: 0x00005A34
		public override string GetReport()
		{
			return Translator.Translate("ReportStanding");
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007845 File Offset: 0x00005A45
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007848 File Offset: 0x00005A48
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_StandFreeze.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_StandFreeze.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007858 File Offset: 0x00005A58
		public virtual void DecorateWaitToil(Toil wait)
		{
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000785A File Offset: 0x00005A5A
		public override void SetInitialPosture()
		{
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000785C File Offset: 0x00005A5C
		public override void Notify_StanceChanged()
		{
			if (this.pawn.stances.curStance is Stance_Mobile)
			{
				base.EndJobWith(8);
			}
		}
	}
}
