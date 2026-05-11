using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x02000119 RID: 281
	public class Ability_Assassinate : Ability
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x00018BF4 File Offset: 0x00016DF4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			this.target = (targets.FirstOrDefault((GlobalTargetInfo t) => t.Thing is Pawn).Thing as Pawn);
			if (this.target == null)
			{
				return;
			}
			this.attacksLeft = Mathf.RoundToInt(this.GetPowerForPawn());
			Map map = this.pawn.Map;
			this.originalPosition = this.pawn.Position;
			this.target.stances.stunner.StunFor(this.attacksLeft * 2, this.pawn, true, true, false);
			this.TeleportPawnTo(GenCollection.RandomElement<IntVec3>(from c in GenAdjFast.AdjacentCellsCardinal(this.target.Position)
			where GenGrid.Walkable(c, map)
			select c));
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00018CD4 File Offset: 0x00016ED4
		public override void Tick()
		{
			base.Tick();
			if (this.attacksLeft > 0)
			{
				this.attacksLeft--;
				this.DoAttack();
				if (this.attacksLeft == 0)
				{
					SoundStarter.PlayOneShot(VPE_DefOf.VPE_Assassinate_Return, this.pawn);
					this.TeleportPawnTo(this.originalPosition);
				}
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00018D30 File Offset: 0x00016F30
		private void DoAttack()
		{
			Verb verb = GenCollection.MaxBy<VerbEntry, float>(this.pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false), (VerbEntry v) => VerbUtility.DPS(v.verb, this.pawn)).verb;
			this.pawn.meleeVerbs.TryMeleeAttack(this.target, verb, true);
			this.pawn.stances.CancelBusyStanceHard();
			FleckMaker.AttachedOverlay(this.target, VPE_DefOf.VPE_Slash, Rand.InsideUnitCircle * 0.3f, 1f, -1f);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00018DBC File Offset: 0x00016FBC
		private void TeleportPawnTo(IntVec3 c)
		{
			FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(this.pawn, FleckDefOf.PsycastSkipFlashEntry, Vector3.zero, 1f, -1f);
			dataAttachedOverlay.link.detachAfterTicks = 1;
			this.pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
			TargetInfo targetInfo;
			targetInfo..ctor(c, this.pawn.Map, false);
			FleckMaker.Static(targetInfo.Cell, targetInfo.Map, FleckDefOf.PsycastSkipInnerExit, 1f);
			FleckMaker.Static(targetInfo.Cell, targetInfo.Map, FleckDefOf.PsycastSkipOuterRingExit, 1f);
			SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Entry, this.pawn);
			SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Exit, targetInfo);
			base.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(this.pawn, this.pawn.Map, 1f), this.pawn.Position, 60, null);
			base.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(targetInfo.Cell, targetInfo.Map, 1f), targetInfo.Cell, 60, null);
			this.pawn.Position = c;
			this.pawn.Notify_Teleported(true, true);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x00018EFC File Offset: 0x000170FC
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.Map.glowGrid.GroundGlowAt(pawn.Position, false, false) <= 0.29f)
				{
					return true;
				}
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustBeInDark"), MessageTypeDefOf.RejectInput, false);
				}
			}
			return false;
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00018F54 File Offset: 0x00017154
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.attacksLeft, "attacksLeft", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.originalPosition, "originalPosition", default(IntVec3), false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x040001C7 RID: 455
		private int attacksLeft;

		// Token: 0x040001C8 RID: 456
		private IntVec3 originalPosition;

		// Token: 0x040001C9 RID: 457
		private Pawn target;
	}
}
