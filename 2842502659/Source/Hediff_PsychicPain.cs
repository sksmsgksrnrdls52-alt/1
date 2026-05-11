using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000061 RID: 97
	public class Hediff_PsychicPain : HediffWithComps
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006B8C File Offset: 0x00004D8C
		public override float PainOffset
		{
			get
			{
				return Mathf.Max(StatExtension.GetStatValue(this.pawn, StatDefOf.PsychicSensitivity, true, -1) - 0.8f, 0f);
			}
		}
	}
}
