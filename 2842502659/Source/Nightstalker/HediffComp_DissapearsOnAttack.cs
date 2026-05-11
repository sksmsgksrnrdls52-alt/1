using System;
using Verse;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x02000122 RID: 290
	public class HediffComp_DissapearsOnAttack : HediffComp
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x00019730 File Offset: 0x00017930
		public override bool CompShouldRemove
		{
			get
			{
				Pawn pawn = base.Pawn;
				Stance stance;
				if (pawn == null)
				{
					stance = null;
				}
				else
				{
					Pawn_StanceTracker stances = pawn.stances;
					stance = ((stances != null) ? stances.curStance : null);
				}
				Stance stance2 = stance;
				Stance_Warmup stance_Warmup = stance2 as Stance_Warmup;
				if (stance_Warmup != null)
				{
					if (stance_Warmup.ticksLeft > 1)
					{
						goto IL_84;
					}
					Verb verb = stance_Warmup.verb;
					if (verb == null)
					{
						goto IL_84;
					}
					VerbProperties verbProps = verb.verbProps;
					if (verbProps == null)
					{
						goto IL_84;
					}
					if (!verbProps.violent)
					{
						goto IL_84;
					}
				}
				else
				{
					Stance_Cooldown stance_Cooldown = stance2 as Stance_Cooldown;
					if (stance_Cooldown == null)
					{
						goto IL_84;
					}
					Verb verb2 = stance_Cooldown.verb;
					if (verb2 == null)
					{
						goto IL_84;
					}
					VerbProperties verbProps2 = verb2.verbProps;
					if (verbProps2 == null || !verbProps2.violent)
					{
						goto IL_84;
					}
				}
				return true;
				IL_84:
				return false;
			}
		}
	}
}
