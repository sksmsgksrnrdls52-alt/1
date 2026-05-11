using System;
using LudeonTK;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200008D RID: 141
	public static class MeditationUtilities
	{
		// Token: 0x060001A0 RID: 416 RVA: 0x00008FA8 File Offset: 0x000071A8
		[DebugAction("Pawns", "Check Meditation Focus Strength", true, false, false, false, false, 0, false, actionType = 2, allowedGameStates = 10)]
		public static void CheckStrength(Pawn pawn)
		{
			float num = StatExtension.GetStatValue(pawn, StatDefOf.MeditationFocusGain, true, -1);
			Log.Message(string.Format("Value: {0}, Explanation:\n{1}", num, StatDefOf.MeditationFocusGain.Worker.GetExplanationFull(StatRequest.For(pawn), 1, num)));
			LocalTargetInfo localTargetInfo = MeditationUtility.BestFocusAt(pawn.Position, pawn);
			if (!localTargetInfo.HasThing)
			{
				return;
			}
			num = StatExtension.GetStatValueForPawn(localTargetInfo.Thing, StatDefOf.MeditationFocusStrength, pawn, true);
			Log.Message(string.Format("Value: {0}, Explanation:\n{1}", num, StatDefOf.MeditationFocusStrength.Worker.GetExplanationFull(StatRequest.For(localTargetInfo.Thing, pawn), 1, num)));
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00009054 File Offset: 0x00007254
		public static bool CanUnlock(this MeditationFocusDef focus, Pawn pawn, out string reason)
		{
			Faction faction;
			if (focus == VPE_DefOf.Dignified && (pawn.royalty == null || !GenCollection.Any<RoyalTitle>(pawn.royalty.AllTitlesForReading) || !pawn.royalty.CanUpdateTitleOfAnyFaction(ref faction)))
			{
				reason = Translator.Translate("VPE.LockedTitle");
				return false;
			}
			MeditationFocusExtension modExtension = focus.GetModExtension<MeditationFocusExtension>();
			if (modExtension != null && !modExtension.canBeUnlocked)
			{
				reason = Translator.Translate("VPE.LockedLocked");
				return false;
			}
			reason = null;
			return true;
		}
	}
}
