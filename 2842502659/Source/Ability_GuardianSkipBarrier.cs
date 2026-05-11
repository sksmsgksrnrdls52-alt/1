using System;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000B1 RID: 177
	public class Ability_GuardianSkipBarrier : Ability, IChannelledPsycast, ILoadReferenceable
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000CD30 File Offset: 0x0000AF30
		public bool IsActive
		{
			get
			{
				return this.pawn.health.hediffSet.HasHediff(VPE_DefOf.VPE_GuardianSkipBarrier, false);
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000CD50 File Offset: 0x0000AF50
		public override Gizmo GetGizmo()
		{
			Hediff hediff = this.pawn.health.hediffSet.GetFirstHediffOfDef(VPE_DefOf.VPE_GuardianSkipBarrier, false);
			if (hediff != null)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = Translator.Translate("VPE.CancelSkipbarrier");
				command_Action.defaultDesc = Translator.Translate("VPE.CancelSkipbarrierDesc");
				command_Action.icon = this.def.icon;
				command_Action.action = delegate()
				{
					this.pawn.health.RemoveHediff(hediff);
				};
				float num = 10f;
				HediffWithLevelCombination requiredHediff = this.def.requiredHediff;
				ushort? num2;
				if (requiredHediff == null)
				{
					num2 = null;
				}
				else
				{
					HediffDef hediffDef = requiredHediff.hediffDef;
					num2 = ((hediffDef != null) ? new ushort?(hediffDef.index) : null);
				}
				ushort? num3 = num2;
				float num4 = num + (float)num3.GetValueOrDefault();
				HediffWithLevelCombination requiredHediff2 = this.def.requiredHediff;
				command_Action.Order = num4 + (float)((requiredHediff2 != null) ? requiredHediff2.minimumLevel : 0);
				return command_Action;
			}
			return base.GetGizmo();
		}
	}
}
