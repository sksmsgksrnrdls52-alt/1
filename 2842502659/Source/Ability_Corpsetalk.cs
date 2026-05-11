using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200006C RID: 108
	public class Ability_Corpsetalk : Ability_TargetCorpse
	{
		// Token: 0x06000148 RID: 328 RVA: 0x000078F4 File Offset: 0x00005AF4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Hediff_CorpseTalk hediff_CorpseTalk = base.ApplyHediff(this.pawn) as Hediff_CorpseTalk;
			if (hediff_CorpseTalk.skillXPDifferences != null)
			{
				hediff_CorpseTalk.ResetSkills();
			}
			else
			{
				hediff_CorpseTalk.skillXPDifferences = new Dictionary<SkillDef, int>();
			}
			Corpse corpse = targets[0].Thing as Corpse;
			foreach (SkillDef skillDef in DefDatabase<SkillDef>.AllDefs)
			{
				SkillRecord skill = this.pawn.skills.GetSkill(skillDef);
				int num = corpse.InnerPawn.skills.GetSkill(skillDef).Level - skill.Level;
				if (num > 0)
				{
					int level = skill.Level;
					skill.Level = Mathf.Min(20, skill.Level + num);
					hediff_CorpseTalk.skillXPDifferences[skillDef] = skill.Level - level;
				}
			}
		}
	}
}
