using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000031 RID: 49
	public class AbilityExtension_GameCondition : AbilityExtension_AbilityMod
	{
		// Token: 0x0600008B RID: 139 RVA: 0x0000433C File Offset: 0x0000253C
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.gameCondition, (this.durationDays != null) ? ((int)(this.durationDays.Value.RandomInRange * 60000f)) : ability.GetDurationForPawn());
			ability.pawn.Map.gameConditionManager.RegisterCondition(gameCondition);
			if (this.sendLetter)
			{
				ChoiceLetter choiceLetter = LetterMaker.MakeLetter(this.gameCondition.LabelCap, this.gameCondition.letterText, LetterDefOf.NegativeEvent, LookTargets.Invalid, null, null, this.gameCondition.letterHyperlinks);
				Find.LetterStack.ReceiveLetter(choiceLetter, null, 0, true);
			}
		}

		// Token: 0x04000022 RID: 34
		public GameConditionDef gameCondition;

		// Token: 0x04000023 RID: 35
		public FloatRange? durationDays;

		// Token: 0x04000024 RID: 36
		public bool sendLetter;
	}
}
