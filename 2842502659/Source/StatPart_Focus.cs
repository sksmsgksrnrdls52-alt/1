using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000C4 RID: 196
	public abstract class StatPart_Focus : StatPart
	{
		// Token: 0x06000295 RID: 661 RVA: 0x0000EADC File Offset: 0x0000CCDC
		public bool ApplyOn(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && this.focus.CanPawnUse(pawn) && StatPart_NearbyFoci.ShouldApply;
		}

		// Token: 0x040000DF RID: 223
		public MeditationFocusDef focus;
	}
}
