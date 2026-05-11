using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000EB RID: 235
	public class Ability_AffectMechs : Ability
	{
		// Token: 0x06000333 RID: 819 RVA: 0x00013ED4 File Offset: 0x000120D4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				foreach (Thing thing in GenCollection.InRandomOrder<Thing>(this.AllTargetsAt(globalTargetInfo.Cell, globalTargetInfo.Map), null).Take(3))
				{
					this.ApplyHediffs(new GlobalTargetInfo[]
					{
						new GlobalTargetInfo(thing)
					});
					CompHaywire compHaywire = ThingCompUtility.TryGetComp<CompHaywire>(thing);
					if (compHaywire != null)
					{
						compHaywire.GoHaywire(this.GetDurationForPawn());
					}
				}
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00013F90 File Offset: 0x00012190
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlight(target);
			foreach (Thing thing in this.AllTargetsAt(target.Cell, null))
			{
				GenDraw.DrawTargetHighlight(thing);
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00013FF0 File Offset: 0x000121F0
		private IEnumerable<Thing> AllTargetsAt(IntVec3 cell, Map map = null)
		{
			Ability_AffectMechs.<AllTargetsAt>d__2 <AllTargetsAt>d__ = new Ability_AffectMechs.<AllTargetsAt>d__2(-2);
			<AllTargetsAt>d__.<>4__this = this;
			<AllTargetsAt>d__.<>3__cell = cell;
			<AllTargetsAt>d__.<>3__map = map;
			return <AllTargetsAt>d__;
		}
	}
}
