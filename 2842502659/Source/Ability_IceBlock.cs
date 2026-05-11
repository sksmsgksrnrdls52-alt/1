using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000065 RID: 101
	public class Ability_IceBlock : Ability
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00006D54 File Offset: 0x00004F54
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			List<IntVec3> list = GenCollection.InRandomOrder<IntVec3>(CellRect.CenteredOn(targets[0].Cell, 5, 5).Cells, null).ToList<IntVec3>();
			list = list.Take(list.Count<IntVec3>() - 5).ToList<IntVec3>();
			AbilityExtension_Building modExtension = this.def.GetModExtension<AbilityExtension_Building>();
			foreach (IntVec3 intVec in list)
			{
				if (GridsUtility.GetEdifice(intVec, this.pawn.Map) == null)
				{
					GenSpawn.Spawn(modExtension.building, intVec, this.pawn.Map, 2).SetFactionDirect(this.pawn.Faction);
				}
			}
		}
	}
}
