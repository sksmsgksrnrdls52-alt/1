using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000139 RID: 313
	public class Ability_TimeQuake : Ability
	{
		// Token: 0x06000482 RID: 1154 RVA: 0x0001BB24 File Offset: 0x00019D24
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			GameCondition_TimeQuake gameCondition_TimeQuake = (GameCondition_TimeQuake)GameConditionMaker.MakeCondition(VPE_DefOf.VPE_TimeQuake, this.GetDurationForPawn());
			gameCondition_TimeQuake.SafeRadius = this.GetRadiusForPawn();
			gameCondition_TimeQuake.Pawn = this.pawn;
			ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(VPE_DefOf.VPE_SkyChanger, null);
			GenSpawn.Spawn(thingWithComps, this.pawn.Position, this.pawn.Map, 0);
			ThingCompUtility.TryGetComp<CompAffectsSky>(thingWithComps).StartFadeInHoldFadeOut(0, this.GetDurationForPawn(), 0, 1f);
			this.pawn.Map.gameConditionManager.RegisterCondition(gameCondition_TimeQuake);
			foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
			{
				if (faction.CanChangeGoodwillFor(this.pawn.Faction, -10))
				{
					faction.TryAffectGoodwillWith(this.pawn.Faction, -10, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
				if (faction.CanChangeGoodwillFor(this.pawn.Faction, -75) && this.pawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Count > 0)
				{
					faction.TryAffectGoodwillWith(this.pawn.Faction, -75, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
		}
	}
}
