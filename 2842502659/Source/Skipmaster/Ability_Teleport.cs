using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Skipmaster
{
	// Token: 0x0200010E RID: 270
	public class Ability_Teleport : Ability
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00017161 File Offset: 0x00015361
		public virtual FleckDef[] EffectSet
		{
			get
			{
				return new FleckDef[]
				{
					FleckDefOf.PsycastSkipFlashEntry,
					FleckDefOf.PsycastSkipInnerExit,
					FleckDefOf.PsycastSkipOuterRingExit
				};
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00017181 File Offset: 0x00015381
		public override void WarmupToil(Toil toil)
		{
			base.WarmupToil(toil);
			toil.AddPreTickAction(delegate()
			{
				if (this.pawn.jobs.curDriver.ticksLeftThisToil != 5)
				{
					return;
				}
				FleckDef[] effectSet = this.EffectSet;
				for (int i = 0; i < base.Comp.currentlyCastingTargets.Length; i += 2)
				{
					Thing thing = base.Comp.currentlyCastingTargets[i].Thing;
					if (thing != null)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawn, effectSet[0], Vector3.zero, 1f, -1f);
							dataAttachedOverlay.link.detachAfterTicks = 5;
							pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
						}
						else
						{
							FleckMaker.Static(GenThing.TrueCenter(thing), thing.Map, FleckDefOf.PsycastSkipFlashEntry, 1f);
						}
						GlobalTargetInfo globalTargetInfo = base.Comp.currentlyCastingTargets[i + 1];
						FleckMaker.Static(globalTargetInfo.Cell, globalTargetInfo.Map, effectSet[1], 1f);
						FleckMaker.Static(globalTargetInfo.Cell, globalTargetInfo.Map, effectSet[2], 1f);
						SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Entry, thing);
						SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Exit, new TargetInfo(globalTargetInfo.Cell, globalTargetInfo.Map, false));
						base.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(thing, thing.Map, 1f), thing.Position, 60, null);
						base.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(globalTargetInfo.Cell, globalTargetInfo.Map, 1f), globalTargetInfo.Cell, 60, null);
					}
				}
			});
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001719C File Offset: 0x0001539C
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			AbilityExtension_Clamor modExtension = this.def.GetModExtension<AbilityExtension_Clamor>();
			for (int i = 0; i < targets.Length; i += 2)
			{
				Thing thing = targets[i].Thing;
				if (thing != null)
				{
					CompCanBeDormant compCanBeDormant = ThingCompUtility.TryGetComp<CompCanBeDormant>(thing);
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
					GlobalTargetInfo globalTargetInfo = targets[i + 1];
					if (thing.Map != globalTargetInfo.Map)
					{
						Pawn pawn = thing as Pawn;
						if (pawn == null)
						{
							goto IL_F0;
						}
						pawn.teleporting = true;
						pawn.ExitMap(true, Rot4.Invalid);
						pawn.teleporting = false;
						GenSpawn.Spawn(pawn, globalTargetInfo.Cell, globalTargetInfo.Map, 0);
					}
					thing.Position = globalTargetInfo.Cell;
					AbilityUtility.DoClamor(thing.Position, (float)modExtension.clamorRadius, this.pawn, modExtension.clamorType);
					AbilityUtility.DoClamor(globalTargetInfo.Cell, (float)modExtension.clamorRadius, this.pawn, modExtension.clamorType);
					Pawn pawn2 = thing as Pawn;
					if (pawn2 != null)
					{
						pawn2.Notify_Teleported(true, true);
					}
				}
				IL_F0:;
			}
			base.Cast(targets);
		}
	}
}
