using MelonLoader;

namespace ComfortMotion.Core
{
    public abstract class Feature
    {
        protected Feature(bool enabledByDefault = false)
        {
            Enabled = Preferences.Category.CreateEntry(Id + "Enabled", enabledByDefault, Name);
        }

        public abstract string Id { get; }

        public virtual string Name => Id;

        public MelonPreferences_Entry<bool> Enabled { get; }

        public bool IsActive => Enabled.Value;

        internal bool WasEnabled;

        public virtual void OnInitialized()
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void OnGui()
        {
        }

        public virtual void DrawOptions()
        {
        }

        internal void ApplyEnabledState()
        {
            if (Enabled.Value)
            {
                OnActivated();
            }
            else
            {
                OnDeactivated();
            }
        }

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnDeactivated()
        {
        }
    }
}