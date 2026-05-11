using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x020000FF RID: 255
	public class Ability_Flashstorm : Ability
	{
		// Token: 0x06000389 RID: 905 RVA: 0x00015D78 File Offset: 0x00013F78
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo target in targets)
			{
				Map map = target.Map;
				Thing conditionCauser = GenSpawn.Spawn(ThingDefOf.Flashstorm, target.Cell, this.pawn.Map, 0);
				GameCondition_PsychicFlashstorm gameCondition_PsychicFlashstorm = (GameCondition_PsychicFlashstorm)GameConditionMaker.MakeCondition(VPE_DefOf.VPE_PsychicFlashstorm, -1);
				gameCondition_PsychicFlashstorm.centerLocation = target.Cell.ToIntVec2;
				gameCondition_PsychicFlashstorm.areaRadiusOverride = new IntRange(Mathf.RoundToInt(this.GetRadiusForPawn()), Mathf.RoundToInt(this.GetRadiusForPawn()));
				gameCondition_PsychicFlashstorm.Duration = this.GetDurationForPawn();
				gameCondition_PsychicFlashstorm.suppressEndMessage = true;
				gameCondition_PsychicFlashstorm.initialStrikeDelay = new IntRange(0, 0);
				gameCondition_PsychicFlashstorm.conditionCauser = conditionCauser;
				gameCondition_PsychicFlashstorm.ambientSound = true;
				gameCondition_PsychicFlashstorm.numStrikes = Mathf.FloorToInt(this.GetPowerForPawn());
				map.gameConditionManager.RegisterCondition(gameCondition_PsychicFlashstorm);
				this.ApplyGoodwillImpact(target, gameCondition_PsychicFlashstorm.AreaRadius);
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00015E7C File Offset: 0x0001407C
		private void ApplyGoodwillImpact(GlobalTargetInfo target, int radius)
		{
			if (this.pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.affectedFactionCache.Clear();
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(target.Cell, target.Map, (float)radius, true))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && thing.Faction != null && thing.Faction != this.pawn.Faction && !FactionUtility.HostileTo(thing.Faction, this.pawn.Faction) && !this.affectedFactionCache.Contains(thing.Faction) && (this.def.applyGoodwillImpactToLodgers || !QuestUtility.IsQuestLodger(pawn)))
				{
					this.affectedFactionCache.Add(thing.Faction);
					Faction.OfPlayer.TryAffectGoodwillWith(thing.Faction, this.def.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			this.affectedFactionCache.Clear();
		}

		// Token: 0x040001AB RID: 427
		private readonly HashSet<Faction> affectedFactionCache = new HashSet<Faction>();
	}
}
