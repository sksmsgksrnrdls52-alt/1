using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x0200013A RID: 314
	public class ThoughtWorker_TimeQuake : ThoughtWorker
	{
		// Token: 0x06000484 RID: 1156 RVA: 0x0001BC9C File Offset: 0x00019E9C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Map.gameConditionManager.ConditionIsActive(VPE_DefOf.VPE_TimeQuake);
		}
	}
}
