# Game notes (behaviour changes)

What this mod does to the game, feature by feature. Written so the community
(and the author) always knows what each toggle changes, and what to re-check
when the game updates.

## Camera motion

- **Disable Head Bobbing** patches `PlayerCamera.HeadBobbing` (prefix,
  return-false). This method is the only writer of `_bobPos` and
  `_delayedBobPos` (position-only bob). Side effect by design: the footstep
  bob audio that is driven from the same method no longer plays while enabled.
- **Disable Camera Sway** patches `PlayerCamera.HeadRot` (prefix,
  return-false). Cleanly removes the fall-tilt and strafe-roll (`_moveRot`).
  Does not touch yaw/pitch input.
- **Disable Screen Shake** patches `PlayerCamera.SetShakePos` (prefix,
  return-false). Removes the world shake fed through `ShakePos`, so
  explosions/boss animations stop shaking the camera. Weapon view-model shake
  is intentionally untouched.
- **Smooth Camera** patches `PlayerCamera.SetCamPosRot` (postfix).
  `position.y` is smooth-damped toward the current value so rock/stair climbs
  and boat heave don't snap the view. With the optional *Smooth turn (yaw)*
  on, `eulerAngles.y` is smooth-damped using a `Mathf.DeltaAngle` unwrap so a
  fast mouse flick glides instead of snapping, and sweeping across the
  -180/180 degree seam doesn't spin the view.
- **Lock ADS FOV** patches `PlayerCamera.SetFov` (prefix, return-false) only
  while a held item's weapon reports `Weapon.IsAds`. It writes `_curFov` and
  the camera FOV back to the base `_origFov`, cancelling the per-ADS zoom
  dip (and the resulting sensitivity change).

## Water

- **Flatten Waves** zeroes the two wave heights on the `WaterManager`
  controller and the `WaterManager` water material (`_Wave_Height_1` /
  `_Wave_Height_2`). Because boat buoyancy, item buoyancy and underwater
  checks all read those heights, the ocean becomes physically flat (no boat
  rocking). Restores the captured values when toggled off.
- **Disable Water Splash** blocks the `WaterSplash` particle event at both
  funnel points (`ParticleManager.Play` and `VFXManager.Play`, 3-arg
  overloads) and stops boat wake (`Boat.ToggleParticles`,
  `_particlesWhenUnderwater`) and propeller froth
  (`Boat.TogglePropellerParticles`, `_particlesWhenPropellerInWater`). Splash
  **audio** is unrelated and still plays.

## World

- **Disable Tree Sway** scans renderers every second and, for any whose
  shader name contains `Windy`, zeroes `_WindStrength`, `_WindScale`,
  `_WindSpeed`, `_WindSpeed2` and `_Sway`. This freezes the wind-driven sway
  of trees, grass and plants. Original values are captured on first touch and
  restored when toggled off.

## Post processing

- **Reduce Post Effects** scans `UnityEngine.Rendering.Volume` profiles every
  second and flips `VolumeComponent.active` off for motion blur, chromatic
  aberration and film grain (on by default), and for depth of field and
  vignette (opt-in). Original `active` states are restored when toggled off.
  If the current scene/map has no volumes configured, nothing visibly changes.

## Meta

- **Comfort Pack** is a no-patch meta toggle: when enabled it records (and, if
  needed, force-enables) the other eight comfort features; when disabled it
  restores exactly what you had before the pack was switched on. A manual
  toggle of a member while the pack is on will be reset to the pre-pack state
  when the pack is turned off.