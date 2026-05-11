using System;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F9 RID: 249
	public static class MechUtility
	{
		// Token: 0x06000374 RID: 884 RVA: 0x000157EF File Offset: 0x000139EF
		public static bool IsMechAlly(this Pawn mech, Pawn other)
		{
			return mech.RaceProps.IsMechanoid && MechanitorUtility.IsPlayerOverseerSubject(mech) && (other.Faction == mech.Faction || (other.IsColonist && mech.IsColonyMech));
		}
	}
}
