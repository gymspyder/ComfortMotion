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

        private static readonly AccessTools.FieldRef<Boat, float> BoatBounciness =
            AccessTools.FieldRefAccess<Boat, float>("_boatBounciness");

        private Material _capturedMaterial;
        private float _defaultHeight1;
        private float _defaultHeight2;
        private Boat _capturedBoat;
        private float _capturedBounciness;

        public override string Id => "flattenWaves";

        public override string Name => "Flatten Waves";

        public override void Tick()
        {
            if (!IsActive)
            {
                return;
            }

            if (TryResolveMaterial(out var material))
            {
                material.SetFloat(WaveHeight1Id, 0f);
                material.SetFloat(WaveHeight2Id, 0f);
                SimulatedHeight1() = 0f;
                SimulatedHeight2() = 0f;
            }

            CalmBoat();
        }

        protected override void OnDeactivated()
        {
            if ((bool)_capturedMaterial)
            {
                _capturedMaterial.SetFloat(WaveHeight1Id, _defaultHeight1);
                _capturedMaterial.SetFloat(WaveHeight2Id, _defaultHeight2);
                SimulatedHeight1() = _defaultHeight1;
                SimulatedHeight2() = _defaultHeight2;
            }

            if ((bool)_capturedBoat)
            {
                BoatBounciness(_capturedBoat) = _capturedBounciness;
            }
        }

        private void CalmBoat()
        {
            var boat = BoatManager.Boat;
            if (!(bool)boat)
            {
                return;
            }

            if (_capturedBoat != boat)
            {
                _capturedBoat = boat;
                _capturedBounciness = BoatBounciness(boat);
            }

            BoatBounciness(boat) = 0f;
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

    [HarmonyPatch(typeof(Boat), "FixedUpdate")]
    internal static class CalmBoatPatch
    {
        private const float VerticalKeep = 0.1f;

        private const float RockKeep = 0.1f;

        private static void Postfix(Boat __instance)
        {
            var feature = Registry.Find<FlattenWavesFeature>();
            if (feature == null || !feature.IsActive || !(bool)BoatManager.Boat || BoatManager.Boat != __instance)
            {
                return;
            }

            var rig = __instance.HiddenPhysicsRig;
            if (!(bool)rig)
            {
                return;
            }

            var linear = rig.linearVelocity;
            linear.y *= VerticalKeep;
            rig.linearVelocity = linear;

            var angular = rig.angularVelocity;
            angular.x *= RockKeep;
            angular.z *= RockKeep;
            rig.angularVelocity = angular;
        }
    }
}