using System;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000C6 RID: 198
	[DefOf]
	public static class VPE_DefOf
	{
		// Token: 0x06000298 RID: 664 RVA: 0x0000EC7E File Offset: 0x0000CE7E
		static VPE_DefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(VPE_DefOf));
		}

		// Token: 0x040000E0 RID: 224
		public static DamageDef VPE_Rot;

		// Token: 0x040000E1 RID: 225
		public static HediffDef VPE_PsycastAbilityImplant;

		// Token: 0x040000E2 RID: 226
		public static HediffDef VPE_Recharge;

		// Token: 0x040000E3 RID: 227
		public static HediffDef VPE_Vortex;

		// Token: 0x040000E4 RID: 228
		public static HediffDef PsychicComa;

		// Token: 0x040000E5 RID: 229
		public static HediffDef VPE_BlockBleeding;

		// Token: 0x040000E6 RID: 230
		public static HediffDef VPE_ControlledFrenzy;

		// Token: 0x040000E7 RID: 231
		public static HediffDef VPE_Regenerating;

		// Token: 0x040000E8 RID: 232
		public static HediffDef VPE_GuardianSkipBarrier;

		// Token: 0x040000E9 RID: 233
		public static HediffDef HeartAttack;

		// Token: 0x040000EA RID: 234
		public static HediffDef VFEP_HypothermicSlowdown;

		// Token: 0x040000EB RID: 235
		public static HediffDef VPE_IceShield;

		// Token: 0x040000EC RID: 236
		public static HediffDef VPE_FrostRay;

		// Token: 0x040000ED RID: 237
		public static HediffDef VPE_IceBlock;

		// Token: 0x040000EE RID: 238
		public static HediffDef VPE_Blizzard;

		// Token: 0x040000EF RID: 239
		public static HediffDef VPE_InfinitePower;

		// Token: 0x040000F0 RID: 240
		public static HediffDef VPE_Lucky;

		// Token: 0x040000F1 RID: 241
		public static HediffDef VPE_UnLucky;

		// Token: 0x040000F2 RID: 242
		public static HediffDef VPE_Essence;

		// Token: 0x040000F3 RID: 243
		public static HediffDef VPE_Darkvision_Display;

		// Token: 0x040000F4 RID: 244
		public static HediffDef VPE_PsychicSoothe;

		// Token: 0x040000F5 RID: 245
		public static HediffDef VPE_GroupLink;

		// Token: 0x040000F6 RID: 246
		public static HediffDef VPE_Hallucination;

		// Token: 0x040000F7 RID: 247
		public static HediffDef VPE_GainedVitality;

		// Token: 0x040000F8 RID: 248
		public static HediffDef VPE_BodiesConsumed;

		// Token: 0x040000F9 RID: 249
		public static HediffDef VPE_DeathShield;

		// Token: 0x040000FA RID: 250
		public static HediffDef TraumaSavant;

		// Token: 0x040000FB RID: 251
		public static HediffDef HypothermicSlowdown;

		// Token: 0x040000FC RID: 252
		public static HediffDef VPE_Sacrificed;

		// Token: 0x040000FD RID: 253
		public static ThingDef VPE_ChainBolt;

		// Token: 0x040000FE RID: 254
		public static ThingDef VPE_Bolt;

		// Token: 0x040000FF RID: 255
		public static ThingDef VPE_Mote_FireBeam;

		// Token: 0x04000100 RID: 256
		public static ThingDef VPE_HurricaneMaker;

		// Token: 0x04000101 RID: 257
		public static ThingDef VPE_Skipdoor;

		// Token: 0x04000102 RID: 258
		public static ThingDef VPE_Mote_GreenMist;

		// Token: 0x04000103 RID: 259
		public static ThingDef VPE_JumpingPawn;

		// Token: 0x04000104 RID: 260
		public static ThingDef VPE_TimeSphere;

		// Token: 0x04000105 RID: 261
		public static ThingDef VPE_SkyChanger;

		// Token: 0x04000106 RID: 262
		public static ThingDef VPE_PsycastAreaEffectMaintained;

		// Token: 0x04000107 RID: 263
		public static ThingDef VPE_HeatPearls;

		// Token: 0x04000108 RID: 264
		public static ThingDef VPE_Eltex;

		// Token: 0x04000109 RID: 265
		public static ThingDef VPE_EltexOre;

		// Token: 0x0400010A RID: 266
		public static ThingDef VPE_PsycastPsychicEffectTransfer;

		// Token: 0x0400010B RID: 267
		public static ThingDef VPE_Mote_Cast;

		// Token: 0x0400010C RID: 268
		public static ThingDef VPE_Psyring;

		// Token: 0x0400010D RID: 269
		public static ThingDef Plant_Brambles;

		// Token: 0x0400010E RID: 270
		public static ThingDef VPE_Shrineshield_Small;

		// Token: 0x0400010F RID: 271
		public static ThingDef VPE_Shrineshield_Large;

		// Token: 0x04000110 RID: 272
		public static ThingDef VPE_Mote_ParalysisPulse;

		// Token: 0x04000111 RID: 273
		public static ThingDef VPE_SoulOrbTransfer;

		// Token: 0x04000112 RID: 274
		public static ThingDef VPE_SoulFromSky;

		// Token: 0x04000113 RID: 275
		[MayRequireBiotech]
		public static ThingDef MechanoidTransponder;

		// Token: 0x04000114 RID: 276
		public static TraitDef VPE_Thrall;

		// Token: 0x04000115 RID: 277
		public static SoundDef VPE_Recharge_Sustainer;

		// Token: 0x04000116 RID: 278
		public static SoundDef VPE_BallLightning_Zap;

		// Token: 0x04000117 RID: 279
		public static SoundDef VPE_Vortex_Sustainer;

		// Token: 0x04000118 RID: 280
		public static SoundDef VPE_RaidPause_Sustainer;

		// Token: 0x04000119 RID: 281
		public static SoundDef VPE_GuardianSkipbarrier_Sustainer;

		// Token: 0x0400011A RID: 282
		public static SoundDef VPE_PowerLeap_Land;

		// Token: 0x0400011B RID: 283
		public static SoundDef VPE_Killskip_Jump_01a;

		// Token: 0x0400011C RID: 284
		public static SoundDef VPE_Killskip_Jump_01b;

		// Token: 0x0400011D RID: 285
		public static SoundDef VPE_Killskip_Jump_01c;

		// Token: 0x0400011E RID: 286
		public static SoundDef VPE_TimeSphere_Sustainer;

		// Token: 0x0400011F RID: 287
		public static SoundDef Psycast_Neuroquake_CastLoop;

		// Token: 0x04000120 RID: 288
		public static SoundDef Psycast_Neuroquake_CastEnd;

		// Token: 0x04000121 RID: 289
		public static SoundDef VPE_FrostRay_Sustainer;

		// Token: 0x04000122 RID: 290
		public static SoundDef VPE_Assassinate_Return;

		// Token: 0x04000123 RID: 291
		public static FleckDef VPE_VortexSpark;

		// Token: 0x04000124 RID: 292
		public static FleckDef VPE_WarlordZap;

		// Token: 0x04000125 RID: 293
		public static FleckDef VPE_AggresiveHeatDump;

		// Token: 0x04000126 RID: 294
		public static FleckDef PsycastAreaEffect;

		// Token: 0x04000127 RID: 295
		public static FleckDef VPE_PsycastSkipFlashEntry_DarkBlue;

		// Token: 0x04000128 RID: 296
		public static FleckDef VPE_Slash;

		// Token: 0x04000129 RID: 297
		public static StatDef VPE_PsyfocusCostFactor;

		// Token: 0x0400012A RID: 298
		public static StatDef VPE_PsychicEntropyMinimum;

		// Token: 0x0400012B RID: 299
		public static JobDef VPE_StandFreeze;

		// Token: 0x0400012C RID: 300
		public static JobDef VPE_EssenceTransfer;

		// Token: 0x0400012D RID: 301
		public static EffecterDef VPE_Haywire;

		// Token: 0x0400012E RID: 302
		public static EffecterDef VPE_Liferot;

		// Token: 0x0400012F RID: 303
		public static EffecterDef Interceptor_BlockedProjectilePsychic;

		// Token: 0x04000130 RID: 304
		public static EffecterDef VPE_Skip_ExitNoDelayRed;

		// Token: 0x04000131 RID: 305
		public static MeditationFocusDef VPE_Archotech;

		// Token: 0x04000132 RID: 306
		public static MeditationFocusDef VPE_Science;

		// Token: 0x04000133 RID: 307
		public static MentalStateDef VPE_Wander_Sad;

		// Token: 0x04000134 RID: 308
		public static MentalStateDef VPE_ManhunterTerritorial;

		// Token: 0x04000135 RID: 309
		public static HistoryEventDef VPE_Foretelling;

		// Token: 0x04000136 RID: 310
		public static HistoryEventDef VPE_GiftedEltex;

		// Token: 0x04000137 RID: 311
		public static HistoryEventDef VPE_SoldEltex;

		// Token: 0x04000138 RID: 312
		public static PawnKindDef VPE_RockConstruct;

		// Token: 0x04000139 RID: 313
		public static PawnKindDef VPE_SteelConstruct;

		// Token: 0x0400013A RID: 314
		public static ThoughtDef EnvironmentDark;

		// Token: 0x0400013B RID: 315
		public static ThoughtDef VPE_Future;

		// Token: 0x0400013C RID: 316
		public static GameConditionDef VPE_PsychicFlashstorm;

		// Token: 0x0400013D RID: 317
		public static GameConditionDef VPE_TimeQuake;

		// Token: 0x0400013E RID: 318
		public static PawnKindDef VPE_SummonedSkeleton;

		// Token: 0x0400013F RID: 319
		public static BodyPartDef Finger;

		// Token: 0x04000140 RID: 320
		[DefAlias("VPE_Hurricane")]
		public static WeatherDef VPE_Hurricane_Weather;

		// Token: 0x04000141 RID: 321
		[DefAlias("VPE_Hurricane")]
		public static GameConditionDef VPE_Hurricane_Condition;

		// Token: 0x04000142 RID: 322
		[DefAlias("VPE_RockConstruct")]
		public static ThingDef VPE_Race_RockConstruct;

		// Token: 0x04000143 RID: 323
		[DefAlias("VPE_SteelConstruct")]
		public static ThingDef VPE_Race_SteelConstruct;

		// Token: 0x04000144 RID: 324
		public static StorytellerDef VPE_Basilicus;

		// Token: 0x04000145 RID: 325
		public static MeditationFocusDef Dignified;

		// Token: 0x04000146 RID: 326
		public static NeedDef Joy;

		// Token: 0x04000147 RID: 327
		public static BodyPartDef Brain;
	}
}
