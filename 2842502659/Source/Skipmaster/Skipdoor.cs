using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using VEF.Buildings;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Skipmaster
{
	// Token: 0x02000116 RID: 278
	[StaticConstructorOnStartup]
	public class Skipdoor : DoorTeleporter, IMinHeatGiver, ILoadReferenceable
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x0001880C File Offset: 0x00016A0C
		public bool IsActive
		{
			get
			{
				return base.Spawned;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00018814 File Offset: 0x00016A14
		public int MinHeat
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00018818 File Offset: 0x00016A18
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.Pawn.Psycasts().AddMinHeatGiver(this);
			if (respawningAfterLoad)
			{
				return;
			}
			this.Pawn.psychicEntropy.TryAddEntropy(50f, this, true, true);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00018850 File Offset: 0x00016A50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.Pawn, "pawn", false);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00018869 File Offset: 0x00016A69
		protected override void Tick()
		{
			base.Tick();
			if (Gen.IsHashIntervalTick(this, 30) && this.HitPoints < base.MaxHitPoints)
			{
				this.HitPoints++;
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00018898 File Offset: 0x00016A98
		public override void DoTeleportEffects(Thing thing, int ticksLeftThisToil, Map targetMap, ref IntVec3 targetCell, DoorTeleporter dest)
		{
			if (ticksLeftThisToil == 5)
			{
				FleckMaker.Static(thing.Position, thing.Map, FleckDefOf.PsycastSkipFlashEntry, 1f);
				FleckMaker.Static(targetCell, targetMap, FleckDefOf.PsycastSkipInnerExit, 1f);
				FleckMaker.Static(targetCell, targetMap, FleckDefOf.PsycastSkipOuterRingExit, 1f);
				SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Entry, this);
				SoundStarter.PlayOneShot(SoundDefOf.Psycast_Skip_Exit, dest);
			}
			else if (ticksLeftThisToil == 15)
			{
				targetCell = GenCollection.RandomElement<IntVec3>(from c in GenAdj.CellsAdjacentCardinal(dest)
				where GenGrid.Standable(c, targetMap)
				select c);
				this.teleportEffecters[thing] = EffecterDefOf.Skip_Exit.Spawn(targetCell, targetMap, 1f);
				this.teleportEffecters[thing].ticksLeft = 15;
			}
			if (this.teleportEffecters.ContainsKey(thing))
			{
				this.teleportEffecters[thing].EffectTick(new TargetInfo(targetCell, targetMap, false), new TargetInfo(targetCell, targetMap, false));
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x000189D5 File Offset: 0x00016BD5
		public override IEnumerable<Gizmo> GetDoorTeleporterGismoz()
		{
			Skipdoor.<GetDoorTeleporterGismoz>d__9 <GetDoorTeleporterGismoz>d__ = new Skipdoor.<GetDoorTeleporterGismoz>d__9(-2);
			<GetDoorTeleporterGismoz>d__.<>4__this = this;
			return <GetDoorTeleporterGismoz>d__;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x000189E8 File Offset: 0x00016BE8
		protected override void PlaySustainer(SoundDef sustainer)
		{
			if (PsycastsMod.Settings.muteSkipdoor)
			{
				Sustainer sustainer2 = this.sustainer;
				if (sustainer2 != null)
				{
					sustainer2.End();
				}
				this.sustainer = null;
				return;
			}
			if (this.sustainer == null)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(sustainer, this);
			}
			this.sustainer.Maintain();
		}

		// Token: 0x040001C6 RID: 454
		public Pawn Pawn;
	}
}
