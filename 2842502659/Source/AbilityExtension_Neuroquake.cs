using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000091 RID: 145
	public class AbilityExtension_Neuroquake : AbilityExtension_AbilityMod
	{
		// Token: 0x060001AE RID: 430 RVA: 0x000096C0 File Offset: 0x000078C0
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			if (this.affectedFactions == null)
			{
				this.affectedFactions = new Dictionary<Faction, Pair<bool, Pawn>>();
			}
			else
			{
				this.affectedFactions.Clear();
			}
			this.giveMentalStateTo.Clear();
			foreach (Pawn pawn in ability.pawn.Map.mapPawns.AllPawnsSpawned)
			{
				if (this.CanApplyEffects(pawn) && !GridsUtility.Fogged(pawn))
				{
					bool flag = !pawn.Spawned || pawn.Position.InHorDistOf(ability.pawn.Position, ability.GetAdditionalRadius()) || !pawn.Position.InHorDistOf(ability.pawn.Position, ability.GetRadiusForPawn());
					this.AffectGoodwill(pawn.HomeFaction, !flag, pawn);
					if (!flag)
					{
						this.giveMentalStateTo.Add(pawn);
					}
					else
					{
						this.GiveNeuroquakeThought(pawn);
					}
				}
			}
			foreach (Map map in Find.Maps)
			{
				if (map != ability.pawn.Map && Find.WorldGrid.TraversalDistanceBetween(map.Tile, ability.pawn.Map.Tile, true, this.worldRangeTiles + 1, false) <= this.worldRangeTiles)
				{
					foreach (Pawn p in map.mapPawns.AllPawns)
					{
						if (this.CanApplyEffects(p))
						{
							this.GiveNeuroquakeThought(p);
						}
					}
				}
			}
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				if (Find.WorldGrid.TraversalDistanceBetween(caravan.Tile, ability.pawn.Map.Tile, true, this.worldRangeTiles + 1, false) <= this.worldRangeTiles)
				{
					foreach (Pawn p2 in caravan.pawns)
					{
						if (this.CanApplyEffects(p2))
						{
							this.GiveNeuroquakeThought(p2);
						}
					}
				}
			}
			foreach (Pawn pawn2 in this.giveMentalStateTo)
			{
				AbilityExtension_GiveMentalState.TryGiveMentalStateWithDuration(pawn2.RaceProps.IsMechanoid ? MentalStateDefOf.BerserkMechanoid : MentalStateDefOf.Berserk, pawn2, ability, StatDefOf.PsychicSensitivity, false);
				RestUtility.WakeUp(pawn2, true);
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.defeated)
				{
					this.AffectGoodwill(faction, false, null);
				}
			}
			if (ability.pawn.Faction == Faction.OfPlayer)
			{
				foreach (KeyValuePair<Faction, Pair<bool, Pawn>> keyValuePair in this.affectedFactions)
				{
					Faction key = keyValuePair.Key;
					bool first = keyValuePair.Value.First;
					Pawn second = keyValuePair.Value.Second;
					int num = first ? this.goodwillImpactForBerserk : this.goodwillImpactForNeuroquake;
					Faction.OfPlayer.TryAffectGoodwillWith(key, num, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			base.Cast(targets, ability);
			this.affectedFactions.Clear();
			this.giveMentalStateTo.Clear();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00009B14 File Offset: 0x00007D14
		private void AffectGoodwill(Faction faction, bool gaveMentalBreak, Pawn p = null)
		{
			Pair<bool, Pawn> pair;
			if (faction != null && !faction.IsPlayer && !FactionUtility.HostileTo(faction, Faction.OfPlayer) && (p == null || !p.IsSlaveOfColony) && (!this.affectedFactions.TryGetValue(faction, out pair) || (!pair.First && gaveMentalBreak)))
			{
				this.affectedFactions[faction] = new Pair<bool, Pawn>(gaveMentalBreak, p);
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009B75 File Offset: 0x00007D75
		private void GiveNeuroquakeThought(Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			if (needs == null)
			{
				return;
			}
			Need_Mood mood = needs.mood;
			if (mood == null)
			{
				return;
			}
			mood.thoughts.memories.TryGainMemory(ThoughtDefOf.NeuroquakeEcho, null, null);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00009BA2 File Offset: 0x00007DA2
		private bool CanApplyEffects(Pawn p)
		{
			return !p.Dead && !p.Suspended && StatExtension.GetStatValue(p, StatDefOf.PsychicSensitivity, true, -1) > float.Epsilon;
		}

		// Token: 0x04000070 RID: 112
		private Dictionary<Faction, Pair<bool, Pawn>> affectedFactions;

		// Token: 0x04000071 RID: 113
		private List<Pawn> giveMentalStateTo = new List<Pawn>();

		// Token: 0x04000072 RID: 114
		public int goodwillImpactForNeuroquake;

		// Token: 0x04000073 RID: 115
		public int goodwillImpactForBerserk;

		// Token: 0x04000074 RID: 116
		public int worldRangeTiles;
	}
}
