using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000096 RID: 150
	public class AbilityExtension_TransferEntropy : AbilityExtension_AbilityMod
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x0000A114 File Offset: 0x00008314
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					if (this.targetReceivesEntropy)
					{
						pawn.psychicEntropy.TryAddEntropy(ability.pawn.psychicEntropy.EntropyValue, ability.pawn, false, true);
					}
					if (!pawn.HasPsylink)
					{
						Hediff hediff = HediffMaker.MakeHediff(VPE_DefOf.PsychicComa, pawn, null);
						pawn.health.AddHediff(hediff, null, null, null);
					}
					ability.pawn.psychicEntropy.RemoveAllEntropy();
					MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_PsychicLinkPulse, ability.pawn, pawn);
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000A1E1 File Offset: 0x000083E1
		public override bool IsEnabledForPawn(Ability ability, out string reason)
		{
			if (ability.pawn.psychicEntropy.EntropyValue <= 0f)
			{
				reason = Translator.Translate("AbilityNoEntropyToDump");
				return false;
			}
			return base.IsEnabledForPawn(ability, ref reason);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000A218 File Offset: 0x00008418
		public override bool Valid(GlobalTargetInfo[] targets, Ability ability, bool throwMessages = false)
		{
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null && !AbilityUtility.ValidateNoMentalState(pawn, throwMessages, null))
				{
					return false;
				}
			}
			return base.Valid(targets, ability, throwMessages);
		}

		// Token: 0x0400007B RID: 123
		public bool targetReceivesEntropy = true;
	}
}
