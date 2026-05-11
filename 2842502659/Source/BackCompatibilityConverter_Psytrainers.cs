using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200002B RID: 43
	public class BackCompatibilityConverter_Psytrainers : BackCompatibilityConverter
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00003C0A File Offset: 0x00001E0A
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return true;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003C10 File Offset: 0x00001E10
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defName == null || !typeof(ThingDef).IsAssignableFrom(defType))
			{
				return null;
			}
			if (defName.StartsWith(ThingDefGenerator_Neurotrainer.PsytrainerDefPrefix))
			{
				string text = defName.Replace(ThingDefGenerator_Neurotrainer.PsytrainerDefPrefix + "_", "");
				if (!text.StartsWith("VPE_"))
				{
					if (text.StartsWith("WordOf"))
					{
						text = text.Replace("WordOf", "Wordof");
					}
					string text2;
					if (!BackCompatibilityConverter_Psytrainers.specialCases.TryGetValue(text, out text2))
					{
						text2 = "VPE_" + text;
					}
					if (DefDatabase<AbilityDef>.GetNamedSilentFail(text2) != null)
					{
						return ThingDefGenerator_Neurotrainer.PsytrainerDefPrefix + "_" + text2;
					}
					Log.Warning(string.Concat(new string[]
					{
						"[VPE] Failed to find psycast for psytrainer called ",
						text2,
						" (old name: ",
						text,
						")"
					}));
					return ThingDefGenerator_Neurotrainer.PsytrainerDefPrefix + "_VPE_Flameball";
				}
			}
			return null;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003D02 File Offset: 0x00001F02
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003D05 File Offset: 0x00001F05
		public override void PostExposeData(object obj)
		{
		}

		// Token: 0x0400001B RID: 27
		private static readonly Dictionary<string, string> specialCases = new Dictionary<string, string>
		{
			{
				"BulletShield",
				"VPE_Skipshield"
			},
			{
				"EntropyDump",
				"VPE_NeuralHeatDump"
			}
		};
	}
}
