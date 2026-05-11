using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000AA RID: 170
	public class AbilityExtension_WordOfLove : AbilityExtension_AbilityMod
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000BDDF File Offset: 0x00009FDF
		public override bool HidePawnTooltips
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000BDE4 File Offset: 0x00009FE4
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			Pawn target = targets[1].Thing as Pawn;
			Pawn pawn = targets[0].Thing as Pawn;
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff_PsychicLove hediff_PsychicLove = (Hediff_PsychicLove)HediffMaker.MakeHediff(HediffDefOf.PsychicLove, pawn, pawn.health.hediffSet.GetBrain());
			hediff_PsychicLove.target = target;
			HediffComp_Disappears hediffComp_Disappears = HediffUtility.TryGetComp<HediffComp_Disappears>(hediff_PsychicLove);
			if (hediffComp_Disappears != null)
			{
				hediffComp_Disappears.ticksToDisappear = (int)((float)ability.GetDurationForPawn() * StatExtension.GetStatValue(pawn, StatDefOf.PsychicSensitivity, true, -1));
			}
			pawn.health.AddHediff(hediff_PsychicLove, null, null, null);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000BEAC File Offset: 0x0000A0AC
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target, Ability ability)
		{
			if (GenCollection.Any<GlobalTargetInfo>((from x in ability.currentTargets
			where x.Thing != null
			select x).ToList<GlobalTargetInfo>()))
			{
				return Translator.Translate("PsychicLoveFor");
			}
			return Translator.Translate("PsychicLoveInduceIn");
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000BF10 File Offset: 0x0000A110
		public override bool ValidateTarget(LocalTargetInfo target, Ability ability, bool showMessages = true)
		{
			List<GlobalTargetInfo> list = (from x in ability.currentTargets
			where x.Thing != null
			select x).ToList<GlobalTargetInfo>();
			if (!GenCollection.Any<GlobalTargetInfo>(list))
			{
				return base.ValidateTarget(target, ability, showMessages);
			}
			Pawn pawn = list[0].Thing as Pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn == pawn2)
			{
				return false;
			}
			if (pawn != null && pawn2 != null && !pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
			{
				Gender gender = pawn.gender;
				Gender gender2 = pawn.story.traits.HasTrait(TraitDefOf.Gay) ? gender : GenderUtility.Opposite(gender);
				if (pawn2.gender != gender2)
				{
					if (showMessages)
					{
						Messages.Message(TranslatorFormattedStringExtensions.Translate("AbilityCantApplyWrongAttractionGender", pawn, pawn2), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000C010 File Offset: 0x0000A210
		public override bool Valid(GlobalTargetInfo[] targets, Ability ability, bool throwMessages = false)
		{
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					if (pawn.story.traits.HasTrait(TraitDefOf.Asexual))
					{
						if (throwMessages)
						{
							Messages.Message(TranslatorFormattedStringExtensions.Translate("AbilityCantApplyOnAsexual", ability.def.label), pawn, MessageTypeDefOf.RejectInput, false);
						}
						return false;
					}
					if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages, null))
					{
						return false;
					}
				}
			}
			return base.Valid(targets, ability, throwMessages);
		}
	}
}
