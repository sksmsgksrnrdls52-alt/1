using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000106 RID: 262
	[StaticConstructorOnStartup]
	public static class TexHurricane
	{
		// Token: 0x040001B1 RID: 433
		public static readonly Material HurricaneOverlay = MaterialPool.MatFrom("Effects/Staticlord/Hurricane/VPEHurricaneWorldOverlay", ShaderDatabase.WorldOverlayTransparent);
	}
}
