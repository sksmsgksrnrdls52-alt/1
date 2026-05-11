using System;
using System.Collections.Generic;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000C1 RID: 193
	public class PsySet : IExposable, IRenameable
	{
		// Token: 0x06000289 RID: 649 RVA: 0x0000E9DC File Offset: 0x0000CBDC
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.Name, "name", null, false);
			Scribe_Collections.Look<AbilityDef>(ref this.Abilities, "abilities", 0);
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000EA01 File Offset: 0x0000CC01
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000EA09 File Offset: 0x0000CC09
		public string RenamableLabel
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000EA12 File Offset: 0x0000CC12
		public string BaseLabel
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000EA1A File Offset: 0x0000CC1A
		public string InspectLabel
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x040000D8 RID: 216
		public HashSet<AbilityDef> Abilities = new HashSet<AbilityDef>();

		// Token: 0x040000D9 RID: 217
		public string Name;
	}
}
