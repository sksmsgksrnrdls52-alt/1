using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200009E RID: 158
	public class AbilityExtension_Psycast : AbilityExtension_AbilityMod
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x0000A6C4 File Offset: 0x000088C4
		public override bool ShowGizmoOnPawn(Pawn pawn)
		{
			Hediff_PsycastAbilities hediff_PsycastAbilities = pawn.Psycasts();
			if (hediff_PsycastAbilities == null)
			{
				Log.Error("AbilityExtension_Psycast.ShowGizmoOnPawn called on a pawn that does not have Psycasts.");
				return false;
			}
			return !hediff_PsycastAbilities.previousUnlockedPaths.Contains(this.path);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000A6FB File Offset: 0x000088FB
		public bool PrereqsCompleted(Pawn pawn)
		{
			return this.PrereqsCompleted(pawn.GetComp<CompAbilities>());
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000A709 File Offset: 0x00008909
		public bool PrereqsCompleted(CompAbilities compAbilities)
		{
			return GenList.NullOrEmpty<AbilityDef>(this.prerequisites) || GenCollection.Any<Ability>(compAbilities.LearnedAbilities, (Ability ab) => this.prerequisites.Contains(ab.def));
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000A734 File Offset: 0x00008934
		public void UnlockWithPrereqs(CompAbilities compAbilities)
		{
			foreach (AbilityDef abilityDef in this.prerequisites)
			{
				AbilityExtension_Psycast modExtension = abilityDef.GetModExtension<AbilityExtension_Psycast>();
				if (modExtension != null)
				{
					modExtension.UnlockWithPrereqs(compAbilities);
				}
				else
				{
					compAbilities.GiveAbility(abilityDef);
				}
			}
			compAbilities.GiveAbility(this.abilityDef);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A7A8 File Offset: 0x000089A8
		public float GetPsyfocusUsedByPawn(Pawn pawn)
		{
			return this.psyfocusCost * StatExtension.GetStatValue(pawn, VPE_DefOf.VPE_PsyfocusCostFactor, true, -1);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A7C0 File Offset: 0x000089C0
		public float GetEntropyUsedByPawn(Pawn pawn)
		{
			return this.entropyGainStatFactors.Aggregate(this.entropyGain, (float current, StatModifier statFactor) => current * (StatExtension.GetStatValue(pawn, statFactor.stat, true, -1) * statFactor.value));
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A7F8 File Offset: 0x000089F8
		public override bool IsEnabledForPawn(Ability ability, out string reason)
		{
			if (!this.path.CanPawnUnlock(ability.pawn) && !this.path.ignoreLockRestrictionsForNeurotrainers)
			{
				reason = this.path.lockedReason;
				return false;
			}
			Hediff_PsycastAbilities hediff_PsycastAbilities;
			if (ability == null)
			{
				hediff_PsycastAbilities = null;
			}
			else
			{
				Pawn pawn = ability.pawn;
				hediff_PsycastAbilities = ((pawn != null) ? pawn.Psycasts() : null);
			}
			Hediff_PsycastAbilities hediff_PsycastAbilities2 = hediff_PsycastAbilities;
			if (hediff_PsycastAbilities2 == null)
			{
				reason = Translator.Translate("VPE.NotPsycaster");
				return false;
			}
			if (ability.pawn.psychicEntropy.PsychicSensitivity < 1E-45f)
			{
				reason = Translator.Translate("CommandPsycastZeroPsychicSensitivity");
				return false;
			}
			float psyfocusUsedByPawn = this.GetPsyfocusUsedByPawn(ability.pawn);
			if (!hediff_PsycastAbilities2.SufficientPsyfocusPresent(psyfocusUsedByPawn))
			{
				reason = TranslatorFormattedStringExtensions.Translate("CommandPsycastNotEnoughPsyfocus", GenText.ToStringPercent(psyfocusUsedByPawn, "#.0"), GenText.ToStringPercent(ability.pawn.psychicEntropy.CurrentPsyfocus, "#.0"), NamedArgumentUtility.Named(ability.def.label, "PSYCASTNAME"), NamedArgumentUtility.Named(ability.pawn, "CASTERNAME"));
				return false;
			}
			if (ability.pawn.psychicEntropy.WouldOverflowEntropy(this.GetEntropyUsedByPawn(ability.pawn)))
			{
				reason = TranslatorFormattedStringExtensions.Translate("CommandPsycastWouldExceedEntropy", ability.def.label);
				return false;
			}
			if (hediff_PsycastAbilities2.CurrentlyChanneling != null)
			{
				reason = TranslatorFormattedStringExtensions.Translate("VPE.CurrentChanneling", hediff_PsycastAbilities2.CurrentlyChanneling.def.LabelCap);
				return false;
			}
			if (ability.pawn.Downed)
			{
				reason = TranslatorFormattedStringExtensions.Translate("IsIncapped", ability.pawn.LabelShort, ability.pawn);
				return false;
			}
			reason = string.Empty;
			return true;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A9C4 File Offset: 0x00008BC4
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			Hediff_PsycastAbilities hediff_PsycastAbilities = (Hediff_PsycastAbilities)ability.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant, false);
			hediff_PsycastAbilities.UseAbility(this.GetPsyfocusUsedByPawn(ability.pawn), this.GetEntropyUsedByPawn(ability.pawn));
			IChannelledPsycast channelledPsycast = ability as IChannelledPsycast;
			if (channelledPsycast != null)
			{
				hediff_PsycastAbilities.BeginChannelling(channelledPsycast);
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000AA2C File Offset: 0x00008C2C
		public override string GetDescription(Ability ability)
		{
			StringBuilder stringBuilder = new StringBuilder();
			float psyfocusUsedByPawn = this.GetPsyfocusUsedByPawn(ability.pawn);
			if (psyfocusUsedByPawn > 1E-45f)
			{
				GenText.AppendInNewLine(stringBuilder, string.Format("{0}: {1}", Translator.Translate("AbilityPsyfocusCost"), GenText.ToStringPercent(psyfocusUsedByPawn)));
			}
			float entropyUsedByPawn = this.GetEntropyUsedByPawn(ability.pawn);
			if (entropyUsedByPawn > 1E-45f)
			{
				GenText.AppendInNewLine(stringBuilder, string.Format("{0}: {1}", Translator.Translate("AbilityEntropyGain"), entropyUsedByPawn));
			}
			return ColoredText.Colorize(stringBuilder.ToString(), Color.cyan);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000AAC8 File Offset: 0x00008CC8
		public override void WarmupToil(Toil toil)
		{
			base.WarmupToil(toil);
			if (!this.showCastBubble)
			{
				return;
			}
			toil.AddPreInitAction(delegate()
			{
				MoteCastBubble moteCastBubble = (MoteCastBubble)ThingMaker.MakeThing(VPE_DefOf.VPE_Mote_Cast, null);
				moteCastBubble.Setup(toil.actor, toil.actor.GetComp<CompAbilities>().currentlyCasting);
				GenSpawn.Spawn(moteCastBubble, toil.actor.Position, toil.actor.Map, 0);
			});
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000AB10 File Offset: 0x00008D10
		public override void TargetingOnGUI(LocalTargetInfo target, Ability ability)
		{
			base.TargetingOnGUI(target, ability);
			if (!this.psychic)
			{
				return;
			}
			List<GlobalTargetInfo> list = (from t in ability.currentTargets
			where t.IsValid && t.Map != null
			select t).ToList<GlobalTargetInfo>();
			GlobalTargetInfo[] array = new GlobalTargetInfo[list.Count + 1];
			list.CopyTo(array, 0);
			array[array.Length - 1] = target.ToGlobalTargetInfo(((list != null) ? list.LastOrDefault<GlobalTargetInfo>().Map : null) ?? ability.pawn.Map);
			ability.ModifyTargets(ref array);
			foreach (GlobalTargetInfo globalTargetInfo in array)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					float statValue = StatExtension.GetStatValue(pawn, StatDefOf.PsychicSensitivity, true, -1);
					if (statValue < 1E-45f)
					{
						Vector3 drawPos = pawn.DrawPos;
						drawPos.z += 1f;
						GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), Translator.Translate("Ineffective"), Color.red);
					}
					else
					{
						Vector3 drawPos2 = pawn.DrawPos;
						drawPos2.z += 1f;
						GenMapUI.DrawText(new Vector2(drawPos2.x, drawPos2.z), StatDefOf.PsychicSensitivity.LabelCap + ": " + GenText.ToStringPercent(statValue), (statValue > float.Epsilon) ? Color.white : Color.red);
					}
				}
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000ACB8 File Offset: 0x00008EB8
		public override bool Valid(GlobalTargetInfo[] targets, Ability ability, bool throwMessages = false)
		{
			bool flag = base.Valid(targets, ability, throwMessages);
			if (flag)
			{
				string text;
				flag = this.IsEnabledForPawn(ability, ref text);
				if (!flag && throwMessages)
				{
					Messages.Message(text, MessageTypeDefOf.RejectInput, false);
				}
			}
			return flag;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000ACF4 File Offset: 0x00008EF4
		public override bool ValidateTarget(LocalTargetInfo target, Ability ability, bool throwMessages = false)
		{
			if (this.psychic)
			{
				Pawn pawn = target.Pawn;
				if (pawn != null && StatExtension.GetStatValue(pawn, StatDefOf.PsychicSensitivity, true, -1) < 1E-45f)
				{
					if (throwMessages)
					{
						Messages.Message(Translator.Translate("Ineffective"), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000080 RID: 128
		public float entropyGain;

		// Token: 0x04000081 RID: 129
		public List<StatModifier> entropyGainStatFactors = new List<StatModifier>();

		// Token: 0x04000082 RID: 130
		public int level;

		// Token: 0x04000083 RID: 131
		public int order;

		// Token: 0x04000084 RID: 132
		public PsycasterPathDef path;

		// Token: 0x04000085 RID: 133
		public List<AbilityDef> prerequisites = new List<AbilityDef>();

		// Token: 0x04000086 RID: 134
		public bool psychic;

		// Token: 0x04000087 RID: 135
		public float psyfocusCost;

		// Token: 0x04000088 RID: 136
		public bool showCastBubble = true;

		// Token: 0x04000089 RID: 137
		public bool spaceAfter;
	}
}
