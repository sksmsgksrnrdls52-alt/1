using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B5 RID: 181
	public class Hediff_RegrowLimbs : HediffWithComps
	{
		// Token: 0x06000256 RID: 598 RVA: 0x0000D4F8 File Offset: 0x0000B6F8
		public override void PostTick()
		{
			base.PostTick();
			if (Find.TickManager.TicksGame % 2500 == 0)
			{
				bool flag = false;
				List<Hediff_Injury> list = this.pawn.health.hediffSet.hediffs.OfType<Hediff_Injury>().ToList<Hediff_Injury>();
				if (GenCollection.Any<Hediff_Injury>(list))
				{
					GenCollection.RandomElement<Hediff_Injury>(list).Heal(1f);
					flag = true;
				}
				else
				{
					List<BodyPartRecord> nonMissingParts = this.pawn.health.hediffSet.GetNotMissingParts(0, 0, null, null).ToList<BodyPartRecord>();
					List<BodyPartRecord> list2 = (from x in this.pawn.def.race.body.AllParts
					where this.pawn.health.hediffSet.PartIsMissing(x) && nonMissingParts.Contains(x.parent) && !this.pawn.health.hediffSet.AncestorHasDirectlyAddedParts(x)
					select x).ToList<BodyPartRecord>();
					if (GenCollection.Any<BodyPartRecord>(list2))
					{
						BodyPartRecord bodyPartRecord = GenCollection.RandomElement<BodyPartRecord>(list2);
						IEnumerable<Hediff_MissingPart> source = this.pawn.health.hediffSet.hediffs.OfType<Hediff_MissingPart>().ToList<Hediff_MissingPart>();
						this.pawn.health.RestorePart(bodyPartRecord, null, true);
						List<Hediff_MissingPart> currentMissingHediffs2 = this.pawn.health.hediffSet.hediffs.OfType<Hediff_MissingPart>().ToList<Hediff_MissingPart>();
						foreach (Hediff_MissingPart hediff_MissingPart in from x in source
						where !currentMissingHediffs2.Contains(x)
						select x)
						{
							Hediff hediff = HediffMaker.MakeHediff(VPE_DefOf.VPE_Regenerating, this.pawn, hediff_MissingPart.Part);
							hediff.Severity = hediff_MissingPart.Part.def.GetMaxHealth(this.pawn) - 1f;
							this.pawn.health.AddHediff(hediff, null, null, null);
						}
						flag = true;
					}
				}
				if (flag)
				{
					FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.HealingCross, 0.42f);
				}
			}
		}
	}
}
