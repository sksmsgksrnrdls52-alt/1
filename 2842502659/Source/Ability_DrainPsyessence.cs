using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000059 RID: 89
	public class Ability_DrainPsyessence : Ability
	{
		// Token: 0x060000FC RID: 252 RVA: 0x00006224 File Offset: 0x00004424
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null)
			{
				if (!pawn.Downed)
				{
					if (showMessages)
					{
						Messages.Message(Translator.Translate("VPE.MustBeDowned"), pawn, MessageTypeDefOf.CautionInput, true);
					}
					return false;
				}
				if ((pawn.Psycasts() == null || pawn.Psycasts().level < 1) && showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustHavePsychicLevel"), pawn, MessageTypeDefOf.CautionInput, true);
				}
			}
			return base.ValidateTarget(target, showMessages);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000062B0 File Offset: 0x000044B0
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				Hediff_PsycastAbilities hediff_PsycastAbilities = pawn.Psycasts();
				Hediff_PsycastAbilities hediff_PsycastAbilities2 = this.pawn.Psycasts();
				int level = hediff_PsycastAbilities.level;
				hediff_PsycastAbilities.experience = 0f;
				hediff_PsycastAbilities2.GainExperience(hediff_PsycastAbilities.experience, true);
				float num = 0f;
				for (int j = 0; j < level; j++)
				{
					num += (float)Hediff_PsycastAbilities.ExperienceRequiredForLevel(j);
				}
				hediff_PsycastAbilities2.GainExperience(num, true);
				pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false));
				pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_PsycastAbilityImplant, false));
				pawn.Kill(null, null);
				pawn.Corpse.GetComp<CompRottable>().RotProgress += 1200000f;
				FilthMaker.TryMakeFilth(pawn.Corpse.Position, pawn.Corpse.Map, ThingDefOf.Filth_CorpseBile, 3, 0, true);
				MoteBetween moteBetween = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_PsycastPsychicEffectTransfer, null);
				moteBetween.Attach(pawn.Corpse, this.pawn);
				moteBetween.Scale = 1f;
				moteBetween.exactPosition = pawn.Corpse.DrawPos;
				GenSpawn.Spawn(moteBetween, pawn.Corpse.Position, pawn.MapHeld, 0);
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.defeated)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(faction, -15, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
		}
	}
}
