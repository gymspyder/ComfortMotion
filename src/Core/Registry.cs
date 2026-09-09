using System;
using System.Collections.Generic;
using ComfortMotion.Features;

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
            Register(new SmoothCameraFeature());
            Register(new SteadyToolFeature());
            Register(new LockAdsFovFeature());
            Register(new ReducePostEffectsFeature());
            Register(new FlattenWavesFeature());
            Register(new TreeSwayFeature());
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

        public static void DisableAll()
        {
            for (var index = 0; index < All.Count; index++)
            {
                All[index].Enabled.Value = false;
            }
        }

        private static void Tick(Feature feature)
        {
            var enabled = feature.Enabled.Value;
            if (enabled != feature.WasEnabled)
            {
                feature.ApplyEnabledState();
                feature.WasEnabled = enabled;
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