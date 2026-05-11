using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000038 RID: 56
	public class SoulFromSky : Skyfaller
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00004960 File Offset: 0x00002B60
		protected override void Impact()
		{
			Pawn innerPawn = this.target.InnerPawn;
			List<Hediff> hediffs = innerPawn.health.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = hediffs[i];
				Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
				if (hediff_MissingPart != null)
				{
					BodyPartRecord part = hediff_MissingPart.Part;
					innerPawn.health.RemoveHediff(hediff);
					innerPawn.health.RestorePart(part, null, true);
				}
				else if (hediff.def != VPE_DefOf.TraumaSavant && (hediff.def.isBad || hediff is Hediff_Addiction) && hediff.def.everCurableByItem)
				{
					innerPawn.health.RemoveHediff(hediff);
				}
			}
			ResurrectionUtility.TryResurrectWithSideEffects(innerPawn);
			if (!innerPawn.Spawned)
			{
				GenSpawn.Spawn(innerPawn, base.Position, base.MapHeld, 0);
			}
			this.Destroy(0);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004A3C File Offset: 0x00002C3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Corpse>(ref this.target, "target", false);
		}

		// Token: 0x04000028 RID: 40
		public Corpse target;
	}
}
