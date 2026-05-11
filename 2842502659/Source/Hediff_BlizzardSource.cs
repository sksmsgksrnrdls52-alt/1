using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200006A RID: 106
	[StaticConstructorOnStartup]
	public class Hediff_BlizzardSource : Hediff_Overlay
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000135 RID: 309 RVA: 0x000073FA File Offset: 0x000055FA
		public override string OverlayPath
		{
			get
			{
				return "Effects/Frostshaper/Blizzard/Blizzard";
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007401 File Offset: 0x00005601
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.pawn.Map.GetComponent<MapComponent_PsycastsManager>().blizzardSources.Add(this);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007428 File Offset: 0x00005628
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.pawn.Map.GetComponent<MapComponent_PsycastsManager>().blizzardSources.Remove(this);
			Ability ability = this.ability;
			if (ability == null)
			{
				return;
			}
			AbilityExtension_PsychicComa modExtension = ability.def.GetModExtension<AbilityExtension_PsychicComa>();
			if (modExtension == null)
			{
				return;
			}
			modExtension.ApplyComa(this.ability);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000747C File Offset: 0x0000567C
		public override void Tick()
		{
			base.Tick();
			Find.CameraDriver.shaker.DoShake(2f);
			this.curAngle += 0.07f;
			if (this.curAngle > 360f)
			{
				this.curAngle = 0f;
			}
			if (this.affectedFactions == null)
			{
				this.affectedFactions = new List<Faction>();
			}
			foreach (IntVec3 intVec in GenCollection.InRandomOrder<IntVec3>(from x in GenRadial.RadialCellsAround(this.pawn.Position, this.ability.GetAdditionalRadius(), this.ability.GetRadiusForPawn())
			where GenGrid.InBounds(x, this.pawn.Map)
			select x, null).Take(Rand.RangeInclusive(9, 12)).ToList<IntVec3>())
			{
				this.pawn.Map.snowGrid.AddDepth(intVec, 0.5f);
			}
			foreach (Pawn pawn in this.ability.pawn.Map.mapPawns.AllPawnsSpawned.ToList<Pawn>())
			{
				if (this.InAffectedArea(pawn.Position))
				{
					Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_Blizzard, false);
					if (hediff != null)
					{
						HediffUtility.TryGetComp<HediffComp_Disappears>(hediff).ticksToDisappear = 60;
					}
					else
					{
						hediff = HediffMaker.MakeHediff(VPE_DefOf.VPE_Blizzard, pawn, null);
						pawn.health.AddHediff(hediff, null, null, null);
					}
					HediffDef hediffDef;
					if (Gen.IsHashIntervalTick(pawn, 60) && pawn.CanReceiveHypothermia(out hediffDef))
					{
						HealthUtility.AdjustSeverity(pawn, hediffDef, 0.02f);
						DamageInfo damageInfo;
						damageInfo..ctor(DamageDefOf.Cut, (float)Rand.RangeInclusive(1, 3), 0f, -1f, null, null, null, 0, null, true, true, 2, true, false);
						pawn.TakeDamage(damageInfo);
					}
					if (this.ability.pawn.Faction == Faction.OfPlayer)
					{
						this.AffectGoodwill(pawn.HomeFaction, pawn);
					}
				}
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000076B8 File Offset: 0x000058B8
		public bool InAffectedArea(IntVec3 cell)
		{
			return !cell.InHorDistOf(this.ability.pawn.Position, this.ability.GetAdditionalRadius()) && cell.InHorDistOf(this.ability.pawn.Position, this.ability.GetRadiusForPawn());
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007710 File Offset: 0x00005910
		private void AffectGoodwill(Faction faction, Pawn p)
		{
			if (faction != null && !faction.IsPlayer && !FactionUtility.HostileTo(faction, Faction.OfPlayer) && (p == null || !p.IsSlaveOfColony) && !this.affectedFactions.Contains(faction))
			{
				Faction.OfPlayer.TryAffectGoodwillWith(faction, this.ability.def.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000777C File Offset: 0x0000597C
		public override void Draw()
		{
			Vector3 drawPos = this.pawn.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(28);
			Matrix4x4 matrix4x = default(Matrix4x4);
			float num = this.ability.GetRadiusForPawn() * 2f;
			matrix4x.SetTRS(drawPos, Quaternion.AngleAxis(this.curAngle, Vector3.up), new Vector3(num, 1f, num));
			Graphics.DrawMesh(MeshPool.plane10, matrix4x, base.OverlayMat, 0, null, 0, this.MatPropertyBlock);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000077FB File Offset: 0x000059FB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.curAngle, "curAngle", 0f, false);
		}

		// Token: 0x04000052 RID: 82
		private List<Faction> affectedFactions;

		// Token: 0x04000053 RID: 83
		private float curAngle;
	}
}
