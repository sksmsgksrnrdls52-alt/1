using System;
using System.Collections.Generic;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000066 RID: 102
	public class Ability_IceWall : Ability
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00006E30 File Offset: 0x00005030
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			IEnumerable<IntVec3> enumerable = GenRadial.RadialCellsAround(targets[0].Cell, 5f, 5.9f);
			AbilityExtension_Building modExtension = this.def.GetModExtension<AbilityExtension_Building>();
			foreach (IntVec3 intVec in enumerable)
			{
				if (GridsUtility.GetEdifice(intVec, this.pawn.Map) == null)
				{
					GenSpawn.Spawn(modExtension.building, intVec, this.pawn.Map, 2).SetFactionDirect(this.pawn.Faction);
				}
			}
		}
	}
}
