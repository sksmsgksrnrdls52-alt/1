using System;
using System.Collections.Generic;
using VEF.Abilities;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200009F RID: 159
	public static class AbilityExtensionPsycastUtility
	{
		// Token: 0x060001E4 RID: 484 RVA: 0x0000AD80 File Offset: 0x00008F80
		public static AbilityExtension_Psycast Psycast(this AbilityDef def)
		{
			AbilityExtension_Psycast modExtension;
			if (AbilityExtensionPsycastUtility.cache.TryGetValue(def, out modExtension))
			{
				return modExtension;
			}
			modExtension = def.GetModExtension<AbilityExtension_Psycast>();
			AbilityExtensionPsycastUtility.cache[def] = modExtension;
			return modExtension;
		}

		// Token: 0x0400008A RID: 138
		private static readonly Dictionary<AbilityDef, AbilityExtension_Psycast> cache = new Dictionary<AbilityDef, AbilityExtension_Psycast>();
	}
}
