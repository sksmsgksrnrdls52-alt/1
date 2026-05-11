using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x02000125 RID: 293
	public class Ability_HealthSwap : Ability
	{
		// Token: 0x0600043B RID: 1083 RVA: 0x000199C0 File Offset: 0x00017BC0
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Pawn pawn = targets[0].Thing as Pawn;
			if (pawn != null)
			{
				Pawn pawn2 = targets[1].Thing as Pawn;
				if (pawn2 != null)
				{
					MoteBetween moteBetween = (MoteBetween)ThingMaker.MakeThing(VPE_DefOf.VPE_PsycastPsychicEffectTransfer, null);
					moteBetween.Attach(pawn, pawn2);
					moteBetween.Scale = 1f;
					moteBetween.exactPosition = pawn.DrawPos;
					GenSpawn.Spawn(moteBetween, pawn.Position, pawn.MapHeld, 0);
					IEnumerable<Hediff> hediffs = pawn.health.hediffSet.hediffs;
					Func<Hediff, bool> predicate;
					if ((predicate = Ability_HealthSwap.<>O.<0>__ShouldTransfer) == null)
					{
						predicate = (Ability_HealthSwap.<>O.<0>__ShouldTransfer = new Func<Hediff, bool>(Ability_HealthSwap.ShouldTransfer));
					}
					List<Hediff> list = hediffs.Where(predicate).ToList<Hediff>();
					IEnumerable<Hediff> hediffs2 = pawn2.health.hediffSet.hediffs;
					Func<Hediff, bool> predicate2;
					if ((predicate2 = Ability_HealthSwap.<>O.<0>__ShouldTransfer) == null)
					{
						predicate2 = (Ability_HealthSwap.<>O.<0>__ShouldTransfer = new Func<Hediff, bool>(Ability_HealthSwap.ShouldTransfer));
					}
					List<Hediff> list2 = hediffs2.Where(predicate2).ToList<Hediff>();
					foreach (Hediff hediff in list)
					{
						pawn.health.RemoveHediff(hediff);
					}
					foreach (Hediff hediff2 in list2)
					{
						pawn2.health.RemoveHediff(hediff2);
					}
					Ability_HealthSwap.AddAll(pawn, list2);
					Ability_HealthSwap.AddAll(pawn2, list);
					return;
				}
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00019B54 File Offset: 0x00017D54
		private static bool ShouldTransfer(Hediff hediff)
		{
			bool flag = hediff is Hediff_Injury || hediff is Hediff_MissingPart || hediff is Hediff_Addiction;
			return flag || hediff.def.tendable || hediff.def.makesSickThought || hediff.def.HasComp(typeof(HediffComp_Immunizable));
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00019BB3 File Offset: 0x00017DB3
		private static void AddAll(Pawn pawn, List<Hediff> hediffs)
		{
			Ability_HealthSwap.<>c__DisplayClass2_0 CS$<>8__locals1 = new Ability_HealthSwap.<>c__DisplayClass2_0();
			CS$<>8__locals1.hediffs = hediffs;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.<AddAll>g__TryAdd|0();
			CS$<>8__locals1.<AddAll>g__TryAdd|0();
		}

		// Token: 0x020001B1 RID: 433
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000351 RID: 849
			public static Func<Hediff, bool> <0>__ShouldTransfer;
		}
	}
}
