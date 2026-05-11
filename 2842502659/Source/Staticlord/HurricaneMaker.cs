using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000101 RID: 257
	public class HurricaneMaker : Thing
	{
		// Token: 0x06000392 RID: 914 RVA: 0x000160D8 File Offset: 0x000142D8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			this.caused = GameConditionMaker.MakeConditionPermanent(VPE_DefOf.VPE_Hurricane_Condition);
			this.caused.conditionCauser = this;
			map.GameConditionManager.RegisterCondition(this.caused);
			base.Map.weatherManager.TransitionTo(VPE_DefOf.VPE_Hurricane_Weather);
			base.Map.weatherDecider.StartNextWeather();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00016143 File Offset: 0x00014343
		public override void Destroy(DestroyMode mode = 0)
		{
			this.caused.End();
			base.Map.weatherDecider.StartNextWeather();
			base.Destroy(mode);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00016167 File Offset: 0x00014367
		protected override void Tick()
		{
			if (!this.Pawn.psychicEntropy.TryAddEntropy(1f, this, true, false) || this.Pawn.Downed)
			{
				this.Destroy(0);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00016197 File Offset: 0x00014397
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<GameCondition>(ref this.caused, "caused", false);
			Scribe_References.Look<Pawn>(ref this.Pawn, "pawn", false);
			if (Scribe.mode == 4)
			{
				this.caused.conditionCauser = this;
			}
		}

		// Token: 0x040001AE RID: 430
		private GameCondition caused;

		// Token: 0x040001AF RID: 431
		public Pawn Pawn;
	}
}
