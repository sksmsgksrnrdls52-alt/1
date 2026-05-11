using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000109 RID: 265
	public class Hediff_Vortexed : Hediff
	{
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00016664 File Offset: 0x00014864
		public override HediffStage CurStage
		{
			get
			{
				if (this.Vortex != null)
				{
					return new HediffStage
					{
						capMods = new List<PawnCapacityModifier>
						{
							new PawnCapacityModifier
							{
								capacity = PawnCapacityDefOf.Moving,
								setMax = Mathf.Lerp(0.5f, 0.9f, IntVec3Utility.DistanceTo(this.pawn.Position, this.Vortex.Position) / 18.9f)
							},
							new PawnCapacityModifier
							{
								capacity = PawnCapacityDefOf.Manipulation,
								setMax = Mathf.Lerp(0.5f, 0.9f, IntVec3Utility.DistanceTo(this.pawn.Position, this.Vortex.Position) / 18.9f)
							}
						}
					};
				}
				return base.CurStage;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0001672B File Offset: 0x0001492B
		public override bool ShouldRemove
		{
			get
			{
				return this.Vortex.Destroyed || IntVec3Utility.DistanceTo(this.pawn.Position, this.Vortex.Position) >= 18.9f;
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00016761 File Offset: 0x00014961
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Vortex>(ref this.Vortex, "vortex", false);
		}

		// Token: 0x040001B6 RID: 438
		public Vortex Vortex;
	}
}
