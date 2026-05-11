using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000060 RID: 96
	public class Ability_WordOf : Ability
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00006AC8 File Offset: 0x00004CC8
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			Hediff_GroupLink hediff_GroupLink = this.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_GroupLink, false) as Hediff_GroupLink;
			if (hediff_GroupLink != null)
			{
				List<GlobalTargetInfo> list = targets.ToList<GlobalTargetInfo>();
				using (List<Pawn>.Enumerator enumerator = hediff_GroupLink.linkedPawns.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn linkedPawn = enumerator.Current;
						if (!list.Any((GlobalTargetInfo x) => x.Thing == linkedPawn))
						{
							list.Add(linkedPawn);
						}
					}
				}
				base.Cast(list.ToArray());
				return;
			}
			base.Cast(targets);
		}
	}
}
