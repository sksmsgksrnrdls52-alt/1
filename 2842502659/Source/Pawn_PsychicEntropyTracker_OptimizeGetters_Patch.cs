using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200001D RID: 29
	[HarmonyPatch]
	public static class Pawn_PsychicEntropyTracker_OptimizeGetters_Patch
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00002FB3 File Offset: 0x000011B3
		private static IEnumerable<MethodBase> TargetMethods()
		{
			return new Pawn_PsychicEntropyTracker_OptimizeGetters_Patch.<TargetMethods>d__0(-2);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002FBC File Offset: 0x000011BC
		private unsafe static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr, MethodBase baseMethod)
		{
			CodeMatcher codeMatcher = new CodeMatcher(instr, null);
			codeMatcher.MatchEndForward(new CodeMatch[]
			{
				CodeMatch.LoadsField(AccessToolsExtensions.DeclaredField(typeof(StatDefOf), "PsychicEntropyMax"), false),
				CodeMatch.LoadsConstant(1L),
				CodeMatch.LoadsConstant(-1L),
				CodeMatch.Calls(Expression.Lambda<Func<<>f__AnonymousDelegate0<Thing, StatDef, bool, int, float>>>(Expression.Convert(Expression.Call(Expression.Constant(methodof(StatExtension.GetStatValue(Thing, StatDef, bool, int)), typeof(MethodInfo)), methodof(MethodInfo.CreateDelegate(Type, object)), new Expression[]
				{
					Expression.Constant(typeof(<>f__AnonymousDelegate0<Thing, StatDef, bool, int, float>), typeof(Type)),
					Expression.Constant(null, typeof(object))
				}), typeof(<>f__AnonymousDelegate0<Thing, StatDef, bool, int, float>)), Array.Empty<ParameterExpression>()))
			});
			if (codeMatcher.IsInvalid)
			{
				string[] array = new string[5];
				array[0] = "Patch to optimize ";
				int num = 1;
				Type declaringType = baseMethod.DeclaringType;
				array[num] = ((declaringType != null) ? declaringType.Name : null);
				array[2] = ".";
				array[3] = baseMethod.Name;
				array[4] = " failed, could not find code sequence responsible for accessing max psychic heat. Either vanilla code changed (was fixed?), or another mod modified this code.";
				Log.Error(string.Concat(array));
				return codeMatcher.Instructions();
			}
			codeMatcher.Advance(-1);
			*codeMatcher.Opcode = OpCodes.Ldc_I4_S;
			*codeMatcher.Operand = 60;
			return codeMatcher.Instructions();
		}
	}
}
