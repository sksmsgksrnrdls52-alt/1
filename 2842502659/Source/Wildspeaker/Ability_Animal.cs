using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000DB RID: 219
	public class Ability_Animal : Ability
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x00012E6C File Offset: 0x0001106C
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null && WildManUtility.AnimalOrWildMan(pawn))
				{
					bool flag = pawn.MentalStateDef == MentalStateDefOf.Manhunter || pawn.MentalStateDef == MentalStateDefOf.ManhunterPermanent;
					if (Rand.Chance(this.GetSuccessChanceOn(pawn)))
					{
						if (flag)
						{
							pawn.MentalState.RecoverFromState();
						}
						else
						{
							InteractionWorker_RecruitAttempt.DoRecruit(this.pawn, pawn, true);
						}
					}
					else if (!flag)
					{
						pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, Translator.Translate("AnimalManhunterFromTaming"), true, false, false, null, false, false, true);
					}
				}
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00012F36 File Offset: 0x00011136
		private float GetSuccessChanceOn(Pawn target)
		{
			return StatExtension.GetStatValue(this.pawn, StatDefOf.PsychicSensitivity, true, -1) - StatExtension.GetStatValueAbstract(target.def, StatDefOf.Wildness, null);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00012F5C File Offset: 0x0001115C
		public override void OnGUI(LocalTargetInfo target)
		{
			base.OnGUI(target);
			List<GlobalTargetInfo> list = (from t in this.currentTargets
			where t.IsValid && t.Map != null
			select t).ToList<GlobalTargetInfo>();
			GlobalTargetInfo[] array = new GlobalTargetInfo[list.Count + 1];
			list.CopyTo(array, 0);
			array[array.Length - 1] = target.ToGlobalTargetInfo(((list != null) ? list.LastOrDefault<GlobalTargetInfo>().Map : null) ?? this.pawn.Map);
			this.ModifyTargets(ref array);
			foreach (GlobalTargetInfo globalTargetInfo in array)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					if (WildManUtility.AnimalOrWildMan(pawn) && this.GetSuccessChanceOn(pawn) > 1E-45f)
					{
						float successChanceOn = this.GetSuccessChanceOn(pawn);
						Vector3 drawPos = pawn.DrawPos;
						drawPos.z += 1f;
						Color color;
						if (successChanceOn >= 0.33f)
						{
							if (successChanceOn >= 0.66f)
							{
								color = Color.green;
							}
							else
							{
								color = Color.white;
							}
						}
						else
						{
							color = Color.yellow;
						}
						Color color2 = color;
						GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), Translator.Translate("VPE.SuccessChance") + ": " + GenText.ToStringPercent(successChanceOn), color2);
					}
					else
					{
						Vector3 drawPos2 = pawn.DrawPos;
						drawPos2.z += 1f;
						GenMapUI.DrawText(new Vector2(drawPos2.x, drawPos2.z), Translator.Translate("Ineffective"), Color.red);
					}
				}
			}
		}
	}
}
