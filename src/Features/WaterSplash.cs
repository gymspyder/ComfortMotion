using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class WaterSplashFeature : Feature
    {
        public override string Id => "disableWaterSplash";

        public override string Name => "Disable Water Splash";
    }

    internal static class ParticleStop
    {
        public static void Stop(ParticleSystem[] systems)
        {
            if (systems == null)
            {
                return;
            }

            foreach (var particleSystem in systems)
            {
                if ((bool)particleSystem && particleSystem.isPlaying)
                {
                    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
    }

    [HarmonyPatch(typeof(ParticleManager), "Play", typeof(string), typeof(Vector3), typeof(Vector3))]
    internal static class ParticleManagerSplashPatch
    {
        private static bool Prefix(string type)
        {
            if (type != "WaterSplash")
            {
                return true;
            }

            var feature = Registry.Find<WaterSplashFeature>();
            return feature == null || !feature.IsActive;
        }
    }

    [HarmonyPatch(typeof(VFXManager), "Play", typeof(string), typeof(Vector3), typeof(Vector3))]
    internal static class VfxManagerSplashPatch
    {
        private static bool Prefix(string type)
        {
            if (type != "WaterSplash")
            {
                return true;
            }

            var feature = Registry.Find<WaterSplashFeature>();
            return feature == null || !feature.IsActive;
        }
    }

    [HarmonyPatch(typeof(Boat), "ToggleParticles")]
    internal static class BoatWakePatch
    {
        private static readonly AccessTools.FieldRef<Boat, ParticleSystem[]> WhenUnderwater =
            AccessTools.FieldRefAccess<Boat, ParticleSystem[]>("_particlesWhenUnderwater");

        private static bool Prefix(Boat __instance)
        {
            var feature = Registry.Find<WaterSplashFeature>();
            if (feature == null || !feature.IsActive)
            {
                return true;
            }

            ParticleStop.Stop(WhenUnderwater(__instance));
            return false;
        }
    }

    [HarmonyPatch(typeof(Boat), "TogglePropellerParticles")]
    internal static class BoatPropellerPatch
    {
        private static readonly AccessTools.FieldRef<Boat, ParticleSystem[]> WhenPropellerInWater =
            AccessTools.FieldRefAccess<Boat, ParticleSystem[]>("_particlesWhenPropellerInWater");

        private static bool Prefix(Boat __instance)
        {
            var feature = Registry.Find<WaterSplashFeature>();
            if (feature == null || !feature.IsActive)
            {
                return true;
            }

            ParticleStop.Stop(WhenPropellerInWater(__instance));
            return false;
        }
    }
}