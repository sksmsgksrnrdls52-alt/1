using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B2 RID: 178
	public class HediffComp_ShouldBeDestroyed : HediffComp
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000CE5D File Offset: 0x0000B05D
		public override bool CompShouldRemove
		{
			get
			{
				return true;
			}
		}
	}
}
