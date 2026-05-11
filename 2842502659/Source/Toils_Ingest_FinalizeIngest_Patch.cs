using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000046 RID: 70
	[HarmonyPatch]
	public static class Toils_Ingest_FinalizeIngest_Patch
	{
		// Token: 0x060000CD RID: 205 RVA: 0x000054D0 File Offset: 0x000036D0
		[HarmonyTargetMethod]
		public static MethodBase GetMethod()
		{
			Type[] nestedTypes = typeof(Toils_Ingest).GetNestedTypes(AccessTools.all);
			for (int i = 0; i < nestedTypes.Length; i++)
			{
				MethodInfo methodInfo = nestedTypes[i].GetMethods(AccessTools.all).FirstOrDefault((MethodInfo x) => x.Name.Contains("<FinalizeIngest>"));
				if (methodInfo != null)
				{
					return methodInfo;
				}
			}
			throw new Exception("Toils_Ingest_FinalizeIngest_Patch failed to find a method to patch.");
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005547 File Offset: 0x00003747
		public static void Prefix(object __instance)
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = Traverse.Create(__instance).Field("ingester").GetValue<Pawn>();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005563 File Offset: 0x00003763
		public static void Postfix()
		{
			RoomStatDef_GetScoreStageIndex_Patch.forPawn = null;
		}
	}
}
