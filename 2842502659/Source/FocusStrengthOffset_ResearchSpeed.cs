using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200008B RID: 139
	public class FocusStrengthOffset_ResearchSpeed : FocusStrengthOffset
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00008D15 File Offset: 0x00006F15
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			return parent is Building_ResearchBench;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00008D20 File Offset: 0x00006F20
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset * StatExtension.GetStatValue(parent, StatDefOf.ResearchSpeedFactor, true, -1);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00008D36 File Offset: 0x00006F36
		public override string GetExplanation(Thing parent)
		{
			return Translator.Translate("Difficulty_ResearchSpeedFactor_Label") + ": " + GenText.ToStringWithSign(this.GetOffset(parent, null), "0%");
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008D68 File Offset: 0x00006F68
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			if (def == null)
			{
				return "";
			}
			return Translator.Translate("Difficulty_ResearchSpeedFactor_Label") + ": " + GenText.ToStringWithSign(this.offset * StatExtension.GetStatValueAbstract(def, StatDefOf.ResearchSpeedFactor, null), "0%");
		}
	}
}
