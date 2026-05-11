using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x02000120 RID: 288
	[HarmonyPatch]
	public class GameCondition_IntenseShadows : GameCondition
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x000195A3 File Offset: 0x000177A3
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(1f, new SkyColorSet(Color.gray, Color.black, Color.black, 1f), 0.25f, 0.25f));
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000195D7 File Offset: 0x000177D7
		public override float SkyTargetLerpFactor(Map map)
		{
			return 1f;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000195DE File Offset: 0x000177DE
		public override void Init()
		{
			base.Init();
			GameCondition_IntenseShadows.intenseShadowMaps.UnionWith(base.AffectedMaps);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000195F8 File Offset: 0x000177F8
		public override void End()
		{
			foreach (Map item in base.AffectedMaps)
			{
				GameCondition_IntenseShadows.intenseShadowMaps.Remove(item);
			}
			base.End();
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00019658 File Offset: 0x00017858
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == 4)
			{
				GameCondition_IntenseShadows.intenseShadowMaps.UnionWith(base.AffectedMaps);
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00019678 File Offset: 0x00017878
		[HarmonyPatch(typeof(GlowGrid), "GroundGlowAt")]
		[HarmonyPostfix]
		public static void GameGlowAt_Postfix(ref float __result, Map ___map)
		{
			if (__result < 0.5f && GameCondition_IntenseShadows.intenseShadowMaps.Contains(___map))
			{
				__result = 0f;
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00019697 File Offset: 0x00017897
		[HarmonyPatch(typeof(GenCelestial), "CurShadowStrength")]
		[HarmonyPostfix]
		public static void CurShadowStrength_Postfix(Map map, ref float __result)
		{
			if (GameCondition_IntenseShadows.intenseShadowMaps.Contains(map))
			{
				__result = 5f;
			}
		}

		// Token: 0x040001D4 RID: 468
		private static readonly HashSet<Map> intenseShadowMaps = new HashSet<Map>();
	}
}
