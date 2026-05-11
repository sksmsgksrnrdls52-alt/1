using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Skipmaster
{
	// Token: 0x02000112 RID: 274
	public class Ability_Chunkskip : Ability
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x00017FA4 File Offset: 0x000161A4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				AbilityExtension_Clamor modExtension = this.def.GetModExtension<AbilityExtension_Clamor>();
				foreach (Thing thing in this.FindClosestChunks(globalTargetInfo.HasThing ? new LocalTargetInfo(globalTargetInfo.Thing) : new LocalTargetInfo(globalTargetInfo.Cell)))
				{
					IntVec3 intVec;
					if (this.FindFreeCell(globalTargetInfo.Cell, this.pawn.Map, out intVec))
					{
						AbilityUtility.DoClamor(thing.Position, (float)modExtension.clamorRadius, this.pawn, modExtension.clamorType);
						AbilityUtility.DoClamor(intVec, (float)modExtension.clamorRadius, this.pawn, modExtension.clamorType);
						base.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(thing.Position, globalTargetInfo.Map, 0.72f), thing.Position, 60, null);
						base.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(intVec, globalTargetInfo.Map, 0.72f), intVec, 60, null);
						FleckMaker.ThrowDustPuffThick(intVec.ToVector3(), globalTargetInfo.Map, Rand.Range(1.5f, 3f), CompAbilityEffect_Chunkskip.DustColor);
						thing.Position = intVec;
					}
				}
				SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Pulse, new TargetInfo(globalTargetInfo.Cell, this.pawn.Map, false));
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0001814C File Offset: 0x0001634C
		public override void WarmupToil(Toil toil)
		{
			base.WarmupToil(toil);
			toil.AddPreTickAction(delegate()
			{
				if (this.pawn.jobs.curDriver.ticksLeftThisToil == 5)
				{
					foreach (Thing thing in this.FindClosestChunks(this.pawn.jobs.curJob.targetA))
					{
						FleckMaker.Static(GenThing.TrueCenter(thing), this.pawn.Map, FleckDefOf.PsycastSkipFlashEntry, 0.72f);
					}
				}
			});
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00018168 File Offset: 0x00016368
		private IEnumerable<Thing> FindClosestChunks(LocalTargetInfo target)
		{
			HashSet<Thing> foundChunks;
			if (this.foundChunksCache.TryGetValue(target, out foundChunks))
			{
				return foundChunks;
			}
			foundChunks = new HashSet<Thing>();
			RegionTraverser.BreadthFirstTraverse(target.Cell, this.pawn.Map, (Region from, Region to) => true, delegate(Region x)
			{
				List<Thing> list = x.ListerThings.ThingsInGroup(61);
				int num = 0;
				while (num < list.Count && (float)foundChunks.Count < this.GetPowerForPawn())
				{
					Thing thing = list[num];
					if (!GridsUtility.Fogged(thing) && !foundChunks.Contains(thing))
					{
						foundChunks.Add(thing);
					}
					num++;
				}
				return (float)foundChunks.Count >= this.GetPowerForPawn();
			}, 999999, 15);
			this.foundChunksCache.Add(target, foundChunks);
			return foundChunks;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001820C File Offset: 0x0001640C
		private bool FindFreeCell(IntVec3 target, Map map, out IntVec3 result)
		{
			return CellFinder.TryFindRandomCellNear(target, map, Mathf.RoundToInt(this.GetRadiusForPawn()) - 1, (IntVec3 cell) => CompAbilityEffect_WithDest.CanTeleportThingTo(cell, map) && GenSight.LineOfSight(cell, target, map, true, null, 0, 0), ref result, -1);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001825C File Offset: 0x0001645C
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlight(target);
			foreach (Thing thing in this.FindClosestChunks(target))
			{
				GenDraw.DrawLineBetween(GenThing.TrueCenter(thing), target.CenterVector3);
				GenDraw.DrawTargetHighlight(thing);
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000182C8 File Offset: 0x000164C8
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!GenGrid.Standable(target.Cell, this.pawn.Map))
			{
				return false;
			}
			if (GridsUtility.Filled(target.Cell, this.pawn.Map))
			{
				return false;
			}
			if (!this.FindClosestChunks(target).Any<Thing>())
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.NoChunks"), this.pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			IntVec3 intVec;
			if (!this.FindFreeCell(target.Cell, this.pawn.Map, out intVec))
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("AbilityNotEnoughFreeSpace"), this.pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return base.ValidateTarget(target, showMessages);
		}

		// Token: 0x040001C3 RID: 451
		private readonly Dictionary<LocalTargetInfo, HashSet<Thing>> foundChunksCache = new Dictionary<LocalTargetInfo, HashSet<Thing>>();
	}
}
