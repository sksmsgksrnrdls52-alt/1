using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000094 RID: 148
	public class Ability_AggressiveHeatDump : Ability
	{
		// Token: 0x060001BA RID: 442 RVA: 0x00009E8E File Offset: 0x0000808E
		public override float GetRadiusForPawn()
		{
			return Mathf.Min(new float[]
			{
				this.pawn.psychicEntropy.EntropyValue / 20f,
				9f * base.GetRadiusForPawn(),
				GenRadial.MaxRadialPatternRadius
			});
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00009ECB File Offset: 0x000080CB
		public override float GetPowerForPawn()
		{
			return this.pawn.psychicEntropy.EntropyValue * base.GetPowerForPawn();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00009EE4 File Offset: 0x000080E4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			float radiusForPawn = this.GetRadiusForPawn();
			float powerForPawn = this.GetPowerForPawn();
			this.pawn.psychicEntropy.RemoveAllEntropy();
			Ability.MakeStaticFleck(targets[0].Cell, targets[0].Thing.Map, FleckDefOf.PsycastAreaEffect, radiusForPawn, 0f);
			Ability.MakeStaticFleck(targets[0].Cell, targets[0].Thing.Map, VPE_DefOf.VPE_AggresiveHeatDump, radiusForPawn, 0f);
			IntVec3 cell = targets[0].Cell;
			Map map = this.pawn.Map;
			float num = radiusForPawn;
			DamageDef flame = DamageDefOf.Flame;
			Thing pawn = this.pawn;
			int num2 = (int)powerForPawn;
			float num3 = -1f;
			SoundDef soundDef = null;
			ThingDef thingDef = null;
			ThingDef thingDef2 = null;
			Thing thing = null;
			ThingDef thingDef3 = null;
			float num4 = 0f;
			int num5 = 1;
			List<Thing> list = new List<Thing>
			{
				this.pawn
			};
			GenExplosion.DoExplosion(cell, map, num, flame, pawn, num2, num3, soundDef, thingDef, thingDef2, thing, thingDef3, num4, num5, null, null, 255, false, null, 0f, 1, 0f, false, null, list, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}
	}
}
