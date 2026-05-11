using System;
using RimWorld.Planet;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000FA RID: 250
	public class Pawn_Construct : Pawn, IMinHeatGiver, ILoadReferenceable
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00015828 File Offset: 0x00013A28
		public bool IsActive
		{
			get
			{
				return base.Spawned || CaravanUtility.GetCaravan(this) != null;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000376 RID: 886 RVA: 0x0001583D File Offset: 0x00013A3D
		public int MinHeat
		{
			get
			{
				return 20;
			}
		}
	}
}
