using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A8 RID: 168
	public class AbilityExtension_PsycastWordOfSerenity : AbilityExtension_Psycast
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000B9CC File Offset: 0x00009BCC
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo target in targets)
			{
				((Hediff_PsycastAbilities)ability.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant, false)).UseAbility(this.PsyfocusCostForTarget(target), base.GetEntropyUsedByPawn(ability.pawn));
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000BA34 File Offset: 0x00009C34
		public float PsyfocusCostForTarget(GlobalTargetInfo target)
		{
			float result;
			switch (this.TargetMentalBreakIntensity(target))
			{
			case 1:
				result = this.psyfocusCostForMinor;
				break;
			case 2:
				result = this.psyfocusCostForMajor;
				break;
			case 3:
				result = this.psyfocusCostForExtreme;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000BA84 File Offset: 0x00009C84
		public MentalBreakIntensity TargetMentalBreakIntensity(GlobalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			MentalStateDef mentalStateDef = (pawn != null) ? pawn.MentalStateDef : null;
			if (mentalStateDef != null)
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].mentalState == mentalStateDef)
					{
						return allDefsListForReading[i].intensity;
					}
				}
			}
			return 1;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		public override bool Valid(GlobalTargetInfo[] targets, Ability ability, bool throwMessages = false)
		{
			foreach (GlobalTargetInfo target in targets)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					if (!AbilityUtility.ValidateHasMentalState(pawn, throwMessages, null))
					{
						return false;
					}
					if (this.exceptions.Contains(pawn.MentalStateDef))
					{
						if (throwMessages)
						{
							Messages.Message(TranslatorFormattedStringExtensions.Translate("AbilityDoesntWorkOnMentalState", ability.def.label, pawn.MentalStateDef.label), pawn, MessageTypeDefOf.RejectInput, false);
						}
						return false;
					}
					float num = this.PsyfocusCostForTarget(target);
					if (num > ability.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
					{
						Pawn pawn2 = ability.pawn;
						if (throwMessages)
						{
							TaggedString taggedString = Translator.Translate("MentalBreakIntensity" + this.TargetMentalBreakIntensity(target).ToString());
							Messages.Message(TranslatorFormattedStringExtensions.Translate("CommandPsycastNotEnoughPsyfocusForMentalBreak", GenText.ToStringPercent(num), taggedString, GenText.ToStringPercent(pawn2.psychicEntropy.CurrentPsyfocus, "0.#"), NamedArgumentUtility.Named(ability.def.label, "PSYCASTNAME"), NamedArgumentUtility.Named(pawn2, "CASTERNAME")), pawn, MessageTypeDefOf.RejectInput, false);
						}
						return false;
					}
				}
			}
			return base.Valid(targets, ability, throwMessages);
		}

		// Token: 0x0400009A RID: 154
		public List<MentalStateDef> exceptions;

		// Token: 0x0400009B RID: 155
		public float psyfocusCostForExtreme = -1f;

		// Token: 0x0400009C RID: 156
		public float psyfocusCostForMajor = -1f;

		// Token: 0x0400009D RID: 157
		public float psyfocusCostForMinor = -1f;
	}
}
