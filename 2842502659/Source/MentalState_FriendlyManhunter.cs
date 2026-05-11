using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000057 RID: 87
	public class MentalState_FriendlyManhunter : MentalState_Manhunter
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x0000613A File Offset: 0x0000433A
		public override bool ForceHostileTo(Faction f)
		{
			return this.pawn.Faction != f;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000614D File Offset: 0x0000434D
		public override bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006150 File Offset: 0x00004350
		public override RandomSocialMode SocialModeMax()
		{
			return 0;
		}
	}
}
