using EntityStates.Bandit2;
using EntityStates.Bandit2.Weapon;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("FireShotgun2", new Type[]
        {
        typeof(FireShotgun2)
        })]
    internal class BanditFireShotgunSkillModifier : SimpleSkillModifier<FireShotgun2>
    {
        public override void OnSkillEnter(FireShotgun2 skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            skillState.bulletCount = skillState.bulletCount + level;
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.05f, level);

            skillState.minFixedSpreadYaw += level * 0.5f;
            skillState.maxFixedSpreadYaw += level;
        }
    }

    [SkillLevelModifier("Bandit2Rifle", new Type[]
        {
        typeof(Bandit2FireRifle)
        })]
    internal class BanditFireRifleSkillModifier : SimpleSkillModifier<Bandit2FireRifle>
    {
        float baseBloom = 0;
        public override void OnSkillEnter(Bandit2FireRifle skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (Mathf.Abs(baseBloom) < 0.01f)
            {
                baseBloom = skillState.spreadBloomValue;
            }

            if (level > 0)
            {
                skillState.procCoefficient = AdditiveScaling(skillState.procCoefficient, 0.1f, level);
                skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.2f, level);

                skillState.spreadBloomValue = baseBloom * (4 / ((float)level + 4));
            }
        }
    }

    [SkillLevelModifier("SlashBlade", new Type[]
    {
    typeof(SlashBlade)
    })]
    internal class BanditBladeSkillModifier : SimpleSkillModifier<SlashBlade>
    {
        Vector3 originalHitboxScale = Vector3.zero;
        public override void OnSkillEnter(SlashBlade slash, int level)
        {
            //Visual Scaling
            slash.swingEffectPrefab.transform.localScale = new Vector3(1 + level, 1 + level, 1 + level);

            base.OnSkillEnter(slash, level);

            slash.damageCoefficient = MultScaling(slash.damageCoefficient, 0.2f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            //Hitbox Scaling
            HitBoxGroup hitboxGroup = characterBody.modelLocator.modelTransform.GetComponent<HitBoxGroup>();

            if (hitboxGroup == null || hitboxGroup.groupName != "SlashBlade")
            {
                Debug.LogWarning("didn't get Bandit's slashHitbox. probably got changed?. aborting");

                return;
            }

            Transform hitboxTransform = hitboxGroup.hitBoxes[0].transform;

            if (originalHitboxScale == Vector3.zero)
            {
                originalHitboxScale = hitboxTransform.localScale;
            }
            hitboxTransform.localScale = new Vector3(MultScaling(originalHitboxScale.x, 0.2f, level), MultScaling(originalHitboxScale.y, 0.2f, level), MultScaling(originalHitboxScale.z, 0.3f, level));
        }
    }

    [SkillLevelModifier("Bandit2SerratedShivs", new Type[]
        {
        typeof(Bandit2FireShiv)
        })]
    internal class BanditFireShivSkillModifier : SimpleSkillModifier<Bandit2FireShiv>
    {
        public override void OnSkillEnter(Bandit2FireShiv skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            skillState.maxShivCount = 1 + (level / 2);
            skillState.baseDuration = skillState.baseDuration + (skillState.baseDuration * (level / 2) * 0.5f);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.1f, level);
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }
    }

    [SkillLevelModifier("ThrowSmokebomb", new Type[]
        {
        typeof(ThrowSmokebomb)
        })]
    internal class BanditSkillThrowSmokebombModifier : SimpleSkillModifier<ThrowSmokebomb>
    {
        static float baseRadius = 0;
        static float baseDamage = 0;

        static BuffDef BanditSpeedBuff;

        public static void RegisterBanditSpeedBuff()
        {
            BuffDef buffDef = new BuffDef
            {
                buffColor = new Color(153, 30, 150),
                buffIndex = (BuffIndex)63,
                canStack = true,
                eliteDef = null,
                iconSprite = Resources.Load<Sprite>("Textures/BuffIcons/texMovespeedBuffIcon"),
                isDebuff = false,
                name = "BanditSpeedBuff"
            };
            BanditSpeedBuff = buffDef;
            BuffAPI.Add(new CustomBuff(buffDef));
        }

        public override void OnSkillEnter(ThrowSmokebomb skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            for (int i = 0; i < level; i++)
            {
                skillState.outer.commonComponents.characterBody.AddTimedBuff(BanditSpeedBuff, StealthMode.duration);
            }

        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if (Mathf.Abs(baseRadius - 0) < 0.1f)
            {
                baseRadius = StealthMode.blastAttackRadius;
                baseDamage = StealthMode.blastAttackDamageCoefficient;
            }

            StealthMode.blastAttackRadius = MultScaling(baseRadius, 0.15f, level);
            StealthMode.blastAttackDamageCoefficient = MultScaling(baseDamage, 0.2f, level);
        }

        public static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig.Invoke(self);

            if (self.HasBuff(BanditSpeedBuff))
            {
                Reflection.SetPropertyValue<float>(self, "moveSpeed", self.moveSpeed + self.GetBuffCount(BanditSpeedBuff));
            }
        }
    }


    [SkillLevelModifier("Bandit2.ResetRevolver", new Type[]
        {
        typeof(FireSidearmResetRevolver)
        })]
    internal class BanditSkillResetRevolverModifier : SimpleSkillModifier<FireSidearmResetRevolver>
    {
        static SkillUpgrade ResetRevolverSkill;

        private void FindSkill(CharacterBody characterBody)
        {
            if (ResetRevolverSkill == null)
            {
                SkillUpgrade[] upgrades = characterBody.GetComponents<SkillUpgrade>();

                for (int i = 0; i < upgrades.Length; i++)
                {
                    if (upgrades[i] != null)
                    {
                        if (upgrades[i].targetBaseSkillName == "Bandit2.ResetRevolver")
                        {
                            ResetRevolverSkill = upgrades[i];
                        }
                    }
                }
            }
        }
        public override void OnSkillEnter(FireSidearmResetRevolver skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            //Attempt to get the skill if it's still invalid.
            FindSkill(skillState.outer.commonComponents.characterBody);
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if(ResetRevolverSkill != null)
            {
                if (di.attacker != null && self != null)
                {
                    CharacterBody body = self.GetComponent<CharacterBody>();
                    if (body != null)
                    {
                        if (di.damageType.HasFlag(RoR2.DamageType.BonusToLowHealth) && di.damageType.HasFlag(RoR2.DamageType.ResetCooldownsOnKill))
                        {
                            di.damage *= Mathf.Lerp(1.0f + ResetRevolverSkill.skillLevel * 0.3f, 1.0f + ResetRevolverSkill.skillLevel * 0.1f, self.combinedHealthFraction);
                        }
                    }
                }
            }

            orig.Invoke(self, di);
        }
    }


    [SkillLevelModifier("Bandit2Desperado", new Type[]
        {
        typeof(FireSidearmSkullRevolver)
        })]
    internal class BanditSkillSkullRevolverModifier : SimpleSkillModifier<FireSidearmSkullRevolver>
    {
        static SkillUpgrade SkullRevolverSkill;

        private void FindSkill(CharacterBody characterBody)
        {
            if (SkullRevolverSkill == null)
            {
                SkillUpgrade[] upgrades = characterBody.GetComponents<SkillUpgrade>();

                for (int i = 0; i < upgrades.Length; i++)
                {
                    if (upgrades[i] != null)
                    {
                        if (upgrades[i].targetBaseSkillName == "Bandit2.ResetRevolver")
                        {
                            SkullRevolverSkill = upgrades[i];
                        }
                    }
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(FireSidearmSkullRevolver skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            //Attempt to get the skill if it's still invalid.
            FindSkill(skillState.outer.commonComponents.characterBody);
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if (SkullRevolverSkill != null) 
            {
                if (di != null && self != null)
                {
                    if (di.damageType.HasFlag(RoR2.DamageType.GiveSkullOnKill))
                    {
                        float remainingHealth = (self.combinedHealth - di.damage);
                        if (remainingHealth / self.fullCombinedHealth < SkullRevolverSkill.skillLevel * 0.01f && remainingHealth > 0)
                        {
                            di.damage += remainingHealth;
                            di.damageType |= DamageType.BypassArmor;
                        }
                    }
                }
            }
            orig.Invoke(self, di);
        }
    }


}
