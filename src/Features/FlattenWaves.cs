using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class FlattenWavesFeature : Feature
    {
        private static readonly int WaveHeight1Id = Shader.PropertyToID("_Wave_Height_1");

        private static readonly int WaveHeight2Id = Shader.PropertyToID("_Wave_Height_2");

        private static readonly AccessTools.FieldRef<WaterManager> ManagerInstance =
            AccessTools.StaticFieldRefAccess<WaterManager>(AccessTools.Field(typeof(WaterManager), "_instance"));

        private static readonly AccessTools.FieldRef<WaterManager, Material> WaterMaterial =
            AccessTools.FieldRefAccess<WaterManager, Material>("_waterMat");

        private static readonly AccessTools.FieldRef<float> SimulatedHeight1 =
            AccessTools.StaticFieldRefAccess<float>(AccessTools.Field(typeof(WaterManager), "_waveHeight1"));

        private static readonly AccessTools.FieldRef<float> SimulatedHeight2 =
            AccessTools.StaticFieldRefAccess<float>(AccessTools.Field(typeof(WaterManager), "_waveHeight2"));

        private Material _capturedMaterial;
        private float _defaultHeight1;
        private float _defaultHeight2;

        public override string Id => "flattenWaves";

        public override string Name => "Flatten Waves";

        public override void Tick()
        {
            if (!IsActive || !TryResolveMaterial(out var material))
            {
                return;
            }

            material.SetFloat(WaveHeight1Id, 0f);
            material.SetFloat(WaveHeight2Id, 0f);
            SimulatedHeight1() = 0f;
            SimulatedHeight2() = 0f;
        }

        protected override void OnDeactivated()
        {
            if (!(bool)_capturedMaterial)
            {
                return;
            }

            _capturedMaterial.SetFloat(WaveHeight1Id, _defaultHeight1);
            _capturedMaterial.SetFloat(WaveHeight2Id, _defaultHeight2);
            SimulatedHeight1() = _defaultHeight1;
            SimulatedHeight2() = _defaultHeight2;
        }

        private bool TryResolveMaterial(out Material material)
        {
            var manager = ManagerInstance();
            material = (bool)manager ? WaterMaterial(manager) : null;

            if (!(bool)material)
            {
                return false;
            }

            if (_capturedMaterial != material)
            {
                _capturedMaterial = material;
                _defaultHeight1 = material.GetFloat(WaveHeight1Id);
                _defaultHeight2 = material.GetFloat(WaveHeight2Id);
            }

            return true;
        }
    }
}