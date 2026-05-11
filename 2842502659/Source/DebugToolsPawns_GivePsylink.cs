using System;
using System.Collections.Generic;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200009C RID: 156
	[HarmonyPatch(typeof(DebugToolsPawns), "GivePsylink")]
	public static class DebugToolsPawns_GivePsylink
	{
		// Token: 0x060001D3 RID: 467 RVA: 0x0000A67D File Offset: 0x0000887D
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			DebugToolsPawns_GivePsylink.<Transpiler>d__0 <Transpiler>d__ = new DebugToolsPawns_GivePsylink.<Transpiler>d__0(-2);
			<Transpiler>d__.<>3__instructions = instructions;
			return <Transpiler>d__;
		}
	}
}
