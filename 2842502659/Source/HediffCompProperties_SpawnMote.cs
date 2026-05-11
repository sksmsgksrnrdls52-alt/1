using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200000F RID: 15
	public class HediffCompProperties_SpawnMote : HediffCompProperties
	{
		// Token: 0x0600002C RID: 44 RVA: 0x00002902 File Offset: 0x00000B02
		public HediffCompProperties_SpawnMote()
		{
			this.compClass = typeof(HediffComp_SpawnMote);
		}

		// Token: 0x04000009 RID: 9
		public ThingDef moteDef;

		// Token: 0x0400000A RID: 10
		public Vector3 offset;

		// Token: 0x0400000B RID: 11
		public float maxScale;
	}
}
