using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A5 RID: 165
	[StaticConstructorOnStartup]
	public class Hediff_PsycastAbilities : Hediff_Abilities
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000AE5D File Offset: 0x0000905D
		public Ability CurrentlyChanneling
		{
			get
			{
				return this.currentlyChanneling as Ability;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000AE6A File Offset: 0x0000906A
		public override HediffStage CurStage
		{
			get
			{
				if (this.curStage == null)
				{
					this.RecacheCurStage();
				}
				return this.curStage;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000AE80 File Offset: 0x00009080
		public IEnumerable<Gizmo> GetPsySetGizmos()
		{
			Hediff_PsycastAbilities.<GetPsySetGizmos>d__18 <GetPsySetGizmos>d__ = new Hediff_PsycastAbilities.<GetPsySetGizmos>d__18(-2);
			<GetPsySetGizmos>d__.<>4__this = this;
			return <GetPsySetGizmos>d__;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000AE90 File Offset: 0x00009090
		private string PsySetLabel(int index)
		{
			if (index == this.psysets.Count)
			{
				return Translator.Translate("VPE.All");
			}
			return this.psysets[index].Name;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000AEC1 File Offset: 0x000090C1
		private IEnumerable<FloatMenuOption> GetPsySetFloatMenuOptions()
		{
			Hediff_PsycastAbilities.<GetPsySetFloatMenuOptions>d__20 <GetPsySetFloatMenuOptions>d__ = new Hediff_PsycastAbilities.<GetPsySetFloatMenuOptions>d__20(-2);
			<GetPsySetFloatMenuOptions>d__.<>4__this = this;
			return <GetPsySetFloatMenuOptions>d__;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000AED1 File Offset: 0x000090D1
		public void InitializeFromPsylink(Hediff_Psylink psylink)
		{
			this.psylink = psylink;
			this.level = psylink.level;
			this.points = this.level;
			if (this.level <= 1)
			{
				this.points = 2;
			}
			this.RecacheCurStage();
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000AF08 File Offset: 0x00009108
		private void RecacheCurStage()
		{
			this.minHeatGivers.RemoveAll((IMinHeatGiver giver) => giver == null || !giver.IsActive);
			HediffStage hediffStage = new HediffStage();
			List<StatModifier> list = new List<StatModifier>();
			list.Add(new StatModifier
			{
				stat = StatDefOf.PsychicEntropyMax,
				value = (float)(this.level * 5 + this.statPoints * 10)
			});
			list.Add(new StatModifier
			{
				stat = StatDefOf.PsychicEntropyRecoveryRate,
				value = (float)this.level * 0.0125f + (float)this.statPoints * 0.05f
			});
			list.Add(new StatModifier
			{
				stat = StatDefOf.PsychicSensitivity,
				value = (float)this.statPoints * 0.05f
			});
			list.Add(new StatModifier
			{
				stat = VPE_DefOf.VPE_PsyfocusCostFactor,
				value = (float)this.statPoints * -0.01f
			});
			StatModifier statModifier = new StatModifier();
			statModifier.stat = VPE_DefOf.VPE_PsychicEntropyMinimum;
			statModifier.value = (float)this.minHeatGivers.Sum(delegate(IMinHeatGiver giver)
			{
				if (giver.MinHeat == 0)
				{
					return 0;
				}
				return giver.MinHeat;
			});
			list.Add(statModifier);
			hediffStage.statOffsets = list;
			hediffStage.becomeVisible = false;
			this.curStage = hediffStage;
			if (PsycastsMod.Settings.changeFocusGain)
			{
				this.curStage.statOffsets.Add(new StatModifier
				{
					stat = StatDefOf.MeditationFocusGain,
					value = (float)this.statPoints * 0.1f
				});
			}
			if (this.pawn != null && this.pawn.Spawned)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000B0C1 File Offset: 0x000092C1
		public void UseAbility(float focus, float entropy)
		{
			this.pawn.psychicEntropy.TryAddEntropy(entropy, null, true, false);
			this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-focus);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000B0EC File Offset: 0x000092EC
		public void ChangeLevel(int levelOffset, bool sendLetter)
		{
			this.ChangeLevel(levelOffset);
			if (sendLetter && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Find.LetterStack.ReceiveLetter(TranslatorFormattedStringExtensions.Translate("VPE.PsylinkGained", this.pawn.LabelShortCap), TranslatorFormattedStringExtensions.Translate("VPE.PsylinkGained.Desc", this.pawn.LabelShortCap, GenText.CapitalizeFirst(GenderUtility.GetPronoun(this.pawn.gender)), Hediff_PsycastAbilities.ExperienceRequiredForLevel(this.level + 1)), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000B198 File Offset: 0x00009398
		public override void ChangeLevel(int levelOffset)
		{
			base.ChangeLevel(levelOffset);
			this.points += levelOffset;
			this.RecacheCurStage();
			if (this.psylink == null)
			{
				this.psylink = this.pawn.health.hediffSet.hediffs.OfType<Hediff_Psylink>().FirstOrDefault<Hediff_Psylink>();
			}
			if (this.psylink == null)
			{
				PawnUtility.ChangePsylinkLevel(this.pawn, this.level, false);
				this.psylink = this.pawn.health.hediffSet.hediffs.OfType<Hediff_Psylink>().First<Hediff_Psylink>();
			}
			this.psylink.level = this.level;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000B240 File Offset: 0x00009440
		public void Reset()
		{
			this.points = this.level;
			this.unlockedPaths.Clear();
			this.unlockedMeditationFoci.Clear();
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
			this.statPoints = 0;
			CompAbilities comp = this.pawn.GetComp<CompAbilities>();
			if (comp != null)
			{
				comp.LearnedAbilities.RemoveAll((Ability a) => a.def.Psycast() != null);
			}
			this.RecacheCurStage();
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B2C4 File Offset: 0x000094C4
		public void GainExperience(float experienceGain, bool sendLetter = true)
		{
			if (this.level >= PsycastsMod.Settings.maxLevel)
			{
				return;
			}
			this.experience += experienceGain;
			bool flag = false;
			while (this.level < PsycastsMod.Settings.maxLevel && this.experience >= (float)Hediff_PsycastAbilities.ExperienceRequiredForLevel(this.level + 1))
			{
				this.ChangeLevel(1, sendLetter && !flag);
				flag = true;
				this.experience -= (float)Hediff_PsycastAbilities.ExperienceRequiredForLevel(this.level);
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B34A File Offset: 0x0000954A
		public bool SufficientPsyfocusPresent(float focusRequired)
		{
			return this.pawn.psychicEntropy.CurrentPsyfocus > focusRequired;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B360 File Offset: 0x00009560
		public override bool SatisfiesConditionForAbility(AbilityDef abilityDef)
		{
			if (!base.SatisfiesConditionForAbility(abilityDef))
			{
				HediffWithLevelCombination requiredHediff = abilityDef.requiredHediff;
				int? num = (requiredHediff != null) ? new int?(requiredHediff.minimumLevel) : null;
				int level = this.psylink.level;
				return num.GetValueOrDefault() <= level & num != null;
			}
			return true;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000B3B9 File Offset: 0x000095B9
		public void AddMinHeatGiver(IMinHeatGiver giver)
		{
			if (!this.minHeatGivers.Contains(giver))
			{
				this.minHeatGivers.Add(giver);
				this.RecacheCurStage();
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000B3DB File Offset: 0x000095DB
		public void BeginChannelling(IChannelledPsycast psycast)
		{
			this.currentlyChanneling = psycast;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000B3E4 File Offset: 0x000095E4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.experience, "experience", 0f, false);
			Scribe_Values.Look<int>(ref this.points, "points", 0, false);
			Scribe_Values.Look<int>(ref this.statPoints, "statPoints", 0, false);
			Scribe_Values.Look<int>(ref this.psysetIndex, "psysetIndex", 0, false);
			Scribe_Values.Look<int>(ref this.maxLevelFromTitles, "maxLevelFromTitles", 0, false);
			Scribe_Collections.Look<PsycasterPathDef>(ref this.previousUnlockedPaths, "previousUnlockedPaths", 4, Array.Empty<object>());
			Scribe_Collections.Look<PsycasterPathDef>(ref this.unlockedPaths, "unlockedPaths", 4, Array.Empty<object>());
			Scribe_Collections.Look<MeditationFocusDef>(ref this.unlockedMeditationFoci, "unlockedMeditationFoci", 4, Array.Empty<object>());
			Scribe_Collections.Look<PsySet>(ref this.psysets, "psysets", 2, Array.Empty<object>());
			Scribe_Collections.Look<IMinHeatGiver>(ref this.minHeatGivers, "minHeatGivers", 3, Array.Empty<object>());
			Scribe_References.Look<Hediff_Psylink>(ref this.psylink, "psylink", false);
			Scribe_References.Look<IChannelledPsycast>(ref this.currentlyChanneling, "currentlyChanneling", false);
			if (this.minHeatGivers == null)
			{
				this.minHeatGivers = new List<IMinHeatGiver>();
			}
			if (Scribe.mode == 4)
			{
				if (this.unlockedPaths == null)
				{
					this.unlockedPaths = new List<PsycasterPathDef>();
				}
				if (this.previousUnlockedPaths == null)
				{
					this.previousUnlockedPaths = new List<PsycasterPathDef>();
				}
				this.RecacheCurStage();
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000B52C File Offset: 0x0000972C
		public void SpentPoints(int count = 1)
		{
			this.points -= count;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000B53C File Offset: 0x0000973C
		public void ImproveStats(int count = 1)
		{
			this.statPoints += count;
			this.RecacheCurStage();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000B552 File Offset: 0x00009752
		public void UnlockPath(PsycasterPathDef path)
		{
			this.unlockedPaths.Add(path);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000B560 File Offset: 0x00009760
		public void UnlockMeditationFocus(MeditationFocusDef focus)
		{
			this.unlockedMeditationFoci.Add(focus);
			MeditationFocusTypeAvailabilityCache.ClearFor(this.pawn);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000B579 File Offset: 0x00009779
		public bool ShouldShow(Ability ability)
		{
			return this.psysetIndex == this.psysets.Count || this.psysets[this.psysetIndex].Abilities.Contains(ability.def);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000B5B1 File Offset: 0x000097B1
		public void RemovePsySet(PsySet set)
		{
			this.psysets.Remove(set);
			this.psysetIndex = Mathf.Clamp(this.psysetIndex, 0, this.psysets.Count);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000B5E0 File Offset: 0x000097E0
		public static int ExperienceRequiredForLevel(int level)
		{
			int result;
			if (level <= 20)
			{
				if (level > 1)
				{
					result = Mathf.RoundToInt((float)Hediff_PsycastAbilities.ExperienceRequiredForLevel(level - 1) * 1.15f);
				}
				else
				{
					result = 100;
				}
			}
			else if (level > 30)
			{
				result = Mathf.RoundToInt((float)Hediff_PsycastAbilities.ExperienceRequiredForLevel(level - 1) * 1.05f);
			}
			else
			{
				result = Mathf.RoundToInt((float)Hediff_PsycastAbilities.ExperienceRequiredForLevel(level - 1) * 1.1f);
			}
			return result;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000B648 File Offset: 0x00009848
		public override void GiveRandomAbilityAtLevel(int? forLevel = null)
		{
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000B64C File Offset: 0x0000984C
		public override void Tick()
		{
			base.Tick();
			IChannelledPsycast channelledPsycast = this.currentlyChanneling;
			if (channelledPsycast != null && !channelledPsycast.IsActive)
			{
				this.currentlyChanneling = null;
			}
			if (this.minHeatGivers.RemoveAll((IMinHeatGiver giver) => giver == null || !giver.IsActive) > 0)
			{
				this.RecacheCurStage();
			}
		}

		// Token: 0x0400008B RID: 139
		private static readonly Texture2D PsySetNext = ContentFinder<Texture2D>.Get("UI/Gizmos/Psyset_Next", true);

		// Token: 0x0400008C RID: 140
		public float experience;

		// Token: 0x0400008D RID: 141
		public int maxLevelFromTitles;

		// Token: 0x0400008E RID: 142
		public int points;

		// Token: 0x0400008F RID: 143
		public List<PsycasterPathDef> previousUnlockedPaths = new List<PsycasterPathDef>();

		// Token: 0x04000090 RID: 144
		public Hediff_Psylink psylink;

		// Token: 0x04000091 RID: 145
		public List<PsySet> psysets = new List<PsySet>();

		// Token: 0x04000092 RID: 146
		public List<MeditationFocusDef> unlockedMeditationFoci = new List<MeditationFocusDef>();

		// Token: 0x04000093 RID: 147
		public List<PsycasterPathDef> unlockedPaths = new List<PsycasterPathDef>();

		// Token: 0x04000094 RID: 148
		private IChannelledPsycast currentlyChanneling;

		// Token: 0x04000095 RID: 149
		private HediffStage curStage;

		// Token: 0x04000096 RID: 150
		private List<IMinHeatGiver> minHeatGivers = new List<IMinHeatGiver>();

		// Token: 0x04000097 RID: 151
		private int psysetIndex;

		// Token: 0x04000098 RID: 152
		private int statPoints;
	}
}
