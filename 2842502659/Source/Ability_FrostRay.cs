using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000AC RID: 172
	public class Ability_FrostRay : Ability
	{
		// Token: 0x06000226 RID: 550 RVA: 0x0000C508 File Offset: 0x0000A708
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Projectile projectile = GenSpawn.Spawn(this.def.GetModExtension<AbilityExtension_Projectile>().projectile, this.pawn.Position, this.pawn.Map, 0) as Projectile;
			AbilityProjectile abilityProjectile = projectile as AbilityProjectile;
			if (abilityProjectile != null)
			{
				abilityProjectile.ability = this;
			}
			if (projectile != null)
			{
				projectile.Launch(this.pawn, this.pawn.DrawPos, (LocalTargetInfo)targets[0], (LocalTargetInfo)targets[0], 1, false, null, null);
			}
			this.pawn.stances.SetStance(new Stance_Stand(this.GetDurationForPawn(), (LocalTargetInfo)targets[0], this.verb));
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000C5C3 File Offset: 0x0000A7C3
		public override void ApplyHediffs(params GlobalTargetInfo[] targetInfo)
		{
			base.ApplyHediff(this.pawn);
		}
	}
}
