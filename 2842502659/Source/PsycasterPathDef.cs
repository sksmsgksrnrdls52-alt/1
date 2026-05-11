using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x020000BD RID: 189
	public class PsycasterPathDef : Def
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0000DD87 File Offset: 0x0000BF87
		public virtual bool CanPawnUnlock(Pawn pawn)
		{
			return this.PawnHasCorrectBackstory(pawn) && this.PawnHasMeme(pawn) && this.PawnHasGene(pawn) && this.PawnIsMechanitor(pawn) && this.PawnHasCorrectFocus(pawn);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000DDB6 File Offset: 0x0000BFB6
		private bool PawnHasMeme(Pawn pawn)
		{
			if (this.requiredMeme != null)
			{
				Ideo ideo = pawn.Ideo;
				return ideo != null && ideo.memes.Contains(this.requiredMeme);
			}
			return true;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
		private bool PawnHasGene(Pawn pawn)
		{
			if (this.requiredGene != null)
			{
				Pawn_GeneTracker genes = pawn.genes;
				bool? flag;
				if (genes == null)
				{
					flag = null;
				}
				else
				{
					Gene gene = genes.GetGene(this.requiredGene);
					flag = ((gene != null) ? new bool?(gene.Active) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
			return true;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000DE38 File Offset: 0x0000C038
		private bool PawnIsMechanitor(Pawn pawn)
		{
			return !this.requiredMechanitor || MechanitorUtility.IsMechanitor(pawn);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000DE4A File Offset: 0x0000C04A
		private bool PawnHasCorrectFocus(Pawn pawn)
		{
			return this.requiredFocus == null || this.requiredFocus.CanPawnUse(pawn);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000DE64 File Offset: 0x0000C064
		private bool PawnHasCorrectBackstory(Pawn pawn)
		{
			if (GenList.NullOrEmpty<BackstoryCategoryAndSlot>(this.requiredBackstoriesAny))
			{
				return true;
			}
			foreach (BackstoryCategoryAndSlot backstoryCategoryAndSlot in this.requiredBackstoriesAny)
			{
				BackstoryDef backstory = pawn.story.GetBackstory(backstoryCategoryAndSlot.slot);
				List<string> list = (backstory != null) ? backstory.spawnCategories : null;
				if (list != null && list.Contains(backstoryCategoryAndSlot.categoryName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000DEF8 File Offset: 0x0000C0F8
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate()
			{
				if (!GenText.NullOrEmpty(this.background))
				{
					this.backgroundImage = ContentFinder<Texture2D>.Get(this.background, true);
				}
				if (!GenText.NullOrEmpty(this.altBackground))
				{
					this.altBackgroundImage = ContentFinder<Texture2D>.Get(this.altBackground, true);
				}
				if (this.width > 0 && this.height > 0)
				{
					Texture2D texture2D = new Texture2D(this.width, this.height);
					Color[] array = new Color[this.width * this.height];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = this.backgroundColor;
					}
					texture2D.SetPixels(array);
					texture2D.Apply();
					if (this.backgroundImage == null)
					{
						this.backgroundImage = texture2D;
					}
					if (this.altBackgroundImage == null)
					{
						this.altBackgroundImage = texture2D;
					}
				}
				if (this.backgroundImage == null && this.altBackgroundImage != null)
				{
					this.backgroundImage = this.altBackgroundImage;
				}
				if (this.altBackgroundImage == null && this.backgroundImage != null)
				{
					this.altBackgroundImage = this.backgroundImage;
				}
			});
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000DF14 File Offset: 0x0000C114
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (PsycasterPathDef.Blank == null)
			{
				PsycasterPathDef.Blank = new AbilityDef();
			}
			PsycasterPathDef.TotalPoints++;
			this.abilities = new List<AbilityDef>();
			foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefsListForReading)
			{
				AbilityExtension_Psycast modExtension = abilityDef.GetModExtension<AbilityExtension_Psycast>();
				if (modExtension != null && modExtension.path == this)
				{
					this.abilities.Add(abilityDef);
				}
			}
			this.MaxLevel = this.abilities.Max((AbilityDef ab) => ab.Psycast().level);
			PsycasterPathDef.TotalPoints += this.abilities.Count;
			this.abilityLevelsInOrder = new AbilityDef[this.MaxLevel][];
			foreach (IGrouping<int, AbilityDef> grouping in from ab in this.abilities
			group ab by ab.Psycast().level)
			{
				this.abilityLevelsInOrder[grouping.Key - 1] = grouping.OrderBy((AbilityDef ab) => ab.Psycast().order).SelectMany(delegate(AbilityDef ab)
				{
					if (!ab.Psycast().spaceAfter)
					{
						return Gen.YieldSingle<AbilityDef>(ab);
					}
					return new List<AbilityDef>
					{
						ab,
						PsycasterPathDef.Blank
					};
				}).ToArray<AbilityDef>();
			}
			this.HasAbilities = this.abilityLevelsInOrder.Any((AbilityDef[] arr) => !GenList.NullOrEmpty<AbilityDef>(arr));
			if (!this.HasAbilities)
			{
				return;
			}
			for (int i = 0; i < this.abilityLevelsInOrder.Length; i++)
			{
				if (this.abilityLevelsInOrder[i] == null)
				{
					this.abilityLevelsInOrder[i] = new AbilityDef[0];
				}
			}
		}

		// Token: 0x040000B4 RID: 180
		public static AbilityDef Blank;

		// Token: 0x040000B5 RID: 181
		public static int TotalPoints;

		// Token: 0x040000B6 RID: 182
		[Unsaved(false)]
		public List<AbilityDef> abilities;

		// Token: 0x040000B7 RID: 183
		[Unsaved(false)]
		public AbilityDef[][] abilityLevelsInOrder;

		// Token: 0x040000B8 RID: 184
		public string altBackground;

		// Token: 0x040000B9 RID: 185
		[Unsaved(false)]
		public Texture2D altBackgroundImage;

		// Token: 0x040000BA RID: 186
		public string background;

		// Token: 0x040000BB RID: 187
		public Color backgroundColor;

		// Token: 0x040000BC RID: 188
		[Unsaved(false)]
		public Texture2D backgroundImage;

		// Token: 0x040000BD RID: 189
		[Unsaved(false)]
		public bool HasAbilities;

		// Token: 0x040000BE RID: 190
		public int height;

		// Token: 0x040000BF RID: 191
		[MustTranslate]
		public string lockedReason;

		// Token: 0x040000C0 RID: 192
		[Unsaved(false)]
		public int MaxLevel;

		// Token: 0x040000C1 RID: 193
		public int order;

		// Token: 0x040000C2 RID: 194
		public List<BackstoryCategoryAndSlot> requiredBackstoriesAny;

		// Token: 0x040000C3 RID: 195
		public MeditationFocusDef requiredFocus;

		// Token: 0x040000C4 RID: 196
		public GeneDef requiredGene;

		// Token: 0x040000C5 RID: 197
		public MemeDef requiredMeme;

		// Token: 0x040000C6 RID: 198
		public bool requiredMechanitor;

		// Token: 0x040000C7 RID: 199
		public bool ignoreLockRestrictionsForNeurotrainers = true;

		// Token: 0x040000C8 RID: 200
		public bool ensureLockRequirement;

		// Token: 0x040000C9 RID: 201
		public string tab;

		// Token: 0x040000CA RID: 202
		public string tooltip;

		// Token: 0x040000CB RID: 203
		public int width;
	}
}
