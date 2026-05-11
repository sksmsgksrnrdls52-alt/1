using System;
using RimWorld;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000035 RID: 53
	public abstract class Ability_TargetCorpse : Ability
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000046BC File Offset: 0x000028BC
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Corpse corpse = target.Thing as Corpse;
			if (corpse == null)
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustBeCorpse"), corpse, MessageTypeDefOf.CautionInput, true);
				}
				return false;
			}
			if (!corpse.InnerPawn.RaceProps.Humanlike)
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustBeCorpseHumanlike"), corpse, MessageTypeDefOf.CautionInput, true);
				}
				return false;
			}
			return base.ValidateTarget(target, showMessages);
		}
	}
}
