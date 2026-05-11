using System;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000104 RID: 260
	public class Command_AbilityToggle : Command_Ability
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x000162DD File Offset: 0x000144DD
		public Command_AbilityToggle(Pawn pawn, Ability ability) : base(pawn, ability)
		{
			if (this.Toggle.Toggle)
			{
				this.disabled = false;
				this.disabledReason = null;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00016302 File Offset: 0x00014502
		public IAbilityToggle Toggle
		{
			get
			{
				return this.ability as IAbilityToggle;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0001630F File Offset: 0x0001450F
		public override string Label
		{
			get
			{
				if (!this.Toggle.Toggle)
				{
					return base.Label;
				}
				return this.Toggle.OffLabel;
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00016330 File Offset: 0x00014530
		public override void ProcessInput(Event ev)
		{
			this.Toggle.Toggle = !this.Toggle.Toggle;
		}
	}
}
