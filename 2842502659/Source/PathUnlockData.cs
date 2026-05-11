using System;
using System.Xml;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000075 RID: 117
	public class PathUnlockData
	{
		// Token: 0x06000164 RID: 356 RVA: 0x000081B4 File Offset: 0x000063B4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured UnlockedPath: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "path", xmlRoot.Name, null, null, null);
			string[] array = xmlRoot.FirstChild.Value.Split(new char[]
			{
				'|'
			});
			this.unlockedAbilityLevelRange = ParseHelper.FromString<IntRange>(array[0]);
			this.unlockedAbilityCount = ParseHelper.FromString<IntRange>(array[1]);
		}

		// Token: 0x0400005B RID: 91
		public PsycasterPathDef path;

		// Token: 0x0400005C RID: 92
		public IntRange unlockedAbilityLevelRange = IntRange.One;

		// Token: 0x0400005D RID: 93
		public IntRange unlockedAbilityCount = IntRange.Zero;
	}
}
