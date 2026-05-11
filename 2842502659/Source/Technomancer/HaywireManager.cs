using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F6 RID: 246
	[HarmonyPatch]
	[StaticConstructorOnStartup]
	public class HaywireManager
	{
		// Token: 0x06000366 RID: 870 RVA: 0x000153D4 File Offset: 0x000135D4
		static HaywireManager()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (typeof(Building_Turret).IsAssignableFrom(thingDef.thingClass))
				{
					thingDef.comps.Add(new CompProperties(typeof(CompHaywire)));
				}
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00015454 File Offset: 0x00013654
		public static bool ShouldTargetAllies(Thing t)
		{
			return HaywireManager.HaywireThings.Contains(t);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00015464 File Offset: 0x00013664
		[HarmonyPatch(typeof(AttackTargetsCache), "GetPotentialTargetsFor")]
		[HarmonyPostfix]
		public static void ChangeTargets(IAttackTargetSearcher th, ref List<IAttackTarget> __result, AttackTargetsCache __instance)
		{
			Thing thing = th as Thing;
			if (thing != null && HaywireManager.HaywireThings.Contains(thing))
			{
				__result.Clear();
				__result.AddRange(__instance.TargetsHostileToColony);
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0001549C File Offset: 0x0001369C
		[HarmonyPatch(typeof(Building_TurretGun), "IsValidTarget")]
		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
		{
			List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
			FieldInfo info = AccessTools.Field(typeof(Building_TurretGun), "mannableComp");
			int num = list.FindIndex((CodeInstruction ins) => CodeInstructionExtensions.LoadsField(ins, info, false));
			Label label = (Label)list[num + 1].operand;
			int num2 = list.FindLastIndex(num, (CodeInstruction ins) => ins.opcode == OpCodes.Ldarg_0);
			list.InsertRange(num2 + 1, new CodeInstruction[]
			{
				new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HaywireManager), "ShouldTargetAllies", null, null)),
				new CodeInstruction(OpCodes.Brtrue, label),
				new CodeInstruction(OpCodes.Ldarg_0, null)
			});
			return list;
		}

		// Token: 0x040001A3 RID: 419
		public static readonly HashSet<Thing> HaywireThings = new HashSet<Thing>();

		// Token: 0x0200019F RID: 415
		[HarmonyPatch]
		public static class OverrideBestAttackTargetValidator
		{
			// Token: 0x06000626 RID: 1574 RVA: 0x0001FD36 File Offset: 0x0001DF36
			[HarmonyTargetMethod]
			public static MethodInfo TargetMethod()
			{
				return AccessTools.Method(AccessTools.Inner(typeof(AttackTargetFinder), "<>c__DisplayClass5_0"), "<BestAttackTarget>b__1", null, null);
			}

			// Token: 0x06000627 RID: 1575 RVA: 0x0001FD58 File Offset: 0x0001DF58
			[HarmonyTranspiler]
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
			{
				List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
				MethodInfo info = AccessTools.Method(typeof(GenHostility), "HostileTo", new Type[]
				{
					typeof(Thing),
					typeof(Thing)
				}, null);
				int startIndex = list.FindIndex((CodeInstruction ins) => CodeInstructionExtensions.Calls(ins, info));
				int num = list.FindLastIndex(startIndex, (CodeInstruction ins) => ins.opcode == OpCodes.Ldarg_0);
				FieldInfo fieldInfo = (FieldInfo)list[num + 1].operand;
				int index = list.FindIndex(startIndex, (CodeInstruction ins) => ins.opcode == OpCodes.Ldc_I4_0);
				list.RemoveAt(index);
				list.InsertRange(index, new CodeInstruction[]
				{
					new CodeInstruction(OpCodes.Ldarg_0, null),
					new CodeInstruction(OpCodes.Ldfld, fieldInfo),
					new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(HaywireManager), "ShouldTargetAllies", null, null))
				});
				return list;
			}
		}
	}
}
