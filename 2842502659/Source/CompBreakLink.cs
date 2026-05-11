using System;
using System.Collections.Generic;
using VEF.AnimalBehaviours;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000079 RID: 121
	public class CompBreakLink : ThingComp, PawnGizmoProvider
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600016C RID: 364 RVA: 0x000083C0 File Offset: 0x000065C0
		public CompProperties_BreakLink Props
		{
			get
			{
				return this.props as CompProperties_BreakLink;
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000083CD File Offset: 0x000065CD
		public IEnumerable<Gizmo> GetGizmos()
		{
			CompBreakLink.<GetGizmos>d__3 <GetGizmos>d__ = new CompBreakLink.<GetGizmos>d__3(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000083E0 File Offset: 0x000065E0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			IMinHeatGiver minHeatGiver = this.parent as IMinHeatGiver;
			if (minHeatGiver != null)
			{
				this.Pawn.Psycasts().AddMinHeatGiver(minHeatGiver);
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008414 File Offset: 0x00006614
		public override void CompTick()
		{
			base.CompTick();
			Pawn pawn = this.Pawn;
			bool flag = pawn == null || pawn.Dead || pawn.Destroyed;
			if (flag)
			{
				this.parent.Kill(null, null);
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000845F File Offset: 0x0000665F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.Pawn, "pawn", false);
		}

		// Token: 0x0400005F RID: 95
		public Pawn Pawn;
	}
}
