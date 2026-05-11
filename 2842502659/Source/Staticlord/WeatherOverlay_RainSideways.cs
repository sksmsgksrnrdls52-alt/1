using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000105 RID: 261
	public class WeatherOverlay_RainSideways : WeatherOverlayDualPanner
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x0001634B File Offset: 0x0001454B
		public WeatherOverlay_RainSideways()
		{
			LongEventHandler.ExecuteWhenFinished(delegate()
			{
				this.worldOverlayMat = TexHurricane.HurricaneOverlay;
				this.worldOverlayPanSpeed1 = 0.015f;
				this.worldPanDir1 = new Vector2(-1f, -0.25f);
				this.worldPanDir1.Normalize();
				this.worldOverlayPanSpeed2 = 0.022f;
				this.worldPanDir2 = new Vector2(-1f, -0.22f);
				this.worldPanDir2.Normalize();
			});
		}
	}
}
