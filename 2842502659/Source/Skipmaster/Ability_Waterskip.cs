using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Skipmaster
{
	// Token: 0x02000115 RID: 277
	public class Ability_Waterskip : Ability
	{
		// Token: 0x060003ED RID: 1005 RVA: 0x000184FC File Offset: 0x000166FC
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			Map map = targets[0].Map;
			foreach (IntVec3 intVec in this.AffectedCells(targets[0].Cell, map))
			{
				List<Thing> thingList = GridsUtility.GetThingList(intVec, map);
				for (int i = thingList.Count - 1; i >= 0; i--)
				{
					Thing thing = thingList[i];
					if (!(thing is Filth) && !(thing is Fire))
					{
						ThingWithComps thingWithComps = thing as ThingWithComps;
						if (thingWithComps != null)
						{
							if (ThingCompUtility.TryGetComp<CompPower>(thingWithComps) != null)
							{
								CompBreakdownable compBreakdownable = ThingCompUtility.TryGetComp<CompBreakdownable>(thingWithComps);
								if (compBreakdownable != null)
								{
									compBreakdownable.DoBreakdown();
								}
								CompFlickable compFlickable = ThingCompUtility.TryGetComp<CompFlickable>(thingWithComps);
								if (compFlickable != null)
								{
									compFlickable.SwitchIsOn = false;
								}
								if (ThingCompUtility.TryGetComp<CompProjectileInterceptor>(thingWithComps) != null || thingWithComps is Building_Turret)
								{
									thingWithComps.TakeDamage(new DamageInfo(DamageDefOf.EMP, 10f, 10f, -1f, this.pawn, null, null, 0, null, true, true, 2, true, false));
								}
							}
							else
							{
								Pawn pawn = thing as Pawn;
								if (pawn != null)
								{
									HediffComp_Invisibility invisibilityComp = InvisibilityUtility.GetInvisibilityComp(pawn);
									if (invisibilityComp != null)
									{
										invisibilityComp.DisruptInvisibility();
									}
								}
							}
						}
					}
					else
					{
						thingList[i].Destroy(0);
					}
				}
				if (!GridsUtility.Filled(intVec, map))
				{
					FilthMaker.TryMakeFilth(intVec, map, ThingDefOf.Filth_Water, 1, 0, true);
				}
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(intVec.ToVector3Shifted(), map, FleckDefOf.WaterskipSplashParticles, 1f);
				dataStatic.rotationRate = (float)Rand.Range(-30, 30);
				dataStatic.rotation = (float)(90 * Rand.RangeInclusive(0, 3));
				map.flecks.CreateFleck(dataStatic);
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x000186DC File Offset: 0x000168DC
		private IEnumerable<IntVec3> AffectedCells(IntVec3 cell, Map map)
		{
			Ability_Waterskip.<AffectedCells>d__1 <AffectedCells>d__ = new Ability_Waterskip.<AffectedCells>d__1(-2);
			<AffectedCells>d__.<>4__this = this;
			<AffectedCells>d__.<>3__cell = cell;
			<AffectedCells>d__.<>3__map = map;
			return <AffectedCells>d__;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x000186FC File Offset: 0x000168FC
		public override void DrawHighlight(LocalTargetInfo target)
		{
			float rangeForPawn = this.GetRangeForPawn();
			if (GenRadial.MaxRadialPatternRadius > rangeForPawn && rangeForPawn >= 1f)
			{
				GenDraw.DrawRadiusRing(this.pawn.Position, rangeForPawn, Color.cyan, null);
			}
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
				GenDraw.DrawFieldEdges(this.AffectedCells(target.Cell, this.pawn.Map).ToList<IntVec3>(), this.ValidateTarget(target, false) ? Color.white : Color.red, null, null, 2900);
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00018790 File Offset: 0x00016990
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (GridsUtility.Filled(target.Cell, this.pawn.Map))
			{
				if (showMessages)
				{
					Messages.Message(TranslatorFormattedStringExtensions.Translate("AbilityOccupiedCells", this.def.LabelCap), target.ToTargetInfo(this.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return base.ValidateTarget(target, showMessages);
		}
	}
}
