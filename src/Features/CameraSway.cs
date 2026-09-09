using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class CameraSwayFeature : Feature
    {
        public override string Id => "disableCameraSway";

        public override string Name => "Disable Camera Sway";
    }

    [HarmonyPatch(typeof(PlayerCamera), "HeadRot")]
    internal static class HeadRotPatch
    {
        private static readonly AccessTools.FieldRef<PlayerCamera, Vector3> MoveRot =
            AccessTools.FieldRefAccess<PlayerCamera, Vector3>("_moveRot");

        private static bool Prefix(PlayerCamera __instance)
        {
            var feature = Registry.Find<CameraSwayFeature>();
            if (feature == null || !feature.IsActive || !Game.IsLocalCamera(__instance))
            {
                return true;
            }

            MoveRot(__instance) = Vector3.zero;
            return false;
        }
    }
}