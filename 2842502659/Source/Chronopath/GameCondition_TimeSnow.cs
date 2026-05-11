using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000137 RID: 311
	[StaticConstructorOnStartup]
	public class GameCondition_TimeSnow : GameCondition
	{
		// Token: 0x06000477 RID: 1143 RVA: 0x0001B594 File Offset: 0x00019794
		public override void PostMake()
		{
			base.PostMake();
			this.worldOverlayMat = GameCondition_TimeSnow.TimeSnowOverlay;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0001B5A8 File Offset: 0x000197A8
		public override void GameConditionDraw(Map map)
		{
			base.GameConditionDraw(map);
			if (this.worldOverlayMat != null)
			{
				Graphics.DrawMesh(MeshPool.wholeMapPlane, map.Center.ToVector3ShiftedWithAltitude(31), Quaternion.identity, this.worldOverlayMat, 0);
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001B5F0 File Offset: 0x000197F0
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			if (this.worldOverlayMat != null)
			{
				this.worldOverlayMat.SetTextureOffset("_MainTex", (float)(Find.TickManager.TicksGame % 3600000) * new Vector2(0.0005f, -0.002f) * this.worldOverlayMat.GetTextureScale("_MainTex").x);
				if (this.worldOverlayMat.HasProperty("_MainTex2"))
				{
					this.worldOverlayMat.SetTextureOffset("_MainTex2", (float)(Find.TickManager.TicksGame % 3600000) * new Vector2(0.0004f, -0.002f) * this.worldOverlayMat.GetTextureScale("_MainTex").x);
				}
			}
		}

		// Token: 0x040001E0 RID: 480
		public static readonly Material TimeSnowOverlay = MaterialPool.MatFrom("Effects/Chronopath/Timesnow/TimesnowWorldOverlay", ShaderDatabase.WorldOverlayTransparent);

		// Token: 0x040001E1 RID: 481
		private Material worldOverlayMat;
	}
}
