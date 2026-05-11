using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200008E RID: 142
	public class StatPart_Group : StatPart_Focus
	{
		// Token: 0x060001A2 RID: 418 RVA: 0x000090D0 File Offset: 0x000072D0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!base.ApplyOn(req) || req.Thing.Map == null || req.Thing.Faction == null)
			{
				return;
			}
			float num = val;
			int num2 = StatPart_Group.MeditatingPawnsAround(req.Thing);
			float num3;
			if (num2 > 1)
			{
				switch (num2)
				{
				case 2:
					num3 = 0.06f;
					break;
				case 3:
					num3 = 0.2f;
					break;
				case 4:
					num3 = 0.45f;
					break;
				default:
					num3 = 0.8f;
					break;
				}
			}
			else
			{
				num3 = 0f;
			}
			val = num + num3;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00009158 File Offset: 0x00007358
		private static int MeditatingPawnsAround(Thing thing)
		{
			return GenCollection.Count<Pawn>(thing.Map.mapPawns.AllHumanlikeSpawned, (Pawn p) => p.CurJobDef == JobDefOf.Meditate && p.Position.InHorDistOf(thing.Position, 5f));
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00009198 File Offset: 0x00007398
		public override string ExplanationPart(StatRequest req)
		{
			if (!base.ApplyOn(req) || req.Thing.Map == null || req.Thing.Faction == null)
			{
				return "";
			}
			int num = StatPart_Group.MeditatingPawnsAround(req.Thing);
			TaggedString taggedString = TranslatorFormattedStringExtensions.Translate("VPE.GroupFocus", num - 1) + ": ";
			StatWorker worker = this.parentStat.Worker;
			float num2;
			if (num > 1)
			{
				switch (num)
				{
				case 2:
					num2 = 0.06f;
					break;
				case 3:
					num2 = 0.2f;
					break;
				case 4:
					num2 = 0.45f;
					break;
				default:
					num2 = 0.8f;
					break;
				}
			}
			else
			{
				num2 = 0f;
			}
			return taggedString + worker.ValueToString(num2, true, 3);
		}
	}
}
