using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using VanillaPsycastsExpanded.HarmonyPatches;
using Verse;

namespace VanillaPsycastsExpanded.Graphics
{
	// Token: 0x020000CD RID: 205
	public class HediffComp_MoteOverHead : HediffComp
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000FBA4 File Offset: 0x0000DDA4
		public HediffCompProperties_Mote Props
		{
			get
			{
				return this.props as HediffCompProperties_Mote;
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000FBB4 File Offset: 0x0000DDB4
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			Pawn pawn = base.Pawn;
			if (base.Pawn.Spawned)
			{
				bool humanlike = pawn.RaceProps.Humanlike;
				List<Vector3> headPosPerRotation = pawn.RaceProps.headPosPerRotation;
				Rot4 rot = (PawnUtility.GetPosture(pawn) != null) ? pawn.Drawer.renderer.LayingFacing() : (humanlike ? Rot4.North : pawn.Rotation);
				Vector3 vector;
				if (humanlike)
				{
					vector = Vector3Utility.RotatedBy(pawn.Drawer.renderer.BaseHeadOffsetAt(rot) + new Vector3(0f, 0f, 0.15f), pawn.Drawer.renderer.BodyAngle(0));
				}
				else
				{
					float bodySizeFactor = pawn.ageTracker.CurLifeStage.bodySizeFactor;
					Vector2 vector2 = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize * bodySizeFactor;
					vector = ((!GenList.NullOrEmpty<Vector3>(headPosPerRotation)) ? Vector2Utility.ScaledBy(headPosPerRotation[rot.AsInt], new Vector3(vector2.x, 1f, vector2.y)) : (HediffComp_MoteOverHead.animalHeadOffsets[rot.AsInt] * pawn.BodySize));
				}
				vector = pawn.DrawPos + vector;
				if (this.mote == null || this.mote.Destroyed)
				{
					this.mote = this.MakeStaticMote(vector, this.Props.mote, 1f);
					this.mote.Graphic.MatSingle.color = this.Props.color;
					return;
				}
				this.mote.exactPosition = vector;
				this.mote.Maintain();
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000FD69 File Offset: 0x0000DF69
		public Mote MakeStaticMote(Vector3 loc, ThingDef moteDef, float scale = 1f)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.exactPosition = loc;
			mote.Scale = scale;
			GenSpawn.Spawn(mote, base.Pawn.Position, base.Pawn.Map, 0);
			return mote;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000FDA3 File Offset: 0x0000DFA3
		public override void CompExposeData()
		{
			base.CompExposeData();
			if (this.mote != null && !this.mote.def.CanBeSaved())
			{
				return;
			}
			Scribe_References.Look<Mote>(ref this.mote, "mote", false);
		}

		// Token: 0x04000155 RID: 341
		private static readonly List<Vector3> animalHeadOffsets = new List<Vector3>
		{
			new Vector3(0f, 0f, 0.4f),
			new Vector3(0.4f, 0f, 0.25f),
			new Vector3(0f, 0f, 0.1f),
			new Vector3(-0.4f, 0f, 0.25f)
		};

		// Token: 0x04000156 RID: 342
		private Mote mote;
	}
}
