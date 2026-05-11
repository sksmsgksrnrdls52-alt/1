using System;
using UnityEngine;
using VEF.Abilities;
using VEF.Weapons;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x020000FD RID: 253
	public class ChainBolt : TeslaProjectile
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00015CAC File Offset: 0x00013EAC
		protected override int MaxBounceCount
		{
			get
			{
				Ability sourceAbility = this.SourceAbility;
				if (sourceAbility == null)
				{
					return base.MaxBounceCount;
				}
				return Mathf.RoundToInt(sourceAbility.GetPowerForPawn());
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00015CD8 File Offset: 0x00013ED8
		private Ability SourceAbility
		{
			get
			{
				CompAbilityProjectile compAbilityProjectile = ThingCompUtility.TryGetComp<CompAbilityProjectile>(this);
				if (compAbilityProjectile != null)
				{
					Ability ability = compAbilityProjectile.ability;
					if (ability != null)
					{
						return ability;
					}
				}
				int count = this.allProjectiles.Count;
				while (count-- > 0)
				{
					compAbilityProjectile = ThingCompUtility.TryGetComp<CompAbilityProjectile>(this.allProjectiles[count]);
					if (compAbilityProjectile != null)
					{
						Ability ability2 = compAbilityProjectile.ability;
						if (ability2 != null)
						{
							return ability2;
						}
					}
				}
				return null;
			}
		}
	}
}
