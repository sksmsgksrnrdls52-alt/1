using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000090 RID: 144
	public class StatPart_NearbyWealth : StatPart_Focus
	{
		// Token: 0x060001AB RID: 427 RVA: 0x00009574 File Offset: 0x00007774
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!base.ApplyOn(req) || req.Thing.Map == null)
			{
				return;
			}
			float num = Mathf.Max(req.Thing.Map.wealthWatcher.WealthTotal, 1000f);
			if (num <= 0f)
			{
				return;
			}
			float num2 = Mathf.Min(GenRadialCached.WealthAround(req.Thing.Position, req.Thing.Map, 6f, true), num);
			val += num2 / num;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000095F8 File Offset: 0x000077F8
		public override string ExplanationPart(StatRequest req)
		{
			if (!base.ApplyOn(req) || req.Thing.Map == null)
			{
				return string.Empty;
			}
			float num = Mathf.Max(req.Thing.Map.wealthWatcher.WealthTotal, 1000f);
			float num2 = Mathf.Min(GenRadialCached.WealthAround(req.Thing.Position, req.Thing.Map, 6f, true), num);
			return TranslatorFormattedStringExtensions.Translate("VPE.WealthNearby", GenText.ToStringMoney(num2, null), GenText.ToStringMoney(num, null)) + ": " + this.parentStat.Worker.ValueToString(num2 / num, true, 3);
		}
	}
}
