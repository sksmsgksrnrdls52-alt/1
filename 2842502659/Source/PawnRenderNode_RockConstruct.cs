using System;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000076 RID: 118
	public class PawnRenderNode_RockConstruct : PawnRenderNode_AnimalPart
	{
		// Token: 0x06000166 RID: 358 RVA: 0x0000824F File Offset: 0x0000644F
		public PawnRenderNode_RockConstruct(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000825C File Offset: 0x0000645C
		public override Graphic GraphicFor(Pawn pawn)
		{
			CompSetStoneColour compSetStoneColour;
			if (ThingCompUtility.TryGetComp<CompSetStoneColour>(pawn, ref compSetStoneColour))
			{
				Graphic graphic = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
				return GraphicDatabase.Get<Graphic_Multi>(graphic.path, ShaderDatabase.Cutout, graphic.drawSize, compSetStoneColour.color);
			}
			return base.GraphicFor(pawn);
		}
	}
}
