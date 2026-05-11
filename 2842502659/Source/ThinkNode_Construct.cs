using System;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000077 RID: 119
	public class ThinkNode_Construct : ThinkNode_Conditional
	{
		// Token: 0x06000168 RID: 360 RVA: 0x000082AD File Offset: 0x000064AD
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.def == VPE_DefOf.VPE_Race_RockConstruct;
		}
	}
}
