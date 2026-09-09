using System.Runtime.CompilerServices;
using ComfortMotion.Core;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class SmoothCameraFeature : Feature
    {
        public SmoothCameraFeature()
        {
            Smoothing = Preferences.Category.CreateEntry(Id + ".smoothing", 0.5f, "Smoothing");
            SmoothYaw = Preferences.Category.CreateEntry(Id + ".smoothYaw", false, "Smooth turn (yaw)");
        }

        public override string Id => "smoothCamera";

        public override string Name => "Smooth Camera";

        public MelonPreferences_Entry<float> Smoothing { get; }

        public MelonPreferences_Entry<bool> SmoothYaw { get; }

        public override void DrawOptions()
        {
            if (!Enabled.Value)
            {
                return;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Smooth", GUILayout.Width(50f));
            var smoothing = GUILayout.HorizontalSlider(Smoothing.Value, 0f, 1f);
            GUILayout.EndHorizontal();
            Smoothing.Value = smoothing;

            SmoothYaw.Value = GUILayout.Toggle(SmoothYaw.Value, "Smooth turn (yaw)");
        }
    }

    [HarmonyPatch(typeof(PlayerCamera), "SetCamPosRot")]
    internal static class SmoothCameraPatch
    {
        private const float MinTau = 0.08f;

        private const float MaxTau = 0.8f;

        private const float YawTau = 0.12f;

        private static readonly ConditionalWeakTable<PlayerCamera, SmoothState> States =
            new ConditionalWeakTable<PlayerCamera, SmoothState>();

        private sealed class SmoothState
        {
            public float PositionY;
            public float VelocityY;
            public float Yaw;
            public float YawVelocity;
        }

        private static void Postfix(PlayerCamera __instance)
        {
            var feature = Registry.Find<SmoothCameraFeature>();
            var camera = __instance.CamTransform;
            if (feature == null || !(bool)camera || !Game.IsLocalCamera(__instance))
            {
                return;
            }

            var state = States.GetOrCreateValue(__instance);

            if (!feature.IsActive)
            {
                Mirror(state, camera);
                return;
            }

            var tau = Mathf.Lerp(MinTau, MaxTau, feature.Smoothing.Value);
            state.PositionY = Mathf.SmoothDamp(state.PositionY, camera.position.y, ref state.VelocityY, tau);
            camera.position = new Vector3(camera.position.x, state.PositionY, camera.position.z);

            if (feature.SmoothYaw.Value)
            {
                var currentYaw = camera.eulerAngles.y;
                state.Yaw = Mathf.SmoothDamp(
                    state.Yaw,
                    state.Yaw + Mathf.DeltaAngle(state.Yaw, currentYaw),
                    ref state.YawVelocity,
                    YawTau);
                camera.eulerAngles = new Vector3(camera.eulerAngles.x, state.Yaw, camera.eulerAngles.z);
            }
            else
            {
                state.Yaw = camera.eulerAngles.y;
                state.YawVelocity = 0f;
            }
        }

        private static void Mirror(SmoothState state, Transform camera)
        {
            state.PositionY = camera.position.y;
            state.VelocityY = 0f;
            state.Yaw = camera.eulerAngles.y;
            state.YawVelocity = 0f;
        }
    }
}