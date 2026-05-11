using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000017 RID: 23
	public interface IMinHeatGiver : ILoadReferenceable
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003B RID: 59
		bool IsActive { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003C RID: 60
		int MinHeat { get; }
	}
}
