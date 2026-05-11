using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using VEF.Buildings;
using Verse;

namespace VanillaPsycastsExpanded.Skipmaster
{
	// Token: 0x02000117 RID: 279
	[HarmonyPatch]
	public static class SkipdoorPatches
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x00018A64 File Offset: 0x00016C64
		[HarmonyPatch(typeof(Pawn), "Kill")]
		[HarmonyPrefix]
		public static void Pawn_Kill_Prefix(Pawn __instance)
		{
			Faction faction = __instance.Faction;
			if (faction != null && faction.IsPlayer)
			{
				foreach (Skipdoor skipdoor in WorldComponent_DoorTeleporterManager.Instance.DoorTeleporters.OfType<Skipdoor>().ToList<Skipdoor>())
				{
					if (skipdoor.Pawn == __instance)
					{
						GenExplosion.DoExplosion(skipdoor.Position, skipdoor.Map, 4.9f, DamageDefOf.Bomb, skipdoor, 35, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
						if (!skipdoor.Destroyed)
						{
							skipdoor.Destroy(0);
						}
					}
				}
			}
		}
	}
}
