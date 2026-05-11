using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x02000127 RID: 295
	public class Ability_LuckTransfer : Ability
	{
		// Token: 0x06000441 RID: 1089 RVA: 0x00019C64 File Offset: 0x00017E64
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Pawn pawn = targets[0].Thing as Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = targets[1].Thing as Pawn;
				if (pawn2 != null)
				{
					MoteBetween moteBetween = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_PsycastPsychicEffectTransfer, null);
					moteBetween.Attach(pawn, pawn2);
					moteBetween.Scale = 1f;
					moteBetween.exactPosition = pawn.DrawPos;
					GenSpawn.Spawn(moteBetween, pawn.Position, pawn.MapHeld, 0);
					MoteBetween moteBetween2 = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_PsycastPsychicEffectTransfer, null);
					moteBetween2.Attach(pawn2, pawn);
					moteBetween2.Scale = 1f;
					moteBetween2.exactPosition = pawn2.DrawPos;
					GenSpawn.Spawn(moteBetween2, pawn2.Position, pawn2.MapHeld, 0);
					int ticksToDisappear = Mathf.RoundToInt((float)this.GetDurationForPawn() * StatExtension.GetStatValue(pawn, StatDefOf.PsychicSensitivity, true, -1));
					HediffUtility.TryGetComp<HediffComp_Disappears>(pawn.health.AddHediff(VPE_DefOf.VPE_Lucky, null, null, null)).ticksToDisappear = ticksToDisappear;
					HediffUtility.TryGetComp<HediffComp_Disappears>(pawn2.health.AddHediff(VPE_DefOf.VPE_UnLucky, null, null, null)).ticksToDisappear = ticksToDisappear;
					return;
				}
			}
		}
	}
}
