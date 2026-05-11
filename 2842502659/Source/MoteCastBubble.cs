using System;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200003E RID: 62
	public class MoteCastBubble : MoteBubble
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00005366 File Offset: 0x00003566
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.durationSecs;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00005379 File Offset: 0x00003579
		public override float Alpha
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005380 File Offset: 0x00003580
		public void Setup(Pawn pawn, Ability ability)
		{
			base.SetupMoteBubble(ability.def.icon, null, null);
			base.Attach(pawn);
			this.durationSecs = Mathf.Max(3f, GenTicks.TicksToSeconds(ability.GetCastTimeForPawn()));
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000053CF File Offset: 0x000035CF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationSecs, "durationSecs", 0f, false);
		}

		// Token: 0x0400003C RID: 60
		private float durationSecs;
	}
}
