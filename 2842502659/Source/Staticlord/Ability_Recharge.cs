using System;
using RimWorld;
using RimWorld.Planet;
using VanillaPsycastsExpanded.Technomancer;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x0200010D RID: 269
	public class Ability_Recharge : Ability
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x00017040 File Offset: 0x00015240
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Hediff hediff = HediffMaker.MakeHediff(VPE_DefOf.VPE_Recharge, this.pawn, null);
				HediffUtility.TryGetComp<HediffComp_Recharge>(hediff).Init(globalTargetInfo.Thing);
				HediffUtility.TryGetComp<HediffComp_Disappears>(hediff).ticksToDisappear = this.GetDurationForPawn();
				this.pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000170C0 File Offset: 0x000152C0
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			Thing thing = target.Thing;
			if (((thing != null) ? ThingCompUtility.TryGetComp<CompPowerBattery>(thing) : null) != null)
			{
				return true;
			}
			if (ModsConfig.BiotechActive)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					RaceProperties raceProps = pawn.RaceProps;
					if (raceProps != null && raceProps.IsMechanoid)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						if (needs != null && needs.energy != null && pawn.IsMechAlly(this.pawn))
						{
							return true;
						}
					}
				}
			}
			if (showMessages)
			{
				Messages.Message(Translator.Translate("VPE.MustTargetBattery"), MessageTypeDefOf.RejectInput, false);
			}
			return false;
		}
	}
}
