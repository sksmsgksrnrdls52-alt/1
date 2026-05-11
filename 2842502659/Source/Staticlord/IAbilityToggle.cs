using System;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000103 RID: 259
	public interface IAbilityToggle
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600039F RID: 927
		// (set) Token: 0x060003A0 RID: 928
		bool Toggle { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060003A1 RID: 929
		string OffLabel { get; }
	}
}
