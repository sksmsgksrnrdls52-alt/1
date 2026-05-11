using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200004A RID: 74
	[HarmonyPatch(typeof(Pawn_SkillTracker), "Learn")]
	public class Pawn_SkillTracker_Learn_Patch
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00005654 File Offset: 0x00003854
		private static bool Prefix(SkillDef sDef, float xp, Pawn ___pawn)
		{
			Pawn_StoryTracker story = ___pawn.story;
			bool? flag;
			if (story == null)
			{
				flag = null;
			}
			else
			{
				TraitSet traits = story.traits;
				flag = ((traits != null) ? new bool?(traits.HasTrait(VPE_DefOf.VPE_Thrall)) : null);
			}
			bool? flag2 = flag;
			return !flag2.GetValueOrDefault() || xp <= 0f;
		}
	}
}
