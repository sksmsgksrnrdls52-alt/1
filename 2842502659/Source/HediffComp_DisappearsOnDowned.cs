using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000012 RID: 18
	public class HediffComp_DisappearsOnDowned : HediffComp
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000029F0 File Offset: 0x00000BF0
		public override bool CompShouldRemove
		{
			get
			{
				return base.Pawn.Downed;
			}
		}
	}
}
