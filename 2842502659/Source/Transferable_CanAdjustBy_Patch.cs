using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200004F RID: 79
	[HarmonyPatch(typeof(Transferable), "CanAdjustBy")]
	public static class Transferable_CanAdjustBy_Patch
	{
		// Token: 0x060000DE RID: 222 RVA: 0x000058E4 File Offset: 0x00003AE4
		public static void Postfix(Transferable __instance)
		{
			if (Transferable_CanAdjustBy_Patch.curTransferable != __instance && Find.WindowStack.IsOpen<Dialog_Trade>() && __instance.CountToTransferToDestination > 0 && TradeSession.trader != null && TradeSession.trader.Faction != Faction.OfEmpire && __instance.ThingDef.IsEltexOrHasEltexMaterial())
			{
				Transferable_CanAdjustBy_Patch.curTransferable = __instance;
				if (TradeSession.giftMode)
				{
					Messages.Message(Translator.Translate("VPE.GiftingEltexWarning"), MessageTypeDefOf.CautionInput, true);
					return;
				}
				Messages.Message(Translator.Translate("VPE.SellingEltexWarning"), MessageTypeDefOf.CautionInput, true);
			}
		}

		// Token: 0x0400003E RID: 62
		public static Transferable curTransferable;
	}
}
