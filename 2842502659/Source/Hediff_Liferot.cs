using System;
using System.Linq;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000072 RID: 114
	public class Hediff_Liferot : HediffWithComps
	{
		// Token: 0x06000158 RID: 344 RVA: 0x00007CE4 File Offset: 0x00005EE4
		public override void Tick()
		{
			base.Tick();
			if (Gen.IsHashIntervalTick(this.pawn, 60))
			{
				BodyPartRecord bodyPartRecord;
				if (GenCollection.TryRandomElement<BodyPartRecord>(from x in this.pawn.health.hediffSet.GetNotMissingParts(0, 0, null, null)
				where x.coverageAbs > 0f
				select x, ref bodyPartRecord))
				{
					this.pawn.TakeDamage(new DamageInfo(VPE_DefOf.VPE_Rot, 1f, 0f, -1f, null, bodyPartRecord, null, 0, null, true, true, 2, true, false));
				}
			}
		}
	}
}
