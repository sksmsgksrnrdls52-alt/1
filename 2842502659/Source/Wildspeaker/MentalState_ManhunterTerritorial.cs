using System;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000E4 RID: 228
	public class MentalState_ManhunterTerritorial : MentalState_Manhunter
	{
		// Token: 0x06000316 RID: 790 RVA: 0x00013869 File Offset: 0x00011A69
		public override bool ForceHostileTo(Faction f)
		{
			return FactionUtility.HostileTo(f, Faction.OfPlayer);
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00013876 File Offset: 0x00011A76
		public override bool ForceHostileTo(Thing t)
		{
			return GenHostility.HostileTo(t, Faction.OfPlayer);
		}
	}
}
