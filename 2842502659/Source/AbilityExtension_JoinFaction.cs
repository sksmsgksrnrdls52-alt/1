using System;
using RimWorld.Planet;
using VEF.Abilities;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200000C RID: 12
	public class AbilityExtension_JoinFaction : AbilityExtension_AbilityMod
	{
		// Token: 0x06000027 RID: 39 RVA: 0x0000286C File Offset: 0x00000A6C
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				if (globalTargetInfo.Thing.Faction != ability.pawn.Faction)
				{
					globalTargetInfo.Thing.SetFaction(ability.pawn.Faction, null);
				}
			}
		}
	}
}
