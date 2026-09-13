using ComfortMotion.Core;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ComfortMotion
{
    public sealed class Menu
    {
        internal static bool IsOpen { get; private set; }

        private readonly Key _menuKey;

        private bool _open;

        private Rect _window = new Rect(20f, 20f, 360f, 240f);

        private static GUIStyle _windowStyle;

        private static Texture2D _dim;

        public Menu()
        {
            _menuKey = Preferences.MenuKey;
        }

        public void Tick()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null || !keyboard[_menuKey].wasPressedThisFrame)
            {
                return;
            }

            _open = !_open;
            IsOpen = _open;
            if (_open)
            {
                PlayerCamera.ToggleMouse(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                PlayerCamera.ToggleMouse(false);
            }
        }

        public void Draw()
        {
            if (!_open)
            {
                return;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            EnsureResources();

            GUI.color = new Color(1f, 1f, 1f, 0.55f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), _dim);
            GUI.color = Color.white;

            _window = GUILayout.Window(0, _window, DrawWindow, "ComfortMotion", _windowStyle);
        }

        private void DrawWindow(int windowId)
        {
            GUILayout.BeginVertical();

            var features = Registry.Features;
            for (var index = 0; index < features.Count; index++)
            {
                var feature = features[index];
                var enabled = feature.Enabled.Value;
                var next = GUILayout.Toggle(enabled, feature.Name);
                if (next != enabled)
                {
                    feature.Enabled.Value = next;
                }

                feature.DrawOptions();
            }

            GUILayout.Space(8f);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Enable all"))
            {
                Registry.EnableAll();
            }

            if (GUILayout.Button("Disable all"))
            {
                Registry.DisableAll();
            }

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private static void EnsureResources()
        {
            if (_dim != null && _windowStyle != null)
            {
                return;
            }

            _dim = CreateSolid(new Color(0f, 0f, 0f, 1f));

            var style = new GUIStyle(GUI.skin.window);
            style.normal.background = CreateSolid(new Color(0.08f, 0.09f, 0.11f, 1f));
            _windowStyle = style;
        }

        private static Texture2D CreateSolid(Color color)
        {
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }

    [HarmonyPatch(typeof(PlayerCamera), "MouseClick")]
    internal static class MenuMouseLockPatch
    {
        private static bool Prefix()
        {
            return !Menu.IsOpen;
        }
    }
}