using System;
using AlienRace;
using UnityEngine;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200002F RID: 47
	[StaticConstructorOnStartup]
	public static class ModCompatibility
	{
		// Token: 0x06000084 RID: 132 RVA: 0x000041A8 File Offset: 0x000023A8
		public static Color GetSkinColorFirst(Pawn pawn)
		{
			AlienPartGenerator.AlienComp alienComp = ThingCompUtility.TryGetComp<AlienPartGenerator.AlienComp>(pawn);
			if (alienComp != null)
			{
				return alienComp.GetChannel("skin").first;
			}
			return Color.white;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000041D8 File Offset: 0x000023D8
		public static Color GetSkinColorSecond(Pawn pawn)
		{
			AlienPartGenerator.AlienComp alienComp = ThingCompUtility.TryGetComp<AlienPartGenerator.AlienComp>(pawn);
			if (alienComp != null)
			{
				return alienComp.GetChannel("skin").second;
			}
			return Color.white;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004208 File Offset: 0x00002408
		public static void SetSkinColorFirst(Pawn pawn, Color color)
		{
			AlienPartGenerator.AlienComp alienComp = ThingCompUtility.TryGetComp<AlienPartGenerator.AlienComp>(pawn);
			if (alienComp != null)
			{
				alienComp.OverwriteColorChannel("skin", new Color?(color), null);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000423C File Offset: 0x0000243C
		public static void SetSkinColorSecond(Pawn pawn, Color color)
		{
			AlienPartGenerator.AlienComp alienComp = ThingCompUtility.TryGetComp<AlienPartGenerator.AlienComp>(pawn);
			if (alienComp != null)
			{
				alienComp.OverwriteColorChannel("skin", null, new Color?(color));
			}
		}

		// Token: 0x04000020 RID: 32
		public static bool AlienRacesIsActive = ModsConfig.IsActive("erdelf.HumanoidAlienRaces") || ModsConfig.IsActive("erdelf.HumanoidAlienRaces_steam");
	}
}
