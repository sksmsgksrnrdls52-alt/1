using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x0200010A RID: 266
	public class Ability_Thunderbolt : Ability
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x00016784 File Offset: 0x00014984
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				foreach (Thing thing in GenList.ListFullCopy<Thing>(GridsUtility.GetThingList(globalTargetInfo.Cell, globalTargetInfo.Map)))
				{
					thing.TakeDamage(new DamageInfo(DamageDefOf.Flame, 25f, -1f, Vector3Utility.AngleToFlat(this.pawn.DrawPos, thing.DrawPos), this.pawn, null, null, 0, null, true, true, 2, true, false));
				}
				GenExplosion.DoExplosion(globalTargetInfo.Cell, globalTargetInfo.Map, this.GetRadiusForPawn(), DamageDefOf.EMP, this.pawn, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				this.pawn.Map.weatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(this.pawn.Map, globalTargetInfo.Cell));
			}
		}
	}
}
