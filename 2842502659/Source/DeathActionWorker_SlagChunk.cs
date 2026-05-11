using System;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200007D RID: 125
	public class DeathActionWorker_SlagChunk : DeathActionWorker
	{
		// Token: 0x06000179 RID: 377 RVA: 0x0000855D File Offset: 0x0000675D
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			if (corpse.Map != null)
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.ChunkSlagSteel, null), corpse.Position, corpse.Map, 0);
				corpse.Destroy(0);
			}
		}
	}
}
