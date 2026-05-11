using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000093 RID: 147
	public class Ability_NeuralHeatDetonation : Ability
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x00009D6A File Offset: 0x00007F6A
		public override float GetRadiusForPawn()
		{
			return Mathf.Min(this.pawn.psychicEntropy.EntropyValue / 10f * base.GetRadiusForPawn(), GenRadial.MaxRadialPatternRadius);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009D94 File Offset: 0x00007F94
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			float radiusForPawn = this.GetRadiusForPawn();
			this.pawn.psychicEntropy.RemoveAllEntropy();
			Ability.MakeStaticFleck(targets[0].Cell, targets[0].Thing.Map, FleckDefOf.PsycastAreaEffect, radiusForPawn, 0f);
			IntVec3 cell = targets[0].Cell;
			Map map = this.pawn.Map;
			float num = radiusForPawn;
			DamageDef flame = DamageDefOf.Flame;
			Thing pawn = this.pawn;
			int num2 = -1;
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
