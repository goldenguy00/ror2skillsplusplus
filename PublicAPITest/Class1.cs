using BepInEx;

using UnityEngine;
using SkillsPlusPlus;
using SkillsPlusPlus.Modifiers;
using System.Collections.Generic;
using System;
using EntityStates;

using RoR2;
using RoR2.Skills;

namespace PublicAPITest {

    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.cwmlolzlz.skills")]
    [BepInPlugin("com.cwmlolzlz.skills.publicapitest", "SkillsPublicTestApi", "0.0.1")]
    public class PublicAPITestPlugin : BaseUnityPlugin {

        public void Awake() {
            SkillModifierManager.LoadSkillModifiers();
        }

    }

    [SkillLevelModifier("Test")]
    public class TestSkillModifier : BaseSkillModifier {

        public override int MaxLevel {
            get { return 4; }
        }

        public override IList<Type> GetEntityStateTypes() {
            return new List<Type>();
        }
    }
}
