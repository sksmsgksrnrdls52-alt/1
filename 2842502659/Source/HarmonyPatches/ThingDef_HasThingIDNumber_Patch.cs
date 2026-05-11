using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded.HarmonyPatches
{
	// Token: 0x0200013E RID: 318
	[HarmonyPatch(typeof(ThingDef), "HasThingIDNumber", 1)]
	public static class ThingDef_HasThingIDNumber_Patch
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x0001C0CC File Offset: 0x0001A2CC
		public static void Postfix(ThingDef __instance, ref bool __result)
		{
			if (__instance.CanBeSaved())
			{
				__result = true;
			}
		}
	}
}
