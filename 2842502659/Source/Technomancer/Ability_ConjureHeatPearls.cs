using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000EC RID: 236
	public class Ability_ConjureHeatPearls : Ability
	{
		// Token: 0x06000337 RID: 823 RVA: 0x00014018 File Offset: 0x00012218
		public override bool IsEnabledForPawn(out string reason)
		{
			if (!base.IsEnabledForPawn(ref reason))
			{
				return false;
			}
			if (this.pawn.psychicEntropy.EntropyValue - StatExtension.GetStatValue(this.pawn, VPE_DefOf.VPE_PsychicEntropyMinimum, true, -1) >= 20f)
			{
				return true;
			}
			reason = Translator.Translate("VPE.NotEnoughHeat");
			return false;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00014070 File Offset: 0x00012270
		public unsafe override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			*Ability_ConjureHeatPearls.currentEntropy.Invoke(this.pawn.psychicEntropy) -= 20f;
			Thing thing = ThingMaker.MakeThing(VPE_DefOf.VPE_HeatPearls, null);
			IntVec3 intVec = this.pawn.Position + GenRadial.RadialPattern[Rand.RangeInclusive(2, GenRadial.NumCellsInRadius(4.9f))];
			IntVec3 intVec2 = intVec;
			Map map = this.pawn.Map;
			float num = 1.9f;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing pawn = this.pawn;
			int num2 = -1;
			float num3 = -1f;
			SoundDef soundDef = null;
			ThingDef thingDef = null;
			ThingDef thingDef2 = null;
			Thing thing2 = null;
			ThingDef thingDef3 = null;
			float num4 = 0f;
			int num5 = 1;
			List<Thing> list = new List<Thing>
			{
				this.pawn,
				thing
			};
			GenExplosion.DoExplosion(intVec2, map, num, bomb, pawn, num2, num3, soundDef, thingDef, thingDef2, thing2, thingDef3, num4, num5, null, null, 255, false, null, 0f, 1, 0f, false, null, list, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			GenSpawn.Spawn(thing, intVec, this.pawn.Map, 0);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00014189 File Offset: 0x00012389
		public override string GetDescriptionForPawn()
		{
			return base.GetDescriptionForPawn() + "\n" + ColoredText.Colorize(TranslatorFormattedStringExtensions.Translate("VPE.MustHaveHeatAmount", 20), Color.red);
		}

		// Token: 0x04000196 RID: 406
		private static readonly AccessTools.FieldRef<Pawn_PsychicEntropyTracker, float> currentEntropy = AccessTools.FieldRefAccess<Pawn_PsychicEntropyTracker, float>("currentEntropy");
	}
}
