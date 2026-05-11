using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000131 RID: 305
	public class AbilityExtension_ReduceResistance : AbilityExtension_AbilityMod
	{
		// Token: 0x06000465 RID: 1125 RVA: 0x0001AEBC File Offset: 0x000190BC
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					Faction hostFaction = pawn.HostFaction;
					if (hostFaction != null && !(pawn.GuestStatus != 1) && hostFaction == ability.pawn.Faction)
					{
						pawn.guest.resistance -= 20f;
					}
				}
			}
		}
	}
}
