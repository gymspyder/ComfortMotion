using ComfortMotion.Core;
using UnityEngine;

namespace ComfortMotion
{
    public sealed class Menu
    {
        private readonly KeyCode _menuKey;

        private bool _open;

        private Rect _window = new Rect(20f, 20f, 340f, 220f);

        public Menu()
        {
            _menuKey = Preferences.MenuKey;
        }

        public void Tick()
        {
            if (!UnityEngine.Input.GetKeyDown(_menuKey))
            {
                return;
            }

            _open = !_open;
            Cursor.lockState = _open ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _open;
        }

        public void Draw()
        {
            if (!_open)
            {
                return;
            }

            _window = GUILayout.Window(0, _window, DrawWindow, "ComfortMotion");
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

            if (GUILayout.Button("Disable all"))
            {
                Registry.DisableAll();
            }

            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}