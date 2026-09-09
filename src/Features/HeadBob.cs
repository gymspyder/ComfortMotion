using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class HeadBobFeature : Feature
    {
        public override string Id => "disableHeadBob";

        public override string Name => "Disable Head Bobbing";
    }

    [HarmonyPatch(typeof(PlayerCamera), "HeadBobbing")]
    internal static class HeadBobbingPatch
    {
        private static readonly AccessTools.FieldRef<PlayerCamera, Vector3> BobPos =
            AccessTools.FieldRefAccess<PlayerCamera, Vector3>("_bobPos");

        private static readonly AccessTools.FieldRef<PlayerCamera, Vector3> DelayedBobPos =
            AccessTools.FieldRefAccess<PlayerCamera, Vector3>("_delayedBobPos");

        private static bool Prefix(PlayerCamera __instance)
        {
            var feature = Registry.Find<HeadBobFeature>();
            if (feature == null || !feature.IsActive || !Game.IsLocalCamera(__instance))
            {
                return true;
            }

            BobPos(__instance) = Vector3.zero;
            DelayedBobPos(__instance) = Vector3.zero;
            return false;
        }
    }
}