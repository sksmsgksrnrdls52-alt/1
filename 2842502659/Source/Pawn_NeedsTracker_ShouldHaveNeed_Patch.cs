using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000049 RID: 73
	[HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
	public class Pawn_NeedsTracker_ShouldHaveNeed_Patch
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x000055D0 File Offset: 0x000037D0
		private static bool Prefix(NeedDef nd, Pawn ___pawn)
		{
			try
			{
				if (nd == NeedDefOf.Rest || nd == VPE_DefOf.Joy)
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
					if (flag2.GetValueOrDefault())
					{
						return false;
					}
				}
			}
			catch
			{
			}
			return true;
		}
	}
}
