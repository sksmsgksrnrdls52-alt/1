using System;
using System.Collections.Generic;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F5 RID: 245
	public class DamageWorker_NeuralHeatBlast : DamageWorker
	{
		// Token: 0x06000364 RID: 868 RVA: 0x0001535C File Offset: 0x0001355C
		protected override void ExplosionDamageThing(Explosion explosion, Thing t, List<Thing> damagedThings, List<Thing> ignoredThings, IntVec3 cell)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null || pawn.psychicEntropy == null || !pawn.HasPsylink)
			{
				return;
			}
			if (damagedThings.Contains(t))
			{
				return;
			}
			damagedThings.Add(t);
			if (ignoredThings != null && ignoredThings.Contains(t))
			{
				return;
			}
			pawn.psychicEntropy.TryAddEntropy(pawn.psychicEntropy.MaxEntropy - pawn.psychicEntropy.EntropyValue, explosion, true, false);
		}
	}
}
