using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x02000124 RID: 292
	public class Hediff_ShadowFocus : HediffWithComps
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00019954 File Offset: 0x00017B54
		public override HediffStage CurStage
		{
			get
			{
				return new HediffStage
				{
					statOffsets = new List<StatModifier>
					{
						new StatModifier
						{
							stat = StatDefOf.PsychicSensitivity,
							value = 1f - this.pawn.MapHeld.glowGrid.GroundGlowAt(this.pawn.PositionHeld, false, false)
						}
					}
				};
			}
		}
	}
}
