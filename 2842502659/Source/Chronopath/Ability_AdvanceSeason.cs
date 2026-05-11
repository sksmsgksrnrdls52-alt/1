using System;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000132 RID: 306
	public class Ability_AdvanceSeason : Ability
	{
		// Token: 0x06000467 RID: 1127 RVA: 0x0001AF4E File Offset: 0x0001914E
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			this.ticksAdvanceLeft = Mathf.CeilToInt(360f);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001AF67 File Offset: 0x00019167
		public override void Tick()
		{
			base.Tick();
			if (this.ticksAdvanceLeft > 0)
			{
				this.ticksAdvanceLeft--;
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 2500);
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001AFA0 File Offset: 0x000191A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksAdvanceLeft, "ticksAdvanceLeft", 0, false);
		}

		// Token: 0x040001DE RID: 478
		private int ticksAdvanceLeft;
	}
}
