using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000018 RID: 24
	public class RaidStrategyWorker_ImmediateAttack_Psycasters : RaidStrategyWorker_ImmediateAttack
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00002AB1 File Offset: 0x00000CB1
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return this.PawnGenOptionsWithRequiredPawns(parms.faction, groupKind).Any<PawnGroupMaker>() && base.CanUseWith(parms, groupKind);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002AD1 File Offset: 0x00000CD1
		protected bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.HasModExtension<PawnKindAbilityExtension_Psycasts>();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002AD9 File Offset: 0x00000CD9
		protected int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null)
		{
			return 1;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002ADC File Offset: 0x00000CDC
		public override float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return Mathf.Max(base.MinimumPoints(faction, groupKind), this.CheapestRequiredPawnCost(faction, groupKind));
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002AF3 File Offset: 0x00000CF3
		public override float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return this.CheapestRequiredPawnCost(faction, groupKind);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002B00 File Offset: 0x00000D00
		private float CheapestRequiredPawnCost(Faction faction, PawnGroupKindDef groupKind)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithRequiredPawns(faction, groupKind);
			if (!enumerable.Any<PawnGroupMaker>())
			{
				string[] array = new string[6];
				array[0] = "Tried to get MinimumPoints for ";
				int num = 1;
				Type type = base.GetType();
				array[num] = ((type != null) ? type.ToString() : null);
				array[2] = " for faction ";
				array[3] = ((faction != null) ? faction.ToString() : null);
				array[4] = " but the faction has no groups with the required pawn kind. groupKind=";
				array[5] = ((groupKind != null) ? groupKind.ToString() : null);
				Log.Error(string.Concat(array));
				return 99999f;
			}
			float num2 = 9999999f;
			foreach (PawnGroupMaker pawnGroupMaker in enumerable)
			{
				foreach (PawnGenOption pawnGenOption in from op in pawnGroupMaker.options
				where this.MatchesRequiredPawnKind(op.kind)
				select op)
				{
					if (pawnGenOption.Cost < num2)
					{
						num2 = pawnGenOption.Cost;
					}
				}
			}
			return num2;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002C14 File Offset: 0x00000E14
		public override bool CanUsePawnGenOption(float pointsTotal, PawnGenOption g, List<PawnGenOptionWithXenotype> chosenGroups, Faction faction = null)
		{
			return chosenGroups == null || chosenGroups.Count >= this.MinRequiredPawnsForPoints(pointsTotal, faction) || this.MatchesRequiredPawnKind(g.kind);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002C3C File Offset: 0x00000E3C
		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithRequiredPawns(Faction faction, PawnGroupKindDef groupKind)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return Enumerable.Empty<PawnGroupMaker>();
			}
			Predicate<PawnGenOption> <>9__1;
			return faction.def.pawnGroupMakers.Where(delegate(PawnGroupMaker gm)
			{
				if (gm.kindDef == groupKind && gm.options != null)
				{
					List<PawnGenOption> options = gm.options;
					Predicate<PawnGenOption> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((PawnGenOption op) => this.MatchesRequiredPawnKind(op.kind)));
					}
					return GenCollection.Any<PawnGenOption>(options, predicate);
				}
				return false;
			});
		}
	}
}
