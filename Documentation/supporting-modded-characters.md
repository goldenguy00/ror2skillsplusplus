# Supporting Skills++ for modded characters

Mod version: 0.0.11

> Note: This guide is for alpha versions of the Skills++ mod. 
> All code examples and described methods may change in future releases. 
> Breaking changes will occur as the mod evolves.

Skills++ is built upon Risk of Rain 2's entity state system that describes the behaviour of a skill.
The core concept for Skills++ is are the `SkillModifier` classes that are associated to skills.
When it comes to implementing your character you will only need to work with the skills since the mod does not care about the character loaded.

```mermaid
graph TD
	A[TypedSkillModifier<T>] -->B[BaseSkillModifier]
	B --> |implements| C[ISkillModifier]
```

## Before you begin

Skills++ deals directly with the internal entity states and logic of skills. 
It is highly recommended that you understand how Risk of Rain 2's skills are implemented using entity state machines.

## Creating the plugin

## Implementing a skill modifier

The simplest way to implement a skill modifier is to subclass `TypedSkillModifier`.
The `TypedSkillModifier` simplifies the implementation immensely by making some assumptions which are often true for simple skills.
There is a single generic type parameter for a `TypedSkillModifier`.
The type represents the entity state used when the skill is activated.

If your custom skill uses multiple entity states then refer to [advanced skill modifier implementations](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/tree/feature/public-api/Documentation/supporting-modded-characters.md#advanced-skill-modifier-implementations)

The other important requirement for a skill modifier is that the class is attributed with the `SkillModifier`.
The attribute needs to name the skills the modifier can be applied to.

There are three methods that are used to implement the 
**OnSkillLeveledUp**
**OnSkillEnter**
**OnSkillExit**

Here is an example of modifying the Commando's primary attack to increase it's rate of fire every level.
```c#
[SkillLevelModifier("FirePistol")]
class CommandoFirePistolSkillModifier : TypedBaseSkillModifier<FirePistol2> {

    public override int MaxLevel {
        get { return 4; }
    }

    public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
        base.OnSkillLeveledUp(level, characterBody, skillDef);
        FirePistol2.baseDuration = MultScaling(0.2f, -0.20f, level);         
    }

}
```

Let's break this down line by line.

```c#
[SkillLevelModifier("FirePistol")]
```
This marks that the `CommandoFirePistolSkillModifier` is for the skill named `FirePistol`.
`FirePistol` is the internal name for Commando's double tap skill.

```c#
class CommandoFirePistolSkillModifier : TypedBaseSkillModifier<FirePistol2> {
```
Here the name of this new class is not important.
`FirePistol2` is the game's internal class for handling the firing of the Commando's primary skill. 
It is used to specify the entity state this skill modifer concerns.

```c#
    public override int MaxLevel {
        get { return 4; }
    }
```
Despite the maximum level for this skill being four this skill can be levelled up three times.
Within Skills++ a skill with no upgrades is considered to be at level one.

```c#
    public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef) {
        base.OnSkillLeveledUp(level, characterBody, skillDef);
        FirePistol2.baseDuration = MultScaling(0.2f, -0.20f, level);         
    }
```
This defines the changes to the skill when the user spends a point on the skill.
This 



## Loading the skill modifier in game

## Common upgrades

### Granting buffs

Coming soon

### Adding stock

Coming soon

### Changing projectile behaviour

Coming soon

### Changing hitbox

Coming soon

## Advanced skill modifier implementations

Sometimes a single skill has complex behaviour that is spread amongst several 

