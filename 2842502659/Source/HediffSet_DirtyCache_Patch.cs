using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200001E RID: 30
	[HarmonyPatch(typeof(HediffSet), "DirtyCache")]
	public static class HediffSet_DirtyCache_Patch
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00003118 File Offset: 0x00001318
		public static void Postfix(HediffSet __instance)
		{
			__instance.pawn.RecheckPaths();
		}
	}
}
