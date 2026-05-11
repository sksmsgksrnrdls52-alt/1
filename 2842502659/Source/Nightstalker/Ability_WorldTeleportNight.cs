using System;
using RimWorld;
using RimWorld.Planet;
using VanillaPsycastsExpanded.Skipmaster;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x0200011B RID: 283
	public class Ability_WorldTeleportNight : Ability_WorldTeleport
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x00019084 File Offset: 0x00017284
		public override bool CanHitTargetTile(GlobalTargetInfo target)
		{
			float num = GenLocalDate.HourFloat(target.Tile);
			return num < 6f || num > 18f;
		}
	}
}
