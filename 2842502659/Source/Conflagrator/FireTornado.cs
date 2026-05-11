using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;
using Verse.Sound;

namespace VanillaPsycastsExpanded.Conflagrator
{
	// Token: 0x020000CA RID: 202
	[StaticConstructorOnStartup]
	[HarmonyPatch]
	public class FireTornado : ThingWithComps
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000EE08 File Offset: 0x0000D008
		private float FadeInOutFactor
		{
			get
			{
				float num = Mathf.Clamp01((float)(Find.TickManager.TicksGame - this.spawnTick) / 120f);
				float num2 = (this.leftFadeOutTicks < 0) ? 1f : Mathf.Min((float)this.leftFadeOutTicks / 120f, 1f);
				return Mathf.Min(num, num2);
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000EE60 File Offset: 0x0000D060
		[HarmonyPatch(typeof(WeatherBuildupUtility), "AddSnowRadial")]
		[HarmonyPrefix]
		public static void FixSnowUtility(ref float radius)
		{
			if (radius > GenRadial.MaxRadialPatternRadius)
			{
				radius = GenRadial.MaxRadialPatternRadius - 1f;
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000EE78 File Offset: 0x0000D078
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector2>(ref this.realPosition, "realPosition", default(Vector2), false);
			Scribe_Values.Look<float>(ref this.direction, "direction", 0f, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.leftFadeOutTicks, "leftFadeOutTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksLeftToDisappear, "ticksLeftToDisappear", 0, false);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000EEF4 File Offset: 0x0000D0F4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				Vector3 vector = base.Position.ToVector3Shifted();
				this.realPosition = new Vector2(vector.x, vector.z);
				this.direction = Rand.Range(0f, 360f);
				this.spawnTick = Find.TickManager.TicksGame;
				this.leftFadeOutTicks = -1;
			}
			this.CreateSustainer();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000EF64 File Offset: 0x0000D164
		public static void ThrowPuff(Vector3 loc, Map map, float scale, Color color)
		{
			if (!GenView.ShouldSpawnMotesAt(loc, map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FireTornado.FireTornadoPuff, 1.9f * scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			dataStatic.instanceColor = new Color?(color);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000EFE4 File Offset: 0x0000D1E4
		protected override void Tick()
		{
			if (base.Spawned)
			{
				if (this.sustainer == null)
				{
					Log.Error("Tornado sustainer is null.");
					this.CreateSustainer();
				}
				this.sustainer.Maintain();
				this.UpdateSustainerVolume();
				base.GetComp<CompWindSource>().wind = 5f * this.FadeInOutFactor;
				if (this.leftFadeOutTicks > 0)
				{
					this.leftFadeOutTicks--;
					if (this.leftFadeOutTicks == 0)
					{
						this.Destroy(0);
						return;
					}
				}
				else
				{
					if (FireTornado.directionNoise == null)
					{
						FireTornado.directionNoise = new Perlin(0.0020000000949949026, 2.0, 0.5, 4, 1948573612, 1);
					}
					this.direction += (float)FireTornado.directionNoise.GetValue((double)Find.TickManager.TicksAbs, (double)((float)(this.thingIDNumber % 500) * 1000f), 0.0) * 0.78f;
					this.realPosition = Vector2Utility.Moved(this.realPosition, this.direction, 0.028333334f);
					IntVec3 intVec = IntVec3Utility.ToIntVec3(new Vector3(this.realPosition.x, 0f, this.realPosition.y));
					if (GenGrid.InBounds(intVec, base.Map))
					{
						base.Position = intVec;
						if (Gen.IsHashIntervalTick(this, 15))
						{
							this.DoFire();
						}
						if (Gen.IsHashIntervalTick(this, 60))
						{
							this.SpawnChemfuel();
						}
						if (this.ticksLeftToDisappear > 0)
						{
							this.ticksLeftToDisappear--;
							if (this.ticksLeftToDisappear == 0)
							{
								this.leftFadeOutTicks = 120;
								Messages.Message(Translator.Translate("MessageTornadoDissipated"), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent, true);
							}
						}
						if (Gen.IsHashIntervalTick(this, 4) && !this.CellImmuneToDamage(base.Position))
						{
							float num = Rand.Range(0.6f, 1f);
							Vector3 vector;
							vector..ctor(this.realPosition.x, 0f, this.realPosition.y);
							vector.y = Altitudes.AltitudeFor(28);
							FireTornado.ThrowPuff(vector + Vector3Utility.RandomHorizontalOffset(1.5f), base.Map, Rand.Range(1.5f, 3f), new Color(num, num, num));
							return;
						}
					}
					else
					{
						this.leftFadeOutTicks = 120;
						Messages.Message(Translator.Translate("MessageTornadoLeftMap"), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent, true);
					}
				}
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000F27C File Offset: 0x0000D47C
		private void DoFire()
		{
			foreach (IntVec3 intVec in GenCollection.InRandomOrder<IntVec3>(from c in GenRadial.RadialCellsAround(base.Position, 4.2f, true)
			where GenGrid.InBounds(c, base.Map) && !this.CellImmuneToDamage(c)
			select c, null).Take(Rand.Range(3, 5)))
			{
				Fire firstThing = GridsUtility.GetFirstThing<Fire>(intVec, base.Map);
				if (firstThing == null)
				{
					FireUtility.TryStartFireIn(intVec, base.Map, 15f, this, null);
				}
				else
				{
					firstThing.fireSize += 1f;
				}
			}
			foreach (Pawn pawn in GenRadial.RadialDistinctThingsAround(base.Position, base.Map, 4.2f, true).OfType<Pawn>())
			{
				Fire fire = (Fire)AttachmentUtility.GetAttachment(pawn, ThingDefOf.Fire);
				if (fire == null)
				{
					FireUtility.TryAttachFire(pawn, 15f, this);
				}
				else
				{
					fire.fireSize += 1f;
				}
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000F3B0 File Offset: 0x0000D5B0
		private void SpawnChemfuel()
		{
			foreach (IntVec3 intVec in GenCollection.InRandomOrder<IntVec3>(from c in GenRadial.RadialCellsAround(base.Position, 4.2f, true)
			where GenGrid.InBounds(c, base.Map) && FilthMaker.CanMakeFilth(c, base.Map, ThingDefOf.Filth_Fuel, 0) && !this.CellImmuneToDamage(c)
			select c, null).Take(Rand.Range(1, 3)))
			{
				FilthMaker.TryMakeFilth(intVec, base.Map, ThingDefOf.Filth_Fuel, 1, 0, true);
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000F438 File Offset: 0x0000D638
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			Rand.PushState();
			Rand.Seed = this.thingIDNumber;
			for (int i = 0; i < 90; i++)
			{
				this.DrawTornadoPart(FireTornado.PartsDistanceFromCenter.RandomInRange, Rand.Range(0f, 360f), Rand.Range(0.9f, 1.1f), Rand.Range(0.52f, 0.88f));
			}
			Rand.PopState();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000F4A8 File Offset: 0x0000D6A8
		private void DrawTornadoPart(float distanceFromCenter, float initialAngle, float speedMultiplier, float colorMultiplier)
		{
			int ticksGame = Find.TickManager.TicksGame;
			float num = 1f / distanceFromCenter;
			float num2 = 25f * speedMultiplier * num;
			float num3 = (initialAngle + (float)ticksGame * num2) % 360f;
			Vector2 vector = Vector2Utility.Moved(this.realPosition, num3, FireTornado.AdjustedDistanceFromCenter(distanceFromCenter));
			vector.y += distanceFromCenter * 4f;
			vector.y += FireTornado.ZOffsetBias;
			Vector3 vector2 = new Vector3(vector.x, Altitudes.AltitudeFor(31) + 0.04054054f * Rand.Range(0f, 1f), vector.y);
			float num4 = distanceFromCenter * 3f;
			float num5 = 1f;
			if (num3 > 270f)
			{
				num5 = GenMath.LerpDouble(270f, 360f, 0f, 1f, num3);
			}
			else if (num3 > 180f)
			{
				num5 = GenMath.LerpDouble(180f, 270f, 1f, 0f, num3);
			}
			float num6 = Mathf.Min(distanceFromCenter / (FireTornado.PartsDistanceFromCenter.max + 2f), 1f);
			float num7 = Mathf.InverseLerp(0.18f, 0.4f, num6);
			Vector3 vector3;
			vector3..ctor(Mathf.Sin((float)ticksGame / 1000f + (float)(this.thingIDNumber * 10)) * 2f, 0f, 0f);
			Vector3 vector4 = vector2 + vector3 * num7;
			float num8 = Mathf.Max(1f - num6, 0f) * num5 * this.FadeInOutFactor;
			Color color;
			color..ctor(colorMultiplier, colorMultiplier, colorMultiplier, num8);
			FireTornado.matPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Matrix4x4 matrix4x = Matrix4x4.TRS(vector4, Quaternion.Euler(0f, num3, 0f), new Vector3(num4, 1f, num4));
			Graphics.DrawMesh(MeshPool.plane10, matrix4x, FireTornado.TornadoMaterial, 0, null, 0, FireTornado.matPropertyBlock);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000F68C File Offset: 0x0000D88C
		private static float AdjustedDistanceFromCenter(float distanceFromCenter)
		{
			float num = Mathf.Min(distanceFromCenter / 8f, 1f);
			num *= num;
			return distanceFromCenter * num;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000F6B2 File Offset: 0x0000D8B2
		private void UpdateSustainerVolume()
		{
			this.sustainer.info.volumeFactor = this.FadeInOutFactor;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000F6CA File Offset: 0x0000D8CA
		private void CreateSustainer()
		{
			LongEventHandler.ExecuteWhenFinished(delegate()
			{
				SoundDef tornado = SoundDefOf.Tornado;
				this.sustainer = SoundStarter.TrySpawnSustainer(tornado, SoundInfo.InMap(this, 1));
				this.UpdateSustainerVolume();
			});
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
		private bool CellImmuneToDamage(IntVec3 c)
		{
			if (GridsUtility.Roofed(c, base.Map) && GridsUtility.GetRoof(c, base.Map).isThickRoof)
			{
				return true;
			}
			Building edifice = GridsUtility.GetEdifice(c, base.Map);
			return edifice != null && edifice.def.category == 3 && (edifice.def.building.isNaturalRock || (edifice.def == ThingDefOf.Wall && edifice.Faction == null));
		}

		// Token: 0x04000149 RID: 329
		private static readonly MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400014A RID: 330
		private static readonly Material TornadoMaterial = MaterialPool.MatFrom("Effects/Conflagrator/FireTornado/FireTornadoFat", ShaderDatabase.MoteGlow, MapMaterialRenderQueues.Tornado);

		// Token: 0x0400014B RID: 331
		private static readonly FloatRange PartsDistanceFromCenter = new FloatRange(1f, 5f);

		// Token: 0x0400014C RID: 332
		private static readonly float ZOffsetBias = -4f * FireTornado.PartsDistanceFromCenter.min;

		// Token: 0x0400014D RID: 333
		private static readonly FleckDef FireTornadoPuff = DefDatabase<FleckDef>.GetNamed("VPE_FireTornadoDustPuff", true);

		// Token: 0x0400014E RID: 334
		private static ModuleBase directionNoise;

		// Token: 0x0400014F RID: 335
		public int ticksLeftToDisappear = -1;

		// Token: 0x04000150 RID: 336
		private float direction;

		// Token: 0x04000151 RID: 337
		private int leftFadeOutTicks = -1;

		// Token: 0x04000152 RID: 338
		private Vector2 realPosition;

		// Token: 0x04000153 RID: 339
		private int spawnTick;

		// Token: 0x04000154 RID: 340
		private Sustainer sustainer;
	}
}
