using System.Reflection;
using ComfortMotion.Core;
using HarmonyLib;
using MelonLoader;

[assembly: MelonInfo(typeof(ComfortMotion.ComfortMod), "ComfortMotion", "1.0.1", "gymspyder")]
[assembly: MelonGame("Dazed Games", "How to Fish")]

namespace ComfortMotion
{
    public sealed class ComfortMod : MelonMod
    {
        private const string HarmonyId = "com.gymspyder.comfortmotion";

        private HarmonyLib.Harmony _harmony;
        private Menu _menu;

        public override void OnInitializeMelon()
        {
            Preferences.Create();
            Registry.Initialize();

            _harmony = new HarmonyLib.Harmony(HarmonyId);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());

            _menu = new Menu();

            LoggerInstance.Msg($"ComfortMotion loaded with {Registry.Count} comfort features.");
        }

        public override void OnUpdate()
        {
            _menu.Tick();
            Registry.TickAll();
        }

        public override void OnGUI()
        {
            Registry.DrawGui();
            _menu.Draw();
        }

        public override void OnApplicationQuit()
        {
            MelonPreferences.Save();
        }
    }
}