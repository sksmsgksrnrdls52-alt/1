using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000100 RID: 256
	public class GameCondition_PsychicFlashstorm : GameCondition_Flashstorm
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00015FBF File Offset: 0x000141BF
		public int TicksBetweenStrikes
		{
			get
			{
				return base.Duration / this.numStrikes;
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00015FD0 File Offset: 0x000141D0
		private Vector3 RandomLocation()
		{
			return this.centerLocation.ToVector3() + Vector3Utility.RotatedBy(new Vector3(Vortex.Wrap(Mathf.Abs(Rand.Gaussian(0f, (float)base.AreaRadius)), (float)base.AreaRadius), 0f, 0f), Rand.Range(0f, 360f));
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00016034 File Offset: 0x00014234
		public unsafe override void GameConditionTick()
		{
			base.GameConditionTick();
			if (*GameCondition_PsychicFlashstorm.nextLightningTicksRef.Invoke(this) - Find.TickManager.TicksGame > this.TicksBetweenStrikes)
			{
				*GameCondition_PsychicFlashstorm.nextLightningTicksRef.Invoke(this) = this.TicksBetweenStrikes + Find.TickManager.TicksGame;
			}
			for (int i = 0; i < 2; i++)
			{
				FleckMaker.ThrowSmoke(this.RandomLocation(), base.SingleMap, 4f);
			}
		}

		// Token: 0x0600038F RID: 911 RVA: 0x000160A5 File Offset: 0x000142A5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numStrikes, "numStrikes", 0, false);
		}

		// Token: 0x040001AC RID: 428
		private static readonly AccessTools.FieldRef<GameCondition_Flashstorm, int> nextLightningTicksRef = AccessTools.FieldRefAccess<GameCondition_Flashstorm, int>("nextLightningTicks");

		// Token: 0x040001AD RID: 429
		public int numStrikes;
	}
}
