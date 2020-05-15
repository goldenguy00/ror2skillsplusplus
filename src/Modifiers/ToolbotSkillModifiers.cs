using System;
using System.Collections.Generic;
using System.Text;

using EntityStates;
using EntityStates.Toolbot;

namespace SkillsPlusPlus.Modifiers {

    [SkillLevelModifier("FireNailgun")]
    class ToolbotSkillModifier : TypedBaseSkillModifier<FireNailgun> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("FireSpear")]
    class ToolbotSpearSkillModifier : TypedBaseSkillModifier<FireSpear> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("FireGrenadeLauncher")]
    class ToolbotGrenadeLauncherSkillModifier : TypedBaseSkillModifier<FireGrenadeLauncher> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("FireBuzzsaw")]
    class ToolbotBuzzsawSkillModifier : TypedBaseSkillModifier<FireBuzzsaw> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("StunDrone")]
    class ToolbotStunDroneSkillModifier : TypedBaseSkillModifier<AimStunDrone> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("ToolbotDash")]
    class ToolbotDashSkillModifier : TypedBaseSkillModifier<ToolbotDash> {

        public override int MaxLevel {
            get { return 4; }
        }
    }

    [SkillLevelModifier("Swap")]
    class ToolbotEquipmentSwapSkillModifier : TypedBaseSkillModifier<StartToolbotStanceSwap> {

        public override int MaxLevel {
            get { return 4; }
        }
    }
}
