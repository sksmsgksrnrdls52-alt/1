using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000DC RID: 220
	public class Ability_ShrineShield : Ability
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x0001312C File Offset: 0x0001132C
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Map map = targets[0].Map;
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.NatureShrine_Small))
			{
				Ability_Spawn.Spawn(thing, VPE_DefOf.VPE_Shrineshield_Small, this);
			}
			foreach (Thing thing2 in map.listerThings.ThingsOfDef(ThingDefOf.NatureShrine_Large))
			{
				Ability_Spawn.Spawn(thing2, VPE_DefOf.VPE_Shrineshield_Large, this);
			}
		}
	}
}
