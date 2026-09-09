using System;
using System.Collections.Generic;
using ComfortMotion.Core;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class TreeSwayFeature : Feature
    {
        private const float ScanInterval = 1f;

        private static readonly int WindStrengthId = Shader.PropertyToID("_WindStrength");

        private static readonly int WindScaleId = Shader.PropertyToID("_WindScale");

        private static readonly int WindSpeedId = Shader.PropertyToID("_WindSpeed");

        private static readonly int WindSpeed2Id = Shader.PropertyToID("_WindSpeed2");

        private static readonly int SwayId = Shader.PropertyToID("_Sway");

        private readonly Dictionary<Material, StoredWind> _stored = new Dictionary<Material, StoredWind>();

        private float _nextScan;

        public override string Id => "disableTreeSway";

        public override string Name => "Disable Tree Sway";

        public override void Tick()
        {
            if (!IsActive || Time.time < _nextScan)
            {
                return;
            }

            _nextScan = Time.time + ScanInterval;

            foreach (var renderer in UnityEngine.Object.FindObjectsByType<Renderer>())
            {
                var material = renderer.sharedMaterial;
                if (!IsWindy(material))
                {
                    continue;
                }

                if (!_stored.TryGetValue(material, out var storedWind))
                {
                    storedWind = new StoredWind(material);
                    _stored.Add(material, storedWind);
                }

                storedWind.Apply(material);
            }

            RemoveStale();
        }

        protected override void OnDeactivated()
        {
            foreach (var pair in _stored)
            {
                if (pair.Key != null)
                {
                    pair.Value.Restore(pair.Key);
                }
            }

            _stored.Clear();
        }

        private static bool IsWindy(Material material)
        {
            return (bool)material && (bool)material.shader &&
                material.shader.name.IndexOf("Windy", StringComparison.Ordinal) >= 0;
        }

        private void RemoveStale()
        {
            if (_stored.Count == 0)
            {
                return;
            }

            List<Material> stale = null;
            foreach (var material in _stored.Keys)
            {
                if ((bool)material)
                {
                    continue;
                }

                stale ??= new List<Material>();
                stale.Add(material);
            }

            if (stale == null)
            {
                return;
            }

            foreach (var material in stale)
            {
                _stored.Remove(material);
            }
        }

        private sealed class StoredWind
        {
            private readonly float _strength;
            private readonly float _scale;
            private readonly float _speed;
            private readonly float _speed2;
            private readonly float _sway;

            public StoredWind(Material material)
            {
                _strength = material.GetFloat(WindStrengthId);
                _scale = material.GetFloat(WindScaleId);
                _speed = material.GetFloat(WindSpeedId);
                _speed2 = material.GetFloat(WindSpeed2Id);
                _sway = material.GetFloat(SwayId);
            }

            public void Apply(Material material)
            {
                material.SetFloat(WindStrengthId, 0f);
                material.SetFloat(WindScaleId, 0f);
                material.SetFloat(WindSpeedId, 0f);
                material.SetFloat(WindSpeed2Id, 0f);
                material.SetFloat(SwayId, 0f);
            }

            public void Restore(Material material)
            {
                material.SetFloat(WindStrengthId, _strength);
                material.SetFloat(WindScaleId, _scale);
                material.SetFloat(WindSpeedId, _speed);
                material.SetFloat(WindSpeed2Id, _speed2);
                material.SetFloat(SwayId, _sway);
            }
        }
    }
}