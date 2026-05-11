using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VanillaPsycastsExpanded;
using Verse;
using Verse.AI;
using Verse.Sound;
using Ability = VEF.Abilities.Ability;

namespace Stunskip
{
    // Credits: Vanilla Expanded Team for the amazing code - Specifically from Vanilla Psycast Expanded , Warlord Tree
    public class Stunskip_Strike_ii : Ability
    {
        
        private int attackInTicks = -1;
     
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            AttackTarget((LocalTargetInfo)targets[0]);
        }

        public override void Tick()
        {
            base.Tick();
            if (attackInTicks != -1 && Find.TickManager.TicksGame >= attackInTicks)
            {
                attackInTicks = -1;
                var target = FindAttackTarget();
                if (target != null) AttackTarget(target);
            }
        }

        private void AttackTarget(LocalTargetInfo target)
        {
            IntVec3 start = pawn.Position;
            IntVec3 end = target.Cell;

            SpawnTrail(start, end, pawn.Map);

            pawn.Position = end;
            pawn.Notify_Teleported(false);
            pawn.stances.SetStance(new Stance_Mobile());

            VerbProperties_AdjustedMeleeDamageAmount_Patch.multiplyByPawnMeleeSkill = true;
            pawn.meleeVerbs.TryMeleeAttack(target.Pawn, null, true);
            VerbProperties_AdjustedMeleeDamageAmount_Patch.multiplyByPawnMeleeSkill = false;


            if (target.Pawn != null)
            {
                TryToKnockBack(pawn, target.Pawn, 10.0f);
            }
        }
        private void TryToKnockBack(Thing attacker, Thing target, float knockBackDistance)
        {
            var map = target.Map;
            Predicate<IntVec3> validator = cell =>
                cell.DistanceTo(target.Position) >= knockBackDistance &&
                GenGrid.Walkable(cell, map) &&
                GenSight.LineOfSight(target.Position, cell, map);

            var possibleCells = GenRadial.RadialCellsAround(target.Position, knockBackDistance, true)
                .Where(cell => validator(cell)).ToList();

            if (possibleCells.Any())
            {
                IntVec3 newPosition = possibleCells.RandomElement();
                target.Position = newPosition;

                if (target is Pawn knockedPawn)
                {
                    knockedPawn.pather.StopDead();
                    knockedPawn.jobs?.StopAll(false, true);
                }
            }
        }

        private void SpawnTrail(IntVec3 from, IntVec3 to, Map map)
        {
            int steps = Mathf.CeilToInt(IntVec3Utility.DistanceTo(from, to));
            for (int i = 0; i <= steps; i++)
            {
                float lerpFactor = i / (float)steps;
                Vector3 pos = Vector3.Lerp(from.ToVector3Shifted(), to.ToVector3Shifted(), lerpFactor);
                FleckMaker.Static(pos.ToIntVec3(), map, StunskipDefOf.VPE_Stunskip_AirPuff, 1f);
            }
        }


        private Pawn FindAttackTarget()
        {
            var targetScanFlags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachableIfCantHitFromMyPos | TargetScanFlags.NeedThreat
                                  | TargetScanFlags.NeedAutoTargetable;
            return (Pawn)AttackTargetFinder.BestAttackTarget(pawn, targetScanFlags, x => x is Pawn pawn && !pawn.Dead, 0f, 999999);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref attackInTicks, "attackInTicks", -1);
        }
    }
}