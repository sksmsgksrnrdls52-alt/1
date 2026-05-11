using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B8 RID: 184
	public class Ability_Killskip : Ability
	{
		// Token: 0x0600025C RID: 604 RVA: 0x0000D831 File Offset: 0x0000BA31
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			this.AttackTarget((LocalTargetInfo)targets[0]);
			this.TryQueueAttackIfDead((LocalTargetInfo)targets[0]);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000D85E File Offset: 0x0000BA5E
		private void TryQueueAttackIfDead(LocalTargetInfo target)
		{
			if (target.Pawn.Dead)
			{
				this.attackInTicks = Find.TickManager.TicksGame + this.def.castTime;
				return;
			}
			this.attackInTicks = -1;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000D894 File Offset: 0x0000BA94
		public override void Tick()
		{
			base.Tick();
			if (this.attackInTicks != -1 && Find.TickManager.TicksGame >= this.attackInTicks)
			{
				this.attackInTicks = -1;
				Pawn pawn = this.FindAttackTarget();
				if (pawn != null)
				{
					this.AttackTarget(pawn);
					this.TryQueueAttackIfDead(pawn);
				}
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000D8EC File Offset: 0x0000BAEC
		private void AttackTarget(LocalTargetInfo target)
		{
			base.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(this.pawn.Position, this.pawn.Map, 0.72f), this.pawn.Position, 60, null);
			base.AddEffecterToMaintain(VPE_DefOf.VPE_Skip_ExitNoDelayRed.Spawn(target.Cell, this.pawn.Map, 0.72f), target.Cell, 60, null);
			this.pawn.Position = target.Cell;
			this.pawn.Notify_Teleported(false, true);
			this.pawn.stances.SetStance(new Stance_Mobile());
			VerbProperties_AdjustedMeleeDamageAmount_Patch.multiplyByPawnMeleeSkill = true;
			this.pawn.meleeVerbs.TryMeleeAttack(target.Pawn, null, true);
			this.pawn.meleeVerbs.TryMeleeAttack(target.Pawn, null, true);
			VerbProperties_AdjustedMeleeDamageAmount_Patch.multiplyByPawnMeleeSkill = false;
			SoundStarter.PlayOneShot(GenCollection.RandomElement<SoundDef>(Ability_Killskip.castSounds), this.pawn);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000D9F4 File Offset: 0x0000BBF4
		private Pawn FindAttackTarget()
		{
			TargetScanFlags targetScanFlags = 297;
			return (Pawn)AttackTargetFinder.BestAttackTarget(this.pawn, targetScanFlags, delegate(Thing x)
			{
				Pawn pawn = x as Pawn;
				return pawn != null && !pawn.Dead;
			}, 0f, 999999f, default(IntVec3), float.MaxValue, false, true, false, false);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000DA53 File Offset: 0x0000BC53
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.attackInTicks, "attackInTicks", -1, false);
		}

		// Token: 0x040000AF RID: 175
		private int attackInTicks = -1;

		// Token: 0x040000B0 RID: 176
		private static List<SoundDef> castSounds = new List<SoundDef>
		{
			VPE_DefOf.VPE_Killskip_Jump_01a,
			VPE_DefOf.VPE_Killskip_Jump_01b,
			VPE_DefOf.VPE_Killskip_Jump_01c
		};
	}
}
