using System;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Harmonist
{
	// Token: 0x02000129 RID: 297
	public class Ability_RandomEvent : Ability
	{
		// Token: 0x06000446 RID: 1094 RVA: 0x00019E50 File Offset: 0x00018050
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Ability_RandomEvent.DoRandomEvent(globalTargetInfo.Map);
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00019E88 File Offset: 0x00018088
		public static void DoRandomEvent(Map map)
		{
			int num = 0;
			do
			{
				try
				{
					IncidentDef incidentDef = GenCollection.RandomElement<IncidentDef>(DefDatabase<IncidentDef>.AllDefs);
					if (incidentDef.Worker.TryExecute(StorytellerUtility.DefaultParmsNow(incidentDef.category, map)))
					{
						return;
					}
				}
				catch (Exception)
				{
				}
				num++;
			}
			while (num <= 1000);
			Log.Error("[VPE] Exceeded 1000 tries to spawn random event");
		}
	}
}
