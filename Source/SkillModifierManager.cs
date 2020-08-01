using System;
using System.Collections.Generic;
using System.Reflection;
using EntityStates;
using Mono.Collections.Generic;
using RoR2.Skills;
using UnityEngine;
using SkillsPlusPlus.Modifiers;
using System.Linq;
using System.Collections.Specialized;

namespace SkillsPlusPlus {
    public sealed class SkillModifierManager {

        private static readonly Dictionary<string, BaseSkillModifier> skillModifiers = new Dictionary<string, BaseSkillModifier>();
        private static readonly Dictionary<Type, ISet<BaseSkillModifier>> stateTypeToSkillModifierDictionary = new Dictionary<Type, ISet<BaseSkillModifier>>();

        /// <summary>
        /// Finds and loads all skill modifiers in the current assembly.
        /// 
        /// Calling this is essential to have your skill modifiers available to Skills++
        /// </summary>
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
                    ConstructorInfo constructorInfo = type.GetConstructor(new Type[0]);
                    if(constructorInfo == null) {
                        Logger.Debug("Failed to find constructor info for {0}", type.FullName);
                        Logger.Debug("Other constructors included");
                        foreach(ConstructorInfo info in type.GetConstructors()) {
                            Logger.Debug(info);
                        }
                        continue;
                    }
                    foreach(SkillLevelModifierAttribute attribute in attributes) {
                        foreach(string registeredSkillName in attribute.skillNames) {
                            if(skillModifiers.ContainsKey(registeredSkillName)) {
                                Logger.Warn("Replacing an existing skill modifier it not permitted. Skill name = {0}", registeredSkillName);
                                continue;
                            } else {
                                object someSkillModifier = constructorInfo.Invoke(new object[0]);
                                if(someSkillModifier is BaseSkillModifier skillModifier) {
                                    skillModifier.skillName = registeredSkillName;
                                    skillModifier.EntityStateTypes = attribute.baseStateTypes;
                                    skillModifiers[registeredSkillName] = skillModifier;
                                    foreach(Type baseStateType in attribute.baseStateTypes) {
                                        if(stateTypeToSkillModifierDictionary.TryGetValue(baseStateType, out ISet<BaseSkillModifier> skillModifiers) == false) {
                                            skillModifiers = new HashSet<BaseSkillModifier>();
                                            stateTypeToSkillModifierDictionary[baseStateType] = skillModifiers;
                                        }
                                        skillModifiers.Add(skillModifier);
                                    }
                                    Logger.Debug("Loaded {0} for skill named \"{1}\"", type.Name, registeredSkillName);
                                }
                            }
                        }
                    }
                } catch(Exception error) {
                    Logger.Error(error);
                    continue;
                }
                
            }
        }

        internal static BaseSkillModifier GetSkillModifier(string skillName) {
            if(skillName != null && skillName.Length != 0) {
                if(skillModifiers.TryGetValue(skillName, out BaseSkillModifier modifier)) {
                    return modifier;
                }
            }
            return NoopSkillModifier.Instance;
        }

        internal static IEnumerable<BaseSkillModifier> GetSkillModifiersForEntityStateType(Type entityStateType) {
            if (stateTypeToSkillModifierDictionary.TryGetValue(entityStateType, out ISet<BaseSkillModifier> modifiers)) {
                return modifiers;
            }
            Logger.Debug("Could not find any ISkillModifiers for entity state {0}", entityStateType.FullName);
            return Enumerable.Empty<BaseSkillModifier>();
        }

        //internal static ISkillModifier GetSkillName(Type entityStateType) {
        //    if (stateTypeToSkillModifierDictionary.TryGetValue(entityStateType, out ISkillModifier modifier)) {
        //        return modifier;
        //    }
        //    return NoopSkillModifier.Instance;
        //}

    }
}
