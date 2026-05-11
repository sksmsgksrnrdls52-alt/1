using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000095 RID: 149
	public class Ability_PsychicShock : Ability
	{
		// Token: 0x060001BE RID: 446 RVA: 0x0000A018 File Offset: 0x00008218
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Thing as Pawn;
			return pawn != null && StatExtension.GetStatValue(pawn, StatDefOf.PsychicSensitivity, true, -1) > 0f && base.ValidateTarget(target, showMessages);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000A054 File Offset: 0x00008254
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				if (Rand.Chance(0.3f))
				{
					Pawn pawn = globalTargetInfo.Thing as Pawn;
					FireUtility.TryAttachFire(globalTargetInfo.Thing, 0.5f, base.Caster);
					BodyPartRecord bodyPartRecord = (pawn != null) ? pawn.health.hediffSet.GetBrain() : null;
					if (bodyPartRecord != null)
					{
						int num = Rand.RangeInclusive(1, 5);
						pawn.TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.pawn, bodyPartRecord, null, 0, null, true, true, 2, true, false));
					}
				}
			}
		}
	}
}
