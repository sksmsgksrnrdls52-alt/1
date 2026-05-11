using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200006E RID: 110
	public class Ability_ParalysisPulse : Ability
	{
		// Token: 0x0600014D RID: 333 RVA: 0x00007B66 File Offset: 0x00005D66
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			MoteMaker.MakeAttachedOverlay(this.pawn, VPE_DefOf.VPE_Mote_ParalysisPulse, Vector3.zero, this.GetRadiusForPawn(), -1f);
		}
	}
}
