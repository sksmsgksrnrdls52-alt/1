using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded.Conflagrator
{
	// Token: 0x020000C9 RID: 201
	public class FireBeam : PowerBeam
	{
		// Token: 0x0600029E RID: 670 RVA: 0x0000ED88 File Offset: 0x0000CF88
		public override void StartStrike()
		{
			base.StartStrike();
			GridsUtility.GetFirstThing<Mote>(base.Position, base.Map).Destroy(0);
			Mote mote = (Mote)ThingMaker.MakeThing(VPE_DefOf.VPE_Mote_FireBeam, null);
			mote.exactPosition = base.Position.ToVector3Shifted();
			mote.Scale = 90f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, base.Position, base.Map, 0);
		}
	}
}
