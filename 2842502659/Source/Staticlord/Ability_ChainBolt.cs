using System;
using RimWorld;
using UnityEngine;
using VEF.Abilities;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x020000FE RID: 254
	public class Ability_ChainBolt : Ability_ShootProjectile
	{
		// Token: 0x06000387 RID: 903 RVA: 0x00015D3B File Offset: 0x00013F3B
		public override float GetPowerForPawn()
		{
			return this.def.power + (float)Mathf.FloorToInt((StatExtension.GetStatValue(this.pawn, StatDefOf.PsychicSensitivity, true, -1) - 1f) * 4f);
		}
	}
}
