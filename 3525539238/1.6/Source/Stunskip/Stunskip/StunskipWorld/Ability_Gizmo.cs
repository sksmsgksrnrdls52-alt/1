using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;
using UnityEngine;
using VEF.Abilities;
using Ability = VEF.Abilities.Ability;
using VanillaPsycastsExpanded;

namespace Stunskip
{
    // Credits: Vanilla Expanded Team for the amazing code - Specifically from Vanilla Psycast Expanded , Protector Tree
    public class Ability_Restrain : Ability
    {
        public bool IsActive => pawn.health.hediffSet.HasHediff(StunskipDefOf.VPE_StunskipWorld);

        public override Gizmo GetGizmo()
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(StunskipDefOf.VPE_StunskipWorld);
            if (hediff != null)
            {
                Texture2D customIcon = ContentFinder<Texture2D>.Get("Abilities/stunskip_cancel", true);
                return new Command_Action
                {
                    defaultLabel = "VPE.CancelStunskip".Translate(),
                    defaultDesc = "VPE.CancelStunskipDesc".Translate(),
                    icon = customIcon,
                    action = delegate { pawn.health.RemoveHediff(hediff); },
                    Order = 10f + (def.requiredHediff?.hediffDef?.index ?? 0) + (def.requiredHediff?.minimumLevel ?? 0)
                };
            }
            return base.GetGizmo();
        }
    }

    public class Ability_Deception : Ability
    {
        public bool IsActive => pawn.health.hediffSet.HasHediff(StunskipDefOf.VPE_DeceptionHediff);

        public override Gizmo GetGizmo()
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(StunskipDefOf.VPE_DeceptionHediff);
            if (hediff != null)
            {
                Texture2D customIcon = ContentFinder<Texture2D>.Get("Abilities/elusion_cancel", true);
                return new Command_Action
                {
                    defaultLabel = "VPE.CancelDeception".Translate(),
                    defaultDesc = "VPE.CancelDeceptionDesc".Translate(),
                    icon = customIcon,
                    action = delegate { pawn.health.RemoveHediff(hediff); },
                    Order = 10f + (def.requiredHediff?.hediffDef?.index ?? 0) + (def.requiredHediff?.minimumLevel ?? 0)
                };
            }
            return base.GetGizmo();

        }
    }

    public class Ability_Evasion : Ability
    {
        public bool IsActive => pawn.health.hediffSet.HasHediff(StunskipDefOf.VPE_EvasionHediff);

        public override Gizmo GetGizmo()
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(StunskipDefOf.VPE_EvasionHediff);
            if (hediff != null)
            {
                Texture2D customIcon = ContentFinder<Texture2D>.Get("Abilities/evasion_cancel", true);
                return new Command_Action
                {
                    defaultLabel = "VPE.CancelEvasion".Translate(),
                    defaultDesc = "VPE.CancelEvasionDesc".Translate(),
                    icon = customIcon,
                    action = delegate { pawn.health.RemoveHediff(hediff); },
                    Order = 10f + (def.requiredHediff?.hediffDef?.index ?? 0) + (def.requiredHediff?.minimumLevel ?? 0)
                };
            }
            return base.GetGizmo();
        }
    }
}

