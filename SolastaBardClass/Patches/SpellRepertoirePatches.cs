using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass
{
    class SpellRepertoirePatches
    {
        class RulesetCharacterComputeAutopreparedSpellsPatcher
        {
            [HarmonyPatch(typeof(RulesetCharacter), "ComputeAutopreparedSpells")]
            internal static class RestModuleHitDice_RollHitDie_Patch
            {
                internal static void Postfix(RulesetCharacter __instance, RulesetSpellRepertoire spellRepertoire)
                {
                    foreach (var s in spellRepertoire.AutoPreparedSpells)
                    {
                        if (spellRepertoire.SpellCastingFeature.SpellKnowledge == RuleDefinitions.SpellKnowledge.Selection
                            && !spellRepertoire.KnownSpells.Contains(s))
                        {
                            spellRepertoire.KnownSpells.Add(s);
                        }
                    }
                }
            }
        }
    }
}
