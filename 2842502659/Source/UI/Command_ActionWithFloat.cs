using System;
using System.Collections.Generic;
using Verse;

namespace VanillaPsycastsExpanded.UI
{
	// Token: 0x020000D4 RID: 212
	public class Command_ActionWithFloat : Command_Action
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002CC RID: 716 RVA: 0x000102A0 File Offset: 0x0000E4A0
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				Func<IEnumerable<FloatMenuOption>> func = this.floatMenuGetter;
				if (func == null)
				{
					return null;
				}
				return func();
			}
		}

		// Token: 0x0400015D RID: 349
		public Func<IEnumerable<FloatMenuOption>> floatMenuGetter;
	}
}
