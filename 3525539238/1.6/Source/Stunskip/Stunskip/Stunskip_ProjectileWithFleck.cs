using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using VanillaPsycastsExpanded.Graphics;
using Verse;
using Verse.Sound;
using VEF.Abilities;

namespace Stunskip
{
    public class Stunskip_ProjectileWithFleck : AbilityProjectile
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = true)
        {
            base.Impact(hitThing);

            if (hitThing != null && hitThing.Spawned)
            {

                FleckMaker.Static(hitThing.Position, hitThing.Map, StunskipDefOf.VPE_Stunskip_Distortion, 1f);
            }
        }
    }
}