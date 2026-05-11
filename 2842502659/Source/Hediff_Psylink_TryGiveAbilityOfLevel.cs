using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200009B RID: 155
	[HarmonyPatch(typeof(Hediff_Psylink), "TryGiveAbilityOfLevel")]
	public static class Hediff_Psylink_TryGiveAbilityOfLevel
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x0000A67A File Offset: 0x0000887A
		public static bool Prefix()
		{
			return false;
		}
	}
}
