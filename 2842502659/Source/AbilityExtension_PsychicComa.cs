using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000080 RID: 128
	public class AbilityExtension_PsychicComa : AbilityExtension_AbilityMod
	{
		// Token: 0x0600017F RID: 383 RVA: 0x000086A0 File Offset: 0x000068A0
		public virtual int GetComaDuration(Ability ability)
		{
			float num = this.hours * 2500f + (float)this.ticks;
			float statValue = StatExtension.GetStatValue(ability.pawn, this.multiplier ?? StatDefOf.PsychicSensitivity, true, -1);
			return Mathf.FloorToInt(num * (Mathf.Approximately(statValue, 0f) ? 10f : (1f / statValue)));
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008700 File Offset: 0x00006900
		public virtual void ApplyComa(Ability ability)
		{
			int comaDuration = this.GetComaDuration(ability);
			if (comaDuration > 0)
			{
				Hediff hediff = HediffMaker.MakeHediff(this.coma ?? VPE_DefOf.PsychicComa, ability.pawn, null);
				HediffUtility.TryGetComp<HediffComp_Disappears>(hediff).ticksToDisappear = comaDuration;
				ability.pawn.health.AddHediff(hediff, null, null, null);
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000875D File Offset: 0x0000695D
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			if (this.autoApply)
			{
				this.ApplyComa(ability);
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008778 File Offset: 0x00006978
		public override string GetDescription(Ability ability)
		{
			int comaDuration = this.GetComaDuration(ability);
			if (comaDuration > 0)
			{
				return ColoredText.Colorize(string.Format("{0}: {1}", Translator.Translate("VPE.PsychicComaDuration"), GenDate.ToStringTicksToPeriod(comaDuration, false, false, true, true, false)), Color.red);
			}
			return string.Empty;
		}

		// Token: 0x04000066 RID: 102
		public float hours;

		// Token: 0x04000067 RID: 103
		public HediffDef coma;

		// Token: 0x04000068 RID: 104
		public StatDef multiplier;

		// Token: 0x04000069 RID: 105
		public int ticks;

		// Token: 0x0400006A RID: 106
		public bool autoApply = true;
	}
}
