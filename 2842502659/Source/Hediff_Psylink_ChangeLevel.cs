using System;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200009A RID: 154
	[HarmonyPatch(typeof(Hediff_Psylink), "ChangeLevel", new Type[]
	{
		typeof(int),
		typeof(bool)
	})]
	public static class Hediff_Psylink_ChangeLevel
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x0000A661 File Offset: 0x00008861
		public static bool Prefix(Hediff_Psylink __instance, int levelOffset, ref bool sendLetter)
		{
			__instance.pawn.Psycasts().ChangeLevel(levelOffset, sendLetter);
			sendLetter = false;
			return false;
		}
	}
}
