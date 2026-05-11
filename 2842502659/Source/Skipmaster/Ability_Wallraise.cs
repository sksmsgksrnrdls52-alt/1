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
	// Token: 0x02000110 RID: 272
	public class Ability_Wallraise : Ability
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00017B70 File Offset: 0x00015D70
		public AbilityExtension_Wallraise Props
		{
			get
			{
				return this.def.GetModExtension<AbilityExtension_Wallraise>();
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00017B80 File Offset: 0x00015D80
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			for (int i = 0; i < targets.Length; i++)
			{
				GlobalTargetInfo globalTargetInfo = targets[i];
				Map map = globalTargetInfo.Map;
				LocalTargetInfo target = globalTargetInfo.HasThing ? new LocalTargetInfo(globalTargetInfo.Thing) : new LocalTargetInfo(globalTargetInfo.Cell);
				List<Thing> list = new List<Thing>();
				list.AddRange(this.Props.AffectedCells(target, map).SelectMany((IntVec3 c) => from t in GridsUtility.GetThingList(c, map)
				where t.def.category == 2
				select t));
				foreach (Thing thing in list)
				{
					thing.DeSpawn(0);
				}
				foreach (IntVec3 intVec in this.Props.AffectedCells(target, map))
				{
					GenSpawn.Spawn(ThingDefOf.RaisedRocks, intVec, map, 0);
					FleckMaker.ThrowDustPuffThick(intVec.ToVector3Shifted(), map, Rand.Range(1.5f, 3f), CompAbilityEffect_Wallraise.DustColor);
				}
				foreach (Thing thing2 in list)
				{
					IntVec3 intVec2 = IntVec3.Invalid;
					for (int j = 0; j < 9; j++)
					{
						IntVec3 intVec3 = thing2.Position + GenRadial.RadialPattern[j];
						if (GenGrid.InBounds(intVec3, map) && GenGrid.Walkable(intVec3, map) && map.thingGrid.ThingsListAtFast(intVec3).Count <= 0)
						{
							intVec2 = intVec3;
							break;
						}
					}
					if (intVec2 != IntVec3.Invalid)
					{
						GenSpawn.Spawn(thing2, intVec2, map, 0);
					}
					else
					{
						GenPlace.TryPlaceThing(thing2, thing2.Position, map, 1, null, null, null, 1);
					}
				}
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00017DD4 File Offset: 0x00015FD4
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlight(target);
			GenDraw.DrawFieldEdges(this.Props.AffectedCells(target, this.pawn.Map).ToList<IntVec3>(), this.ValidateTarget(target, false) ? Color.white : Color.red, null, null, 2900);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00017E30 File Offset: 0x00016030
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = false)
		{
			if (this.Props.AffectedCells(target, this.pawn.Map).Any((IntVec3 c) => GridsUtility.Filled(c, this.pawn.Map)))
			{
				if (showMessages)
				{
					Messages.Message(TranslatorFormattedStringExtensions.Translate("AbilityOccupiedCells", this.def.LabelCap), target.ToTargetInfo(this.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (this.Props.AffectedCells(target, this.pawn.Map).Any((IntVec3 c) => !GenGrid.Standable(c, this.pawn.Map)))
			{
				if (showMessages)
				{
					Messages.Message(TranslatorFormattedStringExtensions.Translate("AbilityUnwalkable", this.def.LabelCap), target.ToTargetInfo(this.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}
	}
}
