using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200004C RID: 76
	[HarmonyPatch(typeof(RitualOutcomeEffectWorker_Bestowing), "Apply")]
	public class RitualOutcomeEffectWorker_Bestowing_Apply_Patch
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0000574C File Offset: 0x0000394C
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
			MethodInfo info1 = AccessTools.Method(typeof(PawnUtility), "GetPsylinkLevel", null, null);
			MethodInfo info2 = AccessTools.Method(typeof(PawnUtility), "GetMaxPsylinkLevelByTitle", null, null);
			int num = list.FindIndex((CodeInstruction ins) => CodeInstructionExtensions.Calls(ins, info1)) - 1;
			int num2 = list.FindIndex((CodeInstruction ins) => CodeInstructionExtensions.Calls(ins, info2)) + 1;
			list.RemoveRange(num, num2 - num + 1);
			list.InsertRange(num, new CodeInstruction[]
			{
				new CodeInstruction(OpCodes.Ldloc_2, null),
				new CodeInstruction(OpCodes.Ldloc, 9),
				new CodeInstruction(OpCodes.Ldloc, 10),
				CodeInstruction.Call(typeof(RitualOutcomeEffectWorker_Bestowing_Apply_Patch), "ApplyTitlePsylink", null, null)
			});
			return list;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005834 File Offset: 0x00003A34
		public static void ApplyTitlePsylink(Pawn pawn, RoyalTitleDef oldTitle, RoyalTitleDef newTitle)
		{
			Hediff_PsycastAbilities hediff_PsycastAbilities = pawn.Psycasts();
			int maxPsylinkLevel = newTitle.maxPsylinkLevel;
			int num = (oldTitle != null) ? oldTitle.maxPsylinkLevel : 0;
			if (hediff_PsycastAbilities == null)
			{
				PawnUtility.ChangePsylinkLevel(pawn, 1, false);
				hediff_PsycastAbilities = pawn.Psycasts();
				hediff_PsycastAbilities.ChangeLevel(maxPsylinkLevel - num, false);
				hediff_PsycastAbilities.maxLevelFromTitles = maxPsylinkLevel;
				return;
			}
			if (hediff_PsycastAbilities.maxLevelFromTitles > maxPsylinkLevel)
			{
				return;
			}
			hediff_PsycastAbilities.ChangeLevel(maxPsylinkLevel - num, false);
			hediff_PsycastAbilities.maxLevelFromTitles = maxPsylinkLevel;
		}
	}
}
