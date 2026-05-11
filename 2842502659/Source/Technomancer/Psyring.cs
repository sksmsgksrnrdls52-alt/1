using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F1 RID: 241
	[HarmonyPatch]
	public class Psyring : Apparel
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600034C RID: 844 RVA: 0x000148C3 File Offset: 0x00012AC3
		public AbilityDef Ability
		{
			get
			{
				return this.ability;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600034D RID: 845 RVA: 0x000148CB File Offset: 0x00012ACB
		public bool Added
		{
			get
			{
				return !this.alreadyHad;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600034E RID: 846 RVA: 0x000148D6 File Offset: 0x00012AD6
		public PsycasterPathDef Path
		{
			get
			{
				return this.ability.Psycast().path;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600034F RID: 847 RVA: 0x000148E8 File Offset: 0x00012AE8
		public override string Label
		{
			get
			{
				return base.Label + " (" + this.ability.LabelCap + ")";
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00014919 File Offset: 0x00012B19
		public void Init(AbilityDef ability)
		{
			this.ability = ability;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00014922 File Offset: 0x00012B22
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<AbilityDef>(ref this.ability, "ability");
			Scribe_Values.Look<bool>(ref this.alreadyHad, "alreadyHad", false, false);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0001494C File Offset: 0x00012B4C
		public override void Notify_Equipped(Pawn pawn)
		{
			base.Notify_Equipped(pawn);
			if (this.ability == null)
			{
				Log.Warning("[VPE] Psyring present with no ability, destroying.");
				this.Destroy(0);
				return;
			}
			CompAbilities comp = pawn.GetComp<CompAbilities>();
			if (comp == null)
			{
				return;
			}
			this.alreadyHad = comp.HasAbility(this.ability);
			if (!this.alreadyHad)
			{
				comp.GiveAbility(this.ability);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000149AB File Offset: 0x00012BAB
		public override void Notify_Unequipped(Pawn pawn)
		{
			base.Notify_Unequipped(pawn);
			if (this.ability == null)
			{
				return;
			}
			if (!this.alreadyHad)
			{
				pawn.GetComp<CompAbilities>().LearnedAbilities.RemoveAll((Ability ab) => ab.def == this.ability);
			}
			this.alreadyHad = false;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000149EC File Offset: 0x00012BEC
		[HarmonyPatch(typeof(FloatMenuOptionProvider_Wear), "GetSingleOptionFor")]
		[HarmonyPostfix]
		public static void EquipConditions(Thing clickedThing, FloatMenuContext context, ref FloatMenuOption __result)
		{
			if (__result == null)
			{
				return;
			}
			Pawn firstSelectedPawn = context.FirstSelectedPawn;
			if (firstSelectedPawn.apparel == null)
			{
				return;
			}
			Psyring psyring = clickedThing as Psyring;
			if (psyring == null)
			{
				return;
			}
			if (__result.Label.Contains(TranslatorFormattedStringExtensions.Translate("ForceWear", psyring.LabelShort, psyring)))
			{
				if (firstSelectedPawn.Psycasts() == null)
				{
					__result = new FloatMenuOption(string.Format("{0} ({1})", TranslatorFormattedStringExtensions.Translate("CannotWear", psyring.LabelShort, psyring), Translator.Translate("VPE.NotPsycaster")), null, 4, null, null, 0f, null, null, true, 0);
				}
				if (firstSelectedPawn.apparel.WornApparel.OfType<Psyring>().Any<Psyring>())
				{
					__result = new FloatMenuOption(string.Format("{0} ({1})", TranslatorFormattedStringExtensions.Translate("CannotWear", psyring.LabelShort, psyring), Translator.Translate("VPE.AlreadyPsyring")), null, 4, null, null, 0f, null, null, true, 0);
				}
			}
		}

		// Token: 0x04000198 RID: 408
		private AbilityDef ability;

		// Token: 0x04000199 RID: 409
		private bool alreadyHad;
	}
}
