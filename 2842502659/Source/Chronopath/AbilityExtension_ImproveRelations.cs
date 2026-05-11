using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000130 RID: 304
	public class AbilityExtension_ImproveRelations : AbilityExtension_AbilityMod
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x0001AE08 File Offset: 0x00019008
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Thing thing = globalTargetInfo.Thing;
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					Faction faction = thing.Faction;
					if (faction != null && !faction.IsPlayer && pawn.Faction.RelationKindWith(ability.pawn.Faction) != null && pawn.guest.HostFaction == null)
					{
						pawn.Faction.TryAffectGoodwillWith(ability.pawn.Faction, 20, true, true, VPE_DefOf.VPE_Foretelling, null);
					}
				}
			}
		}
	}
}
