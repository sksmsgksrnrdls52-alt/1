using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B6 RID: 182
	public class AbilityExtension_ForceJobOnTargetBase : AbilityExtension_AbilityMod
	{
		// Token: 0x06000258 RID: 600 RVA: 0x0000D710 File Offset: 0x0000B910
		protected void ForceJob(GlobalTargetInfo target, Ability ability)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				Job job = JobMaker.MakeJob(this.jobDef, ability.pawn);
				float num = 1f;
				if (this.durationMultiplier != null)
				{
					num = StatExtension.GetStatValue(pawn, this.durationMultiplier, true, -1);
				}
				job.expiryInterval = (int)((float)ability.GetDurationForPawn() * num);
				job.mote = MoteMaker.MakeThoughtBubble(pawn, ability.def.iconPath, true);
				pawn.jobs.StopAll(false, true);
				pawn.jobs.StartJob(job, 16, null, false, true, null, null, false, false, null, false, true, false);
				if (this.fleckOnTarget != null)
				{
					Ability.MakeStaticFleck(pawn.DrawPos, pawn.Map, this.fleckOnTarget, 1f, 0f);
				}
			}
		}

		// Token: 0x040000AC RID: 172
		public JobDef jobDef;

		// Token: 0x040000AD RID: 173
		public StatDef durationMultiplier;

		// Token: 0x040000AE RID: 174
		public FleckDef fleckOnTarget;
	}
}
