using ComfortMotion.Core;
using HarmonyLib;
using MelonLoader;
using UnityEngine;

namespace ComfortMotion.Features
{
    public sealed class AimCrosshairFeature : Feature
    {
        private const float LineLength = 10f;

        private const float LineThickness = 2f;

        private const float SniperAimThreshold = 0.9f;

        private static readonly AccessTools.FieldRef<Weapon, Attachments> AttachmentRef =
            AccessTools.FieldRefAccess<Weapon, Attachments>("_attachments");

        private static readonly AccessTools.FieldRef<Weapon, float> SpreadRef =
            AccessTools.FieldRefAccess<Weapon, float>("_spread");

        private static readonly AccessTools.FieldRef<Weapon, float> AimPercentRef =
            AccessTools.FieldRefAccess<Weapon, float>("_aimPercent");

        private static readonly AccessTools.FieldRef<PlayerCamera, Camera> CameraRef =
            AccessTools.FieldRefAccess<PlayerCamera, Camera>("_cam");

        private static Texture2D _pixel;

        public AimCrosshairFeature()
        {
            Size = Preferences.Category.CreateEntry(Id + "Size", 1f, "Size");
        }

        public override string Id => "aimCrosshair";

        public override string Name => "Aim Crosshair";

        public MelonPreferences_Entry<float> Size { get; }

        public override void DrawOptions()
        {
            if (!Enabled.Value)
            {
                return;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Size", GUILayout.Width(50f));
            var size = GUILayout.HorizontalSlider(Size.Value, 0.5f, 2f);
            GUILayout.EndHorizontal();
            Size.Value = size;
        }

        public override void OnGui()
        {
            if (!IsActive || Menu.IsOpen)
            {
                return;
            }

            if (!Game.TryGetLocalPlayer(out var player))
            {
                return;
            }

            var weapon = player.Holding.HeldItem as Weapon;
            if (!(bool)weapon)
            {
                return;
            }

            var attachments = AttachmentRef(weapon);
            if (attachments != null && attachments.UseSniperUi && AimPercentRef(weapon) > SniperAimThreshold)
            {
                return;
            }

            var camera = CameraRef(player.Camera);
            if (!(bool)camera)
            {
                return;
            }

            DrawCrosshair(SpreadRef(weapon), camera);
        }

        private void DrawCrosshair(float spreadDegrees, Camera camera)
        {
            EnsurePixel();

            var vFovHalf = Mathf.Max(camera.fieldOfView, 1f) * 0.5f * Mathf.Deg2Rad;
            var radius = Mathf.Tan(spreadDegrees * Mathf.Deg2Rad) * (Screen.height * 0.5f / Mathf.Tan(vFovHalf));
            radius *= Size.Value;

            var centerX = Screen.width * 0.5f;
            var centerY = Screen.height * 0.5f;

            var previousColor = GUI.color;
            GUI.color = Color.white;

            var length = Mathf.Max(LineLength * Size.Value, 6f);
            if (radius <= 2f)
            {
                GUI.DrawTexture(new Rect(centerX - LineThickness, centerY - LineThickness, LineThickness * 2f, LineThickness * 2f), _pixel);
            }
            else
            {
                GUI.DrawTexture(new Rect(centerX - LineThickness * 0.5f, centerY - radius - length, LineThickness, length), _pixel);
                GUI.DrawTexture(new Rect(centerX + radius, centerY - LineThickness * 0.5f, length, LineThickness), _pixel);
                GUI.DrawTexture(new Rect(centerX - LineThickness * 0.5f, centerY + radius, LineThickness, length), _pixel);
                GUI.DrawTexture(new Rect(centerX - radius - length, centerY - LineThickness * 0.5f, length, LineThickness), _pixel);
            }

            GUI.color = previousColor;
        }

        private static void EnsurePixel()
        {
            if (_pixel != null)
            {
                return;
            }

            _pixel = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            _pixel.SetPixel(0, 0, Color.white);
            _pixel.Apply();
        }
    }
}