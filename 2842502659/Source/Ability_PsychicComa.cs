using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200005A RID: 90
	public class Ability_PsychicComa : Ability
	{
		// Token: 0x060000FF RID: 255 RVA: 0x000064C8 File Offset: 0x000046C8
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			if (!this.def.HasModExtension<AbilityExtension_PsychicComa>())
			{
				Hediff hediff = HediffMaker.MakeHediff(VPE_DefOf.PsychicComa, this.pawn, null);
				HediffUtility.TryGetComp<HediffComp_Disappears>(hediff).ticksToDisappear = (int)(300000f / StatExtension.GetStatValue(this.pawn, StatDefOf.PsychicSensitivity, true, -1));
				this.pawn.health.AddHediff(hediff, null, null, null);
			}
		}
	}
}
