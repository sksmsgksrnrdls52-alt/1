using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000BE RID: 190
	public class PsycastsMod : Mod
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0000E260 File Offset: 0x0000C460
		public PsycastsMod(ModContentPack content) : base(content)
		{
			PsycastsMod.Harm = new Harmony("OskarPotocki.VanillaPsycastsExpanded");
			PsycastsMod.Settings = base.GetSettings<PsycastSettings>();
			PsycastsMod.Harm.Patch(AccessTools.Method(typeof(ThingDefGenerator_Neurotrainer), "ImpliedThingDefs", null, null), null, new HarmonyMethod(typeof(ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch), "Postfix", null), null, null);
			PsycastsMod.Harm.Patch(AccessTools.Method(typeof(GenDefDatabase), "GetDef", null, null), new HarmonyMethod(base.GetType(), "PreGetDef", null), null, null, null);
			PsycastsMod.Harm.Patch(AccessTools.Method(typeof(GenDefDatabase), "GetDefSilentFail", null, null), new HarmonyMethod(base.GetType(), "PreGetDef", null), null, null, null);
			List<BackCompatibilityConverter> list = (List<BackCompatibilityConverter>)AccessTools.Field(typeof(BackCompatibility), "conversionChain").GetValue(null);
			list.Add(PsycastsMod.psytrainerConverter = new BackCompatibilityConverter_Psytrainers());
			list.Add(new BackCompatibilityConverter_Constructs());
			if (ModsConfig.IsActive("GhostRolly.Rim73") || ModsConfig.IsActive("GhostRolly.Rim73_steam"))
			{
				Log.Warning("Vanilla Psycasts Expanded detected Rim73 mod. The mod is throttling hediff ticking which breaks psycast hediffs. You can turn off Rim73 hediff optimization in its mod settings to ensure proper work of Vanilla Psycasts Expanded.");
			}
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplySettings));
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000E39E File Offset: 0x0000C59E
		public override string SettingsCategory()
		{
			return Translator.Translate("VanillaPsycastsExpanded");
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000E3AF File Offset: 0x0000C5AF
		public override void WriteSettings()
		{
			base.WriteSettings();
			this.ApplySettings();
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000E3BD File Offset: 0x0000C5BD
		private void ApplySettings()
		{
			HediffDefOf.PsychicAmplifier.maxSeverity = (float)PsycastsMod.Settings.maxLevel;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000E3D4 File Offset: 0x0000C5D4
		public override void DoSettingsWindowContents(Rect inRect)
		{
			base.DoSettingsWindowContents(inRect);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect);
			listing_Standard.Label(Translator.Translate("VPE.XPPerPercent") + ": " + PsycastsMod.Settings.XPPerPercent.ToString(), -1f, null);
			PsycastsMod.Settings.XPPerPercent = listing_Standard.Slider(PsycastsMod.Settings.XPPerPercent, 0f, 10f);
			listing_Standard.Label(Translator.Translate("VPE.PsycasterSpawnBaseChance") + ": " + (PsycastsMod.Settings.baseSpawnChance * 100f).ToString() + "%", -1f, null);
			PsycastsMod.Settings.baseSpawnChance = listing_Standard.Slider(PsycastsMod.Settings.baseSpawnChance, 0f, 1f);
			listing_Standard.Label(Translator.Translate("VPE.PsycasterSpawnAdditional") + ": " + (PsycastsMod.Settings.additionalAbilityChance * 100f).ToString() + "%", -1f, null);
			PsycastsMod.Settings.additionalAbilityChance = listing_Standard.Slider(PsycastsMod.Settings.additionalAbilityChance, 0f, 1f);
			listing_Standard.CheckboxLabeled(Translator.Translate("VPE.AllowShrink"), ref PsycastsMod.Settings.shrink, Translator.Translate("VPE.AllowShrink.Desc"), 0f, 1f);
			listing_Standard.CheckboxMultiLabeled(Translator.Translate("VPE.SmallMode"), ref PsycastsMod.Settings.smallMode, Translator.Translate("VPE.SmallMode.Desc"));
			listing_Standard.CheckboxLabeled(Translator.Translate("VPE.MuteSkipdoor"), ref PsycastsMod.Settings.muteSkipdoor, null, 0f, 1f);
			listing_Standard.Label(Translator.Translate("VPE.MaxLevel") + ": " + PsycastsMod.Settings.maxLevel.ToString(), -1f, null);
			PsycastsMod.Settings.maxLevel = (int)listing_Standard.Slider((float)PsycastsMod.Settings.maxLevel, 1f, 300f);
			listing_Standard.CheckboxLabeled(Translator.Translate("VPE.ChangeFocusGain"), ref PsycastsMod.Settings.changeFocusGain, Translator.Translate("VPE.ChangeFocusGain.Desc"), 0f, 1f);
			listing_Standard.End();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000E67C File Offset: 0x0000C87C
		public static void PreGetDef(Type __0, ref string __1, bool __2)
		{
			if (__2)
			{
				string text = PsycastsMod.psytrainerConverter.BackCompatibleDefName(__0, __1, false, null);
				if (text != null)
				{
					__1 = text;
				}
			}
		}

		// Token: 0x040000CC RID: 204
		public static Harmony Harm;

		// Token: 0x040000CD RID: 205
		public static PsycastSettings Settings;

		// Token: 0x040000CE RID: 206
		private static BackCompatibilityConverter_Psytrainers psytrainerConverter;
	}
}
