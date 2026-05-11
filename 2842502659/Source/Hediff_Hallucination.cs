using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000030 RID: 48
	public class Hediff_Hallucination : HediffWithComps
	{
		// Token: 0x06000088 RID: 136 RVA: 0x00004270 File Offset: 0x00002470
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			foreach (ThoughtDef thoughtDef in Hediff_Hallucination.thoughtsToChange)
			{
				Thought_Memory firstMemoryOfDef = this.pawn.needs.mood.thoughts.memories.GetFirstMemoryOfDef(thoughtDef);
				if (firstMemoryOfDef != null)
				{
					firstMemoryOfDef.SetForcedStage(thoughtDef.stages.Count - 1);
				}
			}
		}

		// Token: 0x04000021 RID: 33
		public static List<ThoughtDef> thoughtsToChange = new List<ThoughtDef>
		{
			ThoughtDefOf.AteInImpressiveDiningRoom,
			ThoughtDefOf.JoyActivityInImpressiveRecRoom,
			ThoughtDefOf.SleptInBedroom,
			ThoughtDefOf.SleptInBarracks
		};
	}
}
