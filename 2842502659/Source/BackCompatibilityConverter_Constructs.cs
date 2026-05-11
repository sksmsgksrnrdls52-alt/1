using System;
using System.Xml;
using VanillaPsycastsExpanded.Technomancer;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200002C RID: 44
	public class BackCompatibilityConverter_Constructs : BackCompatibilityConverter
	{
		// Token: 0x06000073 RID: 115 RVA: 0x00003D3B File Offset: 0x00001F3B
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return true;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003D3E File Offset: 0x00001F3E
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			return null;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003D44 File Offset: 0x00001F44
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			bool flag = baseType == typeof(Thing) && providedClassName == GenTypes.GetTypeNameWithoutIgnoredNamespaces(typeof(Pawn));
			if (flag)
			{
				string innerText = node["def"].InnerText;
				bool flag2 = innerText == "VPE_SteelConstruct" || innerText == "VPE_RockConstruct";
				flag = flag2;
			}
			if (flag)
			{
				return typeof(Pawn_Construct);
			}
			return null;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003DC1 File Offset: 0x00001FC1
		public override void PostExposeData(object obj)
		{
		}
	}
}
