namespace ComfortMotion.Core
{
    public static class Game
    {
        public static bool IsInGame => (bool)Server.Instance;

        public static Player LocalPlayer => (bool)Player.LocalPlayer ? Player.LocalPlayer : null;

        public static bool TryGetLocalPlayer(out Player player)
        {
            player = LocalPlayer;
            return player != null;
        }

        public static bool IsLocalCamera(global::PlayerCamera camera)
        {
            return camera != null && TryGetLocalPlayer(out var player) && player.Camera == camera;
        }
    }
}