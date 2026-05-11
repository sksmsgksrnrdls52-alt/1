using System;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x02000128 RID: 296
	public class Ability_HeatFocus : Ability
	{
		// Token: 0x06000443 RID: 1091 RVA: 0x00019DAC File Offset: 0x00017FAC
		public unsafe override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			float num = Mathf.Min(1f - this.pawn.psychicEntropy.CurrentPsyfocus, (this.pawn.psychicEntropy.EntropyValue - StatExtension.GetStatValue(this.pawn, VPE_DefOf.VPE_PsychicEntropyMinimum, true, -1)) * 0.002f);
			this.pawn.psychicEntropy.OffsetPsyfocusDirectly(num);
			*Ability_HeatFocus.currentEntropy.Invoke(this.pawn.psychicEntropy) -= num * 500f;
		}

		// Token: 0x040001D6 RID: 470
		private static readonly AccessTools.FieldRef<Pawn_PsychicEntropyTracker, float> currentEntropy = AccessTools.FieldRefAccess<Pawn_PsychicEntropyTracker, float>("currentEntropy");
	}
}
