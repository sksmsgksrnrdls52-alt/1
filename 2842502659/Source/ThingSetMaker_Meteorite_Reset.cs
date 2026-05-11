using System;
using HarmonyLib;
using RimWorld;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200004D RID: 77
	[HarmonyPatch(typeof(ThingSetMaker_Meteorite), "Reset")]
	public static class ThingSetMaker_Meteorite_Reset
	{
		// Token: 0x060000DC RID: 220 RVA: 0x000058A3 File Offset: 0x00003AA3
		public static void Postfix()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Remove(VPE_DefOf.VPE_EltexOre);
		}
	}
}
