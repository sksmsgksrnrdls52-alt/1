using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000037 RID: 55
	public class Hediff_Deathshield : Hediff_Overlay
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004770 File Offset: 0x00002970
		public override float OverlaySize
		{
			get
			{
				return 1.5f;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00004777 File Offset: 0x00002977
		public override string OverlayPath
		{
			get
			{
				return "Effects/Necropath/Deathshield/Deathshield";
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004780 File Offset: 0x00002980
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (ModCompatibility.AlienRacesIsActive)
			{
				this.skinColor = new Color?(ModCompatibility.GetSkinColorFirst(this.pawn));
				ModCompatibility.SetSkinColorFirst(this.pawn, Hediff_Deathshield.RottenColor);
			}
			else
			{
				this.skinColor = this.pawn.story.skinColorOverride;
				this.pawn.story.skinColorOverride = new Color?(Hediff_Deathshield.RottenColor);
			}
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004808 File Offset: 0x00002A08
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (ModCompatibility.AlienRacesIsActive)
			{
				ModCompatibility.SetSkinColorFirst(this.pawn, this.skinColor.Value);
			}
			else
			{
				this.pawn.story.skinColorOverride = this.skinColor;
			}
			this.pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004865 File Offset: 0x00002A65
		public override void Tick()
		{
			base.Tick();
			this.curAngle += 0.07f;
			if (this.curAngle > 360f)
			{
				this.curAngle = 0f;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004898 File Offset: 0x00002A98
		public override void Draw()
		{
			if (this.pawn.Spawned)
			{
				Vector3 drawPos = this.pawn.DrawPos;
				drawPos.y = Altitudes.AltitudeFor(28);
				Matrix4x4 matrix4x = default(Matrix4x4);
				matrix4x.SetTRS(drawPos, Quaternion.AngleAxis(this.curAngle, Vector3.up), new Vector3(this.OverlaySize, 1f, this.OverlaySize));
				Graphics.DrawMesh(MeshPool.plane10, matrix4x, base.OverlayMat, 0, null, 0, this.MatPropertyBlock);
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000491C File Offset: 0x00002B1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.curAngle, "curAngle", 0f, false);
		}

		// Token: 0x04000025 RID: 37
		private static readonly Color RottenColor = new Color(0.29f, 0.25f, 0.22f);

		// Token: 0x04000026 RID: 38
		public float curAngle;

		// Token: 0x04000027 RID: 39
		public Color? skinColor;
	}
}
