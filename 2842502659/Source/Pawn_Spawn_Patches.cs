using System;
using System.Linq;
using HarmonyLib;
using VanillaPsycastsExpanded.Nightstalker;
using VanillaPsycastsExpanded.Technomancer;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200004B RID: 75
	[HarmonyPatch]
	public static class Pawn_Spawn_Patches
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x000056B8 File Offset: 0x000038B8
		[HarmonyPatch(typeof(Pawn), "SpawnSetup")]
		[HarmonyPostfix]
		public static void PawnPostSpawned(Pawn __instance)
		{
			if (__instance.health.hediffSet.GetAllComps().OfType<HediffComp_Haywire>().Any<HediffComp_Haywire>())
			{
				HaywireManager.HaywireThings.Add(__instance);
			}
			if (__instance.health.hediffSet.hediffs.OfType<Hediff_Darkvision>().Any<Hediff_Darkvision>())
			{
				Hediff_Darkvision.DarkvisionPawns.Add(__instance);
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00005715 File Offset: 0x00003915
		[HarmonyPatch(typeof(Pawn), "DeSpawn")]
		[HarmonyPostfix]
		public static void PawnPostDeSpawned(Pawn __instance)
		{
			if (HaywireManager.HaywireThings.Contains(__instance))
			{
				HaywireManager.HaywireThings.Remove(__instance);
			}
			if (Hediff_Darkvision.DarkvisionPawns.Contains(__instance))
			{
				Hediff_Darkvision.DarkvisionPawns.Remove(__instance);
			}
		}
	}
}
