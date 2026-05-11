using System;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200005B RID: 91
	public class Ability_GroupLink : Ability
	{
		// Token: 0x06000101 RID: 257 RVA: 0x00006543 File Offset: 0x00004743
		public override Hediff ApplyHediff(Pawn targetPawn, HediffDef hediffDef, BodyPartRecord bodyPart, int duration, float severity)
		{
			Hediff_GroupLink hediff_GroupLink = base.ApplyHediff(targetPawn, hediffDef, bodyPart, duration, severity) as Hediff_GroupLink;
			hediff_GroupLink.LinkAllPawnsAround();
			return hediff_GroupLink;
		}
	}
}
