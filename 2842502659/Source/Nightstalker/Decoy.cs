using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.AI;

namespace VanillaPsycastsExpanded.Nightstalker
{
	// Token: 0x0200011E RID: 286
	[HarmonyPatch]
	public class Decoy : ThingWithComps, IAttackTarget, ILoadReferenceable
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x00019310 File Offset: 0x00017510
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			return false;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x00019313 File Offset: 0x00017513
		public Thing Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00019316 File Offset: 0x00017516
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0001931D File Offset: 0x0001751D
		public float TargetPriorityFactor
		{
			get
			{
				return float.MaxValue;
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00019324 File Offset: 0x00017524
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.pawn = base.GetComp<CompAbilitySpawn>().pawn;
			Decoy.mapsWithDecoys.Add(map);
			base.SpawnSetup(map, respawningAfterLoad);
			foreach (IAttackTarget attackTarget in this.pawn.Map.attackTargetsCache.TargetsHostileToFaction(this.pawn.Faction))
			{
				Pawn pawn = attackTarget.Thing as Pawn;
				if (pawn != null)
				{
					JobDef curJobDef = pawn.CurJobDef;
					if (curJobDef == JobDefOf.Wait_Combat || curJobDef == JobDefOf.Goto)
					{
						pawn.jobs.EndCurrentJob(16, true, true);
					}
				}
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000193E4 File Offset: 0x000175E4
		public override void DeSpawn(DestroyMode mode = 0)
		{
			if (!GenCollection.Except<Decoy>(base.Map.listerThings.AllThings.OfType<Decoy>(), this).Any<Decoy>())
			{
				Decoy.mapsWithDecoys.Remove(base.Map);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00019420 File Offset: 0x00017620
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			DecoyOverlayUtility.DrawOverlay = true;
			this.pawn.Drawer.renderer.RenderPawnAt(drawLoc, new Rot4?(Rot4.South), false);
			DecoyOverlayUtility.DrawOverlay = false;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001944F File Offset: 0x0001764F
		[HarmonyPatch(typeof(DamageFlasher), "GetDamagedMat")]
		[HarmonyPrefix]
		private static void GetDuplicateMat(ref Material baseMat)
		{
			if (DecoyOverlayUtility.DrawOverlay)
			{
				baseMat = DecoyOverlayUtility.GetDuplicateMat(baseMat);
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00019464 File Offset: 0x00017664
		[HarmonyPatch(typeof(AttackTargetFinder), "BestAttackTarget")]
		[HarmonyPrefix]
		public static bool BestAttackTarget_Prefix(IAttackTargetSearcher searcher, ref IAttackTarget __result)
		{
			if (!Decoy.mapsWithDecoys.Contains(searcher.Thing.MapHeld))
			{
				return true;
			}
			List<Decoy> list = searcher.Thing.Map.attackTargetsCache.GetPotentialTargetsFor(searcher).OfType<Decoy>().ToList<Decoy>();
			if (GenList.NullOrEmpty<Decoy>(list))
			{
				return true;
			}
			__result = GenCollection.RandomElement<Decoy>(list);
			return false;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000194C0 File Offset: 0x000176C0
		[HarmonyPatch(typeof(JobGiver_AIFightEnemy), "UpdateEnemyTarget")]
		[HarmonyPrefix]
		public static void UpdateEnemyTarget_Prefix(Pawn pawn)
		{
			Thing enemyTarget = pawn.mindState.enemyTarget;
			if (enemyTarget != null && !(enemyTarget is Decoy) && Decoy.mapsWithDecoys.Contains(pawn.MapHeld))
			{
				pawn.mindState.enemyTarget = null;
			}
		}

		// Token: 0x040001CC RID: 460
		private static readonly HashSet<Map> mapsWithDecoys = new HashSet<Map>();

		// Token: 0x040001CD RID: 461
		private Pawn pawn;
	}
}
