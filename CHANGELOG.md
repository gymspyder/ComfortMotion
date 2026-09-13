# Changelog

All notable changes to ComfortMotion will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2026-09-12

### Added

- **Aim Crosshair** feature: 4-line crosshair whose gap matches the weapon's
  actual shot spread (`_spread` cone projected at the current FOV), with a
  Size slider. Hidden while a scoped sniper uses its own reticle.
- **Walk Lock** feature (part of Comfort Pack): removes the per-step walk bob
  from your hands so they stay still while walking and move up/down with the
  terrain instead.
- **Hide Empty Hands** feature (on by default): empty fists are hidden and
  only reappear while punching (gated off for held items, throws, boat
  driving and pickup/drop transitions) — eliminates the fist float and the
  odd pump when bumping into geometry.
- **Enable all** button next to "Disable all".
- Settings now persist reliably: all MelonLoader preference entry ids were
  renamed to be dot-free, fixing a Tomlet serialization crash
  (`TomlNoSuchValueException` on `menu.key`) that silently discarded the whole
  `[ComfortMotion]` section in `MelonPreferences.cfg`.

### Fixed

- F5 menu now opens in a running world (hotkey polls the new Input System, the
  game runs InputSystem-only).
- Menu toggles are clickable in-game: the dark menu forces the cursor
  unlocked every frame and skips the game's per-click cursor re-lock
  (`PlayerCamera.MouseClick` patch).
- Menu is no longer too transparent — dark window with a dimming overlay.
- Removed "Smooth turn (yaw)" — heading lag when looking left/right increased
  motion sickness.
- Loading-screen water is flattened too (feature toggles no longer require a
  loaded world / `Server.Instance`).
- Feature toggles are saved to the MelonLoader preferences file immediately,
  so selections persist between world loads.

### Removed

- **Smooth Camera** feature removed. Smoothing the vertical axis either
  chopped on stairs (snap-bands), floated on falls/jumps (laggy trailing), or
  both — no tuning got the trade-off right, so the feature is gone entirely.

## [1.0.0] - 2026-09-09

### Added

- Initial release.
- 11 comfort features: Disable Head Bobbing, Disable Camera Sway, Disable
  Screen Shake, Disable Water Splash, Smooth Camera, Steady Tool, Lock ADS
  FOV, Reduce Post Effects (+ per-effect toggles), Comfort Pack, Flatten
  Waves, Disable Tree Sway.
- In-game menu (F5) with per-feature toggles and sliders.
- Settings persisted via MelonLoader preferences.