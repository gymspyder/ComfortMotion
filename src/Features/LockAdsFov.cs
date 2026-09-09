using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class LockAdsFovFeature : Feature
    {
        public override string Id => "lockAdsFov";

        public override string Name => "Lock ADS FOV";
    }

    [HarmonyPatch(typeof(PlayerCamera), "SetFov")]
    internal static class LockAdsFovPatch
    {
        private static readonly AccessTools.FieldRef<float> OriginalFov =
            AccessTools.StaticFieldRefAccess<float>(AccessTools.Field(typeof(PlayerCamera), "_origFov"));

        private static readonly AccessTools.FieldRef<PlayerCamera, float> CurrentFov =
            AccessTools.FieldRefAccess<PlayerCamera, float>("_curFov");

        private static readonly AccessTools.FieldRef<PlayerCamera, global::Player> PlayerRef =
            AccessTools.FieldRefAccess<PlayerCamera, global::Player>("_player");

        private static bool Prefix(PlayerCamera __instance)
        {
            var feature = Registry.Find<LockAdsFovFeature>();
            if (feature == null || !feature.IsActive || !Game.IsLocalCamera(__instance))
            {
                return true;
            }

            var player = PlayerRef(__instance);
            if (player == null || player.Holding == null || player.Holding.HeldItem == null ||
                player.Holding.HeldItem.Weapon == null || !player.Holding.HeldItem.Weapon.IsAds)
            {
                return true;
            }

            var camera = __instance.Cam;
            var fov = OriginalFov();
            if (!(bool)camera || fov <= 0f)
            {
                return true;
            }

            CurrentFov(__instance) = fov;
            camera.fieldOfView = fov;
            return false;
        }
    }
}