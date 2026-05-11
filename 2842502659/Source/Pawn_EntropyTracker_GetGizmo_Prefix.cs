using System;
using HarmonyLib;
using RimWorld;
using VanillaPsycastsExpanded.UI;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A0 RID: 160
	[HarmonyPatch(typeof(Pawn_PsychicEntropyTracker), "GetGizmo")]
	public static class Pawn_EntropyTracker_GetGizmo_Prefix
	{
		// Token: 0x060001E6 RID: 486 RVA: 0x0000ADBE File Offset: 0x00008FBE
		[HarmonyPrefix]
		public static void Prefix(Pawn_PsychicEntropyTracker __instance, ref Gizmo ___gizmo)
		{
			if (___gizmo == null)
			{
				___gizmo = new PsychicStatusGizmo(__instance);
			}
		}
	}
}
