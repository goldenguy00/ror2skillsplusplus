using System;
using System.Collections.Generic;
using System.Reflection;
using EntityStates;
using Mono.Collections.Generic;
using RoR2.Skills;
using UnityEngine;
using SkillsPlusPlus.Modifiers;
using Rewired.Platforms.PS4.Internal;
using System.Linq;

namespace SkillsPlusPlus {
    class SkillModifierManager {

        private static Dictionary<string, ISkillModifier> skillModifiers = new Dictionary<string, ISkillModifier>();
        private static Dictionary<Type, ISet<ISkillModifier>> stateTypeToSkillModifierDictionary = new Dictionary<Type, ISet<ISkillModifier>>();

        public static void LoadSkillModifiers() {
            Assembly assembly = Assembly.GetCallingAssembly();
            if(assembly == null) {
                return;
            }
            foreach(Type type in assembly.GetTypes()) {
                var attributes = type.GetCustomAttributes<SkillLevelModifierAttribute>();
                if(attributes == null || attributes.Count() == 0) {
                    continue;
                }
                try {
                    ISkillModifier modifier = type.GetConstructor(new Type[0])?.Invoke(new object[0]) as ISkillModifier;
                    if(modifier == null) {
                        continue;
                    }
                    foreach(SkillLevelModifierAttribute attribute in attributes) {
                        foreach(string registeredSkillName in attribute.skillNames) {
                            if(skillModifiers.ContainsKey(registeredSkillName)) {
                                Logger.Warn("Replacing an existing skill modifier it not permitted. Skill name = {0}", registeredSkillName);
                                continue;
                            } else {
                                skillModifiers[registeredSkillName] = modifier;
                                foreach(Type baseStateType in modifier.GetEntityStateTypes()) {
                                    ISet<ISkillModifier> skillModifiers;
                                    if(stateTypeToSkillModifierDictionary.TryGetValue(baseStateType, out skillModifiers) == false) {
                                        skillModifiers = new HashSet<ISkillModifier>();
                                        stateTypeToSkillModifierDictionary[baseStateType] = skillModifiers;
                                    }
                                    skillModifiers.Add(modifier);
                                }
                                Logger.Debug("Loaded {0} for skill named \"{1}\"", type.Name, registeredSkillName);
                            }
                        }
                    }
                } catch(Exception error) {
                    Logger.Error(error);
                    continue;
                }
                
            }
        }

        internal static ISkillModifier GetSkillModifier(string skillName) {
            if(skillName != null && skillName.Length != 0) {
                if(skillModifiers.TryGetValue(skillName, out ISkillModifier modifier)) {
                    return modifier;
                }
            }
            return NoopSkillModifier.Instance;
        }

        internal static ICollection<ISkillModifier> GetSkillModifiersForEntityStateType(Type entityStateType) {
            if (stateTypeToSkillModifierDictionary.TryGetValue(entityStateType, out ISet<ISkillModifier> modifiers)) {
                return modifiers;
            }
            Logger.Debug("Could not find any ISkillModifiers for entity state {0}", entityStateType.FullName);
            return new Collection<ISkillModifier>();
        }

        //internal static ISkillModifier GetSkillName(Type entityStateType) {
        //    if (stateTypeToSkillModifierDictionary.TryGetValue(entityStateType, out ISkillModifier modifier)) {
        //        return modifier;
        //    }
        //    return NoopSkillModifier.Instance;
        //}

    }
}
