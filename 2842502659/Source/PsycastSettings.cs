using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000BF RID: 191
	public class PsycastSettings : ModSettings
	{
		// Token: 0x06000280 RID: 640 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.XPPerPercent, "xpPerPercent", 1f, false);
			Scribe_Values.Look<float>(ref this.baseSpawnChance, "baseSpawnChance", 0.1f, false);
			Scribe_Values.Look<float>(ref this.additionalAbilityChance, "additionalAbilityChance", 0.1f, false);
			Scribe_Values.Look<bool>(ref this.shrink, "shrink", true, false);
			Scribe_Values.Look<bool>(ref this.muteSkipdoor, "muteSkipdoor", false, false);
			Scribe_Values.Look<MultiCheckboxState>(ref this.smallMode, "smallMode", 2, false);
			Scribe_Values.Look<int>(ref this.maxLevel, "maxLevel", 30, false);
			Scribe_Values.Look<bool>(ref this.changeFocusGain, "changeFocusGain", false, false);
		}

		// Token: 0x040000CF RID: 207
		public float additionalAbilityChance = 0.1f;

		// Token: 0x040000D0 RID: 208
		public float baseSpawnChance = 0.1f;

		// Token: 0x040000D1 RID: 209
		public bool changeFocusGain;

		// Token: 0x040000D2 RID: 210
		public int maxLevel = 30;

		// Token: 0x040000D3 RID: 211
		public bool muteSkipdoor;

		// Token: 0x040000D4 RID: 212
		public bool shrink = true;

		// Token: 0x040000D5 RID: 213
		public MultiCheckboxState smallMode = 2;

		// Token: 0x040000D6 RID: 214
		public float XPPerPercent = 1f;
	}
}
