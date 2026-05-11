using System;
using System.Collections.Generic;
using RimWorld;
using VEF.Abilities;
using VEF.Weapons;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000AE RID: 174
	public class IceBreatheProjectile : ExpandableProjectile
	{
		// Token: 0x0600022F RID: 559 RVA: 0x0000C758 File Offset: 0x0000A958
		public override void DoDamage(IntVec3 pos)
		{
			base.DoDamage(pos);
			try
			{
				if (pos != this.launcher.Position && this.launcher.Map != null && GenGrid.InBounds(pos, this.launcher.Map))
				{
					base.Map.snowGrid.AddDepth(pos, 0.5f);
					List<Thing> list = this.launcher.Map.thingGrid.ThingsListAt(pos);
					for (int i = list.Count - 1; i >= 0; i--)
					{
						if (this.IsDamagable(list[i]))
						{
							this.customImpact = true;
							base.Impact(list[i], false);
							this.customImpact = false;
							Pawn pawn = list[i] as Pawn;
							if (pawn != null)
							{
								float num = 0.5f / IntVec3Utility.DistanceTo(pawn.Position, this.launcher.Position);
								HediffDef hediffDef;
								if (pawn.CanReceiveHypothermia(out hediffDef))
								{
									HealthUtility.AdjustSeverity(pawn, hediffDef, num);
								}
								HealthUtility.AdjustSeverity(pawn, VPE_DefOf.VFEP_HypothermicSlowdown, num);
								if (this.ability.def.goodwillImpact != 0)
								{
									this.ability.ApplyGoodwillImpact(pawn);
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000C8A8 File Offset: 0x0000AAA8
		public override bool IsDamagable(Thing t)
		{
			return (t is Pawn && base.IsDamagable(t)) || t.def == ThingDefOf.Fire;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000C8CA File Offset: 0x0000AACA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
		}

		// Token: 0x040000A0 RID: 160
		public Ability ability;
	}
}
