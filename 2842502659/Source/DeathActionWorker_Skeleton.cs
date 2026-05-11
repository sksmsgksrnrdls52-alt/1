using System;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000036 RID: 54
	public class DeathActionWorker_Skeleton : DeathActionWorker
	{
		// Token: 0x06000098 RID: 152 RVA: 0x00004745 File Offset: 0x00002945
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			FilthMaker.TryMakeFilth(corpse.Position, corpse.Map, ThingDefOf.Filth_CorpseBile, 3, 0, true);
			corpse.Destroy(0);
		}
	}
}
