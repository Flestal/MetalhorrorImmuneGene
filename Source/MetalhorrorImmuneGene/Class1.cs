using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using Verse;

namespace MetalhorrorImmuneGene
{
    [StaticConstructorOnStartup]
    public static class MetalhorrorImmuneGene
    {
        static MetalhorrorImmuneGene()
        {
            new Harmony("Flestal.MetalhorrorImmuneGene").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
    [DefOf]
    public static class MIGDefOf
    {
        public static GeneDef MetalhorrorImmune;
    }
    [HarmonyPatch(typeof(MetalhorrorUtility), "CanBeInfected")]
    public static class MetalhorrorUtility_Patch
    {
        public static void Postfix(ref bool __result, Pawn pawn)
        {
            //Log.Message(pawn.Name+ " : " + descKey+" : "+source.Name);
            if (__result == false) return;
            bool Immune = pawn.RaceProps.Humanlike && pawn.genes.HasGene(MIGDefOf.MetalhorrorImmune) && pawn.genes.GetGene(MIGDefOf.MetalhorrorImmune).Active;
            if (Immune)
            {
                Log.Message(pawn.Name + " is immune to metalhorror.(Gene)");
                __result = false;
            }
        }
    }
    [HarmonyPatch(typeof(Pawn_InfectionVectorTracker), "AddInfectionVector", new Type[] { typeof(InfectionPathway) })]
    public static class AddInfectionVector_Patch
    {
        public static bool Prefix(InfectionPathway pathway)
        {
            bool Immune = pathway.OwnerPawn.genes.HasGene(MIGDefOf.MetalhorrorImmune) && pathway.OwnerPawn.genes.GetGene(MIGDefOf.MetalhorrorImmune).Active;
            if (Immune)
            {
                Log.Message(pathway.OwnerPawn.Name + " is immune to metalhorror.(Gene)");
                return false;
            }
            return true;
        }
    }

}
