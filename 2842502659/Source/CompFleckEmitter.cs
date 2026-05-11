using System;
using RimWorld;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000026 RID: 38
	public class CompFleckEmitter : ThingComp
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000038DF File Offset: 0x00001ADF
		private CompProperties_FleckEmitter Props
		{
			get
			{
				return (CompProperties_FleckEmitter)this.props;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000038EC File Offset: 0x00001AEC
		public override void CompTick()
		{
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return;
			}
			CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
			if (comp2 != null && comp2.ticksLeft <= 0)
			{
				return;
			}
			CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
			if ((comp3 == null || comp3.Initiated) && this.Props.emissionInterval != -1)
			{
				if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
				{
					this.Emit();
					this.ticksSinceLastEmitted = 0;
					return;
				}
				this.ticksSinceLastEmitted++;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003980 File Offset: 0x00001B80
		protected void Emit()
		{
			FleckMaker.Static(this.parent.DrawPos + this.Props.offset, this.parent.MapHeld, this.Props.fleck, this.Props.scale);
			if (!SoundDefHelper.NullOrUndefined(this.Props.soundOnEmission))
			{
				SoundStarter.PlayOneShot(this.Props.soundOnEmission, SoundInfo.InMap(this.parent, 0));
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003A04 File Offset: 0x00001C04
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x0400001A RID: 26
		public int ticksSinceLastEmitted;
	}
}
