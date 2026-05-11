using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200003C RID: 60
	public class GoodwillImpactDelayed : IExposable
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x000050B0 File Offset: 0x000032B0
		public void DoImpact()
		{
			Faction.OfPlayer.TryAffectGoodwillWith(this.factionToImpact, this.goodwillImpact, true, true, this.historyEvent, null);
			if (!GenText.NullOrEmpty(this.relationInfoKey))
			{
				this.letterDesc += "\n\n" + TranslatorFormattedStringExtensions.Translate(this.relationInfoKey, NamedArgumentUtility.Named(this.factionToImpact, "FACTION"), Faction.OfPlayer.GoodwillWith(this.factionToImpact), this.goodwillImpact);
			}
			Find.LetterStack.ReceiveLetter(this.letterLabel, this.letterDesc, LetterDefOf.ThreatSmall, null, this.factionToImpact, null, null, null, 0, true);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005180 File Offset: 0x00003380
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.impactInTicks, "impactInTicks", 0, false);
			Scribe_Values.Look<int>(ref this.goodwillImpact, "goodwillImpact", 0, false);
			Scribe_Values.Look<string>(ref this.letterLabel, "letterLabel", null, false);
			Scribe_Values.Look<string>(ref this.letterDesc, "letterDesc", null, false);
			Scribe_Values.Look<string>(ref this.relationInfoKey, "relationInfoKey", null, false);
			Scribe_References.Look<Faction>(ref this.factionToImpact, "factionToImpact", false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.historyEvent, "historyEvent");
		}

		// Token: 0x04000034 RID: 52
		public int impactInTicks;

		// Token: 0x04000035 RID: 53
		public int goodwillImpact;

		// Token: 0x04000036 RID: 54
		public Faction factionToImpact;

		// Token: 0x04000037 RID: 55
		public HistoryEventDef historyEvent;

		// Token: 0x04000038 RID: 56
		public string letterLabel;

		// Token: 0x04000039 RID: 57
		public string letterDesc;

		// Token: 0x0400003A RID: 58
		public string relationInfoKey;
	}
}
