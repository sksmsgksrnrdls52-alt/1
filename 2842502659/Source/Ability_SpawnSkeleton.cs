using System;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000034 RID: 52
	public class Ability_SpawnSkeleton : Ability_TargetCorpse
	{
		// Token: 0x06000094 RID: 148 RVA: 0x00004620 File Offset: 0x00002820
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Corpse corpse = globalTargetInfo.Thing as Corpse;
				IntVec3 position = corpse.Position;
				corpse.Destroy(0);
				FilthMaker.TryMakeFilth(position, this.pawn.Map, ThingDefOf.Filth_CorpseBile, 3, 0, true);
				GenSpawn.Spawn(PawnGenerator.GeneratePawn(VPE_DefOf.VPE_SummonedSkeleton, this.pawn.Faction, null), position, this.pawn.Map, 0);
			}
		}
	}
}
