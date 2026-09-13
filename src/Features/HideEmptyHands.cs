using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class HideEmptyHandsFeature : Feature
    {
        public HideEmptyHandsFeature()
            : base(true)
        {
        }

        public override string Id => "hideEmptyHands";

        public override string Name => "Hide Empty Hands";
    }

    [HarmonyPatch(typeof(PlayerHands), "LateUpdate")]
    internal static class EmptyHandsVisibilityPatch
    {
        private static readonly AccessTools.FieldRef<PlayerHands, Player> PlayerRef =
            AccessTools.FieldRefAccess<PlayerHands, Player>("_player");

        private static readonly AccessTools.FieldRef<PlayerHands, Renderer> HandModelRight =
            AccessTools.FieldRefAccess<PlayerHands, Renderer>("_handModelRight");

        private static readonly AccessTools.FieldRef<PlayerHands, Renderer> HandModelLeft =
            AccessTools.FieldRefAccess<PlayerHands, Renderer>("_handModelLeft");

        private static readonly AccessTools.FieldRef<PlayerHands, Item> ThrownItem =
            AccessTools.FieldRefAccess<PlayerHands, Item>("_thrownItem");

        private static readonly AccessTools.FieldRef<PlayerHands, Item> PreparedDropItem =
            AccessTools.FieldRefAccess<PlayerHands, Item>("_preparedDropItem");

        private static readonly AccessTools.FieldRef<PlayerHands, byte> PreparedPickupItemID =
            AccessTools.FieldRefAccess<PlayerHands, byte>("_preparedPickupItemID");

        private static readonly AccessTools.FieldRef<PlayerHands, float> PreparedPickupTime =
            AccessTools.FieldRefAccess<PlayerHands, float>("_preparedPickupTime");

        private static void Postfix(PlayerHands __instance)
        {
            var feature = Registry.Find<HideEmptyHandsFeature>();
            if (feature == null || !feature.IsActive)
            {
                return;
            }

            var player = PlayerRef(__instance);
            if (!(bool)player || !player.Owner.IsLocalClient)
            {
                return;
            }

            if ((bool)player.Holding.HeldItem || (bool)ThrownItem(__instance))
            {
                return;
            }

            if ((bool)BoatManager.Boat && BoatManager.Boat.Driver == player)
            {
                return;
            }

            if ((bool)PreparedDropItem(__instance))
            {
                return;
            }

            if (PreparedPickupItemID(__instance) != byte.MaxValue &&
                Time.unscaledTime - PreparedPickupTime(__instance) <= 10f)
            {
                return;
            }

            SetEnabled(__instance, player.Punching.IsAnimatingRight || player.Punching.IsAnimatingLeft);
        }

        private static void SetEnabled(PlayerHands hands, bool enabled)
        {
            var right = HandModelRight(hands);
            var left = HandModelLeft(hands);

            if (right != null && right.enabled != enabled)
            {
                right.enabled = enabled;
            }

            if (left != null && left.enabled != enabled)
            {
                left.enabled = enabled;
            }
        }
    }
}