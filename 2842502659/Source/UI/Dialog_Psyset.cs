using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VanillaPsycastsExpanded.Technomancer;
using VEF.Abilities;
using VEF.Utils;
using Verse;

namespace VanillaPsycastsExpanded.UI
{
	// Token: 0x020000D5 RID: 213
	public class Dialog_Psyset : Window
	{
		// Token: 0x060002CE RID: 718 RVA: 0x000102BC File Offset: 0x0000E4BC
		public Dialog_Psyset(PsySet psyset, Pawn pawn) : base(null)
		{
			this.psyset = psyset;
			this.pawn = pawn;
			this.hediff = pawn.Psycasts();
			this.compAbilities = pawn.GetComp<CompAbilities>();
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.closeOnClickedOutside = true;
			this.paths = GenList.ListFullCopy<PsycasterPathDef>(this.hediff.unlockedPaths);
			foreach (PsycasterPathDef item in pawn.AllPathsFromPsyrings())
			{
				if (!this.paths.Contains(item))
				{
					this.paths.Add(item);
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00010388 File Offset: 0x0000E588
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(480f, 520f);
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0001039C File Offset: 0x0000E59C
		public override void DoWindowContents(Rect inRect)
		{
			inRect.yMax -= 50f;
			Text.Font = 2;
			Widgets.Label(GenUI.LeftHalf(UIUtility.TakeTopPart(ref inRect, 40f)), this.psyset.Name);
			Text.Font = 1;
			int group = DragAndDropWidget.NewGroup(null);
			Rect rect7 = GenUI.ContractedBy(GenUI.LeftHalf(inRect), 3f);
			rect7.xMax -= 8f;
			Widgets.Label(UIUtility.TakeTopPart(ref rect7, 20f), Translator.Translate("VPE.Contents"));
			Widgets.DrawMenuSection(rect7);
			DragAndDropWidget.DropArea(group, rect7, delegate(object obj)
			{
				this.psyset.Abilities.Add((AbilityDef)obj);
			}, null);
			Vector2 vector = rect7.position + new Vector2(8f, 8f);
			using (List<AbilityDef>.Enumerator enumerator = this.psyset.Abilities.ToList<AbilityDef>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityDef def = enumerator.Current;
					Rect rect2 = new Rect(vector, new Vector2(36f, 36f));
					PsycastsUIUtility.DrawAbility(rect2, def);
					TooltipHandler.TipRegion(rect2, () => string.Format("{0}\n\n{1}\n\n{2}", def.LabelCap, def.description, Translator.Translate("VPE.ClickRemove").Resolve().ToUpper()), def.GetHashCode() + 2);
					if (Widgets.ButtonInvisible(rect2, true))
					{
						this.psyset.Abilities.Remove(def);
					}
					vector.x += 44f;
					if (vector.x + 36f >= rect7.xMax)
					{
						vector.x = rect7.xMin + 8f;
						vector.y += 44f;
					}
				}
			}
			Rect inRect2 = GenUI.ContractedBy(GenUI.RightHalf(inRect), 3f);
			Rect rect3 = UIUtility.TakeTopPart(ref inRect2, 50f);
			Rect rect4 = GenUI.ContractedBy(UIUtility.TakeLeftPart(ref rect3, 40f), 0f, 5f);
			Rect rect5 = GenUI.ContractedBy(UIUtility.TakeRightPart(ref rect3, 40f), 0f, 5f);
			if (this.curIdx > 0 && Widgets.ButtonText(rect4, "<", true, true, true, null))
			{
				this.curIdx--;
			}
			if (this.curIdx < this.paths.Count - 1 && Widgets.ButtonText(rect5, ">", true, true, true, null))
			{
				this.curIdx++;
			}
			Text.Anchor = 4;
			Widgets.Label(rect3, string.Format("{0} / {1}", (this.paths.Count > 0) ? (this.curIdx + 1) : 0, this.paths.Count));
			Text.Anchor = 0;
			if (this.paths.Count > 0)
			{
				PsycasterPathDef psycasterPathDef = this.paths[this.curIdx];
				PsycastsUIUtility.DrawPathBackground(ref inRect2, psycasterPathDef, false);
				PsycastsUIUtility.DoPathAbilities(inRect2, psycasterPathDef, this.abilityPos, delegate(Rect rect, AbilityDef def)
				{
					PsycastsUIUtility.DrawAbility(rect, def);
					if (this.compAbilities.HasAbility(def))
					{
						DragAndDropWidget.Draggable(group, rect, def, null, null);
						TooltipHandler.TipRegion(rect, () => string.Format("{0}\n\n{1}", def.LabelCap, def.description), def.GetHashCode() + 1);
						return;
					}
					Widgets.DrawRectFast(rect, new Color(0f, 0f, 0f, 0.6f), null);
				});
			}
			AbilityDef abilityDef = DragAndDropWidget.CurrentlyDraggedDraggable() as AbilityDef;
			if (abilityDef != null)
			{
				PsycastsUIUtility.DrawAbility(new Rect(Event.current.mousePosition, new Vector2(36f, 36f)), abilityDef);
			}
			Rect? rect6 = DragAndDropWidget.HoveringDropAreaRect(group, null);
			if (rect6 != null)
			{
				Rect valueOrDefault = rect6.GetValueOrDefault();
				Widgets.DrawHighlight(valueOrDefault);
			}
		}

		// Token: 0x0400015E RID: 350
		private readonly Dictionary<AbilityDef, Vector2> abilityPos = new Dictionary<AbilityDef, Vector2>();

		// Token: 0x0400015F RID: 351
		private readonly CompAbilities compAbilities;

		// Token: 0x04000160 RID: 352
		private readonly Hediff_PsycastAbilities hediff;

		// Token: 0x04000161 RID: 353
		private readonly PsySet psyset;

		// Token: 0x04000162 RID: 354
		public List<PsycasterPathDef> paths;

		// Token: 0x04000163 RID: 355
		private int curIdx;

		// Token: 0x04000164 RID: 356
		private Pawn pawn;
	}
}
