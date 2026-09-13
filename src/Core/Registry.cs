using System;
using System.Collections.Generic;
using ComfortMotion.Features;
using MelonLoader;

namespace ComfortMotion.Core
{
    public static class Registry
    {
        private static readonly List<Feature> All = new List<Feature>();

        private static readonly Dictionary<Type, Feature> ByType = new Dictionary<Type, Feature>();

        public static int Count => All.Count;

        public static IReadOnlyList<Feature> Features => All;

        public static void Initialize()
        {
            Register(new HeadBobFeature());
            Register(new CameraSwayFeature());
            Register(new ScreenShakeFeature());
            Register(new WaterSplashFeature());
            Register(new SteadyToolFeature());
            Register(new LockAdsFovFeature());
            Register(new ReducePostEffectsFeature());
            Register(new FlattenWavesFeature());
            Register(new TreeSwayFeature());
            Register(new AimCrosshairFeature());
            Register(new WalkLockFeature());
            Register(new HideEmptyHandsFeature());
            Register(new ComfortPackFeature());

            for (var index = 0; index < All.Count; index++)
            {
                All[index].OnInitialized();
            }
        }

        public static T Find<T>()
            where T : Feature
        {
            return ByType.TryGetValue(typeof(T), out var feature) ? (T)feature : null;
        }

        public static void TickAll()
        {
            for (var index = 0; index < All.Count; index++)
            {
                Tick(All[index]);
            }
        }

        public static void DrawGui()
        {
            for (var index = 0; index < All.Count; index++)
            {
                var feature = All[index];
                if (feature.IsActive)
                {
                    feature.OnGui();
                }
            }
        }

        public static void DisableAll()
        {
            for (var index = 0; index < All.Count; index++)
            {
                All[index].Enabled.Value = false;
            }
        }

        public static void EnableAll()
        {
            for (var index = 0; index < All.Count; index++)
            {
                All[index].Enabled.Value = true;
            }
        }

        private static void Tick(Feature feature)
        {
            var enabled = feature.Enabled.Value;
            if (enabled != feature.WasEnabled)
            {
                feature.ApplyEnabledState();
                feature.WasEnabled = enabled;
                MelonPreferences.Save();
            }

            feature.Tick();
        }

        private static void Register(Feature feature)
        {
            ByType[feature.GetType()] = feature;
            All.Add(feature);
        }
    }
}