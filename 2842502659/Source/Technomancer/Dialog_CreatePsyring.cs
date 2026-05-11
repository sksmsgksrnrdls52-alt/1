using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VEF.Abilities;
using VEF.Utils;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F2 RID: 242
	public class Dialog_CreatePsyring : Window
	{
		// Token: 0x06000357 RID: 855 RVA: 0x00014B1C File Offset: 0x00012D1C
		public Dialog_CreatePsyring(Pawn pawn, Thing fuel, List<AbilityDef> excludedAbilities = null) : base(null)
		{
			this.pawn = pawn;
			this.fuel = fuel;
			this.forcePause = true;
			this.doCloseButton = false;
			this.doCloseX = true;
			this.closeOnClickedOutside = true;
			this.closeOnAccept = false;
			this.closeOnCancel = true;
			this.optionalTitle = Translator.Translate("VPE.CreatePsyringTitle");
			this.possibleAbilities = (from ability in pawn.GetComp<CompAbilities>().LearnedAbilities
			let psycast = ability.def.Psycast()
			where psycast != null
			orderby psycast.path.label, psycast.level descending, psycast.order
			select ability.def).Except(pawn.AllAbilitiesFromPsyrings()).Except(excludedAbilities ?? Enumerable.Empty<AbilityDef>()).ToList<AbilityDef>();
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00014C92 File Offset: 0x00012E92
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 800f);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00014CA3 File Offset: 0x00012EA3
		protected override float Margin
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00014CAC File Offset: 0x00012EAC
		private void Create(AbilityDef ability)
		{
			Psyring psyring = (Psyring)ThingMaker.MakeThing(VPE_DefOf.VPE_Psyring, null);
			psyring.Init(ability);
			GenPlace.TryPlaceThing(psyring, this.fuel.PositionHeld, this.fuel.MapHeld, 1, null, null, null, 1);
			if (this.fuel.stackCount == 1)
			{
				this.fuel.Destroy(0);
				return;
			}
			this.fuel.SplitOff(1).Destroy(0);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00014D28 File Offset: 0x00012F28
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect;
			rect..ctor(0f, 0f, inRect.width - 20f, this.lastHeight);
			float num = 5f;
			Widgets.BeginScrollView(inRect, ref this.scrollPos, rect, true);
			foreach (AbilityDef abilityDef in this.possibleAbilities)
			{
				Rect rect2;
				rect2..ctor(5f, num, rect.width, 64f);
				Rect rect3 = UIUtility.TakeLeftPart(ref rect2, 64f);
				rect2.xMin += 5f;
				GUI.DrawTexture(rect3, Command.BGTex);
				GUI.DrawTexture(rect3, abilityDef.icon);
				Widgets.Label(UIUtility.TakeTopPart(ref rect2, 20f), abilityDef.LabelCap);
				if (Widgets.ButtonText(UIUtility.TakeBottomPart(ref rect2, 20f), Translator.Translate("VPE.CreatePsyringButton"), true, true, true, null))
				{
					this.Create(abilityDef);
					this.Close(true);
				}
				Text.Font = 0;
				Widgets.Label(rect2, GenText.Truncate(abilityDef.description, rect2.width, this.truncationCache));
				Text.Font = 1;
				num += 69f;
			}
			this.lastHeight = num;
			Widgets.EndScrollView();
		}

		// Token: 0x0400019A RID: 410
		private const float ABILITY_HEIGHT = 64f;

		// Token: 0x0400019B RID: 411
		private readonly Thing fuel;

		// Token: 0x0400019C RID: 412
		private readonly List<AbilityDef> possibleAbilities;

		// Token: 0x0400019D RID: 413
		private readonly Dictionary<string, string> truncationCache = new Dictionary<string, string>();

		// Token: 0x0400019E RID: 414
		private float lastHeight;

		// Token: 0x0400019F RID: 415
		private Pawn pawn;

		// Token: 0x040001A0 RID: 416
		private Vector2 scrollPos;
	}
}
