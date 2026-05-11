using System;
using Verse;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x02000121 RID: 289
	public class HediffComp_DissapearsInLight : HediffComp
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x000196C4 File Offset: 0x000178C4
		public override bool CompShouldRemove
		{
			get
			{
				Map mapHeld = base.Pawn.MapHeld;
				float? num;
				if (mapHeld == null)
				{
					num = null;
				}
				else
				{
					GlowGrid glowGrid = mapHeld.glowGrid;
					num = ((glowGrid != null) ? new float?(glowGrid.GroundGlowAt(base.Pawn.PositionHeld, false, false)) : null);
				}
				float? num2 = num;
				return num2.GetValueOrDefault() >= 0.21f;
			}
		}
	}
}
