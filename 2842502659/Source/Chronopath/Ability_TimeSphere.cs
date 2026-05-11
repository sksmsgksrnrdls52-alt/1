using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x0200013C RID: 316
	public class Ability_TimeSphere : Ability
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x0001BFF4 File Offset: 0x0001A1F4
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				TimeSphere timeSphere = (TimeSphere)ThingMaker.MakeThing(VPE_DefOf.VPE_TimeSphere, null);
				timeSphere.Duration = this.GetDurationForPawn();
				timeSphere.Radius = this.GetRadiusForPawn();
				GenSpawn.Spawn(timeSphere, globalTargetInfo.Cell, globalTargetInfo.Map, 0);
			}
		}
	}
}
