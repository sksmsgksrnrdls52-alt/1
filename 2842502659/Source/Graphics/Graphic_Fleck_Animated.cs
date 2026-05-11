using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Graphics
{
	// Token: 0x020000D3 RID: 211
	public class Graphic_Fleck_Animated : Graphic_FleckCollection
	{
		// Token: 0x060002CA RID: 714 RVA: 0x000101D8 File Offset: 0x0000E3D8
		public override void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			GraphicData_Animated graphicData_Animated = (GraphicData_Animated)this.data;
			Game game = Current.Game;
			int? num;
			if (game == null)
			{
				num = null;
			}
			else
			{
				TickManager tickManager = game.tickManager;
				num = ((tickManager != null) ? new int?(tickManager.TicksGame) : null);
			}
			int? num2 = num;
			float num3 = (num2 != null) ? ((float)num2.GetValueOrDefault()) : 0f;
			int num4;
			if (graphicData_Animated.random)
			{
				num4 = Mathf.FloorToInt(num3 / (float)graphicData_Animated.ticksPerFrame) % this.subGraphics.Length;
			}
			else
			{
				num4 = Mathf.FloorToInt(drawData.ageSecs * 60f / (float)graphicData_Animated.ticksPerFrame) % this.subGraphics.Length;
			}
			Graphic_Fleck[] subGraphics = this.subGraphics;
			if (subGraphics == null)
			{
				return;
			}
			subGraphics[num4].DrawFleck(drawData, batch);
		}
	}
}
