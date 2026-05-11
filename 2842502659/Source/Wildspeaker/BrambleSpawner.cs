using System;
using RimWorld;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000E6 RID: 230
	public class BrambleSpawner : PlantSpawner
	{
		// Token: 0x06000321 RID: 801 RVA: 0x000139F4 File Offset: 0x00011BF4
		protected override void SetupPlant(Plant plant, IntVec3 loc, Map map)
		{
			base.SetupPlant(plant, loc, map);
			CompDuration compDuration = ThingCompUtility.TryGetComp<CompDuration>(this);
			if (compDuration != null)
			{
				int durationTicksLeft = compDuration.durationTicksLeft;
				Current.Game.GetComponent<GameComponent_PsycastsManager>().removeAfterTicks.Add(new ValueTuple<Thing, int>(plant, Find.TickManager.TicksGame + durationTicksLeft));
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00013A41 File Offset: 0x00011C41
		protected override ThingDef ChoosePlant(IntVec3 loc, Map map)
		{
			return VPE_DefOf.Plant_Brambles;
		}
	}
}
