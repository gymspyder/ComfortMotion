using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class WalkLockFeature : Feature
    {
        public override string Id => "walkLock";

        public override string Name => "Walk Lock";
    }

    [HarmonyPatch(typeof(PlayerToolMovement), "Bob")]
    internal static class ToolWalkBobPatch
    {
        private static readonly AccessTools.FieldRef<PlayerToolMovement, Vector2> BobPos =
            AccessTools.FieldRefAccess<PlayerToolMovement, Vector2>("_bobPos");

        private static void Postfix(PlayerToolMovement __instance)
        {
            var feature = Registry.Find<WalkLockFeature>();
            if (feature == null || !feature.IsActive)
            {
                return;
            }

            BobPos(__instance) = Vector2.zero;
        }
    }

    [HarmonyPatch(typeof(PlayerHands), "SetHandBob")]
    internal static class EmptyHandsWalkBobPatch
    {
        private static readonly AccessTools.FieldRef<PlayerHands, float> CurTpHandBob =
            AccessTools.FieldRefAccess<PlayerHands, float>("_curTpHandBob");

        private static void Postfix(PlayerHands __instance)
        {
            var feature = Registry.Find<WalkLockFeature>();
            if (feature == null || !feature.IsActive)
            {
                return;
            }

            CurTpHandBob(__instance) = 0f;
        }
    }

    [HarmonyPatch(typeof(PlayerHolding), "GetBobOffset")]
    internal static class HeldItemWalkBobPatch
    {
        private static readonly AccessTools.FieldRef<PlayerHolding, Vector3> BobOffset =
            AccessTools.FieldRefAccess<PlayerHolding, Vector3>("_bobOffset");

        private static void Postfix(PlayerHolding __instance)
        {
            var feature = Registry.Find<WalkLockFeature>();
            if (feature == null || !feature.IsActive)
            {
                return;
            }

            BobOffset(__instance) = Vector3.zero;
        }
    }
}