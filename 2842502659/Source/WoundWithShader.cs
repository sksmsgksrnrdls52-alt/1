using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200002E RID: 46
	[HarmonyPatch]
	public class WoundWithShader : FleshTypeDef.Wound
	{
		// Token: 0x06000081 RID: 129 RVA: 0x00004167 File Offset: 0x00002367
		[HarmonyPatch(typeof(FleshTypeDef.Wound), "Resolve")]
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Resolve_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			WoundWithShader.<Resolve_Transpiler>d__1 <Resolve_Transpiler>d__ = new WoundWithShader.<Resolve_Transpiler>d__1(-2);
			<Resolve_Transpiler>d__.<>3__instructions = instructions;
			<Resolve_Transpiler>d__.<>3__generator = generator;
			return <Resolve_Transpiler>d__;
		}

		// Token: 0x0400001F RID: 31
		public ShaderTypeDef shader;
	}
}
