using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200000B RID: 11
	public class JobGiver_Flick : ThinkNode_JobGiver
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000021 RID: 33 RVA: 0x0000275C File Offset: 0x0000095C
		public PathEndMode PathEndMode
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000275F File Offset: 0x0000095F
		public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			JobGiver_Flick.<PotentialWorkThingsGlobal>d__2 <PotentialWorkThingsGlobal>d__ = new JobGiver_Flick.<PotentialWorkThingsGlobal>d__2(-2);
			<PotentialWorkThingsGlobal>d__.<>3__pawn = pawn;
			return <PotentialWorkThingsGlobal>d__;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000276F File Offset: 0x0000096F
		public bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Flick);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002789 File Offset: 0x00000989
		public bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) != null && ReservationUtility.CanReserve(pawn, t, 1, -1, null, forced);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000027BC File Offset: 0x000009BC
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (this.ShouldSkip(pawn, false))
			{
				return null;
			}
			Predicate<Thing> predicate = (Thing x) => this.HasJobOnThing(pawn, x, false);
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(10), this.PathEndMode, TraverseParms.For(pawn, 2, 0, false, false, false, true), 100f, predicate, this.PotentialWorkThingsGlobal(pawn), 0, -1, false, 14, false, false);
			if (thing == null)
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Flick, thing);
		}
	}
}
