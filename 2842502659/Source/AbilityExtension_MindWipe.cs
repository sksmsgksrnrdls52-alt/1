using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using VEF;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A7 RID: 167
	public class AbilityExtension_MindWipe : AbilityExtension_AbilityMod
	{
		// Token: 0x0600020B RID: 523 RVA: 0x0000B79C File Offset: 0x0000999C
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn.Faction != ability.pawn.Faction)
				{
					pawn.SetFaction(ability.pawn.Faction, null);
				}
				pawn.needs.mood.thoughts.memories.Memories.Clear();
				pawn.relations.ClearAllRelations();
				Dictionary<SkillDef, Passion> dictionary = new Dictionary<SkillDef, Passion>();
				foreach (SkillRecord skillRecord in pawn.skills.skills)
				{
					dictionary[skillRecord.def] = skillRecord.passion;
				}
				pawn.skills = new Pawn_SkillTracker(pawn);
				NonPublicMethods.GenerateSkills(pawn, new PawnGenerationRequest(pawn.kindDef, pawn.Faction, 2, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, 8, null, null, null, false, false, false, -1, 0, false));
				foreach (KeyValuePair<SkillDef, Passion> keyValuePair in dictionary)
				{
					pawn.skills.GetSkill(keyValuePair.Key).passion = keyValuePair.Value;
				}
				if (pawn.ideo.Ideo != ability.pawn.Ideo)
				{
					pawn.ideo.SetIdeo(ability.pawn.Ideo);
				}
			}
		}
	}
}
