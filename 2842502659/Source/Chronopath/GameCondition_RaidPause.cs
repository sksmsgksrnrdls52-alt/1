using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Chronopath
{
	// Token: 0x02000136 RID: 310
	[HarmonyPatch]
	public class GameCondition_RaidPause : GameCondition_TimeSnow
	{
		// Token: 0x06000473 RID: 1139 RVA: 0x0001B3EC File Offset: 0x000195EC
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			if (base.TicksPassed % 60 == 0)
			{
				foreach (Map map in base.AffectedMaps)
				{
					foreach (Pawn pawn in map.attackTargetsCache.TargetsHostileToColony.OfType<Pawn>())
					{
						pawn.stances.stunner.StunFor(61, null, false, true, false);
					}
				}
			}
			if (this.sustainer == null)
			{
				this.sustainer = SoundStarter.TrySpawnSustainer(VPE_DefOf.VPE_RaidPause_Sustainer, SoundInfo.OnCamera(0));
				return;
			}
			this.sustainer.Maintain();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001B4C4 File Offset: 0x000196C4
		public override void End()
		{
			this.sustainer.End();
			base.End();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001B4D8 File Offset: 0x000196D8
		[HarmonyPatch(typeof(Pawn_HealthTracker), "PostApplyDamage")]
		[HarmonyPostfix]
		public static void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt, Pawn ___pawn)
		{
			if (totalDamageDealt >= 0f && dinfo.Def.ExternalViolenceFor(___pawn))
			{
				Thing instigator = dinfo.Instigator;
				if (instigator != null && GenHostility.HostileTo(___pawn, instigator) && !GenHostility.HostileTo(instigator, Faction.OfPlayer))
				{
					Map mapHeld = ___pawn.MapHeld;
					if (((mapHeld != null) ? mapHeld.gameConditionManager : null) != null)
					{
						foreach (GameCondition_RaidPause gameCondition_RaidPause in ___pawn.MapHeld.gameConditionManager.ActiveConditions.OfType<GameCondition_RaidPause>().ToList<GameCondition_RaidPause>())
						{
							gameCondition_RaidPause.End();
						}
					}
				}
			}
		}

		// Token: 0x040001DF RID: 479
		private Sustainer sustainer;
	}
}
