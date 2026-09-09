using System.Collections.Generic;
using ComfortMotion.Core;
using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace ComfortMotion.Features
{
    public sealed class ReducePostEffectsFeature : Feature
    {
        private const float ScanInterval = 1f;

        private readonly Dictionary<VolumeComponent, bool> _originals = new Dictionary<VolumeComponent, bool>();

        private float _nextScan;

        public ReducePostEffectsFeature()
        {
            DisableMotionBlur = Preferences.Category.CreateEntry(Id + ".disableMotionBlur", true, "Disable motion blur");
            DisableChromaticAberration = Preferences.Category.CreateEntry(Id + ".disableChromaticAberration", true, "Disable chromatic aberration");
            DisableFilmGrain = Preferences.Category.CreateEntry(Id + ".disableFilmGrain", true, "Disable film grain");
            DisableDepthOfField = Preferences.Category.CreateEntry(Id + ".disableDepthOfField", false, "Disable depth of field");
            DisableVignette = Preferences.Category.CreateEntry(Id + ".disableVignette", false, "Disable vignette");
        }

        public override string Id => "reducePostEffects";

        public override string Name => "Reduce Post Effects";

        public MelonPreferences_Entry<bool> DisableMotionBlur { get; }

        public MelonPreferences_Entry<bool> DisableChromaticAberration { get; }

        public MelonPreferences_Entry<bool> DisableFilmGrain { get; }

        public MelonPreferences_Entry<bool> DisableDepthOfField { get; }

        public MelonPreferences_Entry<bool> DisableVignette { get; }

        public override void DrawOptions()
        {
            if (!Enabled.Value)
            {
                return;
            }

            DisableMotionBlur.Value = GUILayout.Toggle(DisableMotionBlur.Value, "Motion blur");
            DisableChromaticAberration.Value = GUILayout.Toggle(DisableChromaticAberration.Value, "Chromatic aberration");
            DisableFilmGrain.Value = GUILayout.Toggle(DisableFilmGrain.Value, "Film grain");
            DisableDepthOfField.Value = GUILayout.Toggle(DisableDepthOfField.Value, "Depth of field");
            DisableVignette.Value = GUILayout.Toggle(DisableVignette.Value, "Vignette");
        }

        public override void Tick()
        {
            if (!IsActive || Time.time < _nextScan)
            {
                return;
            }

            _nextScan = Time.time + ScanInterval;

            foreach (var volume in Object.FindObjectsByType<Volume>())
            {
                if (volume == null || volume.profile == null)
                {
                    continue;
                }

                foreach (var component in volume.profile.components)
                {
                    if (component == null || !ShouldDisable(component))
                    {
                        continue;
                    }

                    if (!_originals.ContainsKey(component))
                    {
                        _originals.Add(component, component.active);
                    }

                    component.active = false;
                }
            }
        }

        protected override void OnDeactivated()
        {
            foreach (var pair in _originals)
            {
                if (pair.Key != null)
                {
                    pair.Key.active = pair.Value;
                }
            }

            _originals.Clear();
        }

        private bool ShouldDisable(VolumeComponent component)
        {
            switch (component)
            {
                case MotionBlur when DisableMotionBlur.Value:
                    return true;
                case ChromaticAberration when DisableChromaticAberration.Value:
                    return true;
                case FilmGrain when DisableFilmGrain.Value:
                    return true;
                case DepthOfField when DisableDepthOfField.Value:
                    return true;
                case Vignette when DisableVignette.Value:
                    return true;
                default:
                    return false;
            }
        }
    }
}