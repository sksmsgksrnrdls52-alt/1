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
    public class AbilityExtension_StunskipCooldown : AbilityExtension_AbilityMod
    {
        public override bool IsEnabledForPawn(Ability ability, out string reason)
        {
            if (CheckStunskip.curTimeSpeed == isStunskipWorld.Paused)
            {
                reason = "VPE.StunskipIsActive".Translate();
                return false;
            }
            return base.IsEnabledForPawn(ability, out reason);
        }
    }

}
