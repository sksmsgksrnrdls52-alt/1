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
	// Token: 0x0200010F RID: 271
	public class Ability_WorldTeleport : Ability
	{
		// Token: 0x060003CD RID: 973 RVA: 0x00017450 File Offset: 0x00015650
		public override void DoAction()
		{
			Pawn pawn = this.PawnsToSkip().FirstOrDefault((Pawn p) => QuestUtility.IsQuestLodger(p));
			if (pawn != null)
			{
				Dialog_MessageBox.CreateConfirmation(TranslatorFormattedStringExtensions.Translate("FarskipConfirmTeleportingLodger", NamedArgumentUtility.Named(pawn, "PAWN")), new Action(base.DoAction), false, null, 1);
				return;
			}
			base.DoAction();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x000174BC File Offset: 0x000156BC
		private IEnumerable<Pawn> PawnsToSkip()
		{
			Ability_WorldTeleport.<PawnsToSkip>d__1 <PawnsToSkip>d__ = new Ability_WorldTeleport.<PawnsToSkip>d__1(-2);
			<PawnsToSkip>d__.<>4__this = this;
			return <PawnsToSkip>d__;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000174CC File Offset: 0x000156CC
		private Pawn AlliedPawnOnMap(Map targetMap)
		{
			return targetMap.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => !WildManUtility.NonHumanlikeOrWildMan(p) && p.IsColonist && p.HomeFaction == Faction.OfPlayer && !this.PawnsToSkip().Contains(p));
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000174EC File Offset: 0x000156EC
		private bool ShouldEnterMap(GlobalTargetInfo target)
		{
			Caravan caravan = target.WorldObject as Caravan;
			if (caravan != null && caravan.Faction == this.pawn.Faction)
			{
				return false;
			}
			MapParent mapParent = target.WorldObject as MapParent;
			return mapParent != null && mapParent.HasMap && (this.AlliedPawnOnMap(mapParent.Map) != null || mapParent.Map == this.pawn.Map);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001755C File Offset: 0x0001575C
		public override bool CanHitTargetTile(GlobalTargetInfo target)
		{
			Caravan caravan = CaravanUtility.GetCaravan(this.pawn);
			if (caravan != null && caravan.ImmobilizedByMass)
			{
				return false;
			}
			Caravan caravan2 = target.WorldObject as Caravan;
			return (caravan == null || caravan != caravan2) && (this.ShouldEnterMap(target) || (caravan2 != null && caravan2.Faction == this.pawn.Faction)) && base.CanHitTargetTile(target);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000175C0 File Offset: 0x000157C0
		public override bool IsEnabledForPawn(out string reason)
		{
			if (!base.IsEnabledForPawn(ref reason))
			{
				return false;
			}
			Caravan caravan = CaravanUtility.GetCaravan(this.pawn);
			if (caravan != null && caravan.ImmobilizedByMass)
			{
				reason = Translator.Translate("CaravanImmobilizedByMass");
				return false;
			}
			return true;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00017603 File Offset: 0x00015803
		public override void WarmupToil(Toil toil)
		{
			base.WarmupToil(toil);
			toil.AddPreTickAction(delegate()
			{
				if (this.pawn.jobs.curDriver.ticksLeftThisToil != 5)
				{
					return;
				}
				foreach (Pawn pawn in this.PawnsToSkip())
				{
					FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawn, FleckDefOf.PsycastSkipFlashEntry, Vector3.zero, 1f, -1f);
					dataAttachedOverlay.link.detachAfterTicks = 5;
					pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
					base.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(this.pawn, this.pawn.Map, 1f), this.pawn.Position, 60, null);
				}
			});
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00017620 File Offset: 0x00015820
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			Ability_WorldTeleport.<>c__DisplayClass7_0 CS$<>8__locals1 = new Ability_WorldTeleport.<>c__DisplayClass7_0();
			Caravan caravan = CaravanUtility.GetCaravan(this.pawn);
			Ability_WorldTeleport.<>c__DisplayClass7_0 CS$<>8__locals2 = CS$<>8__locals1;
			MapParent mapParent = targets[0].WorldObject as MapParent;
			CS$<>8__locals2.targetMap = ((mapParent != null) ? mapParent.Map : null);
			CS$<>8__locals1.targetCell = IntVec3.Invalid;
			List<Pawn> list = this.PawnsToSkip().ToList<Pawn>();
			if (this.pawn.Spawned)
			{
				SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Pulse, new TargetInfo(targets[0].Cell, this.pawn.Map, false));
			}
			if (CS$<>8__locals1.targetMap != null)
			{
				Pawn pawn = this.AlliedPawnOnMap(CS$<>8__locals1.targetMap);
				if (pawn != null)
				{
					IntVec3 position = pawn.Position;
					CS$<>8__locals1.targetCell = position;
				}
			}
			AbilityExtension_Clamor modExtension = this.def.GetModExtension<AbilityExtension_Clamor>();
			if (CS$<>8__locals1.targetCell.IsValid)
			{
				foreach (Pawn pawn2 in list)
				{
					if (pawn2.Spawned)
					{
						pawn2.teleporting = true;
						pawn2.ExitMap(false, Rot4.Invalid);
						AbilityUtility.DoClamor(pawn2.Position, (float)modExtension.clamorRadius, this.pawn, modExtension.clamorType);
						pawn2.teleporting = false;
					}
					IntVec3 targetCell = CS$<>8__locals1.targetCell;
					Map targetMap = CS$<>8__locals1.targetMap;
					int num = 4;
					Predicate<IntVec3> predicate;
					if ((predicate = CS$<>8__locals1.<>9__0) == null)
					{
						predicate = (CS$<>8__locals1.<>9__0 = ((IntVec3 cell) => cell != CS$<>8__locals1.targetCell && GridsUtility.GetRoom(cell, CS$<>8__locals1.targetMap) == GridsUtility.GetRoom(CS$<>8__locals1.targetCell, CS$<>8__locals1.targetMap)));
					}
					IntVec3 intVec;
					CellFinder.TryFindRandomSpawnCellForPawnNear(targetCell, targetMap, ref intVec, num, predicate);
					GenSpawn.Spawn(pawn2, intVec, CS$<>8__locals1.targetMap, 0);
					if (pawn2.drafter != null && pawn2.IsColonistPlayerControlled)
					{
						pawn2.drafter.Drafted = true;
					}
					pawn2.Notify_Teleported(true, true);
					if (pawn2.IsPrisoner)
					{
						pawn2.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
					base.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(pawn2, pawn2.Map, 1f), pawn2.Position, 60, CS$<>8__locals1.targetMap);
					SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Exit, new TargetInfo(intVec, pawn2.Map, false));
					if ((pawn2.IsColonist || pawn2.RaceProps.packAnimal) && pawn2.Map.IsPlayerHome)
					{
						pawn2.inventory.UnloadEverything = true;
					}
				}
				if (Find.WorldSelector.IsSelected(caravan))
				{
					Find.WorldSelector.Deselect(caravan);
					CameraJumper.TryJump(CS$<>8__locals1.targetCell, CS$<>8__locals1.targetMap, 0);
				}
				if (caravan != null)
				{
					caravan.Destroy();
				}
			}
			else
			{
				Caravan caravan2 = targets[0].WorldObject as Caravan;
				if (caravan2 != null && caravan2.Faction == this.pawn.Faction)
				{
					if (caravan != null)
					{
						caravan.pawns.TryTransferAllToContainer(caravan2.pawns, true);
						caravan2.Notify_Merged(new List<Caravan>
						{
							caravan
						});
						caravan.Destroy();
						goto IL_3C5;
					}
					using (List<Pawn>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn3 = enumerator.Current;
							caravan2.AddPawn(pawn3, true);
							pawn3.ExitMap(false, Rot4.Invalid);
							AbilityUtility.DoClamor(pawn3.Position, (float)modExtension.clamorRadius, this.pawn, modExtension.clamorType);
						}
						goto IL_3C5;
					}
				}
				if (caravan != null)
				{
					caravan.Tile = targets[0].Tile;
					caravan.pather.StopDead();
				}
				else
				{
					CaravanMaker.MakeCaravan(list, this.pawn.Faction, targets[0].Tile, false);
					foreach (Pawn pawn4 in list)
					{
						pawn4.ExitMap(false, Rot4.Invalid);
					}
				}
			}
			IL_3C5:
			base.Cast(targets);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00017A48 File Offset: 0x00015C48
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			GenDraw.DrawRadiusRing(this.pawn.Position, this.GetRadiusForPawn(), Color.blue, null);
		}
	}
}
