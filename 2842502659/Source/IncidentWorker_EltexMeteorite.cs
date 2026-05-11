using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000055 RID: 85
	public class IncidentWorker_EltexMeteorite : IncidentWorker
	{
		// Token: 0x060000ED RID: 237 RVA: 0x00005CEC File Offset: 0x00003EEC
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindCell(out intVec, map);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005D10 File Offset: 0x00003F10
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!this.TryFindCell(out intVec, map))
			{
				return false;
			}
			List<Thing> list = new List<Thing>();
			for (int i = 0; i < 5; i++)
			{
				Building building = (Building)ThingMaker.MakeThing(VPE_DefOf.VPE_EltexOre, null);
				building.canChangeTerrainOnDestroyed = false;
				list.Add(building);
			}
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.MeteoriteIncoming, list, intVec, map);
			LetterDef letterDef = list[0].def.building.isResourceRock ? LetterDefOf.PositiveEvent : LetterDefOf.NeutralEvent;
			string text = GenText.CapitalizeFirst(string.Format(this.def.letterText, list[0].def.label));
			base.SendStandardLetter(this.def.letterLabel, text, letterDef, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005DFC File Offset: 0x00003FFC
		private bool TryFindCell(out IntVec3 cell, Map map)
		{
			int maxMineables = 5;
			return CellFinderLoose.TryFindSkyfallerCell(ThingDefOf.MeteoriteIncoming, map, TerrainAffordanceDefOf.Walkable, ref cell, 10, default(IntVec3), -1, true, false, false, false, true, true, delegate(IntVec3 x)
			{
				int num = Mathf.CeilToInt(Mathf.Sqrt((float)maxMineables)) + 2;
				CellRect cellRect = CellRect.CenteredOn(x, num, num);
				int num2 = 0;
				foreach (IntVec3 intVec in cellRect)
				{
					if (GenGrid.InBounds(intVec, map) && GenGrid.Standable(intVec, map))
					{
						num2++;
					}
				}
				return num2 >= maxMineables;
			});
		}
	}
}
