using System;
using System.Linq;
using RimWorld;
using VEF.Abilities;
using Verse;
using Verse.AI.Group;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x0200012D RID: 301
	public class HediffComp_MindControl : HediffComp_Ability
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x0001A8F0 File Offset: 0x00018AF0
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.oldFaction = base.Pawn.Faction;
			this.oldLord = LordUtility.GetLord(base.Pawn);
			Lord lord = this.oldLord;
			if (lord != null)
			{
				lord.RemovePawn(base.Pawn);
			}
			base.Pawn.SetFaction(this.ability.pawn.Faction, this.ability.pawn);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001A964 File Offset: 0x00018B64
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			base.Pawn.SetFaction(this.oldFaction, null);
			if (!this.oldFaction.IsPlayer)
			{
				Lord lord = this.oldLord;
				if (lord == null || !lord.AnyActivePawn)
				{
					if (GenCollection.Except<Pawn>(base.Pawn.Map.mapPawns.SpawnedPawnsInFaction(this.oldFaction), base.Pawn).Any<Pawn>())
					{
						this.oldLord = LordUtility.GetLord((Pawn)GenClosest.ClosestThing_Global(base.Pawn.Position, base.Pawn.Map.mapPawns.SpawnedPawnsInFaction(this.oldFaction), 99999f, (Thing p) => p != base.Pawn && LordUtility.GetLord((Pawn)p) != null, null, false));
					}
					if (this.oldLord == null)
					{
						LordJob_DefendPoint lordJob_DefendPoint = new LordJob_DefendPoint(base.Pawn.Position, null, null, false, true);
						this.oldLord = LordMaker.MakeNewLord(this.oldFaction, lordJob_DefendPoint, base.Pawn.Map, null);
					}
				}
			}
			Lord lord2 = this.oldLord;
			if (lord2 == null)
			{
				return;
			}
			lord2.AddPawn(base.Pawn);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001AA8A File Offset: 0x00018C8A
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Faction>(ref this.oldFaction, "oldFaction", false);
			Scribe_References.Look<Lord>(ref this.oldLord, "oldLord", false);
		}

		// Token: 0x040001DA RID: 474
		private Faction oldFaction;

		// Token: 0x040001DB RID: 475
		private Lord oldLord;
	}
}
