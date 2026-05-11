using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Conflagrator
{
	// Token: 0x020000C8 RID: 200
	[StaticConstructorOnStartup]
	public class Ability_FireTornado : Ability
	{
		// Token: 0x0600029B RID: 667 RVA: 0x0000ED18 File Offset: 0x0000CF18
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				((FireTornado)GenSpawn.Spawn(Ability_FireTornado.FireTornadoDef, globalTargetInfo.Cell, globalTargetInfo.Map, 0)).ticksLeftToDisappear = this.GetDurationForPawn();
			}
		}

		// Token: 0x04000148 RID: 328
		private static readonly ThingDef FireTornadoDef = ThingDef.Named("VPE_FireTornado");
	}
}
