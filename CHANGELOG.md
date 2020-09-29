# Changelog

## Version 0.2.0

- Added mutliplayer support

## Version 0.1.5

- Refactored internal code to make in more stable for future development
- Fixed mercenary's primary attack not upgrading
- Fixed REX's `DIRECTIVE: Disperse` and `Bramble Volley` skills pulling enemies instead of pushing them away when the skill is levelled up
- Fixed upgrading MUL-T's `Rebar Puncher` not affecting the rate of fire.
- Fixed support for [rob's Aatrox](https://thunderstore.io/package/rob/Aatrox/)
- Fixed Hungering Gaze having insanely high scaling.

## Version 0.1.4

Changes:

- Added Captain upgrades. All vanilla characters are now done!!!!
- Added upgrades for Hungering Gaze. It was a stink one to pick up Visions of Heresy and lose upgrades. They now transfer onto the item.
- Fixed REX's primary fire having too much spread. The base value has reduced back to normal.
- Fixed Huntress's flurry to fire the arrows faster before the ability is cancels.
- Added extra time to Huntress's ballista so the player has more time to take aim and fire all shots.

## Version 0.1.3

Changes:

- Added REX upgrades
- Added ingame setting to change the levels between skill points
- Updated R2API dependancy to 2.5.7

## Version 0.1.2

Changes:

- Removed need for Rewired MonoMod dlls

## Version 0.1.1

Changes:

- Added instructions for console command usage

## Version 0.1.0

Changes:

- Fixed mod to work with changes in the game's 1.0.0 release.
- Added support for controllers to spend skill points. Button is rebindable in gamepad options.
- **Added support for other modded characters to integrate with Skills++.**
- Added console command to disable Skills++ for chosen survivors. This is for all of the [Sniper](https://thunderstore.io/package/Rein/Sniper/) players out there.
- Removed limits on upgrading skills.
- Reduced the rate of acquiring skill points to one per 5 levels.
- Changed percentage/multiplication compound the scaling. As an adjustment some scalings have been pulled back to compensate.
- Removed Blinding Assault cooldown reduction now the mechanism is core to Mercenary's loadout.
- Changed UI to show the upgrades as a number rather than as multiple '+' symbols
- Removed MUL-T's swap duration reduction scaling in favour of a new equipment cooldown buff.

## Version 0.1.0-rc1

Changes:
- Prelease of public API for modded character support

## Version 0.0.11

Changes:

- Added upgrades for Loader
- Improved readme's mod usage section with images.
- Added FAQ section to readme

Bug fixes:

- [#31](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/31) - Fixed Mercenary's eviscerate not applying upgrades
- [#33](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/33) - Fixed Acrid's caustic leap ability having only one charge instead of two.

## Version 0.0.10

Changes:

- Added upgrades for Mercenary
- Replaced `+2.5%` bounce damage from Huntress's Glaive skill with `+10%` flat damage bonus. The bonus damage per bounce made the skill far to over powered as the skill levelled up.
- Added support across the remaining characters for [Classic Items's](https://thunderstore.io/package/ThinkInvis/ClassicItems/) scepter item

Bug fixes:

- Fixed a null pointer happening on launch

## Version 0.0.9

Changes:

- Added upgrades for all of Acrid's abilities

Bug fixes:

- [#28](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/28) - Improved mod compatibility for engi's turrets when using [Classic Items](https://thunderstore.io/package/ThinkInvis/ClassicItems/)

[Changelog](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/master/CHANGELOG.md)

## Version 0.0.8

Changes:

* Removed Engi's thermal harpoon target paint duration per missile
* Added `+50%` targetting range to Engi's thermal harpoon

Bug fixes:

* [#20](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/20), [#22](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/22) - Fixed Acrid's passive ability not working

## Version 0.0.7

Changes:

* Added MULT skill upgrades

Bug fixes:

* [#19](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/19) - Fixed Huntress's arrow rain not dealing any damage
* Fixed robustness of upgrade icons showing for the correct skills

## Version 0.0.6

Changes:

* Added Engineer's skill upgrades
* Buffed Commando's Double tap rate of fire bonus. `+15%` -> `+20%`
* Nerfed Huntress's glaive bound bonus from `+2` to `+1` per level
* Added `+25%` damage to Artificer's Nano-nomb and Nano-spear. Needed more power than just longer charge time
* Buffed Artificer's Flame bolt and Plasma bolt to recharge slightly faster per level

Bug fixes:

* [#13](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/13), [#14](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/14) - Fixed bug where travelling to the Bazaar or Void Plains would reverts the character's loadout to defaults.

## Version 0.0.5

Changes:

* Released Commando's skill upgrade kit

Bug fixes:

* Fixed bug where Huntress's flurry wouldn't fire the correct number of arrows when critting.
* [#12](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/12) - Fixed Artificer's flamethrower dealign 1 damage per tick and not scaling with upgrades purchased.

## Version 0.0.4

Changes:

* Completed upgrade paths for all Huntress abilities
* Improved internal skill upgrade API to support character buffing

## Version 0.0.3

Changes:

* Released upgrade paths for all Artificer abilities.
* Refactored mechism for applying upgrades to abilities. Will make future work easier and much more powerful.

Bug Fixes:

* Removed upgrading for Commando's tactical roll since there is no upgrade path yet

## Version 0.0.2

Bug fixes:

* [#1](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/1) - Fixed host in multiplayer games not being able to buy upgrades
* [#2](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/2) - Fixed MUL-T, and Acrid characters not selecting correctly in character select screen
* [#3](https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/issues/3) - Fixed Commando's special ability diplaying the wring description
* Fixed some skill upgrades carrying over between runs
* Cleaned up print statements

## Version 0.0.1

Initial release of mod
