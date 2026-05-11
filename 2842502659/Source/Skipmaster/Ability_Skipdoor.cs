using System;
using RimWorld.Planet;
using VEF.Abilities;
using VEF.Buildings;
using Verse;

namespace VanillaPsycastsExpanded.Skipmaster
{
	// Token: 0x02000118 RID: 280
	public class Ability_Skipdoor : Ability
	{
		// Token: 0x060003FE RID: 1022 RVA: 0x00018B78 File Offset: 0x00016D78
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Skipdoor skipdoor = (Skipdoor)ThingMaker.MakeThing(VPE_DefOf.VPE_Skipdoor, null);
				skipdoor.Pawn = this.pawn;
				Find.WindowStack.Add(new Dialog_RenameDoorTeleporter(skipdoor));
				GenSpawn.Spawn(skipdoor, globalTargetInfo.Cell, this.pawn.Map, 0);
			}
		}
	}
}
