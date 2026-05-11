using System;
using Verse;

namespace VanillaPsycastsExpanded.HarmonyPatches
{
	// Token: 0x02000140 RID: 320
	[StaticConstructorOnStartup]
	public static class HarmonyInit
	{
		// Token: 0x06000493 RID: 1171 RVA: 0x0001C0EF File Offset: 0x0001A2EF
		static HarmonyInit()
		{
			PsycastsMod.Harm.PatchAll();
		}
	}
}
