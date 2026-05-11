using System;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace VanillaPsycastsExpanded
{
	// Token: 0x02000033 RID: 51
	public class Ability_Resurrect : Ability_TargetCorpse
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00004418 File Offset: 0x00002618
		public override Gizmo GetGizmo()
		{
			Gizmo gizmo = base.GetGizmo();
			if ((from x in this.pawn.health.hediffSet.GetNotMissingParts(0, 0, null, null)
			where x.def == VPE_DefOf.Finger
			select x).All((BodyPartRecord finger) => GenCollection.Any<Hediff>(this.pawn.health.hediffSet.hediffs, (Hediff hediff) => hediff.def == VPE_DefOf.VPE_Sacrificed && hediff.Part == finger)))
			{
				gizmo.Disable(Translator.Translate("VPE.NoAvailableFingers"));
			}
			return gizmo;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004494 File Offset: 0x00002694
		public override void Cast(params GlobalTargetInfo[] targets)
		{
			base.Cast(targets);
			foreach (GlobalTargetInfo globalTargetInfo in targets)
			{
				BodyPartRecord bodyPartRecord;
				if (GenCollection.TryRandomElement<BodyPartRecord>(from x in this.pawn.health.hediffSet.GetNotMissingParts(0, 0, null, null)
				where x.def == VPE_DefOf.Finger
				select x into finger
				where !GenCollection.Any<Hediff>(this.pawn.health.hediffSet.hediffs, (Hediff hediff) => hediff.def == VPE_DefOf.VPE_Sacrificed && hediff.Part == finger)
				select finger, ref bodyPartRecord))
				{
					Corpse corpse = globalTargetInfo.Thing as Corpse;
					SoulFromSky soulFromSky = SkyfallerMaker.MakeSkyfaller(VPE_DefOf.VPE_SoulFromSky) as SoulFromSky;
					soulFromSky.target = corpse;
					GenPlace.TryPlaceThing(soulFromSky, corpse.Position, corpse.Map, 0, null, null, null, 1);
					this.pawn.health.AddHediff(HediffMaker.MakeHediff(VPE_DefOf.VPE_Sacrificed, this.pawn, bodyPartRecord), bodyPartRecord, null, null);
				}
			}
		}
	}
}
