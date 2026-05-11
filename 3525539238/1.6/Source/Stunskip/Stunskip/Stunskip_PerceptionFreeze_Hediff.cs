using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using VEF.Abilities;
using VanillaPsycastsExpanded;

namespace Stunskip
{
    public class Stunskip_ApplyPerceptionFreeze : HediffCompProperties
    {
        public Stunskip_ApplyPerceptionFreeze()
        {
            this.compClass = typeof(HediffComp_ApplyPerceptionFreeze);
        }
    }

    public class HediffComp_ApplyPerceptionFreeze : HediffComp
    {
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);

            Pawn caster = this.Pawn;
            if (caster?.Map == null) return;

            foreach (Pawn other in caster.Map.mapPawns.AllPawnsSpawned)
            {
                if (other == caster) continue;
                if (other.Dead || other.Downed) continue;
                if (other.health.hediffSet.HasHediff(StunskipDefOf.VPE_StunskipWorld)) continue;
                if (other.health.hediffSet.HasHediff(StunskipDefOf.VPE_PerceptionFreeze)) continue;

                HealthUtility.AdjustSeverity(other, StunskipDefOf.VPE_PerceptionFreeze, 1f);
            }
        }
    }
    public class Stunskip_PerceptionFreeze_Hediff : Hediff_Overlay
    {

        public float curAngle;
        public override void Tick()
        {
            base.Tick();
            curAngle += 0.07f;
            if (curAngle > 360)
            {
                curAngle = 0;
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref curAngle, "curAngle");
        }
    }
}