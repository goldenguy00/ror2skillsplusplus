using EntityStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace SkillsPlusPlus.Modifiers {
    public abstract class BaseSkillModifier {

        /// <inheritdoc/>
        internal Type[] EntityStateTypes { get; set; }


        /// <summary>
        /// The list of skillnames associated with this modifier.
        /// </summary>
        internal string[] skillNames { get; set; }

        public BaseSkillModifier() {
            this.skillNames = new string[0];
            this.EntityStateTypes = new Type[0];
        }

        public BaseSkillModifier(string[] skillNames, Type[] entityStateTypes) {
            this.skillNames = skillNames;
            this.EntityStateTypes = entityStateTypes;
        }

        /// <summary>
        /// Called immediately before the character enters one of the listed entity state types
        /// </summary>
        /// <param name="skillState">The entity state instance</param>
        /// <param name="level">The current level of the associated skill</param>
        public virtual void OnSkillEnter(BaseState skillState, int level) { }

        /// <summary>
        /// Called when the character exits one of the listed entity state types
        /// </summary>
        /// <param name="skillState">The entity state instance</param>
        /// <param name="level">The current level of the associated skill</param>
        public virtual void OnSkillExit(BaseState skillState, int level) { }

        /// <summary>
        /// Called when the player spends a skill point one of the listed entity state types
        /// </summary>
        /// <param name="level">The new level of the skill</param>
        /// <param name="characterBody">The player's character body</param>
        /// <param name="skillDef">The associated skill definition</param>
        public virtual void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) { }

        /// <summary>
        /// Provides a string token to replace the associated skill's descriptions.
        /// 
        /// Use null if you do not wish to change the skill's existing description.
        /// </summary>
        /// <remarks>This API is still a work in progress and may be deprecated in later releases</remarks>
        /// 
        /// <returns>The token resources for to replace the associated skills description.</returns>
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
