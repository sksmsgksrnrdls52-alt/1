using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;
using VanillaPsycastsExpanded;

namespace Stunskip
{
    // Credits: Vanilla Expanded Team for the amazing code - Specifically from Vanilla Psycast Expanded , Protector Tree
    public class Hediff_Restrain : HediffWithComps
    {
        private Sustainer sustainer;
        public override void PostTick()
        {
            base.PostTick();
            this.AddEntropy();
            if (this.sustainer == null || this.sustainer.Ended)
                this.sustainer = StunskipDefOf.VPE_StunskipWorld_Sustainer.TrySpawnSustainer(SoundInfo.InMap(this.pawn, MaintenanceType.PerTick));
            this.sustainer.Maintain();
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (!this.sustainer.Ended) this.sustainer?.End();
        }

        private void AddEntropy()
        {
            if (Find.TickManager.TicksGame % 10 == 0) this.pawn.psychicEntropy.TryAddEntropy(2f, overLimit: true);
        }
    }

    public class Hediff_Deception : HediffWithComps
    {
        private Sustainer sustainer;
        public override void PostTick()
        {
            base.PostTick();
            this.AddEntropy();
            if (this.sustainer == null || this.sustainer.Ended)
                this.sustainer = StunskipDefOf.VPE_Deception_Sustainer.TrySpawnSustainer(SoundInfo.InMap(this.pawn, MaintenanceType.PerTick));
            this.sustainer.Maintain();
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (!this.sustainer.Ended) this.sustainer?.End();
        }

        private void AddEntropy()
        {
            if (Find.TickManager.TicksGame % 10 == 0) this.pawn.psychicEntropy.TryAddEntropy(1f, overLimit: true);
        }

    }

    public class Hediff_Evasion : HediffWithComps
    {
        private Sustainer sustainer;
        public override void PostTick()
        {
            base.PostTick();
            this.AddEntropy();
            if (this.sustainer == null || this.sustainer.Ended)
                this.sustainer = StunskipDefOf.VPE_Evasion_Sustainer.TrySpawnSustainer(SoundInfo.InMap(this.pawn, MaintenanceType.PerTick));
            this.sustainer.Maintain();
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (!this.sustainer.Ended) this.sustainer?.End();
        }

        private void AddEntropy()
        {
            if (Find.TickManager.TicksGame % 10 == 0) this.pawn.psychicEntropy.TryAddEntropy(1f, overLimit: true);
        }
    }
}
