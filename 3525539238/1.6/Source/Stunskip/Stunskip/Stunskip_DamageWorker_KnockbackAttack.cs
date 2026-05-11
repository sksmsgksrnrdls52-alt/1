using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using VanillaPsycastsExpanded;
using Verse;
using Verse.AI;
using Verse.Sound;
using VEF;
using VEF.Weapons;
using Ability = VEF.Abilities.Ability;

namespace Stunskip
{
        // Credits: Vanilla Expanded Team for the amazing code - Specifically from Vanilla Framework Expanded
        public class DamageWorker_KnockbackAttack : DamageWorker_Blunt
        {
            private static readonly List<DeferredKnockback> DeferredKnockbacks = new List<DeferredKnockback>();

            public override DamageResult Apply(DamageInfo dinfo, Thing thing)
            {
                if (dinfo.Instigator != null && thing is Pawn targetPawn)
                {
                    var modExtension = this.def.GetModExtension<DamageExtension>();
                    float knockBackDistance = modExtension.pushBackDistance.RandomInRange;

                    if (CheckStunskip.curTimeSpeed == isStunskipWorld.Paused)
                    {
                        DeferredKnockbacks.Add(new DeferredKnockback
                        {
                            Attacker = dinfo.Instigator,
                            Target = targetPawn,
                            Distance = knockBackDistance,
                            Extension = modExtension
                        });
                    }
                    else
                    {
                        ApplyKnockBack(dinfo.Instigator, targetPawn, knockBackDistance, modExtension);
                    }
                }
                return base.Apply(dinfo, thing);
            }

            public static void ProcessDeferredKnockbacks()
             {
                foreach (var knockback in DeferredKnockbacks)
                {
                    if (knockback.Target?.Spawned != true)
                        continue;

                    ApplyKnockBack(knockback.Attacker,knockback.Target,knockback.Distance,knockback.Extension);
                }
                DeferredKnockbacks.Clear();
            }

        private static void ApplyKnockBack(Thing attacker, Thing target, float knockBackDistance, DamageExtension extension)
        {
            if (target == null || target.Map == null)
                return;

            IntVec3 direction = new IntVec3(
                Math.Sign(target.Position.x - attacker.Position.x),
                0,
                Math.Sign(target.Position.z - attacker.Position.z)
            );

            var knockBackCells = Enumerable.Range(1, (int)Math.Ceiling(knockBackDistance))
                .Select(distance => target.Position + (direction * distance))
                .Where(cell => cell.InBounds(target.Map) && cell.Walkable(target.Map) && GenSight.LineOfSight(target.Position, cell, target.Map));

            if (knockBackCells.Any())
            {
                IntVec3 knockBackCell = knockBackCells.Last();
                target.Position = knockBackCell;

                if (target is Pawn pawn)
                {
                    pawn.pather?.StopDead();
                    pawn.jobs?.StopAll(false, true);
                }

                if (extension.fleckOnDamage != null)
                {
                    var fleckSource = extension.fleckOnInstigator ? attacker : target;
                    FleckMaker.Static(fleckSource.Position, fleckSource.Map, extension.fleckOnDamage, extension.fleckRadius);
                }
            }
        }



    }

    public class DeferredKnockback
        {
                public Thing Attacker { get; set; }
                public Thing Target { get; set; }
                public float Distance { get; set; }
                public DamageExtension Extension { get; set; }
        }
}