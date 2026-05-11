using System;
using UnityEngine;
using VanillaPsycastsExpanded.UI;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000C2 RID: 194
	public class Dialog_EditPsysets : Window
	{
		// Token: 0x0600028F RID: 655 RVA: 0x0000EA35 File Offset: 0x0000CC35
		public Dialog_EditPsysets(ITab_Pawn_Psycasts parent) : base(null)
		{
			this.parent = parent;
			this.doCloseX = true;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000EA4C File Offset: 0x0000CC4C
		protected override float Margin
		{
			get
			{
				return 3f;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000EA53 File Offset: 0x0000CC53
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.parent.Size.x * 0.3f, Mathf.Max(300f, this.NeededHeight));
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000EA80 File Offset: 0x0000CC80
		private float NeededHeight
		{
			get
			{
				return this.parent.RequestedPsysetsHeight + this.Margin * 2f;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000EA9A File Offset: 0x0000CC9A
		public override void DoWindowContents(Rect inRect)
		{
			this.parent.DoPsysets(inRect);
			if (this.windowRect.height < this.NeededHeight)
			{
				this.windowRect.height = this.NeededHeight;
			}
		}

		// Token: 0x040000DA RID: 218
		private readonly ITab_Pawn_Psycasts parent;
	}
}
