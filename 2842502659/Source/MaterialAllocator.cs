using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000073 RID: 115
	internal static class MaterialAllocator
	{
		// Token: 0x0600015A RID: 346 RVA: 0x00007D84 File Offset: 0x00005F84
		public static Material Create(Material material)
		{
			Material material2 = new Material(material);
			MaterialAllocator.references[material2] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (Prefs.DevMode ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material2;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007DCC File Offset: 0x00005FCC
		public static Material Create(Shader shader)
		{
			Material material = new Material(shader);
			MaterialAllocator.references[material] = new MaterialAllocator.MaterialInfo
			{
				stackTrace = (Prefs.DevMode ? Environment.StackTrace : "(unavailable)")
			};
			MaterialAllocator.TryReport();
			return material;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007E14 File Offset: 0x00006014
		public static void Destroy(Material material)
		{
			if (!MaterialAllocator.references.ContainsKey(material))
			{
				Log.Error(string.Format("Destroying material {0}, but that material was not created through the MaterialTracker", material));
			}
			MaterialAllocator.references.Remove(material);
			Object.Destroy(material);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007E48 File Offset: 0x00006048
		public static void TryReport()
		{
			if (MaterialAllocator.MaterialWarningThreshold() > MaterialAllocator.nextWarningThreshold)
			{
				MaterialAllocator.nextWarningThreshold = MaterialAllocator.MaterialWarningThreshold();
			}
			if (MaterialAllocator.references.Count > MaterialAllocator.nextWarningThreshold)
			{
				Log.Error(string.Format("Material allocator has allocated {0} materials; this may be a sign of a material leak", MaterialAllocator.references.Count));
				if (Prefs.DevMode)
				{
					MaterialAllocator.MaterialReport();
				}
				MaterialAllocator.nextWarningThreshold *= 2;
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007EB2 File Offset: 0x000060B2
		public static int MaterialWarningThreshold()
		{
			return int.MaxValue;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007EBC File Offset: 0x000060BC
		[DebugOutput("System", false)]
		public static void MaterialReport()
		{
			foreach (string text in (from g in MaterialAllocator.references.GroupBy(delegate(KeyValuePair<Material, MaterialAllocator.MaterialInfo> kvp)
			{
				KeyValuePair<Material, MaterialAllocator.MaterialInfo> keyValuePair = kvp;
				return keyValuePair.Value.stackTrace;
			})
			orderby g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>() descending
			select string.Format("{0}: {1}", g.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>(), g.FirstOrDefault<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>().Value.stackTrace)).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007F78 File Offset: 0x00006178
		[DebugOutput("System", false)]
		public static void MaterialSnapshot()
		{
			MaterialAllocator.snapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in MaterialAllocator.references.GroupBy(delegate(KeyValuePair<Material, MaterialAllocator.MaterialInfo> kvp)
			{
				KeyValuePair<Material, MaterialAllocator.MaterialInfo> keyValuePair = kvp;
				return keyValuePair.Value.stackTrace;
			}))
			{
				MaterialAllocator.snapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008004 File Offset: 0x00006204
		[DebugOutput("System", false)]
		public static void MaterialDelta()
		{
			IEnumerable<string> enumerable = (from v in MaterialAllocator.references.Values
			select v.stackTrace).Concat(MaterialAllocator.snapshot.Keys).Distinct<string>();
			Dictionary<string, int> currentSnapshot = new Dictionary<string, int>();
			foreach (IGrouping<string, KeyValuePair<Material, MaterialAllocator.MaterialInfo>> grouping in MaterialAllocator.references.GroupBy(delegate(KeyValuePair<Material, MaterialAllocator.MaterialInfo> kvp)
			{
				KeyValuePair<Material, MaterialAllocator.MaterialInfo> keyValuePair = kvp;
				return keyValuePair.Value.stackTrace;
			}))
			{
				currentSnapshot[grouping.Key] = grouping.Count<KeyValuePair<Material, MaterialAllocator.MaterialInfo>>();
			}
			IEnumerable<string> source = enumerable;
			Func<string, KeyValuePair<string, int>> <>9__2;
			Func<string, KeyValuePair<string, int>> selector;
			if ((selector = <>9__2) == null)
			{
				selector = (<>9__2 = ((string k) => new KeyValuePair<string, int>(k, GenCollection.TryGetValue<string, int>(currentSnapshot, k, 0) - GenCollection.TryGetValue<string, int>(MaterialAllocator.snapshot, k, 0))));
			}
			foreach (string text in source.Select(selector).OrderByDescending(delegate(KeyValuePair<string, int> kvp)
			{
				KeyValuePair<string, int> keyValuePair = kvp;
				return keyValuePair.Value;
			}).Select(delegate(KeyValuePair<string, int> g)
			{
				string format = "{0}: {1}";
				KeyValuePair<string, int> keyValuePair = g;
				object arg = keyValuePair.Value;
				keyValuePair = g;
				return string.Format(format, arg, keyValuePair.Key);
			}).Take(20))
			{
				Log.Error(text);
			}
		}

		// Token: 0x04000056 RID: 86
		private static readonly Dictionary<Material, MaterialAllocator.MaterialInfo> references = new Dictionary<Material, MaterialAllocator.MaterialInfo>();

		// Token: 0x04000057 RID: 87
		public static int nextWarningThreshold;

		// Token: 0x04000058 RID: 88
		private static Dictionary<string, int> snapshot = new Dictionary<string, int>();

		// Token: 0x02000165 RID: 357
		private struct MaterialInfo
		{
			// Token: 0x04000266 RID: 614
			public string stackTrace;
		}
	}
}
