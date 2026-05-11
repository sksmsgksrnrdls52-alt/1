using System;
using RimWorld;
using UnityEngine;
using VEF.Hediffs;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x0200010C RID: 268
	[StaticConstructorOnStartup]
	public class HediffComp_Recharge : HediffComp_Draw
	{
		// Token: 0x060003BF RID: 959 RVA: 0x00016D98 File Offset: 0x00014F98
		public void Init(Thing t)
		{
			this.target = t;
			this.compPower = ThingCompUtility.TryGetComp<CompPowerBattery>(t);
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

		// Token: 0x060003C0 RID: 960 RVA: 0x00016E14 File Offset: 0x00015014
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.sustainer == null)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(VPE_DefOf.VPE_Recharge_Sustainer, base.Pawn);
			}
			Sustainer sustainer = this.sustainer;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			CompPowerBattery compPowerBattery = this.compPower;
			if (compPowerBattery != null)
			{
				compPowerBattery.AddEnergy(3.3333333f);
			}
			if (this.needPower != null)
			{
				this.needPower.CurLevel += 0.00083333335f;
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

		// Token: 0x060003C1 RID: 961 RVA: 0x00016EC8 File Offset: 0x000150C8
		public override void CompPostPostRemoved()
		{
			this.sustainer.End();
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
			base.CompPostPostRemoved();
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00016F14 File Offset: 0x00015114
		public override void DrawAt(Vector3 drawPos)
		{
			Vector3 vector = GenThing.TrueCenter(this.target);
			Vector3 vector2;
			vector2..ctor(this.Graphic.drawSize.x, 1f, (vector - drawPos).magnitude);
			Matrix4x4 matrix4x = Matrix4x4.TRS(drawPos + (vector - drawPos) / 2f, Quaternion.LookRotation(vector - drawPos), vector2);
			Graphics.DrawMesh(MeshPool.plane10, matrix4x, this.Graphic.MatSingle, 0);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00016F9C File Offset: 0x0001519C
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			if (Scribe.mode == 4)
			{
				this.compPower = ThingCompUtility.TryGetComp<CompPowerBattery>(this.target);
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

		// Token: 0x040001BA RID: 442
		private const float ChargePerTickMech = 0.00083333335f;

		// Token: 0x040001BB RID: 443
		private const float ChargePerTickBattery = 3.3333333f;

		// Token: 0x040001BC RID: 444
		private CompPowerBattery compPower;

		// Token: 0x040001BD RID: 445
		private Building_MechCharger fakeCharger;

		// Token: 0x040001BE RID: 446
		private Need_MechEnergy needPower;

		// Token: 0x040001BF RID: 447
		private Sustainer sustainer;

		// Token: 0x040001C0 RID: 448
		private Thing target;
	}
}
