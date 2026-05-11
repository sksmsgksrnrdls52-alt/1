using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000071 RID: 113
	public class Hediff_CorpseTalk : HediffWithComps
	{
		// Token: 0x06000154 RID: 340 RVA: 0x00007C06 File Offset: 0x00005E06
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.ResetSkills();
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007C14 File Offset: 0x00005E14
		public void ResetSkills()
		{
			foreach (KeyValuePair<SkillDef, int> keyValuePair in this.skillXPDifferences)
			{
				this.pawn.skills.GetSkill(keyValuePair.Key).Level = Mathf.Max(0, this.pawn.skills.GetSkill(keyValuePair.Key).Level - keyValuePair.Value);
			}
			this.skillXPDifferences.Clear();
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007CB4 File Offset: 0x00005EB4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<SkillDef, int>(ref this.skillXPDifferences, "skillXPDifferences", 4, 1);
		}

		// Token: 0x04000055 RID: 85
		public Dictionary<SkillDef, int> skillXPDifferences = new Dictionary<SkillDef, int>();
	}
}
