using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000EA RID: 234
	public class Ability_Construct_Steel : Ability
	{
		// Token: 0x06000330 RID: 816 RVA: 0x00013DA0 File Offset: 0x00011FA0
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(VPE_DefOf.VPE_SteelConstruct, this.pawn.Faction, null);
				ThingCompUtility.TryGetComp<CompBreakLink>(pawn).Pawn = this.pawn;
				Thing thing = globalTargetInfo.Thing;
				GenSpawn.Spawn(pawn, thing.Position, thing.Map, thing.Rotation, 0, false, false);
				thing.SplitOff(1).Destroy(0);
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00013E2C File Offset: 0x0001202C
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
			if (target.Thing.def != ThingDefOf.ChunkSlagSteel)
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustBeSteelSlag"), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (this.pawn.psychicEntropy.MaxEntropy - this.pawn.psychicEntropy.EntropyValue <= 20f)
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.NotEnoughHeat"), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
