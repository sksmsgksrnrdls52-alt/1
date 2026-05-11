using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using VEF.Abilities;
using AbilityDef = VEF.Abilities.AbilityDef;

namespace Stunskip
{
    [DefOf]
    public static class StunskipDefOf
    {
        static StunskipDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(StunskipDefOf));
        }

        public static AbilityDef VPE_stunskip_restrain_i;
        public static AbilityDef VPE_stunskip_restrain_ii;

        public static AbilityDef VPE_stunskip_teleport_ii;
        public static AbilityDef VPE_stunskip_strike_ii;
        public static AbilityDef VPE_air_push;
        public static AbilityDef VPE_deception;

        public static FleckDef VPE_Stunskip_Distortion;
        public static FleckDef VPE_Stunskip_AirPuff;

        public static HediffDef VPE_PerceptionFreeze;
        public static HediffDef VPE_StunskipWorld;
        public static HediffDef VPE_ProjectilePatch;
        public static HediffDef VPE_stuncaster;
        public static HediffDef VPE_DeceptionHediff;
        public static HediffDef VPE_EvasionHediff;

        public static SoundDef VPE_StunskipWorld_Sustainer;
        public static SoundDef VPE_Deception_Sustainer;
        public static SoundDef VPE_Evasion_Sustainer;

    }
}
