﻿using HarmonyLib;
using SolastaBardClass.NewFeatureDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass.Patches
{
    class GameLocationBattleManagerPatcher
    {
        class CharacterBuildingManagerSetPointPoolPatcher
        {
            [HarmonyPatch(typeof(GameLocationBattleManager), "HandleCharacterAttack")]
            internal static class GameLocationBattleManager_HandleCharacterAttack_Patch
            {
                static public System.Collections.IEnumerator convertToEnumerator(List<object> list)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        yield return list[i];
                    }
                }

                internal static void Postfix(GameLocationBattleManager __instance,
                                                            GameLocationCharacter attacker,
                                                            GameLocationCharacter defender,
                                                            ActionModifier attackModifier,
                                                            RulesetAttackMode attackerAttackMode,
                                                            ref System.Collections.IEnumerator __result)
                {
                    if (__instance.battle == null)
                    {
                        return;
                    }

                    List<System.Collections.IEnumerator> extra_events = new List<System.Collections.IEnumerator>();

                    var units = __instance.Battle.AllContenders;                   
                    foreach (GameLocationCharacter unit in units)
                    {
                        if (!unit.RulesetCharacter.IsDeadOrDyingOrUnconscious 
                            && unit.GetActionTypeStatus(ActionDefinitions.ActionType.Reaction, ActionDefinitions.ActionScope.Battle, false) == ActionDefinitions.ActionStatus.Available)
                        {
                            var powers = unit.RulesetCharacter.UsablePowers.Where(u => u.PowerDefinition is NewFeatureDefinitions.IReactionPowerOnAttackAttempt 
                                                                                  && unit.RulesetCharacter.GetRemainingUsesOfPower(u) > 0
                                                                                  && (u.PowerDefinition as NewFeatureDefinitions.IReactionPowerOnAttackAttempt)
                                                                                    .canBeUsed(unit.RulesetCharacter, attacker.RulesetCharacter, defender.RulesetCharacter, attackerAttackMode)
                                                                                 ).ToArray();

                            foreach (var p in powers)
                            {
                                Main.Logger.Log("found: " + p.PowerDefinition.name);
                                CharacterActionParams reactionParams = new CharacterActionParams(unit, (ActionDefinitions.Id)ExtendedActionId.ModifyAttackRollViaPower);
                                reactionParams.TargetCharacters.Add(attacker);
                                reactionParams.TargetCharacters.Add(defender);
                                reactionParams.ActionModifiers.Add(attackModifier);
                                reactionParams.AttackMode = attackerAttackMode;
                                reactionParams.UsablePower = p;
                                IRulesetImplementationService service1 = ServiceRepository.GetService<IRulesetImplementationService>();
                                reactionParams.RulesetEffect = (RulesetEffect)service1.InstantiateEffectPower(attacker.RulesetCharacter, p, false);
                                reactionParams.StringParameter = p.PowerDefinition.Name;                                
                                reactionParams.IsReactionEffect = true;
                                IGameLocationActionService service2 = ServiceRepository.GetService<IGameLocationActionService>();
                                int count = service2.PendingReactionRequestGroups.Count;
                                (service2 as GameLocationActionManager)?.AddInterruptRequest((ReactionRequest)new ReactionRequestUsePower(reactionParams, "ModifyAttackRollViaPower"));
                                extra_events.Add(__instance.WaitForReactions(attacker, service2, count));
                            }
                        }
                    }

                    var all_events = new List<object>();
                    while (__result.MoveNext())
                    {
                        all_events.Add(__result.Current);
                    }
                    all_events.AddRange(extra_events);
                    __result = convertToEnumerator(all_events);
                }
            }
        }
    }
}
