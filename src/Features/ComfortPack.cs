using System.Collections.Generic;
using ComfortMotion.Core;

namespace ComfortMotion.Features
{
    public sealed class ComfortPackFeature : Feature
    {
        private readonly List<Feature> _members = new List<Feature>();

        private readonly Dictionary<Feature, bool> _previous = new Dictionary<Feature, bool>();

        public override string Id => "comfortPack";

        public override string Name => "Comfort Pack";

        public override void OnInitialized()
        {
            AddMember<HeadBobFeature>();
            AddMember<CameraSwayFeature>();
            AddMember<ScreenShakeFeature>();
            AddMember<SmoothCameraFeature>();
            AddMember<SteadyToolFeature>();
            AddMember<WaterSplashFeature>();
            AddMember<FlattenWavesFeature>();
            AddMember<TreeSwayFeature>();
        }

        protected override void OnActivated()
        {
            _previous.Clear();

            foreach (var member in _members)
            {
                _previous[member] = member.Enabled.Value;

                if (!member.Enabled.Value)
                {
                    member.Enabled.Value = true;
                }
            }
        }

        protected override void OnDeactivated()
        {
            foreach (var pair in _previous)
            {
                pair.Key.Enabled.Value = pair.Value;
            }

            _previous.Clear();
        }

        private void AddMember<T>()
            where T : Feature
        {
            var feature = Registry.Find<T>();
            if (feature != null)
            {
                _members.Add(feature);
            }
        }
    }
}