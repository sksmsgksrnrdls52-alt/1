using System;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000BC RID: 188
	public class JumpingPawn : AbilityPawnFlyer
	{
		// Token: 0x0600026B RID: 619 RVA: 0x0000DC22 File Offset: 0x0000BE22
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.FlyingPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc, new Rot4?(base.Rotation), true);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000DC48 File Offset: 0x0000BE48
		protected override void Tick()
		{
			base.Tick();
			if (base.Map != null && Find.TickManager.TicksGame % 3 == 0)
			{
				Map map = base.Map;
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(this.GetDrawPos(), map, VPE_DefOf.VPE_WarlordZap, 1f);
				dataStatic.rotation = Rand.Range(0f, 360f);
				map.flecks.CreateFleck(dataStatic);
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
		private Vector3 GetDrawPos()
		{
			float num = (float)this.ticksFlying / (float)this.ticksFlightTime;
			Vector3 drawPos = this.DrawPos;
			drawPos.y = Altitudes.AltitudeFor(30);
			return drawPos + Vector3.forward * (num - Mathf.Pow(num, 2f)) * 15f;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000DD10 File Offset: 0x0000BF10
		protected override void RespawnPawn()
		{
			Pawn flyingPawn = base.FlyingPawn;
			base.RespawnPawn();
			SoundStarter.PlayOneShot(VPE_DefOf.VPE_PowerLeap_Land, flyingPawn);
			FleckMaker.ThrowSmoke(flyingPawn.DrawPos, flyingPawn.Map, 1f);
			FleckMaker.ThrowDustPuffThick(flyingPawn.DrawPos, flyingPawn.Map, 2f, new Color(1f, 1f, 1f, 2.5f));
		}
	}
}
