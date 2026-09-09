# ComfortMotion

A small, focused MelonLoader mod for **How to Fish** that removes the camera
motion that triggers motion sickness and stalls boats onto calm water.
Everything is toggleable from a single in-game menu.

It is a **standing-alone comfort kit**: only the 11 motion-comfort features,
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
| Smooth Camera | Smooths vertical camera motion (smooth-damped); optional smooth turning (yaw) |
| Steady Tool | Keeps the held tool/rod steady (no sway/bob/look wobble) |
| Lock ADS FOV | Keeps the camera zoomed at your base FOV while aiming down sights |
| Reduce Post Effects | Turns off motion blur, chromatic aberration and film grain (depth of field / vignette optional) |
| Comfort Pack | One switch that enables the whole comfort stack, restores your previous settings when turned off |
| Flatten Waves | Flattens the ocean (physics + visuals) so the boat stops rocking |
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
3. Launch the game. A "ComfortMotion loaded with 11 comfort features" line
   appears in the MelonLoader log (the console window that opens alongside
   the game).

## Usage

- Press **F5** to open/close the ComfortMotion menu.
- Toggle any feature on or off. Toggles persist between sessions.
- **Smooth Camera** also has a *Smoothing* slider and a *Smooth turn (yaw)*
  option.
- **Reduce Post Effects** has per-effect toggles (motion blur, chromatic
  aberration, film grain, depth of field, vignette).
- **Comfort Pack** enables all of the above in one click; turning it off
  restores whatever you had before.
- **Disable all** turns everything off at once (handy as a panic button).
- The menu key can be changed in the MelonLoader preferences file under the
  `ComfortMotion` category (`menu.key`, default `F5`).

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