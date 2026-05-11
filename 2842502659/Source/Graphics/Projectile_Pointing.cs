using System;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Graphics
{
	// Token: 0x020000CF RID: 207
	public class Projectile_Pointing : Projectile_Explosive
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000FE68 File Offset: 0x0000E068
		private Vector3 LookTowards
		{
			get
			{
				return new Vector3(this.destination.x - this.origin.x, this.def.Altitude, this.destination.z - this.origin.z + this.ArcHeightFactor * (4f - 8f * base.DistanceCoveredFraction));
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002BF RID: 703 RVA: 0x0000FED0 File Offset: 0x0000E0D0
		private float ArcHeightFactor
		{
			get
			{
				float num = this.def.projectile.arcHeightFactor;
				float num2 = GenGeo.MagnitudeHorizontalSquared(this.destination - this.origin);
				if (num * num > num2 * 0.2f * 0.2f)
				{
					num = Mathf.Sqrt(num2) * 0.2f;
				}
				return num;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000FF25 File Offset: 0x0000E125
		public override Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation(this.LookTowards);
			}
		}
	}
}
