using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class ScreenShakeFeature : Feature
    {
        public override string Id => "disableScreenShake";

        public override string Name => "Disable Screen Shake";
    }

    [HarmonyPatch(typeof(PlayerCamera), "SetShakePos")]
    internal static class SetShakePosPatch
    {
        private static readonly AccessTools.FieldRef<PlayerCamera, Vector2> ShakePos =
            AccessTools.FieldRefAccess<PlayerCamera, Vector2>("<ShakePos>k__BackingField");

        private static bool Prefix(PlayerCamera __instance)
        {
            var feature = Registry.Find<ScreenShakeFeature>();
            if (feature == null || !feature.IsActive || !Game.IsLocalCamera(__instance))
            {
                return true;
            }

            ShakePos(__instance) = Vector2.zero;
            return false;
        }
    }
}