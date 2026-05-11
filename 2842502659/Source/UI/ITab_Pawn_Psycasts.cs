using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using VEF.Utils;
using Verse;

namespace VanillaPsycastsExpanded.UI
{
	// Token: 0x020000D7 RID: 215
	[StaticConstructorOnStartup]
	public class ITab_Pawn_Psycasts : ITab
	{
		// Token: 0x060002D2 RID: 722 RVA: 0x00010750 File Offset: 0x0000E950
		static ITab_Pawn_Psycasts()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				RaceProperties race = thingDef.race;
				if (race != null && race.Humanlike)
				{
					List<Type> inspectorTabs = thingDef.inspectorTabs;
					if (inspectorTabs != null)
					{
						inspectorTabs.Add(typeof(ITab_Pawn_Psycasts));
					}
					List<InspectTabBase> inspectorTabsResolved = thingDef.inspectorTabsResolved;
					if (inspectorTabsResolved != null)
					{
						inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_Psycasts)));
					}
				}
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000107E4 File Offset: 0x0000E9E4
		public ITab_Pawn_Psycasts()
		{
			this.labelKey = "VPE.Psycasts";
			this.size = new Vector2((float)UI.screenWidth, (float)UI.screenHeight * 0.75f);
			this.pathsByTab = (from def in DefDatabase<PsycasterPathDef>.AllDefs
			group def by def.tab).ToDictionary((IGrouping<string, PsycasterPathDef> group) => group.Key, (IGrouping<string, PsycasterPathDef> group) => group.ToList<PsycasterPathDef>());
			this.foci = (from def in DefDatabase<MeditationFocusDef>.AllDefs
			orderby def.modContentPack.IsOfficialMod descending, def.label descending
			select def).ToList<MeditationFocusDef>();
			this.tabs = (from kv in this.pathsByTab
			select new TabRecord(kv.Key, delegate()
			{
				this.curTab = kv.Key;
			}, () => this.curTab == kv.Key)).ToList<TabRecord>();
			this.curTab = this.pathsByTab.Keys.FirstOrDefault<string>();
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0001092C File Offset: 0x0000EB2C
		public Vector2 Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00010934 File Offset: 0x0000EB34
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0001093C File Offset: 0x0000EB3C
		public float RequestedPsysetsHeight { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00010948 File Offset: 0x0000EB48
		public override bool IsVisible
		{
			get
			{
				Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
				if (pawn != null && pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_PsycastAbilityImplant, false))
				{
					Faction faction = pawn.Faction;
					return faction != null && faction.IsPlayer;
				}
				return false;
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00010994 File Offset: 0x0000EB94
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size.y = this.PaneTopY - 30f;
			this.pathsPerRow = Mathf.FloorToInt(this.size.x * 0.67f / 200f);
			MultiCheckboxState multiCheckboxState = PsycastsMod.Settings.smallMode;
			bool flag = multiCheckboxState == null || (multiCheckboxState != 1 && this.size.y <= 1080f / Prefs.UIScale);
			this.smallMode = flag;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00010A1F File Offset: 0x0000EC1F
		public override void OnOpen()
		{
			base.OnOpen();
			this.pawn = (Pawn)Find.Selector.SingleSelectedThing;
			this.InitCache();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00010A44 File Offset: 0x0000EC44
		private void InitCache()
		{
			PsycastsUIUtility.Hediff = (this.hediff = this.pawn.Psycasts());
			PsycastsUIUtility.CompAbilities = (this.compAbilities = this.pawn.GetComp<CompAbilities>());
			this.abilityPos.Clear();
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00010A90 File Offset: 0x0000EC90
		protected override void CloseTab()
		{
			base.CloseTab();
			this.pawn = null;
			PsycastsUIUtility.Hediff = (this.hediff = null);
			PsycastsUIUtility.CompAbilities = (this.compAbilities = null);
			this.abilityPos.Clear();
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00010AD4 File Offset: 0x0000ECD4
		protected override void FillTab()
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn != null && this.pawn != pawn)
			{
				this.pawn = pawn;
				this.InitCache();
			}
			if (this.devMode && !Prefs.DevMode)
			{
				this.devMode = false;
			}
			if (this.pawn == null || this.hediff == null || this.compAbilities == null)
			{
				return;
			}
			GameFont font = Text.Font;
			TextAnchor anchor = Text.Anchor;
			Rect rect;
			rect..ctor(Vector2.one * 20f, this.size - Vector2.one * 40f);
			Rect rect2 = UIUtility.TakeLeftPart(ref rect, this.size.x * 0.3f);
			Rect rect3 = GenUI.ContractedBy(rect, 5f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect2);
			Text.Font = 2;
			listing_Standard.Label(this.pawn.Name.ToStringFull, -1f, null);
			listing_Standard.Label(TranslatorFormattedStringExtensions.Translate("VPE.PsyLevel", this.hediff.level), -1f, null);
			listing_Standard.Gap(10f);
			if (this.hediff.level < PsycastsMod.Settings.maxLevel)
			{
				Rect rect4 = GenUI.ContractedBy(listing_Standard.GetRect(60f, 1f), 10f, 0f);
				Text.Anchor = 4;
				int num = Hediff_PsycastAbilities.ExperienceRequiredForLevel(this.hediff.level + 1);
				if (this.devMode)
				{
					Text.Font = 1;
					if (Widgets.ButtonText(UIUtility.TakeRightPart(ref rect4, 80f), "Dev: Level up", true, true, true, null))
					{
						this.hediff.GainExperience((float)num, false);
					}
					Text.Font = 2;
				}
				Widgets.FillableBar(rect4, this.hediff.experience / (float)num);
				Widgets.Label(rect4, string.Format("{0} / {1}", GenText.ToStringByStyle(this.hediff.experience, 1, 1), num));
				Text.Font = 0;
				listing_Standard.Label(Translator.Translate("VPE.EarnXP"), -1f, null);
				listing_Standard.Gap(10f);
			}
			Text.Font = 1;
			Text.Anchor = 0;
			listing_Standard.Label(TranslatorFormattedStringExtensions.Translate("VPE.Points", this.hediff.points), -1f, null);
			Text.Font = 0;
			listing_Standard.Label(Translator.Translate("VPE.SpendPoints"), -1f, null);
			listing_Standard.Gap(3f);
			Text.Anchor = 3;
			Text.Font = 1;
			float curHeight = listing_Standard.CurHeight;
			if (listing_Standard.ButtonTextLabeled(Translator.Translate("VPE.PsycasterStats") + (this.smallMode ? string.Format(" ({0})", Translator.Translate("VPE.Hover")) : ""), Translator.Translate("VPE.Upgrade"), 0, null, null))
			{
				int num2 = GenUI.CurrentAdjustmentMultiplier();
				if (this.devMode)
				{
					this.hediff.ImproveStats(num2);
				}
				else if (this.hediff.points >= num2)
				{
					this.hediff.SpentPoints(num2);
					this.hediff.ImproveStats(num2);
				}
				else
				{
					Messages.Message(Translator.Translate("VPE.NotEnoughPoints"), MessageTypeDefOf.RejectInput, false);
				}
			}
			float curHeight2 = listing_Standard.CurHeight;
			if (this.smallMode)
			{
				if (Mouse.IsOver(new Rect(rect2.x, curHeight, rect2.width / 2f, curHeight2 - curHeight)))
				{
					Vector2 size = new Vector2(rect2.width, 150f);
					Find.WindowStack.ImmediateWindow(9040170, new Rect(GenUI.GetMouseAttachedWindowPos(size.x, size.y), size), 3, delegate()
					{
						Listing_Standard listing_Standard2 = new Listing_Standard();
						listing_Standard2.Begin(new Rect(Vector2.one * 5f, size));
						listing_Standard2.StatDisplay(TexPsycasts.IconNeuralHeatLimit, StatDefOf.PsychicEntropyMax, this.pawn);
						listing_Standard2.StatDisplay(TexPsycasts.IconNeuralHeatRegenRate, StatDefOf.PsychicEntropyRecoveryRate, this.pawn);
						listing_Standard2.StatDisplay(TexPsycasts.IconPsychicSensitivity, StatDefOf.PsychicSensitivity, this.pawn);
						if (PsycastsMod.Settings.changeFocusGain)
						{
							listing_Standard2.StatDisplay(TexPsycasts.IconPsyfocusGain, StatDefOf.MeditationFocusGain, this.pawn);
						}
						listing_Standard2.StatDisplay(TexPsycasts.IconPsyfocusCost, VPE_DefOf.VPE_PsyfocusCostFactor, this.pawn);
						listing_Standard2.End();
					}, true, false, 1f, null, false);
				}
			}
			else
			{
				listing_Standard.StatDisplay(TexPsycasts.IconNeuralHeatLimit, StatDefOf.PsychicEntropyMax, this.pawn);
				listing_Standard.StatDisplay(TexPsycasts.IconNeuralHeatRegenRate, StatDefOf.PsychicEntropyRecoveryRate, this.pawn);
				listing_Standard.StatDisplay(TexPsycasts.IconPsychicSensitivity, StatDefOf.PsychicSensitivity, this.pawn);
				if (PsycastsMod.Settings.changeFocusGain)
				{
					listing_Standard.StatDisplay(TexPsycasts.IconPsyfocusGain, StatDefOf.MeditationFocusGain, this.pawn);
				}
				listing_Standard.StatDisplay(TexPsycasts.IconPsyfocusCost, VPE_DefOf.VPE_PsyfocusCostFactor, this.pawn);
			}
			listing_Standard.LabelWithIcon(TexPsycasts.IconFocusTypes, Translator.Translate("VPE.FocusTypes"));
			Text.Anchor = 0;
			Rect rect5 = listing_Standard.GetRect(48f, 1f);
			float num3 = rect2.x;
			foreach (MeditationFocusDef def in this.foci)
			{
				if (num3 + 50f >= rect2.width)
				{
					num3 = rect2.x;
					listing_Standard.Gap(3f);
					rect5 = listing_Standard.GetRect(48f, 1f);
				}
				Rect inRect;
				inRect..ctor(num3, rect5.y, 48f, 48f);
				this.DoFocus(inRect, def);
				num3 += 50f;
			}
			listing_Standard.Gap(10f);
			if (this.smallMode)
			{
				if (listing_Standard.ButtonTextLabeled(Translator.Translate("VPE.PsysetCustomize"), Translator.Translate("VPE.Edit"), 0, null, null))
				{
					Find.WindowStack.Add(new Dialog_EditPsysets(this));
				}
			}
			else
			{
				listing_Standard.Label(Translator.Translate("VPE.PsysetCustomize"), -1f, null);
			}
			Text.Font = 0;
			listing_Standard.Label(Translator.Translate("VPE.PsysetDesc"), -1f, null);
			Rect rect7;
			if (!this.smallMode)
			{
				float num4 = rect2.height - listing_Standard.CurHeight;
				num4 -= 30f;
				if (Prefs.DevMode)
				{
					num4 -= 30f;
				}
				Rect rect6 = listing_Standard.GetRect(num4, 1f);
				Widgets.DrawMenuSection(rect6);
				rect7..ctor(0f, 0f, rect6.width - 20f, this.RequestedPsysetsHeight);
				Widgets.BeginScrollView(GenUI.ContractedBy(rect6, 3f, 6f), ref this.psysetsScrollPos, rect7, true);
				this.DoPsysets(rect7);
				Widgets.EndScrollView();
			}
			listing_Standard.CheckboxLabeled(Translator.Translate("VPE.UseAltBackground"), ref this.useAltBackgrounds, null, 0f, 1f);
			if (Prefs.DevMode)
			{
				listing_Standard.CheckboxLabeled(Translator.Translate("VPE.DevMode"), ref this.devMode, null, 0f, 1f);
			}
			listing_Standard.End();
			if (GenDictionary.NullOrEmpty<string, List<PsycasterPathDef>>(this.pathsByTab))
			{
				Text.Anchor = 4;
				Text.Font = 2;
				Widgets.DrawMenuSection(rect3);
				Widgets.Label(rect3, "No Paths");
			}
			else
			{
				TabDrawer.DrawTabs<TabRecord>(new Rect(rect3.x, rect3.y + 40f, rect3.width, rect3.height), this.tabs, 200f);
				rect3.yMin += 40f;
				Widgets.DrawMenuSection(rect3);
				rect7..ctor(0f, 0f, rect3.width - 20f, this.lastPathsHeight);
				Widgets.BeginScrollView(GenUI.ContractedBy(rect3, 2f), ref this.pathsScrollPos, rect7, true);
				this.DoPaths(rect7);
				Widgets.EndScrollView();
			}
			Text.Font = font;
			Text.Anchor = anchor;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x000112B4 File Offset: 0x0000F4B4
		private void DoFocus(Rect inRect, MeditationFocusDef def)
		{
			Widgets.DrawBox(inRect, 3, Texture2D.grayTexture);
			bool flag = def.CanPawnUse(this.pawn);
			string str;
			bool flag2 = def.CanUnlock(this.pawn, out str);
			GUI.color = (flag ? Color.white : Color.gray);
			GUI.DrawTexture(GenUI.ContractedBy(inRect, 5f), def.Icon());
			GUI.color = Color.white;
			TooltipHandler.TipRegion(inRect, def.LabelCap + (GenText.NullOrEmpty(def.description) ? "" : "\n\n") + def.description + (flag2 ? "" : ("\n\n" + str)));
			Widgets.DrawHighlightIfMouseover(inRect);
			if ((this.hediff.points >= 1 || this.devMode) && !flag && (flag2 || this.devMode) && Widgets.ButtonText(new Rect(inRect.xMax - 13f, inRect.yMax - 13f, 12f, 12f), "▲", true, true, true, null))
			{
				if (!this.devMode)
				{
					this.hediff.SpentPoints(1);
				}
				this.hediff.UnlockMeditationFocus(def);
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000113FC File Offset: 0x0000F5FC
		public void DoPsysets(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			foreach (PsySet psySet in this.hediff.psysets.ToList<PsySet>())
			{
				Rect rect = listing_Standard.GetRect(30f, 1f);
				Widgets.Label(GenUI.LeftHalf(GenUI.LeftHalf(rect)), psySet.Name);
				if (Widgets.ButtonText(GenUI.RightHalf(GenUI.LeftHalf(rect)), Translator.Translate("VPE.Rename"), true, true, true, null))
				{
					Find.WindowStack.Add(new Dialog_RenamePsyset(psySet));
				}
				if (Widgets.ButtonText(GenUI.LeftHalf(GenUI.RightHalf(rect)), Translator.Translate("VPE.Edit"), true, true, true, null))
				{
					Find.WindowStack.Add(new Dialog_Psyset(psySet, this.pawn));
				}
				if (Widgets.ButtonText(GenUI.RightHalf(GenUI.RightHalf(rect)), Translator.Translate("VPE.Remove"), true, true, true, null))
				{
					this.hediff.RemovePsySet(psySet);
				}
			}
			if (Widgets.ButtonText(GenUI.ContractedBy(GenUI.LeftHalf(listing_Standard.GetRect(70f, 1f)), 5f), Translator.Translate("VPE.CreatePsyset"), true, true, true, null))
			{
				PsySet psySet2 = new PsySet
				{
					Name = Translator.Translate("VPE.Untitled")
				};
				this.hediff.psysets.Add(psySet2);
				Find.WindowStack.Add(new Dialog_Psyset(psySet2, this.pawn));
			}
			this.RequestedPsysetsHeight = listing_Standard.CurHeight + 70f;
			listing_Standard.End();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000115E4 File Offset: 0x0000F7E4
		private void DoPaths(Rect inRect)
		{
			Vector2 vector = inRect.position + Vector2.one * 10f;
			float num = (inRect.width - (float)(this.pathsPerRow + 1) * 10f) / (float)this.pathsPerRow;
			float num2 = 0f;
			int num3 = this.pathsPerRow;
			using (IEnumerator<PsycasterPathDef> enumerator = this.pathsByTab[this.curTab].OrderByDescending(new Func<PsycasterPathDef, bool>(this.hediff.unlockedPaths.Contains)).ThenBy((PsycasterPathDef path) => path.order).ThenBy((PsycasterPathDef path) => path.label).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PsycasterPathDef def = enumerator.Current;
					Texture2D texture2D = this.useAltBackgrounds ? def.backgroundImage : def.altBackgroundImage;
					float num4 = num / (float)texture2D.width * (float)texture2D.height + 30f;
					Rect rect;
					rect..ctor(vector, new Vector2(num, num4));
					PsycastsUIUtility.DrawPathBackground(ref rect, def, this.useAltBackgrounds);
					if (this.hediff.unlockedPaths.Contains(def))
					{
						if (def.HasAbilities)
						{
							PsycastsUIUtility.DoPathAbilities(rect, def, this.abilityPos, new Action<Rect, AbilityDef>(this.DoAbility));
						}
					}
					else
					{
						Widgets.DrawRectFast(rect, new Color(0f, 0f, 0f, this.useAltBackgrounds ? 0.7f : 0.55f), null);
						if (this.hediff.points >= 1 || this.devMode)
						{
							Rect rect2 = rect.CenterRect(new Vector2(140f, 30f));
							if (this.devMode || def.CanPawnUnlock(this.pawn))
							{
								if (Widgets.ButtonText(rect2, Translator.Translate("VPE.Unlock"), true, true, true, null))
								{
									if (!this.devMode)
									{
										this.hediff.SpentPoints(1);
									}
									this.hediff.UnlockPath(def);
								}
							}
							else
							{
								GUI.color = Color.grey;
								string text = Translator.Translate("VPE.Locked").Resolve() + ": " + def.lockedReason;
								rect2.width = Mathf.Max(rect2.width, Text.CalcSize(text).x + 10f);
								Widgets.ButtonText(rect2, text, true, true, false, null);
								GUI.color = Color.white;
							}
						}
						TooltipHandler.TipRegion(rect, () => def.tooltip + "\n\n" + Translator.Translate("VPE.AbilitiesList") + "\n" + GenText.ToLineList(from ab in def.abilities
						select ab.label, "  ", true), def.GetHashCode());
					}
					num2 = Mathf.Max(num2, num4 + 10f);
					vector.x += num + 10f;
					num3--;
					if (num3 == 0)
					{
						vector.x = inRect.x + 10f;
						vector.y += num2;
						num3 = this.pathsPerRow;
						num2 = 0f;
					}
				}
			}
			this.lastPathsHeight = vector.y + num2;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00011984 File Offset: 0x0000FB84
		private void DoAbility(Rect inRect, AbilityDef ability)
		{
			bool unlockable = false;
			bool flag = false;
			if (!this.compAbilities.HasAbility(ability))
			{
				if (this.devMode || (ability.Psycast().PrereqsCompleted(this.compAbilities) && this.hediff.points >= 1))
				{
					unlockable = true;
				}
				else
				{
					flag = true;
				}
			}
			if (unlockable)
			{
				Widgets.DrawStrongHighlight(GenUI.ExpandedBy(inRect, 12f), null);
			}
			PsycastsUIUtility.DrawAbility(inRect, ability);
			if (flag)
			{
				Widgets.DrawRectFast(inRect, new Color(0f, 0f, 0f, 0.6f), null);
			}
			TooltipHandler.TipRegion(inRect, () => string.Format("{0}\n\n{1}{2}", ability.LabelCap, ability.description, unlockable ? ("\n\n" + Translator.Translate("VPE.ClickToUnlock").Resolve().ToUpper()) : ""), ability.GetHashCode());
			if (unlockable && Widgets.ButtonInvisible(inRect, true))
			{
				if (!this.devMode)
				{
					this.hediff.SpentPoints(1);
				}
				this.compAbilities.GiveAbility(ability);
			}
		}

		// Token: 0x04000165 RID: 357
		private readonly Dictionary<AbilityDef, Vector2> abilityPos = new Dictionary<AbilityDef, Vector2>();

		// Token: 0x04000166 RID: 358
		private readonly List<MeditationFocusDef> foci;

		// Token: 0x04000167 RID: 359
		private readonly Dictionary<string, List<PsycasterPathDef>> pathsByTab;

		// Token: 0x04000168 RID: 360
		private readonly List<TabRecord> tabs;

		// Token: 0x04000169 RID: 361
		private CompAbilities compAbilities;

		// Token: 0x0400016A RID: 362
		private string curTab;

		// Token: 0x0400016B RID: 363
		private bool devMode;

		// Token: 0x0400016C RID: 364
		private Hediff_PsycastAbilities hediff;

		// Token: 0x0400016D RID: 365
		private float lastPathsHeight;

		// Token: 0x0400016E RID: 366
		private int pathsPerRow;

		// Token: 0x0400016F RID: 367
		private Vector2 pathsScrollPos;

		// Token: 0x04000170 RID: 368
		private Pawn pawn;

		// Token: 0x04000171 RID: 369
		private Vector2 psysetsScrollPos;

		// Token: 0x04000172 RID: 370
		private bool smallMode;

		// Token: 0x04000173 RID: 371
		private bool useAltBackgrounds;
	}
}
