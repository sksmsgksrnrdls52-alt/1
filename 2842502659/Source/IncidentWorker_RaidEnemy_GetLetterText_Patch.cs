using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000021 RID: 33
	[HarmonyPatch(typeof(IncidentWorker_RaidEnemy), "GetLetterText")]
	public static class IncidentWorker_RaidEnemy_GetLetterText_Patch
	{
		// Token: 0x06000058 RID: 88 RVA: 0x000031C4 File Offset: 0x000013C4
		private static bool Prefix(ref string __result, IncidentParms parms, List<Pawn> pawns)
		{
			if (parms.raidStrategy.Worker is RaidStrategyWorker_ImmediateAttack_Psycasters)
			{
				string text = GenText.CapitalizeFirst(string.Format(parms.raidArrivalMode.textEnemy, parms.faction.def.pawnsPlural, ColoredText.ApplyTag(parms.faction.Name, parms.faction)));
				text += "\n\n";
				text += parms.raidStrategy.arrivalTextEnemy;
				IEnumerable<Pawn> source = (from x in pawns
				where x.HasPsylink
				select x).ToList<Pawn>();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value in source.Select(delegate(Pawn x)
				{
					Name name = x.Name;
					return ((name != null) ? name.ToString() : null) + " - " + x.KindLabel;
				}))
				{
					stringBuilder.AppendLine(value);
				}
				text += TranslatorFormattedStringExtensions.Translate("VPE.PsycasterRaidDescription", stringBuilder.ToString());
				Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
				if (pawn != null)
				{
					text += "\n\n";
					text += TranslatorFormattedStringExtensions.Translate("EnemyRaidLeaderPresent", pawn.Faction.def.pawnsPlural, pawn.LabelShort, NamedArgumentUtility.Named(pawn, "LEADER"));
				}
				__result = text;
				return false;
			}
			return true;
		}
	}
}
