using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using VEF.Abilities;
using Verse;

namespace VanillaPsycastsExpanded.Technomancer
{
	// Token: 0x020000F4 RID: 244
	public class Ability_ReverseEngineer : Ability
	{
		// Token: 0x0600035F RID: 863 RVA: 0x00014F3C File Offset: 0x0001313C
		public unsafe override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (ResearchProjectDef researchProjectDef in from project in Ability_ReverseEngineer.GetResearchFor(targets[0].Thing)
			where project != null && !project.IsFinished
			select project)
			{
				int techprints = Find.ResearchManager.GetTechprints(researchProjectDef);
				if (techprints < researchProjectDef.TechprintCount)
				{
					Find.ResearchManager.AddTechprints(researchProjectDef, techprints - researchProjectDef.TechprintCount);
				}
				(*Ability_ReverseEngineer.progressRef.Invoke(Find.ResearchManager))[researchProjectDef] = researchProjectDef.baseCost;
				Find.ResearchManager.ReapplyAllMods();
				TaleRecorder.RecordTale(TaleDefOf.FinishedResearchProject, new object[]
				{
					this.pawn,
					researchProjectDef
				});
				DiaNode diaNode = new DiaNode(TranslatorFormattedStringExtensions.Translate("ResearchFinished", researchProjectDef.LabelCap) + "\n\n" + researchProjectDef.description);
				diaNode.options.Add(DiaOption.DefaultOK);
				Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
				if (!GenText.NullOrEmpty(researchProjectDef.discoveredLetterTitle) && Find.Storyteller.difficulty.AllowedBy(researchProjectDef.discoveredLetterDisabledWhen))
				{
					Find.LetterStack.ReceiveLetter(researchProjectDef.discoveredLetterTitle, researchProjectDef.discoveredLetterText, LetterDefOf.NeutralEvent, null, 0, true);
				}
			}
			targets[0].Thing.Destroy(0);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000150E8 File Offset: 0x000132E8
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			if (!target.HasThing)
			{
				return false;
			}
			List<ResearchProjectDef> researchFor = Ability_ReverseEngineer.GetResearchFor(target.Thing);
			if (GenList.NullOrEmpty<ResearchProjectDef>(researchFor))
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.Research"), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (researchFor.TrueForAll((ResearchProjectDef project) => project.IsFinished))
			{
				if (showMessages)
				{
					Messages.Message(Translator.Translate("VPE.AlreadyResearch"), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00015188 File Offset: 0x00013388
		private static List<ResearchProjectDef> GetResearchFor(Thing t)
		{
			List<ResearchProjectDef> list;
			if (Ability_ReverseEngineer.researchCache.TryGetValue(t, out list))
			{
				return list;
			}
			list = new List<ResearchProjectDef>();
			if (!GenList.NullOrEmpty<ResearchProjectDef>(t.def.researchPrerequisites))
			{
				list.AddRange(t.def.researchPrerequisites);
			}
			if (t.def.recipeMaker != null)
			{
				if (t.def.recipeMaker.researchPrerequisite != null)
				{
					list.Add(t.def.recipeMaker.researchPrerequisite);
				}
				if (!GenList.NullOrEmpty<ResearchProjectDef>(t.def.recipeMaker.researchPrerequisites))
				{
					list.AddRange(t.def.recipeMaker.researchPrerequisites);
				}
			}
			Predicate<ThingDefCountClass> <>9__1;
			foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
			{
				List<ThingDefCountClass> products = recipeDef.products;
				Predicate<ThingDefCountClass> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((ThingDefCountClass prod) => prod.thingDef == t.def));
				}
				if (GenCollection.Any<ThingDefCountClass>(products, predicate))
				{
					if (recipeDef.researchPrerequisite != null)
					{
						list.Add(recipeDef.researchPrerequisite);
					}
					if (!GenList.NullOrEmpty<ResearchProjectDef>(recipeDef.researchPrerequisites))
					{
						list.AddRange(recipeDef.researchPrerequisites);
					}
				}
			}
			list.RemoveAll((ResearchProjectDef proj) => proj == null);
			return Ability_ReverseEngineer.researchCache[t] = list;
		}

		// Token: 0x040001A1 RID: 417
		private static readonly Dictionary<Thing, List<ResearchProjectDef>> researchCache = new Dictionary<Thing, List<ResearchProjectDef>>();

		// Token: 0x040001A2 RID: 418
		private static readonly AccessTools.FieldRef<ResearchManager, Dictionary<ResearchProjectDef, float>> progressRef = AccessTools.FieldRefAccess<ResearchManager, Dictionary<ResearchProjectDef, float>>("progress");
	}
}
