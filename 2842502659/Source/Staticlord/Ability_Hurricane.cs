using System;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Staticlord
{
	// Token: 0x02000102 RID: 258
	public class Ability_Hurricane : Ability, IAbilityToggle, IChannelledPsycast, ILoadReferenceable
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000397 RID: 919 RVA: 0x000161E0 File Offset: 0x000143E0
		// (set) Token: 0x06000398 RID: 920 RVA: 0x000161FF File Offset: 0x000143FF
		public bool Toggle
		{
			get
			{
				HurricaneMaker hurricaneMaker = this.maker;
				return hurricaneMaker != null && hurricaneMaker.Spawned;
			}
			set
			{
				if (value)
				{
					this.DoAction();
					return;
				}
				HurricaneMaker hurricaneMaker = this.maker;
				if (hurricaneMaker == null)
				{
					return;
				}
				hurricaneMaker.Destroy(0);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000399 RID: 921 RVA: 0x0001621C File Offset: 0x0001441C
		public string OffLabel
		{
			get
			{
				return Translator.Translate("VPE.StopHurricane");
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00016230 File Offset: 0x00014430
		public bool IsActive
		{
			get
			{
				HurricaneMaker hurricaneMaker = this.maker;
				return hurricaneMaker != null && hurricaneMaker.Spawned;
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00016250 File Offset: 0x00014450
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			this.maker = (HurricaneMaker)ThingMaker.MakeThing(VPE_DefOf.VPE_HurricaneMaker, null);
			this.maker.Pawn = this.pawn;
			GenSpawn.Spawn(this.maker, this.pawn.Position, this.pawn.Map, 0);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000162AE File Offset: 0x000144AE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<HurricaneMaker>(ref this.maker, "maker", false);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000162C7 File Offset: 0x000144C7
		public override Gizmo GetGizmo()
		{
			return new Command_AbilityToggle(this.pawn, this);
		}

		// Token: 0x040001B0 RID: 432
		private HurricaneMaker maker;
	}
}
