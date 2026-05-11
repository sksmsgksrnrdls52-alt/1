using RimWorld;
using RimWorld.Planet;
using VanillaPsycastsExpanded.Skipmaster;
using VanillaPsycastsExpanded;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using Ability = VEF.Abilities.Ability;

namespace Stunskip
{
    public class Stunskip_Teleport : Stunskip_Teleport_Base
    {
        public override void ModifyTargets(ref GlobalTargetInfo[] targets)
        {
            base.ModifyTargets(ref targets);
            targets = new[] { this.pawn, targets[0] };
        }
    }
}