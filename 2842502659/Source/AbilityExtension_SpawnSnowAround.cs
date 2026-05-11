using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000063 RID: 99
	public class AbilityExtension_SpawnSnowAround : AbilityExtension_AbilityMod
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00006C0C File Offset: 0x00004E0C
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				foreach (IntVec3 intVec in GenRadial.RadialCellsAround(globalTargetInfo.Cell, this.radius, true))
				{
					ability.pawn.Map.snowGrid.AddDepth(intVec, this.depth);
				}
			}
		}

		// Token: 0x0400004B RID: 75
		public float radius;

		// Token: 0x0400004C RID: 76
		public float depth;
	}
}
