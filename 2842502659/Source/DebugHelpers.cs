using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200001C RID: 28
	public static class DebugHelpers
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00002F7C File Offset: 0x0000117C
		public static IEnumerable<CodeInstruction> AddLogs(this IEnumerable<CodeInstruction> instructions, MethodBase original, List<MethodInfo> methods)
		{
			DebugHelpers.<AddLogs>d__0 <AddLogs>d__ = new DebugHelpers.<AddLogs>d__0(-2);
			<AddLogs>d__.<>3__instructions = instructions;
			<AddLogs>d__.<>3__original = original;
			<AddLogs>d__.<>3__methods = methods;
			return <AddLogs>d__;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002F9A File Offset: 0x0000119A
		public static void DoLog<T>(T obj, string header, string context)
		{
			Log.Message(string.Format("[{0}] {1}: {2}", context, header, obj));
		}
	}
}
