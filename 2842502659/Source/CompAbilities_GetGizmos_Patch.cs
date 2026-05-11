using System;
using System.Collections.Generic;
using HarmonyLib;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000081 RID: 129
	[HarmonyPatch(typeof(CompAbilities), "GetGizmos")]
	public static class CompAbilities_GetGizmos_Patch
	{
		// Token: 0x06000184 RID: 388 RVA: 0x000087D4 File Offset: 0x000069D4
		public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> gizmos, CompAbilities __instance)
		{
			CompAbilities_GetGizmos_Patch.<Postfix>d__0 <Postfix>d__ = new CompAbilities_GetGizmos_Patch.<Postfix>d__0(-2);
			<Postfix>d__.<>3__gizmos = gizmos;
			<Postfix>d__.<>3____instance = __instance;
			return <Postfix>d__;
		}
	}
}
