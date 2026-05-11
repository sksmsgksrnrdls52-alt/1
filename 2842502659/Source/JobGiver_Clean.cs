using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000009 RID: 9
	public class JobGiver_Clean : ThinkNode_JobGiver
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000023B9 File Offset: 0x000005B9
		public PathEndMode PathEndMode
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000023BC File Offset: 0x000005BC
		public IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000023CE File Offset: 0x000005CE
		public bool ShouldSkip(Pawn pawn)
		{
			return pawn.Map.listerFilthInHomeArea.FilthInHomeArea.Count == 0;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000023E8 File Offset: 0x000005E8
		public bool HasJobOnThing(Pawn pawn, Thing t)
		{
			Filth filth = t as Filth;
			return filth != null && filth.Map.areaManager.Home[filth.Position] && ReservationUtility.CanReserve(pawn, t, 1, -1, null, false) && filth.TicksSinceThickened >= this.MinTicksSinceThickened;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002448 File Offset: 0x00000648
		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_Clean.<>c__DisplayClass6_0 CS$<>8__locals1 = new JobGiver_Clean.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawn = pawn;
			if (this.ShouldSkip(CS$<>8__locals1.pawn))
			{
				return null;
			}
			Predicate<Thing> predicate = (Thing x) => x.def.category == 6 && CS$<>8__locals1.<>4__this.HasJobOnThing(CS$<>8__locals1.pawn, x);
			Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(15), this.PathEndMode, TraverseParms.For(CS$<>8__locals1.pawn, 2, 0, false, false, false, true), 100f, predicate, this.PotentialWorkThingsGlobal(CS$<>8__locals1.pawn), 0, -1, false, 14, false, false);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Clean);
			job.AddQueuedTarget(1, thing);
			int num = 15;
			CS$<>8__locals1.map = thing.Map;
			CS$<>8__locals1.room = RegionAndRoomQuery.GetRoom(thing, 15);
			for (int i = 0; i < 100; i++)
			{
				IntVec3 intVec = thing.Position + GenRadial.RadialPattern[i];
				if (CS$<>8__locals1.<TryGiveJob>g__ShouldClean|2(intVec))
				{
					List<Thing> thingList = GridsUtility.GetThingList(intVec, CS$<>8__locals1.map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Thing thing2 = thingList[j];
						if (this.HasJobOnThing(CS$<>8__locals1.pawn, thing2) && thing2 != thing)
						{
							job.AddQueuedTarget(1, thing2);
						}
					}
					if (job.GetTargetQueue(1).Count >= num)
					{
						break;
					}
				}
			}
			if (job.targetQueueA != null && job.targetQueueA.Count >= 5)
			{
				GenCollection.SortBy<LocalTargetInfo, int>(job.targetQueueA, (LocalTargetInfo targ) => IntVec3Utility.DistanceToSquared(targ.Cell, CS$<>8__locals1.pawn.Position));
			}
			return job;
		}

		// Token: 0x04000008 RID: 8
		private int MinTicksSinceThickened = 600;
	}
}
