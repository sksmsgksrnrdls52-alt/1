using System;
using System.Collections.Generic;
using System.Linq;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Skipmaster
{
	// Token: 0x02000111 RID: 273
	public class AbilityExtension_Wallraise : AbilityExtension_AbilityMod
	{
		// Token: 0x060003E0 RID: 992 RVA: 0x00017F50 File Offset: 0x00016150
		internal IEnumerable<IntVec3> AffectedCells(LocalTargetInfo target, Map map)
		{
			return from intVec in this.pattern
			select target.Cell + new IntVec3(intVec.x, 0, intVec.z) into intVec2
			where GenGrid.InBounds(intVec2, map)
			select intVec2;
		}

		// Token: 0x040001C1 RID: 449
		public List<IntVec2> pattern;

		// Token: 0x040001C2 RID: 450
		public float screenShakeIntensity;
	}
}
