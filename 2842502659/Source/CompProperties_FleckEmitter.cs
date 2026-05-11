using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000025 RID: 37
	public class CompProperties_FleckEmitter : CompProperties
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000038A5 File Offset: 0x00001AA5
		public CompProperties_FleckEmitter()
		{
			this.compClass = typeof(CompFleckEmitter);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000038CF File Offset: 0x00001ACF
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_FleckEmitter.<ConfigErrors>d__7 <ConfigErrors>d__ = new CompProperties_FleckEmitter.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04000014 RID: 20
		public FleckDef fleck;

		// Token: 0x04000015 RID: 21
		public float scale = 1f;

		// Token: 0x04000016 RID: 22
		public Vector3 offset;

		// Token: 0x04000017 RID: 23
		public int emissionInterval = -1;

		// Token: 0x04000018 RID: 24
		public SoundDef soundOnEmission;

		// Token: 0x04000019 RID: 25
		public string saveKeysPrefix;
	}
}
