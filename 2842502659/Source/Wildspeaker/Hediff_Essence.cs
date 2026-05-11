using System;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded.Wildspeaker
{
	// Token: 0x020000E1 RID: 225
	public class Hediff_Essence : HediffWithComps
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0001350A File Offset: 0x0001170A
		public override string Label
		{
			get
			{
				return base.Label + " " + this.EssenceOf.NameShortColored;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00013534 File Offset: 0x00011734
		public override bool ShouldRemove
		{
			get
			{
				if (this.EssenceOf == null)
				{
					return true;
				}
				if (this.EssenceOf.Dead)
				{
					Corpse corpse = this.EssenceOf.Corpse;
					return corpse == null || !corpse.Spawned;
				}
				return false;
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00013578 File Offset: 0x00011778
		public override void Tick()
		{
			base.Tick();
			Pawn essenceOf = this.EssenceOf;
			if (essenceOf != null && essenceOf.Dead)
			{
				Corpse corpse = essenceOf.Corpse;
				if (corpse != null && corpse.Spawned && this.pawn.CurJobDef != VPE_DefOf.VPE_EssenceTransfer)
				{
					Job job = JobMaker.MakeJob(VPE_DefOf.VPE_EssenceTransfer, this.EssenceOf.Corpse);
					job.forceSleep = true;
					this.pawn.jobs.StartJob(job, 16, null, false, true, null, null, false, false, null, false, true, false);
				}
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00013612 File Offset: 0x00011812
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.EssenceOf, "essenceOf", false);
		}

		// Token: 0x04000191 RID: 401
		public Pawn EssenceOf;
	}
}
