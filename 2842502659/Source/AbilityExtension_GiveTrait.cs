using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000058 RID: 88
	public class AbilityExtension_GiveTrait : AbilityExtension_AbilityMod
	{
		// Token: 0x060000FA RID: 250 RVA: 0x0000615C File Offset: 0x0000435C
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				Pawn_StoryTracker story = pawn.story;
				bool? flag;
				if (story == null)
				{
					flag = null;
				}
				else
				{
					TraitSet traits = story.traits;
					flag = ((traits != null) ? new bool?(!traits.HasTrait(this.trait, this.degree)) : null);
				}
				bool? flag2 = flag;
				if (flag2.GetValueOrDefault())
				{
					pawn.story.traits.GainTrait(new Trait(this.trait, this.degree, false), true);
					pawn.needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
		}

		// Token: 0x04000046 RID: 70
		public TraitDef trait;

		// Token: 0x04000047 RID: 71
		public int degree;
	}
}
