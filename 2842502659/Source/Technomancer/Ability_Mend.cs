using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000EE RID: 238
	public class Ability_Mend : Ability
	{
		// Token: 0x06000342 RID: 834 RVA: 0x00014388 File Offset: 0x00012588
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				Pawn pawn = globalTargetInfo.Thing as Pawn;
				if (pawn != null)
				{
					if (pawn.RaceProps.Humanlike)
					{
						float amountTotal = this.GetPowerForPawn();
						List<ThingWithComps> toHeal = (from t in pawn.equipment.AllEquipmentListForReading.Concat(pawn.apparel.WornApparel)
						where t.def.useHitPoints && t.HitPoints < t.MaxHitPoints
						select t).ToList<ThingWithComps>();
						int num = (int)amountTotal * toHeal.Count;
						int num2 = 0;
						while (amountTotal >= 1f && toHeal.Count > 0 && num2++ <= num)
						{
							toHeal.RemoveAll(delegate(ThingWithComps t)
							{
								amountTotal -= Ability_Mend.Mend(t, (amountTotal >= (float)toHeal.Count) ? (amountTotal / (float)toHeal.Count) : amountTotal);
								return t.HitPoints == t.MaxHitPoints;
							});
						}
						if (num2 >= num)
						{
							Log.Warning(string.Format("[VPE] Too many iterations in Ability_Mend.Cast by {0} on {1}", this.pawn, pawn));
						}
					}
					else if (pawn.RaceProps.IsMechanoid)
					{
						float amountTotal = this.GetPowerForPawn();
						List<Hediff_Injury> toHeal = (from h in pawn.health.hediffSet.hediffs.OfType<Hediff_Injury>()
						where !HediffUtility.IsPermanent(h)
						select h).ToList<Hediff_Injury>();
						int num3 = (int)amountTotal * toHeal.Count;
						int num4 = 0;
						while (amountTotal >= 0f && toHeal.Count > 0 && num4++ <= num3)
						{
							toHeal.RemoveAll(delegate(Hediff_Injury injury)
							{
								float num5 = Mathf.Clamp((amountTotal >= 1f) ? (amountTotal / (float)toHeal.Count) : amountTotal, 0f, injury.Severity);
								injury.Heal(num5);
								amountTotal -= num5;
								return injury.Severity == 0f;
							});
						}
						if (num4 >= num3)
						{
							Log.Warning(string.Format("[VPE] Too many iterations in Ability_Mend.Cast by {0} on {1}", this.pawn, pawn));
						}
						if (toHeal.Count == 0)
						{
							MechRepairUtility.RepairTick(pawn, 1);
						}
					}
				}
				else
				{
					Ability_Mend.Mend(globalTargetInfo.Thing, this.GetPowerForPawn());
				}
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x000145CB File Offset: 0x000127CB
		public override float GetPowerForPawn()
		{
			return (StatExtension.GetStatValue(this.pawn, StatDefOf.PsychicSensitivity, true, -1) - 1f) * 100f;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000145EC File Offset: 0x000127EC
		private static int Mend(Thing t, int amount)
		{
			int hitPoints = t.HitPoints;
			t.HitPoints = Mathf.Clamp(t.HitPoints + amount, t.HitPoints, t.MaxHitPoints);
			return t.HitPoints - hitPoints;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00014627 File Offset: 0x00012827
		private static float Mend(Thing t, float amount)
		{
			return (float)Ability_Mend.Mend(t, (int)amount) + (amount - (float)((int)amount));
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00014638 File Offset: 0x00012838
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				Thing thing = target.Thing;
				if (thing != null)
				{
					ThingDef def = thing.def;
					if (def != null && def.useHitPoints)
					{
						if (thing.HitPoints < thing.MaxHitPoints)
						{
							return true;
						}
						if (showMessages)
						{
							Messages.Message(Translator.Translate("VPE.MustBeDamaged"), MessageTypeDefOf.RejectInput, false);
						}
						return false;
					}
				}
				return false;
			}
			if (pawn.RaceProps.Humanlike)
			{
				if (pawn.Faction != this.pawn.Faction)
				{
					if (showMessages)
					{
						Messages.Message(Translator.Translate("VPE.MustBeAlly"), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				if (!GenCollection.Any<ThingWithComps>(pawn.equipment.AllEquipmentListForReading, (ThingWithComps t) => t.def.useHitPoints && t.HitPoints < t.MaxHitPoints))
				{
					if (!GenCollection.Any<Apparel>(pawn.apparel.WornApparel, (Apparel t) => t.def.useHitPoints && t.HitPoints < t.MaxHitPoints))
					{
						if (showMessages)
						{
							Messages.Message(Translator.Translate("VPE.MustHaveDamagedEquipment"), MessageTypeDefOf.RejectInput, false);
						}
						return false;
					}
				}
				return true;
			}
			else
			{
				if (!pawn.RaceProps.IsMechanoid)
				{
					if (showMessages)
					{
						Messages.Message(Translator.Translate("VPE.NoAnimals"), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				if (!ModsConfig.BiotechActive || !pawn.IsMechAlly(this.pawn))
				{
					if (showMessages)
					{
						Messages.Message(Translator.Translate("VPE.MustBeAlly"), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				if (!MechRepairUtility.CanRepair(pawn))
				{
					if (showMessages)
					{
						Messages.Message(Translator.Translate("VPE.MustBeDamaged"), MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				return true;
			}
		}
	}
}
