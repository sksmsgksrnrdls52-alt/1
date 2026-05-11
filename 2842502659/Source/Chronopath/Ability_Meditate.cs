using System;
using RimWorld.Planet;
using VEF.Abilities;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000135 RID: 309
	public class Ability_Meditate : Ability
	{
		// Token: 0x06000471 RID: 1137 RVA: 0x0001B394 File Offset: 0x00019594
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			this.pawn.psychicEntropy.OffsetPsyfocusDirectly(1f - this.pawn.psychicEntropy.CurrentPsyfocus);
			this.pawn.Psycasts().GainExperience(300f, true);
		}
	}
}
