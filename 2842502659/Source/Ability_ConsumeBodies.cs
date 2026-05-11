using System;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200006D RID: 109
	public class Ability_ConsumeBodies : Ability_TargetCorpse
	{
		// Token: 0x0600014A RID: 330 RVA: 0x000079F8 File Offset: 0x00005BF8
		public override void WarmupToil(Toil toil)
		{
			base.WarmupToil(toil);
			toil.AddPreInitAction(delegate()
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.Comp.currentlyCastingTargets)
				{
					if (globalTargetInfo.HasThing && ThingCompUtility.TryGetComp<CompRottable>(globalTargetInfo.Thing) != null)
					{
						this.AddEffecterToMaintain(VPE_DefOf.VPE_Liferot.Spawn(globalTargetInfo.Thing.Position, this.pawn.Map, 1f), globalTargetInfo.Thing, toil.defaultDuration);
					}
				}
			});
			toil.AddPreTickAction(delegate()
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.Comp.currentlyCastingTargets)
				{
					if (globalTargetInfo.HasThing && ThingCompUtility.TryGetComp<CompRottable>(globalTargetInfo.Thing) != null && Gen.IsHashIntervalTick(globalTargetInfo.Thing, 60))
					{
						FilthMaker.TryMakeFilth(globalTargetInfo.Thing.Position, globalTargetInfo.Thing.Map, ThingDefOf.Filth_CorpseBile, 1, 0, true);
						ThingCompUtility.TryGetComp<CompRottable>(globalTargetInfo.Thing).RotProgress += 60000f;
					}
				}
			});
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007A54 File Offset: 0x00005C54
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			if (!this.pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_BodiesConsumed, false))
			{
				this.pawn.health.AddHediff(VPE_DefOf.VPE_BodiesConsumed, null, null, null);
			}
			Hediff_BodiesConsumed hediff_BodiesConsumed = this.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_BodiesConsumed, false) as Hediff_BodiesConsumed;
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				MoteBetween moteBetween = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_SoulOrbTransfer, null);
				moteBetween.Attach(globalTargetInfo.Thing, this.pawn);
				moteBetween.exactPosition = globalTargetInfo.Thing.DrawPos;
				GenSpawn.Spawn(moteBetween, globalTargetInfo.Thing.Position, this.pawn.Map, 0);
				hediff_BodiesConsumed.consumedBodies++;
				globalTargetInfo.Thing.Destroy(0);
			}
		}
	}
}
