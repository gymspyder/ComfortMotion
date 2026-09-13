# ComfortMotion

A small, focused MelonLoader mod for **How to Fish** that reduces camera movement and visual effects that can contribute to motion discomfort and stalls boats onto calm water.
Everything is toggleable from a single in-game menu.

It is a **standing-alone comfort kit**: only the 13 motion-comfort features,
with its own minimal framework. It does not depend on, and can run without,
any other mod.

> This is an unofficial fan-made modification. It is not made by, affiliated
> with, or endorsed by Dazed Games. Use at your own risk.

## Features

| Feature | What it does |
|---------|--------------|
| Disable Head Bobbing | Removes the walk/run camera bob |
| Disable Camera Sway | Removes the strafe/fall camera tilt and roll |
| Disable Screen Shake | Removes explosion/boss screen shake |
| Disable Water Splash | Hides water splash particles (boat wake, propeller froth, item/projectile entry) — splash *audio* stays |
| Steady Tool | Keeps the held tool/rod steady (no sway/bob/look wobble) |
| Walk Lock | Keeps your hands still while walking (tool, empty hands, and held items like fish), moving up/down with the terrain instead of per-step bobbing |
| Hide Empty Hands | No fists floating in front of the screen — hands appear only when you punch (off when holding or looking at items) |
| Lock ADS FOV | Keeps the camera zoomed at your base FOV while aiming down sights |
| Reduce Post Effects | Turns off motion blur, chromatic aberration and film grain (depth of field / vignette optional) |
| Aim Crosshair | Draws a crosshair that matches the weapon's actual shot spread (size-adjustable) |
| Comfort Pack | One switch that enables the whole comfort stack, restores your previous settings when turned off |
| Flatten Waves | Calms the ocean — flat water AND a dead-stable boat (zeroes wave heights + maximizes buoyancy damping) |
| Disable Tree Sway | Freezes the wind-driven sway of trees, grass and plants |

## Requirements

- **How To Fish** (Steam, current build)
- **MelonLoader 0.7.x** — install it for How to Fish with the official
  MelonLoader installer. This mod goes in the game's **Mods** folder created
  by MelonLoader.

## Install

1. Close the game.
2. Copy `ComfortMotion.dll` into the game's **Mods** folder:
   - `.../Steam/steamapps/common/How to Fish/How to Fish/Mods/`
   - (some installs put it at `.../MelonLoader/Mods/` — use whichever one
     MelonLoader created)
3. Launch the game. A "ComfortMotion loaded with 13 comfort features" line
   appears in the MelonLoader log (the console window that opens alongside
   the game).

## Usage

- Press **F5** to open/close the ComfortMotion menu.
- Toggle any feature on or off. Toggles persist between sessions (and survive
  world reloads).
- **Reduce Post Effects** has per-effect toggles (motion blur, chromatic
  aberration, film grain, depth of field, vignette).
- **Comfort Pack** enables all of the above in one click; turning it off
  restores whatever you had before.
- **Disable all** turns everything off at once (handy as a panic button).
- **Enable all** turns everything on at once.
- **Steady Tool** kills the sway/look-wobble; **Walk Lock** additionally strips
  the per-step walk bob but keeps the terrain-following fall offsets.
- **Hide Empty Hands** (on by default) removes the floating fists entirely.
  They reappear while punching, and staying on during throw/drop/grab
  transitions and boat driving.
- **Aim Crosshair** has a *Size* slider; its gap matches the weapon's shot
  spread (a scoped sniper hides it and uses the game's own reticle).
- The menu key can be changed in the MelonLoader preferences file under the
  `ComfortMotion` category (`menuKey`, default `F5`).

## Configuration

All toggles are stored by MelonLoader's own preferences system
(`UserData/MelonPreferences.cfg`) — no separate config file to edit (but you
can if you like).

## Uninstall

Delete `ComfortMotion.dll` from the **Mods** folder. Nothing else is touched.

## Compatibility notes

- If you run another melon mod that modifies the same camera/water functions
  (e.g. a "comfort pack" feature in another mod), keep only one of them
  enabled at a time to avoid double-patching. This mod is fully self-contained
  and works without any other mod installed.

## Building from source

Requires the .NET SDK (8.0), the game's managed assemblies, and MelonLoader's
loader assemblies. The csproj resolves the game by default from a standard
Steam install; override the paths if yours differ:

```bash
dotnet build ComfortMotion.csproj -c Release \
  -p:GameDir="C:\Program Files (x86)\Steam\steamapps\common\How to Fish\How to Fish" \
  -p:ManagedDir="...\How to Fish_Data\Managed" \
  -p:MelonDir="...\MelonLoader\net35"
```

## Troubleshooting

- Mod not loading — check the MelonLoader console log for errors at startup.
- A feature "does nothing" — the game may have updated; features are verified
  against a specific game build. Report the issue with the version you see in
  the log.

## License & permissions

MIT — see [LICENSE](LICENSE). Source is provided; you may fork and modify it.
If you publish a derivative, it must not include any game assets.

## Changelog

See [CHANGELOG.md](CHANGELOG.md).
