using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000085 RID: 133
	public class ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch
	{
		// Token: 0x06000189 RID: 393 RVA: 0x00008900 File Offset: 0x00006B00
		public static void Postfix(ref IEnumerable<ThingDef> __result, bool hotReload)
		{
			__result = (from def in __result
			where !def.defName.StartsWith(ThingDefGenerator_Neurotrainer.PsytrainerDefPrefix)
			select def).Concat(ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch.ImpliedThingDefs(hotReload));
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008935 File Offset: 0x00006B35
		public static IEnumerable<ThingDef> ImpliedThingDefs(bool hotReload)
		{
			ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch.<ImpliedThingDefs>d__2 <ImpliedThingDefs>d__ = new ThingDefGenerator_Neurotrainer_ImpliedThingDefs_Patch.<ImpliedThingDefs>d__2(-2);
			<ImpliedThingDefs>d__.<>3__hotReload = hotReload;
			return <ImpliedThingDefs>d__;
		}

		// Token: 0x0400006B RID: 107
		public static Func<string, bool, ThingDef> BaseNeurotrainer = AccessTools.Method(typeof(ThingDefGenerator_Neurotrainer), "BaseNeurotrainer", null, null).CreateDelegate<Func<string, bool, ThingDef>>();
	}
}
