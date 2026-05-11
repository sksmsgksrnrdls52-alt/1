using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000E9 RID: 233
	[StaticConstructorOnStartup]
	public class Ability_Construct_Rock : Ability
	{
		// Token: 0x0600032B RID: 811 RVA: 0x00013BA0 File Offset: 0x00011DA0
		static Ability_Construct_Rock()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (Ability_Construct_Rock.IsNonResourceNaturalRock(thingDef) && thingDef.building.mineableThing != null)
				{
					Ability_Construct_Rock.chunkCache.Add(thingDef.building.mineableThing);
				}
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00013C1C File Offset: 0x00011E1C
		public static bool IsNonResourceNaturalRock(ThingDef def)
		{
			return def.category == 3 && def.building != null && def.building.isNaturalRock && !def.building.isResourceRock && !def.IsSmoothed;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00013C54 File Offset: 0x00011E54
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(VPE_DefOf.VPE_RockConstruct, this.pawn.Faction, null);
				ThingCompUtility.TryGetComp<CompBreakLink>(pawn).Pawn = this.pawn;
				Thing thing = globalTargetInfo.Thing;
				GenSpawn.Spawn(pawn, thing.Position, thing.Map, thing.Rotation, 0, false, false);
				ThingCompUtility.TryGetComp<CompSetStoneColour>(pawn).SetStoneColour(thing.def);
				thing.SplitOff(1).Destroy(0);
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00013CF4 File Offset: 0x00011EF4
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
			if (!Ability_Construct_Rock.chunkCache.Contains(target.Thing.def))
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustBeStoneChunk"), MessageTypeDefOf.RejectInput, false);
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

		// Token: 0x04000195 RID: 405
		private static readonly HashSet<ThingDef> chunkCache = new HashSet<ThingDef>();
	}
}
