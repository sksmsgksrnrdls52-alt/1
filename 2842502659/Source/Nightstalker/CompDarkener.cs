using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x0200011C RID: 284
	[HarmonyPatch]
	[StaticConstructorOnStartup]
	public class CompDarkener : ThingComp
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x000190BD File Offset: 0x000172BD
		private CompProperties_Darkness Props
		{
			get
			{
				return (CompProperties_Darkness)this.props;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000190CC File Offset: 0x000172CC
		private static Dictionary<IntVec3, int> DarkCellsFor(Map map, bool create = true)
		{
			Dictionary<IntVec3, int> dictionary;
			if (!CompDarkener.darkCells.TryGetValue(map, out dictionary))
			{
				if (!create)
				{
					return null;
				}
				dictionary = new Dictionary<IntVec3, int>();
				CompDarkener.darkCells[map] = dictionary;
			}
			return dictionary;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00019100 File Offset: 0x00017300
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			Dictionary<IntVec3, int> dictionary = CompDarkener.DarkCellsFor(this.parent.Map, true);
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.Props.darknessRange, true))
			{
				int num;
				if (dictionary.TryGetValue(intVec, out num))
				{
					dictionary[intVec] = num + 1;
				}
				else
				{
					dictionary[intVec] = 1;
				}
				this.parent.Map.glowGrid.LightBlockerAdded(intVec);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000191A4 File Offset: 0x000173A4
		public override void PostDeSpawn(Map map, DestroyMode mode = 0)
		{
			Dictionary<IntVec3, int> dictionary = CompDarkener.DarkCellsFor(map, true);
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.Props.darknessRange, true))
			{
				int num;
				if (dictionary.TryGetValue(intVec, out num))
				{
					if (num == 1)
					{
						dictionary.Remove(intVec);
					}
					else
					{
						dictionary[intVec] = num - 1;
					}
				}
				else
				{
					num = 0;
				}
				bool flag = false;
				List<Thing> list = map.thingGrid.ThingsListAt(intVec);
				for (int i = 0; i < list.Count; i++)
				{
					if (num > 0 || CompDarkener.IsLightBlocker(list[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					GlowGrid glowGrid = map.glowGrid;
					if (glowGrid != null)
					{
						glowGrid.LightBlockerRemoved(intVec);
					}
				}
			}
			if (!dictionary.Any<KeyValuePair<IntVec3, int>>())
			{
				CompDarkener.darkCells.Remove(map);
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000192A0 File Offset: 0x000174A0
		private static bool IsLightBlocker(Thing thing)
		{
			return thing.def.blockLight && thing is Building;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x000192BC File Offset: 0x000174BC
		[HarmonyPatch(typeof(GlowGrid), "GroundGlowAt")]
		[HarmonyPrefix]
		public static void IgnoreSkyDark(IntVec3 c, ref bool ignoreSky, Map ___map)
		{
			Dictionary<IntVec3, int> dictionary;
			if (CompDarkener.darkCells.TryGetValue(___map, out dictionary) && dictionary.ContainsKey(c))
			{
				ignoreSky = true;
			}
		}

		// Token: 0x040001CA RID: 458
		private static readonly Dictionary<Map, Dictionary<IntVec3, int>> darkCells = new Dictionary<Map, Dictionary<IntVec3, int>>();
	}
}
