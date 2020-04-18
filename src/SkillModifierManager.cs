using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace Skills {
    class SkillModifierManager {

        private static Dictionary<string, BaseSkillModifier> skillModifiers = new Dictionary<string, BaseSkillModifier>();

        public static void LoadSkillModifiers() {
            Assembly assembly = Assembly.GetCallingAssembly();
            if(assembly == null) {
                return;
            }
            foreach(Type type in assembly.GetTypes()) {
                var attributes = type.GetCustomAttributes<SkillLevelModifierAttribute>();
                foreach(SkillLevelModifierAttribute attribute in attributes) {
                    if(skillModifiers.ContainsKey(attribute.skillName)) {
                        Debug.LogWarningFormat("Replacing an existing skill modifier it not permitted. Skill name = {0}", attribute.skillName);
                        continue;
                    } else {
                        try {
                            BaseSkillModifier modifier = type.GetConstructor(new Type[0]).Invoke(new object[0]) as BaseSkillModifier;
                            skillModifiers[attribute.skillName] = modifier;
                            Debug.LogFormat("Loaded {0} for skill named \"{1}\"", type.Name, attribute.skillName);
                        } catch (Exception error){
                            Debug.LogError(error);
                        }
                    }
                }
            }
        }

        public static BaseSkillModifier GetSkillModifier(SkillDef skillDef) {
            if(skillDef && skillDef.skillName.Length != 0) {
                if(skillModifiers.TryGetValue(skillDef.skillName, out BaseSkillModifier modifier)) {
                    return modifier;
                }
            }
            return NoopSkillModifier.Instance;
        }

    }

    internal class NoopSkillModifier : BaseSkillModifier {

        internal static NoopSkillModifier Instance = new NoopSkillModifier();

        public override void ApplyChanges(SkillDef skillDef, int level) {
            // do nothing
        }

        public override int MaxLevel() {
            return 1;
        }
    }
}
