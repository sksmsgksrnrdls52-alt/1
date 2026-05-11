using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x0200012A RID: 298
	public class Ability_Skillroll : Ability
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x00019EF4 File Offset: 0x000180F4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Pawn pawn = targets[0].Thing as Pawn;
			int num = 0;
			PawnKindDef kindDef = pawn.kindDef;
			Faction faction = null;
			PawnGenerationContext pawnGenerationContext = 2;
			DevelopmentalStage developmentalStage = pawn.DevelopmentalStage;
			PawnGenerationRequest arg = new PawnGenerationRequest(kindDef, faction, pawnGenerationContext, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStage, null, null, null, false, false, false, -1, 0, false);
			foreach (SkillRecord skillRecord in pawn.skills.skills)
			{
				int levelInt = skillRecord.levelInt;
				skillRecord.levelInt = Ability_Skillroll.finalLevelOfSkill(pawn, skillRecord.def, arg);
				num += levelInt - skillRecord.levelInt;
			}
			num = Mathf.RoundToInt((float)num * 1.1f);
			for (int i = 0; i < num; i++)
			{
				GenCollection.RandomElement<SkillRecord>(from skill in pawn.skills.skills
				where !skill.TotallyDisabled && skill.levelInt < 20
				select skill).levelInt++;
			}
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001A090 File Offset: 0x00018290
		public override bool CanHitTarget(LocalTargetInfo target)
		{
			if (base.CanHitTarget(target))
			{
				Pawn pawn = target.Pawn;
				if (pawn != null)
				{
					Faction faction = pawn.Faction;
					if (faction != null)
					{
						pawn = this.pawn;
						if (pawn != null)
						{
							Faction faction2 = pawn.Faction;
							if (faction2 != null)
							{
								return faction2 == faction || faction2.RelationKindWith(faction) == 2;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x040001D7 RID: 471
		private static readonly Func<Pawn, SkillDef, PawnGenerationRequest, int> finalLevelOfSkill = AccessTools.Method(typeof(PawnGenerator), "FinalLevelOfSkill", null, null).CreateDelegate<Func<Pawn, SkillDef, PawnGenerationRequest, int>>();
	}
}
