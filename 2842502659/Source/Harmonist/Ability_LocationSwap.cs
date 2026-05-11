using System;
using RimWorld.Planet;
using VanillaPsycastsExpanded.Skipmaster;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x02000126 RID: 294
	public class Ability_LocationSwap : Ability_Teleport
	{
		// Token: 0x0600043F RID: 1087 RVA: 0x00019BDC File Offset: 0x00017DDC
		public override void ModifyTargets(ref GlobalTargetInfo[] targets)
		{
			targets = new GlobalTargetInfo[]
			{
				targets[0],
				new GlobalTargetInfo(this.pawn.Position, this.pawn.Map, false),
				this.pawn,
				new GlobalTargetInfo(targets[0].Cell, targets[0].Map, false)
			};
		}
	}
}
