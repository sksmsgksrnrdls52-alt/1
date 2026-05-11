using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200002A RID: 42
	[HarmonyPatch(typeof(Pawn_HealthTracker), "ShouldBeDeadFromLethalDamageThreshold")]
	public static class Pawn_HealthTracker_ShouldBeDeadFromLethalDamageThreshold_Patch
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00003BE1 File Offset: 0x00001DE1
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			Pawn_HealthTracker_ShouldBeDeadFromLethalDamageThreshold_Patch.<Transpiler>d__0 <Transpiler>d__ = new Pawn_HealthTracker_ShouldBeDeadFromLethalDamageThreshold_Patch.<Transpiler>d__0(-2);
			<Transpiler>d__.<>3__instructions = instructions;
			<Transpiler>d__.<>3__generator = generator;
			return <Transpiler>d__;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public static bool IsNotRegeneratingHediff(Hediff hediff)
		{
			return hediff.def != VPE_DefOf.VPE_Regenerating;
		}
	}
}
