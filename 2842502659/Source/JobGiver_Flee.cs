using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200000A RID: 10
	public class JobGiver_Flee : ThinkNode_JobGiver
	{
		// Token: 0x0600001E RID: 30 RVA: 0x000025F4 File Offset: 0x000007F4
		protected override Job TryGiveJob(Pawn pawn)
		{
			List<Pawn> list = (from x in pawn.Map.mapPawns.AllPawnsSpawned
			where !x.Dead && !x.Downed && IntVec3Utility.DistanceTo(x.Position, pawn.Position) < 50f && GenSight.LineOfSight(x.Position, pawn.Position, pawn.Map)
			orderby IntVec3Utility.DistanceTo(x.Position, pawn.Position)
			select x).ToList<Pawn>();
			if (!GenCollection.Any<Pawn>(list))
			{
				return null;
			}
			IntVec3 intVec;
			if (pawn.Faction != Faction.OfPlayer && CellFinderLoose.GetFleeExitPosition(pawn, 10f, ref intVec))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Flee, intVec, list.First<Pawn>());
				job.exitMapOnArrival = true;
				return job;
			}
			return this.FleeJob(pawn, list.First<Pawn>(), list.Cast<Thing>().ToList<Thing>());
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000026BC File Offset: 0x000008BC
		public Job FleeJob(Pawn pawn, Thing danger, List<Thing> dangers)
		{
			Job result = null;
			IntVec3 intVec;
			if (pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Flee)
			{
				intVec = pawn.CurJob.targetA.Cell;
			}
			else
			{
				intVec = CellFinderLoose.GetFleeDest(pawn, dangers, 24f);
			}
			if (intVec == pawn.Position)
			{
				intVec = GenCollection.RandomElement<IntVec3>(GenRadial.RadialCellsAround(pawn.Position, 1f, 15f));
			}
			if (intVec != pawn.Position)
			{
				result = JobMaker.MakeJob(JobDefOf.Flee, intVec, danger);
			}
			return result;
		}
	}
}
