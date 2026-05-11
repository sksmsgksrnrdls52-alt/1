using System;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F8 RID: 248
	public class HediffComp_Haywire : HediffComp
	{
		// Token: 0x06000371 RID: 881 RVA: 0x00015740 File Offset: 0x00013940
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			HaywireManager.HaywireThings.Add(base.Pawn);
			Pawn_StanceTracker stances = base.Pawn.stances;
			if (stances != null)
			{
				stances.CancelBusyStanceHard();
			}
			Pawn_JobTracker jobs = base.Pawn.jobs;
			if (jobs == null)
			{
				return;
			}
			jobs.EndCurrentJob(16, true, true);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00015794 File Offset: 0x00013994
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			HaywireManager.HaywireThings.Remove(base.Pawn);
			Pawn_StanceTracker stances = base.Pawn.stances;
			if (stances != null)
			{
				stances.CancelBusyStanceHard();
			}
			Pawn_JobTracker jobs = base.Pawn.jobs;
			if (jobs == null)
			{
				return;
			}
			jobs.EndCurrentJob(16, true, true);
		}
	}
}
