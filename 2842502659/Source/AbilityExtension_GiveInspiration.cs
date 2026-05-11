using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000A9 RID: 169
	public class AbilityExtension_GiveInspiration : AbilityExtension_AbilityMod
	{
		// Token: 0x06000212 RID: 530 RVA: 0x0000BC84 File Offset: 0x00009E84
		public override void Cast(GlobalTargetInfo[] targets, Ability ability)
		{
			base.Cast(targets, ability);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					if (this.onlyPlayer)
					{
						Faction faction = pawn.Faction;
						if (faction == null || !faction.IsPlayer)
						{
							goto IL_9A;
						}
					}
					InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
					if (randomAvailableInspirationDef != null)
					{
						pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, TranslatorFormattedStringExtensions.Translate("LetterPsychicInspiration", NamedArgumentUtility.Named(pawn, "PAWN"), NamedArgumentUtility.Named(ability.pawn, "CASTER")), true);
					}
				}
				IL_9A:;
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000BD38 File Offset: 0x00009F38
		public override bool CanApplyOn(LocalTargetInfo target, Ability ability, bool throwMessages = false)
		{
			return this.Valid(new GlobalTargetInfo[]
			{
				target.ToGlobalTargetInfo(target.Thing.Map)
			}, ability, false);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000BD64 File Offset: 0x00009F64
		public override bool Valid(GlobalTargetInfo[] targets, Ability ability, bool throwMessages = false)
		{
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					if (this.onlyPlayer)
					{
						Faction faction = pawn.Faction;
						if (faction == null || !faction.IsPlayer)
						{
							goto IL_53;
						}
					}
					if (!AbilityUtility.ValidateNoInspiration(pawn, throwMessages, null))
					{
						return false;
					}
					if (!AbilityUtility.ValidateCanGetInspiration(pawn, throwMessages, null))
					{
						return false;
					}
				}
				IL_53:;
			}
			return base.Valid(targets, ability, throwMessages);
		}

		// Token: 0x0400009E RID: 158
		public bool onlyPlayer;
	}
}
