using System;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000C5 RID: 197
	public static class Listing_Standard_Ext
	{
		// Token: 0x06000297 RID: 663 RVA: 0x0000EB18 File Offset: 0x0000CD18
		public static void CheckboxMultiLabeled(this Listing_Standard listing, string label, ref MultiCheckboxState state, string tooltip = null)
		{
			Rect rect = listing.GetRect(Text.LineHeight, 1f);
			if (listing.BoundingRectCached == null || rect.Overlaps(listing.BoundingRectCached.Value))
			{
				if (!GenText.NullOrEmpty(tooltip))
				{
					MouseoverSounds.DoRegion(rect);
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
					}
					TooltipHandler.TipRegion(rect, tooltip);
				}
				TextAnchor anchor = Text.Anchor;
				Text.Anchor = 3;
				Widgets.Label(rect, label);
				if (Widgets.ButtonInvisible(rect, true))
				{
					MultiCheckboxState multiCheckboxState;
					switch (state)
					{
					case 0:
						multiCheckboxState = 2;
						break;
					case 1:
						multiCheckboxState = 0;
						break;
					case 2:
						multiCheckboxState = 1;
						break;
					default:
						throw new ArgumentOutOfRangeException("state", state, null);
					}
					state = multiCheckboxState;
					if (state == 0)
					{
						SoundStarter.PlayOneShotOnCamera(SoundDefOf.Checkbox_TurnedOn, null);
					}
					else
					{
						SoundStarter.PlayOneShotOnCamera(SoundDefOf.Checkbox_TurnedOff, null);
					}
				}
				Rect rect2;
				rect2..ctor(rect.x + rect.width - 24f, rect.y, 24f, 24f);
				Texture2D texture2D;
				switch (state)
				{
				case 0:
					texture2D = Widgets.CheckboxOnTex;
					break;
				case 1:
					texture2D = Widgets.CheckboxOffTex;
					break;
				case 2:
					texture2D = Widgets.CheckboxPartialTex;
					break;
				default:
					texture2D = BaseContent.ClearTex;
					break;
				}
				GUI.DrawTexture(rect2, texture2D);
				Text.Anchor = anchor;
			}
			listing.Gap(listing.verticalSpacing);
		}
	}
}
