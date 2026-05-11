using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000092 RID: 146
	public class AbilityExtension_GiveMentalState : AbilityExtension_AbilityMod
	{
		// Token: 0x060001B3 RID: 435 RVA: 0x00009BE0 File Offset: 0x00007DE0
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = this.applyToSelf ? ability.pawn : (globalTargetInfo.Thing as Pawn);
				if (pawn != null)
				{
					if (pawn.InMentalState)
					{
						if (!this.clearOthers)
						{
							goto IL_9D;
						}
						pawn.mindState.mentalStateHandler.CurState.RecoverFromState();
					}
					AbilityExtension_GiveMentalState.TryGiveMentalStateWithDuration(pawn.RaceProps.IsMechanoid ? (this.stateDefForMechs ?? this.stateDef) : this.stateDef, pawn, ability, this.durationMultiplier, this.durationScalesWithCaster);
					RestUtility.WakeUp(pawn, true);
				}
				IL_9D:;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00009C98 File Offset: 0x00007E98
		public override bool Valid(GlobalTargetInfo[] targets, Ability ability, bool throwMessages = false)
		{
			Pawn pawn = (from t in targets
			select t.Thing).OfType<Pawn>().FirstOrDefault<Pawn>();
			return pawn == null || AbilityUtility.ValidateNoMentalState(pawn, throwMessages, null);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009CE8 File Offset: 0x00007EE8
		public static void TryGiveMentalStateWithDuration(MentalStateDef def, Pawn p, Ability ability, StatDef multiplierStat, bool durationScalesWithCaster)
		{
			if (p.mindState.mentalStateHandler.TryStartMentalState(def, null, true, false, false, null, false, false, ability.def.GetModExtension<AbilityExtension_Psycast>() != null))
			{
				float num = (float)ability.GetDurationForPawn();
				if (multiplierStat != null)
				{
					if (durationScalesWithCaster)
					{
						num *= StatExtension.GetStatValue(p, multiplierStat, true, -1);
					}
					else
					{
						num *= StatExtension.GetStatValue(ability.pawn, multiplierStat, true, -1);
					}
				}
				p.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = (int)num;
			}
		}

		// Token: 0x04000075 RID: 117
		public bool applyToSelf;

		// Token: 0x04000076 RID: 118
		public bool clearOthers;

		// Token: 0x04000077 RID: 119
		public StatDef durationMultiplier;

		// Token: 0x04000078 RID: 120
		public bool durationScalesWithCaster;

		// Token: 0x04000079 RID: 121
		public MentalStateDef stateDef;

		// Token: 0x0400007A RID: 122
		public MentalStateDef stateDefForMechs;
	}
}
