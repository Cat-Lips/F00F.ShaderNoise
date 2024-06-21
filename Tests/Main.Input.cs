using Godot;

namespace Tests;

public partial class Main
{
    static Main() => MyInput.Init();

    private class MyInput : F00F.MyInput
    {
        static MyInput() => Init<MyInput>();
        public static void Init() { }

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        public static readonly StringName ToggleTerrain;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

        private static class Defaults
        {
            public static readonly Key ToggleTerrain = Key.F12;
        }

        private MyInput() { }
    }
}
