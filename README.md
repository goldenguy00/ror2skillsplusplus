# Risk of Rain 2 - Skills++ Mod

[![Discord](https://img.shields.io/discord/745162241359478894?color=7289DA&label=modding%20discord&logo=discord&logoColor=white)](https://discord.gg/HbqdgMG)
[![source code](https://img.shields.io/static/v1?label=gitlab&logo=gitlab&message=source%20code&color=FCA121)](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/tree/master)

## What's new in 0.1.3

Changes:

- Added REX upgrades
- Added ingame setting to change the levels between skill points
- Updated R2API dependancy to 2.5.7

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

### Controllers

Using controllers is now support for purchasing upgrades for your skills.
In order to begin using it you must bind your desired control to entering the buying mode.
You can find the binding at the end of all the gamepad controls.
![](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/raw/master/Images/skills_gamepad_settings.jpg)

While ingame just press and the bound button and activate the skill you wish to upgrade.

> ***NOTE:*** Multiplayer is not supported yet. Upgrades are not synced between clients and will cause desyn issues.

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

## Modded characters

- Rob's [Aatrox](https://thunderstore.io/package/rob/Aatrox/)
- Rob's [Twitch](https://thunderstore.io/package/rob/Twitch/)

Go and check out all of these characters and give them some love!

If you have added Skills++ to your own modded character let me know and I will add it to the list.

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
  
---

## Vanilla character upgrades

### Commando

| Skill | | Description |
|:-|-|------|
| Double Tap | ![Double tap](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/7/7c/Double_Tap.png/revision/latest/scale-to-width-down/64?cb=20200129200853) | `+20%` rate of fire and `+15%` recoil reduction per level |
| Phase Round | ![Phase Round](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/38/Phase_Round.png/revision/latest/scale-to-width-down/64?cb=20200130010701) | `+30%` damage and `+30%` projectile velocity per level |
| Phase Blast | ![Phase Blast](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/49/Phase_Blast.png/revision/latest/scale-to-width-down/64?cb=20200130003012) | `+30%` bullets fires and `+20%` blast range per level |
| Tactical Dive | ![Tactical Dive](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/25/Tactical_Dive.png/revision/latest/scale-to-width-down/64?cb=20200130010657) | Grants `+0.75 seconds` of invulnerability per level |
| Tactical Slide | ![Tactical Slide](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e8/Tactical_Slide.png/revision/latest/scale-to-width-down/64?cb=20200331165626) | Grants `+0.75 seconds` of bonus attack speed per level |
| Supressive Fire | ![Supressive fire](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/db/Suppressive_Fire.png/revision/latest/scale-to-width-down/64?cb=20200129200914) | `+30%` bullets fired per level |
| Frag Grenade | ![Frag Grenade](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/6b/Frag_Grenade.png/revision/latest/scale-to-width-down/64?cb=20200130003159) | `+20%` explosion damage and `+20%` blast radius per level |

### Huntress

| Skill | | Description |
|:-|-|------|
| Strafe | ![Strafe](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/a/af/Strafe.png/revision/latest/scale-to-width-down/64?cb=20200306011845) | `+20%` range and `+20%` proc chance per level |
| Flurry | ![Flurry](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/24/Flurry.png/revision/latest/scale-to-width-down/64?cb=20200331165622) | `+10%` range and `+1` arrow fired per level. *Crits fire twice as many arrows* |
| Laser Glaive | ![Laser Glaive](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/f/f6/Laser_Glaive.png/revision/latest/scale-to-width-down/64?cb=20200129200860) | `+1` bounce, `+10%` damage, and `+10 units` of bounce range per level |
| Blink | ![Blink](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/1/16/Blink.png/revision/latest/scale-to-width-down/64?cb=20200130010660) | Grants `+1 second` per level or full crit time |
| Phase Blink | ![Phase Blink](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/dd/Phase_Blink.png/revision/latest/scale-to-width-down/64?cb=20200130003634) | Grants `+0.5 seconds` per level or full crit time |
| Arrow Rain | ![Arrow Rain](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/59/Arrow_Rain.png/revision/latest/scale-to-width-down/64?cb=20200130010659) | `+25%` area of effect and `+20%` damage per level |
| Ballista | ![Ballista](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/d5/Ballista.png/revision/latest/scale-to-width-down/64?cb=20200130003742) | `+1` bullet and `+20%` damage per level |

### MUL-T

| Skill | | Description |
|:-|-|------|
| Auto-Nailgun | ![Auto-Nailgun](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/ec/Auto-Nailgun.png/revision/latest/scale-to-width-down/64?cb=20200129200843) | `+20%` damage and `+4` bullets to final burst per level |
| Rebar Puncher | ![Rebar Puncher](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/1/10/Rebar_Puncher.png/revision/latest/scale-to-width-down/64?cb=20200129200906) | `+20%` faster wind up and `+20%` damage per level |
| Scrap Launcher | ![Scrap Launcher](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e6/Scrap_Launcher.png/revision/latest/scale-to-width-down/64?cb=20200130003342) | `+1` stock, `+20%` damage, and `+15%` blast radius per level |
| Power-Saw | ![Power-Saw](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/8e/Power-Saw.png/revision/latest/scale-to-width-down/64?cb=20200130003524) | `+20%` rate of fire, `+20%` damage, and `+30%` blade hitbox size per level |
| Blast Cannister | ![Blast Cannister](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/f/f2/Blast_Canister.png/revision/latest/scale-to-width-down/64?cb=20200129200845) | `+3` child bombs, `+20%` spread radius, `+20%` child blast radius, and `+20%` damage per level |
| Transport Mode | ![Transport Mode](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/85/Transport_Mode.png/revision/latest/scale-to-width-down/64?cb=20200129200840) | `+10%` duration, `+10%` speed, and `+30%` damage per level |
| Swap | ![Swap](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/8d/Retool.png/revision/latest/scale-to-width-down/64?cb=20200129200907) | Upon activating gain `+1 second` of bonus attack speed and `+1 second` equipment cooldown per level |

### Engineer

| Skill | | Description |
|:-|-|------|
| Bouncing Grenades | ![Bouncing Grenades](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/0/08/Bouncing_Grenades.png/revision/latest/scale-to-width-down/64?cb=20200129200847) | `+1` and `+2` to the minimum and maximum grenades fired per level |
| Pressure Mines | ![Pressure Mines](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9e/Pressure_Mines.png/revision/latest/scale-to-width-down/64?cb=20200306011730) | `-10%` arming time, `+20%` damage, `+20%` blast radius, and `+20%` trigger radius per level |
| Spider Mines | ![Spider Mines](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/61/Spider_Mines.png/revision/latest/scale-to-width-down/64?cb=20200130004739) | `+30%` activation range and `+20%` damage per level |
| Bubble Shield | ![Bubble Shield](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/da/Bubble_Shield.png/revision/latest/scale-to-width-down/64?cb=20200129200849) | `+15%` duration and `+15%` size per level |
| Thermal Harpoons | ![Thermal Harpoons](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b7/Thermal_Harpoons.png/revision/latest/scale-to-width-down/64?cb=20200331170409) | `+1` harpoon ammo, `+20%` damage, and `+30%` targetting range per level |
| TR12 Gauss Auto-Turret | ![TR12 Gauss Auto-Turret](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/60/TR12_Gauss_Auto-Turret.png/revision/latest/scale-to-width-down/64?cb=20200130010543) | `+25%` enemy detection range and `+10%` damage per level. `+1` deployable turret per two levels |
| TR58 Carbonizer Turret | ![TR58 Carbonizer Turret](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/4f/TR58_Carbonizer_Turret.png/revision/latest/scale-to-width-down/64?cb=20200130004828) | `+15%` damage, `+25%` proc chance, and `+10%` rate of fire per level. `+1` deployable turret per two levels |

### Artificer

| Skill | | Description |
|:-|-|------|
| Flame bolt | ![Flame bolt](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/5e/Flame_Bolt.png/revision/latest/scale-to-width-down/64?cb=20200129200858) | `+2` stock and `-10%` stock recharge time per level |
| Plasma Bolt | ![Plasma bolt](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/57/Plasma_Bolt.png/revision/latest/scale-to-width-down/64?cb=20200130005015) | `+2` stock and `-10%` stock recharge time per level |
| Charged Nano-Bomb | ![Charged nano-bomb](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/c/c8/Charged_Nano-Bomb.png/revision/latest/scale-to-width-down/64?cb=20200129200850) | `+20%` damage and `+10%` max charge time per level |
| Charged Nano-Spear | ![Charged nano-spear](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/92/Cast_Nano-Spear.png/revision/latest/scale-to-width-down/64?cb=20200130005104) | `+20%` damage and `+10%` max charge time per level |
| Snapfreeze | ![Snapfreeze](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/c/ce/Snapfreeze.png/revision/latest/scale-to-width-down/64?cb=20200130005438) | `+30%` wall damage, `+15%` wall duration, and `+20%` wall length per level |
| Flamethrower | ![Flamethrower](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/a/ad/Flamethrower.png/revision/latest/scale-to-width-down/64?cb=20200129200859) | `+20%` range, `+25%` flame radius, and `+20%` damage per level |
| Ion Surge | ![Ion Surge](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/d1/Ion_Surge.png/revision/latest/scale-to-width-down/64?cb=20200130005149) | `+25%` damage and `+30%` area of effect per level |

### Mercenary

| Skill | | Description |
|:-|-|------|
| Laser Sword | ![Laser Sword](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e2/Laser_Sword.png/revision/latest/scale-to-width-down/64?cb=20200129200901) | `+20%` damage, and `+15%` attack speed per level |
| Whirlwind | ![Whirlwind](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/e/e0/Whirlwind.png/revision/latest/scale-to-width-down/64?cb=20200129200841) | `+25%` damage and `+25%` larger hitbox per level |
| Rising Thunder | ![Rising Thunder](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/d/d4/Rising_Thunder.png/revision/latest/scale-to-width-down/64?cb=20200201131624) | `+25%` damage per level. `+1` stock per two levels |
| Blinding Assault | ![Blinding Assault](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/87/Blinding_Assault.png/revision/latest/scale-to-width-down/64?cb=20200129200845) | `+20%` damage, `+0.5 seconds` extra delay before reset |
| Eviscerate | ![Eviscerate](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9f/Eviscerate.png/revision/latest/scale-to-width-down/64?cb=20200129200857) | `+30%` chain range, `+15%` attack speed, `+10%` proc chance per level |
| Slicing Winds | ![Slicing Winds](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/bc/Slicing_Winds.png/revision/latest/scale-to-width-down/64?cb=20200201131723) | `+20%` attack speed and `+20%` damage per level |

### REX

| Skill | | Description |
|:-|-|------|
| Natural Toxins | ![Natural Toxins](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/57/DIRECTIVE_Inject.png/revision/latest/scale-to-width-down/64?cb=20200129200852) | `+1` syringe and `+10%` damage per level |
| DIRECTIVE: Drill | ![DIRECTIVE: Drill](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/f/fc/DIRECTIVE_Drill.png/revision/latest/scale-to-width-down/64?cb=20200201131130) | `+10%` damage, `+10%` duration, and `+20%` radius per level |
| Seed Barrage | ![Seed Barrage](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b0/Seed_Barrage.png/revision/latest/scale-to-width-down/64?cb=20200129200907) | `+20%` radius and `+20%` damage per level |
| DIRECTIVE: Disperse | ![DIRECTIVE: Disperse](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/22/DIRECTIVE_Disperse.png/revision/latest/scale-to-width-down/64?cb=20200129200851) | `+30%` range, `+20%` angle, `+20%` debuff duration per level |
| Bramble Volley | ![Bramble Volley](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/63/Bramble_Volley.png/revision/latest/scale-to-width-down/64?cb=20200201131036) | `+20%` range, `+20%` angle, `+20%` damage per level |
| Tangling Growth | ![Tangling Growth](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9d/Tangling_Growth.png/revision/latest/scale-to-width-down/64?cb=20200129200918) | `+20%` damage, `+20%` radius, `+10%` healing rate per level |

### Loader

| Skill | | Description |
|:-|-|------|
| Knuckleboom | ![Knuckleboom](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b2/Knuckleboom.png/revision/latest/scale-to-width-down/64?cb=20200130011411) | `+1%` barrier gained and `+20%` damage per level |
| Grapple Fist | ![Grapple Fist](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/25/Grapple_Fist.png/revision/latest/scale-to-width-down/64?cb=20200130011410) | `+30%` hook range per level. |
| Spiked Fist | ![Spiked Fist](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/62/Spiked_Fist.png/revision/latest/scale-to-width-down/64?cb=20200201131735) | `+15%` hook range and `+20%` damage per level |
| Charged Gauntlet | ![Charged Gauntlet](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/7/79/Charged_Gauntlet.png/revision/latest/scale-to-width-down/64?cb=20200130011409) | `+10%` max charge, `+20%` base damage, and `+15%` velocity based damage per level |
| Thunder Gauntlet | ![Thunder Gauntlet](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/f/f3/Thunder_Gauntlet.png/revision/latest/scale-to-width-down/64?cb=20200130011407) | `+15%` damage and `+20%` cone range per level |
| M551 Pylon | ![M551 Pylon](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/37/M551_Pylon.png/revision/latest/scale-to-width-down/64?cb=20200130011403) | `+20%` damage, `+20%` range, and `+20%` rate of fire per level |

### Acrid

| Skill | | Description |
|:-|-|------|
| Vicious Wounds | ![Vicious Wounds](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/5/5a/Vicious_Wounds.png/revision/latest/scale-to-width-down/64?cb=20200130005202) | `+20%` normal damage, `+25%` combo finisher damage, and `+15%` attack speed per level |
| Neurotoxin | ![Neurotoxin](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/2/2b/Neurotoxin.png/revision/latest/scale-to-width-down/64?cb=20200130005160) | `+20%` damage, `+30%` projectile speed, and `+30%` blast radius per level. `+1 stock` per two levels |
| Ravenous Bite | ![Ravenous Bite](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/8/8b/Ravenous_Bite.png/revision/latest/scale-to-width-down/64?cb=20200331165624) | `+20%` damage and `+1` stock per level |
| Caustic Leap | ![Caustic Leap](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9e/Caustic_Leap.png/revision/latest/scale-to-width-down/64?cb=20200130005158) | `+20%` blast damage, `+30%` acid pool damage, and `+20%` acid pool size per level |
| Frenzied Leap | ![Frenzied Leap](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/96/Frenzied_Leap.png/revision/latest/scale-to-width-down/64?cb=20200130005159) | `+25%` blast damage and `+15%` refunded time per level |
| Epidemic | ![Epidemic](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/9/9d/Epidemic.png/revision/latest/scale-to-width-down/64?cb=20200130005159) | `+5` infection bounces, `+20%` infection range, and `+20%` on hit damage per level |

### Captain

| Skill | | Description |
|:-|-|------|
| Vulcan Shotgun | ![Vulcan Shotgun](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b9/Vulcan_Shotgun.png/revision/latest/scale-to-width-down/64?cb=20200811174716) | `+20%` shell count, `+10%` damage per level |
| Power Tazer | ![Power Tazer](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/b/b8/Power_Tazer.png/revision/latest/scale-to-width-down/64?cb=20200811174715) | `+40%` blast radius, `+20%` damage per level |
| Orbital Probe | ![Orbital Probe](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/4/4e/Orbital_Probe.png/revision/latest/scale-to-width-down/64?cb=20200811174713) | `+20%` blast radius, `+20%` damage per level |
| Beacon: Healing | ![Beacon: Healing](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/1/1e/Beacon_Healing.png/revision/latest/scale-to-width-down/64?cb=20200812024640) | `+20%` healing range,  |
| Beacon: Shocking | ![Beacon: Shocking](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/c/c4/Beacon_Shocking.png/revision/latest/scale-to-width-down/64?cb=20200812024643) | `+30%` shock range per level |
| Beacon: Resupply | ![Beacon: Resupply](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/6/67/Beacon_Resupply.png/revision/latest/scale-to-width-down/64?cb=20200812024641) | `+1` resupply stock per level |
| Beacon: Hacking | ![Beacon: Hacking](https://static.wikia.nocookie.net/riskofrain2_gamepedia_en/images/3/38/Beacon_Hacking.png/revision/latest/scale-to-width-down/64?cb=20200812024640) | `+20%` hacking speed, `+20%` hacking range per level |
