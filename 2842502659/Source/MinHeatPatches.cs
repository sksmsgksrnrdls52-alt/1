using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A4 RID: 164
	[HarmonyPatch]
	public static class MinHeatPatches
	{
		// Token: 0x060001EB RID: 491 RVA: 0x0000AE44 File Offset: 0x00009044
		[HarmonyTargetMethods]
		public static IEnumerable<MethodInfo> TargetMethods()
		{
			return new MinHeatPatches.<TargetMethods>d__0(-2);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000AE4D File Offset: 0x0000904D
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			MinHeatPatches.<Transpiler>d__1 <Transpiler>d__ = new MinHeatPatches.<Transpiler>d__1(-2);
			<Transpiler>d__.<>3__instructions = instructions;
			return <Transpiler>d__;
		}
	}
}
