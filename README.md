# Risk of Rain 2 - Skills++ Mod

[![Discord](https://img.shields.io/discord/745162241359478894?color=7289DA&label=modding%20discord&logo=discord&logoColor=white)](https://discord.gg/HbqdgMG) [![source code](https://img.shields.io/static/v1?label=gitlab&logo=gitlab&message=source%20code&color=FCA121)](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/tree/master)

## What's new in 0.2.1

- Added skill upgrade for Strides of Heresy item
- Added ingame skill upgrade descriptions as tooltips on skills
- Added defensive checks to prevent NPE when the player's character is unloaded
- Changed Artificer's Nano Bomb to have the blast size of the bomb increased with levels invested.
- Removed Artificer's Nano Spear scaling of charge time

[Full changelog history](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/master/CHANGELOG.md)

## Installation

Extract the mod's files to your `BepInEx/plugins` folder.

## Usage

While ingame your will earn skill points at certain levels that can be used to purchase upgrades to your characters skills.
By default, your first skill point is earned upon reaching level five, and subsequently rewards every fifth level.

You can change the number of levels between skill points within the gameplay settings in Risk of Rain 2. Changes to this setting will be applied at the start of each run.

![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/levels_per_skillpoint_option.png)

To redeem points open the info screen (hold TAB by default) and click the skill you would like to buy.
Upgrades do not carry over between runs so you will be starting from scratch every time.

1. When you have levelled up enough to buy a skill the icons will change to have a yellow border.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/skillpoint_available.png)

1. Opening the info screen will present 'Buy' buttons over the top of skills that can be purchased.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/buy_options.png)

1. Clicking on any of the 'Buy' buttons will spend a single point and the skill will upgrade.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/skillpoint_spent.png)

## Modded characters

Go and check out all of these characters and give them some love!

If you have added Skills++ to your own modded character let me know and I will add it here.

[![Aatrox by Rob](https://thunderstore.fra1.cdn.digitaloceanspaces.com/live/repository/icons/rob-Aatrox-3.5.0.png.256x256_q85_crop.jpg)](https://thunderstore.io/package/rob/Aatrox/) [![Twitch by Rob](https://thunderstore.fra1.cdn.digitaloceanspaces.com/live/repository/icons/rob-Twitch-2.1.0.png.256x256_q85_crop.jpg)](https://thunderstore.io/package/rob/Twitch/)

### Controllers

Using controllers is now support for purchasing upgrades for your skills.
In order to begin using it you must bind your desired control to entering the buying mode.
You can find the binding at the end of all the gamepad controls.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/skills_gamepad_settings.jpg)

While ingame just press and the bound button and activate the skill you wish to upgrade.

### Disabling survivors

Console commands have been added to disable and reenable Skills++ for selected survivors.
You can use this to exclude survivors that conflict with Skills++.

The two commands are:

- `spp_disable_survivor <survivor name>` Disables Skills++ for the named survivor
- `spp_enable_survivor <survivor name>` Re-enables Skills++ for the named survivor

Example usage:

`spp_disable_survivor Artificer`

By default all characters will be enabled for Skills++.
The enable command is there to re-enable a survivor if the conflict no longer exists.

## Feedback and bug reports

The best way to give feedback or raise bugs is through [my modding discord](https://discord.gg/HbqdgMG).
I welcome everyone who uses Skills++ to drop by and share your thoughts.

## FAQ

**When playing multiplayer my/friend's game doesn't work. What is going wrong?**

Skills++ does have multiplayer support but there may still be gaps in the logic.
The best action to resolve this is to raise it in the [modding discord](https://discord.gg/HbqdgMG) with as much info as possible

**Will Skills++ support modded characters?**
  
Yes. There is support for third party code to integrate with the Skills++ system. Guides are available [here](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/master/Documentation/supporting-modded-characters.md) alongside the mod's source code.

**Skills++ makes the game a cakewalk. Do you recommend any other mods to balance the game?**

I'd highly recommend harb's [Diluvian Difficulty](https://thunderstore.io/package/Harb/DiluvianDifficulty/) mod. It adds two extra difficulties to the game that should level the playing fields a bit more.

## Special thanks

A very special thanks to the following people. They have been amazing people providing feedback and bug reports for the mod

- K'Not Devourer of Worlds
- Maxi
- TEA
  
---

Looking for skill upgrade descriptions?

They are now available ingame when you hover on a skill's icon.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/skill_upgrade_tooltip.png)