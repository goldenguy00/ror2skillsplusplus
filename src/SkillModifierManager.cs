using System;
using System.Collections.Generic;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using UnityEngine;

namespace Skills {
    class SkillModifierManager {

        private static Dictionary<string, ISkillModifier> skillModifiers = new Dictionary<string, ISkillModifier>();
        private static Dictionary<Type, ISkillModifier> stateTypeToSkillModifierDictionary = new Dictionary<Type, ISkillModifier>();

        public static void LoadSkillModifiers() {
            Assembly assembly = Assembly.GetCallingAssembly();
            if(assembly == null) {
                return;
            }
            foreach(Type type in assembly.GetTypes()) {
                var attributes = type.GetCustomAttributes<SkillLevelModifierAttribute>();
                foreach(SkillLevelModifierAttribute attribute in attributes) {
                    if(skillModifiers.ContainsKey(attribute.skillName)) {
                        Logger.Warn("Replacing an existing skill modifier it not permitted. Skill name = {0}", attribute.skillName);
                        continue;
                    } else {
                        try {
                            ISkillModifier modifier = type.GetConstructor(new Type[0]).Invoke(new object[0]) as ISkillModifier;
                            skillModifiers[attribute.skillName] = modifier;
                            stateTypeToSkillModifierDictionary[modifier.GetStateType()] = modifier;
                            Logger.Debug("Loaded {0} for skill named \"{1}\"", type.Name, attribute.skillName);
                        } catch (Exception error){
                            Logger.Error(error);
                        }
                    }
                }
            }
        }

        internal static ISkillModifier GetSkillModifier(SkillDef skillDef) {
            if (skillDef && skillDef.skillName.Length != 0) {
                if (skillModifiers.TryGetValue(skillDef.skillName, out ISkillModifier modifier)) {
                    return modifier;
                }
            }
            return NoopSkillModifier.Instance;
        }

        internal static ISkillModifier GetSkillModifierForEntityStateType(Type entityStateType) {
            if (stateTypeToSkillModifierDictionary.TryGetValue(entityStateType, out ISkillModifier modifier)) {
                return modifier;
            }
            return NoopSkillModifier.Instance;
        }

        internal static ISkillModifier GetSkillName(Type entityStateType) {
            if (stateTypeToSkillModifierDictionary.TryGetValue(entityStateType, out ISkillModifier modifier)) {
                return modifier;
            }
            return NoopSkillModifier.Instance;
        }

    }

    public class NoopSkillModifier : ISkillModifier {

        internal static NoopSkillModifier Instance = new NoopSkillModifier();

        public SkillDef SkillDef { get; set; }

        public int MaxLevel {
            get { return 1; }
        }

        public Type GetStateType() {
            return typeof(BaseState);
        }

        public void OnSkillLeveledUp(int level) {
            // do nothing
        }

        public void OnSkillWillBeUsed(BaseState skillState, int level) {
            // do nothing
        }

        public string GetOverrideSkillDescriptionToken() {
            return null;
        }
    }
}
