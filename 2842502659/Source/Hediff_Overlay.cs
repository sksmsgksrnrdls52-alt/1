using System;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000054 RID: 84
	[StaticConstructorOnStartup]
	public abstract class Hediff_Overlay : Hediff_Ability
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005C70 File Offset: 0x00003E70
		public Material OverlayMat
		{
			get
			{
				if (this.material == null)
				{
					this.material = MaterialPool.MatFrom(this.OverlayPath, ShaderDatabase.MoteGlow);
				}
				return this.material;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005C9C File Offset: 0x00003E9C
		public virtual float OverlaySize
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005CA3 File Offset: 0x00003EA3
		public virtual string OverlayPath { get; }

		// Token: 0x060000EA RID: 234 RVA: 0x00005CAB File Offset: 0x00003EAB
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			Map mapHeld = this.pawn.MapHeld;
			if (mapHeld == null)
			{
				return;
			}
			mapHeld.GetComponent<MapComponent_PsycastsManager>().hediffsToDraw.Add(this);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005CD4 File Offset: 0x00003ED4
		public virtual void Draw()
		{
		}

		// Token: 0x04000040 RID: 64
		public MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04000041 RID: 65
		private Material material;
	}
}
