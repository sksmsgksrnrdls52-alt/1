using System;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000108 RID: 264
	public class Vortex : ThingWithComps
	{
		// Token: 0x060003AB RID: 939 RVA: 0x00016418 File Offset: 0x00014618
		protected override void Tick()
		{
			base.Tick();
			if (this.sustainer == null)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(VPE_DefOf.VPE_Vortex_Sustainer, this);
			}
			this.sustainer.Maintain();
			for (int i = 0; i < 3; i++)
			{
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(this.RandomLocation(), base.Map, VPE_DefOf.VPE_VortexSpark, 1f);
				dataStatic.rotation = Rand.Range(0f, 360f);
				base.Map.flecks.CreateFleck(dataStatic);
				FleckMaker.ThrowSmoke(this.RandomLocation(), base.Map, 4f);
			}
			if (Find.TickManager.TicksGame - this.startTick > 2500)
			{
				this.Destroy(0);
			}
			if (Gen.IsHashIntervalTick(this, 30))
			{
				foreach (Pawn pawn in GenRadial.RadialDistinctThingsAround(base.Position, base.Map, 18.9f, true).OfType<Pawn>())
				{
					if (pawn.RaceProps.IsMechanoid)
					{
						pawn.stances.stunner.StunFor(30, this, false, true, false);
					}
					else if (pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_Vortex, false) == null)
					{
						Hediff_Vortexed hediff_Vortexed = (Hediff_Vortexed)HediffMaker.MakeHediff(VPE_DefOf.VPE_Vortex, pawn, null);
						hediff_Vortexed.Vortex = this;
						pawn.health.AddHediff(hediff_Vortexed, null, null, null);
					}
				}
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000165A8 File Offset: 0x000147A8
		private Vector3 RandomLocation()
		{
			return this.DrawPos + Vector3Utility.RotatedBy(new Vector3(Vortex.Wrap(Mathf.Abs(Rand.Gaussian(0f, 18.9f)), 18.9f), 0f, 0f), Rand.Range(0f, 360f));
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00016601 File Offset: 0x00014801
		public static float Wrap(float x, float max)
		{
			while (x > max)
			{
				x -= max;
			}
			return x;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001660F File Offset: 0x0001480F
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001662C File Offset: 0x0001482C
		public override void DeSpawn(DestroyMode mode = 0)
		{
			base.DeSpawn(mode);
			this.sustainer.End();
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00016640 File Offset: 0x00014840
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x040001B2 RID: 434
		public const float RADIUS = 18.9f;

		// Token: 0x040001B3 RID: 435
		public const int DURATION = 2500;

		// Token: 0x040001B4 RID: 436
		private int startTick;

		// Token: 0x040001B5 RID: 437
		private Sustainer sustainer;
	}
}
