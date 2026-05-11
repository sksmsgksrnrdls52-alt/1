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
using Verse.Sound;
using UnityEngine;

namespace Stunskip.Patch
{
    [StaticConstructorOnStartup]
    public static class StunskipWorld
    {
        static StunskipWorld()
        {
            var harmonyInstance = new Harmony("rimworld.mod.rabbit.stunskipworld");

            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

        }
    }


    [HarmonyPatch(typeof(TickManager), "TickManagerUpdate", new Type[] { })]
    public static class AllTickUpdate
    {
        private static bool wasPreviouslyPaused = false;

        public static bool Prefix(TickManager __instance)
        {
            List<Map> allMaps = Find.Maps;
            if (allMaps == null || allMaps.Count == 0)
            {
                CheckStunskip.curTimeSpeed = isStunskipWorld.Normal;
                return true;
            }

            bool hasStunskipPawn = allMaps
                .SelectMany(map => map.mapPawns.AllPawnsSpawned)
                .Any(pawn => pawn.health?.hediffSet?.HasHediff(StunskipDefOf.VPE_StunskipWorld) == true);

            CheckStunskip.curTimeSpeed = hasStunskipPawn ? isStunskipWorld.Paused : isStunskipWorld.Normal;

            if (CheckStunskip.curTimeSpeed == isStunskipWorld.Normal && wasPreviouslyPaused)
            {
                DamageWorker_KnockbackAttack.ProcessDeferredKnockbacks();
                wasPreviouslyPaused = false;
            }
            else if (CheckStunskip.curTimeSpeed == isStunskipWorld.Paused)
            {
                wasPreviouslyPaused = true;
            }

            return true;
        }

        [HarmonyPatch(typeof(Projectile), "Tick")]
        public static class ProjectileTickUpdate
        {
            public static bool Prefix(Projectile __instance)
            {
                if (__instance.def == ThingDef.Named("VPE_Air_Barrage_Projectile") ||
                    __instance.def == ThingDef.Named("VPE_Air_Javelin_Projectile"))
                    return true;

                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Projectile), "TickInterval")] //1.6//
        public static class ProjectileTickIntervalUpdate
        {
            public static bool Prefix(Projectile __instance, int delta)
            {
                if (__instance.def == ThingDef.Named("VPE_Air_Barrage_Projectile") ||
                    __instance.def == ThingDef.Named("VPE_Air_Javelin_Projectile"))
                    return true;

                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }



        [HarmonyPatch(typeof(Pawn), "Tick", new Type[] { })]
        public static class PawnTickUpdate
        {
            public static bool Prefix(Pawn __instance)
            {
                if (CheckStunskip.curTimeSpeed == isStunskipWorld.Paused)
                {
                    if (__instance?.health?.hediffSet?.HasHediff(StunskipDefOf.VPE_stuncaster) != true)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Pawn), "TickInterval")] //1.6//
        public static class PawnTickIntervalUpdate
        {
            public static bool Prefix(Pawn __instance)
            {
                if (CheckStunskip.curTimeSpeed == isStunskipWorld.Paused)
                {
                    bool isExempt =
                        __instance.health?.hediffSet?.HasHediff(StunskipDefOf.VPE_stuncaster) == true;

                    return isExempt;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Explosion), "Tick", new Type[] { })]
        public static class ExplosionTickUpdate
        {
            public static bool Prefix(Explosion __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Fire), "Tick", new Type[] { })]
        public static class FireTickUpdate
        {
            public static bool Prefix(Fire __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(GameConditionManager), "GameConditionManagerTick", new Type[] { })]
        public static class GameConditionManagerTickUpdate
        {
            public static bool Prefix(GameConditionManager __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(WeatherManager), "WeatherManagerTick", new Type[] { })]
        public static class WeatherTickUpdate
        {
            public static bool Prefix(WeatherManager __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Building_Trap), "Tick", new Type[] { })]
        public static class Building_TrapUpdate
        {
            public static bool Prefix(Building __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Tornado), "Tick", new Type[] { })]
        public static class TornadoTickUpdate
        {
            public static bool Prefix(Tornado __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(PowerBeam), "Tick", new Type[] { })]
        public static class PowerBeamTickUpdate
        {
            public static bool Prefix(PowerBeam __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Bombardment), "Tick", new Type[] { })]
        public static class BombardmentTickUpdate
        {
            public static bool Prefix(Bombardment __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(OrbitalStrike), "Tick", new Type[] { })]
        public static class OrbitalStrikeTickUpdate
        {
            public static bool Prefix(OrbitalStrike __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(QuestManager), "QuestManagerTick", new Type[] { })]
        public static class QuestManagerTickUpdate
        {
            public static bool Prefix(QuestManager __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(EffecterMaintainer), "EffecterMaintainerTick", new Type[] { })]
        public static class EffecterMaintainerTickUpdate
        {
            public static bool Prefix(EffecterMaintainer __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(WindManager), "WindManagerTick", new Type[] { })]
        public static class WindManagerTickUpdate
        {
            public static bool Prefix(WindManager __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Building_TurretGun), "Tick", new Type[] { })]
        public static class Building_TurretGunTickUpdate
        {
            public static bool Prefix(Building __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Building_Turret), "Tick", new Type[] { })]
        public static class Building_TurretTickUpdate
        {
            public static bool Prefix(Building __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(TurretTop), "TurretTopTick", new Type[] { })]
        public static class TurretTopTickUpdate
        {
            public static bool Prefix(TurretTop __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Skyfaller), "Tick", new Type[] { })]
        public static class SkyfallerTickUpdate
        {
            public static bool Prefix(Skyfaller __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        [HarmonyPatch(typeof(Pawn_StanceTracker), "StanceTrackerTick")]
        public static class StanceTrackerTickUpdate
        {
            public static bool Prefix(Pawn_StanceTracker __instance)
            {
                Pawn pawn = __instance.pawn;
                if (CheckStunskip.curTimeSpeed == isStunskipWorld.Paused)
                {
                    if (!pawn.health.hediffSet.HasHediff(StunskipDefOf.VPE_StunskipWorld))
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        [HarmonyPatch(typeof(FleckManager), "FleckManagerTick", new Type[] { })]
        public static class FleckManagerTickUpdate
        {
            public static bool Prefix(FleckManager __instance)
            {
                return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
            }
        }

        //[HarmonyPatch(typeof(Map), "MapPreTick", new Type[] { })]  //This freezes the caster when used//
        //public static class MapPreTickUpdate
        //{
        //    public static bool Prefix(Map __instance)
        //    {
        //        return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
        //    }
        //}

        //[HarmonyPatch(typeof(Projectile_Explosive), "Tick", new Type[] { })]
        //public static class Projectile_ExplosiveTickUpdate
        //{
        //    public static bool Prefix(Projectile __instance)
        //    {
        //        return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
        //    }
        //}

        //[HarmonyPatch(typeof(Gas), "Tick", new Type[] { })]
        //public static class GasTickUpdate
        //{
        //    public static bool Prefix(Gas __instance)
        //    {
        //        return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
        //   }
        //}

        //[HarmonyPatch(typeof(PawnFlyer), "Tick", new Type[] { })]
        //public static class PawnFlyerTickUpdate
        //{
        //   public static bool Prefix(PawnFlyer __instance)
        //    {
        //        return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
        //    }
        //}

        //[HarmonyPatch(typeof(ActiveDropPod), "Tick", new Type[] { })]
        //public static class ActiveDropPodTickUpdate
        // {
        //     public static bool Prefix(ActiveDropPod __instance)
        //     {
        //         return CheckStunskip.curTimeSpeed != isStunskipWorld.Paused;
        //     }
        // }

    }

}