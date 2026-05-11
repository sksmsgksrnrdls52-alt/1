using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.UI
{
	// Token: 0x020000DA RID: 218
	[StaticConstructorOnStartup]
	public static class TexPsycasts
	{
		// Token: 0x04000187 RID: 391
		public static Texture2D IconPsyfocusGain = ContentFinder<Texture2D>.Get("UI/Icons/IconPsyfocusGain", true);

		// Token: 0x04000188 RID: 392
		public static Texture2D IconFocusTypes = ContentFinder<Texture2D>.Get("UI/Icons/IconFocusTypes", true);

		// Token: 0x04000189 RID: 393
		public static Texture2D IconNeuralHeatLimit = ContentFinder<Texture2D>.Get("UI/Icons/IconNeuralHeatLimit", true);

		// Token: 0x0400018A RID: 394
		public static Texture2D IconNeuralHeatRegenRate = ContentFinder<Texture2D>.Get("UI/Icons/IconNeuralHeatRegenRate", true);

		// Token: 0x0400018B RID: 395
		public static Texture2D IconPsychicSensitivity = ContentFinder<Texture2D>.Get("UI/Icons/IconPsychicSensitivity", true);

		// Token: 0x0400018C RID: 396
		public static Texture2D IconPsyfocusCost = ContentFinder<Texture2D>.Get("UI/Icons/IconPsyfocusCost", true);
	}
}
