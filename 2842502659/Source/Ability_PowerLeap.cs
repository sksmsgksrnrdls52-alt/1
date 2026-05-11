using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B9 RID: 185
	public class Ability_PowerLeap : Ability
	{
		// Token: 0x06000264 RID: 612 RVA: 0x0000DAAC File Offset: 0x0000BCAC
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			Map map = base.Caster.Map;
			JumpingPawn jumpingPawn = (JumpingPawn)PawnFlyer.MakeFlyer(VPE_DefOf.VPE_JumpingPawn, base.CasterPawn, targets[0].Cell, null, null, false, null, null, default(LocalTargetInfo));
			jumpingPawn.ability = this;
			GenSpawn.Spawn(jumpingPawn, base.Caster.Position, map, 0);
			base.Cast(targets);
		}
	}
}
