using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200008A RID: 138
	public class FocusStrengthOffset_NearbyOfTechlevel : FocusStrengthOffset
	{
		// Token: 0x06000193 RID: 403 RVA: 0x00008B24 File Offset: 0x00006D24
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			Map mapHeld = parent.MapHeld;
			int num;
			float num2;
			if (mapHeld != null)
			{
				List<Thing> things = this.GetThings(parent.Position, mapHeld);
				num = Mathf.Clamp(things.Count, 1, 10);
				num2 = Mathf.Clamp(things.Sum((Thing t) => t.MarketValue * (float)t.stackCount), 1f, 5000f);
			}
			else
			{
				num = 1;
				num2 = parent.MarketValue * (float)parent.stackCount;
			}
			return (float)num / 5.55f * num2 / 10000f;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008BB0 File Offset: 0x00006DB0
		public override string GetExplanation(Thing parent)
		{
			Map mapHeld = parent.MapHeld;
			int num = (mapHeld != null) ? this.GetThings(parent.Position, mapHeld).Count : 1;
			return TranslatorFormattedStringExtensions.Translate("VPE.ThingsOfLevel", num, this.techLevel.ToString()) + ": " + GenText.ToStringWithSign(this.GetOffset(parent, null), "0%");
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008C2C File Offset: 0x00006E2C
		public override void PostDrawExtraSelectionOverlays(Thing parent, Pawn user = null)
		{
			base.PostDrawExtraSelectionOverlays(parent, user);
			GenDraw.DrawRadiusRing(parent.Position, this.radius, PlaceWorker_MeditationOffsetBuildingsNear.RingColor, null);
			Map mapHeld = parent.MapHeld;
			if (mapHeld != null)
			{
				foreach (Thing thing in this.GetThings(parent.Position, mapHeld))
				{
					GenDraw.DrawLineBetween(GenThing.TrueCenter(parent), GenThing.TrueCenter(thing), 2, 0.2f);
				}
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00008CC0 File Offset: 0x00006EC0
		protected virtual List<Thing> GetThings(IntVec3 cell, Map map)
		{
			return (from t in GenRadialCached.RadialDistinctThingsAround(cell, map, this.radius, true)
			where t.def.techLevel == this.techLevel
			select t).Take(10).ToList<Thing>();
		}

		// Token: 0x0400006D RID: 109
		public float radius = 10f;

		// Token: 0x0400006E RID: 110
		public TechLevel techLevel;
	}
}
