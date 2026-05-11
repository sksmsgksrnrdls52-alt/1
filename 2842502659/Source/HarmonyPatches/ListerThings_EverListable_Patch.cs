using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded.HarmonyPatches
{
	// Token: 0x0200013D RID: 317
	[HarmonyPatch(typeof(ListerThings), "EverListable")]
	public static class ListerThings_EverListable_Patch
	{
		// Token: 0x0600048F RID: 1167 RVA: 0x0001C065 File Offset: 0x0001A265
		public static void Postfix(ThingDef def, ref bool __result)
		{
			if (def.CanBeSaved())
			{
				__result = true;
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001C074 File Offset: 0x0001A274
		public static bool CanBeSaved(this ThingDef def)
		{
			return def != null && (typeof(MoteAttachedScaled).IsAssignableFrom(def.thingClass) || typeof(MoteAttachedMovingAround).IsAssignableFrom(def.thingClass) || typeof(MoteAttachedOneTime).IsAssignableFrom(def.thingClass));
		}
	}
}
