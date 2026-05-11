using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using HarmonyLib;
using RimWorld;
using System.Reflection;
using VEF;

namespace Stunskip.Patch
{
    [StaticConstructorOnStartup]
    public static class Stunskip
    {
        static Stunskip()
        {
            Log.Message("The Stuncaster lurks.");

            Harmony harmony = new Harmony("rimworld.mod.rabbit.stunskip");
            harmony.PatchAll();

        }
    }


    [HarmonyPatch(typeof(Pawn), "Kill")]
    public static class Stunskip_Pawn_Kill_Patch
    {
        private static bool Prefix(Pawn __instance)
        {
            var hediffs = __instance.health.hediffSet;
            if (hediffs.HasHediff(StunskipDefOf.VPE_PerceptionFreeze) ||
            hediffs.HasHediff(StunskipDefOf.VPE_StunskipWorld))
            {
                return false;
            }
            return true;
        }

    }
    
    [HarmonyPatch]
    public static class Stunskip_Projectile_Patch
    {
        [HarmonyPatch(typeof(Projectile), "CanHit")]
        [HarmonyPrefix]
        public static bool CanHit_Prefix(Bullet __instance)
        {
            return __instance.intendedTarget == null || __instance.intendedTarget.Pawn == null || !__instance.intendedTarget.Pawn.health.hediffSet.HasHediff(StunskipDefOf.VPE_ProjectilePatch, false);
        }
    }

    [HarmonyPatch]
    public static class Stunskip_Evasion_Patch
    {
        [HarmonyPatch(typeof(Projectile), "CanHit")]
        [HarmonyPrefix]
        public static bool CanHit_Prefix(Bullet __instance)
        {
            return __instance.intendedTarget == null || __instance.intendedTarget.Pawn == null || !__instance.intendedTarget.Pawn.health.hediffSet.HasHediff(StunskipDefOf.VPE_EvasionHediff, false);
        }
    }

    [HarmonyPatch(typeof(Hediff), "PostRemoved")]
    public static class Hediff_PostRemoved_Patch
    {
        static void Postfix(Hediff __instance)
        {
            if (__instance.def.defName == "VPE_StunskipWorld")
            {
                Pawn pawn = __instance.pawn;
                if (pawn != null)
                {
                    pawn.health.AddHediff(HediffDef.Named("VPE_ProjectilePatch"));
                }
            }
        }
    }


}