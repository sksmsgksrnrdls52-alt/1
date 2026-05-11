using System;
using System.Collections.Generic;
using System.Linq;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F3 RID: 243
	public static class PsyringUtilities
	{
		// Token: 0x0600035C RID: 860 RVA: 0x00014E94 File Offset: 0x00013094
		public static IEnumerable<Psyring> AllPsyrings(this Pawn pawn)
		{
			return pawn.apparel.WornApparel.OfType<Psyring>();
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00014EA8 File Offset: 0x000130A8
		public static IEnumerable<AbilityDef> AllAbilitiesFromPsyrings(this Pawn pawn)
		{
			return (from psyring in pawn.AllPsyrings()
			where psyring.Added
			select psyring.Ability).Distinct<AbilityDef>();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00014F08 File Offset: 0x00013108
		public static IEnumerable<PsycasterPathDef> AllPathsFromPsyrings(this Pawn pawn)
		{
			return (from psyring in pawn.AllPsyrings()
			select psyring.Path).Distinct<PsycasterPathDef>();
		}
	}
}
