using System;
using RimWorld;
using RimWorld.Planet;
using VanillaPsycastsExpanded.Skipmaster;
using Verse;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x0200011A RID: 282
	public class Ability_TeleportDark : Ability_Teleport
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00018FBF File Offset: 0x000171BF
		public override FleckDef[] EffectSet
		{
			get
			{
				return new FleckDef[]
				{
					VPE_DefOf.VPE_PsycastSkipFlashEntry_DarkBlue,
					FleckDefOf.PsycastSkipInnerExit,
					FleckDefOf.PsycastSkipOuterRingExit
				};
			}
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00018FE0 File Offset: 0x000171E0
		public override bool CanHitTarget(LocalTargetInfo target)
		{
			return (double)this.pawn.Map.glowGrid.GroundGlowAt(target.Cell, false, false) <= 0.29 && !GridsUtility.Fogged(target.Cell, this.pawn.Map) && GenGrid.Walkable(target.Cell, this.pawn.Map);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00019049 File Offset: 0x00017249
		public override void ModifyTargets(ref GlobalTargetInfo[] targets)
		{
			base.ModifyTargets(ref targets);
			targets = new GlobalTargetInfo[]
			{
				this.pawn,
				targets[0]
			};
		}
	}
}
