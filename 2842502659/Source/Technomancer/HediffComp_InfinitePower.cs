using System;
using RimWorld;
using UnityEngine;
using VEF.Hediffs;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000FB RID: 251
	[StaticConstructorOnStartup]
	public class HediffComp_InfinitePower : HediffComp_Draw
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0001584C File Offset: 0x00013A4C
		public override bool CompShouldRemove
		{
			get
			{
				bool flag = base.CompShouldRemove;
				if (!flag)
				{
					Thing thing = this.target;
					bool flag2 = thing == null || !thing.Spawned;
					flag = flag2;
				}
				return flag;
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00015880 File Offset: 0x00013A80
		public void Begin(Thing t)
		{
			this.target = t;
			this.compPower = ThingCompUtility.TryGetComp<CompPowerTrader>(t);
			Pawn pawn = t as Pawn;
			Need_MechEnergy need_MechEnergy;
			if (pawn == null)
			{
				need_MechEnergy = null;
			}
			else
			{
				Pawn_NeedsTracker needs = pawn.needs;
				need_MechEnergy = ((needs != null) ? needs.energy : null);
			}
			this.needPower = need_MechEnergy;
			Need_MechEnergy need_MechEnergy2 = this.needPower;
			if (need_MechEnergy2 != null && need_MechEnergy2.currentCharger == null)
			{
				Need_MechEnergy need_MechEnergy3 = this.needPower;
				Building_MechCharger currentCharger;
				if ((currentCharger = this.fakeCharger) == null)
				{
					currentCharger = (this.fakeCharger = new Building_MechCharger());
				}
				need_MechEnergy3.currentCharger = currentCharger;
			}
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000158FC File Offset: 0x00013AFC
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.compPower != null)
			{
				this.compPower.PowerOn = true;
				this.compPower.PowerOutput = 0f;
			}
			Need_MechEnergy need_MechEnergy = this.needPower;
			if (need_MechEnergy != null && need_MechEnergy.currentCharger == null)
			{
				Need_MechEnergy need_MechEnergy2 = this.needPower;
				Building_MechCharger currentCharger;
				if ((currentCharger = this.fakeCharger) == null)
				{
					currentCharger = (this.fakeCharger = new Building_MechCharger());
				}
				need_MechEnergy2.currentCharger = currentCharger;
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001596C File Offset: 0x00013B6C
		public override void DrawAt(Vector3 drawPos)
		{
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(Vector3Utility.Yto0(this.target.DrawPos) + Vector3.up * Altitudes.AltitudeFor(39), Quaternion.AngleAxis(0f, Vector3.up), Vector3.one), HediffComp_InfinitePower.OVERLAY, 0);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000159C8 File Offset: 0x00013BC8
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			CompPowerTrader compPowerTrader = this.compPower;
			if (compPowerTrader != null)
			{
				compPowerTrader.SetUpPowerVars();
			}
			Need_MechEnergy need_MechEnergy = this.needPower;
			if (need_MechEnergy != null)
			{
				Building_MechCharger currentCharger = need_MechEnergy.currentCharger;
				if (currentCharger == this.fakeCharger)
				{
					this.needPower.currentCharger = null;
				}
			}
			this.fakeCharger = null;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00015A1C File Offset: 0x00013C1C
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_References.Look<Building_MechCharger>(ref this.fakeCharger, "fakeCharger", false);
			if (Scribe.mode == 4)
			{
				this.compPower = ThingCompUtility.TryGetComp<CompPowerTrader>(this.target);
				Pawn pawn = this.target as Pawn;
				Need_MechEnergy need_MechEnergy;
				if (pawn == null)
				{
					need_MechEnergy = null;
				}
				else
				{
					Pawn_NeedsTracker needs = pawn.needs;
					need_MechEnergy = ((needs != null) ? needs.energy : null);
				}
				this.needPower = need_MechEnergy;
				Need_MechEnergy need_MechEnergy2 = this.needPower;
				if (need_MechEnergy2 != null && need_MechEnergy2.currentCharger == null)
				{
					Need_MechEnergy need_MechEnergy3 = this.needPower;
					Building_MechCharger currentCharger;
					if ((currentCharger = this.fakeCharger) == null)
					{
						currentCharger = (this.fakeCharger = new Building_MechCharger());
					}
					need_MechEnergy3.currentCharger = currentCharger;
				}
			}
		}

		// Token: 0x040001A6 RID: 422
		private static readonly Material OVERLAY = MaterialPool.MatFrom("Effects/Technomancer/Power/InfinitePowerOverlay", ShaderDatabase.MetaOverlay);

		// Token: 0x040001A7 RID: 423
		private CompPowerTrader compPower;

		// Token: 0x040001A8 RID: 424
		private Building_MechCharger fakeCharger;

		// Token: 0x040001A9 RID: 425
		private Need_MechEnergy needPower;

		// Token: 0x040001AA RID: 426
		private Thing target;
	}
}
