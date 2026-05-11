using System;
using RimWorld;
using VEF.Hediffs;
using Verse;

namespace VanillaPsycastsExpanded.Conflagrator
{
	// Token: 0x020000CB RID: 203
	public class HediffComp_FireShield : HediffComp_Shield
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000F85D File Offset: 0x0000DA5D
		protected override void ApplyDamage(DamageInfo dinfo)
		{
			FireUtility.TryAttachFire(dinfo.Instigator, 25f, base.Pawn);
		}
	}
}
