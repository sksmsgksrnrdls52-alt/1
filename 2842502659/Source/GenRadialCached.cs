using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using HarmonyLib;
using RimWorld;
using VEF.CacheClearing;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200002D RID: 45
	[HarmonyPatch]
	public static class GenRadialCached
	{
		// Token: 0x06000078 RID: 120 RVA: 0x00003DCB File Offset: 0x00001FCB
		static GenRadialCached()
		{
			ClearCaches.clearCacheTypes.Add(typeof(GenRadialCached));
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003E00 File Offset: 0x00002000
		public static IEnumerable<Thing> RadialDistinctThingsAround(IntVec3 center, Map map, float radius, bool useCenter)
		{
			GenRadialCached.Key key = new GenRadialCached.Key(center, radius, map.Index, useCenter);
			return GenRadialCached.RadialDistinctThingsAround(ref key, map);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003E28 File Offset: 0x00002028
		private static IEnumerable<Thing> RadialDistinctThingsAround([RequiresLocation] [In] ref GenRadialCached.Key key, Map map)
		{
			if (GenRadialCached.cache == null)
			{
				GenRadialCached.cache = new Dictionary<GenRadialCached.Key, HashSet<Thing>>();
			}
			HashSet<Thing> hashSet;
			if (GenRadialCached.cache.TryGetValue(key, out hashSet))
			{
				return hashSet;
			}
			hashSet = new HashSet<Thing>();
			int num = GenRadial.NumCellsInRadius(key.radius);
			for (int i = (!key.useCenter) ? 1 : 0; i < num; i++)
			{
				IntVec3 intVec = GenRadial.RadialPattern[i] + key.loc;
				if (GenGrid.InBounds(intVec, map))
				{
					hashSet.UnionWith(GridsUtility.GetThingList(intVec, map));
				}
			}
			GenRadialCached.cache[key] = hashSet;
			return hashSet;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003EC4 File Offset: 0x000020C4
		public static float WealthAround(IntVec3 center, Map map, float radius, bool useCenter)
		{
			GenRadialCached.Key key = new GenRadialCached.Key(center, radius, map.Index, useCenter);
			if (GenRadialCached.wealthCache == null)
			{
				GenRadialCached.wealthCache = new Dictionary<GenRadialCached.Key, float>();
			}
			float result;
			if (GenRadialCached.wealthCache.TryGetValue(key, out result))
			{
				return result;
			}
			IEnumerable<Thing> enumerable = GenRadialCached.RadialDistinctThingsAround(ref key, map);
			float num = 0f;
			foreach (Thing thing in enumerable)
			{
				num += StatExtension.GetStatValue(thing, StatDefOf.MarketValue, true, -1) * (float)thing.stackCount;
			}
			GenRadialCached.wealthCache[key] = num;
			return num;
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003F6C File Offset: 0x0000216C
		public static IEnumerable<CompMeditationFocus> MeditationFociAround(IntVec3 center, Map map, float radius, bool useCenter)
		{
			GenRadialCached.Key key = new GenRadialCached.Key(center, radius, map.Index, useCenter);
			if (GenRadialCached.meditationFocusCache == null)
			{
				GenRadialCached.meditationFocusCache = new Dictionary<GenRadialCached.Key, HashSet<CompMeditationFocus>>();
			}
			HashSet<CompMeditationFocus> hashSet;
			if (GenRadialCached.meditationFocusCache.TryGetValue(key, out hashSet))
			{
				return hashSet;
			}
			hashSet = new HashSet<CompMeditationFocus>();
			foreach (Thing thing in GenRadialCached.RadialDistinctThingsAround(ref key, map))
			{
				CompMeditationFocus compMeditationFocus = ThingCompUtility.TryGetComp<CompMeditationFocus>(thing);
				if (compMeditationFocus != null)
				{
					hashSet.Add(compMeditationFocus);
				}
			}
			GenRadialCached.meditationFocusCache[key] = hashSet;
			return hashSet;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000400C File Offset: 0x0000220C
		[HarmonyPatch(typeof(Thing), "SpawnSetup")]
		[HarmonyPostfix]
		public static void SpawnSetup_Postfix(Thing __instance)
		{
			GenRadialCached.ClearCacheFor(__instance);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004014 File Offset: 0x00002214
		[HarmonyPatch(typeof(Thing), "DeSpawn")]
		[HarmonyPrefix]
		public static void DeSpawn_Prefix(Thing __instance)
		{
			GenRadialCached.ClearCacheFor(__instance);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000401C File Offset: 0x0000221C
		[HarmonyPatch(typeof(MapDeiniter), "Deinit")]
		[HarmonyPostfix]
		public static void Deinit_Postfix(Map map)
		{
			int index = map.Index;
			foreach (KeyValuePair<GenRadialCached.Key, HashSet<Thing>> keyValuePair in GenRadialCached.cache.ToList<KeyValuePair<GenRadialCached.Key, HashSet<Thing>>>())
			{
				GenRadialCached.Key key;
				HashSet<Thing> hashSet;
				GenCollection.Deconstruct<GenRadialCached.Key, HashSet<Thing>>(keyValuePair, ref key, ref hashSet);
				GenRadialCached.Key key2 = key;
				HashSet<Thing> value = hashSet;
				if (key2.mapId >= index)
				{
					GenRadialCached.cache.Remove(key2);
					HashSet<CompMeditationFocus> hashSet2;
					if (GenRadialCached.meditationFocusCache.TryGetValue(key2, out hashSet2))
					{
						GenRadialCached.meditationFocusCache.Remove(key2);
					}
					float naN;
					if (GenRadialCached.wealthCache.TryGetValue(key2, out naN))
					{
						GenRadialCached.wealthCache.Remove(key2);
					}
					else
					{
						naN = float.NaN;
					}
					if (key2.mapId != index)
					{
						GenRadialCached.Key key3 = key2.DecrementMapId();
						GenRadialCached.cache.Add(key3, value);
						if (hashSet2 != null)
						{
							GenRadialCached.meditationFocusCache.Add(key3, hashSet2);
						}
						if (!float.IsNaN(naN))
						{
							GenRadialCached.wealthCache.Add(key3, naN);
						}
					}
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004128 File Offset: 0x00002328
		private static void ClearCacheFor(Thing thing)
		{
			if (!thing.Spawned)
			{
				return;
			}
			GenCollection.RemoveAll<GenRadialCached.Key, HashSet<Thing>>(GenRadialCached.cache, delegate(KeyValuePair<GenRadialCached.Key, HashSet<Thing>> pair)
			{
				if (pair.Key.mapId != thing.Map.Index || !GenAdj.OccupiedRect(thing).ClosestCellTo(pair.Key.loc).InHorDistOf(pair.Key.loc, pair.Key.radius))
				{
					return false;
				}
				GenRadialCached.meditationFocusCache.Remove(pair.Key);
				GenRadialCached.wealthCache.Remove(pair.Key);
				return true;
			});
		}

		// Token: 0x0400001C RID: 28
		private static Dictionary<GenRadialCached.Key, HashSet<Thing>> cache = new Dictionary<GenRadialCached.Key, HashSet<Thing>>();

		// Token: 0x0400001D RID: 29
		private static Dictionary<GenRadialCached.Key, HashSet<CompMeditationFocus>> meditationFocusCache = new Dictionary<GenRadialCached.Key, HashSet<CompMeditationFocus>>();

		// Token: 0x0400001E RID: 30
		private static Dictionary<GenRadialCached.Key, float> wealthCache = new Dictionary<GenRadialCached.Key, float>();

		// Token: 0x02000152 RID: 338
		private readonly struct Key : IEquatable<GenRadialCached.Key>
		{
			// Token: 0x060004E3 RID: 1251 RVA: 0x0001CE3B File Offset: 0x0001B03B
			public Key(IntVec3 loc, float radius, int mapId, bool useCenter)
			{
				this.loc = loc;
				this.radius = radius;
				this.mapId = mapId;
				this.useCenter = useCenter;
			}

			// Token: 0x060004E4 RID: 1252 RVA: 0x0001CE5A File Offset: 0x0001B05A
			public GenRadialCached.Key DecrementMapId()
			{
				return new GenRadialCached.Key(this.loc, this.radius, this.mapId - 1, this.useCenter);
			}

			// Token: 0x060004E5 RID: 1253 RVA: 0x0001CE7C File Offset: 0x0001B07C
			public bool Equals(GenRadialCached.Key other)
			{
				return this.loc.Equals(other.loc) && this.radius.Equals(other.radius) && this.mapId == other.mapId && this.useCenter == other.useCenter;
			}

			// Token: 0x060004E6 RID: 1254 RVA: 0x0001CED4 File Offset: 0x0001B0D4
			public override bool Equals(object obj)
			{
				if (obj is GenRadialCached.Key)
				{
					GenRadialCached.Key other = (GenRadialCached.Key)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x060004E7 RID: 1255 RVA: 0x0001CEFC File Offset: 0x0001B0FC
			public override int GetHashCode()
			{
				return Gen.HashCombineInt(this.loc.GetHashCode(), this.radius.GetHashCode(), this.mapId, this.useCenter.GetHashCode());
			}

			// Token: 0x0400022D RID: 557
			public readonly IntVec3 loc;

			// Token: 0x0400022E RID: 558
			public readonly float radius;

			// Token: 0x0400022F RID: 559
			public readonly int mapId;

			// Token: 0x04000230 RID: 560
			public readonly bool useCenter;
		}
	}
}
