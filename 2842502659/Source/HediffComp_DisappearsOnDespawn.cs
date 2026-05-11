using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200000E RID: 14
	public class HediffComp_DisappearsOnDespawn : HediffComp
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000028EA File Offset: 0x00000AEA
		public override bool CompShouldRemove
		{
			get
			{
				return base.Pawn.MapHeld == null;
			}
		}
	}
}
