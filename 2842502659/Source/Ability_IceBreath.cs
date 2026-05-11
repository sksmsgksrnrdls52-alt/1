using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000032 RID: 50
	public class Ability_IceBreath : Ability_ShootProjectile
	{
		// Token: 0x0600008D RID: 141 RVA: 0x000043F8 File Offset: 0x000025F8
		protected override Projectile ShootProjectile(GlobalTargetInfo target)
		{
			IceBreatheProjectile iceBreatheProjectile = base.ShootProjectile(target) as IceBreatheProjectile;
			iceBreatheProjectile.ability = this;
			return iceBreatheProjectile;
		}
	}
}
