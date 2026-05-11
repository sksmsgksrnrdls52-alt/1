using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000EF RID: 239
	public class Ability_Psyring : Ability
	{
		// Token: 0x06000348 RID: 840 RVA: 0x00014808 File Offset: 0x00012A08
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Thing thing = targets[0].Thing;
			if (thing == null)
			{
				return;
			}
			WindowStack windowStack = Find.WindowStack;
			Pawn pawn = this.pawn;
			Thing fuel = thing;
			PsyringExclusionExtension modExtension = this.def.GetModExtension<PsyringExclusionExtension>();
			windowStack.Add(new Dialog_CreatePsyring(pawn, fuel, (modExtension != null) ? modExtension.excludedAbilities : null));
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0001485C File Offset: 0x00012A5C
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			if (!target.HasThing)
			{
				return false;
			}
			if (target.Thing.def != VPE_DefOf.VPE_Eltex)
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustEltex"), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
