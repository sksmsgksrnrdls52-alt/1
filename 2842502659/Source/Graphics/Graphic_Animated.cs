using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Graphics
{
	// Token: 0x020000D0 RID: 208
	public class Graphic_Animated : Graphic_Collection
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000FF3A File Offset: 0x0000E13A
		public override Material MatSingle
		{
			get
			{
				Graphic curFrame = this.CurFrame;
				if (curFrame == null)
				{
					return null;
				}
				return curFrame.MatSingle;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000FF50 File Offset: 0x0000E150
		private Graphic CurFrame
		{
			get
			{
				Graphic[] subGraphics = this.subGraphics;
				if (subGraphics == null)
				{
					return null;
				}
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
				return subGraphics[Mathf.FloorToInt((((num2 != null) ? ((float)num2.GetValueOrDefault()) : 0f) + (float)this.offset) / (float)((GraphicData_Animated)this.data).ticksPerFrame) % this.subGraphics.Length];
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000FFDD File Offset: 0x0000E1DD
		public int SubGraphicCount
		{
			get
			{
				return this.subGraphics.Length - 1;
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000FFEC File Offset: 0x0000E1EC
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			IAnimationOneTime animationOneTime = thing as IAnimationOneTime;
			if (animationOneTime != null)
			{
				int num = animationOneTime.CurrentIndex();
				Graphic[] subGraphics = this.subGraphics;
				if (subGraphics == null)
				{
					return;
				}
				Graphic graphic = subGraphics[num];
				if (graphic == null)
				{
					return;
				}
				graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
				return;
			}
			else
			{
				Graphic curFrame = this.CurFrame;
				if (curFrame == null)
				{
					return;
				}
				curFrame.DrawWorker(loc, rot, thingDef, thing, extraRotation);
				return;
			}
		}

		// Token: 0x04000159 RID: 345
		private readonly int offset = Rand.Range(1, 1000);
	}
}
