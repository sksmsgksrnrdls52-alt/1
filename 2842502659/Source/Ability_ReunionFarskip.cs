using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000AB RID: 171
	public class Ability_ReunionFarskip : Ability
	{
		// Token: 0x0600021C RID: 540 RVA: 0x0000C0B0 File Offset: 0x0000A2B0
		public override void PreWarmupAction()
		{
			base.PreWarmupAction();
			Map map = this.pawn.Map;
			Mote item = this.SpawnMote(map, VPE_DefOf.VPE_Mote_GreenMist, this.pawn.Position.ToVector3Shifted(), 10f, 20f);
			this.maintainedMotes = new List<Mote>();
			this.maintainedMotes.Add(item);
			List<IntVec3> list = (from x in GenRadial.RadialCellsAround(this.pawn.Position, 3f, true)
			where GenGrid.InBounds(x, map)
			select x).ToList<IntVec3>();
			for (int i = 0; i < 5; i++)
			{
				if (GenCollection.Any<IntVec3>(list))
				{
					IntVec3 item2 = GenCollection.RandomElement<IntVec3>(list);
					list.Remove(item2);
					Mote item3 = this.SpawnMote(map, ThingDef.Named("VPE_Mote_Ghost" + GenCollection.RandomElement<char>("ABCDEFG").ToString()), item2.ToVector3Shifted(), 1f, 0f);
					this.maintainedMotes.Add(item3);
				}
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
		public override void WarmupToil(Toil toil)
		{
			base.WarmupToil(toil);
			toil.AddPreTickAction(delegate()
			{
				foreach (Mote mote in this.maintainedMotes)
				{
					mote.Maintain();
				}
			});
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000C1DF File Offset: 0x0000A3DF
		public List<Pawn> GetLivingFamilyMembers(Pawn pawn)
		{
			return (from x in pawn.relations.FamilyByBlood
			where !x.Dead
			select x).ToList<Pawn>();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000C218 File Offset: 0x0000A418
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Pawn pawn = targets[0].Thing as Pawn;
			List<Pawn> livingFamilyMembers = this.GetLivingFamilyMembers(pawn);
			List<IntVec3> list = (from x in GenRadial.RadialCellsAround(this.pawn.Position, 3f, true)
			where GenGrid.InBounds(x, this.pawn.Map) && GenGrid.Walkable(x, this.pawn.Map)
			select x).ToList<IntVec3>();
			foreach (Pawn pawn2 in livingFamilyMembers)
			{
				GenSpawn.Spawn(pawn2, GenCollection.RandomElement<IntVec3>(list), this.pawn.Map, 0);
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.PsychicShock, pawn2, null);
				BodyPartRecord bodyPartRecord = null;
				GenCollection.TryRandomElement<BodyPartRecord>(pawn2.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.ConsciousnessSource), ref bodyPartRecord);
				pawn2.health.AddHediff(hediff, bodyPartRecord, null, null);
			}
			foreach (Faction faction in (from x in livingFamilyMembers
			select x.Faction into x
			where x != null
			select x).Distinct<Faction>())
			{
				faction.TryAffectGoodwillWith(this.pawn.Faction, -10, true, true, null, null);
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null && pawn.relations != null && !GenCollection.Any<Pawn>(this.GetLivingFamilyMembers(pawn)))
			{
				if (showMessages)
				{
					Messages.Message(TranslatorFormattedStringExtensions.Translate("VPE.MustHaveLivingFamilyMembers", NamedArgumentUtility.Named(pawn, "PAWN")), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return base.ValidateTarget(target, showMessages);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000C420 File Offset: 0x0000A620
		public Mote SpawnMote(Map map, ThingDef moteDef, Vector3 loc, float scale, float rotationRate)
		{
			Mote mote = MoteMaker.MakeStaticMote(loc, map, moteDef, scale, false, 0f);
			mote.rotationRate = rotationRate;
			if (mote.def.mote.needsMaintenance)
			{
				mote.Maintain();
			}
			return mote;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000C45F File Offset: 0x0000A65F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Mote>(ref this.maintainedMotes, "maintainedMotes", 3, Array.Empty<object>());
		}

		// Token: 0x0400009F RID: 159
		private List<Mote> maintainedMotes = new List<Mote>();
	}
}
