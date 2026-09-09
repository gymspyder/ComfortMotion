using System;
using MelonLoader;
using UnityEngine;

namespace ComfortMotion.Core
{
    public static class Preferences
    {
        private static MelonPreferences_Category _category;
        private static MelonPreferences_Entry<string> _menuKey;

        public static MelonPreferences_Category Category => _category;

        public static KeyCode MenuKey { get; private set; } = KeyCode.F5;

        public static void Create()
        {
            _category = MelonPreferences.CreateCategory("ComfortMotion", "Comfort Motion");
            _menuKey = _category.CreateEntry("menu.key", "F5", "Menu key");
            if (Enum.TryParse(_menuKey.Value, out KeyCode parsed))
            {
                MenuKey = parsed;
            }
        }
    }
}