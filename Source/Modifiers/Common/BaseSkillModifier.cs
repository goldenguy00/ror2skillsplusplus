using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace SkillsPlusPlus.Modifiers {
    public abstract class BaseSkillModifier : ISkillModifier {

        /// <inheritdoc/>
        public Type[] EntityStateTypes { get; internal set; }

        /// <inheritdoc/>
        public string skillName { get; internal set; }

        public BaseSkillModifier() {
            this.skillName = "";
            this.EntityStateTypes = new Type[0];
        }

        public BaseSkillModifier(string skillName, Type[] entityStateTypes) {
            this.skillName = skillName;
            this.EntityStateTypes = entityStateTypes;
        }

        /// <inheritdoc/>
        public virtual void OnSkillEnter(BaseState skillState, int level) { }

        /// <inheritdoc/>
        public virtual void OnSkillExit(BaseState skillState, int level) { }

        /// <inheritdoc/>
        public virtual void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) { }

        /// <inheritdoc/>
        public virtual string GetOverrideSkillDescriptionToken() {
            return null;
        }

        internal void ReportBroken(params String[] fields) {
            var fieldInfo = String.Join(",", fields);
            Logger.Warn("Skill {0} is broken. Cannot access the following items: {1}", this.GetType().FullName, fieldInfo);
        }

        #region Helpers

        /// <summary>
        /// A helper method that performs the math to acheive a linear scaling.
        /// <para>
        /// More docs: <see href="https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/feature/public-api/Documentation/scaling-operators.md"/>
        /// </para>
        /// </summary>
        /// <param name="baseValue">The base value when the skill has not been upgraded</param>
        /// <param name="constant">The amount to add every level</param>
        /// <param name="level">The current level to scale up to</param>
        /// <returns></returns>
        public static float AdditiveScaling(float baseValue, float constant, int level) {
            return baseValue + constant * level;
        }

        /// <summary>
        /// A helper method that performs the math to acheive a linear scaling.
        /// <para>
        /// More docs: <see href="https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/feature/public-api/Documentation/scaling-operators.md"/>
        /// </para>
        /// </summary>
        /// <param name="baseValue">The base value when the skill has not been upgraded</param>
        /// <param name="constant">The amount to add every level</param>
        /// <param name="level">The current level to scale up to</param>
        /// <returns></returns>
        public static int AdditiveScaling(int baseValue, int constant, int level) {
            return baseValue + constant * level;
        }

        /// <summary>
        /// A helper method that performs the math to acheive a compounding scaling.
        /// Use this if you want the base value to multiply by a constant amount every level. 
        /// <para>
        /// More docs: <see href="https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/feature/public-api/Documentation/scaling-operators.md"/>
        /// </para>
        /// </summary>
        /// <param name="baseValue">The Base value when the skill has not been upgraded</param>
        /// <param name="multiplier">The multiplication to apply per level</param>
        /// <param name="level">The current level to scale up to</param>
        /// <returns></returns>
        public static float MultScaling(float baseValue, float multiplier, int level) {
            if(multiplier <= -1) {
                Logger.Error("Multipliers less than -1 are not allowed as it causes sporadic behaviour with the scaling.");
                return baseValue;
            }
            return (float)((Math.Pow(multiplier + 1, level) - 1) * baseValue) + baseValue;
        }

        // public static int MultScaling(int baseValue, float multiplier, int level) {
        //     return (int)MultScaling((float)baseValue, multiplier, level);            
        // }

        #endregion
    }
}
