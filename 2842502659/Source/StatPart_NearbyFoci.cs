using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200008F RID: 143
	public class StatPart_NearbyFoci : StatPart
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x00009264 File Offset: 0x00007464
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.Thing == null || req.Pawn == null || !StatPart_NearbyFoci.ShouldApply || req.Thing.Map == null)
			{
				return;
			}
			try
			{
				StatPart_NearbyFoci.ShouldApply = false;
				List<ValueTuple<Thing, float>> list = StatPart_NearbyFoci.AllFociNearby(req.Thing, req.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					val += list[i].Item2;
				}
			}
			finally
			{
				StatPart_NearbyFoci.ShouldApply = true;
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000092F0 File Offset: 0x000074F0
		[return: TupleElementNames(new string[]
		{
			"thing",
			"value"
		})]
		private static List<ValueTuple<Thing, float>> AllFociNearby(Thing main, Pawn pawn)
		{
			CompMeditationFocus compMeditationFocus = ThingCompUtility.TryGetComp<CompMeditationFocus>(main);
			if (compMeditationFocus == null)
			{
				return new List<ValueTuple<Thing, float>>();
			}
			Map map = pawn.Map;
			IntVec3 position = main.Position;
			HashSet<MeditationFocusDef> hashSet = new HashSet<MeditationFocusDef>(compMeditationFocus.Props.focusTypes);
			List<ValueTuple<Thing, List<MeditationFocusDef>, float>> list = new List<ValueTuple<Thing, List<MeditationFocusDef>, float>>();
			foreach (CompMeditationFocus compMeditationFocus2 in GenRadialCached.MeditationFociAround(position, map, MeditationUtility.FocusObjectSearchRadius, true))
			{
				if (compMeditationFocus2.CanPawnUse(pawn))
				{
					float statValueForPawn = StatExtension.GetStatValueForPawn(compMeditationFocus2.parent, StatDefOf.MeditationFocusStrength, pawn, true);
					list.Add(new ValueTuple<Thing, List<MeditationFocusDef>, float>(compMeditationFocus2.parent, compMeditationFocus2.Props.focusTypes, statValueForPawn));
				}
			}
			list.Sort(([TupleElementNames(new string[]
			{
				"thing",
				"types",
				"value"
			})] ValueTuple<Thing, List<MeditationFocusDef>, float> a, [TupleElementNames(new string[]
			{
				"thing",
				"types",
				"value"
			})] ValueTuple<Thing, List<MeditationFocusDef>, float> b) => b.Item3.CompareTo(a.Item3));
			List<ValueTuple<Thing, float>> list2 = new List<ValueTuple<Thing, float>>();
			foreach (ValueTuple<Thing, List<MeditationFocusDef>, float> valueTuple in list)
			{
				Thing item = valueTuple.Item1;
				List<MeditationFocusDef> item2 = valueTuple.Item2;
				float item3 = valueTuple.Item3;
				bool flag = false;
				foreach (MeditationFocusDef item4 in item2)
				{
					if (hashSet.Add(item4))
					{
						flag = true;
					}
				}
				if (flag)
				{
					list2.Add(new ValueTuple<Thing, float>(item, item3));
				}
			}
			return list2;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009490 File Offset: 0x00007690
		public override string ExplanationPart(StatRequest req)
		{
			if (req.Thing == null || req.Pawn == null || !StatPart_NearbyFoci.ShouldApply || req.Thing.Map == null)
			{
				return "";
			}
			string result;
			try
			{
				StatPart_NearbyFoci.ShouldApply = false;
				List<string> list = (from tuple in StatPart_NearbyFoci.AllFociNearby(req.Thing, req.Pawn)
				select tuple.Item1.LabelCap + ": " + StatDefOf.MeditationFocusStrength.Worker.ValueToString(tuple.Item2, true, 3)).ToList<string>();
				result = ((list.Count > 0) ? (Translator.Translate("VPE.Nearby") + ":\n" + GenText.ToLineList(list, "  ", true)) : "");
			}
			finally
			{
				StatPart_NearbyFoci.ShouldApply = true;
			}
			return result;
		}

		// Token: 0x0400006F RID: 111
		public static bool ShouldApply = true;
	}
}
