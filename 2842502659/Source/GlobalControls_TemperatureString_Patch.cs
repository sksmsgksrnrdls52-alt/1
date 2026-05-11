using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000051 RID: 81
	[HarmonyPatch(typeof(GlobalControls), "TemperatureString")]
	public static class GlobalControls_TemperatureString_Patch
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x000059C1 File Offset: 0x00003BC1
		[HarmonyPriority(-2147483648)]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
		{
			GlobalControls_TemperatureString_Patch.<Transpiler>d__0 <Transpiler>d__ = new GlobalControls_TemperatureString_Patch.<Transpiler>d__0(-2);
			<Transpiler>d__.<>3__codeInstructions = codeInstructions;
			return <Transpiler>d__;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000059D4 File Offset: 0x00003BD4
		public static void ModifyTemperatureIfNeeded(ref float result, IntVec3 cell, Map map)
		{
			MapComponent_PsycastsManager cachedComp = GenTemperature_TryGetTemperatureForCell_Patch.cachedComp;
			if (((cachedComp != null) ? cachedComp.map : null) != map)
			{
				GenTemperature_TryGetTemperatureForCell_Patch.cachedComp = map.GetComponent<MapComponent_PsycastsManager>();
			}
			float num;
			if (GenTemperature_TryGetTemperatureForCell_Patch.cachedComp.TryGetOverridenTemperatureFor(cell, out num))
			{
				result = num;
			}
		}
	}
}
