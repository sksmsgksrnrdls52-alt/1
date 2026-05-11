using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using VEF.Utils;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.UI
{
	// Token: 0x020000D8 RID: 216
	[StaticConstructorOnStartup]
	public static class PsycastsUIUtility
	{
		// Token: 0x060002E2 RID: 738 RVA: 0x00011AE4 File Offset: 0x0000FCE4
		static PsycastsUIUtility()
		{
			PsycastsUIUtility.meditationIcons = new Dictionary<MeditationFocusDef, Texture2D>();
			foreach (MeditationFocusDef meditationFocusDef in DefDatabase<MeditationFocusDef>.AllDefs)
			{
				MeditationFocusExtension modExtension = meditationFocusDef.GetModExtension<MeditationFocusExtension>();
				if (modExtension == null)
				{
					string str = "Please ask ";
					ModContentPack modContentPack = meditationFocusDef.modContentPack;
					string text;
					if (modContentPack == null)
					{
						text = null;
					}
					else
					{
						ModMetaData modMetaData = modContentPack.ModMetaData;
						text = ((modMetaData != null) ? modMetaData.AuthorsString : null);
					}
					string arg = str + (text ?? "its authors") + " to add one.";
					ModContentPack modContentPack2 = meditationFocusDef.modContentPack;
					if (modContentPack2 != null && modContentPack2.IsOfficialMod)
					{
						arg = "It's marked as an official DLC, and if that's the case then please report this to Vanilla Expanded team so it can receive an icon.";
					}
					Log.Warning(string.Format("MeditationFocusDef {0} does not have a MeditationFocusExtension, which means it will not have an icon in the Psycasts UI.\n{1}", meditationFocusDef, arg));
					PsycastsUIUtility.meditationIcons.Add(meditationFocusDef, BaseContent.WhiteTex);
				}
				else
				{
					PsycastsUIUtility.meditationIcons.Add(meditationFocusDef, ContentFinder<Texture2D>.Get(modExtension.icon, true));
					if (!GenList.NullOrEmpty<StatPart_Focus>(modExtension.statParts))
					{
						foreach (StatPart_Focus statPart_Focus in modExtension.statParts)
						{
							statPart_Focus.focus = meditationFocusDef;
							statPart_Focus.parentStat = StatDefOf.MeditationFocusStrength;
							StatDef meditationFocusGain = StatDefOf.MeditationFocusGain;
							if (meditationFocusGain.parts == null)
							{
								meditationFocusGain.parts = new List<StatPart>();
							}
							StatDefOf.MeditationFocusGain.parts.Add(statPart_Focus);
						}
					}
				}
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00011CC0 File Offset: 0x0000FEC0
		public static void LabelWithIcon(this Listing_Standard listing, Texture2D icon, string label)
		{
			float num = Text.CalcHeight(label, listing.ColumnWidth);
			Rect rect = listing.GetRect(num, 1f);
			float num2 = (float)icon.width * (num / (float)icon.height);
			GUI.DrawTexture(UIUtility.TakeLeftPart(ref rect, num2), icon);
			rect.xMin += 3f;
			Widgets.Label(rect, label);
			listing.Gap(3f);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00011D2C File Offset: 0x0000FF2C
		public static void StatDisplay(this Listing_Standard listing, Texture2D icon, StatDef stat, Thing thing)
		{
			listing.LabelWithIcon(icon, stat.LabelCap + ": " + stat.Worker.GetStatDrawEntryLabel(stat, StatExtension.GetStatValue(thing, stat, true, -1), stat.toStringNumberSense, StatRequest.For(thing), true));
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00011D7C File Offset: 0x0000FF7C
		public static Rect CenterRect(this Rect rect, Vector2 size)
		{
			return new Rect(rect.center - size / 2f, size);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00011D9B File Offset: 0x0000FF9B
		public static Texture2D Icon(this MeditationFocusDef def)
		{
			return PsycastsUIUtility.meditationIcons[def];
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00011DA8 File Offset: 0x0000FFA8
		public static void DrawPathBackground(ref Rect rect, PsycasterPathDef def, bool altTex = false)
		{
			Texture2D texture2D = altTex ? def.backgroundImage : def.altBackgroundImage;
			GUI.color = new ColorInt(97, 108, 122).ToColor;
			Widgets.DrawBox(GenUI.ExpandedBy(rect, 2f), 1, Texture2D.whiteTexture);
			GUI.color = Color.white;
			Rect rect2 = UIUtility.TakeBottomPart(ref rect, 30f);
			Widgets.DrawRectFast(rect2, Widgets.WindowBGFillColor, null);
			Text.Anchor = 4;
			Widgets.Label(rect2, def.LabelCap);
			GUI.DrawTexture(rect, texture2D);
			Text.Anchor = 0;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00011E3E File Offset: 0x0001003E
		private static bool EnsureInit()
		{
			if (PsycastsUIUtility.Hediff != null && PsycastsUIUtility.CompAbilities != null)
			{
				return true;
			}
			Log.Error("[VPE] PsycastsUIUtility was used without being initialized.");
			return false;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00011E5C File Offset: 0x0001005C
		public static void DoPathAbilities(Rect inRect, PsycasterPathDef path, Dictionary<AbilityDef, Vector2> abilityPos, Action<Rect, AbilityDef> doAbility)
		{
			if (!PsycastsUIUtility.EnsureInit())
			{
				return;
			}
			Func<AbilityDef, bool> <>9__0;
			foreach (AbilityDef abilityDef4 in path.abilities)
			{
				AbilityExtension_Psycast abilityExtension_Psycast = abilityDef4.Psycast();
				List<AbilityDef> list = (abilityExtension_Psycast != null) ? abilityExtension_Psycast.prerequisites : null;
				if (list != null && abilityPos.ContainsKey(abilityDef4))
				{
					IEnumerable<AbilityDef> source = list;
					Func<AbilityDef, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((AbilityDef abilityDef) => abilityPos.ContainsKey(abilityDef)));
					}
					foreach (AbilityDef abilityDef2 in source.Where(predicate))
					{
						Widgets.DrawLine(abilityPos[abilityDef4], abilityPos[abilityDef2], PsycastsUIUtility.CompAbilities.HasAbility(abilityDef2) ? Color.white : Color.grey, 2f);
					}
				}
			}
			for (int i = 0; i < path.abilityLevelsInOrder.Length; i++)
			{
				Rect rect;
				rect..ctor(inRect.x, inRect.y + (float)(path.MaxLevel - 1 - i) * inRect.height / (float)path.MaxLevel + 10f, inRect.width, inRect.height / 5f);
				AbilityDef[] array = path.abilityLevelsInOrder[i];
				for (int j = 0; j < array.Length; j++)
				{
					Rect arg;
					arg..ctor(rect.x + rect.width / 2f + PsycastsUIUtility.abilityTreeXOffsets[array.Length - 1][j], rect.y, 36f, 36f);
					AbilityDef abilityDef3 = array[j];
					if (abilityDef3 != PsycasterPathDef.Blank)
					{
						abilityPos[abilityDef3] = arg.center;
						doAbility(arg, abilityDef3);
					}
				}
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00012078 File Offset: 0x00010278
		public static void DrawAbility(Rect inRect, AbilityDef ability)
		{
			Color color = Mouse.IsOver(inRect) ? GenUI.MouseoverColor : Color.white;
			MouseoverSounds.DoRegion(inRect, SoundDefOf.Mouseover_Command);
			GUI.color = color;
			GUI.DrawTexture(inRect, Command.BGTexShrunk);
			GUI.color = Color.white;
			GUI.DrawTexture(inRect, ability.icon);
		}

		// Token: 0x04000175 RID: 373
		private static readonly Dictionary<MeditationFocusDef, Texture2D> meditationIcons;

		// Token: 0x04000176 RID: 374
		private static readonly float[][] abilityTreeXOffsets = new float[][]
		{
			new float[]
			{
				-18f
			},
			new float[]
			{
				-47f,
				11f
			},
			new float[]
			{
				-69f,
				-18f,
				33f
			}
		};

		// Token: 0x04000177 RID: 375
		public static Hediff_PsycastAbilities Hediff;

		// Token: 0x04000178 RID: 376
		public static CompAbilities CompAbilities;
	}
}
