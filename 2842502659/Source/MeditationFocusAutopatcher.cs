using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200008C RID: 140
	[StaticConstructorOnStartup]
	internal class MeditationFocusAutopatcher
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00008DC4 File Offset: 0x00006FC4
		static MeditationFocusAutopatcher()
		{
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.thingClass != null && typeof(Building_ResearchBench).IsAssignableFrom(thingDef.thingClass))
				{
					ThingDef thingDef2 = thingDef;
					if (thingDef2.comps == null)
					{
						thingDef2.comps = new List<CompProperties>();
					}
					thingDef.comps.Add(new CompProperties_MeditationFocus
					{
						statDef = StatDefOf.MeditationFocusStrength,
						focusTypes = new List<MeditationFocusDef>
						{
							VPE_DefOf.VPE_Science
						},
						offsets = new List<FocusStrengthOffset>
						{
							new FocusStrengthOffset_ResearchSpeed
							{
								offset = 0.5f
							}
						}
					});
					thingDef2 = thingDef;
					if (thingDef2.statBases == null)
					{
						thingDef2.statBases = new List<StatModifier>();
					}
					thingDef.statBases.Add(new StatModifier
					{
						stat = StatDefOf.MeditationFocusStrength,
						value = 0f
					});
				}
				if (thingDef.techLevel == 7)
				{
					ThingDef thingDef2 = thingDef;
					if (thingDef2.comps == null)
					{
						thingDef2.comps = new List<CompProperties>();
					}
					thingDef.comps.Add(new CompProperties_MeditationFocus
					{
						statDef = StatDefOf.MeditationFocusStrength,
						focusTypes = new List<MeditationFocusDef>
						{
							VPE_DefOf.VPE_Archotech
						},
						offsets = new List<FocusStrengthOffset>
						{
							new FocusStrengthOffset_NearbyOfTechlevel
							{
								radius = 4.9f,
								techLevel = 7
							}
						}
					});
					thingDef2 = thingDef;
					if (thingDef2.statBases == null)
					{
						thingDef2.statBases = new List<StatModifier>();
					}
					thingDef.statBases.Add(new StatModifier
					{
						stat = StatDefOf.MeditationFocusStrength,
						value = 0f
					});
				}
			}
		}
	}
}
