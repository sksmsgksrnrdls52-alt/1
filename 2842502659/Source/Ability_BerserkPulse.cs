using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000062 RID: 98
	public class Ability_BerserkPulse : Ability
	{
		// Token: 0x0600011C RID: 284 RVA: 0x00006BB8 File Offset: 0x00004DB8
		public override void ModifyTargets(ref GlobalTargetInfo[] targets)
		{
			this.targetCell = targets[0].Cell;
			base.ModifyTargets(ref targets);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00006BD4 File Offset: 0x00004DD4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Ability.MakeStaticFleck(this.targetCell, this.pawn.Map, VPE_DefOf.PsycastAreaEffect, this.GetRadiusForPawn(), 0f);
		}

		// Token: 0x0400004A RID: 74
		public IntVec3 targetCell;
	}
}
