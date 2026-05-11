using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x0200003B RID: 59
	public class GameComponent_PsycastsManager : GameComponent
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00004D07 File Offset: 0x00002F07
		public GameComponent_PsycastsManager(Game game)
		{
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004D28 File Offset: 0x00002F28
		public override void GameComponentTick()
		{
			base.GameComponentTick();
			for (int i = this.goodwillImpacts.Count - 1; i >= 0; i--)
			{
				GoodwillImpactDelayed goodwillImpactDelayed = this.goodwillImpacts[i];
				if (Find.TickManager.TicksGame >= goodwillImpactDelayed.impactInTicks)
				{
					goodwillImpactDelayed.DoImpact();
					this.goodwillImpacts.RemoveAt(i);
				}
			}
			for (int j = this.removeAfterTicks.Count - 1; j >= 0; j--)
			{
				Thing item = this.removeAfterTicks[j].Item1;
				int item2 = this.removeAfterTicks[j].Item2;
				bool flag = item == null || item.Destroyed;
				if (flag)
				{
					this.removeAfterTicks.RemoveAt(j);
				}
				else if (Find.TickManager.TicksGame >= item2)
				{
					item.Destroy(0);
					this.removeAfterTicks.RemoveAt(j);
				}
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004E07 File Offset: 0x00003007
		public override void StartedNewGame()
		{
			base.StartedNewGame();
			this.inited = true;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004E18 File Offset: 0x00003018
		public override void LoadedGame()
		{
			base.LoadedGame();
			if (this.inited)
			{
				return;
			}
			Log.Message("[VPE] Added to existing save, adding PsyLinks.");
			this.inited = true;
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAliveOrDead.Concat(Find.Maps.SelectMany((Map map) => map.mapPawns.AllPawns)))
			{
				List<Hediff_Psylink> source = new List<Hediff_Psylink>();
				if (pawn != null)
				{
					Pawn_HealthTracker health = pawn.health;
					if (health != null)
					{
						HediffSet hediffSet = health.hediffSet;
						if (hediffSet != null)
						{
							hediffSet.GetHediffs<Hediff_Psylink>(ref source, null);
						}
					}
				}
				Hediff_Psylink hediff_Psylink = (from p in source
				orderby p.level descending
				select p).FirstOrDefault<Hediff_Psylink>();
				if (hediff_Psylink != null && pawn.Psycasts() == null)
				{
					((Hediff_PsycastAbilities)pawn.health.AddHediff(VPE_DefOf.VPE_PsycastAbilityImplant, hediff_Psylink.Part, null, null)).InitializeFromPsylink(hediff_Psylink);
					pawn.abilities.abilities.RemoveAll((Ability ab) => ab is Psycast);
				}
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004F70 File Offset: 0x00003170
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<GoodwillImpactDelayed>(ref this.goodwillImpacts, "goodwillImpacts", 2, Array.Empty<object>());
			if (Scribe.mode == 4 && this.goodwillImpacts == null)
			{
				this.goodwillImpacts = new List<GoodwillImpactDelayed>();
			}
			if (Scribe.mode == 1)
			{
				this.removeAfterTicks_things = new List<Thing>();
				this.removeAfterTicks_ticks = new List<int>();
				for (int i = 0; i < this.removeAfterTicks.Count; i++)
				{
					this.removeAfterTicks_things.Add(this.removeAfterTicks[i].Item1);
					this.removeAfterTicks_ticks.Add(this.removeAfterTicks[i].Item2);
				}
			}
			Scribe_Collections.Look<Thing>(ref this.removeAfterTicks_things, "removeAfterTick_things", 3, Array.Empty<object>());
			Scribe_Collections.Look<int>(ref this.removeAfterTicks_ticks, "removeAfterTick_ticks", 1, Array.Empty<object>());
			if (Scribe.mode == 4)
			{
				this.removeAfterTicks = new List<ValueTuple<Thing, int>>();
				for (int j = 0; j < this.removeAfterTicks_things.Count; j++)
				{
					this.removeAfterTicks.Add(new ValueTuple<Thing, int>(this.removeAfterTicks_things[j], this.removeAfterTicks_ticks[j]));
				}
			}
			Scribe_Values.Look<bool>(ref this.inited, "inited", false, false);
		}

		// Token: 0x0400002F RID: 47
		public List<GoodwillImpactDelayed> goodwillImpacts = new List<GoodwillImpactDelayed>();

		// Token: 0x04000030 RID: 48
		private bool inited;

		// Token: 0x04000031 RID: 49
		[TupleElementNames(new string[]
		{
			"thing",
			"tick"
		})]
		public List<ValueTuple<Thing, int>> removeAfterTicks = new List<ValueTuple<Thing, int>>();

		// Token: 0x04000032 RID: 50
		private List<Thing> removeAfterTicks_things;

		// Token: 0x04000033 RID: 51
		private List<int> removeAfterTicks_ticks;
	}
}
