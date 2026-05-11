using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000FC RID: 252
	public class Ability_Power : Ability
	{
		// Token: 0x06000380 RID: 896 RVA: 0x00015AE8 File Offset: 0x00013CE8
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Hediff hediff = base.ApplyHediff(this.pawn);
				if (hediff != null)
				{
					HediffUtility.TryGetComp<HediffComp_InfinitePower>(hediff).Begin(globalTargetInfo.Thing);
				}
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00015B38 File Offset: 0x00013D38
		public override Hediff ApplyHediff(Pawn targetPawn, HediffDef hediffDef, BodyPartRecord bodyPart, int duration, float severity)
		{
			Hediff hediff = HediffMaker.MakeHediff(hediffDef, targetPawn, bodyPart);
			Hediff_Ability hediff_Ability = hediff as Hediff_Ability;
			if (hediff_Ability != null)
			{
				hediff_Ability.ability = this;
			}
			if (severity > 1E-45f)
			{
				hediff.Severity = severity;
			}
			HediffWithComps hediffWithComps = hediff as HediffWithComps;
			if (hediffWithComps != null)
			{
				foreach (HediffComp hediffComp in hediffWithComps.comps)
				{
					HediffComp_Ability hediffComp_Ability = hediffComp as HediffComp_Ability;
					if (hediffComp_Ability != null)
					{
						hediffComp_Ability.ability = this;
					}
					HediffComp_Disappears hediffComp_Disappears = hediffComp as HediffComp_Disappears;
					if (hediffComp_Disappears != null)
					{
						hediffComp_Disappears.ticksToDisappear = duration;
					}
				}
			}
			targetPawn.health.AddHediff(hediff, null, null, null);
			return hediff;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00015BFC File Offset: 0x00013DFC
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			Thing thing = target.Thing;
			CompPowerTrader compPowerTrader = (thing != null) ? ThingCompUtility.TryGetComp<CompPowerTrader>(thing) : null;
			if (compPowerTrader != null && compPowerTrader.PowerOutput < 0f)
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
				Messages.Message(Translator.Translate("VPE.MustConsumePower"), MessageTypeDefOf.RejectInput, false);
			}
			return false;
		}
	}
}
