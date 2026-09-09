using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class SteadyToolFeature : Feature
    {
        public override string Id => "steadyTool";

        public override string Name => "Steady Tool";
    }

    [HarmonyPatch(typeof(PlayerToolMovement), "Bob")]
    internal static class ToolStrafeTiltPatch
    {
        private static readonly AccessTools.FieldRef<PlayerToolMovement, float> TiltRot =
            AccessTools.FieldRefAccess<PlayerToolMovement, float>("_tiltRot");

        private static void Postfix(PlayerToolMovement __instance)
        {
            var feature = Registry.Find<SteadyToolFeature>();
            if (feature == null || !feature.IsActive)
            {
                return;
            }

            TiltRot(__instance) = 0f;
        }
    }

    [HarmonyPatch(typeof(PlayerToolMovement), "Sway")]
    internal static class ToolMouseSwayPatch
    {
        private static readonly AccessTools.FieldRef<PlayerToolMovement, Vector3> SwayPos =
            AccessTools.FieldRefAccess<PlayerToolMovement, Vector3>("_swayPos");

        private static readonly AccessTools.FieldRef<PlayerToolMovement, Vector3> SwayRot =
            AccessTools.FieldRefAccess<PlayerToolMovement, Vector3>("_swayRot");

        private static bool Prefix(PlayerToolMovement __instance)
        {
            var feature = Registry.Find<SteadyToolFeature>();
            if (feature == null || !feature.IsActive)
            {
                return true;
            }

            SwayPos(__instance) = Vector3.zero;
            SwayRot(__instance) = Vector3.zero;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerToolMovement), "LookAround")]
    internal static class ToolLookPatch
    {
        private static readonly AccessTools.FieldRef<PlayerToolMovement, Vector3> LookRot =
            AccessTools.FieldRefAccess<PlayerToolMovement, Vector3>("_lookRot");

        private static readonly AccessTools.FieldRef<PlayerToolMovement, Transform> SwayTarget =
            AccessTools.FieldRefAccess<PlayerToolMovement, Transform>("_toolSwayTarget");

        private static bool Prefix(PlayerToolMovement __instance)
        {
            var feature = Registry.Find<SteadyToolFeature>();
            if (feature == null || !feature.IsActive)
            {
                return true;
            }

            LookRot(__instance) = Vector3.zero;

            var target = SwayTarget(__instance);
            if ((bool)target && (bool)target.parent)
            {
                target.parent.localEulerAngles = Vector3.zero;
            }

            return false;
        }
    }
}