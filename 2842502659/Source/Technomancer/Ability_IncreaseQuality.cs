using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000ED RID: 237
	public class Ability_IncreaseQuality : Ability
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600033C RID: 828 RVA: 0x000141CF File Offset: 0x000123CF
		private QualityCategory MaxQuality
		{
			get
			{
				return (byte)((int)this.GetPowerForPawn());
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000141DC File Offset: 0x000123DC
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				CompQuality compQuality = ThingCompUtility.TryGetComp<CompQuality>(MinifyUtility.GetInnerIfMinified(globalTargetInfo.Thing));
				if (compQuality == null || compQuality.Quality >= this.MaxQuality || globalTargetInfo.Thing is Book)
				{
					return;
				}
				compQuality.SetQuality(compQuality.Quality + 1, new ArtGenerationContext?(1));
				for (int j = 0; j < 16; j++)
				{
					FleckMaker.ThrowMicroSparks(GenThing.TrueCenter(globalTargetInfo.Thing), this.pawn.Map);
				}
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00014284 File Offset: 0x00012484
		public override float GetPowerForPawn()
		{
			float statValue = StatExtension.GetStatValue(this.pawn, StatDefOf.PsychicSensitivity, true, -1);
			int num;
			if (statValue > 1.2f)
			{
				if (statValue > 2.5f)
				{
					if (statValue <= 2.5f)
					{
						num = 2;
					}
					else
					{
						num = 5;
					}
				}
				else
				{
					num = 4;
				}
			}
			else
			{
				num = 3;
			}
			return (float)num;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000142CE File Offset: 0x000124CE
		public override string GetPowerForPawnDescription()
		{
			return ColoredText.Colorize(TranslatorFormattedStringExtensions.Translate("VPE.MaxQuality", QualityUtility.GetLabel(this.MaxQuality)), Color.cyan);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000142F4 File Offset: 0x000124F4
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			CompQuality compQuality;
			if ((compQuality = ThingCompUtility.TryGetComp<CompQuality>(MinifyUtility.GetInnerIfMinified(target.Thing))) == null)
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustHaveQuality"), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (compQuality.Quality >= this.MaxQuality)
			{
				if (showMessages)
				{
					Messages.Message(TranslatorFormattedStringExtensions.Translate("VPE.QualityTooHigh", QualityUtility.GetLabel(this.MaxQuality)), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
