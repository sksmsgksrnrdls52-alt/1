using System;
using System.Collections.Generic;
using LudeonTK;
using UnityEngine;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x0200011F RID: 287
	public static class DecoyOverlayUtility
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00019516 File Offset: 0x00017716
		public static Color OverlayColor
		{
			get
			{
				return new Color(DecoyOverlayUtility.ColorR, DecoyOverlayUtility.ColorG, DecoyOverlayUtility.ColorB, DecoyOverlayUtility.ColorA);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00019534 File Offset: 0x00017734
		public static Material GetDuplicateMat(Material baseMat)
		{
			Material material;
			if (!DecoyOverlayUtility.Materials.TryGetValue(baseMat, out material))
			{
				material = MaterialAllocator.Create(baseMat);
				material.color = DecoyOverlayUtility.OverlayColor;
				DecoyOverlayUtility.Materials[baseMat] = material;
			}
			return material;
		}

		// Token: 0x040001CE RID: 462
		[TweakValue("00", 0f, 1f)]
		public static float ColorR = 0f;

		// Token: 0x040001CF RID: 463
		[TweakValue("00", 0f, 1f)]
		public static float ColorG = 0f;

		// Token: 0x040001D0 RID: 464
		[TweakValue("00", 0f, 1f)]
		public static float ColorB = 0f;

		// Token: 0x040001D1 RID: 465
		[TweakValue("00", 0f, 1f)]
		public static float ColorA = 1f;

		// Token: 0x040001D2 RID: 466
		private static readonly Dictionary<Material, Material> Materials = new Dictionary<Material, Material>();

		// Token: 0x040001D3 RID: 467
		public static bool DrawOverlay;
	}
}
