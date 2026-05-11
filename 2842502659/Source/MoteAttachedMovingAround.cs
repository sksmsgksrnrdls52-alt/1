using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200001A RID: 26
	public class MoteAttachedMovingAround : MoteAttached
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00002CA4 File Offset: 0x00000EA4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.curPosition = new Vector3(Rand.Range(-0.5f, 0.5f), 0f, Rand.Range(-0.5f, 0.5f));
				this.exactPosition = this.GetRootPosition() + this.curPosition;
				this.exactPosition.y = this.link1.Target.CenterVector3.y + 1f;
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002D2C File Offset: 0x00000F2C
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			this.curPosition = this.GetNewMoveVector();
			Vector3 rootPosition = this.GetRootPosition();
			this.exactPosition = rootPosition + this.curPosition;
			this.exactPosition.y = this.link1.Target.CenterVector3.y + 1f;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002D90 File Offset: 0x00000F90
		public Vector3 GetNewMoveVector()
		{
			Vector2 vector = new Vector2(this.curPosition.x, this.curPosition.z);
			this.direction += Rand.Range(-22.5f, 22.5f);
			if (this.direction < -360f)
			{
				this.direction = Mathf.Abs(this.direction - -360f);
			}
			if (this.direction > 360f)
			{
				this.direction -= 360f;
			}
			Vector2 vector2 = Vector2Utility.Moved(vector, this.direction, 0.01f);
			return Vector3.ClampMagnitude(new Vector3(vector2.x, 0f, vector2.y), 0.5f);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002E48 File Offset: 0x00001048
		public Vector3 GetRootPosition()
		{
			Vector3 vector = this.def.mote.attachedDrawOffset;
			if (this.def.mote.attachedToHead)
			{
				Pawn pawn = this.link1.Target.Thing as Pawn;
				if (pawn != null && pawn.story != null)
				{
					vector = Vector3Utility.RotatedBy(pawn.Drawer.renderer.BaseHeadOffsetAt((PawnUtility.GetPosture(pawn) == null) ? Rot4.North : pawn.Drawer.renderer.LayingFacing()), pawn.Drawer.renderer.BodyAngle(0));
				}
			}
			return this.link1.LastDrawPos + vector;
		}

		// Token: 0x04000010 RID: 16
		private Vector3 curPosition;

		// Token: 0x04000011 RID: 17
		private float direction;
	}
}
