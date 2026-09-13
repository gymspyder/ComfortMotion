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
- **Steady Tool** patches `PlayerToolMovement.LookAround` (prefix,
  `_lookRot` + `_toolSwayTarget.parent.localEulerAngles` zero, return-false),
  `Bob` (postfix, `_tiltRot` zero) and `Sway` (prefix, `_swayPos`/`_swayRot`
  zero, return-false) to kill the mouse-driven tool bend/roll.
- **Walk Lock** patches `PlayerToolMovement.Bob` (postfix,
  `_bobPos = Vector2.zero`), `PlayerHands.SetHandBob` (postfix,
  `_curTpHandBob = 0f`) and `PlayerHolding.GetBobOffset` (postfix,
  `_bobOffset = Vector3.zero`). Those are the three per-step walk-bob
  sources: the tool rig's `_bobPos` (held tool/rod), the empty-hand fist bob
  (`Legs.BodyBobDownPercent() * _tpHandBobbingAmount`, applied as
  `Vector3.down * _curTpHandBob` in `CurDefaultHandPos`), and the held-item
  bob (`_player.Camera.BobPos * HoldBobMulti`, applied around the hold target
  in `MoveItemToHoldPosRot` — this is what bobs a fetched fish). All of them
  keep riding the camera's terrain height and fall offsets, so they're
  planted while walking and follow the terrain up and down.
- **Hide Empty Hands** patches `PlayerHands.LateUpdate` (postfix) to force the
  hand meshes (`_handModelRight/Left`) off whenever the player is empty-handed
  and not animating a punch (`PlayerPunching.IsAnimatingRight/Left`), then on
  again while punching. The enforcement is gated to skip held items, thrown
  items, boat driving, and the prepared pickup/drop poses (which need hands
  visible for their transitions). This removes empty-fist floating entirely —
  including the odd dip/pump the default hand pose does when the player
  collides with walls or the `_curHeadAngle` spring overshoots.
- **Lock ADS FOV** patches `PlayerCamera.SetFov` (prefix, return-false) only
  while a held item's weapon reports `Weapon.IsAds`. It writes `_curFov` and
  the camera FOV back to the base `_origFov`, cancelling the per-ADS zoom
  dip (and the resulting sensitivity change).
- **Aim Crosshair** is a pure OnGUI overlay (no game patch). It reads the
  weapon's private `_spread` (the same degrees used in `Weapon.Shoot`:
  `Quaternion.Euler(randomUnitBall * _spread)`) and draws a 4-line crosshair
  whose gap equals the spread cone projected at the camera's FOV, so the
  crosshair matches the shot width. A scoped sniper fully aimed
  (`Attachments.UseSniperUi` and `_aimPercent > 0.9`, when the game bypasses
  spread and shows its own scope reticle) hides the crosshair. Options: a
  *Size* slider.

## Water

- **Flatten Waves** zeroes the two wave heights on the `WaterManager`
  controller and the `WaterManager` water material (`_Wave_Height_1` /
  `_Wave_Height_2`), and sets the boat's buoyancy spring damping to maximum
  (`Boat._boatBounciness = 0`). Because boat buoyancy, item buoyancy and
  underwater checks all read those heights, the ocean becomes physically flat
  (no boat rocking). The bounciness fix is what stops the boat from bobbing
  even on flat water — flattening the surface alone leaves the boat's own
  springy buoyancy (default 0.35) bouncing it. A `Boat.FixedUpdate` postfix
  (`CalmBoatPatch`) additionally damps the local boat's vertical velocity and
  pitch/roll angular velocity to 0.1 per physics step, killing the residual
  porpoising that still occurs at planing speed (the boat rides shallow there,
  so its buoyancy cap shrinks and the spring weakens). That residual surge is
  also what made the propeller breach the surface, flicking the engine-sound
  pitch target (`_motorSoundPitchInAirMulti`) and cutting motor thrust
  (`__propellerInWater`) — the "audio lag" the bounce caused is gone with the
  bounce; forward drive (horizontal velocity + yaw) is untouched. Restores the
  captured values when toggled off.
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