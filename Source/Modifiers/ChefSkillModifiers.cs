using System;
using System.Collections.Generic;
using UnityEngine;

using SkillsPlusPlus.Modifiers;
using RoR2;

using EntityStates;
using EntityStates.Chef;

using RoR2.Projectile;
using RoR2.Skills;

using System.Linq;
using UnityEngine.AddressableAssets;
using EntityStates.VoidSurvivor.CorruptMode;

namespace SkillsPlusPlus.Source.Modifiers {
    [SkillLevelModifier("Dice", typeof(Dice))]
    class ChefDiceSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is Dice)
            {
                Logger.Debug("Dice");
            }
        }
    }

    [SkillLevelModifier("Sear", typeof(Sear))]
    class ChefSearSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is Sear)
            {
                Logger.Debug("FireCorruptHandBeam");
            }
        }
    }

    [SkillLevelModifier("RolyPoly", typeof(RolyPoly), typeof(ChargeRolyPoly), typeof(RolyPolyWeaponBlockingState),
        typeof(RolyPolyBoostedProjectileTimer))]
    class ChefRolyPolySkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is RolyPoly)
            {
                Logger.Debug("RolyPoly");
            }
            else if (skillState is ChargeRolyPoly)
            {
                Logger.Debug("ChargeRolyPoly");
            }
            else if (skillState is RolyPolyWeaponBlockingState)
            {
                Logger.Debug("RolyPolyWeaponBlockingState");
            }
        }
    }

    [SkillLevelModifier("Glaze", typeof(Glaze))]
    class ChefGlazeSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is Glaze)
            {
                Logger.Debug("Glaze");
            }
        }
    }

    [SkillLevelModifier("YesChef", typeof(YesChef))]
    class ChefYesChefSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is YesChef)
            {
                Logger.Debug("YesChef");
            }
        }
    }
}
/*
    [SkillLevelModifier("", typeof())]
    class SkillModifier : BaseSkillModifier {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level) {
            base.OnSkillEnter(skillState, level);

            if (skillState is ) {
                Logger.Debug("");
            }
        }
    }
    */

