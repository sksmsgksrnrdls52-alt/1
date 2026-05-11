using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000027 RID: 39
	public class GenStep_EltexMeteor : GenStep_ScatterLumpsMineable
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003A5F File Offset: 0x00001C5F
		public override int SeedPart
		{
			get
			{
				return 1634184421;
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003A66 File Offset: 0x00001C66
		public override void Generate(Map map, GenStepParams parms)
		{
			this.forcedDefToScatter = VPE_DefOf.VPE_EltexOre;
			this.count = 1;
			this.forcedLumpSize = 9;
			base.Generate(map, parms);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003A8C File Offset: 0x00001C8C
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			List<CellRect> list;
			return (!MapGenerator.TryGetVar<List<CellRect>>("UsedRects", ref list) || !GenCollection.Any<CellRect>(list, (CellRect x) => x.Contains(c))) && map.reachability.CanReachMapEdge(c, TraverseParms.For(1, 3, false, false, false, true, false));
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003AE8 File Offset: 0x00001CE8
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			base.ScatterAt(c, map, parms, stackCount);
			int num = this.recentLumpCells.Min((IntVec3 x) => x.x);
			int num2 = this.recentLumpCells.Min((IntVec3 x) => x.z);
			int num3 = this.recentLumpCells.Max((IntVec3 x) => x.x);
			int num4 = this.recentLumpCells.Max((IntVec3 x) => x.z);
			CellRect cellRect = CellRect.FromLimits(num, num2, num3, num4);
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
		}
	}
}
