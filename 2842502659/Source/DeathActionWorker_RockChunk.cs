using System;
using Verse;
using Verse.AI.Group;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200007E RID: 126
	public class DeathActionWorker_RockChunk : DeathActionWorker
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00008594 File Offset: 0x00006794
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			if (corpse.Map != null)
			{
				Pawn innerPawn = corpse.InnerPawn;
				ThingDef thingDef;
				if (innerPawn == null)
				{
					thingDef = null;
				}
				else
				{
					CompSetStoneColour compSetStoneColour = ThingCompUtility.TryGetComp<CompSetStoneColour>(innerPawn);
					thingDef = ((compSetStoneColour != null) ? compSetStoneColour.KilledLeave : null);
				}
				ThingDef thingDef2 = thingDef;
				if (thingDef2 != null)
				{
					GenSpawn.Spawn(ThingMaker.MakeThing(thingDef2, null), corpse.Position, corpse.Map, 0);
					corpse.Destroy(0);
				}
			}
		}
	}
}
