# Risk of Rain 2 - Skills++ Mod

Skills++ adds skills upgrades that can be purchased as your character levels up throughout a run.

## Table of Contents

- [Risk of Rain 2 - Skills++ Mod](#risk-of-rain-2---skills-mod)
  - [Table of Contents](#table-of-contents)
  - [What's new in 0.1.1](#whats-new-in-011)
  - [Installation](#installation)
  - [Usage](#usage)
    - [Controllers](#controllers)
    - [Disabling survivors](#disabling-survivors)
  - [Feedback and bug reports](#feedback-and-bug-reports)
  - [FAQ](#faq)
  - [Special thanks](#special-thanks)
  - [Upgrade descriptions](#upgrade-descriptions)
    - [Commando](#commando)
    - [Huntress](#huntress)
    - [MUL-T](#mul-t)
    - [Engineer](#engineer)
    - [Artificer](#artificer)
    - [Mercenary](#mercenary)
    - [Loader](#loader)
    - [Acrid](#acrid)

## What's new in 0.1.1

Changes:

- Added instructions for console command usage

[Full changelog history](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/master/CHANGELOG.md)

## Installation

Extract the mod's files to your `BepInEx/plugins` folder.

## Usage

While ingame your will earn points every third level gained that can be used to purchase upgrades to your characters skills.
Your first skill point is earned upon reaching level 5, and subsequently rewards every fifth level.
To redeem points open the info screen (hold TAB by default) and click the skill you would like to buy.
Upgrades do not carry over between runs so you will be starting from scratch every time.

1. When you have levelled up enough to buy a skill the icons will change to have a yellow border.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/skillpoint_available.png)

1. Opening the info screen will present 'Buy' buttons over the top of skills that can be purchased.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/buy_options.png)

1. Clicking on any of the 'Buy' buttons will spend a single point and the skill will upgrade.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/skillpoint_spent.png)

### Controllers

Using controllers is now support for purchasing upgrades for your skills.
In order to begin using it you must bind your desired control to entering the buying mode.
You can find the binding at the end of all the gamepad controls.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/skills_gamepad_settings.jpg)

While ingame just press and the bound button and activate the skill you wish to upgrade.

> ***NOTE:*** Mutliplayer is possible but not support. There may be cases when game state is not correctly synced between connected clients.

> ***NOTE:*** This mod is still a work in progress. There is no guarantee that Skills++ is compatible with other mods. If you find any issue join the discord

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

**Will Skills++ support modded characters?**
  
Yes. There is support for third party code to integrate with the Skills++ system. Guides are available [here](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/feature/master/Documentation/supporting-modded-characters.md) alongside the mod's source code.

**Skills++ makes the game a cakewalk. Do you recommend any other mods to balance the game?**

I'd highly recommend harb's [Diluvian Difficulty](https://thunderstore.io/package/Harb/DiluvianDifficulty/) mod. It adds a new harder difficulty to the game that should level the playing fields a bit more.

## Special thanks

A very special thanks to the following people. They have been amazing people providing feedback and bug reports for the mod

- K'Not Devourer of Worlds
- Maxi
- TEA

## Upgrade descriptions

### Commando

| Skill | | Description |
|:-|-|------|
| Double Tap | ![Double tap](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/7/7c/Double_Tap.png?version=8348516f319278a9ce5a38a2ab9bde5f) | `+20%` rate of fire and `+15%` recoil reduction per level |
| Phase Round | ![Phase Round](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/3/38/Phase_Round.png?version=4a5aa5a7fc21bb738483e4651e4aae23) | `+30%` damage and `+30%` projectile velocity per level |
| Phase Blast | ![Phase Blast](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/4/49/Phase_Blast.png?version=ecab07bd3d0ea7ac621800fd515cf00a) | `+30%` bullets fires and `+20%` blast range per level |
| Tactical Dive | ![Tactical Dive](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/2/25/Tactical_Dive.png?version=304aa13b595665c29447fd6d42e65d5e) | Grants `+0.75 seconds` of invulnerability per level |
| Tactical Slide | ![Tactical Slide](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/e/e8/Tactical_Slide.png?version=46594be2f8d139de21b67c744bf36101) | Grants `+0.75 seconds` of bonus attack speed per level |
| Supressive Fire | ![Supressive fire](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/d/db/Suppressive_Fire.png?version=c2660d649079b7a33697698cc2460cc7) | `+30%` bullets fired per level |
| Frag Grenade | ![Frag Grenade](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/6/6b/Frag_Grenade.png?version=317d27bd9e4809671a2a5858501bdd63) | `+20%` explosion damage and `+20%` blast radius per level |

### Huntress

| Skill | | Description |
|:-|-|------|
| Strafe | ![Strafe](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/a/af/Strafe.png?version=d0a6dd80cf2a02f8633d4e2d24781c53) | `+20%` range and `+20%` proc chance per level |
| Flurry | ![Flurry](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/2/24/Flurry.png?version=2d9c753d649ead966ad6232f7e4e37f4) | `+10%` range and `+1` arrow fired per level. *Crits fire twice as many arrows* |
| Laser Glaive | ![Laser Glaive](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/f/f6/Laser_Glaive.png?version=8ec21d23c6dcbf77ad57e1579208d85c) | `+1` bounce, `+10%` damage, and `+10 units` of bounce range per level |
| Blink | ![Blink](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/1/16/Blink.png?version=7f46f9aceb48babddfca64c5778500c6) | Grants `+1 second` per level or full crit time |
| Phase Blink | ![Phase Blink](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/d/dd/Phase_Blink.png?version=e5f06fa5f8cbe671e1e730ca9955f1df) | Grants `+0.5 seconds` per level or full crit time |
| Arrow Rain | ![Arrow Rain](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/5/59/Arrow_Rain.png?version=4c6450cf90c89762b61a38224ce9ab66) | `+25%` area of effect and `+20%` damage per level |
| Ballista | ![Ballista](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/d/d5/Ballista.png?version=b606a0a22dc203327b91fc5fd96d2930) | `+1` bullet and `+20%` damage per level |

### MUL-T

| Skill | | Description |
|:-|-|------|
| Auto-Nailgun | ![Auto-Nailgun](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/e/ec/Auto-Nailgun.png/128px-Auto-Nailgun.png?version=e955112af188f212b6911f5feb3c117b) | `+20%` damage and `+4` bullets to final burst per level |
| Rebar Puncher | ![Rebar Puncher](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/1/10/Rebar_Puncher.png/128px-Rebar_Puncher.png?version=c6312b8ad9b87116c00b04a82aae1c99) | `+20%` faster wind up and `+20%` damage per level |
| Scrap Launcher | ![Scrap Launcher](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/e/e6/Scrap_Launcher.png?version=e4b5fde76f9cb3ba3853e874532ce56b) | `+1` stock, `+20%` damage, and `+15%` blast radius per level |
| Power-Saw | ![Power-Saw](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/8/8e/Power-Saw.png/128px-Power-Saw.png?version=de4e4c071db8036e40d2623be38c2a53) | `+20%` rate of fire, `+20%` damage, and `+30%` blade hitbox size per level |
| Blast Cannister | ![Blast Cannister](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/f/f2/Blast_Canister.png/128px-Blast_Canister.png?version=8a890dd16b6811607def84527e8f2b07) | `+3` child bombs, `+20%` spread radius, `+20%` child blast radius, and `+20%` damage per level |
| Transport Mode | ![Transport Mode](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/8/85/Transport_Mode.png/128px-Transport_Mode.png?version=2dbe3c42bd9d303918c79d9d964bb02f) | `+10%` duration, `+10%` speed, and `+30%` damage per level |
| Swap | ![Swap](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/8/8d/Retool.png/128px-Retool.png?version=53426b923f0280f1016f58327be358df) | Upon activating gain `+1 second` of bonus attack speed and `+1 second` equipment cooldown per level |

### Engineer

| Skill | | Description |
|:-|-|------|
| Bouncing Grenades | ![Bouncing Grenades](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/0/08/Bouncing_Grenades.png?version=a49107eccea1acc6be75f6eeb280e77c) | `+1` and `+2` to the minimum and maximum grenades fired per level |
| Pressure Mines | ![Pressure Mines](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/9/9e/Pressure_Mines.png?version=bc1cc946c19e73379eba73b8b181da08) | `-10%` arming time, `+20%` damage, `+20%` blast radius, and `+20%` trigger radius per level |
| Spider Mines | ![Spider Mines](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/6/61/Spider_Mines.png?version=c9fbe949643ffa5288d31c89d8a5e063) | `+30%` activation range and `+20%` damage per level |
| Bubble Shield | ![Bubble Shield](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/d/da/Bubble_Shield.png?version=528e249613f7923f2df377251ec0394a) | `+15%` duration and `+15%` size per level |
| Thermal Harpoons | ![Thermal Harpoons](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/b/b7/Thermal_Harpoons.png?version=bb32066c6dc3552e1bca5dff720dc79c) | `+1` harpoon ammo, `+20%` damage, and `+30%` targetting range per level |
| TR12 Gauss Auto-Turret | ![TR12 Gauss Auto-Turret](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/6/60/TR12_Gauss_Auto-Turret.png?version=462607ca2de8fe7b5d8b9df093e50951) | `+25%` enemy detection range and `+10%` damage per level. `+1` deployable turret per two levels |
| TR58 Carbonizer Turret | ![TR58 Carbonizer Turret](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/4/4f/TR58_Carbonizer_Turret.png?version=8189f9c2f4ef8614ab225be5cd6ccc4f) | `+15%` damage, `+25%` proc chance, and `+10%` rate of fire per level. `+1` deployable turret per two levels |

### Artificer

| Skill | | Description |
|:-|-|------|
| Flame bolt | ![Flame bolt](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/5/5e/Flame_Bolt.png/128px-Flame_Bolt.png?version=0bbd8ebdf6ba9c782f88efe4d54525f8) | `+2` stock and `-10%` stock recharge time per level |
| Plasma Bolt | ![Plasma bolt](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/5/57/Plasma_Bolt.png?version=045ec9e9d331fa33009737333022e6d8) | `+2` stock and `-10%` stock recharge time per level |
| Charged Nano-Bomb | ![Charged nano-bomb](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/c/c8/Charged_Nano-Bomb.png/128px-Charged_Nano-Bomb.png?version=749364707a1c24b755823a2510dbbd7c) | `+20%` damage and `+10%` max charge time per level |
| Charged Nano-Spear | ![Charged nano-spear](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/9/92/Cast_Nano-Spear.png?version=de2ff32f16c4237e2346bfda2e15de5e) | `+20%` damage and `+10%` max charge time per level |
| Snapfreeze | ![Snapfreeze](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/c/ce/Snapfreeze.png?version=6f15a989c9b98a8ad36f39a3ccd0b514) | `+30%` wall damage, `+15%` wall duration, and `+20%` wall length per level |
| Flamethrower | ![Flamethrower](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/a/ad/Flamethrower.png/128px-Flamethrower.png?version=df4f797fe7e0a9fff8e6777a2204b85d) | `+20%` range, `+25%` flame radius, and `+20%` damage per level |
| Ion Surge | ![Ion Surge](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/d/d1/Ion_Surge.png?version=a945f2d18ec9387976fafb92e68b9be9) | `+25%` damage and `+30%` area of effect per level |

### Mercenary

| Skill | | Description |
|:-|-|------|
| Laser Sword | ![Laser Sword](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/e/e2/Laser_Sword.png/128px-Laser_Sword.png?version=3a9875734bc89fdbef834264cfaa4713) | `+20%` damage, and `+15%` attack speed per level |
| Whirlwind | ![Whirlwind](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/e/e0/Whirlwind.png/128px-Whirlwind.png?version=077953d0f21ba522ac23092a1acac734) | `+25%` damage and `+25%` larger hitbox per level |
| Rising Thunder | ![Rising Thunder](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/d/d4/Rising_Thunder.png/128px-Rising_Thunder.png?version=ab9e85a71cf761228816b626a7bafc6d) | `+25%` damage per level. `+1` stock per two levels |
| Blinding Assault | ![Blinding Assault](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/8/87/Blinding_Assault.png/128px-Blinding_Assault.png?version=83b40986e4f6151932ffd8d4d4e7bbe6) | `+20%` damage, `+0.5 seconds` extra delay before reset |
| Eviscerate | ![Eviscerate](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/9/9f/Eviscerate.png/128px-Eviscerate.png?version=073ebfc6fec123029ebed3108195aa7a) | `+30%` chain range, `+15%` attack speed, `+10%` proc chance per level |
| Slicing Winds | ![Slicing Winds](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/thumb/b/bc/Slicing_Winds.png/128px-Slicing_Winds.png?version=39f32fdb68b806c809dc4050d5862417) | `+20%` attack speed and `+20%` damage per level |

### Loader

| Skill | | Description |
|:-|-|------|
| Knuckleboom | ![Knuckleboom](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/b/b2/Knuckleboom.png?version=4d7fb1484fb2bf3f211770644d5e60aa) | `+1%` barrier gained and `+20%` damage per level |
| Grapple Fist | ![Grapple Fist](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/2/25/Grapple_Fist.png?version=bfb4deca6cf26aa24ba26bb1956e0b6f) | `+30%` hook range per level. |
| Spiked Fist | ![Spiked Fist](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/6/62/Spiked_Fist.png?version=57f8c08e3bea7b07fd6a1bfb09905a3a) | `+15%` hook range and `+20%` damage per level |
| Charged Gauntlet | ![Charged Gauntlet](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/7/79/Charged_Gauntlet.png?version=b7b77a9caf70ee5392fc91d72117f487) | `+10%` max charge, `+20%` base damage, and `+15%` velocity based damage per level |
| Thunder Gauntlet | ![Thunder Gauntlet](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/f/f3/Thunder_Gauntlet.png?version=8432d38598a6a1793013abbeb9701420) | `+15%` damage and `+20%` cone range per level |
| M551 Pylon | ![M551 Pylon](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/3/37/M551_Pylon.png?version=8ce17c1b445a3a173a70e909e3389705) | `+20%` damage, `+20%` range, and `+20%` rate of fire per level |

### Acrid

| Skill | | Description |
|:-|-|------|
| Vicious Wounds | ![Vicious Wounds](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/5/5a/Vicious_Wounds.png?version=2bf16e4d4644a16c2c9e450ac0b4490d) | `+20%` normal damage, `+25%` combo finisher damage, and `+15%` attack speed per level |
| Neurotoxin | ![Neurotoxin](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/2/2b/Neurotoxin.png?version=47fb02a7cc34bf60f197a92ffb3c7a0b) | `+20%` damage, `+30%` projectile speed, and `+30%` blast radius per level. `+1 stock` per two levels |
| Ravenous Bite | ![Ravenous Bite](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/8/8b/Ravenous_Bite.png?version=da9fcc5ab5b26987c160f065b8c95d73) | `+20%` damage and `+1` stock per level |
| Caustic Leap | ![Caustic Leap](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/9/9e/Caustic_Leap.png?version=e067644005e3fbe939dc6c0e74cb680c) | `+20%` blast damage, `+30%` acid pool damage, and `+20%` acid pool size per level |
| Frenzied Leap | ![Frenzied Leap](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/9/96/Frenzied_Leap.png?version=05eb87da91cfb184824ae53eac148f4f) | `+25%` blast damage and `+15%` refunded time per level |
| Epidemic | ![Epidemic](https://gamepedia.cursecdn.com/riskofrain2_gamepedia_en/9/9d/Epidemic.png?version=c285fc0546271c32d190c85ae8f5516b) | `+5` infection bounces, `+20%` infection range, and `+20%` on hit damage per level |
