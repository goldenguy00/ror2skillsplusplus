using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using EntityStates;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers {

    internal interface ISkillModifier {

        /// <summary>
        /// Defines the list of <see cref="EntityState"/> types that this skill modifer wishes to alter.
        /// </summary>
        /// <returns></returns>
        IList<Type> GetEntityStateTypes();

        /// <summary>
        /// The name of the associated skill.
        /// </summary>
        string skillName { get; set; }

        /// <summary>
        /// Called immediately before the character enters one of the listed entity state types
        /// </summary>
        /// <param name="skillState">The entity state instance</param>
        /// <param name="level">The current level of the associated skill</param>
        void OnSkillEnter(BaseState skillState, int level);

        /// <summary>
        /// Called when the character exits one of the listed entity state types
        /// </summary>
        /// <param name="skillState">The entity state instance</param>
        /// <param name="level">The current level of the associated skill</param>
        void OnSkillExit(BaseState skillState, int level);

        /// <summary>
        /// Called when the player spends a skill point one of the listed entity state types
        /// </summary>
        /// <param name="level">The new level of the skill</param>
        /// <param name="characterBody">The player's character body</param>
        /// <param name="skillDef">The associated skill definition</param>
        void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef);


        /// <summary>
        /// Provides a string token to replace the associated skill's descriptions.
        /// 
        /// Use null if you do not wish to change the skill's existing description.
        /// </summary>
        /// <remarks>This API is still a work in progress and may be deprecated in later releases</remarks>
        /// 
        /// <returns>The token resources for to replace the associated skills description.</returns>
        string GetOverrideSkillDescriptionToken();

    }
}
