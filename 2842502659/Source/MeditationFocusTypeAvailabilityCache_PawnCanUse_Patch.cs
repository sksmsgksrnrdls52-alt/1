using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200009D RID: 157
	[HarmonyPatch(typeof(MeditationFocusTypeAvailabilityCache), "PawnCanUseInt")]
	public static class MeditationFocusTypeAvailabilityCache_PawnCanUse_Patch
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x0000A68D File Offset: 0x0000888D
		public static void Postfix(Pawn p, MeditationFocusDef type, ref bool __result)
		{
			Hediff_PsycastAbilities hediff_PsycastAbilities = p.Psycasts();
			if (hediff_PsycastAbilities != null && hediff_PsycastAbilities.unlockedMeditationFoci.Contains(type))
			{
				__result = true;
				return;
			}
			MeditationFocusExtension modExtension = type.GetModExtension<MeditationFocusExtension>();
			if (modExtension != null && modExtension.pointsOnly)
			{
				__result = false;
			}
		}
	}
}
