using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000067 RID: 103
	public class Ability_SnapFreeze : Ability
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00006EE4 File Offset: 0x000050E4
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (this.def.GetModExtension<AbilityExtension_Hediff>().targetOnlyEnemies && target.Thing != null && !GenHostility.HostileTo(target.Thing, this.pawn))
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VFEA.TargetMustBeHostile"), target.Thing, MessageTypeDefOf.CautionInput, null, true);
				}
				return false;
			}
			return base.ValidateTarget(target, showMessages);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006F54 File Offset: 0x00005154
		public override void ModifyTargets(ref GlobalTargetInfo[] targets)
		{
			this.targetCell = targets[0].Cell;
			base.ModifyTargets(ref targets);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006F70 File Offset: 0x00005170
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Effecter effecter = EffecterDefOf.Skip_Exit.Spawn(this.targetCell, this.pawn.Map, 3f);
			base.AddEffecterToMaintain(effecter, this.targetCell, 60, null);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006FB8 File Offset: 0x000051B8
		public override void ApplyHediffs(params GlobalTargetInfo[] targetInfo)
		{
			foreach (GlobalTargetInfo globalTargetInfo in targetInfo)
			{
				Ability_SnapFreeze.ApplyHediff(this, (LocalTargetInfo)globalTargetInfo);
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006FEC File Offset: 0x000051EC
		public static void ApplyHediff(Ability ability, LocalTargetInfo targetInfo)
		{
			AbilityExtension_Hediff hediffExtension = ability.def.GetModExtension<AbilityExtension_Hediff>();
			if (targetInfo.Pawn != null)
			{
				AbilityExtension_Hediff hediffExtension2 = hediffExtension;
				if (hediffExtension2 != null && hediffExtension2.applyAuto)
				{
					BodyPartRecord bodyPartRecord = (hediffExtension.bodyPartToApply != null) ? ability.pawn.health.hediffSet.GetNotMissingParts(0, 0, null, null).FirstOrDefault((BodyPartRecord x) => x.def == hediffExtension.bodyPartToApply) : null;
					Hediff hediff = HediffMaker.MakeHediff(hediffExtension.hediff, targetInfo.Pawn, bodyPartRecord);
					if (hediffExtension.severity > 1E-45f)
					{
						hediff.Severity = hediffExtension.severity;
					}
					Hediff_Ability hediff_Ability = hediff as Hediff_Ability;
					if (hediff_Ability != null)
					{
						hediff_Ability.ability = ability;
					}
					int num = ability.GetDurationForPawn();
					float ambientTemperature = targetInfo.Pawn.AmbientTemperature;
					if (ambientTemperature >= 0f)
					{
						num = (int)((float)num * (1f - ambientTemperature / 100f));
					}
					if (hediffExtension.durationMultiplier != null)
					{
						num = (int)((float)num * StatExtension.GetStatValue(targetInfo.Pawn, hediffExtension.durationMultiplier, true, -1));
					}
					HediffWithComps hediffWithComps = hediff as HediffWithComps;
					if (hediffWithComps != null)
					{
						foreach (HediffComp hediffComp in hediffWithComps.comps)
						{
							HediffComp_Ability hediffComp_Ability = hediffComp as HediffComp_Ability;
							if (hediffComp_Ability == null)
							{
								HediffComp_Disappears hediffComp_Disappears = hediffComp as HediffComp_Disappears;
								if (hediffComp_Disappears != null)
								{
									hediffComp_Disappears.ticksToDisappear = num;
								}
							}
							else
							{
								hediffComp_Ability.ability = ability;
							}
						}
					}
					targetInfo.Pawn.health.AddHediff(hediff, null, null, null);
					return;
				}
			}
		}

		// Token: 0x04000050 RID: 80
		public IntVec3 targetCell;
	}
}
