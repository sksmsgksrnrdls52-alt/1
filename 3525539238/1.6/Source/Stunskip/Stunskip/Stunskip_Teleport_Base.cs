using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using VanillaPsycastsExpanded.Skipmaster;
using Ability = VEF.Abilities.Ability;

namespace Stunskip
{
    // Credits: Vanilla Expanded Team for the amazing code - Specifically from Vanilla Psycast Expanded , Skipmaster Tree
    public class Stunskip_Teleport_Base : Ability
    {
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            AbilityExtension_Clamor clamor = this.def.GetModExtension<AbilityExtension_Clamor>();

            for (int i = 0; i < targets.Length; i += 2)
            {
                if (targets[i].Thing is { } t)
                {
                    t.TryGetComp<CompCanBeDormant>()?.WakeUp();
                    GlobalTargetInfo dest = targets[i + 1];

                    IntVec3 origin = t.Position;

                    if (t.Map != dest.Map)
                    {
                        if (t is not Pawn p) continue;
                        p.teleporting = true;
                        p.ExitMap(true, Rot4.Invalid);
                        p.teleporting = false;
                        GenSpawn.Spawn(p, dest.Cell, dest.Map);
                    }

                    t.Position = dest.Cell;

                    SpawnTrail(origin, dest.Cell, dest.Map);

                    AbilityUtility.DoClamor(t.Position, clamor.clamorRadius, this.pawn, clamor.clamorType);
                    AbilityUtility.DoClamor(dest.Cell, clamor.clamorRadius, this.pawn, clamor.clamorType);
                    (t as Pawn)?.Notify_Teleported(false);
                }
            }

            base.Cast(targets);
        }

        public static void SpawnTrail(IntVec3 from, IntVec3 to, Map map)
        {
            int steps = Mathf.CeilToInt(IntVec3Utility.DistanceTo(from, to));

            for (int i = 0; i <= steps; i++)
            {
                float lerpFactor = i / (float)steps;
                Vector3 pos = Vector3.Lerp(from.ToVector3Shifted(), to.ToVector3Shifted(), lerpFactor);
                FleckMaker.Static(pos.ToIntVec3(), map, StunskipDefOf.VPE_Stunskip_AirPuff, 1f);
            }
        }

    }

}