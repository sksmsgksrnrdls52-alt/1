using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200005E RID: 94
	public class Ability_GoodwillImpact : Ability
	{
		// Token: 0x06000111 RID: 273 RVA: 0x00006888 File Offset: 0x00004A88
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn != null && (GenHostility.HostileTo(pawn, this.pawn) || pawn.Faction == this.pawn.Faction || pawn.Faction == null))
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.MustBeAllyOrNeutral"), pawn, MessageTypeDefOf.CautionInput, true);
				}
				return false;
			}
			return base.ValidateTarget(target, showMessages);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000068FC File Offset: 0x00004AFC
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Thing thing = globalTargetInfo.Thing as Pawn;
				int num = (int)Mathf.Max(10f, StatExtension.GetStatValue(this.pawn, StatDefOf.PsychicSensitivity, true, -1) * 100f - 100f);
				thing.Faction.TryAffectGoodwillWith(this.pawn.Faction, num, true, true, null, null);
			}
		}
	}
}
