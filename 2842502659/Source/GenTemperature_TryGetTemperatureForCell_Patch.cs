using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000050 RID: 80
	[HarmonyPatch(typeof(GenTemperature), "TryGetTemperatureForCell")]
	public static class GenTemperature_TryGetTemperatureForCell_Patch
	{
		// Token: 0x060000DF RID: 223 RVA: 0x00005978 File Offset: 0x00003B78
		public static bool Prefix(IntVec3 c, Map map, ref float tempResult, ref bool __result)
		{
			if (map == null)
			{
				return true;
			}
			MapComponent_PsycastsManager mapComponent_PsycastsManager = GenTemperature_TryGetTemperatureForCell_Patch.cachedComp;
			if (((mapComponent_PsycastsManager != null) ? mapComponent_PsycastsManager.map : null) != map)
			{
				GenTemperature_TryGetTemperatureForCell_Patch.cachedComp = map.GetComponent<MapComponent_PsycastsManager>();
			}
			float num;
			if (GenTemperature_TryGetTemperatureForCell_Patch.cachedComp.TryGetOverridenTemperatureFor(c, out num))
			{
				tempResult = num;
				__result = true;
				return false;
			}
			return true;
		}

		// Token: 0x0400003F RID: 63
		public static MapComponent_PsycastsManager cachedComp;
	}
}
