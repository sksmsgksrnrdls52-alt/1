using System;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000087 RID: 135
	public class CompPsytrainer : CompUseEffect_GiveAbility
	{
		// Token: 0x0600018E RID: 398 RVA: 0x00008988 File Offset: 0x00006B88
		public override void DoEffect(Pawn usedBy)
		{
			AbilityDef ability = base.Props.ability;
			PsycasterPathDef psycasterPathDef;
			if (ability == null)
			{
				psycasterPathDef = null;
			}
			else
			{
				AbilityExtension_Psycast abilityExtension_Psycast = ability.Psycast();
				psycasterPathDef = ((abilityExtension_Psycast != null) ? abilityExtension_Psycast.path : null);
			}
			PsycasterPathDef psycasterPathDef2 = psycasterPathDef;
			if (psycasterPathDef2 != null)
			{
				Hediff_PsycastAbilities hediff_PsycastAbilities = usedBy.Psycasts();
				if (hediff_PsycastAbilities != null && !hediff_PsycastAbilities.unlockedPaths.Contains(psycasterPathDef2))
				{
					hediff_PsycastAbilities.UnlockPath(psycasterPathDef2);
				}
			}
			base.DoEffect(usedBy);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000089E4 File Offset: 0x00006BE4
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			Hediff_PsycastAbilities hediff_PsycastAbilities = p.Psycasts();
			bool flag = hediff_PsycastAbilities == null || hediff_PsycastAbilities.level <= 0;
			if (flag)
			{
				return Translator.Translate("VPE.MustBePsycaster");
			}
			AbilityDef ability = base.Props.ability;
			PsycasterPathDef psycasterPathDef;
			if (ability == null)
			{
				psycasterPathDef = null;
			}
			else
			{
				AbilityExtension_Psycast abilityExtension_Psycast = ability.Psycast();
				psycasterPathDef = ((abilityExtension_Psycast != null) ? abilityExtension_Psycast.path : null);
			}
			PsycasterPathDef psycasterPathDef2 = psycasterPathDef;
			if (psycasterPathDef2 != null && !psycasterPathDef2.CanPawnUnlock(p) && !psycasterPathDef2.ignoreLockRestrictionsForNeurotrainers)
			{
				return base.Props.ability.Psycast().path.lockedReason;
			}
			if (p.GetComp<CompAbilities>().HasAbility(base.Props.ability))
			{
				return TranslatorFormattedStringExtensions.Translate("VPE.AlreadyHasPsycast", base.Props.ability.LabelCap);
			}
			return true;
		}
	}
}
