using System;
using System.Globalization;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000078 RID: 120
	public class Command_Ability_Psycast : Command_Ability
	{
		// Token: 0x0600016A RID: 362 RVA: 0x000082C4 File Offset: 0x000064C4
		public Command_Ability_Psycast(Pawn pawn, Ability ability) : base(pawn, ability)
		{
			this.psycastExtension = this.ability.def.GetModExtension<AbilityExtension_Psycast>();
			this.shrinkable = PsycastsMod.Settings.shrink;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600016B RID: 363 RVA: 0x000082F4 File Offset: 0x000064F4
		public override string TopRightLabel
		{
			get
			{
				if (this.ability.AutoCast)
				{
					return null;
				}
				string text = string.Empty;
				float entropyUsedByPawn = this.psycastExtension.GetEntropyUsedByPawn(this.ability.pawn);
				if (entropyUsedByPawn > 1E-45f)
				{
					text += Translator.Translate("NeuralHeatLetter") + ": " + entropyUsedByPawn.ToString(CultureInfo.CurrentCulture) + "\n";
				}
				float psyfocusUsedByPawn = this.psycastExtension.GetPsyfocusUsedByPawn(this.ability.pawn);
				if (psyfocusUsedByPawn > 1E-45f)
				{
					text += Translator.Translate("PsyfocusLetter") + ": " + GenText.ToStringPercent(psyfocusUsedByPawn);
				}
				return GenText.TrimEndNewlines(text);
			}
		}

		// Token: 0x0400005E RID: 94
		private readonly AbilityExtension_Psycast psycastExtension;
	}
}
