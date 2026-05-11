using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.UI
{
	// Token: 0x020000D9 RID: 217
	[StaticConstructorOnStartup]
	public class PsychicStatusGizmo : Gizmo
	{
		// Token: 0x060002EB RID: 747 RVA: 0x000120CC File Offset: 0x000102CC
		public PsychicStatusGizmo(Pawn_PsychicEntropyTracker tracker)
		{
			this.tracker = tracker;
			this.Order = -100f;
			this.LimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Limited", true);
			this.UnlimitedTex = ContentFinder<Texture2D>.Get("UI/Icons/EntropyLimit/Unlimited", true);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00012120 File Offset: 0x00010320
		private static void DrawThreshold(Rect rect, float percent, float entropyValue)
		{
			Rect rect2 = default(Rect);
			rect2.x = rect.x + 3f + (rect.width - 8f) * percent;
			rect2.y = rect.y + rect.height - 9f;
			rect2.width = 2f;
			rect2.height = 6f;
			Rect rect3 = rect2;
			if (entropyValue < percent)
			{
				GUI.DrawTexture(rect3, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(rect3, BaseContent.BlackTex);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000121AC File Offset: 0x000103AC
		private static void DrawPsyfocusTarget(Rect rect, float percent)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			Rect rect2 = default(Rect);
			rect2.x = rect.x + 3f + num;
			rect2.y = rect.y;
			rect2.width = 2f;
			rect2.height = rect.height;
			GUI.DrawTexture(rect2, PsychicStatusGizmo.PsyfocusTargetTex);
			float num2 = UIScaling.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = UIScaling.AdjustCoordToUIScalingCeil(num2 + 4f);
			rect2 = default(Rect);
			rect2.y = rect.y - 3f;
			rect2.height = 5f;
			rect2.xMin = num2;
			rect2.xMax = xMax;
			Rect rect3 = rect2;
			GUI.DrawTexture(rect3, PsychicStatusGizmo.PsyfocusTargetTex);
			Rect rect4 = rect3;
			rect4.y = rect.yMax - 2f;
			GUI.DrawTexture(rect4, PsychicStatusGizmo.PsyfocusTargetTex);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000122A8 File Offset: 0x000104A8
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = GenUI.ContractedBy(rect, 6f);
			float num = Mathf.Repeat(Time.time, 0.85f);
			Command_Ability command_Ability = MapGizmoUtility.LastMouseOverGizmo as Command_Ability;
			AbilityExtension_Psycast abilityExtension_Psycast;
			if (command_Ability == null)
			{
				abilityExtension_Psycast = null;
			}
			else
			{
				Ability ability = command_Ability.ability;
				if (ability == null)
				{
					abilityExtension_Psycast = null;
				}
				else
				{
					AbilityDef def = ability.def;
					abilityExtension_Psycast = ((def != null) ? def.GetModExtension<AbilityExtension_Psycast>() : null);
				}
			}
			AbilityExtension_Psycast abilityExtension_Psycast2 = abilityExtension_Psycast;
			float num2;
			if (num >= 0.1f)
			{
				if (num < 0.25f)
				{
					num2 = 1f;
				}
				else
				{
					num2 = 1f - (num - 0.25f) / 0.6f;
				}
			}
			else
			{
				num2 = num / 0.1f;
			}
			float num3 = num2;
			Widgets.DrawWindowBackground(rect);
			Text.Font = 1;
			Rect rect3 = rect2;
			rect3.y += 6f;
			rect3.height = Text.LineHeight;
			Widgets.Label(rect3, Translator.Translate("PsychicEntropyShort"));
			Rect rect4 = rect2;
			rect4.y += 38f;
			rect4.height = Text.LineHeight;
			Widgets.Label(rect4, Translator.Translate("PsyfocusLabelGizmo"));
			Rect rect5 = rect2;
			rect5.x += 63f;
			rect5.y += 6f;
			rect5.width = 100f;
			rect5.height = 22f;
			float entropyRelativeValue = this.tracker.EntropyRelativeValue;
			Widgets.FillableBar(rect5, Mathf.Min(entropyRelativeValue, 1f), PsychicStatusGizmo.EntropyBarTex, PsychicStatusGizmo.EmptyBarTex, true);
			if (this.tracker.EntropyValue > this.tracker.MaxEntropy)
			{
				Widgets.FillableBar(rect5, Mathf.Min(entropyRelativeValue - 1f, 1f), PsychicStatusGizmo.OverLimitBarTex, PsychicStatusGizmo.EntropyBarTex, true);
			}
			if (abilityExtension_Psycast2 != null)
			{
				float entropyUsedByPawn = abilityExtension_Psycast2.GetEntropyUsedByPawn(this.tracker.Pawn);
				if (entropyUsedByPawn > 1E-45f)
				{
					Rect rect6 = GenUI.ContractedBy(rect5, 3f);
					float width = rect6.width;
					float num4 = this.tracker.EntropyToRelativeValue(this.tracker.EntropyValue + entropyUsedByPawn);
					float num5 = entropyRelativeValue;
					if (num5 > 1f)
					{
						num5 -= 1f;
						num4 -= 1f;
					}
					rect6.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect6.xMin + num5 * width);
					rect6.width = UIScaling.AdjustCoordToUIScalingFloor(Mathf.Max(Mathf.Min(num4, 1f) - num5, 0f) * width);
					GUI.color = new Color(1f, 1f, 1f, num3 * 0.7f);
					GenUI.DrawTextureWithMaterial(rect6, PsychicStatusGizmo.EntropyBarTexAdd, null, default(Rect));
					GUI.color = Color.white;
				}
			}
			if (this.tracker.EntropyValue > this.tracker.MaxEntropy)
			{
				foreach (KeyValuePair<PsychicEntropySeverity, float> keyValuePair in Pawn_PsychicEntropyTracker.EntropyThresholds)
				{
					if (keyValuePair.Value > 1f && keyValuePair.Value < 2f)
					{
						PsychicStatusGizmo.DrawThreshold(rect5, keyValuePair.Value - 1f, entropyRelativeValue);
					}
				}
			}
			string text = this.tracker.EntropyValue.ToString("F0") + " / " + this.tracker.MaxEntropy.ToString("F0");
			Text.Font = 1;
			Text.Anchor = 4;
			Widgets.Label(rect5, text);
			Text.Anchor = 0;
			Text.Font = 0;
			GUI.color = Color.white;
			Rect rect7 = rect2;
			rect7.width = 175f;
			rect7.height = 38f;
			TooltipHandler.TipRegion(rect7, delegate()
			{
				float num9 = this.tracker.EntropyValue / this.tracker.RecoveryRate;
				return string.Format(Translator.Translate("PawnTooltipPsychicEntropyStats"), new object[]
				{
					Mathf.Round(this.tracker.EntropyValue),
					Mathf.Round(this.tracker.MaxEntropy),
					this.tracker.RecoveryRate.ToString("0.#"),
					Mathf.Round(num9)
				}) + "\n\n" + Translator.Translate("PawnTooltipPsychicEntropyDesc");
			}, Gen.HashCombineInt(this.tracker.GetHashCode(), 133858));
			Rect rect8 = rect2;
			rect8.x += 63f;
			rect8.y += 38f;
			rect8.width = 100f;
			rect8.height = 22f;
			bool flag = Mouse.IsOver(rect8);
			Widgets.FillableBar(rect8, Mathf.Min(this.tracker.CurrentPsyfocus, 1f), flag ? PsychicStatusGizmo.PsyfocusBarHighlightTex : PsychicStatusGizmo.PsyfocusBarTex, PsychicStatusGizmo.EmptyBarTex, true);
			if (abilityExtension_Psycast2 != null)
			{
				float psyfocusUsedByPawn = abilityExtension_Psycast2.GetPsyfocusUsedByPawn(this.tracker.Pawn);
				if (psyfocusUsedByPawn > 1E-45f)
				{
					Rect rect9 = GenUI.ContractedBy(rect8, 3f);
					float num6 = Mathf.Max(this.tracker.CurrentPsyfocus - psyfocusUsedByPawn, 0f);
					float width2 = rect9.width;
					rect9.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect9.xMin + num6 * width2);
					rect9.width = UIScaling.AdjustCoordToUIScalingCeil((this.tracker.CurrentPsyfocus - num6) * width2);
					GUI.color = new Color(1f, 1f, 1f, num3);
					GenUI.DrawTextureWithMaterial(rect9, PsychicStatusGizmo.PsyfocusBarTexReduce, null, default(Rect));
					GUI.color = Color.white;
				}
			}
			for (int i = 1; i < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; i++)
			{
				PsychicStatusGizmo.DrawThreshold(rect8, Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i], this.tracker.CurrentPsyfocus);
			}
			float num7 = Mathf.Clamp(Mathf.Round((Event.current.mousePosition.x - (rect8.x + 3f)) / (rect8.width - 8f) * 16f) / 16f, 0f, 1f);
			Event current = Event.current;
			if (current.type == null && current.button == 0 && flag)
			{
				this.selectedPsyfocusTarget = num7;
				this.draggingPsyfocusBar = true;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.MeditationDesiredPsyfocus, 6);
				SoundStarter.PlayOneShotOnCamera(SoundDefOf.DragSlider, null);
				current.Use();
			}
			if (current.type == 3 && current.button == 0 && this.draggingPsyfocusBar && flag)
			{
				if (Math.Abs(num7 - this.selectedPsyfocusTarget) > 1E-45f)
				{
					SoundStarter.PlayOneShotOnCamera(SoundDefOf.DragSlider, null);
				}
				this.selectedPsyfocusTarget = num7;
				current.Use();
			}
			if (current.type == 1 && current.button == 0 && this.draggingPsyfocusBar)
			{
				if (this.selectedPsyfocusTarget >= 0f)
				{
					this.tracker.SetPsyfocusTarget(this.selectedPsyfocusTarget);
				}
				this.selectedPsyfocusTarget = -1f;
				this.draggingPsyfocusBar = false;
				current.Use();
			}
			UIHighlighter.HighlightOpportunity(rect8, "PsyfocusBar");
			PsychicStatusGizmo.DrawPsyfocusTarget(rect8, this.draggingPsyfocusBar ? this.selectedPsyfocusTarget : this.tracker.TargetPsyfocus);
			GUI.color = Color.white;
			Rect rect10 = rect2;
			rect10.y += 38f;
			rect10.width = 175f;
			rect10.height = 38f;
			TooltipHandler.TipRegion(rect10, () => this.tracker.PsyfocusTipString(this.selectedPsyfocusTarget), Gen.HashCombineInt(this.tracker.GetHashCode(), 133873));
			if (this.tracker.Pawn.IsColonistPlayerControlled)
			{
				Rect rect11 = new Rect(rect2.x + (rect2.width - 32f), rect2.y + (rect2.height / 2f - 32f + 4f), 32f, 32f);
				if (Widgets.ButtonImage(rect11, this.tracker.limitEntropyAmount ? this.LimitedTex : this.UnlimitedTex, true, null))
				{
					this.tracker.limitEntropyAmount = !this.tracker.limitEntropyAmount;
					if (this.tracker.limitEntropyAmount)
					{
						SoundStarter.PlayOneShotOnCamera(SoundDefOf.Tick_Low, null);
					}
					else
					{
						SoundStarter.PlayOneShotOnCamera(SoundDefOf.Tick_High, null);
					}
				}
				TooltipHandler.TipRegionByKey(rect11, "PawnTooltipPsychicEntropyLimit");
			}
			float num8;
			if (PsychicStatusGizmo.TryGetPainMultiplier(this.tracker.Pawn, out num8))
			{
				Text.Font = 1;
				Text.Anchor = 4;
				string recoveryBonus = GenText.ToStringPercent(num8 - 1f, "F0");
				float widthCached = GenUI.GetWidthCached(recoveryBonus);
				Rect rect12 = rect2;
				rect12.x += rect2.width - widthCached / 2f - 16f;
				rect12.y += 38f;
				rect12.width = widthCached;
				rect12.height = Text.LineHeight;
				GUI.color = PsychicStatusGizmo.PainBoostColor;
				Widgets.Label(rect12, recoveryBonus);
				GUI.color = Color.white;
				Text.Font = 0;
				Text.Anchor = 0;
				TooltipHandler.TipRegion(GenUI.ContractedBy(rect12, -1f), () => TranslatorFormattedStringExtensions.Translate("PawnTooltipPsychicEntropyPainFocus", GenText.ToStringPercent(this.tracker.Pawn.health.hediffSet.PainTotal, "F0"), recoveryBonus), Gen.HashCombineInt(this.tracker.GetHashCode(), 133878));
			}
			return new GizmoResult(0);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00012BC4 File Offset: 0x00010DC4
		private static bool TryGetPainMultiplier(Pawn pawn, out float painMultiplier)
		{
			List<StatPart> parts = StatDefOf.PsychicEntropyRecoveryRate.parts;
			for (int i = 0; i < parts.Count; i++)
			{
				StatPart_Pain statPart_Pain = parts[i] as StatPart_Pain;
				if (statPart_Pain != null)
				{
					painMultiplier = statPart_Pain.PainFactor(pawn);
					return true;
				}
			}
			painMultiplier = 0f;
			return false;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00012C10 File Offset: 0x00010E10
		public override float GetWidth(float maxWidth)
		{
			return 212f;
		}

		// Token: 0x04000179 RID: 377
		private static readonly Color PainBoostColor = new Color(0.2f, 0.65f, 0.35f);

		// Token: 0x0400017A RID: 378
		private static readonly Texture2D EntropyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.46f, 0.34f, 0.35f));

		// Token: 0x0400017B RID: 379
		private static readonly Texture2D EntropyBarTexAdd = SolidColorMaterials.NewSolidColorTexture(new Color(0.78f, 0.72f, 0.66f));

		// Token: 0x0400017C RID: 380
		private static readonly Texture2D OverLimitBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.75f, 0.2f, 0.15f));

		// Token: 0x0400017D RID: 381
		private static readonly Texture2D PsyfocusBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.34f, 0.42f, 0.43f));

		// Token: 0x0400017E RID: 382
		private static readonly Texture2D PsyfocusBarTexReduce = SolidColorMaterials.NewSolidColorTexture(new Color(0.65f, 0.83f, 0.83f));

		// Token: 0x0400017F RID: 383
		private static readonly Texture2D PsyfocusBarHighlightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.43f, 0.54f, 0.55f));

		// Token: 0x04000180 RID: 384
		private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.03f, 0.035f, 0.05f));

		// Token: 0x04000181 RID: 385
		private static readonly Texture2D PsyfocusTargetTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74f, 0.97f, 0.8f));

		// Token: 0x04000182 RID: 386
		private readonly Texture2D LimitedTex;

		// Token: 0x04000183 RID: 387
		private readonly Pawn_PsychicEntropyTracker tracker;

		// Token: 0x04000184 RID: 388
		private readonly Texture2D UnlimitedTex;

		// Token: 0x04000185 RID: 389
		private bool draggingPsyfocusBar;

		// Token: 0x04000186 RID: 390
		private float selectedPsyfocusTarget = -1f;
	}
}
