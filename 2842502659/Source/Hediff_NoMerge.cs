using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000015 RID: 21
	public class Hediff_NoMerge : HediffWithComps
	{
		// Token: 0x06000038 RID: 56 RVA: 0x00002AA6 File Offset: 0x00000CA6
		public override bool TryMergeWith(Hediff other)
		{
			return false;
		}
	}
}
