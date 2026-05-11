using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000DD RID: 221
	public class Ability_SummonPack : Ability
	{
		// Token: 0x060002FB RID: 763 RVA: 0x00013204 File Offset: 0x00011404
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Map map = targets[0].Map;
			float num = this.GetPowerForPawn();
			List<Pawn> list = new List<Pawn>();
			PawnKindDef pawnKindDef;
			while (num > 0f && AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(num, map.Tile, ref pawnKindDef))
			{
				num -= pawnKindDef.combatPower;
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, null, 2, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, 8, null, null, null, false, false, false, -1, 0, false));
				list.Add(item);
			}
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(ref intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				intVec = CellFinder.RandomEdgeCell(map);
			}
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i];
				GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(intVec, map, 10, null), map, 0);
				pawn.mindState.mentalStateHandler.TryStartMentalState(VPE_DefOf.VPE_ManhunterTerritorial, null, false, false, false, null, false, false, false);
				pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + Rand.Range(25000, 35000);
			}
			Find.LetterStack.ReceiveLetter(Translator.Translate("VPE.PackSummon"), TranslatorFormattedStringExtensions.Translate("VPE.PackSummon.Desc", this.pawn.NameShortColored), LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), null, null, null, null, 0, true);
		}
	}
}
