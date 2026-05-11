using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Skipmaster
{
	// Token: 0x02000114 RID: 276
	public class Ability_Smokepop : Ability
	{
		// Token: 0x060003EB RID: 1003 RVA: 0x00018440 File Offset: 0x00016640
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				GenExplosion.DoExplosion(globalTargetInfo.Cell, globalTargetInfo.Map, this.GetRadiusForPawn(), DamageDefOf.Smoke, this.pawn, -1, -1f, null, null, null, null, null, 0f, 1, new GasType?(0), null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			}
		}
	}
}
