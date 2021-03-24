namespace Engine
{
    public static class Time
    {
        private static float elapsedTime;
        private static float deltaTime;

        public static float ElapsedTime { get => elapsedTime; set => elapsedTime = value; }
        public static float DeltaTime { get => deltaTime; set => deltaTime = value; }
    }
}