using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000039 RID: 57
	public class StatPart_PsycastBodiesConsumed : StatPart
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00004A60 File Offset: 0x00002C60
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					Hediff_BodiesConsumed hediff_BodiesConsumed = pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_BodiesConsumed, false) as Hediff_BodiesConsumed;
					if (hediff_BodiesConsumed != null && hediff_BodiesConsumed.consumedBodies > 0)
					{
						val += (float)hediff_BodiesConsumed.consumedBodies;
					}
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004ABC File Offset: 0x00002CBC
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					Hediff_BodiesConsumed hediff_BodiesConsumed = pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_BodiesConsumed, false) as Hediff_BodiesConsumed;
					if (hediff_BodiesConsumed != null && hediff_BodiesConsumed.consumedBodies > 0)
					{
						return TranslatorFormattedStringExtensions.Translate("VPE.StatsReport_BodiesConsumed", hediff_BodiesConsumed.consumedBodies);
					}
				}
			}
			return null;
		}
	}
}
