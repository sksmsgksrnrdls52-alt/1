using System;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000107 RID: 263
	public class HediffComp_Hurricane : HediffComp_SeverityPerDay
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x000163E8 File Offset: 0x000145E8
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (base.Pawn.Map.weatherManager.CurWeatherPerceived != VPE_DefOf.VPE_Hurricane_Weather)
			{
				base.CompPostTick(ref severityAdjustment);
			}
		}
	}
}
