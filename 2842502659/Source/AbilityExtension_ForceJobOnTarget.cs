using System;
using RimWorld.Planet;
using VEF.Abilities;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B7 RID: 183
	public class AbilityExtension_ForceJobOnTarget : AbilityExtension_ForceJobOnTargetBase
	{
		// Token: 0x0600025A RID: 602 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo target in targets)
			{
				base.ForceJob(target, ability);
			}
		}
	}
}
